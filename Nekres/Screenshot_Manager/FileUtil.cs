using System;
using System.IO;
using System.Security;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;

namespace Nekres.Screenshot_Manager
{
	internal static class FileUtil
	{
		public static string IndexedFilename(string stub, string extension)
		{
			int ix = 1;
			string filename;
			do
			{
				string indexStr = ix.ToString("D3");
				filename = stub + indexStr + "." + extension;
				ix++;
			}
			while (File.Exists(filename));
			return filename;
		}

		public static async Task<bool> MoveAsync(string oldFilePath, string newFilePath)
		{
			return await Task.Run(delegate
			{
				DateTime dateTime = DateTime.UtcNow.AddMilliseconds(10000.0);
				while (DateTime.UtcNow < dateTime)
				{
					try
					{
						File.Move(oldFilePath, newFilePath);
						return true;
					}
					catch (Exception ex) when (((ex is IOException || ex is UnauthorizedAccessException || ex is SecurityException) ? 1 : 0) != 0)
					{
						if (!(DateTime.UtcNow < dateTime))
						{
							ScreenshotManagerModule.Logger.Error(ex, ex.Message);
							break;
						}
					}
				}
				return false;
			});
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
							ScreenshotManagerModule.Logger.Error(ex, ex.Message);
							break;
						}
					}
				}
				return false;
			});
		}
	}
}
