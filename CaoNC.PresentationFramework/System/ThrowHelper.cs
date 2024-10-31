using CaoNC.System.Runtime.CompilerServices;
using System;
using System.Runtime.CompilerServices;

namespace CaoNC.System
{
    internal static class ThrowHelper
    {
        internal static void ThrowArgumentNullException(System.ExceptionArgument argument)
        {
            throw GetArgumentNullException(argument);
        }

        internal static void ThrowArgumentOutOfRangeException(System.ExceptionArgument argument)
        {
            throw GetArgumentOutOfRangeException(argument);
        }

        private static ArgumentNullException GetArgumentNullException(System.ExceptionArgument argument)
        {
            return new ArgumentNullException(GetArgumentName(argument));
        }

        private static ArgumentOutOfRangeException GetArgumentOutOfRangeException(System.ExceptionArgument argument)
        {
            return new ArgumentOutOfRangeException(GetArgumentName(argument));
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static string GetArgumentName(System.ExceptionArgument argument)
        {
            return argument.ToString();
        }

        internal static void ThrowIndexOutOfRangeException()
        {
            throw CreateIndexOutOfRangeException();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static Exception CreateIndexOutOfRangeException()
        {
            return new IndexOutOfRangeException();
        }

        internal static void ThrowArgumentException_InvalidTypeWithPointersNotSupported(Type type)
        {
            throw CreateArgumentException_InvalidTypeWithPointersNotSupported(type);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static Exception CreateArgumentException_InvalidTypeWithPointersNotSupported(Type type)
        {
            return new ArgumentException(SR.Format(SR.Argument_InvalidTypeWithPointersNotSupported, type));
        }

        internal static void ThrowArgumentException_DestinationTooShort()
        {
            throw CreateArgumentException_DestinationTooShort();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static Exception CreateArgumentException_DestinationTooShort()
        {
            return new ArgumentException(SR.Argument_DestinationTooShort);
        }

        internal static void ThrowArrayTypeMismatchException()
        {
            throw CreateArrayTypeMismatchException();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static Exception CreateArrayTypeMismatchException()
        {
            return new ArrayTypeMismatchException();
        }

        internal static void ThrowArgumentOutOfRangeException()
        {
            throw CreateArgumentOutOfRangeException();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static Exception CreateArgumentOutOfRangeException()
        {
            return new ArgumentOutOfRangeException();
        }

        internal static void ThrowArgumentException_OverlapAlignmentMismatch()
        {
            throw CreateArgumentException_OverlapAlignmentMismatch();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static Exception CreateArgumentException_OverlapAlignmentMismatch()
        {
            return new ArgumentException(System.SR.Argument_OverlapAlignmentMismatch);
        }

        internal static void ThrowIfNull(object argument, [CallerArgumentExpression("argument")] string paramName = null)
        {
            if (argument == null)
            {
                Throw(paramName);
            }
        }

        private static void Throw(string paramName)
        {
            throw new ArgumentNullException(paramName);
        }

    }
}
