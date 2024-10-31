using CaoNC.System.Collections;
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
	internal sealed class UnboundedChannel<T> : Channel<T>, IDebugEnumerable<T>
	{
		[DebuggerDisplay("Items = {Count}")]
		[DebuggerTypeProxy(typeof(DebugEnumeratorDebugView<>))]
		private sealed class UnboundedChannelReader : ChannelReader<T>, IDebugEnumerable<T>
		{
			internal readonly UnboundedChannel<T> _parent;

			private readonly AsyncOperation<T> _readerSingleton;

			private readonly AsyncOperation<bool> _waiterSingleton;

			public override Task Completion => _parent._completion.Task;

			public override bool CanCount => true;

			public override bool CanPeek => true;

			public override int Count => _parent._items.Count;

			internal UnboundedChannelReader(UnboundedChannel<T> parent)
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
				UnboundedChannel<T> parent = _parent;
				if (parent._items.TryDequeue(out var result))
				{
					CompleteIfDone(parent);
					return new System.Threading.Tasks.ValueTask<T>(result);
				}
				lock (parent.SyncObj)
				{
					if (parent._items.TryDequeue(out result))
					{
						CompleteIfDone(parent);
						return new System.Threading.Tasks.ValueTask<T>(result);
					}
					if (parent._doneWriting != null)
					{
						return ChannelUtilities.GetInvalidCompletionValueTask<T>(parent._doneWriting);
					}
					if (!cancellationToken.CanBeCanceled)
					{
						AsyncOperation<T> readerSingleton = _readerSingleton;
						if (readerSingleton.TryOwnAndReset())
						{
							parent._blockedReaders.EnqueueTail(readerSingleton);
							return readerSingleton.ValueTaskOfT;
						}
					}
					AsyncOperation<T> asyncOperation = new AsyncOperation<T>(parent._runContinuationsAsynchronously, cancellationToken);
					parent._blockedReaders.EnqueueTail(asyncOperation);
					return asyncOperation.ValueTaskOfT;
				}
			}

			public override bool TryRead(out T item)
			{
				UnboundedChannel<T> parent = _parent;
				if (parent._items.TryDequeue(out item))
				{
					CompleteIfDone(parent);
					return true;
				}
				item = default(T);
				return false;
			}

			public override bool TryPeek(out T item)
			{
				return _parent._items.TryPeek(out item);
			}

			private static void CompleteIfDone(UnboundedChannel<T> parent)
			{
				if (parent._doneWriting != null && parent._items.IsEmpty)
				{
					ChannelUtilities.Complete(parent._completion, parent._doneWriting);
				}
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
				UnboundedChannel<T> parent = _parent;
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
					if (!cancellationToken.CanBeCanceled)
					{
						AsyncOperation<bool> waiterSingleton = _waiterSingleton;
						if (waiterSingleton.TryOwnAndReset())
						{
							ChannelUtilities.QueueWaiter(ref parent._waitingReadersTail, waiterSingleton);
							return waiterSingleton.ValueTaskOfT;
						}
					}
					AsyncOperation<bool> asyncOperation = new AsyncOperation<bool>(parent._runContinuationsAsynchronously, cancellationToken);
					ChannelUtilities.QueueWaiter(ref parent._waitingReadersTail, asyncOperation);
					return asyncOperation.ValueTaskOfT;
				}
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
			internal readonly UnboundedChannel<T> _parent;

			private int ItemsCountForDebugger => _parent._items.Count;

			internal UnboundedChannelWriter(UnboundedChannel<T> parent)
			{
				_parent = parent;
			}

			public override bool TryComplete(Exception error)
			{
				UnboundedChannel<T> parent = _parent;
				bool isEmpty;
				lock (parent.SyncObj)
				{
					if (parent._doneWriting != null)
					{
						return false;
					}
					parent._doneWriting = error ?? ChannelUtilities.s_doneWritingSentinel;
					isEmpty = parent._items.IsEmpty;
				}
				if (isEmpty)
				{
					ChannelUtilities.Complete(parent._completion, error);
				}
				ChannelUtilities.FailOperations<AsyncOperation<T>, T>(parent._blockedReaders, ChannelUtilities.CreateInvalidCompletionException(error));
				ChannelUtilities.WakeUpWaiters(ref parent._waitingReadersTail, result: false, error);
				return true;
			}

			public override bool TryWrite(T item)
			{
				UnboundedChannel<T> parent = _parent;
				AsyncOperation<bool> listTail;
				while (true)
				{
					AsyncOperation<T> asyncOperation = null;
					listTail = null;
					lock (parent.SyncObj)
					{
						if (parent._doneWriting != null)
						{
							return false;
						}
						if (parent._blockedReaders.IsEmpty)
						{
							parent._items.Enqueue(item);
							listTail = parent._waitingReadersTail;
							if (listTail == null)
							{
								return true;
							}
							parent._waitingReadersTail = null;
						}
						else
						{
							asyncOperation = parent._blockedReaders.DequeueHead();
						}
					}
					if (asyncOperation == null)
					{
						break;
					}
					if (asyncOperation.TrySetResult(item))
					{
						return true;
					}
				}
				ChannelUtilities.WakeUpWaiters(ref listTail, result: true);
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

		private readonly ConcurrentQueue<T> _items = new ConcurrentQueue<T>();

		private readonly Deque<AsyncOperation<T>> _blockedReaders = new Deque<AsyncOperation<T>>();

		private readonly bool _runContinuationsAsynchronously;

		private AsyncOperation<bool> _waitingReadersTail;

		private Exception _doneWriting;

		private object SyncObj => _items;

		private int ItemsCountForDebugger => _items.Count;

		private bool ChannelIsClosedForDebugger => _doneWriting != null;

        internal UnboundedChannel(bool runContinuationsAsynchronously)
		{
			_runContinuationsAsynchronously = runContinuationsAsynchronously;
			_completion = new TaskCompletionSource(runContinuationsAsynchronously ?
                CaoNC.TaskCreationOpts.RunContinuationsAsynchronously : CaoNC.TaskCreationOpts.None);
			base.Reader = new UnboundedChannelReader(this);
			base.Writer = new UnboundedChannelWriter(this);
		}

		[Conditional("DEBUG")]
		private void AssertInvariants()
		{
			if (!_items.IsEmpty)
			{
				_ = _runContinuationsAsynchronously;
			}
			if (!_blockedReaders.IsEmpty || _waitingReadersTail != null)
			{
				_ = _runContinuationsAsynchronously;
			}
			_ = _completion.Task.IsCompleted;
		}

		IEnumerator<T> IDebugEnumerable<T>.GetEnumerator()
		{
			return _items.GetEnumerator();
		}
	}
}
