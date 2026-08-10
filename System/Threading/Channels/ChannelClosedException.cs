namespace System.Threading.Channels
{
	internal class ChannelClosedException : InvalidOperationException
	{
		public ChannelClosedException()
			: base(_003Cf4f5f2aa_002Dfdc0_002D4274_002D94a7_002Def43d0100ed5_003ESR.ChannelClosedException_DefaultMessage)
		{
		}

		public ChannelClosedException(string? message)
			: base(message)
		{
		}

		public ChannelClosedException(Exception? innerException)
			: base(_003Cf4f5f2aa_002Dfdc0_002D4274_002D94a7_002Def43d0100ed5_003ESR.ChannelClosedException_DefaultMessage, innerException)
		{
		}

		public ChannelClosedException(string? message, Exception? innerException)
			: base(message, innerException)
		{
		}
	}
}
