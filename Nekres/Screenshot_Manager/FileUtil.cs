using System;
using System.IO;
using System.Security;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;

namespace Nekres.Screenshot_Manager
{
	internal static class FileUtil
	{
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
					catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is SecurityException)
					{
						if (!(DateTime.UtcNow < dateTime))
						{
							ScreenshotManagerModule.Logger.Error(ex, ex.Message);
						}
					}
				}
				return false;
			});
		}

		public static async Task<bool> DeleteAsync(string filePath)
		{
			return await Task.Run(delegate
			{
				DateTime dateTime = DateTime.UtcNow.AddMilliseconds(10000.0);
				while (DateTime.UtcNow < dateTime)
				{
					try
					{
						File.Delete(filePath);
						return true;
					}
					catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is SecurityException)
					{
						if (!(DateTime.UtcNow < dateTime))
						{
							ScreenshotManagerModule.Logger.Error(ex, ex.Message);
						}
					}
				}
				return false;
			});
		}

		public static async Task<bool> SendToRecycleBinAsync(string filePath)
		{
			return await Task.Run(delegate
			{
				DateTime dateTime = DateTime.UtcNow.AddMilliseconds(10000.0);
				while (DateTime.UtcNow < dateTime)
				{
					try
					{
						FileSystem.DeleteFile(filePath, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin, UICancelOption.DoNothing);
						return true;
					}
					catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is SecurityException)
					{
						if (!(DateTime.UtcNow < dateTime))
						{
							ScreenshotManagerModule.Logger.Error(ex, ex.Message);
						}
					}
				}
				return false;
			});
		}
	}
}
