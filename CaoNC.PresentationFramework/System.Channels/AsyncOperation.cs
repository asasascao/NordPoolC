using CaoNC.System.Threading;
using CaoNC.System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using TaskCreationOptions = System.Threading.Tasks.TaskCreationOptions;

namespace CaoNC.System.Channels
{
    internal abstract class AsyncOperation
    {
        protected static readonly Action<object> s_availableSentinel = AvailableSentinel;

        protected static readonly Action<object> s_completedSentinel = CompletedSentinel;

        private static void AvailableSentinel(object s)
        {
        }

        private static void CompletedSentinel(object s)
        {
        }

        protected static void ThrowIncompleteOperationException()
        {
            throw new InvalidOperationException(SR.InvalidOperation_IncompleteAsyncOperation);
        }

        protected static void ThrowMultipleContinuations()
        {
            throw new InvalidOperationException(SR.InvalidOperation_MultipleContinuations);
        }

        protected static void ThrowIncorrectCurrentIdException()
        {
            throw new InvalidOperationException(SR.InvalidOperation_IncorrectToken);
        }
    }

    internal class AsyncOperation<TResult> : AsyncOperation, IValueTaskSource, IValueTaskSource<TResult>
    {
        private readonly CancellationTokenRegistration _registration;

        private readonly bool _pooled;

        private readonly bool _runContinuationsAsynchronously;

        private volatile int _completionReserved;

        private TResult _result;

        private ExceptionDispatchInfo _error;

        private Action<object> _continuation;

        private object _continuationState;

        private object _schedulingContext;

        private ExecutionContext _executionContext;

        private short _currentId;

        public AsyncOperation<TResult> Next { get; set; }

        public CancellationToken CancellationToken { get; }

        public System.Threading.Tasks.ValueTask ValueTask => new System.Threading.Tasks.ValueTask((IValueTaskSource)(object)this, _currentId);

        public System.Threading.Tasks.ValueTask<TResult> ValueTaskOfT => new System.Threading.Tasks.ValueTask<TResult>((IValueTaskSource<TResult>)this, _currentId);

        internal bool IsCompleted => (object)_continuation == AsyncOperation.s_completedSentinel;

        public AsyncOperation(bool runContinuationsAsynchronously, CancellationToken cancellationToken = default(CancellationToken), bool pooled = false)
        {
            _continuation = (pooled ? AsyncOperation.s_availableSentinel : null);
            _pooled = pooled;
            _runContinuationsAsynchronously = runContinuationsAsynchronously;
            if (cancellationToken.CanBeCanceled)
            {
                CancellationToken = cancellationToken;
                _registration = UnsafeRegister(cancellationToken, delegate (object s)
                {
                    AsyncOperation<TResult> asyncOperation = (AsyncOperation<TResult>)s;
                    asyncOperation.TrySetCanceled(asyncOperation.CancellationToken);
                }, this);
            }
        }

        public ValueTaskSourceStatus GetStatus(short token)
        {
            if (_currentId != token)
            {
                AsyncOperation.ThrowIncorrectCurrentIdException();
            }
            if (IsCompleted)
            {
                if (_error != null)
                {
                    if (_error.SourceException is OperationCanceledException)
                    {
                        return (ValueTaskSourceStatus)3;
                    }
                    return (ValueTaskSourceStatus)2;
                }
                return (ValueTaskSourceStatus)1;
            }
            return (ValueTaskSourceStatus)0;
        }

        public TResult GetResult(short token)
        {
            if (_currentId != token)
            {
                AsyncOperation.ThrowIncorrectCurrentIdException();
            }
            if (!IsCompleted)
            {
                AsyncOperation.ThrowIncompleteOperationException();
            }
            ExceptionDispatchInfo error = _error;
            TResult result = _result;
            _currentId++;
            if (_pooled)
            {
                Volatile.Write(ref _continuation, AsyncOperation.s_availableSentinel);
            }
            error?.Throw();
            return result;
        }

        void IValueTaskSource.GetResult(short token)
        {
            if (_currentId != token)
            {
                AsyncOperation.ThrowIncorrectCurrentIdException();
            }
            if (!IsCompleted)
            {
                AsyncOperation.ThrowIncompleteOperationException();
            }
            ExceptionDispatchInfo error = _error;
            _currentId++;
            if (_pooled)
            {
                Volatile.Write(ref _continuation, AsyncOperation.s_availableSentinel);
            }
            error?.Throw();
        }

        public bool TryOwnAndReset()
        {
            if ((object)Interlocked.CompareExchange(ref _continuation, null, AsyncOperation.s_availableSentinel) == AsyncOperation.s_availableSentinel)
            {
                _continuationState = null;
                _result = default(TResult);
                _error = null;
                _schedulingContext = null;
                _executionContext = null;
                return true;
            }
            return false;
        }

        public void OnCompleted(Action<object> continuation, object state, short token, ValueTaskSourceOnCompletedFlags flags)
        {
            //IL_0022: Unknown result type (might be due to invalid IL or missing references)
            //IL_0025: Unknown result type (might be due to invalid IL or missing references)
            //IL_0037: Unknown result type (might be due to invalid IL or missing references)
            //IL_003a: Unknown result type (might be due to invalid IL or missing references)
            if (_currentId != token)
            {
                AsyncOperation.ThrowIncorrectCurrentIdException();
            }
            if (_continuationState != null)
            {
                AsyncOperation.ThrowMultipleContinuations();
            }
            _continuationState = state;
            if ((((int)flags) & 2) != 0)
            {
                _executionContext = ExecutionContext.Capture();
            }
            SynchronizationContext synchronizationContext = null;
            TaskScheduler taskScheduler = null;
            if ((((int)flags) & 1) != 0)
            {
                synchronizationContext = SynchronizationContext.Current;
                if (synchronizationContext != null && synchronizationContext.GetType() != typeof(SynchronizationContext))
                {
                    _schedulingContext = synchronizationContext;
                }
                else
                {
                    synchronizationContext = null;
                    taskScheduler = TaskScheduler.Current;
                    if (taskScheduler != TaskScheduler.Default)
                    {
                        _schedulingContext = taskScheduler;
                    }
                }
            }
            Action<object> action = Interlocked.CompareExchange(ref _continuation, continuation, null);
            if (action == null)
            {
                return;
            }
            if ((object)action != AsyncOperation.s_completedSentinel)
            {
                AsyncOperation.ThrowMultipleContinuations();
            }
            if (_schedulingContext == null)
            {
                if (_executionContext == null)
                {
                    UnsafeQueueUserWorkItem(continuation, state);
                }
                else
                {
                    QueueUserWorkItem(continuation, state);
                }
            }
            else if (synchronizationContext != null)
            {
                synchronizationContext.Post(delegate (object s)
                {
                    KeyValuePair<Action<object>, object> keyValuePair = (KeyValuePair<Action<object>, object>)s;
                    keyValuePair.Key(keyValuePair.Value);
                }, new KeyValuePair<Action<object>, object>(continuation, state));
            }
            else
            {
                Task.Factory.StartNew(continuation, state, CancellationToken.None, 
                    TaskCreationOptions.DenyChildAttach, taskScheduler);
            }
        }

        public bool UnregisterCancellation()
        {
            if (CancellationToken.CanBeCanceled)
            {
                _registration.Dispose();
                return _completionReserved == 0;
            }
            return true;
        }

        public bool TrySetResult(TResult item)
        {
            UnregisterCancellation();
            if (TryReserveCompletionIfCancelable())
            {
                _result = item;
                SignalCompletion();
                return true;
            }
            return false;
        }

        public bool TrySetException(Exception exception)
        {
            UnregisterCancellation();
            if (TryReserveCompletionIfCancelable())
            {
                _error = ExceptionDispatchInfo.Capture(exception);
                SignalCompletion();
                return true;
            }
            return false;
        }

        public bool TrySetCanceled(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (TryReserveCompletionIfCancelable())
            {
                _error = ExceptionDispatchInfo.Capture(new OperationCanceledException(cancellationToken));
                SignalCompletion();
                return true;
            }
            return false;
        }

        private bool TryReserveCompletionIfCancelable()
        {
            if (CancellationToken.CanBeCanceled)
            {
                return Interlocked.CompareExchange(ref _completionReserved, 1, 0) == 0;
            }
            return true;
        }

        private void SignalCompletion()
        {
            if (_continuation == null && Interlocked.CompareExchange(ref _continuation, AsyncOperation.s_completedSentinel, null) == null)
            {
                return;
            }
            if (_schedulingContext == null)
            {
                if (_runContinuationsAsynchronously)
                {
                    UnsafeQueueSetCompletionAndInvokeContinuation();
                    return;
                }
            }
            else if (_schedulingContext is SynchronizationContext synchronizationContext)
            {
                if (_runContinuationsAsynchronously || synchronizationContext != SynchronizationContext.Current)
                {
                    synchronizationContext.Post(delegate (object s)
                    {
                        ((AsyncOperation<TResult>)s).SetCompletionAndInvokeContinuation();
                    }, this);
                    return;
                }
            }
            else
            {
                TaskScheduler taskScheduler = (TaskScheduler)_schedulingContext;
                if (_runContinuationsAsynchronously || taskScheduler != TaskScheduler.Current)
                {
                    Task.Factory.StartNew(delegate (object s)
                    {
                        ((AsyncOperation<TResult>)s).SetCompletionAndInvokeContinuation();
                    }, this, CancellationToken.None, TaskCreationOptions.DenyChildAttach, taskScheduler);
                    return;
                }
            }
            SetCompletionAndInvokeContinuation();
        }

        private void SetCompletionAndInvokeContinuation()
        {
            if (_executionContext == null)
            {
                Action<object> continuation = _continuation;
                _continuation = AsyncOperation.s_completedSentinel;
                continuation(_continuationState);
                return;
            }
            ExecutionContext.Run(_executionContext, delegate (object s)
            {
                AsyncOperation<TResult> asyncOperation = (AsyncOperation<TResult>)s;
                Action<object> continuation2 = asyncOperation._continuation;
                asyncOperation._continuation = AsyncOperation.s_completedSentinel;
                continuation2(asyncOperation._continuationState);
            }, this);
        }

        private void UnsafeQueueSetCompletionAndInvokeContinuation()
        {
            ThreadPool.UnsafeQueueUserWorkItem(delegate (object s)
            {
                ((AsyncOperation<TResult>)s).SetCompletionAndInvokeContinuation();
            }, this);
        }

        private static void UnsafeQueueUserWorkItem(Action<object> action, object state)
        {
            QueueUserWorkItem(action, state);
        }

        private static void QueueUserWorkItem(Action<object> action, object state)
        {
            Task.Factory.StartNew(action, state, CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);
        }

        private static CancellationTokenRegistration UnsafeRegister(CancellationToken cancellationToken, Action<object> action, object state)
        {
            return cancellationToken.Register(action, state);
        }
    }

    internal sealed class VoidAsyncOperationWithData<TData> : AsyncOperation<VoidResult>
    {
        public TData Item { get; set; }

        public VoidAsyncOperationWithData(bool runContinuationsAsynchronously, CancellationToken cancellationToken = default(CancellationToken), bool pooled = false)
            : base(runContinuationsAsynchronously, cancellationToken, pooled)
        {
        }
    }
}
