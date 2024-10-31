using Microsoft.IdentityModel.Tokens;
using Nordpool.ID.PublicApi.v1;
using Nordpool.ID.PublicApi.v1.Contract;
using Nordpool.ID.PublicApi.v1.Order.Request;
using NordPoolC.Connection;
using NordPoolC.Enums;
using NordPoolC.Extensions;
using NordPoolC.Logger;
using NordPoolC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NordPoolC.Message
{
    public class GlobalCacheProxyEx
    {
        static Random random = new Random(Math.Abs((int)BitConverter.ToUInt32(Guid.NewGuid().ToByteArray(), 0)));//随机数

        /// <summary>
        /// 获取最新订单请求
        /// </summary>
        /// <returns>最新订单请求</returns>
        public static OrderEntryRequest GetLastOrderEntryRequest()
            => GlobalCacheProxy.Instance.GetFromLastOrDefaultCache<OrderEntryRequest>();

        /// <summary>
        /// 获取最新订单回报
        /// </summary>
        /// <param name="clientOrderId">订单id</param>
        /// <returns>最新订单回报</returns>
        public static OrderExecutionReport GetOrderExecutionReport(string clientOrderId)
            => GlobalCacheProxy.Instance.GetFromLastOrDefaultCache<OrderExecutionReport>(oer => oer.Orders.Count == 1 && oer.Orders.Single().ClientOrderId == clientOrderId);

        /// <summary>
        /// 获取随机合约id 文件单id area id
        /// </summary>
        /// <param name="clientTarget">连接类型-无 交易 市场数据</param>
        /// <returns>随机合约id 文件单id area id</returns>
        public static ContractPortfolioAndArea GetRandomContractPortfolioAndArea(ConnectServiceType clientTarget= ConnectServiceType.NONE)
        {
            var contracts = GlobalCacheProxy.Instance.GetFromCache<ContractRow>(c =>
            c.ProductType != ProductType.CUSTOM_BLOCK && c.DlvryAreaState.Any(s => s.State == ContractState.ACTI));
            if (contracts.IsNullOrEmpty())
            {
                LogFactory.Instance.Warning(string.Format("[{0}] No valid contract to be used for order creation has been found in cache!", clientTarget));
                return default;
            }

            var randomContract = contracts.ElementAtOrDefault(random.Next(0, contracts.Count()));
            var areas = randomContract != null ? randomContract.DlvryAreaState.Where(s => s.State == ContractState.ACTI) : null;

            var portfolios = GlobalCacheProxy.Instance.GetFromCache<ConfigurationRow>()
            .SelectMany(c => c.Portfolios).Where(p => p.Areas.Any(a => areas.Any(s => s.DlvryAreaId == a.AreaId)));

            var randomPortfolioForContract = portfolios.ElementAtOrDefault(random.Next(0, portfolios.Count()));
            if (randomPortfolioForContract is null)
            {
                LogFactory.Instance.Warning(string.Format("[{0}] No valid portfolio to be used for order creation has been found in cache!.", clientTarget));
                return default;
            }

            var deliveryAreaPortfolio = randomPortfolioForContract.Areas.First(a => areas.Any(s => s.DlvryAreaId == a.AreaId));

            return new ContractPortfolioAndArea() { ClientOrderId = randomContract.ContractId, PortfolioId = randomPortfolioForContract.Id, DeliveryAreaId = deliveryAreaPortfolio.AreaId };
        }
    }

    public class GlobalCacheProxy
    {
        static Lazy<GlobalCacheProxy> globalCache = new Lazy<GlobalCacheProxy>();
        public static GlobalCacheProxy Instance => globalCache.Value;

        MemoryCacheProxy _memoryCacheProxy = new MemoryCacheProxy();

        public void SetCache<T>(T values)
            => SetCache(new T[] { values });

        public void SetCache<T>(List<T> values)
            => _memoryCacheProxy.SetCache(values);

        public void SetCache<T>(IEnumerable<T> values)
            => _memoryCacheProxy.SetCache(values.ToList());

        public ICollection<T> GetFromCache<T>()
            => _memoryCacheProxy.GetFromCache<T>();

        public IEnumerable<T> GetFromCache<T>(Func<T, bool> predicate)
            => _memoryCacheProxy.GetFromCache<T>().Where(predicate);

        public T GetFromLastOrDefaultCache<T>()
            => _memoryCacheProxy.GetFromCache<T>().LastOrDefault();
        
        public T GetFromLastOrDefaultCache<T>(Func<T, bool> predicate)
            => _memoryCacheProxy.GetFromCache<T>().LastOrDefault(predicate);

        public T GetFromFirstOrDefaultCache<T>()
            => _memoryCacheProxy.GetFromCache<T>().FirstOrDefault();
    }
}
