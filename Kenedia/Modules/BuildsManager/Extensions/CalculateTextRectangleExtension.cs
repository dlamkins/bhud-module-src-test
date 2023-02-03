using Microsoft.Xna.Framework;
using MonoGame.Extended.BitmapFonts;

namespace Kenedia.Modules.BuildsManager.Extensions
{
	public static class CalculateTextRectangleExtension
	{
		public static Rectangle CalculateTextRectangle(this Rectangle rect, string text, BitmapFont font)
		{
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			int rows = 1;
			int width = 0;
			BitmapFontRegion placeholder = font.GetCharacterRegion(32);
			foreach (char c in text)
			{
				BitmapFontRegion region = font.GetCharacterRegion((int)c);
				if (region != null && width + region.get_Width() > rect.Width)
				{
					rows++;
					width = 0;
				}
				width += ((region != null) ? region.get_Width() : placeholder.get_Width());
			}
			return new Rectangle(((Rectangle)(ref rect)).get_Location(), new Point(rect.Width, rows * font.get_LineHeight()));
		}

		public static Rectangle CalculateTextRectangle(this BitmapFont font, string text, Rectangle rect)
		{
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			int rows = 1;
			int width = 0;
			BitmapFontRegion placeholder = font.GetCharacterRegion(32);
			foreach (char c in text)
			{
				if (c == '\n')
				{
					rows++;
					width = 0;
					continue;
				}
				BitmapFontRegion region = font.GetCharacterRegion((int)c);
				int cWidth = ((region != null) ? region.get_Width() : placeholder.get_Width());
				if (width + cWidth > rect.Width)
				{
					rows++;
					width = 0;
				}
				width += cWidth;
			}
			return new Rectangle(((Rectangle)(ref rect)).get_Location(), new Point(rect.Width, rows * font.get_LineHeight()));
		}
	}
}
