using System;

namespace CaoNC.System.Channels
{

	/// <summary>Exception thrown when a channel is used after it's been closed.</summary>
	public class ChannelClosedException : InvalidOperationException
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Threading.Channels.ChannelClosedException" /> class.</summary>
		public ChannelClosedException()
			: base(SR.ChannelClosedException_DefaultMessage)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Threading.Channels.ChannelClosedException" /> class.</summary>
		/// <param name="message">The message that describes the error.</param>
		public ChannelClosedException(string message)
			: base(message)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Threading.Channels.ChannelClosedException" /> class.</summary>
		/// <param name="innerException">The exception that is the cause of this exception.</param>
		public ChannelClosedException(Exception innerException)
			: base(SR.ChannelClosedException_DefaultMessage, innerException)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Threading.Channels.ChannelClosedException" /> class.</summary>
		/// <param name="message">The message that describes the error.</param>
		/// <param name="innerException">The exception that is the cause of this exception.</param>
		public ChannelClosedException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
