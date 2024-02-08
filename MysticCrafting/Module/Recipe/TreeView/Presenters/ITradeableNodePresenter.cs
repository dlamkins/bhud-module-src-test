using Blish_HUD.Controls;
using MysticCrafting.Module.Recipe.TreeView.Nodes;

namespace MysticCrafting.Module.Recipe.TreeView.Presenters
{
	public interface ITradeableNodePresenter
	{
		void CalculateTotalPrice(ITradeableItemNode item, Container childContainer = null);
	}
}
