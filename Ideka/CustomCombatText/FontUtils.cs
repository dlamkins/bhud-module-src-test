using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Graphics;
using Blish_HUD.Modules.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.TextureAtlases;
using SpriteFontPlus;

namespace Ideka.CustomCombatText
{
	internal static class FontUtils
	{
		public static SpriteFont? GetSpriteFont(this ContentsManager manager, string fontPath, int fontSize, int textureSize = 1392)
		{
			using Stream file = manager.GetFileStream(fontPath);
			return GetSpriteFont(file, fontSize, textureSize);
		}

		public static SpriteFont? GetSpriteFont(string fontPath, int fontSize, int textureSize = 1392)
		{
			using FileStream file = File.OpenRead(fontPath);
			return GetSpriteFont(file, fontSize, textureSize);
		}

		public static SpriteFont? GetSpriteFont(Stream file, int fontSize, int textureSize = 1392)
		{
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			if (fontSize <= 0)
			{
				throw new ArgumentException("Font size must be greater than 0.", "fontSize");
			}
			byte[] fontData = new byte[file.Length];
			if (file.Read(fontData, 0, fontData.Length) <= 0)
			{
				return null;
			}
			GraphicsDeviceContext ctx = GameService.Graphics.LendGraphicsDeviceContext();
			try
			{
				return TtfFontBaker.Bake(fontData, fontSize, textureSize, textureSize, new _003C_003Ez__ReadOnlyArray<CharacterRange>(new CharacterRange[3]
				{
					CharacterRange.BasicLatin,
					CharacterRange.Latin1Supplement,
					CharacterRange.LatinExtendedA
				})).CreateSpriteFont(((GraphicsDeviceContext)(ref ctx)).get_GraphicsDevice());
			}
			finally
			{
				((GraphicsDeviceContext)(ref ctx)).Dispose();
			}
		}

		public static BitmapFont? GetBitmapFont(this ContentsManager manager, string fontPath, int fontSize, int lineHeight = 0)
		{
			return manager.GetSpriteFont(fontPath, fontSize)?.ToBitmapFont(lineHeight);
		}

		public static BitmapFont ToBitmapFont(this SpriteFont font, int lineHeight = 0)
		{
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Expected O, but got Unknown
			SpriteFont font2 = font;
			if (lineHeight >= 0)
			{
				return new BitmapFont($"{typeof(BitmapFont)}_{Guid.NewGuid():n}", ((IEnumerable<Glyph>)font2.GetGlyphs().Values).Select((Func<Glyph, BitmapFontRegion>)((Glyph glyph) => new BitmapFontRegion(new TextureRegion2D(font2.get_Texture(), ((Rectangle)(ref glyph.BoundsInTexture)).get_Left(), ((Rectangle)(ref glyph.BoundsInTexture)).get_Top(), glyph.BoundsInTexture.Width, glyph.BoundsInTexture.Height), (int)glyph.Character, ((Rectangle)(ref glyph.Cropping)).get_Left(), ((Rectangle)(ref glyph.Cropping)).get_Top(), (int)glyph.WidthIncludingBearings))), (lineHeight > 0) ? lineHeight : font2.get_LineSpacing());
			}
			throw new ArgumentException("Line height cannot be negative.", "lineHeight");
		}
	}
}
