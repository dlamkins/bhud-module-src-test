using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
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

namespace KpRefresher
{
	[Export(typeof(Module))]
	public class KpRefresher : Module
	{
		private static readonly Logger Logger = Logger.GetLogger<KpRefresher>();

		internal static KpRefresher KpRefresherInstance;

		private AsyncTexture2D _windowBackgroundTexture;

		private Texture2D _emblemTexture;

		private Texture2D _cornerIconTexture;

		private Texture2D _cornerIconHoverTexture;

		private CornerIcon _cornerIcon;

		private ContextMenuStrip _cornerIconContextMenu;

		private LoadingSpinner _apiSpinner;

		private KpRefresherWindow _mainWindow;

		private ContextMenuStripItem _notificationNextRefreshAvailable;

		public static ModuleSettings ModuleSettings { get; private set; }

		public static Gw2ApiService Gw2ApiService { get; private set; }

		public static KpMeService KpMeService { get; private set; }

		public static BusinessService BusinessService { get; private set; }

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
			Gw2ApiService = new Gw2ApiService(Gw2ApiManager, Logger);
			KpMeService = new KpMeService(Logger);
			BusinessService = new BusinessService(ModuleSettings, Gw2ApiService, KpMeService, () => (LoadingSpinner)(object)_apiSpinner);
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
			_cornerIconTexture = ContentsManager.GetTexture("corner.png");
			_cornerIconHoverTexture = ContentsManager.GetTexture("corner-hover.png");
			_windowBackgroundTexture = AsyncTexture2D.FromAssetId(155985);
			KpRefresherWindow kpRefresherWindow = new KpRefresherWindow(_windowBackgroundTexture, new Rectangle(40, 26, 913, 691), new Rectangle(50, 26, 893, 681), AsyncTexture2D.op_Implicit(_emblemTexture), ModuleSettings, BusinessService);
			((Control)kpRefresherWindow).set_Size(new Point(500, 700));
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
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Expected O, but got Unknown
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Expected O, but got Unknown
			//IL_021f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0250: Unknown result type (might be due to invalid IL or missing references)
			CornerIcon val = new CornerIcon();
			val.set_Icon(AsyncTexture2D.op_Implicit(_cornerIconTexture));
			((Control)val).set_BasicTooltipText(((Module)this).get_Name() ?? "");
			((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			val.set_HoverIcon(AsyncTexture2D.op_Implicit(_cornerIconHoverTexture));
			val.set_Priority(1283537108);
			_cornerIcon = val;
			((Control)_cornerIcon).add_Click((EventHandler<MouseEventArgs>)delegate
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
			((Control)_cornerIcon).set_Menu(_cornerIconContextMenu);
			LoadingSpinner loadingSpinner = new LoadingSpinner();
			((Control)loadingSpinner).set_Location(new Point(((Control)_cornerIcon).get_Left(), ((Control)_cornerIcon).get_Bottom() + 3));
			((Control)loadingSpinner).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)loadingSpinner).set_Size(new Point(((Control)_cornerIcon).get_Width(), ((Control)_cornerIcon).get_Height()));
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
			CornerIcon cornerIcon = _cornerIcon;
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
			AsyncTexture2D windowBackgroundTexture = _windowBackgroundTexture;
			if (windowBackgroundTexture != null)
			{
				windowBackgroundTexture.Dispose();
			}
			Texture2D emblemTexture = _emblemTexture;
			if (emblemTexture != null)
			{
				((GraphicsResource)emblemTexture).Dispose();
			}
			Texture2D cornerIconTexture = _cornerIconTexture;
			if (cornerIconTexture != null)
			{
				((GraphicsResource)cornerIconTexture).Dispose();
			}
			Texture2D cornerIconHoverTexture = _cornerIconHoverTexture;
			if (cornerIconHoverTexture != null)
			{
				((GraphicsResource)cornerIconHoverTexture).Dispose();
			}
			KpRefresherInstance = null;
		}
	}
}
