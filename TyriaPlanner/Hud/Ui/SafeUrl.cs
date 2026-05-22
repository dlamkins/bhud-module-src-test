using System;
using System.Diagnostics;
using Blish_HUD;

namespace TyriaPlanner.Hud.Ui
{
	public static class SafeUrl
	{
		public static bool IsAllowed(string url)
		{
			if (string.IsNullOrWhiteSpace(url))
			{
				return false;
			}
			if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
			{
				return false;
			}
			string s = uri.Scheme.ToLowerInvariant();
			switch (s)
			{
			default:
				return s == "mumble";
			case "http":
			case "https":
			case "discord":
				return true;
			}
		}

		public static void Open(string url)
		{
			if (!IsAllowed(url))
			{
				Logger.GetLogger(typeof(SafeUrl)).Warn("Refused to open non-whitelisted URL scheme: {0}", new object[1] { url });
				return;
			}
			try
			{
				Process.Start(new ProcessStartInfo
				{
					FileName = url,
					UseShellExecute = true
				});
			}
			catch (Exception ex)
			{
				Logger.GetLogger(typeof(SafeUrl)).Warn(ex, "Browser launch failed for {0}", new object[1] { url });
			}
		}
	}
}
