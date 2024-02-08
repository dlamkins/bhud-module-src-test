using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD.Modules.Managers;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Models;
using MysticCrafting.Models.Items;
using MysticCrafting.Models.TradingPost;
using MysticCrafting.Module.Repositories;
using MysticCrafting.Module.Services.Recurring;
using MysticCrafting.Module.Strings;

namespace MysticCrafting.Module.Services.API
{
	public class TradingPostService : RecurringService, ITradingPostService, IRecurringService
	{
		private readonly Gw2ApiManager _gw2ApiManager;

		private readonly IItemRepository _itemRepository;

		public override string Name => Common.LoadingTradingPost;

		private IList<TradingPostItemPrices> ItemPrices { get; set; } = new List<TradingPostItemPrices>();


		public TradingPostService(Gw2ApiManager apiManager, IItemRepository itemRepository)
		{
			_gw2ApiManager = apiManager;
			_itemRepository = itemRepository;
		}

		public TradingPostItemPrices GetItemPrices(int itemId)
		{
			return ItemPrices.FirstOrDefault((TradingPostItemPrices i) => i.Id == itemId);
		}

		public override async Task<string> LoadAsync()
		{
			List<int> itemIds2 = _itemRepository.GetItems()?.Select((MysticItem i) => i.Id)?.ToList();
			if (_gw2ApiManager == null || itemIds2 == null || !itemIds2.Any())
			{
				throw new Exception("No items were found in the ItemRepository.");
			}
			if (_gw2ApiManager.HasPermission(TokenPermission.Tradingpost))
			{
				IApiV2ObjectList<int> allItemIds = await _gw2ApiManager.Gw2ApiClient.V2.Commerce.Prices.IdsAsync();
				itemIds2 = itemIds2.Where((int id) => allItemIds.Contains(id)).ToList();
				ItemPrices = (await _gw2ApiManager.Gw2ApiClient.V2.Commerce.Prices.ManyAsync(itemIds2)).Select(TradingPostItemPrices.From).ToList();
				return $"{ItemPrices.Count} prices loaded";
			}
			throw new Exception("One or more of the required permissions are missing: Tradingpost.");
		}
	}
}
