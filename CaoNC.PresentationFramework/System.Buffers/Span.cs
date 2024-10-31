using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System;

namespace CaoNC.System
{
    [DebuggerTypeProxy(typeof(SpanDebugView<>))]
    [DebuggerDisplay("{ToString(),raw}")]
    [DebuggerTypeProxy(typeof(SpanDebugView<>))]
    [DebuggerDisplay("{ToString(),raw}")]
    public readonly ref struct Span<T>
    {
        public ref struct Enumerator
        {
            private readonly Span<T> _span;

            private int _index;

            public ref T Current
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get
                {
                    return ref _span[_index];
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal Enumerator(Span<T> span)
            {
                _span = span;
                _index = -1;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext()
            {
                int num = _index + 1;
                if (num < _span.Length)
                {
                    _index = num;
                    return true;
                }
                return false;
            }
        }

        private readonly Pinnable<T> _pinnable;

        private readonly IntPtr _byteOffset;

        private readonly int _length;

        /// <returns></returns>
        public int Length => _length;

        /// <returns></returns>
        public bool IsEmpty => _length == 0;

        /// <returns></returns>
        public static Span<T> Empty => default(Span<T>);

        /// <param name="index"></param>
        /// <returns></returns>
        public unsafe ref T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if ((uint)index >= (uint)_length)
                {
                    System.ThrowHelper.ThrowIndexOutOfRangeException();
                }
                if (_pinnable == null)
                {
                    return ref System.Runtime.CompilerServices.Unsafe.Add<T>(ref System.Runtime.CompilerServices.Unsafe.AsRef<T>(_byteOffset.ToPointer()), index);
                }
                return ref System.Runtime.CompilerServices.Unsafe.Add<T>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset<T>(ref _pinnable.Data, _byteOffset), index);
            }
        }

        internal Pinnable<T> Pinnable => _pinnable;

        internal IntPtr ByteOffset => _byteOffset;

        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(Span<T> left, Span<T> right)
        {
            return !(left == right);
        }

        /// <param name="obj"></param>
        /// <returns></returns>
        [Obsolete("Equals() on Span will always throw an exception. Use == instead.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj)
        {
            throw new NotSupportedException(System.SR.NotSupported_CannotCallEqualsOnSpan);
        }

        /// <returns></returns>
        [Obsolete("GetHashCode() on Span will always throw an exception.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode()
        {
            throw new NotSupportedException(System.SR.NotSupported_CannotCallGetHashCodeOnSpan);
        }

        public static implicit operator Span<T>(T[] array)
        {
            return new Span<T>(array);
        }

        public static implicit operator Span<T>(ArraySegment<T> segment)
        {
            return new Span<T>(segment.Array, segment.Offset, segment.Count);
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        /// <param name="array"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span(T[] array)
        {
            if (array == null)
            {
                this = default(Span<T>);
                return;
            }
            if (default(T) == null && array.GetType() != typeof(T[]))
            {
                System.ThrowHelper.ThrowArrayTypeMismatchException();
            }
            _length = array.Length;
            _pinnable = System.Runtime.CompilerServices.Unsafe.As<Pinnable<T>>((object)array);
            _byteOffset = SpanHelpers.PerTypeValues<T>.ArrayAdjustment;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Span<T> Create(T[] array, int start)
        {
            if (array == null)
            {
                if (start != 0)
                {
                    System.ThrowHelper.ThrowArgumentOutOfRangeException(System.ExceptionArgument.start);
                }
                return default(Span<T>);
            }
            if (default(T) == null && array.GetType() != typeof(T[]))
            {
                System.ThrowHelper.ThrowArrayTypeMismatchException();
            }
            if ((uint)start > (uint)array.Length)
            {
                System.ThrowHelper.ThrowArgumentOutOfRangeException(System.ExceptionArgument.start);
            }
            IntPtr byteOffset = SpanHelpers.PerTypeValues<T>.ArrayAdjustment.Add<T>(start);
            int length = array.Length - start;
            return new Span<T>(System.Runtime.CompilerServices.Unsafe.As<Pinnable<T>>((object)array), byteOffset, length);
        }

        /// <param name="array"></param>
        /// <param name="start"></param>
        /// <param name="length"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span(T[] array, int start, int length)
        {
            if (array == null)
            {
                if (start != 0 || length != 0)
                {
                    System.ThrowHelper.ThrowArgumentOutOfRangeException(System.ExceptionArgument.start);
                }
                this = default(Span<T>);
                return;
            }
            if (default(T) == null && array.GetType() != typeof(T[]))
            {
                System.ThrowHelper.ThrowArrayTypeMismatchException();
            }
            if ((uint)start > (uint)array.Length || (uint)length > (uint)(array.Length - start))
            {
                System.ThrowHelper.ThrowArgumentOutOfRangeException(System.ExceptionArgument.start);
            }
            _length = length;
            _pinnable = System.Runtime.CompilerServices.Unsafe.As<Pinnable<T>>((object)array);
            _byteOffset = SpanHelpers.PerTypeValues<T>.ArrayAdjustment.Add<T>(start);
        }

        /// <param name="pointer"></param>
        /// <param name="length"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [CLSCompliant(false)]
        public unsafe Span(void* pointer, int length)
        {
            if (SpanHelpers.IsReferenceOrContainsReferences<T>())
            {
                System.ThrowHelper.ThrowArgumentException_InvalidTypeWithPointersNotSupported(typeof(T));
            }
            if (length < 0)
            {
                System.ThrowHelper.ThrowArgumentOutOfRangeException(System.ExceptionArgument.start);
            }
            _length = length;
            _pinnable = null;
            _byteOffset = new IntPtr(pointer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal Span(Pinnable<T> pinnable, IntPtr byteOffset, int length)
        {
            _length = length;
            _pinnable = pinnable;
            _byteOffset = byteOffset;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public unsafe ref T GetPinnableReference()
        {
            if (_length != 0)
            {
                if (_pinnable == null)
                {
                    return ref System.Runtime.CompilerServices.Unsafe.AsRef<T>(_byteOffset.ToPointer());
                }
                return ref System.Runtime.CompilerServices.Unsafe.AddByteOffset<T>(ref _pinnable.Data, _byteOffset);
            }
            return ref System.Runtime.CompilerServices.Unsafe.AsRef<T>((void*)null);
        }

        public unsafe void Clear()
        {
            int length = _length;
            if (length == 0)
            {
                return;
            }
            UIntPtr byteLength = (UIntPtr)(ulong)((uint)length * System.Runtime.CompilerServices.Unsafe.SizeOf<T>());
            if ((System.Runtime.CompilerServices.Unsafe.SizeOf<T>() & (sizeof(IntPtr) - 1)) != 0)
            {
                if (_pinnable == null)
                {
                    byte* ptr = (byte*)_byteOffset.ToPointer();
                    SpanHelpers.ClearLessThanPointerSized(ptr, byteLength);
                }
                else
                {
                    SpanHelpers.ClearLessThanPointerSized(ref System.Runtime.CompilerServices.Unsafe.As<T, byte>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset<T>(ref _pinnable.Data, _byteOffset)), byteLength);
                }
            }
            else if (SpanHelpers.IsReferenceOrContainsReferences<T>())
            {
                UIntPtr pointerSizeLength = (UIntPtr)(ulong)(length * System.Runtime.CompilerServices.Unsafe.SizeOf<T>() / sizeof(IntPtr));
                SpanHelpers.ClearPointerSizedWithReferences(ref System.Runtime.CompilerServices.Unsafe.As<T, IntPtr>(ref DangerousGetPinnableReference()), pointerSizeLength);
            }
            else
            {
                SpanHelpers.ClearPointerSizedWithoutReferences(ref System.Runtime.CompilerServices.Unsafe.As<T, byte>(ref DangerousGetPinnableReference()), byteLength);
            }
        }

        /// <param name="value"></param>
        public unsafe void Fill(T value)
        {
            int length = _length;
            if (length == 0)
            {
                return;
            }
            if (System.Runtime.CompilerServices.Unsafe.SizeOf<T>() == 1)
            {
                byte b = System.Runtime.CompilerServices.Unsafe.As<T, byte>(ref value);
                if (_pinnable == null)
                {
                    System.Runtime.CompilerServices.Unsafe.InitBlockUnaligned(_byteOffset.ToPointer(), b, (uint)length);
                }
                else
                {
                    System.Runtime.CompilerServices.Unsafe.InitBlockUnaligned(ref System.Runtime.CompilerServices.Unsafe.As<T, byte>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset<T>(ref _pinnable.Data, _byteOffset)), b, (uint)length);
                }
                return;
            }
            ref T reference = ref DangerousGetPinnableReference();
            int i;
            for (i = 0; i < (length & -8); i += 8)
            {
                System.Runtime.CompilerServices.Unsafe.Add<T>(ref reference, i) = value;
                System.Runtime.CompilerServices.Unsafe.Add<T>(ref reference, i + 1) = value;
                System.Runtime.CompilerServices.Unsafe.Add<T>(ref reference, i + 2) = value;
                System.Runtime.CompilerServices.Unsafe.Add<T>(ref reference, i + 3) = value;
                System.Runtime.CompilerServices.Unsafe.Add<T>(ref reference, i + 4) = value;
                System.Runtime.CompilerServices.Unsafe.Add<T>(ref reference, i + 5) = value;
                System.Runtime.CompilerServices.Unsafe.Add<T>(ref reference, i + 6) = value;
                System.Runtime.CompilerServices.Unsafe.Add<T>(ref reference, i + 7) = value;
            }
            if (i < (length & -4))
            {
                System.Runtime.CompilerServices.Unsafe.Add<T>(ref reference, i) = value;
                System.Runtime.CompilerServices.Unsafe.Add<T>(ref reference, i + 1) = value;
                System.Runtime.CompilerServices.Unsafe.Add<T>(ref reference, i + 2) = value;
                System.Runtime.CompilerServices.Unsafe.Add<T>(ref reference, i + 3) = value;
                i += 4;
            }
            for (; i < length; i++)
            {
                System.Runtime.CompilerServices.Unsafe.Add<T>(ref reference, i) = value;
            }
        }

        /// <param name="destination"></param>
        public void CopyTo(Span<T> destination)
        {
            if (!TryCopyTo(destination))
            {
                System.ThrowHelper.ThrowArgumentException_DestinationTooShort();
            }
        }

        /// <param name="destination"></param>
        /// <returns></returns>
        public bool TryCopyTo(Span<T> destination)
        {
            int length = _length;
            int length2 = destination._length;
            if (length == 0)
            {
                return true;
            }
            if ((uint)length > (uint)length2)
            {
                return false;
            }
            SpanHelpers.CopyTo(src: ref DangerousGetPinnableReference(), dst: ref destination.DangerousGetPinnableReference(), dstLength: length2, srcLength: length);
            return true;
        }

        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(Span<T> left, Span<T> right)
        {
            if (left._length == right._length)
            {
                return System.Runtime.CompilerServices.Unsafe.AreSame<T>(ref left.DangerousGetPinnableReference(), ref right.DangerousGetPinnableReference());
            }
            return false;
        }

        public static implicit operator ReadOnlySpan<T>(Span<T> span)
        {
            return new ReadOnlySpan<T>(span._pinnable, span._byteOffset, span._length);
        }

        public unsafe override string ToString()
        {
            if (typeof(T) == typeof(char))
            {
                fixed (char* value = &System.Runtime.CompilerServices.Unsafe.As<T, char>(ref DangerousGetPinnableReference()))
                {
                    return new string(value, 0, _length);
                }
            }
            return $"System.Span<{typeof(T).Name}>[{_length}]";
        }

        /// <param name="start"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span<T> Slice(int start)
        {
            if ((uint)start > (uint)_length)
            {
                System.ThrowHelper.ThrowArgumentOutOfRangeException(System.ExceptionArgument.start);
            }
            IntPtr byteOffset = _byteOffset.Add<T>(start);
            int length = _length - start;
            return new Span<T>(_pinnable, byteOffset, length);
        }

        /// <param name="start"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span<T> Slice(int start, int length)
        {
            if ((uint)start > (uint)_length || (uint)length > (uint)(_length - start))
            {
                System.ThrowHelper.ThrowArgumentOutOfRangeException(System.ExceptionArgument.start);
            }
            IntPtr byteOffset = _byteOffset.Add<T>(start);
            return new Span<T>(_pinnable, byteOffset, length);
        }

        /// <returns></returns>
        public T[] ToArray()
        {
            if (_length == 0)
            {
                return SpanHelpers.PerTypeValues<T>.EmptyArray;
            }
            T[] array = new T[_length];
            CopyTo(array);
            return array;
        }

        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal unsafe ref T DangerousGetPinnableReference()
        {
            if (_pinnable == null)
            {
                return ref System.Runtime.CompilerServices.Unsafe.AsRef<T>(_byteOffset.ToPointer());
            }
            return ref System.Runtime.CompilerServices.Unsafe.AddByteOffset<T>(ref _pinnable.Data, _byteOffset);
        }
    }
}
