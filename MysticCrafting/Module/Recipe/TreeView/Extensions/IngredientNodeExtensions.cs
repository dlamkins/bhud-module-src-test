using System;
using System.Collections.Generic;
using System.Linq;
using MysticCrafting.Module.Recipe.TreeView.Nodes;
using MysticCrafting.Module.Settings;

namespace MysticCrafting.Module.Recipe.TreeView.Extensions
{
	public static class IngredientNodeExtensions
	{
		public static IEnumerable<IngredientNode> GetParents(this IngredientNode node)
		{
			List<IngredientNode> parents = new List<IngredientNode>();
			for (IngredientNode parent = node.ParentNode; parent != null; parent = parent.ParentNode)
			{
				parents.Add(parent);
			}
			return parents;
		}

		private static void UpdateLink(this IngredientNode node, bool linked, int reservedQuantity)
		{
			node.ReservedQuantity = reservedQuantity;
			node.IsLinked = linked;
			node.UpdateItemCountControls();
			node.ItemCountTooltipView?.UpdateLinkedNodes();
		}

		public static void Link(this IngredientNode node, int reservedQuantity)
		{
			node.UpdateLink(linked: true, reservedQuantity);
		}

		public static void Unlink(this IngredientNode node)
		{
			node.UpdateLink(linked: false, 0);
		}

		public static void UpdateRelatedNodesAndDescendants(this IngredientNode node)
		{
			node.UpdateRelatedNodes();
			foreach (IngredientNode ingredientNode in node.IngredientNodes)
			{
				ingredientNode.UpdateRelatedNodesAndDescendants();
			}
		}

		public static void UpdateRelatedNodes(this IngredientNode node)
		{
			List<IngredientNode> relatedNodes = node.TreeView.IngredientNodes.GetByItemId(node.Item.GameId).ToList();
			int nodeCount = relatedNodes.Count;
			int reservedQuantity = 0;
			foreach (IngredientNode relatedNode in relatedNodes)
			{
				if (nodeCount == 1)
				{
					relatedNode.Unlink();
					continue;
				}
				relatedNode.Link(reservedQuantity);
				if (!relatedNode.IsSharedItem)
				{
					reservedQuantity += relatedNode.RecipeRequiredQuantity * relatedNode.ParentNode.RequiredQuantity;
				}
			}
			foreach (IngredientNode item in relatedNodes)
			{
				item.ItemCountTooltipView?.UpdateLinkedNodes();
			}
		}

		public static int CalculateIndex(this IngredientNode node)
		{
			int returnValue = 1;
			if (node.IngredientIndex > 0)
			{
				IOrderedEnumerable<IngredientNode> siblingNodes = (from n in node.Parent?.Children.OfType<IngredientNode>()
					where n.IngredientIndex < node.IngredientIndex
					orderby n.IngredientIndex
					select n);
				if (siblingNodes != null)
				{
					returnValue += ((IEnumerable<IngredientNode>)siblingNodes).Sum((Func<IngredientNode, int>)GetDescendantCount);
				}
			}
			if (node.ParentNode == null)
			{
				return returnValue;
			}
			return returnValue + node.ParentNode.NodeIndex;
		}

		public static int GetDescendantCount(this IngredientNode node)
		{
			return 1 + (node.IngredientNodes?.Sum((Func<IngredientNode, int>)GetDescendantCount) ?? 0);
		}

		public static void UpdateTradingPostOptions(this IngredientNode node, TradingPostOptions option)
		{
			foreach (TradingPostNode child in node.Children.OfType<TradingPostNode>())
			{
				if (child.Option == option && !child.Selected)
				{
					child.ToggleSelect();
				}
			}
			foreach (IngredientNode ingredientNode in node.IngredientNodes)
			{
				ingredientNode.UpdateTradingPostOptions(option);
			}
		}
	}
}
