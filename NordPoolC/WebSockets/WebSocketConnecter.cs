using CaoNC.System.Threading.Tasks;
using Newtonsoft.Json;
using NordPoolC.Commands;
using NordPoolC.Config;
using NordPoolC.Connection;
using NordPoolC.Exceptions;
using NordPoolC.Extensions;
using NordPoolC.Logger;
using NordPoolC.Message;
using NordPoolC.Security;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Runtime.Remoting.Messaging;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NordPoolC.WebSockets
{
    public class WebSocketConnecter: IConnecter
    {
        private bool _isConnected;
        /// <summary>
        /// 是否连接
        /// </summary>
        public bool IsConnected => _isConnected && _webSocket.State == WebSocketState.Open;

        /// <summary>
        /// 连接uri
        /// </summary>
        public string ConnectionUri { get; private set; }

        private SslPoint _webSocketOptions;
        private string _currentAuthToken;
        private readonly ClientWebSocket _webSocket = new ClientWebSocket();
        private readonly Credentials _credentialsOptions;
        private readonly Func<Task> _connectionEstablishedCallbackAsync;
        private readonly Func<Task> _connectionClosedCallbackAsync;
        private readonly Func<MessageReceivedEventArgs, CancellationToken, Task> _messageReceivedCallbackAsync;
        private readonly Func<StompConnectionException, Task> _stompErrorCallbackAsync;
        private readonly CancellationTokenSource _connectorCancellationTokenSource = new CancellationTokenSource();
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        private readonly string _serverId;


        public WebSocketConnecter(SslPoint webSocketOptions,
        Func<MessageReceivedEventArgs, CancellationToken, Task> messageReceivedCallbackAsync,
        Func<Task> connectionEstablishedCallbackAsync = null,
        Func<Task> connectionClosedCallbackAsync = null,
        Func<StompConnectionException, Task> stompErrorCallbackAsync = null)
        {
            _webSocketOptions = webSocketOptions;
            _credentialsOptions = GlobalConfig.GetConfig<Credentials>();

            _messageReceivedCallbackAsync = messageReceivedCallbackAsync;
            _connectionEstablishedCallbackAsync = connectionEstablishedCallbackAsync;
            _connectionClosedCallbackAsync = connectionClosedCallbackAsync;
            _stompErrorCallbackAsync = stompErrorCallbackAsync;

            _serverId = (new Random()).Next(0, 999).ToString().PadLeft(3, '0');
        }

        /// <summary>
        /// 异步连接
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task ConnectAsync(CancellationToken cancellationToken)
        {
            ConnectionUri = $"wss://{_webSocketOptions.Host}:{_webSocketOptions.SslPort}" +
                $"{_webSocketOptions.Uri}/{_serverId}/{Guid.NewGuid().ToString("N")}/websocket";

            LogFactory.Instance.Debug(string.Format("Connecting to: {0}", ConnectionUri));

            _currentAuthToken = await SsoService.Instance.GetAuthTokenAsync();
            await _webSocket.ConnectAsync(new Uri(ConnectionUri), cancellationToken);

            LogFactory.Instance.Debug("Web socket opened");

            _ = Task.Run(() => StartReceivingMessagesAsync(_connectorCancellationTokenSource.Token), cancellationToken);
        }

        /// <summary>
        /// 异步发送消息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task SendAsync(string message, CancellationToken cancellationToken)
        {
            await SendAsync(message, cancellationToken, Encoding.UTF8);
        }

        /// <summary>
        /// 异步发送消息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task SendAsync(string message, CancellationToken cancellationToken, Encoding encoding)
        {
            await SendAsync(encoding.GetBytes(message), cancellationToken);
        }

        /// <summary>
        /// 异步发送消息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task SendAsync(byte[] message, CancellationToken cancellationToken)
        {
            if (_webSocket.State != WebSocketState.Open)
            {
                LogFactory.Instance.Warning(string.Format("WS Trying to send message {0} while connection is in {1} state", Encoding.UTF8.GetString(message), _webSocket.State));
                return;
            }
            await _webSocket.SendAsync(new ArraySegment<byte>(message), WebSocketMessageType.Text, true, cancellationToken);
        }

        private async Task StartReceivingMessagesAsync(CancellationToken cancellationToken)
        {
            await Task.Yield();

            byte[] array = new byte[1024*1024];
            var receiveBuffer = new ArraySegment<byte>(array);
            var message = new ReceivedMessage();

            try
            {
                while (_webSocket.State != WebSocketState.Closed && !cancellationToken.IsCancellationRequested)
                {
                    var receiveResult = await _webSocket.ReceiveAsync(receiveBuffer, cancellationToken);

                    if (cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }

                    if (_webSocket.State == WebSocketState.CloseReceived &&
                        receiveResult.MessageType == WebSocketMessageType.Close)
                    {
                        await CloseConnectionAsync(cancellationToken);
                        break;
                    }

                    if (_webSocket.State == WebSocketState.Open && receiveResult.MessageType != WebSocketMessageType.Close)
                    {
                        message.Append(receiveBuffer.ToArray(), 0, receiveResult.Count, receiveResult.EndOfMessage);

                        if (receiveResult.EndOfMessage)
                        {
                            await HandleReceivedMessageAsync(message, cancellationToken);
                            message.Dispose();
                            message = new ReceivedMessage();
                        }
                    }
                }
            }
            catch (OperationCanceledException oe) when (cancellationToken.IsCancellationRequested)
            {
                LogFactory.Instance.Debug("Stop application request received. Stopped receiving WebSocket messages."+oe.InnerException);
            }
            catch (Exception ex)
            {
                LogFactory.Instance.Exception(ex, "An error on web socket message receiving");
                await CloseConnectionAsync(CancellationToken.None);
            }
            finally
            {
                array = null;
                message.Dispose();
            }
        }

        private async Task HandleReceivedMessageAsync(ReceivedMessage message, CancellationToken cancellationToken)
        {
            try
            {
                var timestamp = DateTimeOffset.UtcNow;

                if (message.IsSockJsStart())
                {
                    var connectCommand = StompMessageFactory.ConnectionFrame(_currentAuthToken, _webSocketOptions.HeartbeatOutgoingInterval);
                    await SendAsync(connectCommand.ConvertToMessageBytes(), cancellationToken);
                    return;
                }

                if (message.IsError())
                {
                    await OnWebSocketErrorAsync(message);
                    return;
                }

                if (message.IsHeartBeat())
                {
                    return;
                }

                if (message.IsDisconnectCode())
                {
                    return;
                }

                if (message.IsConnectedCommand())
                {
                    _isConnected = true;
                    await OnWebSocketConnectedAsync();
                }
                else
                {
                    await OnMessageReceivedAsync(new MessageReceivedEventArgs { Message = message, Timestamp = timestamp }, cancellationToken);
                }
            }
            catch (Exception e)
            {
                LogFactory.Instance.Exception(e, "An error on web socket message callback");
            }
        }

        private async Task OnWebSocketErrorAsync(ReceivedMessage message)
        {
            try
            {
                var stompFrame = message.ConvertToStompFrame();
                if (stompFrame.Properties.Contains(Headers.Server.Message) && _stompErrorCallbackAsync != null)
                {
                    var errorMessage = stompFrame.Properties[Headers.Server.Message].ToString();
                    await _stompErrorCallbackAsync.Invoke(new StompConnectionException(errorMessage));
                }
            }
            catch (Exception e)
            {
                LogFactory.Instance.Exception(e, "An error on web socket error callback");
            }
        }

        private async Task CloseConnectionAsync(CancellationToken cancellationToken)
        {
            if (!_isConnected)
            {
                return;
            }

            _isConnected = false;

            switch (_webSocket.State)
            {
                case WebSocketState.Open:
                    await SendDisconnectAsync(cancellationToken);
                    await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, cancellationToken);
                    break;
                case WebSocketState.CloseReceived:
                    LogFactory.Instance.Info("Closing WebSocket due to `CloseReceived` state");
                    await _webSocket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, string.Empty, cancellationToken);
                    break;
                default:
                    LogFactory.Instance.Warning(string.Format("Unsupported ws state {0} when closing connection", _webSocket.State));
                    break;
            }

            _connectorCancellationTokenSource.Cancel();

            await OnWebSocketClosedAsync();
        }

        private Task SendDisconnectAsync(CancellationToken cancellationToken)
        {
            LogFactory.Instance.Debug("Sending Disconnect \"1000\"");
            return SendAsync(WebSocketMessages.DisconnectCode, cancellationToken);
        }

        private async Task OnWebSocketConnectedAsync()
        {
            _ = Task.Run(() => PeriodicallyRefreshTokenAsync(_connectorCancellationTokenSource.Token));
            _ = Task.Run(() => PeriodicallySendHeartBeatMessagesAsync(_connectorCancellationTokenSource.Token));

            try
            {
                if (_connectionEstablishedCallbackAsync != null)
                    await _connectionEstablishedCallbackAsync.Invoke();
                LogFactory.Instance.Debug("WebSocket Connection established");
            }
            catch (Exception e)
            {
                LogFactory.Instance.Exception(e, "An error on web socket connected callback");
            }
        }

        private void PeriodicallyRefreshTokenAsync(CancellationToken cancellationToken)
        {
            if (_currentAuthToken == null)
            {
                throw new ArgumentException();
            }

            while (IsConnected && !cancellationToken.IsCancellationRequested)
            {
                var jwt = _jwtSecurityTokenHandler.ReadToken(_currentAuthToken);
                var expirationDate = new DateTimeOffset(jwt.ValidTo, TimeSpan.Zero);
                var timer = new System.Timers.Timer();
                timer.Interval = 5 * 60 * 1000;
                timer.Elapsed += async (sender,e) => {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        timer.Stop();
                        timer = null;
                    }
                    await RefreshAccessTokenAsync(cancellationToken);
                };
                timer.AutoReset = true;
                timer.Start();
            }
        }

        private async Task RefreshAccessTokenAsync(CancellationToken cancellationToken)
        {
            var previousAuthToken = _currentAuthToken;
            _currentAuthToken = await SsoService.Instance.GetAuthTokenAsync();

            var refreshTokenCommand = new NordPoolC.Commands.TokenRefreshCommand
            {
                NewToken = _currentAuthToken,
                OldToken = previousAuthToken
            };

            var stompFrame = StompMessageFactory.SendFrame(refreshTokenCommand.ToString(), "/v1/command");

            LogFactory.Instance.Debug("Sending token refresh frame");

            await SendAsync(stompFrame.ConvertToMessageBytes(), cancellationToken);
        }

        private void PeriodicallySendHeartBeatMessagesAsync(CancellationToken cancellationToken)
        {
            if (_webSocketOptions.HeartbeatOutgoingInterval == 0)
            {
                return;
            }

            var timer = new System.Timers.Timer();
            timer.Interval = _webSocketOptions.HeartbeatOutgoingInterval;
            timer.Elapsed += async (sender, e) => {
                if (!IsConnected || cancellationToken.IsCancellationRequested)
                {
                    timer.Stop();
                    timer = null;
                }

                LogFactory.Instance.Debug("Sending heartbeat frame");

                await SendAsync(WebSocketMessages.ClientHeartBeat, cancellationToken);
            };
            timer.AutoReset = true;
            timer.Start();
        }

        private async Task OnWebSocketClosedAsync()
        {
            try
            {
                if (_connectionClosedCallbackAsync != null)
                    await _connectionClosedCallbackAsync.Invoke();
            }
            catch (Exception e)
            {
                LogFactory.Instance.Exception(e, "Error on web socket closed callback");
            }
        }

        private async Task OnMessageReceivedAsync(MessageReceivedEventArgs eventArgs, CancellationToken cancellationToken)
        {
            LogFactory.Instance.Debug(string.Format("Received new message: {0}", eventArgs.Message));

            try
            {
                if (_messageReceivedCallbackAsync != null)
                    await _messageReceivedCallbackAsync.Invoke(eventArgs, cancellationToken);
            }
            catch (Exception e)
            {
                LogFactory.Instance.Exception(e, "Error on web socket message received callback");
            }
        }

        /// <summary>
        /// 异步释放
        /// </summary>
        /// <returns></returns>
        public async Task DisposeAsync()
        {
            if (_isConnected)
            {
                await CloseConnectionAsync(CancellationToken.None);
            }

            _connectorCancellationTokenSource.Dispose();
            _webSocket.Dispose();
        }
    }
}
