using System;
using Blish_HUD;
using Blish_HUD.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DecorBlishhudModule.Sections.LeftSideTasks
{
	public class BorderCreator
	{
		private static readonly Logger Logger = Logger.GetLogger<BorderCreator>();

		public static Texture2D CreateBorderedTexture(Texture2D originalTexture)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Expected O, but got Unknown
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				GraphicsDeviceContext graphicsContext = GameService.Graphics.LendGraphicsDeviceContext();
				try
				{
					int borderWidth = 15;
					Color innerBorderColor = default(Color);
					((Color)(ref innerBorderColor))._002Ector(86, 76, 55);
					Color outerBorderColor = Color.get_Black();
					int borderedWidth = originalTexture.get_Width() + 2 * borderWidth;
					int borderedHeight = originalTexture.get_Height() + 2 * borderWidth;
					Texture2D borderedTexture = new Texture2D(((GraphicsDeviceContext)(ref graphicsContext)).get_GraphicsDevice(), borderedWidth, borderedHeight);
					Color[] borderedColorData = (Color[])(object)new Color[borderedWidth * borderedHeight];
					for (int y2 = 0; y2 < borderedHeight; y2++)
					{
						for (int x = 0; x < borderedWidth; x++)
						{
							int distanceFromEdge = Math.Min(Math.Min(x, borderedWidth - x - 1), Math.Min(y2, borderedHeight - y2 - 1));
							if (distanceFromEdge < borderWidth)
							{
								float gradientFactor = (float)distanceFromEdge / (float)borderWidth;
								borderedColorData[y2 * borderedWidth + x] = Color.Lerp(outerBorderColor, innerBorderColor, gradientFactor);
							}
							else
							{
								borderedColorData[y2 * borderedWidth + x] = Color.get_Transparent();
							}
						}
					}
					Color[] originalColorData = (Color[])(object)new Color[originalTexture.get_Width() * originalTexture.get_Height()];
					originalTexture.GetData<Color>(originalColorData);
					for (int y = 0; y < originalTexture.get_Height(); y++)
					{
						for (int x2 = 0; x2 < originalTexture.get_Width(); x2++)
						{
							int borderedIndex = (y + borderWidth) * borderedWidth + x2 + borderWidth;
							borderedColorData[borderedIndex] = originalColorData[y * originalTexture.get_Width() + x2];
						}
					}
					borderedTexture.SetData<Color>(borderedColorData);
					return borderedTexture;
				}
				finally
				{
					((GraphicsDeviceContext)(ref graphicsContext)).Dispose();
				}
			}
			catch (Exception ex)
			{
				Logger.Warn($"Failed to create bordered texture. Error: {ex}");
				return null;
			}
		}
	}
}
