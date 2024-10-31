using CaoNC.Microsoft.Extensions.Internal;
using CaoNC.Microsoft.Extensions.Options;
using System;
using System.ComponentModel;

namespace CaoNC.Microsoft.Extensions.Caching.Memory
{
    public class MemoryCacheOptions : IOptions<MemoryCacheOptions>
    {
        private long _sizeLimit = -1L;

        private double _compactionPercentage = 0.05;

        private const int NotSet = -1;

        /// <summary>
        /// Gets or sets the clock used by the cache for expiration.
        /// </summary>
        public ISystemClock Clock { get; set; }

        /// <summary>
        /// Gets or sets the minimum length of time between successive scans for expired items.
        /// </summary>
        public TimeSpan ExpirationScanFrequency { get; set; } = TimeSpan.FromMinutes(1.0);


        internal bool HasSizeLimit => _sizeLimit >= 0;

        internal long SizeLimitValue => _sizeLimit;

        /// <summary>
        /// Gets or sets the maximum size of the cache.
        /// </summary>
        public long? SizeLimit
        {
            get
            {
                if (_sizeLimit >= 0)
                {
                    return _sizeLimit;
                }
                return null;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("value", value, "value must be non-negative.");
                }
                _sizeLimit = value ?? (-1);
            }
        }

        /// <summary>
        /// Enables ot disables the option to compact the cache when the maximum size is exceeded.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This property is retained only for compatibility.  Remove use and instead call MemoryCache.Compact as needed.", true)]
        public bool CompactOnMemoryPressure { get; set; }

        /// <summary>
        /// Gets or sets the amount to compact the cache by when the maximum size is exceeded.
        /// </summary>
        public double CompactionPercentage
        {
            get
            {
                return _compactionPercentage;
            }
            set
            {
                if ((value < 0.0 || value > 1.0) ? true : false)
                {
                    throw new ArgumentOutOfRangeException("value", value, "value must be between 0 and 1 inclusive.");
                }
                _compactionPercentage = value;
            }
        }

        /// <summary>
        /// Gets or sets whether to track linked entries. Disabled by default.
        /// </summary>
        /// <remarks>Prior to .NET 7 this feature was always enabled.</remarks>
        public bool TrackLinkedCacheEntries { get; set; }

        /// <summary>
        /// Gets or sets whether to track memory cache statistics. Disabled by default.
        /// </summary>
        public bool TrackStatistics { get; set; }

        public MemoryCacheOptions Value => this;
    }
}
