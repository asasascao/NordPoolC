using System.Threading;

namespace CaoNC.Microsoft.Extensions.Options
{
    internal sealed class UnnamedOptionsManager<[System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TOptions> : IOptions<TOptions> where TOptions : class
    {
        private readonly IOptionsFactory<TOptions> _factory;

        private volatile object _syncObj;

        private volatile TOptions _value;

        public TOptions Value
        {
            get
            {
                TOptions value = _value;
                if (value != null)
                {
                    return value;
                }
                lock (_syncObj ?? Interlocked.CompareExchange(ref _syncObj, new object(), null) ?? _syncObj)
                {
                    return _value ?? (_value = _factory.Create(Options.DefaultName));
                }
            }
        }

        public UnnamedOptionsManager(IOptionsFactory<TOptions> factory)
        {
            _factory = factory;
        }
    }
}
