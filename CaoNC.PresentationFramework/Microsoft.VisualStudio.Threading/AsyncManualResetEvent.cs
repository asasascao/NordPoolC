using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CaoNC.Microsoft.VisualStudio.Threading
{
    [DebuggerDisplay("Signaled: {IsSet}")]
    public class AsyncManualResetEvent
    {
        //
        // 摘要:
        //     Whether the task completion source should allow executing continuations synchronously.
        private readonly bool allowInliningAwaiters;

        //
        // 摘要:
        //     The object to lock when accessing fields.
        private readonly object syncObject = new object();

        //
        // 摘要:
        //     The source of the task to return from Microsoft.VisualStudio.Threading.AsyncManualResetEvent.WaitAsync.
        private TaskCompletionSourceWithoutInlining<EmptyStruct> taskCompletionSource;

        //
        // 摘要:
        //     A flag indicating whether the event is signaled. When this is set to true, it's
        //     possible that Microsoft.VisualStudio.Threading.AsyncManualResetEvent.taskCompletionSource.Task.IsCompleted
        //     is still false if the completion has been scheduled asynchronously. Thus, this
        //     field should be the definitive answer as to whether the event is signaled because
        //     it is synchronously updated.
        private bool isSet;

        //
        // 摘要:
        //     Gets a value indicating whether the event is currently in a signaled state.
        public bool IsSet
        {
            get
            {
                lock (syncObject)
                {
                    return isSet;
                }
            }
        }

        //
        // 摘要:
        //     Initializes a new instance of the Microsoft.VisualStudio.Threading.AsyncManualResetEvent
        //     class.
        //
        // 参数:
        //   initialState:
        //     A value indicating whether the event should be initially signaled.
        //
        //   allowInliningAwaiters:
        //     A value indicating whether to allow Microsoft.VisualStudio.Threading.AsyncManualResetEvent.WaitAsync
        //     callers' continuations to execute on the thread that calls Microsoft.VisualStudio.Threading.AsyncManualResetEvent.SetAsync
        //     before the call returns. Microsoft.VisualStudio.Threading.AsyncManualResetEvent.SetAsync
        //     callers should not hold private locks if this value is true to avoid deadlocks.
        //     When false, the task returned from Microsoft.VisualStudio.Threading.AsyncManualResetEvent.WaitAsync
        //     may not have fully transitioned to its completed state by the time Microsoft.VisualStudio.Threading.AsyncManualResetEvent.SetAsync
        //     returns to its caller.
        public AsyncManualResetEvent(bool initialState = false, bool allowInliningAwaiters = false)
        {
            this.allowInliningAwaiters = allowInliningAwaiters;
            taskCompletionSource = CreateTaskSource();
            isSet = initialState;
            if (initialState)
            {
                taskCompletionSource.SetResult(EmptyStruct.Instance);
            }
        }

        //
        // 摘要:
        //     Returns a task that will be completed when this event is set.
        public Task WaitAsync()
        {
            lock (syncObject)
            {
                return taskCompletionSource.Task;
            }
        }

        //
        // 摘要:
        //     Sets this event to unblock callers of Microsoft.VisualStudio.Threading.AsyncManualResetEvent.WaitAsync.
        //
        //
        // 返回结果:
        //     A task that completes when the signal has been set.
        //
        // 言论：
        //     On .NET versions prior to 4.6: This method may return before the signal set has
        //     propagated (so Microsoft.VisualStudio.Threading.AsyncManualResetEvent.IsSet may
        //     return false for a bit more if called immediately). The returned task completes
        //     when the signal has definitely been set.
        //
        //     On .NET 4.6 and later: This method is not asynchronous. The returned Task is
        //     always completed.
        [Obsolete("Use Set() instead.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Task SetAsync()
        {
            TaskCompletionSourceWithoutInlining<EmptyStruct> taskCompletionSourceWithoutInlining = null;
            bool flag = false;
            lock (syncObject)
            {
                flag = !isSet;
                taskCompletionSourceWithoutInlining = taskCompletionSource;
                isSet = true;
            }

            Task<EmptyStruct> task = taskCompletionSourceWithoutInlining.Task;
            if (flag)
            {
                taskCompletionSourceWithoutInlining.TrySetResult(default(EmptyStruct));
            }

            return task;
        }

        //
        // 摘要:
        //     Sets this event to unblock callers of Microsoft.VisualStudio.Threading.AsyncManualResetEvent.WaitAsync.
        public void Set()
        {
            SetAsync();
        }

        //
        // 摘要:
        //     Resets this event to a state that will block callers of Microsoft.VisualStudio.Threading.AsyncManualResetEvent.WaitAsync.
        public void Reset()
        {
            lock (syncObject)
            {
                if (isSet)
                {
                    taskCompletionSource = CreateTaskSource();
                    isSet = false;
                }
            }
        }

        //
        // 摘要:
        //     Sets and immediately resets this event, allowing all current waiters to unblock.
        //
        //
        // 返回结果:
        //     A task that completes when the signal has been set.
        //
        // 言论：
        //     On .NET versions prior to 4.6: This method may return before the signal set has
        //     propagated (so Microsoft.VisualStudio.Threading.AsyncManualResetEvent.IsSet may
        //     return false for a bit more if called immediately). The returned task completes
        //     when the signal has definitely been set.
        //
        //     On .NET 4.6 and later: This method is not asynchronous. The returned Task is
        //     always completed.
        [Obsolete("Use PulseAll() instead.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Task PulseAllAsync()
        {
            TaskCompletionSourceWithoutInlining<EmptyStruct> taskCompletionSourceWithoutInlining = null;
            lock (syncObject)
            {
                taskCompletionSourceWithoutInlining = taskCompletionSource;
                taskCompletionSource = CreateTaskSource();
                isSet = false;
            }

            Task task = taskCompletionSourceWithoutInlining.Task;
            taskCompletionSourceWithoutInlining.TrySetResult(default(EmptyStruct));
            return task;
        }

        //
        // 摘要:
        //     Sets and immediately resets this event, allowing all current waiters to unblock.
        public void PulseAll()
        {
            PulseAllAsync();
        }

        //
        // 摘要:
        //     Gets an awaiter that completes when this event is signaled.
        [EditorBrowsable(EditorBrowsableState.Never)]
        public TaskAwaiter GetAwaiter()
        {
            return WaitAsync().GetAwaiter();
        }

        //
        // 摘要:
        //     Creates a new TaskCompletionSource to represent an unset event.
        private TaskCompletionSourceWithoutInlining<EmptyStruct> CreateTaskSource()
        {
            return new TaskCompletionSourceWithoutInlining<EmptyStruct>(allowInliningAwaiters);
        }
    }
}
