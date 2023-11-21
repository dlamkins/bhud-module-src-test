using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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

		protected override async Task<List<Item>> Fetch(Gw2ApiManager apiManager, IProgress<string> progress, CancellationToken cancellationToken)
		{
			List<Item> items = new List<Item>();
			progress.Report("Load item ids...");
			IApiV2ObjectList<int> itemIds = await ((IBulkExpandableClient<Item, int>)(object)apiManager.get_Gw2ApiClient().get_V2().get_Items()).IdsAsync(cancellationToken);
			progress.Report($"Loading items... 0/{((IReadOnlyCollection<int>)itemIds).Count}");
			Logger.Info($"Start loading items: {((IEnumerable<int>)itemIds).First()} - {((IEnumerable<int>)itemIds).Last()}");
			int loadedItems = 0;
			int chunkSize = 200;
			IEnumerable<IEnumerable<IEnumerable<int>>> chunkGroups = ((IEnumerable<int>)itemIds).ChunkBy(chunkSize).ChunkBy(20);
			foreach (IEnumerable<IEnumerable<int>> item in chunkGroups)
			{
				cancellationToken.ThrowIfCancellationRequested();
				List<Task<List<Item>>> tasks = new List<Task<List<Item>>>();
				foreach (IEnumerable<int> idChunk in item)
				{
					cancellationToken.ThrowIfCancellationRequested();
					tasks.Add(FetchChunk(apiManager, idChunk, cancellationToken).ContinueWith(delegate(Task<List<Item>> resultTask)
					{
						List<Item> list = (resultTask.IsFaulted ? new List<Item>() : resultTask.Result);
						int num = Interlocked.Add(ref loadedItems, list.Count);
						progress.Report($"Loading items... {num}/{((IReadOnlyCollection<int>)itemIds).Count}");
						return list;
					}));
				}
				List<Item> fetchedItems = (await Task.WhenAll(tasks)).SelectMany((List<Item> i) => i).ToList();
				items.AddRange(fetchedItems);
			}
			return items;
		}

		private async Task<List<Item>> FetchChunk(Gw2ApiManager apiManager, IEnumerable<int> itemIdChunk, CancellationToken cancellationToken)
		{
			Logger.Debug($"Start loading items by id: {itemIdChunk.First()} - {itemIdChunk.Last()}");
			IReadOnlyList<Item> source = await ((IBulkExpandableClient<Item, int>)(object)apiManager.get_Gw2ApiClient().get_V2().get_Items()).ManyAsync(itemIdChunk, cancellationToken);
			Logger.Debug($"Finished loading items by id: {itemIdChunk.First()} - {itemIdChunk.Last()}");
			return source.Select(Item.FromAPI).ToList();
		}
	}
}
