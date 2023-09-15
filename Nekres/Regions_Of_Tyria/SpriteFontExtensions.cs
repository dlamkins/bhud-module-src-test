using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.TextureAtlases;

namespace Nekres.Regions_Of_Tyria
{
	internal static class SpriteFontExtensions
	{
		public static BitmapFont ToBitmapFont(this SpriteFont font, int lineHeight = 0)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Expected O, but got Unknown
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Expected O, but got Unknown
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Expected O, but got Unknown
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
			return new BitmapFont($"{typeof(BitmapFont)}_{Guid.NewGuid():n}", (IEnumerable<BitmapFontRegion>)regions, (lineHeight > 0) ? lineHeight : font.get_LineSpacing());
		}
	}
}
