namespace CaoNC.System.Channels
{
	public abstract class ChannelOptions
	{
		/// <summary>
		///   <see langword="true" /> if writers to the channel guarantee that there will only ever be at most one write operation
		///       at a time; <see langword="false" /> if no such constraint is guaranteed.</summary>
		public bool SingleWriter { get; set; }

		/// <summary>
		///   <see langword="true" /> readers from the channel guarantee that there will only ever be at most one read operation at a time;
		///       <see langword="false" /> if no such constraint is guaranteed.</summary>
		public bool SingleReader { get; set; }

		/// <summary>
		///   <see langword="true" /> if operations performed on a channel may synchronously invoke continuations subscribed to
		///       notifications of pending async operations; <see langword="false" /> if all continuations should be invoked asynchronously.</summary>
		public bool AllowSynchronousContinuations { get; set; }

		/// <summary>Initializes an instance of the <see cref="T:System.Threading.Channels.ChannelOptions" /> class.</summary>
		protected ChannelOptions()
		{
		}
	}

    public sealed class UnboundedChannelOptions : ChannelOptions
    {
        /// <summary>Initializes a new instance of the <see cref="T:System.Threading.Channels.UnboundedChannelOptions" /> class.</summary>
        public UnboundedChannelOptions()
        {
        }
    }
}
