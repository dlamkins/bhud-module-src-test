using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using MysticCrafting.Module.RecipeTree.TreeView.Nodes;

namespace MysticCrafting.Module.RecipeTree.TreeView.Extensions
{
	public static class IngredientNodeListExtensions
	{
		public static IEnumerable<IngredientNode> GetByItemId(this IList<IngredientNode> nodes, int itemId)
		{
			return nodes?.Where((IngredientNode n) => n.Id == itemId)?.OrderBy((IngredientNode n) => n.NodeIndex);
		}

		public static void RemoveNodeAndDescendants(this IList<IngredientNode> allNodes, IngredientNode node)
		{
			allNodes.Remove(node);
			foreach (IngredientNode child2 in node.ChildIngredientNodes)
			{
				allNodes.RemoveNodeAndDescendants(child2);
			}
			foreach (VendorNode item in ((IEnumerable)((Container)node).get_Children()).OfType<VendorNode>())
			{
				foreach (IngredientNode child in ((IEnumerable)((Container)item).get_Children()).OfType<IngredientNode>())
				{
					allNodes.RemoveNodeAndDescendants(child);
				}
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
	}
}
