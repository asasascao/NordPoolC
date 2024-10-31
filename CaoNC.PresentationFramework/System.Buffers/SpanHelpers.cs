using CaoNC.System.Numerics;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CaoNC.System
{
    internal static class SpanHelpers
    {
        internal struct ComparerComparable<T, TComparer> : IComparable<T> where TComparer : IComparer<T>
        {
            private readonly T _value;

            private readonly TComparer _comparer;

            public ComparerComparable(T value, TComparer comparer)
            {
                _value = value;
                _comparer = comparer;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public int CompareTo(T other)
            {
                TComparer comparer = _comparer;
                return comparer.Compare(_value, other);
            }
        }

        [StructLayout(LayoutKind.Sequential, Size = 64)]
        private struct Reg64
        {
        }

        [StructLayout(LayoutKind.Sequential, Size = 32)]
        private struct Reg32
        {
        }

        [StructLayout(LayoutKind.Sequential, Size = 16)]
        private struct Reg16
        {
        }

        public static class PerTypeValues<T>
        {
            public static readonly bool IsReferenceOrContainsReferences = IsReferenceOrContainsReferencesCore(typeof(T));

            public static readonly T[] EmptyArray = new T[0];

            public static readonly IntPtr ArrayAdjustment = MeasureArrayAdjustment();

            private static IntPtr MeasureArrayAdjustment()
            {
                T[] array = new T[1];
                return System.Runtime.CompilerServices.Unsafe.ByteOffset<T>(ref System.Runtime.CompilerServices.Unsafe.As<Pinnable<T>>((object)array).Data, ref array[0]);
            }
        }

        private const ulong XorPowerOfTwoToHighByte = 283686952306184uL;

        private const ulong XorPowerOfTwoToHighChar = 4295098372uL;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int BinarySearch<T, TComparable>(this ReadOnlySpan<T> span, TComparable comparable) where TComparable : IComparable<T>
        {
            if (comparable == null)
            {
                System.ThrowHelper.ThrowArgumentNullException(System.ExceptionArgument.comparable);
            }
            return BinarySearch(ref MemoryMarshal.GetReference(span), span.Length, comparable);
        }

        public static int BinarySearch<T, TComparable>(ref T spanStart, int length, TComparable comparable) where TComparable : IComparable<T>
        {
            int num = 0;
            int num2 = length - 1;
            while (num <= num2)
            {
                int num3 = (int)((uint)(num2 + num) >> 1);
                int num4 = comparable.CompareTo(System.Runtime.CompilerServices.Unsafe.Add<T>(ref spanStart, num3));
                if (num4 == 0)
                {
                    return num3;
                }
                if (num4 > 0)
                {
                    num = num3 + 1;
                }
                else
                {
                    num2 = num3 - 1;
                }
            }
            return ~num;
        }

        public static int IndexOf(ref byte searchSpace, int searchSpaceLength, ref byte value, int valueLength)
        {
            if (valueLength == 0)
            {
                return 0;
            }
            byte value2 = value;
            ref byte second = ref System.Runtime.CompilerServices.Unsafe.Add<byte>(ref value, 1);
            int num = valueLength - 1;
            int num2 = 0;
            while (true)
            {
                int num3 = searchSpaceLength - num2 - num;
                if (num3 <= 0)
                {
                    break;
                }
                int num4 = IndexOf(ref System.Runtime.CompilerServices.Unsafe.Add<byte>(ref searchSpace, num2), value2, num3);
                if (num4 == -1)
                {
                    break;
                }
                num2 += num4;
                if (SequenceEqual(ref System.Runtime.CompilerServices.Unsafe.Add<byte>(ref searchSpace, num2 + 1), ref second, num))
                {
                    return num2;
                }
                num2++;
            }
            return -1;
        }

        public static int IndexOfAny(ref byte searchSpace, int searchSpaceLength, ref byte value, int valueLength)
        {
            if (valueLength == 0)
            {
                return 0;
            }
            int num = -1;
            for (int i = 0; i < valueLength; i++)
            {
                int num2 = IndexOf(ref searchSpace, System.Runtime.CompilerServices.Unsafe.Add<byte>(ref value, i), searchSpaceLength);
                if ((uint)num2 < (uint)num)
                {
                    num = num2;
                    searchSpaceLength = num2;
                    if (num == 0)
                    {
                        break;
                    }
                }
            }
            return num;
        }

        public static int LastIndexOfAny(ref byte searchSpace, int searchSpaceLength, ref byte value, int valueLength)
        {
            if (valueLength == 0)
            {
                return 0;
            }
            int num = -1;
            for (int i = 0; i < valueLength; i++)
            {
                int num2 = LastIndexOf(ref searchSpace, System.Runtime.CompilerServices.Unsafe.Add<byte>(ref value, i), searchSpaceLength);
                if (num2 > num)
                {
                    num = num2;
                }
            }
            return num;
        }

        public unsafe static int IndexOf(ref byte searchSpace, byte value, int length)
        {
            //IL_01c6: Unknown result type (might be due to invalid IL or missing references)
            //IL_01cb: Unknown result type (might be due to invalid IL or missing references)
            //IL_01cf: Unknown result type (might be due to invalid IL or missing references)
            //IL_01d8: Unknown result type (might be due to invalid IL or missing references)
            //IL_01dd: Unknown result type (might be due to invalid IL or missing references)
            //IL_01e2: Unknown result type (might be due to invalid IL or missing references)
            //IL_01e4: Unknown result type (might be due to invalid IL or missing references)
            //IL_01e9: Unknown result type (might be due to invalid IL or missing references)
            //IL_01ed: Unknown result type (might be due to invalid IL or missing references)
            //IL_020b: Unknown result type (might be due to invalid IL or missing references)
            IntPtr intPtr = (IntPtr)0;
            IntPtr intPtr2 = (IntPtr)length;
            if (Vector.IsHardwareAccelerated && length >= Vector<byte>.Count * 2)
            {
                int num = (int)System.Runtime.CompilerServices.Unsafe.AsPointer<byte>(ref searchSpace) & (Vector<byte>.Count - 1);
                intPtr2 = (IntPtr)((Vector<byte>.Count - num) & (Vector<byte>.Count - 1));
            }
            while (true)
            {
                if ((uint)(void*)intPtr2 >= (uint)8u)
                {
                    intPtr2 -= 8;
                    if (value == System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr))
                    {
                        goto IL_0242;
                    }
                    if (value == System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr + 1))
                    {
                        goto IL_024a;
                    }
                    if (value == System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr + 2))
                    {
                        goto IL_0258;
                    }
                    if (value != System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr + 3))
                    {
                        if (value != System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr + 4))
                        {
                            if (value != System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr + 5))
                            {
                                if (value != System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr + 6))
                                {
                                    if (value == System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr + 7))
                                    {
                                        break;
                                    }
                                    intPtr += 8;
                                    continue;
                                }
                                return (int)(void*)(intPtr + 6);
                            }
                            return (int)(void*)(intPtr + 5);
                        }
                        return (int)(void*)(intPtr + 4);
                    }
                    goto IL_0266;
                }
                if ((uint)(void*)intPtr2 >= (uint)4u)
                {
                    intPtr2 -= 4;
                    if (value == System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr))
                    {
                        goto IL_0242;
                    }
                    if (value == System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr + 1))
                    {
                        goto IL_024a;
                    }
                    if (value == System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr + 2))
                    {
                        goto IL_0258;
                    }
                    if (value == System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr + 3))
                    {
                        goto IL_0266;
                    }
                    intPtr += 4;
                }
                while ((void*)intPtr2 != null)
                {
                    intPtr2 -= 1;
                    if (value != System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr))
                    {
                        intPtr += 1;
                        continue;
                    }
                    goto IL_0242;
                }
                if (Vector.IsHardwareAccelerated && (int)(void*)intPtr < length)
                {
                    intPtr2 = (IntPtr)((length - (int)(void*)intPtr) & ~(Vector<byte>.Count - 1));
                    Vector<byte> vector = GetVector(value);
                    for (; (void*)intPtr2 > (void*)intPtr; intPtr += Vector<byte>.Count)
                    {
                        Vector<byte> val = Vector.Equals<byte>(vector, System.Runtime.CompilerServices.Unsafe.ReadUnaligned<Vector<byte>>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr)));
                        if (!Vector<byte>.Zero.Equals(val))
                        {
                            return (int)(void*)intPtr + LocateFirstFoundByte(val);
                        }
                    }
                    if ((int)(void*)intPtr < length)
                    {
                        intPtr2 = (IntPtr)(length - (int)(void*)intPtr);
                        continue;
                    }
                }
                return -1;
                IL_0266:
                return (int)(void*)(intPtr + 3);
                IL_0242:
                return (int)(void*)intPtr;
                IL_0258:
                return (int)(void*)(intPtr + 2);
                IL_024a:
                return (int)(void*)(intPtr + 1);
            }
            return (int)(void*)(intPtr + 7);
        }

        public static int LastIndexOf(ref byte searchSpace, int searchSpaceLength, ref byte value, int valueLength)
        {
            if (valueLength == 0)
            {
                return 0;
            }
            byte value2 = value;
            ref byte second = ref System.Runtime.CompilerServices.Unsafe.Add<byte>(ref value, 1);
            int num = valueLength - 1;
            int num2 = 0;
            while (true)
            {
                int num3 = searchSpaceLength - num2 - num;
                if (num3 <= 0)
                {
                    break;
                }
                int num4 = LastIndexOf(ref searchSpace, value2, num3);
                if (num4 == -1)
                {
                    break;
                }
                if (SequenceEqual(ref System.Runtime.CompilerServices.Unsafe.Add<byte>(ref searchSpace, num4 + 1), ref second, num))
                {
                    return num4;
                }
                num2 += num3 - num4;
            }
            return -1;
        }

        public unsafe static int LastIndexOf(ref byte searchSpace, byte value, int length)
        {
            //IL_01c8: Unknown result type (might be due to invalid IL or missing references)
            //IL_01cd: Unknown result type (might be due to invalid IL or missing references)
            //IL_01d1: Unknown result type (might be due to invalid IL or missing references)
            //IL_01e4: Unknown result type (might be due to invalid IL or missing references)
            //IL_01e9: Unknown result type (might be due to invalid IL or missing references)
            //IL_01ee: Unknown result type (might be due to invalid IL or missing references)
            //IL_01f0: Unknown result type (might be due to invalid IL or missing references)
            //IL_01f5: Unknown result type (might be due to invalid IL or missing references)
            //IL_01f9: Unknown result type (might be due to invalid IL or missing references)
            //IL_0228: Unknown result type (might be due to invalid IL or missing references)
            IntPtr intPtr = (IntPtr)length;
            IntPtr intPtr2 = (IntPtr)length;
            if (Vector.IsHardwareAccelerated && length >= Vector<byte>.Count * 2)
            {
                int num = (int)System.Runtime.CompilerServices.Unsafe.AsPointer<byte>(ref searchSpace) & (Vector<byte>.Count - 1);
                intPtr2 = (IntPtr)(((length & (Vector<byte>.Count - 1)) + num) & (Vector<byte>.Count - 1));
            }
            while (true)
            {
                if ((uint)(void*)intPtr2 >= (uint)8u)
                {
                    intPtr2 -= 8;
                    intPtr -= 8;
                    if (value == System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr + 7))
                    {
                        break;
                    }
                    if (value != System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr + 6))
                    {
                        if (value != System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr + 5))
                        {
                            if (value != System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr + 4))
                            {
                                if (value != System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr + 3))
                                {
                                    if (value != System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr + 2))
                                    {
                                        if (value != System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr + 1))
                                        {
                                            if (value != System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr))
                                            {
                                                continue;
                                            }
                                            goto IL_0254;
                                        }
                                        goto IL_025c;
                                    }
                                    goto IL_026a;
                                }
                                goto IL_0278;
                            }
                            return (int)(void*)(intPtr + 4);
                        }
                        return (int)(void*)(intPtr + 5);
                    }
                    return (int)(void*)(intPtr + 6);
                }
                if ((uint)(void*)intPtr2 >= (uint)4u)
                {
                    intPtr2 -= 4;
                    intPtr -= 4;
                    if (value == System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr + 3))
                    {
                        goto IL_0278;
                    }
                    if (value == System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr + 2))
                    {
                        goto IL_026a;
                    }
                    if (value == System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr + 1))
                    {
                        goto IL_025c;
                    }
                    if (value == System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr))
                    {
                        goto IL_0254;
                    }
                }
                while ((void*)intPtr2 != null)
                {
                    intPtr2 -= 1;
                    intPtr -= 1;
                    if (value != System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr))
                    {
                        continue;
                    }
                    goto IL_0254;
                }
                if (Vector.IsHardwareAccelerated && (void*)intPtr != null)
                {
                    intPtr2 = (IntPtr)((int)(void*)intPtr & ~(Vector<byte>.Count - 1));
                    Vector<byte> vector = GetVector(value);
                    for (; (uint)(void*)intPtr2 > (uint)(Vector<byte>.Count - 1); intPtr2 -= Vector<byte>.Count)
                    {
                        Vector<byte> val = Vector.Equals<byte>(vector, System.Runtime.CompilerServices.Unsafe.ReadUnaligned<Vector<byte>>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr - Vector<byte>.Count)));
                        if (Vector<byte>.Zero.Equals(val))
                        {
                            intPtr -= Vector<byte>.Count;
                            continue;
                        }
                        return (int)intPtr - Vector<byte>.Count + LocateLastFoundByte(val);
                    }
                    if ((void*)intPtr != null)
                    {
                        intPtr2 = intPtr;
                        continue;
                    }
                }
                return -1;
                IL_0254:
                return (int)(void*)intPtr;
                IL_026a:
                return (int)(void*)(intPtr + 2);
                IL_0278:
                return (int)(void*)(intPtr + 3);
                IL_025c:
                return (int)(void*)(intPtr + 1);
            }
            return (int)(void*)(intPtr + 7);
        }

        public unsafe static int IndexOfAny(ref byte searchSpace, byte value0, byte value1, int length)
        {
            //IL_0269: Unknown result type (might be due to invalid IL or missing references)
            //IL_026e: Unknown result type (might be due to invalid IL or missing references)
            //IL_0271: Unknown result type (might be due to invalid IL or missing references)
            //IL_0276: Unknown result type (might be due to invalid IL or missing references)
            //IL_0281: Unknown result type (might be due to invalid IL or missing references)
            //IL_0286: Unknown result type (might be due to invalid IL or missing references)
            //IL_0288: Unknown result type (might be due to invalid IL or missing references)
            //IL_028a: Unknown result type (might be due to invalid IL or missing references)
            //IL_028c: Unknown result type (might be due to invalid IL or missing references)
            //IL_0291: Unknown result type (might be due to invalid IL or missing references)
            //IL_0293: Unknown result type (might be due to invalid IL or missing references)
            //IL_0295: Unknown result type (might be due to invalid IL or missing references)
            //IL_029a: Unknown result type (might be due to invalid IL or missing references)
            //IL_029f: Unknown result type (might be due to invalid IL or missing references)
            //IL_02a1: Unknown result type (might be due to invalid IL or missing references)
            //IL_02a6: Unknown result type (might be due to invalid IL or missing references)
            //IL_02aa: Unknown result type (might be due to invalid IL or missing references)
            //IL_02c8: Unknown result type (might be due to invalid IL or missing references)
            IntPtr intPtr = (IntPtr)0;
            IntPtr intPtr2 = (IntPtr)length;
            if (Vector.IsHardwareAccelerated && length >= Vector<byte>.Count * 2)
            {
                int num = (int)System.Runtime.CompilerServices.Unsafe.AsPointer<byte>(ref searchSpace) & (Vector<byte>.Count - 1);
                intPtr2 = (IntPtr)((Vector<byte>.Count - num) & (Vector<byte>.Count - 1));
            }
            while (true)
            {
                if ((uint)(void*)intPtr2 >= (uint)8u)
                {
                    intPtr2 -= 8;
                    uint num2 = System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr);
                    if (value0 == num2 || value1 == num2)
                    {
                        goto IL_02ff;
                    }
                    num2 = System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr + 1);
                    if (value0 == num2 || value1 == num2)
                    {
                        goto IL_0307;
                    }
                    num2 = System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr + 2);
                    if (value0 == num2 || value1 == num2)
                    {
                        goto IL_0315;
                    }
                    num2 = System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr + 3);
                    if (value0 != num2 && value1 != num2)
                    {
                        num2 = System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr + 4);
                        if (value0 != num2 && value1 != num2)
                        {
                            num2 = System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr + 5);
                            if (value0 != num2 && value1 != num2)
                            {
                                num2 = System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr + 6);
                                if (value0 != num2 && value1 != num2)
                                {
                                    num2 = System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr + 7);
                                    if (value0 == num2 || value1 == num2)
                                    {
                                        break;
                                    }
                                    intPtr += 8;
                                    continue;
                                }
                                return (int)(void*)(intPtr + 6);
                            }
                            return (int)(void*)(intPtr + 5);
                        }
                        return (int)(void*)(intPtr + 4);
                    }
                    goto IL_0323;
                }
                if ((uint)(void*)intPtr2 >= (uint)4u)
                {
                    intPtr2 -= 4;
                    uint num2 = System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr);
                    if (value0 == num2 || value1 == num2)
                    {
                        goto IL_02ff;
                    }
                    num2 = System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr + 1);
                    if (value0 == num2 || value1 == num2)
                    {
                        goto IL_0307;
                    }
                    num2 = System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr + 2);
                    if (value0 == num2 || value1 == num2)
                    {
                        goto IL_0315;
                    }
                    num2 = System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr + 3);
                    if (value0 == num2 || value1 == num2)
                    {
                        goto IL_0323;
                    }
                    intPtr += 4;
                }
                while ((void*)intPtr2 != null)
                {
                    intPtr2 -= 1;
                    uint num2 = System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr);
                    if (value0 != num2 && value1 != num2)
                    {
                        intPtr += 1;
                        continue;
                    }
                    goto IL_02ff;
                }
                if (Vector.IsHardwareAccelerated && (int)(void*)intPtr < length)
                {
                    intPtr2 = (IntPtr)((length - (int)(void*)intPtr) & ~(Vector<byte>.Count - 1));
                    Vector<byte> vector = GetVector(value0);
                    Vector<byte> vector2 = GetVector(value1);
                    for (; (void*)intPtr2 > (void*)intPtr; intPtr += Vector<byte>.Count)
                    {
                        Vector<byte> val = System.Runtime.CompilerServices.Unsafe.ReadUnaligned<Vector<byte>>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr));
                        Vector<byte> val2 = Vector.BitwiseOr<byte>(Vector.Equals<byte>(val, vector), Vector.Equals<byte>(val, vector2));
                        if (!Vector<byte>.Zero.Equals(val2))
                        {
                            return (int)(void*)intPtr + LocateFirstFoundByte(val2);
                        }
                    }
                    if ((int)(void*)intPtr < length)
                    {
                        intPtr2 = (IntPtr)(length - (int)(void*)intPtr);
                        continue;
                    }
                }
                return -1;
                IL_02ff:
                return (int)(void*)intPtr;
                IL_0315:
                return (int)(void*)(intPtr + 2);
                IL_0307:
                return (int)(void*)(intPtr + 1);
                IL_0323:
                return (int)(void*)(intPtr + 3);
            }
            return (int)(void*)(intPtr + 7);
        }

        public unsafe static int IndexOfAny(ref byte searchSpace, byte value0, byte value1, byte value2, int length)
        {
            //IL_02e3: Unknown result type (might be due to invalid IL or missing references)
            //IL_02e8: Unknown result type (might be due to invalid IL or missing references)
            //IL_02eb: Unknown result type (might be due to invalid IL or missing references)
            //IL_02f0: Unknown result type (might be due to invalid IL or missing references)
            //IL_02f3: Unknown result type (might be due to invalid IL or missing references)
            //IL_02f8: Unknown result type (might be due to invalid IL or missing references)
            //IL_0303: Unknown result type (might be due to invalid IL or missing references)
            //IL_0308: Unknown result type (might be due to invalid IL or missing references)
            //IL_030a: Unknown result type (might be due to invalid IL or missing references)
            //IL_030c: Unknown result type (might be due to invalid IL or missing references)
            //IL_030e: Unknown result type (might be due to invalid IL or missing references)
            //IL_0313: Unknown result type (might be due to invalid IL or missing references)
            //IL_0315: Unknown result type (might be due to invalid IL or missing references)
            //IL_0317: Unknown result type (might be due to invalid IL or missing references)
            //IL_031c: Unknown result type (might be due to invalid IL or missing references)
            //IL_0321: Unknown result type (might be due to invalid IL or missing references)
            //IL_0323: Unknown result type (might be due to invalid IL or missing references)
            //IL_0325: Unknown result type (might be due to invalid IL or missing references)
            //IL_032a: Unknown result type (might be due to invalid IL or missing references)
            //IL_032f: Unknown result type (might be due to invalid IL or missing references)
            //IL_0331: Unknown result type (might be due to invalid IL or missing references)
            //IL_0336: Unknown result type (might be due to invalid IL or missing references)
            //IL_033a: Unknown result type (might be due to invalid IL or missing references)
            //IL_0358: Unknown result type (might be due to invalid IL or missing references)
            IntPtr intPtr = (IntPtr)0;
            IntPtr intPtr2 = (IntPtr)length;
            if (Vector.IsHardwareAccelerated && length >= Vector<byte>.Count * 2)
            {
                int num = (int)System.Runtime.CompilerServices.Unsafe.AsPointer<byte>(ref searchSpace) & (Vector<byte>.Count - 1);
                intPtr2 = (IntPtr)((Vector<byte>.Count - num) & (Vector<byte>.Count - 1));
            }
            while (true)
            {
                if ((uint)(void*)intPtr2 >= (uint)8u)
                {
                    intPtr2 -= 8;
                    uint num2 = System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr);
                    if (value0 == num2 || value1 == num2 || value2 == num2)
                    {
                        goto IL_0393;
                    }
                    num2 = System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr + 1);
                    if (value0 == num2 || value1 == num2 || value2 == num2)
                    {
                        goto IL_039b;
                    }
                    num2 = System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr + 2);
                    if (value0 == num2 || value1 == num2 || value2 == num2)
                    {
                        goto IL_03a9;
                    }
                    num2 = System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr + 3);
                    if (value0 != num2 && value1 != num2 && value2 != num2)
                    {
                        num2 = System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr + 4);
                        if (value0 != num2 && value1 != num2 && value2 != num2)
                        {
                            num2 = System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr + 5);
                            if (value0 != num2 && value1 != num2 && value2 != num2)
                            {
                                num2 = System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr + 6);
                                if (value0 != num2 && value1 != num2 && value2 != num2)
                                {
                                    num2 = System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr + 7);
                                    if (value0 == num2 || value1 == num2 || value2 == num2)
                                    {
                                        break;
                                    }
                                    intPtr += 8;
                                    continue;
                                }
                                return (int)(void*)(intPtr + 6);
                            }
                            return (int)(void*)(intPtr + 5);
                        }
                        return (int)(void*)(intPtr + 4);
                    }
                    goto IL_03b7;
                }
                if ((uint)(void*)intPtr2 >= (uint)4u)
                {
                    intPtr2 -= 4;
                    uint num2 = System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr);
                    if (value0 == num2 || value1 == num2 || value2 == num2)
                    {
                        goto IL_0393;
                    }
                    num2 = System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr + 1);
                    if (value0 == num2 || value1 == num2 || value2 == num2)
                    {
                        goto IL_039b;
                    }
                    num2 = System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr + 2);
                    if (value0 == num2 || value1 == num2 || value2 == num2)
                    {
                        goto IL_03a9;
                    }
                    num2 = System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr + 3);
                    if (value0 == num2 || value1 == num2 || value2 == num2)
                    {
                        goto IL_03b7;
                    }
                    intPtr += 4;
                }
                while ((void*)intPtr2 != null)
                {
                    intPtr2 -= 1;
                    uint num2 = System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr);
                    if (value0 != num2 && value1 != num2 && value2 != num2)
                    {
                        intPtr += 1;
                        continue;
                    }
                    goto IL_0393;
                }
                if (Vector.IsHardwareAccelerated && (int)(void*)intPtr < length)
                {
                    intPtr2 = (IntPtr)((length - (int)(void*)intPtr) & ~(Vector<byte>.Count - 1));
                    Vector<byte> vector = GetVector(value0);
                    Vector<byte> vector2 = GetVector(value1);
                    Vector<byte> vector3 = GetVector(value2);
                    for (; (void*)intPtr2 > (void*)intPtr; intPtr += Vector<byte>.Count)
                    {
                        Vector<byte> val = System.Runtime.CompilerServices.Unsafe.ReadUnaligned<Vector<byte>>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr));
                        Vector<byte> val2 = Vector.BitwiseOr<byte>(Vector.BitwiseOr<byte>(Vector.Equals<byte>(val, vector), Vector.Equals<byte>(val, vector2)), Vector.Equals<byte>(val, vector3));
                        if (!Vector<byte>.Zero.Equals(val2))
                        {
                            return (int)(void*)intPtr + LocateFirstFoundByte(val2);
                        }
                    }
                    if ((int)(void*)intPtr < length)
                    {
                        intPtr2 = (IntPtr)(length - (int)(void*)intPtr);
                        continue;
                    }
                }
                return -1;
                IL_0393:
                return (int)(void*)intPtr;
                IL_039b:
                return (int)(void*)(intPtr + 1);
                IL_03b7:
                return (int)(void*)(intPtr + 3);
                IL_03a9:
                return (int)(void*)(intPtr + 2);
            }
            return (int)(void*)(intPtr + 7);
        }

        public unsafe static int LastIndexOfAny(ref byte searchSpace, byte value0, byte value1, int length)
        {
            //IL_026b: Unknown result type (might be due to invalid IL or missing references)
            //IL_0270: Unknown result type (might be due to invalid IL or missing references)
            //IL_0273: Unknown result type (might be due to invalid IL or missing references)
            //IL_0278: Unknown result type (might be due to invalid IL or missing references)
            //IL_028d: Unknown result type (might be due to invalid IL or missing references)
            //IL_0292: Unknown result type (might be due to invalid IL or missing references)
            //IL_0294: Unknown result type (might be due to invalid IL or missing references)
            //IL_0296: Unknown result type (might be due to invalid IL or missing references)
            //IL_0298: Unknown result type (might be due to invalid IL or missing references)
            //IL_029d: Unknown result type (might be due to invalid IL or missing references)
            //IL_029f: Unknown result type (might be due to invalid IL or missing references)
            //IL_02a1: Unknown result type (might be due to invalid IL or missing references)
            //IL_02a6: Unknown result type (might be due to invalid IL or missing references)
            //IL_02ab: Unknown result type (might be due to invalid IL or missing references)
            //IL_02ad: Unknown result type (might be due to invalid IL or missing references)
            //IL_02b2: Unknown result type (might be due to invalid IL or missing references)
            //IL_02b6: Unknown result type (might be due to invalid IL or missing references)
            //IL_02e5: Unknown result type (might be due to invalid IL or missing references)
            IntPtr intPtr = (IntPtr)length;
            IntPtr intPtr2 = (IntPtr)length;
            if (Vector.IsHardwareAccelerated && length >= Vector<byte>.Count * 2)
            {
                int num = (int)System.Runtime.CompilerServices.Unsafe.AsPointer<byte>(ref searchSpace) & (Vector<byte>.Count - 1);
                intPtr2 = (IntPtr)(((length & (Vector<byte>.Count - 1)) + num) & (Vector<byte>.Count - 1));
            }
            while (true)
            {
                if ((uint)(void*)intPtr2 >= (uint)8u)
                {
                    intPtr2 -= 8;
                    intPtr -= 8;
                    uint num2 = System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr + 7);
                    if (value0 == num2 || value1 == num2)
                    {
                        break;
                    }
                    num2 = System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr + 6);
                    if (value0 != num2 && value1 != num2)
                    {
                        num2 = System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr + 5);
                        if (value0 != num2 && value1 != num2)
                        {
                            num2 = System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr + 4);
                            if (value0 != num2 && value1 != num2)
                            {
                                num2 = System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr + 3);
                                if (value0 != num2 && value1 != num2)
                                {
                                    num2 = System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr + 2);
                                    if (value0 != num2 && value1 != num2)
                                    {
                                        num2 = System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr + 1);
                                        if (value0 != num2 && value1 != num2)
                                        {
                                            num2 = System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr);
                                            if (value0 != num2 && value1 != num2)
                                            {
                                                continue;
                                            }
                                            goto IL_0314;
                                        }
                                        goto IL_031c;
                                    }
                                    goto IL_032a;
                                }
                                goto IL_0338;
                            }
                            return (int)(void*)(intPtr + 4);
                        }
                        return (int)(void*)(intPtr + 5);
                    }
                    return (int)(void*)(intPtr + 6);
                }
                if ((uint)(void*)intPtr2 >= (uint)4u)
                {
                    intPtr2 -= 4;
                    intPtr -= 4;
                    uint num2 = System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr + 3);
                    if (value0 == num2 || value1 == num2)
                    {
                        goto IL_0338;
                    }
                    num2 = System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr + 2);
                    if (value0 == num2 || value1 == num2)
                    {
                        goto IL_032a;
                    }
                    num2 = System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr + 1);
                    if (value0 == num2 || value1 == num2)
                    {
                        goto IL_031c;
                    }
                    num2 = System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr);
                    if (value0 == num2 || value1 == num2)
                    {
                        goto IL_0314;
                    }
                }
                while ((void*)intPtr2 != null)
                {
                    intPtr2 -= 1;
                    intPtr -= 1;
                    uint num2 = System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr);
                    if (value0 != num2 && value1 != num2)
                    {
                        continue;
                    }
                    goto IL_0314;
                }
                if (Vector.IsHardwareAccelerated && (void*)intPtr != null)
                {
                    intPtr2 = (IntPtr)((int)(void*)intPtr & ~(Vector<byte>.Count - 1));
                    Vector<byte> vector = GetVector(value0);
                    Vector<byte> vector2 = GetVector(value1);
                    for (; (uint)(void*)intPtr2 > (uint)(Vector<byte>.Count - 1); intPtr2 -= Vector<byte>.Count)
                    {
                        Vector<byte> val = System.Runtime.CompilerServices.Unsafe.ReadUnaligned<Vector<byte>>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr - Vector<byte>.Count));
                        Vector<byte> val2 = Vector.BitwiseOr<byte>(Vector.Equals<byte>(val, vector), Vector.Equals<byte>(val, vector2));
                        if (Vector<byte>.Zero.Equals(val2))
                        {
                            intPtr -= Vector<byte>.Count;
                            continue;
                        }
                        return (int)intPtr - Vector<byte>.Count + LocateLastFoundByte(val2);
                    }
                    if ((void*)intPtr != null)
                    {
                        intPtr2 = intPtr;
                        continue;
                    }
                }
                return -1;
                IL_0314:
                return (int)(void*)intPtr;
                IL_0338:
                return (int)(void*)(intPtr + 3);
                IL_031c:
                return (int)(void*)(intPtr + 1);
                IL_032a:
                return (int)(void*)(intPtr + 2);
            }
            return (int)(void*)(intPtr + 7);
        }

        public unsafe static int LastIndexOfAny(ref byte searchSpace, byte value0, byte value1, byte value2, int length)
        {
            //IL_02e5: Unknown result type (might be due to invalid IL or missing references)
            //IL_02ea: Unknown result type (might be due to invalid IL or missing references)
            //IL_02ed: Unknown result type (might be due to invalid IL or missing references)
            //IL_02f2: Unknown result type (might be due to invalid IL or missing references)
            //IL_02f5: Unknown result type (might be due to invalid IL or missing references)
            //IL_02fa: Unknown result type (might be due to invalid IL or missing references)
            //IL_0312: Unknown result type (might be due to invalid IL or missing references)
            //IL_0317: Unknown result type (might be due to invalid IL or missing references)
            //IL_0319: Unknown result type (might be due to invalid IL or missing references)
            //IL_031b: Unknown result type (might be due to invalid IL or missing references)
            //IL_031d: Unknown result type (might be due to invalid IL or missing references)
            //IL_0322: Unknown result type (might be due to invalid IL or missing references)
            //IL_0324: Unknown result type (might be due to invalid IL or missing references)
            //IL_0326: Unknown result type (might be due to invalid IL or missing references)
            //IL_032b: Unknown result type (might be due to invalid IL or missing references)
            //IL_0330: Unknown result type (might be due to invalid IL or missing references)
            //IL_0332: Unknown result type (might be due to invalid IL or missing references)
            //IL_0334: Unknown result type (might be due to invalid IL or missing references)
            //IL_0339: Unknown result type (might be due to invalid IL or missing references)
            //IL_033e: Unknown result type (might be due to invalid IL or missing references)
            //IL_0340: Unknown result type (might be due to invalid IL or missing references)
            //IL_0345: Unknown result type (might be due to invalid IL or missing references)
            //IL_0349: Unknown result type (might be due to invalid IL or missing references)
            //IL_037a: Unknown result type (might be due to invalid IL or missing references)
            IntPtr intPtr = (IntPtr)length;
            IntPtr intPtr2 = (IntPtr)length;
            if (Vector.IsHardwareAccelerated && length >= Vector<byte>.Count * 2)
            {
                int num = (int)System.Runtime.CompilerServices.Unsafe.AsPointer<byte>(ref searchSpace) & (Vector<byte>.Count - 1);
                intPtr2 = (IntPtr)(((length & (Vector<byte>.Count - 1)) + num) & (Vector<byte>.Count - 1));
            }
            while (true)
            {
                if ((uint)(void*)intPtr2 >= (uint)8u)
                {
                    intPtr2 -= 8;
                    intPtr -= 8;
                    uint num2 = System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr + 7);
                    if (value0 == num2 || value1 == num2 || value2 == num2)
                    {
                        break;
                    }
                    num2 = System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr + 6);
                    if (value0 != num2 && value1 != num2 && value2 != num2)
                    {
                        num2 = System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr + 5);
                        if (value0 != num2 && value1 != num2 && value2 != num2)
                        {
                            num2 = System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr + 4);
                            if (value0 != num2 && value1 != num2 && value2 != num2)
                            {
                                num2 = System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr + 3);
                                if (value0 != num2 && value1 != num2 && value2 != num2)
                                {
                                    num2 = System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr + 2);
                                    if (value0 != num2 && value1 != num2 && value2 != num2)
                                    {
                                        num2 = System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr + 1);
                                        if (value0 != num2 && value1 != num2 && value2 != num2)
                                        {
                                            num2 = System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr);
                                            if (value0 != num2 && value1 != num2 && value2 != num2)
                                            {
                                                continue;
                                            }
                                            goto IL_03ab;
                                        }
                                        goto IL_03b3;
                                    }
                                    goto IL_03c1;
                                }
                                goto IL_03cf;
                            }
                            return (int)(void*)(intPtr + 4);
                        }
                        return (int)(void*)(intPtr + 5);
                    }
                    return (int)(void*)(intPtr + 6);
                }
                if ((uint)(void*)intPtr2 >= (uint)4u)
                {
                    intPtr2 -= 4;
                    intPtr -= 4;
                    uint num2 = System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr + 3);
                    if (value0 == num2 || value1 == num2 || value2 == num2)
                    {
                        goto IL_03cf;
                    }
                    num2 = System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr + 2);
                    if (value0 == num2 || value1 == num2 || value2 == num2)
                    {
                        goto IL_03c1;
                    }
                    num2 = System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr + 1);
                    if (value0 == num2 || value1 == num2 || value2 == num2)
                    {
                        goto IL_03b3;
                    }
                    num2 = System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr);
                    if (value0 == num2 || value1 == num2 || value2 == num2)
                    {
                        goto IL_03ab;
                    }
                }
                while ((void*)intPtr2 != null)
                {
                    intPtr2 -= 1;
                    intPtr -= 1;
                    uint num2 = System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr);
                    if (value0 != num2 && value1 != num2 && value2 != num2)
                    {
                        continue;
                    }
                    goto IL_03ab;
                }
                if (Vector.IsHardwareAccelerated && (void*)intPtr != null)
                {
                    intPtr2 = (IntPtr)((int)(void*)intPtr & ~(Vector<byte>.Count - 1));
                    Vector<byte> vector = GetVector(value0);
                    Vector<byte> vector2 = GetVector(value1);
                    Vector<byte> vector3 = GetVector(value2);
                    for (; (uint)(void*)intPtr2 > (uint)(Vector<byte>.Count - 1); intPtr2 -= Vector<byte>.Count)
                    {
                        Vector<byte> val = System.Runtime.CompilerServices.Unsafe.ReadUnaligned<Vector<byte>>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref searchSpace, intPtr - Vector<byte>.Count));
                        Vector<byte> val2 = Vector.BitwiseOr<byte>(Vector.BitwiseOr<byte>(Vector.Equals<byte>(val, vector), Vector.Equals<byte>(val, vector2)), Vector.Equals<byte>(val, vector3));
                        if (Vector<byte>.Zero.Equals(val2))
                        {
                            intPtr -= Vector<byte>.Count;
                            continue;
                        }
                        return (int)intPtr - Vector<byte>.Count + LocateLastFoundByte(val2);
                    }
                    if ((void*)intPtr != null)
                    {
                        intPtr2 = intPtr;
                        continue;
                    }
                }
                return -1;
                IL_03ab:
                return (int)(void*)intPtr;
                IL_03cf:
                return (int)(void*)(intPtr + 3);
                IL_03c1:
                return (int)(void*)(intPtr + 2);
                IL_03b3:
                return (int)(void*)(intPtr + 1);
            }
            return (int)(void*)(intPtr + 7);
        }

        public unsafe static bool SequenceEqual(ref byte first, ref byte second, uint length)
        {
            //IL_0049: Unknown result type (might be due to invalid IL or missing references)
            //IL_0055: Unknown result type (might be due to invalid IL or missing references)
            //IL_0085: Unknown result type (might be due to invalid IL or missing references)
            //IL_0091: Unknown result type (might be due to invalid IL or missing references)
            if (System.Runtime.CompilerServices.Unsafe.AreSame<byte>(ref first, ref second))
            {
                goto IL_013d;
            }
            IntPtr intPtr = (IntPtr)0;
            IntPtr intPtr2 = (IntPtr)(void*)length;
            if (Vector.IsHardwareAccelerated && (uint)(void*)intPtr2 >= (uint)Vector<byte>.Count)
            {
                intPtr2 -= Vector<byte>.Count;
                while (true)
                {
                    if ((void*)intPtr2 > (void*)intPtr)
                    {
                        if (System.Runtime.CompilerServices.Unsafe.ReadUnaligned<Vector<byte>>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref first, intPtr)) != System.Runtime.CompilerServices.Unsafe.ReadUnaligned<Vector<byte>>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref second, intPtr)))
                        {
                            break;
                        }
                        intPtr += Vector<byte>.Count;
                        continue;
                    }
                    return System.Runtime.CompilerServices.Unsafe.ReadUnaligned<Vector<byte>>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref first, intPtr2)) == System.Runtime.CompilerServices.Unsafe.ReadUnaligned<Vector<byte>>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref second, intPtr2));
                }
            }
            else
            {
                if ((uint)(void*)intPtr2 < (uint)sizeof(UIntPtr))
                {
                    while ((void*)intPtr2 > (void*)intPtr)
                    {
                        if (System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref first, intPtr) == System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref second, intPtr))
                        {
                            intPtr += 1;
                            continue;
                        }
                        goto IL_013f;
                    }
                    goto IL_013d;
                }
                intPtr2 -= sizeof(UIntPtr);
                while (true)
                {
                    if ((void*)intPtr2 > (void*)intPtr)
                    {
                        if (System.Runtime.CompilerServices.Unsafe.ReadUnaligned<UIntPtr>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref first, intPtr)) != System.Runtime.CompilerServices.Unsafe.ReadUnaligned<UIntPtr>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref second, intPtr)))
                        {
                            break;
                        }
                        intPtr += sizeof(UIntPtr);
                        continue;
                    }
                    return System.Runtime.CompilerServices.Unsafe.ReadUnaligned<UIntPtr>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref first, intPtr2)) == System.Runtime.CompilerServices.Unsafe.ReadUnaligned<UIntPtr>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref second, intPtr2));
                }
            }
            goto IL_013f;
            IL_013d:
            return true;
            IL_013f:
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int LocateFirstFoundByte(Vector<byte> match)
        {
            //IL_0000: Unknown result type (might be due to invalid IL or missing references)
            //IL_0001: Unknown result type (might be due to invalid IL or missing references)
            //IL_0006: Unknown result type (might be due to invalid IL or missing references)
            Vector<ulong> val = Vector.AsVectorUInt64<byte>(match);
            ulong num = 0uL;
            int i;
            for (i = 0; i < Vector<ulong>.Count; i++)
            {
                num = val[i];
                if (num != 0L)
                {
                    break;
                }
            }
            return i * 8 + LocateFirstFoundByte(num);
        }

        public unsafe static int SequenceCompareTo(ref byte first, int firstLength, ref byte second, int secondLength)
        {
            //IL_0057: Unknown result type (might be due to invalid IL or missing references)
            //IL_0063: Unknown result type (might be due to invalid IL or missing references)
            if (!System.Runtime.CompilerServices.Unsafe.AreSame<byte>(ref first, ref second))
            {
                IntPtr intPtr = (IntPtr)((firstLength < secondLength) ? firstLength : secondLength);
                IntPtr intPtr2 = (IntPtr)0;
                IntPtr intPtr3 = (IntPtr)(void*)intPtr;
                if (Vector.IsHardwareAccelerated && (uint)(void*)intPtr3 > (uint)Vector<byte>.Count)
                {
                    intPtr3 -= Vector<byte>.Count;
                    for (; (void*)intPtr3 > (void*)intPtr2 && !(System.Runtime.CompilerServices.Unsafe.ReadUnaligned<Vector<byte>>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref first, intPtr2)) != System.Runtime.CompilerServices.Unsafe.ReadUnaligned<Vector<byte>>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref second, intPtr2))); intPtr2 += Vector<byte>.Count)
                    {
                    }
                }
                else if ((uint)(void*)intPtr3 > (uint)sizeof(UIntPtr))
                {
                    intPtr3 -= sizeof(UIntPtr);
                    for (; (void*)intPtr3 > (void*)intPtr2 && !(System.Runtime.CompilerServices.Unsafe.ReadUnaligned<UIntPtr>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref first, intPtr2)) != System.Runtime.CompilerServices.Unsafe.ReadUnaligned<UIntPtr>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref second, intPtr2))); intPtr2 += sizeof(UIntPtr))
                    {
                    }
                }
                for (; (void*)intPtr > (void*)intPtr2; intPtr2 += 1)
                {
                    int num = System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref first, intPtr2).CompareTo(System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(ref second, intPtr2));
                    if (num != 0)
                    {
                        return num;
                    }
                }
            }
            return firstLength - secondLength;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int LocateLastFoundByte(Vector<byte> match)
        {
            //IL_0000: Unknown result type (might be due to invalid IL or missing references)
            //IL_0001: Unknown result type (might be due to invalid IL or missing references)
            //IL_0006: Unknown result type (might be due to invalid IL or missing references)
            Vector<ulong> val = Vector.AsVectorUInt64<byte>(match);
            ulong num = 0uL;
            int num2;
            for (num2 = Vector<ulong>.Count - 1; num2 >= 0; num2--)
            {
                num = val[num2];
                if (num != 0L)
                {
                    break;
                }
            }
            return num2 * 8 + LocateLastFoundByte(num);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int LocateFirstFoundByte(ulong match)
        {
            ulong num = match ^ (match - 1);
            return (int)(num * 283686952306184L >> 57);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int LocateLastFoundByte(ulong match)
        {
            int num = 7;
            while ((long)match > 0L)
            {
                match <<= 8;
                num--;
            }
            return num;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Vector<byte> GetVector(byte vectorByte)
        {
            //IL_0007: Unknown result type (might be due to invalid IL or missing references)
            //IL_000c: Unknown result type (might be due to invalid IL or missing references)
            return Vector.AsVectorByte<uint>(new Vector<uint>((uint)(vectorByte * 16843009)));
        }

        public unsafe static int SequenceCompareTo(ref char first, int firstLength, ref char second, int secondLength)
        {
            //IL_006c: Unknown result type (might be due to invalid IL or missing references)
            //IL_007d: Unknown result type (might be due to invalid IL or missing references)
            int result = firstLength - secondLength;
            if (!System.Runtime.CompilerServices.Unsafe.AreSame<char>(ref first, ref second))
            {
                IntPtr intPtr = (IntPtr)((firstLength < secondLength) ? firstLength : secondLength);
                IntPtr intPtr2 = (IntPtr)0;
                if ((uint)(void*)intPtr >= (uint)(sizeof(UIntPtr) / 2))
                {
                    if (Vector.IsHardwareAccelerated && (uint)(void*)intPtr >= (uint)Vector<ushort>.Count)
                    {
                        IntPtr intPtr3 = intPtr - Vector<ushort>.Count;
                        while (!(System.Runtime.CompilerServices.Unsafe.ReadUnaligned<Vector<ushort>>(ref System.Runtime.CompilerServices.Unsafe.As<char, byte>(ref System.Runtime.CompilerServices.Unsafe.Add<char>(ref first, intPtr2))) != System.Runtime.CompilerServices.Unsafe.ReadUnaligned<Vector<ushort>>(ref System.Runtime.CompilerServices.Unsafe.As<char, byte>(ref System.Runtime.CompilerServices.Unsafe.Add<char>(ref second, intPtr2)))))
                        {
                            intPtr2 += Vector<ushort>.Count;
                            if ((void*)intPtr3 < (void*)intPtr2)
                            {
                                break;
                            }
                        }
                    }
                    for (; (void*)intPtr >= (void*)(intPtr2 + sizeof(UIntPtr) / 2) && !(System.Runtime.CompilerServices.Unsafe.ReadUnaligned<UIntPtr>(ref System.Runtime.CompilerServices.Unsafe.As<char, byte>(ref System.Runtime.CompilerServices.Unsafe.Add<char>(ref first, intPtr2))) != System.Runtime.CompilerServices.Unsafe.ReadUnaligned<UIntPtr>(ref System.Runtime.CompilerServices.Unsafe.As<char, byte>(ref System.Runtime.CompilerServices.Unsafe.Add<char>(ref second, intPtr2)))); intPtr2 += sizeof(UIntPtr) / 2)
                    {
                    }
                }
                if (sizeof(UIntPtr) > 4 && (void*)intPtr >= (void*)(intPtr2 + 2) && System.Runtime.CompilerServices.Unsafe.ReadUnaligned<int>(ref System.Runtime.CompilerServices.Unsafe.As<char, byte>(ref System.Runtime.CompilerServices.Unsafe.Add<char>(ref first, intPtr2))) == System.Runtime.CompilerServices.Unsafe.ReadUnaligned<int>(ref System.Runtime.CompilerServices.Unsafe.As<char, byte>(ref System.Runtime.CompilerServices.Unsafe.Add<char>(ref second, intPtr2))))
                {
                    intPtr2 += 2;
                }
                for (; (void*)intPtr2 < (void*)intPtr; intPtr2 += 1)
                {
                    int num = System.Runtime.CompilerServices.Unsafe.Add<char>(ref first, intPtr2).CompareTo(System.Runtime.CompilerServices.Unsafe.Add<char>(ref second, intPtr2));
                    if (num != 0)
                    {
                        return num;
                    }
                }
            }
            return result;
        }

        public unsafe static int IndexOf(ref char searchSpace, char value, int length)
        {
            //IL_00bc: Unknown result type (might be due to invalid IL or missing references)
            //IL_00bf: Unknown result type (might be due to invalid IL or missing references)
            //IL_00c4: Unknown result type (might be due to invalid IL or missing references)
            //IL_00c9: Unknown result type (might be due to invalid IL or missing references)
            //IL_00cb: Unknown result type (might be due to invalid IL or missing references)
            //IL_00d0: Unknown result type (might be due to invalid IL or missing references)
            //IL_00d4: Unknown result type (might be due to invalid IL or missing references)
            //IL_00fa: Unknown result type (might be due to invalid IL or missing references)
            fixed (char* ptr = &searchSpace)
            {
                char* ptr2 = ptr;
                char* ptr3 = ptr2 + length;
                if (Vector.IsHardwareAccelerated && length >= Vector<ushort>.Count * 2)
                {
                    int num = ((int)ptr2 & (System.Runtime.CompilerServices.Unsafe.SizeOf<Vector<ushort>>() - 1)) / 2;
                    length = (Vector<ushort>.Count - num) & (Vector<ushort>.Count - 1);
                }
                Vector<ushort> val = default(Vector<ushort>);
                while (true)
                {
                    if (length >= 4)
                    {
                        length -= 4;
                        if (*ptr2 == value)
                        {
                            break;
                        }
                        if (ptr2[1] != value)
                        {
                            if (ptr2[2] != value)
                            {
                                if (ptr2[3] != value)
                                {
                                    ptr2 += 4;
                                    continue;
                                }
                                ptr2++;
                            }
                            ptr2++;
                        }
                        ptr2++;
                        break;
                    }
                    while (length > 0)
                    {
                        length--;
                        if (*ptr2 == value)
                        {
                            goto end_IL_0079;
                        }
                        ptr2++;
                    }
                    if (Vector.IsHardwareAccelerated && ptr2 < ptr3)
                    {
                        length = (int)((ptr3 - ptr2) & ~(Vector<ushort>.Count - 1));
                        val = new Vector<ushort>((ushort)value);
                        while (length > 0)
                        {
                            Vector<ushort> val2 = Vector.Equals<ushort>(val, System.Runtime.CompilerServices.Unsafe.Read<Vector<ushort>>((void*)ptr2));
                            if (Vector<ushort>.Zero.Equals(val2))
                            {
                                ptr2 += Vector<ushort>.Count;
                                length -= Vector<ushort>.Count;
                                continue;
                            }
                            return (int)(ptr2 - ptr) + LocateFirstFoundChar(val2);
                        }
                        if (ptr2 < ptr3)
                        {
                            length = (int)(ptr3 - ptr2);
                            continue;
                        }
                    }
                    return -1;
                    continue;
                    end_IL_0079:
                    break;
                }
                return (int)(ptr2 - ptr);
            }
        }

        public unsafe static int LastIndexOf(ref char searchSpace, char value, int length)
        {
            //IL_00bc: Unknown result type (might be due to invalid IL or missing references)
            //IL_00c0: Unknown result type (might be due to invalid IL or missing references)
            //IL_00c5: Unknown result type (might be due to invalid IL or missing references)
            //IL_00ca: Unknown result type (might be due to invalid IL or missing references)
            //IL_00cc: Unknown result type (might be due to invalid IL or missing references)
            //IL_00d1: Unknown result type (might be due to invalid IL or missing references)
            //IL_00d5: Unknown result type (might be due to invalid IL or missing references)
            //IL_00fc: Unknown result type (might be due to invalid IL or missing references)
            fixed (char* ptr = &searchSpace)
            {
                char* ptr2 = ptr + length;
                char* ptr3 = ptr;
                if (Vector.IsHardwareAccelerated && length >= Vector<ushort>.Count * 2)
                {
                    length = ((int)ptr2 & (System.Runtime.CompilerServices.Unsafe.SizeOf<Vector<ushort>>() - 1)) / 2;
                }
                Vector<ushort> val = default(Vector<ushort>);
                while (true)
                {
                    if (length >= 4)
                    {
                        length -= 4;
                        ptr2 -= 4;
                        if (ptr2[3] == value)
                        {
                            break;
                        }
                        if (ptr2[2] != value)
                        {
                            if (ptr2[1] != value)
                            {
                                if (*ptr2 != value)
                                {
                                    continue;
                                }
                                goto IL_011d;
                            }
                            return (int)(ptr2 - ptr3) + 1;
                        }
                        return (int)(ptr2 - ptr3) + 2;
                    }
                    while (length > 0)
                    {
                        length--;
                        ptr2--;
                        if (*ptr2 != value)
                        {
                            continue;
                        }
                        goto IL_011d;
                    }
                    if (Vector.IsHardwareAccelerated && ptr2 > ptr3)
                    {
                        length = (int)((ptr2 - ptr3) & ~(Vector<ushort>.Count - 1));
                        val = new Vector<ushort>((ushort)value);
                        while (length > 0)
                        {
                            char* ptr4 = ptr2 - Vector<ushort>.Count;
                            Vector<ushort> val2 = Vector.Equals<ushort>(val, System.Runtime.CompilerServices.Unsafe.Read<Vector<ushort>>((void*)ptr4));
                            if (Vector<ushort>.Zero.Equals(val2))
                            {
                                ptr2 -= Vector<ushort>.Count;
                                length -= Vector<ushort>.Count;
                                continue;
                            }
                            return (int)(ptr4 - ptr3) + LocateLastFoundChar(val2);
                        }
                        if (ptr2 > ptr3)
                        {
                            length = (int)(ptr2 - ptr3);
                            continue;
                        }
                    }
                    return -1;
                    IL_011d:
                    return (int)(ptr2 - ptr3);
                }
                return (int)(ptr2 - ptr3) + 3;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int LocateFirstFoundChar(Vector<ushort> match)
        {
            //IL_0000: Unknown result type (might be due to invalid IL or missing references)
            //IL_0001: Unknown result type (might be due to invalid IL or missing references)
            //IL_0006: Unknown result type (might be due to invalid IL or missing references)
            Vector<ulong> val = Vector.AsVectorUInt64<ushort>(match);
            ulong num = 0uL;
            int i;
            for (i = 0; i < Vector<ulong>.Count; i++)
            {
                num = val[i];
                if (num != 0L)
                {
                    break;
                }
            }
            return i * 4 + LocateFirstFoundChar(num);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int LocateFirstFoundChar(ulong match)
        {
            ulong num = match ^ (match - 1);
            return (int)(num * 4295098372L >> 49);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int LocateLastFoundChar(Vector<ushort> match)
        {
            //IL_0000: Unknown result type (might be due to invalid IL or missing references)
            //IL_0001: Unknown result type (might be due to invalid IL or missing references)
            //IL_0006: Unknown result type (might be due to invalid IL or missing references)
            Vector<ulong> val = Vector.AsVectorUInt64<ushort>(match);
            ulong num = 0uL;
            int num2;
            for (num2 = Vector<ulong>.Count - 1; num2 >= 0; num2--)
            {
                num = val[num2];
                if (num != 0L)
                {
                    break;
                }
            }
            return num2 * 4 + LocateLastFoundChar(num);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int LocateLastFoundChar(ulong match)
        {
            int num = 3;
            while ((long)match > 0L)
            {
                match <<= 16;
                num--;
            }
            return num;
        }

        public static int IndexOf<T>(ref T searchSpace, int searchSpaceLength, ref T value, int valueLength) where T : IEquatable<T>
        {
            if (valueLength == 0)
            {
                return 0;
            }
            T value2 = value;
            ref T second = ref System.Runtime.CompilerServices.Unsafe.Add<T>(ref value, 1);
            int num = valueLength - 1;
            int num2 = 0;
            while (true)
            {
                int num3 = searchSpaceLength - num2 - num;
                if (num3 <= 0)
                {
                    break;
                }
                int num4 = IndexOf(ref System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, num2), value2, num3);
                if (num4 == -1)
                {
                    break;
                }
                num2 += num4;
                if (SequenceEqual(ref System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, num2 + 1), ref second, num))
                {
                    return num2;
                }
                num2++;
            }
            return -1;
        }

        public unsafe static int IndexOf<T>(ref T searchSpace, T value, int length) where T : IEquatable<T>
        {
            IntPtr intPtr = (IntPtr)0;
            while (true)
            {
                if (length >= 8)
                {
                    length -= 8;
                    if (!value.Equals(System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, intPtr)))
                    {
                        if (value.Equals(System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, intPtr + 1)))
                        {
                            goto IL_020a;
                        }
                        if (value.Equals(System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, intPtr + 2)))
                        {
                            goto IL_0218;
                        }
                        if (!value.Equals(System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, intPtr + 3)))
                        {
                            if (!value.Equals(System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, intPtr + 4)))
                            {
                                if (!value.Equals(System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, intPtr + 5)))
                                {
                                    if (!value.Equals(System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, intPtr + 6)))
                                    {
                                        if (value.Equals(System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, intPtr + 7)))
                                        {
                                            break;
                                        }
                                        intPtr += 8;
                                        continue;
                                    }
                                    return (int)(void*)(intPtr + 6);
                                }
                                return (int)(void*)(intPtr + 5);
                            }
                            return (int)(void*)(intPtr + 4);
                        }
                        goto IL_0226;
                    }
                }
                else
                {
                    if (length >= 4)
                    {
                        length -= 4;
                        if (value.Equals(System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, intPtr)))
                        {
                            goto IL_0202;
                        }
                        if (value.Equals(System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, intPtr + 1)))
                        {
                            goto IL_020a;
                        }
                        if (value.Equals(System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, intPtr + 2)))
                        {
                            goto IL_0218;
                        }
                        if (value.Equals(System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, intPtr + 3)))
                        {
                            goto IL_0226;
                        }
                        intPtr += 4;
                    }
                    while (true)
                    {
                        if (length > 0)
                        {
                            if (value.Equals(System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, intPtr)))
                            {
                                break;
                            }
                            intPtr += 1;
                            length--;
                            continue;
                        }
                        return -1;
                    }
                }
                goto IL_0202;
                IL_0218:
                return (int)(void*)(intPtr + 2);
                IL_0202:
                return (int)(void*)intPtr;
                IL_020a:
                return (int)(void*)(intPtr + 1);
                IL_0226:
                return (int)(void*)(intPtr + 3);
            }
            return (int)(void*)(intPtr + 7);
        }

        public static int IndexOfAny<T>(ref T searchSpace, T value0, T value1, int length) where T : IEquatable<T>
        {
            int num = 0;
            while (true)
            {
                if (length - num >= 8)
                {
                    T other = System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, num);
                    if (!value0.Equals(other) && !value1.Equals(other))
                    {
                        other = System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, num + 1);
                        if (value0.Equals(other) || value1.Equals(other))
                        {
                            goto IL_02cb;
                        }
                        other = System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, num + 2);
                        if (value0.Equals(other) || value1.Equals(other))
                        {
                            goto IL_02cf;
                        }
                        other = System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, num + 3);
                        if (!value0.Equals(other) && !value1.Equals(other))
                        {
                            other = System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, num + 4);
                            if (!value0.Equals(other) && !value1.Equals(other))
                            {
                                other = System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, num + 5);
                                if (!value0.Equals(other) && !value1.Equals(other))
                                {
                                    other = System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, num + 6);
                                    if (!value0.Equals(other) && !value1.Equals(other))
                                    {
                                        other = System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, num + 7);
                                        if (value0.Equals(other) || value1.Equals(other))
                                        {
                                            break;
                                        }
                                        num += 8;
                                        continue;
                                    }
                                    return num + 6;
                                }
                                return num + 5;
                            }
                            return num + 4;
                        }
                        goto IL_02d3;
                    }
                }
                else
                {
                    if (length - num >= 4)
                    {
                        T other = System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, num);
                        if (value0.Equals(other) || value1.Equals(other))
                        {
                            goto IL_02c9;
                        }
                        other = System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, num + 1);
                        if (value0.Equals(other) || value1.Equals(other))
                        {
                            goto IL_02cb;
                        }
                        other = System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, num + 2);
                        if (value0.Equals(other) || value1.Equals(other))
                        {
                            goto IL_02cf;
                        }
                        other = System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, num + 3);
                        if (value0.Equals(other) || value1.Equals(other))
                        {
                            goto IL_02d3;
                        }
                        num += 4;
                    }
                    while (true)
                    {
                        if (num < length)
                        {
                            T other = System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, num);
                            if (value0.Equals(other) || value1.Equals(other))
                            {
                                break;
                            }
                            num++;
                            continue;
                        }
                        return -1;
                    }
                }
                goto IL_02c9;
                IL_02cf:
                return num + 2;
                IL_02cb:
                return num + 1;
                IL_02d3:
                return num + 3;
                IL_02c9:
                return num;
            }
            return num + 7;
        }

        public static int IndexOfAny<T>(ref T searchSpace, T value0, T value1, T value2, int length) where T : IEquatable<T>
        {
            int num = 0;
            while (true)
            {
                if (length - num >= 8)
                {
                    T other = System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, num);
                    if (!value0.Equals(other) && !value1.Equals(other) && !value2.Equals(other))
                    {
                        other = System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, num + 1);
                        if (value0.Equals(other) || value1.Equals(other) || value2.Equals(other))
                        {
                            goto IL_03c2;
                        }
                        other = System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, num + 2);
                        if (value0.Equals(other) || value1.Equals(other) || value2.Equals(other))
                        {
                            goto IL_03c6;
                        }
                        other = System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, num + 3);
                        if (!value0.Equals(other) && !value1.Equals(other) && !value2.Equals(other))
                        {
                            other = System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, num + 4);
                            if (!value0.Equals(other) && !value1.Equals(other) && !value2.Equals(other))
                            {
                                other = System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, num + 5);
                                if (!value0.Equals(other) && !value1.Equals(other) && !value2.Equals(other))
                                {
                                    other = System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, num + 6);
                                    if (!value0.Equals(other) && !value1.Equals(other) && !value2.Equals(other))
                                    {
                                        other = System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, num + 7);
                                        if (value0.Equals(other) || value1.Equals(other) || value2.Equals(other))
                                        {
                                            break;
                                        }
                                        num += 8;
                                        continue;
                                    }
                                    return num + 6;
                                }
                                return num + 5;
                            }
                            return num + 4;
                        }
                        goto IL_03ca;
                    }
                }
                else
                {
                    if (length - num >= 4)
                    {
                        T other = System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, num);
                        if (value0.Equals(other) || value1.Equals(other) || value2.Equals(other))
                        {
                            goto IL_03c0;
                        }
                        other = System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, num + 1);
                        if (value0.Equals(other) || value1.Equals(other) || value2.Equals(other))
                        {
                            goto IL_03c2;
                        }
                        other = System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, num + 2);
                        if (value0.Equals(other) || value1.Equals(other) || value2.Equals(other))
                        {
                            goto IL_03c6;
                        }
                        other = System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, num + 3);
                        if (value0.Equals(other) || value1.Equals(other) || value2.Equals(other))
                        {
                            goto IL_03ca;
                        }
                        num += 4;
                    }
                    while (true)
                    {
                        if (num < length)
                        {
                            T other = System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, num);
                            if (value0.Equals(other) || value1.Equals(other) || value2.Equals(other))
                            {
                                break;
                            }
                            num++;
                            continue;
                        }
                        return -1;
                    }
                }
                goto IL_03c0;
                IL_03c0:
                return num;
                IL_03c6:
                return num + 2;
                IL_03c2:
                return num + 1;
                IL_03ca:
                return num + 3;
            }
            return num + 7;
        }

        public static int IndexOfAny<T>(ref T searchSpace, int searchSpaceLength, ref T value, int valueLength) where T : IEquatable<T>
        {
            if (valueLength == 0)
            {
                return 0;
            }
            int num = -1;
            for (int i = 0; i < valueLength; i++)
            {
                int num2 = IndexOf(ref searchSpace, System.Runtime.CompilerServices.Unsafe.Add<T>(ref value, i), searchSpaceLength);
                if ((uint)num2 < (uint)num)
                {
                    num = num2;
                    searchSpaceLength = num2;
                    if (num == 0)
                    {
                        break;
                    }
                }
            }
            return num;
        }

        public static int LastIndexOf<T>(ref T searchSpace, int searchSpaceLength, ref T value, int valueLength) where T : IEquatable<T>
        {
            if (valueLength == 0)
            {
                return 0;
            }
            T value2 = value;
            ref T second = ref System.Runtime.CompilerServices.Unsafe.Add<T>(ref value, 1);
            int num = valueLength - 1;
            int num2 = 0;
            while (true)
            {
                int num3 = searchSpaceLength - num2 - num;
                if (num3 <= 0)
                {
                    break;
                }
                int num4 = LastIndexOf(ref searchSpace, value2, num3);
                if (num4 == -1)
                {
                    break;
                }
                if (SequenceEqual(ref System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, num4 + 1), ref second, num))
                {
                    return num4;
                }
                num2 += num3 - num4;
            }
            return -1;
        }

        public static int LastIndexOf<T>(ref T searchSpace, T value, int length) where T : IEquatable<T>
        {
            while (true)
            {
                if (length >= 8)
                {
                    length -= 8;
                    if (value.Equals(System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, length + 7)))
                    {
                        break;
                    }
                    if (value.Equals(System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, length + 6)))
                    {
                        return length + 6;
                    }
                    if (value.Equals(System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, length + 5)))
                    {
                        return length + 5;
                    }
                    if (value.Equals(System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, length + 4)))
                    {
                        return length + 4;
                    }
                    if (value.Equals(System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, length + 3)))
                    {
                        goto IL_01c2;
                    }
                    if (value.Equals(System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, length + 2)))
                    {
                        goto IL_01be;
                    }
                    if (value.Equals(System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, length + 1)))
                    {
                        goto IL_01ba;
                    }
                    if (!value.Equals(System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, length)))
                    {
                        continue;
                    }
                }
                else
                {
                    if (length >= 4)
                    {
                        length -= 4;
                        if (value.Equals(System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, length + 3)))
                        {
                            goto IL_01c2;
                        }
                        if (value.Equals(System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, length + 2)))
                        {
                            goto IL_01be;
                        }
                        if (value.Equals(System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, length + 1)))
                        {
                            goto IL_01ba;
                        }
                        if (value.Equals(System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, length)))
                        {
                            goto IL_01b8;
                        }
                    }
                    do
                    {
                        if (length > 0)
                        {
                            length--;
                            continue;
                        }
                        return -1;
                    }
                    while (!value.Equals(System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, length)));
                }
                goto IL_01b8;
                IL_01be:
                return length + 2;
                IL_01c2:
                return length + 3;
                IL_01ba:
                return length + 1;
                IL_01b8:
                return length;
            }
            return length + 7;
        }

        public static int LastIndexOfAny<T>(ref T searchSpace, T value0, T value1, int length) where T : IEquatable<T>
        {
            while (true)
            {
                if (length >= 8)
                {
                    length -= 8;
                    T other = System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, length + 7);
                    if (value0.Equals(other) || value1.Equals(other))
                    {
                        break;
                    }
                    other = System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, length + 6);
                    if (value0.Equals(other) || value1.Equals(other))
                    {
                        return length + 6;
                    }
                    other = System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, length + 5);
                    if (value0.Equals(other) || value1.Equals(other))
                    {
                        return length + 5;
                    }
                    other = System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, length + 4);
                    if (value0.Equals(other) || value1.Equals(other))
                    {
                        return length + 4;
                    }
                    other = System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, length + 3);
                    if (value0.Equals(other) || value1.Equals(other))
                    {
                        goto IL_02cd;
                    }
                    other = System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, length + 2);
                    if (value0.Equals(other) || value1.Equals(other))
                    {
                        goto IL_02c9;
                    }
                    other = System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, length + 1);
                    if (value0.Equals(other) || value1.Equals(other))
                    {
                        goto IL_02c5;
                    }
                    other = System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, length);
                    if (!value0.Equals(other) && !value1.Equals(other))
                    {
                        continue;
                    }
                }
                else
                {
                    T other;
                    if (length >= 4)
                    {
                        length -= 4;
                        other = System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, length + 3);
                        if (value0.Equals(other) || value1.Equals(other))
                        {
                            goto IL_02cd;
                        }
                        other = System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, length + 2);
                        if (value0.Equals(other) || value1.Equals(other))
                        {
                            goto IL_02c9;
                        }
                        other = System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, length + 1);
                        if (value0.Equals(other) || value1.Equals(other))
                        {
                            goto IL_02c5;
                        }
                        other = System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, length);
                        if (value0.Equals(other) || value1.Equals(other))
                        {
                            goto IL_02c3;
                        }
                    }
                    do
                    {
                        if (length > 0)
                        {
                            length--;
                            other = System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, length);
                            continue;
                        }
                        return -1;
                    }
                    while (!value0.Equals(other) && !value1.Equals(other));
                }
                goto IL_02c3;
                IL_02c9:
                return length + 2;
                IL_02c5:
                return length + 1;
                IL_02c3:
                return length;
                IL_02cd:
                return length + 3;
            }
            return length + 7;
        }

        public static int LastIndexOfAny<T>(ref T searchSpace, T value0, T value1, T value2, int length) where T : IEquatable<T>
        {
            while (true)
            {
                if (length >= 8)
                {
                    length -= 8;
                    T other = System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, length + 7);
                    if (value0.Equals(other) || value1.Equals(other) || value2.Equals(other))
                    {
                        break;
                    }
                    other = System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, length + 6);
                    if (value0.Equals(other) || value1.Equals(other) || value2.Equals(other))
                    {
                        return length + 6;
                    }
                    other = System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, length + 5);
                    if (value0.Equals(other) || value1.Equals(other) || value2.Equals(other))
                    {
                        return length + 5;
                    }
                    other = System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, length + 4);
                    if (value0.Equals(other) || value1.Equals(other) || value2.Equals(other))
                    {
                        return length + 4;
                    }
                    other = System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, length + 3);
                    if (value0.Equals(other) || value1.Equals(other) || value2.Equals(other))
                    {
                        goto IL_03da;
                    }
                    other = System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, length + 2);
                    if (value0.Equals(other) || value1.Equals(other) || value2.Equals(other))
                    {
                        goto IL_03d5;
                    }
                    other = System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, length + 1);
                    if (value0.Equals(other) || value1.Equals(other) || value2.Equals(other))
                    {
                        goto IL_03d0;
                    }
                    other = System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, length);
                    if (!value0.Equals(other) && !value1.Equals(other) && !value2.Equals(other))
                    {
                        continue;
                    }
                }
                else
                {
                    T other;
                    if (length >= 4)
                    {
                        length -= 4;
                        other = System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, length + 3);
                        if (value0.Equals(other) || value1.Equals(other) || value2.Equals(other))
                        {
                            goto IL_03da;
                        }
                        other = System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, length + 2);
                        if (value0.Equals(other) || value1.Equals(other) || value2.Equals(other))
                        {
                            goto IL_03d5;
                        }
                        other = System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, length + 1);
                        if (value0.Equals(other) || value1.Equals(other) || value2.Equals(other))
                        {
                            goto IL_03d0;
                        }
                        other = System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, length);
                        if (value0.Equals(other) || value1.Equals(other) || value2.Equals(other))
                        {
                            goto IL_03cd;
                        }
                    }
                    do
                    {
                        if (length > 0)
                        {
                            length--;
                            other = System.Runtime.CompilerServices.Unsafe.Add<T>(ref searchSpace, length);
                            continue;
                        }
                        return -1;
                    }
                    while (!value0.Equals(other) && !value1.Equals(other) && !value2.Equals(other));
                }
                goto IL_03cd;
                IL_03d0:
                return length + 1;
                IL_03d5:
                return length + 2;
                IL_03da:
                return length + 3;
                IL_03cd:
                return length;
            }
            return length + 7;
        }

        public static int LastIndexOfAny<T>(ref T searchSpace, int searchSpaceLength, ref T value, int valueLength) where T : IEquatable<T>
        {
            if (valueLength == 0)
            {
                return 0;
            }
            int num = -1;
            for (int i = 0; i < valueLength; i++)
            {
                int num2 = LastIndexOf(ref searchSpace, System.Runtime.CompilerServices.Unsafe.Add<T>(ref value, i), searchSpaceLength);
                if (num2 > num)
                {
                    num = num2;
                }
            }
            return num;
        }

        public static bool SequenceEqual<T>(ref T first, ref T second, int length) where T : IEquatable<T>
        {
            if (!System.Runtime.CompilerServices.Unsafe.AreSame<T>(ref first, ref second))
            {
                IntPtr intPtr = (IntPtr)0;
                while (true)
                {
                    if (length >= 8)
                    {
                        length -= 8;
                        if (System.Runtime.CompilerServices.Unsafe.Add<T>(ref first, intPtr).Equals(System.Runtime.CompilerServices.Unsafe.Add<T>(ref second, intPtr)) && System.Runtime.CompilerServices.Unsafe.Add<T>(ref first, intPtr + 1).Equals(System.Runtime.CompilerServices.Unsafe.Add<T>(ref second, intPtr + 1)) && System.Runtime.CompilerServices.Unsafe.Add<T>(ref first, intPtr + 2).Equals(System.Runtime.CompilerServices.Unsafe.Add<T>(ref second, intPtr + 2)) && System.Runtime.CompilerServices.Unsafe.Add<T>(ref first, intPtr + 3).Equals(System.Runtime.CompilerServices.Unsafe.Add<T>(ref second, intPtr + 3)) && System.Runtime.CompilerServices.Unsafe.Add<T>(ref first, intPtr + 4).Equals(System.Runtime.CompilerServices.Unsafe.Add<T>(ref second, intPtr + 4)) && System.Runtime.CompilerServices.Unsafe.Add<T>(ref first, intPtr + 5).Equals(System.Runtime.CompilerServices.Unsafe.Add<T>(ref second, intPtr + 5)) && System.Runtime.CompilerServices.Unsafe.Add<T>(ref first, intPtr + 6).Equals(System.Runtime.CompilerServices.Unsafe.Add<T>(ref second, intPtr + 6)) && System.Runtime.CompilerServices.Unsafe.Add<T>(ref first, intPtr + 7).Equals(System.Runtime.CompilerServices.Unsafe.Add<T>(ref second, intPtr + 7)))
                        {
                            intPtr += 8;
                            continue;
                        }
                        goto IL_028b;
                    }
                    if (length >= 4)
                    {
                        length -= 4;
                        if (!System.Runtime.CompilerServices.Unsafe.Add<T>(ref first, intPtr).Equals(System.Runtime.CompilerServices.Unsafe.Add<T>(ref second, intPtr)) || !System.Runtime.CompilerServices.Unsafe.Add<T>(ref first, intPtr + 1).Equals(System.Runtime.CompilerServices.Unsafe.Add<T>(ref second, intPtr + 1)) || !System.Runtime.CompilerServices.Unsafe.Add<T>(ref first, intPtr + 2).Equals(System.Runtime.CompilerServices.Unsafe.Add<T>(ref second, intPtr + 2)) || !System.Runtime.CompilerServices.Unsafe.Add<T>(ref first, intPtr + 3).Equals(System.Runtime.CompilerServices.Unsafe.Add<T>(ref second, intPtr + 3)))
                        {
                            goto IL_028b;
                        }
                        intPtr += 4;
                    }
                    while (length > 0)
                    {
                        if (System.Runtime.CompilerServices.Unsafe.Add<T>(ref first, intPtr).Equals(System.Runtime.CompilerServices.Unsafe.Add<T>(ref second, intPtr)))
                        {
                            intPtr += 1;
                            length--;
                            continue;
                        }
                        goto IL_028b;
                    }
                    break;
                    IL_028b:
                    return false;
                }
            }
            return true;
        }

        public static int SequenceCompareTo<T>(ref T first, int firstLength, ref T second, int secondLength) where T : IComparable<T>
        {
            int num = firstLength;
            if (num > secondLength)
            {
                num = secondLength;
            }
            for (int i = 0; i < num; i++)
            {
                int num2 = System.Runtime.CompilerServices.Unsafe.Add<T>(ref first, i).CompareTo(System.Runtime.CompilerServices.Unsafe.Add<T>(ref second, i));
                if (num2 != 0)
                {
                    return num2;
                }
            }
            return firstLength.CompareTo(secondLength);
        }

        public unsafe static void CopyTo<T>(ref T dst, int dstLength, ref T src, int srcLength)
        {
            IntPtr intPtr = System.Runtime.CompilerServices.Unsafe.ByteOffset<T>(ref src, ref System.Runtime.CompilerServices.Unsafe.Add<T>(ref src, srcLength));
            IntPtr intPtr2 = System.Runtime.CompilerServices.Unsafe.ByteOffset<T>(ref dst, ref System.Runtime.CompilerServices.Unsafe.Add<T>(ref dst, dstLength));
            IntPtr intPtr3 = System.Runtime.CompilerServices.Unsafe.ByteOffset<T>(ref src, ref dst);
            bool num;
            if (sizeof(IntPtr) != 4)
            {
                if ((ulong)(long)intPtr3 >= (ulong)(long)intPtr)
                {
                    num = (ulong)(long)intPtr3 > (ulong)(-(long)intPtr2);
                    goto IL_006f;
                }
            }
            else if ((uint)(int)intPtr3 >= (uint)(int)intPtr)
            {
                num = (uint)(int)intPtr3 > (uint)(-(int)intPtr2);
                goto IL_006f;
            }
            goto IL_00de;
            IL_00de:
            bool flag = ((sizeof(IntPtr) == 4) ? ((uint)(int)intPtr3 > (uint)(-(int)intPtr2)) : ((ulong)(long)intPtr3 > (ulong)(-(long)intPtr2)));
            int num2 = (flag ? 1 : (-1));
            int num3 = ((!flag) ? (srcLength - 1) : 0);
            int i;
            for (i = 0; i < (srcLength & -8); i += 8)
            {
                System.Runtime.CompilerServices.Unsafe.Add<T>(ref dst, num3) = System.Runtime.CompilerServices.Unsafe.Add<T>(ref src, num3);
                System.Runtime.CompilerServices.Unsafe.Add<T>(ref dst, num3 + num2) = System.Runtime.CompilerServices.Unsafe.Add<T>(ref src, num3 + num2);
                System.Runtime.CompilerServices.Unsafe.Add<T>(ref dst, num3 + num2 * 2) = System.Runtime.CompilerServices.Unsafe.Add<T>(ref src, num3 + num2 * 2);
                System.Runtime.CompilerServices.Unsafe.Add<T>(ref dst, num3 + num2 * 3) = System.Runtime.CompilerServices.Unsafe.Add<T>(ref src, num3 + num2 * 3);
                System.Runtime.CompilerServices.Unsafe.Add<T>(ref dst, num3 + num2 * 4) = System.Runtime.CompilerServices.Unsafe.Add<T>(ref src, num3 + num2 * 4);
                System.Runtime.CompilerServices.Unsafe.Add<T>(ref dst, num3 + num2 * 5) = System.Runtime.CompilerServices.Unsafe.Add<T>(ref src, num3 + num2 * 5);
                System.Runtime.CompilerServices.Unsafe.Add<T>(ref dst, num3 + num2 * 6) = System.Runtime.CompilerServices.Unsafe.Add<T>(ref src, num3 + num2 * 6);
                System.Runtime.CompilerServices.Unsafe.Add<T>(ref dst, num3 + num2 * 7) = System.Runtime.CompilerServices.Unsafe.Add<T>(ref src, num3 + num2 * 7);
                num3 += num2 * 8;
            }
            if (i < (srcLength & -4))
            {
                System.Runtime.CompilerServices.Unsafe.Add<T>(ref dst, num3) = System.Runtime.CompilerServices.Unsafe.Add<T>(ref src, num3);
                System.Runtime.CompilerServices.Unsafe.Add<T>(ref dst, num3 + num2) = System.Runtime.CompilerServices.Unsafe.Add<T>(ref src, num3 + num2);
                System.Runtime.CompilerServices.Unsafe.Add<T>(ref dst, num3 + num2 * 2) = System.Runtime.CompilerServices.Unsafe.Add<T>(ref src, num3 + num2 * 2);
                System.Runtime.CompilerServices.Unsafe.Add<T>(ref dst, num3 + num2 * 3) = System.Runtime.CompilerServices.Unsafe.Add<T>(ref src, num3 + num2 * 3);
                num3 += num2 * 4;
                i += 4;
            }
            for (; i < srcLength; i++)
            {
                System.Runtime.CompilerServices.Unsafe.Add<T>(ref dst, num3) = System.Runtime.CompilerServices.Unsafe.Add<T>(ref src, num3);
                num3 += num2;
            }
            return;
            IL_006f:
            if (!num && !IsReferenceOrContainsReferences<T>())
            {
                ref byte reference = ref System.Runtime.CompilerServices.Unsafe.As<T, byte>(ref dst);
                ref byte reference2 = ref System.Runtime.CompilerServices.Unsafe.As<T, byte>(ref src);
                ulong num4 = (ulong)(long)intPtr;
                uint num6;
                for (ulong num5 = 0uL; num5 < num4; num5 += num6)
                {
                    num6 = (uint)((num4 - num5 > uint.MaxValue) ? uint.MaxValue : (num4 - num5));
                    System.Runtime.CompilerServices.Unsafe.CopyBlock(ref System.Runtime.CompilerServices.Unsafe.Add<byte>(ref reference, (IntPtr)(long)num5), ref System.Runtime.CompilerServices.Unsafe.Add<byte>(ref reference2, (IntPtr)(long)num5), num6);
                }
                return;
            }
            goto IL_00de;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static IntPtr Add<T>(this IntPtr start, int index)
        {
            if (sizeof(IntPtr) == 4)
            {
                uint num = (uint)(index * System.Runtime.CompilerServices.Unsafe.SizeOf<T>());
                return (IntPtr)((byte*)(void*)start + num);
            }
            ulong num2 = (ulong)index * (ulong)System.Runtime.CompilerServices.Unsafe.SizeOf<T>();
            return (IntPtr)((byte*)(void*)start + num2);
        }

        public static bool IsReferenceOrContainsReferences<T>()
        {
            return PerTypeValues<T>.IsReferenceOrContainsReferences;
        }

        private static bool IsReferenceOrContainsReferencesCore(Type type)
        {
            if (type.GetTypeInfo().IsPrimitive)
            {
                return false;
            }
            if (!type.GetTypeInfo().IsValueType)
            {
                return true;
            }
            Type underlyingType = Nullable.GetUnderlyingType(type);
            if (underlyingType != null)
            {
                type = underlyingType;
            }
            if (type.GetTypeInfo().IsEnum)
            {
                return false;
            }
            foreach (FieldInfo declaredField in type.GetTypeInfo().DeclaredFields)
            {
                if (!declaredField.IsStatic && IsReferenceOrContainsReferencesCore(declaredField.FieldType))
                {
                    return true;
                }
            }
            return false;
        }

        public unsafe static void ClearLessThanPointerSized(byte* ptr, UIntPtr byteLength)
        {
            if (sizeof(UIntPtr) == 4)
            {
                System.Runtime.CompilerServices.Unsafe.InitBlockUnaligned((void*)ptr, (byte)0, (uint)byteLength);
                return;
            }
            ulong num = (ulong)byteLength;
            uint num2 = (uint)(num & 0xFFFFFFFFu);
            System.Runtime.CompilerServices.Unsafe.InitBlockUnaligned((void*)ptr, (byte)0, num2);
            num -= num2;
            ptr += num2;
            while (num != 0)
            {
                num2 = (uint)((num >= uint.MaxValue) ? uint.MaxValue : num);
                System.Runtime.CompilerServices.Unsafe.InitBlockUnaligned((void*)ptr, (byte)0, num2);
                ptr += num2;
                num -= num2;
            }
        }

        public unsafe static void ClearLessThanPointerSized(ref byte b, UIntPtr byteLength)
        {
            if (sizeof(UIntPtr) == 4)
            {
                System.Runtime.CompilerServices.Unsafe.InitBlockUnaligned(ref b, (byte)0, (uint)byteLength);
                return;
            }
            ulong num = (ulong)byteLength;
            uint num2 = (uint)(num & 0xFFFFFFFFu);
            System.Runtime.CompilerServices.Unsafe.InitBlockUnaligned(ref b, (byte)0, num2);
            num -= num2;
            long num3 = num2;
            while (num != 0)
            {
                num2 = (uint)((num >= uint.MaxValue) ? uint.MaxValue : num);
                System.Runtime.CompilerServices.Unsafe.InitBlockUnaligned(ref System.Runtime.CompilerServices.Unsafe.Add<byte>(ref b, (IntPtr)num3), (byte)0, num2);
                num3 += num2;
                num -= num2;
            }
        }

        public unsafe static void ClearPointerSizedWithoutReferences(ref byte b, UIntPtr byteLength)
        {
            IntPtr zero;
            for (zero = IntPtr.Zero; zero.LessThanEqual(byteLength - sizeof(Reg64)); zero += sizeof(Reg64))
            {
                System.Runtime.CompilerServices.Unsafe.As<byte, Reg64>(ref System.Runtime.CompilerServices.Unsafe.Add<byte>(ref b, zero)) = default(Reg64);
            }
            if (zero.LessThanEqual(byteLength - sizeof(Reg32)))
            {
                System.Runtime.CompilerServices.Unsafe.As<byte, Reg32>(ref System.Runtime.CompilerServices.Unsafe.Add<byte>(ref b, zero)) = default(Reg32);
                zero += sizeof(Reg32);
            }
            if (zero.LessThanEqual(byteLength - sizeof(Reg16)))
            {
                System.Runtime.CompilerServices.Unsafe.As<byte, Reg16>(ref System.Runtime.CompilerServices.Unsafe.Add<byte>(ref b, zero)) = default(Reg16);
                zero += sizeof(Reg16);
            }
            if (zero.LessThanEqual(byteLength - 8))
            {
                System.Runtime.CompilerServices.Unsafe.As<byte, long>(ref System.Runtime.CompilerServices.Unsafe.Add<byte>(ref b, zero)) = 0L;
                zero += 8;
            }
            if (sizeof(IntPtr) == 4 && zero.LessThanEqual(byteLength - 4))
            {
                System.Runtime.CompilerServices.Unsafe.As<byte, int>(ref System.Runtime.CompilerServices.Unsafe.Add<byte>(ref b, zero)) = 0;
                zero += 4;
            }
        }

        public static void ClearPointerSizedWithReferences(ref IntPtr ip, UIntPtr pointerSizeLength)
        {
            IntPtr intPtr = IntPtr.Zero;
            IntPtr zero = IntPtr.Zero;
            while ((zero = intPtr + 8).LessThanEqual(pointerSizeLength))
            {
                System.Runtime.CompilerServices.Unsafe.Add<IntPtr>(ref ip, intPtr + 0) = default(IntPtr);
                System.Runtime.CompilerServices.Unsafe.Add<IntPtr>(ref ip, intPtr + 1) = default(IntPtr);
                System.Runtime.CompilerServices.Unsafe.Add<IntPtr>(ref ip, intPtr + 2) = default(IntPtr);
                System.Runtime.CompilerServices.Unsafe.Add<IntPtr>(ref ip, intPtr + 3) = default(IntPtr);
                System.Runtime.CompilerServices.Unsafe.Add<IntPtr>(ref ip, intPtr + 4) = default(IntPtr);
                System.Runtime.CompilerServices.Unsafe.Add<IntPtr>(ref ip, intPtr + 5) = default(IntPtr);
                System.Runtime.CompilerServices.Unsafe.Add<IntPtr>(ref ip, intPtr + 6) = default(IntPtr);
                System.Runtime.CompilerServices.Unsafe.Add<IntPtr>(ref ip, intPtr + 7) = default(IntPtr);
                intPtr = zero;
            }
            if ((zero = intPtr + 4).LessThanEqual(pointerSizeLength))
            {
                System.Runtime.CompilerServices.Unsafe.Add<IntPtr>(ref ip, intPtr + 0) = default(IntPtr);
                System.Runtime.CompilerServices.Unsafe.Add<IntPtr>(ref ip, intPtr + 1) = default(IntPtr);
                System.Runtime.CompilerServices.Unsafe.Add<IntPtr>(ref ip, intPtr + 2) = default(IntPtr);
                System.Runtime.CompilerServices.Unsafe.Add<IntPtr>(ref ip, intPtr + 3) = default(IntPtr);
                intPtr = zero;
            }
            if ((zero = intPtr + 2).LessThanEqual(pointerSizeLength))
            {
                System.Runtime.CompilerServices.Unsafe.Add<IntPtr>(ref ip, intPtr + 0) = default(IntPtr);
                System.Runtime.CompilerServices.Unsafe.Add<IntPtr>(ref ip, intPtr + 1) = default(IntPtr);
                intPtr = zero;
            }
            if ((intPtr + 1).LessThanEqual(pointerSizeLength))
            {
                System.Runtime.CompilerServices.Unsafe.Add<IntPtr>(ref ip, intPtr) = default(IntPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private unsafe static bool LessThanEqual(this IntPtr index, UIntPtr length)
        {
            if (sizeof(UIntPtr) != 4)
            {
                return (long)index <= (long)(ulong)length;
            }
            return (int)index <= (int)(uint)length;
        }
    }
}
