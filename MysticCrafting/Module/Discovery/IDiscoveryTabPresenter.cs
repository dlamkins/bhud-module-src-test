using Blish_HUD.Graphics.UI;

namespace MysticCrafting.Module.Discovery
{
	public interface IDiscoveryTabPresenter : IPresenter
	{
		void SearchAsync(string text);
	}
}
