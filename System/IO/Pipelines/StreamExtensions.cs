using System.Buffers;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace System.IO.Pipelines
{
	internal static class StreamExtensions
	{
		public static ValueTask<int> ReadAsync(this Stream stream, Memory<byte> buffer, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (MemoryMarshal.TryGetArray((ReadOnlyMemory<byte>)buffer, out ArraySegment<byte> segment))
			{
				return new ValueTask<int>(stream.ReadAsync(segment.Array, segment.Offset, segment.Count, cancellationToken));
			}
			byte[] array = ArrayPool<byte>.Shared.Rent(buffer.Length);
			return FinishReadAsync(stream.ReadAsync(array, 0, buffer.Length, cancellationToken), array, buffer);
			static async ValueTask<int> FinishReadAsync(Task<int> readTask, byte[] localBuffer, Memory<byte> localDestination)
			{
				try
				{
					int num = await readTask.ConfigureAwait(continueOnCapturedContext: false);
					new Span<byte>(localBuffer, 0, num).CopyTo(localDestination.Span);
					return num;
				}
				finally
				{
					ArrayPool<byte>.Shared.Return(localBuffer);
				}
			}
		}

		public static void Write(this Stream stream, ReadOnlyMemory<byte> buffer)
		{
			if (MemoryMarshal.TryGetArray(buffer, out var segment))
			{
				stream.Write(segment.Array, segment.Offset, segment.Count);
				return;
			}
			byte[] array = ArrayPool<byte>.Shared.Rent(buffer.Length);
			try
			{
				buffer.Span.CopyTo(array);
				stream.Write(array, 0, buffer.Length);
			}
			finally
			{
				ArrayPool<byte>.Shared.Return(array);
			}
		}

		public static ValueTask WriteAsync(this Stream stream, ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (MemoryMarshal.TryGetArray(buffer, out var segment))
			{
				return new ValueTask(stream.WriteAsync(segment.Array, segment.Offset, segment.Count, cancellationToken));
			}
			byte[] array = ArrayPool<byte>.Shared.Rent(buffer.Length);
			buffer.Span.CopyTo(array);
			return new ValueTask(FinishWriteAsync(stream.WriteAsync(array, 0, buffer.Length, cancellationToken), array));
		}

		private static async Task FinishWriteAsync(Task writeTask, byte[] localBuffer)
		{
			try
			{
				await writeTask.ConfigureAwait(continueOnCapturedContext: false);
			}
			finally
			{
				ArrayPool<byte>.Shared.Return(localBuffer);
			}
		}

		public static Task CopyToAsync(this Stream source, Stream destination, CancellationToken cancellationToken = default(CancellationToken))
		{
			return source.CopyToAsync(destination, 81920, cancellationToken);
		}
	}
}
