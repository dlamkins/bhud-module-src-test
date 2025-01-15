using Blish_HUD.Controls;

namespace SL.ChatLinks.UI
{
	public sealed class MainIcon : CornerIcon
	{
		public MainIcon(MainIconViewModel vm)
			: this(vm.Texture, vm.HoverTexture, vm.Name)
		{
			((Control)this).set_Parent((Container)(object)Control.get_Graphics().get_SpriteScreen());
			((CornerIcon)this).set_Priority(vm.Priority);
		}
	}
}
