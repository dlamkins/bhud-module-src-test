using System.IO;

namespace LiteDB
{
	internal static class StreamExtensions
	{
		public static void FlushToDisk(this Stream stream)
		{
			FileStream fstream = stream as FileStream;
			if (fstream != null)
			{
				fstream.Flush(flushToDisk: true);
			}
			else
			{
				stream.Flush();
			}
		}
	}
}
