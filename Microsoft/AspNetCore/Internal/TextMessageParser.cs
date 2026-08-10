using System;
using System.Buffers;
using System.Runtime.CompilerServices;

namespace Microsoft.AspNetCore.Internal
{
	internal static class TextMessageParser
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool TryParseMessage(ref ReadOnlySequence<byte> buffer, out ReadOnlySequence<byte> payload)
		{
			if (buffer.IsSingleSegment)
			{
				int num = buffer.First.Span.IndexOf<byte>(30);
				if (num == -1)
				{
					payload = default(ReadOnlySequence<byte>);
					return false;
				}
				payload = buffer.Slice(0, num);
				buffer = buffer.Slice(num + 1);
				return true;
			}
			return TryParseMessageMultiSegment(ref buffer, out payload);
		}

		private static bool TryParseMessageMultiSegment(ref ReadOnlySequence<byte> buffer, out ReadOnlySequence<byte> payload)
		{
			SequencePosition? sequencePosition = buffer.PositionOf<byte>(30);
			if (!sequencePosition.HasValue)
			{
				payload = default(ReadOnlySequence<byte>);
				return false;
			}
			payload = buffer.Slice(0, sequencePosition.Value);
			buffer = buffer.Slice(buffer.GetPosition(1L, sequencePosition.Value));
			return true;
		}
	}
}
