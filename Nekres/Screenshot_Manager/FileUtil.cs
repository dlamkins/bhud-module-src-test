using System;
using System.IO;
using System.Threading.Tasks;

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
					catch (IOException ex)
					{
						if (!(DateTime.UtcNow < dateTime))
						{
							ScreenshotManagerModule.Logger.Error(ex.Message + ex.StackTrace);
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
					catch (IOException ex)
					{
						if (!(DateTime.UtcNow < dateTime))
						{
							ScreenshotManagerModule.Logger.Error(ex.Message + ex.StackTrace);
						}
					}
				}
				return false;
			});
		}
	}
}
