using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Kenedia.Modules.Characters.Models;
using Kenedia.Modules.Characters.Res;
using Kenedia.Modules.Characters.Services;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.Structs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.Characters.Controls
{
	public class CharacterEdit : AnchoredContainer
	{
		private readonly AsyncTexture2D _presentTexture = AsyncTexture2D.FromAssetId(593864);

		private readonly AsyncTexture2D _presentTextureOpen = AsyncTexture2D.FromAssetId(593865);

		private readonly List<Tag> _tags = new List<Tag>();

		private readonly ImageButton _closeButton;

		private readonly Panel _tagContainer;

		private readonly TextBox _tagBox;

		private readonly ImageButton _addTag;

		private readonly TagFlowPanel _tagPanel;

		private readonly ImageButton _image;

		private readonly Label _name;

		private readonly Checkbox _show;

		private readonly Checkbox _radial;

		private readonly ImageButton _birthdayButton;

		private readonly Panel _buttonContainer;

		private readonly Button _captureImages;

		private readonly Button _openFolder;

		private readonly Panel _imagePanelParent;

		private readonly FlowPanel _imagePanel;

		private readonly TagList _allTags;

		private readonly SettingsModel _settings;

		private readonly Action _refreshCharacters;

		private Character_Model _character;

		private ImageButton _noImgButton;

		public Func<string> AccountImagePath { get; set; }

		public Character_Model Character
		{
			get
			{
				return _character;
			}
			set
			{
				_character = value;
				ApplyCharacter();
			}
		}

		public CharacterEdit(TextureManager tM, Action togglePotrait, Func<string> accountPath, TagList allTags, SettingsModel settings, Action refreshCharacters)
		{
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_0149: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_021d: Unknown result type (might be due to invalid IL or missing references)
			//IL_024f: Unknown result type (might be due to invalid IL or missing references)
			//IL_028a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0299: Unknown result type (might be due to invalid IL or missing references)
			//IL_0307: Unknown result type (might be due to invalid IL or missing references)
			//IL_0316: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0445: Unknown result type (might be due to invalid IL or missing references)
			//IL_0480: Unknown result type (might be due to invalid IL or missing references)
			//IL_048c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0526: Unknown result type (might be due to invalid IL or missing references)
			//IL_0538: Unknown result type (might be due to invalid IL or missing references)
			//IL_05da: Unknown result type (might be due to invalid IL or missing references)
			//IL_0615: Unknown result type (might be due to invalid IL or missing references)
			//IL_0690: Unknown result type (might be due to invalid IL or missing references)
			//IL_069f: Unknown result type (might be due to invalid IL or missing references)
			//IL_06f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_070b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0728: Unknown result type (might be due to invalid IL or missing references)
			//IL_0738: Unknown result type (might be due to invalid IL or missing references)
			//IL_0742: Unknown result type (might be due to invalid IL or missing references)
			//IL_0761: Unknown result type (might be due to invalid IL or missing references)
			//IL_079b: Unknown result type (might be due to invalid IL or missing references)
			//IL_07b7: Unknown result type (might be due to invalid IL or missing references)
			AccountImagePath = accountPath;
			_allTags = allTags;
			_settings = settings;
			_refreshCharacters = refreshCharacters;
			((Container)this).set_HeightSizingMode((SizingMode)1);
			((Container)this).set_WidthSizingMode((SizingMode)1);
			base.ContentPadding = new RectangleDimensions(5, 5, 5, 5);
			base.BackgroundImage = AsyncTexture2D.FromAssetId(156003);
			base.TextureRectangle = new Rectangle(26, 26, Math.Min(base.BackgroundImage.get_Width() - 100, ((Control)this).get_Width()), Math.Min(base.BackgroundImage.get_Height() - 100, ((Control)this).get_Height()));
			base.BorderColor = Color.get_Black();
			base.BorderWidth = new RectangleDimensions(2);
			Dummy dummy = new Dummy();
			((Control)dummy).set_Parent((Container)(object)this);
			((Control)dummy).set_Width(355);
			ImageButton imageButton = new ImageButton();
			((Control)imageButton).set_Parent((Container)(object)this);
			((Control)imageButton).set_Size(new Point(20, 20));
			((Control)imageButton).set_Location(new Point(335, 5));
			imageButton.Texture = AsyncTexture2D.FromAssetId(156012);
			imageButton.HoveredTexture = AsyncTexture2D.FromAssetId(156011);
			imageButton.TextureRectangle = new Rectangle(7, 7, 20, 20);
			imageButton.SetLocalizedTooltip = () => strings.Close;
			imageButton.ClickAction = delegate
			{
				((Control)this).Hide();
			};
			((Control)imageButton).set_ZIndex(11);
			_closeButton = imageButton;
			ImageButton imageButton2 = new ImageButton();
			((Control)imageButton2).set_Parent((Container)(object)this);
			imageButton2.Texture = AsyncTexture2D.FromAssetId(358353);
			imageButton2.HoveredTexture = AsyncTexture2D.FromAssetId(358353);
			((Control)imageButton2).set_BackgroundColor(Color.get_Black() * 0.4f);
			((Control)imageButton2).set_Size(new Point(70, 70));
			imageButton2.ClickAction = delegate
			{
				ShowImages();
			};
			_image = imageButton2;
			Label label = new Label();
			((Label)label).set_Text(strings.CharacterName);
			((Control)label).set_Parent((Container)(object)this);
			((Label)label).set_TextColor(Colors.ColonialWhite);
			((Label)label).set_Font(GameService.Content.get_DefaultFont16());
			((Label)label).set_AutoSizeWidth(true);
			((Control)label).set_Location(new Point(((Control)_image).get_Right() + 5 + 2, 0));
			_name = label;
			Checkbox checkbox = new Checkbox();
			((Control)checkbox).set_Parent((Container)(object)this);
			((Control)checkbox).set_Location(new Point(((Control)_image).get_Right() + 5 + 2, ((Control)_name).get_Bottom() + 5 + 2));
			((Control)checkbox).set_Size(new Point(100, 21));
			checkbox.SetLocalizedText = () => strings.ShowInList;
			checkbox.CheckedChangedAction = delegate(bool b)
			{
				if (Character != null)
				{
					Character.Show = b;
				}
				_refreshCharacters?.Invoke();
			};
			_show = checkbox;
			Checkbox checkbox2 = new Checkbox();
			((Control)checkbox2).set_Parent((Container)(object)this);
			((Control)checkbox2).set_Location(new Point(((Control)_image).get_Right() + 5 + 2, ((Control)_show).get_Bottom()));
			((Control)checkbox2).set_Size(new Point(100, 21));
			checkbox2.SetLocalizedText = () => strings.ShowOnRadial;
			checkbox2.SetLocalizedTooltip = () => strings.ShowOnRadial_Tooltip;
			checkbox2.CheckedChangedAction = delegate(bool b)
			{
				if (Character != null)
				{
					Character.ShowOnRadial = b;
				}
				_refreshCharacters?.Invoke();
			};
			_radial = checkbox2;
			int x = (355 - (((Control)_radial).get_Right() + 5 + 2) - 48) / 2;
			ImageButton imageButton3 = new ImageButton();
			((Control)imageButton3).set_Parent((Container)(object)this);
			((Control)imageButton3).set_Location(new Point(((Control)_radial).get_Right() + 5 + 2 + x, ((Control)_name).get_Bottom()));
			((Control)imageButton3).set_Size(new Point(48, 48));
			imageButton3.Texture = _presentTexture;
			imageButton3.HoveredTexture = _presentTextureOpen;
			imageButton3.ClickAction = delegate
			{
				Character.HadBirthday = false;
				_refreshCharacters?.Invoke();
				((Control)_birthdayButton).Hide();
			};
			imageButton3.SetLocalizedTooltip = () => (Character == null) ? string.Empty : (string.Format(strings.Birthday_Text, Character.Name, Character.Age) + "\n" + strings.ClickBirthdayToMarkAsOpen);
			((Control)imageButton3).set_Visible(false);
			_birthdayButton = imageButton3;
			Panel panel = new Panel();
			((Control)panel).set_Parent((Container)(object)this);
			((Control)panel).set_Location(new Point(0, ((Control)_image).get_Bottom() + 5 + 2));
			((Control)panel).set_Width(355);
			((Container)panel).set_HeightSizingMode((SizingMode)1);
			_buttonContainer = panel;
			Button button = new Button();
			((Control)button).set_Parent((Container)(object)_buttonContainer);
			((Control)button).set_Size(new Point(136, 25));
			((Control)button).set_Location(new Point(0));
			button.SetLocalizedText = () => strings.CaptureImages;
			button.SetLocalizedTooltip = () => strings.TogglePortraitCapture_Tooltip;
			((StandardButton)button).set_Icon(AsyncTexture2D.op_Implicit(tM.GetIcon(TextureManager.Icons.Camera)));
			((StandardButton)button).set_ResizeIcon(true);
			button.ClickAction = togglePotrait;
			_captureImages = button;
			Button button2 = new Button();
			((Control)button2).set_Parent((Container)(object)_buttonContainer);
			((Control)button2).set_Location(new Point(((Control)_captureImages).get_Right() + 4, 0));
			((Control)button2).set_Size(new Point(136, 25));
			button2.SetLocalizedText = () => string.Format(strings.OpenItem, strings.Folder);
			button2.SetLocalizedTooltip = () => strings.OpenPortraitFolder;
			((StandardButton)button2).set_Icon(AsyncTexture2D.op_Implicit(tM.GetIcon(TextureManager.Icons.Folder)));
			((StandardButton)button2).set_ResizeIcon(true);
			button2.ClickAction = delegate
			{
				string text = AccountImagePath?.Invoke();
				if (!string.IsNullOrEmpty(text))
				{
					Process.Start(new ProcessStartInfo
					{
						Arguments = text,
						FileName = "explorer.exe"
					});
				}
			};
			_openFolder = button2;
			Panel panel2 = new Panel();
			((Control)panel2).set_Parent((Container)(object)this);
			((Control)panel2).set_Location(new Point(0, ((Control)_image).get_Bottom() + 5 + 2));
			((Control)panel2).set_Width(355);
			((Container)panel2).set_HeightSizingMode((SizingMode)1);
			_tagContainer = panel2;
			TextBox textBox = new TextBox();
			((Control)textBox).set_Parent((Container)(object)_tagContainer);
			((Control)textBox).set_Size(new Point(329, 24));
			((TextInputBase)textBox).set_PlaceholderText(strings.Tag_Placeholder);
			textBox.EnterPressedAction = delegate(string t)
			{
				if (t != null && t.Length > 0 && !_allTags.Contains(t))
				{
					_allTags.Add(t);
					Character.AddTag(t);
					_tags.Add(AddTag(t, active: true));
					_refreshCharacters?.Invoke();
					((TextInputBase)_tagBox).set_Text((string)null);
				}
			};
			_tagBox = textBox;
			ImageButton imageButton4 = new ImageButton();
			((Control)imageButton4).set_Parent((Container)(object)_tagContainer);
			imageButton4.Texture = AsyncTexture2D.op_Implicit(tM.GetControlTexture(TextureManager.ControlTextures.Plus_Button));
			imageButton4.HoveredTexture = AsyncTexture2D.op_Implicit(tM.GetControlTexture(TextureManager.ControlTextures.Plus_Button_Hovered));
			((Control)imageButton4).set_Location(new Point(((Control)_tagBox).get_Right() + 2, ((Control)_tagBox).get_Top()));
			((Control)imageButton4).set_Size(new Point(24, 24));
			((Control)imageButton4).set_BasicTooltipText(string.Format(strings.AddItem, strings.Tag));
			imageButton4.ClickAction = delegate
			{
				if (((TextInputBase)_tagBox).get_Text() != null && ((TextInputBase)_tagBox).get_Text().Length > 0 && !_allTags.Contains(((TextInputBase)_tagBox).get_Text()))
				{
					_allTags.Add(((TextInputBase)_tagBox).get_Text());
					Character.AddTag(((TextInputBase)_tagBox).get_Text());
					_tags.Add(AddTag(((TextInputBase)_tagBox).get_Text(), active: true));
					_refreshCharacters?.Invoke();
					((TextInputBase)_tagBox).set_Text((string)null);
				}
			};
			_addTag = imageButton4;
			TagFlowPanel tagFlowPanel = new TagFlowPanel();
			((Control)tagFlowPanel).set_Parent((Container)(object)_tagContainer);
			((Control)tagFlowPanel).set_Location(new Point(5, ((Control)_tagBox).get_Bottom() + 5));
			((FlowPanel)tagFlowPanel).set_ControlPadding(new Vector2(3f, 2f));
			_tagPanel = tagFlowPanel;
			Panel panel3 = new Panel();
			((Control)panel3).set_Parent((Container)(object)this);
			panel3.BorderColor = Color.get_Black();
			panel3.BackgroundColor = Color.get_Black() * 0.4f;
			((Control)panel3).set_Location(new Point(0, ((Control)_buttonContainer).get_Bottom() + 10));
			panel3.BorderWidth = new RectangleDimensions(2);
			((Control)panel3).set_Visible(false);
			_imagePanelParent = panel3;
			FlowPanel flowPanel = new FlowPanel();
			((Control)flowPanel).set_Parent((Container)(object)_imagePanelParent);
			((FlowPanel)flowPanel).set_ControlPadding(new Vector2(5f));
			flowPanel.ContentPadding = new RectangleDimensions(5);
			((FlowPanel)flowPanel).set_OuterControlPadding(new Vector2(0f));
			((Panel)flowPanel).set_CanScroll(true);
			((Control)flowPanel).set_ZIndex(11);
			_imagePanel = flowPanel;
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);
			ShowImages(toggle: false);
			IEnumerable<string> tagList = _tags.Select((Tag e) => e.Text);
			TagList allTags = _allTags;
			IEnumerable<string> deleteTags = tagList.Except(allTags);
			IEnumerable<string> addTags = allTags.Except(tagList);
			if (deleteTags.Any() || addTags.Any())
			{
				List<Tag> deleteList = new List<Tag>();
				foreach (string tag2 in deleteTags)
				{
					Tag t2 = _tags.FirstOrDefault((Tag e) => e.Text == tag2);
					if (t2 != null)
					{
						deleteList.Add(t2);
					}
				}
				foreach (Tag t in deleteList)
				{
					((Control)t).Dispose();
					_tags.Remove(t);
				}
				foreach (string tag in addTags)
				{
					AddTag(tag, Character != null && Character.Tags.Contains(tag));
				}
			}
			_tagPanel.FitWidestTag(355);
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			base.OnResized(e);
			if (base.BackgroundImage != null)
			{
				base.TextureRectangle = new Rectangle(30, 30, Math.Min(base.BackgroundImage.get_Width() - 100, ((Control)this).get_Width()), Math.Min(base.BackgroundImage.get_Height() - 100, ((Control)this).get_Height()));
			}
			if (_tagPanel != null)
			{
				_tagPanel.FitWidestTag(355);
			}
		}

		public void LoadImages(object sender, EventArgs e)
		{
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			string path = AccountImagePath?.Invoke();
			if (string.IsNullOrEmpty(path))
			{
				return;
			}
			List<string> images = new List<string>(Directory.GetFiles(path, "*.png", SearchOption.AllDirectories));
			_settings.PanelSize.get_Value();
			int imageSize = 80;
			int maxHeight = ((Control)GameService.Graphics.get_SpriteScreen()).get_Height() / 2;
			int width = (int)Math.Min(710f, Math.Min(((Control)GameService.Graphics.get_SpriteScreen()).get_Height() / 2, ((FlowPanel)_imagePanel).get_OuterControlPadding().Y * 2f + (float)(images.Count + 1) * ((float)imageSize + ((FlowPanel)_imagePanel).get_ControlPadding().X)));
			int height = (int)Math.Min(maxHeight, (double)(((FlowPanel)_imagePanel).get_OuterControlPadding().Y * 2f) + (double)(images.Count + 1) / Math.Floor((double)width / (double)imageSize) * (double)((float)imageSize + ((FlowPanel)_imagePanel).get_ControlPadding().Y));
			((Container)_imagePanel).get_Children().Clear();
			GameService.Graphics.QueueMainThreadRender((Action<GraphicsDevice>)delegate(GraphicsDevice graphicsDevice)
			{
				//IL_0056: Unknown result type (might be due to invalid IL or missing references)
				//IL_010c: Unknown result type (might be due to invalid IL or missing references)
				//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
				//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
				//IL_0206: Unknown result type (might be due to invalid IL or missing references)
				//IL_0228: Unknown result type (might be due to invalid IL or missing references)
				//IL_0239: Unknown result type (might be due to invalid IL or missing references)
				//IL_0243: Unknown result type (might be due to invalid IL or missing references)
				AsyncTexture2D val = null;
				if (((Control)this).get_Visible() && Character != null)
				{
					val = Character.SpecializationIcon;
					CharacterEdit characterEdit = this;
					ImageButton imageButton = new ImageButton();
					((Control)imageButton).set_Parent((Container)(object)_imagePanel);
					((Control)imageButton).set_Size(new Point(imageSize));
					imageButton.Texture = val;
					imageButton.SetLocalizedTooltip = () => strings.SetSpecializationIcon;
					imageButton.ClickAction = delegate
					{
						Character.IconPath = null;
						Character.Icon = null;
						ApplyCharacter();
					};
					characterEdit._noImgButton = imageButton;
					foreach (string p in images)
					{
						AsyncTexture2D texture = AsyncTexture2D.op_Implicit(TextureUtil.FromStreamPremultiplied(graphicsDevice, (Stream)new FileStream(p, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)));
						ImageButton imageButton2 = new ImageButton();
						((Control)imageButton2).set_Parent((Container)(object)_imagePanel);
						((Control)imageButton2).set_Size(new Point(imageSize));
						imageButton2.Texture = texture;
						imageButton2.ClickAction = delegate
						{
							Character.IconPath = p.Replace(Character.ModulePath, string.Empty);
							Character.Icon = texture;
							ApplyCharacter();
						};
					}
					((Control)_imagePanel).set_Width(width);
					((Control)_imagePanel).set_Height(height);
					((Control)_imagePanel).Invalidate();
					((Control)_imagePanelParent).set_Width(width);
					((Control)_imagePanelParent).set_Height(height);
					((Control)_closeButton).set_Location((((Control)_imagePanelParent).get_Right() > 355) ? new Point(((Control)_imagePanelParent).get_Right() - ((Control)_closeButton).get_Size().X, ((Container)this).get_AutoSizePadding().Y) : new Point(355 - ((Control)_closeButton).get_Size().X, ((Container)this).get_AutoSizePadding().Y));
				}
			});
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
			ImageButton closeButton = _closeButton;
			if (closeButton != null)
			{
				((Control)closeButton).Dispose();
			}
			FlowPanel imagePanel = _imagePanel;
			if (imagePanel != null)
			{
				((Control)imagePanel).Dispose();
			}
			Panel imagePanelParent = _imagePanelParent;
			if (imagePanelParent != null)
			{
				((Control)imagePanelParent).Dispose();
			}
		}

		private void ShowImages(bool toggle = true)
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			if (((Control)_imagePanelParent).get_Visible() || !toggle)
			{
				((Control)_closeButton).set_Location(new Point(355 - ((Control)_closeButton).get_Size().X, ((Container)this).get_AutoSizePadding().Y));
				((Control)_tagContainer).Show();
				((Control)_buttonContainer).Hide();
				((Control)_imagePanelParent).Hide();
				((Control)_imagePanelParent).set_Width(0);
				((Control)_imagePanelParent).set_Height(0);
			}
			else
			{
				((Control)_tagContainer).Hide();
				((Control)_buttonContainer).Show();
				((Control)_imagePanelParent).Show();
				LoadImages(null, null);
			}
		}

		private void ApplyCharacter()
		{
			if (Character == null)
			{
				return;
			}
			_image.Texture = Character.Icon;
			((Label)_name).set_Text(Character.Name);
			((Checkbox)_show).set_Checked(Character.Show);
			((Checkbox)_radial).set_Checked(Character.ShowOnRadial);
			((Control)_birthdayButton).set_BasicTooltipText(_birthdayButton.SetLocalizedTooltip?.Invoke());
			((Control)_birthdayButton).set_Visible(Character.HadBirthday);
			foreach (Tag t in _tags)
			{
				t.SetActive(_character.Tags.Contains(t.Text));
			}
			if (_noImgButton != null)
			{
				_noImgButton.Texture = Character.SpecializationIcon;
			}
		}

		private Tag AddTag(string txt, bool active = false)
		{
			Tag obj = new Tag
			{
				Text = txt
			};
			((Control)obj).set_Parent((Container)(object)_tagPanel);
			obj.Active = active;
			obj.CanInteract = true;
			obj.ShowDelete = false;
			Tag tag = obj;
			tag.ActiveChanged += Tag_ActiveChanged;
			_tagPanel.FitWidestTag(355);
			_tags.Add(tag);
			return tag;
		}

		private void Tag_ActiveChanged(object sender, EventArgs e)
		{
			Tag tag = (Tag)sender;
			if (tag.Active && !_character.Tags.Contains(tag.Text))
			{
				_character.AddTag(tag.Text);
			}
			else
			{
				_character.RemoveTag(tag.Text);
			}
		}
	}
}
