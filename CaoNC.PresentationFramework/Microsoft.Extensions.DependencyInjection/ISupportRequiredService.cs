using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaoNC.Microsoft.Extensions.DependencyInjection
{
    public interface ISupportRequiredService
    {
        /// <summary>
        /// Gets service of type <paramref name="serviceType" /> from the <see cref="T:System.IServiceProvider" /> implementing
        /// this interface.
        /// </summary>
        /// <param name="serviceType">An object that specifies the type of service object to get.</param>
        /// <returns>A service object of type <paramref name="serviceType" />.
        /// Throws an exception if the <see cref="T:System.IServiceProvider" /> cannot create the object.</returns>
        object GetRequiredService(Type serviceType);
    }
}
