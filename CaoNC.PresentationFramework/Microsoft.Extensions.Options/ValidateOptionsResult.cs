using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaoNC.Microsoft.Extensions.Options
{
    public class ValidateOptionsResult
    {
        /// <summary>
        /// Result when validation was skipped due to name not matching.
        /// </summary>
        public static readonly ValidateOptionsResult Skip = new ValidateOptionsResult
        {
            Skipped = true
        };

        /// <summary>
        /// Validation was successful.
        /// </summary>
        public static readonly ValidateOptionsResult Success = new ValidateOptionsResult
        {
            Succeeded = true
        };

        /// <summary>
        /// True if validation was successful.
        /// </summary>
        public bool Succeeded { get; protected set; }

        /// <summary>
        /// True if validation was not run.
        /// </summary>
        public bool Skipped { get; protected set; }

        /// <summary>
        /// True if validation failed.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.MemberNotNullWhen(true, "Failures")]
        [System.Diagnostics.CodeAnalysis.MemberNotNullWhen(true, "FailureMessage")]
        public bool Failed
        {
            [System.Diagnostics.CodeAnalysis.MemberNotNullWhen(true, "Failures")]
            [System.Diagnostics.CodeAnalysis.MemberNotNullWhen(true, "FailureMessage")]
            get;
            [System.Diagnostics.CodeAnalysis.MemberNotNullWhen(true, "Failures")]
            [System.Diagnostics.CodeAnalysis.MemberNotNullWhen(true, "FailureMessage")]
            protected set;
        }

        /// <summary>
        /// Used to describe why validation failed.
        /// </summary>
        public string FailureMessage { get; protected set; }

        /// <summary>
        /// Full list of failures (can be multiple).
        /// </summary>
        public IEnumerable<string> Failures { get; protected set; }

        /// <summary>
        /// Returns a failure result.
        /// </summary>
        /// <param name="failureMessage">The reason for the failure.</param>
        /// <returns>The failure result.</returns>
        public static ValidateOptionsResult Fail(string failureMessage)
        {
            ValidateOptionsResult validateOptionsResult = new ValidateOptionsResult();
            validateOptionsResult.Failed = true;
            validateOptionsResult.FailureMessage = failureMessage;
            validateOptionsResult.Failures = new string[1] { failureMessage };
            return validateOptionsResult;
        }

        /// <summary>
        /// Returns a failure result.
        /// </summary>
        /// <param name="failures">The reasons for the failure.</param>
        /// <returns>The failure result.</returns>
        public static ValidateOptionsResult Fail(IEnumerable<string> failures)
        {
            return new ValidateOptionsResult
            {
                Failed = true,
                FailureMessage = string.Join("; ", failures),
                Failures = failures
            };
        }
    }

}
