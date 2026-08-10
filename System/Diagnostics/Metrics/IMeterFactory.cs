namespace System.Diagnostics.Metrics
{
	internal interface IMeterFactory : IDisposable
	{
		Meter Create(MeterOptions options);
	}
}
