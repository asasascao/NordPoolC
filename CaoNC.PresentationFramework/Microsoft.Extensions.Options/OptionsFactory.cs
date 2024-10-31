using System.Collections.Generic;
using System;

namespace CaoNC.Microsoft.Extensions.Options
{
    public class OptionsFactory<[System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TOptions> : IOptionsFactory<TOptions> where TOptions : class
    {
        private readonly IConfigureOptions<TOptions>[] _setups;

        private readonly IPostConfigureOptions<TOptions>[] _postConfigures;

        private readonly IValidateOptions<TOptions>[] _validations;

        /// <summary>
        /// Initializes a new instance with the specified options configurations.
        /// </summary>
        /// <param name="setups">The configuration actions to run.</param>
        /// <param name="postConfigures">The initialization actions to run.</param>
        public OptionsFactory(IEnumerable<IConfigureOptions<TOptions>> setups, IEnumerable<IPostConfigureOptions<TOptions>> postConfigures)
            : this(setups, postConfigures, (IEnumerable<IValidateOptions<TOptions>>)(new IValidateOptions<TOptions>[0]))
        {
        }

        /// <summary>
        /// Initializes a new instance with the specified options configurations.
        /// </summary>
        /// <param name="setups">The configuration actions to run.</param>
        /// <param name="postConfigures">The initialization actions to run.</param>
        /// <param name="validations">The validations to run.</param>
        public OptionsFactory(IEnumerable<IConfigureOptions<TOptions>> setups, IEnumerable<IPostConfigureOptions<TOptions>> postConfigures, IEnumerable<IValidateOptions<TOptions>> validations)
        {
            _setups = (setups as IConfigureOptions<TOptions>[]) ?? new List<IConfigureOptions<TOptions>>(setups).ToArray();
            _postConfigures = (postConfigures as IPostConfigureOptions<TOptions>[]) ?? new List<IPostConfigureOptions<TOptions>>(postConfigures).ToArray();
            _validations = (validations as IValidateOptions<TOptions>[]) ?? new List<IValidateOptions<TOptions>>(validations).ToArray();
        }

        /// <summary>
        /// Returns a configured <typeparamref name="TOptions" /> instance with the given <paramref name="name" />.
        /// </summary>
        /// <param name="name">The name of the <typeparamref name="TOptions" /> instance to create.</param>
        /// <returns>The created <typeparamref name="TOptions" /> instance with the given <paramref name="name" />.</returns>
        /// <exception cref="T:Microsoft.Extensions.Options.OptionsValidationException">One or more <see cref="T:Microsoft.Extensions.Options.IValidateOptions`1" /> return failed <see cref="T:Microsoft.Extensions.Options.ValidateOptionsResult" /> when validating the <typeparamref name="TOptions" /> instance been created.</exception>
        /// <exception cref="T:System.MissingMethodException">The <typeparamref name="TOptions" /> does not have a public parameterless constructor or <typeparamref name="TOptions" /> is <see langword="abstract" />.</exception>
        public TOptions Create(string name)
        {
            TOptions val = CreateInstance(name);
            IConfigureOptions<TOptions>[] setups = _setups;
            foreach (IConfigureOptions<TOptions> configureOptions in setups)
            {
                if (configureOptions is IConfigureNamedOptions<TOptions> configureNamedOptions)
                {
                    configureNamedOptions.Configure(name, val);
                }
                else if (name == Options.DefaultName)
                {
                    configureOptions.Configure(val);
                }
            }
            IPostConfigureOptions<TOptions>[] postConfigures = _postConfigures;
            foreach (IPostConfigureOptions<TOptions> postConfigureOptions in postConfigures)
            {
                postConfigureOptions.PostConfigure(name, val);
            }
            if (_validations.Length != 0)
            {
                List<string> list = new List<string>();
                IValidateOptions<TOptions>[] validations = _validations;
                foreach (IValidateOptions<TOptions> validateOptions in validations)
                {
                    ValidateOptionsResult validateOptionsResult = validateOptions.Validate(name, val);
                    if (validateOptionsResult != null && validateOptionsResult.Failed)
                    {
                        list.AddRange(validateOptionsResult.Failures);
                    }
                }
                if (list.Count > 0)
                {
                    throw new OptionsValidationException(name, typeof(TOptions), list);
                }
            }
            return val;
        }

        /// <summary>
        /// Creates a new instance of options type.
        /// </summary>
        /// <param name="name">The name of the <typeparamref name="TOptions" /> instance to create.</param>
        /// <returns>The created <typeparamref name="TOptions" /> instance.</returns>
        /// <exception cref="T:System.MissingMethodException">The <typeparamref name="TOptions" /> does not have a public parameterless constructor or <typeparamref name="TOptions" /> is <see langword="abstract" />.</exception>
        protected virtual TOptions CreateInstance(string name)
        {
            return Activator.CreateInstance<TOptions>();
        }
    }

}
