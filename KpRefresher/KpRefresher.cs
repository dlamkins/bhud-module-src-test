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
using Gw2Sharp.WebApi.V2.Models;
using KpRefresher.Services;
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
			BusinessService = new BusinessService(ModuleSettings, Gw2ApiService, KpMeService, () => _apiSpinner);
			Gw2ApiManager.add_SubtokenUpdated((EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>>)OnApiSubTokenUpdated);
		}

		private async void OnApiSubTokenUpdated(object sender, ValueEventArgs<IEnumerable<TokenPermission>> e)
		{
			await BusinessService.RefreshBaseData();
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
			//IL_0052: Expected O, but got Unknown
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Expected O, but got Unknown
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Expected O, but got Unknown
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Expected O, but got Unknown
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Expected O, but got Unknown
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Expected O, but got Unknown
			//IL_0176: Unknown result type (might be due to invalid IL or missing references)
			//IL_017b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0194: Unknown result type (might be due to invalid IL or missing references)
			//IL_019e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01da: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e6: Expected O, but got Unknown
			CornerIcon val = new CornerIcon();
			val.set_Icon(AsyncTexture2D.op_Implicit(_cornerIconTexture));
			((Control)val).set_BasicTooltipText(((Module)this).get_Name() ?? "");
			((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			val.set_HoverIcon(AsyncTexture2D.op_Implicit(_cornerIconHoverTexture));
			_cornerIcon = val;
			((Control)_cornerIcon).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				((WindowBase2)_mainWindow).ToggleWindow();
			});
			_cornerIconContextMenu = new ContextMenuStrip();
			ContextMenuStripItem refeshKpMenuItem = new ContextMenuStripItem("Refresh KillProof.me data");
			((Control)refeshKpMenuItem).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				await BusinessService.RefreshKillproofMe();
			});
			ContextMenuStripItem copyKpToClipboard = new ContextMenuStripItem("Copy KillProof.me Id to clipboard");
			((Control)copyKpToClipboard).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				BusinessService.CopyKpToClipboard();
			});
			_notificationNextRefreshAvailable = new ContextMenuStripItem("Notify when refresh available");
			((Control)_notificationNextRefreshAvailable).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				if (!BusinessService.NotificationNextRefreshAvailabledActivated)
				{
					await BusinessService.ActivateNotificationNextRefreshAvailable();
					_notificationNextRefreshAvailable.set_Text("Cancel notification for next refresh");
				}
				else
				{
					BusinessService.ResetNotificationNextRefreshAvailable();
					_notificationNextRefreshAvailable.set_Text("Notify when refresh available");
				}
			});
			ContextMenuStripItem openKpUrl = new ContextMenuStripItem("Open KillProof.me website");
			((Control)openKpUrl).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				BusinessService.OpenKpUrl();
			});
			_cornerIconContextMenu.AddMenuItem(refeshKpMenuItem);
			_cornerIconContextMenu.AddMenuItem(copyKpToClipboard);
			_cornerIconContextMenu.AddMenuItem(_notificationNextRefreshAvailable);
			_cornerIconContextMenu.AddMenuItem(openKpUrl);
			((Control)_cornerIcon).set_Menu(_cornerIconContextMenu);
			LoadingSpinner val2 = new LoadingSpinner();
			((Control)val2).set_Location(new Point(((Control)_cornerIcon).get_Left(), ((Control)_cornerIcon).get_Bottom() + 3));
			((Control)val2).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)val2).set_Size(new Point(((Control)_cornerIcon).get_Width(), ((Control)_cornerIcon).get_Height()));
			((Control)val2).set_BasicTooltipText("Fetching Api Data");
			((Control)val2).set_Visible(false);
			_apiSpinner = val2;
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
			if (BusinessService.NotificationNextRefreshAvailabledActivated)
			{
				BusinessService.NotificationNextRefreshAvailabledTimer += gameTime.get_ElapsedGameTime().TotalMilliseconds;
				if (BusinessService.NotificationNextRefreshAvailabledTimer > BusinessService.NotificationNextRefreshAvailabledTimerEndValue)
				{
					BusinessService.NextRefreshIsAvailable();
					_notificationNextRefreshAvailable.set_Text("Notify when refresh available");
				}
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
