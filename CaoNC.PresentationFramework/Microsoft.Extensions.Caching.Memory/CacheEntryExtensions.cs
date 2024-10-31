using CaoNC.Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaoNC.Microsoft.Extensions.Caching.Memory
{
    public static class CacheEntryExtensions
    {
        /// <summary>
        /// Sets the priority for keeping the cache entry in the cache during a memory pressure tokened cleanup.
        /// </summary>
        /// <param name="entry">The entry to set the priority for.</param>
        /// <param name="priority">The <see cref="T:Microsoft.Extensions.Caching.Memory.CacheItemPriority" /> to set on the entry.</param>
        /// <returns>The <see cref="T:Microsoft.Extensions.Caching.Memory.ICacheEntry" /> for chaining.</returns>
        public static ICacheEntry SetPriority(this ICacheEntry entry, CacheItemPriority priority)
        {
            entry.Priority = priority;
            return entry;
        }

        /// <summary>
        /// Expire the cache entry if the given <see cref="T:Microsoft.Extensions.Primitives.IChangeToken" /> expires.
        /// </summary>
        /// <param name="entry">The <see cref="T:Microsoft.Extensions.Caching.Memory.ICacheEntry" />.</param>
        /// <param name="expirationToken">The <see cref="T:Microsoft.Extensions.Primitives.IChangeToken" /> that causes the cache entry to expire.</param>
        /// <returns>The <see cref="T:Microsoft.Extensions.Caching.Memory.ICacheEntry" /> for chaining.</returns>
        public static ICacheEntry AddExpirationToken(this ICacheEntry entry, IChangeToken expirationToken)
        {
            System.ThrowHelper.ThrowIfNull(expirationToken, "expirationToken");
            entry.ExpirationTokens.Add(expirationToken);
            return entry;
        }

        /// <summary>
        /// Sets an absolute expiration time, relative to now.
        /// </summary>
        /// <param name="entry">The <see cref="T:Microsoft.Extensions.Caching.Memory.ICacheEntry" />.</param>
        /// <param name="relative">The <see cref="T:System.TimeSpan" /> representing the expiration time relative to now.</param>
        /// <returns>The <see cref="T:Microsoft.Extensions.Caching.Memory.ICacheEntry" /> for chaining.</returns>
        public static ICacheEntry SetAbsoluteExpiration(this ICacheEntry entry, TimeSpan relative)
        {
            entry.AbsoluteExpirationRelativeToNow = relative;
            return entry;
        }

        /// <summary>
        /// Sets an absolute expiration date for the cache entry.
        /// </summary>
        /// <param name="entry">The <see cref="T:Microsoft.Extensions.Caching.Memory.ICacheEntry" />.</param>
        /// <param name="absolute">A <see cref="T:System.DateTimeOffset" /> representing the expiration time in absolute terms.</param>
        /// <returns>The <see cref="T:Microsoft.Extensions.Caching.Memory.ICacheEntry" /> for chaining.</returns>
        public static ICacheEntry SetAbsoluteExpiration(this ICacheEntry entry, DateTimeOffset absolute)
        {
            entry.AbsoluteExpiration = absolute;
            return entry;
        }

        /// <summary>
        /// Sets how long the cache entry can be inactive (e.g. not accessed) before it will be removed.
        /// This will not extend the entry lifetime beyond the absolute expiration (if set).
        /// </summary>
        /// <param name="entry">The <see cref="T:Microsoft.Extensions.Caching.Memory.ICacheEntry" />.</param>
        /// <param name="offset">A <see cref="T:System.TimeSpan" /> representing a sliding expiration.</param>
        /// <returns>The <see cref="T:Microsoft.Extensions.Caching.Memory.ICacheEntry" /> for chaining.</returns>
        public static ICacheEntry SetSlidingExpiration(this ICacheEntry entry, TimeSpan offset)
        {
            entry.SlidingExpiration = offset;
            return entry;
        }

        /// <summary>
        /// The given callback will be fired after the cache entry is evicted from the cache.
        /// </summary>
        /// <param name="entry">The <see cref="T:Microsoft.Extensions.Caching.Memory.ICacheEntry" />.</param>
        /// <param name="callback">The callback to run after the entry is evicted.</param>
        /// <returns>The <see cref="T:Microsoft.Extensions.Caching.Memory.ICacheEntry" /> for chaining.</returns>
        public static ICacheEntry RegisterPostEvictionCallback(this ICacheEntry entry, PostEvictionDelegate callback)
        {
            System.ThrowHelper.ThrowIfNull(callback, "callback");
            return entry.RegisterPostEvictionCallbackNoValidation(callback, null);
        }

        /// <summary>
        /// The given callback will be fired after the cache entry is evicted from the cache.
        /// </summary>
        /// <param name="entry">The <see cref="T:Microsoft.Extensions.Caching.Memory.ICacheEntry" />.</param>
        /// <param name="callback">The callback to run after the entry is evicted.</param>
        /// <param name="state">The state to pass to the post-eviction callback.</param>
        /// <returns>The <see cref="T:Microsoft.Extensions.Caching.Memory.ICacheEntry" /> for chaining.</returns>
        public static ICacheEntry RegisterPostEvictionCallback(this ICacheEntry entry, PostEvictionDelegate callback, object state)
        {
            System.ThrowHelper.ThrowIfNull(callback, "callback");
            return entry.RegisterPostEvictionCallbackNoValidation(callback, state);
        }

        private static ICacheEntry RegisterPostEvictionCallbackNoValidation(this ICacheEntry entry, PostEvictionDelegate callback, object state)
        {
            entry.PostEvictionCallbacks.Add(new PostEvictionCallbackRegistration
            {
                EvictionCallback = callback,
                State = state
            });
            return entry;
        }

        /// <summary>
        /// Sets the value of the cache entry.
        /// </summary>
        /// <param name="entry">The <see cref="T:Microsoft.Extensions.Caching.Memory.ICacheEntry" />.</param>
        /// <param name="value">The value to set on the <paramref name="entry" />.</param>
        /// <returns>The <see cref="T:Microsoft.Extensions.Caching.Memory.ICacheEntry" /> for chaining.</returns>
        public static ICacheEntry SetValue(this ICacheEntry entry, object value)
        {
            entry.Value = value;
            return entry;
        }

        /// <summary>
        /// Sets the size of the cache entry value.
        /// </summary>
        /// <param name="entry">The <see cref="T:Microsoft.Extensions.Caching.Memory.ICacheEntry" />.</param>
        /// <param name="size">The size to set on the <paramref name="entry" />.</param>
        /// <returns>The <see cref="T:Microsoft.Extensions.Caching.Memory.ICacheEntry" /> for chaining.</returns>
        public static ICacheEntry SetSize(this ICacheEntry entry, long size)
        {
            if (size < 0)
            {
                throw new ArgumentOutOfRangeException("size", size, "size must be non-negative.");
            }
            entry.Size = size;
            return entry;
        }

        /// <summary>
        /// Applies the values of an existing <see cref="T:Microsoft.Extensions.Caching.Memory.MemoryCacheEntryOptions" /> to the entry.
        /// </summary>
        /// <param name="entry">The <see cref="T:Microsoft.Extensions.Caching.Memory.ICacheEntry" />.</param>
        /// <param name="options">Set the values of these options on the <paramref name="entry" />.</param>
        /// <returns>The <see cref="T:Microsoft.Extensions.Caching.Memory.ICacheEntry" /> for chaining.</returns>
        public static ICacheEntry SetOptions(this ICacheEntry entry, MemoryCacheEntryOptions options)
        {
            System.ThrowHelper.ThrowIfNull(options, "options");
            entry.AbsoluteExpiration = options.AbsoluteExpiration;
            entry.AbsoluteExpirationRelativeToNow = options.AbsoluteExpirationRelativeToNow;
            entry.SlidingExpiration = options.SlidingExpiration;
            entry.Priority = options.Priority;
            entry.Size = options.Size;
            foreach (IChangeToken expirationToken in options.ExpirationTokens)
            {
                entry.AddExpirationToken(expirationToken);
            }
            for (int i = 0; i < options.PostEvictionCallbacks.Count; i++)
            {
                PostEvictionCallbackRegistration postEvictionCallbackRegistration = options.PostEvictionCallbacks[i];
                if (postEvictionCallbackRegistration.EvictionCallback == null)
                {
                    ThrowNullCallback(i, "options");
                }
                entry.RegisterPostEvictionCallbackNoValidation(postEvictionCallbackRegistration.EvictionCallback, postEvictionCallbackRegistration.State);
            }
            return entry;
        }

        [System.Diagnostics.CodeAnalysis.DoesNotReturn]
        private static void ThrowNullCallback(int index, string paramName)
        {
            string message = $"MemoryCacheEntryOptions.PostEvictionCallbacks contains a PostEvictionCallbackRegistration with a null EvictionCallback at index {index}.";
            throw new ArgumentException(message, paramName);
        }
    }
}
