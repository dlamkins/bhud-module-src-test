using System;
using System.IO;
using Blish_HUD;
using Blish_HUD.Graphics;
using Blish_HUD.Modules.Managers;
using Microsoft.Xna.Framework.Graphics;
using SpriteFontPlus;

namespace Nekres.ChatMacros.Core
{
	internal static class ContentsManagerExtensions
	{
		internal static CharacterRange GeneralPunctuation = new CharacterRange('\u2000', '\u206f');

		internal static CharacterRange Arrows = new CharacterRange('←', '⇿');

		internal static CharacterRange MathematicalOperators = new CharacterRange('∀', '⋿');

		internal static CharacterRange BoxDrawing = new CharacterRange('─', '╰');

		internal static CharacterRange GeometricShapes = new CharacterRange('■', '◿');

		internal static CharacterRange MiscellaneousSymbols = new CharacterRange('☀', '⛿');

		public static readonly CharacterRange[] Gw2CharacterRange = new CharacterRange[9]
		{
			CharacterRange.BasicLatin,
			CharacterRange.Latin1Supplement,
			CharacterRange.LatinExtendedA,
			GeneralPunctuation,
			Arrows,
			MathematicalOperators,
			BoxDrawing,
			GeometricShapes,
			MiscellaneousSymbols
		};

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
					return TtfFontBaker.Bake(fontData, fontSize, textureSize, textureSize, Gw2CharacterRange).CreateSpriteFont(((GraphicsDeviceContext)(ref ctx)).get_GraphicsDevice());
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
			return manager.GetSpriteFont(fontPath, fontSize)?.ToBitmapFont(lineHeight);
		}
	}
}
