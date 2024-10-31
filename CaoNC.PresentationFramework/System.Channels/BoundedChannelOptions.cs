using System;

namespace CaoNC.System.Channels
{

	/// <summary>Provides options that control the behavior of bounded <see cref="T:System.Threading.Channels.Channel`1" /> instances.</summary>
	public sealed class BoundedChannelOptions : ChannelOptions
	{
		private int _capacity;

		private BoundedChannelFullMode _mode;

		/// <summary>Gets or sets the maximum number of items the bounded channel may store.</summary>
		public int Capacity
		{
			get
			{
				return _capacity;
			}
			set
			{
				if (value < 1)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				_capacity = value;
			}
		}

		/// <summary>Gets or sets the behavior incurred by write operations when the channel is full.</summary>
		public BoundedChannelFullMode FullMode
		{
			get
			{
				return _mode;
			}
			set
			{
				if ((uint)value <= 3u)
				{
					_mode = value;
					return;
				}
				throw new ArgumentOutOfRangeException("value");
			}
		}

		/// <summary>Initializes the options.</summary>
		/// <param name="capacity">The maximum number of items the bounded channel may store.</param>
		public BoundedChannelOptions(int capacity)
		{
			if (capacity < 1)
			{
				throw new ArgumentOutOfRangeException("capacity");
			}
			_capacity = capacity;
		}
	}
}
