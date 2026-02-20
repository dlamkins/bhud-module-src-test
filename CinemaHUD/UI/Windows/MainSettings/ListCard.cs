using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Controls.Effects;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CinemaHUD.UI.Windows.MainSettings
{
	public class ListCard : Panel
	{
		private static readonly Logger Logger = Logger.GetLogger<ListCard>();

		private static readonly AsyncTexture2D _textureMenuItemFade = AsyncTexture2D.FromAssetId(156044);

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

		private readonly ScrollingHighlightEffect _scrollEffect;

		private bool _isSelected;

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

		public ListCard(Container parent, string title, string subtitle, bool isSelected, int textPanelWidth = 400, IEnumerable<ListCardButton> buttons = null, AsyncTexture2D avatarTexture = null, AsyncTexture2D iconTexture = null)
			: this()
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Expected O, but got Unknown
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Expected O, but got Unknown
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Expected O, but got Unknown
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Expected O, but got Unknown
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			//IL_014e: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Expected O, but got Unknown
			//IL_0167: Unknown result type (might be due to invalid IL or missing references)
			//IL_016c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			//IL_017b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0182: Unknown result type (might be due to invalid IL or missing references)
			//IL_0189: Unknown result type (might be due to invalid IL or missing references)
			//IL_0199: Unknown result type (might be due to invalid IL or missing references)
			//IL_01aa: Expected O, but got Unknown
			//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f3: Expected O, but got Unknown
			((Container)this).set_WidthSizingMode((SizingMode)2);
			((Control)this).set_Height(60);
			((Control)this).set_Parent(parent);
			_isSelected = isSelected;
			ScrollingHighlightEffect val = new ScrollingHighlightEffect((Control)(object)this);
			((ControlEffect)val).set_Size(new Vector2((float)((Control)this).get_Width(), (float)((Control)this).get_Height()));
			_scrollEffect = val;
			((Control)this).set_EffectBehind((ControlEffect)(object)_scrollEffect);
			UpdateSelectedState();
			int textPanelLeft = 60;
			Image val2 = new Image();
			((Control)val2).set_Size(new Point(44, 44));
			((Control)val2).set_Left(8);
			((Control)val2).set_Top(8);
			val2.set_Texture(AsyncTexture2D.op_Implicit(Textures.get_TransparentPixel()));
			((Control)val2).set_Parent((Container)(object)this);
			_avatarImage = val2;
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
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			((Panel)this).PaintBeforeChildren(spriteBatch, bounds);
			if (_textureMenuItemFade.get_HasTexture() && ShouldDrawDarkStripe())
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_textureMenuItemFade), bounds, Color.get_Black() * 0.4f);
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
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			Point mousePos = e.get_MousePosition();
			foreach (Control button in _buttons)
			{
				Rectangle absoluteBounds = button.get_AbsoluteBounds();
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
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Expected O, but got Unknown
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
					_buttons.Add((Control)(object)glowButton);
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
					_buttons.Add((Control)(object)button);
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
			foreach (Control button in _buttons)
			{
				totalButtonWidth += button.get_Width() + 5;
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
			if (_buttons.Count == 0)
			{
				return;
			}
			List<Control> list = new List<Control>(_buttons);
			int rightPosition = ((Control)this).get_Width() - 5 - 15;
			foreach (Control button in list)
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
			if (texture != null)
			{
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
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			if (textureWidth <= 0 || textureHeight <= 0)
			{
				Logger.Warn($"Invalid avatar dimensions: {textureWidth}x{textureHeight}");
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
	}
}
