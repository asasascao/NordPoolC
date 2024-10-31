using CaoNC.Microsoft.Extensions.Caching.Distributed;
using CaoNC.Microsoft.Extensions.Logging;
using CaoNC.Microsoft.Extensions.Logging.Abstractions;
using CaoNC.Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CaoNC.Microsoft.Extensions.Caching.Memory
{
    public class MemoryDistributedCache : IDistributedCache
    {
        private readonly MemoryCache _memCache;

        /// <summary>
        /// Creates a new <see cref="T:Microsoft.Extensions.Caching.Distributed.MemoryDistributedCache" /> instance.
        /// </summary>
        /// <param name="optionsAccessor">The options of the cache.</param>
        public MemoryDistributedCache(IOptions<MemoryDistributedCacheOptions> optionsAccessor)
            : this(optionsAccessor, (ILoggerFactory)(object)NullLoggerFactory.Instance)
        {
        }

        /// <summary>
        /// Creates a new <see cref="T:Microsoft.Extensions.Caching.Distributed.MemoryDistributedCache" /> instance.
        /// </summary>
        /// <param name="optionsAccessor">The options of the cache.</param>
        /// <param name="loggerFactory">The logger factory to create <see cref="T:Microsoft.Extensions.Logging.ILogger" /> used to log messages.</param>
        public MemoryDistributedCache(IOptions<MemoryDistributedCacheOptions> optionsAccessor, ILoggerFactory loggerFactory)
        {
            System.ThrowHelper.ThrowIfNull(optionsAccessor, "optionsAccessor");
            System.ThrowHelper.ThrowIfNull(loggerFactory, "loggerFactory");
            _memCache = new MemoryCache((IOptions<MemoryCacheOptions>)(object)optionsAccessor.Value, loggerFactory);
        }

        /// <summary>
        /// Gets the specified item associated with a key from the <see cref="T:Microsoft.Extensions.Caching.Memory.IMemoryCache" /> as a byte array.
        /// </summary>
        /// <param name="key">The key of the item to get.</param>
        /// <returns>The byte array value of the key.</returns>
        public byte[] Get(string key)
        {
            System.ThrowHelper.ThrowIfNull(key, "key");
            return (byte[])CacheExtensions.Get((IMemoryCache)(object)_memCache, (object)key);
        }

        /// <summary>
        /// Asynchronously gets the specified item associated with a key from the <see cref="T:Microsoft.Extensions.Caching.Memory.IMemoryCache" /> as a byte array.
        /// </summary>
        /// <param name="key">The key of the item to get.</param>
        /// <param name="token">The <see cref="T:System.Threading.CancellationToken" /> to use to cancel operation.</param>
        /// <returns>The task for getting the byte array value associated with the specified key from the cache.</returns>
        public Task<byte[]> GetAsync(string key, CancellationToken token = default(CancellationToken))
        {
            System.ThrowHelper.ThrowIfNull(key, "key");
            return Task.FromResult(Get(key));
        }

        /// <summary>
        /// Sets the specified item associated with a key in the <see cref="T:Microsoft.Extensions.Caching.Memory.IMemoryCache" /> as a byte array.
        /// </summary>
        /// <param name="key">The key of the item to set.</param>
        /// <param name="value">The byte array value of the item to set.</param>
        /// <param name="options">The cache options for the item to set.</param>
        public void Set(string key, byte[] value, DistributedCacheEntryOptions options)
        {
            //IL_0021: Unknown result type (might be due to invalid IL or missing references)
            //IL_0027: Expected O, but got Unknown
            System.ThrowHelper.ThrowIfNull(key, "key");
            System.ThrowHelper.ThrowIfNull(value, "value");
            System.ThrowHelper.ThrowIfNull(options, "options");
            MemoryCacheEntryOptions val = new MemoryCacheEntryOptions();
            val.AbsoluteExpiration=options.AbsoluteExpiration;
            val.AbsoluteExpirationRelativeToNow=options.AbsoluteExpirationRelativeToNow;
            val.SlidingExpiration=options.SlidingExpiration;
            val.Size=(long?)value.Length;
            CacheExtensions.Set<byte[]>((IMemoryCache)(object)_memCache, (object)key, value, val);
        }

        /// <summary>
        /// Asynchronously sets the specified item associated with a key in the <see cref="T:Microsoft.Extensions.Caching.Memory.IMemoryCache" /> as a byte array.
        /// </summary>
        /// <param name="key">The key of the item to set.</param>
        /// <param name="value">The byte array value of the item to set.</param>
        /// <param name="options">The cache options for the item to set.</param>
        /// <param name="token">The <see cref="T:System.Threading.CancellationToken" /> to use to cancel operation.</param>
        /// <returns>The task for setting the byte array value associated with the specified key in the cache.</returns>
        public Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options, CancellationToken token = default(CancellationToken))
        {
            System.ThrowHelper.ThrowIfNull(key, "key");
            System.ThrowHelper.ThrowIfNull(value, "value");
            System.ThrowHelper.ThrowIfNull(options, "options");
            Set(key, value, options);
            return CreateCompletedTask();
        }

        /// <summary>
        /// Refreshes the specified item associated with a key from the <see cref="T:Microsoft.Extensions.Caching.Memory.IMemoryCache" />.
        /// </summary>
        /// <param name="key">The key of the item to refresh.</param>
        public void Refresh(string key)
        {
            System.ThrowHelper.ThrowIfNull(key, "key");
            _memCache.TryGetValue(key, out var _);
        }

        /// <summary>
        /// Asynchronously refreshes the specified item associated with a key from the <see cref="T:Microsoft.Extensions.Caching.Memory.IMemoryCache" />.
        /// </summary>
        /// <param name="key">The key of the item to refresh.</param>
        /// <param name="token">The <see cref="T:System.Threading.CancellationToken" /> to use to cancel operation.</param>
        /// <returns>The task for refreshing the specified key in the cache.</returns>
        public Task RefreshAsync(string key, CancellationToken token = default(CancellationToken))
        {
            System.ThrowHelper.ThrowIfNull(key, "key");
            Refresh(key);
            return CreateCompletedTask();
        }

        /// <summary>
        /// Removes the specified item associated with a key from the <see cref="T:Microsoft.Extensions.Caching.Memory.IMemoryCache" />.
        /// </summary>
        /// <param name="key">The key of the item to remove.</param>
        public void Remove(string key)
        {
            System.ThrowHelper.ThrowIfNull(key, "key");
            _memCache.Remove(key);
        }

        /// <summary>
        /// Asynchronously removes the specified item associated with a key from the <see cref="T:Microsoft.Extensions.Caching.Memory.IMemoryCache" />.
        /// </summary>
        /// <param name="key">The key of the item to remove.</param>
        /// <param name="token">The <see cref="T:System.Threading.CancellationToken" /> to use to cancel operation.</param>
        /// <returns>The task for removing the specified key from the cache.</returns>
        public Task RemoveAsync(string key, CancellationToken token = default(CancellationToken))
        {
            System.ThrowHelper.ThrowIfNull(key, "key");
            Remove(key);
            return CreateCompletedTask();
        }

        private static Task CreateCompletedTask()
        {
            // 使用 TaskCompletionSource 来创建一个已经完成的任务  
            var tcs = new TaskCompletionSource<bool>();
            tcs.SetResult(true); // 设置结果为 true（对于不返回结果的任务，这个值不重要）  
            return tcs.Task;
        }
    }
}
