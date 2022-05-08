using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.BuildsManager
{
	public class Control_TemplateTooltip : Control
	{
		public Control_TemplateTooltip()
			: this()
		{
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
		}
	}
}
