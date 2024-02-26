using System;
using System.IO;

namespace Nekres.FailScreens.Core
{
	internal static class FileUtil
	{
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
