namespace System.IO.Pipelines
{
	internal sealed class DuplexPipe : IDuplexPipe
	{
		public readonly struct DuplexPipePair
		{
			public IDuplexPipe Transport { get; }

			public IDuplexPipe Application { get; }

			public DuplexPipePair(IDuplexPipe transport, IDuplexPipe application)
			{
				Transport = transport;
				Application = application;
			}
		}

		public PipeReader Input { get; }

		public PipeWriter Output { get; }

		public DuplexPipe(PipeReader reader, PipeWriter writer)
		{
			Input = reader;
			Output = writer;
		}

		public static DuplexPipePair CreateConnectionPair(PipeOptions inputOptions, PipeOptions outputOptions)
		{
			Pipe pipe = new Pipe(inputOptions);
			Pipe pipe2 = new Pipe(outputOptions);
			DuplexPipe application = new DuplexPipe(pipe2.Reader, pipe.Writer);
			return new DuplexPipePair(new DuplexPipe(pipe.Reader, pipe2.Writer), application);
		}
	}
}
