using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Security;
using System.Threading.Tasks;
using Blish_HUD;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.Xna.Framework.Graphics;

namespace Nekres.Screenshot_Manager
{
	internal class TextureUtil
	{
		public static async Task<Texture2D> GetThumbnail(string filePath)
		{
			return await Task.Run<Texture2D>(async delegate
			{
				DateTime timeout = DateTime.UtcNow.AddMilliseconds(10000.0);
				while (DateTime.UtcNow < timeout)
				{
					try
					{
						using ShellFile shellFile = ShellFile.FromFilePath(filePath);
						if (shellFile.Thumbnail == null)
						{
							throw new ShellException();
						}
						shellFile.Thumbnail.FormatOption = ShellThumbnailFormatOption.ThumbnailOnly;
						using Bitmap shellThumb = shellFile.Thumbnail.ExtraLargeBitmap;
						using MemoryStream textureStream = new MemoryStream();
						shellThumb.Save(textureStream, ImageFormat.Jpeg);
						byte[] buffer = new byte[textureStream.Length];
						textureStream.Position = 0L;
						await textureStream.ReadAsync(buffer, 0, buffer.Length);
						return Texture2D.FromStream(GameService.Graphics.get_GraphicsDevice(), (Stream)textureStream);
					}
					catch (Exception ex) when (ex is ShellException || ex is IOException || ex is UnauthorizedAccessException || ex is SecurityException || ex is InvalidOperationException)
					{
						if (!(DateTime.UtcNow < timeout))
						{
							ScreenshotManagerModule.Logger.Error(ex, ex.Message);
							return Textures.get_Pixel();
						}
					}
				}
				return Textures.get_Pixel();
			});
		}

		public static async Task<Texture2D> GetScreenShot(string filePath)
		{
			return await Task.Run<Texture2D>(async delegate
			{
				DateTime timeout = DateTime.UtcNow.AddMilliseconds(10000.0);
				while (DateTime.UtcNow < timeout)
				{
					try
					{
						using FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
						using Image source = Image.FromStream(fs);
						using Bitmap target = new Bitmap(source);
						using Graphics graphic = Graphics.FromImage(target);
						graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
						graphic.SmoothingMode = SmoothingMode.HighSpeed;
						graphic.PixelOffsetMode = PixelOffsetMode.Default;
						graphic.CompositingQuality = CompositingQuality.Default;
						graphic.DrawImage(target, 0, 0);
						using MemoryStream textureStream = new MemoryStream();
						target.Save(textureStream, ImageFormat.Jpeg);
						byte[] buffer = new byte[textureStream.Length];
						textureStream.Position = 0L;
						await textureStream.ReadAsync(buffer, 0, buffer.Length);
						return Texture2D.FromStream(GameService.Graphics.get_GraphicsDevice(), (Stream)textureStream);
					}
					catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is SecurityException || ex is InvalidOperationException)
					{
						if (!(DateTime.UtcNow < timeout))
						{
							ScreenshotManagerModule.Logger.Error(ex, ex.Message);
							return Textures.get_Pixel();
						}
					}
				}
				return Textures.get_Pixel();
			});
		}
	}
}
