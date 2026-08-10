namespace System.Diagnostics
{
	internal delegate ActivitySamplingResult SampleActivity<T>(ref ActivityCreationOptions<T> options);
}
