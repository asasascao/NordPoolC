using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CaoNC.Microsoft.VisualStudio.Threading
{
    internal class TaskCompletionSourceWithoutInlining<T> : TaskCompletionSource<T>
    {
        //
        // 摘要:
        //     The Task that we expose to others that may not inline continuations.
        private readonly Task<T> exposedTask;

        //
        // 摘要:
        //     Gets the Microsoft.VisualStudio.Threading.TaskCompletionSourceWithoutInlining`1.Task
        //     that may never complete inline with completion of this System.Threading.Tasks.TaskCompletionSource`1.
        internal new Task<T> Task
        {
            get
            {
                if (!base.Task.IsCompleted)
                {
                    return exposedTask;
                }

                return base.Task;
            }
        }

        //
        // 摘要:
        //     Initializes a new instance of the Microsoft.VisualStudio.Threading.TaskCompletionSourceWithoutInlining`1
        //     class.
        //
        // 参数:
        //   allowInliningContinuations:
        //     true to allow continuations to be inlined; otherwise false.
        //
        //   options:
        //     TaskCreationOptions to pass on to the base constructor.
        //
        //   state:
        //     The state to set on the Task.
        internal TaskCompletionSourceWithoutInlining(bool allowInliningContinuations, TaskCreationOptions options = TaskCreationOptions.None, object state = null)
            : base(state, AdjustFlags(options, allowInliningContinuations))
        {
            exposedTask = base.Task;
        }

        //
        // 摘要:
        //     Modifies the specified flags to include RunContinuationsAsynchronously if wanted
        //     by the caller and supported by the platform.
        //
        // 参数:
        //   options:
        //     The base options supplied by the caller.
        //
        //   allowInliningContinuations:
        //     true to allow inlining continuations.
        //
        // 返回结果:
        //     The possibly modified flags.
        private static TaskCreationOptions AdjustFlags(TaskCreationOptions options, bool allowInliningContinuations)
        {
            if (!allowInliningContinuations)
            {
                return options | (TaskCreationOptions)Enum.Parse(typeof(TaskCreationOptions), "64"); 
            }

            return options & ~(TaskCreationOptions)Enum.Parse(typeof(TaskCreationOptions), "64");
        }
    }
}
