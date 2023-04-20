using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Controls.Effects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RaidClears.Features.Shared.Models
{
	public class ContextMenuStripItemSeparator : ContextMenuStripItem
	{
		public ContextMenuStripItemSeparator()
			: this()
		{
			((Control)this).set_Enabled(false);
			((Control)this).set_EffectBehind((ControlEffect)null);
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(0, bounds.Height / 2, bounds.Width, 1), Color.get_White() * 0.8f);
		}
	}
}
