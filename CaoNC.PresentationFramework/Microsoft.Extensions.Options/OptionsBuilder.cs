using CaoNC.Microsoft.Extensions.DependencyInjection;
using System;

namespace CaoNC.Microsoft.Extensions.Options
{
    public class OptionsBuilder<TOptions> where TOptions : class
    {
        private const string DefaultValidationFailureMessage = "A validation error has occurred.";

        /// <summary>
        /// The default name of the <typeparamref name="TOptions" /> instance.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> for the options being configured.
        /// </summary>
        public IServiceCollection Services { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> for the options being configured.</param>
        /// <param name="name">The default name of the <typeparamref name="TOptions" /> instance, if null <see cref="F:Microsoft.Extensions.Options.Options.DefaultName" /> is used.</param>
        public OptionsBuilder(IServiceCollection services, string name)
        {
            System.ThrowHelper.ThrowIfNull(services, "services");
            Services = services;
            Name = name ?? Options.DefaultName;
        }

        /// <summary>
        /// Registers an action used to configure a particular type of options.
        /// Note: These are run before all <seealso cref="M:Microsoft.Extensions.Options.OptionsBuilder`1.PostConfigure(System.Action{`0})" />.
        /// </summary>
        /// <param name="configureOptions">The action used to configure the options.</param>
        /// <returns>The current <see cref="T:Microsoft.Extensions.Options.OptionsBuilder`1" />.</returns>
        public virtual OptionsBuilder<TOptions> Configure(Action<TOptions> configureOptions)
        {
            System.ThrowHelper.ThrowIfNull(configureOptions, "configureOptions");
            Services.AddSingleton((IConfigureOptions<TOptions>)new ConfigureNamedOptions<TOptions>(Name, configureOptions));
            return this;
        }

        /// <summary>
        /// Registers an action used to configure a particular type of options.
        /// Note: These are run before all <seealso cref="M:Microsoft.Extensions.Options.OptionsBuilder`1.PostConfigure(System.Action{`0})" />.
        /// </summary>
        /// <typeparam name="TDep">A dependency used by the action.</typeparam>
        /// <param name="configureOptions">The action used to configure the options.</param>
        /// <returns>The current <see cref="T:Microsoft.Extensions.Options.OptionsBuilder`1" />.</returns>
        public virtual OptionsBuilder<TOptions> Configure<TDep>(Action<TOptions, TDep> configureOptions) where TDep : class
        {
            Action<TOptions, TDep> configureOptions2 = configureOptions;
            System.ThrowHelper.ThrowIfNull(configureOptions2, "configureOptions");
            Services.AddTransient((Func<IServiceProvider, IConfigureOptions<TOptions>>)((IServiceProvider sp) => new ConfigureNamedOptions<TOptions, TDep>(Name, sp.GetRequiredService<TDep>(), configureOptions2)));
            return this;
        }

        /// <summary>
        /// Registers an action used to configure a particular type of options.
        /// Note: These are run before all <seealso cref="M:Microsoft.Extensions.Options.OptionsBuilder`1.PostConfigure(System.Action{`0})" />.
        /// </summary>
        /// <typeparam name="TDep1">The first dependency used by the action.</typeparam>
        /// <typeparam name="TDep2">The second dependency used by the action.</typeparam>
        /// <param name="configureOptions">The action used to configure the options.</param>
        /// <returns>The current <see cref="T:Microsoft.Extensions.Options.OptionsBuilder`1" />.</returns>
        public virtual OptionsBuilder<TOptions> Configure<TDep1, TDep2>(Action<TOptions, TDep1, TDep2> configureOptions) where TDep1 : class where TDep2 : class
        {
            Action<TOptions, TDep1, TDep2> configureOptions2 = configureOptions;
            System.ThrowHelper.ThrowIfNull(configureOptions2, "configureOptions");
            Services.AddTransient((Func<IServiceProvider, IConfigureOptions<TOptions>>)((IServiceProvider sp) => new ConfigureNamedOptions<TOptions, TDep1, TDep2>(Name, sp.GetRequiredService<TDep1>(), sp.GetRequiredService<TDep2>(), configureOptions2)));
            return this;
        }

        /// <summary>
        /// Registers an action used to configure a particular type of options.
        /// Note: These are run before all <seealso cref="M:Microsoft.Extensions.Options.OptionsBuilder`1.PostConfigure(System.Action{`0})" />.
        /// </summary>
        /// <typeparam name="TDep1">The first dependency used by the action.</typeparam>
        /// <typeparam name="TDep2">The second dependency used by the action.</typeparam>
        /// <typeparam name="TDep3">The third dependency used by the action.</typeparam>
        /// <param name="configureOptions">The action used to configure the options.</param>
        /// <returns>The current <see cref="T:Microsoft.Extensions.Options.OptionsBuilder`1" />.</returns>
        public virtual OptionsBuilder<TOptions> Configure<TDep1, TDep2, TDep3>(Action<TOptions, TDep1, TDep2, TDep3> configureOptions) where TDep1 : class where TDep2 : class where TDep3 : class
        {
            Action<TOptions, TDep1, TDep2, TDep3> configureOptions2 = configureOptions;
            System.ThrowHelper.ThrowIfNull(configureOptions2, "configureOptions");
            Services.AddTransient((Func<IServiceProvider, IConfigureOptions<TOptions>>)((IServiceProvider sp) => new ConfigureNamedOptions<TOptions, TDep1, TDep2, TDep3>(Name, sp.GetRequiredService<TDep1>(), sp.GetRequiredService<TDep2>(), sp.GetRequiredService<TDep3>(), configureOptions2)));
            return this;
        }

        /// <summary>
        /// Registers an action used to configure a particular type of options.
        /// Note: These are run before all <seealso cref="M:Microsoft.Extensions.Options.OptionsBuilder`1.PostConfigure(System.Action{`0})" />.
        /// </summary>
        /// <typeparam name="TDep1">The first dependency used by the action.</typeparam>
        /// <typeparam name="TDep2">The second dependency used by the action.</typeparam>
        /// <typeparam name="TDep3">The third dependency used by the action.</typeparam>
        /// <typeparam name="TDep4">The fourth dependency used by the action.</typeparam>
        /// <param name="configureOptions">The action used to configure the options.</param>
        /// <returns>The current <see cref="T:Microsoft.Extensions.Options.OptionsBuilder`1" />.</returns>
        public virtual OptionsBuilder<TOptions> Configure<TDep1, TDep2, TDep3, TDep4>(Action<TOptions, TDep1, TDep2, TDep3, TDep4> configureOptions) where TDep1 : class where TDep2 : class where TDep3 : class where TDep4 : class
        {
            Action<TOptions, TDep1, TDep2, TDep3, TDep4> configureOptions2 = configureOptions;
            System.ThrowHelper.ThrowIfNull(configureOptions2, "configureOptions");
            Services.AddTransient((Func<IServiceProvider, IConfigureOptions<TOptions>>)((IServiceProvider sp) => new ConfigureNamedOptions<TOptions, TDep1, TDep2, TDep3, TDep4>(Name, sp.GetRequiredService<TDep1>(), sp.GetRequiredService<TDep2>(), sp.GetRequiredService<TDep3>(), sp.GetRequiredService<TDep4>(), configureOptions2)));
            return this;
        }

        /// <summary>
        /// Registers an action used to configure a particular type of options.
        /// Note: These are run before all <seealso cref="M:Microsoft.Extensions.Options.OptionsBuilder`1.PostConfigure(System.Action{`0})" />.
        /// </summary>
        /// <typeparam name="TDep1">The first dependency used by the action.</typeparam>
        /// <typeparam name="TDep2">The second dependency used by the action.</typeparam>
        /// <typeparam name="TDep3">The third dependency used by the action.</typeparam>
        /// <typeparam name="TDep4">The fourth dependency used by the action.</typeparam>
        /// <typeparam name="TDep5">The fifth dependency used by the action.</typeparam>
        /// <param name="configureOptions">The action used to configure the options.</param>
        /// <returns>The current <see cref="T:Microsoft.Extensions.Options.OptionsBuilder`1" />.</returns>
        public virtual OptionsBuilder<TOptions> Configure<TDep1, TDep2, TDep3, TDep4, TDep5>(Action<TOptions, TDep1, TDep2, TDep3, TDep4, TDep5> configureOptions) where TDep1 : class where TDep2 : class where TDep3 : class where TDep4 : class where TDep5 : class
        {
            Action<TOptions, TDep1, TDep2, TDep3, TDep4, TDep5> configureOptions2 = configureOptions;
            System.ThrowHelper.ThrowIfNull(configureOptions2, "configureOptions");
            Services.AddTransient((Func<IServiceProvider, IConfigureOptions<TOptions>>)((IServiceProvider sp) => new ConfigureNamedOptions<TOptions, TDep1, TDep2, TDep3, TDep4, TDep5>(Name, sp.GetRequiredService<TDep1>(), sp.GetRequiredService<TDep2>(), sp.GetRequiredService<TDep3>(), sp.GetRequiredService<TDep4>(), sp.GetRequiredService<TDep5>(), configureOptions2)));
            return this;
        }

        /// <summary>
        /// Registers an action used to configure a particular type of options.
        /// Note: These are run after all <seealso cref="M:Microsoft.Extensions.Options.OptionsBuilder`1.Configure(System.Action{`0})" />.
        /// </summary>
        /// <param name="configureOptions">The action used to configure the options.</param>
        /// <returns>The current <see cref="T:Microsoft.Extensions.Options.OptionsBuilder`1" />.</returns>
        public virtual OptionsBuilder<TOptions> PostConfigure(Action<TOptions> configureOptions)
        {
            System.ThrowHelper.ThrowIfNull(configureOptions, "configureOptions");
            Services.AddSingleton((IPostConfigureOptions<TOptions>)new PostConfigureOptions<TOptions>(Name, configureOptions));
            return this;
        }

        /// <summary>
        /// Registers an action used to post configure a particular type of options.
        /// Note: These are run after all <seealso cref="M:Microsoft.Extensions.Options.OptionsBuilder`1.Configure(System.Action{`0})" />.
        /// </summary>
        /// <typeparam name="TDep">The dependency used by the action.</typeparam>
        /// <param name="configureOptions">The action used to configure the options.</param>
        /// <returns>The current <see cref="T:Microsoft.Extensions.Options.OptionsBuilder`1" />.</returns>
        public virtual OptionsBuilder<TOptions> PostConfigure<TDep>(Action<TOptions, TDep> configureOptions) where TDep : class
        {
            Action<TOptions, TDep> configureOptions2 = configureOptions;
            System.ThrowHelper.ThrowIfNull(configureOptions2, "configureOptions");
            Services.AddTransient((Func<IServiceProvider, IPostConfigureOptions<TOptions>>)((IServiceProvider sp) => new PostConfigureOptions<TOptions, TDep>(Name, sp.GetRequiredService<TDep>(), configureOptions2)));
            return this;
        }

        /// <summary>
        /// Registers an action used to post configure a particular type of options.
        /// Note: These are run after all <seealso cref="M:Microsoft.Extensions.Options.OptionsBuilder`1.Configure(System.Action{`0})" />.
        /// </summary>
        /// <typeparam name="TDep1">The first dependency used by the action.</typeparam>
        /// <typeparam name="TDep2">The second dependency used by the action.</typeparam>
        /// <param name="configureOptions">The action used to configure the options.</param>
        /// <returns>The current <see cref="T:Microsoft.Extensions.Options.OptionsBuilder`1" />.</returns>
        public virtual OptionsBuilder<TOptions> PostConfigure<TDep1, TDep2>(Action<TOptions, TDep1, TDep2> configureOptions) where TDep1 : class where TDep2 : class
        {
            Action<TOptions, TDep1, TDep2> configureOptions2 = configureOptions;
            System.ThrowHelper.ThrowIfNull(configureOptions2, "configureOptions");
            Services.AddTransient((Func<IServiceProvider, IPostConfigureOptions<TOptions>>)((IServiceProvider sp) => new PostConfigureOptions<TOptions, TDep1, TDep2>(Name, sp.GetRequiredService<TDep1>(), sp.GetRequiredService<TDep2>(), configureOptions2)));
            return this;
        }

        /// <summary>
        /// Registers an action used to post configure a particular type of options.
        /// Note: These are run after all <seealso cref="M:Microsoft.Extensions.Options.OptionsBuilder`1.Configure(System.Action{`0})" />.
        /// </summary>
        /// <typeparam name="TDep1">The first dependency used by the action.</typeparam>
        /// <typeparam name="TDep2">The second dependency used by the action.</typeparam>
        /// <typeparam name="TDep3">The third dependency used by the action.</typeparam>
        /// <param name="configureOptions">The action used to configure the options.</param>
        /// <returns>The current <see cref="T:Microsoft.Extensions.Options.OptionsBuilder`1" />.</returns>
        public virtual OptionsBuilder<TOptions> PostConfigure<TDep1, TDep2, TDep3>(Action<TOptions, TDep1, TDep2, TDep3> configureOptions) where TDep1 : class where TDep2 : class where TDep3 : class
        {
            Action<TOptions, TDep1, TDep2, TDep3> configureOptions2 = configureOptions;
            System.ThrowHelper.ThrowIfNull(configureOptions2, "configureOptions");
            Services.AddTransient((Func<IServiceProvider, IPostConfigureOptions<TOptions>>)((IServiceProvider sp) => new PostConfigureOptions<TOptions, TDep1, TDep2, TDep3>(Name, sp.GetRequiredService<TDep1>(), sp.GetRequiredService<TDep2>(), sp.GetRequiredService<TDep3>(), configureOptions2)));
            return this;
        }

        /// <summary>
        /// Registers an action used to post configure a particular type of options.
        /// Note: These are run after all <seealso cref="M:Microsoft.Extensions.Options.OptionsBuilder`1.Configure(System.Action{`0})" />.
        /// </summary>
        /// <typeparam name="TDep1">The first dependency used by the action.</typeparam>
        /// <typeparam name="TDep2">The second dependency used by the action.</typeparam>
        /// <typeparam name="TDep3">The third dependency used by the action.</typeparam>
        /// <typeparam name="TDep4">The fourth dependency used by the action.</typeparam>
        /// <param name="configureOptions">The action used to configure the options.</param>
        /// <returns>The current <see cref="T:Microsoft.Extensions.Options.OptionsBuilder`1" />.</returns>
        public virtual OptionsBuilder<TOptions> PostConfigure<TDep1, TDep2, TDep3, TDep4>(Action<TOptions, TDep1, TDep2, TDep3, TDep4> configureOptions) where TDep1 : class where TDep2 : class where TDep3 : class where TDep4 : class
        {
            Action<TOptions, TDep1, TDep2, TDep3, TDep4> configureOptions2 = configureOptions;
            System.ThrowHelper.ThrowIfNull(configureOptions2, "configureOptions");
            Services.AddTransient((Func<IServiceProvider, IPostConfigureOptions<TOptions>>)((IServiceProvider sp) => new PostConfigureOptions<TOptions, TDep1, TDep2, TDep3, TDep4>(Name, sp.GetRequiredService<TDep1>(), sp.GetRequiredService<TDep2>(), sp.GetRequiredService<TDep3>(), sp.GetRequiredService<TDep4>(), configureOptions2)));
            return this;
        }

        /// <summary>
        /// Registers an action used to post configure a particular type of options.
        /// Note: These are run after all <seealso cref="M:Microsoft.Extensions.Options.OptionsBuilder`1.Configure(System.Action{`0})" />.
        /// </summary>
        /// <typeparam name="TDep1">The first dependency used by the action.</typeparam>
        /// <typeparam name="TDep2">The second dependency used by the action.</typeparam>
        /// <typeparam name="TDep3">The third dependency used by the action.</typeparam>
        /// <typeparam name="TDep4">The fourth dependency used by the action.</typeparam>
        /// <typeparam name="TDep5">The fifth dependency used by the action.</typeparam>
        /// <param name="configureOptions">The action used to configure the options.</param>
        /// <returns>The current <see cref="T:Microsoft.Extensions.Options.OptionsBuilder`1" />.</returns>
        public virtual OptionsBuilder<TOptions> PostConfigure<TDep1, TDep2, TDep3, TDep4, TDep5>(Action<TOptions, TDep1, TDep2, TDep3, TDep4, TDep5> configureOptions) where TDep1 : class where TDep2 : class where TDep3 : class where TDep4 : class where TDep5 : class
        {
            Action<TOptions, TDep1, TDep2, TDep3, TDep4, TDep5> configureOptions2 = configureOptions;
            System.ThrowHelper.ThrowIfNull(configureOptions2, "configureOptions");
            Services.AddTransient((Func<IServiceProvider, IPostConfigureOptions<TOptions>>)((IServiceProvider sp) => new PostConfigureOptions<TOptions, TDep1, TDep2, TDep3, TDep4, TDep5>(Name, sp.GetRequiredService<TDep1>(), sp.GetRequiredService<TDep2>(), sp.GetRequiredService<TDep3>(), sp.GetRequiredService<TDep4>(), sp.GetRequiredService<TDep5>(), configureOptions2)));
            return this;
        }

        /// <summary>
        /// Register a validation action for an options type using a default failure message.
        /// </summary>
        /// <param name="validation">The validation function.</param>
        /// <returns>The current <see cref="T:Microsoft.Extensions.Options.OptionsBuilder`1" />.</returns>
        public virtual OptionsBuilder<TOptions> Validate(Func<TOptions, bool> validation)
        {
            return Validate(validation, "A validation error has occurred.");
        }

        /// <summary>
        /// Register a validation action for an options type.
        /// </summary>
        /// <param name="validation">The validation function.</param>
        /// <param name="failureMessage">The failure message to use when validation fails.</param>
        /// <returns>The current <see cref="T:Microsoft.Extensions.Options.OptionsBuilder`1" />.</returns>
        public virtual OptionsBuilder<TOptions> Validate(Func<TOptions, bool> validation, string failureMessage)
        {
            System.ThrowHelper.ThrowIfNull(validation, "validation");
            Services.AddSingleton((IValidateOptions<TOptions>)new ValidateOptions<TOptions>(Name, validation, failureMessage));
            return this;
        }

        /// <summary>
        /// Register a validation action for an options type using a default failure message.
        /// </summary>
        /// <typeparam name="TDep">The dependency used by the validation function.</typeparam>
        /// <param name="validation">The validation function.</param>
        /// <returns>The current <see cref="T:Microsoft.Extensions.Options.OptionsBuilder`1" />.</returns>
        public virtual OptionsBuilder<TOptions> Validate<TDep>(Func<TOptions, TDep, bool> validation) 
        {
            return Validate(validation, "A validation error has occurred.");
        }

        /// <summary>
        /// Register a validation action for an options type.
        /// </summary>
        /// <typeparam name="TDep">The dependency used by the validation function.</typeparam>
        /// <param name="validation">The validation function.</param>
        /// <param name="failureMessage">The failure message to use when validation fails.</param>
        /// <returns>The current <see cref="T:Microsoft.Extensions.Options.OptionsBuilder`1" />.</returns>
        public virtual OptionsBuilder<TOptions> Validate<TDep>(Func<TOptions, TDep, bool> validation, string failureMessage)
        {
            Func<TOptions, TDep, bool> validation2 = validation;
            string failureMessage2 = failureMessage;
            System.ThrowHelper.ThrowIfNull(validation2, "validation");
            Services.AddTransient((Func<IServiceProvider, IValidateOptions<TOptions>>)((IServiceProvider sp) => new ValidateOptions<TOptions, TDep>(Name, sp.GetRequiredService<TDep>(), validation2, failureMessage2)));
            return this;
        }

        /// <summary>
        /// Register a validation action for an options type using a default failure message.
        /// </summary>
        /// <typeparam name="TDep1">The first dependency used by the validation function.</typeparam>
        /// <typeparam name="TDep2">The second dependency used by the validation function.</typeparam>
        /// <param name="validation">The validation function.</param>
        /// <returns>The current <see cref="T:Microsoft.Extensions.Options.OptionsBuilder`1" />.</returns>
        public virtual OptionsBuilder<TOptions> Validate<TDep1, TDep2>(Func<TOptions, TDep1, TDep2, bool> validation)
        {
            return Validate(validation, "A validation error has occurred.");
        }

        /// <summary>
        /// Register a validation action for an options type.
        /// </summary>
        /// <typeparam name="TDep1">The first dependency used by the validation function.</typeparam>
        /// <typeparam name="TDep2">The second dependency used by the validation function.</typeparam>
        /// <param name="validation">The validation function.</param>
        /// <param name="failureMessage">The failure message to use when validation fails.</param>
        /// <returns>The current <see cref="T:Microsoft.Extensions.Options.OptionsBuilder`1" />.</returns>
        public virtual OptionsBuilder<TOptions> Validate<TDep1, TDep2>(Func<TOptions, TDep1, TDep2, bool> validation, string failureMessage)
        {
            Func<TOptions, TDep1, TDep2, bool> validation2 = validation;
            string failureMessage2 = failureMessage;
            System.ThrowHelper.ThrowIfNull(validation2, "validation");
            Services.AddTransient((Func<IServiceProvider, IValidateOptions<TOptions>>)((IServiceProvider sp) => new ValidateOptions<TOptions, TDep1, TDep2>(Name, sp.GetRequiredService<TDep1>(), sp.GetRequiredService<TDep2>(), validation2, failureMessage2)));
            return this;
        }

        /// <summary>
        /// Register a validation action for an options type using a default failure message.
        /// </summary>
        /// <typeparam name="TDep1">The first dependency used by the validation function.</typeparam>
        /// <typeparam name="TDep2">The second dependency used by the validation function.</typeparam>
        /// <typeparam name="TDep3">The third dependency used by the validation function.</typeparam>
        /// <param name="validation">The validation function.</param>
        /// <returns>The current <see cref="T:Microsoft.Extensions.Options.OptionsBuilder`1" />.</returns>
        public virtual OptionsBuilder<TOptions> Validate<TDep1, TDep2, TDep3>(Func<TOptions, TDep1, TDep2, TDep3, bool> validation) 
        {
            return Validate(validation, "A validation error has occurred.");
        }

        /// <summary>
        /// Register a validation action for an options type.
        /// </summary>
        /// <typeparam name="TDep1">The first dependency used by the validation function.</typeparam>
        /// <typeparam name="TDep2">The second dependency used by the validation function.</typeparam>
        /// <typeparam name="TDep3">The third dependency used by the validation function.</typeparam>
        /// <param name="validation">The validation function.</param>
        /// <param name="failureMessage">The failure message to use when validation fails.</param>
        /// <returns>The current <see cref="T:Microsoft.Extensions.Options.OptionsBuilder`1" />.</returns>
        public virtual OptionsBuilder<TOptions> Validate<TDep1, TDep2, TDep3>(Func<TOptions, TDep1, TDep2, TDep3, bool> validation, string failureMessage)
        {
            Func<TOptions, TDep1, TDep2, TDep3, bool> validation2 = validation;
            string failureMessage2 = failureMessage;
            System.ThrowHelper.ThrowIfNull(validation2, "validation");
            Services.AddTransient((Func<IServiceProvider, IValidateOptions<TOptions>>)((IServiceProvider sp) => new ValidateOptions<TOptions, TDep1, TDep2, TDep3>(Name, sp.GetRequiredService<TDep1>(), sp.GetRequiredService<TDep2>(), sp.GetRequiredService<TDep3>(), validation2, failureMessage2)));
            return this;
        }

        /// <summary>
        /// Register a validation action for an options type using a default failure message.
        /// </summary>
        /// <typeparam name="TDep1">The first dependency used by the validation function.</typeparam>
        /// <typeparam name="TDep2">The second dependency used by the validation function.</typeparam>
        /// <typeparam name="TDep3">The third dependency used by the validation function.</typeparam>
        /// <typeparam name="TDep4">The fourth dependency used by the validation function.</typeparam>
        /// <param name="validation">The validation function.</param>
        /// <returns>The current <see cref="T:Microsoft.Extensions.Options.OptionsBuilder`1" />.</returns>
        public virtual OptionsBuilder<TOptions> Validate<TDep1, TDep2, TDep3, TDep4>(Func<TOptions, TDep1, TDep2, TDep3, TDep4, bool> validation) 
        {
            return Validate(validation, "A validation error has occurred.");
        }

        /// <summary>
        /// Register a validation action for an options type.
        /// </summary>
        /// <typeparam name="TDep1">The first dependency used by the validation function.</typeparam>
        /// <typeparam name="TDep2">The second dependency used by the validation function.</typeparam>
        /// <typeparam name="TDep3">The third dependency used by the validation function.</typeparam>
        /// <typeparam name="TDep4">The fourth dependency used by the validation function.</typeparam>
        /// <param name="validation">The validation function.</param>
        /// <param name="failureMessage">The failure message to use when validation fails.</param>
        /// <returns>The current <see cref="T:Microsoft.Extensions.Options.OptionsBuilder`1" />.</returns>
        public virtual OptionsBuilder<TOptions> Validate<TDep1, TDep2, TDep3, TDep4>(Func<TOptions, TDep1, TDep2, TDep3, TDep4, bool> validation, string failureMessage) 
        {
            Func<TOptions, TDep1, TDep2, TDep3, TDep4, bool> validation2 = validation;
            string failureMessage2 = failureMessage;
            System.ThrowHelper.ThrowIfNull(validation2, "validation");
            Services.AddTransient((Func<IServiceProvider, IValidateOptions<TOptions>>)((IServiceProvider sp) => new ValidateOptions<TOptions, TDep1, TDep2, TDep3, TDep4>(Name, sp.GetRequiredService<TDep1>(), sp.GetRequiredService<TDep2>(), sp.GetRequiredService<TDep3>(), sp.GetRequiredService<TDep4>(), validation2, failureMessage2)));
            return this;
        }

        /// <summary>
        /// Register a validation action for an options type using a default failure message.
        /// </summary>
        /// <typeparam name="TDep1">The first dependency used by the validation function.</typeparam>
        /// <typeparam name="TDep2">The second dependency used by the validation function.</typeparam>
        /// <typeparam name="TDep3">The third dependency used by the validation function.</typeparam>
        /// <typeparam name="TDep4">The fourth dependency used by the validation function.</typeparam>
        /// <typeparam name="TDep5">The fifth dependency used by the validation function.</typeparam>
        /// <param name="validation">The validation function.</param>
        /// <returns>The current <see cref="T:Microsoft.Extensions.Options.OptionsBuilder`1" />.</returns>
        public virtual OptionsBuilder<TOptions> Validate<TDep1, TDep2, TDep3, TDep4, TDep5>(Func<TOptions, TDep1, TDep2, TDep3, TDep4, TDep5, bool> validation) 
        {
            return Validate(validation, "A validation error has occurred.");
        }

        /// <summary>
        /// Register a validation action for an options type.
        /// </summary>
        /// <typeparam name="TDep1">The first dependency used by the validation function.</typeparam>
        /// <typeparam name="TDep2">The second dependency used by the validation function.</typeparam>
        /// <typeparam name="TDep3">The third dependency used by the validation function.</typeparam>
        /// <typeparam name="TDep4">The fourth dependency used by the validation function.</typeparam>
        /// <typeparam name="TDep5">The fifth dependency used by the validation function.</typeparam>
        /// <param name="validation">The validation function.</param>
        /// <param name="failureMessage">The failure message to use when validation fails.</param>
        /// <returns>The current <see cref="T:Microsoft.Extensions.Options.OptionsBuilder`1" />.</returns>
        public virtual OptionsBuilder<TOptions> Validate<TDep1, TDep2, TDep3, TDep4, TDep5>(Func<TOptions, TDep1, TDep2, TDep3, TDep4, TDep5, bool> validation, string failureMessage)
        {
            Func<TOptions, TDep1, TDep2, TDep3, TDep4, TDep5, bool> validation2 = validation;
            string failureMessage2 = failureMessage;
            System.ThrowHelper.ThrowIfNull(validation2, "validation");
            Services.AddTransient((Func<IServiceProvider, IValidateOptions<TOptions>>)((IServiceProvider sp) => new ValidateOptions<TOptions, TDep1, TDep2, TDep3, TDep4, TDep5>(Name, sp.GetRequiredService<TDep1>(), sp.GetRequiredService<TDep2>(), sp.GetRequiredService<TDep3>(), sp.GetRequiredService<TDep4>(), sp.GetRequiredService<TDep5>(), validation2, failureMessage2)));
            return this;
        }
    }
}
