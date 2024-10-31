using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaoNC.Microsoft.Extensions.Options
{
    public class ConfigureNamedOptions<TOptions> : IConfigureNamedOptions<TOptions>, IConfigureOptions<TOptions> where TOptions : class
    {
        /// <summary>
        /// The options name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The configuration action.
        /// </summary>
        public Action<TOptions> Action { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of the options.</param>
        /// <param name="action">The action to register.</param>
        public ConfigureNamedOptions(string name, Action<TOptions> action)
        {
            Name = name;
            Action = action;
        }

        /// <summary>
        /// Invokes the registered configure <see cref="P:Microsoft.Extensions.Options.ConfigureNamedOptions`1.Action" /> if the <paramref name="name" /> matches.
        /// </summary>
        /// <param name="name">The name of the options instance being configured.</param>
        /// <param name="options">The options instance to configure.</param>
        public virtual void Configure(string name, TOptions options)
        {
            System.ThrowHelper.ThrowIfNull(options, "options");
            if (Name == null || name == Name)
            {
                Action?.Invoke(options);
            }
        }

        /// <summary>
        /// Invoked to configure a <typeparamref name="TOptions" /> instance with the <see cref="F:Microsoft.Extensions.Options.Options.DefaultName" />.
        /// </summary>
        /// <param name="options">The options instance to configure.</param>
        public void Configure(TOptions options)
        {
            Configure(Options.DefaultName, options);
        }
    }

    public class ConfigureNamedOptions<TOptions, TDep> : IConfigureNamedOptions<TOptions>, IConfigureOptions<TOptions> where TOptions : class where TDep : class
    {
        /// <summary>
        /// The options name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The configuration action.
        /// </summary>
        public Action<TOptions, TDep> Action { get; }

        /// <summary>
        /// The dependency.
        /// </summary>
        public TDep Dependency { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of the options.</param>
        /// <param name="dependency">A dependency.</param>
        /// <param name="action">The action to register.</param>
        public ConfigureNamedOptions(string name, TDep dependency, Action<TOptions, TDep> action)
        {
            Name = name;
            Action = action;
            Dependency = dependency;
        }

        /// <summary>
        /// Invokes the registered configure <see cref="P:Microsoft.Extensions.Options.ConfigureNamedOptions`2.Action" /> if the <paramref name="name" /> matches.
        /// </summary>
        /// <param name="name">The name of the options instance being configured.</param>
        /// <param name="options">The options instance to configure.</param>
        public virtual void Configure(string name, TOptions options)
        {
            System.ThrowHelper.ThrowIfNull(options, "options");
            if (Name == null || name == Name)
            {
                Action?.Invoke(options, Dependency);
            }
        }

        /// <summary>
        /// Invoked to configure a <typeparamref name="TOptions" /> instance with the <see cref="F:Microsoft.Extensions.Options.Options.DefaultName" />.
        /// </summary>
        /// <param name="options">The options instance to configure.</param>
        public void Configure(TOptions options)
        {
            Configure(Options.DefaultName, options);
        }
    }

    public class ConfigureNamedOptions<TOptions, TDep1, TDep2> : IConfigureNamedOptions<TOptions>, IConfigureOptions<TOptions> where TOptions : class where TDep1 : class where TDep2 : class
    {
        /// <summary>
        /// The options name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The configuration action.
        /// </summary>
        public Action<TOptions, TDep1, TDep2> Action { get; }

        /// <summary>
        /// The first dependency.
        /// </summary>
        public TDep1 Dependency1 { get; }

        /// <summary>
        /// The second dependency.
        /// </summary>
        public TDep2 Dependency2 { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of the options.</param>
        /// <param name="dependency">A dependency.</param>
        /// <param name="dependency2">A second dependency.</param>
        /// <param name="action">The action to register.</param>
        public ConfigureNamedOptions(string name, TDep1 dependency, TDep2 dependency2, Action<TOptions, TDep1, TDep2> action)
        {
            Name = name;
            Action = action;
            Dependency1 = dependency;
            Dependency2 = dependency2;
        }

        /// <summary>
        /// Invokes the registered configure <see cref="P:Microsoft.Extensions.Options.ConfigureNamedOptions`3.Action" /> if the <paramref name="name" /> matches.
        /// </summary>
        /// <param name="name">The name of the options instance being configured.</param>
        /// <param name="options">The options instance to configure.</param>
        public virtual void Configure(string name, TOptions options)
        {
            System.ThrowHelper.ThrowIfNull(options, "options");
            if (Name == null || name == Name)
            {
                Action?.Invoke(options, Dependency1, Dependency2);
            }
        }

        /// <summary>
        /// Invoked to configure a <typeparamref name="TOptions" /> instance with the <see cref="F:Microsoft.Extensions.Options.Options.DefaultName" />.
        /// </summary>
        /// <param name="options">The options instance to configure.</param>
        public void Configure(TOptions options)
        {
            Configure(Options.DefaultName, options);
        }
    }

    public class ConfigureNamedOptions<TOptions, TDep1, TDep2, TDep3> : IConfigureNamedOptions<TOptions>, IConfigureOptions<TOptions> where TOptions : class where TDep1 : class where TDep2 : class where TDep3 : class
    {
        /// <summary>
        /// The options name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The configuration action.
        /// </summary>
        public Action<TOptions, TDep1, TDep2, TDep3> Action { get; }

        /// <summary>
        /// The first dependency.
        /// </summary>
        public TDep1 Dependency1 { get; }

        /// <summary>
        /// The second dependency.
        /// </summary>
        public TDep2 Dependency2 { get; }

        /// <summary>
        /// The third dependency.
        /// </summary>
        public TDep3 Dependency3 { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of the options.</param>
        /// <param name="dependency">A dependency.</param>
        /// <param name="dependency2">A second dependency.</param>
        /// <param name="dependency3">A third dependency.</param>
        /// <param name="action">The action to register.</param>
        public ConfigureNamedOptions(string name, TDep1 dependency, TDep2 dependency2, TDep3 dependency3, Action<TOptions, TDep1, TDep2, TDep3> action)
        {
            Name = name;
            Action = action;
            Dependency1 = dependency;
            Dependency2 = dependency2;
            Dependency3 = dependency3;
        }

        /// <summary>
        /// Invokes the registered configure <see cref="P:Microsoft.Extensions.Options.ConfigureNamedOptions`4.Action" /> if the <paramref name="name" /> matches.
        /// </summary>
        /// <param name="name">The name of the options instance being configured.</param>
        /// <param name="options">The options instance to configure.</param>
        public virtual void Configure(string name, TOptions options)
        {
            System.ThrowHelper.ThrowIfNull(options, "options");
            if (Name == null || name == Name)
            {
                Action?.Invoke(options, Dependency1, Dependency2, Dependency3);
            }
        }

        /// <summary>
        /// Invoked to configure a <typeparamref name="TOptions" /> instance with the <see cref="F:Microsoft.Extensions.Options.Options.DefaultName" />.
        /// </summary>
        /// <param name="options">The options instance to configure.</param>
        public void Configure(TOptions options)
        {
            Configure(Options.DefaultName, options);
        }
    }

    public class ConfigureNamedOptions<TOptions, TDep1, TDep2, TDep3, TDep4> : IConfigureNamedOptions<TOptions>, IConfigureOptions<TOptions> where TOptions : class where TDep1 : class where TDep2 : class where TDep3 : class where TDep4 : class
    {
        /// <summary>
        /// The options name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The configuration action.
        /// </summary>
        public Action<TOptions, TDep1, TDep2, TDep3, TDep4> Action { get; }

        /// <summary>
        /// The first dependency.
        /// </summary>
        public TDep1 Dependency1 { get; }

        /// <summary>
        /// The second dependency.
        /// </summary>
        public TDep2 Dependency2 { get; }

        /// <summary>
        /// The third dependency.
        /// </summary>
        public TDep3 Dependency3 { get; }

        /// <summary>
        /// The fourth dependency.
        /// </summary>
        public TDep4 Dependency4 { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of the options.</param>
        /// <param name="dependency1">A dependency.</param>
        /// <param name="dependency2">A second dependency.</param>
        /// <param name="dependency3">A third dependency.</param>
        /// <param name="dependency4">A fourth dependency.</param>
        /// <param name="action">The action to register.</param>
        public ConfigureNamedOptions(string name, TDep1 dependency1, TDep2 dependency2, TDep3 dependency3, TDep4 dependency4, Action<TOptions, TDep1, TDep2, TDep3, TDep4> action)
        {
            Name = name;
            Action = action;
            Dependency1 = dependency1;
            Dependency2 = dependency2;
            Dependency3 = dependency3;
            Dependency4 = dependency4;
        }

        /// <summary>
        /// Invokes the registered configure <see cref="P:Microsoft.Extensions.Options.ConfigureNamedOptions`5.Action" /> if the <paramref name="name" /> matches.
        /// </summary>
        /// <param name="name">The name of the options instance being configured.</param>
        /// <param name="options">The options instance to configure.</param>
        public virtual void Configure(string name, TOptions options)
        {
            System.ThrowHelper.ThrowIfNull(options, "options");
            if (Name == null || name == Name)
            {
                Action?.Invoke(options, Dependency1, Dependency2, Dependency3, Dependency4);
            }
        }

        /// <summary>
        /// Invoked to configure a <typeparamref name="TOptions" /> instance with the <see cref="F:Microsoft.Extensions.Options.Options.DefaultName" />.
        /// </summary>
        /// <param name="options">The options instance to configure.</param>
        public void Configure(TOptions options)
        {
            Configure(Options.DefaultName, options);
        }
    }

    public class ConfigureNamedOptions<TOptions, TDep1, TDep2, TDep3, TDep4, TDep5> : IConfigureNamedOptions<TOptions>, IConfigureOptions<TOptions> where TOptions : class where TDep1 : class where TDep2 : class where TDep3 : class where TDep4 : class where TDep5 : class
    {
        /// <summary>
        /// The options name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The configuration action.
        /// </summary>
        public Action<TOptions, TDep1, TDep2, TDep3, TDep4, TDep5> Action { get; }

        /// <summary>
        /// The first dependency.
        /// </summary>
        public TDep1 Dependency1 { get; }

        /// <summary>
        /// The second dependency.
        /// </summary>
        public TDep2 Dependency2 { get; }

        /// <summary>
        /// The third dependency.
        /// </summary>
        public TDep3 Dependency3 { get; }

        /// <summary>
        /// The fourth dependency.
        /// </summary>
        public TDep4 Dependency4 { get; }

        /// <summary>
        /// The fifth dependency.
        /// </summary>
        public TDep5 Dependency5 { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of the options.</param>
        /// <param name="dependency1">A dependency.</param>
        /// <param name="dependency2">A second dependency.</param>
        /// <param name="dependency3">A third dependency.</param>
        /// <param name="dependency4">A fourth dependency.</param>
        /// <param name="dependency5">A fifth dependency.</param>
        /// <param name="action">The action to register.</param>
        public ConfigureNamedOptions(string name, TDep1 dependency1, TDep2 dependency2, TDep3 dependency3, TDep4 dependency4, TDep5 dependency5, Action<TOptions, TDep1, TDep2, TDep3, TDep4, TDep5> action)
        {
            Name = name;
            Action = action;
            Dependency1 = dependency1;
            Dependency2 = dependency2;
            Dependency3 = dependency3;
            Dependency4 = dependency4;
            Dependency5 = dependency5;
        }

        /// <summary>
        /// Invokes the registered configure <see cref="P:Microsoft.Extensions.Options.ConfigureNamedOptions`6.Action" /> if the <paramref name="name" /> matches.
        /// </summary>
        /// <param name="name">The name of the options instance being configured.</param>
        /// <param name="options">The options instance to configure.</param>
        public virtual void Configure(string name, TOptions options)
        {
            System.ThrowHelper.ThrowIfNull(options, "options");
            if (Name == null || name == Name)
            {
                Action?.Invoke(options, Dependency1, Dependency2, Dependency3, Dependency4, Dependency5);
            }
        }

        /// <summary>
        /// Invoked to configure a <typeparamref name="TOptions" /> instance with the <see cref="F:Microsoft.Extensions.Options.Options.DefaultName" />.
        /// </summary>
        /// <param name="options">The options instance to configure.</param>
        public void Configure(TOptions options)
        {
            Configure(Options.DefaultName, options);
        }
    }
}
