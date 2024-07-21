using System.Collections.Generic;
using Atzie.MysticCrafting.Models.Currencies;

namespace MysticCrafting.Module.RecipeTree.TreeView.Nodes
{
	public interface ITradeableItemNode
	{
		bool Active { get; set; }

		int OrderUnitCount { get; set; }

		int TradingPostPrice { get; set; }

		int VendorPriceUnitCount { get; set; }

		IList<CurrencyQuantity> VendorPrice { get; set; }

		IList<CurrencyQuantity> TotalVendorPrice { get; set; }

		CurrencyQuantity TotalCoinPrice { get; set; }

		IList<CurrencyQuantity> GrandTotalPrice { get; set; }

		void ResetPrices();
	}
}
