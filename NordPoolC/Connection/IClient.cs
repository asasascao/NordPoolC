using Apache.NMS.Stomp.Protocol;
using NordPoolC.Enums;
using NordPoolC.Stomp;
using NordPoolC.Subscription;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NordPoolC.Connection
{
    public interface IClient
    {
        /// <summary>
        /// 客户端id
        /// </summary>
        string ClientId { get; }

        /// <summary>
        /// 连接类型-无 交易 市场数据
        /// </summary>
        ConnectServiceType ClientTarget { get; }

        /// <summary>
        /// 连接列表
        /// </summary>
        List<IStompConnecter> Connecters { get; }

        /// <summary>
        /// 主连接
        /// </summary>
        IStompConnecter MainConnecter { get; }

        /// <summary>
        /// 设置主连接
        /// </summary>
        /// <param name="stompConnecter">主连接</param>
        /// <returns>连接池</returns>
        IClient SetMainConnecter(IStompConnecter stompConnecter);

        /// <summary>
        /// 添加连接
        /// </summary>
        /// <param name="stompConnecter">连接</param>
        /// <returns>连接池</returns>
        IClient AddConnecter(IStompConnecter stompConnecter);

        /// <summary>
        /// 打开主连接
        /// </summary>
        /// <param name="cancellationToken">取消token</param>
        /// <returns>是否成功</returns>
        Task<bool> OpenAsync(CancellationToken cancellationToken);

        /// <summary>
        /// 打开所有连接
        /// </summary>
        /// <param name="cancellationToken">取消token</param>
        /// <returns>是否成功</returns>
        Task<bool> OpenAllAsync(CancellationToken cancellationToken);

        /// <summary>
        /// 订阅
        /// </summary>
        /// <typeparam name="TValue">订阅类型</typeparam>
        /// <param name="request">订阅请求</param>
        /// <param name="cancellationToken">取消token</param>
        /// <returns>订阅连接</returns>
        Task<ISubscription<TValue>> SubscribeAsync<TValue>(SubscribeRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// 订阅所有
        /// </summary>
        /// <typeparam name="TValue">订阅类型</typeparam>
        /// <param name="request">订阅请求</param>
        /// <param name="cancellationToken">取消token</param>
        /// <returns>订阅连接</returns>
        Task<IDictionary<IStompConnecter, ISubscription<TValue>>> SubscribeAllAsync<TValue>(SubscribeRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// 发送
        /// </summary>
        /// <typeparam name="TRequest">请求类型</typeparam>
        /// <param name="payloadFrame">StompFrame</param>
        /// <param name="cancellationToken">取消token</param>
        /// <returns>void</returns>
        Task SendAsync<TRequest>(StompFrame payloadFrame, CancellationToken cancellationToken);

        /// <summary>
        /// 发送
        /// </summary>
        /// <typeparam name="TRequest">请求类型</typeparam>
        /// <param name="request">请求</param>
        /// <param name="destination">目标地址</param>
        /// <param name="cancellationToken">取消token</param>
        /// <returns>void</returns>
        Task SendAsync<TRequest>(TRequest request, string destination, CancellationToken cancellationToken)
            where TRequest : class, new();

        /// <summary>
        /// 压入发送队列
        /// </summary>
        /// <typeparam name="TRequest">请求类型</typeparam>
        /// <param name="request">请求</param>
        /// <param name="destination">目标地址</param>
        /// <returns>void</returns>
        Task PushToSendAsync<TRequest>(TRequest request, string destination)
            where TRequest : class, new();

        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <param name="subscriptionId">订阅id</param>
        /// <param name="cancellationToken">取消token</param>
        /// <returns>void</returns>
        Task UnsubscribeAsync(string subscriptionId, CancellationToken cancellationToken);

        /// <summary>
        /// 取消所有订阅
        /// </summary>
        /// <param name="subscriptionId">订阅id</param>
        /// <param name="cancellationToken">取消token</param>
        /// <returns>void</returns>
        Task UnsubscribeAllAsync(string subscriptionId, CancellationToken cancellationToken);

        /// <summary>
        /// 断连
        /// </summary>
        /// <param name="cancellationToken">取消token</param>
        /// <returns>void</returns>
        Task DisconnectAsync(CancellationToken cancellationToken);

        /// <summary>
        /// 断连
        /// </summary>
        /// <returns>void</returns>
        Task DisconnectAsync();
    }
}
