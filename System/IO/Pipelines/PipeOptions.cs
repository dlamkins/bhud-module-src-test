using System.Buffers;

namespace System.IO.Pipelines
{
	internal class PipeOptions
	{
		private const int DefaultMinimumSegmentSize = 4096;

		public static PipeOptions Default { get; } = new PipeOptions(null, null, null, -1L, -1L);


		public bool UseSynchronizationContext { get; }

		public long PauseWriterThreshold { get; }

		public long ResumeWriterThreshold { get; }

		public int MinimumSegmentSize { get; }

		public PipeScheduler WriterScheduler { get; }

		public PipeScheduler ReaderScheduler { get; }

		public MemoryPool<byte> Pool { get; }

		internal bool IsDefaultSharedMemoryPool { get; }

		internal int InitialSegmentPoolSize { get; }

		internal int MaxSegmentPoolSize { get; }

		public PipeOptions(MemoryPool<byte>? pool = null, PipeScheduler? readerScheduler = null, PipeScheduler? writerScheduler = null, long pauseWriterThreshold = -1L, long resumeWriterThreshold = -1L, int minimumSegmentSize = -1, bool useSynchronizationContext = true)
		{
			MinimumSegmentSize = ((minimumSegmentSize == -1) ? 4096 : minimumSegmentSize);
			InitialSegmentPoolSize = 4;
			MaxSegmentPoolSize = 256;
			if (pauseWriterThreshold == -1)
			{
				pauseWriterThreshold = 65536L;
			}
			else if (pauseWriterThreshold < 0)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.pauseWriterThreshold);
			}
			if (resumeWriterThreshold == -1)
			{
				resumeWriterThreshold = 32768L;
			}
			else if (resumeWriterThreshold < 0 || resumeWriterThreshold > pauseWriterThreshold)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.resumeWriterThreshold);
			}
			Pool = pool ?? MemoryPool<byte>.Shared;
			IsDefaultSharedMemoryPool = Pool == MemoryPool<byte>.Shared;
			ReaderScheduler = readerScheduler ?? PipeScheduler.ThreadPool;
			WriterScheduler = writerScheduler ?? PipeScheduler.ThreadPool;
			PauseWriterThreshold = pauseWriterThreshold;
			ResumeWriterThreshold = resumeWriterThreshold;
			UseSynchronizationContext = useSynchronizationContext;
		}
	}
}
