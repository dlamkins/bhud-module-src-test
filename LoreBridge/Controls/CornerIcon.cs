using Blish_HUD.Content;
using Blish_HUD.Controls;
using LoreBridge.Resources;

namespace LoreBridge.Controls
{
	public sealed class CornerIcon : CornerIcon
	{
		public CornerIcon()
			: this()
		{
			((Control)this).set_Visible(true);
			((CornerIcon)this).set_Icon(AsyncTexture2D.op_Implicit(Textures.Icon));
			((CornerIcon)this).set_HoverIcon(AsyncTexture2D.op_Implicit(Textures.IconHover));
			((CornerIcon)this).set_IconName("LoreBridge");
		}
	}
}
