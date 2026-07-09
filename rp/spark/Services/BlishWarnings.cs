using System;
using Blish_HUD.Debug;

namespace rp.spark.Services
{
	internal static class BlishWarnings
	{
		private const int HttpAccessDeniedHResult = -2147467259;

		public static void FileSaveBlocked(UnauthorizedAccessException exception, string path, string actionDescription)
		{
			if (exception != null)
			{
				TryNotify(delegate
				{
					Contingency.NotifyFileSaveAccessDenied(path ?? string.Empty, actionDescription ?? "save SPARK data", true);
				});
			}
		}

		public static void HttpBlocked(Exception exception, string actionDescription)
		{
			if (IsHttpBlocked(exception))
			{
				TryNotify(delegate
				{
					Contingency.NotifyHttpAccessDenied(actionDescription ?? "connect to the SPARK webserver");
				});
			}
		}

		public static bool IsHttpBlocked(Exception exception)
		{
			return HasHResult(exception, -2147467259);
		}

		private static void TryNotify(Action notify)
		{
			try
			{
				notify?.Invoke();
			}
			catch
			{
			}
		}

		private static bool HasHResult(Exception exception, int hResult)
		{
			while (exception != null)
			{
				if (exception.HResult == hResult)
				{
					return true;
				}
				exception = exception.InnerException;
			}
			return false;
		}
	}
}
