using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.TextureAtlases;

namespace Estreya.BlishHUD.Shared.Extensions
{
	public static class FontExtensions
	{
		public static BitmapFont ToBitmapFont(this SpriteFont spriteFont)
		{
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Expected O, but got Unknown
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Expected O, but got Unknown
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Expected O, but got Unknown
			Texture2D texture = spriteFont.get_Texture();
			List<BitmapFontRegion> regions = new List<BitmapFontRegion>();
			foreach (Glyph glyph in spriteFont.GetGlyphs().Values)
			{
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
			return new BitmapFont(Guid.NewGuid().ToString(), (IEnumerable<BitmapFontRegion>)regions, spriteFont.get_LineSpacing());
		}
	}
}
