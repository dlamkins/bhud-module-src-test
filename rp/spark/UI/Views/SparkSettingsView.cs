using System;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using rp.spark.Models;
using rp.spark.Services;

namespace rp.spark.UI.Views
{
	public class SparkSettingsView : View
	{
		private const int ContentWidth = 660;

		private const int ControlHeight = 30;

		private const int StatusLineHeight = 24;

		private const int RowGap = 8;

		private const int LeftPadding = 8;

		private readonly SparkSettings _settings;

		private readonly Func<ServerSyncStatus> _getServerSyncStatus;

		private readonly Action<Action<ServerSyncStatus>> _watchServerSyncStatus;

		private readonly Action<Action<ServerSyncStatus>> _unwatchServerSyncStatus;

		private readonly Action _requestServerSync;

		private readonly Action _enforceGameplayWindowVisibility;

		private readonly Func<string> _getImportantNotice;

		private readonly SparkSettingsButtons _buttons;

		private readonly Action _openBlocklist;

		private readonly Action<Action> _watchBlockedAccountsChanged;

		private readonly Action<Action> _unwatchBlockedAccountsChanged;

		private readonly Action<bool> _maturePreferenceChanged;

		private Label _blockedAccountsLabel;

		private Container _buildPanel;

		private Panel _matureConfirmationPanel;

		private bool _isUnloaded;

		private Label _serverStatusLabel;

		private Label _readinessLabel;

		private static readonly TimeSpan NoticeRefreshInterval = TimeSpan.FromMilliseconds(500.0);

		private CancellationTokenSource _noticeRefreshCancel;

		private Task _noticeRefreshTask;

		public SparkSettingsView(Action openProfileManager, Action openProfileViewer, Action openOnlineList, Action openNearby, Action openSavedProfiles, Action openAbout, Action openBlocklist, Func<Task<string>> waitForInitialState, Func<string> getCurrentStateMessage, Action requestStateRefresh, SparkSettings settings, Func<ServerSyncStatus> getServerSyncStatus, Action<Action<ServerSyncStatus>> watchServerSyncStatus, Action<Action<ServerSyncStatus>> unwatchServerSyncStatus, Action requestServerSync, Func<string> getImportantNotice, Func<bool> shouldHideGameplayWindows, Action enforceGameplayWindowVisibility, Action<Action> watchBlockedAccountsChanged, Action<Action> unwatchBlockedAccountsChanged, Action<bool> maturePreferenceChanged)
			: this()
		{
			_settings = settings;
			_getServerSyncStatus = getServerSyncStatus;
			_watchServerSyncStatus = watchServerSyncStatus;
			_unwatchServerSyncStatus = unwatchServerSyncStatus;
			_requestServerSync = requestServerSync;
			_getImportantNotice = getImportantNotice;
			_enforceGameplayWindowVisibility = enforceGameplayWindowVisibility;
			_buttons = new SparkSettingsButtons(openProfileManager, openProfileViewer, openOnlineList, openNearby, openSavedProfiles, openAbout, GetMatureProfilesButtonText, ToggleMatureProfiles, waitForInitialState, getCurrentStateMessage, requestStateRefresh, shouldHideGameplayWindows);
			_openBlocklist = openBlocklist;
			_watchBlockedAccountsChanged = watchBlockedAccountsChanged;
			_unwatchBlockedAccountsChanged = unwatchBlockedAccountsChanged;
			_maturePreferenceChanged = maturePreferenceChanged;
		}

		protected override void Build(Container buildPanel)
		{
			_isUnloaded = false;
			_buildPanel = buildPanel;
			BuildSettings(buildPanel);
			WatchServer();
			WatchGameState();
			StartNoticeRefresh();
			RefreshServerStatus();
		}

		private void BuildSettings(Container buildPanel)
		{
			FlowPanel settingsStack = SparkFormLayout.AddAutoStack(buildPanel, 660, 6);
			((Control)settingsStack).set_Left(8);
			BuildServerStatus(settingsStack);
			BuildReadinessNotice(settingsStack);
			_buttons.Build((Container)(object)settingsStack);
			SparkFormLayout.AddSpacer((Container)(object)settingsStack, 660, 4);
			BuildPresence(settingsStack);
			BuildGlobalSettings(settingsStack);
			SparkFormLayout.AddSpacer((Container)(object)settingsStack, 660, 4);
			BuildBlockSummary(settingsStack);
		}

		private void BuildReadinessNotice(FlowPanel settingsStack)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			_readinessLabel = SparkFormLayout.AddLabel((Container)(object)settingsStack, string.Empty, 660, 24, GameService.Content.get_DefaultFont14(), SparkViewUI.WarningTextColor, strokeText: true);
			_readinessLabel.set_WrapText(true);
			RefreshReadinessNotice();
		}

		private void BuildServerStatus(FlowPanel settingsStack)
		{
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			FlowPanel serverRow = SparkFormLayout.AddRow((Container)(object)settingsStack, 660, 24, 5);
			SparkFormLayout.AddLabel((Container)(object)serverRow, "Server status:", 95, 24, GameService.Content.get_DefaultFont14());
			_serverStatusLabel = SparkFormLayout.AddLabel((Container)(object)serverRow, ServerStatusText(_getServerSyncStatus?.Invoke()), 560, 24, GameService.Content.get_DefaultFont14(), SparkViewUI.SecondaryTextColor);
		}

		private void BuildPresence(FlowPanel settingsStack)
		{
			RPStatus currentStatus = ((_settings.CurrentStatus.get_Value() != RPStatus.Offline) ? _settings.CurrentStatus.get_Value() : RPStatus.Online);
			FlowPanel statusRow = SparkFormLayout.AddRow((Container)(object)settingsStack, 660, 30, 8);
			SparkFormLayout.AddLabel((Container)(object)statusRow, "Status:", 55, 30, GameService.Content.get_DefaultFont14());
			Dropdown statusDropdown = SparkFormLayout.AddDropdown((Container)(object)statusRow, ProfileLabels.RpStatusOptions, ProfileLabels.StatusLabel(currentStatus), 155, 30);
			statusDropdown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				_settings.CurrentStatus.set_Value(ProfileLabels.ParseStatus(statusDropdown.get_SelectedItem()?.ToString()));
				_requestServerSync?.Invoke();
			});
			Checkbox broadcastCheckbox = SparkFormLayout.AddCheckbox((Container)(object)statusRow, "Share my profile", _settings.BroadcastProfile.get_Value(), 230);
			broadcastCheckbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				_settings.BroadcastProfile.set_Value(broadcastCheckbox.get_Checked());
			});
			broadcastCheckbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				_requestServerSync?.Invoke();
			});
			Checkbox hideLocationCheckbox = SparkFormLayout.AddCheckbox((Container)(object)statusRow, "Hide my location", _settings.HideLocation.get_Value(), 180);
			hideLocationCheckbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				_settings.HideLocation.set_Value(hideLocationCheckbox.get_Checked());
			});
			hideLocationCheckbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				_requestServerSync?.Invoke();
			});
		}

		private void BuildBlockSummary(FlowPanel settingsStack)
		{
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			FlowPanel blockRow = SparkFormLayout.AddRow((Container)(object)settingsStack, 660, 30, 12);
			SparkFormLayout.AddLabel((Container)(object)blockRow, "Blocked accounts:", 125, 30, GameService.Content.get_DefaultFont14());
			_blockedAccountsLabel = SparkFormLayout.AddLabel((Container)(object)blockRow, string.Empty, 90, 30, GameService.Content.get_DefaultFont14(), SparkViewUI.SecondaryTextColor);
			((Control)SparkFormLayout.AddButton((Container)(object)blockRow, "Manage Blocks", 130, 30)).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_openBlocklist?.Invoke();
			});
			_watchBlockedAccountsChanged?.Invoke(OnBlockedAccountsChanged);
			RefreshBlockedAccountCount();
		}

		private void OnBlockedAccountsChanged()
		{
			SparkUiThread.Queue(delegate
			{
				if (!_isUnloaded)
				{
					RefreshBlockedAccountCount();
				}
			});
		}

		private void RefreshBlockedAccountCount()
		{
			if (_blockedAccountsLabel != null)
			{
				int count = _settings?.GetBlockedAccountNames().Count ?? 0;
				_blockedAccountsLabel.set_Text((count == 1) ? "1 blocked" : $"{count} blocked");
			}
		}

		private void WatchServer()
		{
			_watchServerSyncStatus?.Invoke(OnServerStatus);
		}

		private void UnwatchServer()
		{
			_unwatchServerSyncStatus?.Invoke(OnServerStatus);
		}

		private void StartNoticeRefresh()
		{
			StopNoticeRefresh();
			_noticeRefreshCancel = new CancellationTokenSource();
			_noticeRefreshTask = RefreshNoticeLoopAsync(_noticeRefreshCancel.Token);
		}

		private async Task RefreshNoticeLoopAsync(CancellationToken cancellationToken)
		{
			while (!cancellationToken.IsCancellationRequested)
			{
				try
				{
					await Task.Delay(NoticeRefreshInterval, cancellationToken);
				}
				catch (OperationCanceledException)
				{
					return;
				}
				SparkUiThread.Queue(delegate
				{
					if (!_isUnloaded)
					{
						RefreshReadinessNotice();
					}
				});
			}
		}

		private void StopNoticeRefresh()
		{
			CancellationTokenSource cancellation = _noticeRefreshCancel;
			Task task = _noticeRefreshTask;
			_noticeRefreshCancel = null;
			_noticeRefreshTask = null;
			if (cancellation != null)
			{
				cancellation.Cancel();
				TaskCleanup.DisposeWhenComplete(task, cancellation);
			}
		}

		private void OnServerStatus(ServerSyncStatus status)
		{
			SparkUiThread.Queue(delegate
			{
				if (!_isUnloaded)
				{
					RefreshServerStatus(status);
				}
			});
		}

		private void RefreshServerStatus(ServerSyncStatus status = null)
		{
			if (_serverStatusLabel != null)
			{
				_serverStatusLabel.set_Text(ServerStatusText(_getServerSyncStatus?.Invoke() ?? status));
				RefreshReadinessNotice();
			}
		}

		private void WatchGameState()
		{
			GameService.Gw2Mumble.add_IsAvailableChanged((EventHandler<ValueEventArgs<bool>>)OnGameStateChanged);
			((GameService)GameService.Gw2Mumble).add_FinishedLoading((EventHandler<EventArgs>)OnGameStateChanged);
			GameService.Gw2Mumble.get_PlayerCharacter().add_NameChanged((EventHandler<ValueEventArgs<string>>)OnGameStateChanged);
			GameService.Gw2Mumble.get_UI().add_IsMapOpenChanged((EventHandler<ValueEventArgs<bool>>)OnGameStateChanged);
			GameService.GameIntegration.get_Gw2Instance().add_IsInGameChanged((EventHandler<ValueEventArgs<bool>>)OnGameStateChanged);
		}

		private void UnwatchGameState()
		{
			GameService.Gw2Mumble.remove_IsAvailableChanged((EventHandler<ValueEventArgs<bool>>)OnGameStateChanged);
			((GameService)GameService.Gw2Mumble).remove_FinishedLoading((EventHandler<EventArgs>)OnGameStateChanged);
			GameService.Gw2Mumble.get_PlayerCharacter().remove_NameChanged((EventHandler<ValueEventArgs<string>>)OnGameStateChanged);
			GameService.Gw2Mumble.get_UI().remove_IsMapOpenChanged((EventHandler<ValueEventArgs<bool>>)OnGameStateChanged);
			GameService.GameIntegration.get_Gw2Instance().remove_IsInGameChanged((EventHandler<ValueEventArgs<bool>>)OnGameStateChanged);
		}

		private void OnGameStateChanged(object sender, EventArgs e)
		{
			SparkUiThread.Queue(delegate
			{
				if (!_isUnloaded)
				{
					RefreshReadinessNotice();
				}
			});
		}

		private void RefreshReadinessNotice()
		{
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			if (_readinessLabel != null)
			{
				string notice = GetImportantNotice();
				_readinessLabel.set_Text(string.IsNullOrWhiteSpace(notice) ? "SPARK tools ready." : notice);
				_readinessLabel.set_TextColor((Color)(string.IsNullOrWhiteSpace(notice) ? new Color(140, 220, 140) : SparkViewUI.WarningTextColor));
				((Control)_readinessLabel).set_Height(24);
				((Control)_readinessLabel).set_Visible(true);
			}
		}

		private void BuildGlobalSettings(FlowPanel settingsStack)
		{
			FlowPanel optionsRow = SparkFormLayout.AddRow((Container)(object)settingsStack, 660, 30, 12);
			SparkFormLayout.AddLabel((Container)(object)optionsRow, "Region:", 55, 30, GameService.Content.get_DefaultFont14());
			Dropdown regionDropdown = SparkFormLayout.AddDropdown((Container)(object)optionsRow, new string[2]
			{
				ProfileRegion.NA.ToString(),
				ProfileRegion.EU.ToString()
			}, _settings.RegionFilter.get_Value().ToString(), 90, 30);
			regionDropdown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				if (Enum.TryParse<ProfileRegion>(regionDropdown.get_SelectedItem()?.ToString(), out var result))
				{
					_settings.RegionFilter.set_Value(result);
					_requestServerSync?.Invoke();
				}
			});
			Checkbox autoRefreshCheckbox = SparkFormLayout.AddCheckbox((Container)(object)optionsRow, "Auto-refresh Online List", _settings.AutoRefreshOnlineProfiles.get_Value(), 220);
			autoRefreshCheckbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				_settings.AutoRefreshOnlineProfiles.set_Value(autoRefreshCheckbox.get_Checked());
			});
			Checkbox autoHideCheckbox = SparkFormLayout.AddCheckbox((Container)(object)optionsRow, "Auto-hide UI", _settings.AutoHideGameUi.get_Value(), 230);
			autoHideCheckbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				_settings.AutoHideGameUi.set_Value(autoHideCheckbox.get_Checked());
				RefreshReadinessNotice();
				_buttons.Refresh();
				_enforceGameplayWindowVisibility?.Invoke();
			});
			Checkbox cornerIconCheckbox = SparkFormLayout.AddCheckbox((Container)(object)optionsRow, "Show SPARK icon", _settings.ShowCornerIcon.get_Value(), 125);
			cornerIconCheckbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				_settings.ShowCornerIcon.set_Value(cornerIconCheckbox.get_Checked());
			});
		}

		private void SetMatureProfilesEnabled(bool enabled)
		{
			_settings.ShowMatureProfiles.set_Value(enabled);
			_buttons.RefreshMatureButtonText();
			_maturePreferenceChanged?.Invoke(enabled);
		}

		private void OpenMatureConfirmation()
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Expected O, but got Unknown
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0160: Unknown result type (might be due to invalid IL or missing references)
			//IL_016b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			//IL_017d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0185: Unknown result type (might be due to invalid IL or missing references)
			//IL_018f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
			CloseMatureConfirmation();
			Container popupParent = (Container)(((object)_buildPanel) ?? ((object)GameService.Graphics.get_SpriteScreen()));
			Panel val = new Panel();
			val.set_ShowBorder(true);
			val.set_Title("Enable Mature Profiles?");
			((Control)val).set_Size(new Point(500, 190));
			((Control)val).set_Location(GetCenteredPopupLocation(popupParent, 500, 190));
			((Control)val).set_Parent(popupParent);
			((Control)val).set_BackgroundColor(new Color(38, 35, 32));
			((Control)val).set_ClipsBounds(false);
			((Control)val).set_ZIndex(100);
			_matureConfirmationPanel = val;
			StandardButton val2 = new StandardButton();
			val2.set_Text("X");
			((Control)val2).set_Location(new Point(468, -28));
			((Control)val2).set_Size(new Point(24, 24));
			((Control)val2).set_Parent((Container)(object)_matureConfirmationPanel);
			((Control)val2).set_ClipsBounds(false);
			((Control)val2).set_ZIndex(10011);
			((Control)val2).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				CloseMatureConfirmation();
			});
			Label val3 = new Label();
			val3.set_Text("Enabling this will allow you to view profiles marked as mature/18+. These profiles may contain explicit details not suitable for minors." + Environment.NewLine + Environment.NewLine + "Are you sure you want to continue?");
			val3.set_Font(GameService.Content.get_DefaultFont14());
			val3.set_TextColor(Color.get_White());
			val3.set_WrapText(true);
			((Control)val3).set_Location(new Point(16, 6));
			((Control)val3).set_Size(new Point(468, 92));
			((Control)val3).set_Parent((Container)(object)_matureConfirmationPanel);
			StandardButton val4 = new StandardButton();
			val4.set_Text("Show Mature Profiles");
			((Control)val4).set_Location(new Point(200, 108));
			((Control)val4).set_Size(new Point(165, 32));
			((Control)val4).set_Parent((Container)(object)_matureConfirmationPanel);
			((Control)val4).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				SetMatureProfilesEnabled(enabled: true);
				CloseMatureConfirmation();
			});
			StandardButton val5 = new StandardButton();
			val5.set_Text("No");
			((Control)val5).set_Location(new Point(379, 108));
			((Control)val5).set_Size(new Point(105, 32));
			((Control)val5).set_Parent((Container)(object)_matureConfirmationPanel);
			((Control)val5).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				CloseMatureConfirmation();
			});
		}

		private void CloseMatureConfirmation()
		{
			Panel matureConfirmationPanel = _matureConfirmationPanel;
			if (matureConfirmationPanel != null)
			{
				((Control)matureConfirmationPanel).Dispose();
			}
			_matureConfirmationPanel = null;
		}

		private string GetMatureProfilesButtonText()
		{
			if (!_settings.ShowMatureProfiles.get_Value())
			{
				return "Mature Profiles Hidden";
			}
			return "Mature Profiles Visible";
		}

		private void ToggleMatureProfiles()
		{
			if (_settings.ShowMatureProfiles.get_Value())
			{
				SetMatureProfilesEnabled(enabled: false);
			}
			else
			{
				OpenMatureConfirmation();
			}
		}

		private static Point GetCenteredPopupLocation(Container parent, int width, int height)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			Point size;
			if (parent == null)
			{
				size = ((Control)GameService.Graphics.get_SpriteScreen()).get_Size();
			}
			else
			{
				Rectangle contentRegion = parent.get_ContentRegion();
				size = ((Rectangle)(ref contentRegion)).get_Size();
			}
			int x = (size.X - width) / 2;
			int y = (size.Y - height) / 2;
			return new Point(Math.Max(8, x), Math.Max(8, y));
		}

		private string GetImportantNotice()
		{
			try
			{
				return _getImportantNotice?.Invoke() ?? string.Empty;
			}
			catch
			{
				return string.Empty;
			}
		}

		private static string ServerStatusText(ServerSyncStatus status)
		{
			if (status == null)
			{
				return "Disconnected";
			}
			if (string.IsNullOrWhiteSpace(status.DisplayName))
			{
				return status.Message;
			}
			if (!string.IsNullOrWhiteSpace(status.Message))
			{
				return status.DisplayName + ": " + status.Message;
			}
			return status.DisplayName;
		}

		protected override void Unload()
		{
			_isUnloaded = true;
			CloseMatureConfirmation();
			_buildPanel = null;
			StopNoticeRefresh();
			_buttons.Dispose();
			_unwatchBlockedAccountsChanged?.Invoke(OnBlockedAccountsChanged);
			UnwatchServer();
			UnwatchGameState();
		}
	}
}
