using CaoNC.System.Threading;
using CaoNC.System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaoNC.Microsoft.Extensions.DependencyInjection
{
    [DebuggerDisplay("{ServiceProvider,nq}")]
    public readonly struct AsyncServiceScope : IServiceScope, IDisposable, IAsyncDisposable
    {
        private readonly IServiceScope _serviceScope;

        /// <inheritdoc />
        public IServiceProvider ServiceProvider => _serviceScope.ServiceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Microsoft.Extensions.DependencyInjection.AsyncServiceScope" /> struct.
        /// Wraps an instance of <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceScope" />.
        /// </summary>
        /// <param name="serviceScope">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceScope" /> instance to wrap.</param>
        public AsyncServiceScope(IServiceScope serviceScope)
        {
            System.ThrowHelper.ThrowIfNull(serviceScope, "serviceScope");
            _serviceScope = serviceScope;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _serviceScope.Dispose();
        }

        /// <inheritdoc />
        public ValueTask DisposeAsync()
        {
            if (_serviceScope is IAsyncDisposable asyncDisposable)
            {
                return asyncDisposable.DisposeAsync();
            }
            _serviceScope.Dispose();
            return default(ValueTask);
        }
    }
}
