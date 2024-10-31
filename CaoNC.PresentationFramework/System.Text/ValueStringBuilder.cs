﻿using CaoNC.System.Memory;
using CaoNC.System;
using System.Runtime.CompilerServices;
using System;
using CaoNC.System.Buffers;

namespace CaoNC.System.Text
{
    internal ref struct ValueStringBuilder
    {
        private char[] _arrayToReturnToPool;

        private Span<char> _chars;

        private int _pos;

        public int Length
        {
            get
            {
                return _pos;
            }
            set
            {
                _pos = value;
            }
        }

        public int Capacity => _chars.Length;

        public ref char this[int index] => ref _chars[index];

        /// <summary>Returns the underlying storage of the builder.</summary>
        public Span<char> RawChars => _chars;

        public ValueStringBuilder(Span<char> initialBuffer)
        {
            _arrayToReturnToPool = null;
            _chars = initialBuffer;
            _pos = 0;
        }

        public ValueStringBuilder(int initialCapacity)
        {
            _arrayToReturnToPool = ArrayPool<char>.Shared.Rent(initialCapacity);
            _chars = _arrayToReturnToPool;
            _pos = 0;
        }

        public void EnsureCapacity(int capacity)
        {
            if ((uint)capacity > (uint)_chars.Length)
            {
                Grow(capacity - _pos);
            }
        }

        /// <summary>
        /// Get a pinnable reference to the builder.
        /// Does not ensure there is a null char after <see cref="P:System.Text.ValueStringBuilder.Length" />
        /// This overload is pattern matched in the C# 7.3+ compiler so you can omit
        /// the explicit method call, and write eg "fixed (char* c = builder)"
        /// </summary>
        public ref char GetPinnableReference()
        {
            return ref MemoryMarshal.GetReference(_chars);
        }

        /// <summary>
        /// Get a pinnable reference to the builder.
        /// </summary>
        /// <param name="terminate">Ensures that the builder has a null char after <see cref="P:System.Text.ValueStringBuilder.Length" /></param>
        public ref char GetPinnableReference(bool terminate)
        {
            if (terminate)
            {
                EnsureCapacity(Length + 1);
                _chars[Length] = '\0';
            }
            return ref MemoryMarshal.GetReference(_chars);
        }

        public override string ToString()
        {
            string result = _chars.Slice(0, _pos).ToString();
            Dispose();
            return result;
        }

        /// <summary>
        /// Returns a span around the contents of the builder.
        /// </summary>
        /// <param name="terminate">Ensures that the builder has a null char after <see cref="P:System.Text.ValueStringBuilder.Length" /></param>
        public ReadOnlySpan<char> AsSpan(bool terminate)
        {
            if (terminate)
            {
                EnsureCapacity(Length + 1);
                _chars[Length] = '\0';
            }
            return _chars.Slice(0, _pos);
        }

        public ReadOnlySpan<char> AsSpan()
        {
            return _chars.Slice(0, _pos);
        }

        public ReadOnlySpan<char> AsSpan(int start)
        {
            return _chars.Slice(start, _pos - start);
        }

        public ReadOnlySpan<char> AsSpan(int start, int length)
        {
            return _chars.Slice(start, length);
        }

        public bool TryCopyTo(Span<char> destination, out int charsWritten)
        {
            if (_chars.Slice(0, _pos).TryCopyTo(destination))
            {
                charsWritten = _pos;
                Dispose();
                return true;
            }
            charsWritten = 0;
            Dispose();
            return false;
        }

        public void Insert(int index, char value, int count)
        {
            if (_pos > _chars.Length - count)
            {
                Grow(count);
            }
            int length = _pos - index;
            _chars.Slice(index, length).CopyTo(_chars.Slice(index + count));
            _chars.Slice(index, count).Fill(value);
            _pos += count;
        }

        public void Insert(int index, string s)
        {
            if (s != null)
            {
                int length = s.Length;
                if (_pos > _chars.Length - length)
                {
                    Grow(length);
                }
                int length2 = _pos - index;
                _chars.Slice(index, length2).CopyTo(_chars.Slice(index + length));
                s.AsSpan().CopyTo(_chars.Slice(index));
                _pos += length;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Append(char c)
        {
            int pos = _pos;
            Span<char> chars = _chars;
            if ((uint)pos < (uint)chars.Length)
            {
                chars[pos] = c;
                _pos = pos + 1;
            }
            else
            {
                GrowAndAppend(c);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Append(string s)
        {
            if (s != null)
            {
                int pos = _pos;
                if (s.Length == 1 && (uint)pos < (uint)_chars.Length)
                {
                    _chars[pos] = s[0];
                    _pos = pos + 1;
                }
                else
                {
                    AppendSlow(s);
                }
            }
        }

        private void AppendSlow(string s)
        {
            int pos = _pos;
            if (pos > _chars.Length - s.Length)
            {
                Grow(s.Length);
            }
            s.AsSpan().CopyTo(_chars.Slice(pos));
            _pos += s.Length;
        }

        public void Append(char c, int count)
        {
            if (_pos > _chars.Length - count)
            {
                Grow(count);
            }
            Span<char> span = _chars.Slice(_pos, count);
            for (int i = 0; i < span.Length; i++)
            {
                span[i] = c;
            }
            _pos += count;
        }

        public unsafe void Append(char* value, int length)
        {
            int pos = _pos;
            if (pos > _chars.Length - length)
            {
                Grow(length);
            }
            Span<char> span = _chars.Slice(_pos, length);
            for (int i = 0; i < span.Length; i++)
            {
                ref char reference = ref span[i];
                char* num = value;
                value = num + 1;
                reference = *num;
            }
            _pos += length;
        }

        public void Append(ReadOnlySpan<char> value)
        {
            int pos = _pos;
            if (pos > _chars.Length - value.Length)
            {
                Grow(value.Length);
            }
            value.CopyTo(_chars.Slice(_pos));
            _pos += value.Length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span<char> AppendSpan(int length)
        {
            int pos = _pos;
            if (pos > _chars.Length - length)
            {
                Grow(length);
            }
            _pos = pos + length;
            return _chars.Slice(pos, length);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void GrowAndAppend(char c)
        {
            Grow(1);
            Append(c);
        }

        /// <summary>
        /// Resize the internal buffer either by doubling current buffer size or
        /// by adding <paramref name="additionalCapacityBeyondPos" /> to
        /// <see cref="F:System.Text.ValueStringBuilder._pos" /> whichever is greater.
        /// </summary>
        /// <param name="additionalCapacityBeyondPos">
        /// Number of chars requested beyond current position.
        /// </param>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private void Grow(int additionalCapacityBeyondPos)
        {
            int num = (int)Math.Max((uint)(_pos + additionalCapacityBeyondPos), Math.Min((uint)(_chars.Length * 2), 2147483591u));
            char[] array = ArrayPool<char>.Shared.Rent(num);
            _chars.Slice(0, _pos).CopyTo(array);
            char[] arrayToReturnToPool = _arrayToReturnToPool;
            _chars = (_arrayToReturnToPool = array);
            if (arrayToReturnToPool != null)
            {
                ArrayPool<char>.Shared.Return(arrayToReturnToPool, false);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            char[] arrayToReturnToPool = _arrayToReturnToPool;
            this = default(ValueStringBuilder);
            if (arrayToReturnToPool != null)
            {
                ArrayPool<char>.Shared.Return(arrayToReturnToPool, false);
            }
        }
    }
}
