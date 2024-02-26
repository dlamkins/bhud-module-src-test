using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.TextureAtlases;

namespace Blish_HUD.Extended
{
	internal static class SpriteFontExtensions
	{
		public static BitmapFontEx ToBitmapFont(this SpriteFont font, int lineHeight = 0)
		{
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Expected O, but got Unknown
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Expected O, but got Unknown
			if (lineHeight < 0)
			{
				throw new ArgumentException("Line height cannot be negative.", "lineHeight");
			}
			List<BitmapFontRegion> regions = new List<BitmapFontRegion>();
			foreach (Glyph glyph in font.GetGlyphs().Values)
			{
				Texture2D texture = font.get_Texture();
				Rectangle val = glyph.BoundsInTexture;
				int left = ((Rectangle)(ref val)).get_Left();
				val = glyph.BoundsInTexture;
				TextureRegion2D val2 = new TextureRegion2D(texture, left, ((Rectangle)(ref val)).get_Top(), glyph.BoundsInTexture.Width, glyph.BoundsInTexture.Height);
				char character = glyph.Character;
				val = glyph.Cropping;
				int left2 = ((Rectangle)(ref val)).get_Left();
				val = glyph.Cropping;
				BitmapFontRegion region = new BitmapFontRegion(val2, (int)character, left2, ((Rectangle)(ref val)).get_Top(), (int)glyph.WidthIncludingBearings);
				regions.Add(region);
			}
			return new BitmapFontEx($"{typeof(BitmapFont)}_{Guid.NewGuid():n}", regions, (lineHeight > 0) ? lineHeight : font.get_LineSpacing(), font.get_Texture());
		}
	}
}
