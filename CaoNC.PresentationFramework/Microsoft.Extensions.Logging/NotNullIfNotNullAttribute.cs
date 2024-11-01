﻿using System;

namespace CaoNC.Microsoft.Extensions.Logging
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.ReturnValue, AllowMultiple = true, Inherited = false)]
    internal sealed class NotNullIfNotNullAttribute : Attribute
    {
        /// <summary>Gets the associated parameter name.</summary>
        public string ParameterName { get; }

        /// <summary>Initializes the attribute with the associated parameter name.</summary>
        /// <param name="parameterName">
        /// The associated parameter name.  The output will be non-null if the argument to the parameter specified is non-null.
        /// </param>
        public NotNullIfNotNullAttribute(string parameterName)
        {
            ParameterName = parameterName;
        }
    }
}
