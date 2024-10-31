using CaoNC.Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaoNC.Microsoft.Extensions.Caching.Memory
{
    public class MemoryCacheEntryOptions
    {
        private DateTimeOffset? _absoluteExpiration;

        private TimeSpan? _absoluteExpirationRelativeToNow;

        private TimeSpan? _slidingExpiration;

        private long? _size;

        /// <summary>
        /// Gets or sets an absolute expiration date for the cache entry.
        /// </summary>
        public DateTimeOffset? AbsoluteExpiration
        {
            get
            {
                return _absoluteExpiration;
            }
            set
            {
                _absoluteExpiration = value;
            }
        }

        /// <summary>
        /// Gets or sets an absolute expiration time, relative to now.
        /// </summary>
        public TimeSpan? AbsoluteExpirationRelativeToNow
        {
            get
            {
                return _absoluteExpirationRelativeToNow;
            }
            set
            {
                if (value <= TimeSpan.Zero)
                {
                    throw new ArgumentOutOfRangeException("AbsoluteExpirationRelativeToNow", value, "The relative expiration value must be positive.");
                }
                _absoluteExpirationRelativeToNow = value;
            }
        }

        /// <summary>
        /// Gets or sets how long a cache entry can be inactive (e.g. not accessed) before it will be removed.
        /// This will not extend the entry lifetime beyond the absolute expiration (if set).
        /// </summary>
        public TimeSpan? SlidingExpiration
        {
            get
            {
                return _slidingExpiration;
            }
            set
            {
                if (value <= TimeSpan.Zero)
                {
                    throw new ArgumentOutOfRangeException("SlidingExpiration", value, "The sliding expiration value must be positive.");
                }
                _slidingExpiration = value;
            }
        }

        /// <summary>
        /// Gets the <see cref="T:Microsoft.Extensions.Primitives.IChangeToken" /> instances which cause the cache entry to expire.
        /// </summary>
        public IList<IChangeToken> ExpirationTokens { get; } = new List<IChangeToken>();


        /// <summary>
        /// Gets or sets the callbacks will be fired after the cache entry is evicted from the cache.
        /// </summary>
        public IList<PostEvictionCallbackRegistration> PostEvictionCallbacks { get; } = new List<PostEvictionCallbackRegistration>();


        /// <summary>
        /// Gets or sets the priority for keeping the cache entry in the cache during a
        /// memory pressure triggered cleanup. The default is <see cref="F:Microsoft.Extensions.Caching.Memory.CacheItemPriority.Normal" />.
        /// </summary>
        public CacheItemPriority Priority { get; set; } = CacheItemPriority.Normal;


        /// <summary>
        /// Gets or sets the size of the cache entry value.
        /// </summary>
        public long? Size
        {
            get
            {
                return _size;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("value", value, "value must be non-negative.");
                }
                _size = value;
            }
        }
    }
}
