using System;
using System.Diagnostics;
using Blish_HUD;
using Gw2Sharp.WebApi;

namespace FarmingTracker
{
	public class WikiService
	{
		public static void OpenWikiSearchInDefaultBrowser(string wikiSearchTerm)
		{
			string languageString = GetWikiLanguageString();
			string percentEscapedSearchTerm = Uri.EscapeDataString(wikiSearchTerm);
			OpenUrlInDefaultBrowser("https://wiki-" + languageString + ".guildwars2.com/index.php?&search=" + percentEscapedSearchTerm);
		}

		public static void OpenWikiIdQueryInDefaultBrowser(int apiId)
		{
			OpenUrlInDefaultBrowser($"https://wiki.guildwars2.com/wiki/Special:RunQuery/Search_by_id?title=Special%3ARunQuery%2FSearch_by_id&pfRunQueryFormName=Search+by+id&Search+by+id%5Bid%5D={apiId}&Search+by+id%5Bcontext%5D=&wpRunQuery=&pf_free_text=");
		}

		private static string GetWikiLanguageString()
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Expected I4, but got Unknown
			Locale value = GameService.Overlay.get_UserLocale().get_Value();
			return (int)value switch
			{
				1 => "es", 
				2 => "de", 
				3 => "fr", 
				_ => "en", 
			};
		}

		private static void OpenUrlInDefaultBrowser(string url)
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
	}
}
