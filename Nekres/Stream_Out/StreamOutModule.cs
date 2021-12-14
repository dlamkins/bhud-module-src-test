using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;

namespace Nekres.Stream_Out
{
	[Export(typeof(Module))]
	public class StreamOutModule : Module
	{
		private enum UnicodeSigning
		{
			None,
			Prefixed,
			Suffixed
		}

		internal static readonly Logger Logger = Logger.GetLogger<StreamOutModule>();

		internal static StreamOutModule ModuleInstance;

		private SettingEntry<bool> _onlyLastDigitSettingEntry;

		private SettingEntry<UnicodeSigning> _addUnicodeSymbols;

		private SettingEntry<DateTime?> _resetTimeWvW;

		private SettingEntry<DateTime?> _resetTimeDaily;

		private SettingEntry<int> _sessionKillsWvW;

		private SettingEntry<int> _sessionKillsWvwDaily;

		private SettingEntry<int> _sessionKillsPvP;

		private SettingEntry<int> _totalKillsAtResetWvW;

		private SettingEntry<int> _totalKillsAtResetPvP;

		private SettingEntry<int> _totalDeathsAtResetWvW;

		private SettingEntry<int> _totalDeathsAtResetDaily;

		private SettingEntry<int> _sessionDeathsWvW;

		private SettingEntry<int> _sessionDeathsDaily;

		private SettingEntry<Guid> _accountGuid;

		private SettingEntry<string> _accountName;

		private const string SERVER_ADDRESS = "server_address.txt";

		private const string CHARACTER_NAME = "character_name.txt";

		private const string MAP_TYPE = "map_type.txt";

		private const string MAP_NAME = "map_name.txt";

		private const string PROFESSION_ICON = "profession_icon.png";

		private const string WALLET_COINS = "wallet_coins.png";

		private const string WALLET_KARMA = "wallet_karma.png";

		private const string WVW_KILLS_WEEK = "wvw_kills_week.txt";

		private const string WVW_KILLS_DAY = "wvw_kills_day.txt";

		private const string WVW_KILLS_TOTAL = "wvw_kills_total.txt";

		private const string WVW_RANK = "wvw_rank.txt";

		private const string PVP_KILLS_TOTAL = "pvp_kills_total.txt";

		private const string PVP_KILLS_DAY = "pvp_kills_day.txt";

		private const string PVP_RANK = "pvp_rank.txt";

		private const string PVP_RANK_ICON = "pvp_rank_icon.png";

		private const string PVP_TIER_ICON = "pvp_tier_icon.png";

		private const string PVP_WIN_LOSS_RATIO = "pvp_win_loss_ratio.txt";

		private const string KILLPROOF_ME_UNSTABLE_FRACTAL_ESSENCE = "unstable_fractal_essence.txt";

		private const string KILLPROOF_ME_LEGENDARY_DIVINATION = "legendary_divination.txt";

		private const string KILLPROOF_ME_LEGENDARY_INSIGHT = "legendary_insight.txt";

		private const string DEATHS_WEEK = "deaths_week.txt";

		private const string DEATHS_DAY = "deaths_day.txt";

		private const string SKULL = "☠";

		private const string SWORDS = "⚔";

		private const string KILLPROOF_API_URL = "https://killproof.me/api/kp/";

		private bool _hasSubToken;

		private DateTime? _prevApiRequestTime;

		private string _prevServerAddress = "";

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		internal Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		[ImportingConstructor]
		public StreamOutModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
			ModuleInstance = this;
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			_onlyLastDigitSettingEntry = settings.DefineSetting<bool>("OnlyLastDigits", true, (Func<string>)(() => "Only Output Last Digits of Server Address"), (Func<string>)(() => "Only outputs the last digits of the server address you are currently connected to.\nThis is the address shown when entering \"/ip\" in chat."));
			_addUnicodeSymbols = settings.DefineSetting<UnicodeSigning>("UnicodeSymbols", UnicodeSigning.Suffixed, (Func<string>)(() => "Numeric Value Signing"), (Func<string>)(() => "The way numeric values should be signed with unicode symbols."));
			SettingCollection cache = settings.AddSubCollection("CachedValues", false);
			cache.set_RenderInUi(false);
			_accountGuid = cache.DefineSetting<Guid>("AccountGuid", Guid.Empty, (Func<string>)null, (Func<string>)null);
			_accountName = cache.DefineSetting<string>("AccountName", string.Empty, (Func<string>)null, (Func<string>)null);
			_resetTimeWvW = cache.DefineSetting<DateTime?>("ResetTimeWvW", (DateTime?)null, (Func<string>)null, (Func<string>)null);
			_resetTimeDaily = cache.DefineSetting<DateTime?>("ResetTimeDaily", (DateTime?)null, (Func<string>)null, (Func<string>)null);
			_sessionKillsWvW = cache.DefineSetting<int>("SessionKillsWvW", 0, (Func<string>)null, (Func<string>)null);
			_sessionKillsWvwDaily = cache.DefineSetting<int>("SessionsKillsWvWDaily", 0, (Func<string>)null, (Func<string>)null);
			_sessionKillsPvP = cache.DefineSetting<int>("SessionKillsPvP", 0, (Func<string>)null, (Func<string>)null);
			_sessionDeathsWvW = cache.DefineSetting<int>("SessionDeathsWvW", 0, (Func<string>)null, (Func<string>)null);
			_sessionDeathsDaily = cache.DefineSetting<int>("SessionDeathsDaily", 0, (Func<string>)null, (Func<string>)null);
			_totalKillsAtResetWvW = cache.DefineSetting<int>("TotalKillsAtResetWvW", 0, (Func<string>)null, (Func<string>)null);
			_totalKillsAtResetPvP = cache.DefineSetting<int>("TotalKillsAtResetPvP", 0, (Func<string>)null, (Func<string>)null);
			_totalDeathsAtResetWvW = cache.DefineSetting<int>("TotalDeathsAtResetWvW", 0, (Func<string>)null, (Func<string>)null);
			_totalDeathsAtResetDaily = cache.DefineSetting<int>("TotalDeathsAtResetDaily", 0, (Func<string>)null, (Func<string>)null);
		}

		protected override void Initialize()
		{
			Gw2ApiManager.add_SubtokenUpdated((EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>>)SubTokenUpdated);
		}

		private void SubTokenUpdated(object o, ValueEventArgs<IEnumerable<TokenPermission>> e)
		{
			_hasSubToken = true;
		}

		protected override async Task LoadAsync()
		{
			await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/wvw_kills_week.txt", "0⚔", overwrite: false);
			await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/wvw_kills_total.txt", "0⚔", overwrite: false);
			await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/wvw_kills_day.txt", "0⚔", overwrite: false);
			await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/pvp_kills_day.txt", "0⚔", overwrite: false);
			await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/pvp_kills_total.txt", "0⚔", overwrite: false);
			await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/deaths_week.txt", "0☠", overwrite: false);
			await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/deaths_day.txt", "0☠", overwrite: false);
			await Task.Run(delegate
			{
				Gw2Util.GeneratePvpTierImage(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/pvp_tier_icon.png", 1, 3, overwrite: false);
			});
			await Task.Run(delegate
			{
				Gw2Util.GenerateCoinsImage(ModuleInstance.DirectoriesManager.GetFullDirectoryPath("stream_out") + "/wallet_coins.png", 10000000, overwrite: false);
			});
			await Task.Run(delegate
			{
				Gw2Util.GenerateKarmaImage(ModuleInstance.DirectoriesManager.GetFullDirectoryPath("stream_out") + "/wallet_karma.png", 10000000, overwrite: false);
			});
			await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/pvp_rank.txt", "Bronze I", overwrite: false);
			await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/pvp_win_loss_ratio.txt", "1.0", overwrite: false);
			await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/wvw_rank.txt", "1 : Invader", overwrite: false);
			ExtractIcons("1614804.png", DirectoriesManager.GetFullDirectoryPath("stream_out") + "/pvp_rank_icon.png");
			string staticDir = DirectoriesManager.GetFullDirectoryPath("stream_out") + "/static";
			if (!Directory.Exists(staticDir))
			{
				Directory.CreateDirectory(staticDir);
			}
			ExtractIcons("legendary_divination.png", staticDir + "/legendary_divination.png");
			ExtractIcons("legendary_insight.png", staticDir + "/legendary_insight.png");
			ExtractIcons("unstable_fractal_essence.png", staticDir + "/unstable_fractal_essence.png");
		}

		private void ExtractIcons(string iconName, string iconOutputName)
		{
			if (File.Exists(iconName))
			{
				return;
			}
			using Stream texStr = ContentsManager.GetFileStream(iconName);
			using Bitmap icon = new Bitmap(texStr);
			icon.Save(iconOutputName, ImageFormat.Png);
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			GameService.Gw2Mumble.get_PlayerCharacter().add_NameChanged((EventHandler<ValueEventArgs<string>>)OnNameChanged);
			GameService.Gw2Mumble.get_PlayerCharacter().add_SpecializationChanged((EventHandler<ValueEventArgs<int>>)OnSpecializationChanged);
			GameService.Gw2Mumble.get_CurrentMap().add_MapChanged((EventHandler<ValueEventArgs<int>>)OnMapChanged);
			OnNameChanged(null, new ValueEventArgs<string>(GameService.Gw2Mumble.get_PlayerCharacter().get_Name()));
			OnSpecializationChanged(null, new ValueEventArgs<int>(GameService.Gw2Mumble.get_PlayerCharacter().get_Specialization()));
			OnMapChanged(null, new ValueEventArgs<int>(GameService.Gw2Mumble.get_CurrentMap().get_Id()));
			((Module)this).OnModuleLoaded(e);
		}

		protected override async void Update(GameTime gameTime)
		{
			if (GameService.Gw2Mumble.get_IsAvailable() && !_prevServerAddress.Equals(GameService.Gw2Mumble.get_Info().get_ServerAddress(), StringComparison.InvariantCultureIgnoreCase))
			{
				_prevServerAddress = GameService.Gw2Mumble.get_Info().get_ServerAddress();
				await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/server_address.txt", string.IsNullOrEmpty(GameService.Gw2Mumble.get_Info().get_ServerAddress()) ? string.Empty : (_onlyLastDigitSettingEntry.get_Value() ? ("*" + GameService.Gw2Mumble.get_Info().get_ServerAddress().Substring(GameService.Gw2Mumble.get_Info().get_ServerAddress().LastIndexOf('.'))) : GameService.Gw2Mumble.get_Info().get_ServerAddress()));
			}
			if (_hasSubToken && (!_prevApiRequestTime.HasValue || DateTime.UtcNow.Subtract(_prevApiRequestTime.Value).TotalSeconds > 300.0))
			{
				_prevApiRequestTime = DateTime.UtcNow;
				await CheckForReset();
				await UpdateWallet();
				await UpdateStandingsForPvP();
				await UpdateStatsForPvp();
				await UpdateRankForWvw();
				await UpdateKillsAndDeaths();
				await UpdateKillProofs();
			}
		}

		private async Task UpdateKillsAndDeaths()
		{
			string prefixKills = ((_addUnicodeSymbols.get_Value() == UnicodeSigning.Prefixed) ? "⚔" : string.Empty);
			string suffixKills = ((_addUnicodeSymbols.get_Value() == UnicodeSigning.Suffixed) ? "⚔" : string.Empty);
			string prefixDeaths = ((_addUnicodeSymbols.get_Value() == UnicodeSigning.Prefixed) ? "☠" : string.Empty);
			string suffixDeaths = ((_addUnicodeSymbols.get_Value() == UnicodeSigning.Suffixed) ? "☠" : string.Empty);
			int totalKillsWvW = await RequestTotalKillsForWvW();
			if (totalKillsWvW >= 0)
			{
				int currentKills = totalKillsWvW - _totalKillsAtResetWvW.get_Value();
				_sessionKillsWvW.set_Value(currentKills);
				_sessionKillsWvwDaily.set_Value(currentKills);
				await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/wvw_kills_week.txt", $"{prefixKills}{_sessionKillsWvW.get_Value()}{suffixKills}");
				await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/wvw_kills_total.txt", $"{prefixKills}{totalKillsWvW}{suffixKills}");
				await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/wvw_kills_day.txt", $"{prefixKills}{_sessionKillsWvwDaily.get_Value()}{suffixKills}");
			}
			int totalKillsPvP = await RequestTotalKillsForPvP();
			if (totalKillsPvP >= 0)
			{
				_sessionKillsPvP.set_Value(totalKillsPvP - _totalKillsAtResetPvP.get_Value());
				await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/pvp_kills_day.txt", $"{prefixKills}{_sessionKillsPvP.get_Value()}{suffixKills}");
				await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/pvp_kills_total.txt", $"{prefixKills}{totalKillsPvP}{suffixKills}");
			}
			int totalDeaths = await RequestTotalDeaths();
			if (totalDeaths >= 0)
			{
				_sessionDeathsDaily.set_Value(totalDeaths - _totalDeathsAtResetDaily.get_Value());
				_sessionDeathsWvW.set_Value(totalDeaths - _totalDeathsAtResetWvW.get_Value());
				await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/deaths_week.txt", $"{prefixDeaths}{_sessionDeathsWvW.get_Value()}{suffixDeaths}");
				await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/deaths_day.txt", $"{prefixDeaths}{_sessionDeathsDaily.get_Value()}{suffixDeaths}");
			}
		}

		protected override void Unload()
		{
			GameService.Gw2Mumble.get_PlayerCharacter().remove_NameChanged((EventHandler<ValueEventArgs<string>>)OnNameChanged);
			GameService.Gw2Mumble.get_PlayerCharacter().remove_SpecializationChanged((EventHandler<ValueEventArgs<int>>)OnSpecializationChanged);
			GameService.Gw2Mumble.get_CurrentMap().remove_MapChanged((EventHandler<ValueEventArgs<int>>)OnMapChanged);
			Gw2ApiManager.remove_SubtokenUpdated((EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>>)SubTokenUpdated);
			ModuleInstance = null;
		}

		private async void OnMapChanged(object o, ValueEventArgs<int> e)
		{
			if (e.get_Value() <= 0)
			{
				await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/map_name.txt", string.Empty);
				await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/map_type.txt", string.Empty);
				return;
			}
			Map map = await ((IBulkExpandableClient<Map, int>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Maps()).GetAsync(e.get_Value(), default(CancellationToken));
			string location = map.get_Name();
			if (map.get_Name().Equals(map.get_RegionName(), StringComparison.InvariantCultureIgnoreCase))
			{
				ContinentFloorRegionMapSector defaultSector = (await RequestSectors(map.get_ContinentId(), map.get_DefaultFloor(), map.get_RegionId(), map.get_Id())).FirstOrDefault();
				if (defaultSector != null && !string.IsNullOrEmpty(defaultSector.get_Name()))
				{
					location = defaultSector.get_Name().Replace("<br>", " ");
				}
			}
			await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/map_name.txt", location);
			string type = string.Empty;
			MapType value = map.get_Type().get_Value();
			MapType val = value;
			switch (val - 1)
			{
			case 8:
			case 9:
			case 10:
			case 11:
			case 13:
			case 14:
			case 17:
				type = "WvW";
				break;
			case 4:
			case 15:
				type = ((map.get_Id() != 350) ? "PvE" : "PvP");
				break;
			case 1:
				type = "PvP";
				break;
			case 2:
				type = "GvG";
				break;
			case 0:
			case 3:
			case 5:
			case 6:
			case 7:
			case 12:
				type = ((Enum)(object)map.get_Type().get_Value()).ToDisplayString();
				break;
			}
			await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/map_type.txt", type);
		}

		private async void OnNameChanged(object o, ValueEventArgs<string> e)
		{
			await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/character_name.txt", e.get_Value() ?? string.Empty);
		}

		private async void OnSpecializationChanged(object o, ValueEventArgs<int> e)
		{
			if (e.get_Value() <= 0)
			{
				ClearImage(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/profession_icon.png");
				return;
			}
			Specialization specialization = await ((IBulkExpandableClient<Specialization, int>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Specializations()).GetAsync(e.get_Value(), default(CancellationToken));
			Profession profession = await ((IBulkAliasExpandableClient<Profession, ProfessionType>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Professions()).GetAsync(GameService.Gw2Mumble.get_PlayerCharacter().get_Profession(), default(CancellationToken));
			StreamOutModule streamOutModule = this;
			RenderUrl? val = (specialization.get_Elite() ? specialization.get_ProfessionIconBig() : new RenderUrl?(profession.get_IconBig()));
			await streamOutModule.SaveToImage(val.HasValue ? RenderUrl.op_Implicit(val.GetValueOrDefault()) : null, DirectoriesManager.GetFullDirectoryPath("stream_out") + "/profession_icon.png");
		}

		private async Task SaveToImage(string renderUri, string path)
		{
			await Gw2ApiManager.get_Gw2ApiClient().get_Render().DownloadToByteArrayAsync(renderUri, default(CancellationToken))
				.ContinueWith(delegate(Task<byte[]> textureDataResponse)
				{
					if (textureDataResponse.IsFaulted)
					{
						Logger.Warn("Request to render service for " + renderUri + " failed.");
					}
					else
					{
						using MemoryStream stream = new MemoryStream(textureDataResponse.Result);
						using Bitmap bitmap = new Bitmap(stream);
						bitmap.Save(path, ImageFormat.Png);
					}
				});
		}

		private void ClearImage(string path)
		{
			if (!File.Exists(path))
			{
				return;
			}
			using MemoryStream stream = new MemoryStream(File.ReadAllBytes(path));
			using Bitmap bitmap = (Bitmap)Image.FromStream(stream);
			using (Graphics gfx = Graphics.FromImage(bitmap))
			{
				gfx.Clear(Color.Transparent);
				gfx.Flush();
			}
			bitmap.Save(path, ImageFormat.Png);
		}

		private async Task<IEnumerable<ContinentFloorRegionMapSector>> RequestSectors(int continentId, int floor, int regionId, int mapId)
		{
			return await ((IAllExpandableClient<ContinentFloorRegionMapSector>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Continents()
				.get_Item(continentId)
				.get_Floors()
				.get_Item(floor)
				.get_Regions()
				.get_Item(regionId)
				.get_Maps()
				.get_Item(mapId)
				.get_Sectors()).AllAsync(default(CancellationToken)).ContinueWith(delegate(Task<IApiV2ObjectList<ContinentFloorRegionMapSector>> task)
			{
				IEnumerable<ContinentFloorRegionMapSector> result2;
				if (!task.IsFaulted)
				{
					IEnumerable<ContinentFloorRegionMapSector> result = (IEnumerable<ContinentFloorRegionMapSector>)task.Result;
					result2 = result;
				}
				else
				{
					result2 = Enumerable.Empty<ContinentFloorRegionMapSector>();
				}
				return result2;
			});
		}

		private async Task<int> RequestTotalDeaths()
		{
			if (!Gw2ApiManager.HasPermissions((IEnumerable<TokenPermission>)(object)new TokenPermission[2]
			{
				(TokenPermission)1,
				(TokenPermission)3
			}))
			{
				return -1;
			}
			return await ((IAllExpandableClient<Character>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Characters()).AllAsync(default(CancellationToken)).ContinueWith((Task<IApiV2ObjectList<Character>> task) => task.IsFaulted ? (-1) : ((IEnumerable<Character>)task.Result).Sum((Character x) => x.get_Deaths()));
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
			return await ((IBlobClient<IApiV2ObjectList<AccountAchievement>>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Account()
				.get_Achievements()).GetAsync(default(CancellationToken)).ContinueWith((Task<IApiV2ObjectList<AccountAchievement>> response) => response.IsFaulted ? (-1) : ((IEnumerable<AccountAchievement>)response.Result).Single((AccountAchievement x) => x.get_Id() == 239).get_Current());
		}

		private async Task ResetWorldVersusWorld(int worldId, bool force = false)
		{
			int num;
			if (!force && _resetTimeWvW.get_Value().HasValue)
			{
				DateTime utcNow = DateTime.UtcNow;
				DateTime? value = _resetTimeWvW.get_Value();
				num = ((utcNow < value) ? 1 : 0);
			}
			else
			{
				num = 0;
			}
			if (num == 0)
			{
				SettingEntry<DateTime?> resetTimeWvW = _resetTimeWvW;
				resetTimeWvW.set_Value(await GetWvWResetTime(worldId));
				_sessionKillsWvW.set_Value(0);
				_sessionDeathsWvW.set_Value(0);
				SettingEntry<int> totalKillsAtResetWvW = _totalKillsAtResetWvW;
				totalKillsAtResetWvW.set_Value(await RequestTotalKillsForWvW());
				SettingEntry<int> totalDeathsAtResetWvW = _totalDeathsAtResetWvW;
				totalDeathsAtResetWvW.set_Value(await RequestTotalDeaths());
			}
		}

		private async Task ResetDaily(bool force = false)
		{
			int num;
			if (!force && _resetTimeDaily.get_Value().HasValue)
			{
				DateTime utcNow = DateTime.UtcNow;
				DateTime? value = _resetTimeDaily.get_Value();
				num = ((utcNow < value) ? 1 : 0);
			}
			else
			{
				num = 0;
			}
			if (num == 0)
			{
				_resetTimeDaily.set_Value((DateTime?)GetDailyResetTime());
				_sessionKillsPvP.set_Value(0);
				_sessionDeathsDaily.set_Value(0);
				_sessionKillsWvwDaily.set_Value(0);
				SettingEntry<int> totalKillsAtResetPvP = _totalKillsAtResetPvP;
				totalKillsAtResetPvP.set_Value(await RequestTotalKillsForPvP());
				SettingEntry<int> totalDeathsAtResetDaily = _totalDeathsAtResetDaily;
				totalDeathsAtResetDaily.set_Value(await RequestTotalDeaths());
			}
		}

		private DateTime GetDailyResetTime()
		{
			DateTime nextDay = DateTime.UtcNow.AddDays(1.0);
			return new DateTime(nextDay.Year, nextDay.Month, nextDay.Day, 2, 0, 0).ToUniversalTime();
		}

		private async Task<DateTime?> GetWvWResetTime(int worldId)
		{
			return await ((IBlobClient<WvwMatch>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Wvw()
				.get_Matches()
				.World(worldId)).GetAsync(default(CancellationToken)).ContinueWith((Task<WvwMatch> r) => r.IsFaulted ? null : new DateTime?(r.Result.get_EndTime().UtcDateTime));
		}

		private async Task CheckForReset()
		{
			if (!Gw2ApiManager.HasPermission((TokenPermission)1))
			{
				return;
			}
			await ((IBlobClient<Account>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Account()).GetAsync(default(CancellationToken)).ContinueWith((Func<Task<Account>, Task>)async delegate(Task<Account> response)
			{
				if (!response.IsFaulted)
				{
					bool isNewAcc = !response.Result.get_Id().Equals(_accountGuid.get_Value());
					_accountName.set_Value(response.Result.get_Name());
					_accountGuid.set_Value(response.Result.get_Id());
					await ResetWorldVersusWorld(response.Result.get_World(), isNewAcc);
					await ResetDaily(isNewAcc);
				}
			});
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
			await ((IAllExpandableClient<PvpSeason>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Pvp()
				.get_Seasons()).AllAsync(default(CancellationToken)).ContinueWith((Func<Task<IApiV2ObjectList<PvpSeason>>, Task>)async delegate(Task<IApiV2ObjectList<PvpSeason>> task)
			{
				if (!task.IsFaulted)
				{
					PvpSeason season = ((IEnumerable<PvpSeason>)task.Result).OrderByDescending((PvpSeason x) => x.get_End()).First();
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
								for (int i = 0; i < ranksTotal; i++)
								{
									PvpSeasonRank currentRank = season.get_Ranks()[i];
									PvpSeasonRank nextRank = ((i + 1 < ranksTotal) ? season.get_Ranks()[i + 1] : null);
									int tiersTotal = currentRank.get_Tiers().Count;
									for (int j = 0; j < tiersTotal; j++)
									{
										int currentTier = currentRank.get_Tiers()[j].get_Rating();
										object obj;
										if (j + 1 >= tiersTotal)
										{
											obj = ((nextRank != null) ? nextRank.get_Tiers()[0] : null);
										}
										else
										{
											obj = currentRank.get_Tiers()[j + 1];
										}
										PvpSeasonRankTier nextTier = (PvpSeasonRankTier)obj;
										if (i == 0 && j == 0 && standing.get_Current().get_Rating() < currentTier)
										{
											found = true;
											break;
										}
										if (i + 1 == ranksTotal && j + 1 == tiersTotal && standing.get_Current().get_Rating() >= currentTier)
										{
											rank = season.get_Ranks().Last();
											tier = tiersTotal;
											found = true;
											break;
										}
										int num;
										if (currentTier <= standing.get_Current().get_Rating())
										{
											num = ((standing.get_Current().get_Rating() < ((nextTier != null) ? nextTier.get_Rating() : 0)) ? 1 : 0);
										}
										else
										{
											num = 0;
										}
										if (num != 0)
										{
											tier = j + 1;
											rank = currentRank;
											found = true;
											break;
										}
									}
									if (found)
									{
										break;
									}
								}
								await Task.Run(delegate
								{
									Gw2Util.GeneratePvpTierImage(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/pvp_tier_icon.png", tier, rank.get_Tiers().Count);
								});
								await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/pvp_rank.txt", rank.get_Name() + " " + tier.ToRomanNumeral());
								await SaveToImage(RenderUrl.op_Implicit(rank.get_OverlaySmall()), DirectoriesManager.GetFullDirectoryPath("stream_out") + "/pvp_rank_icon.png");
							}
						}
					});
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
			await ((IBlobClient<PvpStats>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Pvp()
				.get_Stats()).GetAsync(default(CancellationToken)).ContinueWith((Func<Task<PvpStats>, Task>)async delegate(Task<PvpStats> task)
			{
				if (!task.IsFaulted)
				{
					KeyValuePair<string, PvpStatsAggregate>[] ranked = (from x in task.Result.get_Ladders()
						where !x.Key.Contains("unranked") && x.Key.Contains("ranked")
						select x).ToArray();
					int wins = ranked.Sum((KeyValuePair<string, PvpStatsAggregate> x) => x.Value.get_Wins());
					int losses = ranked.Sum((KeyValuePair<string, PvpStatsAggregate> x) => x.Value.get_Losses());
					int byes = ranked.Sum((KeyValuePair<string, PvpStatsAggregate> x) => x.Value.get_Byes());
					int desertions = ranked.Sum((KeyValuePair<string, PvpStatsAggregate> x) => x.Value.get_Desertions());
					double totalGames = wins + losses + desertions + byes;
					if (!(totalGames <= 0.0))
					{
						double winRatio = (double)(wins + byes) / totalGames * 100.0;
						double lossRatio = (double)(losses + desertions) / totalGames * 100.0;
						await FileUtil.WriteAllTextAsync(data: Math.Round(winRatio / lossRatio, 1).ToString(CultureInfo.InvariantCulture), filePath: DirectoriesManager.GetFullDirectoryPath("stream_out") + "/pvp_win_loss_ratio.txt");
					}
				}
			});
		}

		private async Task UpdateWallet()
		{
			if (!Gw2ApiManager.HasPermissions((IEnumerable<TokenPermission>)(object)new TokenPermission[2]
			{
				(TokenPermission)1,
				(TokenPermission)10
			}))
			{
				return;
			}
			await ((IBlobClient<IApiV2ObjectList<AccountCurrency>>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Account()
				.get_Wallet()).GetAsync(default(CancellationToken)).ContinueWith((Func<Task<IApiV2ObjectList<AccountCurrency>>, Task>)async delegate(Task<IApiV2ObjectList<AccountCurrency>> task)
			{
				if (!task.IsFaulted)
				{
					int coins = ((IEnumerable<AccountCurrency>)task.Result).First((AccountCurrency x) => x.get_Id() == 1).get_Value();
					await Task.Run(delegate
					{
						Gw2Util.GenerateCoinsImage(ModuleInstance.DirectoriesManager.GetFullDirectoryPath("stream_out") + "/wallet_coins.png", coins);
					});
					int karma = ((IEnumerable<AccountCurrency>)task.Result).First((AccountCurrency x) => x.get_Id() == 2).get_Value();
					await Task.Run(delegate
					{
						Gw2Util.GenerateKarmaImage(ModuleInstance.DirectoriesManager.GetFullDirectoryPath("stream_out") + "/wallet_karma.png", karma);
					});
				}
			});
		}

		private async Task UpdateKillProofs()
		{
			await TaskUtil.GetJsonResponse<object>(string.Format("{0}{1}?lang={2}", "https://killproof.me/api/kp/", _accountName.get_Value(), GameService.Overlay.get_UserLocale().get_Value())).ContinueWith((Func<Task<(bool, object)>, Task>)async delegate(Task<(bool, dynamic)> task)
			{
				if (!task.IsFaulted && ((ValueTuple<bool, object>)task.Result).Item1)
				{
					IEnumerable<object> killProofs = ((dynamic)((ValueTuple<bool, object>)task.Result).Item2).killproofs;
					if (!killProofs.IsNullOrEmpty())
					{
						string killproofDir = DirectoriesManager.GetFullDirectoryPath("stream_out") + "/killproof.me";
						if (!Directory.Exists(killproofDir))
						{
							Directory.CreateDirectory(killproofDir);
						}
						int count = 0;
						AsyncTaskMethodBuilder asyncTaskMethodBuilder = default(AsyncTaskMethodBuilder);
						foreach (object killProof in killProofs)
						{
							switch ((int)((dynamic)killProof).id)
							{
							case 88485:
							{
								object awaiter5 = FileUtil.WriteAllTextAsync(killproofDir + "/legendary_divination.txt", ((dynamic)killProof).amount.ToString()).GetAwaiter();
								if (!(bool)((dynamic)awaiter5).IsCompleted)
								{
									ICriticalNotifyCompletion awaiter2 = awaiter5 as ICriticalNotifyCompletion;
									_003C_003CUpdateKillProofs_003Eb__82_0_003Ed stateMachine = (_003C_003CUpdateKillProofs_003Eb__82_0_003Ed)/*Error near IL_047e: stateMachine*/;
									if (awaiter2 == null)
									{
										INotifyCompletion awaiter3 = (INotifyCompletion)awaiter5;
										asyncTaskMethodBuilder.AwaitOnCompleted(ref awaiter3, ref stateMachine);
									}
									else
									{
										asyncTaskMethodBuilder.AwaitUnsafeOnCompleted(ref awaiter2, ref stateMachine);
									}
									/*Error near IL_04b6: leave MoveNext - await not detected correctly*/;
								}
								((dynamic)awaiter5).GetResult();
								count++;
								break;
							}
							case 77302:
							{
								object awaiter4 = FileUtil.WriteAllTextAsync(killproofDir + "/legendary_insight.txt", ((dynamic)killProof).amount.ToString()).GetAwaiter();
								if (!(bool)((dynamic)awaiter4).IsCompleted)
								{
									ICriticalNotifyCompletion awaiter2 = awaiter4 as ICriticalNotifyCompletion;
									_003C_003CUpdateKillProofs_003Eb__82_0_003Ed stateMachine = (_003C_003CUpdateKillProofs_003Eb__82_0_003Ed)/*Error near IL_0754: stateMachine*/;
									if (awaiter2 == null)
									{
										INotifyCompletion awaiter3 = (INotifyCompletion)awaiter4;
										asyncTaskMethodBuilder.AwaitOnCompleted(ref awaiter3, ref stateMachine);
									}
									else
									{
										asyncTaskMethodBuilder.AwaitUnsafeOnCompleted(ref awaiter2, ref stateMachine);
									}
									/*Error near IL_078c: leave MoveNext - await not detected correctly*/;
								}
								((dynamic)awaiter4).GetResult();
								count++;
								break;
							}
							case 94020:
							{
								object awaiter = FileUtil.WriteAllTextAsync(killproofDir + "/unstable_fractal_essence.txt", ((dynamic)killProof).amount.ToString()).GetAwaiter();
								if (!(bool)((dynamic)awaiter).IsCompleted)
								{
									ICriticalNotifyCompletion awaiter2 = awaiter as ICriticalNotifyCompletion;
									_003C_003CUpdateKillProofs_003Eb__82_0_003Ed stateMachine = (_003C_003CUpdateKillProofs_003Eb__82_0_003Ed)/*Error near IL_0a2a: stateMachine*/;
									if (awaiter2 == null)
									{
										INotifyCompletion awaiter3 = (INotifyCompletion)awaiter;
										asyncTaskMethodBuilder.AwaitOnCompleted(ref awaiter3, ref stateMachine);
									}
									else
									{
										asyncTaskMethodBuilder.AwaitUnsafeOnCompleted(ref awaiter2, ref stateMachine);
									}
									/*Error near IL_0a62: leave MoveNext - await not detected correctly*/;
								}
								((dynamic)awaiter).GetResult();
								count++;
								break;
							}
							}
							if (count == 3)
							{
								break;
							}
						}
					}
				}
			});
		}
	}
}
