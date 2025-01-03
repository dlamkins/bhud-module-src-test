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
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework.Graphics;
using SL.ChatLinks.Integrations;
using SL.ChatLinks.Logging;
using SL.ChatLinks.Storage;
using SL.ChatLinks.UI;
using SL.ChatLinks.UI.Tabs.Items.Services;
using SL.Common;
using SL.Common.Controls.Items.Services;
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

		[ImportingConstructor]
		public Module([Import("ModuleParameters")] ModuleParameters parameters)
			: this(parameters)
		{
		}

		protected override void Initialize()
		{
			ServiceCollection serviceCollection = new ServiceCollection();
			serviceCollection.AddSingleton<ModuleParameters>(base.ModuleParameters);
			serviceCollection.AddSingleton<IViewsFactory, ViewsFactory>();
			serviceCollection.AddGw2Client();
			serviceCollection.AddDbContext<ChatLinksContext>(delegate(DbContextOptionsBuilder optionsBuilder)
			{
				string text = DatabaseLocation();
				SqliteConnection connection = new SqliteConnection("Data Source=" + text);
				Levenshtein.RegisterLevenshteinFunction(connection);
				optionsBuilder.UseSqlite(connection);
			}, ServiceLifetime.Transient, ServiceLifetime.Transient);
			serviceCollection.AddTransient<ItemSeeder>();
			serviceCollection.AddTransient<MainIcon>();
			serviceCollection.AddTransient<MainWindow>();
			serviceCollection.AddTransient<ItemSearch>();
			serviceCollection.AddHttpClient<ItemIcons>();
			serviceCollection.AddLogging(delegate(ILoggingBuilder builder)
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
			ServiceProvider serviceProvider = (_serviceProvider = (ServiceProvider?)(ServiceLocator.ServiceProvider = serviceCollection.BuildServiceProvider()));
			SQLite3Provider_dynamic_cdecl.Setup("sliekens.e_sqlite3", new ModuleGetFunctionPointer("sliekens.e_sqlite3"));
			raw.SetProvider(new SQLite3Provider_dynamic_cdecl());
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
			ServiceLocator.ServiceProvider = null;
		}
	}
}
