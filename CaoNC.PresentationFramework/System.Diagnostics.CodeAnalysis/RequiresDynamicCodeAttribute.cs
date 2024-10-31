using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaoNC.System.Diagnostics.CodeAnalysis
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Constructor | AttributeTargets.Method, Inherited = false)]
    internal sealed class RequiresDynamicCodeAttribute : Attribute
    {
        /// <summary>
        /// Gets a message that contains information about the usage of dynamic code.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Gets or sets an optional URL that contains more information about the method,
        /// why it requires dynamic code, and what options a consumer has to deal with it.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute" /> class
        /// with the specified message.
        /// </summary>
        /// <param name="message">
        /// A message that contains information about the usage of dynamic code.
        /// </param>
        public RequiresDynamicCodeAttribute(string message)
        {
            Message = message;
        }
    }

}
