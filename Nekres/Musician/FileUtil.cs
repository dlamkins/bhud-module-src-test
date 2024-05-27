using System;
using System.IO;
using System.Security;
using System.Threading.Tasks;
using Blish_HUD;
using Microsoft.VisualBasic.FileIO;

namespace Nekres.Musician
{
	internal static class FileUtil
	{
		private static readonly Logger Logger = Logger.GetLogger(typeof(FileUtil));

		public const int FileTimeOutMilliseconds = 10000;

		public static async Task WriteAllTextAsync(string filePath, string data, bool overwrite = true)
		{
			if (string.IsNullOrEmpty(filePath) || (!overwrite && File.Exists(filePath)))
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

		public static async Task<bool> DeleteAsync(string filePath, bool sendToRecycleBin = true)
		{
			return await Task.Run(delegate
			{
				DateTime dateTime = DateTime.UtcNow.AddMilliseconds(10000.0);
				while (DateTime.UtcNow < dateTime)
				{
					try
					{
						if (sendToRecycleBin)
						{
							FileSystem.DeleteFile(filePath, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin, UICancelOption.DoNothing);
						}
						else
						{
							File.Delete(filePath);
						}
						return true;
					}
					catch (Exception ex) when (((ex is IOException || ex is UnauthorizedAccessException || ex is SecurityException) ? 1 : 0) != 0)
					{
						if (!(DateTime.UtcNow < dateTime))
						{
							Logger.Error(ex, ex.Message);
							break;
						}
					}
				}
				return false;
			});
		}
	}
}
