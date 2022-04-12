using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Kenedia.Modules.Characters.Strings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.Characters
{
	public class ImageSelector : BasicContainer
	{
		private Character _assignedCharacter = new Character();

		private Texture2D selected_Image;

		private Label name_Label;

		private Image character_Image;

		private Image selector_Image;

		private FlowPanel images_Panel;

		private List<string> ImageNames = new List<string>();

		private HeaderUnderlined header_HeaderUnderlined;

		private StandardButton save_Button;

		private StandardButton default_Button;

		private StandardButton refresh_Button;

		private StandardButton cancel_Button;

		private StandardButton create_Button;

		public Character assignedCharacter
		{
			get
			{
				return _assignedCharacter;
			}
			set
			{
				if (assignedCharacter != value)
				{
					_assignedCharacter = value;
					setCharacter();
					selected_Image = null;
				}
			}
		}

		public ImageSelector(int width, int height)
		{
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Expected O, but got Unknown
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Expected O, but got Unknown
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_014b: Expected O, but got Unknown
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0151: Unknown result type (might be due to invalid IL or missing references)
			//IL_0158: Unknown result type (might be due to invalid IL or missing references)
			//IL_0167: Unknown result type (might be due to invalid IL or missing references)
			//IL_0174: Unknown result type (might be due to invalid IL or missing references)
			//IL_0187: Unknown result type (might be due to invalid IL or missing references)
			//IL_0193: Expected O, but got Unknown
			//IL_0194: Unknown result type (might be due to invalid IL or missing references)
			//IL_0199: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01be: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d1: Expected O, but got Unknown
			//IL_0205: Unknown result type (might be due to invalid IL or missing references)
			//IL_020a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0211: Unknown result type (might be due to invalid IL or missing references)
			//IL_021c: Unknown result type (might be due to invalid IL or missing references)
			//IL_024f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0263: Expected O, but got Unknown
			//IL_027b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0280: Unknown result type (might be due to invalid IL or missing references)
			//IL_0287: Unknown result type (might be due to invalid IL or missing references)
			//IL_0292: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b8: Expected O, but got Unknown
			//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0310: Expected O, but got Unknown
			//IL_0328: Unknown result type (might be due to invalid IL or missing references)
			//IL_032d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0334: Unknown result type (might be due to invalid IL or missing references)
			//IL_033f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0351: Unknown result type (might be due to invalid IL or missing references)
			//IL_0365: Expected O, but got Unknown
			//IL_037d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0382: Unknown result type (might be due to invalid IL or missing references)
			//IL_0389: Unknown result type (might be due to invalid IL or missing references)
			//IL_0394: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ba: Expected O, but got Unknown
			//IL_03d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_03de: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0406: Unknown result type (might be due to invalid IL or missing references)
			//IL_041b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0422: Unknown result type (might be due to invalid IL or missing references)
			//IL_042e: Expected O, but got Unknown
			((Control)this).set_Width(width);
			((Control)this).set_Height(height);
			ContentService contentService = new ContentService();
			int iPadding = 5;
			Image val = new Image();
			((Control)val).set_Parent((Container)(object)this);
			val.set_Texture(AsyncTexture2D.op_Implicit(Textures.Emblems[2]));
			((Control)val).set_Size(new Point(64, 64));
			((Control)val).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				((Control)this).Hide();
			});
			HeaderUnderlined headerUnderlined = new HeaderUnderlined();
			((Control)headerUnderlined).set_Parent((Container)(object)this);
			headerUnderlined.Text = common.SelectImage;
			((Control)headerUnderlined).set_Location(new Point(72, iPadding));
			headerUnderlined.Font = contentService.get_DefaultFont32();
			((Control)headerUnderlined).set_Width(300);
			header_HeaderUnderlined = headerUnderlined;
			Image val2 = new Image();
			((Control)val2).set_Parent((Container)(object)this);
			((Control)val2).set_Size(new Point(75, 75));
			((Control)val2).set_Location(new Point(((Control)this).get_Width() - 75 - iPadding, iPadding));
			val2.set_Texture(AsyncTexture2D.op_Implicit(new Character().getProfessionTexture()));
			character_Image = val2;
			Panel val3 = new Panel();
			((Control)val3).set_Parent((Container)(object)this);
			((Control)val3).set_Location(new Point(0, 75 + iPadding));
			((Control)val3).set_Height(((Control)this).get_Height() - iPadding - 75);
			((Control)val3).set_Padding(new Thickness(8f, 8f));
			((Container)val3).set_WidthSizingMode((SizingMode)2);
			Panel panel = val3;
			Image val4 = new Image();
			((Control)val4).set_Parent((Container)(object)panel);
			((Control)val4).set_Size(new Point(104, 110));
			((Control)val4).set_Location(new Point(0, 0));
			val4.set_Texture(AsyncTexture2D.op_Implicit(Textures.Backgrounds[17]));
			((Control)val4).set_Visible(true);
			selector_Image = val4;
			Label val5 = new Label();
			((Control)val5).set_Parent((Container)(object)this);
			val5.set_Text("Character Name");
			val5.set_AutoSizeWidth(true);
			val5.set_Font(contentService.get_DefaultFont18());
			val5.set_AutoSizeHeight(true);
			val5.set_VerticalAlignment((VerticalAlignment)0);
			name_Label = val5;
			((Control)name_Label).set_Location(new Point(((Control)this).get_Width() - ((Control)character_Image).get_Width() - iPadding * 3 - ((Control)name_Label).get_Width(), iPadding));
			StandardButton val6 = new StandardButton();
			((Control)val6).set_Parent((Container)(object)this);
			val6.set_Text(common.UseDefault);
			((Control)val6).set_Location(new Point(((Control)this).get_Width() - ((Control)character_Image).get_Width() - iPadding * 3 - 125, iPadding + ((Control)name_Label).get_Height() + iPadding));
			((Control)val6).set_Size(new Point(125, 25));
			default_Button = val6;
			((Control)default_Button).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				selected_Image = null;
				character_Image.set_Texture(AsyncTexture2D.op_Implicit(assignedCharacter.getProfessionTexture(includeCustom: false)));
			});
			StandardButton val7 = new StandardButton();
			((Control)val7).set_Parent((Container)(object)this);
			val7.set_Text(common.RefreshImages);
			((Control)val7).set_Location(new Point(75, 55));
			((Control)val7).set_Size(new Point(150, 25));
			refresh_Button = val7;
			((Control)refresh_Button).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				LoadImages();
			});
			StandardButton val8 = new StandardButton();
			((Control)val8).set_Parent((Container)(object)this);
			val8.set_Text(common.CreateImages);
			((Control)val8).set_Location(new Point(230, 55));
			((Control)val8).set_Size(new Point(150, 25));
			create_Button = val8;
			((Control)create_Button).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				if (!GameService.GameIntegration.get_Gw2Instance().get_IsInGame())
				{
					Module.RECT lpRect = default(Module.RECT);
					Module.GetWindowRect(GameService.GameIntegration.get_Gw2Instance().get_Gw2WindowHandle(), ref lpRect);
					Module.MoveWindow(GameService.GameIntegration.get_Gw2Instance().get_Gw2WindowHandle(), lpRect.Left, lpRect.Top, 1100, 800, Repaint: false);
					((Control)this).Hide();
					((Control)Module.MainWidow).Hide();
					Module.screenCapture = true;
					((Control)Module.screenCaptureWindow).set_Visible(true);
				}
			});
			StandardButton val9 = new StandardButton();
			((Control)val9).set_Parent((Container)(object)this);
			val9.set_Text(common.Save);
			((Control)val9).set_Location(new Point(400, 10));
			((Control)val9).set_Size(new Point(125, 25));
			save_Button = val9;
			((Control)save_Button).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				assignedCharacter.Icon = ((selected_Image != null) ? selected_Image.Name : "");
				assignedCharacter.characterControl.UpdateUI();
				Module.subWindow.setCharacter(assignedCharacter);
				assignedCharacter.Save();
				((Control)this).set_Visible(false);
			});
			StandardButton val10 = new StandardButton();
			((Control)val10).set_Parent((Container)(object)this);
			val10.set_Text(common.Cancel);
			((Control)val10).set_Location(new Point(550, 10));
			((Control)val10).set_Size(new Point(125, 25));
			cancel_Button = val10;
			((Control)cancel_Button).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				((Control)this).set_Visible(false);
			});
			FlowPanel val11 = new FlowPanel();
			((Control)val11).set_Parent((Container)(object)this);
			((Control)val11).set_Location(new Point(0, 75 + iPadding));
			((Control)val11).set_Height(((Control)this).get_Height() - iPadding - 75);
			((Panel)val11).set_CanScroll(true);
			val11.set_ControlPadding(new Vector2(8f, 8f));
			((Panel)val11).set_ShowBorder(true);
			((Container)val11).set_WidthSizingMode((SizingMode)2);
			images_Panel = val11;
		}

		public void LoadImages()
		{
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Expected O, but got Unknown
			Texture2D[] customImages = Textures.CustomImages;
			foreach (Texture2D pic in customImages)
			{
				if (pic != null && !ImageNames.Contains(pic.Name))
				{
					Image val = new Image();
					((Control)val).set_Parent((Container)(object)images_Panel);
					((Control)val).set_Size(new Point(96, 96));
					val.set_Texture(AsyncTexture2D.op_Implicit(pic));
					Image img = val;
					((Control)img).add_MouseEntered((EventHandler<MouseEventArgs>)delegate
					{
						((Control)selector_Image).set_Location(new Point(((Control)img).get_Location().X, ((Control)img).get_Location().Y - ((Container)images_Panel).get_VerticalScrollOffset()));
					});
					((Control)img).add_Click((EventHandler<MouseEventArgs>)delegate
					{
						character_Image.set_Texture(img.get_Texture());
						selected_Image = pic;
					});
					ImageNames.Add(pic.Name);
				}
			}
			((Control)images_Panel).Invalidate();
		}

		private void setCharacter()
		{
			name_Label.set_Text(assignedCharacter.Name);
			character_Image.set_Texture(AsyncTexture2D.op_Implicit(assignedCharacter.getProfessionTexture()));
		}

		public void UpdateLanguage()
		{
			header_HeaderUnderlined.Text = common.SelectImage;
			save_Button.set_Text(common.Save);
			create_Button.set_Text(common.CreateImages);
			refresh_Button.set_Text(common.RefreshImages);
			default_Button.set_Text(common.UseDefault);
			cancel_Button.set_Text(common.Cancel);
		}
	}
}
