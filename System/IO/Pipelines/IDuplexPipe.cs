namespace System.IO.Pipelines
{
	internal interface IDuplexPipe
	{
		PipeReader Input { get; }

		PipeWriter Output { get; }
	}
}
