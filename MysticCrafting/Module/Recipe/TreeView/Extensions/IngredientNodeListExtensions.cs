using System.Collections.Generic;
using System.Linq;
using MysticCrafting.Module.Recipe.TreeView.Nodes;

namespace MysticCrafting.Module.Recipe.TreeView.Extensions
{
	public static class IngredientNodeListExtensions
	{
		public static IEnumerable<IngredientNode> GetByItemId(this IList<IngredientNode> nodes, int itemId)
		{
			return nodes?.Where((IngredientNode n) => n.Item.GameId == itemId)?.OrderBy((IngredientNode n) => n.NodeIndex);
		}

		public static void RemoveNodeAndDescendants(this IList<IngredientNode> allNodes, IngredientNode node)
		{
			allNodes.Remove(node);
			foreach (IngredientNode child in node.IngredientNodes)
			{
				allNodes.RemoveNodeAndDescendants(child);
			}
		}

		public static bool IsFirstNode(this IList<IngredientNode> nodes, IngredientNode node)
		{
			if (nodes.Count == 0)
			{
				return false;
			}
			return nodes.First() == node;
		}

		public static void Reindex(this IList<IngredientNode> nodes, int startIndex = 0)
		{
			foreach (IngredientNode item in nodes.Where((IngredientNode n) => n.NodeIndex > startIndex))
			{
				item.NodeIndex = item.CalculateIndex();
			}
		}
	}
}
