using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Kenedia.Modules.Characters.Strings;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.Characters
{
	public class ScreenCaptureWindow : BasicContainer
	{
		public Action LoadCustomImages;

		private List<StandardButton> captureButtons = new List<StandardButton>();

		private StandardButton captureAll_Button;

		private StandardButton close_Button;

		private Label disclaimer_Label;

		public ScreenCaptureWindow(Microsoft.Xna.Framework.Point Dimensions)
		{
			_ = GameService.Graphics.UIScaleMultiplier;
			_ = GameService.Graphics.Resolution;
			int CharacterImageSize = 140;
			int Image_Gap = -10;
			int topMenuHeight = 60;
			base.Size = Dimensions;
			Image_Gap = 17;
			CharacterImageSize = 124;
			captureAll_Button = new StandardButton
			{
				Parent = this,
				Text = common.CaptureAll,
				Location = new Microsoft.Xna.Framework.Point(6 + 5 * (CharacterImageSize + Image_Gap), 0),
				Size = new Microsoft.Xna.Framework.Point(CharacterImageSize, topMenuHeight - 30)
			};
			ContentService contentService = new ContentService();
			disclaimer_Label = new Label
			{
				Parent = this,
				Text = common.UISizeDisclaimer,
				Location = new Microsoft.Xna.Framework.Point(0, 30),
				Size = new Microsoft.Xna.Framework.Point(base.Width - CharacterImageSize - Image_Gap, topMenuHeight - 30),
				HorizontalAlignment = HorizontalAlignment.Center,
				TextColor = Microsoft.Xna.Framework.Color.Red,
				Font = contentService.DefaultFont18
			};
			close_Button = new StandardButton
			{
				Parent = this,
				Text = common.Close,
				Location = new Microsoft.Xna.Framework.Point(6 + 5 * (CharacterImageSize + Image_Gap), 30),
				Size = new Microsoft.Xna.Framework.Point(CharacterImageSize, topMenuHeight - 30)
			};
			close_Button.Click += delegate
			{
				base.Visible = false;
				Module.MainWidow.Show();
				Module.ImageSelectorWindow.Show();
				Module.ResetGameWindow();
			};
			for (int i = 0; i < 5; i++)
			{
				int[] offsets = new int[5] { -1, 0, 0, 1, 1 };
				BasicContainer ctn = new BasicContainer
				{
					showBackground = false,
					FrameColor = Microsoft.Xna.Framework.Color.Transparent,
					Parent = this,
					Location = new Microsoft.Xna.Framework.Point(4 + offsets[i] + i * (CharacterImageSize + Image_Gap), 1 + topMenuHeight),
					Size = new Microsoft.Xna.Framework.Point(CharacterImageSize, CharacterImageSize),
					Visible = false
				};
				StandardButton btn = new StandardButton
				{
					Parent = this,
					Text = common.Capture,
					Location = new Microsoft.Xna.Framework.Point(4 + offsets[i] + i * (CharacterImageSize + Image_Gap), 0),
					Size = new Microsoft.Xna.Framework.Point(CharacterImageSize, topMenuHeight - 30)
				};
				btn.Click += delegate
				{
					click();
				};
				captureAll_Button.Click += delegate
				{
					click();
				};
				captureButtons.Add(btn);
				void click()
				{
					List<string> images = Directory.GetFiles(Module.GlobalImagesPath, "*.png", SearchOption.AllDirectories).ToList();
					CharacterImageSize = 110;
					int TitleBarHeight = 33;
					int SideBarWidth = 10;
					Module.RECT clientRectangle = default(Module.RECT);
					Module.GetWindowRect(GameService.GameIntegration.Gw2Instance.Gw2WindowHandle, ref clientRectangle);
					Microsoft.Xna.Framework.Rectangle cPos = ctn.AbsoluteBounds;
					double factor = GameService.Graphics.UIScaleMultiplier;
					using (Bitmap bitmap = new Bitmap(CharacterImageSize, CharacterImageSize))
					{
						using (Graphics g = System.Drawing.Graphics.FromImage(bitmap))
						{
							int x = (int)((double)cPos.X * factor);
							int y = (int)((double)cPos.Y * factor);
							g.CopyFromScreen(new System.Drawing.Point(clientRectangle.Left + x + SideBarWidth, clientRectangle.Top + y + TitleBarHeight), System.Drawing.Point.Empty, new Size(CharacterImageSize, CharacterImageSize));
						}
						bitmap.Save(Module.GlobalImagesPath + "Image " + (images.Count + 1) + ".png", ImageFormat.Png);
					}
					LoadCustomImages();
				}
			}
		}

		public void UpdateLanguage()
		{
			disclaimer_Label.Text = common.UISizeDisclaimer;
			close_Button.Text = common.Close;
			captureAll_Button.Text = common.CaptureAll;
			foreach (StandardButton captureButton in captureButtons)
			{
				captureButton.Text = common.Capture;
			}
		}
	}
}
