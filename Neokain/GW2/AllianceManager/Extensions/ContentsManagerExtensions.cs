using System;
using System.IO;
using Blish_HUD;
using Blish_HUD.Graphics;
using Blish_HUD.Modules.Managers;
using Microsoft.Xna.Framework.Graphics;
using Neokain.GW2.AllianceManager.Utils;
using SpriteFontPlus;

namespace Neokain.GW2.AllianceManager.Extensions
{
	public static class ContentsManagerExtensions
	{
		public static CharacterRange GeneralPunctuation = new CharacterRange('\u2000', '\u206f');

		public static CharacterRange Arrows = new CharacterRange('←', '⇿');

		public static CharacterRange MathematicalOperators = new CharacterRange('∀', '⋿');

		public static CharacterRange BoxDrawing = new CharacterRange('─', '╰');

		public static CharacterRange GeometricShapes = new CharacterRange('■', '◿');

		public static CharacterRange MiscellaneousSymbols = new CharacterRange('☀', '⛿');

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
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			if (fontSize <= 0)
			{
				throw new ArgumentException("Font size must be greater than 0.", "fontSize");
			}
			using Stream fileStream = manager.GetFileStream(fontPath);
			byte[] numArray = new byte[fileStream.Length];
			if (fileStream.Read(numArray, 0, numArray.Length) <= 0)
			{
				return null;
			}
			GraphicsDeviceContext graphicsDeviceContext = GameService.Graphics.LendGraphicsDeviceContext();
			try
			{
				return TtfFontBaker.Bake(numArray, fontSize, textureSize, textureSize, Gw2CharacterRange).CreateSpriteFont(((GraphicsDeviceContext)(ref graphicsDeviceContext)).get_GraphicsDevice());
			}
			finally
			{
				((GraphicsDeviceContext)(ref graphicsDeviceContext)).Dispose();
			}
		}

		public static BitmapFont GetBitmapFont(this ContentsManager manager, string fontPath, int fontSize, int lineHeight = 0)
		{
			return manager.GetSpriteFont(fontPath, fontSize)?.ToBitmapFont(lineHeight);
		}
	}
}
