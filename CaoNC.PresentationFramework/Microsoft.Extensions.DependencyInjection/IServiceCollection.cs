using System.Collections;
using System.Collections.Generic;

namespace CaoNC.Microsoft.Extensions.DependencyInjection
{
    public interface IServiceCollection : IList<ServiceDescriptor>, ICollection<ServiceDescriptor>, IEnumerable<ServiceDescriptor>, IEnumerable
    {
    }
}
