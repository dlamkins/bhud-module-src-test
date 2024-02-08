using System.Collections.Generic;
using System.Linq;
using MysticCrafting.Models.Items;
using MysticCrafting.Module.Models;

namespace MysticCrafting.Module.Recipe.TreeView.Nodes
{
	public class BaseIngredientNode : IngredientNode
	{
		public override int RecipeRequiredQuantity => Levels.Sum((MysticIngredientLevel p) => p.Quantity) ?? 1;

		public List<MysticIngredientLevel> Levels { get; set; } = new List<MysticIngredientLevel>();


		public override int TotalRequiredQuantity => Levels?.Sum((MysticIngredientLevel p) => p.RequiredTotalQuantity) ?? 0;

		public BaseIngredientNode(MysticItem item)
			: base(item)
		{
		}
	}
}
