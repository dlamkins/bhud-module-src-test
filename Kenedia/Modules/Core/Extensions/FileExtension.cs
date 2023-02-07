using System.IO;
using System.Threading.Tasks;

namespace Kenedia.Modules.Core.Extensions
{
	public static class FileExtension
	{
		public static async Task<bool> WaitForFileUnlock(string path, int maxDuration = 2500)
		{
			bool locked = IsFileLocked(new FileInfo(path));
			for (int i = 0; i < maxDuration / 250; i++)
			{
				if (locked)
				{
					await Task.Delay(250);
					locked = IsFileLocked(new FileInfo(path));
				}
			}
			return !locked;
		}

		public static bool IsFileLocked(FileInfo file)
		{
			if (!File.Exists(file.FullName))
			{
				return false;
			}
			try
			{
				using FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
				stream.Close();
			}
			catch (IOException)
			{
				return true;
			}
			return false;
		}
	}
}
