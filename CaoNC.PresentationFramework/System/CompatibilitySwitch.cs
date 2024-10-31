using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Security;

namespace CaoNC.System
{
    public static class CompatibilitySwitch
    {
        [SecurityCritical]
        public static bool IsEnabled(string compatibilitySwitchName)
        {
            return IsEnabledInternalCall(compatibilitySwitchName, onlyDB: true);
        }

        [SecurityCritical]
        public static string GetValue(string compatibilitySwitchName)
        {
            return GetValueInternalCall(compatibilitySwitchName, onlyDB: true);
        }

        [SecurityCritical]
        internal static bool IsEnabledInternal(string compatibilitySwitchName)
        {
            return IsEnabledInternalCall(compatibilitySwitchName, onlyDB: false);
        }

        [SecurityCritical]
        internal static string GetValueInternal(string compatibilitySwitchName)
        {
            return GetValueInternalCall(compatibilitySwitchName, onlyDB: false);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        [SecurityCritical]
        internal static extern string GetAppContextOverridesInternalCall();

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern bool IsEnabledInternalCall(string compatibilitySwitchName, bool onlyDB);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern string GetValueInternalCall(string compatibilitySwitchName, bool onlyDB);
    }
}
