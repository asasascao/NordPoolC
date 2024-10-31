using CaoNC.Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaoNC.Microsoft.Extensions.Caching.Memory
{
    public static class CacheExtensions
    {
        /// <summary>
        /// Gets the value associated with this key if present.
        /// </summary>
        /// <param name="cache">The <see cref="T:Microsoft.Extensions.Caching.Memory.IMemoryCache" /> instance this method extends.</param>
        /// <param name="key">The key of the value to get.</param>
        /// <returns>The value associated with this key, or <c>null</c> if the key is not present.</returns>
        public static object Get(this IMemoryCache cache, object key)
        {
            cache.TryGetValue(key, out var value);
            return value;
        }

        /// <summary>
        /// Gets the value associated with this key if present.
        /// </summary>
        /// <typeparam name="TItem">The type of the object to get.</typeparam>
        /// <param name="cache">The <see cref="T:Microsoft.Extensions.Caching.Memory.IMemoryCache" /> instance this method extends.</param>
        /// <param name="key">The key of the value to get.</param>
        /// <returns>The value associated with this key, or <c>default(TItem)</c> if the key is not present.</returns>
        public static TItem Get<TItem>(this IMemoryCache cache, object key)
        {
            return (TItem)(cache.Get(key) ?? ((object)default(TItem)));
        }

        /// <summary>
        /// Try to get the value associated with the given key.
        /// </summary>
        /// <typeparam name="TItem">The type of the object to get.</typeparam>
        /// <param name="cache">The <see cref="T:Microsoft.Extensions.Caching.Memory.IMemoryCache" /> instance this method extends.</param>
        /// <param name="key">The key of the value to get.</param>
        /// <param name="value">The value associated with the given key.</param>
        /// <returns><c>true</c> if the key was found. <c>false</c> otherwise.</returns>
        public static bool TryGetValue<TItem>(this IMemoryCache cache, object key, out TItem value)
        {
            if (cache.TryGetValue(key, out var value2))
            {
                if (value2 == null)
                {
                    value = default(TItem);
                    return true;
                }
                if (value2 is TItem val)
                {
                    value = val;
                    return true;
                }
            }
            value = default(TItem);
            return false;
        }

        /// <summary>
        /// Associate a value with a key in the <see cref="T:Microsoft.Extensions.Caching.Memory.IMemoryCache" />.
        /// </summary>
        /// <typeparam name="TItem">The type of the object to set.</typeparam>
        /// <param name="cache">The <see cref="T:Microsoft.Extensions.Caching.Memory.IMemoryCache" /> instance this method extends.</param>
        /// <param name="key">The key of the entry to add.</param>
        /// <param name="value">The value to associate with the key.</param>
        /// <returns>The value that was set.</returns>
        public static TItem Set<TItem>(this IMemoryCache cache, object key, TItem value)
        {
            using (ICacheEntry cacheEntry = cache.CreateEntry(key))
            {
                cacheEntry.Value = value;
            }
            return value;
        }

        /// <summary>
        /// Sets a cache entry with the given key and value that will expire in the given duration.
        /// </summary>
        /// <typeparam name="TItem">The type of the object to set.</typeparam>
        /// <param name="cache">The <see cref="T:Microsoft.Extensions.Caching.Memory.IMemoryCache" /> instance this method extends.</param>
        /// <param name="key">The key of the entry to add.</param>
        /// <param name="value">The value to associate with the key.</param>
        /// <param name="absoluteExpiration">The point in time at which the cache entry will expire.</param>
        /// <returns>The value that was set.</returns>
        public static TItem Set<TItem>(this IMemoryCache cache, object key, TItem value, DateTimeOffset absoluteExpiration)
        {
            using (ICacheEntry cacheEntry = cache.CreateEntry(key))
            {
                cacheEntry.AbsoluteExpiration = absoluteExpiration;
                cacheEntry.Value = value;
            }
            return value;
        }

        /// <summary>
        /// Sets a cache entry with the given key and value that will expire in the given duration from now.
        /// </summary>
        /// <typeparam name="TItem">The type of the object to set.</typeparam>
        /// <param name="cache">The <see cref="T:Microsoft.Extensions.Caching.Memory.IMemoryCache" /> instance this method extends.</param>
        /// <param name="key">The key of the entry to add.</param>
        /// <param name="value">The value to associate with the key.</param>
        /// <param name="absoluteExpirationRelativeToNow">The duration from now after which the cache entry will expire.</param>
        /// <returns>The value that was set.</returns>
        public static TItem Set<TItem>(this IMemoryCache cache, object key, TItem value, TimeSpan absoluteExpirationRelativeToNow)
        {
            using (ICacheEntry cacheEntry = cache.CreateEntry(key))
            {
                cacheEntry.AbsoluteExpirationRelativeToNow = absoluteExpirationRelativeToNow;
                cacheEntry.Value = value;
            }
            return value;
        }

        /// <summary>
        /// Sets a cache entry with the given key and value that will expire when <see cref="T:Microsoft.Extensions.Primitives.IChangeToken" /> expires.
        /// </summary>
        /// <typeparam name="TItem">The type of the object to set.</typeparam>
        /// <param name="cache">The <see cref="T:Microsoft.Extensions.Caching.Memory.IMemoryCache" /> instance this method extends.</param>
        /// <param name="key">The key of the entry to add.</param>
        /// <param name="value">The value to associate with the key.</param>
        /// <param name="expirationToken">The <see cref="T:Microsoft.Extensions.Primitives.IChangeToken" /> that causes the cache entry to expire.</param>
        /// <returns>The value that was set.</returns>
        public static TItem Set<TItem>(this IMemoryCache cache, object key, TItem value, IChangeToken expirationToken)
        {
            using (ICacheEntry cacheEntry = cache.CreateEntry(key))
            {
                cacheEntry.AddExpirationToken(expirationToken);
                cacheEntry.Value = value;
            }
            return value;
        }

        /// <summary>
        /// Sets a cache entry with the given key and value and apply the values of an existing <see cref="T:Microsoft.Extensions.Caching.Memory.MemoryCacheEntryOptions" /> to the created entry.
        /// </summary>
        /// <typeparam name="TItem">The type of the object to set.</typeparam>
        /// <param name="cache">The <see cref="T:Microsoft.Extensions.Caching.Memory.IMemoryCache" /> instance this method extends.</param>
        /// <param name="key">The key of the entry to add.</param>
        /// <param name="value">The value to associate with the key.</param>
        /// <param name="options">The existing <see cref="T:Microsoft.Extensions.Caching.Memory.MemoryCacheEntryOptions" /> instance to apply to the new entry.</param>
        /// <returns>The value that was set.</returns>
        public static TItem Set<TItem>(this IMemoryCache cache, object key, TItem value, MemoryCacheEntryOptions options)
        {
            using (ICacheEntry cacheEntry = cache.CreateEntry(key))
            {
                if (options != null)
                {
                    cacheEntry.SetOptions(options);
                }
                cacheEntry.Value = value;
            }
            return value;
        }

        /// <summary>
        /// Gets the value associated with this key if it exists, or generates a new entry using the provided key and a value from the given factory if the key is not found.
        /// </summary>
        /// <typeparam name="TItem">The type of the object to get.</typeparam>
        /// <param name="cache">The <see cref="T:Microsoft.Extensions.Caching.Memory.IMemoryCache" /> instance this method extends.</param>
        /// <param name="key">The key of the entry to look for or create.</param>
        /// <param name="factory">The factory that creates the value associated with this key if the key does not exist in the cache.</param>
        /// <returns>The value associated with this key.</returns>
        public static TItem GetOrCreate<TItem>(this IMemoryCache cache, object key, Func<ICacheEntry, TItem> factory)
        {
            if (!cache.TryGetValue(key, out var value))
            {
                using (ICacheEntry cacheEntry = cache.CreateEntry(key))
                {
                    value = (cacheEntry.Value = factory(cacheEntry));
                }
            }
            return (TItem)value;
        }

        /// <summary>
        /// Asynchronously gets the value associated with this key if it exists, or generates a new entry using the provided key and a value from the given factory if the key is not found.
        /// </summary>
        /// <typeparam name="TItem">The type of the object to get.</typeparam>
        /// <param name="cache">The <see cref="T:Microsoft.Extensions.Caching.Memory.IMemoryCache" /> instance this method extends.</param>
        /// <param name="key">The key of the entry to look for or create.</param>
        /// <param name="factory">The factory task that creates the value associated with this key if the key does not exist in the cache.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static async Task<TItem> GetOrCreateAsync<TItem>(this IMemoryCache cache, object key, Func<ICacheEntry, Task<TItem>> factory)
        {
            if (!cache.TryGetValue(key, out var value))
            {
                using (ICacheEntry entry = cache.CreateEntry(key))
                {
                    value = (entry.Value = await factory(entry).ConfigureAwait(continueOnCapturedContext: false));
                }
            }
            return (TItem)value;
        }
    }

}
