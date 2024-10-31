using CaoNC.Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System;

namespace CaoNC.Microsoft.Extensions.Options
{
    public class OptionsMonitor<[System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TOptions> : IOptionsMonitor<TOptions>, IDisposable where TOptions : class
    {
        internal sealed class ChangeTrackerDisposable : IDisposable
        {
            private readonly Action<TOptions, string> _listener;

            private readonly OptionsMonitor<TOptions> _monitor;

            public ChangeTrackerDisposable(OptionsMonitor<TOptions> monitor, Action<TOptions, string> listener)
            {
                _listener = listener;
                _monitor = monitor;
            }

            public void OnChange(TOptions options, string name)
            {
                _listener(options, name);
            }

            public void Dispose()
            {
                _monitor._onChange -= OnChange;
            }
        }

        private readonly IOptionsMonitorCache<TOptions> _cache;

        private readonly IOptionsFactory<TOptions> _factory;

        private readonly List<IDisposable> _registrations = new List<IDisposable>();

        /// <summary>
        /// The present value of the options, equivalent to <c>Get(Options.DefaultName)</c>.
        /// </summary>
        /// <exception cref="T:Microsoft.Extensions.Options.OptionsValidationException">One or more <see cref="T:Microsoft.Extensions.Options.IValidateOptions`1" /> return failed <see cref="T:Microsoft.Extensions.Options.ValidateOptionsResult" /> when validating the <typeparamref name="TOptions" /> instance been created.</exception>
        /// <exception cref="T:System.MissingMethodException">The <typeparamref name="TOptions" /> does not have a public parameterless constructor or <typeparamref name="TOptions" /> is <see langword="abstract" />.</exception>
        public TOptions CurrentValue => Get(Options.DefaultName);

        internal event Action<TOptions, string> _onChange;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="factory">The factory to use to create options.</param>
        /// <param name="sources">The sources used to listen for changes to the options instance.</param>
        /// <param name="cache">The cache used to store options.</param>
        public OptionsMonitor(IOptionsFactory<TOptions> factory, IEnumerable<IOptionsChangeTokenSource<TOptions>> sources, IOptionsMonitorCache<TOptions> cache)
        {
            _factory = factory;
            _cache = cache;
            if (sources is IOptionsChangeTokenSource<TOptions>[] array)
            {
                IOptionsChangeTokenSource<TOptions>[] array2 = array;
                foreach (IOptionsChangeTokenSource<TOptions> source2 in array2)
                {
                    RegisterSource(source2);
                }
                return;
            }
            foreach (IOptionsChangeTokenSource<TOptions> source3 in sources)
            {
                RegisterSource(source3);
            }
        }

        void RegisterSource(IOptionsChangeTokenSource<TOptions> source)
        {
            IDisposable item = ChangeToken.OnChange(source.GetChangeToken, InvokeChanged, source.Name);
            _registrations.Add(item);
        }

        private void InvokeChanged(string name)
        {
            if (name == null)
            {
                name = Options.DefaultName;
            }
            _cache.TryRemove(name);
            TOptions arg = Get(name);
            this._onChange?.Invoke(arg, name);
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
            if (!(_cache is OptionsCache<TOptions> optionsCache))
            {
                string localName = name ?? Options.DefaultName;
                IOptionsFactory<TOptions> localFactory = _factory;
                return _cache.GetOrAdd(localName, () => localFactory.Create(localName));
            }
            return optionsCache.GetOrAdd(name, (string name1, IOptionsFactory<TOptions> factory) => factory.Create(name1), _factory);
        }

        /// <summary>
        /// Registers a listener to be called whenever <typeparamref name="TOptions" /> changes.
        /// </summary>
        /// <param name="listener">The action to be invoked when <typeparamref name="TOptions" /> has changed.</param>
        /// <returns>An <see cref="T:System.IDisposable" /> which should be disposed to stop listening for changes.</returns>
        public IDisposable OnChange(Action<TOptions, string> listener)
        {
            ChangeTrackerDisposable changeTrackerDisposable = new ChangeTrackerDisposable(this, listener);
            _onChange += changeTrackerDisposable.OnChange;
            return changeTrackerDisposable;
        }

        /// <summary>
        /// Removes all change registration subscriptions.
        /// </summary>
        public void Dispose()
        {
            foreach (IDisposable registration in _registrations)
            {
                registration.Dispose();
            }
            _registrations.Clear();
        }
    }

}
