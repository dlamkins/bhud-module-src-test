using System.Collections.Generic;
using Atzie.MysticCrafting.Models.Currencies;

namespace MysticCrafting.Module.Services.API
{
	public interface IWalletService : IApiService
	{
		IEnumerable<CurrencyQuantity> GetQuantities();

		CurrencyQuantity GetQuantity(int id);
	}
}
