using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CaoNC.System.Threading
{
    public class TaskEx
    {
        [FriendAccessAllowed]
        internal static Task FromCancellation(CancellationToken cancellationToken)
        {
            if (!cancellationToken.IsCancellationRequested)
            {
                throw new ArgumentOutOfRangeException("cancellationToken");
            }
            return TaskEx.FromCanceled(cancellationToken);
        }

        [__DynamicallyInvokable]
        public static Task FromCanceled(CancellationToken cancellationToken)
        {
            return FromCancellation(cancellationToken);
        }

        [FriendAccessAllowed]
        internal static Task<TResult> FromCancellation<TResult>(CancellationToken cancellationToken)
        {
            if (!cancellationToken.IsCancellationRequested)
            {
                throw new ArgumentOutOfRangeException("cancellationToken");
            }
            return TaskEx.FromCanceled<TResult>(cancellationToken);
        }

        [__DynamicallyInvokable]
        public static Task<TResult> FromCanceled<TResult>(CancellationToken cancellationToken)
        {
            return FromCancellation<TResult>(cancellationToken);
        }

        [__DynamicallyInvokable]
        public static Task FromException(Exception exception)
        {
            return CreateFailedTask(exception);
        }

        private static Task CreateFailedTask(Exception exception)
        {
            var tcs = new TaskCompletionSource<bool>();
            tcs.SetException(exception);
            return tcs.Task;
        }

        [__DynamicallyInvokable]
        public static Task<T> FromException<T>(Exception exception)
        {
            return CreateFailedTask<T>(exception);
        }

        private static Task<T> CreateFailedTask<T>(Exception exception)
        {
            var tcs = new TaskCompletionSource<T>();
            tcs.SetException(exception);
            return tcs.Task;
        }
    }
}
