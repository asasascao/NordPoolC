﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaoNC.Microsoft.Extensions.Options
{
    public class OptionsValidationException : Exception
    {
        /// <summary>
        /// The name of the options instance that failed.
        /// </summary>
        public string OptionsName { get; }

        /// <summary>
        /// The type of the options that failed.
        /// </summary>
        public Type OptionsType { get; }

        /// <summary>
        /// The validation failures.
        /// </summary>
        public IEnumerable<string> Failures { get; }

        /// <summary>
        /// The message is a semicolon separated list of the <see cref="P:Microsoft.Extensions.Options.OptionsValidationException.Failures" />.
        /// </summary>
        public override string Message => string.Join("; ", Failures);

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="optionsName">The name of the options instance that failed.</param>
        /// <param name="optionsType">The options type that failed.</param>
        /// <param name="failureMessages">The validation failure messages.</param>
        public OptionsValidationException(string optionsName, Type optionsType, IEnumerable<string> failureMessages)
        {
            System.ThrowHelper.ThrowIfNull(optionsName, "optionsName");
            System.ThrowHelper.ThrowIfNull(optionsType, "optionsType");
            Failures = failureMessages ?? new List<string>();
            OptionsType = optionsType;
            OptionsName = optionsName;
        }
    }
}
