using System;
using System.Collections.Generic;
using Atzie.MysticCrafting.Models.Currencies;
using MysticCrafting.Module.Services;

namespace MysticCrafting.Module.Repositories
{
	public interface ICurrencyRepository : IDisposable
	{
		Currency CoinCurrency { get; }

		IList<Currency> Currencies { get; }

		void Initialize(ISqliteDbService service);

		Currency GetCoinCurrency();

		Currency GetCurrency(int id);
	}
}
