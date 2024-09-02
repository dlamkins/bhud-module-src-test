using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD.Modules.Managers;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;

namespace FarmingTracker
{
	public class CurrencyDetailsSetter
	{
		public static async Task<Dictionary<int, CurrencyDetails>> CreateCacheWithAllApiCurrencies(Gw2ApiManager gw2ApiManager)
		{
			new List<Currency>();
			IReadOnlyList<Currency> apiCurrencies;
			try
			{
				apiCurrencies = (IReadOnlyList<Currency>)(await ((IAllExpandableClient<Currency>)(object)gw2ApiManager.get_Gw2ApiClient().get_V2().get_Currencies()).AllAsync(default(CancellationToken)));
			}
			catch (Exception e)
			{
				throw new Gw2ApiException("API error: CreateCacheWithAllApiCurrencies", e);
			}
			Dictionary<int, CurrencyDetails> currencyDetailsById = new Dictionary<int, CurrencyDetails>();
			foreach (Currency apiCurrency in apiCurrencies)
			{
				currencyDetailsById[apiCurrency.get_Id()] = new CurrencyDetails
				{
					Name = apiCurrency.get_Name(),
					Description = apiCurrency.get_Description(),
					IconAssetId = TextureService.GetIconAssetId(apiCurrency.get_Icon())
				};
			}
			return currencyDetailsById;
		}

		public static void SetCurrencyDetailsFromCache(Dictionary<int, Stat> currencyById, Dictionary<int, CurrencyDetails> currencyDetailsByIdCache)
		{
			List<Stat> currenciesWithoutDetails = currencyById.Values.Where((Stat c) => c.Details.State == ApiStatDetailsState.MissingBecauseApiNotCalledYet).ToList();
			if (!currenciesWithoutDetails.Any())
			{
				return;
			}
			if (Module.DebugEnabled)
			{
				Module.Logger.Debug("currencies no details " + string.Join(" ", currenciesWithoutDetails.Select((Stat c) => c.ApiId)));
			}
			List<int> missingInApiCurrencyIds = new List<int>();
			foreach (Stat currencyWithoutDetails in currenciesWithoutDetails)
			{
				if (currencyDetailsByIdCache.TryGetValue(currencyWithoutDetails.ApiId, out var currencyDetails))
				{
					currencyWithoutDetails.Details.Name = currencyDetails.Name;
					currencyWithoutDetails.Details.Description = currencyDetails.Description;
					currencyWithoutDetails.Details.IconAssetId = currencyDetails.IconAssetId;
					currencyWithoutDetails.Details.WikiSearchTerm = currencyDetails.Name;
					currencyWithoutDetails.Details.State = ApiStatDetailsState.SetByApi;
				}
				else
				{
					missingInApiCurrencyIds.Add(currencyWithoutDetails.ApiId);
					currencyWithoutDetails.Details.State = ApiStatDetailsState.MissingBecauseUnknownByApi;
				}
			}
			if (missingInApiCurrencyIds.Any() && Module.DebugEnabled)
			{
				Module.Logger.Debug("currencies api miss   " + string.Join(" ", missingInApiCurrencyIds));
			}
		}
	}
}
