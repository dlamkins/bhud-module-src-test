using System;
using System.Collections.Generic;
using Atzie.MysticCrafting.Models.Currencies;
using MysticCrafting.Module.Services;

namespace MysticCrafting.Module.Repositories
{
	public interface ICurrencyRepository : IDisposable
	{
		void Initialize(ISqliteDbService service);

		IList<Currency> GetCurrencies();

		Currency GetCurrency(int id);

		Currency GetCurrency(string name);

		Currency GetCoinCurrency();
	}
}
