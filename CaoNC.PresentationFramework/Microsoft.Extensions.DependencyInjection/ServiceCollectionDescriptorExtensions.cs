using CaoNC.System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using System;

namespace CaoNC.Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionDescriptorExtensions
    {
        /// <summary>
        /// Adds the specified <paramref name="descriptor" /> to the <paramref name="collection" />.
        /// </summary>
        /// <param name="collection">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.</param>
        /// <param name="descriptor">The <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" /> to add.</param>
        /// <returns>A reference to the current instance of <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.</returns>
        public static IServiceCollection Add(this IServiceCollection collection, ServiceDescriptor descriptor)
        {
            System.ThrowHelper.ThrowIfNull(collection, "collection");
            System.ThrowHelper.ThrowIfNull(descriptor, "descriptor");
            collection.Add(descriptor);
            return collection;
        }

        /// <summary>
        /// Adds a sequence of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" /> to the <paramref name="collection" />.
        /// </summary>
        /// <param name="collection">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.</param>
        /// <param name="descriptors">The <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" />s to add.</param>
        /// <returns>A reference to the current instance of <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.</returns>
        public static IServiceCollection Add(this IServiceCollection collection, IEnumerable<ServiceDescriptor> descriptors)
        {
            System.ThrowHelper.ThrowIfNull(collection, "collection");
            System.ThrowHelper.ThrowIfNull(descriptors, "descriptors");
            foreach (ServiceDescriptor descriptor in descriptors)
            {
                collection.Add(descriptor);
            }
            return collection;
        }

        /// <summary>
        /// Adds the specified <paramref name="descriptor" /> to the <paramref name="collection" /> if the
        /// service type hasn't already been registered.
        /// </summary>
        /// <param name="collection">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.</param>
        /// <param name="descriptor">The <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" /> to add.</param>
        public static void TryAdd(this IServiceCollection collection, ServiceDescriptor descriptor)
        {
            System.ThrowHelper.ThrowIfNull(collection, "collection");
            System.ThrowHelper.ThrowIfNull(descriptor, "descriptor");
            int count = collection.Count;
            for (int i = 0; i < count; i++)
            {
                if (collection[i].ServiceType == descriptor.ServiceType && object.Equals(collection[i].ServiceKey, descriptor.ServiceKey))
                {
                    return;
                }
            }
            collection.Add(descriptor);
        }

        /// <summary>
        /// Adds the specified <paramref name="descriptors" /> to the <paramref name="collection" /> if the
        /// service type hasn't already been registered.
        /// </summary>
        /// <param name="collection">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.</param>
        /// <param name="descriptors">The <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" />s to add.</param>
        public static void TryAdd(this IServiceCollection collection, IEnumerable<ServiceDescriptor> descriptors)
        {
            System.ThrowHelper.ThrowIfNull(collection, "collection");
            System.ThrowHelper.ThrowIfNull(descriptors, "descriptors");
            foreach (ServiceDescriptor descriptor in descriptors)
            {
                TryAdd(collection, descriptor);
            }
        }

        /// <summary>
        /// Adds the specified <paramref name="service" /> as a <see cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient" /> service
        /// to the <paramref name="collection" /> if the service type hasn't already been registered.
        /// </summary>
        /// <param name="collection">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.</param>
        /// <param name="service">The type of the service to register.</param>
        public static void TryAddTransient(this IServiceCollection collection, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type service)
        {
            System.ThrowHelper.ThrowIfNull(collection, "collection");
            System.ThrowHelper.ThrowIfNull(service, "service");
            ServiceDescriptor descriptor = ServiceDescriptor.Transient(service, service);
            TryAdd(collection, descriptor);
        }

        /// <summary>
        /// Adds the specified <paramref name="service" /> as a <see cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient" /> service
        /// with the <paramref name="implementationType" /> implementation
        /// to the <paramref name="collection" /> if the service type hasn't already been registered.
        /// </summary>
        /// <param name="collection">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.</param>
        /// <param name="service">The type of the service to register.</param>
        /// <param name="implementationType">The implementation type of the service.</param>
        public static void TryAddTransient(this IServiceCollection collection, Type service, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type implementationType)
        {
            System.ThrowHelper.ThrowIfNull(collection, "collection");
            System.ThrowHelper.ThrowIfNull(service, "service");
            System.ThrowHelper.ThrowIfNull(implementationType, "implementationType");
            ServiceDescriptor descriptor = ServiceDescriptor.Transient(service, implementationType);
            TryAdd(collection, descriptor);
        }

        /// <summary>
        /// Adds the specified <paramref name="service" /> as a <see cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient" /> service
        /// using the factory specified in <paramref name="implementationFactory" />
        /// to the <paramref name="collection" /> if the service type hasn't already been registered.
        /// </summary>
        /// <param name="collection">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.</param>
        /// <param name="service">The type of the service to register.</param>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        public static void TryAddTransient(this IServiceCollection collection, Type service, Func<IServiceProvider, object> implementationFactory)
        {
            System.ThrowHelper.ThrowIfNull(collection, "collection");
            System.ThrowHelper.ThrowIfNull(service, "service");
            System.ThrowHelper.ThrowIfNull(implementationFactory, "implementationFactory");
            ServiceDescriptor descriptor = ServiceDescriptor.Transient(service, implementationFactory);
            TryAdd(collection, descriptor);
        }

        /// <summary>
        /// Adds the specified <typeparamref name="TService" /> as a <see cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient" /> service
        /// to the <paramref name="collection" /> if the service type hasn't already been registered.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <param name="collection">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.</param>
        public static void TryAddTransient<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TService>(this IServiceCollection collection) where TService : class
        {
            System.ThrowHelper.ThrowIfNull(collection, "collection");
            TryAddTransient(collection, typeof(TService), typeof(TService));
        }

        /// <summary>
        /// Adds the specified <typeparamref name="TService" /> as a <see cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient" /> service
        /// implementation type specified in <typeparamref name="TImplementation" />
        /// to the <paramref name="collection" /> if the service type hasn't already been registered.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <param name="collection">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.</param>
        public static void TryAddTransient<TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation>(this IServiceCollection collection) where TService : class where TImplementation : class, TService
        {
            System.ThrowHelper.ThrowIfNull(collection, "collection");
            TryAddTransient(collection, typeof(TService), typeof(TImplementation));
        }

        /// <summary>
        /// Adds the specified <typeparamref name="TService" /> as a <see cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient" /> service
        /// using the factory specified in <paramref name="implementationFactory" />
        /// to the <paramref name="services" /> if the service type hasn't already been registered.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.</param>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        public static void TryAddTransient<TService>(this IServiceCollection services, Func<IServiceProvider, TService> implementationFactory) where TService : class
        {
            TryAdd(services, ServiceDescriptor.Transient(implementationFactory));
        }

        /// <summary>
        /// Adds the specified <paramref name="service" /> as a <see cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped" /> service
        /// to the <paramref name="collection" /> if the service type hasn't already been registered.
        /// </summary>
        /// <param name="collection">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.</param>
        /// <param name="service">The type of the service to register.</param>
        public static void TryAddScoped(this IServiceCollection collection, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type service)
        {
            System.ThrowHelper.ThrowIfNull(collection, "collection");
            System.ThrowHelper.ThrowIfNull(service, "service");
            ServiceDescriptor descriptor = ServiceDescriptor.Scoped(service, service);
            TryAdd(collection, descriptor);
        }

        /// <summary>
        /// Adds the specified <paramref name="service" /> as a <see cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped" /> service
        /// with the <paramref name="implementationType" /> implementation
        /// to the <paramref name="collection" /> if the service type hasn't already been registered.
        /// </summary>
        /// <param name="collection">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.</param>
        /// <param name="service">The type of the service to register.</param>
        /// <param name="implementationType">The implementation type of the service.</param>
        public static void TryAddScoped(this IServiceCollection collection, Type service, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type implementationType)
        {
            System.ThrowHelper.ThrowIfNull(collection, "collection");
            System.ThrowHelper.ThrowIfNull(service, "service");
            System.ThrowHelper.ThrowIfNull(implementationType, "implementationType");
            ServiceDescriptor descriptor = ServiceDescriptor.Scoped(service, implementationType);
            TryAdd(collection, descriptor);
        }

        /// <summary>
        /// Adds the specified <paramref name="service" /> as a <see cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped" /> service
        /// using the factory specified in <paramref name="implementationFactory" />
        /// to the <paramref name="collection" /> if the service type hasn't already been registered.
        /// </summary>
        /// <param name="collection">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.</param>
        /// <param name="service">The type of the service to register.</param>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        public static void TryAddScoped(this IServiceCollection collection, Type service, Func<IServiceProvider, object> implementationFactory)
        {
            System.ThrowHelper.ThrowIfNull(collection, "collection");
            System.ThrowHelper.ThrowIfNull(service, "service");
            System.ThrowHelper.ThrowIfNull(implementationFactory, "implementationFactory");
            ServiceDescriptor descriptor = ServiceDescriptor.Scoped(service, implementationFactory);
            TryAdd(collection, descriptor);
        }

        /// <summary>
        /// Adds the specified <typeparamref name="TService" /> as a <see cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped" /> service
        /// to the <paramref name="collection" /> if the service type hasn't already been registered.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <param name="collection">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.</param>
        public static void TryAddScoped<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TService>(this IServiceCollection collection) where TService : class
        {
            System.ThrowHelper.ThrowIfNull(collection, "collection");
            TryAddScoped(collection, typeof(TService), typeof(TService));
        }

        /// <summary>
        /// Adds the specified <typeparamref name="TService" /> as a <see cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped" /> service
        /// implementation type specified in <typeparamref name="TImplementation" />
        /// to the <paramref name="collection" /> if the service type hasn't already been registered.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <param name="collection">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.</param>
        public static void TryAddScoped<TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation>(this IServiceCollection collection) where TService : class where TImplementation : class, TService
        {
            System.ThrowHelper.ThrowIfNull(collection, "collection");
            TryAddScoped(collection, typeof(TService), typeof(TImplementation));
        }

        /// <summary>
        /// Adds the specified <typeparamref name="TService" /> as a <see cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped" /> service
        /// using the factory specified in <paramref name="implementationFactory" />
        /// to the <paramref name="services" /> if the service type hasn't already been registered.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.</param>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        public static void TryAddScoped<TService>(this IServiceCollection services, Func<IServiceProvider, TService> implementationFactory) where TService : class
        {
            TryAdd(services, ServiceDescriptor.Scoped(implementationFactory));
        }

        /// <summary>
        /// Adds the specified <paramref name="service" /> as a <see cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton" /> service
        /// to the <paramref name="collection" /> if the service type hasn't already been registered.
        /// </summary>
        /// <param name="collection">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.</param>
        /// <param name="service">The type of the service to register.</param>
        public static void TryAddSingleton(this IServiceCollection collection, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type service)
        {
            System.ThrowHelper.ThrowIfNull(collection, "collection");
            System.ThrowHelper.ThrowIfNull(service, "service");
            ServiceDescriptor descriptor = ServiceDescriptor.Singleton(service, service);
            TryAdd(collection, descriptor);
        }

        /// <summary>
        /// Adds the specified <paramref name="service" /> as a <see cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton" /> service
        /// with the <paramref name="implementationType" /> implementation
        /// to the <paramref name="collection" /> if the service type hasn't already been registered.
        /// </summary>
        /// <param name="collection">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.</param>
        /// <param name="service">The type of the service to register.</param>
        /// <param name="implementationType">The implementation type of the service.</param>
        public static void TryAddSingleton(this IServiceCollection collection, Type service, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type implementationType)
        {
            System.ThrowHelper.ThrowIfNull(collection, "collection");
            System.ThrowHelper.ThrowIfNull(service, "service");
            System.ThrowHelper.ThrowIfNull(implementationType, "implementationType");
            ServiceDescriptor descriptor = ServiceDescriptor.Singleton(service, implementationType);
            TryAdd(collection, descriptor);
        }

        /// <summary>
        /// Adds the specified <paramref name="service" /> as a <see cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton" /> service
        /// using the factory specified in <paramref name="implementationFactory" />
        /// to the <paramref name="collection" /> if the service type hasn't already been registered.
        /// </summary>
        /// <param name="collection">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.</param>
        /// <param name="service">The type of the service to register.</param>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        public static void TryAddSingleton(this IServiceCollection collection, Type service, Func<IServiceProvider, object> implementationFactory)
        {
            System.ThrowHelper.ThrowIfNull(collection, "collection");
            System.ThrowHelper.ThrowIfNull(service, "service");
            System.ThrowHelper.ThrowIfNull(implementationFactory, "implementationFactory");
            ServiceDescriptor descriptor = ServiceDescriptor.Singleton(service, implementationFactory);
            TryAdd(collection, descriptor);
        }

        /// <summary>
        /// Adds the specified <typeparamref name="TService" /> as a <see cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton" /> service
        /// to the <paramref name="collection" /> if the service type hasn't already been registered.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <param name="collection">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.</param>
        public static void TryAddSingleton<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TService>(this IServiceCollection collection) where TService : class
        {
            System.ThrowHelper.ThrowIfNull(collection, "collection");
            TryAddSingleton(collection, typeof(TService), typeof(TService));
        }

        /// <summary>
        /// Adds the specified <typeparamref name="TService" /> as a <see cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton" /> service
        /// implementation type specified in <typeparamref name="TImplementation" />
        /// to the <paramref name="collection" /> if the service type hasn't already been registered.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <param name="collection">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.</param>
        public static void TryAddSingleton<TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation>(this IServiceCollection collection) where TService : class where TImplementation : class, TService
        {
            System.ThrowHelper.ThrowIfNull(collection, "collection");
            TryAddSingleton(collection, typeof(TService), typeof(TImplementation));
        }

        /// <summary>
        /// Adds the specified <typeparamref name="TService" /> as a <see cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton" /> service
        /// with an instance specified in <paramref name="instance" />
        /// to the <paramref name="collection" /> if the service type hasn't already been registered.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <param name="collection">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.</param>
        /// <param name="instance">The instance of the service to add.</param>
        public static void TryAddSingleton<TService>(this IServiceCollection collection, TService instance) where TService : class
        {
            System.ThrowHelper.ThrowIfNull(collection, "collection");
            System.ThrowHelper.ThrowIfNull(instance, "instance");
            ServiceDescriptor descriptor = ServiceDescriptor.Singleton(typeof(TService), instance);
            TryAdd(collection, descriptor);
        }

        /// <summary>
        /// Adds the specified <typeparamref name="TService" /> as a <see cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton" /> service
        /// using the factory specified in <paramref name="implementationFactory" />
        /// to the <paramref name="services" /> if the service type hasn't already been registered.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.</param>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        public static void TryAddSingleton<TService>(this IServiceCollection services, Func<IServiceProvider, TService> implementationFactory) where TService : class
        {
            TryAdd(services, ServiceDescriptor.Singleton(implementationFactory));
        }

        /// <summary>
        /// Adds a <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" /> if an existing descriptor with the same
        /// <see cref="P:Microsoft.Extensions.DependencyInjection.ServiceDescriptor.ServiceType" /> and an implementation that does not already exist
        /// in <paramref name="services.." />.
        /// </summary>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.</param>
        /// <param name="descriptor">The <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" />.</param>
        /// <remarks>
        /// Use <see cref="M:Microsoft.Extensions.DependencyInjection.Extensions.ServiceCollectionDescriptorExtensions.TryAddEnumerable(Microsoft.Extensions.DependencyInjection.IServiceCollection,Microsoft.Extensions.DependencyInjection.ServiceDescriptor)" /> when registering a service implementation of a
        /// service type that
        /// supports multiple registrations of the same service type. Using
        /// <see cref="M:Microsoft.Extensions.DependencyInjection.Extensions.ServiceCollectionDescriptorExtensions.Add(Microsoft.Extensions.DependencyInjection.IServiceCollection,Microsoft.Extensions.DependencyInjection.ServiceDescriptor)" /> is not idempotent and can add
        /// duplicate
        /// <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" /> instances if called twice. Using
        /// <see cref="M:Microsoft.Extensions.DependencyInjection.Extensions.ServiceCollectionDescriptorExtensions.TryAddEnumerable(Microsoft.Extensions.DependencyInjection.IServiceCollection,Microsoft.Extensions.DependencyInjection.ServiceDescriptor)" /> will prevent registration
        /// of multiple implementation types.
        /// </remarks>
        public static void TryAddEnumerable(this IServiceCollection services, ServiceDescriptor descriptor)
        {
            System.ThrowHelper.ThrowIfNull(services, "services");
            System.ThrowHelper.ThrowIfNull(descriptor, "descriptor");
            Type implementationType = descriptor.GetImplementationType();
            if (implementationType == typeof(object) || implementationType == descriptor.ServiceType)
            {
                throw new ArgumentException(System.SR.Format(System.SR.TryAddIndistinguishableTypeToEnumerable, implementationType, descriptor.ServiceType), "descriptor");
            }
            int count = services.Count;
            for (int i = 0; i < count; i++)
            {
                ServiceDescriptor serviceDescriptor = services[i];
                if (serviceDescriptor.ServiceType == descriptor.ServiceType && serviceDescriptor.GetImplementationType() == implementationType && object.Equals(serviceDescriptor.ServiceKey, descriptor.ServiceKey))
                {
                    return;
                }
            }
            services.Add(descriptor);
        }

        /// <summary>
        /// Adds the specified <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" />s if an existing descriptor with the same
        /// <see cref="P:Microsoft.Extensions.DependencyInjection.ServiceDescriptor.ServiceType" /> and an implementation that does not already exist
        /// in <paramref name="services.." />.
        /// </summary>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.</param>
        /// <param name="descriptors">The <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" />s.</param>
        /// <remarks>
        /// Use <see cref="M:Microsoft.Extensions.DependencyInjection.Extensions.ServiceCollectionDescriptorExtensions.TryAddEnumerable(Microsoft.Extensions.DependencyInjection.IServiceCollection,Microsoft.Extensions.DependencyInjection.ServiceDescriptor)" /> when registering a service
        /// implementation of a service type that
        /// supports multiple registrations of the same service type. Using
        /// <see cref="M:Microsoft.Extensions.DependencyInjection.Extensions.ServiceCollectionDescriptorExtensions.Add(Microsoft.Extensions.DependencyInjection.IServiceCollection,Microsoft.Extensions.DependencyInjection.ServiceDescriptor)" /> is not idempotent and can add
        /// duplicate
        /// <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" /> instances if called twice. Using
        /// <see cref="M:Microsoft.Extensions.DependencyInjection.Extensions.ServiceCollectionDescriptorExtensions.TryAddEnumerable(Microsoft.Extensions.DependencyInjection.IServiceCollection,Microsoft.Extensions.DependencyInjection.ServiceDescriptor)" /> will prevent registration
        /// of multiple implementation types.
        /// </remarks>
        public static void TryAddEnumerable(this IServiceCollection services, IEnumerable<ServiceDescriptor> descriptors)
        {
            System.ThrowHelper.ThrowIfNull(services, "services");
            System.ThrowHelper.ThrowIfNull(descriptors, "descriptors");
            foreach (ServiceDescriptor descriptor in descriptors)
            {
                TryAddEnumerable(services, descriptor);
            }
        }

        /// <summary>
        /// Removes the first service in <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> with the same service type
        /// as <paramref name="descriptor" /> and adds <paramref name="descriptor" /> to the collection.
        /// </summary>
        /// <param name="collection">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.</param>
        /// <param name="descriptor">The <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" /> to replace with.</param>
        /// <returns>The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> for chaining.</returns>
        public static IServiceCollection Replace(this IServiceCollection collection, ServiceDescriptor descriptor)
        {
            System.ThrowHelper.ThrowIfNull(collection, "collection");
            System.ThrowHelper.ThrowIfNull(descriptor, "descriptor");
            int count = collection.Count;
            for (int i = 0; i < count; i++)
            {
                if (collection[i].ServiceType == descriptor.ServiceType && object.Equals(collection[i].ServiceKey, descriptor.ServiceKey))
                {
                    collection.RemoveAt(i);
                    break;
                }
            }
            collection.Add(descriptor);
            return collection;
        }

        /// <summary>
        /// Removes all services of type <typeparamref name="T" /> in <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <param name="collection">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.</param>
        /// <returns>The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> for chaining.</returns>
        public static IServiceCollection RemoveAll<T>(this IServiceCollection collection)
        {
            return RemoveAll(collection, typeof(T));
        }

        /// <summary>
        /// Removes all services of type <paramref name="serviceType" /> in <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <param name="collection">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.</param>
        /// <param name="serviceType">The service type to remove.</param>
        /// <returns>The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> for chaining.</returns>
        public static IServiceCollection RemoveAll(this IServiceCollection collection, Type serviceType)
        {
            System.ThrowHelper.ThrowIfNull(serviceType, "serviceType");
            for (int num = collection.Count - 1; num >= 0; num--)
            {
                ServiceDescriptor serviceDescriptor = collection[num];
                if (serviceDescriptor.ServiceType == serviceType && serviceDescriptor.ServiceKey == null)
                {
                    collection.RemoveAt(num);
                }
            }
            return collection;
        }

        /// <summary>
        /// Adds the specified <paramref name="service" /> as a <see cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient" /> service
        /// to the <paramref name="collection" /> if the service type hasn't already been registered.
        /// </summary>
        /// <param name="collection">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.</param>
        /// <param name="service">The type of the service to register.</param>
        /// <param name="serviceKey">The service key.</param>
        public static void TryAddKeyedTransient(this IServiceCollection collection, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type service, object serviceKey)
        {
            System.ThrowHelper.ThrowIfNull(collection, "collection");
            System.ThrowHelper.ThrowIfNull(service, "service");
            ServiceDescriptor descriptor = ServiceDescriptor.KeyedTransient(service, serviceKey, service);
            TryAdd(collection, descriptor);
        }

        /// <summary>
        /// Adds the specified <paramref name="service" /> as a <see cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient" /> service
        /// with the <paramref name="implementationType" /> implementation
        /// to the <paramref name="collection" /> if the service type hasn't already been registered.
        /// </summary>
        /// <param name="collection">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.</param>
        /// <param name="service">The type of the service to register.</param>
        /// <param name="serviceKey">The service key.</param>
        /// <param name="implementationType">The implementation type of the service.</param>
        public static void TryAddKeyedTransient(this IServiceCollection collection, Type service, object serviceKey, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type implementationType)
        {
            System.ThrowHelper.ThrowIfNull(collection, "collection");
            System.ThrowHelper.ThrowIfNull(service, "service");
            System.ThrowHelper.ThrowIfNull(implementationType, "implementationType");
            ServiceDescriptor descriptor = ServiceDescriptor.KeyedTransient(service, serviceKey, implementationType);
            TryAdd(collection, descriptor);
        }

        /// <summary>
        /// Adds the specified <paramref name="service" /> as a <see cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient" /> service
        /// using the factory specified in <paramref name="implementationFactory" />
        /// to the <paramref name="collection" /> if the service type hasn't already been registered.
        /// </summary>
        /// <param name="collection">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.</param>
        /// <param name="service">The type of the service to register.</param>
        /// <param name="serviceKey">The service key.</param>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        public static void TryAddKeyedTransient(this IServiceCollection collection, Type service, object serviceKey, Func<IServiceProvider, object, object> implementationFactory)
        {
            System.ThrowHelper.ThrowIfNull(collection, "collection");
            System.ThrowHelper.ThrowIfNull(service, "service");
            System.ThrowHelper.ThrowIfNull(implementationFactory, "implementationFactory");
            ServiceDescriptor descriptor = ServiceDescriptor.KeyedTransient(service, serviceKey, implementationFactory);
            TryAdd(collection, descriptor);
        }

        /// <summary>
        /// Adds the specified <typeparamref name="TService" /> as a <see cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient" /> service
        /// to the <paramref name="collection" /> if the service type hasn't already been registered.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <param name="collection">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.</param>
        /// <param name="serviceKey">The service key.</param>
        public static void TryAddKeyedTransient<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TService>(this IServiceCollection collection, object serviceKey) where TService : class
        {
            System.ThrowHelper.ThrowIfNull(collection, "collection");
            TryAddKeyedTransient(collection, typeof(TService), serviceKey, typeof(TService));
        }

        /// <summary>
        /// Adds the specified <typeparamref name="TService" /> as a <see cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient" /> service
        /// implementation type specified in <typeparamref name="TImplementation" />
        /// to the <paramref name="collection" /> if the service type hasn't already been registered.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <param name="collection">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.</param>
        /// <param name="serviceKey">The service key.</param>
        public static void TryAddKeyedTransient<TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation>(this IServiceCollection collection, object serviceKey) where TService : class where TImplementation : class, TService
        {
            System.ThrowHelper.ThrowIfNull(collection, "collection");
            TryAddKeyedTransient(collection, typeof(TService), serviceKey, typeof(TImplementation));
        }

        /// <summary>
        /// Adds the specified <typeparamref name="TService" /> as a <see cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient" /> service
        /// using the factory specified in <paramref name="implementationFactory" />
        /// to the <paramref name="services" /> if the service type hasn't already been registered.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.</param>
        /// <param name="serviceKey">The service key.</param>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        public static void TryAddKeyedTransient<TService>(this IServiceCollection services, object serviceKey, Func<IServiceProvider, object, TService> implementationFactory) where TService : class
        {
            TryAdd(services, ServiceDescriptor.KeyedTransient(serviceKey, implementationFactory));
        }

        /// <summary>
        /// Adds the specified <paramref name="service" /> as a <see cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped" /> service
        /// to the <paramref name="collection" /> if the service type hasn't already been registered.
        /// </summary>
        /// <param name="collection">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.</param>
        /// <param name="service">The type of the service to register.</param>
        /// <param name="serviceKey">The service key.</param>
        public static void TryAddKeyedScoped(this IServiceCollection collection, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type service, object serviceKey)
        {
            System.ThrowHelper.ThrowIfNull(collection, "collection");
            System.ThrowHelper.ThrowIfNull(service, "service");
            ServiceDescriptor descriptor = ServiceDescriptor.KeyedScoped(service, serviceKey, service);
            TryAdd(collection, descriptor);
        }

        /// <summary>
        /// Adds the specified <paramref name="service" /> as a <see cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped" /> service
        /// with the <paramref name="implementationType" /> implementation
        /// to the <paramref name="collection" /> if the service type hasn't already been registered.
        /// </summary>
        /// <param name="collection">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.</param>
        /// <param name="service">The type of the service to register.</param>
        /// <param name="serviceKey">The service key.</param>
        /// <param name="implementationType">The implementation type of the service.</param>
        public static void TryAddKeyedScoped(this IServiceCollection collection, Type service, object serviceKey, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type implementationType)
        {
            System.ThrowHelper.ThrowIfNull(collection, "collection");
            System.ThrowHelper.ThrowIfNull(service, "service");
            System.ThrowHelper.ThrowIfNull(implementationType, "implementationType");
            ServiceDescriptor descriptor = ServiceDescriptor.KeyedScoped(service, serviceKey, implementationType);
            TryAdd(collection, descriptor);
        }

        /// <summary>
        /// Adds the specified <paramref name="service" /> as a <see cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped" /> service
        /// using the factory specified in <paramref name="implementationFactory" />
        /// to the <paramref name="collection" /> if the service type hasn't already been registered.
        /// </summary>
        /// <param name="collection">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.</param>
        /// <param name="service">The type of the service to register.</param>
        /// <param name="serviceKey">The service key.</param>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        public static void TryAddKeyedScoped(this IServiceCollection collection, Type service, object serviceKey, Func<IServiceProvider, object, object> implementationFactory)
        {
            System.ThrowHelper.ThrowIfNull(collection, "collection");
            System.ThrowHelper.ThrowIfNull(service, "service");
            System.ThrowHelper.ThrowIfNull(implementationFactory, "implementationFactory");
            ServiceDescriptor descriptor = ServiceDescriptor.KeyedScoped(service, serviceKey, implementationFactory);
            TryAdd(collection, descriptor);
        }

        /// <summary>
        /// Adds the specified <typeparamref name="TService" /> as a <see cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped" /> service
        /// to the <paramref name="collection" /> if the service type hasn't already been registered.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <param name="collection">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.</param>
        /// <param name="serviceKey">The service key.</param>
        public static void TryAddKeyedScoped<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TService>(this IServiceCollection collection, object serviceKey) where TService : class
        {
            System.ThrowHelper.ThrowIfNull(collection, "collection");
            TryAddKeyedScoped(collection, typeof(TService), serviceKey, typeof(TService));
        }

        /// <summary>
        /// Adds the specified <typeparamref name="TService" /> as a <see cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped" /> service
        /// implementation type specified in <typeparamref name="TImplementation" />
        /// to the <paramref name="collection" /> if the service type hasn't already been registered.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <param name="collection">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.</param>
        /// <param name="serviceKey">The service key.</param>
        public static void TryAddKeyedScoped<TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation>(this IServiceCollection collection, object serviceKey) where TService : class where TImplementation : class, TService
        {
            System.ThrowHelper.ThrowIfNull(collection, "collection");
            TryAddKeyedScoped(collection, typeof(TService), serviceKey, typeof(TImplementation));
        }

        /// <summary>
        /// Adds the specified <typeparamref name="TService" /> as a <see cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped" /> service
        /// using the factory specified in <paramref name="implementationFactory" />
        /// to the <paramref name="services" /> if the service type hasn't already been registered.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.</param>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        /// <param name="serviceKey">The service key.</param>
        public static void TryAddKeyedScoped<TService>(this IServiceCollection services, object serviceKey, Func<IServiceProvider, object, TService> implementationFactory) where TService : class
        {
            TryAdd(services, ServiceDescriptor.KeyedScoped(serviceKey, implementationFactory));
        }

        /// <summary>
        /// Adds the specified <paramref name="service" /> as a <see cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton" /> service
        /// to the <paramref name="collection" /> if the service type hasn't already been registered.
        /// </summary>
        /// <param name="collection">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.</param>
        /// <param name="service">The type of the service to register.</param>
        /// <param name="serviceKey">The service key.</param>
        public static void TryAddKeyedSingleton(this IServiceCollection collection, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type service, object serviceKey)
        {
            System.ThrowHelper.ThrowIfNull(collection, "collection");
            System.ThrowHelper.ThrowIfNull(service, "service");
            ServiceDescriptor descriptor = ServiceDescriptor.KeyedSingleton(service, serviceKey, service);
            TryAdd(collection, descriptor);
        }

        /// <summary>
        /// Adds the specified <paramref name="service" /> as a <see cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton" /> service
        /// with the <paramref name="implementationType" /> implementation
        /// to the <paramref name="collection" /> if the service type hasn't already been registered.
        /// </summary>
        /// <param name="collection">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.</param>
        /// <param name="service">The type of the service to register.</param>
        /// <param name="serviceKey">The service key.</param>
        /// <param name="implementationType">The implementation type of the service.</param>
        public static void TryAddKeyedSingleton(this IServiceCollection collection, Type service, object serviceKey, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type implementationType)
        {
            System.ThrowHelper.ThrowIfNull(collection, "collection");
            System.ThrowHelper.ThrowIfNull(service, "service");
            System.ThrowHelper.ThrowIfNull(implementationType, "implementationType");
            ServiceDescriptor descriptor = ServiceDescriptor.KeyedSingleton(service, serviceKey, implementationType);
            TryAdd(collection, descriptor);
        }

        /// <summary>
        /// Adds the specified <paramref name="service" /> as a <see cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton" /> service
        /// using the factory specified in <paramref name="implementationFactory" />
        /// to the <paramref name="collection" /> if the service type hasn't already been registered.
        /// </summary>
        /// <param name="collection">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.</param>
        /// <param name="service">The type of the service to register.</param>
        /// <param name="serviceKey">The service key.</param>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        public static void TryAddKeyedSingleton(this IServiceCollection collection, Type service, object serviceKey, Func<IServiceProvider, object, object> implementationFactory)
        {
            System.ThrowHelper.ThrowIfNull(collection, "collection");
            System.ThrowHelper.ThrowIfNull(service, "service");
            System.ThrowHelper.ThrowIfNull(implementationFactory, "implementationFactory");
            ServiceDescriptor descriptor = ServiceDescriptor.KeyedSingleton(service, serviceKey, implementationFactory);
            TryAdd(collection, descriptor);
        }

        /// <summary>
        /// Adds the specified <typeparamref name="TService" /> as a <see cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton" /> service
        /// to the <paramref name="collection" /> if the service type hasn't already been registered.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <param name="collection">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.</param>
        /// <param name="serviceKey">The service key.</param>
        public static void TryAddKeyedSingleton<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TService>(this IServiceCollection collection, object serviceKey) where TService : class
        {
            System.ThrowHelper.ThrowIfNull(collection, "collection");
            TryAddKeyedSingleton(collection, typeof(TService), serviceKey, typeof(TService));
        }

        /// <summary>
        /// Adds the specified <typeparamref name="TService" /> as a <see cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton" /> service
        /// implementation type specified in <typeparamref name="TImplementation" />
        /// to the <paramref name="collection" /> if the service type hasn't already been registered.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <param name="collection">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.</param>
        /// <param name="serviceKey">The service key.</param>
        public static void TryAddKeyedSingleton<TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation>(this IServiceCollection collection, object serviceKey) where TService : class where TImplementation : class, TService
        {
            System.ThrowHelper.ThrowIfNull(collection, "collection");
            TryAddKeyedSingleton(collection, typeof(TService), serviceKey, typeof(TImplementation));
        }

        /// <summary>
        /// Adds the specified <typeparamref name="TService" /> as a <see cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton" /> service
        /// with an instance specified in <paramref name="instance" />
        /// to the <paramref name="collection" /> if the service type hasn't already been registered.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <param name="collection">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.</param>
        /// <param name="serviceKey">The service key.</param>
        /// <param name="instance">The instance of the service to add.</param>
        public static void TryAddKeyedSingleton<TService>(this IServiceCollection collection, object serviceKey, TService instance) where TService : class
        {
            System.ThrowHelper.ThrowIfNull(collection, "collection");
            System.ThrowHelper.ThrowIfNull(instance, "instance");
            ServiceDescriptor descriptor = ServiceDescriptor.KeyedSingleton(typeof(TService), serviceKey, instance);
            TryAdd(collection, descriptor);
        }

        /// <summary>
        /// Adds the specified <typeparamref name="TService" /> as a <see cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton" /> service
        /// using the factory specified in <paramref name="implementationFactory" />
        /// to the <paramref name="services" /> if the service type hasn't already been registered.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.</param>
        /// <param name="serviceKey">The service key.</param>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        public static void TryAddKeyedSingleton<TService>(this IServiceCollection services, object serviceKey, Func<IServiceProvider, object, TService> implementationFactory) where TService : class
        {
            TryAdd(services, ServiceDescriptor.KeyedSingleton(serviceKey, implementationFactory));
        }

        /// <summary>
        /// Removes all services of type <typeparamref name="T" /> in <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <param name="collection">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.</param>
        /// <param name="serviceKey">The service key.</param>
        /// <returns>The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> for chaining.</returns>
        public static IServiceCollection RemoveAllKeyed<T>(this IServiceCollection collection, object serviceKey)
        {
            return RemoveAllKeyed(collection, typeof(T), serviceKey);
        }

        /// <summary>
        /// Removes all services of type <paramref name="serviceType" /> in <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <param name="collection">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.</param>
        /// <param name="serviceType">The service type to remove.</param>
        /// <param name="serviceKey">The service key.</param>
        /// <returns>The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> for chaining.</returns>
        public static IServiceCollection RemoveAllKeyed(this IServiceCollection collection, Type serviceType, object serviceKey)
        {
            System.ThrowHelper.ThrowIfNull(serviceType, "serviceType");
            for (int num = collection.Count - 1; num >= 0; num--)
            {
                ServiceDescriptor serviceDescriptor = collection[num];
                if (serviceDescriptor.ServiceType == serviceType && object.Equals(serviceDescriptor.ServiceKey, serviceKey))
                {
                    collection.RemoveAt(num);
                }
            }
            return collection;
        }
    }
}
