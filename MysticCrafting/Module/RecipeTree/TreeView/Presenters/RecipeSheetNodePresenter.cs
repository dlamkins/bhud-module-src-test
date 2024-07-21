using System.Linq;
using Atzie.MysticCrafting.Models.Crafting;
using Atzie.MysticCrafting.Models.Items;
using Blish_HUD.Controls;
using MysticCrafting.Module.RecipeTree.TreeView.Controls;
using MysticCrafting.Module.RecipeTree.TreeView.Nodes;
using MysticCrafting.Module.Services;

namespace MysticCrafting.Module.RecipeTree.TreeView.Presenters
{
	public class RecipeSheetNodePresenter : IRecipeSheetNodePresenter
	{
		private readonly IRecipeDetailsViewPresenter _recipeDetailsPresenter;

		private readonly IIngredientNodePresenter _ingredientNodePresenter;

		public RecipeSheetNodePresenter(IRecipeDetailsViewPresenter recipeDetailsPresenter, IIngredientNodePresenter ingredientNodePresenter)
		{
			_recipeDetailsPresenter = recipeDetailsPresenter;
			_ingredientNodePresenter = ingredientNodePresenter;
		}

		public void BuildNode(Recipe recipe, Container parent)
		{
			if (ServiceContainer.PlayerUnlocksService.RecipeUnlocked(recipe) || recipe.RecipeSheets == null || !recipe.RecipeSheets.Any())
			{
				return;
			}
			Item sheet = recipe.RecipeSheets.FirstOrDefault();
			if (sheet != null)
			{
				RecipeSheetNode recipeSheetNode = new RecipeSheetNode(recipe, sheet, parent);
				((Control)recipeSheetNode).set_Parent(parent);
				((Control)recipeSheetNode).set_Width(((Control)parent).get_Width() - 25);
				recipeSheetNode.PanelHeight = 45;
				recipeSheetNode.PanelExtensionHeight = 0;
				recipeSheetNode.IsSharedItem = true;
				RecipeSheetNode node = recipeSheetNode;
				node.Build(parent);
				node.OnPanelClick += delegate
				{
					_recipeDetailsPresenter.SaveScrollDistance();
				};
				ItemTab selectedTab = _ingredientNodePresenter.BuildTabs(node, node.Item);
				if (selectedTab != null)
				{
					_ingredientNodePresenter.BuildChildren(selectedTab.ItemSource, ((Control)((Control)selectedTab).get_Parent()).get_Parent());
				}
				else
				{
					node.BuildMissingTabsLabel();
				}
			}
		}
	}
}
