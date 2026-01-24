using System;
using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Modules;
using Blish_HUD.Settings;
using GuildWars2;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SL.Adapters;
using SL.Adapters.Logging;
using SL.ChatLinks.Integrations;
using SL.ChatLinks.Storage;
using SL.ChatLinks.UI;
using SL.ChatLinks.UI.Tabs.Achievements;
using SL.ChatLinks.UI.Tabs.Achievements.Tooltips;
using SL.ChatLinks.UI.Tabs.Items;
using SL.ChatLinks.UI.Tabs.Items.Collections;
using SL.ChatLinks.UI.Tabs.Items.Tooltips;
using SL.ChatLinks.UI.Tabs.Items.Upgrades;
using SL.Common;
using SL.Common.Progression;

namespace SL.ChatLinks
{
	[Export(typeof(Module))]
	public class ChatLinksModule : Module
	{
		private IEventAggregator? _eventAggregator;

		private ServiceProvider? _serviceProvider;

		private ModuleSettings? _moduleSettings;

		private MumbleListener? _listener;

		private readonly Clock _clock = new Clock();

		[ImportingConstructor]
		public ChatLinksModule([Import("ModuleParameters")] ModuleParameters parameters)
			: this(parameters)
		{
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			_moduleSettings = new ModuleSettings(settings);
		}

		protected override void Initialize()
		{
			if (_moduleSettings == null)
			{
				throw new InvalidOperationException("Module settings not defined.");
			}
			ServiceCollection services = new ServiceCollection();
			services.AddSingleton<ModuleParameters>(base.ModuleParameters);
			services.AddSingleton(_moduleSettings);
			services.ConfigureOptions(_moduleSettings);
			((IServiceCollection)services).AddSingleton((IOptionsChangeTokenSource<ChatLinkOptions>)_moduleSettings);
			services.AddDatabase(delegate(DatabaseOptions options)
			{
				options.Directory = base.ModuleParameters.get_DirectoriesManager().GetFullDirectoryPath("chat-links-data");
			});
			services.AddSingleton<DatabaseSeeder>();
			services.AddSingleton<ILocale, OverlayLocale>();
			services.AddLocalization(delegate(LocalizationOptions options)
			{
				options.ResourcesPath = "Resources";
			});
			services.AddGw2Client();
			services.AddStaticDataClient();
			services.AddSingleton<ITokenProvider, Gw2SharpTokenProvider>();
			services.AddSingleton<IEventAggregator, DefaultEventAggregator>();
			services.AddTransient<IClipBoard, WpfClipboard>();
			services.AddSingleton<CurrentAccount>();
			services.AddSingleton<AchievementsProgress>();
			services.AddSingleton<AccountBank>();
			services.AddSingleton<AccountMaterialStorage>();
			services.AddSingleton<UnlockedDyes>();
			services.AddSingleton<UnlockedFinishers>();
			services.AddSingleton<UnlockedGliderSkins>();
			services.AddSingleton<UnlockedJadeBotSkins>();
			services.AddSingleton<UnlockedMailCarriers>();
			services.AddSingleton<UnlockedMiniatures>();
			services.AddSingleton<UnlockedMountSkins>();
			services.AddSingleton<UnlockedMistChampionSkins>();
			services.AddSingleton<UnlockedNovelties>();
			services.AddSingleton<UnlockedOutfits>();
			services.AddSingleton<UnlockedRecipes>();
			services.AddSingleton<UnlockedWardrobe>();
			services.AddHttpClient<IconsService>();
			services.AddSingleton<IconsCache>();
			services.AddMemoryCache();
			services.AddTransient(delegate
			{
				string currentMumbleMapName = GameService.Gw2Mumble.get_CurrentMumbleMapName();
				TimeSpan refreshInterval = default(TimeSpan);
				return GameLink.Open(in refreshInterval, currentMumbleMapName);
			});
			services.AddTransient<MumbleListener>();
			services.AddLogging(delegate(ILoggingBuilder builder)
			{
				builder.Services.AddSingleton<ILoggerProvider, LoggingAdapterProvider<ChatLinksModule>>();
				if (ApplicationSettings.get_Instance().get_DebugEnabled() || GameService.Debug.get_EnableDebugLogging().get_Value())
				{
					builder.SetMinimumLevel(LogLevel.Debug);
					builder.AddFilter("System", LogLevel.Information);
					builder.AddFilter("Microsoft", LogLevel.Information);
					builder.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);
				}
				else
				{
					builder.AddFilter("System", LogLevel.Warning);
					builder.AddFilter("Microsoft", LogLevel.Warning);
					builder.AddFilter("Microsoft.EntityFrameworkCore.Query", LogLevel.Critical);
				}
			});
			services.AddTransient<MainIcon>();
			services.AddTransient<MainIconViewModel>();
			services.AddTransient<MainWindow>();
			services.AddTransient<MainWindowViewModel>();
			services.AddFactoryDelegate<ItemsTabViewModel.Factory>();
			services.AddFactoryDelegate<ItemsTabViewModel.Factory>();
			services.AddFactoryDelegate<ItemsListViewModel.Factory>();
			services.AddFactoryDelegate<ItemTooltipViewModel.Factory>();
			services.AddFactoryDelegate<ChatLinkEditorViewModel.Factory>();
			services.AddFactoryDelegate<UpgradeEditorViewModel.Factory>();
			services.AddFactoryDelegate<UpgradeSelectorViewModel.Factory>();
			services.AddFactoryDelegate<UpgradeSlotViewModel.Factory>();
			services.AddTransient<ItemSearch>();
			services.AddSingleton<Customizer>();
			services.AddFactoryDelegate<AchievementsTabViewModel.Factory>();
			services.AddFactoryDelegate<AchievementTileViewModel.Factory>();
			services.AddFactoryDelegate<AchievementTooltipViewModel.Factory>();
			_serviceProvider = services.BuildServiceProvider();
			_eventAggregator = _serviceProvider.GetRequiredService<IEventAggregator>();
			Sqlite3Setup.Run();
		}

		protected override async Task LoadAsync()
		{
			ILogger<ChatLinksModule> logger = _serviceProvider.GetRequiredService<ILogger<ChatLinksModule>>();
			ILocale locale = _serviceProvider.GetRequiredService<ILocale>();
			DatabaseSeeder seeder = _serviceProvider.GetRequiredService<DatabaseSeeder>();
			CurrentAccount account = _serviceProvider.GetRequiredService<CurrentAccount>();
			try
			{
				await seeder.Migrate(locale.Current).ConfigureAwait(continueOnCapturedContext: false);
			}
			catch (Exception reason3)
			{
				logger.LogWarning(reason3, "Database migration failed, starting with potentially invalid database schema.");
			}
			_serviceProvider.GetRequiredService<MainIcon>();
			_serviceProvider.GetRequiredService<MainWindow>();
			try
			{
				await seeder.Sync(locale.Current, CancellationToken.None).ConfigureAwait(continueOnCapturedContext: false);
				await seeder.Optimize(locale.Current, CancellationToken.None).ConfigureAwait(continueOnCapturedContext: false);
			}
			catch (Exception reason2)
			{
				logger.LogWarning(reason2, "Database sync failed, starting with potentially stale data.");
			}
			try
			{
				await account.Validate(force: false, CancellationToken.None).ConfigureAwait(continueOnCapturedContext: false);
			}
			catch (Exception reason)
			{
				logger.LogWarning(reason, "One or more account details could not be validated.");
			}
			_clock.HourStarted += new EventHandler(OnHourStarted);
			_listener = _serviceProvider.GetRequiredService<MumbleListener>();
			_listener!.Start();
		}

		private void OnHourStarted(object sender, EventArgs e)
		{
			_eventAggregator?.Publish(new HourStarted());
		}

		protected override void Unload()
		{
			_eventAggregator?.Publish(new ModuleUnloading());
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_clock.Dispose();
				_serviceProvider?.Dispose();
				_moduleSettings?.Dispose();
				_listener?.Dispose();
			}
			((Module)this).Dispose(disposing);
		}
	}
}
