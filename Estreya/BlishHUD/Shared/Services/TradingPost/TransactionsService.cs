using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD.Modules.Managers;
using Estreya.BlishHUD.Shared.Extensions;
using Estreya.BlishHUD.Shared.Models.GW2API.Commerce;
using Estreya.BlishHUD.Shared.Models.GW2API.Items;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;

namespace Estreya.BlishHUD.Shared.Services.TradingPost
{
	public class TransactionsService : APIService<Transaction>
	{
		private readonly ItemService _itemService;

		private readonly SynchronizedCollection<int> _subscribedItemIds = new SynchronizedCollection<int>();

		public List<Transaction> Buys => base.APIObjectList?.Where((Transaction x) => x.Type == TransactionType.Buy).ToList();

		public List<Transaction> Sells => base.APIObjectList?.Where((Transaction x) => x.Type == TransactionType.Sell).ToList();

		public List<Transaction> Transactions => base.APIObjectList?.ToArray().ToList();

		public TransactionsService(APIServiceConfiguration configuration, ItemService itemService, Gw2ApiManager apiManager)
			: base(apiManager, configuration)
		{
			_itemService = itemService;
		}

		public async Task AddItemSubscribtions(params int[] ids)
		{
			foreach (int id in ids)
			{
				if (!_subscribedItemIds.Contains(id))
				{
					await ((IBulkExpandableClient<CommerceListings, int>)(object)_apiManager.get_Gw2ApiClient().get_V2().get_Commerce()
						.get_Listings()).GetAsync(id, default(CancellationToken));
					_subscribedItemIds.Add(id);
					Logger.Debug($"Added item {id} to subscribers.");
				}
			}
			await Reload();
		}

		protected override async Task<List<Transaction>> Fetch(Gw2ApiManager apiManager, IProgress<string> progress, CancellationToken cancellationToken)
		{
			List<int> tradeableItems = _subscribedItemIds.ToArray().ToList();
			int loadedItemIds = 0;
			progress.Report($"Loading buys/sells for {loadedItemIds}/{tradeableItems.Count} items: 0 transactions.");
			List<IEnumerable<IEnumerable<int>>> parallelItemChunks = tradeableItems.OrderBy((int x) => x).ChunkBy(200).ToList()
				.ChunkBy(1)
				.ToList();
			List<Transaction> transactions = new List<Transaction>();
			foreach (IEnumerable<IEnumerable<int>> parallelItemChunk in parallelItemChunks)
			{
				transactions.AddRange((await Task.WhenAll(parallelItemChunk.Select((IEnumerable<int> x) => LoadTransactions(x, apiManager, cancellationToken)).ToArray())).SelectMany((List<Transaction> x) => x));
				Interlocked.Add(ref loadedItemIds, parallelItemChunk.SelectMany((IEnumerable<int> x) => x).Count());
				progress.Report($"Loading buys/sells for {loadedItemIds}/{tradeableItems.Count} items: {transactions.Count} transactions.");
			}
			bool loadItems = transactions.Count > 0;
			if (loadItems)
			{
				progress.Report("Waiting for " + _itemService.GetType().Name + " to complete...");
				if (!(await _itemService.WaitForCompletion(TimeSpan.FromMinutes(10.0))))
				{
					loadItems = false;
					Logger.Warn("ItemService did not complete in the predefined timespan.");
				}
			}
			if (loadItems)
			{
				progress.Report("Loading items...");
				{
					foreach (IGrouping<int, Transaction> itemGroup in from x in transactions
						group x by x.ItemId)
					{
						Item item = _itemService.GetItemById(itemGroup.Key);
						foreach (Transaction item2 in itemGroup)
						{
							item2.Item = item;
						}
					}
					return transactions;
				}
			}
			return transactions;
		}

		private async Task<List<Transaction>> LoadTransactions(IEnumerable<int> ids, Gw2ApiManager apiManager, CancellationToken cancellationToken)
		{
			List<Transaction> transactions = new List<Transaction>();
			try
			{
				List<Transaction> mappedListings = (await ((IBulkExpandableClient<CommerceListings, int>)(object)apiManager.get_Gw2ApiClient().get_V2().get_Commerce()
					.get_Listings()).ManyAsync(ids, cancellationToken)).ToList().SelectMany(delegate(CommerceListings itemListing)
				{
					List<Transaction> list = new List<Transaction>();
					foreach (CommerceListing current in itemListing.get_Buys())
					{
						Transaction item = new Transaction
						{
							ItemId = itemListing.get_Id(),
							Price = current.get_UnitPrice(),
							Quantity = current.get_Quantity(),
							Created = DateTime.MinValue,
							Type = TransactionType.Buy
						};
						list.Add(item);
					}
					foreach (CommerceListing current2 in itemListing.get_Sells())
					{
						Transaction item2 = new Transaction
						{
							ItemId = itemListing.get_Id(),
							Price = current2.get_UnitPrice(),
							Quantity = current2.get_Quantity(),
							Created = DateTime.MinValue,
							Type = TransactionType.Sell
						};
						list.Add(item2);
					}
					return list;
				}).ToList();
				transactions.AddRange(mappedListings);
				return transactions;
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, $"Could not load global buys from items {ids.First()} - {ids.Last()}.");
				return transactions;
			}
		}

		public PriceRange GetBuyPricesForItem(int itemId)
		{
			IEnumerable<Transaction> transactions = from t in Buys
				where t.ItemId == itemId
				select t into b
				orderby b.Price
				select b;
			PriceRange result;
			if (!transactions.Any())
			{
				result = default(PriceRange);
				result.Lowest = 0;
				result.Highest = 0;
				return result;
			}
			result = default(PriceRange);
			result.Lowest = transactions.First().Price;
			result.Highest = transactions.Last().Price;
			return result;
		}

		public int GetBuyQuantity(int itemId)
		{
			return (from t in Buys
				where t.ItemId == itemId
				select t into b
				orderby b.Price
				select b).Sum((Transaction t) => t.Quantity);
		}

		public int GetLowestBuyQuantity(int itemId)
		{
			return (from t in Buys
				where t.ItemId == itemId
				select t into b
				orderby b.Price
				select b).FirstOrDefault()?.Quantity ?? 0;
		}

		public int GetHighestBuyQuantity(int itemId)
		{
			return (from t in Buys
				where t.ItemId == itemId
				select t into b
				orderby b.Price
				select b).LastOrDefault()?.Quantity ?? 0;
		}

		public PriceRange GetSellPricesForItem(int itemId)
		{
			IEnumerable<Transaction> transactions = from t in Sells
				where t.ItemId == itemId
				select t into b
				orderby b.Price
				select b;
			PriceRange result;
			if (!transactions.Any())
			{
				result = default(PriceRange);
				result.Lowest = 0;
				result.Highest = 0;
				return result;
			}
			result = default(PriceRange);
			result.Lowest = transactions.First().Price;
			result.Highest = transactions.Last().Price;
			return result;
		}

		public int GetSellQuantity(int itemId)
		{
			return (from t in Sells
				where t.ItemId == itemId
				select t into b
				orderby b.Price
				select b).Sum((Transaction t) => t.Quantity);
		}

		public int GetLowestSellQuantity(int itemId)
		{
			return (from s in Sells
				where s.ItemId == itemId
				select s into b
				orderby b.Price
				select b).FirstOrDefault()?.Quantity ?? 0;
		}

		public int GetHighestSellQuantity(int itemId)
		{
			return (from s in Sells
				where s.ItemId == itemId
				select s into b
				orderby b.Price
				select b).LastOrDefault()?.Quantity ?? 0;
		}
	}
}
