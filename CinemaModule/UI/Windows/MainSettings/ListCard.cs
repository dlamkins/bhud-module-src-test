using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Controls.Effects;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CinemaModule.UI.Windows.MainSettings
{
	public class ListCard : Panel
	{
		private static readonly Logger Logger = Logger.GetLogger<ListCard>();

		public const int DefaultCardWidth = 450;

		public const int DefaultCardHeight = 60;

		public const int DefaultTextPanelWidth = 400;

		private const int ImageMaxHeight = 44;

		private const int ImageDefaultWidth = 44;

		private const int ImageMaxWidth = 80;

		private const int ImageLeft = 8;

		private const int ImageTop = 8;

		private const int TextPanelTop = 8;

		private const int TextPanelHeight = 48;

		private const int ButtonHeight = 28;

		private const int ButtonTop = 16;

		private const int ButtonSpacing = 5;

		private const int TitleIconSize = 18;

		private const int TitleIconSpacing = 4;

		private readonly Label _titleLabel;

		private readonly Label _subtitleLabel;

		private readonly Image _avatarImage;

		private readonly Image _titleIcon;

		private readonly FlowPanel _textPanel;

		private readonly Panel _titleRow;

		private readonly List<Control> _buttons = new List<Control>();

		private readonly object _buttonsLock = new object();

		private readonly ScrollingHighlightEffect _scrollEffect;

		private readonly bool _showAvatar;

		private bool _isSelected;

		private LoadingSpinner _loadingSpinner;

		public Label SubtitleLabel => _subtitleLabel;

		public string Title
		{
			get
			{
				return _titleLabel.get_Text();
			}
			set
			{
				_titleLabel.set_Text(value);
			}
		}

		public bool IsSelected
		{
			get
			{
				return _isSelected;
			}
			set
			{
				if (_isSelected != value)
				{
					_isSelected = value;
					UpdateSelectedState();
				}
			}
		}

		public ListCard(Container parent, string title, string subtitle, bool isSelected, int textPanelWidth = 400, IEnumerable<ListCardButton> buttons = null, AsyncTexture2D avatarTexture = null, AsyncTexture2D iconTexture = null, bool showAvatar = true)
			: this()
		{
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Expected O, but got Unknown
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Expected O, but got Unknown
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Expected O, but got Unknown
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			//IL_0128: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Expected O, but got Unknown
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0165: Unknown result type (might be due to invalid IL or missing references)
			//IL_016c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0174: Unknown result type (might be due to invalid IL or missing references)
			//IL_0185: Expected O, but got Unknown
			//IL_018d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0192: Unknown result type (might be due to invalid IL or missing references)
			//IL_0199: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01af: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d0: Expected O, but got Unknown
			//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0208: Unknown result type (might be due to invalid IL or missing references)
			//IL_0219: Expected O, but got Unknown
			((Container)this).set_WidthSizingMode((SizingMode)2);
			((Control)this).set_Height(60);
			((Control)this).set_Parent(parent);
			_isSelected = isSelected;
			_showAvatar = showAvatar;
			ScrollingHighlightEffect val = new ScrollingHighlightEffect((Control)(object)this);
			((ControlEffect)val).set_Size(new Vector2((float)((Control)this).get_Width(), (float)((Control)this).get_Height()));
			_scrollEffect = val;
			((Control)this).set_EffectBehind((ControlEffect)(object)_scrollEffect);
			UpdateSelectedState();
			int textPanelLeft = (_showAvatar ? 60 : 8);
			if (_showAvatar)
			{
				Image val2 = new Image();
				((Control)val2).set_Size(new Point(44, 44));
				((Control)val2).set_Left(8);
				((Control)val2).set_Top(8);
				val2.set_Texture(AsyncTexture2D.op_Implicit(Textures.get_TransparentPixel()));
				((Control)val2).set_Parent((Container)(object)this);
				_avatarImage = val2;
			}
			FlowPanel val3 = new FlowPanel();
			val3.set_FlowDirection((ControlFlowDirection)3);
			((Control)val3).set_Left(textPanelLeft);
			((Control)val3).set_Top(8);
			((Control)val3).set_Width(textPanelWidth);
			((Control)val3).set_Height(48);
			val3.set_ControlPadding(new Vector2(0f, 4f));
			((Control)val3).set_Parent((Container)(object)this);
			_textPanel = val3;
			Panel val4 = new Panel();
			((Control)val4).set_Height(20);
			((Container)val4).set_WidthSizingMode((SizingMode)2);
			((Control)val4).set_Parent((Container)(object)_textPanel);
			_titleRow = val4;
			int titleLeft = 0;
			int titleMaxWidth = textPanelWidth;
			if (iconTexture != null)
			{
				Image val5 = new Image();
				((Control)val5).set_Size(new Point(18, 18));
				((Control)val5).set_Left(0);
				((Control)val5).set_Top(1);
				val5.set_Texture(iconTexture);
				((Control)val5).set_Parent((Container)(object)_titleRow);
				_titleIcon = val5;
				titleLeft = 22;
				titleMaxWidth -= titleLeft;
			}
			Label val6 = new Label();
			val6.set_Text(title);
			((Control)val6).set_Height(20);
			((Control)val6).set_Width(titleMaxWidth);
			((Control)val6).set_Left(titleLeft);
			val6.set_Font(GameService.Content.get_DefaultFont16());
			((Control)val6).set_Parent((Container)(object)_titleRow);
			_titleLabel = val6;
			Label val7 = new Label();
			val7.set_Text(subtitle);
			((Control)val7).set_Height(18);
			((Control)val7).set_Width(textPanelWidth);
			val7.set_TextColor(Color.get_LightGray());
			val7.set_Font(GameService.Content.get_DefaultFont14());
			((Control)val7).set_Parent((Container)(object)_textPanel);
			_subtitleLabel = val7;
			UpdateTitleCentering();
			if (buttons != null)
			{
				CreateButtonControls(buttons);
			}
			if (avatarTexture != null)
			{
				SetAvatar(avatarTexture);
			}
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			((Panel)this).PaintBeforeChildren(spriteBatch, bounds);
			AsyncTexture2D menuItemFade = CinemaModule.Instance.TextureService?.GetMenuItemFade();
			if (menuItemFade != null && menuItemFade.get_HasTexture() && ShouldDrawDarkStripe())
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(menuItemFade), bounds, Color.get_Black() * 0.4f);
			}
		}

		private bool ShouldDrawDarkStripe()
		{
			if (((Control)this).get_Parent() == null)
			{
				return false;
			}
			int index = 0;
			foreach (Control child in ((Control)this).get_Parent().get_Children())
			{
				if (child == this)
				{
					break;
				}
				if (child is ListCard)
				{
					index++;
				}
			}
			return index % 2 == 0;
		}

		protected override void OnMouseEntered(MouseEventArgs e)
		{
			((Control)this).OnMouseEntered(e);
			((ControlEffect)_scrollEffect).Enable();
		}

		protected override void OnMouseLeft(MouseEventArgs e)
		{
			((Control)this).OnMouseLeft(e);
			if (!_isSelected)
			{
				((ControlEffect)_scrollEffect).Disable();
			}
		}

		private void UpdateSelectedState()
		{
			_scrollEffect.set_ForceActive(_isSelected);
		}

		protected override void OnClick(MouseEventArgs e)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			Point mousePos = e.get_MousePosition();
			Control[] buttonsCopy;
			lock (_buttonsLock)
			{
				buttonsCopy = _buttons.ToArray();
			}
			Control[] array = buttonsCopy;
			for (int i = 0; i < array.Length; i++)
			{
				Rectangle absoluteBounds = array[i].get_AbsoluteBounds();
				if (((Rectangle)(ref absoluteBounds)).Contains(mousePos))
				{
					return;
				}
			}
			((Panel)this).OnClick(e);
		}

		private void CreateButtonControls(IEnumerable<ListCardButton> buttonConfigs)
		{
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Expected O, but got Unknown
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0142: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0164: Expected O, but got Unknown
			int rightPosition = ((Control)this).get_Width() - 5 - 15;
			foreach (ListCardButton buttonConfig in buttonConfigs)
			{
				rightPosition -= buttonConfig.Width;
				if (buttonConfig.Icon != null && string.IsNullOrEmpty(buttonConfig.Text))
				{
					GlowButton val = new GlowButton();
					val.set_Icon(buttonConfig.Icon);
					((Control)val).set_Size(new Point(buttonConfig.Width, 28));
					((Control)val).set_Left(rightPosition);
					((Control)val).set_Top(16);
					((Control)val).set_BasicTooltipText(buttonConfig.Tooltip);
					((Control)val).set_Parent((Container)(object)this);
					GlowButton glowButton = val;
					if (buttonConfig.OnClick != null)
					{
						((Control)glowButton).add_Click((EventHandler<MouseEventArgs>)delegate
						{
							buttonConfig.OnClick();
						});
					}
					lock (_buttonsLock)
					{
						_buttons.Add((Control)(object)glowButton);
					}
				}
				else
				{
					StandardButton val2 = new StandardButton();
					val2.set_Text(buttonConfig.Text ?? "");
					((Control)val2).set_Width(buttonConfig.Width);
					((Control)val2).set_Height(28);
					((Control)val2).set_Left(rightPosition);
					((Control)val2).set_Top(16);
					((Control)val2).set_BasicTooltipText(buttonConfig.Tooltip);
					((Control)val2).set_Parent((Container)(object)this);
					StandardButton button = val2;
					if (buttonConfig.Icon != null)
					{
						button.set_Icon(buttonConfig.Icon);
					}
					if (buttonConfig.OnClick != null)
					{
						((Control)button).add_Click((EventHandler<MouseEventArgs>)delegate
						{
							buttonConfig.OnClick();
						});
					}
					lock (_buttonsLock)
					{
						_buttons.Add((Control)(object)button);
					}
				}
				rightPosition -= 5;
			}
		}

		public override void RecalculateLayout()
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			((Panel)this).RecalculateLayout();
			if (_scrollEffect != null)
			{
				((ControlEffect)_scrollEffect).set_Size(new Vector2((float)((Control)this).get_Width(), (float)((Control)this).get_Height()));
				UpdateTextPanelWidth();
				RepositionButtons();
			}
		}

		private void UpdateTextPanelWidth()
		{
			if (_textPanel == null || _titleLabel == null || _subtitleLabel == null)
			{
				return;
			}
			int totalButtonWidth = 0;
			lock (_buttonsLock)
			{
				foreach (Control button in _buttons)
				{
					totalButtonWidth += button.get_Width() + 5;
				}
			}
			int availableWidth = ((Control)this).get_Width() - ((Control)_textPanel).get_Left() - totalButtonWidth - 5 - 20;
			if (availableWidth > 0)
			{
				((Control)_textPanel).set_Width(availableWidth);
				((Control)_titleLabel).set_Width(availableWidth - ((((Control)_titleLabel).get_Left() > 0) ? ((Control)_titleLabel).get_Left() : 0));
				((Control)_subtitleLabel).set_Width(availableWidth);
				((Control)_textPanel).Invalidate();
			}
		}

		private void RepositionButtons()
		{
			Control[] buttonsCopy;
			lock (_buttonsLock)
			{
				if (_buttons.Count == 0)
				{
					return;
				}
				buttonsCopy = _buttons.ToArray();
			}
			int rightPosition = ((Control)this).get_Width() - 5 - 15;
			Control[] array = buttonsCopy;
			foreach (Control button in array)
			{
				rightPosition -= button.get_Width();
				button.set_Left(rightPosition);
				rightPosition -= 5;
			}
		}

		public void SetSubtitle(string text, Color? color = null)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			_subtitleLabel.set_Text(text);
			if (color.HasValue)
			{
				_subtitleLabel.set_TextColor(color.Value);
			}
			UpdateTitleCentering();
		}

		public void SetAvatar(AsyncTexture2D texture)
		{
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			if (_avatarImage == null)
			{
				return;
			}
			if (texture == null)
			{
				_avatarImage.set_Texture((AsyncTexture2D)null);
				((Control)_avatarImage).set_Size(new Point(44, 44));
				((Control)_textPanel).set_Left(60);
				return;
			}
			_avatarImage.set_Texture(texture);
			if (texture.get_Texture() != null)
			{
				UpdateAvatarSize(texture.get_Texture().get_Width(), texture.get_Texture().get_Height());
			}
			else
			{
				texture.add_TextureSwapped((EventHandler<ValueChangedEventArgs<Texture2D>>)OnAvatarTextureSwapped);
			}
		}

		private void OnAvatarTextureSwapped(object sender, ValueChangedEventArgs<Texture2D> e)
		{
			AsyncTexture2D asyncTexture = (AsyncTexture2D)((sender is AsyncTexture2D) ? sender : null);
			if (asyncTexture != null)
			{
				asyncTexture.remove_TextureSwapped((EventHandler<ValueChangedEventArgs<Texture2D>>)OnAvatarTextureSwapped);
			}
			if (e.get_NewValue() != null)
			{
				UpdateAvatarSize(e.get_NewValue().get_Width(), e.get_NewValue().get_Height());
			}
		}

		private void UpdateAvatarSize(int textureWidth, int textureHeight)
		{
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			if (_avatarImage == null || textureWidth <= 0 || textureHeight <= 0)
			{
				if (textureWidth <= 0 || textureHeight <= 0)
				{
					Logger.Warn($"Invalid avatar dimensions: {textureWidth}x{textureHeight}");
				}
				return;
			}
			float aspectRatio = (float)textureWidth / (float)textureHeight;
			int newWidth = (int)(44f * aspectRatio);
			newWidth = Math.Min(newWidth, 80);
			newWidth = Math.Max(newWidth, 44);
			((Control)_avatarImage).set_Size(new Point(newWidth, 44));
			((Control)_textPanel).set_Left(8 + newWidth + 8);
		}

		private void UpdateTitleCentering()
		{
			bool hasSubtitle = !string.IsNullOrEmpty(_subtitleLabel.get_Text());
			((Control)_subtitleLabel).set_Visible(hasSubtitle);
			if (hasSubtitle)
			{
				((Control)_textPanel).set_Top(8);
			}
			else
			{
				((Control)_textPanel).set_Top(20);
			}
		}

		public void ShowLoading(bool show)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Expected O, but got Unknown
			if (show)
			{
				if (_loadingSpinner == null)
				{
					LoadingSpinner val = new LoadingSpinner();
					((Control)val).set_Size(new Point(32, 32));
					((Control)val).set_Left(((Control)_textPanel).get_Left());
					((Control)val).set_Top(14);
					((Control)val).set_Parent((Container)(object)this);
					_loadingSpinner = val;
				}
				((Control)_loadingSpinner).set_Visible(true);
				((Control)_textPanel).set_Visible(false);
			}
			else
			{
				if (_loadingSpinner != null)
				{
					((Control)_loadingSpinner).set_Visible(false);
				}
				((Control)_textPanel).set_Visible(true);
			}
		}
	}
}
