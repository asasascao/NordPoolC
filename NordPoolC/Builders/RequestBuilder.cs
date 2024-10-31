using Nordpool.ID.PublicApi.v1.Order;
using Nordpool.ID.PublicApi.v1.Order.Request;
using NordPoolC.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NordPoolC.Builders
{
    public class RequestBuilder
    {
        /// <summary>
        /// 创建订单请求
        /// </summary>
        /// <returns>订单请求</returns>
        public static OrderEntryRequest CreateOrderEntryRequest()
            => new OrderEntryRequest();

        /// <summary>
        /// 创建改单请求
        /// </summary>
        /// <returns>改单请求</returns>
        public static OrderModificationRequest CreateOrderModifyRequest()
            => new OrderModificationRequest();
    }
}
