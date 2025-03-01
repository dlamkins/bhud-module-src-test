using System;
using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Modules;
using Blish_HUD.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SL.Adapters;
using SL.ChatLinks.Integrations;
using SL.ChatLinks.Logging;
using SL.ChatLinks.Storage;
using SL.ChatLinks.UI;
using SL.ChatLinks.UI.Tabs.Achievements;
using SL.ChatLinks.UI.Tabs.Achievements.Tooltips;
using SL.ChatLinks.UI.Tabs.Items;
using SL.ChatLinks.UI.Tabs.Items.Collections;
using SL.ChatLinks.UI.Tabs.Items.Tooltips;
using SL.ChatLinks.UI.Tabs.Items.Upgrades;
using SL.Common;
using SQLitePCL;

namespace SL.ChatLinks
{
	[Export(typeof(Module))]
	public class ChatLinksModule : Module
	{
		private IEventAggregator? _eventAggregator;

		private ServiceProvider? _serviceProvider;

		private ModuleSettings? _moduleSettings;

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
			services.AddTransient<ItemsTabViewModelFactory>();
			services.AddTransient<ItemsListViewModelFactory>();
			services.AddTransient<ItemTooltipViewModelFactory>();
			services.AddTransient<ChatLinkEditorViewModelFactory>();
			services.AddTransient<UpgradeEditorViewModelFactory>();
			services.AddTransient<UpgradeSelectorViewModelFactory>();
			services.AddTransient<ItemSearch>();
			services.AddSingleton<Customizer>();
			services.AddSingleton<AccountUnlocks>();
			services.AddHttpClient<ItemIcons>();
			services.AddTransient<AchievementsTabViewModelFactory>();
			services.AddTransient<AchievementTileViewModelFactory>();
			services.AddTransient<AchievementTooltipViewModelFactory>();
			_serviceProvider = services.BuildServiceProvider();
			_eventAggregator = _serviceProvider.GetRequiredService<IEventAggregator>();
			SetupSqlite3();
		}

		protected override async Task LoadAsync()
		{
			ILogger<ChatLinksModule> logger = _serviceProvider.GetRequiredService<ILogger<ChatLinksModule>>();
			ILocale locale = _serviceProvider.GetRequiredService<ILocale>();
			DatabaseSeeder seeder = _serviceProvider.GetRequiredService<DatabaseSeeder>();
			try
			{
				await seeder.Migrate(locale.Current).ConfigureAwait(continueOnCapturedContext: false);
			}
			catch (Exception reason2)
			{
				logger.LogWarning(reason2, "Database migration failed, starting with potentially invalid database schema.");
			}
			_serviceProvider.GetRequiredService<MainIcon>();
			_serviceProvider.GetRequiredService<MainWindow>();
			try
			{
				await seeder.Sync(locale.Current, CancellationToken.None).ConfigureAwait(continueOnCapturedContext: false);
				await seeder.Optimize(locale.Current, CancellationToken.None).ConfigureAwait(continueOnCapturedContext: false);
			}
			catch (Exception reason)
			{
				logger.LogWarning(reason, "Database sync failed, starting with potentially stale data.");
			}
			_clock.HourStarted += new EventHandler(OnHourStarted);
		}

		private void OnHourStarted(object sender, EventArgs e)
		{
			_eventAggregator?.Publish(new HourStarted());
		}

		private static void SetupSqlite3()
		{
			SQLite3Provider_dynamic_cdecl.Setup("e_sqlite3", new ModuleGetFunctionPointer("sliekens.e_sqlite3"));
			raw.SetProvider(new SQLite3Provider_dynamic_cdecl());
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
			}
			((Module)this).Dispose(disposing);
		}
	}
}
