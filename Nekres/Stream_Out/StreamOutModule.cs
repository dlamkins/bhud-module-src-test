using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi;
using Gw2Sharp.WebApi.Exceptions;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Nekres.Stream_Out.UI.Models;
using Nekres.Stream_Out.UI.Views;

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

		private SettingEntry<bool> _useCatmanderTag;

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

		private const string COMMANDER_ICON = "commander_icon.png";

		private const string WALLET_COINS = "wallet_coins.png";

		private const string WALLET_KARMA = "wallet_karma.png";

		private const string GUILD_NAME = "guild_name.txt";

		private const string GUILD_TAG = "guild_tag.txt";

		private const string GUILD_EMBLEM = "guild_emblem.png";

		private const string GUILD_MOTD = "guild_motd.txt";

		private const string WVW_KILLS_WEEK = "wvw_kills_week.txt";

		private const string WVW_KILLS_DAY = "wvw_kills_day.txt";

		private const string WVW_KILLS_TOTAL = "wvw_kills_total.txt";

		private const string WVW_RANK = "wvw_rank.txt";

		private const string PVP_KILLS_TOTAL = "pvp_kills_total.txt";

		private const string PVP_KILLS_DAY = "pvp_kills_day.txt";

		private const string PVP_RANK = "pvp_rank.txt";

		private const string PVP_RANK_ICON = "pvp_rank_icon.png";

		private const string PVP_TIER_ICON = "pvp_tier_icon.png";

		private const string PVP_WINRATE = "pvp_winrate.txt";

		private const string KILLPROOF_ME_UNSTABLE_FRACTAL_ESSENCE = "unstable_fractal_essence.txt";

		private const string KILLPROOF_ME_LEGENDARY_DIVINATION = "legendary_divination.txt";

		private const string KILLPROOF_ME_LEGENDARY_INSIGHT = "legendary_insight.txt";

		private const string DEATHS_WEEK = "deaths_week.txt";

		private const string DEATHS_DAY = "deaths_day.txt";

		private const string SKULL = "☠";

		private const string SWORDS = "⚔";

		private const string KILLPROOF_API_URL = "https://killproof.me/api/kp/";

		private Bitmap Commander_Icon;

		private Bitmap Catmander_Icon;

		private Regex GUILD_MOTD_PUBLIC = new Regex("(?<=\\[public\\]).*(?=\\[\\/public\\])", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);

		private bool _hasSubToken;

		private DateTime? _prevApiRequestTime;

		private string _prevServerAddress = "";

		internal SettingsManager SettingsManager => ModuleParameters.SettingsManager;

		internal ContentsManager ContentsManager => ModuleParameters.ContentsManager;

		internal DirectoriesManager DirectoriesManager => ModuleParameters.DirectoriesManager;

		internal Gw2ApiManager Gw2ApiManager => ModuleParameters.Gw2ApiManager;

		[ImportingConstructor]
		public StreamOutModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: base(moduleParameters)
		{
			ModuleInstance = this;
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			_onlyLastDigitSettingEntry = settings.DefineSetting("OnlyLastDigits", defaultValue: true, () => "Only Output Last Digits of Server Address", () => "Only outputs the last digits of the server address you are currently connected to.\nThis is the address shown when entering \"/ip\" in chat.");
			_addUnicodeSymbols = settings.DefineSetting("UnicodeSymbols", UnicodeSigning.Suffixed, () => "Numeric Value Signing", () => "The way numeric values should be signed with unicode symbols.");
			_useCatmanderTag = settings.DefineSetting("CatmanderTag", defaultValue: false, () => "Use Catmander Tag", () => "Replaces the commander_icon.png with the Catmander icon if you tag up as Commander in-game.");
			SettingCollection cache = settings.AddSubCollection("CachedValues");
			cache.RenderInUi = false;
			_accountGuid = cache.DefineSetting("AccountGuid", Guid.Empty);
			_accountName = cache.DefineSetting("AccountName", string.Empty);
			_resetTimeWvW = cache.DefineSetting<DateTime?>("ResetTimeWvW", null);
			_resetTimeDaily = cache.DefineSetting<DateTime?>("ResetTimeDaily", null);
			_sessionKillsWvW = cache.DefineSetting("SessionKillsWvW", 0);
			_sessionKillsWvwDaily = cache.DefineSetting("SessionsKillsWvWDaily", 0);
			_sessionKillsPvP = cache.DefineSetting("SessionKillsPvP", 0);
			_sessionDeathsWvW = cache.DefineSetting("SessionDeathsWvW", 0);
			_sessionDeathsDaily = cache.DefineSetting("SessionDeathsDaily", 0);
			_totalKillsAtResetWvW = cache.DefineSetting("TotalKillsAtResetWvW", 0);
			_totalKillsAtResetPvP = cache.DefineSetting("TotalKillsAtResetPvP", 0);
			_totalDeathsAtResetWvW = cache.DefineSetting("TotalDeathsAtResetWvW", 0);
			_totalDeathsAtResetDaily = cache.DefineSetting("TotalDeathsAtResetDaily", 0);
		}

		public override IView GetSettingsView()
		{
			return new CustomSettingsView(new CustomSettingsModel(SettingsManager.ModuleSettings));
		}

		protected override void Initialize()
		{
			Gw2ApiManager.SubtokenUpdated += SubTokenUpdated;
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
			await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/pvp_winrate.txt", "50%", overwrite: false);
			await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/wvw_rank.txt", "1 : Invader", overwrite: false);
			string moduleDir = DirectoriesManager.GetFullDirectoryPath("stream_out");
			ExtractIcons("1614804.png", moduleDir, "pvp_rank_icon.png");
			ExtractIcons(_useCatmanderTag.Value ? "catmander_tag_white.png" : "commander_tag_white.png", moduleDir, "commander_icon.png");
			if (!GameService.Gw2Mumble.PlayerCharacter.IsCommander)
			{
				ClearImage(moduleDir + "/commander_icon.png");
			}
			ExtractIcons("legendary_divination.png", moduleDir + "/static/", "legendary_divination.png");
			ExtractIcons("legendary_insight.png", moduleDir + "/static/", "legendary_insight.png");
			ExtractIcons("unstable_fractal_essence.png", moduleDir + "/static/", "unstable_fractal_essence.png");
		}

		private void ExtractIcons(string iconName, string outputDir, string iconOutputName)
		{
			if (!Directory.Exists(outputDir))
			{
				Directory.CreateDirectory(outputDir);
			}
			string fullname = outputDir + "/" + iconOutputName;
			if (System.IO.File.Exists(fullname))
			{
				return;
			}
			using Stream texStr = ContentsManager.GetFileStream(iconName);
			using Bitmap icon = new Bitmap(texStr);
			icon.Save(fullname, ImageFormat.Png);
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			GameService.Gw2Mumble.PlayerCharacter.NameChanged += OnNameChanged;
			GameService.Gw2Mumble.PlayerCharacter.SpecializationChanged += OnSpecializationChanged;
			GameService.Gw2Mumble.PlayerCharacter.IsCommanderChanged += OnIsCommanderChanged;
			GameService.Gw2Mumble.CurrentMap.MapChanged += OnMapChanged;
			_useCatmanderTag.SettingChanged += OnUseCatmanderTagSettingChanged;
			OnNameChanged(null, new ValueEventArgs<string>(GameService.Gw2Mumble.PlayerCharacter.Name));
			OnSpecializationChanged(null, new ValueEventArgs<int>(GameService.Gw2Mumble.PlayerCharacter.Specialization));
			OnMapChanged(null, new ValueEventArgs<int>(GameService.Gw2Mumble.CurrentMap.Id));
			base.OnModuleLoaded(e);
		}

		protected override async void Update(GameTime gameTime)
		{
			if (GameService.Gw2Mumble.IsAvailable && !_prevServerAddress.Equals(GameService.Gw2Mumble.Info.ServerAddress, StringComparison.InvariantCultureIgnoreCase))
			{
				_prevServerAddress = GameService.Gw2Mumble.Info.ServerAddress;
				await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/server_address.txt", string.IsNullOrEmpty(GameService.Gw2Mumble.Info.ServerAddress) ? string.Empty : (_onlyLastDigitSettingEntry.Value ? ("*" + GameService.Gw2Mumble.Info.ServerAddress.Substring(GameService.Gw2Mumble.Info.ServerAddress.LastIndexOf('.'))) : GameService.Gw2Mumble.Info.ServerAddress));
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
				await UpdateGuild();
			}
		}

		private async Task UpdateKillsAndDeaths()
		{
			string prefixKills = ((_addUnicodeSymbols.Value == UnicodeSigning.Prefixed) ? "⚔" : string.Empty);
			string suffixKills = ((_addUnicodeSymbols.Value == UnicodeSigning.Suffixed) ? "⚔" : string.Empty);
			string prefixDeaths = ((_addUnicodeSymbols.Value == UnicodeSigning.Prefixed) ? "☠" : string.Empty);
			string suffixDeaths = ((_addUnicodeSymbols.Value == UnicodeSigning.Suffixed) ? "☠" : string.Empty);
			int totalKillsWvW = await RequestTotalKillsForWvW();
			if (totalKillsWvW >= 0)
			{
				int currentKills = totalKillsWvW - _totalKillsAtResetWvW.Value;
				_sessionKillsWvW.Value = currentKills;
				_sessionKillsWvwDaily.Value = currentKills;
				await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/wvw_kills_week.txt", $"{prefixKills}{_sessionKillsWvW.Value}{suffixKills}");
				await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/wvw_kills_total.txt", $"{prefixKills}{totalKillsWvW}{suffixKills}");
				await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/wvw_kills_day.txt", $"{prefixKills}{_sessionKillsWvwDaily.Value}{suffixKills}");
			}
			int totalKillsPvP = await RequestTotalKillsForPvP();
			if (totalKillsPvP >= 0)
			{
				_sessionKillsPvP.Value = totalKillsPvP - _totalKillsAtResetPvP.Value;
				await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/pvp_kills_day.txt", $"{prefixKills}{_sessionKillsPvP.Value}{suffixKills}");
				await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/pvp_kills_total.txt", $"{prefixKills}{totalKillsPvP}{suffixKills}");
			}
			int totalDeaths = await RequestTotalDeaths();
			if (totalDeaths >= 0)
			{
				_sessionDeathsDaily.Value = totalDeaths - _totalDeathsAtResetDaily.Value;
				_sessionDeathsWvW.Value = totalDeaths - _totalDeathsAtResetWvW.Value;
				await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/deaths_week.txt", $"{prefixDeaths}{_sessionDeathsWvW.Value}{suffixDeaths}");
				await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/deaths_day.txt", $"{prefixDeaths}{_sessionDeathsDaily.Value}{suffixDeaths}");
			}
		}

		protected override void Unload()
		{
			GameService.Gw2Mumble.PlayerCharacter.NameChanged -= OnNameChanged;
			GameService.Gw2Mumble.PlayerCharacter.SpecializationChanged -= OnSpecializationChanged;
			GameService.Gw2Mumble.CurrentMap.MapChanged -= OnMapChanged;
			GameService.Gw2Mumble.PlayerCharacter.IsCommanderChanged -= OnIsCommanderChanged;
			Gw2ApiManager.SubtokenUpdated -= SubTokenUpdated;
			_useCatmanderTag.SettingChanged -= OnUseCatmanderTagSettingChanged;
			ModuleInstance = null;
		}

		private async void OnMapChanged(object o, ValueEventArgs<int> e)
		{
			if (e.Value <= 0)
			{
				await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/map_name.txt", string.Empty);
				await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/map_type.txt", string.Empty);
				return;
			}
			Map map;
			try
			{
				map = await Gw2ApiManager.Gw2ApiClient.V2.Maps.GetAsync(e.Value);
				if (map == null)
				{
					throw new NullReferenceException("Unknown error.");
				}
			}
			catch (Exception ex) when (ex is UnexpectedStatusException || ex is NullReferenceException)
			{
				Logger.Warn(CommonStrings.WebApiDown);
				return;
			}
			string location = map.Name;
			if (map.Name.Equals(map.RegionName, StringComparison.InvariantCultureIgnoreCase))
			{
				ContinentFloorRegionMapSector defaultSector = (await RequestSectors(map.ContinentId, map.DefaultFloor, map.RegionId, map.Id)).FirstOrDefault();
				if (defaultSector != null && !string.IsNullOrEmpty(defaultSector.Name))
				{
					location = defaultSector.Name.Replace("<br>", " ");
				}
			}
			await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/map_name.txt", location);
			string type = string.Empty;
			switch (map.Type.Value)
			{
			case MapType.Center:
			case MapType.BlueHome:
			case MapType.GreenHome:
			case MapType.RedHome:
			case MapType.JumpPuzzle:
			case MapType.EdgeOfTheMists:
			case MapType.WvwLounge:
				type = "WvW";
				break;
			case MapType.Public:
			case MapType.PublicMini:
				type = ((map.Id != 350) ? "PvE" : "PvP");
				break;
			case MapType.Pvp:
				type = "PvP";
				break;
			case MapType.Gvg:
				type = "GvG";
				break;
			case MapType.CharacterCreate:
			case MapType.Instance:
			case MapType.Tournament:
			case MapType.Tutorial:
			case MapType.UserTournament:
			case MapType.FortunesVale:
				type = map.Type.Value.ToDisplayString();
				break;
			}
			await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/map_type.txt", type);
		}

		private async void OnNameChanged(object o, ValueEventArgs<string> e)
		{
			await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/character_name.txt", e.Value ?? string.Empty);
		}

		private async void OnSpecializationChanged(object o, ValueEventArgs<int> e)
		{
			if (e.Value <= 0)
			{
				ClearImage(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/profession_icon.png");
				return;
			}
			try
			{
				Specialization specialization = await Gw2ApiManager.Gw2ApiClient.V2.Specializations.GetAsync(e.Value);
				Profession profession = await Gw2ApiManager.Gw2ApiClient.V2.Professions.GetAsync(GameService.Gw2Mumble.PlayerCharacter.Profession);
				StreamOutModule streamOutModule = this;
				RenderUrl? renderUrl = (specialization.Elite ? specialization.ProfessionIconBig : new RenderUrl?(profession.IconBig));
				await streamOutModule.SaveToImage(renderUrl.HasValue ? ((string)renderUrl.GetValueOrDefault()) : null, DirectoriesManager.GetFullDirectoryPath("stream_out") + "/profession_icon.png");
			}
			catch (UnexpectedStatusException)
			{
				Logger.Warn(CommonStrings.WebApiDown);
			}
		}

		private async void OnIsCommanderChanged(object o, ValueEventArgs<bool> e)
		{
			if (!e.Value)
			{
				ClearImage(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/commander_icon.png");
			}
			else
			{
				SaveCommanderIcon(_useCatmanderTag.Value);
			}
		}

		private async void SaveCommanderIcon(bool useCatmanderIcon)
		{
			if (useCatmanderIcon)
			{
				if (Catmander_Icon == null)
				{
					using Stream stream = ContentsManager.GetFileStream("catmander_tag_white.png");
					Catmander_Icon = new Bitmap(stream);
					await stream.FlushAsync();
				}
				Catmander_Icon.Save(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/commander_icon.png", ImageFormat.Png);
				return;
			}
			if (Commander_Icon == null)
			{
				using Stream stream = ContentsManager.GetFileStream("commander_tag_white.png");
				Commander_Icon = new Bitmap(stream);
				await stream.FlushAsync();
			}
			Commander_Icon.Save(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/commander_icon.png", ImageFormat.Png);
		}

		private void OnUseCatmanderTagSettingChanged(object o, ValueChangedEventArgs<bool> e)
		{
			if (GameService.Gw2Mumble.PlayerCharacter.IsCommander)
			{
				SaveCommanderIcon(e.NewValue);
			}
		}

		private async Task SaveToImage(string renderUri, string path)
		{
			await Gw2ApiManager.Gw2ApiClient.Render.DownloadToByteArrayAsync(renderUri).ContinueWith(delegate(Task<byte[]> textureDataResponse)
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
			if (!System.IO.File.Exists(path))
			{
				return;
			}
			using MemoryStream stream = new MemoryStream(System.IO.File.ReadAllBytes(path));
			using Bitmap bitmap = (Bitmap)Image.FromStream(stream);
			using (Graphics gfx = Graphics.FromImage(bitmap))
			{
				gfx.Clear(System.Drawing.Color.Transparent);
				gfx.Flush();
			}
			bitmap.Save(path, ImageFormat.Png);
		}

		private async Task<IEnumerable<ContinentFloorRegionMapSector>> RequestSectors(int continentId, int floor, int regionId, int mapId)
		{
			return await Gw2ApiManager.Gw2ApiClient.V2.Continents[continentId].Floors[floor].Regions[regionId].Maps[mapId].Sectors.AllAsync().ContinueWith((Task<IApiV2ObjectList<ContinentFloorRegionMapSector>> task) => (!task.IsFaulted) ? task.Result : Enumerable.Empty<ContinentFloorRegionMapSector>());
		}

		private async Task<int> RequestTotalDeaths()
		{
			if (!Gw2ApiManager.HasPermissions(new TokenPermission[2]
			{
				TokenPermission.Account,
				TokenPermission.Characters
			}))
			{
				return -1;
			}
			return await Gw2ApiManager.Gw2ApiClient.V2.Characters.AllAsync().ContinueWith((Task<IApiV2ObjectList<Character>> task) => (!task.IsFaulted) ? task.Result.Sum((Character x) => x.Deaths) : (-1));
		}

		private async Task UpdateGuild()
		{
			if (!GameService.Gw2Mumble.IsAvailable || !Gw2ApiManager.HasPermissions(new TokenPermission[3]
			{
				TokenPermission.Account,
				TokenPermission.Characters,
				TokenPermission.Guilds
			}))
			{
				return;
			}
			Guid guildId = await Gw2ApiManager.Gw2ApiClient.V2.Characters[GameService.Gw2Mumble.PlayerCharacter.Name].Core.GetAsync().ContinueWith((Task<CharactersCore> task) => (!task.IsFaulted) ? task.Result.Guild : Guid.Empty);
			if (guildId.Equals(Guid.Empty))
			{
				await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/guild_name.txt", string.Empty);
				await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/guild_tag.txt", string.Empty);
				await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/guild_motd.txt", string.Empty);
				ClearImage(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/guild_emblem.png");
				return;
			}
			await Gw2ApiManager.Gw2ApiClient.V2.Guild[guildId].GetAsync().ContinueWith((Func<Task<Guild>, Task>)async delegate(Task<Guild> task)
			{
				if (!task.IsFaulted)
				{
					string name = task.Result.Name;
					string tag = task.Result.Tag;
					string motd = task.Result.Motd ?? string.Empty;
					await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/guild_name.txt", name);
					await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/guild_tag.txt", "[" + tag + "]");
					await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/guild_motd.txt", GUILD_MOTD_PUBLIC.Match(motd).Value);
					GuildEmblem emblem = task.Result.Emblem;
					if (emblem == null)
					{
						ClearImage(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/guild_emblem.png");
					}
					else
					{
						Emblem bg = await Gw2ApiManager.Gw2ApiClient.V2.Emblem.Backgrounds.GetAsync(emblem.Background.Id);
						Emblem fg = await Gw2ApiManager.Gw2ApiClient.V2.Emblem.Foregrounds.GetAsync(emblem.Foreground.Id);
						List<RenderUrl> layersCombined = new List<RenderUrl>();
						if (bg != null)
						{
							layersCombined.AddRange(bg.Layers);
						}
						if (fg != null)
						{
							layersCombined.AddRange(fg.Layers.Skip(1));
						}
						List<Bitmap> layers = new List<Bitmap>();
						foreach (RenderUrl renderUrl in layersCombined)
						{
							using MemoryStream textureStream = new MemoryStream();
							await renderUrl.DownloadToStreamAsync(textureStream);
							layers.Add(new Bitmap(textureStream));
						}
						if (!layers.Any())
						{
							ClearImage(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/guild_emblem.png");
						}
						else
						{
							List<int> colorsCombined = new List<int>();
							colorsCombined.AddRange(emblem.Background.Colors);
							colorsCombined.AddRange(emblem.Foreground.Colors);
							List<Gw2Sharp.WebApi.V2.Models.Color> colors = new List<Gw2Sharp.WebApi.V2.Models.Color>();
							foreach (int color2 in colorsCombined)
							{
								List<Gw2Sharp.WebApi.V2.Models.Color> list = colors;
								list.Add(await Gw2ApiManager.Gw2ApiClient.V2.Colors.GetAsync(color2));
							}
							Bitmap result = new Bitmap(256, 256);
							for (int i = 0; i < layers.Count; i++)
							{
								Bitmap layer = layers[i].FitTo(result);
								if (colors.Any())
								{
									System.Drawing.Color color = System.Drawing.Color.FromArgb(colors[i].Cloth.Rgb[0], colors[i].Cloth.Rgb[1], colors[i].Cloth.Rgb[2]);
									layer.Colorize(color);
								}
								if (bg != null && i < bg.Layers.Count)
								{
									layer.Flip(emblem.Flags.Any((ApiEnum<GuildEmblemFlag> x) => x == GuildEmblemFlag.FlipBackgroundHorizontal), emblem.Flags.Any((ApiEnum<GuildEmblemFlag> x) => x == GuildEmblemFlag.FlipBackgroundVertical));
								}
								else
								{
									layer.Flip(emblem.Flags.Any((ApiEnum<GuildEmblemFlag> x) => x == GuildEmblemFlag.FlipForegroundHorizontal), emblem.Flags.Any((ApiEnum<GuildEmblemFlag> x) => x == GuildEmblemFlag.FlipForegroundVertical));
								}
								Bitmap bitmap = result.Merge(layer);
								result.Dispose();
								layer.Dispose();
								result = bitmap;
							}
							result?.Save(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/guild_emblem.png", ImageFormat.Png);
						}
					}
				}
			});
		}

		private async Task<int> RequestTotalKillsForWvW()
		{
			if (!Gw2ApiManager.HasPermissions(new TokenPermission[2]
			{
				TokenPermission.Account,
				TokenPermission.Progression
			}))
			{
				return -1;
			}
			return await Gw2ApiManager.Gw2ApiClient.V2.Account.Achievements.GetAsync().ContinueWith((Task<IApiV2ObjectList<AccountAchievement>> response) => response.IsFaulted ? (-1) : response.Result.Single((AccountAchievement x) => x.Id == 283).Current);
		}

		private async Task<int> RequestTotalKillsForPvP()
		{
			if (!Gw2ApiManager.HasPermissions(new TokenPermission[2]
			{
				TokenPermission.Account,
				TokenPermission.Progression
			}))
			{
				return -1;
			}
			return await Gw2ApiManager.Gw2ApiClient.V2.Account.Achievements.GetAsync().ContinueWith((Task<IApiV2ObjectList<AccountAchievement>> response) => response.IsFaulted ? (-1) : response.Result.Single((AccountAchievement x) => x.Id == 239).Current);
		}

		private async Task ResetWorldVersusWorld(int worldId, bool force = false)
		{
			if (!force && _resetTimeWvW.Value.HasValue)
			{
				DateTime utcNow = DateTime.UtcNow;
				DateTime? value = _resetTimeWvW.Value;
				if (utcNow < value)
				{
					return;
				}
			}
			SettingEntry<DateTime?> resetTimeWvW = _resetTimeWvW;
			resetTimeWvW.Value = await GetWvWResetTime(worldId);
			_sessionKillsWvW.Value = 0;
			_sessionDeathsWvW.Value = 0;
			SettingEntry<int> totalKillsAtResetWvW = _totalKillsAtResetWvW;
			totalKillsAtResetWvW.Value = await RequestTotalKillsForWvW();
			totalKillsAtResetWvW = _totalDeathsAtResetWvW;
			totalKillsAtResetWvW.Value = await RequestTotalDeaths();
		}

		private async Task ResetDaily(bool force = false)
		{
			if (!force && _resetTimeDaily.Value.HasValue)
			{
				DateTime utcNow = DateTime.UtcNow;
				DateTime? value = _resetTimeDaily.Value;
				if (utcNow < value)
				{
					return;
				}
			}
			_resetTimeDaily.Value = GetDailyResetTime();
			_sessionKillsPvP.Value = 0;
			_sessionDeathsDaily.Value = 0;
			_sessionKillsWvwDaily.Value = 0;
			SettingEntry<int> totalKillsAtResetPvP = _totalKillsAtResetPvP;
			totalKillsAtResetPvP.Value = await RequestTotalKillsForPvP();
			totalKillsAtResetPvP = _totalDeathsAtResetDaily;
			totalKillsAtResetPvP.Value = await RequestTotalDeaths();
		}

		private DateTime GetDailyResetTime()
		{
			DateTime nextDay = DateTime.UtcNow.AddDays(1.0);
			return new DateTime(nextDay.Year, nextDay.Month, nextDay.Day, 2, 0, 0).ToUniversalTime();
		}

		private async Task<DateTime?> GetWvWResetTime(int worldId)
		{
			return await Gw2ApiManager.Gw2ApiClient.V2.Wvw.Matches.World(worldId).GetAsync().ContinueWith((Task<WvwMatch> r) => r.IsFaulted ? null : new DateTime?(r.Result.EndTime.UtcDateTime));
		}

		private async Task CheckForReset()
		{
			if (!Gw2ApiManager.HasPermission(TokenPermission.Account))
			{
				return;
			}
			await Gw2ApiManager.Gw2ApiClient.V2.Account.GetAsync().ContinueWith((Func<Task<Account>, Task>)async delegate(Task<Account> response)
			{
				if (!response.IsFaulted)
				{
					bool isNewAcc = !response.Result.Id.Equals(_accountGuid.Value);
					_accountName.Value = response.Result.Name;
					_accountGuid.Value = response.Result.Id;
					await ResetWorldVersusWorld(response.Result.World, isNewAcc);
					await ResetDaily(isNewAcc);
				}
			});
		}

		private async Task UpdateRankForWvw()
		{
			if (!Gw2ApiManager.HasPermissions(new TokenPermission[2]
			{
				TokenPermission.Account,
				TokenPermission.Progression
			}))
			{
				return;
			}
			await Gw2ApiManager.Gw2ApiClient.V2.Account.GetAsync().ContinueWith((Func<Task<Account>, Task>)async delegate(Task<Account> response)
			{
				if (!response.IsFaulted)
				{
					int? wvwRank = response.Result.WvwRank;
					if (wvwRank.HasValue && !(wvwRank <= 0))
					{
						await Gw2ApiManager.Gw2ApiClient.V2.Wvw.Ranks.AllAsync().ContinueWith((Func<Task<IApiV2ObjectList<WvwRank>>, Task>)async delegate(Task<IApiV2ObjectList<WvwRank>> t)
						{
							if (!t.IsFaulted)
							{
								WvwRank wvwRankObj = t.Result.MaxBy((WvwRank y) => wvwRank >= y.MinRank);
								await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/wvw_rank.txt", $"{wvwRank:N0} : {wvwRankObj.Title}");
							}
						});
					}
				}
			});
		}

		private async Task UpdateStandingsForPvP()
		{
			if (!Gw2ApiManager.HasPermissions(new TokenPermission[2]
			{
				TokenPermission.Account,
				TokenPermission.Pvp
			}))
			{
				return;
			}
			await Gw2ApiManager.Gw2ApiClient.V2.Pvp.Seasons.AllAsync().ContinueWith((Func<Task<IApiV2ObjectList<PvpSeason>>, Task>)async delegate(Task<IApiV2ObjectList<PvpSeason>> task)
			{
				if (!task.IsFaulted)
				{
					PvpSeason season = task.Result.OrderByDescending((PvpSeason x) => x.End).First();
					await Gw2ApiManager.Gw2ApiClient.V2.Pvp.Standings.GetAsync().ContinueWith((Func<Task<ApiV2BaseObjectList<PvpStanding>>, Task>)async delegate(Task<ApiV2BaseObjectList<PvpStanding>> t)
					{
						if (!t.IsFaulted)
						{
							PvpStanding standing = t.Result.FirstOrDefault((PvpStanding x) => x.SeasonId.Equals(season.Id));
							if (standing != null && standing.Current.Rating.HasValue)
							{
								PvpSeasonRank rank = season.Ranks.First();
								int tier = 1;
								bool found = false;
								int ranksTotal = season.Ranks.Count;
								List<PvpSeasonRankTier> data = season.Ranks.SelectMany((PvpSeasonRank x) => x.Tiers).ToList();
								int maxRating = data.MaxBy((PvpSeasonRankTier y) => y.Rating).Rating;
								int rating = data.MinBy((PvpSeasonRankTier y) => y.Rating).Rating;
								if (standing.Current.Rating > maxRating)
								{
									rank = season.Ranks.Last();
									tier = rank.Tiers.Count;
									found = true;
								}
								if (standing.Current.Rating < rating)
								{
									rank = season.Ranks.First();
									tier = 1;
									found = true;
								}
								for (int i = 0; i < ranksTotal; i++)
								{
									if (found)
									{
										break;
									}
									PvpSeasonRank currentRank = season.Ranks[i];
									int tiersTotal = currentRank.Tiers.Count;
									for (int j = 0; j < tiersTotal; j++)
									{
										int rating2 = currentRank.Tiers[j].Rating;
										if (!(standing.Current.Rating > rating2))
										{
											tier = j + 1;
											rank = currentRank;
											found = true;
											break;
										}
									}
								}
								await Task.Run(delegate
								{
									Gw2Util.GeneratePvpTierImage(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/pvp_tier_icon.png", tier, rank.Tiers.Count);
								});
								await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/pvp_rank.txt", rank.Name + " " + tier.ToRomanNumeral());
								await SaveToImage((string)rank.OverlaySmall, DirectoriesManager.GetFullDirectoryPath("stream_out") + "/pvp_rank_icon.png");
							}
						}
					});
				}
			});
		}

		private async Task UpdateStatsForPvp()
		{
			if (!Gw2ApiManager.HasPermissions(new TokenPermission[2]
			{
				TokenPermission.Account,
				TokenPermission.Pvp
			}))
			{
				return;
			}
			await Gw2ApiManager.Gw2ApiClient.V2.Pvp.Stats.GetAsync().ContinueWith((Func<Task<PvpStats>, Task>)async delegate(Task<PvpStats> task)
			{
				if (!task.IsFaulted)
				{
					KeyValuePair<string, PvpStatsAggregate>[] source = task.Result.Ladders.Where<KeyValuePair<string, PvpStatsAggregate>>((KeyValuePair<string, PvpStatsAggregate> x) => !x.Key.Contains("unranked") && x.Key.Contains("ranked")).ToArray();
					int wins = source.Sum((KeyValuePair<string, PvpStatsAggregate> x) => x.Value.Wins);
					int losses = source.Sum((KeyValuePair<string, PvpStatsAggregate> x) => x.Value.Losses);
					int byes = source.Sum((KeyValuePair<string, PvpStatsAggregate> x) => x.Value.Byes);
					int desertions = source.Sum((KeyValuePair<string, PvpStatsAggregate> x) => x.Value.Desertions);
					double totalGames = wins + losses + desertions + byes;
					if (!(totalGames <= 0.0))
					{
						double winRatio = (double)(wins + byes) / totalGames * 100.0;
						await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/pvp_winrate.txt", Math.Round(winRatio).ToString(CultureInfo.InvariantCulture) + "%");
					}
				}
			});
		}

		private async Task UpdateWallet()
		{
			if (!Gw2ApiManager.HasPermissions(new TokenPermission[2]
			{
				TokenPermission.Account,
				TokenPermission.Wallet
			}))
			{
				return;
			}
			await Gw2ApiManager.Gw2ApiClient.V2.Account.Wallet.GetAsync().ContinueWith((Func<Task<IApiV2ObjectList<AccountCurrency>>, Task>)async delegate(Task<IApiV2ObjectList<AccountCurrency>> task)
			{
				if (!task.IsFaulted)
				{
					int coins = task.Result.First((AccountCurrency x) => x.Id == 1).Value;
					await Task.Run(delegate
					{
						Gw2Util.GenerateCoinsImage(ModuleInstance.DirectoriesManager.GetFullDirectoryPath("stream_out") + "/wallet_coins.png", coins);
					});
					int karma = task.Result.First((AccountCurrency x) => x.Id == 2).Value;
					await Task.Run(delegate
					{
						Gw2Util.GenerateKarmaImage(ModuleInstance.DirectoriesManager.GetFullDirectoryPath("stream_out") + "/wallet_karma.png", karma);
					});
				}
			});
		}

		private unsafe async Task UpdateKillProofs()
		{
			await TaskUtil.GetJsonResponse<object>(string.Format("{0}{1}?lang={2}", "https://killproof.me/api/kp/", _accountName.Value, GameService.Overlay.UserLocale.Value)).ContinueWith((Func<Task<(bool, object)>, Task>)async delegate(Task<(bool, dynamic)> task)
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
								object awaiter = FileUtil.WriteAllTextAsync(killproofDir + "/legendary_divination.txt", ((dynamic)killProof).amount.ToString()).GetAwaiter();
								if (!(bool)((dynamic)awaiter).IsCompleted)
								{
									ICriticalNotifyCompletion awaiter2 = awaiter as ICriticalNotifyCompletion;
									if (awaiter2 == null)
									{
										INotifyCompletion awaiter3 = (INotifyCompletion)awaiter;
										asyncTaskMethodBuilder.AwaitOnCompleted(ref awaiter3, ref *(_003C_003CUpdateKillProofs_003Eb__96_0_003Ed*)/*Error near IL_041c: stateMachine*/);
									}
									else
									{
										asyncTaskMethodBuilder.AwaitUnsafeOnCompleted(ref awaiter2, ref *(_003C_003CUpdateKillProofs_003Eb__96_0_003Ed*)/*Error near IL_042f: stateMachine*/);
									}
									/*Error near IL_0438: leave MoveNext - await not detected correctly*/;
								}
								((dynamic)awaiter).GetResult();
								count++;
								break;
							}
							case 77302:
							{
								object awaiter = FileUtil.WriteAllTextAsync(killproofDir + "/legendary_insight.txt", ((dynamic)killProof).amount.ToString()).GetAwaiter();
								if (!(bool)((dynamic)awaiter).IsCompleted)
								{
									ICriticalNotifyCompletion awaiter2 = awaiter as ICriticalNotifyCompletion;
									if (awaiter2 == null)
									{
										INotifyCompletion awaiter3 = (INotifyCompletion)awaiter;
										asyncTaskMethodBuilder.AwaitOnCompleted(ref awaiter3, ref *(_003C_003CUpdateKillProofs_003Eb__96_0_003Ed*)/*Error near IL_06d7: stateMachine*/);
									}
									else
									{
										asyncTaskMethodBuilder.AwaitUnsafeOnCompleted(ref awaiter2, ref *(_003C_003CUpdateKillProofs_003Eb__96_0_003Ed*)/*Error near IL_06ea: stateMachine*/);
									}
									/*Error near IL_06f3: leave MoveNext - await not detected correctly*/;
								}
								((dynamic)awaiter).GetResult();
								count++;
								break;
							}
							case 94020:
							{
								object awaiter = FileUtil.WriteAllTextAsync(killproofDir + "/unstable_fractal_essence.txt", ((dynamic)killProof).amount.ToString()).GetAwaiter();
								if (!(bool)((dynamic)awaiter).IsCompleted)
								{
									ICriticalNotifyCompletion awaiter2 = awaiter as ICriticalNotifyCompletion;
									if (awaiter2 == null)
									{
										INotifyCompletion awaiter3 = (INotifyCompletion)awaiter;
										asyncTaskMethodBuilder.AwaitOnCompleted(ref awaiter3, ref *(_003C_003CUpdateKillProofs_003Eb__96_0_003Ed*)/*Error near IL_0992: stateMachine*/);
									}
									else
									{
										asyncTaskMethodBuilder.AwaitUnsafeOnCompleted(ref awaiter2, ref *(_003C_003CUpdateKillProofs_003Eb__96_0_003Ed*)/*Error near IL_09a5: stateMachine*/);
									}
									/*Error near IL_09ae: leave MoveNext - await not detected correctly*/;
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
