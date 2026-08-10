using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;

namespace Neokain.GW2.AllianceManager.Controls.Home
{
	internal class QuickActionTile : Panel
	{
		private const int TILE_WIDTH = 100;

		private const int TILE_HEIGHT = 90;

		private const int ICON_SIZE = 48;

		private const int PADDING = 8;

		private static readonly Color NormalBackgroundColor = new Color(40, 40, 40, 200);

		private static readonly Color HoverBackgroundColor = new Color(60, 60, 60, 220);

		private static readonly Color PressedBackgroundColor = new Color(80, 80, 80, 240);

		private readonly Image _tileIcon;

		private readonly Label _titleLabel;

		private bool _isHovered;

		private bool _isPressed;

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

		public event EventHandler Clicked;

		public QuickActionTile(AsyncTexture2D icon, string title)
			: this()
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Expected O, but got Unknown
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Expected O, but got Unknown
			((Control)this).set_Width(100);
			((Control)this).set_Height(90);
			((Control)this).set_BackgroundColor(NormalBackgroundColor);
			Image val = new Image(icon);
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Width(48);
			((Control)val).set_Height(48);
			((Control)val).set_Left(26);
			((Control)val).set_Top(8);
			_tileIcon = val;
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)this);
			val2.set_Text(title);
			val2.set_Font(GameService.Content.get_DefaultFont14());
			val2.set_TextColor(Color.get_White());
			val2.set_HorizontalAlignment((HorizontalAlignment)1);
			val2.set_AutoSizeWidth(false);
			val2.set_AutoSizeHeight(true);
			((Control)val2).set_Width(84);
			((Control)val2).set_Left(8);
			((Control)val2).set_Top(60);
			_titleLabel = val2;
		}

		protected override void OnMouseEntered(MouseEventArgs e)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			_isHovered = true;
			((Control)this).set_BackgroundColor(HoverBackgroundColor);
			((Control)this).OnMouseEntered(e);
		}

		protected override void OnMouseLeft(MouseEventArgs e)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			_isHovered = false;
			_isPressed = false;
			((Control)this).set_BackgroundColor(NormalBackgroundColor);
			((Control)this).OnMouseLeft(e);
		}

		protected override void OnLeftMouseButtonPressed(MouseEventArgs e)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			_isPressed = true;
			((Control)this).set_BackgroundColor(PressedBackgroundColor);
			((Control)this).OnLeftMouseButtonPressed(e);
		}

		protected override void OnLeftMouseButtonReleased(MouseEventArgs e)
		{
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			if (_isPressed && _isHovered)
			{
				this.Clicked?.Invoke(this, EventArgs.Empty);
			}
			_isPressed = false;
			((Control)this).set_BackgroundColor(_isHovered ? HoverBackgroundColor : NormalBackgroundColor);
			((Control)this).OnLeftMouseButtonReleased(e);
		}
	}
}
