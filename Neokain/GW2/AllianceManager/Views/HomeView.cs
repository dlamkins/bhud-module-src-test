using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Modules;
using Microsoft.Xna.Framework;
using MonoGame.Extended.BitmapFonts;
using Neokain.GW2.AllianceManager.Controls.Home;
using Neokain.GW2.AllianceManager.Windows;
using Neokain.GW2.WebClient;
using Neokain.GW2.WebClient.Models.Accounts;
using Neokain.GW2.WebClient.Models.Alliances;

namespace Neokain.GW2.AllianceManager.Views
{
	internal class HomeView : View
	{
		private const int QUICK_ACTIONS_ASSET_ID_CURRENCY = 156909;

		private const int QUICK_ACTIONS_ASSET_ID_FAVORITES = 157328;

		private const int QUICK_ACTIONS_ASSET_ID_SETTINGS = 155052;

		private const int QUICK_ACTIONS_ASSET_ID_BROADCAST = 102478;

		private readonly Module _module;

		private readonly Gw2WebClient _webClient;

		private FlowPanel _mainPanel;

		private FlowPanel _quickActionsPanel;

		private Panel _statusPanel;

		private FlowPanel _announcementsPanel;

		private Label _connectionStatusLabel;

		private Label _accountLabel;

		private Label _versionLabel;

		private QuickActionTile _broadcastTile;

		private AllianceBroadcastWindow _broadcastWindow;

		private List<AllianceMembershipDto> _allianceMemberships;

		public HomeView(Module module, Gw2WebClient webClient)
			: this()
		{
			_module = module ?? throw new ArgumentNullException("module");
			_webClient = webClient ?? throw new ArgumentNullException("webClient");
		}

		protected override async Task<bool> Load(IProgress<string> progress)
		{
			progress.Report("Loading home...");
			return await ((View<IPresenter>)this).Load(progress);
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent(buildPanel);
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Container)val).set_HeightSizingMode((SizingMode)2);
			val.set_FlowDirection((ControlFlowDirection)3);
			((Panel)val).set_CanScroll(true);
			((Control)val).set_Padding(new Thickness(10f));
			val.set_ControlPadding(new Vector2(10f));
			_mainPanel = val;
			BuildQuickActionsSection();
			BuildStatusSection();
			BuildAnnouncementsSection();
			if (_webClient != null)
			{
				_webClient.ConnectionStateChanged += new EventHandler<ConnectionStateChangedEventArgs>(OnConnectionStateChanged);
			}
			((View<IPresenter>)this).Build(buildPanel);
		}

		private void BuildQuickActionsSection()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Expected O, but got Unknown
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)_mainPanel);
			val.set_Text("Quick Actions");
			val.set_Font((BitmapFont)(object)_module.FontService.DejaVuSansHeader);
			val.set_AutoSizeWidth(true);
			val.set_AutoSizeHeight(true);
			FlowPanel val2 = new FlowPanel();
			((Control)val2).set_Parent((Container)(object)_mainPanel);
			((Container)val2).set_WidthSizingMode((SizingMode)2);
			((Container)val2).set_HeightSizingMode((SizingMode)1);
			val2.set_FlowDirection((ControlFlowDirection)0);
			val2.set_ControlPadding(new Vector2(10f));
			_quickActionsPanel = val2;
			QuickActionTile quickActionTile = new QuickActionTile(AsyncTexture2D.FromAssetId(156909), "Currencies");
			((Control)quickActionTile).set_Parent((Container)(object)_quickActionsPanel);
			((Control)quickActionTile).set_BasicTooltipText("Toggle the currency tracker window");
			quickActionTile.Clicked += delegate
			{
				_module.ToggleCurrencyWindow();
			};
			QuickActionTile quickActionTile2 = new QuickActionTile(AsyncTexture2D.FromAssetId(157328), "Favorites");
			((Control)quickActionTile2).set_Parent((Container)(object)_quickActionsPanel);
			((Control)quickActionTile2).set_BasicTooltipText("Toggle the favorites window");
			quickActionTile2.Clicked += delegate
			{
				_module.ToggleFavoritesWindow();
			};
			QuickActionTile quickActionTile3 = new QuickActionTile(AsyncTexture2D.FromAssetId(155052), "Settings");
			((Control)quickActionTile3).set_Parent((Container)(object)_quickActionsPanel);
			((Control)quickActionTile3).set_BasicTooltipText("Open module settings");
			quickActionTile3.Clicked += delegate
			{
				OpenModuleSettings();
			};
			QuickActionTile quickActionTile4 = new QuickActionTile(AsyncTexture2D.FromAssetId(102478), "Broadcast");
			((Control)quickActionTile4).set_Parent((Container)(object)_quickActionsPanel);
			((Control)quickActionTile4).set_BasicTooltipText("Loading alliances...");
			((Control)quickActionTile4).set_Enabled(false);
			_broadcastTile = quickActionTile4;
			_broadcastTile.Clicked += OnBroadcastClicked;
		}

		private void BuildStatusSection()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Expected O, but got Unknown
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Expected O, but got Unknown
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Expected O, but got Unknown
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Expected O, but got Unknown
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_016a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0180: Unknown result type (might be due to invalid IL or missing references)
			//IL_0187: Unknown result type (might be due to invalid IL or missing references)
			//IL_0193: Expected O, but got Unknown
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)_mainPanel);
			val.set_Text("Status");
			val.set_Font((BitmapFont)(object)_module.FontService.DejaVuSansHeader);
			val.set_AutoSizeWidth(true);
			val.set_AutoSizeHeight(true);
			Panel val2 = new Panel();
			((Control)val2).set_Parent((Container)(object)_mainPanel);
			((Container)val2).set_WidthSizingMode((SizingMode)2);
			((Control)val2).set_Height(80);
			((Control)val2).set_BackgroundColor(new Color(40, 40, 40, 180));
			_statusPanel = val2;
			FlowPanel val3 = new FlowPanel();
			((Control)val3).set_Parent((Container)(object)_statusPanel);
			((Container)val3).set_WidthSizingMode((SizingMode)2);
			((Container)val3).set_HeightSizingMode((SizingMode)2);
			val3.set_FlowDirection((ControlFlowDirection)3);
			((Control)val3).set_Padding(new Thickness(10f));
			val3.set_ControlPadding(new Vector2(5f));
			FlowPanel statusFlow = val3;
			Label val4 = new Label();
			((Control)val4).set_Parent((Container)(object)statusFlow);
			val4.set_Text(GetConnectionStatusText());
			val4.set_Font((BitmapFont)(object)_module.FontService.DejaVuSansDefault);
			val4.set_AutoSizeWidth(true);
			val4.set_AutoSizeHeight(true);
			_connectionStatusLabel = val4;
			Label val5 = new Label();
			((Control)val5).set_Parent((Container)(object)statusFlow);
			val5.set_Text("Account: Loading...");
			val5.set_Font((BitmapFont)(object)_module.FontService.DejaVuSansDefault);
			val5.set_AutoSizeWidth(true);
			val5.set_AutoSizeHeight(true);
			_accountLabel = val5;
			string version = GetModuleVersion();
			Label val6 = new Label();
			((Control)val6).set_Parent((Container)(object)statusFlow);
			val6.set_Text("Module Version: " + version);
			val6.set_Font((BitmapFont)(object)_module.FontService.DejaVuSansDefault);
			val6.set_AutoSizeWidth(true);
			val6.set_AutoSizeHeight(true);
			_versionLabel = val6;
			LoadAccountInfoAsync();
		}

		private void BuildAnnouncementsSection()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Expected O, but got Unknown
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)_mainPanel);
			val.set_Text("Announcements");
			val.set_Font((BitmapFont)(object)_module.FontService.DejaVuSansHeader);
			val.set_AutoSizeWidth(true);
			val.set_AutoSizeHeight(true);
			FlowPanel val2 = new FlowPanel();
			((Control)val2).set_Parent((Container)(object)_mainPanel);
			((Container)val2).set_WidthSizingMode((SizingMode)2);
			((Control)val2).set_Height(100);
			val2.set_FlowDirection((ControlFlowDirection)3);
			((Control)val2).set_BackgroundColor(new Color(40, 40, 40, 180));
			((Panel)val2).set_CanScroll(true);
			((Control)val2).set_Padding(new Thickness(10f));
			_announcementsPanel = val2;
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)_announcementsPanel);
			val3.set_Text("Welcome to Alliance Manager!");
			val3.set_Font((BitmapFont)(object)_module.FontService.DejaVuSansDefault);
			val3.set_TextColor(Color.get_LightGray());
			val3.set_AutoSizeWidth(true);
			val3.set_AutoSizeHeight(true);
			Label val4 = new Label();
			((Control)val4).set_Parent((Container)(object)_announcementsPanel);
			val4.set_Text("Use the quick actions above to access features.");
			val4.set_Font((BitmapFont)(object)_module.FontService.DejaVuSansDefault);
			val4.set_TextColor(Color.get_Gray());
			val4.set_AutoSizeWidth(true);
			val4.set_AutoSizeHeight(true);
		}

		private async Task LoadAccountInfoAsync()
		{
			_ = 1;
			try
			{
				Gw2WebClient webClient = _webClient;
				if (webClient == null || !webClient.IsConnected)
				{
					_accountLabel.set_Text("Account: Not connected");
					UpdateBroadcastTileState();
					return;
				}
				AccountDataDto account = await _webClient.GetMyAccount();
				if (account != null)
				{
					_accountLabel.set_Text("Account: " + account.Name);
					try
					{
						_allianceMemberships = await _webClient.GetAccountAllianceMemberships(account.Id);
					}
					catch
					{
						_allianceMemberships = new List<AllianceMembershipDto>();
					}
				}
				else
				{
					_accountLabel.set_Text("Account: Not authenticated");
					_allianceMemberships = new List<AllianceMembershipDto>();
				}
				UpdateBroadcastTileState();
			}
			catch (Exception)
			{
				_accountLabel.set_Text("Account: Error loading");
				_allianceMemberships = new List<AllianceMembershipDto>();
				UpdateBroadcastTileState();
			}
		}

		private void UpdateBroadcastTileState()
		{
			if (_broadcastTile != null)
			{
				int count = _allianceMemberships?.Count ?? 0;
				switch (count)
				{
				case 0:
					((Control)_broadcastTile).set_Enabled(false);
					((Control)_broadcastTile).set_BasicTooltipText("You are not in any alliance");
					break;
				case 1:
					((Control)_broadcastTile).set_Enabled(true);
					((Control)_broadcastTile).set_BasicTooltipText("Broadcast to [" + _allianceMemberships[0].AllianceTag + "] " + _allianceMemberships[0].AllianceName);
					break;
				default:
					((Control)_broadcastTile).set_Enabled(true);
					((Control)_broadcastTile).set_BasicTooltipText($"Broadcast to one of your {count} alliances");
					break;
				}
			}
		}

		private void OnBroadcastClicked(object sender, EventArgs e)
		{
			if (_allianceMemberships != null && _allianceMemberships.Count != 0)
			{
				if (_broadcastWindow == null)
				{
					_broadcastWindow = new AllianceBroadcastWindow(_module, _webClient, _allianceMemberships);
				}
				else
				{
					_broadcastWindow.UpdateAllianceMemberships(_allianceMemberships);
				}
				((Control)_broadcastWindow).Show();
				((WindowBase2)_broadcastWindow).BringWindowToFront();
			}
		}

		private string GetConnectionStatusText()
		{
			if (_webClient == null)
			{
				return "Connection: Not initialized";
			}
			if (_webClient.IsConnected)
			{
				return "Connection: Connected";
			}
			ConnectionState state = _webClient.State;
			return $"Connection: {state}";
		}

		private string GetModuleVersion()
		{
			Module instance = Module.Instance;
			return ((instance == null) ? null : ((object)((Module)instance).get_Version())?.ToString()) ?? "0.0.0";
		}

		private void OpenModuleSettings()
		{
			((Control)GameService.Overlay.get_BlishHudWindow()).Show();
		}

		private void OnConnectionStateChanged(object sender, ConnectionStateChangedEventArgs e)
		{
			if (_connectionStatusLabel != null)
			{
				_connectionStatusLabel.set_Text(GetConnectionStatusText());
			}
			if (e.NewState == ConnectionState.Connected)
			{
				LoadAccountInfoAsync();
			}
			else if (_accountLabel != null)
			{
				_accountLabel.set_Text("Account: Not connected");
			}
		}

		protected override void Unload()
		{
			if (_webClient != null)
			{
				_webClient.ConnectionStateChanged -= new EventHandler<ConnectionStateChangedEventArgs>(OnConnectionStateChanged);
			}
			AllianceBroadcastWindow broadcastWindow = _broadcastWindow;
			if (broadcastWindow != null)
			{
				((Control)broadcastWindow).Dispose();
			}
			FlowPanel mainPanel = _mainPanel;
			if (mainPanel != null)
			{
				((Control)mainPanel).Dispose();
			}
			((View<IPresenter>)this).Unload();
		}
	}
}
