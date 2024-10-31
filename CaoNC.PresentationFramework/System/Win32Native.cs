using System;
using System.IO;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Principal;
using System.Text;
using System.Threading;
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;

namespace CaoNC.System
{
    [SecurityCritical]
    [SuppressUnmanagedCodeSecurity]
    internal static class Win32Native
    {
        [DllImport("advapi32.dll", BestFitMapping = false, CharSet = CharSet.Auto)]
        internal static extern int RegOpenKeyEx(SafeRegistryHandle hKey, string lpSubKey, int ulOptions, int samDesired, out SafeRegistryHandle hkResult);

        [DllImport("advapi32.dll", BestFitMapping = false, CharSet = CharSet.Auto)]
        internal static extern int RegOpenKeyEx(IntPtr hKey, string lpSubKey, int ulOptions, int samDesired, out SafeRegistryHandle hkResult);

        [DllImport("advapi32.dll", BestFitMapping = false, CharSet = CharSet.Auto)]
        internal static extern int RegQueryValueEx(SafeRegistryHandle hKey, string lpValueName, int[] lpReserved, ref int lpType, [Out] byte[] lpData, ref int lpcbData);

        [DllImport("advapi32.dll", BestFitMapping = false, CharSet = CharSet.Auto)]
        internal static extern int RegQueryValueEx(SafeRegistryHandle hKey, string lpValueName, int[] lpReserved, ref int lpType, ref int lpData, ref int lpcbData);

        [DllImport("advapi32.dll", BestFitMapping = false, CharSet = CharSet.Auto)]
        internal static extern int RegQueryValueEx(SafeRegistryHandle hKey, string lpValueName, int[] lpReserved, ref int lpType, ref long lpData, ref int lpcbData);

        [DllImport("advapi32.dll", BestFitMapping = false, CharSet = CharSet.Auto)]
        internal static extern int RegQueryValueEx(SafeRegistryHandle hKey, string lpValueName, int[] lpReserved, ref int lpType, [Out] char[] lpData, ref int lpcbData);

        [DllImport("advapi32.dll", BestFitMapping = false, CharSet = CharSet.Auto)]
        internal static extern int RegQueryValueEx(SafeRegistryHandle hKey, string lpValueName, int[] lpReserved, ref int lpType, [Out] StringBuilder lpData, ref int lpcbData);
    }
}
