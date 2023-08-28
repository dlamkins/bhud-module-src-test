using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Kenedia.Modules.Core.Extensions
{
	public static class FileExtension
	{
		public static async Task<bool> WaitForFileUnlock(string path, int maxDuration = 2500, CancellationToken? cancellationToken = null)
		{
			cancellationToken.GetValueOrDefault();
			if (!cancellationToken.HasValue)
			{
				CancellationToken none = CancellationToken.None;
				cancellationToken = none;
			}
			bool locked = IsFileLocked(new FileInfo(path));
			for (int i = 0; i < maxDuration / 250; i++)
			{
				if (locked)
				{
					if (!cancellationToken.HasValue || !cancellationToken.GetValueOrDefault().IsCancellationRequested)
					{
						await Task.Delay(250, cancellationToken.Value);
						locked = IsFileLocked(new FileInfo(path));
					}
				}
			}
			int result;
			if (!locked)
			{
				result = ((!cancellationToken.HasValue || !cancellationToken.GetValueOrDefault().IsCancellationRequested) ? 1 : 0);
			}
			else
			{
				result = 0;
			}
			return (byte)result != 0;
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
