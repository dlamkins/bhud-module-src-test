using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.GameIntegration.GfxSettings;
using Blish_HUD.Input;
using Kenedia.Modules.Characters.Strings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Expected O, but got Unknown
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Expected O, but got Unknown
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Expected O, but got Unknown
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Expected O, but got Unknown
			//IL_0209: Unknown result type (might be due to invalid IL or missing references)
			//IL_020e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0215: Unknown result type (might be due to invalid IL or missing references)
			//IL_0220: Unknown result type (might be due to invalid IL or missing references)
			//IL_0245: Unknown result type (might be due to invalid IL or missing references)
			//IL_0262: Expected O, but got Unknown
			GameService.Graphics.get_UIScaleMultiplier();
			GameService.Graphics.get_Resolution();
			int CharacterImageSize = 140;
			int Image_Gap = -10;
			int topMenuHeight = 60;
			((Control)this).set_Size(Dimensions);
			Image_Gap = 17;
			CharacterImageSize = 124;
			StandardButton val = new StandardButton();
			((Control)val).set_Parent((Container)(object)this);
			val.set_Text(common.CaptureAll);
			((Control)val).set_Location(new Microsoft.Xna.Framework.Point(6 + 5 * (CharacterImageSize + Image_Gap), 0));
			((Control)val).set_Size(new Microsoft.Xna.Framework.Point(CharacterImageSize, topMenuHeight - 30));
			captureAll_Button = val;
			ContentService contentService = new ContentService();
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)this);
			val2.set_Text(common.UISizeDisclaimer);
			((Control)val2).set_Location(new Microsoft.Xna.Framework.Point(0, 25));
			((Control)val2).set_Size(new Microsoft.Xna.Framework.Point(((Control)this).get_Width() - CharacterImageSize - Image_Gap, topMenuHeight - 30));
			val2.set_HorizontalAlignment((HorizontalAlignment)1);
			val2.set_TextColor(Microsoft.Xna.Framework.Color.Red);
			val2.set_Font(contentService.get_DefaultFont18());
			disclaimer_Label = val2;
			StandardButton val3 = new StandardButton();
			((Control)val3).set_Parent((Container)(object)this);
			val3.set_Text(common.Close);
			((Control)val3).set_Location(new Microsoft.Xna.Framework.Point(6 + 5 * (CharacterImageSize + Image_Gap), 30));
			((Control)val3).set_Size(new Microsoft.Xna.Framework.Point(CharacterImageSize, topMenuHeight - 30));
			close_Button = val3;
			((Control)close_Button).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				((Control)this).set_Visible(false);
				((Control)Module.MainWidow).Show();
				((Control)Module.ImageSelectorWindow).Show();
				Module.ResetGameWindow();
				if (LoadCustomImages != null)
				{
					LoadCustomImages();
				}
			});
			for (int i = 0; i < 5; i++)
			{
				int[] offsets = new int[5] { -1, 0, 0, 1, 1 };
				BasicContainer obj = new BasicContainer
				{
					showBackground = false,
					FrameColor = Microsoft.Xna.Framework.Color.Transparent
				};
				((Control)obj).set_Parent((Container)(object)this);
				((Control)obj).set_Location(new Microsoft.Xna.Framework.Point(4 + offsets[i] + i * (CharacterImageSize + Image_Gap), 1 + topMenuHeight));
				((Control)obj).set_Size(new Microsoft.Xna.Framework.Point(CharacterImageSize, CharacterImageSize));
				((Control)obj).set_Visible(false);
				BasicContainer ctn = obj;
				StandardButton val4 = new StandardButton();
				((Control)val4).set_Parent((Container)(object)this);
				val4.set_Text(common.Capture);
				((Control)val4).set_Location(new Microsoft.Xna.Framework.Point(4 + offsets[i] + i * (CharacterImageSize + Image_Gap), 0));
				((Control)val4).set_Size(new Microsoft.Xna.Framework.Point(CharacterImageSize, topMenuHeight - 30));
				StandardButton btn = val4;
				((Control)btn).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					click();
				});
				((Control)captureAll_Button).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					click();
				});
				captureButtons.Add(btn);
				void click()
				{
					//IL_001e: Unknown result type (might be due to invalid IL or missing references)
					//IL_0028: Unknown result type (might be due to invalid IL or missing references)
					ScreenModeSetting? screenMode = GameService.GameIntegration.get_GfxSettings().get_ScreenMode();
					if ((screenMode.HasValue ? ScreenModeSetting.op_Implicit(screenMode.GetValueOrDefault()) : null) == ScreenModeSetting.op_Implicit(ScreenModeSetting.get_Windowed()))
					{
						Regex regex = new Regex("Image.*[0-9].png");
						List<string> images = (from path in Directory.GetFiles(Module.GlobalImagesPath, "*.png", SearchOption.AllDirectories)
							where regex.IsMatch(path)
							select path).ToList();
						CharacterImageSize = 110;
						IntPtr gw2WindowHandle = GameService.GameIntegration.get_Gw2Instance().get_Gw2WindowHandle();
						Module.RECT wndBounds = default(Module.RECT);
						Module.RECT clientRectangle = default(Module.RECT);
						Module.GetWindowRect(gw2WindowHandle, ref wndBounds);
						Module.GetClientRect(gw2WindowHandle, out clientRectangle);
						int TitleBarHeight = wndBounds.Bottom - wndBounds.Top - (clientRectangle.Bottom - clientRectangle.Top);
						int SideBarWidth = wndBounds.Right - wndBounds.Left - (clientRectangle.Right - clientRectangle.Left);
						Microsoft.Xna.Framework.Rectangle cPos = ((Control)ctn).get_AbsoluteBounds();
						double factor = GameService.Graphics.get_UIScaleMultiplier();
						using Bitmap bitmap = new Bitmap(CharacterImageSize, CharacterImageSize);
						using (Graphics g = Graphics.FromImage(bitmap))
						{
							int x = (int)((double)cPos.X * factor);
							int y = (int)((double)cPos.Y * factor);
							g.CopyFromScreen(new System.Drawing.Point(clientRectangle.Left + x + SideBarWidth, clientRectangle.Top + y + TitleBarHeight), System.Drawing.Point.Empty, new Size(CharacterImageSize, CharacterImageSize));
						}
						bitmap.Save(Module.GlobalImagesPath + "Image " + (images.Count + 1) + ".png", ImageFormat.Png);
						ScreenNotification.ShowNotification(string.Format(common.CaptureNotification, "Image " + (images.Count + 1) + ".png"), (NotificationType)1, (Texture2D)null, 4);
					}
					else
					{
						ScreenNotification.ShowNotification(string.Format(common.UIScale_Error, Environment.NewLine), (NotificationType)2, (Texture2D)null, 4);
					}
				}
			}
		}

		public void UpdateLanguage()
		{
			disclaimer_Label.set_Text(common.UISizeDisclaimer);
			close_Button.set_Text(common.Close);
			captureAll_Button.set_Text(common.CaptureAll);
			foreach (StandardButton captureButton in captureButtons)
			{
				captureButton.set_Text(common.Capture);
			}
		}
	}
}
