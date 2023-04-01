using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD.Extended;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Gw2Sharp.WebApi;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;

namespace Nekres.Stream_Out.Core.Services
{
	internal class PvpService : ExportService
	{
		private const string PVP_KILLS_TOTAL = "pvp_kills_total.txt";

		private const string PVP_KILLS_DAY = "pvp_kills_day.txt";

		private const string PVP_RANK = "pvp_rank.txt";

		private const string PVP_RANK_ICON = "pvp_rank_icon.png";

		private const string PVP_TIER_ICON = "pvp_tier_icon.png";

		private const string PVP_WINRATE = "pvp_winrate.txt";

		private const string SWORDS = "⚔";

		private SettingEntry<int> _killsDaily;

		private SettingEntry<int> _killsAtResetDaily;

		private Gw2ApiManager Gw2ApiManager => StreamOutModule.Instance?.Gw2ApiManager;

		private DirectoriesManager DirectoriesManager => StreamOutModule.Instance?.DirectoriesManager;

		private ContentsManager ContentsManager => StreamOutModule.Instance?.ContentsManager;

		private StreamOutModule.UnicodeSigning UnicodeSigning => StreamOutModule.Instance?.AddUnicodeSymbols.get_Value() ?? StreamOutModule.UnicodeSigning.Suffixed;

		public PvpService(SettingCollection settings)
			: base(settings)
		{
			_killsDaily = settings.DefineSetting<int>(GetType().Name + "_kills_daily", 0, (Func<string>)null, (Func<string>)null);
			_killsAtResetDaily = settings.DefineSetting<int>(GetType().Name + "_kills_daily_reset", 0, (Func<string>)null, (Func<string>)null);
		}

		public override async Task Initialize()
		{
			await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/pvp_rank.txt", "Bronze I", overwrite: false);
			await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/pvp_winrate.txt", "50%", overwrite: false);
			await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/pvp_kills_day.txt", "0⚔", overwrite: false);
			await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/pvp_kills_total.txt", "0⚔", overwrite: false);
			await Gw2Util.GeneratePvpTierImage(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/pvp_tier_icon.png", 1, 3, overwrite: false);
			string moduleDir = DirectoriesManager.GetFullDirectoryPath("stream_out");
			ContentsManager.ExtractIcons("1614804.png", Path.Combine(moduleDir, "pvp_rank_icon.png"));
		}

		private async Task<int> RequestTotalKillsForPvP()
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
				.get_Achievements()).GetAsync(default(CancellationToken))).Unwrap();
			if (achievements == null)
			{
				return -1;
			}
			AccountAchievement obj = ((IEnumerable<AccountAchievement>)achievements).FirstOrDefault((AccountAchievement x) => x.get_Id() == 239);
			return (obj != null) ? obj.get_Current() : (-1);
		}

		private async Task UpdateStandingsForPvP()
		{
			if (!Gw2ApiManager.HasPermissions((IEnumerable<TokenPermission>)(object)new TokenPermission[2]
			{
				(TokenPermission)1,
				(TokenPermission)7
			}))
			{
				return;
			}
			IApiV2ObjectList<PvpSeason> seasons = await TaskUtil.RetryAsync(() => ((IAllExpandableClient<PvpSeason>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Pvp()
				.get_Seasons()).AllAsync(default(CancellationToken))).Unwrap();
			if (seasons == null)
			{
				return;
			}
			PvpSeason season = ((IEnumerable<PvpSeason>)seasons).OrderByDescending((PvpSeason x) => x.get_End()).First();
			await ((IBlobClient<ApiV2BaseObjectList<PvpStanding>>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Pvp()
				.get_Standings()).GetAsync(default(CancellationToken)).ContinueWith((Func<Task<ApiV2BaseObjectList<PvpStanding>>, Task>)async delegate(Task<ApiV2BaseObjectList<PvpStanding>> t)
			{
				if (!t.IsFaulted)
				{
					PvpStanding standing = ((IEnumerable<PvpStanding>)t.Result).FirstOrDefault((PvpStanding x) => x.get_SeasonId().Equals(season.get_Id()));
					if (standing != null && standing.get_Current().get_Rating().HasValue)
					{
						PvpSeasonRank rank = season.get_Ranks().First();
						int tier = 1;
						bool found = false;
						int ranksTotal = season.get_Ranks().Count;
						List<PvpSeasonRankTier> data = season.get_Ranks().SelectMany((PvpSeasonRank x) => x.get_Tiers()).ToList();
						int maxRating = data.MaxBy((PvpSeasonRankTier y) => y.get_Rating()).get_Rating();
						int rating = data.MinBy((PvpSeasonRankTier y) => y.get_Rating()).get_Rating();
						if (standing.get_Current().get_Rating() > maxRating)
						{
							rank = season.get_Ranks().Last();
							tier = rank.get_Tiers().Count;
							found = true;
						}
						if (standing.get_Current().get_Rating() < rating)
						{
							rank = season.get_Ranks().First();
							tier = 1;
							found = true;
						}
						for (int i = 0; i < ranksTotal; i++)
						{
							if (found)
							{
								break;
							}
							PvpSeasonRank currentRank = season.get_Ranks()[i];
							int tiersTotal = currentRank.get_Tiers().Count;
							for (int j = 0; j < tiersTotal; j++)
							{
								int rating2 = currentRank.get_Tiers()[j].get_Rating();
								if (!(standing.get_Current().get_Rating() > rating2))
								{
									tier = j + 1;
									rank = currentRank;
									found = true;
									break;
								}
							}
						}
						await Task.Run(() => Gw2Util.GeneratePvpTierImage(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/pvp_tier_icon.png", tier, rank.get_Tiers().Count));
						await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/pvp_rank.txt", rank.get_Name() + " " + tier.ToRomanNumeral());
						await TextureUtil.SaveToImage(RenderUrl.op_Implicit(rank.get_Overlay()), DirectoriesManager.GetFullDirectoryPath("stream_out") + "/pvp_rank_icon.png");
					}
				}
			});
		}

		private async Task UpdateStatsForPvp()
		{
			if (!Gw2ApiManager.HasPermissions((IEnumerable<TokenPermission>)(object)new TokenPermission[2]
			{
				(TokenPermission)1,
				(TokenPermission)7
			}))
			{
				return;
			}
			PvpStats stats = await TaskUtil.RetryAsync(() => ((IBlobClient<PvpStats>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Pvp()
				.get_Stats()).GetAsync(default(CancellationToken))).Unwrap();
			if (stats != null)
			{
				KeyValuePair<string, PvpStatsAggregate>[] source = (from x in stats.get_Ladders()
					where !x.Key.Contains("unranked") && x.Key.Contains("ranked")
					select x).ToArray();
				int wins = source.Sum((KeyValuePair<string, PvpStatsAggregate> x) => x.Value.get_Wins());
				int losses = source.Sum((KeyValuePair<string, PvpStatsAggregate> x) => x.Value.get_Losses());
				int byes = source.Sum((KeyValuePair<string, PvpStatsAggregate> x) => x.Value.get_Byes());
				int desertions = source.Sum((KeyValuePair<string, PvpStatsAggregate> x) => x.Value.get_Desertions());
				double totalGames = wins + losses + desertions + byes;
				if (!(totalGames <= 0.0))
				{
					double winRatio = (double)(wins + byes) / totalGames * 100.0;
					await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/pvp_winrate.txt", Math.Round(winRatio).ToString(CultureInfo.InvariantCulture) + "%");
				}
			}
		}

		protected override async Task<bool> ResetDaily()
		{
			int totalKills = await RequestTotalKillsForPvP();
			if (totalKills < 0)
			{
				return false;
			}
			_killsDaily.set_Value(0);
			_killsAtResetDaily.set_Value(totalKills);
			return true;
		}

		protected override async Task Update()
		{
			await UpdateStandingsForPvP();
			await UpdateStatsForPvp();
			string prefixKills = ((UnicodeSigning == StreamOutModule.UnicodeSigning.Prefixed) ? "⚔" : string.Empty);
			string suffixKills = ((UnicodeSigning == StreamOutModule.UnicodeSigning.Suffixed) ? "⚔" : string.Empty);
			int totalKills = await RequestTotalKillsForPvP();
			if (totalKills >= 0)
			{
				_killsDaily.set_Value(totalKills - _killsAtResetDaily.get_Value());
				await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/pvp_kills_day.txt", $"{prefixKills}{_killsDaily.get_Value()}{suffixKills}");
				await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/pvp_kills_total.txt", $"{prefixKills}{totalKills}{suffixKills}");
			}
		}

		public override async Task Clear()
		{
			string dir = DirectoriesManager.GetFullDirectoryPath("stream_out");
			await FileUtil.DeleteAsync(Path.Combine(dir, "pvp_kills_total.txt"));
			await FileUtil.DeleteAsync(Path.Combine(dir, "pvp_kills_day.txt"));
			await FileUtil.DeleteAsync(Path.Combine(dir, "pvp_rank.txt"));
			await FileUtil.DeleteAsync(Path.Combine(dir, "pvp_rank_icon.png"));
			await FileUtil.DeleteAsync(Path.Combine(dir, "pvp_tier_icon.png"));
			await FileUtil.DeleteAsync(Path.Combine(dir, "pvp_winrate.txt"));
		}

		public override void Dispose()
		{
		}
	}
}
