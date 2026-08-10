namespace System.IO.Pipelines
{
	internal readonly struct PipeCompletionCallback
	{
		public readonly Action<Exception?, object?> Callback;

		public readonly object? State;

		public PipeCompletionCallback(Action<Exception?, object?> callback, object? state)
		{
			Callback = callback;
			State = state;
		}
	}
}
