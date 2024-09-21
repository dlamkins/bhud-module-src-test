using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Atzie.MysticCrafting.Models.Items;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using MysticCrafting.Models.TradingPost;
using MysticCrafting.Module.Strings;
using SQLite;

namespace MysticCrafting.Module.Services.API
{
	public class TradingPostService : ApiService, ITradingPostService, IApiService
	{
		private static readonly Logger Logger = Logger.GetLogger<TradingPostService>();

		private readonly Gw2ApiManager _gw2ApiManager;

		public override string Name => Common.LoadingTradingPost;

		private SQLiteAsyncConnection Connection { get; }

		public override List<TokenPermission> Permissions => new List<TokenPermission> { (TokenPermission)8 };

		public new bool CanReloadManually => false;

		public event EventHandler<ItemPriceChangedEventArgs> ItemPriceChanged;

		public TradingPostService(Gw2ApiManager apiManager, ISqliteDbService dbService)
			: base(apiManager)
		{
			_gw2ApiManager = apiManager;
			Connection = new SQLiteAsyncConnection(dbService.DatabaseFilePath);
			base.ExecutionIntervalMinutes = 5;
		}

		private bool CanLoad()
		{
			if (_gw2ApiManager == null)
			{
				Logger.Error("Gw2ApiManager is null.");
				return false;
			}
			if (!_gw2ApiManager.HasPermission((TokenPermission)8))
			{
				Logger.Error("Failed to retrieve Trading Post prices due missing permission 'Tradingpost'.");
				return false;
			}
			return true;
		}

		public async Task<IList<Item>> UpdatePricesSafeAsync(IList<Item> items)
		{
			if (!CanLoad() || items == null || items.Count == 0)
			{
				return null;
			}
			try
			{
				base.Loading = true;
				IList<Item> obj = await UpdatePricesAsync(items);
				base.LastLoaded = DateTime.Now;
				IList<Item> list = obj;
				return list ?? new List<Item>();
			}
			catch (Exception ex)
			{
				Logger.Error("Exception occurred while trying to retrieve Trading Post prices from the GW2 API: " + ex.Message + ".");
				base.LastFailed = DateTime.Now;
			}
			finally
			{
				base.Loading = false;
				base.Loaded = true;
			}
			return null;
		}

		public async Task<IList<Item>> UpdatePricesAsync(IList<Item> items)
		{
			items = items.Where((Item i) => i.CanBeTraded).ToList();
			List<int> ids = items.Select((Item i) => i.Id).ToList();
			if (!ids.Any())
			{
				return null;
			}
			List<TradingPostItemPrices> list = (await ((IBulkExpandableClient<CommercePrices, int>)(object)_gw2ApiManager.get_Gw2ApiClient().get_V2().get_Commerce()
				.get_Prices()).ManyAsync((IEnumerable<int>)ids, default(CancellationToken))).Select(TradingPostItemPrices.From).ToList();
			List<Item> itemsUpdated = new List<Item>();
			foreach (TradingPostItemPrices price in list)
			{
				Item item2 = items.FirstOrDefault((Item i) => i.Id == price.Id);
				if (item2 != null && (!item2.CanBeTraded || item2.TradingPostBuy != price.BuyPrice.UnitPrice || item2.TradingPostSell != price.SellPrice.UnitPrice))
				{
					item2.CanBeTraded = true;
					item2.TradingPostBuy = price.BuyPrice.UnitPrice;
					item2.TradingPostSell = price.SellPrice.UnitPrice;
					item2.TradingPostLastUpdated = DateTime.Now;
					itemsUpdated.Add(item2);
				}
			}
			if (itemsUpdated.Any())
			{
				await Connection.UpdateAllAsync(itemsUpdated);
			}
			foreach (Item item in itemsUpdated)
			{
				this.ItemPriceChanged?.Invoke(this, new ItemPriceChangedEventArgs
				{
					Item = item
				});
			}
			return itemsUpdated;
		}

		public override Task<string> LoadAsync()
		{
			throw new NotImplementedException("Trading Post service only supports loading specific item prices.");
		}
	}
}
