using System.Buffers;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace System.IO
{
	internal static class StreamExtensions
	{
		public static ValueTask WriteAsync(this Stream stream, ReadOnlySequence<byte> buffer, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (buffer.IsSingleSegment)
			{
				MemoryMarshal.TryGetArray(buffer.First, out var segment);
				return new ValueTask(stream.WriteAsync(segment.Array, segment.Offset, segment.Count, cancellationToken));
			}
			return WriteMultiSegmentAsync(stream, buffer, cancellationToken);
		}

		private static async ValueTask WriteMultiSegmentAsync(Stream stream, ReadOnlySequence<byte> buffer, CancellationToken cancellationToken)
		{
			SequencePosition position = buffer.Start;
			ReadOnlyMemory<byte> memory;
			while (buffer.TryGet(ref position, out memory))
			{
				MemoryMarshal.TryGetArray(memory, out var segment);
				await stream.WriteAsync(segment.Array, segment.Offset, segment.Count, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			}
		}
	}
}
