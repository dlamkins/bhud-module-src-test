using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Neokain.GW2.AllianceManager.Controls.Shared;
using Neokain.GW2.AllianceManager.Services.Connection;
using Neokain.GW2.AllianceManager.Views;
using Neokain.GW2.WebClient;
using Neokain.GW2.WebClient.Models.Accounts;
using Neokain.GW2.WebClient.Models.Alliances;
using Neokain.GW2.WebClient.Models.Guilds;

namespace Neokain.GW2.AllianceManager.Windows
{
	public class MainWindow : TabbedWindow2
	{
		private readonly Module _module;

		private readonly Gw2WebClient _webClient;

		private Tab _homeTab;

		private Tab _connectionTab;

		private Tab _accountTab;

		private Dictionary<Guid, Tab> _allianceTabs = new Dictionary<Guid, Tab>();

		private Dictionary<Guid, Tab> _guildTabs = new Dictionary<Guid, Tab>();

		private IView _homeView;

		private IView _connectionView;

		private IView _accountView;

		private Dictionary<Guid, IView> _allianceViews = new Dictionary<Guid, IView>();

		private Dictionary<Guid, IView> _guildViews = new Dictionary<Guid, IView>();

		private LoadingOverlay _loadingOverlay;

		private ModuleConnectionState _lastModuleState;

		private bool _hasConnectedOnce;

		private const int DefaultGuildIconAssetId = 155052;

		public MainWindow(Module module, Gw2WebClient webClient)
			: this(AsyncTexture2D.FromAssetId(155985), new Rectangle(40, 26, 913, 691), new Rectangle(95, 26, 850, 670))
		{
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			_module = module ?? throw new ArgumentNullException("module");
			_webClient = webClient ?? throw new ArgumentNullException("webClient");
			((WindowBase2)this).set_Title("Alliance & Guild Manager");
			((WindowBase2)this).set_SavesPosition(true);
			((WindowBase2)this).set_Id("Gw2SinAlliance_mainWindow");
			((Control)this).set_Location(new Point(100, 100));
			CreateStaticTabs();
			InitializeLoadingOverlay();
			if (_webClient != null)
			{
				_webClient.ConnectionStateChanged += new EventHandler<ConnectionStateChangedEventArgs>(OnConnectionStateChanged);
			}
			_lastModuleState = _module.ConnectionStatus?.State ?? ModuleConnectionState.NoKey;
			_module.ConnectionStatusChanged += OnModuleConnectionStatusChanged;
			((TabbedWindow2)this).add_TabChanged((EventHandler<ValueChangedEventArgs<Tab>>)OnTabChanged);
			UpdateTabAvailability();
			ConnectionStatus connectionStatus = _module.ConnectionStatus;
			if (connectionStatus != null && connectionStatus.State == ModuleConnectionState.Connected)
			{
				if (_homeTab != null)
				{
					((TabbedWindow2)this).set_SelectedTab(_homeTab);
				}
			}
			else if (_connectionTab != null)
			{
				((TabbedWindow2)this).set_SelectedTab(_connectionTab);
			}
			UpdateLoadingOverlayVisibility();
			RefreshDynamicTabsAsync();
		}

		private void InitializeLoadingOverlay()
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			LoadingOverlay loadingOverlay = new LoadingOverlay("Connecting to server...");
			((Control)loadingOverlay).set_Parent((Container)(object)this);
			((Control)loadingOverlay).set_Location(Point.get_Zero());
			((Control)loadingOverlay).set_Width(((Container)this).get_ContentRegion().Width);
			((Control)loadingOverlay).set_Height(((Container)this).get_ContentRegion().Height);
			((Control)loadingOverlay).set_Visible(false);
			loadingOverlay.OnReconnect = delegate
			{
				_module.RequestReconnect();
			};
			loadingOverlay.OnOpenConnectionTab = delegate
			{
				if (_connectionTab != null)
				{
					((TabbedWindow2)this).set_SelectedTab(_connectionTab);
				}
			};
			_loadingOverlay = loadingOverlay;
			((Container)this).add_ContentResized((EventHandler<RegionChangedEventArgs>)OnContentResized);
		}

		private void OnContentResized(object sender, RegionChangedEventArgs e)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			if (_loadingOverlay != null)
			{
				((Control)_loadingOverlay).set_Width(((Container)this).get_ContentRegion().Width);
				((Control)_loadingOverlay).set_Height(((Container)this).get_ContentRegion().Height);
			}
		}

		public void UpdateLoadingMessage(string message)
		{
			_loadingOverlay?.UpdateMessage(message);
		}

		private void UpdateLoadingOverlayVisibility()
		{
			if (_loadingOverlay == null)
			{
				return;
			}
			ConnectionStatus status = _module.ConnectionStatus;
			ModuleConnectionState state = status?.State ?? ModuleConnectionState.NoKey;
			bool isOnConnectionTab = ((TabbedWindow2)this).get_SelectedTab() == _connectionTab;
			bool visible = ConnectionOverlayPolicy.ShouldShowOverlay(state, isOnConnectionTab);
			if (visible && ((Control)_loadingOverlay).get_Parent() != this)
			{
				((Control)_loadingOverlay).set_Parent((Container)(object)this);
			}
			((Control)_loadingOverlay).set_Visible(visible);
			if (visible)
			{
				string message = ConnectionOverlayPolicy.OverlayText(status);
				string countdown = ConnectionOverlayPolicy.CountdownText(status, DateTime.UtcNow);
				if (countdown.Length > 0)
				{
					message = message + "  (" + countdown + ")";
				}
				_loadingOverlay.SetState(message, ConnectionOverlayPolicy.CanReconnect(state), state == ModuleConnectionState.Connecting);
			}
		}

		private async Task RefreshDynamicTabsAsync(bool reloadSelectedViewOnConnect = false)
		{
			try
			{
				Gw2WebClient webClient = _webClient;
				if (webClient == null || !webClient.IsConnected || !_webClient.IsVerified)
				{
					GameService.Overlay.QueueMainThreadUpdate((Action<GameTime>)delegate
					{
						ClearAllDynamicTabs();
					});
					return;
				}
				AccountDataDto accountData = await _webClient.GetMyAccount();
				if (accountData == null)
				{
					GameService.Overlay.QueueMainThreadUpdate((Action<GameTime>)delegate
					{
						ClearAllDynamicTabs();
					});
					return;
				}
				List<Guid> allianceIds = new List<Guid>();
				Dictionary<Guid, string> allianceNames = new Dictionary<Guid, string>();
				try
				{
					List<AllianceMembershipDto> allianceMemberships = await _webClient.GetAllianceMemberships(accountData.Id);
					if (allianceMemberships != null)
					{
						foreach (AllianceMembershipDto membership2 in allianceMemberships)
						{
							allianceIds.Add(membership2.AllianceId);
							try
							{
								AllianceDetailDto allianceDetail = await _webClient.GetAllianceDetail(membership2.AllianceId);
								if (allianceDetail != null)
								{
									allianceNames[membership2.AllianceId] = allianceDetail.Name;
								}
								else
								{
									allianceNames[membership2.AllianceId] = "Alliance " + membership2.AllianceId.ToString().Substring(0, 8);
								}
							}
							catch
							{
								allianceNames[membership2.AllianceId] = "Alliance " + membership2.AllianceId.ToString().Substring(0, 8);
							}
						}
					}
				}
				catch (Exception ex3)
				{
					Logger.GetLogger<MainWindow>().Warn(ex3, "Failed to load alliances");
				}
				List<Guid> guildIds = new List<Guid>();
				Dictionary<Guid, string> guildNames = new Dictionary<Guid, string>();
				try
				{
					List<GuildMembershipDto> guildMemberships = await _webClient.GetGuilds(accountData.Id);
					if (guildMemberships != null)
					{
						foreach (GuildMembershipDto membership in guildMemberships)
						{
							guildIds.Add(membership.GuildId);
							try
							{
								GuildDetailDto guildDetail = await _webClient.GetGuildDetail(membership.GuildId);
								if (guildDetail != null)
								{
									guildNames[membership.GuildId] = guildDetail.Name;
								}
								else
								{
									guildNames[membership.GuildId] = "Guild " + membership.GuildId.ToString().Substring(0, 8);
								}
							}
							catch
							{
								guildNames[membership.GuildId] = "Guild " + membership.GuildId.ToString().Substring(0, 8);
							}
						}
					}
				}
				catch (Exception ex2)
				{
					Logger.GetLogger<MainWindow>().Warn(ex2, "Failed to load guilds");
				}
				GameService.Overlay.QueueMainThreadUpdate((Action<GameTime>)delegate
				{
					ApplyDynamicTabDiff(allianceIds, guildIds, allianceNames, guildNames, reloadSelectedViewOnConnect);
				});
			}
			catch (Exception ex)
			{
				Logger.GetLogger<MainWindow>().Error(ex, "Failed to refresh dynamic tabs");
				GameService.Overlay.QueueMainThreadUpdate((Action<GameTime>)delegate
				{
					ClearAllDynamicTabs();
				});
			}
		}

		private void ApplyDynamicTabDiff(List<Guid> allianceIds, List<Guid> guildIds, Dictionary<Guid, string> allianceNames, Dictionary<Guid, string> guildNames, bool reloadSelectedViewOnConnect)
		{
			UpdateDynamicTabsWithNames(allianceIds, guildIds, allianceNames, guildNames);
			if (((TabbedWindow2)this).get_SelectedTab() != null && !((TabbedWindow2)this).get_Tabs().Contains(((TabbedWindow2)this).get_SelectedTab()) && _homeTab != null)
			{
				((TabbedWindow2)this).set_SelectedTab(_homeTab);
			}
			else if (reloadSelectedViewOnConnect)
			{
				ReloadSelectedView();
			}
		}

		private void UpdateDynamicTabsWithNames(List<Guid> allianceIds, List<Guid> guildIds, Dictionary<Guid, string> allianceNames, Dictionary<Guid, string> guildNames)
		{
			List<Guid> newAllianceIds = allianceIds ?? new List<Guid>();
			List<Guid> newGuildIds = guildIds ?? new List<Guid>();
			HashSet<Guid> keysBefore = new HashSet<Guid>(_allianceTabs.Keys);
			keysBefore.UnionWith(_guildTabs.Keys);
			RemoveOldAllianceTabs(newAllianceIds);
			RemoveOldGuildTabs(newGuildIds);
			foreach (Guid allianceId in newAllianceIds)
			{
				if (!_allianceTabs.ContainsKey(allianceId))
				{
					string allianceName;
					string name2 = (allianceNames.TryGetValue(allianceId, out allianceName) ? allianceName : ("Alliance " + allianceId.ToString().Substring(0, 8)));
					AddAllianceTab(allianceId, name2);
				}
			}
			foreach (Guid guildId in newGuildIds)
			{
				if (!_guildTabs.ContainsKey(guildId))
				{
					string guildName;
					string name = (guildNames.TryGetValue(guildId, out guildName) ? guildName : ("Guild " + guildId.ToString().Substring(0, 8)));
					AddGuildTab(guildId, name);
				}
			}
			HashSet<Guid> keysAfter = new HashSet<Guid>(_allianceTabs.Keys);
			keysAfter.UnionWith(_guildTabs.Keys);
			if (!keysBefore.SetEquals(keysAfter))
			{
				ReorderTabs();
			}
		}

		private void CreateStaticTabs()
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Expected O, but got Unknown
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Expected O, but got Unknown
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Expected O, but got Unknown
			Tab val = new Tab(AsyncTexture2D.FromAssetId(155052), (Func<IView>)(() => GetOrCreateHomeView()), "Home", (int?)null);
			val.set_Enabled(true);
			_homeTab = val;
			((TabbedWindow2)this).get_Tabs().Add(_homeTab);
			Tab val2 = new Tab(AsyncTexture2D.FromAssetId(156025), (Func<IView>)(() => GetOrCreateConnectionView()), "Connection", (int?)null);
			val2.set_Enabled(true);
			_connectionTab = val2;
			((TabbedWindow2)this).get_Tabs().Add(_connectionTab);
			Tab val3 = new Tab(AsyncTexture2D.FromAssetId(156679), (Func<IView>)(() => GetOrCreateAccountView()), "Account", (int?)null);
			val3.set_Enabled(false);
			_accountTab = val3;
			((TabbedWindow2)this).get_Tabs().Add(_accountTab);
		}

		private void AddNewAllianceTabs(List<Guid> allianceIds)
		{
			foreach (Guid allianceId in allianceIds)
			{
				if (!_allianceTabs.ContainsKey(allianceId))
				{
					string allianceName = "Alliance " + allianceId.ToString().Substring(0, 8);
					AddAllianceTab(allianceId, allianceName);
				}
			}
		}

		private void AddNewGuildTabs(List<Guid> guildIds)
		{
			foreach (Guid guildId in guildIds)
			{
				if (!_guildTabs.ContainsKey(guildId))
				{
					string guildName = "Guild " + guildId.ToString().Substring(0, 8);
					AddGuildTab(guildId, guildName);
				}
			}
		}

		private void AddAllianceTab(Guid allianceId, string allianceName)
		{
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Expected O, but got Unknown
			if (!_allianceTabs.ContainsKey(allianceId))
			{
				Tab val = new Tab(AsyncTexture2D.FromAssetId(155052), (Func<IView>)(() => GetOrCreateAllianceView(allianceId)), allianceName, (int?)null);
				val.set_Enabled(true);
				Tab tab = val;
				_allianceTabs[allianceId] = tab;
				((TabbedWindow2)this).get_Tabs().Add(tab);
			}
		}

		private void AddGuildTab(Guid guildId, string guildName)
		{
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Expected O, but got Unknown
			if (!_guildTabs.ContainsKey(guildId))
			{
				Tab val = new Tab(AsyncTexture2D.FromAssetId(155052), (Func<IView>)(() => GetOrCreateGuildView(guildId)), guildName, (int?)null);
				val.set_Enabled(true);
				Tab tab = val;
				_guildTabs[guildId] = tab;
				((TabbedWindow2)this).get_Tabs().Add(tab);
				LoadGuildEmblemAsync(guildId, tab);
			}
		}

		private async Task LoadGuildEmblemAsync(Guid guildId, Tab tab)
		{
			try
			{
				byte[] pngBytes = await _webClient.GetGuildEmblemAsync(guildId, 32);
				if (pngBytes == null || pngBytes.Length == 0)
				{
					return;
				}
				GameService.Graphics.QueueMainThreadRender((Action<GraphicsDevice>)delegate(GraphicsDevice device)
				{
					//IL_001b: Unknown result type (might be due to invalid IL or missing references)
					//IL_0025: Expected O, but got Unknown
					try
					{
						using MemoryStream memoryStream = new MemoryStream(pngBytes);
						Texture2D val = Texture2D.FromStream(device, (Stream)memoryStream);
						tab.set_Icon(new AsyncTexture2D(val));
					}
					catch (Exception ex2)
					{
						Logger.GetLogger<MainWindow>().Warn(ex2, $"Failed to create texture for guild {guildId}");
					}
				});
			}
			catch (Exception ex)
			{
				Logger.GetLogger<MainWindow>().Warn(ex, $"Failed to load emblem for guild {guildId}");
			}
		}

		private void RemoveOldAllianceTabs(List<Guid> currentAllianceIds)
		{
			foreach (Guid allianceId in _allianceTabs.Keys.Where((Guid id) => !currentAllianceIds.Contains(id)).ToList())
			{
				if (_allianceTabs.TryGetValue(allianceId, out var tab))
				{
					((TabbedWindow2)this).get_Tabs().Remove(tab);
					_allianceTabs.Remove(allianceId);
					_allianceViews.Remove(allianceId);
				}
			}
		}

		private void RemoveOldGuildTabs(List<Guid> currentGuildIds)
		{
			foreach (Guid guildId in _guildTabs.Keys.Where((Guid id) => !currentGuildIds.Contains(id)).ToList())
			{
				if (_guildTabs.TryGetValue(guildId, out var tab))
				{
					((TabbedWindow2)this).get_Tabs().Remove(tab);
					_guildTabs.Remove(guildId);
					_guildViews.Remove(guildId);
				}
			}
		}

		private void ClearAllDynamicTabs()
		{
			bool needsTabSwitch = _allianceTabs.Values.Contains(((TabbedWindow2)this).get_SelectedTab()) || _guildTabs.Values.Contains(((TabbedWindow2)this).get_SelectedTab());
			foreach (Tab tab2 in _allianceTabs.Values.ToList())
			{
				((TabbedWindow2)this).get_Tabs().Remove(tab2);
			}
			_allianceTabs.Clear();
			_allianceViews.Clear();
			foreach (Tab tab in _guildTabs.Values.ToList())
			{
				((TabbedWindow2)this).get_Tabs().Remove(tab);
			}
			_guildTabs.Clear();
			_guildViews.Clear();
			if (needsTabSwitch && _homeTab != null)
			{
				((TabbedWindow2)this).set_SelectedTab(_homeTab);
			}
		}

		private void ReorderTabs()
		{
			List<Tab> orderedTabs = new List<Tab>();
			orderedTabs.Add(_homeTab);
			orderedTabs.Add(_connectionTab);
			orderedTabs.Add(_accountTab);
			List<Tab> sortedAlliances = _allianceTabs.Values.OrderBy((Tab t) => t.get_Name()).ToList();
			orderedTabs.AddRange(sortedAlliances);
			List<Tab> sortedGuilds = _guildTabs.Values.OrderBy((Tab t) => t.get_Name()).ToList();
			orderedTabs.AddRange(sortedGuilds);
			foreach (Tab tab2 in ((IEnumerable<Tab>)((TabbedWindow2)this).get_Tabs()).ToList())
			{
				((TabbedWindow2)this).get_Tabs().Remove(tab2);
			}
			foreach (Tab tab in orderedTabs)
			{
				((TabbedWindow2)this).get_Tabs().Add(tab);
			}
		}

		private void UpdateTabAvailability()
		{
			bool isConnected = _webClient?.IsConnected ?? false;
			if (_accountTab != null)
			{
				_accountTab.set_Enabled(isConnected);
			}
		}

		private IView GetOrCreateHomeView()
		{
			if (_homeView == null)
			{
				_homeView = (IView)(object)new HomeView(_module, _webClient);
			}
			return _homeView;
		}

		private IView GetOrCreateConnectionView()
		{
			if (_connectionView == null)
			{
				_connectionView = (IView)(object)new ConnectionView(_module);
			}
			return _connectionView;
		}

		private IView GetOrCreateAccountView()
		{
			if (_accountView == null)
			{
				_accountView = (IView)(object)new AccountView(_module, _webClient);
			}
			return _accountView;
		}

		private IView GetOrCreateAllianceView(Guid allianceId)
		{
			if (!_allianceViews.TryGetValue(allianceId, out var view))
			{
				view = (IView)(object)new AllianceView(allianceId, _module, _webClient);
				_allianceViews[allianceId] = view;
			}
			return view;
		}

		private IView GetOrCreateGuildView(Guid guildId)
		{
			if (!_guildViews.TryGetValue(guildId, out var view))
			{
				view = (IView)(object)new GuildView(guildId, _module, _webClient);
				_guildViews[guildId] = view;
			}
			return view;
		}

		private void OnConnectionStateChanged(object sender, ConnectionStateChangedEventArgs e)
		{
			GameService.Overlay.QueueMainThreadUpdate((Action<GameTime>)delegate
			{
				UpdateLoadingOverlayVisibility();
				UpdateTabAvailability();
			});
		}

		private void OnModuleConnectionStatusChanged(object sender, EventArgs e)
		{
			GameService.Overlay.QueueMainThreadUpdate((Action<GameTime>)delegate
			{
				UpdateLoadingOverlayVisibility();
				ModuleConnectionState moduleConnectionState = _module.ConnectionStatus?.State ?? ModuleConnectionState.NoKey;
				bool num = moduleConnectionState == ModuleConnectionState.Connected && _lastModuleState != ModuleConnectionState.Connected;
				_lastModuleState = moduleConnectionState;
				if (num)
				{
					bool hasConnectedOnce = _hasConnectedOnce;
					_hasConnectedOnce = true;
					RefreshDynamicTabsAsync(hasConnectedOnce);
				}
			});
		}

		private void ReloadSelectedView()
		{
			Tab tab = ((TabbedWindow2)this).get_SelectedTab();
			if (tab != null && tab.get_View() != null)
			{
				((WindowBase2)this).ShowView(tab.get_View()());
			}
		}

		private void OnTabChanged(object sender, ValueChangedEventArgs<Tab> e)
		{
			UpdateLoadingOverlayVisibility();
		}

		protected override void DisposeControl()
		{
			if (_webClient != null)
			{
				_webClient.ConnectionStateChanged -= new EventHandler<ConnectionStateChangedEventArgs>(OnConnectionStateChanged);
			}
			if (_module != null)
			{
				_module.ConnectionStatusChanged -= OnModuleConnectionStatusChanged;
			}
			((TabbedWindow2)this).remove_TabChanged((EventHandler<ValueChangedEventArgs<Tab>>)OnTabChanged);
			((Container)this).remove_ContentResized((EventHandler<RegionChangedEventArgs>)OnContentResized);
			((WindowBase2)this).DisposeControl();
		}
	}
}
