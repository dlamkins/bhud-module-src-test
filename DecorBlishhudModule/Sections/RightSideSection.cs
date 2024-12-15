using System;
using System.Collections;
using System.IO;
using System.Linq;
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
	public class RightSideSection
	{
		private static readonly Logger Logger = Logger.GetLogger<DecorModule>();

		public static async Task UpdateDecorationImageAsync(Decoration decoration, Container _decorWindow, Image _decorationImage)
		{
			Label decorationNameLabel = ((IEnumerable)_decorWindow.get_Children()).OfType<Label>().FirstOrDefault();
			decorationNameLabel.set_Text("");
			Panel val = new Panel();
			((Control)val).set_Parent(_decorWindow);
			((Control)val).set_Location(new Point(770, 550));
			val.set_Title("Copied !");
			((Control)val).set_Width(80);
			((Control)val).set_Height(45);
			val.set_ShowBorder(true);
			((Control)val).set_Opacity(0f);
			((Control)val).set_Visible(false);
			Panel savedPanel = val;
			((Control)decorationNameLabel).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				if (!((Control)savedPanel).get_Visible())
				{
					SaveTasks.CopyTextToClipboard(decoration.Name);
					SaveTasks.ShowSavedPanel(savedPanel);
				}
			});
			((Control)_decorationImage).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				if (!((Control)savedPanel).get_Visible())
				{
					SaveTasks.CopyTextToClipboard(decoration.Name);
					SaveTasks.ShowSavedPanel(savedPanel);
				}
			});
			CenterTextInParent(decorationNameLabel, (Control)(object)_decorWindow);
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
						decorationNameLabel.set_Text(decoration.Name.Replace(" ", " \ud83d\udeaa ") ?? "Unknown Decoration");
						CenterTextInParent(decorationNameLabel, (Control)(object)_decorWindow);
						PositionTextAboveImage(decorationNameLabel, _decorationImage);
					}
				}
				catch (Exception ex)
				{
					Logger.Warn("Failed to load decoration image for '" + decoration.Name + "'. Error: " + ex.ToString());
					decorationNameLabel.set_Text("Error Loading Decoration");
					CenterTextInParent(decorationNameLabel, (Control)(object)_decorWindow);
				}
			}
			else
			{
				_decorationImage.set_Texture((AsyncTexture2D)null);
				AdjustImageSize(null, null);
				decorationNameLabel.set_Text(decoration.Name ?? "Unknown Decoration");
				CenterTextInParent(decorationNameLabel, (Control)(object)_decorWindow);
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

		public static void CenterTextInParent(Label label, Control parent)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			float textWidth = GameService.Content.get_DefaultFont18().MeasureString(label.get_Text()).Width;
			int parentWidth = parent.get_Width();
			int startX = (460 + parentWidth - (int)textWidth) / 2;
			if (label.get_Text() == "Loading...")
			{
				((Control)label).set_Location(new Point(startX, ((Control)label).get_Location().Y + 300));
			}
			else
			{
				((Control)label).set_Location(new Point(startX, ((Control)label).get_Location().Y));
			}
		}

		public static void PositionTextAboveImage(Label text, Image image)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			int textY = ((Control)image).get_Location().Y - ((Control)text).get_Height() + 30;
			((Control)text).set_Location(new Point(((Control)text).get_Location().X, textY));
		}

		public static void CenterImageInParent(Image image, Control parent)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			int centerX = (parent.get_Width() - ((Control)image).get_Size().X) / 2;
			int centerY = (parent.get_Height() - ((Control)image).get_Size().Y) / 2;
			((Control)image).set_Location(new Point(centerX + 230, centerY - 40));
		}

		public static void AdjustImageSize(Texture2D loadedTexture, Image decorationImage)
		{
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			if (loadedTexture != null)
			{
				int maxDimension = 500;
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
	}
}
