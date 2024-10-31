using NordPoolC.Config;
using NordPoolC.Connection;
using NordPoolC.Enums;
using NordPoolC.Stomp;
using System;
using System.Threading.Tasks;
using System.Threading;
using Nordpool.ID.PublicApi.v1;
using NordPoolC.Subscription;

namespace NordPoolC.Extensions
{
    public class StompClientEx
    {
        /// <summary>
        /// 创建连接池并打开连接池
        /// </summary>
        /// <param name="clientTarget">连接类型-无 交易 市场数据</param>
        /// <param name="clientId">客户端id</param>
        /// <param name="cancellationToken">取消token</param>
        /// <param name="connectionAttemptTimeout">连接超时时间</param>
        /// <returns>连接池</returns>
        public static async Task<IClient> CreateAndOpenAsync(ConnectServiceType clientTarget, string clientId,
            CancellationToken cancellationToken, TimeSpan? connectionAttemptTimeout = null)
        {
            if (connectionAttemptTimeout == null)
            {
                connectionAttemptTimeout = TimeSpan.FromSeconds(60);
            }
            var timeoutCancellationToken = new CancellationTokenSource((TimeSpan)connectionAttemptTimeout).Token;
            var connectedCancellationToken = CancellationTokenSource.CreateLinkedTokenSource(timeoutCancellationToken, cancellationToken).Token;

            IClient client = new StompClient(clientId, clientTarget);
            client.SetMainConnecter(client.CreateStompConnecter(clientTarget));
            await client.OpenAsync(connectedCancellationToken);

            return client;
        }

        /// <summary>
        /// 创建连接池
        /// </summary>
        /// <param name="clientTarget">连接类型-无 交易 市场数据</param>
        /// <param name="clientId">客户端id</param>
        /// <returns>连接池</returns>
        public static IClient CreateAsync(ConnectServiceType clientTarget,string clientId)
        {
            IClient client = new StompClient(clientId, clientTarget);
            client.SetMainConnecter(client.CreateStompConnecter(clientTarget));

            return client;
        }
    }
}
