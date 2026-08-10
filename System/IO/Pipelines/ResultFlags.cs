namespace System.IO.Pipelines
{
	[Flags]
	internal enum ResultFlags : byte
	{
		None = 0x0,
		Canceled = 0x1,
		Completed = 0x2
	}
}
