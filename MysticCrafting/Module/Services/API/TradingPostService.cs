using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD.Modules.Managers;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
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
			IList<int> itemIds2 = _itemRepository.GetItemIds();
			if (_gw2ApiManager == null || itemIds2 == null || !itemIds2.Any())
			{
				throw new Exception("No items were found in the ItemRepository.");
			}
			if (_gw2ApiManager.HasPermission((TokenPermission)8))
			{
				IApiV2ObjectList<int> allItemIds = await ((IBulkExpandableClient<CommercePrices, int>)(object)_gw2ApiManager.get_Gw2ApiClient().get_V2().get_Commerce()
					.get_Prices()).IdsAsync(default(CancellationToken));
				itemIds2 = itemIds2.Where((int id) => ((IEnumerable<int>)allItemIds).Contains(id)).ToList();
				ItemPrices = (await ((IBulkExpandableClient<CommercePrices, int>)(object)_gw2ApiManager.get_Gw2ApiClient().get_V2().get_Commerce()
					.get_Prices()).ManyAsync((IEnumerable<int>)itemIds2, default(CancellationToken))).Select(TradingPostItemPrices.From).ToList();
				return $"{ItemPrices.Count} prices loaded";
			}
			throw new Exception("One or more of the required permissions are missing: Tradingpost.");
		}
	}
}
