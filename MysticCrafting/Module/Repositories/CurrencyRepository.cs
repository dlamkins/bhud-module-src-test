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
		private SQLiteConnection Connection { get; set; }

		public void Initialize(ISqliteDbService service)
		{
			Connection = new SQLiteConnection(service.DatabaseFilePath);
		}

		public IList<Currency> GetCurrencies()
		{
			return Connection.Table<Currency>().ToList();
		}

		public Currency GetCurrency(int id)
		{
			return Connection.Find<Currency>(id);
		}

		public Currency GetCurrency(string name)
		{
			return Connection.Query<Currency>("SELECT * FROM Currency WHERE Name LIKE '" + name + "' LIMIT 1 COLLATE NOCASE", Array.Empty<object>()).FirstOrDefault();
		}

		public Currency GetCoinCurrency()
		{
			return Connection.Find<Currency>(1);
		}

		public void Dispose()
		{
			Connection.Dispose();
		}
	}
}
