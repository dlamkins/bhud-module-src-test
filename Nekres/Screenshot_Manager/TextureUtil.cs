using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using Blish_HUD;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.Xna.Framework;
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
				//IL_004b: Unknown result type (might be due to invalid IL or missing references)
				//IL_005f: Unknown result type (might be due to invalid IL or missing references)
				//IL_007a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0084: Unknown result type (might be due to invalid IL or missing references)
				//IL_0089: Unknown result type (might be due to invalid IL or missing references)
				DateTime dateTime = DateTime.UtcNow.AddMilliseconds(10000.0);
				int num3 = default(int);
				int num4 = default(int);
				while (DateTime.UtcNow < dateTime)
				{
					if (!File.Exists(filePath))
					{
						return Textures.get_Pixel();
					}
					try
					{
						using FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
						using Image image = Image.FromStream(stream);
						int num = GameService.Graphics.get_Resolution().X - 100;
						int num2 = GameService.Graphics.get_Resolution().Y - 100;
						Point val = PointExtensions.ResizeKeepAspect(new Point(image.Width, image.Height), num, num2, false);
						((Point)(ref val)).Deconstruct(ref num3, ref num4);
						int width = num3;
						int height = num4;
						using Bitmap bitmap = new Bitmap(image, width, height);
						using Graphics graphics = Graphics.FromImage(bitmap);
						graphics.CompositingQuality = CompositingQuality.HighSpeed;
						graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
						graphics.SmoothingMode = SmoothingMode.HighSpeed;
						graphics.DrawImage(bitmap, 0, 0, width, height);
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
