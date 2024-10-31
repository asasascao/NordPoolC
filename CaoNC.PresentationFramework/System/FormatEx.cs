using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaoNC.System
{
    internal struct ParamsArray
    {
        private static readonly object[] oneArgArray = new object[1];

        private static readonly object[] twoArgArray = new object[2];

        private static readonly object[] threeArgArray = new object[3];

        private readonly object arg0;

        private readonly object arg1;

        private readonly object arg2;

        private readonly object[] args;

        public int Length => args.Length;

        public object this[int index]
        {
            get
            {
                if (index != 0)
                {
                    return GetAtSlow(index);
                }
                return arg0;
            }
        }

        public ParamsArray(object arg0)
        {
            this.arg0 = arg0;
            arg1 = null;
            arg2 = null;
            args = oneArgArray;
        }

        public ParamsArray(object arg0, object arg1)
        {
            this.arg0 = arg0;
            this.arg1 = arg1;
            arg2 = null;
            args = twoArgArray;
        }

        public ParamsArray(object arg0, object arg1, object arg2)
        {
            this.arg0 = arg0;
            this.arg1 = arg1;
            this.arg2 = arg2;
            args = threeArgArray;
        }

        public ParamsArray(object[] args)
        {
            int num = args.Length;
            arg0 = ((num > 0) ? args[0] : null);
            arg1 = ((num > 1) ? args[1] : null);
            arg2 = ((num > 2) ? args[2] : null);
            this.args = args;
        }

        private object GetAtSlow(int index)
        {
            if (index == 1) return arg1;
            else if (index == 2) return arg2;
            else return args[index];
        }
    }

    public static class StringBuilderEx
    {
        internal static StringBuilder AppendFormatHelper(this StringBuilder builder, IFormatProvider provider, string format, ParamsArray args)
        {
            if (format == null)
            {
                throw new ArgumentNullException("format");
            }
            int num = 0;
            int length = format.Length;
            char c = '\0';
            ICustomFormatter customFormatter = null;
            if (provider != null)
            {
                customFormatter = (ICustomFormatter)provider.GetFormat(typeof(ICustomFormatter));
            }
            while (true)
            {
                int num2 = num;
                int num3 = num;
                while (num < length)
                {
                    c = format[num];
                    num++;
                    if (c == '}')
                    {
                        if (num < length && format[num] == '}')
                        {
                            num++;
                        }
                        else
                        {
                            throw new FormatException(Environment.GetResourceString("Format_InvalidString"));
                        }
                    }
                    if (c == '{')
                    {
                        if (num >= length || format[num] != '{')
                        {
                            num--;
                            break;
                        }
                        num++;
                    }
                    builder.Append(c);
                }
                if (num == length)
                {
                    break;
                }
                num++;
                if (num == length || (c = format[num]) < '0' || c > '9')
                {
                    throw new FormatException(Environment.GetResourceString("Format_InvalidString"));
                }
                int num4 = 0;
                do
                {
                    num4 = num4 * 10 + c - 48;
                    num++;
                    if (num == length)
                    {
                        throw new FormatException(Environment.GetResourceString("Format_InvalidString"));
                    }
                    c = format[num];
                }
                while (c >= '0' && c <= '9' && num4 < 1000000);
                if (num4 >= args.Length)
                {
                    throw new FormatException(Environment.GetResourceString("Format_IndexOutOfRange"));
                }
                for (; num < length; num++)
                {
                    if ((c = format[num]) != ' ')
                    {
                        break;
                    }
                }
                bool flag = false;
                int num5 = 0;
                if (c == ',')
                {
                    for (num++; num < length && format[num] == ' '; num++)
                    {
                    }
                    if (num == length)
                    {
                        throw new FormatException(Environment.GetResourceString("Format_InvalidString"));
                    }
                    c = format[num];
                    if (c == '-')
                    {
                        flag = true;
                        num++;
                        if (num == length)
                        {
                            throw new FormatException(Environment.GetResourceString("Format_InvalidString"));
                        }
                        c = format[num];
                    }
                    if (c < '0' || c > '9')
                    {
                        throw new FormatException(Environment.GetResourceString("Format_InvalidString"));
                    }
                    do
                    {
                        num5 = num5 * 10 + c - 48;
                        num++;
                        if (num == length)
                        {
                            throw new FormatException(Environment.GetResourceString("Format_InvalidString"));
                        }
                        c = format[num];
                    }
                    while (c >= '0' && c <= '9' && num5 < 1000000);
                }
                for (; num < length; num++)
                {
                    if ((c = format[num]) != ' ')
                    {
                        break;
                    }
                }
                object obj = args[num4];
                StringBuilder stringBuilder = null;
                if (c == ':')
                {
                    num++;
                    num2 = num;
                    num3 = num;
                    while (true)
                    {
                        if (num == length)
                        {
                            throw new FormatException(Environment.GetResourceString("Format_InvalidString"));
                        }
                        c = format[num];
                        num++;
                        if (c == '{')
                        {
                            if (num < length && format[num] == '{')
                            {
                                num++;
                            }
                            else
                            {
                                throw new FormatException(Environment.GetResourceString("Format_InvalidString"));
                            }
                        }
                        else if (c == '}')
                        {
                            if (num >= length || format[num] != '}')
                            {
                                break;
                            }
                            num++;
                        }
                        if (stringBuilder == null)
                        {
                            stringBuilder = new StringBuilder();
                        }
                        stringBuilder.Append(c);
                    }
                    num--;
                }
                if (c != '}')
                {
                    throw new FormatException(Environment.GetResourceString("Format_InvalidString"));
                }
                num++;
                string text = null;
                string text2 = null;
                if (customFormatter != null)
                {
                    if (stringBuilder != null)
                    {
                        text = stringBuilder.ToString();
                    }
                    text2 = customFormatter.Format(text, obj, provider);
                }
                if (text2 == null)
                {
                    if (obj is IFormattable formattable)
                    {
                        if (text == null && stringBuilder != null)
                        {
                            text = stringBuilder.ToString();
                        }
                        text2 = formattable.ToString(text, provider);
                    }
                    else if (obj != null)
                    {
                        text2 = obj.ToString();
                    }
                }
                if (text2 == null)
                {
                    text2 = string.Empty;
                }
                int num6 = num5 - text2.Length;
                if (!flag && num6 > 0)
                {
                    stringBuilder.Append(' ', num6);
                }
                stringBuilder.Append(text2);
                if (flag && num6 > 0)
                {
                    stringBuilder.Append(' ', num6);
                }
            }
            return builder;
        }
    }

    public class FormatEx
    {
        [__DynamicallyInvokable]
        public static string Format(string format, object arg0)
        {
            return FormatHelper(null, format, new ParamsArray(arg0));
        }

        private static string FormatHelper(IFormatProvider provider, string format, ParamsArray args)
        {
            if ((object)format == null)
            {
                throw new ArgumentNullException("format");
            }
            return StringBuilderCache.GetStringAndRelease(StringBuilderCache.Acquire(format.Length + args.Length * 8).AppendFormatHelper(provider, format, args));
        }
    }
}
