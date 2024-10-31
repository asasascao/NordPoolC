using CaoNC.Microsoft.Extensions.Caching.Distributed;
using CaoNC.Microsoft.Extensions.DependencyInjection;
using System;

namespace CaoNC.Microsoft.Extensions.Caching.Memory
{
    public static class MemoryCacheServiceCollectionExtensions
    {
        /// <summary>
        /// Adds a non distributed in memory implementation of <see cref="T:Microsoft.Extensions.Caching.Memory.IMemoryCache" /> to the
        /// <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add services to.</param>
        /// <returns>The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> so that additional calls can be chained.</returns>
        public static IServiceCollection AddMemoryCache(this IServiceCollection services)
        {
            System.ThrowHelper.ThrowIfNull(services, "services");
            services.AddOptions();
            services.TryAdd(ServiceDescriptor.Singleton<IMemoryCache, MemoryCache>());
            return services;
        }

        /// <summary>
        /// Adds a non distributed in memory implementation of <see cref="T:Microsoft.Extensions.Caching.Memory.IMemoryCache" /> to the
        /// <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add services to.</param>
        /// <param name="setupAction">
        /// The <see cref="T:System.Action`1" /> to configure the provided <see cref="T:Microsoft.Extensions.Caching.Memory.MemoryCacheOptions" />.
        /// </param>
        /// <returns>The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> so that additional calls can be chained.</returns>
        public static IServiceCollection AddMemoryCache(this IServiceCollection services, Action<MemoryCacheOptions> setupAction)
        {
            System.ThrowHelper.ThrowIfNull(services, "services");
            System.ThrowHelper.ThrowIfNull(setupAction, "setupAction");
            services.AddMemoryCache();
            services.Configure<MemoryCacheOptions>(setupAction);
            return services;
        }

        /// <summary>
        /// Adds a default implementation of <see cref="T:Microsoft.Extensions.Caching.Distributed.IDistributedCache" /> that stores items in memory
        /// to the <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />. Frameworks that require a distributed cache to work
        /// can safely add this dependency as part of their dependency list to ensure that there is at least
        /// one implementation available.
        /// </summary>
        /// <remarks>
        /// <see cref="M:Microsoft.Extensions.DependencyInjection.MemoryCacheServiceCollectionExtensions.AddDistributedMemoryCache(Microsoft.Extensions.DependencyInjection.IServiceCollection)" /> should only be used in single
        /// server scenarios as this cache stores items in memory and doesn't expand across multiple machines.
        /// For those scenarios it is recommended to use a proper distributed cache that can expand across
        /// multiple machines.
        /// </remarks>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add services to.</param>
        /// <returns>The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> so that additional calls can be chained.</returns>
        public static IServiceCollection AddDistributedMemoryCache(this IServiceCollection services)
        {
            System.ThrowHelper.ThrowIfNull(services, "services");
            OptionsServiceCollectionExtensions.AddOptions(services);
            ServiceCollectionDescriptorExtensions.TryAdd(services, ServiceDescriptor.Singleton<IDistributedCache, MemoryDistributedCache>());
            return services;
        }

        /// <summary>
        /// Adds a default implementation of <see cref="T:Microsoft.Extensions.Caching.Distributed.IDistributedCache" /> that stores items in memory
        /// to the <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />. Frameworks that require a distributed cache to work
        /// can safely add this dependency as part of their dependency list to ensure that there is at least
        /// one implementation available.
        /// </summary>
        /// <remarks>
        /// <see cref="M:Microsoft.Extensions.DependencyInjection.MemoryCacheServiceCollectionExtensions.AddDistributedMemoryCache(Microsoft.Extensions.DependencyInjection.IServiceCollection)" /> should only be used in single
        /// server scenarios as this cache stores items in memory and doesn't expand across multiple machines.
        /// For those scenarios it is recommended to use a proper distributed cache that can expand across
        /// multiple machines.
        /// </remarks>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add services to.</param>
        /// <param name="setupAction">
        /// The <see cref="T:System.Action`1" /> to configure the provided <see cref="T:Microsoft.Extensions.Caching.Memory.MemoryDistributedCacheOptions" />.
        /// </param>
        /// <returns>The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> so that additional calls can be chained.</returns>
        public static IServiceCollection AddDistributedMemoryCache(this IServiceCollection services, Action<MemoryDistributedCacheOptions> setupAction)
        {
            System.ThrowHelper.ThrowIfNull(services, "services");
            System.ThrowHelper.ThrowIfNull(setupAction, "setupAction");
            services.AddDistributedMemoryCache();
            OptionsServiceCollectionExtensions.Configure<MemoryDistributedCacheOptions>(services, setupAction);
            return services;
        }
    }
}
