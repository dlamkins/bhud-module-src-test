using Blish_HUD.Controls;
using MysticCrafting.Module.RecipeTree.TreeView.Nodes;

namespace MysticCrafting.Module.RecipeTree.TreeView.Presenters
{
	public interface ITradeableNodePresenter
	{
		void CalculateTotalPrice(ITradeableItemNode item, Container childContainer = null);

		void RecalculateParents(ITradeableItemNode item);
	}
}
