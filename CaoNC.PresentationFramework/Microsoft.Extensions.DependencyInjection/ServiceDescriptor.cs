using CaoNC.System.Diagnostics.CodeAnalysis;
using System.Diagnostics;
using System;

namespace CaoNC.Microsoft.Extensions.DependencyInjection
{
    [DebuggerDisplay("{DebuggerToString(),nq}")]
    public class ServiceDescriptor
    {
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        private Type _implementationType;

        private object _implementationInstance;

        private object _implementationFactory;

        /// <summary>
        /// Gets the <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceLifetime" /> of the service.
        /// </summary>
        public ServiceLifetime Lifetime { get; }

        /// <summary>
        /// Get the key of the service, if applicable.
        /// </summary>
        public object ServiceKey { get; }

        /// <summary>
        /// Gets the <see cref="T:System.Type" /> of the service.
        /// </summary>
        public Type ServiceType { get; }

        /// <summary>
        /// Gets the <see cref="T:System.Type" /> that implements the service,
        /// or returns <see langword="null" /> if <see cref="P:Microsoft.Extensions.DependencyInjection.ServiceDescriptor.IsKeyedService" /> is <see langword="true" />.
        /// </summary>
        /// <remarks>
        /// If <see cref="P:Microsoft.Extensions.DependencyInjection.ServiceDescriptor.IsKeyedService" /> is <see langword="true" />, <see cref="P:Microsoft.Extensions.DependencyInjection.ServiceDescriptor.KeyedImplementationType" /> should be called instead.
        /// </remarks>
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        public Type ImplementationType
        {
            get
            {
                if (!IsKeyedService)
                {
                    return _implementationType;
                }
                return null;
            }
        }

        /// <summary>
        /// Gets the <see cref="T:System.Type" /> that implements the service,
        /// or throws <see cref="T:System.InvalidOperationException" /> if <see cref="P:Microsoft.Extensions.DependencyInjection.ServiceDescriptor.IsKeyedService" /> is <see langword="false" />.
        /// </summary>
        /// <remarks>
        /// If <see cref="P:Microsoft.Extensions.DependencyInjection.ServiceDescriptor.IsKeyedService" /> is <see langword="false" />, <see cref="P:Microsoft.Extensions.DependencyInjection.ServiceDescriptor.ImplementationType" /> should be called instead.
        /// </remarks>
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        public Type KeyedImplementationType
        {
            get
            {
                if (!IsKeyedService)
                {
                    ThrowNonKeyedDescriptor();
                }
                return _implementationType;
            }
        }

        /// <summary>
        /// Gets the instance that implements the service,
        /// or returns <see langword="null" /> if <see cref="P:Microsoft.Extensions.DependencyInjection.ServiceDescriptor.IsKeyedService" /> is <see langword="true" />.
        /// </summary>
        /// <remarks>
        /// If <see cref="P:Microsoft.Extensions.DependencyInjection.ServiceDescriptor.IsKeyedService" /> is <see langword="true" />, <see cref="P:Microsoft.Extensions.DependencyInjection.ServiceDescriptor.KeyedImplementationInstance" /> should be called instead.
        /// </remarks>
        public object ImplementationInstance
        {
            get
            {
                if (!IsKeyedService)
                {
                    return _implementationInstance;
                }
                return null;
            }
        }

        /// <summary>
        /// Gets the instance that implements the service,
        /// or throws <see cref="T:System.InvalidOperationException" /> if <see cref="P:Microsoft.Extensions.DependencyInjection.ServiceDescriptor.IsKeyedService" /> is <see langword="false" />.
        /// </summary>
        /// <remarks>
        /// If <see cref="P:Microsoft.Extensions.DependencyInjection.ServiceDescriptor.IsKeyedService" /> is <see langword="false" />, <see cref="P:Microsoft.Extensions.DependencyInjection.ServiceDescriptor.ImplementationInstance" /> should be called instead.
        /// </remarks>
        public object KeyedImplementationInstance
        {
            get
            {
                if (!IsKeyedService)
                {
                    ThrowNonKeyedDescriptor();
                }
                return _implementationInstance;
            }
        }

        /// <summary>
        /// Gets the factory used for creating service instance,
        /// or returns <see langword="null" /> if <see cref="P:Microsoft.Extensions.DependencyInjection.ServiceDescriptor.IsKeyedService" /> is <see langword="true" />.
        /// </summary>
        /// <remarks>
        /// If <see cref="P:Microsoft.Extensions.DependencyInjection.ServiceDescriptor.IsKeyedService" /> is <see langword="true" />, <see cref="P:Microsoft.Extensions.DependencyInjection.ServiceDescriptor.KeyedImplementationFactory" /> should be called instead.
        /// </remarks>
        public Func<IServiceProvider, object> ImplementationFactory
        {
            get
            {
                if (!IsKeyedService)
                {
                    return (Func<IServiceProvider, object>)_implementationFactory;
                }
                return null;
            }
        }

        /// <summary>
        /// Gets the factory used for creating Keyed service instances,
        /// or throws <see cref="T:System.InvalidOperationException" /> if <see cref="P:Microsoft.Extensions.DependencyInjection.ServiceDescriptor.IsKeyedService" /> is <see langword="false" />.
        /// </summary>
        /// <remarks>
        /// If <see cref="P:Microsoft.Extensions.DependencyInjection.ServiceDescriptor.IsKeyedService" /> is <see langword="false" />, <see cref="P:Microsoft.Extensions.DependencyInjection.ServiceDescriptor.ImplementationFactory" /> should be called instead.
        /// </remarks>
        public Func<IServiceProvider, object, object> KeyedImplementationFactory
        {
            get
            {
                if (!IsKeyedService)
                {
                    ThrowNonKeyedDescriptor();
                }
                return (Func<IServiceProvider, object, object>)_implementationFactory;
            }
        }

        /// <summary>
        /// Indicates whether the service is a keyed service.
        /// </summary>
        public bool IsKeyedService => ServiceKey != null;

        /// <summary>
        /// Initializes a new instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" /> with the specified <paramref name="implementationType" />.
        /// </summary>
        /// <param name="serviceType">The <see cref="T:System.Type" /> of the service.</param>
        /// <param name="implementationType">The <see cref="T:System.Type" /> implementing the service.</param>
        /// <param name="lifetime">The <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceLifetime" /> of the service.</param>
        public ServiceDescriptor(Type serviceType, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type implementationType, ServiceLifetime lifetime)
            : this(serviceType, null, implementationType, lifetime)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" /> with the specified <paramref name="implementationType" />.
        /// </summary>
        /// <param name="serviceType">The <see cref="T:System.Type" /> of the service.</param>
        /// <param name="serviceKey">The <see cref="P:Microsoft.Extensions.DependencyInjection.ServiceDescriptor.ServiceKey" /> of the service.</param>
        /// <param name="implementationType">The <see cref="T:System.Type" /> implementing the service.</param>
        /// <param name="lifetime">The <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceLifetime" /> of the service.</param>
        public ServiceDescriptor(Type serviceType, object serviceKey, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type implementationType, ServiceLifetime lifetime)
            : this(serviceType, serviceKey, lifetime)
        {
            System.ThrowHelper.ThrowIfNull(serviceType, "serviceType");
            System.ThrowHelper.ThrowIfNull(implementationType, "implementationType");
            _implementationType = implementationType;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" /> with the specified <paramref name="instance" />
        /// as a <see cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton" />.
        /// </summary>
        /// <param name="serviceType">The <see cref="T:System.Type" /> of the service.</param>
        /// <param name="instance">The instance implementing the service.</param>
        public ServiceDescriptor(Type serviceType, object instance)
            : this(serviceType, null, instance)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" /> with the specified <paramref name="instance" />
        /// as a <see cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton" />.
        /// </summary>
        /// <param name="serviceType">The <see cref="T:System.Type" /> of the service.</param>
        /// <param name="serviceKey">The <see cref="P:Microsoft.Extensions.DependencyInjection.ServiceDescriptor.ServiceKey" /> of the service.</param>
        /// <param name="instance">The instance implementing the service.</param>
        public ServiceDescriptor(Type serviceType, object serviceKey, object instance)
            : this(serviceType, serviceKey, ServiceLifetime.Singleton)
        {
            System.ThrowHelper.ThrowIfNull(serviceType, "serviceType");
            System.ThrowHelper.ThrowIfNull(instance, "instance");
            _implementationInstance = instance;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" /> with the specified <paramref name="factory" />.
        /// </summary>
        /// <param name="serviceType">The <see cref="T:System.Type" /> of the service.</param>
        /// <param name="factory">A factory used for creating service instances.</param>
        /// <param name="lifetime">The <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceLifetime" /> of the service.</param>
        public ServiceDescriptor(Type serviceType, Func<IServiceProvider, object> factory, ServiceLifetime lifetime)
            : this(serviceType, (object)null, lifetime)
        {
            System.ThrowHelper.ThrowIfNull(serviceType, "serviceType");
            System.ThrowHelper.ThrowIfNull(factory, "factory");
            _implementationFactory = factory;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" /> with the specified <paramref name="factory" />.
        /// </summary>
        /// <param name="serviceType">The <see cref="T:System.Type" /> of the service.</param>
        /// <param name="serviceKey">The <see cref="P:Microsoft.Extensions.DependencyInjection.ServiceDescriptor.ServiceKey" /> of the service.</param>
        /// <param name="factory">A factory used for creating service instances.</param>
        /// <param name="lifetime">The <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceLifetime" /> of the service.</param>
        public ServiceDescriptor(Type serviceType, object serviceKey, Func<IServiceProvider, object, object> factory, ServiceLifetime lifetime)
        {
            Func<IServiceProvider, object, object> factory2 = factory;
            Lifetime = lifetime;
            ServiceType = serviceType;
            ServiceKey = serviceKey;
            System.ThrowHelper.ThrowIfNull(serviceType, "serviceType");
            System.ThrowHelper.ThrowIfNull(factory2, "factory");
            if (serviceKey == null)
            {
                Func<IServiceProvider, object> func = (Func<IServiceProvider, object>)(_implementationFactory = (Func<IServiceProvider, object>)((IServiceProvider sp) => factory2(sp, null)));
            }
            else
            {
                _implementationFactory = factory2;
            }
        }

        private ServiceDescriptor(Type serviceType, object serviceKey, ServiceLifetime lifetime)
        {
            Lifetime = lifetime;
            ServiceType = serviceType;
            ServiceKey = serviceKey;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            string text = string.Format("{0}: {1} {2}: {3} ", "ServiceType", ServiceType, "Lifetime", Lifetime);
            if (IsKeyedService)
            {
                text += string.Format("{0}: {1} ", "ServiceKey", ServiceKey);
                if (KeyedImplementationType != null)
                {
                    return text + string.Format("{0}: {1}", "KeyedImplementationType", KeyedImplementationType);
                }
                if (KeyedImplementationFactory != null)
                {
                    return text + string.Format("{0}: {1}", "KeyedImplementationFactory", KeyedImplementationFactory.Method);
                }
                return text + string.Format("{0}: {1}", "KeyedImplementationInstance", KeyedImplementationInstance);
            }
            if (ImplementationType != null)
            {
                return text + string.Format("{0}: {1}", "ImplementationType", ImplementationType);
            }
            if (ImplementationFactory != null)
            {
                return text + string.Format("{0}: {1}", "ImplementationFactory", ImplementationFactory.Method);
            }
            return text + string.Format("{0}: {1}", "ImplementationInstance", ImplementationInstance);
        }

        internal Type GetImplementationType()
        {
            if (ServiceKey == null)
            {
                if (ImplementationType != null)
                {
                    return ImplementationType;
                }
                if (ImplementationInstance != null)
                {
                    return ImplementationInstance.GetType();
                }
                if (ImplementationFactory != null)
                {
                    Type[] genericTypeArguments = ImplementationFactory.GetType().GenericTypeArguments;
                    return genericTypeArguments[1];
                }
            }
            else
            {
                if (KeyedImplementationType != null)
                {
                    return KeyedImplementationType;
                }
                if (KeyedImplementationInstance != null)
                {
                    return KeyedImplementationInstance.GetType();
                }
                if (KeyedImplementationFactory != null)
                {
                    Type[] genericTypeArguments2 = KeyedImplementationFactory.GetType().GenericTypeArguments;
                    return genericTypeArguments2[2];
                }
            }
            return null;
        }

        /// <summary>
        /// Creates an instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" /> with the specified
        /// <typeparamref name="TService" />, <typeparamref name="TImplementation" />,
        /// and the <see cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient" /> lifetime.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <returns>A new instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" />.</returns>
        public static ServiceDescriptor Transient<TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation>() where TService : class where TImplementation : class, TService
        {
            return ServiceDescriptor.DescribeKeyed<TService, TImplementation>((object)null, ServiceLifetime.Transient);
        }

        /// <summary>
        /// Creates an instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" /> with the specified
        /// <typeparamref name="TService" />, <typeparamref name="TImplementation" />,
        /// and the <see cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient" /> lifetime.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="serviceKey">The <see cref="P:Microsoft.Extensions.DependencyInjection.ServiceDescriptor.ServiceKey" /> of the service.</param>
        /// <returns>A new instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" />.</returns>
        public static ServiceDescriptor KeyedTransient<TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation>(object serviceKey) where TService : class where TImplementation : class, TService
        {
            return ServiceDescriptor.DescribeKeyed<TService, TImplementation>(serviceKey, ServiceLifetime.Transient);
        }

        /// <summary>
        /// Creates an instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" /> with the specified
        /// <paramref name="service" /> and <paramref name="implementationType" />
        /// and the <see cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient" /> lifetime.
        /// </summary>
        /// <param name="service">The type of the service.</param>
        /// <param name="implementationType">The type of the implementation.</param>
        /// <returns>A new instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" />.</returns>
        public static ServiceDescriptor Transient(Type service, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type implementationType)
        {
            System.ThrowHelper.ThrowIfNull(service, "service");
            System.ThrowHelper.ThrowIfNull(implementationType, "implementationType");
            return Describe(service, implementationType, ServiceLifetime.Transient);
        }

        /// <summary>
        /// Creates an instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" /> with the specified
        /// <paramref name="service" /> and <paramref name="implementationType" />
        /// and the <see cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient" /> lifetime.
        /// </summary>
        /// <param name="service">The type of the service.</param>
        /// <param name="serviceKey">The <see cref="P:Microsoft.Extensions.DependencyInjection.ServiceDescriptor.ServiceKey" /> of the service.</param>
        /// <param name="implementationType">The type of the implementation.</param>
        /// <returns>A new instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" />.</returns>
        public static ServiceDescriptor KeyedTransient(Type service, object serviceKey, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type implementationType)
        {
            System.ThrowHelper.ThrowIfNull(service, "service");
            System.ThrowHelper.ThrowIfNull(implementationType, "implementationType");
            return DescribeKeyed(service, serviceKey, implementationType, ServiceLifetime.Transient);
        }

        /// <summary>
        /// Creates an instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" /> with the specified
        /// <typeparamref name="TService" />, <typeparamref name="TImplementation" />,
        /// <paramref name="implementationFactory" />,
        /// and the <see cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient" /> lifetime.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="implementationFactory">A factory to create new instances of the service implementation.</param>
        /// <returns>A new instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" />.</returns>
        public static ServiceDescriptor Transient<TService, TImplementation>(Func<IServiceProvider, TImplementation> implementationFactory) where TService : class where TImplementation : class, TService
        {
            System.ThrowHelper.ThrowIfNull(implementationFactory, "implementationFactory");
            return Describe(typeof(TService), implementationFactory, ServiceLifetime.Transient);
        }

        /// <summary>
        /// Creates an instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" /> with the specified
        /// <typeparamref name="TService" />, <typeparamref name="TImplementation" />,
        /// <paramref name="implementationFactory" />,
        /// and the <see cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient" /> lifetime.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="serviceKey">The <see cref="P:Microsoft.Extensions.DependencyInjection.ServiceDescriptor.ServiceKey" /> of the service.</param>
        /// <param name="implementationFactory">A factory to create new instances of the service implementation.</param>
        /// <returns>A new instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" />.</returns>
        public static ServiceDescriptor KeyedTransient<TService, TImplementation>(object serviceKey, Func<IServiceProvider, object, TImplementation> implementationFactory) where TService : class where TImplementation : class, TService
        {
            System.ThrowHelper.ThrowIfNull(implementationFactory, "implementationFactory");
            return DescribeKeyed(typeof(TService), serviceKey, implementationFactory, ServiceLifetime.Transient);
        }

        /// <summary>
        /// Creates an instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" /> with the specified
        /// <typeparamref name="TService" />, <paramref name="implementationFactory" />,
        /// and the <see cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient" /> lifetime.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="implementationFactory">A factory to create new instances of the service implementation.</param>
        /// <returns>A new instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" />.</returns>
        public static ServiceDescriptor Transient<TService>(Func<IServiceProvider, TService> implementationFactory) where TService : class
        {
            System.ThrowHelper.ThrowIfNull(implementationFactory, "implementationFactory");
            return Describe(typeof(TService), implementationFactory, ServiceLifetime.Transient);
        }

        /// <summary>
        /// Creates an instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" /> with the specified
        /// <typeparamref name="TService" />, <paramref name="implementationFactory" />,
        /// and the <see cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient" /> lifetime.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="serviceKey">The <see cref="P:Microsoft.Extensions.DependencyInjection.ServiceDescriptor.ServiceKey" /> of the service.</param>
        /// <param name="implementationFactory">A factory to create new instances of the service implementation.</param>
        /// <returns>A new instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" />.</returns>
        public static ServiceDescriptor KeyedTransient<TService>(object serviceKey, Func<IServiceProvider, object, TService> implementationFactory) where TService : class
        {
            System.ThrowHelper.ThrowIfNull(implementationFactory, "implementationFactory");
            return DescribeKeyed(typeof(TService), serviceKey, implementationFactory, ServiceLifetime.Transient);
        }

        /// <summary>
        /// Creates an instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" /> with the specified
        /// <paramref name="service" />, <paramref name="implementationFactory" />,
        /// and the <see cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient" /> lifetime.
        /// </summary>
        /// <param name="service">The type of the service.</param>
        /// <param name="implementationFactory">A factory to create new instances of the service implementation.</param>
        /// <returns>A new instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" />.</returns>
        public static ServiceDescriptor Transient(Type service, Func<IServiceProvider, object> implementationFactory)
        {
            System.ThrowHelper.ThrowIfNull(service, "service");
            System.ThrowHelper.ThrowIfNull(implementationFactory, "implementationFactory");
            return Describe(service, implementationFactory, ServiceLifetime.Transient);
        }

        /// <summary>
        /// Creates an instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" /> with the specified
        /// <paramref name="service" />, <paramref name="implementationFactory" />,
        /// and the <see cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient" /> lifetime.
        /// </summary>
        /// <param name="service">The type of the service.</param>
        /// <param name="serviceKey">The <see cref="P:Microsoft.Extensions.DependencyInjection.ServiceDescriptor.ServiceKey" /> of the service.</param>
        /// <param name="implementationFactory">A factory to create new instances of the service implementation.</param>
        /// <returns>A new instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" />.</returns>
        public static ServiceDescriptor KeyedTransient(Type service, object serviceKey, Func<IServiceProvider, object, object> implementationFactory)
        {
            System.ThrowHelper.ThrowIfNull(service, "service");
            System.ThrowHelper.ThrowIfNull(implementationFactory, "implementationFactory");
            return DescribeKeyed(service, serviceKey, implementationFactory, ServiceLifetime.Transient);
        }

        /// <summary>
        /// Creates an instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" /> with the specified
        /// <typeparamref name="TService" />, <typeparamref name="TImplementation" />,
        /// and the <see cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped" /> lifetime.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <returns>A new instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" />.</returns>
        public static ServiceDescriptor Scoped<TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation>() where TService : class where TImplementation : class, TService
        {
            return ServiceDescriptor.DescribeKeyed<TService, TImplementation>((object)null, ServiceLifetime.Scoped);
        }

        /// <summary>
        /// Creates an instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" /> with the specified
        /// <typeparamref name="TService" />, <typeparamref name="TImplementation" />,
        /// and the <see cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped" /> lifetime.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="serviceKey">The <see cref="P:Microsoft.Extensions.DependencyInjection.ServiceDescriptor.ServiceKey" /> of the service.</param>
        /// <returns>A new instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" />.</returns>
        public static ServiceDescriptor KeyedScoped<TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation>(object serviceKey) where TService : class where TImplementation : class, TService
        {
            return ServiceDescriptor.DescribeKeyed<TService, TImplementation>(serviceKey, ServiceLifetime.Scoped);
        }

        /// <summary>
        /// Creates an instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" /> with the specified
        /// <paramref name="service" /> and <paramref name="implementationType" />
        /// and the <see cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped" /> lifetime.
        /// </summary>
        /// <param name="service">The type of the service.</param>
        /// <param name="implementationType">The type of the implementation.</param>
        /// <returns>A new instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" />.</returns>
        public static ServiceDescriptor Scoped(Type service, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type implementationType)
        {
            return Describe(service, implementationType, ServiceLifetime.Scoped);
        }

        /// <summary>
        /// Creates an instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" /> with the specified
        /// <paramref name="service" /> and <paramref name="implementationType" />
        /// and the <see cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped" /> lifetime.
        /// </summary>
        /// <param name="service">The type of the service.</param>
        /// <param name="serviceKey">The <see cref="P:Microsoft.Extensions.DependencyInjection.ServiceDescriptor.ServiceKey" /> of the service.</param>
        /// <param name="implementationType">The type of the implementation.</param>
        /// <returns>A new instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" />.</returns>
        public static ServiceDescriptor KeyedScoped(Type service, object serviceKey, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type implementationType)
        {
            return DescribeKeyed(service, serviceKey, implementationType, ServiceLifetime.Scoped);
        }

        /// <summary>
        /// Creates an instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" /> with the specified
        /// <typeparamref name="TService" />, <typeparamref name="TImplementation" />,
        /// <paramref name="implementationFactory" />,
        /// and the <see cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped" /> lifetime.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="implementationFactory">A factory to create new instances of the service implementation.</param>
        /// <returns>A new instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" />.</returns>
        public static ServiceDescriptor Scoped<TService, TImplementation>(Func<IServiceProvider, TImplementation> implementationFactory) where TService : class where TImplementation : class, TService
        {
            System.ThrowHelper.ThrowIfNull(implementationFactory, "implementationFactory");
            return Describe(typeof(TService), implementationFactory, ServiceLifetime.Scoped);
        }

        /// <summary>
        /// Creates an instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" /> with the specified
        /// <typeparamref name="TService" />, <typeparamref name="TImplementation" />,
        /// <paramref name="implementationFactory" />,
        /// and the <see cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped" /> lifetime.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="serviceKey">The <see cref="P:Microsoft.Extensions.DependencyInjection.ServiceDescriptor.ServiceKey" /> of the service.</param>
        /// <param name="implementationFactory">A factory to create new instances of the service implementation.</param>
        /// <returns>A new instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" />.</returns>
        public static ServiceDescriptor KeyedScoped<TService, TImplementation>(object serviceKey, Func<IServiceProvider, object, TImplementation> implementationFactory) where TService : class where TImplementation : class, TService
        {
            System.ThrowHelper.ThrowIfNull(implementationFactory, "implementationFactory");
            return DescribeKeyed(typeof(TService), serviceKey, implementationFactory, ServiceLifetime.Scoped);
        }

        /// <summary>
        /// Creates an instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" /> with the specified
        /// <typeparamref name="TService" />, <paramref name="implementationFactory" />,
        /// and the <see cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped" /> lifetime.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="implementationFactory">A factory to create new instances of the service implementation.</param>
        /// <returns>A new instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" />.</returns>
        public static ServiceDescriptor Scoped<TService>(Func<IServiceProvider, TService> implementationFactory) where TService : class
        {
            System.ThrowHelper.ThrowIfNull(implementationFactory, "implementationFactory");
            return Describe(typeof(TService), implementationFactory, ServiceLifetime.Scoped);
        }

        /// <summary>
        /// Creates an instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" /> with the specified
        /// <typeparamref name="TService" />, <paramref name="implementationFactory" />,
        /// and the <see cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped" /> lifetime.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="serviceKey">The <see cref="P:Microsoft.Extensions.DependencyInjection.ServiceDescriptor.ServiceKey" /> of the service.</param>
        /// <param name="implementationFactory">A factory to create new instances of the service implementation.</param>
        /// <returns>A new instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" />.</returns>
        public static ServiceDescriptor KeyedScoped<TService>(object serviceKey, Func<IServiceProvider, object, TService> implementationFactory) where TService : class
        {
            System.ThrowHelper.ThrowIfNull(implementationFactory, "implementationFactory");
            return DescribeKeyed(typeof(TService), serviceKey, implementationFactory, ServiceLifetime.Scoped);
        }

        /// <summary>
        /// Creates an instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" /> with the specified
        /// <paramref name="service" />, <paramref name="implementationFactory" />,
        /// and the <see cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped" /> lifetime.
        /// </summary>
        /// <param name="service">The type of the service.</param>
        /// <param name="implementationFactory">A factory to create new instances of the service implementation.</param>
        /// <returns>A new instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" />.</returns>
        public static ServiceDescriptor Scoped(Type service, Func<IServiceProvider, object> implementationFactory)
        {
            System.ThrowHelper.ThrowIfNull(service, "service");
            System.ThrowHelper.ThrowIfNull(implementationFactory, "implementationFactory");
            return Describe(service, implementationFactory, ServiceLifetime.Scoped);
        }

        /// <summary>
        /// Creates an instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" /> with the specified
        /// <paramref name="service" />, <paramref name="implementationFactory" />,
        /// and the <see cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped" /> lifetime.
        /// </summary>
        /// <param name="service">The type of the service.</param>
        /// <param name="serviceKey">The <see cref="P:Microsoft.Extensions.DependencyInjection.ServiceDescriptor.ServiceKey" /> of the service.</param>
        /// <param name="implementationFactory">A factory to create new instances of the service implementation.</param>
        /// <returns>A new instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" />.</returns>
        public static ServiceDescriptor KeyedScoped(Type service, object serviceKey, Func<IServiceProvider, object, object> implementationFactory)
        {
            System.ThrowHelper.ThrowIfNull(service, "service");
            System.ThrowHelper.ThrowIfNull(implementationFactory, "implementationFactory");
            return DescribeKeyed(service, serviceKey, implementationFactory, ServiceLifetime.Scoped);
        }

        /// <summary>
        /// Creates an instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" /> with the specified
        /// <typeparamref name="TService" />, <typeparamref name="TImplementation" />,
        /// and the <see cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton" /> lifetime.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <returns>A new instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" />.</returns>
        public static ServiceDescriptor Singleton<TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation>() where TService : class where TImplementation : class, TService
        {
            return ServiceDescriptor.DescribeKeyed<TService, TImplementation>((object)null, ServiceLifetime.Singleton);
        }

        /// <summary>
        /// Creates an instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" /> with the specified
        /// <typeparamref name="TService" />, <typeparamref name="TImplementation" />,
        /// and the <see cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton" /> lifetime.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="serviceKey">The <see cref="P:Microsoft.Extensions.DependencyInjection.ServiceDescriptor.ServiceKey" /> of the service.</param>
        /// <returns>A new instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" />.</returns>
        public static ServiceDescriptor KeyedSingleton<TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation>(object serviceKey) where TService : class where TImplementation : class, TService
        {
            return ServiceDescriptor.DescribeKeyed<TService, TImplementation>(serviceKey, ServiceLifetime.Singleton);
        }

        /// <summary>
        /// Creates an instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" /> with the specified
        /// <paramref name="service" /> and <paramref name="implementationType" />
        /// and the <see cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton" /> lifetime.
        /// </summary>
        /// <param name="service">The type of the service.</param>
        /// <param name="implementationType">The type of the implementation.</param>
        /// <returns>A new instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" />.</returns>
        public static ServiceDescriptor Singleton(Type service, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type implementationType)
        {
            System.ThrowHelper.ThrowIfNull(service, "service");
            System.ThrowHelper.ThrowIfNull(implementationType, "implementationType");
            return Describe(service, implementationType, ServiceLifetime.Singleton);
        }

        /// <summary>
        /// Creates an instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" /> with the specified
        /// <paramref name="service" /> and <paramref name="implementationType" />
        /// and the <see cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton" /> lifetime.
        /// </summary>
        /// <param name="service">The type of the service.</param>
        /// <param name="serviceKey">The <see cref="P:Microsoft.Extensions.DependencyInjection.ServiceDescriptor.ServiceKey" /> of the service.</param>
        /// <param name="implementationType">The type of the implementation.</param>
        /// <returns>A new instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" />.</returns>
        public static ServiceDescriptor KeyedSingleton(Type service, object serviceKey, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type implementationType)
        {
            System.ThrowHelper.ThrowIfNull(service, "service");
            System.ThrowHelper.ThrowIfNull(implementationType, "implementationType");
            return DescribeKeyed(service, serviceKey, implementationType, ServiceLifetime.Singleton);
        }

        /// <summary>
        /// Creates an instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" /> with the specified
        /// <typeparamref name="TService" />, <typeparamref name="TImplementation" />,
        /// <paramref name="implementationFactory" />,
        /// and the <see cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton" /> lifetime.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="implementationFactory">A factory to create new instances of the service implementation.</param>
        /// <returns>A new instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" />.</returns>
        public static ServiceDescriptor Singleton<TService, TImplementation>(Func<IServiceProvider, TImplementation> implementationFactory) where TService : class where TImplementation : class, TService
        {
            System.ThrowHelper.ThrowIfNull(implementationFactory, "implementationFactory");
            return Describe(typeof(TService), implementationFactory, ServiceLifetime.Singleton);
        }

        /// <summary>
        /// Creates an instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" /> with the specified
        /// <typeparamref name="TService" />, <typeparamref name="TImplementation" />,
        /// <paramref name="implementationFactory" />,
        /// and the <see cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton" /> lifetime.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="serviceKey">The <see cref="P:Microsoft.Extensions.DependencyInjection.ServiceDescriptor.ServiceKey" /> of the service.</param>
        /// <param name="implementationFactory">A factory to create new instances of the service implementation.</param>
        /// <returns>A new instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" />.</returns>
        public static ServiceDescriptor KeyedSingleton<TService, TImplementation>(object serviceKey, Func<IServiceProvider, object, TImplementation> implementationFactory) where TService : class where TImplementation : class, TService
        {
            System.ThrowHelper.ThrowIfNull(implementationFactory, "implementationFactory");
            return DescribeKeyed(typeof(TService), serviceKey, implementationFactory, ServiceLifetime.Singleton);
        }

        /// <summary>
        /// Creates an instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" /> with the specified
        /// <typeparamref name="TService" />, <paramref name="implementationFactory" />,
        /// and the <see cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton" /> lifetime.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="implementationFactory">A factory to create new instances of the service implementation.</param>
        /// <returns>A new instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" />.</returns>
        public static ServiceDescriptor Singleton<TService>(Func<IServiceProvider, TService> implementationFactory) where TService : class
        {
            System.ThrowHelper.ThrowIfNull(implementationFactory, "implementationFactory");
            return Describe(typeof(TService), implementationFactory, ServiceLifetime.Singleton);
        }

        /// <summary>
        /// Creates an instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" /> with the specified
        /// <typeparamref name="TService" />, <paramref name="implementationFactory" />,
        /// and the <see cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton" /> lifetime.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="serviceKey">The <see cref="P:Microsoft.Extensions.DependencyInjection.ServiceDescriptor.ServiceKey" /> of the service.</param>
        /// <param name="implementationFactory">A factory to create new instances of the service implementation.</param>
        /// <returns>A new instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" />.</returns>
        public static ServiceDescriptor KeyedSingleton<TService>(object serviceKey, Func<IServiceProvider, object, TService> implementationFactory) where TService : class
        {
            System.ThrowHelper.ThrowIfNull(implementationFactory, "implementationFactory");
            return DescribeKeyed(typeof(TService), serviceKey, implementationFactory, ServiceLifetime.Singleton);
        }

        /// <summary>
        /// Creates an instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" /> with the specified
        /// <paramref name="serviceType" />, <paramref name="implementationFactory" />,
        /// and the <see cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton" /> lifetime.
        /// </summary>
        /// <param name="serviceType">The type of the service.</param>
        /// <param name="implementationFactory">A factory to create new instances of the service implementation.</param>
        /// <returns>A new instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" />.</returns>
        public static ServiceDescriptor Singleton(Type serviceType, Func<IServiceProvider, object> implementationFactory)
        {
            System.ThrowHelper.ThrowIfNull(serviceType, "serviceType");
            System.ThrowHelper.ThrowIfNull(implementationFactory, "implementationFactory");
            return Describe(serviceType, implementationFactory, ServiceLifetime.Singleton);
        }

        /// <summary>
        /// Creates an instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" /> with the specified
        /// <paramref name="serviceType" />, <paramref name="implementationFactory" />,
        /// and the <see cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton" /> lifetime.
        /// </summary>
        /// <param name="serviceType">The type of the service.</param>
        /// <param name="serviceKey">The <see cref="P:Microsoft.Extensions.DependencyInjection.ServiceDescriptor.ServiceKey" /> of the service.</param>
        /// <param name="implementationFactory">A factory to create new instances of the service implementation.</param>
        /// <returns>A new instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" />.</returns>
        public static ServiceDescriptor KeyedSingleton(Type serviceType, object serviceKey, Func<IServiceProvider, object, object> implementationFactory)
        {
            System.ThrowHelper.ThrowIfNull(serviceType, "serviceType");
            System.ThrowHelper.ThrowIfNull(implementationFactory, "implementationFactory");
            return DescribeKeyed(serviceType, serviceKey, implementationFactory, ServiceLifetime.Singleton);
        }

        /// <summary>
        /// Creates an instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" /> with the specified
        /// <typeparamref name="TService" />, <paramref name="implementationInstance" />,
        /// and the <see cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton" /> lifetime.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="implementationInstance">The instance of the implementation.</param>
        /// <returns>A new instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" />.</returns>
        public static ServiceDescriptor Singleton<TService>(TService implementationInstance) where TService : class
        {
            System.ThrowHelper.ThrowIfNull(implementationInstance, "implementationInstance");
            return Singleton(typeof(TService), implementationInstance);
        }

        /// <summary>
        /// Creates an instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" /> with the specified
        /// <typeparamref name="TService" />, <paramref name="implementationInstance" />,
        /// and the <see cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton" /> lifetime.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="serviceKey">The <see cref="P:Microsoft.Extensions.DependencyInjection.ServiceDescriptor.ServiceKey" /> of the service.</param>
        /// <param name="implementationInstance">The instance of the implementation.</param>
        /// <returns>A new instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" />.</returns>
        public static ServiceDescriptor KeyedSingleton<TService>(object serviceKey, TService implementationInstance) where TService : class
        {
            System.ThrowHelper.ThrowIfNull(implementationInstance, "implementationInstance");
            return KeyedSingleton(typeof(TService), serviceKey, implementationInstance);
        }

        /// <summary>
        /// Creates an instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" /> with the specified
        /// <paramref name="serviceType" />, <paramref name="implementationInstance" />,
        /// and the <see cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton" /> lifetime.
        /// </summary>
        /// <param name="serviceType">The type of the service.</param>
        /// <param name="implementationInstance">The instance of the implementation.</param>
        /// <returns>A new instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" />.</returns>
        public static ServiceDescriptor Singleton(Type serviceType, object implementationInstance)
        {
            System.ThrowHelper.ThrowIfNull(serviceType, "serviceType");
            System.ThrowHelper.ThrowIfNull(implementationInstance, "implementationInstance");
            return new ServiceDescriptor(serviceType, implementationInstance);
        }

        /// <summary>
        /// Creates an instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" /> with the specified
        /// <paramref name="serviceType" />, <paramref name="implementationInstance" />,
        /// and the <see cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton" /> lifetime.
        /// </summary>
        /// <param name="serviceType">The type of the service.</param>
        /// <param name="serviceKey">The <see cref="P:Microsoft.Extensions.DependencyInjection.ServiceDescriptor.ServiceKey" /> of the service.</param>
        /// <param name="implementationInstance">The instance of the implementation.</param>
        /// <returns>A new instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" />.</returns>
        public static ServiceDescriptor KeyedSingleton(Type serviceType, object serviceKey, object implementationInstance)
        {
            System.ThrowHelper.ThrowIfNull(serviceType, "serviceType");
            System.ThrowHelper.ThrowIfNull(implementationInstance, "implementationInstance");
            return new ServiceDescriptor(serviceType, serviceKey, implementationInstance);
        }

        private static ServiceDescriptor DescribeKeyed<TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation>(object serviceKey, ServiceLifetime lifetime) where TService : class where TImplementation : class, TService
        {
            return DescribeKeyed(typeof(TService), serviceKey, typeof(TImplementation), lifetime);
        }

        /// <summary>
        /// Creates an instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" /> with the specified
        /// <paramref name="serviceType" />, <paramref name="implementationType" />,
        /// and <paramref name="lifetime" />.
        /// </summary>
        /// <param name="serviceType">The type of the service.</param>
        /// <param name="implementationType">The type of the implementation.</param>
        /// <param name="lifetime">The lifetime of the service.</param>
        /// <returns>A new instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" />.</returns>
        public static ServiceDescriptor Describe(Type serviceType, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type implementationType, ServiceLifetime lifetime)
        {
            return new ServiceDescriptor(serviceType, implementationType, lifetime);
        }

        /// <summary>
        /// Creates an instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" /> with the specified
        /// <paramref name="serviceType" />, <paramref name="implementationType" />,
        /// and <paramref name="lifetime" />.
        /// </summary>
        /// <param name="serviceType">The type of the service.</param>
        /// <param name="serviceKey">The <see cref="P:Microsoft.Extensions.DependencyInjection.ServiceDescriptor.ServiceKey" /> of the service.</param>
        /// <param name="implementationType">The type of the implementation.</param>
        /// <param name="lifetime">The lifetime of the service.</param>
        /// <returns>A new instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" />.</returns>
        public static ServiceDescriptor DescribeKeyed(Type serviceType, object serviceKey, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type implementationType, ServiceLifetime lifetime)
        {
            return new ServiceDescriptor(serviceType, serviceKey, implementationType, lifetime);
        }

        /// <summary>
        /// Creates an instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" /> with the specified
        /// <paramref name="serviceType" />, <paramref name="implementationFactory" />,
        /// and <paramref name="lifetime" />.
        /// </summary>
        /// <param name="serviceType">The type of the service.</param>
        /// <param name="implementationFactory">A factory to create new instances of the service implementation.</param>
        /// <param name="lifetime">The lifetime of the service.</param>
        /// <returns>A new instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" />.</returns>
        public static ServiceDescriptor Describe(Type serviceType, Func<IServiceProvider, object> implementationFactory, ServiceLifetime lifetime)
        {
            return new ServiceDescriptor(serviceType, implementationFactory, lifetime);
        }

        /// <summary>
        /// Creates an instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" /> with the specified
        /// <paramref name="serviceType" />, <paramref name="implementationFactory" />,
        /// and <paramref name="lifetime" />.
        /// </summary>
        /// <param name="serviceType">The type of the service.</param>
        /// <param name="serviceKey">The <see cref="P:Microsoft.Extensions.DependencyInjection.ServiceDescriptor.ServiceKey" /> of the service.</param>
        /// <param name="implementationFactory">A factory to create new instances of the service implementation.</param>
        /// <param name="lifetime">The lifetime of the service.</param>
        /// <returns>A new instance of <see cref="T:Microsoft.Extensions.DependencyInjection.ServiceDescriptor" />.</returns>
        public static ServiceDescriptor DescribeKeyed(Type serviceType, object serviceKey, Func<IServiceProvider, object, object> implementationFactory, ServiceLifetime lifetime)
        {
            return new ServiceDescriptor(serviceType, serviceKey, implementationFactory, lifetime);
        }

        private string DebuggerToString()
        {
            string text = $"Lifetime = {Lifetime}, ServiceType = \"{ServiceType.FullName}\"";
            if (IsKeyedService)
            {
                text += $", ServiceKey = \"{ServiceKey}\"";
                if (KeyedImplementationType != null)
                {
                    return text + ", KeyedImplementationType = \"" + KeyedImplementationType.FullName + "\"";
                }
                if (KeyedImplementationFactory != null)
                {
                    return text + $", KeyedImplementationFactory = {KeyedImplementationFactory.Method}";
                }
                return text + $", KeyedImplementationInstance = {KeyedImplementationInstance}";
            }
            if (ImplementationType != null)
            {
                return text + ", ImplementationType = \"" + ImplementationType.FullName + "\"";
            }
            if (ImplementationFactory != null)
            {
                return text + $", ImplementationFactory = {ImplementationFactory.Method}";
            }
            return text + $", ImplementationInstance = {ImplementationInstance}";
        }

        private static void ThrowNonKeyedDescriptor()
        {
            throw new InvalidOperationException(System.SR.NonKeyedDescriptorMisuse);
        }
    }
}
