using Atzie.MysticCrafting.Models.Crafting;
using Atzie.MysticCrafting.Models.Items;
using Blish_HUD.Controls;
using MysticCrafting.Module.Models;
using MysticCrafting.Module.RecipeTree.TreeView.Controls;
using MysticCrafting.Module.RecipeTree.TreeView.Nodes;

namespace MysticCrafting.Module.RecipeTree.TreeView.Presenters
{
	public interface IIngredientNodePresenter
	{
		void BuildChildren(IItemSource itemSource, Container parent);

		ItemIngredientNode BuildItemNode(Item item, Container parent, bool expandable = false, bool isPrimaryNode = false, bool openByDefault = true, int quantity = 1);

		ItemIngredientNode BuildItemNode(Ingredient ingredient, Container parent, bool loadingChildren = false, bool expandable = false);

		void BuildNodes(Recipe recipe, Container parent);

		ItemTab BuildTabs(TreeNodeBase node, Item item);
	}
}
