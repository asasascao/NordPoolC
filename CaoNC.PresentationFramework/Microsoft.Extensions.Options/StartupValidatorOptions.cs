using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaoNC.Microsoft.Extensions.Options
{
    internal sealed class StartupValidatorOptions
    {
        public Dictionary<Tuple<Type, string>, Action> _validators { get; } = new Dictionary<Tuple<Type, string>, Action>();

    }
}
