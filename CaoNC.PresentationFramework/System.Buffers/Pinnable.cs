using System.Runtime.InteropServices;

namespace CaoNC.System
{
    [StructLayout(LayoutKind.Sequential)]
    internal sealed class Pinnable<T>
    {
        public T Data;
    }
}
