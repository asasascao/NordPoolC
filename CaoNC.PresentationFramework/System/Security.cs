using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Globalization;

namespace CaoNC.System
{
    internal class Environment
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        [SecurityCritical]
        internal static extern string GetResourceFromDefault(string key);

        [SecuritySafeCritical]
        internal static string GetResourceString(string key)
        {
            return GetResourceFromDefault(key);
        }

        [SecuritySafeCritical]
        internal static string GetResourceString(string key, params object[] values)
        {
            string resourceString = GetResourceString(key);
            return string.Format(CultureInfo.CurrentCulture, resourceString, values);
        }
    }
}
