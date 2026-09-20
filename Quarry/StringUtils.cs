using Blish_HUD;
using MonoGame.Extended.BitmapFonts;

namespace Quarry
{
	public static class StringUtils
	{
		public static string TrimNameToWidth(string name, int maxWidth)
		{
			return TrimNameToWidth(name, maxWidth, GameService.Content.get_DefaultFont14());
		}

		public static string TrimNameToWidth(string name, int maxWidth, BitmapFont font)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			if (font.MeasureString(name).Width <= (float)maxWidth)
			{
				return name;
			}
			for (int length = name.Length - 1; length > 0; length--)
			{
				string candidate = name.Substring(0, length) + "…";
				if (font.MeasureString(candidate).Width <= (float)maxWidth)
				{
					return candidate;
				}
			}
			return "…";
		}
	}
}
