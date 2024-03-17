using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD.Modules.Managers;
using Estreya.BlishHUD.Shared.Models.GW2API.Commerce;
using Estreya.BlishHUD.Shared.Models.GW2API.Items;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;

namespace Estreya.BlishHUD.Shared.Services.TradingPost
{
	public class PlayerTransactionsService : APIService<PlayerTransaction>
	{
		private readonly ItemService _itemService;

		public List<PlayerTransaction> Buys => base.APIObjectList?.Where((PlayerTransaction x) => x.Type == TransactionType.Buy).ToList();

		public List<PlayerTransaction> Sells => base.APIObjectList?.Where((PlayerTransaction x) => x.Type == TransactionType.Sell).ToList();

		public List<PlayerTransaction> Transactions => base.APIObjectList?.ToArray().ToList();

		public PlayerTransactionsService(APIServiceConfiguration configuration, ItemService itemService, Gw2ApiManager apiManager)
			: base(apiManager, configuration)
		{
			_itemService = itemService;
		}

		protected override async Task<List<PlayerTransaction>> Fetch(Gw2ApiManager apiManager, IProgress<string> progress, CancellationToken cancellationToken)
		{
			List<PlayerTransaction> transactions = new List<PlayerTransaction>();
			progress.Report("Loading player buy orders...");
			List<PlayerTransaction> buys = (from x in ((IEnumerable<CommerceTransactionCurrent>)(await ((IBlobClient<IApiV2ObjectList<CommerceTransactionCurrent>>)(object)apiManager.get_Gw2ApiClient().get_V2().get_Commerce()
					.get_Transactions()
					.get_Current()
					.get_Buys()).GetAsync(cancellationToken))).ToList()
				select new PlayerTransaction
				{
					ItemId = x.get_ItemId(),
					Price = x.get_Price(),
					Quantity = x.get_Quantity(),
					Created = x.get_Created().UtcDateTime,
					Type = TransactionType.Buy
				}).ToList();
			if (buys.Count > 0)
			{
				transactions.AddRange(buys);
			}
			progress.Report("Loading player sell offers...");
			List<PlayerTransaction> sells = (from x in ((IEnumerable<CommerceTransactionCurrent>)(await ((IBlobClient<IApiV2ObjectList<CommerceTransactionCurrent>>)(object)apiManager.get_Gw2ApiClient().get_V2().get_Commerce()
					.get_Transactions()
					.get_Current()
					.get_Sells()).GetAsync(cancellationToken))).ToList()
				select new PlayerTransaction
				{
					ItemId = x.get_ItemId(),
					Price = x.get_Price(),
					Quantity = x.get_Quantity(),
					Created = x.get_Created().UtcDateTime,
					Type = TransactionType.Sell
				}).ToList();
			if (sells.Count > 0)
			{
				transactions.AddRange(sells);
			}
			IEnumerable<int> itemIds = transactions.Select((PlayerTransaction t) => t.ItemId).Distinct();
			progress.Report("Check highest transactions...");
			Dictionary<int, CommercePrices> itemPriceLookup = (await ((IBulkExpandableClient<CommercePrices, int>)(object)apiManager.get_Gw2ApiClient().get_V2().get_Commerce()
				.get_Prices()).ManyAsync(itemIds, cancellationToken)).ToDictionary((CommercePrices item) => item.get_Id());
			foreach (PlayerTransaction transaction in transactions)
			{
				switch (transaction.Type)
				{
				case TransactionType.Buy:
					transaction.IsHighest = itemPriceLookup[transaction.ItemId].get_Buys().get_UnitPrice() == transaction.Price;
					break;
				case TransactionType.Sell:
					transaction.IsHighest = itemPriceLookup[transaction.ItemId].get_Sells().get_UnitPrice() == transaction.Price;
					break;
				}
			}
			if (transactions.Count > 0)
			{
				progress.Report("Waiting for " + _itemService.GetType().Name + " to complete...");
				if (!(await _itemService.WaitForCompletion(TimeSpan.FromMinutes(10.0))))
				{
					Logger.Warn("ItemService did not complete in the predefined timespan.");
				}
			}
			progress.Report("Loading items...");
			foreach (IGrouping<int, PlayerTransaction> itemGroup in from x in transactions
				group x by x.ItemId)
			{
				Item item2 = _itemService.GetItemById(itemGroup.Key);
				foreach (PlayerTransaction item3 in itemGroup)
				{
					item3.Item = item2;
				}
			}
			return transactions;
		}
	}
}
