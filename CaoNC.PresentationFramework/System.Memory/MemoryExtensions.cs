﻿using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System;

namespace CaoNC.System.Memory
{
    public static class MemoryExtensions
    {
        internal static readonly IntPtr StringAdjustment = MeasureStringAdjustment();

        public static ReadOnlySpan<char> Trim(this ReadOnlySpan<char> span)
        {
            return span.TrimStart().TrimEnd();
        }

        public static ReadOnlySpan<char> TrimStart(this ReadOnlySpan<char> span)
        {
            int i;
            for (i = 0; i < span.Length && char.IsWhiteSpace(span[i]); i++)
            {
            }
            return span.Slice(i);
        }

        public static ReadOnlySpan<char> TrimEnd(this ReadOnlySpan<char> span)
        {
            int num = span.Length - 1;
            while (num >= 0 && char.IsWhiteSpace(span[num]))
            {
                num--;
            }
            return span.Slice(0, num + 1);
        }

        public static ReadOnlySpan<char> Trim(this ReadOnlySpan<char> span, char trimChar)
        {
            return span.TrimStart(trimChar).TrimEnd(trimChar);
        }

        public static ReadOnlySpan<char> TrimStart(this ReadOnlySpan<char> span, char trimChar)
        {
            int i;
            for (i = 0; i < span.Length && span[i] == trimChar; i++)
            {
            }
            return span.Slice(i);
        }

        public static ReadOnlySpan<char> TrimEnd(this ReadOnlySpan<char> span, char trimChar)
        {
            int num = span.Length - 1;
            while (num >= 0 && span[num] == trimChar)
            {
                num--;
            }
            return span.Slice(0, num + 1);
        }

        public static ReadOnlySpan<char> Trim(this ReadOnlySpan<char> span, ReadOnlySpan<char> trimChars)
        {
            return span.TrimStart(trimChars).TrimEnd(trimChars);
        }

        public static ReadOnlySpan<char> TrimStart(this ReadOnlySpan<char> span, ReadOnlySpan<char> trimChars)
        {
            if (trimChars.IsEmpty)
            {
                return span.TrimStart();
            }
            int i;
            for (i = 0; i < span.Length; i++)
            {
                int num = 0;
                while (num < trimChars.Length)
                {
                    if (span[i] != trimChars[num])
                    {
                        num++;
                        continue;
                    }
                    goto IL_003c;
                }
                break;
                IL_003c:;
            }
            return span.Slice(i);
        }

        public static ReadOnlySpan<char> TrimEnd(this ReadOnlySpan<char> span, ReadOnlySpan<char> trimChars)
        {
            if (trimChars.IsEmpty)
            {
                return span.TrimEnd();
            }
            int num;
            for (num = span.Length - 1; num >= 0; num--)
            {
                int num2 = 0;
                while (num2 < trimChars.Length)
                {
                    if (span[num] != trimChars[num2])
                    {
                        num2++;
                        continue;
                    }
                    goto IL_0044;
                }
                break;
                IL_0044:;
            }
            return span.Slice(0, num + 1);
        }

        public static bool IsWhiteSpace(this ReadOnlySpan<char> span)
        {
            for (int i = 0; i < span.Length; i++)
            {
                if (!char.IsWhiteSpace(span[i]))
                {
                    return false;
                }
            }
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOf<T>(this Span<T> span, T value) where T : IEquatable<T>
        {
            if (typeof(T) == typeof(byte))
            {
                return SpanHelpers.IndexOf(ref System.Runtime.CompilerServices.Unsafe.As<T, byte>(ref MemoryMarshal.GetReference(span)), System.Runtime.CompilerServices.Unsafe.As<T, byte>(ref value), span.Length);
            }
            if (typeof(T) == typeof(char))
            {
                return SpanHelpers.IndexOf(ref System.Runtime.CompilerServices.Unsafe.As<T, char>(ref MemoryMarshal.GetReference(span)), System.Runtime.CompilerServices.Unsafe.As<T, char>(ref value), span.Length);
            }
            return SpanHelpers.IndexOf(ref MemoryMarshal.GetReference(span), value, span.Length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOf<T>(this Span<T> span, ReadOnlySpan<T> value) where T : IEquatable<T>
        {
            if (typeof(T) == typeof(byte))
            {
                return SpanHelpers.IndexOf(ref System.Runtime.CompilerServices.Unsafe.As<T, byte>(ref MemoryMarshal.GetReference(span)), span.Length, ref System.Runtime.CompilerServices.Unsafe.As<T, byte>(ref MemoryMarshal.GetReference(value)), value.Length);
            }
            return SpanHelpers.IndexOf(ref MemoryMarshal.GetReference(span), span.Length, ref MemoryMarshal.GetReference(value), value.Length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LastIndexOf<T>(this Span<T> span, T value) where T : IEquatable<T>
        {
            if (typeof(T) == typeof(byte))
            {
                return SpanHelpers.LastIndexOf(ref System.Runtime.CompilerServices.Unsafe.As<T, byte>(ref MemoryMarshal.GetReference(span)), System.Runtime.CompilerServices.Unsafe.As<T, byte>(ref value), span.Length);
            }
            if (typeof(T) == typeof(char))
            {
                return SpanHelpers.LastIndexOf(ref System.Runtime.CompilerServices.Unsafe.As<T, char>(ref MemoryMarshal.GetReference(span)), System.Runtime.CompilerServices.Unsafe.As<T, char>(ref value), span.Length);
            }
            return SpanHelpers.LastIndexOf(ref MemoryMarshal.GetReference(span), value, span.Length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LastIndexOf<T>(this Span<T> span, ReadOnlySpan<T> value) where T : IEquatable<T>
        {
            if (typeof(T) == typeof(byte))
            {
                return SpanHelpers.LastIndexOf(ref System.Runtime.CompilerServices.Unsafe.As<T, byte>(ref MemoryMarshal.GetReference(span)), span.Length, ref System.Runtime.CompilerServices.Unsafe.As<T, byte>(ref MemoryMarshal.GetReference(value)), value.Length);
            }
            return SpanHelpers.LastIndexOf(ref MemoryMarshal.GetReference(span), span.Length, ref MemoryMarshal.GetReference(value), value.Length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool SequenceEqual<T>(this Span<T> span, ReadOnlySpan<T> other) where T : IEquatable<T>
        {
            int length = span.Length;
            if (default(T) != null && IsTypeComparableAsBytes<T>(out var size))
            {
                if (length == other.Length)
                {
                    return SpanHelpers.SequenceEqual(ref System.Runtime.CompilerServices.Unsafe.As<T, byte>(ref MemoryMarshal.GetReference(span)), ref System.Runtime.CompilerServices.Unsafe.As<T, byte>(ref MemoryMarshal.GetReference(other)), (uint)length * size);
                }
                return false;
            }
            if (length == other.Length)
            {
                return SpanHelpers.SequenceEqual(ref MemoryMarshal.GetReference(span), ref MemoryMarshal.GetReference(other), length);
            }
            return false;
        }

        public static int SequenceCompareTo<T>(this Span<T> span, ReadOnlySpan<T> other) where T : IComparable<T>
        {
            if (typeof(T) == typeof(byte))
            {
                return SpanHelpers.SequenceCompareTo(ref System.Runtime.CompilerServices.Unsafe.As<T, byte>(ref MemoryMarshal.GetReference(span)), span.Length, ref System.Runtime.CompilerServices.Unsafe.As<T, byte>(ref MemoryMarshal.GetReference(other)), other.Length);
            }
            if (typeof(T) == typeof(char))
            {
                return SpanHelpers.SequenceCompareTo(ref System.Runtime.CompilerServices.Unsafe.As<T, char>(ref MemoryMarshal.GetReference(span)), span.Length, ref System.Runtime.CompilerServices.Unsafe.As<T, char>(ref MemoryMarshal.GetReference(other)), other.Length);
            }
            return SpanHelpers.SequenceCompareTo(ref MemoryMarshal.GetReference(span), span.Length, ref MemoryMarshal.GetReference(other), other.Length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOf<T>(this ReadOnlySpan<T> span, T value) where T : IEquatable<T>
        {
            if (typeof(T) == typeof(byte))
            {
                return SpanHelpers.IndexOf(ref System.Runtime.CompilerServices.Unsafe.As<T, byte>(ref MemoryMarshal.GetReference(span)), System.Runtime.CompilerServices.Unsafe.As<T, byte>(ref value), span.Length);
            }
            if (typeof(T) == typeof(char))
            {
                return SpanHelpers.IndexOf(ref System.Runtime.CompilerServices.Unsafe.As<T, char>(ref MemoryMarshal.GetReference(span)), System.Runtime.CompilerServices.Unsafe.As<T, char>(ref value), span.Length);
            }
            return SpanHelpers.IndexOf(ref MemoryMarshal.GetReference(span), value, span.Length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOf<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> value) where T : IEquatable<T>
        {
            if (typeof(T) == typeof(byte))
            {
                return SpanHelpers.IndexOf(ref System.Runtime.CompilerServices.Unsafe.As<T, byte>(ref MemoryMarshal.GetReference(span)), span.Length, ref System.Runtime.CompilerServices.Unsafe.As<T, byte>(ref MemoryMarshal.GetReference(value)), value.Length);
            }
            return SpanHelpers.IndexOf(ref MemoryMarshal.GetReference(span), span.Length, ref MemoryMarshal.GetReference(value), value.Length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LastIndexOf<T>(this ReadOnlySpan<T> span, T value) where T : IEquatable<T>
        {
            if (typeof(T) == typeof(byte))
            {
                return SpanHelpers.LastIndexOf(ref System.Runtime.CompilerServices.Unsafe.As<T, byte>(ref MemoryMarshal.GetReference(span)), System.Runtime.CompilerServices.Unsafe.As<T, byte>(ref value), span.Length);
            }
            if (typeof(T) == typeof(char))
            {
                return SpanHelpers.LastIndexOf(ref System.Runtime.CompilerServices.Unsafe.As<T, char>(ref MemoryMarshal.GetReference(span)), System.Runtime.CompilerServices.Unsafe.As<T, char>(ref value), span.Length);
            }
            return SpanHelpers.LastIndexOf(ref MemoryMarshal.GetReference(span), value, span.Length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LastIndexOf<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> value) where T : IEquatable<T>
        {
            if (typeof(T) == typeof(byte))
            {
                return SpanHelpers.LastIndexOf(ref System.Runtime.CompilerServices.Unsafe.As<T, byte>(ref MemoryMarshal.GetReference(span)), span.Length, ref System.Runtime.CompilerServices.Unsafe.As<T, byte>(ref MemoryMarshal.GetReference(value)), value.Length);
            }
            return SpanHelpers.LastIndexOf(ref MemoryMarshal.GetReference(span), span.Length, ref MemoryMarshal.GetReference(value), value.Length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfAny<T>(this Span<T> span, T value0, T value1) where T : IEquatable<T>
        {
            if (typeof(T) == typeof(byte))
            {
                return SpanHelpers.IndexOfAny(ref System.Runtime.CompilerServices.Unsafe.As<T, byte>(ref MemoryMarshal.GetReference(span)), System.Runtime.CompilerServices.Unsafe.As<T, byte>(ref value0), System.Runtime.CompilerServices.Unsafe.As<T, byte>(ref value1), span.Length);
            }
            return SpanHelpers.IndexOfAny(ref MemoryMarshal.GetReference(span), value0, value1, span.Length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfAny<T>(this Span<T> span, T value0, T value1, T value2) where T : IEquatable<T>
        {
            if (typeof(T) == typeof(byte))
            {
                return SpanHelpers.IndexOfAny(ref System.Runtime.CompilerServices.Unsafe.As<T, byte>(ref MemoryMarshal.GetReference(span)), System.Runtime.CompilerServices.Unsafe.As<T, byte>(ref value0), System.Runtime.CompilerServices.Unsafe.As<T, byte>(ref value1), System.Runtime.CompilerServices.Unsafe.As<T, byte>(ref value2), span.Length);
            }
            return SpanHelpers.IndexOfAny(ref MemoryMarshal.GetReference(span), value0, value1, value2, span.Length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfAny<T>(this Span<T> span, ReadOnlySpan<T> values) where T : IEquatable<T>
        {
            if (typeof(T) == typeof(byte))
            {
                return SpanHelpers.IndexOfAny(ref System.Runtime.CompilerServices.Unsafe.As<T, byte>(ref MemoryMarshal.GetReference(span)), span.Length, ref System.Runtime.CompilerServices.Unsafe.As<T, byte>(ref MemoryMarshal.GetReference(values)), values.Length);
            }
            return SpanHelpers.IndexOfAny(ref MemoryMarshal.GetReference(span), span.Length, ref MemoryMarshal.GetReference(values), values.Length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfAny<T>(this ReadOnlySpan<T> span, T value0, T value1) where T : IEquatable<T>
        {
            if (typeof(T) == typeof(byte))
            {
                return SpanHelpers.IndexOfAny(ref System.Runtime.CompilerServices.Unsafe.As<T, byte>(ref MemoryMarshal.GetReference(span)), System.Runtime.CompilerServices.Unsafe.As<T, byte>(ref value0), System.Runtime.CompilerServices.Unsafe.As<T, byte>(ref value1), span.Length);
            }
            return SpanHelpers.IndexOfAny(ref MemoryMarshal.GetReference(span), value0, value1, span.Length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfAny<T>(this ReadOnlySpan<T> span, T value0, T value1, T value2) where T : IEquatable<T>
        {
            if (typeof(T) == typeof(byte))
            {
                return SpanHelpers.IndexOfAny(ref System.Runtime.CompilerServices.Unsafe.As<T, byte>(ref MemoryMarshal.GetReference(span)), System.Runtime.CompilerServices.Unsafe.As<T, byte>(ref value0), System.Runtime.CompilerServices.Unsafe.As<T, byte>(ref value1), System.Runtime.CompilerServices.Unsafe.As<T, byte>(ref value2), span.Length);
            }
            return SpanHelpers.IndexOfAny(ref MemoryMarshal.GetReference(span), value0, value1, value2, span.Length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfAny<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> values) where T : IEquatable<T>
        {
            if (typeof(T) == typeof(byte))
            {
                return SpanHelpers.IndexOfAny(ref System.Runtime.CompilerServices.Unsafe.As<T, byte>(ref MemoryMarshal.GetReference(span)), span.Length, ref System.Runtime.CompilerServices.Unsafe.As<T, byte>(ref MemoryMarshal.GetReference(values)), values.Length);
            }
            return SpanHelpers.IndexOfAny(ref MemoryMarshal.GetReference(span), span.Length, ref MemoryMarshal.GetReference(values), values.Length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LastIndexOfAny<T>(this Span<T> span, T value0, T value1) where T : IEquatable<T>
        {
            if (typeof(T) == typeof(byte))
            {
                return SpanHelpers.LastIndexOfAny(ref System.Runtime.CompilerServices.Unsafe.As<T, byte>(ref MemoryMarshal.GetReference(span)), System.Runtime.CompilerServices.Unsafe.As<T, byte>(ref value0), System.Runtime.CompilerServices.Unsafe.As<T, byte>(ref value1), span.Length);
            }
            return SpanHelpers.LastIndexOfAny(ref MemoryMarshal.GetReference(span), value0, value1, span.Length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LastIndexOfAny<T>(this Span<T> span, T value0, T value1, T value2) where T : IEquatable<T>
        {
            if (typeof(T) == typeof(byte))
            {
                return SpanHelpers.LastIndexOfAny(ref System.Runtime.CompilerServices.Unsafe.As<T, byte>(ref MemoryMarshal.GetReference(span)), System.Runtime.CompilerServices.Unsafe.As<T, byte>(ref value0), System.Runtime.CompilerServices.Unsafe.As<T, byte>(ref value1), System.Runtime.CompilerServices.Unsafe.As<T, byte>(ref value2), span.Length);
            }
            return SpanHelpers.LastIndexOfAny(ref MemoryMarshal.GetReference(span), value0, value1, value2, span.Length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LastIndexOfAny<T>(this Span<T> span, ReadOnlySpan<T> values) where T : IEquatable<T>
        {
            if (typeof(T) == typeof(byte))
            {
                return SpanHelpers.LastIndexOfAny(ref System.Runtime.CompilerServices.Unsafe.As<T, byte>(ref MemoryMarshal.GetReference(span)), span.Length, ref System.Runtime.CompilerServices.Unsafe.As<T, byte>(ref MemoryMarshal.GetReference(values)), values.Length);
            }
            return SpanHelpers.LastIndexOfAny(ref MemoryMarshal.GetReference(span), span.Length, ref MemoryMarshal.GetReference(values), values.Length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LastIndexOfAny<T>(this ReadOnlySpan<T> span, T value0, T value1) where T : IEquatable<T>
        {
            if (typeof(T) == typeof(byte))
            {
                return SpanHelpers.LastIndexOfAny(ref System.Runtime.CompilerServices.Unsafe.As<T, byte>(ref MemoryMarshal.GetReference(span)), System.Runtime.CompilerServices.Unsafe.As<T, byte>(ref value0), System.Runtime.CompilerServices.Unsafe.As<T, byte>(ref value1), span.Length);
            }
            return SpanHelpers.LastIndexOfAny(ref MemoryMarshal.GetReference(span), value0, value1, span.Length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LastIndexOfAny<T>(this ReadOnlySpan<T> span, T value0, T value1, T value2) where T : IEquatable<T>
        {
            if (typeof(T) == typeof(byte))
            {
                return SpanHelpers.LastIndexOfAny(ref System.Runtime.CompilerServices.Unsafe.As<T, byte>(ref MemoryMarshal.GetReference(span)), System.Runtime.CompilerServices.Unsafe.As<T, byte>(ref value0), System.Runtime.CompilerServices.Unsafe.As<T, byte>(ref value1), System.Runtime.CompilerServices.Unsafe.As<T, byte>(ref value2), span.Length);
            }
            return SpanHelpers.LastIndexOfAny(ref MemoryMarshal.GetReference(span), value0, value1, value2, span.Length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LastIndexOfAny<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> values) where T : IEquatable<T>
        {
            if (typeof(T) == typeof(byte))
            {
                return SpanHelpers.LastIndexOfAny(ref System.Runtime.CompilerServices.Unsafe.As<T, byte>(ref MemoryMarshal.GetReference(span)), span.Length, ref System.Runtime.CompilerServices.Unsafe.As<T, byte>(ref MemoryMarshal.GetReference(values)), values.Length);
            }
            return SpanHelpers.LastIndexOfAny(ref MemoryMarshal.GetReference(span), span.Length, ref MemoryMarshal.GetReference(values), values.Length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool SequenceEqual<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> other) where T : IEquatable<T>
        {
            int length = span.Length;
            if (default(T) != null && IsTypeComparableAsBytes<T>(out var size))
            {
                if (length == other.Length)
                {
                    return SpanHelpers.SequenceEqual(ref System.Runtime.CompilerServices.Unsafe.As<T, byte>(ref MemoryMarshal.GetReference(span)), ref System.Runtime.CompilerServices.Unsafe.As<T, byte>(ref MemoryMarshal.GetReference(other)), (uint)length * size);
                }
                return false;
            }
            if (length == other.Length)
            {
                return SpanHelpers.SequenceEqual(ref MemoryMarshal.GetReference(span), ref MemoryMarshal.GetReference(other), length);
            }
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int SequenceCompareTo<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> other) where T : IComparable<T>
        {
            if (typeof(T) == typeof(byte))
            {
                return SpanHelpers.SequenceCompareTo(ref System.Runtime.CompilerServices.Unsafe.As<T, byte>(ref MemoryMarshal.GetReference(span)), span.Length, ref System.Runtime.CompilerServices.Unsafe.As<T, byte>(ref MemoryMarshal.GetReference(other)), other.Length);
            }
            if (typeof(T) == typeof(char))
            {
                return SpanHelpers.SequenceCompareTo(ref System.Runtime.CompilerServices.Unsafe.As<T, char>(ref MemoryMarshal.GetReference(span)), span.Length, ref System.Runtime.CompilerServices.Unsafe.As<T, char>(ref MemoryMarshal.GetReference(other)), other.Length);
            }
            return SpanHelpers.SequenceCompareTo(ref MemoryMarshal.GetReference(span), span.Length, ref MemoryMarshal.GetReference(other), other.Length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool StartsWith<T>(this Span<T> span, ReadOnlySpan<T> value) where T : IEquatable<T>
        {
            int length = value.Length;
            if (default(T) != null && IsTypeComparableAsBytes<T>(out var size))
            {
                if (length <= span.Length)
                {
                    return SpanHelpers.SequenceEqual(ref System.Runtime.CompilerServices.Unsafe.As<T, byte>(ref MemoryMarshal.GetReference(span)), ref System.Runtime.CompilerServices.Unsafe.As<T, byte>(ref MemoryMarshal.GetReference(value)), (uint)length * size);
                }
                return false;
            }
            if (length <= span.Length)
            {
                return SpanHelpers.SequenceEqual(ref MemoryMarshal.GetReference(span), ref MemoryMarshal.GetReference(value), length);
            }
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool StartsWith<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> value) where T : IEquatable<T>
        {
            int length = value.Length;
            if (default(T) != null && IsTypeComparableAsBytes<T>(out var size))
            {
                if (length <= span.Length)
                {
                    return SpanHelpers.SequenceEqual(ref System.Runtime.CompilerServices.Unsafe.As<T, byte>(ref MemoryMarshal.GetReference(span)), ref System.Runtime.CompilerServices.Unsafe.As<T, byte>(ref MemoryMarshal.GetReference(value)), (uint)length * size);
                }
                return false;
            }
            if (length <= span.Length)
            {
                return SpanHelpers.SequenceEqual(ref MemoryMarshal.GetReference(span), ref MemoryMarshal.GetReference(value), length);
            }
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool EndsWith<T>(this Span<T> span, ReadOnlySpan<T> value) where T : IEquatable<T>
        {
            int length = span.Length;
            int length2 = value.Length;
            if (default(T) != null && IsTypeComparableAsBytes<T>(out var size))
            {
                if (length2 <= length)
                {
                    return SpanHelpers.SequenceEqual(ref System.Runtime.CompilerServices.Unsafe.As<T, byte>(ref System.Runtime.CompilerServices.Unsafe.Add<T>(ref MemoryMarshal.GetReference(span), length - length2)), ref System.Runtime.CompilerServices.Unsafe.As<T, byte>(ref MemoryMarshal.GetReference(value)), (uint)length2 * size);
                }
                return false;
            }
            if (length2 <= length)
            {
                return SpanHelpers.SequenceEqual(ref System.Runtime.CompilerServices.Unsafe.Add<T>(ref MemoryMarshal.GetReference(span), length - length2), ref MemoryMarshal.GetReference(value), length2);
            }
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool EndsWith<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> value) where T : IEquatable<T>
        {
            int length = span.Length;
            int length2 = value.Length;
            if (default(T) != null && IsTypeComparableAsBytes<T>(out var size))
            {
                if (length2 <= length)
                {
                    return SpanHelpers.SequenceEqual(ref System.Runtime.CompilerServices.Unsafe.As<T, byte>(ref System.Runtime.CompilerServices.Unsafe.Add<T>(ref MemoryMarshal.GetReference(span), length - length2)), ref System.Runtime.CompilerServices.Unsafe.As<T, byte>(ref MemoryMarshal.GetReference(value)), (uint)length2 * size);
                }
                return false;
            }
            if (length2 <= length)
            {
                return SpanHelpers.SequenceEqual(ref System.Runtime.CompilerServices.Unsafe.Add<T>(ref MemoryMarshal.GetReference(span), length - length2), ref MemoryMarshal.GetReference(value), length2);
            }
            return false;
        }

        public static void Reverse<T>(this Span<T> span)
        {
            ref T reference = ref MemoryMarshal.GetReference(span);
            int num = 0;
            int num2 = span.Length - 1;
            while (num < num2)
            {
                T val = System.Runtime.CompilerServices.Unsafe.Add<T>(ref reference, num);
                System.Runtime.CompilerServices.Unsafe.Add<T>(ref reference, num) = System.Runtime.CompilerServices.Unsafe.Add<T>(ref reference, num2);
                System.Runtime.CompilerServices.Unsafe.Add<T>(ref reference, num2) = val;
                num++;
                num2--;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<T> AsSpan<T>(this T[] array)
        {
            return new Span<T>(array);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<T> AsSpan<T>(this T[] array, int start, int length)
        {
            return new Span<T>(array, start, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<T> AsSpan<T>(this ArraySegment<T> segment)
        {
            return new Span<T>(segment.Array, segment.Offset, segment.Count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<T> AsSpan<T>(this ArraySegment<T> segment, int start)
        {
            if ((uint)start > segment.Count)
            {
                System.ThrowHelper.ThrowArgumentOutOfRangeException(System.ExceptionArgument.start);
            }
            return new Span<T>(segment.Array, segment.Offset + start, segment.Count - start);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<T> AsSpan<T>(this ArraySegment<T> segment, int start, int length)
        {
            if ((uint)start > segment.Count)
            {
                System.ThrowHelper.ThrowArgumentOutOfRangeException(System.ExceptionArgument.start);
            }
            if ((uint)length > segment.Count - start)
            {
                System.ThrowHelper.ThrowArgumentOutOfRangeException(System.ExceptionArgument.length);
            }
            return new Span<T>(segment.Array, segment.Offset + start, length);
        }

        public static Memory<T> AsMemory<T>(this T[] array)
        {
            return new Memory<T>(array);
        }

        public static Memory<T> AsMemory<T>(this T[] array, int start)
        {
            return new Memory<T>(array, start);
        }

        public static Memory<T> AsMemory<T>(this T[] array, int start, int length)
        {
            return new Memory<T>(array, start, length);
        }

        public static Memory<T> AsMemory<T>(this ArraySegment<T> segment)
        {
            return new Memory<T>(segment.Array, segment.Offset, segment.Count);
        }

        public static Memory<T> AsMemory<T>(this ArraySegment<T> segment, int start)
        {
            if ((uint)start > segment.Count)
            {
                System.ThrowHelper.ThrowArgumentOutOfRangeException(System.ExceptionArgument.start);
            }
            return new Memory<T>(segment.Array, segment.Offset + start, segment.Count - start);
        }

        public static Memory<T> AsMemory<T>(this ArraySegment<T> segment, int start, int length)
        {
            if ((uint)start > segment.Count)
            {
                System.ThrowHelper.ThrowArgumentOutOfRangeException(System.ExceptionArgument.start);
            }
            if ((uint)length > segment.Count - start)
            {
                System.ThrowHelper.ThrowArgumentOutOfRangeException(System.ExceptionArgument.length);
            }
            return new Memory<T>(segment.Array, segment.Offset + start, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyTo<T>(this T[] source, Span<T> destination)
        {
            new ReadOnlySpan<T>(source).CopyTo(destination);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyTo<T>(this T[] source, Memory<T> destination)
        {
            source.CopyTo(destination.Span);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Overlaps<T>(this Span<T> span, ReadOnlySpan<T> other)
        {
            return ((ReadOnlySpan<T>)span).Overlaps(other);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Overlaps<T>(this Span<T> span, ReadOnlySpan<T> other, out int elementOffset)
        {
            return ((ReadOnlySpan<T>)span).Overlaps(other, out elementOffset);
        }

        public static bool Overlaps<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> other)
        {
            if (span.IsEmpty || other.IsEmpty)
            {
                return false;
            }
            IntPtr intPtr = System.Runtime.CompilerServices.Unsafe.ByteOffset<T>(ref MemoryMarshal.GetReference(span), ref MemoryMarshal.GetReference(other));
            if (System.Runtime.CompilerServices.Unsafe.SizeOf<IntPtr>() == 4)
            {
                if ((uint)(int)intPtr >= (uint)(span.Length * System.Runtime.CompilerServices.Unsafe.SizeOf<T>()))
                {
                    return (uint)(int)intPtr > (uint)(-(other.Length * System.Runtime.CompilerServices.Unsafe.SizeOf<T>()));
                }
                return true;
            }
            if ((ulong)(long)intPtr >= (ulong)((long)span.Length * (long)System.Runtime.CompilerServices.Unsafe.SizeOf<T>()))
            {
                return (ulong)(long)intPtr > (ulong)(-((long)other.Length * (long)System.Runtime.CompilerServices.Unsafe.SizeOf<T>()));
            }
            return true;
        }

        public static bool Overlaps<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> other, out int elementOffset)
        {
            if (span.IsEmpty || other.IsEmpty)
            {
                elementOffset = 0;
                return false;
            }
            IntPtr intPtr = System.Runtime.CompilerServices.Unsafe.ByteOffset<T>(ref MemoryMarshal.GetReference(span), ref MemoryMarshal.GetReference(other));
            if (System.Runtime.CompilerServices.Unsafe.SizeOf<IntPtr>() == 4)
            {
                if ((uint)(int)intPtr < (uint)(span.Length * System.Runtime.CompilerServices.Unsafe.SizeOf<T>()) || (uint)(int)intPtr > (uint)(-(other.Length * System.Runtime.CompilerServices.Unsafe.SizeOf<T>())))
                {
                    if ((int)intPtr % System.Runtime.CompilerServices.Unsafe.SizeOf<T>() != 0)
                    {
                        System.ThrowHelper.ThrowArgumentException_OverlapAlignmentMismatch();
                    }
                    elementOffset = (int)intPtr / System.Runtime.CompilerServices.Unsafe.SizeOf<T>();
                    return true;
                }
                elementOffset = 0;
                return false;
            }
            if ((ulong)(long)intPtr < (ulong)((long)span.Length * (long)System.Runtime.CompilerServices.Unsafe.SizeOf<T>()) || (ulong)(long)intPtr > (ulong)(-((long)other.Length * (long)System.Runtime.CompilerServices.Unsafe.SizeOf<T>())))
            {
                if ((long)intPtr % System.Runtime.CompilerServices.Unsafe.SizeOf<T>() != 0L)
                {
                    System.ThrowHelper.ThrowArgumentException_OverlapAlignmentMismatch();
                }
                elementOffset = (int)((long)intPtr / System.Runtime.CompilerServices.Unsafe.SizeOf<T>());
                return true;
            }
            elementOffset = 0;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int BinarySearch<T>(this Span<T> span, IComparable<T> comparable)
        {
            return span.BinarySearch<T, IComparable<T>>(comparable);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int BinarySearch<T, TComparable>(this Span<T> span, TComparable comparable) where TComparable : IComparable<T>
        {
            return BinarySearch((ReadOnlySpan<T>)span, comparable);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int BinarySearch<T, TComparer>(this Span<T> span, T value, TComparer comparer) where TComparer : IComparer<T>
        {
            return ((ReadOnlySpan<T>)span).BinarySearch(value, comparer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int BinarySearch<T>(this ReadOnlySpan<T> span, IComparable<T> comparable)
        {
            return MemoryExtensions.BinarySearch<T, IComparable<T>>(span, comparable);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int BinarySearch<T, TComparable>(this ReadOnlySpan<T> span, TComparable comparable) where TComparable : IComparable<T>
        {
            return SpanHelpers.BinarySearch(span, comparable);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int BinarySearch<T, TComparer>(this ReadOnlySpan<T> span, T value, TComparer comparer) where TComparer : IComparer<T>
        {
            if (comparer == null)
            {
                System.ThrowHelper.ThrowArgumentNullException(System.ExceptionArgument.comparer);
            }
            SpanHelpers.ComparerComparable<T, TComparer> comparable = new SpanHelpers.ComparerComparable<T, TComparer>(value, comparer);
            return BinarySearch(span, comparable);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsTypeComparableAsBytes<T>(out uint size)
        {
            if (typeof(T) == typeof(byte) || typeof(T) == typeof(sbyte))
            {
                size = (uint)1;
                return true;
            }
            if (typeof(T) == typeof(char) || typeof(T) == typeof(short) || typeof(T) == typeof(ushort))
            {
                size = (uint)2;
                return true;
            }
            if (typeof(T) == typeof(int) || typeof(T) == typeof(uint))
            {
                size = (uint)4;
                return true;
            }
            if (typeof(T) == typeof(long) || typeof(T) == typeof(ulong))
            {
                size = (uint)8;
                return true;
            }
            size = default(uint);
            return false;
        }

        public static Span<T> AsSpan<T>(this T[] array, int start)
        {
            return Span<T>.Create(array, start);
        }

        public static bool Contains(this ReadOnlySpan<char> span, ReadOnlySpan<char> value, StringComparison comparisonType)
        {
            return span.IndexOf(value, comparisonType) >= 0;
        }

        public static bool Equals(this ReadOnlySpan<char> span, ReadOnlySpan<char> other, StringComparison comparisonType)
        {
            switch (comparisonType)
            {
                case StringComparison.Ordinal:
                    return span.SequenceEqual(other);
                case StringComparison.OrdinalIgnoreCase:
                    if (span.Length != other.Length)
                    {
                        return false;
                    }
                    return EqualsOrdinalIgnoreCase(span, other);
                default:
                    return span.ToString().Equals(other.ToString(), comparisonType);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool EqualsOrdinalIgnoreCase(ReadOnlySpan<char> span, ReadOnlySpan<char> other)
        {
            if (other.Length == 0)
            {
                return true;
            }
            return CompareToOrdinalIgnoreCase(span, other) == 0;
        }

        public static int CompareTo(this ReadOnlySpan<char> span, ReadOnlySpan<char> other, StringComparison comparisonType)
        {
            if (comparisonType== StringComparison.Ordinal)
            {
                return span.SequenceCompareTo(other);
            }
            else if (comparisonType == StringComparison.OrdinalIgnoreCase)
            {
                return CompareToOrdinalIgnoreCase(span, other);
            }
            return string.Compare(span.ToString(), other.ToString(), comparisonType);
        }

        private unsafe static int CompareToOrdinalIgnoreCase(ReadOnlySpan<char> strA, ReadOnlySpan<char> strB)
        {
            int num = Math.Min(strA.Length, strB.Length);
            int num2 = num;
            fixed (char* ptr = &MemoryMarshal.GetReference(strA))
            {
                fixed (char* ptr3 = &MemoryMarshal.GetReference(strB))
                {
                    char* ptr2 = ptr;
                    char* ptr4 = ptr3;
                    while (num != 0 && *ptr2 <= '\u007f' && *ptr4 <= '\u007f')
                    {
                        int num3 = *ptr2;
                        int num4 = *ptr4;
                        if (num3 == num4)
                        {
                            ptr2++;
                            ptr4++;
                            num--;
                            continue;
                        }
                        if ((uint)(num3 - 97) <= 25u)
                        {
                            num3 -= 32;
                        }
                        if ((uint)(num4 - 97) <= 25u)
                        {
                            num4 -= 32;
                        }
                        if (num3 != num4)
                        {
                            return num3 - num4;
                        }
                        ptr2++;
                        ptr4++;
                        num--;
                    }
                    if (num == 0)
                    {
                        return strA.Length - strB.Length;
                    }
                    num2 -= num;
                    return string.Compare(strA.Slice(num2).ToString(), strB.Slice(num2).ToString(), StringComparison.OrdinalIgnoreCase);
                }
            }
        }

        public static int IndexOf(this ReadOnlySpan<char> span, ReadOnlySpan<char> value, StringComparison comparisonType)
        {
            if (comparisonType == StringComparison.Ordinal)
            {
                return span.IndexOf(value);
            }
            return span.ToString().IndexOf(value.ToString(), comparisonType);
        }

        public static int ToLower(this ReadOnlySpan<char> source, Span<char> destination, CultureInfo culture)
        {
            if (culture == null)
            {
                System.ThrowHelper.ThrowArgumentNullException(System.ExceptionArgument.culture);
            }
            if (destination.Length < source.Length)
            {
                return -1;
            }
            string text = source.ToString();
            string text2 = text.ToLower(culture);
            text2.AsSpan().CopyTo(destination);
            return source.Length;
        }

        public static int ToLowerInvariant(this ReadOnlySpan<char> source, Span<char> destination)
        {
            return source.ToLower(destination, CultureInfo.InvariantCulture);
        }

        public static int ToUpper(this ReadOnlySpan<char> source, Span<char> destination, CultureInfo culture)
        {
            if (culture == null)
            {
                System.ThrowHelper.ThrowArgumentNullException(System.ExceptionArgument.culture);
            }
            if (destination.Length < source.Length)
            {
                return -1;
            }
            string text = source.ToString();
            string text2 = text.ToUpper(culture);
            text2.AsSpan().CopyTo(destination);
            return source.Length;
        }

        public static int ToUpperInvariant(this ReadOnlySpan<char> source, Span<char> destination)
        {
            return source.ToUpper(destination, CultureInfo.InvariantCulture);
        }

        public static bool EndsWith(this ReadOnlySpan<char> span, ReadOnlySpan<char> value, StringComparison comparisonType)
        {
            switch (comparisonType)
            {
                case StringComparison.Ordinal:
                    return span.EndsWith(value);
                case StringComparison.OrdinalIgnoreCase:
                    if (value.Length <= span.Length)
                    {
                        return EqualsOrdinalIgnoreCase(span.Slice(span.Length - value.Length), value);
                    }
                    return false;
                default:
                    {
                        string text = span.ToString();
                        string value2 = value.ToString();
                        return text.EndsWith(value2, comparisonType);
                    }
            }
        }

        public static bool StartsWith(this ReadOnlySpan<char> span, ReadOnlySpan<char> value, StringComparison comparisonType)
        {
            switch (comparisonType)
            {
                case StringComparison.Ordinal:
                    return span.StartsWith(value);
                case StringComparison.OrdinalIgnoreCase:
                    if (value.Length <= span.Length)
                    {
                        return EqualsOrdinalIgnoreCase(span.Slice(0, value.Length), value);
                    }
                    return false;
                default:
                    {
                        string text = span.ToString();
                        string value2 = value.ToString();
                        return text.StartsWith(value2, comparisonType);
                    }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<char> AsSpan(this string text)
        {
            if (text == null)
            {
                return default(ReadOnlySpan<char>);
            }
            return new ReadOnlySpan<char>(System.Runtime.CompilerServices.Unsafe.As<Pinnable<char>>((object)text), StringAdjustment, text.Length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<char> AsSpan(this string text, int start)
        {
            if (text == null)
            {
                if (start != 0)
                {
                    System.ThrowHelper.ThrowArgumentOutOfRangeException(System.ExceptionArgument.start);
                }
                return default(ReadOnlySpan<char>);
            }
            if ((uint)start > (uint)text.Length)
            {
                System.ThrowHelper.ThrowArgumentOutOfRangeException(System.ExceptionArgument.start);
            }
            return new ReadOnlySpan<char>(System.Runtime.CompilerServices.Unsafe.As<Pinnable<char>>((object)text), StringAdjustment + start * 2, text.Length - start);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<char> AsSpan(this string text, int start, int length)
        {
            if (text == null)
            {
                if (start != 0 || length != 0)
                {
                    System.ThrowHelper.ThrowArgumentOutOfRangeException(System.ExceptionArgument.start);
                }
                return default(ReadOnlySpan<char>);
            }
            if ((uint)start > (uint)text.Length || (uint)length > (uint)(text.Length - start))
            {
                System.ThrowHelper.ThrowArgumentOutOfRangeException(System.ExceptionArgument.start);
            }
            return new ReadOnlySpan<char>(System.Runtime.CompilerServices.Unsafe.As<Pinnable<char>>((object)text), StringAdjustment + start * 2, length);
        }

        public static ReadOnlyMemory<char> AsMemory(this string text)
        {
            if (text == null)
            {
                return default(ReadOnlyMemory<char>);
            }
            return new ReadOnlyMemory<char>(text, 0, text.Length);
        }

        public static ReadOnlyMemory<char> AsMemory(this string text, int start)
        {
            if (text == null)
            {
                if (start != 0)
                {
                    System.ThrowHelper.ThrowArgumentOutOfRangeException(System.ExceptionArgument.start);
                }
                return default(ReadOnlyMemory<char>);
            }
            if ((uint)start > (uint)text.Length)
            {
                System.ThrowHelper.ThrowArgumentOutOfRangeException(System.ExceptionArgument.start);
            }
            return new ReadOnlyMemory<char>(text, start, text.Length - start);
        }

        public static ReadOnlyMemory<char> AsMemory(this string text, int start, int length)
        {
            if (text == null)
            {
                if (start != 0 || length != 0)
                {
                    System.ThrowHelper.ThrowArgumentOutOfRangeException(System.ExceptionArgument.start);
                }
                return default(ReadOnlyMemory<char>);
            }
            if ((uint)start > (uint)text.Length || (uint)length > (uint)(text.Length - start))
            {
                System.ThrowHelper.ThrowArgumentOutOfRangeException(System.ExceptionArgument.start);
            }
            return new ReadOnlyMemory<char>(text, start, length);
        }

        private unsafe static IntPtr MeasureStringAdjustment()
        {
            string text = "a";
            fixed (char* ptr = text)
            {
                return System.Runtime.CompilerServices.Unsafe.ByteOffset<char>(ref System.Runtime.CompilerServices.Unsafe.As<Pinnable<char>>((object)text).Data, ref System.Runtime.CompilerServices.Unsafe.AsRef<char>((void*)ptr));
            }
        }
    }
}
