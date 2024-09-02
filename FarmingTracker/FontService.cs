using System;
using System.Collections.Generic;
using Blish_HUD;
using MonoGame.Extended.BitmapFonts;

namespace FarmingTracker
{
	public class FontService
	{
		public Dictionary<FontSize, BitmapFont> Fonts = new Dictionary<FontSize, BitmapFont>();

		public FontService()
		{
			CreateFontSizeDict();
		}

		private void CreateFontSizeDict()
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			FontSize[] array = (FontSize[])Enum.GetValues(typeof(FontSize));
			foreach (FontSize fontSize in array)
			{
				Fonts[fontSize] = GameService.Content.GetFont((FontFace)0, fontSize, (FontStyle)0);
			}
		}
	}
}
