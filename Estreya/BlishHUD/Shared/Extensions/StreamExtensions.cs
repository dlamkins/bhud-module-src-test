using System;
using System.IO;

namespace Estreya.BlishHUD.Shared.Extensions
{
	public static class StreamExtensions
	{
		public static byte[] ToByteArray(this Stream input)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			MemoryStream memStream = input as MemoryStream;
			if (memStream != null)
			{
				return memStream.ToArray();
			}
			byte[] buffer = new byte[16384];
			using MemoryStream ms = new MemoryStream();
			int read;
			while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
			{
				ms.Write(buffer, 0, read);
			}
			return ms.ToArray();
		}
	}
}
