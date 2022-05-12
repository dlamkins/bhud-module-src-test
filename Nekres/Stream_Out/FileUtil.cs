using System;
using System.IO;
using System.Threading.Tasks;
using Blish_HUD;

namespace Nekres.Stream_Out
{
	internal static class FileUtil
	{
		private static readonly Logger Logger = Logger.GetLogger(typeof(FileUtil));

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
					catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
					{
						if (!(DateTime.UtcNow < dateTime))
						{
							StreamOutModule.Logger.Error(ex, ex.Message);
							break;
						}
					}
				}
				return false;
			});
		}

		public static async Task<bool> DeleteDirectoryAsync(string dirPath)
		{
			return await Task.Run(delegate
			{
				DateTime dateTime = DateTime.UtcNow.AddMilliseconds(10000.0);
				while (DateTime.UtcNow < dateTime)
				{
					try
					{
						Directory.Delete(dirPath, recursive: true);
						return true;
					}
					catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
					{
						if (!(DateTime.UtcNow < dateTime))
						{
							StreamOutModule.Logger.Error(ex, ex.Message);
							break;
						}
					}
				}
				return false;
			});
		}
	}
}
