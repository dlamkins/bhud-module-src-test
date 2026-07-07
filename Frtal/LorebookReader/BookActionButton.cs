using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Frtal.LorebookReader
{
	public sealed class BookActionButton : Control
	{
		private readonly Texture2D _icon;

		private readonly Texture2D _iconHover;

		private readonly Action _onClick;

		private bool _hovered;

		private bool _pressed;

		private static readonly Color BgNormal = new Color(10, 8, 4, 110);

		private static readonly Color BgHover = new Color(30, 23, 12, 160);

		private static readonly Color BgPressed = new Color(6, 4, 2, 180);

		private const int IconPad = 4;

		public BookActionButton(Texture2D icon, Texture2D iconHover, string tooltip, Action onClick)
			: this()
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			_icon = icon;
			_iconHover = iconHover ?? icon;
			_onClick = onClick;
			((Control)this).set_Size(new Point(40, 40));
			((Control)this).set_Visible(false);
			((Control)this).set_BasicTooltipText(tooltip);
		}

		protected override CaptureType CapturesInput()
		{
			return (CaptureType)4;
		}

		protected override void OnMouseEntered(MouseEventArgs e)
		{
			((Control)this).OnMouseEntered(e);
			_hovered = true;
		}

		protected override void OnMouseLeft(MouseEventArgs e)
		{
			((Control)this).OnMouseLeft(e);
			_hovered = false;
			_pressed = false;
		}

		protected override void OnLeftMouseButtonPressed(MouseEventArgs e)
		{
			((Control)this).OnLeftMouseButtonPressed(e);
			_pressed = true;
		}

		protected override void OnLeftMouseButtonReleased(MouseEventArgs e)
		{
			((Control)this).OnLeftMouseButtonReleased(e);
			if (_pressed)
			{
				_pressed = false;
				_onClick?.Invoke();
			}
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			Color bg = (_pressed ? BgPressed : (_hovered ? BgHover : BgNormal));
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), bounds, bg);
			Texture2D tex = (_hovered ? _iconHover : _icon);
			if (tex != null)
			{
				int off = (_pressed ? 1 : 0);
				Rectangle iconRect = default(Rectangle);
				((Rectangle)(ref iconRect))._002Ector(bounds.X + 4 + off, bounds.Y + 4 + off, bounds.Width - 8, bounds.Height - 8);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, tex, iconRect);
			}
		}
	}
}
