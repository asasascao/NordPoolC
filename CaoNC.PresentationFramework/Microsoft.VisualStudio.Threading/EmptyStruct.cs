using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CaoNC.Microsoft.VisualStudio.Threading
{
    [StructLayout(LayoutKind.Sequential, Size = 1)]
    internal readonly struct EmptyStruct
    {
        //
        // 摘要:
        //     Gets an instance of the empty struct.
        internal static EmptyStruct Instance => default(EmptyStruct);
    }
}
