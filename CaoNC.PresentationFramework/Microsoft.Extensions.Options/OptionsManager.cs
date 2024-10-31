using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaoNC.Microsoft.Extensions.Options
{
    public class OptionsManager<[System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TOptions> : IOptions<TOptions>, IOptionsSnapshot<TOptions> where TOptions : class
    {
        private readonly IOptionsFactory<TOptions> _factory;

        private readonly OptionsCache<TOptions> _cache = new OptionsCache<TOptions>();

        /// <summary>
        /// The default configured <typeparamref name="TOptions" /> instance, equivalent to Get(Options.DefaultName).
        /// </summary>
        public TOptions Value => Get(Options.DefaultName);

        /// <summary>
        /// Initializes a new instance with the specified options configurations.
        /// </summary>
        /// <param name="factory">The factory to use to create options.</param>
        public OptionsManager(IOptionsFactory<TOptions> factory)
        {
            _factory = factory;
        }

        /// <summary>
        /// Returns a configured <typeparamref name="TOptions" /> instance with the given <paramref name="name" />.
        /// </summary>
        /// <param name="name">The name of the <typeparamref name="TOptions" /> instance, if <see langword="null" /> <see cref="F:Microsoft.Extensions.Options.Options.DefaultName" /> is used.</param>
        /// <returns>The <typeparamref name="TOptions" /> instance that matches the given <paramref name="name" />.</returns>
        /// <exception cref="T:Microsoft.Extensions.Options.OptionsValidationException">One or more <see cref="T:Microsoft.Extensions.Options.IValidateOptions`1" /> return failed <see cref="T:Microsoft.Extensions.Options.ValidateOptionsResult" /> when validating the <typeparamref name="TOptions" /> instance been created.</exception>
        /// <exception cref="T:System.MissingMethodException">The <typeparamref name="TOptions" /> does not have a public parameterless constructor or <typeparamref name="TOptions" /> is <see langword="abstract" />.</exception>
        public virtual TOptions Get(string name)
        {
            if (name == null)
            {
                name = Options.DefaultName;
            }
            if (!_cache.TryGetValue(name, out var options))
            {
                IOptionsFactory<TOptions> localFactory = _factory;
                string localName = name;
                return _cache.GetOrAdd(name, () => localFactory.Create(localName));
            }
            return options;
        }
    }

}
