using Atzie.MysticCrafting.Models.Crafting;
using Blish_HUD.Controls;

namespace MysticCrafting.Module.RecipeTree.TreeView.Nodes
{
	public class UnknownIngredientNode : IngredientNode
	{
		public UnknownIngredientNode(int id, int quantity, Container parent)
			: base(id, quantity, parent)
		{
		}

		public UnknownIngredientNode(Ingredient ingredient, Container parent, int? index = null, bool loadingChildren = false)
			: base(ingredient.Id, ingredient.Quantity, parent, index, showUnitCount: false, loadingChildren)
		{
			base.Name = $"{ingredient.Quantity} {ingredient.Name}";
		}

		protected override void BuildMenuStrip()
		{
		}

		public override bool UpdatePlayerUnitCount()
		{
			return false;
		}
	}
}
