using Apache.NMS.Stomp.Commands;
using Apache.NMS.Stomp.Protocol;
using CaoNC.Microsoft.Extensions.Logging;
using CaoNC.Microsoft.VisualStudio.Threading;
using CaoNC.System.Channels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NordPoolC.Config;
using NordPoolC.Connection;
using NordPoolC.Enums;
using NordPoolC.Exceptions;
using NordPoolC.Extensions;
using NordPoolC.Logger;
using NordPoolC.Message;
using NordPoolC.Security;
using NordPoolC.Subscription;
using NordPoolC.WebSockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace NordPoolC.Stomp
{
    public class StompClient : IClient
    {
        public ConnectServiceType ClientTarget { get; }
        public string ClientId { get; }


        List<IStompConnecter> connecters = new List<IStompConnecter>();
        public List<IStompConnecter> Connecters { get; }
        public IStompConnecter MainConnecter => connecters.FirstOrDefault(o => o.IsMain);

        public StompClient(string clientId, ConnectServiceType target)
        {
            ClientTarget = target;
            ClientId = clientId;
        }

        public IClient SetMainConnecter(IStompConnecter stompConnecter)
        {
            if (connecters == null)
            {
                connecters = new List<IStompConnecter>();
            }
            var main_connect=connecters.FirstOrDefault(o => o.IsMain);
            if (main_connect != null)
            {
                main_connect.IsMain = false;
            }
            stompConnecter.IsMain = true;
            if (!connecters.Contains(stompConnecter))
            {
                connecters.Add(stompConnecter);
            }
            return this;
        }

        public IClient AddConnecter(IStompConnecter stompConnecter)
        {
            if(connecters==null)
            {
                connecters = new List<IStompConnecter>();
            }
            if(connecters.Count==0)
            {
                stompConnecter.IsMain = true;
            }
            connecters.Add(stompConnecter);
            return this;
        }

        public async Task DisconnectAsync()
        {
            await DisconnectAsync(CancellationToken.None);
        }

        public async Task DisconnectAsync(CancellationToken cancellationToken)
        {
            if (connecters == null) return;
            if (connecters.Count<=0) return;
            foreach (var connecter in connecters)
            {
                await connecter.DisconnectAsync(cancellationToken);
            }
        }

        public async Task<bool> OpenAsync(CancellationToken cancellationToken)
        {
            if (MainConnecter == null) return false;
            if (connecters.Count <= 0) return false;
            return await MainConnecter.ConnectAsync(cancellationToken);
        }

        public async Task<bool> OpenAllAsync(CancellationToken cancellationToken)
        {
            if (connecters == null) return false;
            if (connecters.Count <= 0) return false;
            bool res = true;
            foreach (var connecter in connecters)
            {
                res = await connecter.ConnectAsync(cancellationToken);
                if(res==false)
                {
                    return false;
                }
            }
            return true;
        }

        public async Task SendAsync<TRequest>(StompFrame payloadFrame, CancellationToken cancellationToken)
        {
            if (MainConnecter == null) return;
            await MainConnecter.SendAsync(payloadFrame, cancellationToken);
        }

        public async Task SendAsync<TRequest>(TRequest request, string destination, CancellationToken cancellationToken) 
            where TRequest : class, new()
        {
            if (MainConnecter == null) return;
            await MainConnecter.SendAsync(request, destination, cancellationToken);
        }

        public async Task PushToSendAsync<TRequest>(TRequest request, string destination)
            where TRequest : class, new()
        {
            var connet=connecters.GroupBy(c => c.RequestsCount)
            .OrderBy(g => g.Count())
            .FirstOrDefault()?.FirstOrDefault();
            if (connet == null) return;
            await connet.PushToSendAsync(request, destination);
        }

        public async Task<IDictionary<IStompConnecter, ISubscription<TValue>>> SubscribeAllAsync<TValue>(SubscribeRequest request, CancellationToken cancellationToken)
        {
            if (connecters == null) return null;
            if (connecters.Count <= 0) return null;
            IDictionary<IStompConnecter, ISubscription<TValue>> reslist = new Dictionary<IStompConnecter, ISubscription<TValue>>();
            foreach (var connecter in connecters)
            {
                var res=await connecter.SubscribeAsync<TValue>(request, cancellationToken);
                reslist.Add(connecter, res);
            }
            return reslist;
        }

        public async Task UnsubscribeAllAsync(string subscriptionId, CancellationToken cancellationToken)
        {
            if (connecters == null) return;
            if (connecters.Count <= 0) return;
            foreach (var connecter in connecters)
            {
                await connecter.UnsubscribeAsync(subscriptionId, cancellationToken);
            }
        }

        public async Task<ISubscription<TValue>> SubscribeAsync<TValue>(SubscribeRequest request, CancellationToken cancellationToken)
        {
            if (MainConnecter == null) return null;
            return await MainConnecter.SubscribeAsync<TValue>(request, cancellationToken);
        }

        public async Task UnsubscribeAsync(string subscriptionId, CancellationToken cancellationToken)
        {
            if (MainConnecter == null) return;
            await MainConnecter.UnsubscribeAsync(subscriptionId, cancellationToken);
        }
    }
}
