using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
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
			return await Task.Run(delegate
			{
				using ShellFile shellFile = ShellFile.FromFilePath(filePath);
				using Bitmap bitmap = shellFile.Thumbnail.ExtraLargeBitmap;
				using MemoryStream memoryStream = new MemoryStream();
				bitmap.Save(memoryStream, ImageFormat.Jpeg);
				byte[] array = new byte[memoryStream.Length];
				memoryStream.Position = 0L;
				memoryStream.Read(array, 0, array.Length);
				return Texture2D.FromStream(GameService.Graphics.get_GraphicsDevice(), (Stream)memoryStream);
			});
		}

		public static async Task<Texture2D> GetScreenShot(string filePath)
		{
			return await Task.Run((Func<Texture2D>)delegate
			{
				DateTime dateTime = DateTime.UtcNow.AddMilliseconds(10000.0);
				while (DateTime.UtcNow < dateTime)
				{
					if (!File.Exists(filePath))
					{
						return Textures.get_Pixel();
					}
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
						return Texture2D.FromStream(GameService.Graphics.get_GraphicsDevice(), (Stream)memoryStream);
					}
					catch (IOException ex)
					{
						if (!(DateTime.UtcNow < dateTime))
						{
							ScreenshotManagerModule.Logger.Error(ex.Message);
							return Textures.get_Pixel();
						}
					}
				}
				return Textures.get_Pixel();
			});
		}
	}
}
