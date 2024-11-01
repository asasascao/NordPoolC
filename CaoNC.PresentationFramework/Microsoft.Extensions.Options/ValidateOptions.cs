﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaoNC.Microsoft.Extensions.Options
{
    public class ValidateOptions<TOptions> : IValidateOptions<TOptions> where TOptions : class
    {
        /// <summary>
        /// The options name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The validation function.
        /// </summary>
        public Func<TOptions, bool> Validation { get; }

        /// <summary>
        /// The error to return when validation fails.
        /// </summary>
        public string FailureMessage { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Options name.</param>
        /// <param name="validation">Validation function.</param>
        /// <param name="failureMessage">Validation failure message.</param>
        public ValidateOptions(string name, Func<TOptions, bool> validation, string failureMessage)
        {
            System.ThrowHelper.ThrowIfNull(validation, "validation");
            Name = name;
            Validation = validation;
            FailureMessage = failureMessage;
        }

        /// <summary>
        /// Validates a specific named options instance (or all when <paramref name="name" /> is null).
        /// </summary>
        /// <param name="name">The name of the options instance being validated.</param>
        /// <param name="options">The options instance.</param>
        /// <returns>The <see cref="T:Microsoft.Extensions.Options.ValidateOptionsResult" /> result.</returns>
        public ValidateOptionsResult Validate(string name, TOptions options)
        {
            if (Name == null || name == Name)
            {
                if (Validation(options))
                {
                    return ValidateOptionsResult.Success;
                }
                return ValidateOptionsResult.Fail(FailureMessage);
            }
            return ValidateOptionsResult.Skip;
        }
    }

    public class ValidateOptions<TOptions, TDep> : IValidateOptions<TOptions> where TOptions : class
    {
        /// <summary>
        /// The options name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The validation function.
        /// </summary>
        public Func<TOptions, TDep, bool> Validation { get; }

        /// <summary>
        /// The error to return when validation fails.
        /// </summary>
        public string FailureMessage { get; }

        /// <summary>
        /// The dependency.
        /// </summary>
        public TDep Dependency { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Options name.</param>
        /// <param name="dependency">The dependency.</param>
        /// <param name="validation">Validation function.</param>
        /// <param name="failureMessage">Validation failure message.</param>
        public ValidateOptions(string name, TDep dependency, Func<TOptions, TDep, bool> validation, string failureMessage)
        {
            System.ThrowHelper.ThrowIfNull(validation, "validation");
            Name = name;
            Validation = validation;
            FailureMessage = failureMessage;
            Dependency = dependency;
        }

        /// <summary>
        /// Validates a specific named options instance (or all when <paramref name="name" /> is null).
        /// </summary>
        /// <param name="name">The name of the options instance being validated.</param>
        /// <param name="options">The options instance.</param>
        /// <returns>The <see cref="T:Microsoft.Extensions.Options.ValidateOptionsResult" /> result.</returns>
        public ValidateOptionsResult Validate(string name, TOptions options)
        {
            if (Name == null || name == Name)
            {
                if (Validation(options, Dependency))
                {
                    return ValidateOptionsResult.Success;
                }
                return ValidateOptionsResult.Fail(FailureMessage);
            }
            return ValidateOptionsResult.Skip;
        }
    }

    public class ValidateOptions<TOptions, TDep1, TDep2> : IValidateOptions<TOptions> where TOptions : class
    {
        /// <summary>
        /// The options name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The validation function.
        /// </summary>
        public Func<TOptions, TDep1, TDep2, bool> Validation { get; }

        /// <summary>
        /// The error to return when validation fails.
        /// </summary>
        public string FailureMessage { get; }

        /// <summary>
        /// The first dependency.
        /// </summary>
        public TDep1 Dependency1 { get; }

        /// <summary>
        /// The second dependency.
        /// </summary>
        public TDep2 Dependency2 { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Options name.</param>
        /// <param name="dependency1">The first dependency.</param>
        /// <param name="dependency2">The second dependency.</param>
        /// <param name="validation">Validation function.</param>
        /// <param name="failureMessage">Validation failure message.</param>
        public ValidateOptions(string name, TDep1 dependency1, TDep2 dependency2, Func<TOptions, TDep1, TDep2, bool> validation, string failureMessage)
        {
            System.ThrowHelper.ThrowIfNull(validation, "validation");
            Name = name;
            Validation = validation;
            FailureMessage = failureMessage;
            Dependency1 = dependency1;
            Dependency2 = dependency2;
        }

        /// <summary>
        /// Validates a specific named options instance (or all when <paramref name="name" /> is null).
        /// </summary>
        /// <param name="name">The name of the options instance being validated.</param>
        /// <param name="options">The options instance.</param>
        /// <returns>The <see cref="T:Microsoft.Extensions.Options.ValidateOptionsResult" /> result.</returns>
        public ValidateOptionsResult Validate(string name, TOptions options)
        {
            if (Name == null || name == Name)
            {
                if (Validation(options, Dependency1, Dependency2))
                {
                    return ValidateOptionsResult.Success;
                }
                return ValidateOptionsResult.Fail(FailureMessage);
            }
            return ValidateOptionsResult.Skip;
        }
    }

    public class ValidateOptions<TOptions, TDep1, TDep2, TDep3> : IValidateOptions<TOptions> where TOptions : class
    {
        /// <summary>
        /// The options name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The validation function.
        /// </summary>
        public Func<TOptions, TDep1, TDep2, TDep3, bool> Validation { get; }

        /// <summary>
        /// The error to return when validation fails.
        /// </summary>
        public string FailureMessage { get; }

        /// <summary>
        /// The first dependency.
        /// </summary>
        public TDep1 Dependency1 { get; }

        /// <summary>
        /// The second dependency.
        /// </summary>
        public TDep2 Dependency2 { get; }

        /// <summary>
        /// The third dependency.
        /// </summary>
        public TDep3 Dependency3 { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Options name.</param>
        /// <param name="dependency1">The first dependency.</param>
        /// <param name="dependency2">The second dependency.</param>
        /// <param name="dependency3">The third dependency.</param>
        /// <param name="validation">Validation function.</param>
        /// <param name="failureMessage">Validation failure message.</param>
        public ValidateOptions(string name, TDep1 dependency1, TDep2 dependency2, TDep3 dependency3, Func<TOptions, TDep1, TDep2, TDep3, bool> validation, string failureMessage)
        {
            System.ThrowHelper.ThrowIfNull(validation, "validation");
            Name = name;
            Validation = validation;
            FailureMessage = failureMessage;
            Dependency1 = dependency1;
            Dependency2 = dependency2;
            Dependency3 = dependency3;
        }

        /// <summary>
        /// Validates a specific named options instance (or all when <paramref name="name" /> is null).
        /// </summary>
        /// <param name="name">The name of the options instance being validated.</param>
        /// <param name="options">The options instance.</param>
        /// <returns>The <see cref="T:Microsoft.Extensions.Options.ValidateOptionsResult" /> result.</returns>
        public ValidateOptionsResult Validate(string name, TOptions options)
        {
            if (Name == null || name == Name)
            {
                if (Validation(options, Dependency1, Dependency2, Dependency3))
                {
                    return ValidateOptionsResult.Success;
                }
                return ValidateOptionsResult.Fail(FailureMessage);
            }
            return ValidateOptionsResult.Skip;
        }
    }

    public class ValidateOptions<TOptions, TDep1, TDep2, TDep3, TDep4> : IValidateOptions<TOptions> where TOptions : class
    {
        /// <summary>
        /// The options name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The validation function.
        /// </summary>
        public Func<TOptions, TDep1, TDep2, TDep3, TDep4, bool> Validation { get; }

        /// <summary>
        /// The error to return when validation fails.
        /// </summary>
        public string FailureMessage { get; }

        /// <summary>
        /// The first dependency.
        /// </summary>
        public TDep1 Dependency1 { get; }

        /// <summary>
        /// The second dependency.
        /// </summary>
        public TDep2 Dependency2 { get; }

        /// <summary>
        /// The third dependency.
        /// </summary>
        public TDep3 Dependency3 { get; }

        /// <summary>
        /// The fourth dependency.
        /// </summary>
        public TDep4 Dependency4 { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Options name.</param>
        /// <param name="dependency1">The first dependency.</param>
        /// <param name="dependency2">The second dependency.</param>
        /// <param name="dependency3">The third dependency.</param>
        /// <param name="dependency4">The fourth dependency.</param>
        /// <param name="validation">Validation function.</param>
        /// <param name="failureMessage">Validation failure message.</param>
        public ValidateOptions(string name, TDep1 dependency1, TDep2 dependency2, TDep3 dependency3, TDep4 dependency4, Func<TOptions, TDep1, TDep2, TDep3, TDep4, bool> validation, string failureMessage)
        {
            System.ThrowHelper.ThrowIfNull(validation, "validation");
            Name = name;
            Validation = validation;
            FailureMessage = failureMessage;
            Dependency1 = dependency1;
            Dependency2 = dependency2;
            Dependency3 = dependency3;
            Dependency4 = dependency4;
        }

        /// <summary>
        /// Validates a specific named options instance (or all when <paramref name="name" /> is null).
        /// </summary>
        /// <param name="name">The name of the options instance being validated.</param>
        /// <param name="options">The options instance.</param>
        /// <returns>The <see cref="T:Microsoft.Extensions.Options.ValidateOptionsResult" /> result.</returns>
        public ValidateOptionsResult Validate(string name, TOptions options)
        {
            if (Name == null || name == Name)
            {
                if (Validation(options, Dependency1, Dependency2, Dependency3, Dependency4))
                {
                    return ValidateOptionsResult.Success;
                }
                return ValidateOptionsResult.Fail(FailureMessage);
            }
            return ValidateOptionsResult.Skip;
        }
    }

    public class ValidateOptions<TOptions, TDep1, TDep2, TDep3, TDep4, TDep5> : IValidateOptions<TOptions> where TOptions : class
    {
        /// <summary>
        /// The options name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The validation function.
        /// </summary>
        public Func<TOptions, TDep1, TDep2, TDep3, TDep4, TDep5, bool> Validation { get; }

        /// <summary>
        /// The error to return when validation fails.
        /// </summary>
        public string FailureMessage { get; }

        /// <summary>
        /// The first dependency.
        /// </summary>
        public TDep1 Dependency1 { get; }

        /// <summary>
        /// The second dependency.
        /// </summary>
        public TDep2 Dependency2 { get; }

        /// <summary>
        /// The third dependency.
        /// </summary>
        public TDep3 Dependency3 { get; }

        /// <summary>
        /// The fourth dependency.
        /// </summary>
        public TDep4 Dependency4 { get; }

        /// <summary>
        /// The fifth dependency.
        /// </summary>
        public TDep5 Dependency5 { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Options name.</param>
        /// <param name="dependency1">The first dependency.</param>
        /// <param name="dependency2">The second dependency.</param>
        /// <param name="dependency3">The third dependency.</param>
        /// <param name="dependency4">The fourth dependency.</param>
        /// <param name="dependency5">The fifth dependency.</param>
        /// <param name="validation">Validation function.</param>
        /// <param name="failureMessage">Validation failure message.</param>
        public ValidateOptions(string name, TDep1 dependency1, TDep2 dependency2, TDep3 dependency3, TDep4 dependency4, TDep5 dependency5, Func<TOptions, TDep1, TDep2, TDep3, TDep4, TDep5, bool> validation, string failureMessage)
        {
            System.ThrowHelper.ThrowIfNull(validation, "validation");
            Name = name;
            Validation = validation;
            FailureMessage = failureMessage;
            Dependency1 = dependency1;
            Dependency2 = dependency2;
            Dependency3 = dependency3;
            Dependency4 = dependency4;
            Dependency5 = dependency5;
        }

        /// <summary>
        /// Validates a specific named options instance (or all when <paramref name="name" /> is null).
        /// </summary>
        /// <param name="name">The name of the options instance being validated.</param>
        /// <param name="options">The options instance.</param>
        /// <returns>The <see cref="T:Microsoft.Extensions.Options.ValidateOptionsResult" /> result.</returns>
        public ValidateOptionsResult Validate(string name, TOptions options)
        {
            if (Name == null || name == Name)
            {
                if (Validation(options, Dependency1, Dependency2, Dependency3, Dependency4, Dependency5))
                {
                    return ValidateOptionsResult.Success;
                }
                return ValidateOptionsResult.Fail(FailureMessage);
            }
            return ValidateOptionsResult.Skip;
        }
    }

}
