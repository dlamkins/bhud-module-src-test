using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Blish_HUD.Debug;

namespace Estreya.BlishHUD.EventTable.Utils
{
	public static class FileUtil
	{
		private static async Task<byte[]> ReadBytesAsync(Stream stream)
		{
			if (stream == null)
			{
				return null;
			}
			try
			{
				byte[] result = new byte[stream.Length];
				await stream.ReadAsync(result, 0, (int)stream.Length);
				return result;
			}
			catch (Exception ex)
			{
				Contingency.NotifyFileSaveAccessDenied("stream", ex?.Message ?? "Failed to read stream.", false);
			}
			return new byte[0];
		}

		public static async Task<byte[]> ReadBytesAsync(string path)
		{
			if (string.IsNullOrWhiteSpace(path) && !File.Exists(path))
			{
				return null;
			}
			try
			{
				using FileStream fileStream = new FileStream(path, FileMode.Open);
				byte[] result = new byte[fileStream.Length];
				await fileStream.ReadAsync(result, 0, (int)fileStream.Length);
				return result;
			}
			catch (Exception ex)
			{
				Contingency.NotifyFileSaveAccessDenied(path, ex?.Message ?? "Failed to read file.", false);
			}
			return new byte[0];
		}

		public static async Task<string> ReadStringAsync(string path)
		{
			string result;
			if (string.IsNullOrWhiteSpace(path) && !File.Exists(path))
			{
				result = null;
			}
			else
			{
				Encoding uTF = Encoding.UTF8;
				result = uTF.GetString(await ReadBytesAsync(path));
			}
			return result;
		}

		public static async Task<string> ReadStringAsync(Stream stream)
		{
			string result;
			if (stream == null)
			{
				result = null;
			}
			else
			{
				Encoding uTF = Encoding.UTF8;
				result = uTF.GetString(await ReadBytesAsync(stream));
			}
			return result;
		}

		public static async Task<string[]> ReadLinesAsync(string path)
		{
			if (string.IsNullOrWhiteSpace(path) && !File.Exists(path))
			{
				return null;
			}
			string text = await ReadStringAsync(path);
			return string.IsNullOrWhiteSpace(text) ? null : text.Split(new string[1] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
		}

		public static async Task WriteBytesAsync(string path, byte[] data)
		{
			if (string.IsNullOrWhiteSpace(path))
			{
				return;
			}
			try
			{
				using FileStream SourceStream = new FileStream(path, FileMode.Create);
				await SourceStream.WriteAsync(data, 0, data.Length);
			}
			catch (Exception ex)
			{
				Contingency.NotifyFileSaveAccessDenied(path, ex?.Message ?? "Failed to write file.", false);
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

		public static string CreateTempFile(string extension)
		{
			string tempFileName = Path.GetTempFileName();
			string tempFileNameWithExtension = Path.ChangeExtension(tempFileName, extension);
			File.Move(tempFileName, tempFileNameWithExtension);
			return tempFileNameWithExtension;
		}

		public static string SanitizeFileName(string fileName)
		{
			string invalidChars = Regex.Escape(new string(Path.GetInvalidFileNameChars()));
			string invalidRegStr = string.Format("([{0}]*\\.+$)|([{0}]+)", invalidChars);
			return Regex.Replace(fileName, invalidRegStr, "_");
		}
	}
}
