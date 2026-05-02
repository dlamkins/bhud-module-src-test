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
				Logger.GetLogger<ContentsManager>().Warn((Exception)e, e.Message);
			}
		}

		public static SpriteFont GetSpriteFont(this ContentsManager contentsManager, string fontPath, int fontSize, Gw2FontRanges ranges = Gw2FontRanges.Default, int textureSize = 1392)
		{
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			if (fontSize <= 0)
			{
				throw new ArgumentException("Font size must be greater than 0.", "fontSize");
			}
			if (textureSize <= 0)
			{
				throw new ArgumentException("Texture size must be greater than 0.", "textureSize");
			}
			using Stream fontStream = contentsManager.GetFileStream(fontPath);
			byte[] fontData = new byte[fontStream.Length];
			if (fontStream.Read(fontData, 0, fontData.Length) > 0)
			{
				GraphicsDeviceContext ctx = GameService.Graphics.LendGraphicsDeviceContext();
				try
				{
					SpriteFont result = TtfFontBaker.Bake(fontData, fontSize, textureSize, textureSize, FontUtil.GetRanges(ranges)).CreateSpriteFont(((GraphicsDeviceContext)(ref ctx)).get_GraphicsDevice());
					Logger.GetLogger<ContentsManager>().Debug("Successfully loaded font {dataReaderFilePath}.", new object[1] { fontPath });
					return result;
				}
				catch (Exception e)
				{
					Logger.GetLogger<ContentsManager>().Warn(e, "Unable to load font {dataReaderFilePath}.", new object[1] { fontPath });
				}
				finally
				{
					((GraphicsDeviceContext)(ref ctx)).Dispose();
				}
			}
			return null;
		}

		public static BitmapFontEx GetBitmapFont(this ContentsManager contentsManager, string fontPath, int fontSize, Gw2FontRanges ranges = Gw2FontRanges.Default, int lineHeight = 0, int textureSize = 1392)
		{
			return contentsManager.GetSpriteFont(fontPath, fontSize, Gw2FontRanges.Default, textureSize)?.ToBitmapFont(lineHeight);
		}
	}
}
