using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Blish_HUD;
using Blish_HUD.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.Core.Extensions
{
	public static class Texture2DExtension
	{
		public static Texture2D CreateTexture2D(this MemoryStream s)
		{
			using GraphicsDeviceContext device = GameService.Graphics.LendGraphicsDeviceContext();
			return Texture2D.FromStream(device.GraphicsDevice, (Stream)s);
		}

		public static Texture2D CreateTexture2D(this Bitmap bitmap)
		{
			MemoryStream s = new MemoryStream();
			bitmap.Save(s, ImageFormat.Png);
			using GraphicsDeviceContext device = GameService.Graphics.LendGraphicsDeviceContext();
			return Texture2D.FromStream(device.GraphicsDevice, (Stream)s);
		}

		public static Texture2D ToGrayScaledPalettable(this Texture2D original)
		{
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Expected O, but got Unknown
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			if (original == null || ((GraphicsResource)original).get_IsDisposed())
			{
				return null;
			}
			Color[] colors = (Color[])(object)new Color[original.get_Width() * original.get_Height()];
			original.GetData<Color>(colors);
			Color[] destColors = (Color[])(object)new Color[original.get_Width() * original.get_Height()];
			Texture2D newTexture;
			using (GraphicsDeviceContext device = GameService.Graphics.LendGraphicsDeviceContext())
			{
				newTexture = new Texture2D(device.GraphicsDevice, original.get_Width(), original.get_Height());
			}
			for (int i = 0; i < original.get_Width(); i++)
			{
				for (int j = 0; j < original.get_Height(); j++)
				{
					int index = i + j * original.get_Width();
					Color originalColor = colors[index];
					float maxval = 1.79f;
					float grayScale = (float)(int)((Color)(ref originalColor)).get_R() / 255f * 0.3f + (float)(int)((Color)(ref originalColor)).get_G() / 255f * 0.59f + (float)(int)((Color)(ref originalColor)).get_B() / 255f * 0.11f + (float)(int)((Color)(ref originalColor)).get_A() / 255f * 0.79f;
					grayScale /= maxval;
					destColors[index] = new Color(grayScale, grayScale, grayScale, (float)(int)((Color)(ref originalColor)).get_A());
				}
			}
			newTexture.SetData<Color>(destColors);
			return newTexture;
		}
	}
}
