using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using RaidClears.Features.Shared.Models;

namespace RaidClears.Features.Strikes.Services
{
	public static class WeeklyStrikeClearsService
	{
		private static readonly List<TokenPermission> NecessaryPermissions = new List<TokenPermission>
		{
			(TokenPermission)1,
			(TokenPermission)6
		};

		public static async Task RefreshFromApiAsync()
		{
			StrikeData strikeData = Service.StrikeData;
			if (strikeData.WeeklyAchievementId == 0 || strikeData.WeeklyAchievementBitStrikeIds == null || strikeData.WeeklyAchievementBitStrikeIds.Count == 0)
			{
				return;
			}
			Gw2ApiManager gw2ApiManager = Service.Gw2ApiManager;
			Logger logger = Logger.GetLogger<Module>();
			if (gw2ApiManager == null || !gw2ApiManager.HasPermissions((IEnumerable<TokenPermission>)NecessaryPermissions))
			{
				return;
			}
			string account = Service.CurrentAccountName;
			if (string.IsNullOrEmpty(account))
			{
				return;
			}
			try
			{
				AccountAchievement achievement = (((IEnumerable<AccountAchievement>)(await ((IBlobClient<IApiV2ObjectList<AccountAchievement>>)(object)gw2ApiManager.get_Gw2ApiClient().get_V2().get_Account()
					.get_Achievements()).GetAsync(default(CancellationToken)).ConfigureAwait(continueOnCapturedContext: false)))?.ToList() ?? new List<AccountAchievement>()).Find((AccountAchievement x) => x.get_Id() == strikeData.WeeklyAchievementId);
				if (achievement == null)
				{
					return;
				}
				List<string> mapping = strikeData.WeeklyAchievementBitStrikeIds;
				StrikePersistance persistence = Service.StrikePersistance;
				if (achievement.get_Done())
				{
					foreach (string strikeId2 in mapping)
					{
						BossEncounter mission2 = strikeData.GetBossEncounterById(strikeId2);
						persistence.SaveClear(account, mission2);
					}
					return;
				}
				HashSet<int> completedBits = new HashSet<int>(achievement.get_Bits() ?? Array.Empty<int>());
				for (int i = 0; i < mapping.Count; i++)
				{
					string strikeId = mapping[i];
					BossEncounter mission = strikeData.GetBossEncounterById(strikeId);
					if (completedBits.Contains(i))
					{
						persistence.SaveClear(account, mission);
					}
					else
					{
						persistence.RemoveClear(account, mission);
					}
				}
			}
			catch (Exception e)
			{
				logger.Warn(e, "Could not refresh weekly strike clears from API");
			}
		}
	}
}
