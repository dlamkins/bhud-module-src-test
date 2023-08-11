using System.IO;
using Blish_HUD;
using Blish_HUD.Extended;
using Gw2Sharp.WebApi;
using MonoGame.Extended.BitmapFonts;

namespace Nekres.ProofLogix.Core
{
	public static class AssetUtil
	{
		private const char ELLIPSIS = '…';

		private const char BRACKET_LEFT = '[';

		private const char BRACKET_RIGHT = ']';

		public static int GetId(string assetUri)
		{
			return int.Parse(Path.GetFileNameWithoutExtension(assetUri));
		}

		public static string GetItemDisplayName(string name, int quantity, bool brackets = true)
		{
			if (quantity == 1)
			{
				if (!brackets)
				{
					return name;
				}
				return $"{'['}{name}{']'}";
			}
			if (!brackets)
			{
				return $"{quantity} {name}";
			}
			return $"{'['}{quantity} {name}{']'}";
		}

		public static string Truncate(string text, int maxWidth, BitmapFont font)
		{
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			string result = text;
			for (int width = (int)font.MeasureString(result).Width; width > maxWidth; width = (int)font.MeasureString(result).Width)
			{
				result = result.Substring(0, result.Length - 1);
			}
			if (result.Length >= text.Length)
			{
				return result;
			}
			return result.TrimEnd() + "…";
		}

		public static string GetWikiLink(string wikiPage)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Invalid comparison between Unknown and I4
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Invalid comparison between Unknown and I4
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			Locale value = GameService.Overlay.get_UserLocale().get_Value();
			if ((int)value > 3)
			{
				if (value - 4 > 1)
				{
				}
				return "https://wiki.guildwars2.com/index.php?search=" + wikiPage;
			}
			return "https://wiki-" + GameService.Overlay.get_UserLocale().get_Value().Code() + ".guildwars2.com/wiki/" + wikiPage;
		}
	}
}
