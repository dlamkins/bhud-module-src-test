using System.Buffers;

namespace Microsoft.AspNetCore.Internal
{
	internal static class TextMessageFormatter
	{
		public const byte RecordSeparator = 30;

		public static void WriteRecordSeparator(IBufferWriter<byte> output)
		{
			output.GetSpan(1)[0] = 30;
			output.Advance(1);
		}
	}
}
