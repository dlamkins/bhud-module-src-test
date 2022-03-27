using System;
using System.IO;
using System.Security;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;

namespace Nekres.Inquest_Module
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
						_ = DateTime.UtcNow < dateTime;
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
						_ = DateTime.UtcNow < dateTime;
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
						_ = DateTime.UtcNow < dateTime;
					}
				}
				return false;
			});
		}
	}
}
