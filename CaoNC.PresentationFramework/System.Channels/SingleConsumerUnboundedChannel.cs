using CaoNC.System.Threading;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace CaoNC.System.Channels
{
	[DebuggerDisplay("Items = {ItemsCountForDebugger}, Closed = {ChannelIsClosedForDebugger}")]
	[DebuggerTypeProxy(typeof(DebugEnumeratorDebugView<>))]
	internal sealed class SingleConsumerUnboundedChannel<T> : Channel<T>, IDebugEnumerable<T>
	{
		[DebuggerDisplay("Items = {ItemsCountForDebugger}")]
		[DebuggerTypeProxy(typeof(DebugEnumeratorDebugView<>))]
		private sealed class UnboundedChannelReader : ChannelReader<T>, IDebugEnumerable<T>
		{
			internal readonly SingleConsumerUnboundedChannel<T> _parent;

			private readonly AsyncOperation<T> _readerSingleton;

			private readonly AsyncOperation<bool> _waiterSingleton;

			public override Task Completion => _parent._completion.Task;

			public override bool CanPeek => true;

			private int ItemsCountForDebugger => _parent._items.Count;

			internal UnboundedChannelReader(SingleConsumerUnboundedChannel<T> parent)
			{
				_parent = parent;
				_readerSingleton = new AsyncOperation<T>(parent._runContinuationsAsynchronously, default(CancellationToken), pooled: true);
				_waiterSingleton = new AsyncOperation<bool>(parent._runContinuationsAsynchronously, default(CancellationToken), pooled: true);
			}

			public override System.Threading.Tasks.ValueTask<T> ReadAsync(CancellationToken cancellationToken)
			{
				if (cancellationToken.IsCancellationRequested)
				{
					return new System.Threading.Tasks.ValueTask<T>(TaskEx.FromCanceled<T>(cancellationToken));
				}
				if (TryRead(out var item))
				{
					return new System.Threading.Tasks.ValueTask<T>(item);
				}
				SingleConsumerUnboundedChannel<T> parent = _parent;
				AsyncOperation<T> asyncOperation;
				AsyncOperation<T> asyncOperation2;
				lock (parent.SyncObj)
				{
					if (TryRead(out item))
					{
						return new System.Threading.Tasks.ValueTask<T>(item);
					}
					if (parent._doneWriting != null)
					{
						return ChannelUtilities.GetInvalidCompletionValueTask<T>(parent._doneWriting);
					}
					asyncOperation = parent._blockedReader;
					if (!cancellationToken.CanBeCanceled && _readerSingleton.TryOwnAndReset())
					{
						asyncOperation2 = _readerSingleton;
						if (asyncOperation2 == asyncOperation)
						{
							asyncOperation = null;
						}
					}
					else
					{
						asyncOperation2 = new AsyncOperation<T>(_parent._runContinuationsAsynchronously, cancellationToken);
					}
					parent._blockedReader = asyncOperation2;
				}
				asyncOperation?.TrySetCanceled();
				return asyncOperation2.ValueTaskOfT;
			}

			public override bool TryRead(out T item)
			{
				SingleConsumerUnboundedChannel<T> parent = _parent;
				if (parent._items.TryDequeue(out item))
				{
					if (parent._doneWriting != null && parent._items.IsEmpty)
					{
						ChannelUtilities.Complete(parent._completion, parent._doneWriting);
					}
					return true;
				}
				return false;
			}

			public override bool TryPeek(out T item)
			{
				return _parent._items.TryPeek(out item);
			}

			public override System.Threading.Tasks.ValueTask<bool> WaitToReadAsync(CancellationToken cancellationToken)
			{
				if (cancellationToken.IsCancellationRequested)
				{
					return new System.Threading.Tasks.ValueTask<bool>(TaskEx.FromCanceled<bool>(cancellationToken));
				}
				if (!_parent._items.IsEmpty)
				{
					return new System.Threading.Tasks.ValueTask<bool>(true);
				}
				SingleConsumerUnboundedChannel<T> parent = _parent;
				AsyncOperation<bool> asyncOperation = null;
				AsyncOperation<bool> asyncOperation2;
				lock (parent.SyncObj)
				{
					if (!parent._items.IsEmpty)
					{
						return new System.Threading.Tasks.ValueTask<bool>(true);
					}
					if (parent._doneWriting != null)
					{
						return (parent._doneWriting != ChannelUtilities.s_doneWritingSentinel) ? new System.Threading.Tasks.ValueTask<bool>(TaskEx.FromException<bool>(parent._doneWriting)) : default(System.Threading.Tasks.ValueTask<bool>);
					}
					asyncOperation = parent._waitingReader;
					if (!cancellationToken.CanBeCanceled && _waiterSingleton.TryOwnAndReset())
					{
						asyncOperation2 = _waiterSingleton;
						if (asyncOperation2 == asyncOperation)
						{
							asyncOperation = null;
						}
					}
					else
					{
						asyncOperation2 = new AsyncOperation<bool>(_parent._runContinuationsAsynchronously, cancellationToken);
					}
					parent._waitingReader = asyncOperation2;
				}
				asyncOperation?.TrySetCanceled();
				return asyncOperation2.ValueTaskOfT;
			}

			IEnumerator<T> IDebugEnumerable<T>.GetEnumerator()
			{
				return _parent._items.GetEnumerator();
			}
		}

		[DebuggerDisplay("Items = {ItemsCountForDebugger}")]
		[DebuggerTypeProxy(typeof(DebugEnumeratorDebugView<>))]
		private sealed class UnboundedChannelWriter : ChannelWriter<T>, IDebugEnumerable<T>
		{
			internal readonly SingleConsumerUnboundedChannel<T> _parent;

			private int ItemsCountForDebugger => _parent._items.Count;

			internal UnboundedChannelWriter(SingleConsumerUnboundedChannel<T> parent)
			{
				_parent = parent;
			}

			public override bool TryComplete(Exception error)
			{
				AsyncOperation<T> asyncOperation = null;
				AsyncOperation<bool> asyncOperation2 = null;
				bool flag = false;
				SingleConsumerUnboundedChannel<T> parent = _parent;
				lock (parent.SyncObj)
				{
					if (parent._doneWriting != null)
					{
						return false;
					}
					parent._doneWriting = error ?? ChannelUtilities.s_doneWritingSentinel;
					if (parent._items.IsEmpty)
					{
						flag = true;
						if (parent._blockedReader != null)
						{
							asyncOperation = parent._blockedReader;
							parent._blockedReader = null;
						}
						if (parent._waitingReader != null)
						{
							asyncOperation2 = parent._waitingReader;
							parent._waitingReader = null;
						}
					}
				}
				if (flag)
				{
					ChannelUtilities.Complete(parent._completion, error);
				}
				if (asyncOperation != null)
				{
					error = ChannelUtilities.CreateInvalidCompletionException(error);
					asyncOperation.TrySetException(error);
				}
				if (asyncOperation2 != null)
				{
					if (error != null)
					{
						asyncOperation2.TrySetException(error);
					}
					else
					{
						asyncOperation2.TrySetResult(item: false);
					}
				}
				return true;
			}

			public override bool TryWrite(T item)
			{
				SingleConsumerUnboundedChannel<T> parent = _parent;
				AsyncOperation<T> asyncOperation;
				do
				{
					asyncOperation = null;
					AsyncOperation<bool> asyncOperation2 = null;
					lock (parent.SyncObj)
					{
						if (parent._doneWriting != null)
						{
							return false;
						}
						asyncOperation = parent._blockedReader;
						if (asyncOperation != null)
						{
							parent._blockedReader = null;
						}
						else
						{
							parent._items.Enqueue(item);
							asyncOperation2 = parent._waitingReader;
							if (asyncOperation2 == null)
							{
								return true;
							}
							parent._waitingReader = null;
						}
					}
					if (asyncOperation2 != null)
					{
						asyncOperation2.TrySetResult(item: true);
						return true;
					}
				}
				while (!asyncOperation.TrySetResult(item));
				return true;
			}

			public override System.Threading.Tasks.ValueTask<bool> WaitToWriteAsync(CancellationToken cancellationToken)
			{
				Exception doneWriting = _parent._doneWriting;
				if (!cancellationToken.IsCancellationRequested)
				{
					if (doneWriting != null)
					{
						if (doneWriting == ChannelUtilities.s_doneWritingSentinel)
						{
							return default(System.Threading.Tasks.ValueTask<bool>);
						}
						return new System.Threading.Tasks.ValueTask<bool>(TaskEx.FromException<bool>(doneWriting));
					}
					return new System.Threading.Tasks.ValueTask<bool>(true);
				}
				return new System.Threading.Tasks.ValueTask<bool>(TaskEx.FromCanceled<bool>(cancellationToken));
			}

			public override System.Threading.Tasks.ValueTask WriteAsync(T item, CancellationToken cancellationToken)
			{
				if (!cancellationToken.IsCancellationRequested)
				{
					if (!TryWrite(item))
					{
						return new System.Threading.Tasks.ValueTask(TaskEx.FromException(ChannelUtilities.CreateInvalidCompletionException(_parent._doneWriting)));
					}
					return default(System.Threading.Tasks.ValueTask);
				}
				return new System.Threading.Tasks.ValueTask(TaskEx.FromCanceled(cancellationToken));
			}

			IEnumerator<T> IDebugEnumerable<T>.GetEnumerator()
			{
				return _parent._items.GetEnumerator();
			}
		}

		private readonly TaskCompletionSource _completion;

		private readonly CaoNC.System.Collections.SingleProducerSingleConsumerQueue<T> _items = 
			new CaoNC.System.Collections.SingleProducerSingleConsumerQueue<T>();

		private readonly bool _runContinuationsAsynchronously;

		private volatile Exception _doneWriting;

		private AsyncOperation<T> _blockedReader;

		private AsyncOperation<bool> _waitingReader;

		private object SyncObj => _items;

		private int ItemsCountForDebugger => _items.Count;

		private bool ChannelIsClosedForDebugger => _doneWriting != null;

		internal SingleConsumerUnboundedChannel(bool runContinuationsAsynchronously)
		{
			_runContinuationsAsynchronously = runContinuationsAsynchronously;
            _completion = new TaskCompletionSource(runContinuationsAsynchronously ?
                CaoNC.TaskCreationOpts.RunContinuationsAsynchronously : CaoNC.TaskCreationOpts.None);
            base.Reader = new UnboundedChannelReader(this);
			base.Writer = new UnboundedChannelWriter(this);
		}

		IEnumerator<T> IDebugEnumerable<T>.GetEnumerator()
		{
			return _items.GetEnumerator();
		}
	}
}
