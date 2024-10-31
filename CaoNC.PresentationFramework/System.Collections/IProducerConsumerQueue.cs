using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace CaoNC.System.Collections
{
    internal interface IProducerConsumerQueue<T> : IEnumerable<T>, IEnumerable
    {
        bool IsEmpty { get; }

        int Count { get; }

        void Enqueue(T item);

        bool TryDequeue(out T result);

        int GetCountSafe(object syncObj);
    }
}
