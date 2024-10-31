using CaoNC.System.Diagnostics.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaoNC.Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionServiceExtensions
    {
        /// <summary>
        /// Adds a transient service of the type specified in <paramref name="serviceType" /> with an
        /// implementation of the type specified in <paramref name="implementationType" /> to the
        /// specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the service to.</param>
        /// <param name="serviceType">The type of the service to register.</param>
        /// <param name="implementationType">The implementation type of the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient" />
        public static IServiceCollection AddTransient(this IServiceCollection services, Type serviceType, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type implementationType)
        {
            System.ThrowHelper.ThrowIfNull(services, "services");
            System.ThrowHelper.ThrowIfNull(serviceType, "serviceType");
            System.ThrowHelper.ThrowIfNull(implementationType, "implementationType");
            return Add(services, serviceType, implementationType, ServiceLifetime.Transient);
        }

        /// <summary>
        /// Adds a transient service of the type specified in <paramref name="serviceType" /> with a
        /// factory specified in <paramref name="implementationFactory" /> to the
        /// specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the service to.</param>
        /// <param name="serviceType">The type of the service to register.</param>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient" />
        public static IServiceCollection AddTransient(this IServiceCollection services, Type serviceType, Func<IServiceProvider, object> implementationFactory)
        {
            System.ThrowHelper.ThrowIfNull(services, "services");
            System.ThrowHelper.ThrowIfNull(serviceType, "serviceType");
            System.ThrowHelper.ThrowIfNull(implementationFactory, "implementationFactory");
            return Add(services, serviceType, implementationFactory, ServiceLifetime.Transient);
        }

        /// <summary>
        /// Adds a transient service of the type specified in <typeparamref name="TService" /> with an
        /// implementation type specified in <typeparamref name="TImplementation" /> to the
        /// specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the service to.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient" />
        public static IServiceCollection AddTransient<TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation>(this IServiceCollection services) where TService : class where TImplementation : class, TService
        {
            System.ThrowHelper.ThrowIfNull(services, "services");
            return services.AddTransient(typeof(TService), typeof(TImplementation));
        }

        /// <summary>
        /// Adds a transient service of the type specified in <paramref name="serviceType" /> to the
        /// specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the service to.</param>
        /// <param name="serviceType">The type of the service to register and the implementation to use.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient" />
        public static IServiceCollection AddTransient(this IServiceCollection services, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type serviceType)
        {
            System.ThrowHelper.ThrowIfNull(services, "services");
            System.ThrowHelper.ThrowIfNull(serviceType, "serviceType");
            return services.AddTransient(serviceType, serviceType);
        }

        /// <summary>
        /// Adds a transient service of the type specified in <typeparamref name="TService" /> to the
        /// specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the service to.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient" />
        public static IServiceCollection AddTransient<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TService>(this IServiceCollection services) where TService : class
        {
            System.ThrowHelper.ThrowIfNull(services, "services");
            return services.AddTransient(typeof(TService));
        }

        /// <summary>
        /// Adds a transient service of the type specified in <typeparamref name="TService" /> with a
        /// factory specified in <paramref name="implementationFactory" /> to the
        /// specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the service to.</param>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient" />
        public static IServiceCollection AddTransient<TService>(this IServiceCollection services, Func<IServiceProvider, TService> implementationFactory) where TService : class
        {
            System.ThrowHelper.ThrowIfNull(services, "services");
            System.ThrowHelper.ThrowIfNull(implementationFactory, "implementationFactory");
            return services.AddTransient(typeof(TService), implementationFactory);
        }

        /// <summary>
        /// Adds a transient service of the type specified in <typeparamref name="TService" /> with an
        /// implementation type specified in <typeparamref name="TImplementation" /> using the
        /// factory specified in <paramref name="implementationFactory" /> to the
        /// specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the service to.</param>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient" />
        public static IServiceCollection AddTransient<TService, TImplementation>(this IServiceCollection services, Func<IServiceProvider, TImplementation> implementationFactory) where TService : class where TImplementation : class, TService
        {
            System.ThrowHelper.ThrowIfNull(services, "services");
            System.ThrowHelper.ThrowIfNull(implementationFactory, "implementationFactory");
            return services.AddTransient(typeof(TService), implementationFactory);
        }

        /// <summary>
        /// Adds a scoped service of the type specified in <paramref name="serviceType" /> with an
        /// implementation of the type specified in <paramref name="implementationType" /> to the
        /// specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the service to.</param>
        /// <param name="serviceType">The type of the service to register.</param>
        /// <param name="implementationType">The implementation type of the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped" />
        public static IServiceCollection AddScoped(this IServiceCollection services, Type serviceType, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type implementationType)
        {
            System.ThrowHelper.ThrowIfNull(services, "services");
            System.ThrowHelper.ThrowIfNull(serviceType, "serviceType");
            System.ThrowHelper.ThrowIfNull(implementationType, "implementationType");
            return Add(services, serviceType, implementationType, ServiceLifetime.Scoped);
        }

        /// <summary>
        /// Adds a scoped service of the type specified in <paramref name="serviceType" /> with a
        /// factory specified in <paramref name="implementationFactory" /> to the
        /// specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the service to.</param>
        /// <param name="serviceType">The type of the service to register.</param>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped" />
        public static IServiceCollection AddScoped(this IServiceCollection services, Type serviceType, Func<IServiceProvider, object> implementationFactory)
        {
            System.ThrowHelper.ThrowIfNull(services, "services");
            System.ThrowHelper.ThrowIfNull(serviceType, "serviceType");
            System.ThrowHelper.ThrowIfNull(implementationFactory, "implementationFactory");
            return Add(services, serviceType, implementationFactory, ServiceLifetime.Scoped);
        }

        /// <summary>
        /// Adds a scoped service of the type specified in <typeparamref name="TService" /> with an
        /// implementation type specified in <typeparamref name="TImplementation" /> to the
        /// specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the service to.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped" />
        public static IServiceCollection AddScoped<TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation>(this IServiceCollection services) where TService : class where TImplementation : class, TService
        {
            System.ThrowHelper.ThrowIfNull(services, "services");
            return services.AddScoped(typeof(TService), typeof(TImplementation));
        }

        /// <summary>
        /// Adds a scoped service of the type specified in <paramref name="serviceType" /> to the
        /// specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the service to.</param>
        /// <param name="serviceType">The type of the service to register and the implementation to use.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped" />
        public static IServiceCollection AddScoped(this IServiceCollection services, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type serviceType)
        {
            System.ThrowHelper.ThrowIfNull(services, "services");
            System.ThrowHelper.ThrowIfNull(serviceType, "serviceType");
            return services.AddScoped(serviceType, serviceType);
        }

        /// <summary>
        /// Adds a scoped service of the type specified in <typeparamref name="TService" /> to the
        /// specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the service to.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped" />
        public static IServiceCollection AddScoped<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TService>(this IServiceCollection services) where TService : class
        {
            System.ThrowHelper.ThrowIfNull(services, "services");
            return services.AddScoped(typeof(TService));
        }

        /// <summary>
        /// Adds a scoped service of the type specified in <typeparamref name="TService" /> with a
        /// factory specified in <paramref name="implementationFactory" /> to the
        /// specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the service to.</param>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped" />
        public static IServiceCollection AddScoped<TService>(this IServiceCollection services, Func<IServiceProvider, TService> implementationFactory) where TService : class
        {
            System.ThrowHelper.ThrowIfNull(services, "services");
            System.ThrowHelper.ThrowIfNull(implementationFactory, "implementationFactory");
            return services.AddScoped(typeof(TService), implementationFactory);
        }

        /// <summary>
        /// Adds a scoped service of the type specified in <typeparamref name="TService" /> with an
        /// implementation type specified in <typeparamref name="TImplementation" /> using the
        /// factory specified in <paramref name="implementationFactory" /> to the
        /// specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the service to.</param>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped" />
        public static IServiceCollection AddScoped<TService, TImplementation>(this IServiceCollection services, Func<IServiceProvider, TImplementation> implementationFactory) where TService : class where TImplementation : class, TService
        {
            System.ThrowHelper.ThrowIfNull(services, "services");
            System.ThrowHelper.ThrowIfNull(implementationFactory, "implementationFactory");
            return services.AddScoped(typeof(TService), implementationFactory);
        }

        /// <summary>
        /// Adds a singleton service of the type specified in <paramref name="serviceType" /> with an
        /// implementation of the type specified in <paramref name="implementationType" /> to the
        /// specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the service to.</param>
        /// <param name="serviceType">The type of the service to register.</param>
        /// <param name="implementationType">The implementation type of the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton" />
        public static IServiceCollection AddSingleton(this IServiceCollection services, Type serviceType, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type implementationType)
        {
            System.ThrowHelper.ThrowIfNull(services, "services");
            System.ThrowHelper.ThrowIfNull(serviceType, "serviceType");
            System.ThrowHelper.ThrowIfNull(implementationType, "implementationType");
            return Add(services, serviceType, implementationType, ServiceLifetime.Singleton);
        }

        /// <summary>
        /// Adds a singleton service of the type specified in <paramref name="serviceType" /> with a
        /// factory specified in <paramref name="implementationFactory" /> to the
        /// specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the service to.</param>
        /// <param name="serviceType">The type of the service to register.</param>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton" />
        public static IServiceCollection AddSingleton(this IServiceCollection services, Type serviceType, Func<IServiceProvider, object> implementationFactory)
        {
            System.ThrowHelper.ThrowIfNull(services, "services");
            System.ThrowHelper.ThrowIfNull(serviceType, "serviceType");
            System.ThrowHelper.ThrowIfNull(implementationFactory, "implementationFactory");
            return Add(services, serviceType, implementationFactory, ServiceLifetime.Singleton);
        }

        /// <summary>
        /// Adds a singleton service of the type specified in <typeparamref name="TService" /> with an
        /// implementation type specified in <typeparamref name="TImplementation" /> to the
        /// specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the service to.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton" />
        public static IServiceCollection AddSingleton<TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation>(this IServiceCollection services) where TService : class where TImplementation : class, TService
        {
            System.ThrowHelper.ThrowIfNull(services, "services");
            return services.AddSingleton(typeof(TService), typeof(TImplementation));
        }

        /// <summary>
        /// Adds a singleton service of the type specified in <paramref name="serviceType" /> to the
        /// specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the service to.</param>
        /// <param name="serviceType">The type of the service to register and the implementation to use.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton" />
        public static IServiceCollection AddSingleton(this IServiceCollection services, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type serviceType)
        {
            System.ThrowHelper.ThrowIfNull(services, "services");
            System.ThrowHelper.ThrowIfNull(serviceType, "serviceType");
            return services.AddSingleton(serviceType, serviceType);
        }

        /// <summary>
        /// Adds a singleton service of the type specified in <typeparamref name="TService" /> to the
        /// specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the service to.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton" />
        public static IServiceCollection AddSingleton<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TService>(this IServiceCollection services) where TService : class
        {
            System.ThrowHelper.ThrowIfNull(services, "services");
            return services.AddSingleton(typeof(TService));
        }

        /// <summary>
        /// Adds a singleton service of the type specified in <typeparamref name="TService" /> with a
        /// factory specified in <paramref name="implementationFactory" /> to the
        /// specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the service to.</param>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton" />
        public static IServiceCollection AddSingleton<TService>(this IServiceCollection services, Func<IServiceProvider, TService> implementationFactory) where TService : class
        {
            System.ThrowHelper.ThrowIfNull(services, "services");
            System.ThrowHelper.ThrowIfNull(implementationFactory, "implementationFactory");
            return services.AddSingleton(typeof(TService), implementationFactory);
        }

        /// <summary>
        /// Adds a singleton service of the type specified in <typeparamref name="TService" /> with an
        /// implementation type specified in <typeparamref name="TImplementation" /> using the
        /// factory specified in <paramref name="implementationFactory" /> to the
        /// specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the service to.</param>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton" />
        public static IServiceCollection AddSingleton<TService, TImplementation>(this IServiceCollection services, Func<IServiceProvider, TImplementation> implementationFactory) where TService : class where TImplementation : class, TService
        {
            System.ThrowHelper.ThrowIfNull(services, "services");
            System.ThrowHelper.ThrowIfNull(implementationFactory, "implementationFactory");
            return services.AddSingleton(typeof(TService), implementationFactory);
        }

        /// <summary>
        /// Adds a singleton service of the type specified in <paramref name="serviceType" /> with an
        /// instance specified in <paramref name="implementationInstance" /> to the
        /// specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the service to.</param>
        /// <param name="serviceType">The type of the service to register.</param>
        /// <param name="implementationInstance">The instance of the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton" />
        public static IServiceCollection AddSingleton(this IServiceCollection services, Type serviceType, object implementationInstance)
        {
            System.ThrowHelper.ThrowIfNull(services, "services");
            System.ThrowHelper.ThrowIfNull(serviceType, "serviceType");
            System.ThrowHelper.ThrowIfNull(implementationInstance, "implementationInstance");
            ServiceDescriptor item = new ServiceDescriptor(serviceType, implementationInstance);
            services.Add(item);
            return services;
        }

        /// <summary>
        /// Adds a singleton service of the type specified in <typeparamref name="TService" /> with an
        /// instance specified in <paramref name="implementationInstance" /> to the
        /// specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the service to.</param>
        /// <param name="implementationInstance">The instance of the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton" />
        public static IServiceCollection AddSingleton<TService>(this IServiceCollection services, TService implementationInstance) where TService : class
        {
            System.ThrowHelper.ThrowIfNull(services, "services");
            System.ThrowHelper.ThrowIfNull(implementationInstance, "implementationInstance");
            return services.AddSingleton(typeof(TService), implementationInstance);
        }

        private static IServiceCollection Add(IServiceCollection collection, Type serviceType, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type implementationType, ServiceLifetime lifetime)
        {
            ServiceDescriptor item = new ServiceDescriptor(serviceType, implementationType, lifetime);
            collection.Add(item);
            return collection;
        }

        private static IServiceCollection Add(IServiceCollection collection, Type serviceType, Func<IServiceProvider, object> implementationFactory, ServiceLifetime lifetime)
        {
            ServiceDescriptor item = new ServiceDescriptor(serviceType, implementationFactory, lifetime);
            collection.Add(item);
            return collection;
        }

        /// <summary>
        /// Adds a transient service of the type specified in <paramref name="serviceType" /> with an
        /// implementation of the type specified in <paramref name="implementationType" /> to the
        /// specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the service to.</param>
        /// <param name="serviceType">The type of the service to register.</param>
        /// <param name="serviceKey">The <see cref="P:Microsoft.Extensions.DependencyInjection.ServiceDescriptor.ServiceKey" /> of the service.</param>
        /// <param name="implementationType">The implementation type of the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient" />
        public static IServiceCollection AddKeyedTransient(this IServiceCollection services, Type serviceType, object serviceKey, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type implementationType)
        {
            System.ThrowHelper.ThrowIfNull(services, "services");
            System.ThrowHelper.ThrowIfNull(serviceType, "serviceType");
            System.ThrowHelper.ThrowIfNull(implementationType, "implementationType");
            return AddKeyed(services, serviceType, serviceKey, implementationType, ServiceLifetime.Transient);
        }

        /// <summary>
        /// Adds a transient service of the type specified in <paramref name="serviceType" /> with a
        /// factory specified in <paramref name="implementationFactory" /> to the
        /// specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the service to.</param>
        /// <param name="serviceType">The type of the service to register.</param>
        /// <param name="serviceKey">The <see cref="P:Microsoft.Extensions.DependencyInjection.ServiceDescriptor.ServiceKey" /> of the service.</param>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient" />
        public static IServiceCollection AddKeyedTransient(this IServiceCollection services, Type serviceType, object serviceKey, Func<IServiceProvider, object, object> implementationFactory)
        {
            System.ThrowHelper.ThrowIfNull(services, "services");
            System.ThrowHelper.ThrowIfNull(serviceType, "serviceType");
            System.ThrowHelper.ThrowIfNull(implementationFactory, "implementationFactory");
            return AddKeyed(services, serviceType, serviceKey, implementationFactory, ServiceLifetime.Transient);
        }

        /// <summary>
        /// Adds a transient service of the type specified in <typeparamref name="TService" /> with an
        /// implementation type specified in <typeparamref name="TImplementation" /> to the
        /// specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the service to.</param>
        /// <param name="serviceKey">The <see cref="P:Microsoft.Extensions.DependencyInjection.ServiceDescriptor.ServiceKey" /> of the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient" />
        public static IServiceCollection AddKeyedTransient<TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation>(this IServiceCollection services, object serviceKey) where TService : class where TImplementation : class, TService
        {
            System.ThrowHelper.ThrowIfNull(services, "services");
            return services.AddKeyedTransient(typeof(TService), serviceKey, typeof(TImplementation));
        }

        /// <summary>
        /// Adds a transient service of the type specified in <paramref name="serviceType" /> to the
        /// specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the service to.</param>
        /// <param name="serviceType">The type of the service to register and the implementation to use.</param>
        /// <param name="serviceKey">The <see cref="P:Microsoft.Extensions.DependencyInjection.ServiceDescriptor.ServiceKey" /> of the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient" />
        public static IServiceCollection AddKeyedTransient(this IServiceCollection services, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type serviceType, object serviceKey)
        {
            System.ThrowHelper.ThrowIfNull(services, "services");
            System.ThrowHelper.ThrowIfNull(serviceType, "serviceType");
            return services.AddKeyedTransient(serviceType, serviceKey, serviceType);
        }

        /// <summary>
        /// Adds a transient service of the type specified in <typeparamref name="TService" /> to the
        /// specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the service to.</param>
        /// <param name="serviceKey">The <see cref="P:Microsoft.Extensions.DependencyInjection.ServiceDescriptor.ServiceKey" /> of the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient" />
        public static IServiceCollection AddKeyedTransient<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TService>(this IServiceCollection services, object serviceKey) where TService : class
        {
            System.ThrowHelper.ThrowIfNull(services, "services");
            return services.AddKeyedTransient(typeof(TService), serviceKey);
        }

        /// <summary>
        /// Adds a transient service of the type specified in <typeparamref name="TService" /> with a
        /// factory specified in <paramref name="implementationFactory" /> to the
        /// specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the service to.</param>
        /// <param name="serviceKey">The <see cref="P:Microsoft.Extensions.DependencyInjection.ServiceDescriptor.ServiceKey" /> of the service.</param>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient" />
        public static IServiceCollection AddKeyedTransient<TService>(this IServiceCollection services, object serviceKey, Func<IServiceProvider, object, TService> implementationFactory) where TService : class
        {
            System.ThrowHelper.ThrowIfNull(services, "services");
            System.ThrowHelper.ThrowIfNull(implementationFactory, "implementationFactory");
            return services.AddKeyedTransient(typeof(TService), serviceKey, implementationFactory);
        }

        /// <summary>
        /// Adds a transient service of the type specified in <typeparamref name="TService" /> with an
        /// implementation type specified in <typeparamref name="TImplementation" /> using the
        /// factory specified in <paramref name="implementationFactory" /> to the
        /// specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the service to.</param>
        /// <param name="serviceKey">The <see cref="P:Microsoft.Extensions.DependencyInjection.ServiceDescriptor.ServiceKey" /> of the service.</param>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient" />
        public static IServiceCollection AddKeyedTransient<TService, TImplementation>(this IServiceCollection services, object serviceKey, Func<IServiceProvider, object, TImplementation> implementationFactory) where TService : class where TImplementation : class, TService
        {
            System.ThrowHelper.ThrowIfNull(services, "services");
            System.ThrowHelper.ThrowIfNull(implementationFactory, "implementationFactory");
            return services.AddKeyedTransient(typeof(TService), serviceKey, implementationFactory);
        }

        /// <summary>
        /// Adds a scoped service of the type specified in <paramref name="serviceType" /> with an
        /// implementation of the type specified in <paramref name="implementationType" /> to the
        /// specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the service to.</param>
        /// <param name="serviceType">The type of the service to register.</param>
        /// <param name="serviceKey">The <see cref="P:Microsoft.Extensions.DependencyInjection.ServiceDescriptor.ServiceKey" /> of the service.</param>
        /// <param name="implementationType">The implementation type of the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped" />
        public static IServiceCollection AddKeyedScoped(this IServiceCollection services, Type serviceType, object serviceKey, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type implementationType)
        {
            System.ThrowHelper.ThrowIfNull(services, "services");
            System.ThrowHelper.ThrowIfNull(serviceType, "serviceType");
            System.ThrowHelper.ThrowIfNull(implementationType, "implementationType");
            return AddKeyed(services, serviceType, serviceKey, implementationType, ServiceLifetime.Scoped);
        }

        /// <summary>
        /// Adds a scoped service of the type specified in <paramref name="serviceType" /> with a
        /// factory specified in <paramref name="implementationFactory" /> to the
        /// specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the service to.</param>
        /// <param name="serviceType">The type of the service to register.</param>
        /// <param name="serviceKey">The <see cref="P:Microsoft.Extensions.DependencyInjection.ServiceDescriptor.ServiceKey" /> of the service.</param>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped" />
        public static IServiceCollection AddKeyedScoped(this IServiceCollection services, Type serviceType, object serviceKey, Func<IServiceProvider, object, object> implementationFactory)
        {
            System.ThrowHelper.ThrowIfNull(services, "services");
            System.ThrowHelper.ThrowIfNull(serviceType, "serviceType");
            System.ThrowHelper.ThrowIfNull(implementationFactory, "implementationFactory");
            return AddKeyed(services, serviceType, serviceKey, implementationFactory, ServiceLifetime.Scoped);
        }

        /// <summary>
        /// Adds a scoped service of the type specified in <typeparamref name="TService" /> with an
        /// implementation type specified in <typeparamref name="TImplementation" /> to the
        /// specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the service to.</param>
        /// <param name="serviceKey">The <see cref="P:Microsoft.Extensions.DependencyInjection.ServiceDescriptor.ServiceKey" /> of the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped" />
        public static IServiceCollection AddKeyedScoped<TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation>(this IServiceCollection services, object serviceKey) where TService : class where TImplementation : class, TService
        {
            System.ThrowHelper.ThrowIfNull(services, "services");
            return services.AddKeyedScoped(typeof(TService), serviceKey, typeof(TImplementation));
        }

        /// <summary>
        /// Adds a scoped service of the type specified in <paramref name="serviceType" /> to the
        /// specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the service to.</param>
        /// <param name="serviceType">The type of the service to register and the implementation to use.</param>
        /// <param name="serviceKey">The <see cref="P:Microsoft.Extensions.DependencyInjection.ServiceDescriptor.ServiceKey" /> of the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped" />
        public static IServiceCollection AddKeyedScoped(this IServiceCollection services, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type serviceType, object serviceKey)
        {
            System.ThrowHelper.ThrowIfNull(services, "services");
            System.ThrowHelper.ThrowIfNull(serviceType, "serviceType");
            return services.AddKeyedScoped(serviceType, serviceKey, serviceType);
        }

        /// <summary>
        /// Adds a scoped service of the type specified in <typeparamref name="TService" /> to the
        /// specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the service to.</param>
        /// <param name="serviceKey">The <see cref="P:Microsoft.Extensions.DependencyInjection.ServiceDescriptor.ServiceKey" /> of the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped" />
        public static IServiceCollection AddKeyedScoped<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TService>(this IServiceCollection services, object serviceKey) where TService : class
        {
            System.ThrowHelper.ThrowIfNull(services, "services");
            return services.AddKeyedScoped(typeof(TService), serviceKey);
        }

        /// <summary>
        /// Adds a scoped service of the type specified in <typeparamref name="TService" /> with a
        /// factory specified in <paramref name="implementationFactory" /> to the
        /// specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the service to.</param>
        /// <param name="serviceKey">The <see cref="P:Microsoft.Extensions.DependencyInjection.ServiceDescriptor.ServiceKey" /> of the service.</param>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped" />
        public static IServiceCollection AddKeyedScoped<TService>(this IServiceCollection services, object serviceKey, Func<IServiceProvider, object, TService> implementationFactory) where TService : class
        {
            System.ThrowHelper.ThrowIfNull(services, "services");
            System.ThrowHelper.ThrowIfNull(implementationFactory, "implementationFactory");
            return services.AddKeyedScoped(typeof(TService), serviceKey, implementationFactory);
        }

        /// <summary>
        /// Adds a scoped service of the type specified in <typeparamref name="TService" /> with an
        /// implementation type specified in <typeparamref name="TImplementation" /> using the
        /// factory specified in <paramref name="implementationFactory" /> to the
        /// specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the service to.</param>
        /// <param name="serviceKey">The <see cref="P:Microsoft.Extensions.DependencyInjection.ServiceDescriptor.ServiceKey" /> of the service.</param>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped" />
        public static IServiceCollection AddKeyedScoped<TService, TImplementation>(this IServiceCollection services, object serviceKey, Func<IServiceProvider, object, TImplementation> implementationFactory) where TService : class where TImplementation : class, TService
        {
            System.ThrowHelper.ThrowIfNull(services, "services");
            System.ThrowHelper.ThrowIfNull(implementationFactory, "implementationFactory");
            return services.AddKeyedScoped(typeof(TService), serviceKey, implementationFactory);
        }

        /// <summary>
        /// Adds a singleton service of the type specified in <paramref name="serviceType" /> with an
        /// implementation of the type specified in <paramref name="implementationType" /> to the
        /// specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the service to.</param>
        /// <param name="serviceType">The type of the service to register.</param>
        /// <param name="serviceKey">The <see cref="P:Microsoft.Extensions.DependencyInjection.ServiceDescriptor.ServiceKey" /> of the service.</param>
        /// <param name="implementationType">The implementation type of the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton" />
        public static IServiceCollection AddKeyedSingleton(this IServiceCollection services, Type serviceType, object serviceKey, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type implementationType)
        {
            System.ThrowHelper.ThrowIfNull(services, "services");
            System.ThrowHelper.ThrowIfNull(serviceType, "serviceType");
            System.ThrowHelper.ThrowIfNull(implementationType, "implementationType");
            return AddKeyed(services, serviceType, serviceKey, implementationType, ServiceLifetime.Singleton);
        }

        /// <summary>
        /// Adds a singleton service of the type specified in <paramref name="serviceType" /> with a
        /// factory specified in <paramref name="implementationFactory" /> to the
        /// specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the service to.</param>
        /// <param name="serviceType">The type of the service to register.</param>
        /// <param name="serviceKey">The <see cref="P:Microsoft.Extensions.DependencyInjection.ServiceDescriptor.ServiceKey" /> of the service.</param>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton" />
        public static IServiceCollection AddKeyedSingleton(this IServiceCollection services, Type serviceType, object serviceKey, Func<IServiceProvider, object, object> implementationFactory)
        {
            System.ThrowHelper.ThrowIfNull(services, "services");
            System.ThrowHelper.ThrowIfNull(serviceType, "serviceType");
            System.ThrowHelper.ThrowIfNull(implementationFactory, "implementationFactory");
            return AddKeyed(services, serviceType, serviceKey, implementationFactory, ServiceLifetime.Singleton);
        }

        /// <summary>
        /// Adds a singleton service of the type specified in <typeparamref name="TService" /> with an
        /// implementation type specified in <typeparamref name="TImplementation" /> to the
        /// specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the service to.</param>
        /// <param name="serviceKey">The <see cref="P:Microsoft.Extensions.DependencyInjection.ServiceDescriptor.ServiceKey" /> of the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton" />
        public static IServiceCollection AddKeyedSingleton<TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation>(this IServiceCollection services, object serviceKey) where TService : class where TImplementation : class, TService
        {
            System.ThrowHelper.ThrowIfNull(services, "services");
            return services.AddKeyedSingleton(typeof(TService), serviceKey, typeof(TImplementation));
        }

        /// <summary>
        /// Adds a singleton service of the type specified in <paramref name="serviceType" /> to the
        /// specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the service to.</param>
        /// <param name="serviceType">The type of the service to register and the implementation to use.</param>
        /// <param name="serviceKey">The <see cref="P:Microsoft.Extensions.DependencyInjection.ServiceDescriptor.ServiceKey" /> of the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton" />
        public static IServiceCollection AddKeyedSingleton(this IServiceCollection services, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type serviceType, object serviceKey)
        {
            System.ThrowHelper.ThrowIfNull(services, "services");
            System.ThrowHelper.ThrowIfNull(serviceType, "serviceType");
            return services.AddKeyedSingleton(serviceType, serviceKey, serviceType);
        }

        /// <summary>
        /// Adds a singleton service of the type specified in <typeparamref name="TService" /> to the
        /// specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the service to.</param>
        /// <param name="serviceKey">The <see cref="P:Microsoft.Extensions.DependencyInjection.ServiceDescriptor.ServiceKey" /> of the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton" />
        public static IServiceCollection AddKeyedSingleton<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TService>(this IServiceCollection services, object serviceKey) where TService : class
        {
            System.ThrowHelper.ThrowIfNull(services, "services");
            return services.AddKeyedSingleton(typeof(TService), serviceKey, typeof(TService));
        }

        /// <summary>
        /// Adds a singleton service of the type specified in <typeparamref name="TService" /> with a
        /// factory specified in <paramref name="implementationFactory" /> to the
        /// specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the service to.</param>
        /// <param name="serviceKey">The <see cref="P:Microsoft.Extensions.DependencyInjection.ServiceDescriptor.ServiceKey" /> of the service.</param>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton" />
        public static IServiceCollection AddKeyedSingleton<TService>(this IServiceCollection services, object serviceKey, Func<IServiceProvider, object, TService> implementationFactory) where TService : class
        {
            System.ThrowHelper.ThrowIfNull(services, "services");
            System.ThrowHelper.ThrowIfNull(implementationFactory, "implementationFactory");
            return services.AddKeyedSingleton(typeof(TService), serviceKey, implementationFactory);
        }

        /// <summary>
        /// Adds a singleton service of the type specified in <typeparamref name="TService" /> with an
        /// implementation type specified in <typeparamref name="TImplementation" /> using the
        /// factory specified in <paramref name="implementationFactory" /> to the
        /// specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the service to.</param>
        /// <param name="serviceKey">The <see cref="P:Microsoft.Extensions.DependencyInjection.ServiceDescriptor.ServiceKey" /> of the service.</param>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton" />
        public static IServiceCollection AddKeyedSingleton<TService, TImplementation>(this IServiceCollection services, object serviceKey, Func<IServiceProvider, object, TImplementation> implementationFactory) where TService : class where TImplementation : class, TService
        {
            System.ThrowHelper.ThrowIfNull(services, "services");
            System.ThrowHelper.ThrowIfNull(implementationFactory, "implementationFactory");
            return services.AddKeyedSingleton(typeof(TService), serviceKey, implementationFactory);
        }

        /// <summary>
        /// Adds a singleton service of the type specified in <paramref name="serviceType" /> with an
        /// instance specified in <paramref name="implementationInstance" /> to the
        /// specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the service to.</param>
        /// <param name="serviceType">The type of the service to register.</param>
        /// <param name="serviceKey">The <see cref="P:Microsoft.Extensions.DependencyInjection.ServiceDescriptor.ServiceKey" /> of the service.</param>
        /// <param name="implementationInstance">The instance of the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton" />
        public static IServiceCollection AddKeyedSingleton(this IServiceCollection services, Type serviceType, object serviceKey, object implementationInstance)
        {
            System.ThrowHelper.ThrowIfNull(services, "services");
            System.ThrowHelper.ThrowIfNull(serviceType, "serviceType");
            System.ThrowHelper.ThrowIfNull(implementationInstance, "implementationInstance");
            ServiceDescriptor item = new ServiceDescriptor(serviceType, serviceKey, implementationInstance);
            services.Add(item);
            return services;
        }

        /// <summary>
        /// Adds a singleton service of the type specified in <typeparamref name="TService" /> with an
        /// instance specified in <paramref name="implementationInstance" /> to the
        /// specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the service to.</param>
        /// <param name="serviceKey">The <see cref="P:Microsoft.Extensions.DependencyInjection.ServiceDescriptor.ServiceKey" /> of the service.</param>
        /// <param name="implementationInstance">The instance of the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton" />
        public static IServiceCollection AddKeyedSingleton<TService>(this IServiceCollection services, object serviceKey, TService implementationInstance) where TService : class
        {
            System.ThrowHelper.ThrowIfNull(services, "services");
            System.ThrowHelper.ThrowIfNull(implementationInstance, "implementationInstance");
            return services.AddKeyedSingleton(typeof(TService), serviceKey, implementationInstance);
        }

        private static IServiceCollection AddKeyed(IServiceCollection collection, Type serviceType, object serviceKey, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type implementationType, ServiceLifetime lifetime)
        {
            ServiceDescriptor item = new ServiceDescriptor(serviceType, serviceKey, implementationType, lifetime);
            collection.Add(item);
            return collection;
        }

        private static IServiceCollection AddKeyed(IServiceCollection collection, Type serviceType, object serviceKey, Func<IServiceProvider, object, object> implementationFactory, ServiceLifetime lifetime)
        {
            ServiceDescriptor item = new ServiceDescriptor(serviceType, serviceKey, implementationFactory, lifetime);
            collection.Add(item);
            return collection;
        }
    }

}
