using System.IO.Pipelines;

namespace Microsoft.AspNetCore.Http.Connections.Client.Internal
{
	internal static class ClientPipeOptions
	{
		public static PipeOptions DefaultOptions = new PipeOptions(null, writerScheduler: PipeScheduler.ThreadPool, readerScheduler: PipeScheduler.ThreadPool, pauseWriterThreshold: -1L, resumeWriterThreshold: -1L, minimumSegmentSize: -1, useSynchronizationContext: false);
	}
}
