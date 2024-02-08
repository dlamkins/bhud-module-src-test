using System.Collections.Generic;
using MysticCrafting.Models.Commerce;
using MysticCrafting.Module.Services.Recurring;

namespace MysticCrafting.Module.Services.API
{
	public interface IWalletService : IRecurringService
	{
		IEnumerable<MysticCurrencyQuantity> GetQuantities();

		MysticCurrencyQuantity GetQuantity(int id);
	}
}
