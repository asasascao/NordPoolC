using NordPoolC.Builders;
using NordPoolC.Enums;
using NordPoolC.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NordPoolC.Subscription
{
    public class SubscribeRequest
    {
        private static int _subCounter;

        /// <summary>
        /// 订阅id
        /// </summary>
        public string SubscriptionId { get; }

        /// <summary>
        /// 目标
        /// </summary>
        public string Destination { get; }

        /// <summary>
        /// 类别
        /// </summary>
        public string Type { get; }

        private SubscribeRequest(string subscriptionId, string type, string destination)
        {
            SubscriptionId = subscriptionId;
            Destination = destination;
            Type = type;
        }

        /// <summary>
        /// 发送区域
        /// </summary>
        /// <param name="subscriptionId">订阅id</param>
        /// <param name="user">用户</param>
        /// <param name="version">版本</param>
        /// <returns>订阅请求</returns>
        public static SubscribeRequest DeliveryAreas(string subscriptionId, string user, string version)
        {
            return new SubscribeRequest(subscriptionId, "delivery_areas", DestinationBuilder.ComposeDestination(user, version, PublishingMode.STREAMING, "deliveryAreas"));
        }

        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="subscriptionId">订阅id</param>
        /// <param name="user">用户</param>
        /// <param name="version">版本</param>
        /// <returns>订阅请求</returns>
        public static SubscribeRequest Configuration(string subscriptionId, string user, string version)
        {
            return new SubscribeRequest(subscriptionId, "configuration", DestinationBuilder.ComposeDestination(user, version, "configuration"));
        }

        /// <summary>
        /// 订单执行回报
        /// </summary>
        /// <param name="subscriptionId">订阅id</param>
        /// <param name="user">用户</param>
        /// <param name="version">版本</param>
        /// <param name="mode">发送模式</param>
        /// <returns>订阅请求</returns>
        public static SubscribeRequest OrderExecutionReports(string subscriptionId, string user, string version, PublishingMode mode)
        {
            return new SubscribeRequest(subscriptionId, "order_execution_report", DestinationBuilder.ComposeDestination(user, version, mode, "orderExecutionReport"));
        }

        /// <summary>
        /// 契约
        /// </summary>
        /// <param name="subscriptionId">订阅id</param>
        /// <param name="user">用户</param>
        /// <param name="version">版本</param>
        /// <param name="mode">发送模式</param>
        /// <returns>订阅请求</returns>
        public static SubscribeRequest Contracts(string subscriptionId, string user, string version, PublishingMode mode)
        {
            return new SubscribeRequest(subscriptionId, "contracts", DestinationBuilder.ComposeDestination(user, version, mode, "contracts"));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subscriptionId">订阅id</param>
        /// <param name="user">用户</param>
        /// <param name="version">版本</param>
        /// <param name="mode">发送模式</param>
        /// <param name="deliveryAreaId">发送区域id</param>
        /// <returns>订阅请求</returns>
        public static SubscribeRequest LocalView(string subscriptionId, string user, string version, PublishingMode mode, int deliveryAreaId)
        {
            return new SubscribeRequest(subscriptionId, "localview", DestinationBuilder.ComposeDestination(user, version, mode, $"localview/{deliveryAreaId}"));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subscriptionId">订阅id</param>
        /// <param name="user">用户</param>
        /// <param name="version">版本</param>
        /// <param name="mode">发送模式</param>
        /// <returns>订阅请求</returns>
        public static SubscribeRequest PrivateTrades(string subscriptionId, string user, string version, PublishingMode mode)
        {
            return new SubscribeRequest(subscriptionId, "private_trade", DestinationBuilder.ComposeDestination(user, version, mode, "privateTrade"));
        }

        /// <summary>
        /// 票
        /// </summary>
        /// <param name="subscriptionId">订阅id</param>
        /// <param name="user">用户</param>
        /// <param name="version">版本</param>
        /// <param name="mode">发送模式</param>
        /// <returns>订阅请求</returns>
        public static SubscribeRequest Ticker(string subscriptionId, string user, string version, PublishingMode mode)
        {
            return new SubscribeRequest(subscriptionId, "ticker", DestinationBuilder.ComposeDestination(user, version, mode, "ticker"));
        }

        /// <summary>
        /// 我的票
        /// </summary>
        /// <param name="subscriptionId">订阅id</param>
        /// <param name="user">用户</param>
        /// <param name="version">版本</param>
        /// <param name="mode">发送模式</param>
        /// <returns>订阅请求</returns>
        public static SubscribeRequest MyTicker(string subscriptionId, string user, string version, PublishingMode mode)
        {
            return new SubscribeRequest(subscriptionId, "my_ticker", DestinationBuilder.ComposeDestination(user, version, mode, "myTicker"));
        }

        /// <summary>
        /// 公共统计
        /// </summary>
        /// <param name="subscriptionId">订阅id</param>
        /// <param name="user">用户</param>
        /// <param name="version">版本</param>
        /// <param name="mode">发送模式</param>
        /// <param name="deliveryAreaId">发送区域id</param>
        /// <returns>订阅请求</returns>
        public static SubscribeRequest PublicStatistics(string subscriptionId, string user, string version, PublishingMode mode, int deliveryAreaId)
        {
            return new SubscribeRequest(subscriptionId, "public_statistics", DestinationBuilder.ComposeDestination(user, version, mode, $"publicStatistics/{deliveryAreaId}"));
        }

        /// <summary>
        /// 限流
        /// </summary>
        /// <param name="subscriptionId">订阅id</param>
        /// <param name="user">用户</param>
        /// <param name="version">版本</param>
        /// <param name="mode">发送模式</param>
        /// <returns>订阅请求</returns>
        public static SubscribeRequest ThrottlingLimits(string subscriptionId, string user, string version, PublishingMode mode)
        {
            return new SubscribeRequest(subscriptionId, "throttling_limits", DestinationBuilder.ComposeDestination(user, version, mode, "throttlingLimits"));
        }

        /// <summary>
        /// 容量
        /// </summary>
        /// <param name="subscriptionId">订阅id</param>
        /// <param name="user">用户</param>
        /// <param name="version">版本</param>
        /// <param name="mode">发送模式</param>
        /// <param name="deliveryAreaId">发送区域id</param>
        /// <param name="additionalDeliveryAreas">扩展发送区域</param>
        /// <returns>订阅请求</returns>
        public static SubscribeRequest Capacities(string subscriptionId, string user, string version, PublishingMode mode, int deliveryAreaId, IEnumerable<int> additionalDeliveryAreas)
        {
            var additionalAreasPart = additionalDeliveryAreas.Any() ? $"/{string.Join("-", additionalDeliveryAreas)}" : string.Empty;
            return new SubscribeRequest(subscriptionId, "capacities", DestinationBuilder.ComposeDestination(user, version, mode, $"capacities/{deliveryAreaId}{additionalAreasPart}"));
        }
    }
}
