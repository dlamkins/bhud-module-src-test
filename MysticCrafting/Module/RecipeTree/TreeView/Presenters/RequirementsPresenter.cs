using System;
using System.Collections;
using System.Linq;
using Blish_HUD.Controls;
using MysticCrafting.Module.RecipeTree.TreeView.Extensions;
using MysticCrafting.Module.RecipeTree.TreeView.Nodes;

namespace MysticCrafting.Module.RecipeTree.TreeView.Presenters
{
	public class RequirementsPresenter : IRequirementsPresenter
	{
		public void CalculateTotalUnitCount(IngredientNode node)
		{
			int totalUnitCount2 = node.TotalUnitCount;
			int totalUnitCount = node.TotalUnitCount;
			RecipeSheetNode recipeSheetNode = node as RecipeSheetNode;
			if (recipeSheetNode != null)
			{
				totalUnitCount = CalculateRecipeSheetTotalUnitCount(recipeSheetNode);
			}
			else if (node.IsSharedItem)
			{
				totalUnitCount = 1;
			}
			else
			{
				ITradeableItemNode tradeableNode = node.ParentNode as ITradeableItemNode;
				totalUnitCount = ((tradeableNode == null) ? (node.UnitCountNumber ?? node.UnitCount) : (node.UnitCount * tradeableNode.OrderUnitCount / tradeableNode.VendorPriceUnitCount));
			}
			if (totalUnitCount2 == totalUnitCount)
			{
				return;
			}
			node.TotalUnitCount = totalUnitCount;
			CalculateOrderUnitCount(node);
			foreach (IngredientNode childNode2 in node.ChildIngredientNodes)
			{
				CalculateTotalUnitCount(childNode2);
			}
			foreach (VendorNode item in ((IEnumerable)((Container)node).get_Children()).OfType<VendorNode>())
			{
				item.OrderUnitCount = totalUnitCount;
				foreach (IngredientNode childNode in ((IEnumerable)((Container)item).get_Children()).OfType<IngredientNode>())
				{
					CalculateTotalUnitCount(childNode);
				}
			}
			UpdateTotalRequiredAmount(node);
		}

		private int CalculateRecipeSheetTotalUnitCount(RecipeSheetNode node)
		{
			int totalUnitCount = 1;
			if (node.IsSharedItem && !node.TreeView.IngredientNodes.GetByItemId(node.Item.Id).ToList().IsFirstNode(node))
			{
				totalUnitCount = 0;
			}
			if (node.RecipeUnlocked)
			{
				totalUnitCount = 0;
			}
			return totalUnitCount;
		}

		public void UpdateTotalRequiredAmount(IngredientNode node)
		{
			foreach (IngredientNode childNode in node.ChildIngredientNodes)
			{
				CalculateTotalUnitCount(childNode);
			}
			node.CalculateTotalPrices();
			node.UpdateItemCountControls();
			node.UpdateRelatedNodes();
		}

		public void CalculateTotalPlayerCount(IngredientNode node)
		{
			node.TotalPlayerUnitCount = Math.Max(0, node.PlayerUnitCount - node.ReservedUnitCount);
			CalculateOrderUnitCount(node);
		}

		public void CalculateOrderUnitCount(IngredientNode node, bool updateChildren = false)
		{
			if (node.IsLinked && node.TotalPlayerUnitCount > node.TotalUnitCount)
			{
				node.OrderUnitCount = 0;
			}
			else
			{
				node.OrderUnitCount = Math.Max(0, node.TotalUnitCount - node.TotalPlayerUnitCount);
			}
			if (!updateChildren)
			{
				return;
			}
			foreach (IngredientNode childNode2 in node.ChildIngredientNodes)
			{
				CalculateTotalUnitCount(childNode2);
			}
			foreach (VendorNode item in ((IEnumerable)((Container)node).get_Children()).OfType<VendorNode>())
			{
				item.OrderUnitCount = node.OrderUnitCount;
				foreach (IngredientNode childNode in ((IEnumerable)((Container)item).get_Children()).OfType<IngredientNode>())
				{
					CalculateTotalUnitCount(childNode);
				}
			}
		}
	}
}
