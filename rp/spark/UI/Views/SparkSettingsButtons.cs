using System;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;

namespace rp.spark.UI.Views
{
	internal sealed class SparkSettingsButtons : IDisposable
	{
		private static readonly TimeSpan StatePollInterval = TimeSpan.FromSeconds(1.0);

		private readonly Action _openProfileManager;

		private readonly Action _openProfileViewer;

		private readonly Action _openOnlineList;

		private readonly Action _openSavedProfiles;

		private readonly Action _openAbout;

		private readonly Action _openNearby;

		private readonly Func<Task<string>> _waitForPlayerStateMessageAsync;

		private readonly Func<string> _getPlayerStateMessage;

		private readonly Action _reloadPlayerState;

		private readonly Func<string> _getMatureProfilesButtonText;

		private readonly Action _toggleMatureProfiles;

		private bool _isDisposed;

		private int _refreshId;

		private string _lastMessage;

		private CancellationTokenSource _statePollCancel;

		private Task _statePollTask;

		private StandardButton _openButton;

		private StandardButton _viewButton;

		private StandardButton _onlineListButton;

		private StandardButton _savedProfilesButton;

		private StandardButton _nearbyButton;

		private StandardButton _matureProfilesButton;

		private readonly Func<bool> _shouldHideGameplayWindows;

		public SparkSettingsButtons(Action openProfileManager, Action openProfileViewer, Action openOnlineList, Action openNearby, Action openSavedProfiles, Action openAbout, Func<string> getMatureProfilesButtonText, Action toggleMatureProfiles, Func<Task<string>> waitForPlayerStateMessageAsync, Func<string> getPlayerStateMessage, Action reloadPlayerState, Func<bool> shouldHideGameplayWindows)
		{
			_openProfileManager = openProfileManager;
			_openProfileViewer = openProfileViewer;
			_openOnlineList = openOnlineList;
			_openSavedProfiles = openSavedProfiles;
			_openAbout = openAbout;
			_openNearby = openNearby;
			_waitForPlayerStateMessageAsync = waitForPlayerStateMessageAsync;
			_getPlayerStateMessage = getPlayerStateMessage;
			_reloadPlayerState = reloadPlayerState;
			_shouldHideGameplayWindows = shouldHideGameplayWindows;
			_getMatureProfilesButtonText = getMatureProfilesButtonText;
			_toggleMatureProfiles = toggleMatureProfiles;
		}

		public void Build(Container buildPanel)
		{
			_isDisposed = false;
			FlowPanel parent = SparkFormLayout.AddAutoStack(buildPanel, 660, 6);
			FlowPanel firstRow = SparkFormLayout.AddRow((Container)(object)parent, 660, 30, 8);
			_openButton = SparkFormLayout.AddButton((Container)(object)firstRow, "Profile Editor", 122, 30, enabled: false);
			((Control)_openButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_openProfileManager();
			});
			_viewButton = SparkFormLayout.AddButton((Container)(object)firstRow, "My Profile", 110, 30, enabled: false);
			((Control)_viewButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_openProfileViewer();
			});
			_onlineListButton = SparkFormLayout.AddButton((Container)(object)firstRow, "Online List", 110, 30);
			((Control)_onlineListButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_openOnlineList();
			});
			_nearbyButton = SparkFormLayout.AddButton((Container)(object)firstRow, "Nearby Players", 126, 30);
			((Control)_nearbyButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_openNearby();
			});
			FlowPanel secondRow = SparkFormLayout.AddRow((Container)(object)parent, 660, 30, 8);
			_savedProfilesButton = SparkFormLayout.AddButton((Container)(object)secondRow, "Saved Profiles", 126, 30);
			((Control)_savedProfilesButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_openSavedProfiles();
			});
			((Control)SparkFormLayout.AddButton((Container)(object)secondRow, "About", 80, 30)).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_openAbout();
			});
			_matureProfilesButton = SparkFormLayout.AddButton((Container)(object)secondRow, MatureProfilesButtonText(), 190, 30);
			((Control)_matureProfilesButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_toggleMatureProfiles?.Invoke();
			});
			WatchGameState();
			StartStatePolling();
			RefreshButtonText();
			RefreshButtonState(reloadPlayerState: false);
		}

		private async Task RefreshButtonStateAsync(int refreshVersion)
		{
			string resultText;
			try
			{
				resultText = await _waitForPlayerStateMessageAsync();
			}
			catch
			{
				resultText = "Unavailable";
			}
			if (_isDisposed)
			{
				return;
			}
			SparkUiThread.Queue(delegate
			{
				if (!_isDisposed && refreshVersion == _refreshId)
				{
					ApplyButtonState(resultText);
					RefreshButtonText();
				}
			});
		}

		private void WatchGameState()
		{
			GameService.Gw2Mumble.add_IsAvailableChanged((EventHandler<ValueEventArgs<bool>>)OnGameStateChanged);
			((GameService)GameService.Gw2Mumble).add_FinishedLoading((EventHandler<EventArgs>)OnGameStateChanged);
			GameService.Gw2Mumble.get_PlayerCharacter().add_NameChanged((EventHandler<ValueEventArgs<string>>)OnGameStateChanged);
			GameService.Gw2Mumble.get_CurrentMap().add_MapChanged((EventHandler<ValueEventArgs<int>>)OnGameStateChanged);
			GameService.Gw2Mumble.get_UI().add_IsMapOpenChanged((EventHandler<ValueEventArgs<bool>>)OnGameStateChanged);
			GameService.GameIntegration.get_Gw2Instance().add_Gw2Started((EventHandler<EventArgs>)OnGameStateChanged);
			GameService.GameIntegration.get_Gw2Instance().add_Gw2Closed((EventHandler<EventArgs>)OnGameStateChanged);
			GameService.GameIntegration.get_Gw2Instance().add_IsInGameChanged((EventHandler<ValueEventArgs<bool>>)OnGameStateChanged);
		}

		private void UnwatchGameState()
		{
			GameService.Gw2Mumble.remove_IsAvailableChanged((EventHandler<ValueEventArgs<bool>>)OnGameStateChanged);
			((GameService)GameService.Gw2Mumble).remove_FinishedLoading((EventHandler<EventArgs>)OnGameStateChanged);
			GameService.Gw2Mumble.get_PlayerCharacter().remove_NameChanged((EventHandler<ValueEventArgs<string>>)OnGameStateChanged);
			GameService.Gw2Mumble.get_CurrentMap().remove_MapChanged((EventHandler<ValueEventArgs<int>>)OnGameStateChanged);
			GameService.Gw2Mumble.get_UI().remove_IsMapOpenChanged((EventHandler<ValueEventArgs<bool>>)OnGameStateChanged);
			GameService.GameIntegration.get_Gw2Instance().remove_Gw2Started((EventHandler<EventArgs>)OnGameStateChanged);
			GameService.GameIntegration.get_Gw2Instance().remove_Gw2Closed((EventHandler<EventArgs>)OnGameStateChanged);
			GameService.GameIntegration.get_Gw2Instance().remove_IsInGameChanged((EventHandler<ValueEventArgs<bool>>)OnGameStateChanged);
		}

		private void OnGameStateChanged(object sender, EventArgs e)
		{
			RefreshButtonState(reloadPlayerState: true);
		}

		private string RefreshButtonText()
		{
			if (_isDisposed || _openButton == null || _viewButton == null)
			{
				return _lastMessage;
			}
			try
			{
				string message = _getPlayerStateMessage?.Invoke() ?? string.Empty;
				ApplyButtonState(message);
				return message;
			}
			catch
			{
				ApplyButtonState("Unavailable");
				return "Unavailable";
			}
		}

		public void RefreshMatureButtonText()
		{
			if (_matureProfilesButton != null)
			{
				_matureProfilesButton.set_Text(MatureProfilesButtonText());
			}
		}

		private string MatureProfilesButtonText()
		{
			try
			{
				return _getMatureProfilesButtonText?.Invoke() ?? "Mature Profiles";
			}
			catch
			{
				return "Mature Profiles";
			}
		}

		private void ApplyButtonState(string resultText)
		{
			if (_openButton != null && _viewButton != null)
			{
				bool hideGameplayWindows = ShouldHideGameplayWindows();
				string unavailableMessage = resultText ?? string.Empty;
				bool canUseProfileTools = !hideGameplayWindows && string.IsNullOrWhiteSpace(unavailableMessage);
				_lastMessage = unavailableMessage;
				_openButton.set_Text("Profile Editor");
				((Control)_openButton).set_Enabled(canUseProfileTools);
				_viewButton.set_Text("My Profile");
				((Control)_viewButton).set_Enabled(canUseProfileTools);
				if (_onlineListButton != null)
				{
					((Control)_onlineListButton).set_Enabled(!hideGameplayWindows);
				}
				if (_savedProfilesButton != null)
				{
					((Control)_savedProfilesButton).set_Enabled(!hideGameplayWindows);
				}
				if (_nearbyButton != null)
				{
					((Control)_nearbyButton).set_Enabled(!hideGameplayWindows);
				}
			}
		}

		private bool ShouldHideGameplayWindows()
		{
			try
			{
				return _shouldHideGameplayWindows?.Invoke() ?? false;
			}
			catch
			{
				return false;
			}
		}

		public void Refresh()
		{
			if (!_isDisposed)
			{
				RefreshButtonState(reloadPlayerState: false);
			}
		}

		private void RefreshButtonState(bool reloadPlayerState)
		{
			if (_isDisposed)
			{
				return;
			}
			if (reloadPlayerState)
			{
				try
				{
					_reloadPlayerState?.Invoke();
				}
				catch
				{
				}
			}
			int refreshVersion = Interlocked.Increment(ref _refreshId);
			SparkUiThread.Queue(delegate
			{
				RefreshButtonText();
			});
			RefreshButtonStateAsync(refreshVersion);
		}

		private void StartStatePolling()
		{
			StopStatePolling();
			_statePollCancel = new CancellationTokenSource();
			_statePollTask = PollButtonStateAsync(_statePollCancel.Token);
		}

		private async Task PollButtonStateAsync(CancellationToken cancellationToken)
		{
			while (!cancellationToken.IsCancellationRequested)
			{
				try
				{
					await Task.Delay(StatePollInterval, cancellationToken);
				}
				catch (OperationCanceledException)
				{
					return;
				}
				if (_isDisposed)
				{
					break;
				}
				SparkUiThread.Queue(delegate
				{
					if (!_isDisposed)
					{
						string lastMessage = _lastMessage;
						string text = RefreshButtonText();
						if (!string.Equals(lastMessage, text, StringComparison.Ordinal) && string.IsNullOrWhiteSpace(text))
						{
							RefreshButtonState(reloadPlayerState: true);
						}
					}
				});
			}
		}

		private void StopStatePolling()
		{
			CancellationTokenSource cancellation = _statePollCancel;
			_statePollCancel = null;
			if (cancellation == null)
			{
				return;
			}
			try
			{
				cancellation.Cancel();
			}
			catch (ObjectDisposedException)
			{
			}
			finally
			{
				Task statePollTask = _statePollTask;
				_statePollTask = null;
				TaskCleanup.DisposeWhenComplete(statePollTask, cancellation);
			}
		}

		public void Dispose()
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				StopStatePolling();
				UnwatchGameState();
			}
		}
	}
}
