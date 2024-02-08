using System.Diagnostics;
using MysticCrafting.Models.Items;

namespace MysticCrafting.Module.Helpers
{
	public static class LinkHelper
	{
		public static void OpenUrl(string url)
		{
			Process process = new Process();
			process.StartInfo.UseShellExecute = true;
			process.StartInfo.FileName = url;
			process.Start();
		}

		public static void OpenWiki(MysticWikiLink wikiLink)
		{
			OpenUrl("https:" + wikiLink.WikiUrl);
		}

		public static void OpenGw2Bltc(int itemId)
		{
			OpenUrl($"https://www.gw2bltc.com/en/item/{itemId}");
		}
	}
}
