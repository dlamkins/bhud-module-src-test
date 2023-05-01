using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD.Modules.Managers;
using Estreya.BlishHUD.Shared.Extensions;
using Estreya.BlishHUD.Shared.Models.GW2API.Items;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;

namespace Estreya.BlishHUD.Shared.Services
{
	public class ItemService : FilesystemAPIService<Item>
	{
		protected override string BASE_FOLDER_STRUCTURE => "items";

		protected override string FILE_NAME => "items.json";

		public List<Item> Items => base.APIObjectList;

		public ItemService(APIServiceConfiguration configuration, Gw2ApiManager apiManager, string baseFolderPath)
			: base(apiManager, configuration, baseFolderPath)
		{
		}

		public Item GetItemByName(string name)
		{
			if (!_apiObjectListLock.IsFree())
			{
				return null;
			}
			using (_apiObjectListLock.Lock())
			{
				foreach (Item item in Items)
				{
					if (item.Name == name)
					{
						return item;
					}
				}
				return null;
			}
		}

		public Item GetItemById(int id)
		{
			if (!_apiObjectListLock.IsFree())
			{
				return null;
			}
			using (_apiObjectListLock.Lock())
			{
				foreach (Item item in Items)
				{
					if (item.Id == id)
					{
						return item;
					}
				}
				return null;
			}
		}

		protected override async Task<List<Item>> Fetch(Gw2ApiManager apiManager, IProgress<string> progress)
		{
			List<Item> items = new List<Item>();
			progress.Report("Load item ids...");
			IApiV2ObjectList<int> itemIds = await ((IBulkExpandableClient<Item, int>)(object)apiManager.get_Gw2ApiClient().get_V2().get_Items()).IdsAsync(_cancellationTokenSource.Token);
			Logger.Info($"Start loading items: {((IEnumerable<int>)itemIds).First()} - {((IEnumerable<int>)itemIds).Last()}");
			int chunkSize = 200;
			foreach (IEnumerable<int> itemIdChunk in ((IEnumerable<int>)itemIds).ChunkBy(chunkSize))
			{
				if (_cancellationTokenSource.Token.IsCancellationRequested)
				{
					break;
				}
				try
				{
					try
					{
						items.AddRange(await FetchChunk(apiManager, progress, itemIdChunk));
					}
					catch (Exception ex2)
					{
						Logger.Warn(ex2, $"Failed loading items with chunk size {chunkSize}:");
						chunkSize = 10;
						Logger.Debug($"Try load failed chunk in smaller chunk size: {chunkSize}");
						foreach (IEnumerable<int> smallerItemIdChunk in itemIdChunk.ChunkBy(chunkSize))
						{
							try
							{
								items.AddRange(await FetchChunk(apiManager, progress, smallerItemIdChunk));
							}
							catch (Exception smallerEx)
							{
								Logger.Warn(smallerEx, $"Failed loading items with chunk size {chunkSize}:");
							}
						}
					}
				}
				catch (Exception ex)
				{
					Logger.Warn(ex, "Failed loading items:");
				}
			}
			return items;
		}

		private async Task<List<Item>> FetchChunk(Gw2ApiManager apiManager, IProgress<string> progress, IEnumerable<int> itemIdChunk)
		{
			string message = $"Start loading items by id: {itemIdChunk.First()} - {itemIdChunk.Last()}";
			progress.Report(message);
			Logger.Debug(message);
			return (await ((IBulkExpandableClient<Item, int>)(object)apiManager.get_Gw2ApiClient().get_V2().get_Items()).ManyAsync(itemIdChunk, _cancellationTokenSource.Token)).Select((Item apiItem) => Item.FromAPI(apiItem)).ToList();
		}
	}
}
