using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using Newtonsoft.Json;
using RaidClears.Features.Shared.Models;

namespace RaidClears.Features.Shared.Services
{
	public class DailyBountyProgressService
	{
		private sealed class AchievementCategoryDto
		{
			[JsonProperty("id")]
			public int Id { get; set; }

			[JsonProperty("name")]
			public string Name { get; set; } = "";


			[JsonProperty("achievements")]
			public List<int> Achievements { get; set; } = new List<int>();

		}

		private static readonly List<TokenPermission> NecessaryPermissions = new List<TokenPermission>
		{
			(TokenPermission)1,
			(TokenPermission)6
		};

		private readonly object _lock = new object();

		private HashSet<int> _completedBountyAchievementIds = new HashSet<int>();

		public IReadOnlyCollection<int> CompletedDailyBountyAchievementIds
		{
			get
			{
				lock (_lock)
				{
					return new List<int>(_completedBountyAchievementIds);
				}
			}
		}

		public bool IsBountyCompleted(int achievementId)
		{
			lock (_lock)
			{
				return _completedBountyAchievementIds.Contains(achievementId);
			}
		}

		public async Task RefreshFromApiAsync()
		{
			DailyBountyData bountyData = Service.DailyBountyData;
			if (bountyData == null || !bountyData.Enabled || string.IsNullOrWhiteSpace(bountyData.DailyBountyCategoryUrl))
			{
				lock (_lock)
				{
					_completedBountyAchievementIds.Clear();
				}
				return;
			}
			List<int> bountyAchievementIds = await FetchCategoryAchievementIdsAsync(bountyData.DailyBountyCategoryUrl).ConfigureAwait(continueOnCapturedContext: false);
			if (bountyAchievementIds == null || bountyAchievementIds.Count == 0)
			{
				lock (_lock)
				{
					_completedBountyAchievementIds.Clear();
				}
				return;
			}
			Gw2ApiManager gw2ApiManager = Service.Gw2ApiManager;
			Logger logger = Logger.GetLogger<Module>();
			if (gw2ApiManager == null || !gw2ApiManager.HasPermissions((IEnumerable<TokenPermission>)NecessaryPermissions))
			{
				lock (_lock)
				{
					_completedBountyAchievementIds.Clear();
				}
				return;
			}
			try
			{
				List<AccountAchievement> obj = ((IEnumerable<AccountAchievement>)(await ((IBlobClient<IApiV2ObjectList<AccountAchievement>>)(object)gw2ApiManager.get_Gw2ApiClient().get_V2().get_Account()
					.get_Achievements()).GetAsync(default(CancellationToken)).ConfigureAwait(continueOnCapturedContext: false)))?.ToList() ?? new List<AccountAchievement>();
				HashSet<int> bountyIdsSet = new HashSet<int>(bountyAchievementIds);
				HashSet<int> completed = new HashSet<int>();
				foreach (AccountAchievement ach in obj)
				{
					if (bountyIdsSet.Contains(ach.get_Id()) && ach.get_Done())
					{
						completed.Add(ach.get_Id());
					}
				}
				lock (_lock)
				{
					_completedBountyAchievementIds = completed;
				}
			}
			catch (Exception ex)
			{
				logger.Warn(ex, "Could not fetch daily bounty progress from API");
				lock (_lock)
				{
					_completedBountyAchievementIds.Clear();
				}
			}
		}

		private static async Task<List<int>?> FetchCategoryAchievementIdsAsync(string categoryUrl)
		{
			if (string.IsNullOrWhiteSpace(categoryUrl))
			{
				return null;
			}
			try
			{
				using WebClient webClient = new WebClient();
				webClient.Encoding = Encoding.UTF8;
				return JsonConvert.DeserializeObject<AchievementCategoryDto>(await webClient.DownloadStringTaskAsync(new Uri(categoryUrl)).ConfigureAwait(continueOnCapturedContext: false))?.Achievements ?? new List<int>();
			}
			catch (Exception ex)
			{
				Logger.GetLogger<Module>().Warn(ex, "Could not fetch daily bounty category from {Url}", new object[1] { categoryUrl });
				return null;
			}
		}
	}
}
