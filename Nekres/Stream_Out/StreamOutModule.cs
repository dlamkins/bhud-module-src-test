using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
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
			_allExportServices = new List<ExportService>
			{
				new CharacterService(),
				new ClientService(),
				new GuildService(),
				new KillProofService(),
				new MapService(),
				new PvpService(),
				new WalletService(),
				new WvwService()
			};
			Gw2ApiManager.add_SubtokenUpdated((EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>>)SubTokenUpdated);
		}

		private void SubTokenUpdated(object o, ValueEventArgs<IEnumerable<TokenPermission>> e)
		{
			HasSubToken = true;
		}

		protected override async Task LoadAsync()
		{
			foreach (ExportService allExportService in _allExportServices)
			{
				await allExportService.Initialize();
			}
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
			foreach (ExportService allExportService in _allExportServices)
			{
				await allExportService.DoUpdate();
			}
		}

		protected override void Unload()
		{
			Gw2ApiManager.remove_SubtokenUpdated((EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>>)SubTokenUpdated);
			foreach (ExportService allExportService in _allExportServices)
			{
				allExportService?.Dispose();
			}
			Instance = null;
		}
	}
}
