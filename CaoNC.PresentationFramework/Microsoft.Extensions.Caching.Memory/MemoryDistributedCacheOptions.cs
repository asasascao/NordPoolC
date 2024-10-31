namespace CaoNC.Microsoft.Extensions.Caching.Memory
{
    public class MemoryDistributedCacheOptions : MemoryCacheOptions
    {
        /// <summary>
        /// Initializes a new instance of <see cref="T:Microsoft.Extensions.Caching.Memory.MemoryDistributedCacheOptions" />.
        /// </summary>
        public MemoryDistributedCacheOptions()
        {
            base.SizeLimit = 209715200L;
        }
    }
}
