using System;
using System.Collections.Generic;
using Blish_HUD;
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

		public new int Padding
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
		{
			CharacterControl characterControl = this;
			ContentService contentService = new ContentService();
			assignedCharacter = c;
			base.Height = 76;
			WidthSizingMode = SizingMode.Fill;
			base.ShowBorder = false;
			base.Tooltip = new CharacterTooltip(assignedCharacter)
			{
				Parent = this
			};
			bool defaultIcon = assignedCharacter.Icon == null || assignedCharacter.Icon == "";
			border_TopRight = new Image
			{
				Parent = this,
				Location = new Point(_Padding, _Padding),
				Size = new Point(base.ContentRegion.Height - _Padding * 2, base.ContentRegion.Height - _Padding * 2),
				Texture = Textures.Backgrounds[7],
				Visible = defaultIcon
			};
			border_BottomLeft = new Image
			{
				Parent = this,
				Location = new Point(_Padding, _Padding),
				Size = new Point(base.ContentRegion.Height - _Padding * 2, base.ContentRegion.Height - _Padding * 2),
				Texture = Textures.Backgrounds[6],
				Visible = defaultIcon
			};
			character_Image = new Image
			{
				Texture = c.getProfessionTexture(),
				Location = (defaultIcon ? new Point(_Padding + _FramePadding, _Padding + _FramePadding) : new Point(_Padding, _Padding)),
				Size = (defaultIcon ? new Point(base.ContentRegion.Height - _Padding * 2 - _FramePadding * 2, base.ContentRegion.Height - _Padding * 2 - _FramePadding * 2) : new Point(base.ContentRegion.Height - _Padding * 2, base.ContentRegion.Height - _Padding * 2)),
				Parent = this,
				Tooltip = base.Tooltip
			};
			background_Panel = new Panel
			{
				Location = new Point(character_Image.Location.X, character_Image.Location.Y + character_Image.Height - 38),
				Size = new Point(22, character_Image.Height - (character_Image.Location.Y + character_Image.Height - 38)),
				Parent = this,
				BackgroundColor = new Color(43, 43, 43, 255)
			};
			level_Label = new Label
			{
				Parent = this,
				AutoSizeWidth = true,
				Location = new Point(character_Image.Location.X + 5, character_Image.Location.Y + character_Image.Height - 12),
				Font = contentService.GetFont(ContentService.FontFace.Menomonia, ContentService.FontSize.Size11, ContentService.FontStyle.Regular),
				Text = c.Level.ToString(),
				Tooltip = base.Tooltip,
				Height = (base.ContentRegion.Height - _Padding * 2) / 2,
				VerticalAlignment = VerticalAlignment.Middle
			};
			profession_Image = new Image
			{
				Texture = c.getProfessionTexture(includeCustom: false, baseIcons: true),
				Location = new Point(character_Image.Location.X, character_Image.Location.Y + character_Image.Height - 36),
				Size = new Point(24, 24),
				Parent = this,
				Tooltip = base.Tooltip
			};
			name_Label = new Label
			{
				Parent = this,
				AutoSizeWidth = true,
				Location = new Point(character_Image.Location.X + character_Image.Width + _Padding * 2 + _FramePadding, _Padding),
				Font = contentService.GetFont(ContentService.FontFace.Menomonia, ContentService.FontSize.Size18, ContentService.FontStyle.Regular),
				Text = c.Name,
				Tooltip = base.Tooltip,
				Height = (base.ContentRegion.Height - _Padding * 2) / 2,
				VerticalAlignment = VerticalAlignment.Middle
			};
			separator_Image = new Image
			{
				Location = new Point(character_Image.Location.X + character_Image.Width + _Padding + _FramePadding, base.ContentRegion.Height / 2 - (int)Math.Ceiling((decimal)_Padding / 2m)),
				Texture = Textures.Icons[19],
				Parent = this,
				Size = new Point(base.Width - character_Image.Width - 2 - _Padding * 3, (int)Math.Ceiling((decimal)_Padding / 2m)),
				Tooltip = base.Tooltip
			};
			time_Label = new Label
			{
				Location = new Point(character_Image.Location.X + character_Image.Width + _Padding * 2 + _FramePadding, base.ContentRegion.Height / 2 + _Padding - (contentService.DefaultFont18.LineHeight - contentService.DefaultFont12.LineHeight)),
				Text = "00:00:00",
				Parent = this,
				Height = (base.ContentRegion.Height - _Padding * 2) / 2,
				AutoSizeWidth = true,
				Font = contentService.GetFont(ContentService.FontFace.Menomonia, ContentService.FontSize.Size12, ContentService.FontStyle.Regular),
				VerticalAlignment = VerticalAlignment.Middle,
				Tooltip = base.Tooltip
			};
			birthday_Image = new Image
			{
				Texture = Textures.Icons[17],
				Parent = this,
				Location = new Point(base.Width - 150, base.ContentRegion.Height / 2 - 2),
				Size = new Point(32, 32),
				Visible = true
			};
			int iBegin = character_Image.Location.X + character_Image.Width + _Padding * 3 + _FramePadding;
			int iWidth = (base.Width - iBegin) / 2;
			if (c.Crafting.Count > 0)
			{
				crafting_Panel = new FlowPanel
				{
					Location = new Point(iBegin + iWidth, time_Label.Location.Y),
					Parent = this,
					Height = base.ContentRegion.Height,
					Width = (base.Width - (character_Image.Location.X + character_Image.Width + _Padding * 2 + _FramePadding)) / 2,
					FlowDirection = ControlFlowDirection.SingleLeftToRight,
					Tooltip = base.Tooltip
				};
				foreach (CharacterCrafting crafting in c.Crafting)
				{
					if (crafting.Active)
					{
						crafting_Images.Add(new DataImage
						{
							Texture = Textures.Crafting[crafting.Id],
							Size = new Point(24, 24),
							Parent = crafting_Panel,
							Visible = (!Module.Settings.OnlyMaxCrafting.Value || crafting.Id == 4 || (crafting.Id == 7 && crafting.Rating == 400) || crafting.Rating == 500),
							Id = crafting.Id,
							Crafting = crafting,
							Tooltip = base.Tooltip
						});
					}
				}
			}
			switch_Image = new Image
			{
				Location = new Point(base.Width - 45, 10),
				Texture = Textures.Icons[12],
				Size = new Point(32, 32),
				Parent = this,
				BasicTooltipText = string.Format(common.Switch, c.Name),
				Visible = false
			};
			switch_Image.Click += delegate
			{
				if (Module.Settings.SwapModifier.Value.PrimaryKey == Keys.None || Module.Settings.SwapModifier.Value.IsTriggering)
				{
					c.Swap();
				}
			};
			switch_Image.MouseEntered += delegate
			{
				if (Module.Settings.SwapModifier.Value.PrimaryKey == Keys.None || Module.Settings.SwapModifier.Value.IsTriggering)
				{
					characterControl.switch_Image.Texture = Textures.Icons[21];
				}
			};
			switch_Image.MouseLeft += delegate
			{
				characterControl.switch_Image.Texture = Textures.Icons[12];
			};
			base.MouseEntered += delegate
			{
				characterControl.BackgroundTexture = Textures.Icons[20];
				characterControl.switch_Image.Visible = Module.Settings.SwapModifier.Value.PrimaryKey == Keys.None || Module.Settings.SwapModifier.Value.IsTriggering;
			};
			base.MouseMoved += delegate
			{
				characterControl.BackgroundTexture = Textures.Icons[20];
				characterControl.switch_Image.Visible = Module.Settings.SwapModifier.Value.PrimaryKey == Keys.None || Module.Settings.SwapModifier.Value.IsTriggering;
			};
			base.MouseLeft += delegate
			{
				characterControl.BackgroundTexture = null;
				characterControl.switch_Image.Visible = false;
			};
			base.Click += delegate
			{
				if (Module.subWindow.Visible)
				{
					if (!characterControl.switch_Image.MouseOver && Module.subWindow.assignedCharacter == characterControl.assignedCharacter)
					{
						Module.subWindow.Hide();
					}
					if (Module.subWindow.assignedCharacter != characterControl.assignedCharacter)
					{
						Module.subWindow.setCharacter(characterControl.assignedCharacter);
					}
					if (Module.ImageSelectorWindow.Visible)
					{
						Module.ImageSelectorWindow.assignedCharacter = characterControl.assignedCharacter;
					}
				}
				else if (!characterControl.switch_Image.MouseOver)
				{
					Module.subWindow.Show();
					Module.filterWindow.Hide();
					if (Module.subWindow.assignedCharacter != characterControl.assignedCharacter)
					{
						Module.subWindow.setCharacter(characterControl.assignedCharacter);
					}
					if (Module.ImageSelectorWindow.Visible)
					{
						Module.ImageSelectorWindow.assignedCharacter = characterControl.assignedCharacter;
					}
				}
			};
			base.Click += isDoubleClicked;
			base.Resized += delegate
			{
				characterControl.AdjustLayout();
			};
		}

		private void isDoubleClicked(object sender, MouseEventArgs e)
		{
			if (e.IsDoubleClick && Module.Settings.DoubleClickToEnter.Value && (Module.Settings.SwapModifier.Value.PrimaryKey == Keys.None || Module.Settings.SwapModifier.Value.IsTriggering))
			{
				assignedCharacter.Swap();
			}
		}

		public void UpdateLanguage()
		{
			((CharacterTooltip)base.Tooltip)._Update();
		}

		public void UpdateUI()
		{
			if (assignedCharacter.hadBirthdaySinceLogin())
			{
				birthday_Image.Visible = true;
				birthday_Image.BasicTooltipText = assignedCharacter.Name + " had Birthday! They are now " + assignedCharacter.Years + " years old.";
			}
			else
			{
				birthday_Image.Visible = false;
			}
			character_Image.Texture = assignedCharacter.getProfessionTexture();
			profession_Image.Texture = assignedCharacter.getProfessionTexture(includeCustom: false, baseIcons: true);
			TimeSpan t = TimeSpan.FromSeconds(assignedCharacter.seconds);
			time_Label.Text = string.Format("{3} " + common.Days + " {0:00}:{1:00}:{2:00}", t.Hours, t.Minutes, t.Seconds, t.Days);
			if (base.Tooltip.Visible)
			{
				((CharacterTooltip)base.Tooltip)._Update();
			}
			AdjustLayout();
		}

		private void AdjustLayout()
		{
			bool defaultIcon = assignedCharacter.Icon == null || assignedCharacter.Icon == "";
			character_Image.Location = (defaultIcon ? new Point(_Padding + _FramePadding, _Padding + _FramePadding) : new Point(_Padding, _Padding));
			character_Image.Size = (defaultIcon ? new Point(base.ContentRegion.Height - _Padding * 2 - _FramePadding * 2, base.ContentRegion.Height - _Padding * 2 - _FramePadding * 2) : new Point(base.ContentRegion.Height - _Padding * 2, base.ContentRegion.Height - _Padding * 2));
			border_BottomLeft.Visible = defaultIcon;
			border_TopRight.Visible = defaultIcon;
			Image referenceImage = (defaultIcon ? border_TopRight : character_Image);
			separator_Image.Location = new Point(referenceImage.Location.X + referenceImage.Width + _Padding, base.ContentRegion.Height / 2 - (int)Math.Ceiling((decimal)_Padding / 2m));
			separator_Image.Size = new Point(base.Width - referenceImage.Width - _Padding * 3, 4);
			birthday_Image.Location = new Point(referenceImage.Location.X + referenceImage.Width - birthday_Image.Width + 4, referenceImage.Location.Y + referenceImage.Height - birthday_Image.Height + 4);
			switch_Image.Location = new Point(base.Width - switch_Image.Width - _Padding, (base.Height - switch_Image.Height - _Padding) / 2 + 2);
			if (assignedCharacter.Crafting.Count > 0)
			{
				int iBegin = character_Image.Location.X + character_Image.Width + _Padding * 3 + _FramePadding;
				int iWidth = (base.Width - iBegin) / 2;
				crafting_Panel.Location = new Point(iBegin + iWidth, time_Label.Location.Y + (time_Label.Height - 24) / 2);
				crafting_Panel.WidthSizingMode = SizingMode.Fill;
			}
			level_Label.Location = new Point(character_Image.Location.X + 3, character_Image.Location.Y + character_Image.Height - 10 - level_Label.Font.LineHeight);
			profession_Image.Location = new Point(character_Image.Location.X - 2, character_Image.Location.Y + character_Image.Height - 38);
			profession_Image.Visible = !defaultIcon;
			background_Panel.Location = new Point(character_Image.Location.X, character_Image.Location.Y + character_Image.Height - 38);
			background_Panel.Size = new Point(22, character_Image.Height - (character_Image.Location.Y + character_Image.Height - 41));
			background_Panel.Visible = !defaultIcon;
		}
	}
}
