using Blish_HUD.Controls;
using MysticCrafting.Models;

namespace MysticCrafting.Module.Recipe.TreeView.Presenters
{
	public interface IRecipeSheetNodePresenter
	{
		void BuildNode(MysticRecipe recipe, Container parent);
	}
}
