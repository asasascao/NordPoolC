using System.Threading;

namespace CaoNC.System.Threading
{
    internal class SparselyPopulatedArrayFragment<T> where T : class
    {
        internal readonly T[] m_elements;

        internal volatile int m_freeCount;

        internal volatile SparselyPopulatedArrayFragment<T> m_next;

        internal volatile SparselyPopulatedArrayFragment<T> m_prev;

        internal T this[int index] => Volatile.Read(ref m_elements[index]);

        internal int Length => m_elements.Length;

        internal SparselyPopulatedArrayFragment<T> Prev => m_prev;

        internal SparselyPopulatedArrayFragment(int size)
            : this(size, (SparselyPopulatedArrayFragment<T>)null)
        {
        }

        internal SparselyPopulatedArrayFragment(int size, SparselyPopulatedArrayFragment<T> prev)
        {
            m_elements = new T[size];
            m_freeCount = size;
            m_prev = prev;
        }

        internal T SafeAtomicRemove(int index, T expectedElement)
        {
            T val = Interlocked.CompareExchange(ref m_elements[index], null, expectedElement);
            if (val != null)
            {
                m_freeCount++;
            }
            return val;
        }
    }

    internal struct SparselyPopulatedArrayAddInfo<T> where T : class
    {
        private SparselyPopulatedArrayFragment<T> m_source;

        private int m_index;

        internal SparselyPopulatedArrayFragment<T> Source => m_source;

        internal int Index => m_index;

        internal SparselyPopulatedArrayAddInfo(SparselyPopulatedArrayFragment<T> source, int index)
        {
            m_source = source;
            m_index = index;
        }
    }
}
