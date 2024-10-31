using System;

namespace CaoNC.System.Channels
{

	/// <summary>Provides static methods for creating channels.</summary>
	public static class Channel
	{
		/// <summary>Creates an unbounded channel usable by any number of readers and writers concurrently.</summary>
		/// <typeparam name="T">The type of data in the channel.</typeparam>
		/// <returns>The created channel.</returns>
		public static Channel<T> CreateUnbounded<T>()
		{
			return new UnboundedChannel<T>(runContinuationsAsynchronously: true);
		}

		/// <summary>Creates an unbounded channel subject to the provided options.</summary>
		/// <param name="options">Options that guide the behavior of the channel.</param>
		/// <typeparam name="T">Specifies the type of data in the channel.</typeparam>
		/// <returns>The created channel.</returns>
		public static Channel<T> CreateUnbounded<T>(UnboundedChannelOptions options)
		{
			if (options == null)
			{
				throw new ArgumentNullException("options");
			}
			if (options.SingleReader)
			{
				return new SingleConsumerUnboundedChannel<T>(!options.AllowSynchronousContinuations);
			}
			return new UnboundedChannel<T>(!options.AllowSynchronousContinuations);
		}

		/// <summary>Creates a channel with the specified maximum capacity.</summary>
		/// <param name="capacity">The maximum number of items the channel may store.</param>
		/// <typeparam name="T">Specifies the type of data in the channel.</typeparam>
		/// <returns>The created channel.</returns>
		public static Channel<T> CreateBounded<T>(int capacity)
		{
			if (capacity < 1)
			{
				throw new ArgumentOutOfRangeException("capacity");
			}
			return new BoundedChannel<T>(capacity, BoundedChannelFullMode.Wait, runContinuationsAsynchronously: true, null);
		}

		/// <summary>Creates a channel with the specified maximum capacity.</summary>
		/// <param name="options">Options that guide the behavior of the channel.</param>
		/// <typeparam name="T">Specifies the type of data in the channel.</typeparam>
		/// <returns>The created channel.</returns>
		public static Channel<T> CreateBounded<T>(BoundedChannelOptions options)
		{
			return CreateBounded<T>(options, null);
		}

		/// <summary>Creates a channel subject to the provided options.</summary>
		/// <param name="options">Options that guide the behavior of the channel.</param>
		/// <param name="itemDropped">Delegate that will be called when item is being dropped from channel. See <see cref="T:System.Threading.Channels.BoundedChannelFullMode" />.</param>
		/// <typeparam name="T">Specifies the type of data in the channel.</typeparam>
		/// <returns>The created channel.</returns>
		public static Channel<T> CreateBounded<T>(BoundedChannelOptions options, Action<T> itemDropped)
		{
			if (options == null)
			{
				throw new ArgumentNullException("options");
			}
			return new BoundedChannel<T>(options.Capacity, options.FullMode, !options.AllowSynchronousContinuations, itemDropped);
		}
	}
	/// <summary>Provides a base class for channels that support reading and writing elements of type <typeparamref name="T" />.</summary>
	/// <typeparam name="T">Specifies the type of data readable and writable in the channel.</typeparam>
	public abstract class Channel<T> : Channel<T, T>
	{
		/// <summary>Initializes an instance of the <see cref="T:System.Threading.Channels.Channel`1" /> class.</summary>
		protected Channel()
		{
		}
	}
	/// <summary>Provides a base class for channels that support reading elements of type <typeparamref name="TRead" /> and writing elements of type <typeparamref name="TWrite" />.</summary>
	/// <typeparam name="TWrite">Specifies the type of data that may be written to the channel.</typeparam>
	/// <typeparam name="TRead">Specifies the type of data that may be read from the channel.</typeparam>
	public abstract class Channel<TWrite, TRead>
	{
		/// <summary>Gets the readable half of this channel.</summary>
		public ChannelReader<TRead> Reader { get; protected set; }

		/// <summary>Gets the writable half of this channel.</summary>
		public ChannelWriter<TWrite> Writer { get; protected set; }

		/// <summary>Implicit cast from a <see cref="T:System.Threading.Channels.Channel`2" /> to its readable half.</summary>
		/// <param name="channel">The <see cref="T:System.Threading.Channels.Channel`2" /> being cast.</param>
		/// <returns>The readable half.</returns>
		public static implicit operator ChannelReader<TRead>(Channel<TWrite, TRead> channel)
		{
			return channel.Reader;
		}

		/// <summary>Implicit cast from a <see cref="T:System.Threading.Channels.Channel`2" /> to its writable half.</summary>
		/// <param name="channel">The <see cref="T:System.Threading.Channels.Channel`2" /> being cast.</param>
		/// <returns>The writable half.</returns>
		public static implicit operator ChannelWriter<TWrite>(Channel<TWrite, TRead> channel)
		{
			return channel.Writer;
		}

		/// <summary>Initializes an instance of the <see cref="T:System.Threading.Channels.Channel`2" /> class.</summary>
		protected Channel()
		{
		}
	}
}
