using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using MysticCrafting.Models.Commerce;
using MysticCrafting.Module.Extensions;
using MysticCrafting.Module.Recipe.TreeView.Nodes;
using MysticCrafting.Module.Repositories;

namespace MysticCrafting.Module.Recipe.TreeView.Presenters
{
	public class TradeableNodePresenter : ITradeableNodePresenter
	{
		private readonly ICurrencyRepository _currencyRepository;

		private MysticCurrency coinCurrency => _currencyRepository.GetCoinCurrency();

		public TradeableNodePresenter(ICurrencyRepository currencyRepository)
		{
			_currencyRepository = currencyRepository;
		}

		public void CalculateTotalPrice(ITradeableItemNode item, Container childContainer = null)
		{
			IEnumerable<MysticCurrencyQuantity> totalVendorPrice = CalculateVendorTotalPrice(item, childContainer);
			MysticCurrencyQuantity totalCoinPrice = CalculateCoinTotalPrice(item, childContainer);
			List<MysticCurrencyQuantity> totalPrice = new List<MysticCurrencyQuantity> { totalCoinPrice };
			totalPrice.AddRange(totalVendorPrice);
			item.GrandTotalPrice = totalPrice.CombineQuantities().ToList();
			item.TotalVendorPrice = totalVendorPrice.ExcludingCoins().ToList();
			item.TotalCoinPrice = new List<MysticCurrencyQuantity>
			{
				totalCoinPrice,
				totalVendorPrice.CoinQuantity()
			}?.CombineQuantities()?.FirstOrDefault();
			Control control = item as Control;
			if (control != null)
			{
				ITradeableItemNode parent = control.Parent as ITradeableItemNode;
				if (parent != null)
				{
					CalculateTotalPrice(parent);
				}
			}
		}

		public MysticCurrencyQuantity CalculateCoinTotalPrice(ITradeableItemNode item, Container childContainer = null)
		{
			MysticCurrencyQuantity price = new MysticCurrencyQuantity
			{
				Currency = coinCurrency
			};
			if (item.RequiredQuantity < 1)
			{
				return price;
			}
			if (item.TradingPostPrice != 0)
			{
				price.Count = item.TradingPostPrice * item.RequiredQuantity;
			}
			else if (childContainer != null)
			{
				price.Count = childContainer.Children.OfType<ITradeableItemNode>().Sum((ITradeableItemNode i) => i.GrandTotalPrice.CoinCount());
			}
			else
			{
				Container container = item as Container;
				if (container != null)
				{
					price.Count = container.Children.OfType<ITradeableItemNode>().Sum((ITradeableItemNode i) => i.GrandTotalPrice.CoinCount());
				}
			}
			return price;
		}

		public IEnumerable<MysticCurrencyQuantity> CalculateVendorTotalPrice(ITradeableItemNode item, Container childContainer = null)
		{
			if (item.RequiredQuantity < 1)
			{
				return new List<MysticCurrencyQuantity>();
			}
			if (item.VendorUnitPrice != null && item.VendorUnitPrice.Any())
			{
				return (from i in item.VendorUnitPrice.CombineQuantities()
					select new MysticCurrencyQuantity
					{
						Currency = i.Currency,
						Count = i.Count * item.RequiredQuantity / item.VendorUnitQuantity
					}).ToList();
			}
			if (childContainer != null)
			{
				return childContainer.Children.OfType<ITradeableItemNode>().SelectMany((ITradeableItemNode p) => p.TotalVendorPrice).CombineQuantities();
			}
			Container container = item as Container;
			if (container != null)
			{
				return container.Children.OfType<ITradeableItemNode>().SelectMany((ITradeableItemNode p) => p.TotalVendorPrice).CombineQuantities();
			}
			return new List<MysticCurrencyQuantity>();
		}
	}
}
