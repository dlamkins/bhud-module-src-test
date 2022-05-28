using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Kenedia.Modules.QoL.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.QoL.UI
{
	public class Hotbar_Button : Control
	{
		public SubModule SubModule;

		public int Index;

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, SubModule.Active ? SubModule.ModuleIcon_Active : SubModule.ModuleIcon, bounds, (Rectangle?)SubModule.ModuleIcon.get_Bounds(), Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
		}

		protected override void OnClick(MouseEventArgs e)
		{
			((Control)this).OnClick(e);
			SubModule.ToggleModule();
		}

		protected override void DisposeControl()
		{
			((Control)this).DisposeControl();
			SubModule = null;
		}

		public Hotbar_Button()
			: this()
		{
		}
	}
}
