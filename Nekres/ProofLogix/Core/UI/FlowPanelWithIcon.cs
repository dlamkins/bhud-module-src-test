using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nekres.ProofLogix.Core.UI
{
	public class FlowPanelWithIcon : FlowPanel
	{
		private AsyncTexture2D _icon;

		private Rectangle _layoutHeaderIconBounds;

		public AsyncTexture2D Icon
		{
			get
			{
				return _icon;
			}
			set
			{
				((Control)this).SetProperty<AsyncTexture2D>(ref _icon, value, false, "Icon");
			}
		}

		public FlowPanelWithIcon(AsyncTexture2D icon)
			: this()
		{
			_icon = icon;
		}

		public override void RecalculateLayout()
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			((FlowPanel)this).RecalculateLayout();
			Rectangle layoutHeaderBounds = (Rectangle)this.GetPrivateField("_layoutHeaderBounds").GetValue(this);
			_layoutHeaderIconBounds = new Rectangle(((Rectangle)(ref layoutHeaderBounds)).get_Left() + 10, 2, 32, 32);
			this.GetPrivateField("_layoutHeaderTextBounds").SetValue(this, (object)new Rectangle(((Rectangle)(ref _layoutHeaderIconBounds)).get_Right() + 7, 0, layoutHeaderBounds.Width - _layoutHeaderIconBounds.Width - 10, 36));
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			((Panel)this).PaintBeforeChildren(spriteBatch, bounds);
			if (!string.IsNullOrEmpty(((Panel)this)._title))
			{
				AsyncTexture2D icon = _icon;
				if (icon != null && icon.get_HasSwapped() && icon.get_HasTexture())
				{
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_icon), _layoutHeaderIconBounds, Color.get_White());
				}
			}
		}
	}
}
