using System;
using System.IO;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics;
using Blish_HUD.Input;
using DecorBlishhudModule.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DecorBlishhudModule.Sections
{
	public class BigImageSection
	{
		private static readonly Logger Logger = Logger.GetLogger<DecorModule>();

		private static Panel bigImagePanel;

		public static async Task UpdateDecorationImageAsync(Decoration decoration, Container _decorWindow, Image _decorationImage)
		{
			((Control)_decorationImage).set_ZIndex(101);
			Texture2D textureX = DecorModule.DecorModuleInstance.X2;
			Texture2D textureXActive = DecorModule.DecorModuleInstance.X2Active;
			Image val = new Image(AsyncTexture2D.op_Implicit(textureX));
			((Control)val).set_Parent(_decorWindow);
			((Control)val).set_Size(new Point(30, 30));
			((Control)val).set_ZIndex(102);
			((Control)val).set_Visible(false);
			Image textureXImage = val;
			if (bigImagePanel == null)
			{
				Panel val2 = new Panel();
				((Control)val2).set_Parent(_decorWindow);
				((Control)val2).set_Size(new Point(1045, 632));
				((Control)val2).set_Location(new Point(11, 43));
				((Control)val2).set_BackgroundColor(Color.get_Black());
				((Control)val2).set_Opacity(0.5f);
				((Control)val2).set_Visible(false);
				((Control)val2).set_ZIndex(100);
				bigImagePanel = val2;
			}
			_decorationImage.set_Texture((AsyncTexture2D)null);
			AdjustImageSize(null, null);
			if (!string.IsNullOrEmpty(decoration.ImageUrl))
			{
				try
				{
					Texture2D borderedTexture = CreateBorderedTexture(await DecorModule.DecorModuleInstance.Client.GetByteArrayAsync(decoration.ImageUrl));
					if (borderedTexture != null)
					{
						_decorationImage.set_Texture(AsyncTexture2D.op_Implicit(borderedTexture));
						AdjustImageSize(borderedTexture, _decorationImage);
						CenterImageInParent(_decorationImage, (Control)(object)_decorWindow);
						PositionXIconAtTopLeft(textureXImage, _decorationImage);
						((Control)bigImagePanel).set_Visible(true);
						if (((Control)_decorationImage).get_Visible())
						{
							((Control)textureXImage).set_Visible(true);
						}
					}
				}
				catch (Exception ex)
				{
					Logger.Warn("Failed to load decoration image for '" + decoration.Name + "'. Error: " + ex.ToString());
				}
				((Control)textureXImage).add_MouseEntered((EventHandler<MouseEventArgs>)delegate
				{
					textureXImage.set_Texture(AsyncTexture2D.op_Implicit(textureXActive));
				});
				((Control)textureXImage).add_MouseLeft((EventHandler<MouseEventArgs>)delegate
				{
					textureXImage.set_Texture(AsyncTexture2D.op_Implicit(textureX));
				});
				((Control)_decorationImage).add_Click((EventHandler<MouseEventArgs>)async delegate
				{
					await Task.Delay(100);
					if (((Control)_decorationImage).get_Visible() || ((Control)bigImagePanel).get_Visible() || ((Control)textureXImage).get_Visible())
					{
						((Control)_decorationImage).set_Visible(false);
						((Control)bigImagePanel).set_Visible(false);
						((Control)textureXImage).set_Visible(false);
					}
				});
				((Control)_decorWindow).add_Click((EventHandler<MouseEventArgs>)async delegate
				{
					await Task.Delay(100);
					if (((Control)_decorationImage).get_Visible() || ((Control)bigImagePanel).get_Visible() || ((Control)textureXImage).get_Visible())
					{
						((Control)_decorationImage).set_Visible(false);
						((Control)bigImagePanel).set_Visible(false);
						((Control)textureXImage).set_Visible(false);
					}
				});
			}
			else
			{
				_decorationImage.set_Texture((AsyncTexture2D)null);
				AdjustImageSize(null, null);
				((Control)bigImagePanel).set_Visible(false);
				((Control)textureXImage).set_Visible(false);
			}
		}

		private static Texture2D CreateBorderedTexture(byte[] imageResponse)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Expected O, but got Unknown
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				using MemoryStream memoryStream = new MemoryStream(imageResponse);
				GraphicsDeviceContext graphicsContext = GameService.Graphics.LendGraphicsDeviceContext();
				try
				{
					Texture2D originalTexture = Texture2D.FromStream(((GraphicsDeviceContext)(ref graphicsContext)).get_GraphicsDevice(), (Stream)memoryStream);
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
				Logger.Warn("Failed to create bordered texture. Error: " + ex.ToString());
				return null;
			}
		}

		public static void CenterImageInParent(Image image, Control parent)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			int centerX = (parent.get_Width() - ((Control)image).get_Size().X) / 2;
			int centerY = (parent.get_Height() - ((Control)image).get_Size().Y) / 2;
			((Control)image).set_Location(new Point(centerX - 48, centerY - 40));
		}

		public static void AdjustImageSize(Texture2D loadedTexture, Image decorationImage)
		{
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			if (loadedTexture != null)
			{
				int maxDimension = 600;
				float aspectRatio = (float)loadedTexture.get_Width() / (float)loadedTexture.get_Height();
				int targetWidth;
				int targetHeight;
				if (loadedTexture.get_Width() > loadedTexture.get_Height())
				{
					targetWidth = maxDimension;
					targetHeight = (int)((float)maxDimension / aspectRatio);
				}
				else
				{
					targetHeight = maxDimension;
					targetWidth = (int)((float)maxDimension * aspectRatio);
				}
				((Control)decorationImage).set_Size(new Point(targetWidth, targetHeight));
			}
		}

		private static void PositionXIconAtTopLeft(Image xIcon, Image decorationImage)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			int offsetX = 35;
			int offsetY = 5;
			((Control)xIcon).set_Location(new Point(((Control)decorationImage).get_Location().X + ((Control)decorationImage).get_Size().X - offsetX, ((Control)decorationImage).get_Location().Y + offsetY));
		}
	}
}
