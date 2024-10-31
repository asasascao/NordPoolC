using Apache.NMS.Stomp.Protocol;
using NordPoolC.Subscription;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NordPoolC.Connection
{
    public interface IStompConnecter
    {
        /// <summary>
        /// 是否主连接
        /// </summary>
        bool IsMain { get; set; }

        /// <summary>
        /// 积压请求数
        /// </summary>
        int RequestsCount { get; }
        
        /// <summary>
        /// 连接
        /// </summary>
        /// <param name="cancellationToken">取消token</param>
        /// <returns>是否成功</returns>
        Task<bool> ConnectAsync(CancellationToken cancellationToken);

        /// <summary>
        /// 订阅
        /// </summary>
        /// <typeparam name="TValue">订阅类型</typeparam>
        /// <param name="request">订阅请求</param>
        /// <param name="cancellationToken">取消token</param>
        /// <returns>订阅连接</returns>
        Task<ISubscription<TValue>> SubscribeAsync<TValue>(SubscribeRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// 压入请求发送队列
        /// </summary>
        /// <typeparam name="TRequest">发送请求类型</typeparam>
        /// <param name="request">请求体</param>
        /// <param name="destination">目标路径</param>
        /// <returns>void</returns>
        Task PushToSendAsync<TRequest>(TRequest request, string destination)
            where TRequest : class, new();

        /// <summary>
        /// 发送请求
        /// </summary>
        /// <typeparam name="TRequest">发送请求类型</typeparam>
        /// <param name="request">请求体</param>
        /// <param name="destination">目标路径</param>
        /// <param name="cancellationToken">取消token</param>
        /// <returns>void</returns>
        Task SendAsync<TRequest>(TRequest request, string destination, CancellationToken cancellationToken)
            where TRequest : class, new();

        /// <summary>
        /// 发送请求
        /// </summary>
        /// <param name="payloadFrame">StompFrame</param>
        /// <param name="cancellationToken">取消token</param>
        /// <returns>void</returns>
        Task SendAsync(StompFrame payloadFrame, CancellationToken cancellationToken);

        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <param name="subscriptionId">订阅id</param>
        /// <param name="cancellationToken">取消token</param>
        /// <returns>void</returns>
        Task UnsubscribeAsync(string subscriptionId, CancellationToken cancellationToken);

        /// <summary>
        /// 断连
        /// </summary>
        /// <param name="cancellationToken">取消token</param>
        /// <returns>void</returns>
        Task DisconnectAsync(CancellationToken cancellationToken);
    }
}
