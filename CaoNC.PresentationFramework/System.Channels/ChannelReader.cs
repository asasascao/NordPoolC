using CaoNC.System.Threading;
using CaoNC.System.Threading.Tasks;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace CaoNC.System.Channels
{

	/// <summary>Provides a base class for reading from a channel.</summary>
	/// <typeparam name="T">Specifies the type of data that may be read from the channel.</typeparam>
	public abstract class ChannelReader<T>
	{
		/// <summary>Gets a <see cref="T:System.Threading.Tasks.Task" /> that completes when no more data will ever
		///       be available to be read from this channel.</summary>
		public virtual Task Completion => ChannelUtilities.s_neverCompletingTask;

		/// <summary>Gets a value that indicates whether <see cref="P:System.Threading.Channels.ChannelReader`1.Count" /> is available for use on this <see cref="T:System.Threading.Channels.ChannelReader`1" /> instance.</summary>
		public virtual bool CanCount => false;

		/// <summary>Gets a value that indicates whether <see cref="M:System.Threading.Channels.ChannelReader`1.TryPeek(`0@)" /> is available for use on this <see cref="T:System.Threading.Channels.ChannelReader`1" /> instance.</summary>
		/// <returns>
		///   <see langword="true" /> if peeking is supported by this channel instance; <see langword="false" /> otherwise.</returns>
		public virtual bool CanPeek => false;

		/// <summary>Gets the current number of items available from this channel reader.</summary>
		/// <exception cref="T:System.NotSupportedException">Counting is not supported on this instance.</exception>
		public virtual int Count
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		/// <summary>Attempts to read an item from the channel.</summary>
		/// <param name="item">The read item, or a default value if no item could be read.</param>
		/// <returns>
		///   <see langword="true" /> if an item was read; otherwise, <see langword="false" />.</returns>
		public abstract bool TryRead(out T item);

		/// <summary>Attempts to peek at an item from the channel.</summary>
		/// <param name="item">The peeked item, or a default value if no item could be peeked.</param>
		/// <returns>
		///   <see langword="true" /> if an item was read; otherwise, <see langword="false" />.</returns>
		public virtual bool TryPeek(out T item)
		{
			item = default(T);
			return false;
		}

		/// <summary>Returns a <see cref="T:System.Threading.Tasks.ValueTask`1" /> that will complete when data is available to read.</summary>
		/// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> used to cancel the wait operation.</param>
		/// <returns>
		///   <para>A <see cref="T:System.Threading.Tasks.ValueTask`1" /> that will complete with a <see langword="true" /> result when data is available to read
		///       or with a <see langword="false" /> result when no further data will ever be available to be read due to the channel completing successfully.</para>
		///   <para>If the channel completes with an exception, the task will also complete with an exception.</para>
		/// </returns>
		public abstract System.Threading.Tasks.ValueTask<bool> WaitToReadAsync(CancellationToken cancellationToken = default(CancellationToken));

		/// <summary>Asynchronously reads an item from the channel.</summary>
		/// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> used to cancel the read operation.</param>
		/// <returns>A <see cref="T:System.Threading.Tasks.ValueTask`1" /> that represents the asynchronous read operation.</returns>
		public virtual System.Threading.Tasks.ValueTask<T> ReadAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return new System.Threading.Tasks.ValueTask<T>(TaskEx.FromCanceled<T>(cancellationToken));
			}
            try
            {
                if (TryRead(out var item))
                {
                    return new ValueTask<T>(item);
                }
            }
            catch (Exception ex) when (!(ex is ChannelClosedException) && !(ex is OperationCanceledException))
            {
                return new ValueTask<T>(TaskEx.FromException<T>(ex));
            }
            return new ValueTask<T>(ReadAsyncCore(cancellationToken));

            async Task<T> ReadAsyncCore(CancellationToken ct)
            {
                T item2;
                do
                {
                    if (!(await WaitToReadAsync(ct).ConfigureAwait(continueOnCapturedContext: false)))
                    {
                        throw new ChannelClosedException();
                    }
                }
                while (!TryRead(out item2));
                return item2;
            }
        }

        protected ChannelReader()
		{
		}
	}
}
