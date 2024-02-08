using Blish_HUD.Controls;
using MysticCrafting.Models;
using MysticCrafting.Models.Items;
using MysticCrafting.Module.Models;
using MysticCrafting.Module.Recipe.TreeView.Controls;
using MysticCrafting.Module.Recipe.TreeView.Nodes;

namespace MysticCrafting.Module.Recipe.TreeView.Presenters
{
	public interface IIngredientNodePresenter
	{
		void BuildChildren(IItemSource itemSource, Container parent);

		IngredientNode BuildNode(MysticItem item, Container parent, bool expandable = false);

		IngredientNode BuildNode(MysticIngredient ingredient, Container parent, bool loadingChildren = false, bool expandable = false);

		void BuildNodes(MysticRecipe recipe, Container parent);

		ItemTab BuildTabs(TreeNodeBase node, MysticItem item);
	}
}
