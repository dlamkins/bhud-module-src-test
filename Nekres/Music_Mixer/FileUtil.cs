using System;
using System.IO;
using System.Security;
using System.Threading.Tasks;

namespace Nekres.Music_Mixer
{
	internal static class FileUtil
	{
		public static string Sanitize(string fileName, string replacement = " ")
		{
			string[] temp = fileName.Trim().Split(Path.GetInvalidFileNameChars());
			for (int i = 0; i < temp.Length; i++)
			{
				temp[i] = temp[i].Trim();
			}
			return string.Join(replacement, temp);
		}

		public static async Task<bool> DeleteAsync(string filePath)
		{
			return await Task.Run(delegate
			{
				DateTime dateTime = DateTime.UtcNow.AddMilliseconds(5000.0);
				while (DateTime.UtcNow < dateTime)
				{
					try
					{
						File.Delete(filePath);
						return true;
					}
					catch (Exception ex) when (((ex is IOException || ex is UnauthorizedAccessException || ex is SecurityException) ? 1 : 0) != 0)
					{
						if (!(DateTime.UtcNow < dateTime))
						{
							MusicMixer.Logger.Error(ex, ex.Message);
							break;
						}
					}
				}
				return false;
			});
		}

		public static bool IsFileLocked(string uri)
		{
			FileStream stream = null;
			try
			{
				stream = File.Open(uri, FileMode.Open, FileAccess.Read, FileShare.None);
			}
			catch (IOException ex) when ((ex.HResult & 0xFFFF) == 32)
			{
				return true;
			}
			catch (IOException ex2) when ((ex2.HResult & 0xFFFF) == 33)
			{
				return true;
			}
			catch (Exception)
			{
				return false;
			}
			finally
			{
				stream?.Dispose();
			}
			return false;
		}
	}
}
