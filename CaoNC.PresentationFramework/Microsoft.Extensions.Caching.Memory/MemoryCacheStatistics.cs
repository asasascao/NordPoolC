namespace CaoNC.Microsoft.Extensions.Caching.Memory
{
    public class MemoryCacheStatistics
    {
        /// <summary>
        /// Gets the number of <see cref="T:Microsoft.Extensions.Caching.Memory.ICacheEntry" /> instances currently in the memory cache.
        /// </summary>
        public long CurrentEntryCount { get; set; } = 0;

        /// <summary>
        /// Gets an estimated sum of all the <see cref="P:Microsoft.Extensions.Caching.Memory.ICacheEntry.Size" /> values currently in the memory cache.
        /// </summary>
        /// <returns>Returns <see langword="null" /> if size isn't being tracked. The common MemoryCache implementation tracks size whenever a SizeLimit is set on the cache.</returns>
        public long? CurrentEstimatedSize { get; set; } = null;

        /// <summary>
        /// Gets the total number of cache misses.
        /// </summary>
        public long TotalMisses { get; set; } = 0;

        /// <summary>
        /// Gets the total number of cache hits.
        /// </summary>
        public long TotalHits { get; set; } = 0;

        /// <summary>
        /// Initializes an instance of MemoryCacheStatistics.
        /// </summary>
        public MemoryCacheStatistics()
        {
        }
    }

}
