using System.IO;
using Blish_HUD;
using Blish_HUD.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpriteFontPlus;

namespace Estreya.BlishHUD.Shared.Utils
{
	public static class FontUtils
	{
		public static SpriteFont FromTrueTypeFont(byte[] ttfData, float fontSize, int bitmapWidth, int bitmapHeight)
		{
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			TtfFontBakerResult fontBakeResult = TtfFontBaker.Bake(ttfData, fontSize, bitmapWidth, bitmapHeight, new CharacterRange[4]
			{
				CharacterRange.BasicLatin,
				CharacterRange.Latin1Supplement,
				CharacterRange.LatinExtendedA,
				CharacterRange.LatinExtendedB
			});
			GraphicsDeviceContext ctx = GameService.Graphics.LendGraphicsDeviceContext();
			try
			{
				return fontBakeResult.CreateSpriteFont(((GraphicsDeviceContext)(ref ctx)).get_GraphicsDevice());
			}
			finally
			{
				((GraphicsDeviceContext)(ref ctx)).Dispose();
			}
		}

		public static SpriteFont FromBMFont(string fontPath)
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			string fontDirectory = Path.GetFullPath(fontPath);
			using Stream stream = TitleContainer.OpenStream(fontPath);
			using StreamReader reader = new StreamReader(stream);
			string fontData = reader.ReadToEnd();
			GraphicsDeviceContext ctx = GameService.Graphics.LendGraphicsDeviceContext();
			try
			{
				return BMFontLoader.Load(fontData, (string name) => TitleContainer.OpenStream(Path.Combine(fontDirectory, name)), ((GraphicsDeviceContext)(ref ctx)).get_GraphicsDevice());
			}
			finally
			{
				((GraphicsDeviceContext)(ref ctx)).Dispose();
			}
		}
	}
}
