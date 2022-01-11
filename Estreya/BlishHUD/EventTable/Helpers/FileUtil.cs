using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Estreya.BlishHUD.EventTable.Helpers
{
	internal class FileUtil
	{
		public static async Task<byte[]> ReadBytesAsync(string path)
		{
			if (string.IsNullOrWhiteSpace(path) && !File.Exists(path))
			{
				return null;
			}
			byte[] result;
			using (FileStream SourceStream = File.Open(path, FileMode.Open))
			{
				result = new byte[SourceStream.Length];
				await SourceStream.ReadAsync(result, 0, (int)SourceStream.Length);
			}
			return result;
		}

		public static async Task<string> ReadStringAsync(string path)
		{
			if (string.IsNullOrWhiteSpace(path) && !File.Exists(path))
			{
				return null;
			}
			Encoding uTF = Encoding.UTF8;
			return uTF.GetString(await ReadBytesAsync(path));
		}

		public static async Task<string[]> ReadLinesAsync(string path)
		{
			if (string.IsNullOrWhiteSpace(path) && !File.Exists(path))
			{
				return null;
			}
			string text = await ReadStringAsync(path);
			if (string.IsNullOrWhiteSpace(text))
			{
				return null;
			}
			return text.Split(new string[1] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
		}

		public static async Task WriteBytesAsync(string path, byte[] data)
		{
			if (!string.IsNullOrWhiteSpace(path))
			{
				using FileStream SourceStream = File.Open(path, FileMode.Create);
				await SourceStream.WriteAsync(data, 0, data.Length);
			}
		}

		public static async Task WriteStringAsync(string path, string data)
		{
			if (!string.IsNullOrWhiteSpace(path))
			{
				byte[] byteData = Encoding.UTF8.GetBytes(data);
				await WriteBytesAsync(path, byteData);
			}
		}

		public static async Task WriteLinesAsync(string path, string[] data)
		{
			if (!string.IsNullOrWhiteSpace(path))
			{
				string stringData = string.Join("\r\n", data);
				await WriteStringAsync(path, stringData);
			}
		}
	}
}
