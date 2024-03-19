using System.Diagnostics;
using System.Runtime.InteropServices;
using Blish_HUD;

namespace DanceDanceRotationModule.Util
{
	public class UrlHelper
	{
		private static readonly Logger Logger = Logger.GetLogger<UrlHelper>();

		public static void OpenUrl(string url)
		{
			Logger.Info("Opening URL: " + url);
			try
			{
				Process.Start(url);
			}
			catch
			{
				if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
				{
					url = url.Replace("&", "^&");
					Process.Start(new ProcessStartInfo(url)
					{
						UseShellExecute = true
					});
					return;
				}
				if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
				{
					Process.Start("xdg-open", url);
					return;
				}
				if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
				{
					Process.Start("open", url);
					return;
				}
				throw;
			}
		}
	}
}
