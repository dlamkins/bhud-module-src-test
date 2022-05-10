using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Kenedia.Modules.Characters.Strings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Kenedia.Modules.Characters
{
	public class CharacterControl : Panel
	{
		public Character assignedCharacter;

		public Image character_Image;

		public Image profession_Image;

		public Image birthday_Image;

		public Image switch_Image;

		public Image separator_Image;

		public Image border_TopRight;

		public Image border_BottomLeft;

		public Label name_Label;

		public Label time_Label;

		public Label level_Label;

		public Panel background_Panel;

		public FlowPanel crafting_Panel;

		public List<DataImage> crafting_Images = new List<DataImage>();

		private int _FramePadding = 6;

		private int _Padding = 4;

		public int Padding
		{
			get
			{
				return _Padding;
			}
			set
			{
				_Padding = value;
				AdjustLayout();
			}
		}

		public CharacterControl(Character c)
			: this()
		{
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Expected O, but got Unknown
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Expected O, but got Unknown
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_0165: Unknown result type (might be due to invalid IL or missing references)
			//IL_0177: Unknown result type (might be due to invalid IL or missing references)
			//IL_0183: Expected O, but got Unknown
			//IL_0184: Unknown result type (might be due to invalid IL or missing references)
			//IL_0189: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0253: Unknown result type (might be due to invalid IL or missing references)
			//IL_025a: Unknown result type (might be due to invalid IL or missing references)
			//IL_026b: Expected O, but got Unknown
			//IL_026c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0271: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_0305: Expected O, but got Unknown
			//IL_0306: Unknown result type (might be due to invalid IL or missing references)
			//IL_030b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0312: Unknown result type (might be due to invalid IL or missing references)
			//IL_0319: Unknown result type (might be due to invalid IL or missing references)
			//IL_0355: Unknown result type (might be due to invalid IL or missing references)
			//IL_0365: Unknown result type (might be due to invalid IL or missing references)
			//IL_037f: Unknown result type (might be due to invalid IL or missing references)
			//IL_038b: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b3: Expected O, but got Unknown
			//IL_03b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_040b: Unknown result type (might be due to invalid IL or missing references)
			//IL_041a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0421: Unknown result type (might be due to invalid IL or missing references)
			//IL_0432: Expected O, but got Unknown
			//IL_0433: Unknown result type (might be due to invalid IL or missing references)
			//IL_0438: Unknown result type (might be due to invalid IL or missing references)
			//IL_043f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0446: Unknown result type (might be due to invalid IL or missing references)
			//IL_0483: Unknown result type (might be due to invalid IL or missing references)
			//IL_0493: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_04cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d8: Expected O, but got Unknown
			//IL_04d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_04de: Unknown result type (might be due to invalid IL or missing references)
			//IL_0541: Unknown result type (might be due to invalid IL or missing references)
			//IL_0554: Unknown result type (might be due to invalid IL or missing references)
			//IL_055b: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b4: Expected O, but got Unknown
			//IL_05b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_061d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0628: Unknown result type (might be due to invalid IL or missing references)
			//IL_062f: Unknown result type (might be due to invalid IL or missing references)
			//IL_064b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0652: Unknown result type (might be due to invalid IL or missing references)
			//IL_0662: Unknown result type (might be due to invalid IL or missing references)
			//IL_0669: Unknown result type (might be due to invalid IL or missing references)
			//IL_067a: Expected O, but got Unknown
			//IL_067b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0680: Unknown result type (might be due to invalid IL or missing references)
			//IL_0693: Unknown result type (might be due to invalid IL or missing references)
			//IL_069a: Unknown result type (might be due to invalid IL or missing references)
			//IL_06c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_06cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_06db: Expected O, but got Unknown
			//IL_072b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0730: Unknown result type (might be due to invalid IL or missing references)
			//IL_074f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0756: Unknown result type (might be due to invalid IL or missing references)
			//IL_0767: Unknown result type (might be due to invalid IL or missing references)
			//IL_07a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_07a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_07ba: Expected O, but got Unknown
			//IL_08b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_08b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_08cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_08df: Unknown result type (might be due to invalid IL or missing references)
			//IL_08ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_08f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0910: Unknown result type (might be due to invalid IL or missing references)
			//IL_091c: Expected O, but got Unknown
			CharacterControl characterControl = this;
			ContentService contentService = new ContentService();
			assignedCharacter = c;
			((Control)this).set_Height(76);
			((Container)this).set_WidthSizingMode((SizingMode)2);
			((Panel)this).set_ShowBorder(false);
			CharacterTooltip characterTooltip = new CharacterTooltip(assignedCharacter);
			((Control)characterTooltip).set_Parent((Container)(object)this);
			((Control)this).set_Tooltip((Tooltip)(object)characterTooltip);
			bool defaultIcon = assignedCharacter.Icon == null || assignedCharacter.Icon == "";
			Image val = new Image();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Location(new Point(_Padding, _Padding));
			((Control)val).set_Size(new Point(((Container)this).get_ContentRegion().Height - _Padding * 2, ((Container)this).get_ContentRegion().Height - _Padding * 2));
			val.set_Texture(AsyncTexture2D.op_Implicit(Textures.Backgrounds[7]));
			((Control)val).set_Visible(defaultIcon);
			border_TopRight = val;
			Image val2 = new Image();
			((Control)val2).set_Parent((Container)(object)this);
			((Control)val2).set_Location(new Point(_Padding, _Padding));
			((Control)val2).set_Size(new Point(((Container)this).get_ContentRegion().Height - _Padding * 2, ((Container)this).get_ContentRegion().Height - _Padding * 2));
			val2.set_Texture(AsyncTexture2D.op_Implicit(Textures.Backgrounds[6]));
			((Control)val2).set_Visible(defaultIcon);
			border_BottomLeft = val2;
			Image val3 = new Image();
			val3.set_Texture(AsyncTexture2D.op_Implicit(c.getProfessionTexture()));
			((Control)val3).set_Location(defaultIcon ? new Point(_Padding + _FramePadding, _Padding + _FramePadding) : new Point(_Padding, _Padding));
			((Control)val3).set_Size(defaultIcon ? new Point(((Container)this).get_ContentRegion().Height - _Padding * 2 - _FramePadding * 2, ((Container)this).get_ContentRegion().Height - _Padding * 2 - _FramePadding * 2) : new Point(((Container)this).get_ContentRegion().Height - _Padding * 2, ((Container)this).get_ContentRegion().Height - _Padding * 2));
			((Control)val3).set_Parent((Container)(object)this);
			((Control)val3).set_Tooltip(((Control)this).get_Tooltip());
			character_Image = val3;
			Panel val4 = new Panel();
			((Control)val4).set_Location(new Point(((Control)character_Image).get_Location().X, ((Control)character_Image).get_Location().Y + ((Control)character_Image).get_Height() - 38));
			((Control)val4).set_Size(new Point(22, ((Control)character_Image).get_Height() - (((Control)character_Image).get_Location().Y + ((Control)character_Image).get_Height() - 38)));
			((Control)val4).set_Parent((Container)(object)this);
			((Control)val4).set_BackgroundColor(new Color(43, 43, 43, 255));
			background_Panel = val4;
			Label val5 = new Label();
			((Control)val5).set_Parent((Container)(object)this);
			val5.set_AutoSizeWidth(true);
			((Control)val5).set_Location(new Point(((Control)character_Image).get_Location().X + 5, ((Control)character_Image).get_Location().Y + ((Control)character_Image).get_Height() - 12));
			val5.set_Font(contentService.GetFont((FontFace)0, (FontSize)11, (FontStyle)0));
			val5.set_Text(c.Level.ToString());
			((Control)val5).set_Tooltip(((Control)this).get_Tooltip());
			((Control)val5).set_Height((((Container)this).get_ContentRegion().Height - _Padding * 2) / 2);
			val5.set_VerticalAlignment((VerticalAlignment)1);
			level_Label = val5;
			Image val6 = new Image();
			val6.set_Texture(AsyncTexture2D.op_Implicit(c.getProfessionTexture(includeCustom: false, baseIcons: true)));
			((Control)val6).set_Location(new Point(((Control)character_Image).get_Location().X, ((Control)character_Image).get_Location().Y + ((Control)character_Image).get_Height() - 36));
			((Control)val6).set_Size(new Point(24, 24));
			((Control)val6).set_Parent((Container)(object)this);
			((Control)val6).set_Tooltip(((Control)this).get_Tooltip());
			profession_Image = val6;
			Label val7 = new Label();
			((Control)val7).set_Parent((Container)(object)this);
			val7.set_AutoSizeWidth(true);
			((Control)val7).set_Location(new Point(((Control)character_Image).get_Location().X + ((Control)character_Image).get_Width() + _Padding * 2 + _FramePadding, _Padding));
			val7.set_Font(contentService.GetFont((FontFace)0, (FontSize)18, (FontStyle)0));
			val7.set_Text(c.Name);
			((Control)val7).set_Tooltip(((Control)this).get_Tooltip());
			((Control)val7).set_Height((((Container)this).get_ContentRegion().Height - _Padding * 2) / 2);
			val7.set_VerticalAlignment((VerticalAlignment)1);
			name_Label = val7;
			Image val8 = new Image();
			((Control)val8).set_Location(new Point(((Control)character_Image).get_Location().X + ((Control)character_Image).get_Width() + _Padding + _FramePadding, ((Container)this).get_ContentRegion().Height / 2 - (int)Math.Ceiling((decimal)_Padding / 2m)));
			val8.set_Texture(AsyncTexture2D.op_Implicit(Textures.Icons[19]));
			((Control)val8).set_Parent((Container)(object)this);
			((Control)val8).set_Size(new Point(((Control)this).get_Width() - ((Control)character_Image).get_Width() - 2 - _Padding * 3, (int)Math.Ceiling((decimal)_Padding / 2m)));
			((Control)val8).set_Tooltip(((Control)this).get_Tooltip());
			separator_Image = val8;
			Label val9 = new Label();
			((Control)val9).set_Location(new Point(((Control)character_Image).get_Location().X + ((Control)character_Image).get_Width() + _Padding * 2 + _FramePadding, ((Container)this).get_ContentRegion().Height / 2 + _Padding - (contentService.get_DefaultFont18().LineHeight - contentService.get_DefaultFont12().LineHeight)));
			val9.set_Text("00:00:00");
			((Control)val9).set_Parent((Container)(object)this);
			((Control)val9).set_Height((((Container)this).get_ContentRegion().Height - _Padding * 2) / 2);
			val9.set_AutoSizeWidth(true);
			val9.set_Font(contentService.GetFont((FontFace)0, (FontSize)12, (FontStyle)0));
			val9.set_VerticalAlignment((VerticalAlignment)1);
			((Control)val9).set_Tooltip(((Control)this).get_Tooltip());
			time_Label = val9;
			Image val10 = new Image();
			val10.set_Texture(AsyncTexture2D.op_Implicit(Textures.Icons[17]));
			((Control)val10).set_Parent((Container)(object)this);
			((Control)val10).set_Location(new Point(((Control)this).get_Width() - 150, ((Container)this).get_ContentRegion().Height / 2 - 2));
			((Control)val10).set_Size(new Point(32, 32));
			((Control)val10).set_Visible(true);
			birthday_Image = val10;
			int iBegin = ((Control)character_Image).get_Location().X + ((Control)character_Image).get_Width() + _Padding * 3 + _FramePadding;
			int iWidth = (((Control)this).get_Width() - iBegin) / 2;
			if (c.Crafting.Count > 0)
			{
				FlowPanel val11 = new FlowPanel();
				((Control)val11).set_Location(new Point(iBegin + iWidth, ((Control)time_Label).get_Location().Y));
				((Control)val11).set_Parent((Container)(object)this);
				((Control)val11).set_Height(((Container)this).get_ContentRegion().Height);
				((Control)val11).set_Width((((Control)this).get_Width() - (((Control)character_Image).get_Location().X + ((Control)character_Image).get_Width() + _Padding * 2 + _FramePadding)) / 2);
				val11.set_FlowDirection((ControlFlowDirection)2);
				((Control)val11).set_Tooltip(((Control)this).get_Tooltip());
				crafting_Panel = val11;
				foreach (CharacterCrafting crafting in c.Crafting)
				{
					if (crafting.Active)
					{
						List<DataImage> list = crafting_Images;
						DataImage dataImage = new DataImage();
						((Image)dataImage).set_Texture(AsyncTexture2D.op_Implicit(Textures.Crafting[crafting.Id]));
						((Control)dataImage).set_Size(new Point(24, 24));
						((Control)dataImage).set_Parent((Container)(object)crafting_Panel);
						((Control)dataImage).set_Visible(!Module.Settings.OnlyMaxCrafting.get_Value() || ((crafting.Id == 4 || crafting.Id == 7) && crafting.Rating == 400) || crafting.Rating == 500);
						dataImage.Id = crafting.Id;
						dataImage.Crafting = crafting;
						((Control)dataImage).set_Tooltip(((Control)this).get_Tooltip());
						list.Add(dataImage);
					}
				}
			}
			Image val12 = new Image();
			((Control)val12).set_Location(new Point(((Control)this).get_Width() - 45, 10));
			val12.set_Texture(AsyncTexture2D.op_Implicit(Textures.Icons[12]));
			((Control)val12).set_Size(new Point(32, 32));
			((Control)val12).set_Parent((Container)(object)this);
			((Control)val12).set_BasicTooltipText(string.Format(common.Switch, c.Name));
			((Control)val12).set_Visible(false);
			switch_Image = val12;
			((Control)switch_Image).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				if (Module.Settings.SwapModifier.get_Value().get_PrimaryKey() == Keys.None || Module.Settings.SwapModifier.get_Value().get_IsTriggering())
				{
					c.Swap();
				}
			});
			((Control)switch_Image).add_MouseEntered((EventHandler<MouseEventArgs>)delegate
			{
				if (Module.Settings.SwapModifier.get_Value().get_PrimaryKey() == Keys.None || Module.Settings.SwapModifier.get_Value().get_IsTriggering())
				{
					characterControl.switch_Image.set_Texture(AsyncTexture2D.op_Implicit(Textures.Icons[21]));
				}
			});
			((Control)switch_Image).add_MouseLeft((EventHandler<MouseEventArgs>)delegate
			{
				characterControl.switch_Image.set_Texture(AsyncTexture2D.op_Implicit(Textures.Icons[12]));
			});
			((Control)this).add_MouseEntered((EventHandler<MouseEventArgs>)delegate
			{
				((Panel)characterControl).set_BackgroundTexture(AsyncTexture2D.op_Implicit(Textures.Icons[20]));
				((Control)characterControl.switch_Image).set_Visible(Module.Settings.SwapModifier.get_Value().get_PrimaryKey() == Keys.None || Module.Settings.SwapModifier.get_Value().get_IsTriggering());
			});
			((Control)this).add_MouseMoved((EventHandler<MouseEventArgs>)delegate
			{
				((Panel)characterControl).set_BackgroundTexture(AsyncTexture2D.op_Implicit(Textures.Icons[20]));
				((Control)characterControl.switch_Image).set_Visible(Module.Settings.SwapModifier.get_Value().get_PrimaryKey() == Keys.None || Module.Settings.SwapModifier.get_Value().get_IsTriggering());
			});
			((Control)this).add_MouseLeft((EventHandler<MouseEventArgs>)delegate
			{
				((Panel)characterControl).set_BackgroundTexture((AsyncTexture2D)null);
				((Control)characterControl.switch_Image).set_Visible(false);
			});
			((Control)this).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				if (((Control)Module.subWindow).get_Visible())
				{
					if (!((Control)characterControl.switch_Image).get_MouseOver() && Module.subWindow.assignedCharacter == characterControl.assignedCharacter)
					{
						((Control)Module.subWindow).Hide();
					}
					if (Module.subWindow.assignedCharacter != characterControl.assignedCharacter)
					{
						Module.subWindow.setCharacter(characterControl.assignedCharacter);
					}
					if (((Control)Module.ImageSelectorWindow).get_Visible())
					{
						Module.ImageSelectorWindow.assignedCharacter = characterControl.assignedCharacter;
					}
				}
				else if (!((Control)characterControl.switch_Image).get_MouseOver())
				{
					((Control)Module.subWindow).Show();
					((Control)Module.filterWindow).Hide();
					if (Module.subWindow.assignedCharacter != characterControl.assignedCharacter)
					{
						Module.subWindow.setCharacter(characterControl.assignedCharacter);
					}
					if (((Control)Module.ImageSelectorWindow).get_Visible())
					{
						Module.ImageSelectorWindow.assignedCharacter = characterControl.assignedCharacter;
					}
				}
			});
			((Control)this).add_Click((EventHandler<MouseEventArgs>)isDoubleClicked);
			((Control)this).add_Resized((EventHandler<ResizedEventArgs>)delegate
			{
				characterControl.AdjustLayout();
			});
		}

		private void isDoubleClicked(object sender, MouseEventArgs e)
		{
			if (e.get_IsDoubleClick() && Module.Settings.DoubleClickToEnter.get_Value() && (Module.Settings.SwapModifier.get_Value().get_PrimaryKey() == Keys.None || Module.Settings.SwapModifier.get_Value().get_IsTriggering()))
			{
				assignedCharacter.Swap();
			}
		}

		public void UpdateLanguage()
		{
			((CharacterTooltip)(object)((Control)this).get_Tooltip())._Update();
		}

		public void UpdateUI()
		{
			if (assignedCharacter.hadBirthdaySinceLogin())
			{
				((Control)birthday_Image).set_Visible(true);
				((Control)birthday_Image).set_BasicTooltipText(assignedCharacter.Name + " had Birthday! They are now " + assignedCharacter.Years + " years old.");
			}
			else
			{
				((Control)birthday_Image).set_Visible(false);
			}
			character_Image.set_Texture(AsyncTexture2D.op_Implicit(assignedCharacter.getProfessionTexture()));
			profession_Image.set_Texture(AsyncTexture2D.op_Implicit(assignedCharacter.getProfessionTexture(includeCustom: false, baseIcons: true)));
			TimeSpan t = TimeSpan.FromSeconds(assignedCharacter.seconds);
			time_Label.set_Text(string.Format("{3} " + common.Days + " {0:00}:{1:00}:{2:00}", t.Hours, t.Minutes, t.Seconds, t.Days));
			if (((Control)((Control)this).get_Tooltip()).get_Visible())
			{
				((CharacterTooltip)(object)((Control)this).get_Tooltip())._Update();
			}
			AdjustLayout();
		}

		private void AdjustLayout()
		{
			bool defaultIcon = assignedCharacter.Icon == null || assignedCharacter.Icon == "";
			((Control)character_Image).set_Location(defaultIcon ? new Point(_Padding + _FramePadding, _Padding + _FramePadding) : new Point(_Padding, _Padding));
			((Control)character_Image).set_Size(defaultIcon ? new Point(((Container)this).get_ContentRegion().Height - _Padding * 2 - _FramePadding * 2, ((Container)this).get_ContentRegion().Height - _Padding * 2 - _FramePadding * 2) : new Point(((Container)this).get_ContentRegion().Height - _Padding * 2, ((Container)this).get_ContentRegion().Height - _Padding * 2));
			((Control)border_BottomLeft).set_Visible(defaultIcon);
			((Control)border_TopRight).set_Visible(defaultIcon);
			Image referenceImage = (defaultIcon ? border_TopRight : character_Image);
			((Control)separator_Image).set_Location(new Point(((Control)referenceImage).get_Location().X + ((Control)referenceImage).get_Width() + _Padding, ((Container)this).get_ContentRegion().Height / 2 - (int)Math.Ceiling((decimal)_Padding / 2m)));
			((Control)separator_Image).set_Size(new Point(((Control)this).get_Width() - ((Control)referenceImage).get_Width() - _Padding * 3, 4));
			((Control)birthday_Image).set_Location(new Point(((Control)referenceImage).get_Location().X + ((Control)referenceImage).get_Width() - ((Control)birthday_Image).get_Width() + 4, ((Control)referenceImage).get_Location().Y + ((Control)referenceImage).get_Height() - ((Control)birthday_Image).get_Height() + 4));
			((Control)switch_Image).set_Location(new Point(((Control)this).get_Width() - ((Control)switch_Image).get_Width() - _Padding, (((Control)this).get_Height() - ((Control)switch_Image).get_Height() - _Padding) / 2 + 2));
			if (assignedCharacter.Crafting.Count > 0)
			{
				int iBegin = ((Control)character_Image).get_Location().X + ((Control)character_Image).get_Width() + _Padding * 3 + _FramePadding;
				int iWidth = (((Control)this).get_Width() - iBegin) / 2;
				((Control)crafting_Panel).set_Location(new Point(iBegin + iWidth, ((Control)time_Label).get_Location().Y + (((Control)time_Label).get_Height() - 24) / 2));
				((Container)crafting_Panel).set_WidthSizingMode((SizingMode)2);
			}
			((Control)level_Label).set_Location(new Point(((Control)character_Image).get_Location().X + 3, ((Control)character_Image).get_Location().Y + ((Control)character_Image).get_Height() - 10 - level_Label.get_Font().LineHeight));
			((Control)profession_Image).set_Location(new Point(((Control)character_Image).get_Location().X - 2, ((Control)character_Image).get_Location().Y + ((Control)character_Image).get_Height() - 38));
			((Control)profession_Image).set_Visible(!defaultIcon);
			((Control)background_Panel).set_Location(new Point(((Control)character_Image).get_Location().X, ((Control)character_Image).get_Location().Y + ((Control)character_Image).get_Height() - 38));
			((Control)background_Panel).set_Size(new Point(22, ((Control)character_Image).get_Height() - (((Control)character_Image).get_Location().Y + ((Control)character_Image).get_Height() - 41)));
			((Control)background_Panel).set_Visible(!defaultIcon);
		}
	}
}
