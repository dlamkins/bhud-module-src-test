using System;
using System.IO;
using System.Threading.Tasks;
using Blish_HUD.Graphics;
using Blish_HUD.Modules.Managers;
using Microsoft.Xna.Framework.Graphics;
using SpriteFontPlus;

namespace Blish_HUD.Extended
{
	public static class ContentsManagerExtensions
	{
		private static Logger Logger = Logger.GetLogger<ContentsManager>();

		internal static CharacterRange GeneralPunctuation = new CharacterRange('\u2000', '\u206f');

		internal static CharacterRange Arrows = new CharacterRange('←', '⇿');

		internal static CharacterRange MathematicalOperators = new CharacterRange('∀', '⋿');

		internal static CharacterRange BoxDrawing = new CharacterRange('─', '╰');

		internal static CharacterRange GeometricShapes = new CharacterRange('■', '◿');

		internal static CharacterRange MiscellaneousSymbols = new CharacterRange('☀', '⛿');

		internal static readonly CharacterRange[] Gw2CharacterRange = new CharacterRange[9]
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

		public static async Task Extract(this ContentsManager contentsManager, string refFilePath, string outFilePath, bool overwrite = true)
		{
			if (string.IsNullOrEmpty(refFilePath))
			{
				throw new ArgumentException("refFilePath cannot be empty.", "refFilePath");
			}
			if (string.IsNullOrEmpty(outFilePath))
			{
				throw new ArgumentException("outFilePath cannot be empty.", "outFilePath");
			}
			if (!overwrite && File.Exists(outFilePath))
			{
				return;
			}
			try
			{
				Directory.CreateDirectory(Path.GetDirectoryName(outFilePath));
				using Stream stream = contentsManager.GetFileStream(refFilePath);
				if (stream == null)
				{
					throw new FileNotFoundException("File not found: '" + refFilePath + "'");
				}
				stream.Position = 0L;
				using FileStream file = File.Create(outFilePath);
				file.Position = 0L;
				await stream.CopyToAsync(file);
			}
			catch (IOException e)
			{
				Logger.Warn((Exception)e, e.Message);
			}
		}

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

		public static BitmapFontEx GetBitmapFont(this ContentsManager manager, string fontPath, int fontSize, int lineHeight = 0)
		{
			return manager.GetSpriteFont(fontPath, fontSize)?.ToBitmapFont(lineHeight);
		}
	}
}
