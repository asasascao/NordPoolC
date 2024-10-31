using CaoNC.Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaoNC.Microsoft.Extensions.DependencyInjection
{
    [System.Diagnostics.CodeAnalysis.UnconditionalSuppressMessage("ReflectionAnalysis", "IL2091:UnrecognizedReflectionPattern", Justification = "Workaround for https://github.com/mono/linker/issues/1416. Outer method has been annotated with DynamicallyAccessedMembers.")]
    public static class OptionsBuilderExtensions
    {
        /// <summary>
        /// Enforces options validation check on start rather than in runtime.
        /// </summary>
        /// <typeparam name="TOptions">The type of options.</typeparam>
        /// <param name="optionsBuilder">The <see cref="T:Microsoft.Extensions.Options.OptionsBuilder`1" /> to configure options instance.</param>
        /// <returns>The <see cref="T:Microsoft.Extensions.Options.OptionsBuilder`1" /> so that additional calls can be chained.</returns>
        public static OptionsBuilder<TOptions> ValidateOnStart<[System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TOptions>(this OptionsBuilder<TOptions> optionsBuilder) where TOptions : class
        {
            OptionsBuilder<TOptions> optionsBuilder2 = optionsBuilder;
            System.ThrowHelper.ThrowIfNull(optionsBuilder2, "optionsBuilder");
            optionsBuilder2.Services.AddTransient<IStartupValidator, StartupValidator>();
            optionsBuilder2.Services.AddOptions<StartupValidatorOptions>().Configure(delegate (StartupValidatorOptions vo, IOptionsMonitor<TOptions> options)
            {
                vo._validators[new Tuple<Type, string>(typeof(TOptions), optionsBuilder2.Name)] = delegate
                {
                    options.Get(optionsBuilder2.Name);
                };
            });
            return optionsBuilder2;
        }
    }
}
