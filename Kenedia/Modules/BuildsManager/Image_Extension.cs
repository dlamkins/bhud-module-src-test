using Blish_HUD.Content;
using Blish_HUD.Controls;

namespace Kenedia.Modules.BuildsManager
{
	public static class Image_Extension
	{
		public static void setTexture(this Image ctrl, GW2API.Item item, string path)
		{
			ctrl.set_Texture(AsyncTexture2D.op_Implicit(item.getIcon(path, ctrl)));
			((Control)ctrl).set_BasicTooltipText(item.Name);
		}
	}
}
