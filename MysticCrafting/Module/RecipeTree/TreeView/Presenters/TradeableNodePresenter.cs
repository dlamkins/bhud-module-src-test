using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Atzie.MysticCrafting.Models.Currencies;
using Blish_HUD.Controls;
using MysticCrafting.Module.Extensions;
using MysticCrafting.Module.RecipeTree.TreeView.Nodes;
using MysticCrafting.Module.Repositories;

namespace MysticCrafting.Module.RecipeTree.TreeView.Presenters
{
	public class TradeableNodePresenter : ITradeableNodePresenter
	{
		private readonly ICurrencyRepository _currencyRepository;

		private Currency coinCurrency => _currencyRepository.GetCoinCurrency();

		public TradeableNodePresenter(ICurrencyRepository currencyRepository)
		{
			_currencyRepository = currencyRepository;
		}

		public void CalculateTotalPrice(ITradeableItemNode item, Container childContainer = null)
		{
			List<CurrencyQuantity> totalVendorPrice = CalculateVendorTotalPrice(item, childContainer).ToList();
			CurrencyQuantity totalCoinPrice = CalculateCoinTotalPrice(item, childContainer);
			List<CurrencyQuantity> totalPrice = new List<CurrencyQuantity> { totalCoinPrice };
			totalPrice.AddRange(totalVendorPrice);
			item.TotalVendorPrice = totalVendorPrice.ExcludingCoins().ToList();
			item.TotalCoinPrice = new List<CurrencyQuantity>
			{
				totalCoinPrice,
				totalVendorPrice.CoinQuantity()
			}.CombineQuantities()?.FirstOrDefault();
			item.GrandTotalPrice = totalPrice.CombineQuantities().ToList();
			RecalculateParents(item);
		}

		public void RecalculateParents(ITradeableItemNode item)
		{
			Control control = (Control)((item is Control) ? item : null);
			if (control != null)
			{
				ITradeableItemNode parent = control.get_Parent() as ITradeableItemNode;
				if (parent != null)
				{
					CalculateTotalPrice(parent);
				}
			}
		}

		public CurrencyQuantity CalculateCoinTotalPrice(ITradeableItemNode item, Container childContainer = null)
		{
			CurrencyQuantity price = new CurrencyQuantity
			{
				Currency = coinCurrency
			};
			if (item.OrderUnitCount < 1)
			{
				return price;
			}
			if (item.TradingPostPrice != 0L)
			{
				price.Count = item.TradingPostPrice * item.OrderUnitCount;
				return price;
			}
			Container container = (Container)(((object)childContainer) ?? ((object)((item is Container) ? item : null)));
			if (container == null)
			{
				return price;
			}
			IEnumerable<ITradeableItemNode> containerChildren = ((IEnumerable)container.get_Children()).OfType<ITradeableItemNode>();
			if (!(container is VendorNode))
			{
				containerChildren = containerChildren.Where((ITradeableItemNode c) => c.Active);
			}
			price.Count = containerChildren.Sum((ITradeableItemNode c) => c.GrandTotalPrice.CoinCount());
			return price;
		}

		public IEnumerable<CurrencyQuantity> CalculateVendorTotalPrice(ITradeableItemNode item, Container childContainer = null)
		{
			CurrencyIngredientNode currencyNode = item as CurrencyIngredientNode;
			if (currencyNode != null && currencyNode.CurrencyQuantity != null)
			{
				return new List<CurrencyQuantity>
				{
					new CurrencyQuantity
					{
						Count = currencyNode.TotalUnitCount,
						Currency = currencyNode.CurrencyQuantity.Currency
					}
				};
			}
			if (item.OrderUnitCount < 1)
			{
				return new List<CurrencyQuantity>();
			}
			object obj = ((object)childContainer) ?? ((object)((item is Container) ? item : null));
			IEnumerable<ITradeableItemNode> containerChildren = ((IEnumerable)((Container)obj).get_Children()).OfType<ITradeableItemNode>();
			if (!(obj is VendorNode))
			{
				containerChildren = containerChildren.Where((ITradeableItemNode c) => c.Active);
			}
			return containerChildren.SelectMany((ITradeableItemNode p) => p.TotalVendorPrice).CombineQuantities();
		}
	}
}
