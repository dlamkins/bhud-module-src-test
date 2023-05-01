using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD.Modules.Managers;
using Estreya.BlishHUD.Shared.Extensions;
using Estreya.BlishHUD.Shared.Models.GW2API.Commerce;
using Estreya.BlishHUD.Shared.Models.GW2API.Items;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;

namespace Estreya.BlishHUD.Shared.Services
{
	public class TradingPostService : APIService<TradingPostService.TransactionMapping>
	{
		public enum TransactionMappingType
		{
			Sell,
			Buy,
			Own
		}

		public struct TransactionMapping
		{
			public TransactionMappingType Type;

			public List<Transaction> Transactions;
		}

		private readonly ItemService _itemService;

		public TransactionMappingType Scopes { get; set; } = TransactionMappingType.Own;


		public List<Transaction> Buys => base.APIObjectList.Where((TransactionMapping mapping) => mapping.Type == TransactionMappingType.Buy).SelectMany((TransactionMapping mapping) => mapping.Transactions).ToList();

		public List<Transaction> Sells => base.APIObjectList.Where((TransactionMapping mapping) => mapping.Type == TransactionMappingType.Sell).SelectMany((TransactionMapping mapping) => mapping.Transactions).ToList();

		public List<PlayerTransaction> OwnTransactions => base.APIObjectList.Where((TransactionMapping mapping) => mapping.Type == TransactionMappingType.Own).SelectMany((TransactionMapping mapping) => mapping.Transactions.Select((Transaction transactions) => transactions as PlayerTransaction)).ToList();

		public TradingPostService(APIServiceConfiguration configuration, Gw2ApiManager apiManager, ItemService itemService)
			: base(apiManager, configuration)
		{
			_itemService = itemService;
		}

		protected override async Task<List<TransactionMapping>> Fetch(Gw2ApiManager apiManager, IProgress<string> progress)
		{
			bool num = (Scopes & TransactionMappingType.Own) != 0;
			bool loadBuy = (Scopes & TransactionMappingType.Buy) != 0;
			bool loadSell = (Scopes & TransactionMappingType.Sell) != 0;
			List<TransactionMapping> transactions = new List<TransactionMapping>();
			TransactionMapping item2;
			if (num)
			{
				progress.Report("Loading player buy orders...");
				List<Transaction> buys = ((IEnumerable<CommerceTransactionCurrent>)((IEnumerable<CommerceTransactionCurrent>)(await ((IBlobClient<IApiV2ObjectList<CommerceTransactionCurrent>>)(object)apiManager.get_Gw2ApiClient().get_V2().get_Commerce()
					.get_Transactions()
					.get_Current()
					.get_Buys()).GetAsync(default(CancellationToken)))).ToList()).Select((Func<CommerceTransactionCurrent, Transaction>)((CommerceTransactionCurrent x) => new PlayerTransaction
				{
					ItemId = x.get_ItemId(),
					Price = x.get_Price(),
					Quantity = x.get_Quantity(),
					Created = x.get_Created().UtcDateTime,
					Type = TransactionType.Buy
				})).ToList();
				if (buys.Count > 0)
				{
					item2 = new TransactionMapping
					{
						Type = TransactionMappingType.Own,
						Transactions = buys
					};
					transactions.Add(item2);
				}
				progress.Report("Loading player sell offers...");
				List<Transaction> sells = ((IEnumerable<CommerceTransactionCurrent>)((IEnumerable<CommerceTransactionCurrent>)(await ((IBlobClient<IApiV2ObjectList<CommerceTransactionCurrent>>)(object)apiManager.get_Gw2ApiClient().get_V2().get_Commerce()
					.get_Transactions()
					.get_Current()
					.get_Sells()).GetAsync(default(CancellationToken)))).ToList()).Select((Func<CommerceTransactionCurrent, Transaction>)((CommerceTransactionCurrent x) => new PlayerTransaction
				{
					ItemId = x.get_ItemId(),
					Price = x.get_Price(),
					Quantity = x.get_Quantity(),
					Created = x.get_Created().UtcDateTime,
					Type = TransactionType.Sell
				})).ToList();
				if (sells.Count > 0)
				{
					item2 = new TransactionMapping
					{
						Type = TransactionMappingType.Own,
						Transactions = sells
					};
					transactions.Add(item2);
				}
				IEnumerable<int> itemIds = transactions.SelectMany((TransactionMapping transaction) => transaction.Transactions.Select((Transaction transaction) => transaction.ItemId)).Distinct();
				progress.Report("Check highest transactions...");
				Dictionary<int, CommercePrices> itemPriceLookup = (await ((IBulkExpandableClient<CommercePrices, int>)(object)apiManager.get_Gw2ApiClient().get_V2().get_Commerce()
					.get_Prices()).ManyAsync(itemIds, default(CancellationToken))).ToDictionary((CommercePrices item) => item.get_Id());
				foreach (TransactionMapping item5 in transactions.Where((TransactionMapping mapping) => mapping.Type == TransactionMappingType.Own))
				{
					foreach (Transaction transaction3 in item5.Transactions)
					{
						PlayerTransaction playerTransaction = transaction3 as PlayerTransaction;
						if (playerTransaction != null)
						{
							switch (transaction3.Type)
							{
							case TransactionType.Buy:
								playerTransaction.IsHighest = itemPriceLookup[transaction3.ItemId].get_Buys().get_UnitPrice() == transaction3.Price;
								break;
							case TransactionType.Sell:
								playerTransaction.IsHighest = itemPriceLookup[transaction3.ItemId].get_Sells().get_UnitPrice() == transaction3.Price;
								break;
							}
						}
					}
				}
			}
			if ((transactions.Count > 0 || loadBuy || loadSell) && !(await _itemService.WaitForCompletion(TimeSpan.FromMinutes(10.0))))
			{
				Logger.Warn("ItemService did not complete in the predefined timespan.");
			}
			if (loadBuy || loadSell)
			{
				List<Item> tradeableItems = _itemService.Items.Where((Item item) => !item.Flags.Any((ItemFlag flag) => (int)flag == 2 || (int)flag == 14)).ToList();
				progress.Report("Loading global buys/sells...");
				foreach (IEnumerable<Item> itemChunk in tradeableItems.ChunkBy(200))
				{
					progress.Report($"Loading global buys/sells from items {itemChunk.First().Id} - {itemChunk.Last().Id}...");
					List<Transaction> mappedListings = (await ((IBulkExpandableClient<CommerceListings, int>)(object)apiManager.get_Gw2ApiClient().get_V2().get_Commerce()
						.get_Listings()).ManyAsync(itemChunk.Select((Item item) => item.Id), _cancellationTokenSource.Token)).ToList().SelectMany(delegate(CommerceListings itemListing)
					{
						List<Transaction> list = new List<Transaction>();
						if (loadBuy)
						{
							progress.Report($"Loading global buys: {itemListing.get_Buys().Count}...");
							foreach (CommerceListing current in itemListing.get_Buys())
							{
								Transaction item3 = new Transaction
								{
									ItemId = itemListing.get_Id(),
									Price = current.get_UnitPrice(),
									Quantity = current.get_Quantity(),
									Created = DateTime.MinValue,
									Type = TransactionType.Buy
								};
								list.Add(item3);
							}
						}
						if (loadSell)
						{
							progress.Report($"Loading global sells: {itemListing.get_Sells().Count}...");
							{
								foreach (CommerceListing current2 in itemListing.get_Sells())
								{
									Transaction item4 = new Transaction
									{
										ItemId = itemListing.get_Id(),
										Price = current2.get_UnitPrice(),
										Quantity = current2.get_Quantity(),
										Created = DateTime.MinValue,
										Type = TransactionType.Sell
									};
									list.Add(item4);
								}
								return list;
							}
						}
						return list;
					}).ToList();
					item2 = new TransactionMapping
					{
						Type = TransactionMappingType.Buy,
						Transactions = mappedListings.Where((Transaction mappedListing) => mappedListing.Type == TransactionType.Buy).ToList()
					};
					transactions.Add(item2);
					item2 = new TransactionMapping
					{
						Type = TransactionMappingType.Sell,
						Transactions = mappedListings.Where((Transaction mappedListing) => mappedListing.Type == TransactionType.Sell).ToList()
					};
					transactions.Add(item2);
				}
			}
			progress.Report("Loading items...");
			foreach (TransactionMapping item6 in transactions)
			{
				foreach (Transaction transaction2 in item6.Transactions)
				{
					transaction2.Item = _itemService.GetItemById(transaction2.ItemId);
				}
			}
			return transactions;
		}

		public async Task<int> GetPriceForItem(int itemId, TransactionType transactionType)
		{
			switch (transactionType)
			{
			case TransactionType.Buy:
			{
				IEnumerable<Transaction> itemBuys = Buys.Where((Transaction buy) => buy.ItemId == itemId);
				if (itemBuys.Any())
				{
					return itemBuys.First().Price;
				}
				return (await ((IBulkExpandableClient<CommercePrices, int>)(object)_apiManager.get_Gw2ApiClient().get_V2().get_Commerce()
					.get_Prices()).GetAsync(itemId, default(CancellationToken))).get_Buys().get_UnitPrice();
			}
			case TransactionType.Sell:
			{
				IEnumerable<Transaction> itemSells = Sells.Where((Transaction sell) => sell.ItemId == itemId);
				if (itemSells.Any())
				{
					return itemSells.First().Price;
				}
				return (await ((IBulkExpandableClient<CommercePrices, int>)(object)_apiManager.get_Gw2ApiClient().get_V2().get_Commerce()
					.get_Prices()).GetAsync(itemId, default(CancellationToken))).get_Sells().get_UnitPrice();
			}
			default:
				throw new ArgumentException($"Invalid transaction type: {transactionType}");
			}
		}
	}
}
