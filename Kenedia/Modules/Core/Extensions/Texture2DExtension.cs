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
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			GraphicsDeviceContext device = GameService.Graphics.LendGraphicsDeviceContext();
			try
			{
				return Texture2D.FromStream(((GraphicsDeviceContext)(ref device)).get_GraphicsDevice(), (Stream)s);
			}
			finally
			{
				((GraphicsDeviceContext)(ref device)).Dispose();
			}
		}

		public static Texture2D ToGrayScaledPalettable(this Texture2D original)
		{
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
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
			GraphicsDeviceContext device = GameService.Graphics.LendGraphicsDeviceContext();
			Texture2D newTexture;
			try
			{
				newTexture = new Texture2D(((GraphicsDeviceContext)(ref device)).get_GraphicsDevice(), original.get_Width(), original.get_Height());
			}
			finally
			{
				((GraphicsDeviceContext)(ref device)).Dispose();
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
