﻿using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System;

namespace CaoNC.System
{
    public static class MemoryMarshal
    {
        public static bool TryGetArray<T>(ReadOnlyMemory<T> memory, out ArraySegment<T> segment)
        {
            int start;
            int length;
            object objectStartLength = memory.GetObjectStartLength(out start, out length);
            if (start < 0)
            {
                if (((MemoryManager<T>)objectStartLength).TryGetArray(out var segment2))
                {
                    segment = new ArraySegment<T>(segment2.Array, segment2.Offset + (start & 0x7FFFFFFF), length);
                    return true;
                }
            }
            else if (objectStartLength is T[] array)
            {
                segment = new ArraySegment<T>(array, start, length & 0x7FFFFFFF);
                return true;
            }
            if ((length & 0x7FFFFFFF) == 0)
            {
                segment = new ArraySegment<T>(SpanHelpers.PerTypeValues<T>.EmptyArray);
                return true;
            }
            segment = default(ArraySegment<T>);
            return false;
        }

        public static bool TryGetMemoryManager<T, TManager>(ReadOnlyMemory<T> memory, out TManager manager) where TManager : MemoryManager<T>
        {
            int start;
            int length;
            TManager val = (manager = memory.GetObjectStartLength(out start, out length) as TManager);
            return manager != null;
        }

        public static bool TryGetMemoryManager<T, TManager>(ReadOnlyMemory<T> memory, out TManager manager, out int start, out int length) where TManager : MemoryManager<T>
        {
            TManager val = (manager = memory.GetObjectStartLength(out start, out length) as TManager);
            start &= int.MaxValue;
            if (manager == null)
            {
                start = 0;
                length = 0;
                return false;
            }
            return true;
        }

        public static IEnumerable<T> ToEnumerable<T>(ReadOnlyMemory<T> memory)
        {
            for (int i = 0; i < memory.Length; i++)
            {
                yield return memory.Span[i];
            }
        }

        public static bool TryGetString(ReadOnlyMemory<char> memory, out string text, out int start, out int length)
        {
            if (memory.GetObjectStartLength(out var start2, out var length2) is string text2)
            {
                text = text2;
                start = start2;
                length = length2;
                return true;
            }
            text = null;
            start = 0;
            length = 0;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Read<T>(ReadOnlySpan<byte> source) where T : struct
        {
            if (SpanHelpers.IsReferenceOrContainsReferences<T>())
            {
                System.ThrowHelper.ThrowArgumentException_InvalidTypeWithPointersNotSupported(typeof(T));
            }
            if (System.Runtime.CompilerServices.Unsafe.SizeOf<T>() > source.Length)
            {
                System.ThrowHelper.ThrowArgumentOutOfRangeException(System.ExceptionArgument.length);
            }
            return System.Runtime.CompilerServices.Unsafe.ReadUnaligned<T>(ref GetReference(source));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryRead<T>(ReadOnlySpan<byte> source, out T value) where T : struct
        {
            if (SpanHelpers.IsReferenceOrContainsReferences<T>())
            {
                System.ThrowHelper.ThrowArgumentException_InvalidTypeWithPointersNotSupported(typeof(T));
            }
            if (System.Runtime.CompilerServices.Unsafe.SizeOf<T>() > (uint)source.Length)
            {
                value = default(T);
                return false;
            }
            value = System.Runtime.CompilerServices.Unsafe.ReadUnaligned<T>(ref GetReference(source));
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write<T>(Span<byte> destination, ref T value) where T : struct
        {
            if (SpanHelpers.IsReferenceOrContainsReferences<T>())
            {
                System.ThrowHelper.ThrowArgumentException_InvalidTypeWithPointersNotSupported(typeof(T));
            }
            if ((uint)System.Runtime.CompilerServices.Unsafe.SizeOf<T>() > (uint)destination.Length)
            {
                System.ThrowHelper.ThrowArgumentOutOfRangeException(System.ExceptionArgument.length);
            }
            System.Runtime.CompilerServices.Unsafe.WriteUnaligned<T>(ref GetReference(destination), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWrite<T>(Span<byte> destination, ref T value) where T : struct
        {
            if (SpanHelpers.IsReferenceOrContainsReferences<T>())
            {
                System.ThrowHelper.ThrowArgumentException_InvalidTypeWithPointersNotSupported(typeof(T));
            }
            if (System.Runtime.CompilerServices.Unsafe.SizeOf<T>() > (uint)destination.Length)
            {
                return false;
            }
            System.Runtime.CompilerServices.Unsafe.WriteUnaligned<T>(ref GetReference(destination), value);
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Memory<T> CreateFromPinnedArray<T>(T[] array, int start, int length)
        {
            if (array == null)
            {
                if (start != 0 || length != 0)
                {
                    System.ThrowHelper.ThrowArgumentOutOfRangeException();
                }
                return default(Memory<T>);
            }
            if (default(T) == null && array.GetType() != typeof(T[]))
            {
                System.ThrowHelper.ThrowArrayTypeMismatchException();
            }
            if ((uint)start > (uint)array.Length || (uint)length > (uint)(array.Length - start))
            {
                System.ThrowHelper.ThrowArgumentOutOfRangeException();
            }
            return new Memory<T>((object)array, start, length | int.MinValue);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<byte> AsBytes<T>(Span<T> span) where T : struct
        {
            if (SpanHelpers.IsReferenceOrContainsReferences<T>())
            {
                System.ThrowHelper.ThrowArgumentException_InvalidTypeWithPointersNotSupported(typeof(T));
            }
            int length = checked(span.Length * System.Runtime.CompilerServices.Unsafe.SizeOf<T>());
            return new Span<byte>(System.Runtime.CompilerServices.Unsafe.As<Pinnable<byte>>((object)span.Pinnable), span.ByteOffset, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<byte> AsBytes<T>(ReadOnlySpan<T> span) where T : struct
        {
            if (SpanHelpers.IsReferenceOrContainsReferences<T>())
            {
                System.ThrowHelper.ThrowArgumentException_InvalidTypeWithPointersNotSupported(typeof(T));
            }
            int length = checked(span.Length * System.Runtime.CompilerServices.Unsafe.SizeOf<T>());
            return new ReadOnlySpan<byte>(System.Runtime.CompilerServices.Unsafe.As<Pinnable<byte>>((object)span.Pinnable), span.ByteOffset, length);
        }

        public static Memory<T> AsMemory<T>(ReadOnlyMemory<T> memory)
        {
            return System.Runtime.CompilerServices.Unsafe.As<ReadOnlyMemory<T>, Memory<T>>(ref memory);
        }

        public unsafe static ref T GetReference<T>(Span<T> span)
        {
            if (span.Pinnable == null)
            {
                return ref System.Runtime.CompilerServices.Unsafe.AsRef<T>(span.ByteOffset.ToPointer());
            }
            return ref System.Runtime.CompilerServices.Unsafe.AddByteOffset<T>(ref span.Pinnable.Data, span.ByteOffset);
        }

        public unsafe static ref T GetReference<T>(ReadOnlySpan<T> span)
        {
            if (span.Pinnable == null)
            {
                return ref System.Runtime.CompilerServices.Unsafe.AsRef<T>(span.ByteOffset.ToPointer());
            }
            return ref System.Runtime.CompilerServices.Unsafe.AddByteOffset<T>(ref span.Pinnable.Data, span.ByteOffset);
        }

        public static Span<TTo> Cast<TFrom, TTo>(Span<TFrom> span) where TFrom : struct where TTo : struct
        {
            if (SpanHelpers.IsReferenceOrContainsReferences<TFrom>())
            {
                System.ThrowHelper.ThrowArgumentException_InvalidTypeWithPointersNotSupported(typeof(TFrom));
            }
            if (SpanHelpers.IsReferenceOrContainsReferences<TTo>())
            {
                System.ThrowHelper.ThrowArgumentException_InvalidTypeWithPointersNotSupported(typeof(TTo));
            }
            checked
            {
                int length = (int)unchecked(checked(unchecked((long)span.Length) * unchecked((long)System.Runtime.CompilerServices.Unsafe.SizeOf<TFrom>())) / System.Runtime.CompilerServices.Unsafe.SizeOf<TTo>());
                return new Span<TTo>(System.Runtime.CompilerServices.Unsafe.As<Pinnable<TTo>>((object)span.Pinnable), span.ByteOffset, length);
            }
        }

        public static ReadOnlySpan<TTo> Cast<TFrom, TTo>(ReadOnlySpan<TFrom> span) where TFrom : struct where TTo : struct
        {
            if (SpanHelpers.IsReferenceOrContainsReferences<TFrom>())
            {
                System.ThrowHelper.ThrowArgumentException_InvalidTypeWithPointersNotSupported(typeof(TFrom));
            }
            if (SpanHelpers.IsReferenceOrContainsReferences<TTo>())
            {
                System.ThrowHelper.ThrowArgumentException_InvalidTypeWithPointersNotSupported(typeof(TTo));
            }
            checked
            {
                int length = (int)unchecked(checked(unchecked((long)span.Length) * unchecked((long)System.Runtime.CompilerServices.Unsafe.SizeOf<TFrom>())) / System.Runtime.CompilerServices.Unsafe.SizeOf<TTo>());
                return new ReadOnlySpan<TTo>(System.Runtime.CompilerServices.Unsafe.As<Pinnable<TTo>>((object)span.Pinnable), span.ByteOffset, length);
            }
        }
    }
}
