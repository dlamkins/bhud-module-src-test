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

		private readonly Func<string> _getImportantNotice;

		private readonly SparkSettingsButtons _buttons;

		private readonly Action _openSettings;

		private readonly Action _openBlocklist;

		private readonly Action<Action> _watchBlockedAccountsChanged;

		private readonly Action<Action> _unwatchBlockedAccountsChanged;

		private readonly MatureProfilesConfirmation _matureProfilesConfirm;

		private Label _blockedAccountsLabel;

		private Dropdown _statusDropdown;

		private Dropdown _regionDropdown;

		private Container _buildPanel;

		private bool _isUnloaded;

		private Label _serverStatusLabel;

		private Label _readinessLabel;

		private static readonly TimeSpan NoticeRefreshInterval = TimeSpan.FromMilliseconds(500.0);

		private CancellationTokenSource _noticeRefreshCancel;

		private Task _noticeRefreshTask;

		public SparkSettingsView(Action openProfileManager, Action openProfileViewer, Action openOnlineList, Action openNearby, Action openSavedProfiles, Action openAbout, Action openSettings, Action openBlocklist, Func<Task<string>> waitForInitialState, Func<string> getCurrentStateMessage, Action requestStateRefresh, SparkSettings settings, Func<ServerSyncStatus> getServerSyncStatus, Action<Action<ServerSyncStatus>> watchServerSyncStatus, Action<Action<ServerSyncStatus>> unwatchServerSyncStatus, Action requestServerSync, Func<string> getImportantNotice, Func<bool> shouldHideGameplayWindows, Action<Action> watchBlockedAccountsChanged, Action<Action> unwatchBlockedAccountsChanged, Action<bool> maturePreferenceChanged)
			: this()
		{
			_settings = settings;
			_getServerSyncStatus = getServerSyncStatus;
			_watchServerSyncStatus = watchServerSyncStatus;
			_unwatchServerSyncStatus = unwatchServerSyncStatus;
			_requestServerSync = requestServerSync;
			_getImportantNotice = getImportantNotice;
			_buttons = new SparkSettingsButtons(openProfileManager, openProfileViewer, openOnlineList, openNearby, openSavedProfiles, openAbout, GetMatureProfilesButtonText, ToggleMatureProfiles, waitForInitialState, getCurrentStateMessage, requestStateRefresh, shouldHideGameplayWindows);
			_openSettings = openSettings;
			_openBlocklist = openBlocklist;
			_watchBlockedAccountsChanged = watchBlockedAccountsChanged;
			_unwatchBlockedAccountsChanged = unwatchBlockedAccountsChanged;
			_matureProfilesConfirm = new MatureProfilesConfirmation(settings, maturePreferenceChanged);
		}

		protected override void Build(Container buildPanel)
		{
			_isUnloaded = false;
			_buildPanel = buildPanel;
			BuildSettings(buildPanel);
			WatchServer();
			WatchGameState();
			WatchSettings();
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
			((Control)SparkFormLayout.AddButton((Container)(object)SparkFormLayout.AddRow((Container)(object)settingsStack, 660, 30, 8), "Settings", 110, 30)).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_openSettings?.Invoke();
			});
			SparkFormLayout.AddSpacer((Container)(object)settingsStack, 660, 4);
			BuildPresence(settingsStack);
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
			if (_settings.CurrentStatus.get_Value() != RPStatus.Offline)
			{
				_settings.CurrentStatus.get_Value();
			}
			FlowPanel statusRow = SparkFormLayout.AddRow((Container)(object)settingsStack, 660, 30, 8);
			SparkFormLayout.AddLabel((Container)(object)statusRow, "Status:", 55, 30, GameService.Content.get_DefaultFont14());
			_statusDropdown = SparkFormLayout.AddDropdown((Container)(object)statusRow, ProfileLabels.RpStatusOptions, ProfileLabels.StatusLabel(_settings.CurrentStatus.get_Value()), 155, 30);
			_statusDropdown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				RPStatus rPStatus = ProfileLabels.ParseStatus(_statusDropdown.get_SelectedItem()?.ToString());
				if (_settings.CurrentStatus.get_Value() != rPStatus)
				{
					_settings.CurrentStatus.set_Value(rPStatus);
					_requestServerSync?.Invoke();
				}
			});
			SyncStatusDropdownFromSettings();
			SparkFormLayout.AddLabel((Container)(object)statusRow, "Region:", 58, 30, GameService.Content.get_DefaultFont14());
			_regionDropdown = SparkFormLayout.AddDropdown((Container)(object)statusRow, new string[2]
			{
				ProfileRegion.NA.ToString(),
				ProfileRegion.EU.ToString()
			}, _settings.RegionFilter.get_Value().ToString(), 90, 30);
			_regionDropdown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				if (Enum.TryParse<ProfileRegion>(_regionDropdown.get_SelectedItem()?.ToString(), out var result) && _settings.RegionFilter.get_Value() != result)
				{
					_settings.RegionFilter.set_Value(result);
					_requestServerSync?.Invoke();
				}
			});
			SyncRegionDropdownFromSettings();
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

		private void WatchSettings()
		{
			_settings.CurrentStatus.add_SettingChanged((EventHandler<ValueChangedEventArgs<RPStatus>>)OnCurrentStatusChanged);
			_settings.RegionFilter.add_SettingChanged((EventHandler<ValueChangedEventArgs<ProfileRegion>>)OnRegionFilterChanged);
			_settings.ShowMatureProfiles.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnMatureProfilesChanged);
		}

		private void UnwatchSettings()
		{
			_settings.CurrentStatus.remove_SettingChanged((EventHandler<ValueChangedEventArgs<RPStatus>>)OnCurrentStatusChanged);
			_settings.RegionFilter.remove_SettingChanged((EventHandler<ValueChangedEventArgs<ProfileRegion>>)OnRegionFilterChanged);
			_settings.ShowMatureProfiles.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnMatureProfilesChanged);
		}

		private void OnCurrentStatusChanged(object sender, ValueChangedEventArgs<RPStatus> e)
		{
			SparkUiThread.Queue(delegate
			{
				if (!_isUnloaded)
				{
					SyncStatusDropdownFromSettings();
				}
			});
		}

		private void OnRegionFilterChanged(object sender, ValueChangedEventArgs<ProfileRegion> e)
		{
			SparkUiThread.Queue(delegate
			{
				if (!_isUnloaded)
				{
					SyncRegionDropdownFromSettings();
				}
			});
		}

		private void OnMatureProfilesChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			SparkUiThread.Queue(delegate
			{
				if (!_isUnloaded)
				{
					_buttons.RefreshMatureButtonText();
				}
			});
		}

		private void SyncRegionDropdownFromSettings()
		{
			if (_regionDropdown != null)
			{
				string label = _settings.RegionFilter.get_Value().ToString();
				if (!string.Equals(_regionDropdown.get_SelectedItem()?.ToString(), label, StringComparison.Ordinal))
				{
					_regionDropdown.set_SelectedItem(label);
				}
			}
		}

		private void SyncStatusDropdownFromSettings()
		{
			if (_statusDropdown != null)
			{
				string label = ProfileLabels.StatusLabel((_settings.CurrentStatus.get_Value() != RPStatus.Offline) ? _settings.CurrentStatus.get_Value() : RPStatus.Online);
				if (!string.Equals(_statusDropdown.get_SelectedItem()?.ToString(), label, StringComparison.Ordinal))
				{
					_statusDropdown.set_SelectedItem(label);
				}
			}
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

		private string GetMatureProfilesButtonText()
		{
			return _matureProfilesConfirm.ButtonText;
		}

		private void ToggleMatureProfiles()
		{
			_matureProfilesConfirm.Toggle(_buildPanel);
			_buttons.RefreshMatureButtonText();
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
			_matureProfilesConfirm.Dispose();
			_buildPanel = null;
			StopNoticeRefresh();
			_buttons.Dispose();
			_unwatchBlockedAccountsChanged?.Invoke(OnBlockedAccountsChanged);
			UnwatchServer();
			UnwatchGameState();
			UnwatchSettings();
			_statusDropdown = null;
			_regionDropdown = null;
		}
	}
}
