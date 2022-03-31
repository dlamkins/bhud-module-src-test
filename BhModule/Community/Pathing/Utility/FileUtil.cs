using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace BhModule.Community.Pathing.Utility
{
	public static class FileUtil
	{
		private const string NEWLINE = "\r\n";

		public static async Task<byte[]> ReadAsync(string path)
		{
			using FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
			byte[] result = new byte[(int)stream.Length];
			int readIndex = 0;
			while (readIndex < result.Length)
			{
				int num = readIndex;
				readIndex = num + await stream.ReadAsync(result, readIndex, result.Length - readIndex);
			}
			return result;
		}

		public static async Task<string> ReadTextAsync(string path, Encoding encoding = null)
		{
			if (encoding == null)
			{
				encoding = Encoding.UTF8;
			}
			Encoding encoding2 = encoding;
			return encoding2.GetString(await ReadAsync(path));
		}

		public static async Task<string[]> ReadLinesAsync(string path, Encoding encoding = null)
		{
			return (await ReadTextAsync(path, encoding)).Split(new string[1] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
		}

		public static async Task WriteBytesAsync(string path, byte[] data)
		{
			using FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Write, 4096, FileOptions.Asynchronous);
			await fileStream.WriteAsync(data, 0, data.Length);
		}

		public static async Task WriteStreamAsync(string path, Stream stream)
		{
			using FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Write, 4096, FileOptions.Asynchronous);
			await stream.CopyToAsync(fileStream);
		}

		public static async Task WriteTextAsync(string path, string text, Encoding encoding = null)
		{
			if (encoding == null)
			{
				encoding = Encoding.UTF8;
			}
			await WriteBytesAsync(path, encoding.GetBytes(text));
		}

		public static async Task WriteLinesAsync(string path, IEnumerable<string> lines, Encoding encoding = null)
		{
			await WriteTextAsync(path, string.Join("\r\n", lines), encoding);
		}
	}
}
