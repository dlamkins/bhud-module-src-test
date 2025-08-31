using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nekres.Screenshot_Manager.Properties;

namespace Nekres.Screenshot_Manager.UI.Controls
{
	public class ResponsiveThumbnail : ThumbnailBase
	{
		private const int MAX_FILE_NAME_LENGTH = 50;

		private readonly IEnumerable<char> _invalidFileNameCharacters;

		private static Texture2D _completeHeartIcon;

		private static Texture2D _incompleteHeartIcon;

		private static Texture2D _trashcanClosedIcon64;

		private static Texture2D _trashcanOpenIcon64;

		private static Texture2D _inspectIcon;

		private bool _mouseOverNameTextBox;

		private Rectangle _nameTextBoxBounds;

		private bool _mouseOverFavButton;

		private Rectangle _favButtonBounds;

		private bool _mouseOverDelButton;

		private Rectangle _delButtonBounds;

		private bool _mouseOverInspect;

		private Rectangle _inspectButtonBounds;

		private bool _isFavorite;

		public TextBox NameTextBox { get; private set; }

		public bool IsFavorite
		{
			get
			{
				return _isFavorite;
			}
			set
			{
				if (SetProperty(ref _isFavorite, value, invalidateLayout: false, "IsFavorite"))
				{
					this.FavoriteChanged?.Invoke(this, new ValueEventArgs<bool>(value));
				}
			}
		}

		public event EventHandler<EventArgs> OnInspect;

		public event EventHandler<ValueEventArgs<bool>> FavoriteChanged;

		public event EventHandler<EventArgs> OnDelete;

		public static void DisposeTextures()
		{
			Texture2D completeHeartIcon = _completeHeartIcon;
			if (completeHeartIcon != null)
			{
				((GraphicsResource)completeHeartIcon).Dispose();
			}
			Texture2D incompleteHeartIcon = _incompleteHeartIcon;
			if (incompleteHeartIcon != null)
			{
				((GraphicsResource)incompleteHeartIcon).Dispose();
			}
			Texture2D trashcanClosedIcon = _trashcanClosedIcon64;
			if (trashcanClosedIcon != null)
			{
				((GraphicsResource)trashcanClosedIcon).Dispose();
			}
			Texture2D trashcanOpenIcon = _trashcanOpenIcon64;
			if (trashcanOpenIcon != null)
			{
				((GraphicsResource)trashcanOpenIcon).Dispose();
			}
			Texture2D inspectIcon = _inspectIcon;
			if (inspectIcon != null)
			{
				((GraphicsResource)inspectIcon).Dispose();
			}
		}

		public ResponsiveThumbnail(AsyncTexture2D texture, string fileName)
			: base(texture, fileName)
		{
			_invalidFileNameCharacters = Path.GetInvalidFileNameChars().Union(Path.GetInvalidPathChars());
			_completeHeartIcon = _completeHeartIcon ?? ScreenshotManagerModule.ModuleInstance.ContentsManager.GetTexture("complete_heart.png");
			_incompleteHeartIcon = _incompleteHeartIcon ?? ScreenshotManagerModule.ModuleInstance.ContentsManager.GetTexture("incomplete_heart.png");
			_trashcanClosedIcon64 = _trashcanClosedIcon64 ?? ScreenshotManagerModule.ModuleInstance.ContentsManager.GetTexture("trashcanClosed_icon_64x64.png");
			_trashcanOpenIcon64 = _trashcanOpenIcon64 ?? ScreenshotManagerModule.ModuleInstance.ContentsManager.GetTexture("trashcanOpen_icon_64x64.png");
			_inspectIcon = _inspectIcon ?? ScreenshotManagerModule.ModuleInstance.ContentsManager.GetTexture("inspect.png");
		}

		protected override void OnMouseMoved(MouseEventArgs e)
		{
			InvalidateMousePosition();
			base.OnMouseMoved(e);
		}

		protected override void OnMoved(MovedEventArgs e)
		{
			InvalidateMousePosition();
			base.OnMoved(e);
		}

		private void InvalidateMousePosition()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			Point relPos = base.RelativeMousePosition;
			_mouseOverFavButton = ((Rectangle)(ref _favButtonBounds)).Contains(relPos);
			_mouseOverDelButton = ((Rectangle)(ref _delButtonBounds)).Contains(relPos);
			_mouseOverInspect = ((Rectangle)(ref _inspectButtonBounds)).Contains(relPos);
			_mouseOverNameTextBox = ((Rectangle)(ref _nameTextBoxBounds)).Contains(relPos);
			if (_mouseOverFavButton)
			{
				base.BasicTooltipText = (IsFavorite ? Resources.Unfavourite : Resources.Favourite);
			}
			else if (_mouseOverDelButton)
			{
				base.BasicTooltipText = Resources.Delete_Image_;
			}
			else if (_mouseOverInspect)
			{
				base.BasicTooltipText = Resources.Click_To_Zoom;
			}
			else
			{
				base.BasicTooltipText = Resources.Right_Click_to_Copy;
			}
		}

		protected override void OnLeftMouseButtonReleased(MouseEventArgs e)
		{
			if (_mouseOverInspect)
			{
				this.OnInspect?.Invoke(this, EventArgs.Empty);
			}
			else if (_mouseOverFavButton)
			{
				IsFavorite = !IsFavorite;
				GameService.Content.PlaySoundEffectByName("color-change");
			}
			else if (_mouseOverDelButton)
			{
				this.OnDelete?.Invoke(this, EventArgs.Empty);
			}
			base.OnLeftMouseButtonReleased(e);
			InvalidateMousePosition();
		}

		protected override void OnRightMouseButtonReleased(MouseEventArgs e)
		{
			if (base.Texture.HasTexture)
			{
				if (_mouseOverFavButton || _mouseOverDelButton || _mouseOverNameTextBox)
				{
					return;
				}
				base.Texture.Texture.ToBitmap().SaveToClipboard(ImageFormat.Bmp);
				ScreenNotification.ShowNotification(Resources.Copied_to_Clipboard_);
				GameService.Content.PlaySoundEffectByName("color-change");
			}
			base.OnRightMouseButtonReleased(e);
		}

		protected override void OnMouseLeft(MouseEventArgs e)
		{
			_mouseOverFavButton = false;
			_mouseOverDelButton = false;
			_mouseOverInspect = false;
			_mouseOverNameTextBox = false;
			base.OnMouseLeft(e);
		}

		private void CreateNameTextBox()
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			if (NameTextBox != null)
			{
				return;
			}
			NameTextBox = new TextBox
			{
				Parent = this,
				MaxLength = 50,
				Size = ((Rectangle)(ref _nameTextBoxBounds)).get_Size(),
				Location = ((Rectangle)(ref _nameTextBoxBounds)).get_Location(),
				Text = Path.GetFileNameWithoutExtension(base.FileName),
				BasicTooltipText = Resources.Rename_Image
			};
			NameTextBox.InputFocusChanged += async delegate(object o, ValueEventArgs<bool> e)
			{
				if (!e.Value && !NameTextBox.Text.Equals(Path.GetFileNameWithoutExtension(base.FileName)))
				{
					if (string.IsNullOrEmpty(NameTextBox.Text))
					{
						ScreenNotification.ShowNotification(Resources.Image_name_cannot_be_empty_, ScreenNotification.NotificationType.Error);
						NameTextBox.Text = Path.GetFileNameWithoutExtension(base.FileName);
						GameService.Content.PlaySoundEffectByName("error");
					}
					else if (NameTextBox.Text.Length > 50)
					{
						ScreenNotification.ShowNotification(Resources.Please_enter_a_different_image_name_, ScreenNotification.NotificationType.Error);
						NameTextBox.Text = Path.GetFileNameWithoutExtension(base.FileName);
						GameService.Content.PlaySoundEffectByName("error");
					}
					else if (NameTextBox.Text.Any((char x) => _invalidFileNameCharacters.Any((char y) => y.Equals(x))))
					{
						ScreenNotification.ShowNotification(Resources.The_image_name_contains_invalid_characters_, ScreenNotification.NotificationType.Error);
						NameTextBox.Text = Path.GetFileNameWithoutExtension(base.FileName);
						GameService.Content.PlaySoundEffectByName("error");
					}
					else
					{
						string ext = Path.GetExtension(base.FileName);
						string path = Path.GetDirectoryName(base.FileName);
						if (path != null)
						{
							string newName = Path.Combine(path, NameTextBox.Text + ext);
							if (File.Exists(newName))
							{
								ScreenNotification.ShowNotification(Resources.A_duplicate_image_name_was_specified_, ScreenNotification.NotificationType.Error);
								NameTextBox.Text = Path.GetFileNameWithoutExtension(base.FileName);
								GameService.Content.PlaySoundEffectByName("error");
							}
							else if (!(await FileUtil.MoveAsync(base.FileName, newName)))
							{
								ScreenNotification.ShowNotification(string.Format(Resources.Unable_to_rename_image__0__, "“" + Path.GetFileNameWithoutExtension(base.FileName) + "”"), ScreenNotification.NotificationType.Error);
								NameTextBox.Text = Path.GetFileNameWithoutExtension(base.FileName);
								GameService.Content.PlaySoundEffectByName("error");
							}
							else
							{
								base.FileName = newName;
							}
						}
					}
				}
			};
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_0167: Unknown result type (might be due to invalid IL or missing references)
			//IL_0170: Unknown result type (might be due to invalid IL or missing references)
			base.PaintBeforeChildren(spriteBatch, bounds);
			if (_mouseOverInspect)
			{
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, bounds, Color.get_Black() * 0.8f);
			}
			_inspectButtonBounds = new Rectangle((base.Width - _inspectIcon.get_Width() / 2) / 2, (base.Height - _inspectIcon.get_Height() / 2) / 2, _inspectIcon.get_Width() / 2, _inspectIcon.get_Height() / 2);
			spriteBatch.DrawOnCtrl(this, _inspectIcon, _inspectButtonBounds, Color.get_White() * (_mouseOverInspect ? 1f : 0.25f));
			Texture2D delTexture = (_mouseOverDelButton ? _trashcanOpenIcon64 : _trashcanClosedIcon64);
			_nameTextBoxBounds = new Rectangle(0, base.Height - 30, base.Width - delTexture.get_Width() / 2 - 5, 30);
			Texture2D favTexture = (IsFavorite ? _completeHeartIcon : _incompleteHeartIcon);
			_favButtonBounds = new Rectangle(base.Width - favTexture.get_Width() - 10, 5, favTexture.get_Width(), favTexture.get_Height());
			spriteBatch.DrawOnCtrl(this, favTexture, _favButtonBounds);
			_delButtonBounds = new Rectangle(base.Width - delTexture.get_Width() / 2 - 8, base.Height - delTexture.get_Height() / 2, delTexture.get_Width() / 2, delTexture.get_Height() / 2);
			spriteBatch.DrawOnCtrl(this, delTexture, _delButtonBounds);
			CreateNameTextBox();
		}
	}
}
