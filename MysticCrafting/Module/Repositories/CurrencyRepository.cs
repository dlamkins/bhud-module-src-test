using System;
using System.Collections.Generic;
using System.Linq;
using JsonFlatFileDataStore;
using MysticCrafting.Models.Commerce;
using MysticCrafting.Module.Services;

namespace MysticCrafting.Module.Repositories
{
	public class CurrencyRepository : ICurrencyRepository
	{
		private IDocumentCollection<MysticCurrency> Currencies { get; } = ServiceContainer.Store.GetCollection<MysticCurrency>();


		public IList<MysticCurrency> GetCurrencies()
		{
			return Currencies.AsQueryable().ToList();
		}

		public MysticCurrency GetCurrency(int id)
		{
			return Currencies.AsQueryable().FirstOrDefault((MysticCurrency v) => v.GameId == id);
		}

		public MysticCurrency GetCurrency(string name)
		{
			return Currencies.AsQueryable().FirstOrDefault((MysticCurrency v) => v.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
		}

		public MysticCurrency GetCoinCurrency()
		{
			return Currencies.AsQueryable().FirstOrDefault((MysticCurrency v) => v.GameId == 1);
		}
	}
}
