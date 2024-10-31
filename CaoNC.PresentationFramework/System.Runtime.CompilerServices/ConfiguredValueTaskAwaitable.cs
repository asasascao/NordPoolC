using CaoNC.System.Diagnostics;
using CaoNC.System.Threading.Tasks;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace CaoNC.System.Runtime.CompilerServices
{
    [StructLayout(LayoutKind.Auto)]
    public readonly struct ConfiguredValueTaskAwaitable
    {
        [StructLayout(LayoutKind.Auto)]
        public readonly struct ConfiguredValueTaskAwaiter : ICriticalNotifyCompletion, INotifyCompletion
        {
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
            internal ConfiguredValueTaskAwaiter(ValueTask value)
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
                    task.ConfigureAwait(_value._continueOnCapturedContext).GetAwaiter().OnCompleted(continuation);
                }
                else if (obj != null)
                {
                    Unsafe.As<IValueTaskSource>(obj).OnCompleted(ValueTaskAwaiter.s_invokeActionDelegate, continuation, _value._token, ValueTaskSourceOnCompletedFlags.FlowExecutionContext | (_value._continueOnCapturedContext ? ValueTaskSourceOnCompletedFlags.UseSchedulingContext : ValueTaskSourceOnCompletedFlags.None));
                }
                else
                {
                    ValueTask.CompletedTask.ConfigureAwait(_value._continueOnCapturedContext).GetAwaiter().OnCompleted(continuation);
                }
            }

            public void UnsafeOnCompleted(Action continuation)
            {
                object obj = _value._obj;
                if (obj is Task task)
                {
                    task.ConfigureAwait(_value._continueOnCapturedContext).GetAwaiter().UnsafeOnCompleted(continuation);
                }
                else if (obj != null)
                {
                    Unsafe.As<IValueTaskSource>(obj).OnCompleted(ValueTaskAwaiter.s_invokeActionDelegate, continuation, _value._token, _value._continueOnCapturedContext ? ValueTaskSourceOnCompletedFlags.UseSchedulingContext : ValueTaskSourceOnCompletedFlags.None);
                }
                else
                {
                    ValueTask.CompletedTask.ConfigureAwait(_value._continueOnCapturedContext).GetAwaiter().UnsafeOnCompleted(continuation);
                }
            }
        }

        private readonly ValueTask _value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal ConfiguredValueTaskAwaitable(ValueTask value)
        {
            _value = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ConfiguredValueTaskAwaiter GetAwaiter()
        {
            return new ConfiguredValueTaskAwaiter(_value);
        }
    }

    [StructLayout(LayoutKind.Auto)]
    public readonly struct ConfiguredValueTaskAwaitable<TResult>
    {
        /// <typeparam name="TResult"></typeparam>
        [StructLayout(LayoutKind.Auto)]
        public readonly struct ConfiguredValueTaskAwaiter : ICriticalNotifyCompletion, INotifyCompletion
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
            internal ConfiguredValueTaskAwaiter(ValueTask<TResult> value)
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
                    task.ConfigureAwait(_value._continueOnCapturedContext).GetAwaiter().OnCompleted(continuation);
                }
                else if (obj != null)
                {
                    Unsafe.As<IValueTaskSource<TResult>>(obj).OnCompleted(ValueTaskAwaiter.s_invokeActionDelegate, continuation, _value._token, ValueTaskSourceOnCompletedFlags.FlowExecutionContext | (_value._continueOnCapturedContext ? ValueTaskSourceOnCompletedFlags.UseSchedulingContext : ValueTaskSourceOnCompletedFlags.None));
                }
                else
                {
                    ValueTask.CompletedTask.ConfigureAwait(_value._continueOnCapturedContext).GetAwaiter().OnCompleted(continuation);
                }
            }

            /// <param name="continuation"></param>
            public void UnsafeOnCompleted(Action continuation)
            {
                object obj = _value._obj;
                if (obj is Task<TResult> task)
                {
                    task.ConfigureAwait(_value._continueOnCapturedContext).GetAwaiter().UnsafeOnCompleted(continuation);
                }
                else if (obj != null)
                {
                    Unsafe.As<IValueTaskSource<TResult>>(obj).OnCompleted(ValueTaskAwaiter.s_invokeActionDelegate, continuation, _value._token, _value._continueOnCapturedContext ? ValueTaskSourceOnCompletedFlags.UseSchedulingContext : ValueTaskSourceOnCompletedFlags.None);
                }
                else
                {
                    ValueTask.CompletedTask.ConfigureAwait(_value._continueOnCapturedContext).GetAwaiter().UnsafeOnCompleted(continuation);
                }
            }
        }

        private readonly ValueTask<TResult> _value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal ConfiguredValueTaskAwaitable(ValueTask<TResult> value)
        {
            _value = value;
        }

        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ConfiguredValueTaskAwaiter GetAwaiter()
        {
            return new ConfiguredValueTaskAwaiter(_value);
        }
    }
}
