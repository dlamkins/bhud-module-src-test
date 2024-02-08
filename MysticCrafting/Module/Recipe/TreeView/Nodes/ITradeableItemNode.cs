using System.Collections.Generic;
using MysticCrafting.Models.Commerce;

namespace MysticCrafting.Module.Recipe.TreeView.Nodes
{
	public interface ITradeableItemNode
	{
		int RequiredQuantity { get; }

		int TradingPostPrice { get; set; }

		int VendorUnitQuantity { get; set; }

		IList<MysticCurrencyQuantity> VendorUnitPrice { get; set; }

		IList<MysticCurrencyQuantity> TotalVendorPrice { get; set; }

		MysticCurrencyQuantity TotalCoinPrice { get; set; }

		IList<MysticCurrencyQuantity> GrandTotalPrice { get; set; }

		void ResetPrices();
	}
}
