using Nordpool.ID.PublicApi.v1.Order.Request;
using Nordpool.ID.PublicApi.v1.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nordpool.ID.PublicApi.v1.Portfolio;
using static System.Net.Mime.MediaTypeNames;
using System.Security.Policy;
using NordPoolC.Connection;
using NordPoolC.Message;
using Nordpool.ID.PublicApi.v1;

namespace NordPoolC.Model.EntryExtensions
{
    public static class OrderEntryRequestEx
    {
        public static OrderEntryRequest Init(this OrderEntryRequest req,
            IEnumerable<OrderEntry> orders, bool rejectpartially = false)
        {
            req.RequestId = Guid.NewGuid().ToString();
            req.RejectPartially = rejectpartially;
            req.Orders = orders.ToList();
            return req;
        }
    }

    public static class OrderModificationRequestEx
    {
        public static OrderModificationRequest Init(this OrderModificationRequest req,
            IEnumerable<OrderModification> orders, OrderModificationType? orderModificationType = OrderModificationType.DEAC)
        {
            req.RequestId = Guid.NewGuid().ToString();
            req.OrderModificationType = orderModificationType;
            req.Orders = orders.ToList();
            return req;
        }
    }

    public static class OrderEntryEx
    {
        public static OrderEntry Init(this OrderEntry order,
            ContractPortfolioAndArea cpa,
            DateTimeOffset expireTime,
            string order_text= "New order",
            int quantity=1000,
            long unitPrice = 2500,
            OrderSide side = OrderSide.BUY,
            OrderType orderType = OrderType.LIMIT,
            OrderState state = OrderState.ACTI,
            TimeInForce timeInForce = TimeInForce.GFS,
            ExecutionRestriction executionRestriction = ExecutionRestriction.NON,
            long clipSize=1,
            long clipPriceChange=1)
        {
            order.Text = order_text;
            order.ClientOrderId = Guid.NewGuid().ToString();
            order.PortfolioId = cpa.PortfolioId;
            order.Side = side;
            order.ContractIds = (new string[] { cpa.ClientOrderId }).ToList();
            order.OrderType = orderType;
            order.Quantity = quantity;
            order.State = state;
            order.UnitPrice = unitPrice;
            order.TimeInForce = timeInForce;
            order.DeliveryAreaId = cpa.DeliveryAreaId;
            order.ExecutionRestriction = executionRestriction;
            order.ExpireTime = expireTime;
            order.ClipSize = clipSize;
            order.ClipPriceChange = clipPriceChange;
            return order;
        }
    }

    public static class OrderModificationEx
    {
        public static OrderModification Init(this OrderModification req,
            string modifyOrderId,
            OrderEntry order,
            string order_text = "Modified order")
        {
            req.RevisionNo = 0L;
            req.ClientOrderId = order.ClientOrderId;
            req.OrderId = modifyOrderId;
            req.PortfolioId = order.PortfolioId;
            req.ContractIds = order.ContractIds;
            req.OrderType = order.OrderType;
            req.UnitPrice = order.UnitPrice;
            req.Quantity = order.Quantity;
            req.TimeInForce = order.TimeInForce;
            req.ExecutionRestriction = order.ExecutionRestriction;
            req.ExpireTime = order.ExpireTime;
            req.Text = order_text;
            req.ClipSize = order.ClipSize;
            req.ClipPriceChange = order.ClipPriceChange;
            return req;
        }
    }
}
