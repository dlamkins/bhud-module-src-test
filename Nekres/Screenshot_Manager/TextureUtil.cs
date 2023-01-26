using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Security;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Graphics;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.Xna.Framework.Graphics;

namespace Nekres.Screenshot_Manager
{
	internal class TextureUtil
	{
		public static async Task<Texture2D> GetThumbnail(string filePath)
		{
			return await Task.Run((Func<Texture2D>)delegate
			{
				//IL_008f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0094: Unknown result type (might be due to invalid IL or missing references)
				DateTime dateTime = DateTime.UtcNow.AddMilliseconds(10000.0);
				while (DateTime.UtcNow < dateTime)
				{
					try
					{
						using ShellFile shellFile = ShellFile.FromFilePath(filePath);
						if (shellFile.Thumbnail == null)
						{
							throw new ShellException();
						}
						shellFile.Thumbnail.FormatOption = ShellThumbnailFormatOption.ThumbnailOnly;
						using Bitmap bitmap = shellFile.Thumbnail.ExtraLargeBitmap;
						using MemoryStream memoryStream = new MemoryStream();
						bitmap.Save(memoryStream, ImageFormat.Jpeg);
						byte[] array = new byte[memoryStream.Length];
						memoryStream.Position = 0L;
						memoryStream.Read(array, 0, array.Length);
						GraphicsDeviceContext val = GameService.Graphics.LendGraphicsDeviceContext();
						try
						{
							return Texture2D.FromStream(((GraphicsDeviceContext)(ref val)).get_GraphicsDevice(), (Stream)memoryStream);
						}
						finally
						{
							((GraphicsDeviceContext)(ref val)).Dispose();
						}
					}
					catch (Exception ex) when (ex is ShellException || ex is IOException || ex is UnauthorizedAccessException || ex is SecurityException || ex is InvalidOperationException)
					{
						if (!(DateTime.UtcNow < dateTime))
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
			return await Task.Run((Func<Texture2D>)delegate
			{
				//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
				DateTime dateTime = DateTime.UtcNow.AddMilliseconds(10000.0);
				while (DateTime.UtcNow < dateTime)
				{
					try
					{
						using FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
						using Image original = Image.FromStream(stream);
						using Bitmap bitmap = new Bitmap(original);
						using Graphics graphics = Graphics.FromImage(bitmap);
						graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
						graphics.SmoothingMode = SmoothingMode.HighSpeed;
						graphics.PixelOffsetMode = PixelOffsetMode.Default;
						graphics.CompositingQuality = CompositingQuality.Default;
						graphics.DrawImage(bitmap, 0, 0);
						using MemoryStream memoryStream = new MemoryStream();
						bitmap.Save(memoryStream, ImageFormat.Jpeg);
						byte[] array = new byte[memoryStream.Length];
						memoryStream.Position = 0L;
						memoryStream.Read(array, 0, array.Length);
						GraphicsDeviceContext val = GameService.Graphics.LendGraphicsDeviceContext();
						try
						{
							return Texture2D.FromStream(((GraphicsDeviceContext)(ref val)).get_GraphicsDevice(), (Stream)memoryStream);
						}
						finally
						{
							((GraphicsDeviceContext)(ref val)).Dispose();
						}
					}
					catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is SecurityException || ex is InvalidOperationException)
					{
						if (!(DateTime.UtcNow < dateTime))
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
