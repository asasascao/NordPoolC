using CaoNC.System.Threading;
using CaoNC.System.Threading.Tasks;
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace CaoNC.System.Channels
{
	/// <summary>Provides a base class for writing to a channel.</summary>
	/// <typeparam name="T">Specifies the type of data that may be written to the channel.</typeparam>
	public abstract class ChannelWriter<T>
	{
		/// <summary>Attempts to mark the channel as being completed, meaning no more data will be written to it.</summary>
		/// <param name="error">An <see cref="T:System.Exception" /> indicating the failure causing no more data to be written, or null for success.</param>
		/// <returns>
		///   <see langword="true" /> if this operation successfully completes the channel; otherwise, <see langword="false" /> if the channel could not be marked for completion,
		///       for example due to having already been marked as such, or due to not supporting completion.
		///     .</returns>
		public virtual bool TryComplete(Exception error = null)
		{
			return false;
		}

		/// <summary>Attempts to write the specified item to the channel.</summary>
		/// <param name="item">The item to write.</param>
		/// <returns>
		///   <see langword="true" /> if the item was written; otherwise, <see langword="false" />.</returns>
		public abstract bool TryWrite(T item);

		/// <summary>Returns a <see cref="T:System.Threading.Tasks.ValueTask`1" /> that will complete when space is available to write an item.</summary>
		/// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> used to cancel the wait operation.</param>
		/// <returns>A <see cref="T:System.Threading.Tasks.ValueTask`1" /> that will complete with a <see langword="true" /> result when space is available to write an item
		///       or with a <see langword="false" /> result when no further writing will be permitted.</returns>
		public abstract System.Threading.Tasks.ValueTask<bool> WaitToWriteAsync(CancellationToken cancellationToken = default(CancellationToken));

		/// <summary>Asynchronously writes an item to the channel.</summary>
		/// <param name="item">The value to write to the channel.</param>
		/// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> used to cancel the write operation.</param>
		/// <returns>A <see cref="T:System.Threading.Tasks.ValueTask" /> that represents the asynchronous write operation.</returns>
		public virtual System.Threading.Tasks.ValueTask WriteAsync(T item, CancellationToken cancellationToken = default(CancellationToken))
		{
			try
			{
				return cancellationToken.IsCancellationRequested ? 
					new System.Threading.Tasks.ValueTask((Task)TaskEx.FromCanceled<T>(cancellationToken)) :
					(TryWrite(item) ? default(System.Threading.Tasks.ValueTask) :
					new System.Threading.Tasks.ValueTask(WriteAsyncCore(item, cancellationToken)));
			}
			catch (Exception exception)
			{
				return new System.Threading.Tasks.ValueTask(TaskEx.FromException(exception));
			}
		}

        private async Task WriteAsyncCore(T innerItem, CancellationToken ct)
        {
            while (await WaitToWriteAsync(ct).ConfigureAwait(continueOnCapturedContext: false))
            {
                if (TryWrite(innerItem))
                {
					return;
                }
            }
            throw ChannelUtilities.CreateInvalidCompletionException();
        }

        /// <summary>Mark the channel as being complete, meaning no more items will be written to it.</summary>
        /// <param name="error">Optional Exception indicating a failure that's causing the channel to complete.</param>
        /// <exception cref="T:System.InvalidOperationException">The channel has already been marked as complete.</exception>
        public void Complete(Exception error = null)
		{
			if (!TryComplete(error))
			{
				throw ChannelUtilities.CreateInvalidCompletionException();
			}
		}

		/// <summary>Initializes an instance of the <see cref="T:System.Threading.Channels.ChannelWriter`1" /> class.</summary>
		protected ChannelWriter()
		{
		}
	}
}
