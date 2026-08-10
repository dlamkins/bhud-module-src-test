using System.Buffers;

namespace System.IO
{
	internal static class TextWriterExtensions
	{
		public static void WritePartialString(this TextWriter writer, string value, int offset, int count)
		{
			if (offset == 0 && count == value.Length)
			{
				writer.Write(value);
				return;
			}
			ReadOnlySpan<char> readOnlySpan = value.AsSpan(offset, count);
			char[] array = ArrayPool<char>.Shared.Rent(readOnlySpan.Length);
			readOnlySpan.CopyTo(array);
			writer.Write(array, 0, readOnlySpan.Length);
			ArrayPool<char>.Shared.Return(array);
		}
	}
}
