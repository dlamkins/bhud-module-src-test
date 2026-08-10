using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.TextureAtlases;
using Neokain.GW2.AllianceManager.Utils;

namespace Neokain.GW2.AllianceManager.Extensions
{
	internal static class SpriteFontExtensions
	{
		public static BitmapFont ToBitmapFont(this SpriteFont font, int lineHeight = 0)
		{
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Expected O, but got Unknown
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Expected O, but got Unknown
			if (lineHeight < 0)
			{
				throw new ArgumentException("Line height cannot be negative.", "lineHeight");
			}
			List<BitmapFontRegion> regions = new List<BitmapFontRegion>();
			foreach (Glyph value in font.GetGlyphs().Values)
			{
				Texture2D texture = font.get_Texture();
				Rectangle rectangle = value.BoundsInTexture;
				int left1 = ((Rectangle)(ref rectangle)).get_Left();
				rectangle = value.BoundsInTexture;
				int top1 = ((Rectangle)(ref rectangle)).get_Top();
				int width = value.BoundsInTexture.Width;
				int height = value.BoundsInTexture.Height;
				TextureRegion2D textureRegion2D = new TextureRegion2D(texture, left1, top1, width, height);
				int character = value.Character;
				rectangle = value.Cropping;
				int left2 = ((Rectangle)(ref rectangle)).get_Left();
				rectangle = value.Cropping;
				int top2 = ((Rectangle)(ref rectangle)).get_Top();
				int includingBearings = (int)value.WidthIncludingBearings;
				BitmapFontRegion bitmapFontRegion = new BitmapFontRegion(textureRegion2D, character, left2, top2, includingBearings);
				regions.Add(bitmapFontRegion);
			}
			return new BitmapFont($"{typeof(BitmapFont)}_{Guid.NewGuid():n}", regions, (lineHeight > 0) ? lineHeight : font.get_LineSpacing(), font.get_Texture());
		}
	}
}
