using System.Collections.Generic;
using System.Text;
using System;

namespace CaoNC.Microsoft.Extensions.Logging
{
    internal static class TypeNameHelper
    {
        private readonly struct DisplayNameOptions
        {
            public bool FullName { get; }

            public bool IncludeGenericParameters { get; }

            public bool IncludeGenericParameterNames { get; }

            public char NestedTypeDelimiter { get; }

            public DisplayNameOptions(bool fullName, bool includeGenericParameterNames, bool includeGenericParameters, char nestedTypeDelimiter)
            {
                FullName = fullName;
                IncludeGenericParameters = includeGenericParameters;
                IncludeGenericParameterNames = includeGenericParameterNames;
                NestedTypeDelimiter = nestedTypeDelimiter;
            }
        }

        private const char DefaultNestedTypeDelimiter = '+';

        private static readonly Dictionary<Type, string> _builtInTypeNames = new Dictionary<Type, string>
    {
        {
            typeof(void),
            "void"
        },
        {
            typeof(bool),
            "bool"
        },
        {
            typeof(byte),
            "byte"
        },
        {
            typeof(char),
            "char"
        },
        {
            typeof(decimal),
            "decimal"
        },
        {
            typeof(double),
            "double"
        },
        {
            typeof(float),
            "float"
        },
        {
            typeof(int),
            "int"
        },
        {
            typeof(long),
            "long"
        },
        {
            typeof(object),
            "object"
        },
        {
            typeof(sbyte),
            "sbyte"
        },
        {
            typeof(short),
            "short"
        },
        {
            typeof(string),
            "string"
        },
        {
            typeof(uint),
            "uint"
        },
        {
            typeof(ulong),
            "ulong"
        },
        {
            typeof(ushort),
            "ushort"
        }
    };

        [return: NotNullIfNotNull("item")]
        public static string GetTypeDisplayName(object item, bool fullName = true)
        {
            if (item != null)
            {
                return GetTypeDisplayName(item.GetType(), fullName);
            }
            return null;
        }

        /// <summary>
        /// Pretty print a type name.
        /// </summary>
        /// <param name="type">The <see cref="T:System.Type" />.</param>
        /// <param name="fullName"><c>true</c> to print a fully qualified name.</param>
        /// <param name="includeGenericParameterNames"><c>true</c> to include generic parameter names.</param>
        /// <param name="includeGenericParameters"><c>true</c> to include generic parameters.</param>
        /// <param name="nestedTypeDelimiter">Character to use as a delimiter in nested type names</param>
        /// <returns>The pretty printed type name.</returns>
        public static string GetTypeDisplayName(Type type, bool fullName = true, bool includeGenericParameterNames = false, bool includeGenericParameters = true, char nestedTypeDelimiter = '+')
        {
            StringBuilder builder = null;
            DisplayNameOptions options = new DisplayNameOptions(fullName, includeGenericParameterNames, includeGenericParameters, nestedTypeDelimiter);
            string text = ProcessType(ref builder, type, in options);
            return text ?? builder?.ToString() ?? string.Empty;
        }

        private static string ProcessType(ref StringBuilder builder, Type type, in DisplayNameOptions options)
        {
            string value;
            if (type.IsGenericType)
            {
                Type[] genericArguments = type.GetGenericArguments();
                if (builder == null)
                {
                    builder = new StringBuilder();
                }
                ProcessGenericType(builder, type, genericArguments, genericArguments.Length, in options);
            }
            else if (type.IsArray)
            {
                if (builder == null)
                {
                    builder = new StringBuilder();
                }
                ProcessArrayType(builder, type, in options);
            }
            else if (_builtInTypeNames.TryGetValue(type, out value))
            {
                if (builder == null)
                {
                    return value;
                }
                builder.Append(value);
            }
            else if (type.IsGenericParameter)
            {
                if (options.IncludeGenericParameterNames)
                {
                    if (builder == null)
                    {
                        return type.Name;
                    }
                    builder.Append(type.Name);
                }
            }
            else
            {
                string text = (options.FullName ? type.FullName : type.Name);
                if (builder == null)
                {
                    if (options.NestedTypeDelimiter != '+')
                    {
                        return text.Replace('+', options.NestedTypeDelimiter);
                    }
                    return text;
                }
                builder.Append(text);
                if (options.NestedTypeDelimiter != '+')
                {
                    builder.Replace('+', options.NestedTypeDelimiter, builder.Length - text.Length, text.Length);
                }
            }
            return null;
        }

        private static void ProcessArrayType(StringBuilder builder, Type type, in DisplayNameOptions options)
        {
            Type type2 = type;
            while (type2.IsArray)
            {
                type2 = type2.GetElementType();
            }
            ProcessType(ref builder, type2, in options);
            while (type.IsArray)
            {
                builder.Append('[');
                builder.Append(',', type.GetArrayRank() - 1);
                builder.Append(']');
                type = type.GetElementType();
            }
        }

        private static void ProcessGenericType(StringBuilder builder, Type type, Type[] genericArguments, int length, in DisplayNameOptions options)
        {
            int num = 0;
            if (type.IsNested)
            {
                num = type.DeclaringType.GetGenericArguments().Length;
            }
            if (options.FullName)
            {
                if (type.IsNested)
                {
                    ProcessGenericType(builder, type.DeclaringType, genericArguments, num, in options);
                    builder.Append(options.NestedTypeDelimiter);
                }
                else if (!string.IsNullOrEmpty(type.Namespace))
                {
                    builder.Append(type.Namespace);
                    builder.Append('.');
                }
            }
            int num2 = type.Name.IndexOf('`');
            if (num2 <= 0)
            {
                builder.Append(type.Name);
                return;
            }
            builder.Append(type.Name, 0, num2);
            if (!options.IncludeGenericParameters)
            {
                return;
            }
            builder.Append('<');
            for (int i = num; i < length; i++)
            {
                ProcessType(ref builder, genericArguments[i], in options);
                if (i + 1 != length)
                {
                    builder.Append(',');
                    if (options.IncludeGenericParameterNames || !genericArguments[i + 1].IsGenericParameter)
                    {
                        builder.Append(' ');
                    }
                }
            }
            builder.Append('>');
        }
    }
}
