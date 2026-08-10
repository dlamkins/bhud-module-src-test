namespace System.Threading.Channels
{
	internal abstract class ChannelOptions
	{
		public bool SingleWriter { get; set; }

		public bool SingleReader { get; set; }

		public bool AllowSynchronousContinuations { get; set; }
	}
}
