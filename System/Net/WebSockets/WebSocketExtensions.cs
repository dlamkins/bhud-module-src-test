using System.Buffers;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.WebSockets
{
	internal static class WebSocketExtensions
	{
		public static ValueTask SendAsync(this WebSocket webSocket, ReadOnlySequence<byte> buffer, WebSocketMessageType webSocketMessageType, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (buffer.IsSingleSegment)
			{
				MemoryMarshal.TryGetArray(buffer.First, out var segment);
				return new ValueTask(webSocket.SendAsync(segment, webSocketMessageType, endOfMessage: true, cancellationToken));
			}
			return SendMultiSegmentAsync(webSocket, buffer, webSocketMessageType, cancellationToken);
		}

		private static async ValueTask SendMultiSegmentAsync(WebSocket webSocket, ReadOnlySequence<byte> buffer, WebSocketMessageType webSocketMessageType, CancellationToken cancellationToken = default(CancellationToken))
		{
			SequencePosition position = buffer.Start;
			buffer.TryGet(ref position, out var memory);
			ReadOnlyMemory<byte> segment;
			while (buffer.TryGet(ref position, out segment))
			{
				MemoryMarshal.TryGetArray(memory, out var segment2);
				await webSocket.SendAsync(segment2, webSocketMessageType, endOfMessage: false, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				memory = segment;
			}
			MemoryMarshal.TryGetArray(memory, out var segment3);
			await webSocket.SendAsync(segment3, webSocketMessageType, endOfMessage: true, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}
	}
}
