using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaoNC.Microsoft.Extensions.Options
{
    public interface IConfigureOptions<in TOptions> where TOptions : class
    {
        /// <summary>
        /// Invoked to configure a <typeparamref name="TOptions" /> instance.
        /// </summary>
        /// <param name="options">The options instance to configure.</param>
        void Configure(TOptions options);
    }

    public interface IPostConfigureOptions<in TOptions> where TOptions : class
    {
        /// <summary>
        /// Invoked to configure a <typeparamref name="TOptions" /> instance.
        /// </summary>
        /// <param name="name">The name of the options instance being configured.</param>
        /// <param name="options">The options instance to configured.</param>
        void PostConfigure(string name, TOptions options);
    }

    public interface IValidateOptions<TOptions> where TOptions : class
    {
        /// <summary>
        /// Validates a specific named options instance (or all when name is null).
        /// </summary>
        /// <param name="name">The name of the options instance being validated.</param>
        /// <param name="options">The options instance.</param>
        /// <returns>The <see cref="T:Microsoft.Extensions.Options.ValidateOptionsResult" /> result.</returns>
        ValidateOptionsResult Validate(string name, TOptions options);
    }

}
