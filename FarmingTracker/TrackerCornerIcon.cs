using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;

namespace FarmingTracker
{
	public class TrackerCornerIcon : CornerIcon
	{
		private readonly EventHandler<MouseEventArgs> _cornerIconClickEventHandler;

		public TrackerCornerIcon(Services services, EventHandler<MouseEventArgs> cornerIconClickEventHandler)
			: this()
		{
			_cornerIconClickEventHandler = cornerIconClickEventHandler;
			((CornerIcon)this).set_Icon(AsyncTexture2D.op_Implicit(services.TextureService.CornerIconTexture));
			((CornerIcon)this).set_HoverIcon(AsyncTexture2D.op_Implicit(services.TextureService.CornerIconHoverTexture));
			((Control)this).set_BasicTooltipText("Open farming tracker window");
			((CornerIcon)this).set_Priority(1788560160);
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)this).add_Click(cornerIconClickEventHandler);
		}

		protected override void DisposeControl()
		{
			((Control)this).remove_Click(_cornerIconClickEventHandler);
			((CornerIcon)this).DisposeControl();
		}
	}
}
