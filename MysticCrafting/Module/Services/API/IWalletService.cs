using System.Collections.Generic;
using Atzie.MysticCrafting.Models.Currencies;
using MysticCrafting.Module.Services.Recurring;

namespace MysticCrafting.Module.Services.API
{
	public interface IWalletService : IRecurringService
	{
		IEnumerable<CurrencyQuantity> GetQuantities();

		CurrencyQuantity GetQuantity(int id);
	}
}
