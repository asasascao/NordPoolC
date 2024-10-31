using CaoNC.Microsoft.Extensions.Logging;
using CaoNC.Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Nordpool.ID.PublicApi.v1;
using Nordpool.ID.PublicApi.v1.Statistic;
using Nordpool.ID.PublicApi.v1.Throttling;
using NordPoolC.Config;
using NordPoolC.Connection;
using NordPoolC.Enums;
using NordPoolC.Extensions;
using NordPoolC.Logger;
using NordPoolC.Stomp;
using NordPoolC.Subscription;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NordPoolC
{
    public class NPool : INPool
    {
        /// <summary>
        /// 配置ini 配置文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns>交易池</returns>
        public INPool ConfigIniFile(string path)
        {
            GlobalConfig.LoadFromIniFile(path);
            return this;
        }

        /// <summary>
        /// 创建订阅请求创建器
        /// </summary>
        /// <returns>订阅请求创建器</returns>
        public SubscribeRequestBuilder CreateSubscribeRequestBuilder()
        {
            return SubscribeRequestBuilder.CreateBuilder(GlobalConfig.GetConfig<Credentials>().Username, GlobalConfig.Version);
        }

        /// <summary>
        /// 创建市场数据 连接池
        /// </summary>
        /// <returns>连接池</returns>
        public async Task<IClient> CreateMarketDataClient()
        {
            return await CreateMarketDataClient(new CancellationTokenSource());
        }

        /// <summary>
        /// 创建市场数据 连接池
        /// </summary>
        /// <param name="cancellationTokenSource">取消token</param>
        /// <returns>连接池</returns>
        public async Task<IClient> CreateMarketDataClient(CancellationTokenSource cancellationTokenSource)
        {
            return await StompClientEx.CreateAndOpenAsync(ConnectServiceType.MARKET_DATA, GlobalConfig.GetConfig<PConfig>().clientId, cancellationTokenSource.Token);
        }

        /// <summary>
        /// 创建交易 连接池
        /// </summary>
        /// <returns>连接池</returns>
        public async Task<IClient> CreateTradeClient()
        {
            return await CreateTradeClient(new CancellationTokenSource());
        }

        /// <summary>
        /// 创建交易 连接池
        /// </summary>
        /// <param name="cancellationTokenSource">取消token</param>
        /// <returns>连接池</returns>
        public async Task<IClient> CreateTradeClient(CancellationTokenSource cancellationTokenSource)
        {
            return await StompClientEx.CreateAndOpenAsync(ConnectServiceType.TRADING, GlobalConfig.GetConfig<PConfig>().clientId, cancellationTokenSource.Token);
        }
    }
}
