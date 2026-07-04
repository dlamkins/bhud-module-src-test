using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;

namespace WhereIsMyPSNA
{
	internal class PsnaDataService
	{
		public class SlotData
		{
			public bool IsNotDetermined;

			public int ItemId;

			public Texture2D IconTexture;

			public Texture2D CraftedIconTexture;
		}

		public class FetchResult
		{
			public SlotData[] Slots;

			public Texture2D KarmaTexture;
		}

		private class Gw2Currency
		{
			[JsonProperty("icon")]
			public string Icon { get; set; }
		}

		private static readonly Logger Logger = Logger.GetLogger<PsnaDataService>();

		private static readonly HttpClient Http = new HttpClient();

		private const string SheetCsvUrl = "https://docs.google.com/spreadsheets/d/14Jf0-RAcva1w-vx71vK1YvRIcGi4sQfWcrZHyhwAzwQ/export?format=csv&gid=0";

		private const string Gw2KarmaCurrency = "https://api.guildwars2.com/v2/currencies/2";

		private const string CoinGoldUrl = "https://render.guildwars2.com/file/090A980A96D39FD36FBB004903644C6DBEFB1FFB/156904.png";

		private const string CoinSilverUrl = "https://render.guildwars2.com/file/E5A2197D78ECE4AE0349C8B3710D033D22DB0DA6/156907.png";

		private const string CoinCopperUrl = "https://render.guildwars2.com/file/6CF8F96A3299CFC75D5CC90617C3C70331A1EF0E/156902.png";

		private static readonly int[] ScheduleToSheetCol = new int[6] { 0, 1, 3, 2, 4, 5 };

		private readonly Gw2ApiManager _apiManager;

		private readonly ContentsManager _contentsManager;

		private readonly CommunitySubmissionService _submissionService;

		public FetchResult LastResult { get; private set; }

		public bool IsFetching { get; private set; }

		public HashSet<int> KnownCraftingRecipeIds { get; private set; }

		public bool CoinTexturesLoaded { get; private set; }

		public Texture2D CoinGoldTexture { get; private set; }

		public Texture2D CoinSilverTexture { get; private set; }

		public Texture2D CoinCopperTexture { get; private set; }

		public bool LastFetchFailed { get; private set; }

		public event Action DataUpdated;

		public event Action<string> StatusChanged;

		public PsnaDataService(Gw2ApiManager apiManager, ContentsManager contentsManager, CommunitySubmissionService submissionService)
		{
			_apiManager = apiManager;
			_contentsManager = contentsManager;
			_submissionService = submissionService;
		}

		public void Fetch()
		{
			if (!IsFetching)
			{
				IsFetching = true;
				RunFullFetchAsync();
			}
		}

		private async Task RunFullFetchAsync()
		{
			LastFetchFailed = false;
			try
			{
				Task accountRecipesTask = FetchAccountRecipesAsync();
				if (!CoinTexturesLoaded)
				{
					Texture2D[] coins = await Task.WhenAll<Texture2D>(FetchTextureAsync("https://render.guildwars2.com/file/090A980A96D39FD36FBB004903644C6DBEFB1FFB/156904.png"), FetchTextureAsync("https://render.guildwars2.com/file/E5A2197D78ECE4AE0349C8B3710D033D22DB0DA6/156907.png"), FetchTextureAsync("https://render.guildwars2.com/file/6CF8F96A3299CFC75D5CC90617C3C70331A1EF0E/156902.png"));
					CoinGoldTexture = coins[0];
					CoinSilverTexture = coins[1];
					CoinCopperTexture = coins[2];
					CoinTexturesLoaded = true;
				}
				this.StatusChanged?.Invoke(Strings.Get("Status_FetchingSchedule"));
				Task<Texture2D> karmaTask = FetchKarmaIconAsync();
				Task<(int[] itemIds, bool[] todayFlags)> sheetTask = FetchSheetAsync();
				Task<Dictionary<string, int>> fallbackTask = _submissionService.FetchCurrentSubmissionsAsync();
				await Task.WhenAll(karmaTask, sheetTask, fallbackTask);
				Texture2D karmaTexture = karmaTask.Result;
				(int[] itemIds, bool[] todayFlags) result = sheetTask.Result;
				int[] itemIds = result.itemIds;
				bool[] todayFlags = result.todayFlags;
				Dictionary<string, int> fallbackSubmissions = fallbackTask.Result;
				this.StatusChanged?.Invoke(Strings.Get("Status_LoadingRecipeData"));
				SlotData[] slotResults = new SlotData[6];
				Task<Texture2D>[] iconTasks = new Task<Texture2D>[6];
				Task<Texture2D>[] craftedIconTasks = new Task<Texture2D>[6];
				PsnaSchedule.AgentLocation[] locations = PsnaSchedule.GetTodaysLocations();
				for (int j = 0; j < 6; j++)
				{
					int sheetCol = ScheduleToSheetCol[j];
					slotResults[j] = new SlotData();
					int resolvedItemId = 0;
					string shortNpc;
					int fallbackId;
					if (todayFlags[sheetCol] && itemIds[sheetCol] > 0)
					{
						resolvedItemId = itemIds[sheetCol];
					}
					else if (_submissionService.TryGetShortName(locations[j].Npc, out shortNpc) && fallbackSubmissions.TryGetValue(shortNpc, out fallbackId))
					{
						resolvedItemId = fallbackId;
					}
					if (resolvedItemId <= 0 || !RecipeDefs.ByRecipeSheetId.TryGetValue(resolvedItemId, out var def))
					{
						slotResults[j].IsNotDetermined = true;
						iconTasks[j] = Task.FromResult<Texture2D>(null);
						craftedIconTasks[j] = Task.FromResult<Texture2D>(null);
					}
					else
					{
						slotResults[j].ItemId = resolvedItemId;
						iconTasks[j] = LoadBundledTextureAsync(def.SheetIconFile);
						craftedIconTasks[j] = LoadBundledTextureAsync(def.IconFile);
					}
				}
				this.StatusChanged?.Invoke(Strings.Get("Status_LoadingIcons"));
				await Task.WhenAll(iconTasks.Concat(craftedIconTasks).ToArray());
				for (int i = 0; i < 6; i++)
				{
					if (!slotResults[i].IsNotDetermined)
					{
						slotResults[i].IconTexture = iconTasks[i].Result;
						slotResults[i].CraftedIconTexture = craftedIconTasks[i].Result;
					}
				}
				this.StatusChanged?.Invoke(Strings.Get("Status_CheckingKnownRecipes"));
				await accountRecipesTask;
				LastResult = new FetchResult
				{
					Slots = slotResults,
					KarmaTexture = karmaTexture
				};
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to fetch PSNA data.");
				LastFetchFailed = true;
			}
			finally
			{
				IsFetching = false;
				this.DataUpdated?.Invoke();
			}
		}

		private async Task FetchAccountRecipesAsync()
		{
			if (!_apiManager.HasPermissions((IEnumerable<TokenPermission>)(object)new TokenPermission[1] { (TokenPermission)1 }))
			{
				Logger.Warn("Skipping account recipes fetch: missing Account permission.");
				return;
			}
			try
			{
				KnownCraftingRecipeIds = new HashSet<int>((IEnumerable<int>)(await ((IBlobClient<IApiV2ObjectList<int>>)(object)_apiManager.get_Gw2ApiClient().get_V2().get_Account()
					.get_Recipes()).GetAsync(default(CancellationToken))));
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to fetch account recipes.");
			}
		}

		private async Task<(int[] itemIds, bool[] todayFlags)> FetchSheetAsync()
		{
			string[] lines = (await Http.GetStringAsync("https://docs.google.com/spreadsheets/d/14Jf0-RAcva1w-vx71vK1YvRIcGi4sQfWcrZHyhwAzwQ/export?format=csv&gid=0")).Split('\n');
			string[] idLine = lines[1].Split(',');
			string[] dateLine = lines[2].Split(',');
			string currentDate = ((dateLine.Length > 6) ? dateLine[6].Trim().Trim('"') : "");
			int[] itemIds = new int[6];
			bool[] todayFlags = new bool[6];
			for (int col = 0; col < 6; col++)
			{
				if (col < idLine.Length && int.TryParse(idLine[col].Trim().Trim('"'), out var id))
				{
					itemIds[col] = id;
				}
				string colDate = ((col < dateLine.Length) ? dateLine[col].Trim().Trim('"') : "");
				todayFlags[col] = !string.IsNullOrEmpty(currentDate) && colDate == currentDate;
			}
			return (itemIds, todayFlags);
		}

		private Task<Texture2D> LoadBundledTextureAsync(string relativePath)
		{
			if (string.IsNullOrEmpty(relativePath))
			{
				return Task.FromResult<Texture2D>(null);
			}
			TaskCompletionSource<Texture2D> tcs = new TaskCompletionSource<Texture2D>();
			GameService.Graphics.QueueMainThreadRender((Action<GraphicsDevice>)delegate
			{
				try
				{
					tcs.SetResult(_contentsManager.GetTexture(relativePath));
				}
				catch (Exception exception)
				{
					tcs.SetException(exception);
				}
			});
			return tcs.Task;
		}

		private async Task<Texture2D> FetchKarmaIconAsync()
		{
			_ = 1;
			try
			{
				Gw2Currency currency = JsonConvert.DeserializeObject<Gw2Currency>(await Http.GetStringAsync("https://api.guildwars2.com/v2/currencies/2"));
				if (!string.IsNullOrEmpty(currency?.Icon))
				{
					return await FetchTextureAsync(currency.Icon);
				}
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to fetch karma icon.");
			}
			return null;
		}

		private async Task<Texture2D> FetchTextureAsync(string url)
		{
			_ = 1;
			try
			{
				byte[] bytes = await Http.GetByteArrayAsync(url);
				TaskCompletionSource<Texture2D> tcs = new TaskCompletionSource<Texture2D>();
				GameService.Graphics.QueueMainThreadRender((Action<GraphicsDevice>)delegate(GraphicsDevice gd)
				{
					try
					{
						using MemoryStream memoryStream = new MemoryStream(bytes);
						tcs.SetResult(Texture2D.FromStream(gd, (Stream)memoryStream));
					}
					catch (Exception exception)
					{
						tcs.SetException(exception);
					}
				});
				return await tcs.Task;
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to fetch texture: " + url);
				return null;
			}
		}
	}
}
