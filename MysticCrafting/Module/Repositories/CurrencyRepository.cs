using System;
using System.Collections.Generic;
using System.Linq;
using Atzie.MysticCrafting.Models.Currencies;
using MysticCrafting.Module.Services;
using SQLite;

namespace MysticCrafting.Module.Repositories
{
	public class CurrencyRepository : ICurrencyRepository, IDisposable
	{
		private Currency _coinCurrency;

		private SQLiteConnection Connection { get; set; }

		public Currency CoinCurrency => _coinCurrency ?? (_coinCurrency = Currencies.FirstOrDefault((Currency c) => c.Id == 1));

		public IList<Currency> Currencies { get; private set; }

		public void Initialize(ISqliteDbService service)
		{
			Connection = new SQLiteConnection(service.DatabaseFilePath);
			Currencies = GetCurrencies();
		}

		public Currency GetCoinCurrency()
		{
			return Currencies.FirstOrDefault((Currency c) => c.Id == 1);
		}

		private IList<Currency> GetCurrencies()
		{
			return Connection.Table<Currency>().ToList();
		}

		public Currency GetCurrency(int id)
		{
			return Currencies.FirstOrDefault((Currency c) => c.Id == id);
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				Connection?.Dispose();
			}
		}
	}
}
