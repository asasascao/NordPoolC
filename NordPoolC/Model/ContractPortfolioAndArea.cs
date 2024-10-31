using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NordPoolC.Model
{
    public class ContractPortfolioAndArea
    {
        /// <summary>Id for the order, provided by the client to track their own orders</summary>
		public string ClientOrderId { get; set; }

        /// <summary>The portfolio id of the current order</summary>
        public string PortfolioId { get; set; }

        public long DeliveryAreaId { get; set; }
    }
}
