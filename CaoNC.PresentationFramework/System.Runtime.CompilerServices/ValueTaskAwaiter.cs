﻿using CaoNC.System.Diagnostics;
using CaoNC.System.Threading.Tasks;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace CaoNC.System.Runtime.CompilerServices
{
    public readonly struct ValueTaskAwaiter : ICriticalNotifyCompletion, INotifyCompletion
    {
        internal static readonly Action<object> s_invokeActionDelegate = delegate (object state)
        {
            if (!(state is Action action))
            {
                System.ThrowHelper.ThrowArgumentOutOfRangeException(System.ExceptionArgument.state);
            }
            else
            {
                action();
            }
        };

        private readonly ValueTask _value;

        public bool IsCompleted
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return _value.IsCompleted;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal ValueTaskAwaiter(ValueTask value)
        {
            _value = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [StackTraceHidden]
        public void GetResult()
        {
            _value.ThrowIfCompletedUnsuccessfully();
        }

        public void OnCompleted(Action continuation)
        {
            object obj = _value._obj;
            if (obj is Task task)
            {
                task.GetAwaiter().OnCompleted(continuation);
            }
            else if (obj != null)
            {
                Unsafe.As<IValueTaskSource>(obj).OnCompleted(s_invokeActionDelegate, continuation, _value._token, ValueTaskSourceOnCompletedFlags.UseSchedulingContext | ValueTaskSourceOnCompletedFlags.FlowExecutionContext);
            }
            else
            {
                ValueTask.CompletedTask.GetAwaiter().OnCompleted(continuation);
            }
        }

        public void UnsafeOnCompleted(Action continuation)
        {
            object obj = _value._obj;
            if (obj is Task task)
            {
                task.GetAwaiter().UnsafeOnCompleted(continuation);
            }
            else if (obj != null)
            {
                Unsafe.As<IValueTaskSource>(obj).OnCompleted(s_invokeActionDelegate, continuation, _value._token, ValueTaskSourceOnCompletedFlags.UseSchedulingContext);
            }
            else
            {
                ValueTask.CompletedTask.GetAwaiter().UnsafeOnCompleted(continuation);
            }
        }
    }

    /// <typeparam name="TResult"></typeparam>
    public readonly struct ValueTaskAwaiter<TResult> : ICriticalNotifyCompletion, INotifyCompletion
    {
        private readonly ValueTask<TResult> _value;

        /// <returns></returns>
        public bool IsCompleted
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return _value.IsCompleted;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal ValueTaskAwaiter(ValueTask<TResult> value)
        {
            _value = value;
        }

        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [StackTraceHidden]
        public TResult GetResult()
        {
            return _value.Result;
        }

        /// <param name="continuation"></param>
        public void OnCompleted(Action continuation)
        {
            object obj = _value._obj;
            if (obj is Task<TResult> task)
            {
                task.GetAwaiter().OnCompleted(continuation);
            }
            else if (obj != null)
            {
                Unsafe.As<IValueTaskSource<TResult>>(obj).OnCompleted(ValueTaskAwaiter.s_invokeActionDelegate, continuation, _value._token, ValueTaskSourceOnCompletedFlags.UseSchedulingContext | ValueTaskSourceOnCompletedFlags.FlowExecutionContext);
            }
            else
            {
                ValueTask.CompletedTask.GetAwaiter().OnCompleted(continuation);
            }
        }

        /// <param name="continuation"></param>
        public void UnsafeOnCompleted(Action continuation)
        {
            object obj = _value._obj;
            if (obj is Task<TResult> task)
            {
                task.GetAwaiter().UnsafeOnCompleted(continuation);
            }
            else if (obj != null)
            {
                Unsafe.As<IValueTaskSource<TResult>>(obj).OnCompleted(ValueTaskAwaiter.s_invokeActionDelegate, continuation, _value._token, ValueTaskSourceOnCompletedFlags.UseSchedulingContext);
            }
            else
            {
                ValueTask.CompletedTask.GetAwaiter().UnsafeOnCompleted(continuation);
            }
        }
    }
}
