namespace System.Threading.Tasks.Sources
{
	[Flags]
	internal enum ValueTaskSourceOnCompletedFlags
	{
		None = 0x0,
		UseSchedulingContext = 0x1,
		FlowExecutionContext = 0x2
	}
}
