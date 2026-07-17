using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Modules.Managers;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using Newtonsoft.Json;

namespace WhereIsMyPSNA
{
	internal class PsnaDataService
	{
		public class SlotData
		{
			public bool IsNotDetermined;

			public int ItemId;

			public AsyncTexture2D IconTexture;

			public AsyncTexture2D CraftedIconTexture;
		}

		public class FetchResult
		{
			public SlotData[] Slots;

			public AsyncTexture2D KarmaTexture;
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

		private const int CoinGoldAssetId = 156904;

		private const int CoinSilverAssetId = 156907;

		private const int CoinCopperAssetId = 156902;

		private static readonly int[] ScheduleToSheetCol = new int[6] { 0, 1, 3, 2, 4, 5 };

		private readonly Gw2ApiManager _apiManager;

		private readonly CommunitySubmissionService _submissionService;

		private static readonly Regex AssetIdRegex = new Regex("/(\\d+)\\.png$", RegexOptions.Compiled);

		public FetchResult LastResult { get; private set; }

		public bool IsFetching { get; private set; }

		public HashSet<int> KnownCraftingRecipeIds { get; private set; }

		public bool CoinTexturesLoaded { get; private set; }

		public AsyncTexture2D CoinGoldTexture { get; private set; }

		public AsyncTexture2D CoinSilverTexture { get; private set; }

		public AsyncTexture2D CoinCopperTexture { get; private set; }

		public bool LastFetchFailed { get; private set; }

		public event Action DataUpdated;

		public event Action<string> StatusChanged;

		public PsnaDataService(Gw2ApiManager apiManager, CommunitySubmissionService submissionService)
		{
			_apiManager = apiManager;
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
					CoinGoldTexture = GetTexture(156904);
					CoinSilverTexture = GetTexture(156907);
					CoinCopperTexture = GetTexture(156902);
					CoinTexturesLoaded = true;
				}
				this.StatusChanged?.Invoke(Strings.Get("Status_FetchingSchedule"));
				Task<AsyncTexture2D> karmaTask = FetchKarmaIconAsync();
				Task<(int[] itemIds, bool[] todayFlags)> sheetTask = FetchSheetAsync();
				Task<Dictionary<string, int>> fallbackTask = _submissionService.FetchCurrentSubmissionsAsync();
				await Task.WhenAll(karmaTask, sheetTask, fallbackTask);
				AsyncTexture2D karmaTexture = karmaTask.Result;
				(int[] itemIds, bool[] todayFlags) result = sheetTask.Result;
				int[] itemIds = result.itemIds;
				bool[] todayFlags = result.todayFlags;
				Dictionary<string, int> fallbackSubmissions = fallbackTask.Result;
				this.StatusChanged?.Invoke(Strings.Get("Status_LoadingRecipeData"));
				SlotData[] slotResults = new SlotData[6];
				PsnaSchedule.AgentLocation[] locations = PsnaSchedule.GetTodaysLocations();
				for (int i = 0; i < 6; i++)
				{
					int sheetCol = ScheduleToSheetCol[i];
					slotResults[i] = new SlotData();
					int resolvedItemId = 0;
					string shortNpc;
					int fallbackId;
					if (todayFlags[sheetCol] && itemIds[sheetCol] > 0)
					{
						resolvedItemId = itemIds[sheetCol];
					}
					else if (_submissionService.TryGetShortName(locations[i].Npc, out shortNpc) && fallbackSubmissions.TryGetValue(shortNpc, out fallbackId))
					{
						resolvedItemId = fallbackId;
					}
					if (resolvedItemId <= 0 || !RecipeDefs.ByRecipeSheetId.TryGetValue(resolvedItemId, out var def))
					{
						slotResults[i].IsNotDetermined = true;
						continue;
					}
					slotResults[i].ItemId = resolvedItemId;
					slotResults[i].IconTexture = GetTexture(def.SheetIconId);
					slotResults[i].CraftedIconTexture = GetTexture(def.IconId);
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

		private async Task<AsyncTexture2D> FetchKarmaIconAsync()
		{
			try
			{
				Gw2Currency currency = JsonConvert.DeserializeObject<Gw2Currency>(await Http.GetStringAsync("https://api.guildwars2.com/v2/currencies/2"));
				Match match = (string.IsNullOrEmpty(currency?.Icon) ? null : AssetIdRegex.Match(currency.Icon));
				if (match != null && match.Success && int.TryParse(match.Groups[1].Value, out var assetId))
				{
					return GetTexture(assetId);
				}
				Logger.Warn("Could not extract asset id from karma icon URL: '" + currency?.Icon + "'");
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to fetch karma icon.");
			}
			return null;
		}

		private static AsyncTexture2D GetTexture(int assetId)
		{
			AsyncTexture2D texture = default(AsyncTexture2D);
			if (!AsyncTexture2D.TryFromAssetId(assetId, ref texture))
			{
				return null;
			}
			return texture;
		}
	}
}
