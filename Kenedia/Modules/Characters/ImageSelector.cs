using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Controls;
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
			base.Width = width;
			base.Height = height;
			ContentService contentService = new ContentService();
			int iPadding = 5;
			Image image = new Image();
			image.Parent = this;
			image.Texture = Textures.Emblems[2];
			image.Size = new Point(64, 64);
			image.Click += delegate
			{
				Hide();
			};
			header_HeaderUnderlined = new HeaderUnderlined
			{
				Parent = this,
				Text = common.SelectImage,
				Location = new Point(72, iPadding),
				Font = contentService.DefaultFont32,
				Width = 300
			};
			character_Image = new Image
			{
				Parent = this,
				Size = new Point(75, 75),
				Location = new Point(base.Width - 75 - iPadding, iPadding),
				Texture = new Character().getProfessionTexture()
			};
			Panel panel = new Panel
			{
				Parent = this,
				Location = new Point(0, 75 + iPadding),
				Height = base.Height - iPadding - 75,
				Padding = new Thickness(8f, 8f),
				WidthSizingMode = SizingMode.Fill
			};
			selector_Image = new Image
			{
				Parent = panel,
				Size = new Point(104, 110),
				Location = new Point(0, 0),
				Texture = Textures.Backgrounds[17],
				Visible = true
			};
			name_Label = new Label
			{
				Parent = this,
				Text = "Character Name",
				AutoSizeWidth = true,
				Font = contentService.DefaultFont18,
				AutoSizeHeight = true,
				VerticalAlignment = VerticalAlignment.Top
			};
			name_Label.Location = new Point(base.Width - character_Image.Width - iPadding * 3 - name_Label.Width, iPadding);
			default_Button = new StandardButton
			{
				Parent = this,
				Text = common.UseDefault,
				Location = new Point(base.Width - character_Image.Width - iPadding * 3 - 125, iPadding + name_Label.Height + iPadding),
				Size = new Point(125, 25)
			};
			default_Button.Click += delegate
			{
				selected_Image = null;
				character_Image.Texture = assignedCharacter.getProfessionTexture(includeCustom: false);
			};
			refresh_Button = new StandardButton
			{
				Parent = this,
				Text = common.RefreshImages,
				Location = new Point(75, 55),
				Size = new Point(150, 25)
			};
			refresh_Button.Click += delegate
			{
				LoadImages();
			};
			create_Button = new StandardButton
			{
				Parent = this,
				Text = common.CreateImages,
				Location = new Point(230, 55),
				Size = new Point(150, 25)
			};
			create_Button.Click += delegate
			{
				Module.screenCaptureWindow.Visible = true;
				if (!GameService.GameIntegration.Gw2Instance.IsInGame)
				{
					Module.RECT lpRect = default(Module.RECT);
					Module.GetWindowRect(GameService.GameIntegration.Gw2Instance.Gw2WindowHandle, ref lpRect);
					Module.MoveWindow(GameService.GameIntegration.Gw2Instance.Gw2WindowHandle, lpRect.Left, lpRect.Top, 1100, 800, Repaint: false);
					Hide();
					Module.MainWidow.Hide();
					Module.screenCapture = true;
				}
			};
			save_Button = new StandardButton
			{
				Parent = this,
				Text = common.Save,
				Location = new Point(400, 10),
				Size = new Point(125, 25)
			};
			save_Button.Click += delegate
			{
				assignedCharacter.Icon = ((selected_Image != null) ? selected_Image.Name : "");
				assignedCharacter.characterControl.UpdateUI();
				Module.subWindow.setCharacter(assignedCharacter);
				assignedCharacter.Save();
				base.Visible = false;
			};
			cancel_Button = new StandardButton
			{
				Parent = this,
				Text = common.Cancel,
				Location = new Point(550, 10),
				Size = new Point(125, 25)
			};
			cancel_Button.Click += delegate
			{
				base.Visible = false;
			};
			images_Panel = new FlowPanel
			{
				Parent = this,
				Location = new Point(0, 75 + iPadding),
				Height = base.Height - iPadding - 75,
				CanScroll = true,
				ControlPadding = new Vector2(8f, 8f),
				ShowBorder = true,
				WidthSizingMode = SizingMode.Fill
			};
		}

		public void LoadImages()
		{
			Texture2D[] customImages = Textures.CustomImages;
			foreach (Texture2D pic in customImages)
			{
				if (pic != null && !ImageNames.Contains(pic.Name))
				{
					Image img = new Image
					{
						Parent = images_Panel,
						Size = new Point(96, 96),
						Texture = pic
					};
					img.MouseEntered += delegate
					{
						selector_Image.Location = new Point(img.Location.X, img.Location.Y - images_Panel.VerticalScrollOffset);
					};
					img.Click += delegate
					{
						character_Image.Texture = img.Texture;
						selected_Image = pic;
					};
					ImageNames.Add(pic.Name);
				}
			}
			images_Panel.Invalidate();
		}

		private void setCharacter()
		{
			name_Label.Text = assignedCharacter.Name;
			character_Image.Texture = assignedCharacter.getProfessionTexture();
		}

		public void UpdateLanguage()
		{
			header_HeaderUnderlined.Text = common.SelectImage;
			save_Button.Text = common.Save;
			create_Button.Text = common.CreateImages;
			refresh_Button.Text = common.RefreshImages;
			default_Button.Text = common.UseDefault;
			cancel_Button.Text = common.Cancel;
		}
	}
}
