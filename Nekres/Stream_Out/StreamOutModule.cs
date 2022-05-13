using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Nekres.Stream_Out.Core.Services;
using Nekres.Stream_Out.UI.Models;
using Nekres.Stream_Out.UI.Views;

namespace Nekres.Stream_Out
{
	[Export(typeof(Module))]
	public class StreamOutModule : Module
	{
		internal enum UnicodeSigning
		{
			None,
			Prefixed,
			Suffixed
		}

		internal static readonly Logger Logger = Logger.GetLogger<StreamOutModule>();

		internal static StreamOutModule Instance;

		internal SettingEntry<bool> OnlyLastDigitSettingEntry;

		internal SettingEntry<UnicodeSigning> AddUnicodeSymbols;

		internal SettingEntry<bool> UseCatmanderTag;

		internal SettingEntry<bool> ExportClientInfo;

		internal SettingEntry<bool> ExportMapInfo;

		internal SettingEntry<bool> ExportWalletInfo;

		internal SettingEntry<bool> ExportPvpInfo;

		internal SettingEntry<bool> ExportWvwInfo;

		internal SettingEntry<bool> ExportGuildInfo;

		internal SettingEntry<bool> ExportCharacterInfo;

		internal SettingEntry<bool> ExportKillProofs;

		internal SettingEntry<DateTime> ResetTimeWvW;

		internal SettingEntry<DateTime> ResetTimeDaily;

		internal SettingEntry<int> SessionKillsWvW;

		internal SettingEntry<int> SessionKillsWvwDaily;

		internal SettingEntry<int> SessionKillsPvP;

		internal SettingEntry<int> TotalKillsAtResetWvW;

		internal SettingEntry<int> TotalKillsAtResetPvP;

		internal SettingEntry<int> TotalDeathsAtResetWvW;

		internal SettingEntry<int> TotalDeathsAtResetDaily;

		internal SettingEntry<int> SessionDeathsWvW;

		internal SettingEntry<int> SessionDeathsDaily;

		internal SettingEntry<Guid> AccountGuid;

		internal SettingEntry<string> AccountName;

		internal string WebApiDown = "Unable to connect to the official Guild Wars 2 WebApi. Check if the WebApi is down for maintenance.";

		private List<ExportService> _allExportServices;

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		internal Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		internal bool HasSubToken { get; private set; }

		[ImportingConstructor]
		public StreamOutModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
			Instance = this;
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			OnlyLastDigitSettingEntry = settings.DefineSetting<bool>("OnlyLastDigits", true, (Func<string>)(() => "Only Output Last Digits of Server Address"), (Func<string>)(() => "Only outputs the last digits of the server address you are currently connected to.\nThis is the address shown when entering \"/ip\" in chat."));
			AddUnicodeSymbols = settings.DefineSetting<UnicodeSigning>("UnicodeSymbols", UnicodeSigning.Suffixed, (Func<string>)(() => "Numeric Value Signing"), (Func<string>)(() => "The way numeric values should be signed with unicode symbols."));
			UseCatmanderTag = settings.DefineSetting<bool>("CatmanderTag", false, (Func<string>)(() => "Use Catmander Tag"), (Func<string>)(() => "Replaces the Commander icon with the Catmander icon if you tag up as Commander in-game."));
			SettingCollection toggles = settings.AddSubCollection("Export Toggles", true, false);
			ExportClientInfo = toggles.DefineSetting<bool>("clientInfo", true, (Func<string>)(() => "Export Client Info"), (Func<string>)(() => "Client info such as server address."));
			ExportCharacterInfo = toggles.DefineSetting<bool>("characterInfo", true, (Func<string>)(() => "Export Character Info"), (Func<string>)(() => "Character info such as name, deaths, profession and commander tag."));
			ExportGuildInfo = toggles.DefineSetting<bool>("guildInfo", true, (Func<string>)(() => "Export Guild Info"), (Func<string>)(() => "Guild info such as guild name, tag, emblem and part of the Message of the Day that is surrounded by [public]<text>[/public]."));
			ExportKillProofs = toggles.DefineSetting<bool>("killsProofs", true, (Func<string>)(() => "Export Kill Proofs"), (Func<string>)(() => "Kill Proofs such as Legendary Divination, Legendary Insight and Unstable Fractal Essence.\nFor more info visit: www.killproof.me"));
			ExportMapInfo = toggles.DefineSetting<bool>("mapInfo", true, (Func<string>)(() => "Export Map Info"), (Func<string>)(() => "Map info such as map name and map type."));
			ExportPvpInfo = toggles.DefineSetting<bool>("pvpInfo", true, (Func<string>)(() => "Export PvP Info"), (Func<string>)(() => "PvP info such as rank, tier, win rate and kills."));
			ExportWvwInfo = toggles.DefineSetting<bool>("wvwInfo", true, (Func<string>)(() => "Export WvW info"), (Func<string>)(() => "WvW info such as rank and kills"));
			ExportWalletInfo = toggles.DefineSetting<bool>("walletInfo", true, (Func<string>)(() => "Export Wallet Info"), (Func<string>)(() => "Currencies such as coins and karma."));
			SettingCollection cache = settings.AddSubCollection("CachedValues", false, false);
			AccountGuid = cache.DefineSetting<Guid>("AccountGuid", Guid.Empty, (Func<string>)null, (Func<string>)null);
			AccountName = cache.DefineSetting<string>("AccountName", string.Empty, (Func<string>)null, (Func<string>)null);
			ResetTimeWvW = cache.DefineSetting<DateTime>("ResetTimeWvW", DateTime.UtcNow, (Func<string>)null, (Func<string>)null);
			ResetTimeDaily = cache.DefineSetting<DateTime>("ResetTimeDaily", DateTime.UtcNow, (Func<string>)null, (Func<string>)null);
			SessionKillsWvW = cache.DefineSetting<int>("SessionKillsWvW", 0, (Func<string>)null, (Func<string>)null);
			SessionKillsWvwDaily = cache.DefineSetting<int>("SessionsKillsWvWDaily", 0, (Func<string>)null, (Func<string>)null);
			SessionKillsPvP = cache.DefineSetting<int>("SessionKillsPvP", 0, (Func<string>)null, (Func<string>)null);
			SessionDeathsWvW = cache.DefineSetting<int>("SessionDeathsWvW", 0, (Func<string>)null, (Func<string>)null);
			SessionDeathsDaily = cache.DefineSetting<int>("SessionDeathsDaily", 0, (Func<string>)null, (Func<string>)null);
			TotalKillsAtResetWvW = cache.DefineSetting<int>("TotalKillsAtResetWvW", 0, (Func<string>)null, (Func<string>)null);
			TotalKillsAtResetPvP = cache.DefineSetting<int>("TotalKillsAtResetPvP", 0, (Func<string>)null, (Func<string>)null);
			TotalDeathsAtResetWvW = cache.DefineSetting<int>("TotalDeathsAtResetWvW", 0, (Func<string>)null, (Func<string>)null);
			TotalDeathsAtResetDaily = cache.DefineSetting<int>("TotalDeathsAtResetDaily", 0, (Func<string>)null, (Func<string>)null);
		}

		public override IView GetSettingsView()
		{
			return (IView)(object)new CustomSettingsView(new CustomSettingsModel(SettingsManager.get_ModuleSettings()));
		}

		protected override void Initialize()
		{
			_allExportServices = new List<ExportService>();
			ExportCharacterInfo.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)ToggleService<CharacterService>);
			ExportClientInfo.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)ToggleService<ClientService>);
			ExportGuildInfo.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)ToggleService<GuildService>);
			ExportKillProofs.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)ToggleService<KillProofService>);
			ExportMapInfo.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)ToggleService<MapService>);
			ExportPvpInfo.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)ToggleService<PvpService>);
			ExportWvwInfo.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)ToggleService<WvwService>);
			ExportWalletInfo.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)ToggleService<WalletService>);
			Gw2ApiManager.add_SubtokenUpdated((EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>>)SubTokenUpdated);
		}

		private async void ToggleService<TType>(object o, ValueChangedEventArgs<bool> e) where TType : ExportService
		{
			await ToggleService<TType>(e.get_NewValue());
		}

		private async Task ToggleService<TType>(bool enabled) where TType : ExportService
		{
			ExportService service2 = _allExportServices.FirstOrDefault((ExportService x) => x.GetType() == typeof(TType));
			if (enabled && service2 == null)
			{
				service2 = (TType)Activator.CreateInstance(typeof(TType));
				_allExportServices.Add(service2);
				await service2.Initialize();
			}
			else if (!enabled && service2 != null)
			{
				_allExportServices.Remove(service2);
				await service2.Clear();
				service2.Dispose();
			}
		}

		private void SubTokenUpdated(object o, ValueEventArgs<IEnumerable<TokenPermission>> e)
		{
			HasSubToken = true;
		}

		protected override async Task LoadAsync()
		{
			await ToggleService<CharacterService>(ExportCharacterInfo.get_Value());
			await ToggleService<ClientService>(ExportClientInfo.get_Value());
			await ToggleService<GuildService>(ExportGuildInfo.get_Value());
			await ToggleService<KillProofService>(ExportKillProofs.get_Value());
			await ToggleService<MapService>(ExportMapInfo.get_Value());
			await ToggleService<PvpService>(ExportPvpInfo.get_Value());
			await ToggleService<WvwService>(ExportWvwInfo.get_Value());
			await ToggleService<WalletService>(ExportWalletInfo.get_Value());
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			((Module)this).OnModuleLoaded(e);
		}

		protected override async void Update(GameTime gameTime)
		{
			if (!HasSubToken)
			{
				return;
			}
			try
			{
				foreach (ExportService service in _allExportServices.ToList())
				{
					if (service != null)
					{
						await service.DoUpdate();
					}
				}
			}
			catch (Exception ex) when (ex is InvalidOperationException || ex is NullReferenceException)
			{
			}
		}

		protected override void Unload()
		{
			ExportCharacterInfo.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)ToggleService<CharacterService>);
			ExportClientInfo.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)ToggleService<ClientService>);
			ExportGuildInfo.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)ToggleService<GuildService>);
			ExportKillProofs.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)ToggleService<KillProofService>);
			ExportMapInfo.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)ToggleService<MapService>);
			ExportPvpInfo.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)ToggleService<PvpService>);
			ExportWvwInfo.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)ToggleService<WvwService>);
			ExportWalletInfo.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)ToggleService<WalletService>);
			Gw2ApiManager.remove_SubtokenUpdated((EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>>)SubTokenUpdated);
			foreach (ExportService allExportService in _allExportServices)
			{
				allExportService?.Dispose();
			}
			Instance = null;
		}
	}
}
