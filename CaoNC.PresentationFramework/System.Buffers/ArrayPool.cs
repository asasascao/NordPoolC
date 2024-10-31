using System.Diagnostics;
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Diagnostics.Tracing;

namespace CaoNC.System.Buffers
{
    public abstract class ArrayPool<T>
    {
        private static ArrayPool<T> s_sharedInstance;

        /// <summary>Gets a shared <see cref="T:System.Buffers.ArrayPool`1"></see> instance.</summary>
        /// <returns>A shared <see cref="System.Buffers.ArrayPool`1"></see> instance.</returns>
        public static ArrayPool<T> Shared
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return Volatile.Read(ref s_sharedInstance) ?? EnsureSharedCreated();
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static ArrayPool<T> EnsureSharedCreated()
        {
            Interlocked.CompareExchange(ref s_sharedInstance, Create(), null);
            return s_sharedInstance;
        }

        /// <summary>Creates a new instance of the <see cref="T:System.Buffers.ArrayPool`1"></see> class.</summary>
        /// <returns>A new instance of the <see cref="System.Buffers.ArrayPool`1"></see> class.</returns>
        public static ArrayPool<T> Create()
        {
            return new DefaultArrayPool<T>();
        }

        /// <summary>Creates a new instance of the <see cref="T:System.Buffers.ArrayPool`1"></see> class using the specifed configuration.</summary>
        /// <param name="maxArrayLength">The maximum length of an array instance that may be stored in the pool.</param>
        /// <param name="maxArraysPerBucket">The maximum number of array instances that may be stored in each bucket in the pool. The pool groups arrays of similar lengths into buckets for faster access.</param>
        /// <returns>A new instance of the <see cref="System.Buffers.ArrayPool`1"></see> class with the specified configuration.</returns>
        public static ArrayPool<T> Create(int maxArrayLength, int maxArraysPerBucket)
        {
            return new DefaultArrayPool<T>(maxArrayLength, maxArraysPerBucket);
        }

        /// <summary>Retrieves a buffer that is at least the requested length.</summary>
        /// <param name="minimumLength">The minimum length of the array.</param>
        /// <returns>An array of type <see cref="T[]"></see> that is at least <paramref name="minimumLength">minimumLength</paramref> in length.</returns>
        public abstract T[] Rent(int minimumLength);

        /// <summary>Returns an array to the pool that was previously obtained using the <see cref="M:System.Buffers.ArrayPool`1.Rent(System.Int32)"></see> method on the same <see cref="T:System.Buffers.ArrayPool`1"></see> instance.</summary>
        /// <param name="array">A buffer to return to the pool that was previously obtained using the <see cref="M:System.Buffers.ArrayPool`1.Rent(System.Int32)"></see> method.</param>
        /// <param name="clearArray">Indicates whether the contents of the buffer should be cleared before reuse. If <paramref name="clearArray">clearArray</paramref> is set to true, and if the pool will store the buffer to enable subsequent reuse, the <see cref="M:System.Buffers.ArrayPool`1.Return(`0[],System.Boolean)"></see> method will clear the <paramref name="array">array</paramref> of its contents so that a subsequent caller using the <see cref="M:System.Buffers.ArrayPool`1.Rent(System.Int32)"></see> method will not see the content of the previous caller. If <paramref name="clearArray">clearArray</paramref> is set to false or if the pool will release the buffer, the array&amp;#39;s contents are left unchanged.</param>
        public abstract void Return(T[] array, bool clearArray = false);

        /// <summary>Initializes a new instance of the <see cref="T:System.Buffers.ArrayPool`1"></see> class.</summary>
        protected ArrayPool()
        {
        }
    }

    internal sealed class DefaultArrayPool<T> : ArrayPool<T>
    {
        private sealed class Bucket
        {
            internal readonly int _bufferLength;

            private readonly T[][] _buffers;

            private readonly int _poolId;

            private SpinLock _lock;

            private int _index;

            internal int Id => GetHashCode();

            internal Bucket(int bufferLength, int numberOfBuffers, int poolId)
            {
                _lock = new SpinLock(Debugger.IsAttached);
                _buffers = new T[numberOfBuffers][];
                _bufferLength = bufferLength;
                _poolId = poolId;
            }

            internal T[] Rent()
            {
                T[][] buffers = _buffers;
                T[] array = null;
                bool lockTaken = false;
                bool flag = false;
                try
                {
                    _lock.Enter(ref lockTaken);
                    if (_index < buffers.Length)
                    {
                        array = buffers[_index];
                        buffers[_index++] = null;
                        flag = array == null;
                    }
                }
                finally
                {
                    if (lockTaken)
                    {
                        _lock.Exit(useMemoryBarrier: false);
                    }
                }
                if (flag)
                {
                    array = new T[_bufferLength];
                    ArrayPoolEventSource log = ArrayPoolEventSource.Log;
                    if (log.IsEnabled())
                    {
                        log.BufferAllocated(array.GetHashCode(), _bufferLength, _poolId, Id, ArrayPoolEventSource.BufferAllocatedReason.Pooled);
                    }
                }
                return array;
            }

            internal void Return(T[] array)
            {
                if (array.Length != _bufferLength)
                {
                    throw new ArgumentException(SR.ArgumentException_BufferNotFromPool, "array");
                }
                bool lockTaken = false;
                try
                {
                    _lock.Enter(ref lockTaken);
                    if (_index != 0)
                    {
                        _buffers[--_index] = array;
                    }
                }
                finally
                {
                    if (lockTaken)
                    {
                        _lock.Exit(useMemoryBarrier: false);
                    }
                }
            }
        }

        private const int DefaultMaxArrayLength = 1048576;

        private const int DefaultMaxNumberOfArraysPerBucket = 50;

        private static T[] s_emptyArray;

        private readonly Bucket[] _buckets;

        private int Id => GetHashCode();

        internal DefaultArrayPool()
            : this(1048576, 50)
        {
        }

        internal DefaultArrayPool(int maxArrayLength, int maxArraysPerBucket)
        {
            if (maxArrayLength <= 0)
            {
                throw new ArgumentOutOfRangeException("maxArrayLength");
            }
            if (maxArraysPerBucket <= 0)
            {
                throw new ArgumentOutOfRangeException("maxArraysPerBucket");
            }
            if (maxArrayLength > 1073741824)
            {
                maxArrayLength = 1073741824;
            }
            else if (maxArrayLength < 16)
            {
                maxArrayLength = 16;
            }
            int id = Id;
            int num = Utilities.SelectBucketIndex(maxArrayLength);
            Bucket[] array = new Bucket[num + 1];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = new Bucket(Utilities.GetMaxSizeForBucket(i), maxArraysPerBucket, id);
            }
            _buckets = array;
        }

        public override T[] Rent(int minimumLength)
        {
            if (minimumLength < 0)
            {
                throw new ArgumentOutOfRangeException("minimumLength");
            }
            if (minimumLength == 0)
            {
                return s_emptyArray ?? (s_emptyArray = new T[0]);
            }
            ArrayPoolEventSource log = ArrayPoolEventSource.Log;
            T[] array = null;
            int num = Utilities.SelectBucketIndex(minimumLength);
            if (num < _buckets.Length)
            {
                int num2 = num;
                do
                {
                    array = _buckets[num2].Rent();
                    if (array != null)
                    {
                        if (log.IsEnabled())
                        {
                            log.BufferRented(array.GetHashCode(), array.Length, Id, _buckets[num2].Id);
                        }
                        return array;
                    }
                }
                while (++num2 < _buckets.Length && num2 != num + 2);
                array = new T[_buckets[num]._bufferLength];
            }
            else
            {
                array = new T[minimumLength];
            }
            if (log.IsEnabled())
            {
                int hashCode = array.GetHashCode();
                int bucketId = -1;
                log.BufferRented(hashCode, array.Length, Id, bucketId);
                log.BufferAllocated(hashCode, array.Length, Id, bucketId, (num >= _buckets.Length) ? ArrayPoolEventSource.BufferAllocatedReason.OverMaximumSize : ArrayPoolEventSource.BufferAllocatedReason.PoolExhausted);
            }
            return array;
        }

        public override void Return(T[] array, bool clearArray = false)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }
            if (array.Length == 0)
            {
                return;
            }
            int num = Utilities.SelectBucketIndex(array.Length);
            if (num < _buckets.Length)
            {
                if (clearArray)
                {
                    Array.Clear(array, 0, array.Length);
                }
                _buckets[num].Return(array);
            }
            ArrayPoolEventSource log = ArrayPoolEventSource.Log;
            if (log.IsEnabled())
            {
                log.BufferReturned(array.GetHashCode(), array.Length, Id);
            }
        }
    }

}
