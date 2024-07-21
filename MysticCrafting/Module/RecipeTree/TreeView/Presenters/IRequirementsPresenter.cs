using MysticCrafting.Module.RecipeTree.TreeView.Nodes;

namespace MysticCrafting.Module.RecipeTree.TreeView.Presenters
{
	public interface IRequirementsPresenter
	{
		void CalculateTotalPlayerCount(IngredientNode node);

		void CalculateTotalUnitCount(IngredientNode node);

		void CalculateOrderUnitCount(IngredientNode node, bool updateChildren = false);
	}
}
