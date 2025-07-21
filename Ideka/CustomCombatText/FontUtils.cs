using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Graphics;
using Blish_HUD.Modules.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.TextureAtlases;
using SpriteFontPlus;

namespace Ideka.CustomCombatText
{
	internal static class FontUtils
	{
		internal static readonly CharacterRange[] Gw2CharacterRange = new CharacterRange[9]
		{
			CharacterRange.BasicLatin,
			CharacterRange.Latin1Supplement,
			CharacterRange.LatinExtendedA,
			new CharacterRange('₣', '₾'),
			new CharacterRange('←', '⇿'),
			new CharacterRange('∀', '⋿'),
			new CharacterRange('①', '⓿'),
			new CharacterRange('─', '╿'),
			new CharacterRange('■', '◿')
		};

		public static SpriteFont? GetSpriteFont(this ContentsManager manager, string fontPath, float fontSize, int textureSize = 1392)
		{
			using Stream file = manager.GetFileStream(fontPath);
			return GetSpriteFont(file, fontSize, textureSize);
		}

		public static SpriteFont? GetSpriteFont(string fontPath, float fontSize, int textureSize = 1392)
		{
			using FileStream file = File.OpenRead(fontPath);
			return GetSpriteFont(file, fontSize, textureSize);
		}

		public static SpriteFont? GetSpriteFont(Stream file, float fontSize, int textureSize = 1392)
		{
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			if (fontSize <= 0f)
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
				return TtfFontBaker.Bake(fontData, fontSize, textureSize, textureSize, Gw2CharacterRange).CreateSpriteFont(((GraphicsDeviceContext)(ref ctx)).get_GraphicsDevice());
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

		public static Size2 MeasureStringFixed(this BitmapFont font, string str)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			if (!str.EndsWith(" "))
			{
				return font.MeasureString(str);
			}
			string trimmed = str.Trim(' ');
			int spaceCount = str.Length - trimmed.Length;
			Size2 size = font.MeasureString(trimmed);
			if (spaceCount == 0)
			{
				return size;
			}
			float spaceWidth = font.MeasureString(" ").Width;
			float aWidth = font.MeasureString("a").Width;
			spaceWidth = Math.Max(spaceWidth, font.MeasureString("a ").Width - aWidth);
			spaceWidth = Math.Max(spaceWidth, font.MeasureString(" a").Width - aWidth);
			size.Width += spaceWidth * (float)spaceCount;
			return size;
		}
	}
}
