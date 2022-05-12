using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;

namespace Nekres.Stream_Out.Core.Services
{
	internal class WvwService : ExportService
	{
		private const string WVW_KILLS_WEEK = "wvw_kills_week.txt";

		private const string WVW_KILLS_DAY = "wvw_kills_day.txt";

		private const string WVW_KILLS_TOTAL = "wvw_kills_total.txt";

		private const string WVW_RANK = "wvw_rank.txt";

		private const string SWORDS = "⚔";

		private Gw2ApiManager Gw2ApiManager => StreamOutModule.Instance?.Gw2ApiManager;

		private DirectoriesManager DirectoriesManager => StreamOutModule.Instance?.DirectoriesManager;

		private StreamOutModule.UnicodeSigning UnicodeSigning => StreamOutModule.Instance?.AddUnicodeSymbols.get_Value() ?? StreamOutModule.UnicodeSigning.Suffixed;

		private SettingEntry<string> AccountName => StreamOutModule.Instance?.AccountName;

		private SettingEntry<Guid> AccountGuid => StreamOutModule.Instance?.AccountGuid;

		private SettingEntry<DateTime> ResetTimeWvW => StreamOutModule.Instance.ResetTimeWvW;

		private SettingEntry<int> SessionKillsWvW => StreamOutModule.Instance?.SessionKillsWvW;

		private SettingEntry<int> SessionDeathsWvW => StreamOutModule.Instance?.SessionDeathsWvW;

		private SettingEntry<int> SessionKillsWvwDaily => StreamOutModule.Instance?.SessionKillsWvwDaily;

		private SettingEntry<int> TotalKillsAtResetWvW => StreamOutModule.Instance?.TotalKillsAtResetWvW;

		private SettingEntry<int> TotalDeathsAtResetWvW => StreamOutModule.Instance?.TotalDeathsAtResetWvW;

		public override async Task Initialize()
		{
			await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/wvw_kills_week.txt", "0⚔", overwrite: false);
			await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/wvw_kills_total.txt", "0⚔", overwrite: false);
			await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/wvw_kills_day.txt", "0⚔", overwrite: false);
			await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/wvw_rank.txt", "1 : Invader", overwrite: false);
		}

		private async Task UpdateRankForWvw()
		{
			if (!Gw2ApiManager.HasPermissions((IEnumerable<TokenPermission>)(object)new TokenPermission[2]
			{
				(TokenPermission)1,
				(TokenPermission)6
			}))
			{
				return;
			}
			await ((IBlobClient<Account>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Account()).GetAsync(default(CancellationToken)).ContinueWith((Func<Task<Account>, Task>)async delegate(Task<Account> response)
			{
				if (!response.IsFaulted)
				{
					int? wvwRank = response.Result.get_WvwRank();
					if (wvwRank.HasValue && !(wvwRank <= 0))
					{
						await ((IAllExpandableClient<WvwRank>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Wvw()
							.get_Ranks()).AllAsync(default(CancellationToken)).ContinueWith((Func<Task<IApiV2ObjectList<WvwRank>>, Task>)async delegate(Task<IApiV2ObjectList<WvwRank>> t)
						{
							if (!t.IsFaulted)
							{
								WvwRank wvwRankObj = ((IEnumerable<WvwRank>)t.Result).MaxBy((WvwRank y) => wvwRank >= y.get_MinRank());
								await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/wvw_rank.txt", $"{wvwRank:N0} : {wvwRankObj.get_Title()}");
							}
						});
					}
				}
			});
		}

		private async Task<int> RequestTotalKillsForWvW()
		{
			if (!Gw2ApiManager.HasPermissions((IEnumerable<TokenPermission>)(object)new TokenPermission[2]
			{
				(TokenPermission)1,
				(TokenPermission)6
			}))
			{
				return -1;
			}
			return await ((IBlobClient<IApiV2ObjectList<AccountAchievement>>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Account()
				.get_Achievements()).GetAsync(default(CancellationToken)).ContinueWith((Task<IApiV2ObjectList<AccountAchievement>> response) => response.IsFaulted ? (-1) : ((IEnumerable<AccountAchievement>)response.Result).Single((AccountAchievement x) => x.get_Id() == 283).get_Current());
		}

		private async Task ResetWorldVersusWorld(int worldId, bool force = false)
		{
			if (force || !(DateTime.UtcNow < ResetTimeWvW.get_Value()))
			{
				SettingEntry<DateTime> resetTimeWvW = ResetTimeWvW;
				resetTimeWvW.set_Value(await GetWvWResetTime(worldId));
				SessionKillsWvW.set_Value(0);
				SessionDeathsWvW.set_Value(0);
				SettingEntry<int> totalKillsAtResetWvW = TotalKillsAtResetWvW;
				totalKillsAtResetWvW.set_Value(await RequestTotalKillsForWvW());
				totalKillsAtResetWvW = TotalDeathsAtResetWvW;
				totalKillsAtResetWvW.set_Value(await CharacterService.RequestTotalDeaths());
			}
		}

		private async Task<DateTime> GetWvWResetTime(int worldId)
		{
			return await ((IBlobClient<WvwMatch>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Wvw()
				.get_Matches()
				.World(worldId)).GetAsync(default(CancellationToken)).ContinueWith((Task<WvwMatch> r) => (!r.IsFaulted) ? r.Result.get_EndTime().UtcDateTime : DateTime.UtcNow);
		}

		protected override async Task ResetDaily()
		{
			SessionKillsWvwDaily.set_Value(0);
		}

		protected override async Task Update()
		{
			if (!Gw2ApiManager.HasPermission((TokenPermission)1))
			{
				return;
			}
			await UpdateRankForWvw();
			await ((IBlobClient<Account>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Account()).GetAsync(default(CancellationToken)).ContinueWith((Func<Task<Account>, Task>)async delegate(Task<Account> response)
			{
				if (!response.IsFaulted)
				{
					bool isNewAcc = !response.Result.get_Id().Equals(AccountGuid.get_Value());
					AccountName.set_Value(response.Result.get_Name());
					AccountGuid.set_Value(response.Result.get_Id());
					await ResetWorldVersusWorld(response.Result.get_World(), isNewAcc).ContinueWith((Func<Task, Task>)async delegate
					{
						string prefixKills = ((UnicodeSigning == StreamOutModule.UnicodeSigning.Prefixed) ? "⚔" : string.Empty);
						string suffixKills = ((UnicodeSigning == StreamOutModule.UnicodeSigning.Suffixed) ? "⚔" : string.Empty);
						int totalKillsWvW = await RequestTotalKillsForWvW();
						if (totalKillsWvW >= 0)
						{
							int currentKills = totalKillsWvW - TotalKillsAtResetWvW.get_Value();
							SessionKillsWvW.set_Value(currentKills);
							SessionKillsWvwDaily.set_Value(currentKills);
							await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/wvw_kills_week.txt", $"{prefixKills}{SessionKillsWvW.get_Value()}{suffixKills}");
							await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/wvw_kills_total.txt", $"{prefixKills}{totalKillsWvW}{suffixKills}");
							await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/wvw_kills_day.txt", $"{prefixKills}{SessionKillsWvwDaily.get_Value()}{suffixKills}");
						}
					});
				}
			});
		}

		public override async Task Clear()
		{
			string dir = DirectoriesManager.GetFullDirectoryPath("stream_out");
			await FileUtil.DeleteAsync(Path.Combine(dir, "wvw_kills_week.txt"));
			await FileUtil.DeleteAsync(Path.Combine(dir, "wvw_kills_day.txt"));
			await FileUtil.DeleteAsync(Path.Combine(dir, "wvw_kills_total.txt"));
			await FileUtil.DeleteAsync(Path.Combine(dir, "wvw_rank.txt"));
		}

		public override void Dispose()
		{
		}
	}
}
