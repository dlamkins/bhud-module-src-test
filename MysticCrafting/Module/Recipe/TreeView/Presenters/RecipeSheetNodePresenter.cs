using System.Linq;
using Blish_HUD.Controls;
using MysticCrafting.Models;
using MysticCrafting.Models.Items;
using MysticCrafting.Module.Recipe.TreeView.Controls;
using MysticCrafting.Module.Recipe.TreeView.Nodes;
using MysticCrafting.Module.Services;

namespace MysticCrafting.Module.Recipe.TreeView.Presenters
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

		public void BuildNode(MysticRecipe recipe, Container parent)
		{
			if (ServiceContainer.PlayerUnlocksService.RecipeUnlocked(recipe) || recipe.RecipeSheetIds == null || !recipe.RecipeSheetIds.Any())
			{
				return;
			}
			int sheetId = recipe.RecipeSheetIds.FirstOrDefault();
			MysticItem item = ServiceContainer.ItemRepository.GetItem(sheetId);
			if (item != null)
			{
				MysticIngredient ingredient = new MysticIngredient
				{
					Quantity = 1,
					GameId = recipe.RecipeSheetIds.FirstOrDefault(),
					Item = item,
					Index = 0,
					Name = item.Name
				};
				RecipeSheetNode node = new RecipeSheetNode(recipe, ingredient, item, parent)
				{
					Parent = parent,
					Width = parent.Width - 25,
					PanelHeight = 45,
					PanelExtensionHeight = 0,
					IsSharedItem = true
				};
				node.Build(parent);
				node.OnPanelClick += delegate
				{
					_recipeDetailsPresenter.SaveScrollDistance();
				};
				ItemTab selectedTab = _ingredientNodePresenter.BuildTabs(node, node.Item);
				if (selectedTab != null)
				{
					_ingredientNodePresenter.BuildChildren(selectedTab.ItemSource, selectedTab.Parent.Parent);
				}
				else
				{
					node.BuildMissingTabsLabel();
				}
			}
		}
	}
}
