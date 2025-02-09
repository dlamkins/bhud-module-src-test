using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Microsoft.Xna.Framework.Graphics;
using SL.ChatLinks.Storage;
using SL.Common;
using SL.Common.ModelBinding;

namespace SL.ChatLinks.UI
{
	public class MainIconViewModel : ViewModel
	{
		[CompilerGenerated]
		private ILogger<MainIconViewModel> _003Clogger_003EP;

		[CompilerGenerated]
		private IStringLocalizer<MainIcon> _003Clocalizer_003EP;

		[CompilerGenerated]
		private IEventAggregator _003CeventAggregator_003EP;

		[CompilerGenerated]
		private ILocale _003Clocale_003EP;

		[CompilerGenerated]
		private DatabaseSeeder _003Cseeder_003EP;

		[CompilerGenerated]
		private ModuleSettings _003Csettings_003EP;

		private string? _loadingMessage;

		private string? _tooltipText;

		private bool _raiseStackSize;

		private bool _bananaMode;

		public string Name => (string)_003Clocalizer_003EP["Name"];

		public string KoFiLabel => (string)_003Clocalizer_003EP["Buy me a coffee"];

		public string SyncLabel => (string)_003Clocalizer_003EP["Sync database"];

		public string BananaModeLabel => (string)_003Clocalizer_003EP["Banana of Imagination-mode"];

		public string RaiseStackSizeLabel => (string)_003Clocalizer_003EP["Raise stack size limit"];

		public AsyncTexture2D Texture => AsyncTexture2D.FromAssetId(155156);

		public AsyncTexture2D HoverTexture => AsyncTexture2D.FromAssetId(155157);

		public int Priority => 745727698;

		public string? LoadingMessage
		{
			get
			{
				return _loadingMessage;
			}
			set
			{
				SetField(ref _loadingMessage, value, "LoadingMessage");
			}
		}

		public string? TooltipText
		{
			get
			{
				return _tooltipText;
			}
			set
			{
				SetField(ref _tooltipText, value, "TooltipText");
			}
		}

		public bool RaiseStackSize
		{
			get
			{
				return _raiseStackSize;
			}
			set
			{
				if (SetField(ref _raiseStackSize, value, "RaiseStackSize"))
				{
					_003Csettings_003EP.RaiseStackSize = value;
				}
			}
		}

		public bool BananaMode
		{
			get
			{
				return _bananaMode;
			}
			set
			{
				if (SetField(ref _bananaMode, value, "BananaMode"))
				{
					_003Csettings_003EP.BananaMode = value;
				}
			}
		}

		public AsyncRelayCommand ClickCommand => new AsyncRelayCommand(async delegate
		{
			await _003CeventAggregator_003EP.PublishAsync(new MainIconClicked(), CancellationToken.None);
		});

		public AsyncRelayCommand SyncCommand => new AsyncRelayCommand(async delegate
		{
			_ = 1;
			try
			{
				await (await Task.Factory.StartNew((Func<Task>)async delegate
				{
					await _003Cseeder_003EP.Sync(_003Clocale_003EP.Current, CancellationToken.None);
				}, TaskCreationOptions.LongRunning));
				ScreenNotification.ShowNotification((string)_003Clocalizer_003EP["Chat Links database is up-to-date"], (NotificationType)5, (Texture2D)null, 4);
			}
			catch (Exception reason)
			{
				_003Clogger_003EP.LogError(reason, "Sync failed");
				ScreenNotification.ShowNotification((string)_003Clocalizer_003EP["Sync failed"], (NotificationType)1, (Texture2D)null, 4);
			}
		}, () => string.IsNullOrEmpty(LoadingMessage), delegate(EventHandler handler)
		{
			DatabaseUpdated += handler;
		}, delegate(EventHandler handler)
		{
			DatabaseUpdated -= handler;
		});

		public RelayCommand KoFiCommand => new RelayCommand(delegate
		{
			Process.Start("https://ko-fi.com/sliekens");
		});

		private event EventHandler? DatabaseUpdated;

		public MainIconViewModel(ILogger<MainIconViewModel> logger, IStringLocalizer<MainIcon> localizer, IEventAggregator eventAggregator, ILocale locale, DatabaseSeeder seeder, ModuleSettings settings)
		{
			_003Clogger_003EP = logger;
			_003Clocalizer_003EP = localizer;
			_003CeventAggregator_003EP = eventAggregator;
			_003Clocale_003EP = locale;
			_003Cseeder_003EP = seeder;
			_003Csettings_003EP = settings;
			_raiseStackSize = _003Csettings_003EP.RaiseStackSize;
			_bananaMode = _003Csettings_003EP.BananaMode;
			base._002Ector();
		}

		public void Initialize()
		{
			_003CeventAggregator_003EP.Subscribe(new Action<DatabaseSyncProgress>(OnDatabaseSyncProgress));
			_003CeventAggregator_003EP.Subscribe(new Action<DatabaseSyncCompleted>(OnDatabaseSyncCompleted));
			_003CeventAggregator_003EP.Subscribe(new Action<LocaleChanged>(OnLocaleChanged));
			_003CeventAggregator_003EP.Subscribe(new Action<ModuleUnloading>(OnModuleUnloading));
			ChangeToken.OnChange(_003Csettings_003EP.GetChangeToken, delegate(ModuleSettings moduleSettings)
			{
				BananaMode = moduleSettings.BananaMode;
				RaiseStackSize = moduleSettings.RaiseStackSize;
			}, _003Csettings_003EP);
		}

		private void OnModuleUnloading(ModuleUnloading unloading)
		{
			_003CeventAggregator_003EP.Unsubscribe<DatabaseSyncProgress>(new Action<DatabaseSyncProgress>(OnDatabaseSyncProgress));
			_003CeventAggregator_003EP.Unsubscribe<DatabaseSyncCompleted>(new Action<DatabaseSyncCompleted>(OnDatabaseSyncCompleted));
			_003CeventAggregator_003EP.Unsubscribe<LocaleChanged>(new Action<LocaleChanged>(OnLocaleChanged));
			_003CeventAggregator_003EP.Unsubscribe<ModuleUnloading>(new Action<ModuleUnloading>(OnModuleUnloading));
		}

		private void OnDatabaseSyncProgress(DatabaseSyncProgress args)
		{
			LocalizedString step = _003Clocalizer_003EP[args.Step];
			LoadingMessage = (string)_003Clocalizer_003EP["Downloading", new object[3]
			{
				step,
				args.Report.ResultCount,
				args.Report.ResultTotal
			}];
			this.DatabaseUpdated?.Invoke(this, EventArgs.Empty);
		}

		private void OnDatabaseSyncCompleted(DatabaseSyncCompleted args)
		{
			LoadingMessage = null;
			TooltipText = null;
			this.DatabaseUpdated?.Invoke(this, EventArgs.Empty);
		}

		private void OnLocaleChanged(LocaleChanged changed)
		{
			OnPropertyChanged("Name");
			OnPropertyChanged("KoFiLabel");
			OnPropertyChanged("SyncLabel");
			OnPropertyChanged("BananaModeLabel");
			OnPropertyChanged("RaiseStackSizeLabel");
		}
	}
}
