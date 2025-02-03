using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Kenedia.Modules.Characters.Models;
using Kenedia.Modules.Characters.Res;
using Kenedia.Modules.Characters.Services;
using Kenedia.Modules.Characters.Views;
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

		private readonly ImageButton _delete;

		private readonly List<Tag> _tags = new List<Tag>();

		private readonly ImageButton _closeButton;

		private readonly Kenedia.Modules.Core.Controls.Panel _tagContainer;

		private readonly Kenedia.Modules.Core.Controls.TextBox _tagBox;

		private readonly ImageButton _addTag;

		private readonly TagFlowPanel _tagPanel;

		private readonly ImageButton _image;

		private readonly Kenedia.Modules.Core.Controls.Label _name;

		private readonly Kenedia.Modules.Core.Controls.Checkbox _show;

		private readonly Kenedia.Modules.Core.Controls.Checkbox _radial;

		private readonly Kenedia.Modules.Core.Controls.Checkbox _beta;

		private readonly ImageButton _birthdayButton;

		private readonly Kenedia.Modules.Core.Controls.Panel _buttonContainer;

		private readonly Button _captureImages;

		private readonly Button _openFolder;

		private readonly Kenedia.Modules.Core.Controls.Panel _imagePanelParent;

		private readonly Kenedia.Modules.Core.Controls.FlowPanel _imagePanel;

		private readonly TagList _allTags;

		private readonly Settings _settings;

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

		public CharacterEdit(TextureManager tM, Action togglePotrait, Func<string> accountPath, TagList allTags, Settings settings, Action refreshCharacters)
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
			//IL_03a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0435: Unknown result type (might be due to invalid IL or missing references)
			//IL_0447: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_04fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_056c: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_064d: Unknown result type (might be due to invalid IL or missing references)
			//IL_065f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0701: Unknown result type (might be due to invalid IL or missing references)
			//IL_073c: Unknown result type (might be due to invalid IL or missing references)
			//IL_07c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_07d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_082f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0844: Unknown result type (might be due to invalid IL or missing references)
			//IL_0861: Unknown result type (might be due to invalid IL or missing references)
			//IL_0871: Unknown result type (might be due to invalid IL or missing references)
			//IL_087b: Unknown result type (might be due to invalid IL or missing references)
			//IL_089a: Unknown result type (might be due to invalid IL or missing references)
			//IL_08d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_08f0: Unknown result type (might be due to invalid IL or missing references)
			AccountImagePath = accountPath;
			_allTags = allTags;
			_settings = settings;
			_refreshCharacters = refreshCharacters;
			HeightSizingMode = SizingMode.AutoSize;
			WidthSizingMode = SizingMode.AutoSize;
			base.ContentPadding = new RectangleDimensions(5, 5, 5, 5);
			base.BackgroundImage = AsyncTexture2D.FromAssetId(156003);
			base.TextureRectangle = new Rectangle(26, 26, Math.Min(base.BackgroundImage.Width - 100, base.Width), Math.Min(base.BackgroundImage.Height - 100, base.Height));
			base.BorderColor = Color.get_Black();
			base.BorderWidth = new RectangleDimensions(2);
			new Dummy
			{
				Parent = this,
				Width = 355
			};
			_closeButton = new ImageButton
			{
				Parent = this,
				Size = new Point(20, 20),
				Location = new Point(335, 5),
				Texture = AsyncTexture2D.FromAssetId(156012),
				HoveredTexture = AsyncTexture2D.FromAssetId(156011),
				TextureRectangle = new Rectangle(7, 7, 20, 20),
				SetLocalizedTooltip = () => strings.Close,
				ClickAction = delegate
				{
					Hide();
				},
				ZIndex = 11
			};
			_image = new ImageButton
			{
				Parent = this,
				Texture = AsyncTexture2D.FromAssetId(358353),
				HoveredTexture = AsyncTexture2D.FromAssetId(358353),
				BackgroundColor = Color.get_Black() * 0.4f,
				Size = new Point(70, 70),
				ClickAction = delegate
				{
					ShowImages(!_imagePanelParent.Visible);
				}
			};
			_name = new Kenedia.Modules.Core.Controls.Label
			{
				Text = strings.CharacterName,
				Parent = this,
				TextColor = ContentService.Colors.ColonialWhite,
				Font = GameService.Content.DefaultFont16,
				AutoSizeWidth = true,
				Location = new Point(_image.Right + 5 + 2, 0)
			};
			_show = new Kenedia.Modules.Core.Controls.Checkbox
			{
				Parent = this,
				Location = new Point(_image.Right + 5 + 2, _name.Bottom + 5 + 2),
				Size = new Point(100, 21),
				SetLocalizedText = () => strings.ShowInList,
				CheckedChangedAction = delegate(bool b)
				{
					if (Character != null)
					{
						Character.Show = b;
					}
					_refreshCharacters?.Invoke();
				}
			};
			_radial = new Kenedia.Modules.Core.Controls.Checkbox
			{
				Parent = this,
				Location = new Point(_image.Right + 5 + 2, _show.Bottom),
				Size = new Point(100, 21),
				SetLocalizedText = () => strings.ShowOnRadial,
				SetLocalizedTooltip = () => strings.ShowOnRadial_Tooltip,
				CheckedChangedAction = delegate(bool b)
				{
					if (Character != null)
					{
						Character.ShowOnRadial = b;
					}
					_refreshCharacters?.Invoke();
				}
			};
			_beta = new Kenedia.Modules.Core.Controls.Checkbox
			{
				Parent = this,
				Location = new Point(_image.Right + 5 + 2, _radial.Bottom),
				Size = new Point(100, 21),
				SetLocalizedText = () => strings.IsBeta,
				SetLocalizedTooltip = () => strings.IsBeta_Tooltip,
				CheckedChangedAction = delegate(bool b)
				{
					if (Character != null)
					{
						Character.Beta = b;
					}
				}
			};
			_delete = new ImageButton
			{
				Parent = this,
				Size = new Point(40, 40),
				Location = new Point(315, 30),
				Texture = (AsyncTexture2D)tM.GetControlTexture(TextureManager.ControlTextures.Delete_Button),
				HoveredTexture = (AsyncTexture2D)tM.GetControlTexture(TextureManager.ControlTextures.Delete_Button_Hovered),
				SetLocalizedTooltip = () => string.Format(strings.DeleteItem, (Character != null) ? Character.Name : "Character"),
				ClickAction = new Action<MouseEventArgs>(ConfirmDelete),
				ZIndex = 11
			};
			int x = (355 - (_radial.Right + 5 + 2) - 48) / 2;
			_birthdayButton = new ImageButton
			{
				Parent = this,
				Location = new Point(_radial.Right + 5 + 2 + x, _name.Bottom),
				Size = new Point(48, 48),
				Texture = _presentTexture,
				HoveredTexture = _presentTextureOpen,
				ClickAction = delegate
				{
					Character.HadBirthday = false;
					_refreshCharacters?.Invoke();
					_birthdayButton.Hide();
				},
				SetLocalizedTooltip = () => (Character == null) ? string.Empty : (string.Format(strings.Birthday_Text, Character.Name, Character.Age) + "\n" + strings.ClickBirthdayToMarkAsOpen),
				Visible = false
			};
			_buttonContainer = new Kenedia.Modules.Core.Controls.Panel
			{
				Parent = this,
				Location = new Point(0, _beta.Bottom + 5 + 2),
				Width = 355,
				HeightSizingMode = SizingMode.AutoSize
			};
			_captureImages = new Button
			{
				Parent = _buttonContainer,
				Size = new Point(136, 25),
				Location = new Point(0),
				SetLocalizedText = () => strings.CaptureImages,
				SetLocalizedTooltip = () => strings.TogglePortraitCapture_Tooltip,
				Icon = tM.GetIcon(TextureManager.Icons.Camera),
				ResizeIcon = true,
				ClickAction = togglePotrait
			};
			_openFolder = new Button
			{
				Parent = _buttonContainer,
				Location = new Point(_captureImages.Right + 4, 0),
				Size = new Point(136, 25),
				SetLocalizedText = () => string.Format(strings.OpenItem, strings.Folder),
				SetLocalizedTooltip = () => strings.OpenPortraitFolder,
				Icon = tM.GetIcon(TextureManager.Icons.Folder),
				ResizeIcon = true,
				ClickAction = delegate
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
				}
			};
			_tagContainer = new Kenedia.Modules.Core.Controls.Panel
			{
				Parent = this,
				Location = new Point(0, _beta.Bottom + 5 + 2),
				Width = 355,
				HeightSizingMode = SizingMode.AutoSize
			};
			_tagBox = new Kenedia.Modules.Core.Controls.TextBox
			{
				Parent = _tagContainer,
				Size = new Point(329, 24),
				PlaceholderText = strings.Tag_Placeholder,
				TextChangedAction = delegate
				{
					LastInteraction = DateTime.Now;
				},
				EnterPressedAction = delegate(string t)
				{
					if (t != null && t.Length > 0 && !_allTags.Contains(t))
					{
						_allTags.Add(t);
						Character.AddTag(t);
						_tags.Add(AddTag(t, active: true));
						_refreshCharacters?.Invoke();
						_tagBox.Text = null;
					}
				}
			};
			_addTag = new ImageButton
			{
				Parent = _tagContainer,
				Texture = (AsyncTexture2D)tM.GetControlTexture(TextureManager.ControlTextures.Plus_Button),
				HoveredTexture = (AsyncTexture2D)tM.GetControlTexture(TextureManager.ControlTextures.Plus_Button_Hovered),
				Location = new Point(_tagBox.Right + 2, _tagBox.Top),
				Size = new Point(24, 24),
				BasicTooltipText = string.Format(strings.AddItem, strings.Tag),
				ClickAction = delegate
				{
					if (_tagBox.Text != null && _tagBox.Text.Length > 0 && !_allTags.Contains(_tagBox.Text))
					{
						_allTags.Add(_tagBox.Text);
						Character.AddTag(_tagBox.Text);
						_tags.Add(AddTag(_tagBox.Text, active: true));
						_refreshCharacters?.Invoke();
						_tagBox.Text = null;
					}
				}
			};
			_tagPanel = new TagFlowPanel
			{
				Parent = _tagContainer,
				Location = new Point(5, _tagBox.Bottom + 5),
				ControlPadding = new Vector2(3f, 2f)
			};
			_imagePanelParent = new Kenedia.Modules.Core.Controls.Panel
			{
				Parent = this,
				BorderColor = Color.get_Black(),
				BackgroundColor = Color.get_Black() * 0.4f,
				Location = new Point(0, _buttonContainer.Bottom + 10),
				BorderWidth = new RectangleDimensions(2),
				Visible = false
			};
			_imagePanel = new Kenedia.Modules.Core.Controls.FlowPanel
			{
				Parent = _imagePanelParent,
				ControlPadding = new Vector2(5f),
				ContentPadding = new RectangleDimensions(5),
				OuterControlPadding = new Vector2(0f),
				CanScroll = true,
				ZIndex = 11
			};
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			base.UpdateContainer(gameTime);
			if (_tagBox.Focused)
			{
				LastInteraction = DateTime.Now;
			}
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
					t.Dispose();
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
				base.TextureRectangle = new Rectangle(30, 30, Math.Min(base.BackgroundImage.Width - 100, base.Width), Math.Min(base.BackgroundImage.Height - 100, base.Height));
			}
			if (_tagPanel != null)
			{
				_tagPanel.FitWidestTag(355);
			}
		}

		public void LoadImages(object sender, EventArgs e)
		{
			string path = AccountImagePath?.Invoke();
			if (string.IsNullOrEmpty(path))
			{
				return;
			}
			List<string> images = new List<string>(Directory.GetFiles(path, "*.png", SearchOption.AllDirectories));
			_ = _settings.PanelSize.Value;
			int imageSize = 80;
			GameService.Graphics.QueueMainThreadRender(delegate(GraphicsDevice graphicsDevice)
			{
				//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
				//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
				//IL_022d: Unknown result type (might be due to invalid IL or missing references)
				//IL_023e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0248: Unknown result type (might be due to invalid IL or missing references)
				//IL_026a: Unknown result type (might be due to invalid IL or missing references)
				//IL_027b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0285: Unknown result type (might be due to invalid IL or missing references)
				//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
				//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
				//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
				//IL_0319: Unknown result type (might be due to invalid IL or missing references)
				AsyncTexture2D asyncTexture2D = null;
				if (Character == null)
				{
					Character_Model character_Model2 = (Character = (base.Anchor as MainWindow)?.CharacterCards.FirstOrDefault()?.Character);
				}
				if (Character != null)
				{
					_imagePanel.Children.Clear();
					asyncTexture2D = Character.SpecializationIcon;
					if (base.Anchor != null && base.Anchor.Visible)
					{
						(base.Anchor as MainWindow)?.ShowAttached(this);
						ShowImages(toggle: true, loadImages: false);
					}
					_noImgButton = new ImageButton
					{
						Parent = _imagePanel,
						Size = new Point(imageSize),
						Texture = asyncTexture2D,
						SetLocalizedTooltip = () => strings.SetSpecializationIcon,
						ClickAction = delegate
						{
							Character.IconPath = null;
							Character.Icon = null;
							ApplyCharacter();
						}
					};
					foreach (string p in images)
					{
						AsyncTexture2D texture = TextureUtil.FromStreamPremultiplied(graphicsDevice, new FileStream(p, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
						new ImageButton
						{
							Parent = _imagePanel,
							Size = new Point(imageSize),
							Texture = texture,
							ClickAction = delegate
							{
								Character.IconPath = p.Replace(Character.ModulePath, string.Empty);
								Character.Icon = texture;
								ApplyCharacter();
							}
						};
					}
					AdjustImagePanelHeight(images);
					_closeButton.Location = ((_imagePanelParent.Right > 355) ? new Point(_imagePanelParent.Right - _closeButton.Size.X, base.AutoSizePadding.Y) : new Point(355 - _closeButton.Size.X, base.AutoSizePadding.Y));
					_delete.Location = ((_imagePanelParent.Right > 355) ? new Point(_imagePanelParent.Right - _delete.Size.X, _delete.Top) : new Point(355 - _delete.Size.X, _delete.Top));
				}
			});
		}

		private void AdjustImagePanelHeight(List<string> images)
		{
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			int imageSize = 80;
			int maxHeight = GameService.Graphics.SpriteScreen.Height / 3;
			int cols = Math.Min(images.Count + 1, Math.Min(640, GameService.Graphics.SpriteScreen.Height / 3) / 80);
			int rows = (int)Math.Ceiling((double)(images.Count + 1) / (double)cols);
			int width = cols * imageSize + (cols - 1) * (int)_imagePanel.ControlPadding.X + (int)_imagePanel.OuterControlPadding.X + 30;
			int height = rows * imageSize + (rows - 1) * (int)_imagePanel.ControlPadding.Y + (int)_imagePanel.OuterControlPadding.Y + 10;
			_imagePanelParent.Width = width + 10;
			_imagePanelParent.Height = Math.Min(maxHeight, height);
			_imagePanel.Width = width;
			_imagePanel.Height = Math.Min(maxHeight, height);
			_imagePanel.Invalidate();
			_imagePanelParent.Invalidate();
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
			_closeButton?.Dispose();
			_imagePanel?.Dispose();
			_imagePanelParent?.Dispose();
		}

		public void ShowImages(bool toggle = true, bool loadImages = true)
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			if (loadImages && toggle)
			{
				LoadImages(null, null);
			}
			if (!toggle)
			{
				_closeButton.Location = new Point(355 - _closeButton.Size.X, base.AutoSizePadding.Y);
				_delete.Location = new Point(355 - _delete.Size.X, _delete.Top);
				_tagContainer.Show();
				_buttonContainer.Hide();
				_imagePanelParent.Hide();
				_imagePanelParent.Width = 0;
				_imagePanelParent.Height = 0;
			}
			else
			{
				_tagContainer.Hide();
				_buttonContainer.Show();
				_imagePanelParent.Show();
				_imagePanelParent.Invalidate();
			}
		}

		private void ApplyCharacter()
		{
			if (Character == null)
			{
				return;
			}
			_image.Texture = Character.Icon;
			_name.Text = Character.Name;
			_show.Checked = Character.Show;
			_beta.Checked = Character.Beta;
			_radial.Checked = Character.ShowOnRadial;
			_birthdayButton.BasicTooltipText = _birthdayButton.SetLocalizedTooltip?.Invoke();
			_birthdayButton.Visible = Character.HadBirthday;
			_delete.UserLocale_SettingChanged(null, null);
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
			Tag tag = new Tag
			{
				Text = txt,
				Parent = _tagPanel,
				Active = active,
				CanInteract = true,
				ShowDelete = false
			};
			tag.ActiveChanged += new EventHandler(Tag_ActiveChanged);
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

		private async void ConfirmDelete(MouseEventArgs m)
		{
			if (await new BaseDialog("Delete Character", "Are you sure to delete " + Character?.Name + "?").ShowDialog() == DialogResult.OK)
			{
				Character?.Delete();
				Hide();
			}
		}
	}
}
