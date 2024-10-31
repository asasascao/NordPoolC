using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaoNC.Microsoft.Extensions.Options
{
    public class OptionsCache<[System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TOptions> : IOptionsMonitorCache<TOptions> where TOptions : class
    {
        private readonly ConcurrentDictionary<string, Lazy<TOptions>> _cache = new ConcurrentDictionary<string, Lazy<TOptions>>(1, 31, StringComparer.Ordinal);

        /// <summary>
        /// Clears all options instances from the cache.
        /// </summary>
        public void Clear()
        {
            _cache.Clear();
        }

        /// <summary>
        /// Gets a named options instance, or adds a new instance created with <paramref name="createOptions" />.
        /// </summary>
        /// <param name="name">The name of the options instance.</param>
        /// <param name="createOptions">The func used to create the new instance.</param>
        /// <returns>The options instance.</returns>
        public virtual TOptions GetOrAdd(string name, Func<TOptions> createOptions)
        {
            System.ThrowHelper.ThrowIfNull(createOptions, "createOptions");
            if (name == null)
            {
                name = Options.DefaultName;
            }
            if (!_cache.TryGetValue(name, out var value))
            {
                value = _cache.GetOrAdd(name, new Lazy<TOptions>(createOptions));
            }
            return value.Value;
        }

        internal TOptions GetOrAdd<TArg>(string name, Func<string, TArg, TOptions> createOptions, TArg factoryArgument)
        {
            string localName = name;
            Func<string, TArg, TOptions> localCreateOptions = createOptions;
            TArg localFactoryArgument = factoryArgument;
            return GetOrAdd(name, () => localCreateOptions(localName ?? Options.DefaultName, localFactoryArgument));
        }

        /// <summary>
        /// Gets a named options instance, if available.
        /// </summary>
        /// <param name="name">The name of the options instance.</param>
        /// <param name="options">The options instance.</param>
        /// <returns>true if the options were retrieved; otherwise, false.</returns>
        internal bool TryGetValue(string name, [System.Diagnostics.CodeAnalysis.MaybeNullWhen(false)] out TOptions options)
        {
            if (_cache.TryGetValue(name ?? Options.DefaultName, out var value))
            {
                options = value.Value;
                return true;
            }
            options = null;
            return false;
        }

        /// <summary>
        /// Tries to adds a new option to the cache, will return false if the name already exists.
        /// </summary>
        /// <param name="name">The name of the options instance.</param>
        /// <param name="options">The options instance.</param>
        /// <returns>Whether anything was added.</returns>
        public virtual bool TryAdd(string name, TOptions options)
        {
            TOptions options2 = options;
            System.ThrowHelper.ThrowIfNull(options2, "options");
            return _cache.TryAdd(name ?? Options.DefaultName, new Lazy<TOptions>(() => options2));
        }

        /// <summary>
        /// Try to remove an options instance.
        /// </summary>
        /// <param name="name">The name of the options instance.</param>
        /// <returns>Whether anything was removed.</returns>
        public virtual bool TryRemove(string name)
        {
            Lazy<TOptions> value;
            return _cache.TryRemove(name ?? Options.DefaultName, out value);
        }
    }
}
