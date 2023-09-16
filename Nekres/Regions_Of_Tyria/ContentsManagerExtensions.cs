using System;
using System.IO;
using Blish_HUD;
using Blish_HUD.Graphics;
using Blish_HUD.Modules.Managers;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using SpriteFontPlus;

namespace Nekres.Regions_Of_Tyria
{
	internal static class ContentsManagerExtensions
	{
		public static SpriteFont GetSpriteFont(this ContentsManager manager, string fontPath, int fontSize, int textureSize = 1392)
		{
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			if (fontSize <= 0)
			{
				throw new ArgumentException("Font size must be greater than 0.", "fontSize");
			}
			using Stream fontStream = manager.GetFileStream(fontPath);
			byte[] fontData = new byte[fontStream.Length];
			if (fontStream.Read(fontData, 0, fontData.Length) > 0)
			{
				GraphicsDeviceContext ctx = GameService.Graphics.LendGraphicsDeviceContext();
				try
				{
					return TtfFontBaker.Bake(fontData, fontSize, textureSize, textureSize, new CharacterRange[3]
					{
						CharacterRange.BasicLatin,
						CharacterRange.Latin1Supplement,
						CharacterRange.LatinExtendedA
					}).CreateSpriteFont(((GraphicsDeviceContext)(ref ctx)).get_GraphicsDevice());
				}
				finally
				{
					((GraphicsDeviceContext)(ref ctx)).Dispose();
				}
			}
			return null;
		}

		public static BitmapFont GetBitmapFont(this ContentsManager manager, string fontPath, int fontSize, int lineHeight = 0)
		{
			if (lineHeight < 0)
			{
				throw new ArgumentException("Line height cannot be negative.", "lineHeight");
			}
			return manager.GetSpriteFont(fontPath, fontSize)?.ToBitmapFont(lineHeight);
		}
	}
}
