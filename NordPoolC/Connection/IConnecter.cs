using Apache.NMS.Stomp.Protocol;
using NordPoolC.Subscription;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NordPoolC.Connection
{
    public interface IConnecter
    {
        /// <summary>
        /// 是否连接
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// 连接uri
        /// </summary>
        string ConnectionUri { get; }

        /// <summary>
        /// 连接
        /// </summary>
        /// <param name="cancellationToken">取消token</param>
        /// <returns>void</returns>
        Task ConnectAsync(CancellationToken cancellationToken);

        /// <summary>
        /// 发送
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="cancellationToken">取消token</param>
        /// <returns>void</returns>
        Task SendAsync(string message, CancellationToken cancellationToken);

        /// <summary>
        /// 发送
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="cancellationToken">取消token</param>
        /// <param name="encoding">编码</param>
        /// <returns>void</returns>
        Task SendAsync(string message, CancellationToken cancellationToken, Encoding encoding);

        /// <summary>
        /// 发送
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="cancellationToken">取消token</param>
        /// <returns>void</returns>
        Task SendAsync(byte[] message, CancellationToken cancellationToken);

        /// <summary>
        /// 释放
        /// </summary>
        /// <returns>void</returns>
        Task DisposeAsync();
    }
}
