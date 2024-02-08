using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JsonFlatFileDataStore;
using MysticCrafting.Models.Commerce;
using MysticCrafting.Module.Services;

namespace MysticCrafting.Module.Repositories
{
	public class FlatCurrencyRepository : ICurrencyRepository, IRepository
	{
		private readonly IDataService _dataService;

		private IDocumentCollection<MysticCurrency> Currencies { get; set; }

		public bool LocalOnly => false;

		public bool Loaded { get; private set; }

		public string FileName => "data.json";

		public FlatCurrencyRepository(IDataService dataService)
		{
			_dataService = dataService;
		}

		public async Task<string> LoadAsync()
		{
			Currencies = ServiceContainer.Store.GetCollection<MysticCurrency>();
			Loaded = true;
			return $"{Currencies.Count} currencies loaded";
		}

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
