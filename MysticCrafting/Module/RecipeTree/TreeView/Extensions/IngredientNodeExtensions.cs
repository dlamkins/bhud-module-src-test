using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using MysticCrafting.Module.RecipeTree.TreeView.Nodes;
using MysticCrafting.Module.Settings;

namespace MysticCrafting.Module.RecipeTree.TreeView.Extensions
{
	public static class IngredientNodeExtensions
	{
		public class ReservationGroup
		{
			public int NodeIndex { get; set; }

			public int Count { get; set; }

			public int Group { get; set; }
		}

		public static IEnumerable<IngredientNode> GetParents(this IngredientNode node)
		{
			List<IngredientNode> parents = new List<IngredientNode>();
			for (IngredientNode parent = node.ParentIngredientNode; parent != null; parent = parent.ParentIngredientNode)
			{
				parents.Add(parent);
			}
			return parents;
		}

		private static void UpdateLink(this IngredientNode node, bool linked, int reservedQuantity)
		{
			node.ReservedUnitCount = reservedQuantity;
			node.IsLinked = linked;
			node.UpdateItemCountControls();
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
			foreach (IngredientNode childIngredientNode in node.ChildIngredientNodes)
			{
				childIngredientNode.UpdateRelatedNodesAndDescendants();
			}
			foreach (VendorNode item in ((IEnumerable)((Container)node).get_Children()).OfType<VendorNode>())
			{
				foreach (IngredientNode item2 in ((IEnumerable)((Container)item).get_Children()).OfType<IngredientNode>())
				{
					item2.UpdateRelatedNodesAndDescendants();
				}
			}
		}

		public static void UpdateRelatedNodes(this IngredientNode node)
		{
			List<IngredientNode> relatedNodes = node.TreeView.IngredientNodes.GetByItemId(node.Id).ToList();
			int nodeCount = relatedNodes.Count;
			int reservedQuantity = 0;
			List<ReservationGroup> reservations = new List<ReservationGroup>();
			foreach (IngredientNode relatedNode in relatedNodes)
			{
				if (nodeCount == 1)
				{
					relatedNode.Unlink();
					continue;
				}
				int groupedReservedQuantity = 0;
				int groupId = relatedNode.ReservedGroup.GetValueOrDefault();
				groupedReservedQuantity = reservations.Where((ReservationGroup r) => r.Group == 0 || r.Group != groupId).Sum((ReservationGroup r) => r.Count);
				relatedNode.Link(groupedReservedQuantity);
				int reservationCount = 0;
				if (!relatedNode.IsSharedItem && relatedNode.Active)
				{
					ITradeableItemNode parentNode = relatedNode.ParentNode as ITradeableItemNode;
					if (parentNode != null)
					{
						reservationCount = relatedNode.UnitCount * parentNode.OrderUnitCount;
					}
				}
				reservations.Add(new ReservationGroup
				{
					Count = reservationCount,
					NodeIndex = relatedNode.NodeIndex,
					Group = groupId
				});
				reservedQuantity += reservationCount;
			}
			foreach (IngredientNode item in relatedNodes)
			{
				item.PlayerCountTooltipView?.UpdateLinkedNodes();
				item.UpdateReservedQuantityTooltip();
			}
		}

		public static int GetReservedQuantity(this TreeNodeBase node, int reservedId, int nodeIndex = 0)
		{
			if (node.TreeView == null)
			{
				return 0;
			}
			if (nodeIndex == 0)
			{
				nodeIndex = node.NodeIndex;
			}
			return (from n in node.TreeView.IngredientNodes.GetByItemId(reservedId)
				where n.NodeIndex < nodeIndex
				select n).Sum((IngredientNode n) => n.TotalUnitCount);
		}

		public static void UpdateTradingPostOptions(this IngredientNode node, TradingPostOptions option)
		{
			foreach (TradingPostNode child in ((IEnumerable)((Container)node).get_Children()).OfType<TradingPostNode>())
			{
				if (child.Option == option && !child.Selected)
				{
					child.ToggleSelect();
				}
			}
			foreach (IngredientNode childIngredientNode in node.ChildIngredientNodes)
			{
				childIngredientNode.UpdateTradingPostOptions(option);
			}
		}
	}
}
