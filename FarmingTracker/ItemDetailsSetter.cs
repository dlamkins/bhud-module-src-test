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
	public class ItemDetailsSetter
	{
		private const string GW2_API_DOES_NOT_KNOW_IDS = "all ids provided are invalid";

		public static async Task SetItemDetailsFromApi(Dictionary<int, Stat> itemById, Gw2ApiManager gw2ApiManager)
		{
			List<int> itemIdsWithoutDetails = (from i in itemById.Values
				where i.Details.State == ApiStatDetailsState.MissingBecauseApiNotCalledYet
				select i.ApiId).ToList();
			if (!itemIdsWithoutDetails.Any())
			{
				return;
			}
			if (DebugMode.DebugLoggingRequired)
			{
				Module.Logger.Debug("items      no details " + string.Join(" ", itemIdsWithoutDetails));
			}
			Task<IReadOnlyList<Item>> apiItemsTask = ((IBulkExpandableClient<Item, int>)(object)gw2ApiManager.get_Gw2ApiClient().get_V2().get_Items()).ManyAsync((IEnumerable<int>)itemIdsWithoutDetails, default(CancellationToken));
			Task<IReadOnlyList<CommercePrices>> apiPricesTask = ((IBulkExpandableClient<CommercePrices, int>)(object)gw2ApiManager.get_Gw2ApiClient().get_V2().get_Commerce()
				.get_Prices()).ManyAsync((IEnumerable<int>)itemIdsWithoutDetails, default(CancellationToken));
			IReadOnlyList<Item> apiItems = null;
			IReadOnlyList<CommercePrices> apiPrices = null;
			try
			{
				await Task.WhenAll(apiItemsTask, apiPricesTask);
			}
			catch (Exception e)
			{
				if (apiItemsTask.IsFaulted)
				{
					if (!ApiCouldNotFindIds(apiItemsTask.Exception))
					{
						throw new Gw2ApiException("API error: get stat details from v2.items failed", e);
					}
					apiItems = new List<Item>();
				}
				if (apiPricesTask.IsFaulted)
				{
					if (!ApiCouldNotFindIds(apiPricesTask.Exception))
					{
						throw new Gw2ApiException("API error: get trading post prices from v2.prices failed", e);
					}
					apiPrices = new List<CommercePrices>();
				}
			}
			if (apiItems == null)
			{
				apiItems = apiItemsTask.Result;
			}
			if (apiPrices == null)
			{
				apiPrices = apiPricesTask.Result;
			}
			foreach (CommercePrices apiPrice in apiPrices)
			{
				Stat stat = itemById[apiPrice.get_Id()];
				stat.Details.Unsigned_SellsUnitPriceInCopper = apiPrice.get_Sells().get_UnitPrice();
				stat.Details.Unsigned_BuysUnitPriceInCopper = apiPrice.get_Buys().get_UnitPrice();
			}
			if (apiItems.Any() && DebugMode.DebugLoggingRequired)
			{
				Module.Logger.Debug("items      from api   " + string.Join(" ", apiItems.Select((Item c) => c.get_Id())));
			}
			foreach (Item apiItem in apiItems)
			{
				Stat stat2 = itemById[apiItem.get_Id()];
				stat2.Details.Name = apiItem.get_Name();
				stat2.Details.Description = apiItem.get_Description() ?? "";
				stat2.Details.IconAssetId = TextureService.GetIconAssetId(apiItem.get_Icon());
				stat2.Details.Rarity = ApiEnum<ItemRarity>.op_Implicit(apiItem.get_Rarity());
				stat2.Details.ItemFlags = apiItem.get_Flags();
				stat2.Details.Type = ApiEnum<ItemType>.op_Implicit(apiItem.get_Type());
				stat2.Details.WikiSearchTerm = apiItem.get_ChatLink();
				stat2.Details.Unsigned_VendorValueInCopper = apiItem.get_VendorValue();
				stat2.Details.State = ApiStatDetailsState.SetByApi;
			}
			List<Stat> itemsUnknownByApi = itemById.Values.Where((Stat i) => i.Details.State == ApiStatDetailsState.MissingBecauseApiNotCalledYet).ToList();
			if (!itemsUnknownByApi.Any())
			{
				return;
			}
			if (DebugMode.DebugLoggingRequired)
			{
				Module.Logger.Debug("items      api MISS   " + string.Join(" ", itemsUnknownByApi.Select((Stat i) => i.ApiId)));
			}
			foreach (Stat item in itemsUnknownByApi)
			{
				item.Details.State = ApiStatDetailsState.MissingBecauseUnknownByApi;
			}
		}

		private static bool ApiCouldNotFindIds(Exception exception)
		{
			return exception.InnerException.Message.Contains("all ids provided are invalid");
		}
	}
}
