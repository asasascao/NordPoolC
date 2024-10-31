using NordPoolC.Connection;
using NordPoolC.Subscription;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NordPoolC
{
    public interface INPool
    {
        /// <summary>
        /// 配置ini 配置文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns>交易池</returns>
        INPool ConfigIniFile(string path);

        /// <summary>
        /// 创建订阅请求创建器
        /// </summary>
        /// <returns>订阅请求创建器</returns>
        SubscribeRequestBuilder CreateSubscribeRequestBuilder();

        /// <summary>
        /// 创建市场数据 连接池
        /// </summary>
        /// <returns>连接池</returns>
        Task<IClient> CreateMarketDataClient();

        /// <summary>
        /// 创建市场数据 连接池
        /// </summary>
        /// <param name="cancellationTokenSource">取消token</param>
        /// <returns>连接池</returns>
        Task<IClient> CreateMarketDataClient(CancellationTokenSource cancellationTokenSource);

        /// <summary>
        /// 创建交易 连接池
        /// </summary>
        /// <returns>连接池</returns>
        Task<IClient> CreateTradeClient();

        /// <summary>
        /// 创建交易 连接池
        /// </summary>
        /// <param name="cancellationTokenSource">取消token</param>
        /// <returns>连接池</returns>
        Task<IClient> CreateTradeClient(CancellationTokenSource cancellationTokenSource);
    }
}
