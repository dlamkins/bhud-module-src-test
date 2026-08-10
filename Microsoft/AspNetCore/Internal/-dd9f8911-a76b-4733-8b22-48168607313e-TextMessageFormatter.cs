using System.Buffers;

namespace Microsoft.AspNetCore.Internal
{
	internal static class _003Cdd9f8911_002Da76b_002D4733_002D8b22_002D48168607313e_003ETextMessageFormatter
	{
		public const byte RecordSeparator = 30;

		public static void WriteRecordSeparator(IBufferWriter<byte> output)
		{
			output.GetSpan(1)[0] = 30;
			output.Advance(1);
		}
	}
}
