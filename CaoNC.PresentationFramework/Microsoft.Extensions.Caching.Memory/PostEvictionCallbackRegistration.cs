namespace CaoNC.Microsoft.Extensions.Caching.Memory
{
    public class PostEvictionCallbackRegistration
    {
        /// <summary>
        /// Gets or sets the callback delegate that will be fired after an entry is evicted from the cache.
        /// </summary>
        public PostEvictionDelegate EvictionCallback { get; set; }

        /// <summary>
        /// Gets or sets the state to pass to the callback delegate.
        /// </summary>
        public object State { get; set; }
    }
}
