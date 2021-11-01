using System;
using System.IO;
using System.Threading.Tasks;
using Blish_HUD;

namespace Nekres.Stream_Out
{
	public static class FileUtil
	{
		private static readonly Logger Logger = Logger.GetLogger(typeof(FileUtil));

		public static async Task WriteAllTextAsync(string filePath, string data)
		{
			if (string.IsNullOrEmpty(filePath))
			{
				return;
			}
			if (data == null)
			{
				data = string.Empty;
			}
			try
			{
				using StreamWriter sw = new StreamWriter(filePath);
				await sw.WriteAsync(data);
			}
			catch (ArgumentException aEx)
			{
				Logger.Error(aEx.Message);
			}
			catch (UnauthorizedAccessException uaEx)
			{
				Logger.Error(uaEx.Message);
			}
			catch (IOException ioEx)
			{
				Logger.Error(ioEx.Message);
			}
		}
	}
}
