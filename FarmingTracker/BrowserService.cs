using System;
using System.Diagnostics;
using Blish_HUD;
using Gw2Sharp.WebApi;

namespace FarmingTracker
{
	public class BrowserService
	{
		public static void OpenUrlInDefaultBrowser(string url)
		{
			try
			{
				Process.Start(new ProcessStartInfo
				{
					FileName = url,
					UseShellExecute = true
				});
			}
			catch (Exception e)
			{
				Module.Logger.Error(e, "Failed to open url in default browser.");
			}
		}

		public static string GetGw2EfficiencyLanguageString()
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Expected I4, but got Unknown
			Locale value = GameService.Overlay.get_UserLocale().get_Value();
			return (value - 1) switch
			{
				0 => "es", 
				1 => "de", 
				2 => "fr", 
				_ => "en", 
			};
		}
	}
}
