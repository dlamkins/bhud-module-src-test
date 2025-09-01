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
				if (((Control)this).SetProperty<bool>(ref _isFavorite, value, false, "IsFavorite"))
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
			((Control)this).OnMouseMoved(e);
		}

		protected override void OnMoved(MovedEventArgs e)
		{
			InvalidateMousePosition();
			((Control)this).OnMoved(e);
		}

		private void InvalidateMousePosition()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			Point relPos = ((Control)this).get_RelativeMousePosition();
			_mouseOverFavButton = ((Rectangle)(ref _favButtonBounds)).Contains(relPos);
			_mouseOverDelButton = ((Rectangle)(ref _delButtonBounds)).Contains(relPos);
			_mouseOverInspect = ((Rectangle)(ref _inspectButtonBounds)).Contains(relPos);
			_mouseOverNameTextBox = ((Rectangle)(ref _nameTextBoxBounds)).Contains(relPos);
			if (_mouseOverFavButton)
			{
				((Control)this).set_BasicTooltipText(IsFavorite ? Resources.Unfavourite : Resources.Favourite);
			}
			else if (_mouseOverDelButton)
			{
				((Control)this).set_BasicTooltipText(Resources.Delete_Image_);
			}
			else if (_mouseOverInspect)
			{
				((Control)this).set_BasicTooltipText(Resources.Click_To_Zoom);
			}
			else
			{
				((Control)this).set_BasicTooltipText(Resources.Right_Click_to_Copy);
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
			((Control)this).OnLeftMouseButtonReleased(e);
			InvalidateMousePosition();
		}

		protected override void OnRightMouseButtonReleased(MouseEventArgs e)
		{
			if (File.Exists(base.FileName))
			{
				if (_mouseOverFavButton || _mouseOverDelButton || _mouseOverNameTextBox)
				{
					return;
				}
				if (TryLoadImage(out var tex))
				{
					tex.ToBitmap().SaveToClipboard(GetImageFormatFromExtension(base.FileName));
					ScreenNotification.ShowNotification(Resources.Copied_to_Clipboard_, (NotificationType)0, (Texture2D)null, 4);
					GameService.Content.PlaySoundEffectByName("color-change");
					((GraphicsResource)tex).Dispose();
				}
			}
			((Control)this).OnRightMouseButtonReleased(e);
		}

		private static ImageFormat GetImageFormatFromExtension(string extension)
		{
			string text = extension.ToUpperInvariant();
			if (text != null)
			{
				int length = text.Length;
				if (length != 4)
				{
					if (length == 5)
					{
						char c = text[1];
						if (c != 'J')
						{
							if (c == 'T' && text == ".TIFF")
							{
								goto IL_010a;
							}
						}
						else if (text == ".JPEG")
						{
							goto IL_00fe;
						}
					}
				}
				else
				{
					switch (text[1])
					{
					case 'B':
						if (!(text == ".BMP"))
						{
							goto IL_0116;
						}
						return ImageFormat.Bmp;
					case 'P':
						if (!(text == ".PNG"))
						{
							goto IL_0116;
						}
						return ImageFormat.Png;
					case 'J':
						break;
					case 'G':
						if (!(text == ".GIF"))
						{
							goto IL_0116;
						}
						return ImageFormat.Gif;
					case 'T':
						goto IL_00b6;
					case 'I':
						if (!(text == ".ICO"))
						{
							goto IL_0116;
						}
						return ImageFormat.Icon;
					default:
						goto IL_0116;
					}
					if (text == ".JPG")
					{
						goto IL_00fe;
					}
				}
			}
			goto IL_0116;
			IL_010a:
			return ImageFormat.Tiff;
			IL_0116:
			return ImageFormat.Png;
			IL_00b6:
			if (text == ".TIF")
			{
				goto IL_010a;
			}
			goto IL_0116;
			IL_00fe:
			return ImageFormat.Jpeg;
		}

		protected override void OnMouseLeft(MouseEventArgs e)
		{
			_mouseOverFavButton = false;
			_mouseOverDelButton = false;
			_mouseOverInspect = false;
			_mouseOverNameTextBox = false;
			((Control)this).OnMouseLeft(e);
		}

		private void CreateNameTextBox()
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Expected O, but got Unknown
			if (NameTextBox != null)
			{
				return;
			}
			TextBox val = new TextBox();
			((Control)val).set_Parent((Container)(object)this);
			((TextInputBase)val).set_MaxLength(50);
			((Control)val).set_Size(((Rectangle)(ref _nameTextBoxBounds)).get_Size());
			((Control)val).set_Location(((Rectangle)(ref _nameTextBoxBounds)).get_Location());
			((TextInputBase)val).set_Text(Path.GetFileNameWithoutExtension(base.FileName));
			((Control)val).set_BasicTooltipText(Resources.Rename_Image);
			NameTextBox = val;
			((TextInputBase)NameTextBox).add_InputFocusChanged((EventHandler<ValueEventArgs<bool>>)async delegate(object o, ValueEventArgs<bool> e)
			{
				if (!e.get_Value() && !((TextInputBase)NameTextBox).get_Text().Equals(Path.GetFileNameWithoutExtension(base.FileName)))
				{
					if (string.IsNullOrEmpty(((TextInputBase)NameTextBox).get_Text()))
					{
						ScreenNotification.ShowNotification(Resources.Image_name_cannot_be_empty_, (NotificationType)2, (Texture2D)null, 4);
						((TextInputBase)NameTextBox).set_Text(Path.GetFileNameWithoutExtension(base.FileName));
						GameService.Content.PlaySoundEffectByName("error");
					}
					else if (((TextInputBase)NameTextBox).get_Text().Length > 50)
					{
						ScreenNotification.ShowNotification(Resources.Please_enter_a_different_image_name_, (NotificationType)2, (Texture2D)null, 4);
						((TextInputBase)NameTextBox).set_Text(Path.GetFileNameWithoutExtension(base.FileName));
						GameService.Content.PlaySoundEffectByName("error");
					}
					else if (((TextInputBase)NameTextBox).get_Text().Any((char x) => _invalidFileNameCharacters.Any((char y) => y.Equals(x))))
					{
						ScreenNotification.ShowNotification(Resources.The_image_name_contains_invalid_characters_, (NotificationType)2, (Texture2D)null, 4);
						((TextInputBase)NameTextBox).set_Text(Path.GetFileNameWithoutExtension(base.FileName));
						GameService.Content.PlaySoundEffectByName("error");
					}
					else
					{
						string ext = Path.GetExtension(base.FileName);
						string path = Path.GetDirectoryName(base.FileName);
						if (path != null)
						{
							string newName = Path.Combine(path, ((TextInputBase)NameTextBox).get_Text() + ext);
							if (File.Exists(newName))
							{
								ScreenNotification.ShowNotification(Resources.A_duplicate_image_name_was_specified_, (NotificationType)2, (Texture2D)null, 4);
								((TextInputBase)NameTextBox).set_Text(Path.GetFileNameWithoutExtension(base.FileName));
								GameService.Content.PlaySoundEffectByName("error");
							}
							else if (!(await FileUtil.MoveAsync(base.FileName, newName)))
							{
								ScreenNotification.ShowNotification(string.Format(Resources.Unable_to_rename_image__0__, "“" + Path.GetFileNameWithoutExtension(base.FileName) + "”"), (NotificationType)2, (Texture2D)null, 4);
								((TextInputBase)NameTextBox).set_Text(Path.GetFileNameWithoutExtension(base.FileName));
								GameService.Content.PlaySoundEffectByName("error");
							}
							else
							{
								base.FileName = newName;
							}
						}
					}
				}
			});
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
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), bounds, Color.get_Black() * 0.8f);
			}
			_inspectButtonBounds = new Rectangle((((Control)this).get_Width() - _inspectIcon.get_Width() / 2) / 2, (((Control)this).get_Height() - _inspectIcon.get_Height() / 2) / 2, _inspectIcon.get_Width() / 2, _inspectIcon.get_Height() / 2);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _inspectIcon, _inspectButtonBounds, Color.get_White() * (_mouseOverInspect ? 1f : 0.25f));
			Texture2D delTexture = (_mouseOverDelButton ? _trashcanOpenIcon64 : _trashcanClosedIcon64);
			_nameTextBoxBounds = new Rectangle(0, ((Control)this).get_Height() - 30, ((Control)this).get_Width() - delTexture.get_Width() / 2 - 5, 30);
			Texture2D favTexture = (IsFavorite ? _completeHeartIcon : _incompleteHeartIcon);
			_favButtonBounds = new Rectangle(((Control)this).get_Width() - favTexture.get_Width() - 10, 5, favTexture.get_Width(), favTexture.get_Height());
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, favTexture, _favButtonBounds);
			_delButtonBounds = new Rectangle(((Control)this).get_Width() - delTexture.get_Width() / 2 - 8, ((Control)this).get_Height() - delTexture.get_Height() / 2, delTexture.get_Width() / 2, delTexture.get_Height() / 2);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, delTexture, _delButtonBounds);
			CreateNameTextBox();
		}
	}
}
