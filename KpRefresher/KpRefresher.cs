using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.GameIntegration;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Gw2Sharp.WebApi;
using Gw2Sharp.WebApi.V2.Models;
using KpRefresher.Ressources;
using KpRefresher.Services;
using KpRefresher.UI.Controls;
using KpRefresher.UI.Views;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SemVer;

namespace KpRefresher
{
	[Export(typeof(Module))]
	public class KpRefresher : Module
	{
		private static readonly Logger Logger = Logger.GetLogger<KpRefresher>();

		internal static KpRefresher KpRefresherInstance;

		private AsyncTexture2D _windowBackgroundTexture;

		private Texture2D _emblemTexture;

		private ContextMenuStrip _cornerIconContextMenu;

		private LoadingSpinner _apiSpinner;

		private KpRefresherWindow _mainWindow;

		private ContextMenuStripItem _notificationNextRefreshAvailable;

		public static ModuleSettings ModuleSettings { get; private set; }

		public static Gw2ApiService Gw2ApiService { get; private set; }

		public static KpMeService KpMeService { get; private set; }

		public static BusinessService BusinessService { get; private set; }

		public static CornerIcon CornerIcon { get; private set; }

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		internal Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		[ImportingConstructor]
		public KpRefresher([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
			KpRefresherInstance = this;
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			ModuleSettings = new ModuleSettings(settings);
		}

		protected override void Initialize()
		{
			if (Program.get_OverlayVersion() < new SemVer.Version(1, 1, 0))
			{
				try
				{
					typeof(TacOIntegration).GetProperty("TacOIsRunning").GetSetMethod(nonPublic: true)?.Invoke(GameService.GameIntegration.get_TacO(), new object[1] { true });
				}
				catch
				{
				}
			}
			CornerIcon cornerIcon = new CornerIcon(ContentsManager);
			((Control)cornerIcon).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((CornerIcon)cornerIcon).set_Priority(1283537108);
			CornerIcon = cornerIcon;
			Gw2ApiService = new Gw2ApiService(Gw2ApiManager, Logger);
			KpMeService = new KpMeService(Logger);
			BusinessService = new BusinessService(ModuleSettings, Gw2ApiService, KpMeService, () => (LoadingSpinner)(object)_apiSpinner, CornerIcon, Logger);
			Gw2ApiManager.add_SubtokenUpdated((EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>>)OnApiSubTokenUpdated);
			GameService.Overlay.get_UserLocale().add_SettingChanged((EventHandler<ValueChangedEventArgs<Locale>>)OnLocaleChanged);
		}

		private async void OnApiSubTokenUpdated(object sender, ValueEventArgs<IEnumerable<TokenPermission>> e)
		{
			await BusinessService.RefreshBaseData();
		}

		private void OnLocaleChanged(object sender, ValueChangedEventArgs<Locale> eventArgs)
		{
			LocalizingService.OnLocaleChanged(sender, eventArgs);
		}

		protected override async Task LoadAsync()
		{
			GameService.Gw2Mumble.get_CurrentMap().add_MapChanged((EventHandler<ValueEventArgs<int>>)CurrentMap_MapChanged);
			_emblemTexture = ContentsManager.GetTexture("emblem.png");
			_windowBackgroundTexture = AsyncTexture2D.FromAssetId(155985);
			KpRefresherWindow kpRefresherWindow = new KpRefresherWindow(_windowBackgroundTexture, new Rectangle(40, 26, 913, 691), new Rectangle(50, 26, 893, 681), AsyncTexture2D.op_Implicit(_emblemTexture), ModuleSettings, BusinessService);
			((Control)kpRefresherWindow).set_Size(new Point(520, 700));
			_mainWindow = kpRefresherWindow;
			_mainWindow.BuildUi();
		}

		private void CurrentMap_MapChanged(object sender, ValueEventArgs<int> e)
		{
			if (ModuleSettings.RefreshOnMapChange.get_Value())
			{
				BusinessService.MapChanged();
				_mainWindow.RefreshLoadingSpinnerState();
			}
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			HandleCornerIcon();
			((Module)this).OnModuleLoaded(e);
		}

		private void HandleCornerIcon()
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			//IL_01be: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
			((Control)CornerIcon).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				((WindowBase2)_mainWindow).ToggleWindow();
			});
			_cornerIconContextMenu = new ContextMenuStrip();
			ContextMenuStripItem refeshKpMenuItem = new ContextMenuStripItem
			{
				SetLocalizedText = () => strings.CornerIcon_Refresh
			};
			((Control)refeshKpMenuItem).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				await BusinessService.RefreshKillproofMe();
			});
			ContextMenuStripItem copyKpToClipboard = new ContextMenuStripItem
			{
				SetLocalizedText = () => strings.CornerIcon_Copy
			};
			((Control)copyKpToClipboard).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				BusinessService.CopyKpToClipboard();
			});
			_notificationNextRefreshAvailable = new ContextMenuStripItem
			{
				SetLocalizedText = () => strings.CornerIcon_Notify
			};
			((Control)_notificationNextRefreshAvailable).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				if (!BusinessService.NotificationNextRefreshAvailabledActivated)
				{
					if (await BusinessService.ActivateNotificationNextRefreshAvailable())
					{
						_notificationNextRefreshAvailable.SetLocalizedText = () => strings.CornerIcon_CancelNotify;
					}
				}
				else
				{
					BusinessService.ResetNotificationNextRefreshAvailable();
					_notificationNextRefreshAvailable.SetLocalizedText = () => strings.CornerIcon_Notify;
				}
			});
			ContextMenuStripItem openKpUrl = new ContextMenuStripItem
			{
				SetLocalizedText = () => strings.CornerIcon_OpenWebsite
			};
			((Control)openKpUrl).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				BusinessService.OpenKpUrl();
			});
			_cornerIconContextMenu.AddMenuItem((ContextMenuStripItem)(object)refeshKpMenuItem);
			_cornerIconContextMenu.AddMenuItem((ContextMenuStripItem)(object)copyKpToClipboard);
			_cornerIconContextMenu.AddMenuItem((ContextMenuStripItem)(object)_notificationNextRefreshAvailable);
			_cornerIconContextMenu.AddMenuItem((ContextMenuStripItem)(object)openKpUrl);
			((Control)CornerIcon).set_Menu(_cornerIconContextMenu);
			LoadingSpinner loadingSpinner = new LoadingSpinner();
			((Control)loadingSpinner).set_Location(new Point(((Control)CornerIcon).get_Left(), ((Control)CornerIcon).get_Bottom() + 3));
			((Control)loadingSpinner).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)loadingSpinner).set_Size(new Point(((Control)CornerIcon).get_Width(), ((Control)CornerIcon).get_Height()));
			loadingSpinner.SetLocalizedTooltip = () => strings.LoadingSpinner_Fetch;
			((Control)loadingSpinner).set_Visible(false);
			_apiSpinner = loadingSpinner;
		}

		protected override void Update(GameTime gameTime)
		{
			if (BusinessService.RefreshScheduled)
			{
				BusinessService.ScheduleTimer += gameTime.get_ElapsedGameTime().TotalMilliseconds;
				if (BusinessService.ScheduleTimer > BusinessService.ScheduleTimerEndValue)
				{
					BusinessService.RefreshKillproofMe(fromUpdateLoop: true).ContinueWith(delegate
					{
						_mainWindow.RefreshLoadingSpinnerState();
					});
				}
			}
			if (!BusinessService.NotificationNextRefreshAvailabledActivated)
			{
				return;
			}
			BusinessService.NotificationNextRefreshAvailabledTimer += gameTime.get_ElapsedGameTime().TotalMilliseconds;
			if (BusinessService.NotificationNextRefreshAvailabledTimer > BusinessService.NotificationNextRefreshAvailabledTimerEndValue)
			{
				BusinessService.NextRefreshIsAvailable();
				_notificationNextRefreshAvailable.SetLocalizedText = () => strings.CornerIcon_Notify;
			}
		}

		protected override void Unload()
		{
			Gw2ApiManager.remove_SubtokenUpdated((EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>>)OnApiSubTokenUpdated);
			GameService.Gw2Mumble.get_CurrentMap().remove_MapChanged((EventHandler<ValueEventArgs<int>>)CurrentMap_MapChanged);
			CornerIcon cornerIcon = CornerIcon;
			if (cornerIcon != null)
			{
				((Control)cornerIcon).Dispose();
			}
			ContextMenuStrip cornerIconContextMenu = _cornerIconContextMenu;
			if (cornerIconContextMenu != null)
			{
				((Control)cornerIconContextMenu).Dispose();
			}
			LoadingSpinner apiSpinner = _apiSpinner;
			if (apiSpinner != null)
			{
				((Control)apiSpinner).Dispose();
			}
			KpRefresherWindow mainWindow = _mainWindow;
			if (mainWindow != null)
			{
				((Control)mainWindow).Dispose();
			}
			Texture2D emblemTexture = _emblemTexture;
			if (emblemTexture != null)
			{
				((GraphicsResource)emblemTexture).Dispose();
			}
			KpRefresherInstance = null;
		}
	}
}
