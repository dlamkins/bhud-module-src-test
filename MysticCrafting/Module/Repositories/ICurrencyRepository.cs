using System.Collections.Generic;
using MysticCrafting.Models.Commerce;

namespace MysticCrafting.Module.Repositories
{
	public interface ICurrencyRepository
	{
		IList<MysticCurrency> GetCurrencies();

		MysticCurrency GetCurrency(int id);

		MysticCurrency GetCurrency(string name);

		MysticCurrency GetCoinCurrency();
	}
}
