using CaoNC.Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;

namespace NordPoolC.Extensions
{
    public class MemoryCacheProxy
    {
        private readonly MemoryCache _memoryCache = new MemoryCache(new MemoryCacheOptions());

        /// <summary>
        /// 写入缓存
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="data">数据</param>
        public void SetCache<T>(List<T> data)
        {
            var targetDataTypeKey = typeof(T).ToString();

            if (!_memoryCache.TryGetValue<List<T>>(targetDataTypeKey, out var dataByType))
            {
                dataByType = new List<T>(0);
            }

            dataByType.AddRange(data);

            _ = _memoryCache.Set(targetDataTypeKey, dataByType);
        }

        /// <summary>
        /// 读取缓存
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <returns>数据</returns>
        public ICollection<T> GetFromCache<T>()
        {
            var targetDataTypeKey = typeof(T).ToString();

            _ = _memoryCache.TryGetValue<List<T>>(targetDataTypeKey, out var dataByType);

            return dataByType!=null&& dataByType.Count>0 ? dataByType.ToList(): new List<T>(0);
        }
    }
}
