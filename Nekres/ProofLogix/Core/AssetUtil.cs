using System.IO;
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

		public static string GetItemDisplayName(string name, int quantity)
		{
			if (quantity == 1)
			{
				return $"{'['}{name}{']'}";
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
	}
}
