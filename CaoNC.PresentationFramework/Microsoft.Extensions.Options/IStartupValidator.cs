using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaoNC.Microsoft.Extensions.Options
{
    public interface IStartupValidator
    {
        /// <summary>
        /// Calls the <see cref="T:Microsoft.Extensions.Options.IValidateOptions`1" /> validators.
        /// </summary>
        /// <exception cref="T:Microsoft.Extensions.Options.OptionsValidationException">One or more <see cref="T:Microsoft.Extensions.Options.IValidateOptions`1" /> return failed <see cref="T:Microsoft.Extensions.Options.ValidateOptionsResult" /> when validating.</exception>
        void Validate();
    }
}
