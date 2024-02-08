using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MysticCrafting.Models.Commerce;
using MysticCrafting.Module.Services;

namespace MysticCrafting.Module.Repositories
{
	public class CurrencyRepository : ICurrencyRepository, IRepository
	{
		private readonly IDataService _dataService;

		private IList<MysticCurrency> Currencies { get; set; }

		public bool LocalOnly => false;

		public bool Loaded { get; private set; }

		public string FileName => "currency_data.json";

		public CurrencyRepository(IDataService dataService)
		{
			_dataService = dataService;
		}

		public async Task<string> LoadAsync()
		{
			Currencies = (await _dataService.LoadFromFileAsync<List<MysticCurrency>>(FileName)) ?? new List<MysticCurrency>();
			Loaded = true;
			return $"{Currencies.Count} currencies loaded";
		}

		public IList<MysticCurrency> GetCurrencies()
		{
			return Currencies;
		}

		public MysticCurrency GetCurrency(int id)
		{
			return Currencies.FirstOrDefault((MysticCurrency v) => v.Id == id);
		}

		public MysticCurrency GetCurrency(string name)
		{
			return Currencies.FirstOrDefault((MysticCurrency v) => v.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
		}

		public MysticCurrency GetCoinCurrency()
		{
			return Currencies.FirstOrDefault((MysticCurrency v) => v.Id == 1);
		}
	}
}
