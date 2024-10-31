using System.Threading.Tasks;
using System.Threading;

namespace CaoNC.Microsoft.Extensions.Caching.Distributed
{
    public interface IDistributedCache
    {
        /// <summary>
        /// Gets a value with the given key.
        /// </summary>
        /// <param name="key">A string identifying the requested value.</param>
        /// <returns>The located value or null.</returns>
        byte[] Get(string key);

        /// <summary>
        /// Gets a value with the given key.
        /// </summary>
        /// <param name="key">A string identifying the requested value.</param>
        /// <param name="token">Optional. The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing the located value or null.</returns>
        Task<byte[]> GetAsync(string key, CancellationToken token = default(CancellationToken));

        /// <summary>
        /// Sets a value with the given key.
        /// </summary>
        /// <param name="key">A string identifying the requested value.</param>
        /// <param name="value">The value to set in the cache.</param>
        /// <param name="options">The cache options for the value.</param>
        void Set(string key, byte[] value, DistributedCacheEntryOptions options);

        /// <summary>
        /// Sets the value with the given key.
        /// </summary>
        /// <param name="key">A string identifying the requested value.</param>
        /// <param name="value">The value to set in the cache.</param>
        /// <param name="options">The cache options for the value.</param>
        /// <param name="token">Optional. The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation.</returns>
        Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options, CancellationToken token = default(CancellationToken));

        /// <summary>
        /// Refreshes a value in the cache based on its key, resetting its sliding expiration timeout (if any).
        /// </summary>
        /// <param name="key">A string identifying the requested value.</param>
        void Refresh(string key);

        /// <summary>
        /// Refreshes a value in the cache based on its key, resetting its sliding expiration timeout (if any).
        /// </summary>
        /// <param name="key">A string identifying the requested value.</param>
        /// <param name="token">Optional. The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation.</returns>
        Task RefreshAsync(string key, CancellationToken token = default(CancellationToken));

        /// <summary>
        /// Removes the value with the given key.
        /// </summary>
        /// <param name="key">A string identifying the requested value.</param>
        void Remove(string key);

        /// <summary>
        /// Removes the value with the given key.
        /// </summary>
        /// <param name="key">A string identifying the requested value.</param>
        /// <param name="token">Optional. The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation.</returns>
        Task RemoveAsync(string key, CancellationToken token = default(CancellationToken));
    }
}
