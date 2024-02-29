using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;

namespace MysticCrafting.Module.Discovery.ItemList
{
	public interface IItemListPresenter : IPresenter
	{
		void RarityDropdown_ValueChanged(object sender, ValueChangedEventArgs e);

		void Reload();
	}
}
