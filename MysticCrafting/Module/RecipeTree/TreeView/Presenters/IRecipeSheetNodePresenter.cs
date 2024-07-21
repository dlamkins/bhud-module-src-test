using Atzie.MysticCrafting.Models.Crafting;
using Blish_HUD.Controls;

namespace MysticCrafting.Module.RecipeTree.TreeView.Presenters
{
	public interface IRecipeSheetNodePresenter
	{
		void BuildNode(Recipe recipe, Container parent);
	}
}
