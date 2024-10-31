using CaoNC.System.Threading.Tasks;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading.Tasks;

namespace CaoNC.System.Runtime.CompilerServices
{
    [StructLayout(LayoutKind.Auto)]
    public struct AsyncValueTaskMethodBuilder
    {
        private AsyncTaskMethodBuilder _methodBuilder;

        private bool _haveResult;

        private bool _useBuilder;

        public ValueTask Task
        {
            get
            {
                if (_haveResult)
                {
                    return default(ValueTask);
                }
                _useBuilder = true;
                return new ValueTask(_methodBuilder.Task);
            }
        }

        public static AsyncValueTaskMethodBuilder Create()
        {
            return default(AsyncValueTaskMethodBuilder);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Start<TStateMachine>(ref TStateMachine stateMachine) where TStateMachine : IAsyncStateMachine
        {
            _methodBuilder.Start(ref stateMachine);
        }

        public void SetStateMachine(IAsyncStateMachine stateMachine)
        {
            _methodBuilder.SetStateMachine(stateMachine);
        }

        public void SetResult()
        {
            if (_useBuilder)
            {
                _methodBuilder.SetResult();
            }
            else
            {
                _haveResult = true;
            }
        }

        public void SetException(Exception exception)
        {
            _methodBuilder.SetException(exception);
        }

        public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine) where TAwaiter : INotifyCompletion where TStateMachine : IAsyncStateMachine
        {
            _useBuilder = true;
            _methodBuilder.AwaitOnCompleted(ref awaiter, ref stateMachine);
        }

        [SecuritySafeCritical]
        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine) where TAwaiter : ICriticalNotifyCompletion where TStateMachine : IAsyncStateMachine
        {
            _useBuilder = true;
            _methodBuilder.AwaitUnsafeOnCompleted(ref awaiter, ref stateMachine);
        }
    }

    [StructLayout(LayoutKind.Auto)]
    public struct AsyncValueTaskMethodBuilder<TResult>
    {
        private AsyncTaskMethodBuilder<TResult> _methodBuilder;

        private TResult _result;

        private bool _haveResult;

        private bool _useBuilder;

        /// <returns></returns>
        public ValueTask<TResult> Task
        {
            get
            {
                if (_haveResult)
                {
                    return new ValueTask<TResult>(_result);
                }
                _useBuilder = true;
                return new ValueTask<TResult>(_methodBuilder.Task);
            }
        }

        /// <returns></returns>
        public static AsyncValueTaskMethodBuilder<TResult> Create()
        {
            return default(AsyncValueTaskMethodBuilder<TResult>);
        }

        /// <param name="stateMachine"></param>
        /// <typeparam name="TStateMachine"></typeparam>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Start<TStateMachine>(ref TStateMachine stateMachine) where TStateMachine : IAsyncStateMachine
        {
            _methodBuilder.Start(ref stateMachine);
        }

        /// <param name="stateMachine"></param>
        public void SetStateMachine(IAsyncStateMachine stateMachine)
        {
            _methodBuilder.SetStateMachine(stateMachine);
        }

        /// <param name="result"></param>
        public void SetResult(TResult result)
        {
            if (_useBuilder)
            {
                _methodBuilder.SetResult(result);
                return;
            }
            _result = result;
            _haveResult = true;
        }

        /// <param name="exception"></param>
        public void SetException(Exception exception)
        {
            _methodBuilder.SetException(exception);
        }

        /// <param name="awaiter"></param>
        /// <param name="stateMachine"></param>
        /// <typeparam name="TAwaiter"></typeparam>
        /// <typeparam name="TStateMachine"></typeparam>
        public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine) where TAwaiter : INotifyCompletion where TStateMachine : IAsyncStateMachine
        {
            _useBuilder = true;
            _methodBuilder.AwaitOnCompleted(ref awaiter, ref stateMachine);
        }

        /// <param name="awaiter"></param>
        /// <param name="stateMachine"></param>
        /// <typeparam name="TAwaiter"></typeparam>
        /// <typeparam name="TStateMachine"></typeparam>
        [SecuritySafeCritical]
        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine) where TAwaiter : ICriticalNotifyCompletion where TStateMachine : IAsyncStateMachine
        {
            _useBuilder = true;
            _methodBuilder.AwaitUnsafeOnCompleted(ref awaiter, ref stateMachine);
        }
    }
}
