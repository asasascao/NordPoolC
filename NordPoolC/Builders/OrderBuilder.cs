using Nordpool.ID.PublicApi.v1.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NordPoolC.Builders
{
    public class OrderBuilder
    {
        /// <summary>
        /// 创建订单
        /// </summary>
        /// <returns>订单</returns>
        public static OrderEntry CreateOrder()
            => new OrderEntry();

        /// <summary>
        /// 创建改单
        /// </summary>
        /// <returns>改单</returns>
        public static OrderModification CreateModifyOrder()
            => new OrderModification();
    }
}
