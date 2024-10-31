using CaoNC.Microsoft.Extensions.Options;
using System.Collections.Generic;
using System;

namespace CaoNC.Microsoft.Extensions.DependencyInjection
{
    public static class OptionsServiceCollectionExtensions
    {
        /// <summary>
        /// Adds services required for using options.
        /// </summary>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the services to.</param>
        /// <returns>The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> so that additional calls can be chained.</returns>
        public static IServiceCollection AddOptions(this IServiceCollection services)
        {
            System.ThrowHelper.ThrowIfNull(services, "services");
            services.TryAdd(ServiceDescriptor.Singleton(typeof(IOptions<>), typeof(UnnamedOptionsManager<>)));
            services.TryAdd(ServiceDescriptor.Scoped(typeof(IOptionsSnapshot<>), typeof(OptionsManager<>)));
            services.TryAdd(ServiceDescriptor.Singleton(typeof(IOptionsMonitor<>), typeof(OptionsMonitor<>)));
            services.TryAdd(ServiceDescriptor.Transient(typeof(IOptionsFactory<>), typeof(OptionsFactory<>)));
            services.TryAdd(ServiceDescriptor.Singleton(typeof(IOptionsMonitorCache<>), typeof(OptionsCache<>)));
            return services;
        }

        /// <summary>
        /// Adds services required for using options and enforces options validation check on start rather than in runtime.
        /// </summary>
        /// <remarks>
        /// The <seealso cref="M:Microsoft.Extensions.DependencyInjection.OptionsBuilderExtensions.ValidateOnStart``1(Microsoft.Extensions.Options.OptionsBuilder{``0})" /> extension is called by this method.
        /// </remarks>
        /// <typeparam name="TOptions">The options type to be configured.</typeparam>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the services to.</param>
        /// <param name="name">The name of the options instance.</param>
        /// <returns>The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> so that additional calls can be chained.</returns>
        public static OptionsBuilder<TOptions> AddOptionsWithValidateOnStart<[System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.All)] TOptions>(this IServiceCollection services, string name = null) where TOptions : class
        {
            return new OptionsBuilder<TOptions>(services, name ?? CaoNC.Microsoft.Extensions.Options.Options.DefaultName).ValidateOnStart();
        }

        /// <summary>
        /// Adds services required for using options and enforces options validation check on start rather than in runtime.
        /// </summary>
        /// <remarks>
        /// The <seealso cref="M:Microsoft.Extensions.DependencyInjection.OptionsBuilderExtensions.ValidateOnStart``1(Microsoft.Extensions.Options.OptionsBuilder{``0})" /> extension is called by this method.
        /// </remarks>
        /// <typeparam name="TOptions">The options type to be configured.</typeparam>
        /// <typeparam name="TValidateOptions">The <see cref="T:Microsoft.Extensions.Options.IValidateOptions`1" /> validator type.</typeparam>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the services to.</param>
        /// <param name="name">The name of the options instance.</param>
        /// <returns>The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> so that additional calls can be chained.</returns>
        public static OptionsBuilder<TOptions> AddOptionsWithValidateOnStart<[System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.All)] TOptions, [System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicConstructors)] TValidateOptions>(this IServiceCollection services, string name = null) where TOptions : class where TValidateOptions : class, IValidateOptions<TOptions>
        {
            services.AddOptions().TryAddEnumerable(ServiceDescriptor.Singleton<IValidateOptions<TOptions>, TValidateOptions>());
            return new OptionsBuilder<TOptions>(services, name ?? CaoNC.Microsoft.Extensions.Options.Options.DefaultName).ValidateOnStart();
        }

        /// <summary>
        /// Registers an action used to configure a particular type of options.
        /// Note: These are run before all <seealso cref="M:Microsoft.Extensions.DependencyInjection.OptionsServiceCollectionExtensions.PostConfigure``1(Microsoft.Extensions.DependencyInjection.IServiceCollection,System.Action{``0})" />.
        /// </summary>
        /// <typeparam name="TOptions">The options type to be configured.</typeparam>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the services to.</param>
        /// <param name="configureOptions">The action used to configure the options.</param>
        /// <returns>The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> so that additional calls can be chained.</returns>
        public static IServiceCollection Configure<TOptions>(this IServiceCollection services, Action<TOptions> configureOptions) where TOptions : class
        {
            return services.Configure(CaoNC.Microsoft.Extensions.Options.Options.DefaultName, configureOptions);
        }

        /// <summary>
        /// Registers an action used to configure a particular type of options.
        /// Note: These are run before all <seealso cref="M:Microsoft.Extensions.DependencyInjection.OptionsServiceCollectionExtensions.PostConfigure``1(Microsoft.Extensions.DependencyInjection.IServiceCollection,System.Action{``0})" />.
        /// </summary>
        /// <typeparam name="TOptions">The options type to be configured.</typeparam>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the services to.</param>
        /// <param name="name">The name of the options instance.</param>
        /// <param name="configureOptions">The action used to configure the options.</param>
        /// <returns>The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> so that additional calls can be chained.</returns>
        public static IServiceCollection Configure<TOptions>(this IServiceCollection services, string name, Action<TOptions> configureOptions) where TOptions : class
        {
            System.ThrowHelper.ThrowIfNull(services, "services");
            System.ThrowHelper.ThrowIfNull(configureOptions, "configureOptions");
            services.AddOptions();
            services.AddSingleton((IConfigureOptions<TOptions>)new ConfigureNamedOptions<TOptions>(name, configureOptions));
            return services;
        }

        /// <summary>
        /// Registers an action used to configure all instances of a particular type of options.
        /// </summary>
        /// <typeparam name="TOptions">The options type to be configured.</typeparam>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the services to.</param>
        /// <param name="configureOptions">The action used to configure the options.</param>
        /// <returns>The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> so that additional calls can be chained.</returns>
        public static IServiceCollection ConfigureAll<TOptions>(this IServiceCollection services, Action<TOptions> configureOptions) where TOptions : class
        {
            return services.Configure(null, configureOptions);
        }

        /// <summary>
        /// Registers an action used to initialize a particular type of options.
        /// Note: These are run after all <seealso cref="M:Microsoft.Extensions.DependencyInjection.OptionsServiceCollectionExtensions.Configure``1(Microsoft.Extensions.DependencyInjection.IServiceCollection,System.Action{``0})" />.
        /// </summary>
        /// <typeparam name="TOptions">The options type to be configured.</typeparam>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the services to.</param>
        /// <param name="configureOptions">The action used to configure the options.</param>
        /// <returns>The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> so that additional calls can be chained.</returns>
        public static IServiceCollection PostConfigure<TOptions>(this IServiceCollection services, Action<TOptions> configureOptions) where TOptions : class
        {
            return services.PostConfigure(CaoNC.Microsoft.Extensions.Options.Options.DefaultName, configureOptions);
        }

        /// <summary>
        /// Registers an action used to configure a particular type of options.
        /// Note: These are run after all <seealso cref="M:Microsoft.Extensions.DependencyInjection.OptionsServiceCollectionExtensions.Configure``1(Microsoft.Extensions.DependencyInjection.IServiceCollection,System.Action{``0})" />.
        /// </summary>
        /// <typeparam name="TOptions">The options type to be configure.</typeparam>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the services to.</param>
        /// <param name="name">The name of the options instance.</param>
        /// <param name="configureOptions">The action used to configure the options.</param>
        /// <returns>The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> so that additional calls can be chained.</returns>
        public static IServiceCollection PostConfigure<TOptions>(this IServiceCollection services, string name, Action<TOptions> configureOptions) where TOptions : class
        {
            System.ThrowHelper.ThrowIfNull(services, "services");
            System.ThrowHelper.ThrowIfNull(configureOptions, "configureOptions");
            services.AddOptions();
            services.AddSingleton((IPostConfigureOptions<TOptions>)new PostConfigureOptions<TOptions>(name, configureOptions));
            return services;
        }

        /// <summary>
        /// Registers an action used to post configure all instances of a particular type of options.
        /// Note: These are run after all <seealso cref="M:Microsoft.Extensions.DependencyInjection.OptionsServiceCollectionExtensions.Configure``1(Microsoft.Extensions.DependencyInjection.IServiceCollection,System.Action{``0})" />.
        /// </summary>
        /// <typeparam name="TOptions">The options type to be configured.</typeparam>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the services to.</param>
        /// <param name="configureOptions">The action used to configure the options.</param>
        /// <returns>The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> so that additional calls can be chained.</returns>
        public static IServiceCollection PostConfigureAll<TOptions>(this IServiceCollection services, Action<TOptions> configureOptions) where TOptions : class
        {
            return services.PostConfigure(null, configureOptions);
        }

        /// <summary>
        /// Registers a type that will have all of its <see cref="T:Microsoft.Extensions.Options.IConfigureOptions`1" />,
        /// <see cref="T:Microsoft.Extensions.Options.IPostConfigureOptions`1" />, and <see cref="T:Microsoft.Extensions.Options.IValidateOptions`1" />
        /// registered.
        /// </summary>
        /// <typeparam name="TConfigureOptions">The type that will configure options.</typeparam>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the services to.</param>
        /// <returns>The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> so that additional calls can be chained.</returns>
        public static IServiceCollection ConfigureOptions<[System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicConstructors)] TConfigureOptions>(this IServiceCollection services) where TConfigureOptions : class
        {
            return services.ConfigureOptions(typeof(TConfigureOptions));
        }

        private static IEnumerable<Type> FindConfigurationServices(Type type)
        {
            Type[] array = GetInterfacesOnType(type);
            foreach (Type type2 in array)
            {
                if (type2.IsGenericType)
                {
                    Type genericTypeDefinition = type2.GetGenericTypeDefinition();
                    if (genericTypeDefinition == typeof(IConfigureOptions<>) || genericTypeDefinition == typeof(IPostConfigureOptions<>) || genericTypeDefinition == typeof(IValidateOptions<>))
                    {
                        yield return type2;
                    }
                }
            }
        }

        static Type[] GetInterfacesOnType(Type t)
        {
            return t.GetInterfaces();
        }

        private static void ThrowNoConfigServices(Type type)
        {
            throw new InvalidOperationException((type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Action<>)) ?
                System.SR.Error_NoConfigurationServicesAndAction : System.SR.Error_NoConfigurationServices);
        }

        /// <summary>
        /// Registers a type that will have all of its <see cref="T:Microsoft.Extensions.Options.IConfigureOptions`1" />,
        /// <see cref="T:Microsoft.Extensions.Options.IPostConfigureOptions`1" />, and <see cref="T:Microsoft.Extensions.Options.IValidateOptions`1" />
        /// registered.
        /// </summary>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the services to.</param>
        /// <param name="configureType">The type that will configure options.</param>
        /// <returns>The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> so that additional calls can be chained.</returns>
        public static IServiceCollection ConfigureOptions(this IServiceCollection services, [System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicConstructors)] Type configureType)
        {
            services.AddOptions();
            bool flag = false;
            foreach (Type item in FindConfigurationServices(configureType))
            {
                services.AddTransient(item, configureType);
                flag = true;
            }
            if (!flag)
            {
                ThrowNoConfigServices(configureType);
            }
            return services;
        }

        /// <summary>
        /// Registers an object that will have all of its <see cref="T:Microsoft.Extensions.Options.IConfigureOptions`1" />,
        /// <see cref="T:Microsoft.Extensions.Options.IPostConfigureOptions`1" />, and <see cref="T:Microsoft.Extensions.Options.IValidateOptions`1" />
        /// registered.
        /// </summary>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the services to.</param>
        /// <param name="configureInstance">The instance that will configure options.</param>
        /// <returns>The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> so that additional calls can be chained.</returns>
        public static IServiceCollection ConfigureOptions(this IServiceCollection services, object configureInstance)
        {
            services.AddOptions();
            Type type = configureInstance.GetType();
            bool flag = false;
            foreach (Type item in FindConfigurationServices(type))
            {
                services.AddSingleton(item, configureInstance);
                flag = true;
            }
            if (!flag)
            {
                ThrowNoConfigServices(type);
            }
            return services;
        }

        /// <summary>
        /// Gets an options builder that forwards Configure calls for the same <typeparamref name="TOptions" /> to the underlying service collection.
        /// </summary>
        /// <typeparam name="TOptions">The options type to be configured.</typeparam>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the services to.</param>
        /// <returns>The <see cref="T:Microsoft.Extensions.Options.OptionsBuilder`1" /> so that configure calls can be chained in it.</returns>
        public static OptionsBuilder<TOptions> AddOptions<TOptions>(this IServiceCollection services) where TOptions : class
        {
            return services.AddOptions<TOptions>(CaoNC.Microsoft.Extensions.Options.Options.DefaultName);
        }

        /// <summary>
        /// Gets an options builder that forwards Configure calls for the same named <typeparamref name="TOptions" /> to the underlying service collection.
        /// </summary>
        /// <typeparam name="TOptions">The options type to be configured.</typeparam>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the services to.</param>
        /// <param name="name">The name of the options instance.</param>
        /// <returns>The <see cref="T:Microsoft.Extensions.Options.OptionsBuilder`1" /> so that configure calls can be chained in it.</returns>
        public static OptionsBuilder<TOptions> AddOptions<TOptions>(this IServiceCollection services, string name) where TOptions : class
        {
            System.ThrowHelper.ThrowIfNull(services, "services");
            services.AddOptions();
            return new OptionsBuilder<TOptions>(services, name);
        }
    }

}
