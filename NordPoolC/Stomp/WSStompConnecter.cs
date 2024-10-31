using CaoNC.Microsoft.VisualStudio.Threading;
using CaoNC.System.Channels;
using CaoNC.Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NordPoolC.Config;
using NordPoolC.Connection;
using NordPoolC.Exceptions;
using NordPoolC.Extensions;
using NordPoolC.Logger;
using NordPoolC.Message;
using NordPoolC.Subscription;
using NordPoolC.WebSockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Apache.NMS.Stomp.Protocol;
using Apache.NMS.Stomp.Commands;
using System.Collections.Concurrent;
using System.Web.Configuration;

namespace NordPoolC.Stomp
{
    public class WSStompConnecter : IStompConnecter
    {
        private readonly IConnecter _webSocketConnecter;

        private bool _connectionEstablished;
        private bool _connectionClosed;

        private readonly AsyncManualResetEvent _connectionEstablishedEvent = new AsyncManualResetEvent(false);
        private readonly AsyncManualResetEvent _connectionClosedEvent = new AsyncManualResetEvent(false);

        private readonly Dictionary<string, Subscription.Subscription> _subscriptions
            = new Dictionary<string, Subscription.Subscription>();

        private ConcurrentQueue<StompFrame> requests = new ConcurrentQueue<StompFrame>();
        public int RequestsCount => requests.Count();

        public bool IsConnected => _webSocketConnecter.IsConnected;

        public string ConnectionUri => _webSocketConnecter.ConnectionUri;

        public bool IsMain { get; set; } = false;

        public IClient Parent { get; set; }

        public WSStompConnecter(SslPoint webSocketOptions)
        {
            _webSocketConnecter = new WebSocketConnecter(webSocketOptions,
           OnMessageReceivedAsync, OnConnectionEstablishedAsync, OnConnectionClosedAsync, OnStompErrorAsync);
        }

        private Task OnConnectionEstablishedAsync()
        {
            _connectionEstablished = true;
            _connectionEstablishedEvent.Set();
            LogFactory.Instance.Info(string.Format("[{0}] WSStomp Connection established for client {1}", Parent==null?0:Parent.ClientTarget, Parent == null ? "0" : Parent.ClientId));

            return Task.FromResult(0);
        }

        private Task OnConnectionClosedAsync()
        {
            _connectionClosedEvent.Set();

            if (_connectionClosed)
            {
                LogFactory.Instance.Info(string.Format("[{0}] WSStomp Connection closing for client {1}", Parent == null ? 0 : Parent.ClientTarget, Parent == null ? "0" : Parent.ClientId));
            }
            else
            {
                LogFactory.Instance.Error(string.Format("[{0}] WSStomp Connection closed unexpectedly for client {1}", Parent == null ? 0 : Parent.ClientTarget, Parent == null ? "0" : Parent.ClientId));
            }

            foreach (var subscription in _subscriptions.Values)
            {
                subscription.Close();
            }

            return Task.FromResult(0);
        }

        private Task OnMessageReceivedAsync(MessageReceivedEventArgs e, CancellationToken cancellationToken)
        {
            var isMessage = e.Message.IsMessageCommand();
            if (!isMessage)
            {
                return Task.FromResult(0);
            }

            var stompFrame = e.Message.ConvertToStompFrame();

            if (stompFrame.Properties.TryGetValue(Headers.Server.Subscription, out var subscriptionId))
            {
                LogFactory.Instance.Info(string.Format("[{0}][Frame({1}):Metadata] WSStomp destination={2}, sentAt={3}, snapshot={4}, publishingMode={5}, sequenceNo={6}",
                Parent == null ? 0 : Parent.ClientTarget,
                subscriptionId,
                stompFrame.GetDestination(),
                stompFrame.GetSentAtTimestamp(),
                stompFrame.IsSnapshot(),
                stompFrame.GetPublishingMode(),
                stompFrame.GetSequenceNumber()));

                if (_subscriptions.TryGetValue(subscriptionId.ToString(), out var targetSubscription))
                {
                    targetSubscription.OnMessage(stompFrame, e.Timestamp);
                }
                else
                {
                    LogFactory.Instance.Warning(string.Format("[{0}][Frame({1})] WSStomp Received message for subscription that is not assigned to current client", Parent == null ? 0 : Parent.ClientTarget, subscriptionId));
                }
            }
            else
            {
                LogFactory.Instance.Warning(string.Format("[{0}] WSStomp Unrecognized message received from {1}. Command:{2}\nHeaders:\n{3}\n{4}",
                Parent == null ? 0 : Parent.ClientTarget,
                _webSocketConnecter.ConnectionUri,
                stompFrame.Command,
                string.Join("\n", stompFrame.Properties.AsEnumerable().Select(header => $"{header.Key}:{header.Value}")),
                Encoding.UTF8.GetString(stompFrame.Content)));
            }

            return Task.FromResult(0);
        }

        private Task OnStompErrorAsync(StompConnectionException exception)
        {
            LogFactory.Instance.Exception(exception,string.Format("[{0}] WSStomp An error on web socket message processing", Parent == null ? 0 : Parent.ClientTarget));

            return Task.FromResult(0);
        }

        public async Task<bool> ConnectAsync(CancellationToken cancellationToken)
        {
            await _webSocketConnecter.ConnectAsync(cancellationToken);

            await _connectionEstablishedEvent.WaitAsync();
            return _webSocketConnecter.IsConnected;
        }

        public async Task<ISubscription<TValue>> SubscribeAsync<TValue>(SubscribeRequest request, CancellationToken cancellationToken)
        {
            if (!_webSocketConnecter.IsConnected)
            {
                var target=(Parent == null ? 0 : Parent.ClientTarget);
                throw new SubscriptionFailedException(
                    $"WSStomp [{target}][Destination:{request.Destination}] Failed to subscribe because no connection is established! Connect first!");
            }

            var subscription = new StompSubscription<TValue>(
                request.SubscriptionId,
                request.Type,
                request.Destination,
                Channel.CreateBounded<ReceivedMessage<IReadOnlyCollection<TValue>>>(new BoundedChannelOptions(30_000)
                {
                    FullMode = BoundedChannelFullMode.Wait
                }));

            _subscriptions[subscription.Id] = subscription;
            var subscribeFrame = StompMessageFactory.SubscribeFrame(request.Destination, request.SubscriptionId);
            await _webSocketConnecter.SendStompFrameAsync(subscribeFrame, cancellationToken);
            return subscription;
        }

        public async Task PushToSendAsync<TRequest>(TRequest request, string destination)
            where TRequest : class, new ()
        {
            var payload = JsonConvert.SerializeObject(request);
            var payloadFrame = StompMessageFactory.SendFrame(payload, destination);
            requests.Enqueue(payloadFrame);
            if(requests.Count>0)
            {
                if (requests.TryDequeue(out var frame))
                {
                    await SendAsync(frame);
                }
            }
        }

        public async Task SendAsync<TRequest>(TRequest request, string destination, CancellationToken cancellationToken)
            where TRequest : class, new()
        {
            var payload = JsonConvert.SerializeObject(request);
            var payloadFrame = StompMessageFactory.SendFrame(payload, destination);
            await _webSocketConnecter.SendStompFrameAsync(payloadFrame, cancellationToken);
        }

        private async Task SendAsync(StompFrame payloadFrame)
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            await _webSocketConnecter.SendStompFrameAsync(payloadFrame, cts.Token);

            if (requests.Count > 0)
            {
                if (requests.TryDequeue(out var frame))
                {
                    await SendAsync(frame);
                }
            }
        }

        public async Task SendAsync(StompFrame payloadFrame, CancellationToken cancellationToken)
        {
            await _webSocketConnecter.SendStompFrameAsync(payloadFrame, cancellationToken);
        }

        public async Task UnsubscribeAsync(string subscriptionId, CancellationToken cancellationToken)
        {
            var unsubscribeFrame = StompMessageFactory.Unsubscribe(subscriptionId);
            await _webSocketConnecter.SendStompFrameAsync(unsubscribeFrame, cancellationToken);
            if (_subscriptions.ContainsKey(subscriptionId))
            {
                _subscriptions[subscriptionId].Close();
                _subscriptions.Remove(subscriptionId);

                LogFactory.Instance.Info(string.Format("[{0}][SubscriptionId:{1}] Unsubscribed", Parent == null ? 0 : Parent.ClientTarget, subscriptionId));
            }
        }

        public async Task DisconnectAsync(CancellationToken cancellationToken)
        {
            await UnsubscribeAllAsync(cancellationToken);

            _connectionClosed = true;
            await _webSocketConnecter.DisposeAsync();
        }

        private async Task UnsubscribeAllAsync(CancellationToken cancellationToken)
        {
            if (!_webSocketConnecter.IsConnected)
            {
                return;
            }

            var subscriptions = _subscriptions
                .Values
                .ToList();

            foreach (var subscription in subscriptions)
            {
                await UnsubscribeAsync(subscription.Id, cancellationToken);
            }
        }
    }
}
