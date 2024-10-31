using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaoNC.Microsoft.Extensions.DependencyInjection
{
    public interface IServiceScope : IDisposable
    {
        /// <summary>
        /// The <see cref="T:System.IServiceProvider" /> used to resolve dependencies from the scope.
        /// </summary>
        IServiceProvider ServiceProvider { get; }
    }
}
