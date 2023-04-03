using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD.Extended;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;

namespace Nekres.Stream_Out.Core.Services
{
	internal class WvwService : ExportService
	{
		private SettingEntry<DateTime> _nextResetTimeWvW;

		private SettingEntry<DateTime> _lastResetTimeWvW;

		private SettingEntry<int> _killsAtResetDaily;

		private SettingEntry<int> _killsAtResetMatch;

		private const string WVW_KILLS_WEEK = "wvw_kills_week.txt";

		private const string WVW_KILLS_DAY = "wvw_kills_day.txt";

		private const string WVW_KILLS_TOTAL = "wvw_kills_total.txt";

		private const string WVW_RANK = "wvw_rank.txt";

		private const string SWORDS = "⚔";

		private Gw2ApiManager Gw2ApiManager => StreamOutModule.Instance?.Gw2ApiManager;

		private DirectoriesManager DirectoriesManager => StreamOutModule.Instance?.DirectoriesManager;

		private StreamOutModule.UnicodeSigning UnicodeSigning => StreamOutModule.Instance?.AddUnicodeSymbols.get_Value() ?? StreamOutModule.UnicodeSigning.Suffixed;

		public WvwService(SettingCollection settings)
			: base(settings)
		{
			_nextResetTimeWvW = settings.DefineSetting<DateTime>(GetType().Name + "_next_reset", DateTime.UtcNow.AddSeconds(1.0), (Func<string>)null, (Func<string>)null);
			_lastResetTimeWvW = settings.DefineSetting<DateTime>(GetType().Name + "_last_reset", DateTime.UtcNow, (Func<string>)null, (Func<string>)null);
			_killsAtResetDaily = settings.DefineSetting<int>(GetType().Name + "_kills_daily_reset", 0, (Func<string>)null, (Func<string>)null);
			_killsAtResetMatch = settings.DefineSetting<int>(GetType().Name + "_kills_match_reset", 0, (Func<string>)null, (Func<string>)null);
		}

		public override async Task Initialize()
		{
			await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/wvw_kills_week.txt", "0⚔", overwrite: false);
			await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/wvw_kills_total.txt", "0⚔", overwrite: false);
			await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/wvw_kills_day.txt", "0⚔", overwrite: false);
			await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/wvw_rank.txt", "1 : Invader", overwrite: false);
		}

		private async Task UpdateRankForWvw(Account account)
		{
			if (!Gw2ApiManager.HasPermissions((IEnumerable<TokenPermission>)(object)new TokenPermission[2]
			{
				(TokenPermission)1,
				(TokenPermission)6
			}))
			{
				return;
			}
			int? wvwRankNum = account.get_WvwRank();
			if (!wvwRankNum.HasValue || wvwRankNum <= 0)
			{
				return;
			}
			IApiV2ObjectList<WvwRank> wvwRanks = await TaskUtil.RetryAsync(() => ((IAllExpandableClient<WvwRank>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Wvw()
				.get_Ranks()).AllAsync(default(CancellationToken)));
			if (wvwRanks != null)
			{
				WvwRank wvwRankObj = ((IEnumerable<WvwRank>)wvwRanks).MaxBy((WvwRank y) => wvwRankNum >= y.get_MinRank());
				await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/wvw_rank.txt", $"{wvwRankNum:N0} : {wvwRankObj.get_Title()}");
			}
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
			IApiV2ObjectList<AccountAchievement> achievements = await TaskUtil.RetryAsync(() => ((IBlobClient<IApiV2ObjectList<AccountAchievement>>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Account()
				.get_Achievements()).GetAsync(default(CancellationToken)));
			if (achievements == null)
			{
				return -1;
			}
			AccountAchievement obj = ((IEnumerable<AccountAchievement>)achievements).FirstOrDefault((AccountAchievement x) => x.get_Id() == 283);
			return (obj != null) ? obj.get_Current() : (-1);
		}

		private async Task<bool> ResetWvWMatch(int worldId)
		{
			if (_lastResetTimeWvW.get_Value() < _nextResetTimeWvW.get_Value() && DateTime.UtcNow > _nextResetTimeWvW.get_Value())
			{
				WvwMatch wvwWorldMatch = await TaskUtil.RetryAsync(() => ((IBlobClient<WvwMatch>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Wvw()
					.get_Matches()
					.World(worldId)).GetAsync(default(CancellationToken)));
				if (wvwWorldMatch == null)
				{
					return false;
				}
				int totalKills = await RequestTotalKillsForWvW();
				if (totalKills < 0)
				{
					return false;
				}
				_killsAtResetDaily.set_Value(totalKills);
				_killsAtResetMatch.set_Value(totalKills);
				_lastResetTimeWvW.set_Value(DateTime.UtcNow);
				_nextResetTimeWvW.set_Value(wvwWorldMatch.get_EndTime().UtcDateTime.AddMinutes(5.0));
			}
			return true;
		}

		protected override async Task<bool> ResetDaily()
		{
			int totalKills = await RequestTotalKillsForWvW();
			if (totalKills < 0)
			{
				return false;
			}
			_killsAtResetDaily.set_Value(totalKills);
			return true;
		}

		protected override async Task Update()
		{
			Account account = StreamOutModule.Instance?.Account;
			if (account == null)
			{
				return;
			}
			await UpdateRankForWvw(account);
			if (await ResetWvWMatch(account.get_World()))
			{
				string prefixKills = ((UnicodeSigning == StreamOutModule.UnicodeSigning.Prefixed) ? "⚔" : string.Empty);
				string suffixKills = ((UnicodeSigning == StreamOutModule.UnicodeSigning.Suffixed) ? "⚔" : string.Empty);
				int totalKillsWvW = await RequestTotalKillsForWvW();
				if (totalKillsWvW >= 0)
				{
					await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/wvw_kills_total.txt", $"{prefixKills}{totalKillsWvW}{suffixKills}");
					int killsDaily = totalKillsWvW - _killsAtResetDaily.get_Value();
					await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/wvw_kills_day.txt", $"{prefixKills}{killsDaily}{suffixKills}");
					int killsMatch = totalKillsWvW - _killsAtResetMatch.get_Value();
					await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/wvw_kills_week.txt", $"{prefixKills}{killsMatch}{suffixKills}");
				}
			}
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
