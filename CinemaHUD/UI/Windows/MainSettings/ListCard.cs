using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using CinemaModule;
using Microsoft.Xna.Framework;

namespace CinemaHUD.UI.Windows.MainSettings
{
	public class ListCard : Panel
	{
		public const int DefaultCardWidth = 450;

		public const int DefaultCardHeight = 60;

		public const int DefaultTextPanelWidth = 400;

		private const int ImageSize = 44;

		private const int ImageLeft = 8;

		private const int ImageTop = 8;

		private const int TextPanelTop = 8;

		private const int TextPanelHeight = 48;

		private const int ButtonHeight = 28;

		private const int ButtonTop = 16;

		private const int ButtonSpacing = 5;

		private const int TitleIconSize = 18;

		private const int TitleIconSpacing = 4;

		public static readonly Color SelectedColor = new Color(60, 90, 60, 200);

		public static readonly Color DefaultColor = new Color(45, 45, 48, 180);

		private readonly Label _titleLabel;

		private readonly Label _subtitleLabel;

		private readonly Image _avatarImage;

		private readonly Image _titleIcon;

		private readonly FlowPanel _textPanel;

		private readonly Panel _titleRow;

		private readonly List<StandardButton> _buttons = new List<StandardButton>();

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
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0021: Unknown result type (might be due to invalid IL or missing references)
				if (_isSelected != value)
				{
					_isSelected = value;
					((Control)this).set_BackgroundColor(_isSelected ? SelectedColor : DefaultColor);
				}
			}
		}

		public ListCard(Container parent, string title, string subtitle, bool isSelected, int textPanelWidth = 400, IEnumerable<ListCardButton> buttons = null, AsyncTexture2D avatarTexture = null, AsyncTexture2D iconTexture = null)
			: this()
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Expected O, but got Unknown
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Expected O, but got Unknown
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Expected O, but got Unknown
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_0128: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Expected O, but got Unknown
			//IL_015d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_0169: Unknown result type (might be due to invalid IL or missing references)
			//IL_0170: Unknown result type (might be due to invalid IL or missing references)
			//IL_0177: Unknown result type (might be due to invalid IL or missing references)
			//IL_017e: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_019f: Expected O, but got Unknown
			//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e6: Expected O, but got Unknown
			((Control)this).set_Width(450);
			((Control)this).set_Height(60);
			((Control)this).set_BackgroundColor(isSelected ? SelectedColor : DefaultColor);
			((Panel)this).set_BackgroundTexture(global::CinemaModule.CinemaModule.Instance.TextureService.GetCardBackground());
			((Control)this).set_Parent(parent);
			_isSelected = isSelected;
			int textPanelLeft = 60;
			Image val = new Image();
			((Control)val).set_Size(new Point(44, 44));
			((Control)val).set_Left(8);
			((Control)val).set_Top(8);
			val.set_Texture(avatarTexture ?? AsyncTexture2D.op_Implicit(Textures.get_TransparentPixel()));
			((Control)val).set_Parent((Container)(object)this);
			_avatarImage = val;
			FlowPanel val2 = new FlowPanel();
			val2.set_FlowDirection((ControlFlowDirection)3);
			((Control)val2).set_Left(textPanelLeft);
			((Control)val2).set_Top(8);
			((Control)val2).set_Width(textPanelWidth);
			((Control)val2).set_Height(48);
			val2.set_ControlPadding(new Vector2(0f, 4f));
			((Control)val2).set_Parent((Container)(object)this);
			_textPanel = val2;
			Panel val3 = new Panel();
			((Container)val3).set_HeightSizingMode((SizingMode)1);
			((Container)val3).set_WidthSizingMode((SizingMode)1);
			((Control)val3).set_Parent((Container)(object)_textPanel);
			_titleRow = val3;
			int titleLeft = 0;
			if (iconTexture != null)
			{
				Image val4 = new Image();
				((Control)val4).set_Size(new Point(18, 18));
				((Control)val4).set_Left(0);
				((Control)val4).set_Top(1);
				val4.set_Texture(iconTexture);
				((Control)val4).set_Parent((Container)(object)_titleRow);
				_titleIcon = val4;
				titleLeft = 22;
			}
			Label val5 = new Label();
			val5.set_Text(title);
			val5.set_AutoSizeHeight(true);
			val5.set_AutoSizeWidth(true);
			((Control)val5).set_Left(titleLeft);
			val5.set_Font(GameService.Content.get_DefaultFont16());
			((Control)val5).set_Parent((Container)(object)_titleRow);
			_titleLabel = val5;
			Label val6 = new Label();
			val6.set_Text(subtitle);
			val6.set_AutoSizeHeight(true);
			val6.set_AutoSizeWidth(true);
			val6.set_TextColor(Color.get_LightGray());
			val6.set_Font(GameService.Content.get_DefaultFont14());
			((Control)val6).set_Parent((Container)(object)_textPanel);
			_subtitleLabel = val6;
			UpdateTitleCentering();
			if (buttons != null)
			{
				AddButtons(buttons, textPanelWidth);
			}
		}

		protected override void OnClick(MouseEventArgs e)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			Point mousePos = e.get_MousePosition();
			foreach (StandardButton button in _buttons)
			{
				Rectangle absoluteBounds = ((Control)button).get_AbsoluteBounds();
				if (((Rectangle)(ref absoluteBounds)).Contains(mousePos))
				{
					return;
				}
			}
			((Panel)this).OnClick(e);
		}

		private void AddButtons(IEnumerable<ListCardButton> buttons, int textPanelWidth)
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Expected O, but got Unknown
			int rightPosition = ((Control)this).get_Width() - 5 - 10;
			foreach (ListCardButton buttonConfig in buttons)
			{
				rightPosition -= buttonConfig.Width;
				StandardButton val = new StandardButton();
				val.set_Text(buttonConfig.Text);
				((Control)val).set_Width(buttonConfig.Width);
				((Control)val).set_Height(28);
				((Control)val).set_Left(rightPosition);
				((Control)val).set_Top(16);
				((Control)val).set_Parent((Container)(object)this);
				StandardButton button = val;
				if (buttonConfig.OnClick != null)
				{
					((Control)button).add_Click((EventHandler<MouseEventArgs>)delegate
					{
						buttonConfig.OnClick();
					});
				}
				_buttons.Add(button);
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
			}
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
