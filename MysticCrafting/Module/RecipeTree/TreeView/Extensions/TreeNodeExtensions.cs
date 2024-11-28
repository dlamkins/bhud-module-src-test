using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Atzie.MysticCrafting.Models.Items;
using Blish_HUD.Controls;
using MysticCrafting.Models.TradingPost;
using MysticCrafting.Module.Extensions;
using MysticCrafting.Module.RecipeTree.TreeView.Nodes;
using MysticCrafting.Module.Services;
using MysticCrafting.Module.Settings;

namespace MysticCrafting.Module.RecipeTree.TreeView.Extensions
{
	public static class TreeNodeExtensions
	{
		public static void ReIndex(this TreeView treeView)
		{
			int index = 1;
			((IEnumerable)((Container)treeView).get_Children()).OfType<TreeNodeBase>().CalculateIndices(ref index);
		}

		public static void CalculateIndices(this IEnumerable<TreeNodeBase> nodes, ref int index)
		{
			foreach (TreeNodeBase node in nodes)
			{
				node.NodeIndex = index;
				(node as IngredientNode)?.UpdateRelatedNodes();
				index++;
				(from t in ((IEnumerable)((Container)node).get_Children()).OfType<TreeNodeBase>()
					where ((Control)t).get_Visible()
					select t).CalculateIndices(ref index);
			}
		}

		public static void UpdatePrices(this TreeView treeView, Item mainItem)
		{
			List<Item> items = mainItem.FlattenItems().ToList();
			Task.Run(async delegate
			{
				IList<Item> updatedItems = await ServiceContainer.TradingPostService.UpdatePricesSafeAsync(items);
				treeView.UpdatePrices(updatedItems);
			});
		}

		public static void UpdatePrices(this TreeView treeView, IList<Item> items)
		{
			if (items == null || items.Count == 0)
			{
				return;
			}
			foreach (ItemIngredientNode node in from n in treeView.IngredientNodes.OfType<ItemIngredientNode>()
				where items.Contains(n.Item)
				select n)
			{
				if (!node.Item.CanBeTraded)
				{
					continue;
				}
				foreach (TradingPostNode tpNode in ((IEnumerable)((Container)node).get_Children()).OfType<TradingPostNode>())
				{
					Item item = items.FirstOrDefault((Item i) => i == node.Item);
					if (item != null)
					{
						switch (tpNode.Option)
						{
						case TradingPostOptions.Buy:
							tpNode.UnitPrice = item.TradingPostBuy.GetValueOrDefault();
							break;
						case TradingPostOptions.Sell:
							tpNode.UnitPrice = item.TradingPostSell.GetValueOrDefault();
							break;
						default:
							throw new ArgumentOutOfRangeException();
						}
						node.Item.TradingPostLastUpdated = ServiceContainer.TradingPostService.LastLoaded;
						if (tpNode.Selected)
						{
							node.TradingPostPrice = tpNode.UnitPrice;
						}
					}
				}
			}
		}

		public static void UpdatePrices(this TreeView treeView, IList<TradingPostItemPrices> prices)
		{
			List<int> updatedItemIds = prices.Select((TradingPostItemPrices i) => i.Id).ToList();
			foreach (ItemIngredientNode node in from n in treeView.IngredientNodes.OfType<ItemIngredientNode>()
				where n.Item.CanBeTraded && updatedItemIds.Contains(n.Item.Id)
				select n)
			{
				if (!node.Item.CanBeTraded)
				{
					continue;
				}
				foreach (TradingPostNode tpNode in ((IEnumerable)((Container)node).get_Children()).OfType<TradingPostNode>())
				{
					TradingPostItemPrices item = prices.FirstOrDefault((TradingPostItemPrices i) => i.Id == node.Item.Id);
					if (item != null)
					{
						switch (tpNode.Option)
						{
						case TradingPostOptions.Buy:
							tpNode.UnitPrice = item.BuyPrice.UnitPrice;
							break;
						case TradingPostOptions.Sell:
							tpNode.UnitPrice = item.SellPrice.UnitPrice;
							break;
						default:
							throw new ArgumentOutOfRangeException();
						}
						node.Item.TradingPostLastUpdated = ServiceContainer.TradingPostService.LastLoaded;
						node.TradingPostPrice = tpNode.UnitPrice;
					}
				}
			}
		}
	}
}
