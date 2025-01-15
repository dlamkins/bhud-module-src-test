using System;
using System.ComponentModel.Composition;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Settings;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Xna.Framework.Graphics;
using SL.ChatLinks.Integrations;
using SL.ChatLinks.Logging;
using SL.ChatLinks.Storage;
using SL.ChatLinks.UI;
using SL.ChatLinks.UI.Tabs.Items;
using SL.ChatLinks.UI.Tabs.Items.Collections;
using SL.ChatLinks.UI.Tabs.Items.Tooltips;
using SL.ChatLinks.UI.Tabs.Items.Upgrades;
using SL.Common;
using SQLitePCL;

namespace SL.ChatLinks
{
	[Export(typeof(Module))]
	public class Module : Module
	{
		private MainIcon? _cornerIcon;

		private MainWindow? _mainWindow;

		private ServiceProvider? _serviceProvider;

		private ContextMenuStripItem? _syncButton;

		private SettingEntry<bool>? _raiseStackSize;

		private SettingEntry<bool>? _bananaMode;

		[ImportingConstructor]
		public Module([Import("ModuleParameters")] ModuleParameters parameters)
			: this(parameters)
		{
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			_raiseStackSize = settings.DefineSetting<bool>("RaiseStackSize", false, (Func<string>)(() => "Raise the maximum item stack size from 250 to 255"), (Func<string>)(() => "When enabled, you can generate chat links with stacks of 255 items."));
			_bananaMode = settings.DefineSetting<bool>("BananaMode", false, (Func<string>)(() => "Banana of Imagination-mode"), (Func<string>)(() => "When enabled, you can add an upgrade component to any item."));
		}

		protected override void Initialize()
		{
			ServiceCollection services = new ServiceCollection();
			services.AddSingleton<SettingCollection>(base.ModuleParameters.get_SettingsManager().get_ModuleSettings());
			services.Configure(delegate(ChatLinkOptions options)
			{
				options.RaiseStackSize = _raiseStackSize!.get_Value();
				options.BananaMode = _bananaMode!.get_Value();
			});
			services.AddSingleton<IOptionsChangeTokenSource<ChatLinkOptions>, ChatLinkOptionsAdapter>();
			services.AddGw2Client();
			services.AddDbContext<ChatLinksContext>(delegate(DbContextOptionsBuilder optionsBuilder)
			{
				string text = DatabaseLocation();
				SqliteConnection connection = new SqliteConnection("Data Source=" + text);
				Levenshtein.RegisterLevenshteinFunction(connection);
				optionsBuilder.UseSqlite(connection);
			}, ServiceLifetime.Transient, ServiceLifetime.Transient);
			services.AddTransient<ItemSeeder>();
			services.AddSingleton<IEventAggregator, DefaultEventAggregator>();
			services.AddTransient<MainIcon>();
			services.AddTransient<MainIconViewModel>();
			services.AddTransient<MainWindow>();
			services.AddTransient<MainWindowViewModel>();
			services.AddTransient<ItemsTabView>();
			services.AddTransient<ItemsTabViewFactory>();
			services.AddTransient<ItemsTabViewModel>();
			services.AddTransient<ItemsTabViewModelFactory>();
			services.AddTransient<ItemsListViewModelFactory>();
			services.AddTransient<ItemTooltipViewModelFactory>();
			services.AddTransient<ChatLinkEditorViewModelFactory>();
			services.AddTransient<UpgradeEditorViewModelFactory>();
			services.AddTransient<UpgradeSelectorViewModelFactory>();
			services.AddTransient<ItemSearch>();
			services.AddSingleton<Customizer>();
			services.AddHttpClient<ItemIcons>();
			services.AddTransient<IClipBoard, WpfClipboard>();
			services.AddLogging(delegate(ILoggingBuilder builder)
			{
				builder.Services.AddSingleton<ILoggerProvider, LoggingAdapterProvider<Module>>();
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
			_serviceProvider = services.BuildServiceProvider();
			SetupSqlite3();
		}

		protected override async Task LoadAsync()
		{
			ILogger<Module> logger = Resolve<ILogger<Module>>();
			try
			{
				await FirstTimeSetup();
			}
			catch (Exception reason)
			{
				logger.LogError(reason, "First-time setup failed.");
			}
			await using ChatLinksContext context = Resolve<ChatLinksContext>();
			await context.Database.MigrateAsync();
			_cornerIcon = Resolve<MainIcon>();
			_mainWindow = Resolve<MainWindow>();
			((Control)_cornerIcon).add_Click((EventHandler<MouseEventArgs>)CornerIcon_Click);
			ItemSeeder seeder = Resolve<ItemSeeder>();
			Progress<string> progress = new Progress<string>(delegate(string report)
			{
				((CornerIcon)_cornerIcon).set_LoadingMessage(report);
			});
			if (!Program.get_IsMainThread())
			{
				await seeder.Seed(progress, CancellationToken.None);
			}
			else
			{
				await (await Task.Factory.StartNew((Func<Task>)async delegate
				{
					await seeder.Seed(progress, CancellationToken.None);
				}, TaskCreationOptions.LongRunning));
			}
			((CornerIcon)_cornerIcon).set_LoadingMessage((string)null);
			((Control)_cornerIcon).set_BasicTooltipText((string)null);
			((Control)_cornerIcon).set_Menu(new ContextMenuStrip());
			_syncButton = ((Control)_cornerIcon).get_Menu().AddMenuItem("Sync database");
			((Control)_syncButton).add_Click((EventHandler<MouseEventArgs>)SyncClicked);
		}

		private static void SetupSqlite3()
		{
			SQLite3Provider_dynamic_cdecl.Setup("e_sqlite3", new ModuleGetFunctionPointer("sliekens.e_sqlite3"));
			raw.SetProvider(new SQLite3Provider_dynamic_cdecl());
		}

		private async Task FirstTimeSetup()
		{
			string databaseLocation = DatabaseLocation();
			FileInfo fileInfo = new FileInfo(databaseLocation);
			if (fileInfo == null || (fileInfo.Exists && fileInfo.Length != 0L) || 1 == 0)
			{
				return;
			}
			using Stream seed = base.ModuleParameters.get_ContentsManager().GetFileStream("data.zip");
			using ZipArchive unzip = new ZipArchive(seed, ZipArchiveMode.Read);
			ZipArchiveEntry data = unzip.GetEntry("data.db");
			if (data == null)
			{
				return;
			}
			using Stream dataStream = data.Open();
			using FileStream fileStream = File.Create(databaseLocation);
			await dataStream.CopyToAsync(fileStream);
		}

		private T Resolve<T>() where T : notnull
		{
			return _serviceProvider.GetRequiredService<T>();
		}

		private string DatabaseLocation()
		{
			return Path.Combine(base.ModuleParameters.get_DirectoriesManager().GetFullDirectoryPath("chat-links-data"), "data.db");
		}

		private async void SyncClicked(object sender, MouseEventArgs e)
		{
			ILogger<Module> logger = Resolve<ILogger<Module>>();
			try
			{
				((Control)_syncButton).set_Enabled(false);
				ItemSeeder seeder = Resolve<ItemSeeder>();
				Progress<string> progress = new Progress<string>(delegate(string report)
				{
					((CornerIcon)_cornerIcon).set_LoadingMessage(report);
				});
				if (!Program.get_IsMainThread())
				{
					await seeder.Seed(progress, CancellationToken.None);
				}
				else
				{
					await (await Task.Factory.StartNew((Func<Task>)async delegate
					{
						await seeder.Seed(progress, CancellationToken.None);
					}, TaskCreationOptions.LongRunning));
				}
				ScreenNotification.ShowNotification("Everything is up-to-date.", (NotificationType)5, (Texture2D)null, 4);
			}
			catch (Exception reason)
			{
				logger.LogError(reason, "Sync failed");
				ScreenNotification.ShowNotification("Sync failed, try again later.", (NotificationType)1, (Texture2D)null, 4);
			}
			finally
			{
				((CornerIcon)_cornerIcon).set_LoadingMessage((string)null);
				((Control)_cornerIcon).set_BasicTooltipText((string)null);
				((Control)_syncButton).set_Enabled(true);
			}
		}

		private void CornerIcon_Click(object sender, EventArgs e)
		{
			MainWindow? mainWindow = _mainWindow;
			if (mainWindow != null)
			{
				((WindowBase2)mainWindow).ToggleWindow();
			}
		}

		protected override void Unload()
		{
			MainIcon? cornerIcon = _cornerIcon;
			if (cornerIcon != null)
			{
				((Control)cornerIcon).Dispose();
			}
			MainWindow? mainWindow = _mainWindow;
			if (mainWindow != null)
			{
				((Control)mainWindow).Dispose();
			}
			_serviceProvider?.Dispose();
		}
	}
}
