using System;
using System.Runtime.CompilerServices;

namespace CaoNC.System.Threading
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
    [FriendAccessAllowed]
    internal sealed class FriendAccessAllowedAttribute : Attribute
    {
    }
}
