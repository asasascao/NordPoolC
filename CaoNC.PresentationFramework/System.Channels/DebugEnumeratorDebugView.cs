using System.Collections.Generic;
using System.Diagnostics;

namespace CaoNC.System.Channels
{
    internal interface IDebugEnumerable<T>
    {
        IEnumerator<T> GetEnumerator();
    }

    internal sealed class DebugEnumeratorDebugView<T>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		public T[] Items { get; }

		public DebugEnumeratorDebugView(IDebugEnumerable<T> enumerable)
		{
			List<T> list = new List<T>();
			foreach (T item in enumerable)
			{
				list.Add(item);
			}
			Items = list.ToArray();
		}
	}
}
