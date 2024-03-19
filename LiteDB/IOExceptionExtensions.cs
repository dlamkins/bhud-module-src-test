using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace LiteDB
{
	internal static class IOExceptionExtensions
	{
		private const int ERROR_SHARING_VIOLATION = 32;

		private const int ERROR_LOCK_VIOLATION = 33;

		public static bool IsLocked(this IOException ex)
		{
			int errorCode = Marshal.GetHRForException(ex) & 0xFFFF;
			if (errorCode != 32)
			{
				return errorCode == 33;
			}
			return true;
		}

		public static void WaitIfLocked(this IOException ex, int timerInMilliseconds)
		{
			if (ex.IsLocked())
			{
				if (timerInMilliseconds > 0)
				{
					Task.Delay(timerInMilliseconds).Wait();
				}
				return;
			}
			throw ex;
		}
	}
}
