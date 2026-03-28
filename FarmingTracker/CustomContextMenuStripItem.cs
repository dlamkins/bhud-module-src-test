using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Controls.Effects;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FarmingTracker
{
	public class CustomContextMenuStripItem : Control
	{
		private const int BULLET_SIZE = 18;

		private const int HORIZONTAL_PADDING = 6;

		private const int TEXT_LEFTPADDING = 30;

		private const string NOT_CLICKABLE_HEADER_TOOLTIP = "Header cannot be clicked. Click on one of the other entries instead.";

		private readonly AsyncTexture2D _textureBullet = AsyncTexture2D.FromAssetId(155038);

		private readonly bool _bulletIsVisible;

		private string _text = string.Empty;

		public string Text
		{
			get
			{
				return _text;
			}
			set
			{
				((Control)this).SetProperty<string>(ref _text, value, true, "Text");
			}
		}

		public CustomContextMenuStripItem(string text, Container parent, bool isHeader = false)
			: this()
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Expected O, but got Unknown
			((Control)this).set_EffectBehind((ControlEffect)new ScrollingHighlightEffect((Control)(object)this));
			Text = text;
			((Control)this).set_Parent(parent);
			((Control)this).set_Enabled(!isHeader);
			_bulletIsVisible = !isHeader;
			if (isHeader)
			{
				((Control)this).set_BasicTooltipText("Header cannot be clicked. Click on one of the other entries instead.");
			}
		}

		public override void RecalculateLayout()
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			int nWidth = (int)GameService.Content.get_DefaultFont14().MeasureString(_text).Width + 30 + 30;
			Container parent = ((Control)this).get_Parent();
			if (parent != null)
			{
				((Control)this).set_Width(Math.Max(((Control)parent).get_Width() - 4, nWidth));
			}
			else
			{
				((Control)this).set_Width(nWidth);
			}
		}

		protected override void OnClick(MouseEventArgs e)
		{
			((Control)this).OnClick(e);
			Container parent = ((Control)this).get_Parent();
			if (parent != null)
			{
				((Control)parent).Hide();
			}
		}

		protected override void OnMouseEntered(MouseEventArgs e)
		{
			((Control)this).OnMouseEntered(e);
		}

		protected override void OnMouseLeft(MouseEventArgs e)
		{
			((Control)this).OnMouseLeft(e);
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			Color modifierTint = ((!((Control)this).get_Enabled()) ? StandardColors.get_DisabledText() : (((Control)this).get_MouseOver() ? StandardColors.get_Tinted() : StandardColors.get_Default()));
			if (_bulletIsVisible)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_textureBullet), new Rectangle(6, base._size.Y / 2 - 9, 18, 18), modifierTint);
			}
			int textLeftPadding = (_bulletIsVisible ? 30 : 6);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, _text, Control.get_Content().get_DefaultFont14(), new Rectangle(textLeftPadding + 1, 1, base._size.X - 30 - 6, base._size.Y), StandardColors.get_Shadow(), false, (HorizontalAlignment)0, (VerticalAlignment)1);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, _text, Control.get_Content().get_DefaultFont14(), new Rectangle(textLeftPadding, 0, base._size.X - 30 - 6, base._size.Y), base._enabled ? StandardColors.get_Default() : StandardColors.get_DisabledText(), false, (HorizontalAlignment)0, (VerticalAlignment)1);
		}
	}
}
