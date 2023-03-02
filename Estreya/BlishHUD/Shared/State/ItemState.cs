using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD.Modules.Managers;
using Estreya.BlishHUD.Shared.Extensions;
using Estreya.BlishHUD.Shared.Models.GW2API.Items;
using Estreya.BlishHUD.Shared.Utils;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using Newtonsoft.Json;

namespace Estreya.BlishHUD.Shared.State
{
	public class ItemState : APIState<Item>
	{
		private const string BASE_FOLDER_STRUCTURE = "items";

		private const string FILE_NAME = "items.json";

		private const string LAST_UPDATED_FILE_NAME = "last_updated.txt";

		private const string DATE_TIME_FORMAT = "yyyy-MM-ddTHH:mm:ss";

		private readonly string _baseFolderPath;

		private AsyncLock _itemLock = new AsyncLock();

		private List<Item> _items;

		private string DirectoryPath => Path.Combine(_baseFolderPath, "items");

		public List<Item> Items => _items;

		public ItemState(APIStateConfiguration configuration, Gw2ApiManager apiManager, string baseFolderPath)
			: base(apiManager, configuration)
		{
			_baseFolderPath = baseFolderPath;
			base.Updated += ItemState_Updated;
		}

		private void ItemState_Updated(object sender, EventArgs e)
		{
			using (_itemLock.Lock())
			{
				_items = base.APIObjectList.ToArray().ToList();
			}
		}

		protected override Task DoInitialize()
		{
			using (_itemLock.Lock())
			{
				_items = new List<Item>();
			}
			return Task.CompletedTask;
		}

		protected override Task DoClear()
		{
			using (_itemLock.Lock())
			{
				_items.Clear();
			}
			return Task.CompletedTask;
		}

		protected override async Task Load()
		{
			_ = 3;
			try
			{
				bool canLoadFiles = CanLoadFiles();
				bool shouldLoadFiles = await ShouldLoadFiles();
				if (canLoadFiles)
				{
					try
					{
						base.Loading = true;
						List<Item> items = JsonConvert.DeserializeObject<List<Item>>(await FileUtil.ReadStringAsync(Path.Combine(DirectoryPath, "items.json")));
						using (_itemLock.Lock())
						{
							_items = items;
						}
					}
					finally
					{
						base.Loading = false;
						SignalCompletion();
					}
				}
				if (!shouldLoadFiles)
				{
					await LoadFromAPI(!canLoadFiles);
					if (!_cancellationTokenSource.Token.IsCancellationRequested)
					{
						await Save();
					}
				}
				Logger.Debug("Loaded {0} items.", new object[1] { base.APIObjectList.Count });
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed loading items:");
			}
		}

		private bool CanLoadFiles()
		{
			if (!Directory.Exists(DirectoryPath))
			{
				return false;
			}
			if (!File.Exists(Path.Combine(DirectoryPath, "items.json")))
			{
				return false;
			}
			return true;
		}

		private async Task<bool> ShouldLoadFiles()
		{
			if (!CanLoadFiles())
			{
				return false;
			}
			string lastUpdatedFilePath = Path.Combine(DirectoryPath, "last_updated.txt");
			if (File.Exists(lastUpdatedFilePath))
			{
				if (!DateTime.TryParseExact(await FileUtil.ReadStringAsync(lastUpdatedFilePath), "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var lastUpdated))
				{
					Logger.Debug("Failed parsing last updated.");
					return false;
				}
				DateTime lastUpdatedUTC = new DateTime(lastUpdated.Ticks, DateTimeKind.Utc);
				return lastUpdatedUTC >= DateTime.ParseExact("2022-09-20", "yyyy-MM-dd", CultureInfo.InvariantCulture) && DateTime.UtcNow - lastUpdatedUTC <= TimeSpan.FromDays(5.0);
			}
			return false;
		}

		protected override async Task Save()
		{
			if (Directory.Exists(DirectoryPath))
			{
				Directory.Delete(DirectoryPath, recursive: true);
			}
			Directory.CreateDirectory(DirectoryPath);
			using (await _apiObjectListLock.LockAsync())
			{
				string itemJson = JsonConvert.SerializeObject(base.APIObjectList, Formatting.Indented);
				await FileUtil.WriteStringAsync(Path.Combine(DirectoryPath, "items.json"), itemJson);
			}
			await CreateLastUpdatedFile();
		}

		private async Task CreateLastUpdatedFile()
		{
			await FileUtil.WriteStringAsync(Path.Combine(DirectoryPath, "last_updated.txt"), DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss"));
		}

		public Item GetItemByName(string name)
		{
			if (!_itemLock.IsFree())
			{
				return null;
			}
			using (_itemLock.Lock())
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
			if (!_itemLock.IsFree())
			{
				return null;
			}
			using (_itemLock.Lock())
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

		protected override void DoUnload()
		{
			base.Updated -= ItemState_Updated;
		}
	}
}
