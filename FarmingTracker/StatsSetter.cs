using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD.Modules.Managers;

namespace FarmingTracker
{
	public class StatsSetter
	{
		private Dictionary<int, CurrencyDetails> _currencyDetailsByIdCache = new Dictionary<int, CurrencyDetails>();

		public async Task SetDetailsAndProfitFromApi(Dictionary<int, Stat> itemById, Dictionary<int, Stat> currencyById, Gw2ApiManager gw2ApiManager)
		{
			if (HasToInitializeCache())
			{
				_currencyDetailsByIdCache = await CurrencyDetailsSetter.CreateCacheWithAllApiCurrencies(gw2ApiManager);
			}
			CurrencyDetailsSetter.SetCurrencyDetailsFromCache(currencyById, _currencyDetailsByIdCache);
			await ItemDetailsSetter.SetItemDetailsFromApi(itemById, gw2ApiManager);
			StatProfitSetter.SetProfits(itemById);
		}

		private bool HasToInitializeCache()
		{
			return !_currencyDetailsByIdCache.Any();
		}
	}
}
