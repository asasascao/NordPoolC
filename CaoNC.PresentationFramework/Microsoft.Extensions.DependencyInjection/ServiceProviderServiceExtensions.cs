using CaoNC.System.Diagnostics.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaoNC.Microsoft.Extensions.DependencyInjection
{
    public static class ServiceProviderServiceExtensions
    {
        /// <summary>
        /// Get service of type <typeparamref name="T" /> from the <see cref="T:System.IServiceProvider" />.
        /// </summary>
        /// <typeparam name="T">The type of service object to get.</typeparam>
        /// <param name="provider">The <see cref="T:System.IServiceProvider" /> to retrieve the service object from.</param>
        /// <returns>A service object of type <typeparamref name="T" /> or null if there is no such service.</returns>
        public static T GetService<T>(this IServiceProvider provider)
        {
            System.ThrowHelper.ThrowIfNull(provider, "provider");
            return (T)provider.GetService(typeof(T));
        }

        /// <summary>
        /// Get service of type <paramref name="serviceType" /> from the <see cref="T:System.IServiceProvider" />.
        /// </summary>
        /// <param name="provider">The <see cref="T:System.IServiceProvider" /> to retrieve the service object from.</param>
        /// <param name="serviceType">An object that specifies the type of service object to get.</param>
        /// <returns>A service object of type <paramref name="serviceType" />.</returns>
        /// <exception cref="T:System.InvalidOperationException">There is no service of type <paramref name="serviceType" />.</exception>
        public static object GetRequiredService(this IServiceProvider provider, Type serviceType)
        {
            System.ThrowHelper.ThrowIfNull(provider, "provider");
            System.ThrowHelper.ThrowIfNull(serviceType, "serviceType");
            if (provider is ISupportRequiredService supportRequiredService)
            {
                return supportRequiredService.GetRequiredService(serviceType);
            }
            object service = provider.GetService(serviceType);
            if (service == null)
            {
                throw new InvalidOperationException(System.SR.Format(System.SR.NoServiceRegistered, serviceType));
            }
            return service;
        }

        /// <summary>
        /// Get service of type <typeparamref name="T" /> from the <see cref="T:System.IServiceProvider" />.
        /// </summary>
        /// <typeparam name="T">The type of service object to get.</typeparam>
        /// <param name="provider">The <see cref="T:System.IServiceProvider" /> to retrieve the service object from.</param>
        /// <returns>A service object of type <typeparamref name="T" />.</returns>
        /// <exception cref="T:System.InvalidOperationException">There is no service of type <typeparamref name="T" />.</exception>
        public static T GetRequiredService<T>(this IServiceProvider provider)
        {
            System.ThrowHelper.ThrowIfNull(provider, "provider");
            return (T)provider.GetRequiredService(typeof(T));
        }

        /// <summary>
        /// Get an enumeration of services of type <typeparamref name="T" /> from the <see cref="T:System.IServiceProvider" />.
        /// </summary>
        /// <typeparam name="T">The type of service object to get.</typeparam>
        /// <param name="provider">The <see cref="T:System.IServiceProvider" /> to retrieve the services from.</param>
        /// <returns>An enumeration of services of type <typeparamref name="T" />.</returns>
        public static IEnumerable<T> GetServices<T>(this IServiceProvider provider)
        {
            System.ThrowHelper.ThrowIfNull(provider, "provider");
            return provider.GetRequiredService<IEnumerable<T>>();
        }

        /// <summary>
        /// Get an enumeration of services of type <paramref name="serviceType" /> from the <see cref="T:System.IServiceProvider" />.
        /// </summary>
        /// <param name="provider">The <see cref="T:System.IServiceProvider" /> to retrieve the services from.</param>
        /// <param name="serviceType">An object that specifies the type of service object to get.</param>
        /// <returns>An enumeration of services of type <paramref name="serviceType" />.</returns>
        [RequiresDynamicCode("The native code for an IEnumerable<serviceType> might not be available at runtime.")]
        public static IEnumerable<object> GetServices(this IServiceProvider provider, Type serviceType)
        {
            System.ThrowHelper.ThrowIfNull(provider, "provider");
            System.ThrowHelper.ThrowIfNull(serviceType, "serviceType");
            Type serviceType2 = typeof(IEnumerable<>).MakeGenericType(serviceType);
            return (IEnumerable<object>)provider.GetRequiredService(serviceType2);
        }

        /// <summary>
        /// Creates a new <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceScope" /> that can be used to resolve scoped services.
        /// </summary>
        /// <param name="provider">The <see cref="T:System.IServiceProvider" /> to create the scope from.</param>
        /// <returns>A <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceScope" /> that can be used to resolve scoped services.</returns>
        public static IServiceScope CreateScope(this IServiceProvider provider)
        {
            return provider.GetRequiredService<IServiceScopeFactory>().CreateScope();
        }

        /// <summary>
        /// Creates a new <see cref="T:Microsoft.Extensions.DependencyInjection.AsyncServiceScope" /> that can be used to resolve scoped services.
        /// </summary>
        /// <param name="provider">The <see cref="T:System.IServiceProvider" /> to create the scope from.</param>
        /// <returns>An <see cref="T:Microsoft.Extensions.DependencyInjection.AsyncServiceScope" /> that can be used to resolve scoped services.</returns>
        public static AsyncServiceScope CreateAsyncScope(this IServiceProvider provider)
        {
            return new AsyncServiceScope(provider.CreateScope());
        }

        /// <summary>
        /// Creates a new <see cref="T:Microsoft.Extensions.DependencyInjection.AsyncServiceScope" /> that can be used to resolve scoped services.
        /// </summary>
        /// <param name="serviceScopeFactory">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceScopeFactory" /> to create the scope from.</param>
        /// <returns>An <see cref="T:Microsoft.Extensions.DependencyInjection.AsyncServiceScope" /> that can be used to resolve scoped services.</returns>
        public static AsyncServiceScope CreateAsyncScope(this IServiceScopeFactory serviceScopeFactory)
        {
            return new AsyncServiceScope(serviceScopeFactory.CreateScope());
        }
    }
}
