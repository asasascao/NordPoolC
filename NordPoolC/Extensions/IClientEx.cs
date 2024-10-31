using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Nordpool.ID.PublicApi.v1;
using Nordpool.ID.PublicApi.v1.Contract;
using Nordpool.ID.PublicApi.v1.Order.Request;
using Nordpool.ID.PublicApi.v1.Statistic;
using Nordpool.ID.PublicApi.v1.Throttling;
using NordPoolC.Builders;
using NordPoolC.Config;
using NordPoolC.Connection;
using NordPoolC.Enums;
using NordPoolC.Logger;
using NordPoolC.Message;
using NordPoolC.Model;
using NordPoolC.Stomp;
using NordPoolC.Subscription;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NordPoolC.Extensions
{
    public static class IClientEx
    {
        /// <summary>
        /// 创建websocket stomp连接
        /// </summary>
        /// <param name="client">连接池</param>
        /// <param name="clientTarget">连接类型-无 交易 市场数据</param>
        /// <returns>websocket stomp连接</returns>
        /// <exception cref="ArgumentOutOfRangeException">连接类型-无的时候触发</exception>
        public static WSStompConnecter CreateStompConnecter(this IClient client, ConnectServiceType clientTarget)
        {
            var _endpointsOptions = GlobalConfig.GetConfig<Endpoints>();
            var webSocketOptionsForClient = _endpointsOptions.Trading;
            if (clientTarget == ConnectServiceType.TRADING)
            {
                webSocketOptionsForClient = _endpointsOptions.Trading;
            }
            if (clientTarget == ConnectServiceType.MARKET_DATA)
            {
                webSocketOptionsForClient = _endpointsOptions.MarketData;
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(clientTarget));
            }
            var connecter = new WSStompConnecter(webSocketOptionsForClient);
            connecter.Parent = client;
            return connecter;
        }

        /// <summary>
        /// delivery areas
        /// </summary>
        /// <param name="client">连接池</param>
        /// <param name="subscribeRequestBuilder">订阅请求创建器</param>
        /// <returns>void</returns>
        public static async Task SubscribeDeliveryAreasAsync(this IClient client, SubscribeRequestBuilder subscribeRequestBuilder)
            => await SubscribeDeliveryAreasAsync(client, subscribeRequestBuilder, CancellationToken.None);

        /// <summary>
        /// delivery areas
        /// </summary>
        /// <param name="client">连接池</param>
        /// <param name="subscribeRequestBuilder">订阅请求创建器</param>
        /// <param name="cancellationToken">取消token</param>
        /// <returns>void</returns>
        public static async Task SubscribeDeliveryAreasAsync(this IClient client, SubscribeRequestBuilder subscribeRequestBuilder, CancellationToken cancellationToken)
        {
            var deliveryAreasRequest = subscribeRequestBuilder.CreateDeliveryAreas();
            var subscription = await client.SubscribeAsync<DeliveryAreaRow>(deliveryAreasRequest, cancellationToken);
            ReadSubscriptionChannel(client.ClientTarget, subscription, cancellationToken);
        }

        /// <summary>
        /// 配置 订阅
        /// </summary>
        /// <param name="client">连接池</param>
        /// <param name="subscribeRequestBuilder">订阅请求创建器</param>
        /// <returns>void</returns>
        public static async Task SubscribeConfigurationsAsync(this IClient client, SubscribeRequestBuilder subscribeRequestBuilder)
            => await SubscribeConfigurationsAsync(client, subscribeRequestBuilder, CancellationToken.None);

        /// <summary>
        /// 配置 订阅
        /// </summary>
        /// <param name="client">连接池</param>
        /// <param name="subscribeRequestBuilder">订阅请求创建器</param>
        /// <param name="cancellationToken">取消token</param>
        /// <returns>void</returns>
        public static async Task SubscribeConfigurationsAsync(this IClient client, SubscribeRequestBuilder subscribeRequestBuilder, CancellationToken cancellationToken)
        {
            var configurationsSubscription = subscribeRequestBuilder.CreateConfiguration();
            var subscription = await client.SubscribeAsync<ConfigurationRow>(configurationsSubscription, cancellationToken);
            ReadSubscriptionChannel(client.ClientTarget, subscription, cancellationToken);
        }

        /// <summary>
        /// 订阅订单回报
        /// </summary>
        /// <param name="client">连接池</param>
        /// <param name="subscribeRequestBuilder">订阅请求创建器</param>
        /// <param name="publishingMode">发送方式</param>
        /// <returns>void</returns>
        public static async Task SubscribeOrderExecutionReportAsync(this IClient client, SubscribeRequestBuilder subscribeRequestBuilder, PublishingMode publishingMode)
            => await SubscribeOrderExecutionReportAsync(client, subscribeRequestBuilder, publishingMode, CancellationToken.None);

        /// <summary>
        /// 订阅订单回报
        /// </summary>
        /// <param name="client">连接池</param>
        /// <param name="subscribeRequestBuilder">订阅请求创建器</param>
        /// <param name="publishingMode">发送方式</param>
        /// <param name="cancellationToken">取消token</param>
        /// <returns>void</returns>
        public static async Task SubscribeOrderExecutionReportAsync(this IClient client, SubscribeRequestBuilder subscribeRequestBuilder, PublishingMode publishingMode,
            CancellationToken cancellationToken)
        {
            var orderExecutionReportSubscription = subscribeRequestBuilder.CreateOrderExecutionReport(publishingMode);
            var subscription = await client.SubscribeAsync<OrderExecutionReport>(orderExecutionReportSubscription, cancellationToken);
            ReadSubscriptionChannel(client.ClientTarget, subscription, cancellationToken);
        }

        /// <summary>
        /// 订阅合约
        /// </summary>
        /// <param name="client">连接池</param>
        /// <param name="subscribeRequestBuilder">订阅请求创建器</param>
        /// <param name="publishingMode">发送方式</param>
        /// <returns>void</returns>
        public static async Task SubscribeContractsAsync(this IClient client, SubscribeRequestBuilder subscribeRequestBuilder, PublishingMode publishingMode)
            => await SubscribeContractsAsync(client, subscribeRequestBuilder, publishingMode, CancellationToken.None);

        /// <summary>
        /// 订阅合约
        /// </summary>
        /// <param name="client">连接池</param>
        /// <param name="subscribeRequestBuilder">订阅请求创建器</param>
        /// <param name="publishingMode">发送方式</param>
        /// <param name="cancellationToken">取消token</param>
        /// <returns>void</returns>
        public static async Task SubscribeContractsAsync(this IClient client, SubscribeRequestBuilder subscribeRequestBuilder, PublishingMode publishingMode,
            CancellationToken cancellationToken)
        {
            var contractsSubscription = subscribeRequestBuilder.CreateContracts(publishingMode);
            var subscription = await client.SubscribeAsync<ContractRow>(contractsSubscription, cancellationToken);
            ReadSubscriptionChannel(client.ClientTarget, subscription, cancellationToken);
        }

        /// <summary>
        /// 订阅local view
        /// </summary>
        /// <param name="client">连接池</param>
        /// <param name="subscribeRequestBuilder">订阅请求创建器</param>
        /// <param name="publishingMode">发送方式</param>
        /// <returns>void</returns>
        public static async Task SubscribeLocalViewsAsync(this IClient client, SubscribeRequestBuilder subscribeRequestBuilder, PublishingMode publishingMode)
            => await SubscribeLocalViewsAsync(client, subscribeRequestBuilder, publishingMode, CancellationToken.None);

        /// <summary>
        /// 订阅local view
        /// </summary>
        /// <param name="client">连接池</param>
        /// <param name="subscribeRequestBuilder">订阅请求创建器</param>
        /// <param name="publishingMode">发送方式</param>
        /// <param name="cancellationToken">取消token</param>
        /// <returns>void</returns>
        public static async Task SubscribeLocalViewsAsync(this IClient client, SubscribeRequestBuilder subscribeRequestBuilder, PublishingMode publishingMode,
            CancellationToken cancellationToken)
        {
            var localViewsSubscription = subscribeRequestBuilder.CreateLocalViews(publishingMode, GlobalConfig.DemoArea);
            var subscription = await client.SubscribeAsync<LocalViewRow>(localViewsSubscription, cancellationToken);
            ReadSubscriptionChannel(client.ClientTarget, subscription, cancellationToken);
        }

        /// <summary>
        /// 订阅私密交易
        /// </summary>
        /// <param name="client">连接池</param>
        /// <param name="subscribeRequestBuilder">订阅请求创建器</param>
        /// <param name="publishingMode">发送方式</param>
        /// <returns>void</returns>
        public static async Task SubscribePrivateTradesAsync(this IClient client, SubscribeRequestBuilder subscribeRequestBuilder, PublishingMode publishingMode)
           => await SubscribePrivateTradesAsync(client, subscribeRequestBuilder, publishingMode, CancellationToken.None);

        /// <summary>
        /// 订阅私密交易
        /// </summary>
        /// <param name="client">连接池</param>
        /// <param name="subscribeRequestBuilder">订阅请求创建器</param>
        /// <param name="publishingMode">发送方式</param>
        /// <param name="cancellationToken">取消token</param>
        /// <returns>void</returns>
        public static async Task SubscribePrivateTradesAsync(this IClient client, SubscribeRequestBuilder subscribeRequestBuilder, PublishingMode publishingMode,
            CancellationToken cancellationToken)
        {
            var privateTradesSubscription = subscribeRequestBuilder.CreatePrivateTrades(publishingMode);
            var subscription = await client.SubscribeAsync<PrivateTradeRow>(privateTradesSubscription, cancellationToken);
            ReadSubscriptionChannel(client.ClientTarget, subscription, cancellationToken);
        }

        /// <summary>
        /// 订阅 ticker
        /// </summary>
        /// <param name="client">连接池</param>
        /// <param name="subscribeRequestBuilder">订阅请求创建器</param>
        /// <param name="publishingMode">发送方式</param>
        /// <returns>void</returns>
        public static async Task SubscribeTickersAsync(this IClient client, SubscribeRequestBuilder subscribeRequestBuilder, PublishingMode publishingMode)
           => await SubscribeTickersAsync(client, subscribeRequestBuilder, publishingMode, CancellationToken.None);

        /// <summary>
        /// 订阅 ticker
        /// </summary>
        /// <param name="client">连接池</param>
        /// <param name="subscribeRequestBuilder">订阅请求创建器</param>
        /// <param name="publishingMode">发送方式</param>
        /// <param name="cancellationToken">取消token</param>
        /// <returns>void</returns>
        public static async Task SubscribeTickersAsync(this IClient client, SubscribeRequestBuilder subscribeRequestBuilder, PublishingMode publishingMode,
            CancellationToken cancellationToken)
        {
            var tickersSubscription = subscribeRequestBuilder.CreateTicker(publishingMode);
            var subscription = await client.SubscribeAsync<PublicTradeRow>(tickersSubscription, cancellationToken);
            ReadSubscriptionChannel(client.ClientTarget, subscription, cancellationToken);
        }

        /// <summary>
        /// 订阅 my ticker
        /// </summary>
        /// <param name="client">连接池</param>
        /// <param name="subscribeRequestBuilder">订阅请求创建器</param>
        /// <param name="publishingMode">发送方式</param>
        /// <returns>void</returns>
        public static async Task SubscribeMyTickersAsync(this IClient client, SubscribeRequestBuilder subscribeRequestBuilder, PublishingMode publishingMode)
           => await SubscribeMyTickersAsync(client, subscribeRequestBuilder, publishingMode, CancellationToken.None);

        /// <summary>
        /// 订阅 my ticker
        /// </summary>
        /// <param name="client">连接池</param>
        /// <param name="subscribeRequestBuilder">订阅请求创建器</param>
        /// <param name="publishingMode">发送方式</param>
        /// <param name="cancellationToken">取消token</param>
        /// <returns>void</returns>
        public static async Task SubscribeMyTickersAsync(this IClient client, SubscribeRequestBuilder subscribeRequestBuilder, PublishingMode publishingMode,
            CancellationToken cancellationToken)
        {
            var myTickersSubscription = subscribeRequestBuilder.CreateMyTicker(publishingMode);
            var subscription = await client.SubscribeAsync<PublicTradeRow>(myTickersSubscription, cancellationToken);
            ReadSubscriptionChannel(client.ClientTarget, subscription, cancellationToken);
        }

        /// <summary>
        /// 订阅公共统计数据
        /// </summary>
        /// <param name="client">连接池</param>
        /// <param name="subscribeRequestBuilder">订阅请求创建器</param>
        /// <param name="publishingMode">发送方式</param>
        /// <returns>void</returns>
        public static async Task SubscribePublicStatisticsAsync(this IClient client, SubscribeRequestBuilder subscribeRequestBuilder, PublishingMode publishingMode)
           => await SubscribePublicStatisticsAsync(client, subscribeRequestBuilder, publishingMode, CancellationToken.None);

        /// <summary>
        /// 订阅公共统计数据
        /// </summary>
        /// <param name="client">连接池</param>
        /// <param name="subscribeRequestBuilder">订阅请求创建器</param>
        /// <param name="publishingMode">发送方式</param>
        /// <param name="cancellationToken">取消token</param>
        /// <returns>void</returns>
        public static async Task SubscribePublicStatisticsAsync(this IClient client, SubscribeRequestBuilder subscribeRequestBuilder, PublishingMode publishingMode,
            CancellationToken cancellationToken)
        {
            var publicStatisticsSubscription = subscribeRequestBuilder.CreatePublicStatistics(publishingMode, GlobalConfig.DemoArea);
            var subscription =
                await client.SubscribeAsync<PublicStatisticRow>(publicStatisticsSubscription, cancellationToken);
            ReadSubscriptionChannel(client.ClientTarget, subscription, cancellationToken);
        }

        /// <summary>
        /// 订阅限流
        /// </summary>
        /// <param name="client">连接池</param>
        /// <param name="subscribeRequestBuilder">订阅请求创建器</param>
        /// <param name="publishingMode">发送方式</param>
        /// <returns>void</returns>
        public static async Task SubscribeThrottlingLimitsAsync(this IClient client, SubscribeRequestBuilder subscribeRequestBuilder, PublishingMode publishingMode)
           => await SubscribeThrottlingLimitsAsync(client, subscribeRequestBuilder, publishingMode, CancellationToken.None);

        /// <summary>
        /// 订阅限流
        /// </summary>
        /// <param name="client">连接池</param>
        /// <param name="subscribeRequestBuilder">订阅请求创建器</param>
        /// <param name="publishingMode">发送方式</param>
        /// <param name="cancellationToken">取消token</param>
        /// <returns>void</returns>
        public static async Task SubscribeThrottlingLimitsAsync(this IClient client, SubscribeRequestBuilder subscribeRequestBuilder, PublishingMode publishingMode,
            CancellationToken cancellationToken)
        {
            var throttlingLimitsSubscription = subscribeRequestBuilder.CreateThrottlingLimits(publishingMode);
            var subscription = await client.SubscribeAsync<ThrottlingLimitMessage>(throttlingLimitsSubscription, cancellationToken);
            ReadSubscriptionChannel(client.ClientTarget, subscription, cancellationToken);

            // Set automatic unsubscription of throttling limit topic after 10s
            _ = Task.Run(async () =>
            {
                await Task.Delay(10000, cancellationToken);
                await client.UnsubscribeAsync(subscription.Id, cancellationToken);
            }, cancellationToken);
        }

        /// <summary>
        /// 订阅 capacities
        /// </summary>
        /// <param name="client">连接池</param>
        /// <param name="subscribeRequestBuilder">订阅请求创建器</param>
        /// <param name="publishingMode">发送方式</param>
        /// <returns>void</returns>
        public static async Task SubscribeCapacitiesAsync(this IClient client, SubscribeRequestBuilder subscribeRequestBuilder, PublishingMode publishingMode)
           => await SubscribeCapacitiesAsync(client, subscribeRequestBuilder, publishingMode, CancellationToken.None);

        /// <summary>
        /// 订阅 capacities
        /// </summary>
        /// <param name="client">连接池</param>
        /// <param name="subscribeRequestBuilder">订阅请求创建器</param>
        /// <param name="publishingMode">发送方式</param>
        /// <param name="cancellationToken">取消token</param>
        /// <returns>void</returns>
        public static async Task SubscribeCapacitiesAsync(IClient client, SubscribeRequestBuilder subscribeRequestBuilder, PublishingMode publishingMode,
            CancellationToken cancellationToken)
        {
            var contractsSubscription = subscribeRequestBuilder.CreateCapacities(publishingMode, GlobalConfig.DemoArea);
            var subscription = await client.SubscribeAsync<CapacityRow>(contractsSubscription, cancellationToken);
            ReadSubscriptionChannel(client.ClientTarget, subscription, cancellationToken);
        }

        private static void ReadSubscriptionChannel<TValue>(ConnectServiceType clientTarget,
            ISubscription<TValue> subscription, CancellationToken cancellationToken)
        {
            _ = Task.Run(async () =>
            {
                var channelReader = subscription.OutputChannel;
                while (await channelReader.WaitToReadAsync(cancellationToken))
                {
                    if (!channelReader.TryRead(out var message))
                    {
                        continue;
                    }

                    GlobalCacheProxy.Instance.SetCache(message.Data);
                    var responseString = JsonConvert.SerializeObject(message);

                    // Trimming response content
                    responseString = responseString.Length > 250
                        ? responseString.Substring(0, 250) + "..."
                        : responseString;

                    LogFactory.Instance.Info(string.Format("[{0}][Frame({1}):Response] {2}", clientTarget, subscription.Id, responseString));
                }
            }, cancellationToken);
        }

        /// <summary>
        /// 下单
        /// </summary>
        /// <param name="client">连接池</param>
        /// <param name="req">订单请求</param>
        /// <returns>取消token</returns>
        public static async Task SendOrderEntryRequestAsync(this IClient client, OrderEntryRequest req)
          => await SendOrderEntryRequestAsync(client, req, CancellationToken.None);

        /// <summary>
        /// 下单
        /// </summary>
        /// <param name="client">连接池</param>
        /// <param name="req">订单请求</param>
        /// <param name="cancellationToken">取消token</param>
        /// <returns>void</returns>
        public static async Task SendOrderEntryRequestAsync(this IClient client, OrderEntryRequest req, CancellationToken cancellationToken)
        {
            GlobalCacheProxy.Instance.SetCache(req);
            var dest= DestinationBuilder.ComposeDestination(GlobalConfig.Version, "orderEntryRequest");
            LogFactory.Instance.Info(string.Format("[{0}] Attempting to send correct order request.", client.ClientTarget));
            await client.SendAsync(req, dest, cancellationToken);
        }

        /// <summary>
        /// 推单
        /// </summary>
        /// <param name="client">连接池</param>
        /// <param name="req">订单请求</param>
        /// <returns>void</returns>
        public static async Task PushOrderEntryToQueueRequestAsync(this IClient client, OrderEntryRequest req)
        {
            GlobalCacheProxy.Instance.SetCache(req);
            var dest = DestinationBuilder.ComposeDestination(GlobalConfig.Version, "orderEntryRequest");
            LogFactory.Instance.Info(string.Format("[{0}] Attempting to send correct order request.", client.ClientTarget));
            await client.PushToSendAsync(req, dest);
        }

        /// <summary>
        /// 改单
        /// </summary>
        /// <param name="client">连接池</param>
        /// <param name="req">修改订单请求</param>
        /// <returns>void</returns>
        public static async Task SendOrderModifyRequestAsync(this IClient client, OrderModificationRequest req)
          => await SendOrderModifyRequestAsync(client, req, CancellationToken.None);

        /// <summary>
        /// 改单
        /// </summary>
        /// <param name="client">连接池</param>
        /// <param name="req">修改订单请求</param>
        /// <param name="cancellationToken">取消token</param>
        /// <returns>void</returns>
        public static async Task SendOrderModifyRequestAsync(this IClient client, OrderModificationRequest req, CancellationToken cancellationToken)
        {
            GlobalCacheProxy.Instance.SetCache(req);
            var dest = DestinationBuilder.ComposeDestination(GlobalConfig.Version, "orderModificationRequest");
            LogFactory.Instance.Info(string.Format("[{0}] Attempting to send an correct order modification request.", client.ClientTarget));
            await client.SendAsync(req, dest, cancellationToken);
        }
    }
}
