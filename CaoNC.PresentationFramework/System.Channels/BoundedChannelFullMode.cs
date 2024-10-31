namespace CaoNC.System.Channels
{
	/// <summary>Specifies the behavior to use when writing to a bounded channel that is already full.</summary>
	public enum BoundedChannelFullMode
	{
		/// <summary>Waits for space to be available in order to complete the write operation.</summary>
		Wait,
		/// <summary>Removes and ignores the newest item in the channel in order to make room for the item being written.</summary>
		DropNewest,
		/// <summary>Removes and ignores the oldest item in the channel in order to make room for the item being written.</summary>
		DropOldest,
		/// <summary>Drops the item being written.</summary>
		DropWrite
	}
}
