using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using MysticCrafting.Models;
using MysticCrafting.Models.Items;
using MysticCrafting.Module.Extensions;
using MysticCrafting.Module.Services;
using MysticCrafting.Module.Services.API;

namespace MysticCrafting.Module.Recipe.TreeView.Nodes
{
	public class RecipeSheetNode : IngredientNode
	{
		public bool RecipeUnlocked;

		private Label _recipeUnlockedLabel;

		private readonly IPlayerUnlocksService _playerUnlocksService = ServiceContainer.PlayerUnlocksService;

		public RecipeSheetNode(MysticItem item, int? quantity = null, int? index = null)
			: base(item, quantity, index)
		{
		}

		public RecipeSheetNode(MysticIngredient ingredient, MysticItem item, Container parent, bool loadingChildren = false)
			: base(ingredient, item, parent, loadingChildren)
		{
		}

		public override void BuildItemCountControls()
		{
			RecipeUnlocked = _playerUnlocksService.RecipeUnlocked(base.Item.Id);
			string labelText = (RecipeUnlocked ? "Unlocked" : "Locked");
			_recipeUnlockedLabel = new Label
			{
				Parent = this,
				Text = labelText,
				Location = new Point(base.Icon.Right + 5, 18),
				Width = 200,
				Font = base.Font,
				TextColor = ColorHelper.FromRarity(base.Item.Rarity),
				StrokeText = true,
				AutoSizeHeight = true
			};
		}

		public override void UpdateItemCountControls()
		{
		}
	}
}
