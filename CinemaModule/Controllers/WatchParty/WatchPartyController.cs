using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using CinemaModule.Models.WatchParty;
using CinemaModule.Services.WatchParty;
using CinemaModule.Services.YouTube;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;

namespace CinemaModule.Controllers.WatchParty
{
	public class WatchPartyController : IDisposable
	{
		private static readonly Logger Logger = Logger.GetLogger<WatchPartyController>();

		private readonly WatchPartyConnectionManager _connectionManager;

		private readonly Gw2IdentityService _identityService;

		private readonly YouTubeService _youtubeService;

		private readonly object _stateLock = new object();

		private WatchPartyLocalState _currentState;

		private string _currentRoomId;

		private string _currentPassword;

		private bool _isDisposed;

		private double _localPlaybackTime;

		private MemberState _localMemberState;

		private MemberState _lastReportedMemberState;

		private ServerStatus _serverStatus;

		private string _serverVersion;

		private long _lastSequenceNumber;

		private double _measuredLatencySeconds;

		private bool? _pendingPlayState;

		private readonly object _pendingPlayStateLock = new object();

		public WatchPartyLocalState CurrentState
		{
			get
			{
				lock (_stateLock)
				{
					return _currentState;
				}
			}
		}

		public bool IsInRoom => _currentRoomId != null;

		public bool IsConnected => _connectionManager.IsConnected;

		public bool IsHost
		{
			get
			{
				lock (_stateLock)
				{
					return IsInRoom && _currentState?.HostUsername == LocalGw2Name;
				}
			}
		}

		public bool IsApiAvailable => _identityService.IsAvailable;

		public string LocalGw2Name => _identityService.AccountName;

		public double LocalPlaybackTime => _localPlaybackTime;

		public MemberState LocalMemberState => _localMemberState;

		public ServerStatus ServerStatus => _serverStatus;

		public string ServerVersion => _serverVersion;

		public double MeasuredLatencySeconds => _measuredLatencySeconds;

		public WatchPartyRoom CurrentRoom
		{
			get
			{
				lock (_stateLock)
				{
					WatchPartyLocalState state = _currentState;
					if (state == null || _currentRoomId == null)
					{
						return null;
					}
					return new WatchPartyRoom
					{
						RoomId = state.RoomId,
						RoomName = state.RoomName,
						Description = state.Description,
						HostUsername = state.HostUsername,
						MemberCount = (state.Members?.Count ?? 0)
					};
				}
			}
		}

		public event EventHandler<List<WatchPartyRoom>> RoomsUpdated;

		public event EventHandler<WatchPartyStateArgs> StateChanged;

		public event EventHandler RoomJoined;

		public event EventHandler RoomLeft;

		public event EventHandler<bool> HostStatusChanged;

		public event EventHandler<string> Reconnecting;

		public event EventHandler Reconnected;

		public event EventHandler ConnectionLost;

		public event EventHandler<string> ErrorOccurred;

		public event EventHandler<string> MemberBanned;

		public event EventHandler ApiAvailabilityChanged;

		public event EventHandler ServerStatusChanged;

		public WatchPartyController(Gw2ApiManager gw2ApiManager, YouTubeService youtubeService)
		{
			_identityService = new Gw2IdentityService(gw2ApiManager);
			_connectionManager = new WatchPartyConnectionManager();
			_youtubeService = youtubeService;
			_connectionManager.SetHandlerRegistration(RegisterHubHandlers);
			_identityService.ApiAvailabilityChanged += OnApiAvailabilityChanged;
			_connectionManager.ConnectionClosed += OnConnectionClosed;
			_connectionManager.ConnectionReconnecting += OnConnectionReconnecting;
			_connectionManager.ConnectionReconnected += OnConnectionReconnected;
		}

		private void OnApiAvailabilityChanged(object sender, EventArgs e)
		{
			this.ApiAvailabilityChanged?.Invoke(this, EventArgs.Empty);
		}

		private async Task EnsureConnectedAsync()
		{
			if (!_connectionManager.IsConnected)
			{
				await _connectionManager.EnsureConnectedAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
		}

		private void RegisterHubHandlers(HubConnection conn)
		{
			conn.On("ReceiveFullState", (Action<WatchPartyState>)OnFullStateReceived);
			conn.On("ReceiveSyncUpdate", (Action<SyncPayload>)OnSyncUpdateReceived);
			conn.On("ReceivePlayStateUpdate", (Action<SyncPayload>)OnPlayStateUpdateReceived);
			conn.On("ReceiveQueueUpdate", (Action<List<QueueItem>>)OnQueueUpdateReceived);
			conn.On("ReceiveMemberTimes", (Action<Dictionary<string, double>>)OnMemberTimesReceived);
			conn.On("ReceiveMemberStates", (Action<Dictionary<string, MemberState>>)OnMemberStatesReceived);
			conn.On("ForceLoadVideo", (Action<string>)OnForceLoadVideo);
			conn.On("ReceiveRoomListUpdate", (Action<List<WatchPartyRoom>>)OnRoomListUpdateReceived);
			conn.On("RoomDeleted", (Action)OnRoomDeleted);
			conn.On("MemberBanned", (Action<string>)OnMemberBanned);
		}

		private void OnRoomListUpdateReceived(List<WatchPartyRoom> rooms)
		{
			if (!_isDisposed)
			{
				this.RoomsUpdated?.Invoke(this, rooms ?? new List<WatchPartyRoom>());
			}
		}

		private Task OnConnectionClosed(Exception error)
		{
			if (_isDisposed)
			{
				return Task.CompletedTask;
			}
			Logger.Warn(error, "Connection closed while in room, clearing state");
			ClearRoomState();
			this.ConnectionLost?.Invoke(this, EventArgs.Empty);
			return Task.CompletedTask;
		}

		private Task OnConnectionReconnecting(Exception error)
		{
			if (_isDisposed)
			{
				return Task.CompletedTask;
			}
			Logger.Warn(error, "Connection reconnecting");
			this.Reconnecting?.Invoke(this, error?.Message ?? "Connection lost");
			return Task.CompletedTask;
		}

		private async Task OnConnectionReconnected(string connectionId)
		{
			if (_isDisposed)
			{
				return;
			}
			if (_currentRoomId != null && !string.IsNullOrEmpty(LocalGw2Name))
			{
				try
				{
					await _connectionManager.InvokeAsync<WatchPartyRoom>("JoinRoom", new object[3] { _currentRoomId, LocalGw2Name, _currentPassword }).ConfigureAwait(continueOnCapturedContext: false);
					await _connectionManager.InvokeAsync("RequestState", _currentRoomId).ConfigureAwait(continueOnCapturedContext: false);
				}
				catch (Exception ex)
				{
					Logger.Error(ex, "Failed to rejoin room " + _currentRoomId + " after reconnection");
					ClearRoomState();
					this.RoomLeft?.Invoke(this, EventArgs.Empty);
				}
			}
			this.Reconnected?.Invoke(this, EventArgs.Empty);
		}

		public async Task CheckServerStatusAsync()
		{
			ServerStatus previousStatus = _serverStatus;
			_serverStatus = ServerStatus.Checking;
			try
			{
				await EnsureConnectedAsync().ConfigureAwait(continueOnCapturedContext: false);
				_serverVersion = await _connectionManager.InvokeAsync<string>("GetVersion", Array.Empty<object>()).ConfigureAwait(continueOnCapturedContext: false);
				string clientVersion = "2.0.0";
				_serverStatus = ((string.IsNullOrEmpty(_serverVersion) || _serverVersion == clientVersion) ? ServerStatus.Online : ServerStatus.VersionMismatch);
				if (_serverStatus == ServerStatus.VersionMismatch)
				{
					Logger.Warn("Server version mismatch: server=" + _serverVersion + ", client=" + clientVersion);
				}
			}
			catch (Exception ex)
			{
				Logger.Error("Failed to check server status: " + ex.Message);
				_serverStatus = ServerStatus.Offline;
				_serverVersion = null;
			}
			if (previousStatus != _serverStatus)
			{
				this.ServerStatusChanged?.Invoke(this, EventArgs.Empty);
			}
		}

		public async Task RefreshRoomsAsync()
		{
			await EnsureConnectedAsync().ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				List<WatchPartyRoom> rooms = await _connectionManager.InvokeAsync<List<WatchPartyRoom>>("GetRooms", Array.Empty<object>()).ConfigureAwait(continueOnCapturedContext: false);
				this.RoomsUpdated?.Invoke(this, rooms);
			}
			catch (Exception ex)
			{
				Logger.Error("Failed to get rooms: " + ex.Message);
				this.RoomsUpdated?.Invoke(this, new List<WatchPartyRoom>());
			}
		}

		public async Task<bool> CreateRoomAsync(string name, bool isPrivate, string password, WatchPartySharedLocation sharedLocation = null)
		{
			if (!(await _identityService.EnsureAvailableAsync().ConfigureAwait(continueOnCapturedContext: false)))
			{
				this.ErrorOccurred?.Invoke(this, "GW2 API key with Account permission required.");
				return false;
			}
			try
			{
				await EnsureConnectedAsync().ConfigureAwait(continueOnCapturedContext: false);
				_currentRoomId = (await _connectionManager.InvokeAsync<WatchPartyRoom>("CreateRoom", new object[5] { name, isPrivate, password, LocalGw2Name, sharedLocation }).ConfigureAwait(continueOnCapturedContext: false)).RoomId;
				_currentPassword = (isPrivate ? password : null);
				lock (_stateLock)
				{
					_currentState = null;
				}
				ResetMemberStateTracking();
				this.RoomJoined?.Invoke(this, EventArgs.Empty);
				await RequestStateAsync().ConfigureAwait(continueOnCapturedContext: false);
				return true;
			}
			catch (Exception ex)
			{
				Logger.Error("Failed to create room: " + ex.Message);
				this.ErrorOccurred?.Invoke(this, "Failed to create room: " + ex.Message);
				return false;
			}
		}

		public async Task<bool> JoinRoomAsync(string roomId, string password = null)
		{
			if (!(await _identityService.EnsureAvailableAsync().ConfigureAwait(continueOnCapturedContext: false)))
			{
				this.ErrorOccurred?.Invoke(this, "GW2 API key with Account permission required.");
				return false;
			}
			try
			{
				await EnsureConnectedAsync().ConfigureAwait(continueOnCapturedContext: false);
				if (IsInRoom && _currentRoomId != roomId)
				{
					await TryLeaveCurrentRoomAsync().ConfigureAwait(continueOnCapturedContext: false);
				}
				await _connectionManager.InvokeAsync<WatchPartyRoom>("JoinRoom", new object[3] { roomId, LocalGw2Name, password }).ConfigureAwait(continueOnCapturedContext: false);
				_currentRoomId = roomId;
				_currentPassword = password;
				lock (_stateLock)
				{
					_currentState = null;
				}
				ResetMemberStateTracking();
				this.RoomJoined?.Invoke(this, EventArgs.Empty);
				await RequestStateAsync().ConfigureAwait(continueOnCapturedContext: false);
				return true;
			}
			catch (Exception ex)
			{
				Logger.Error("Failed to join room: " + ex.Message);
				this.ErrorOccurred?.Invoke(this, "Failed to join room.");
				return false;
			}
		}

		private async Task TryLeaveCurrentRoomAsync()
		{
			try
			{
				await _connectionManager.InvokeAsync("LeaveRoom", _currentRoomId).ConfigureAwait(continueOnCapturedContext: false);
			}
			catch (Exception ex)
			{
				Logger.Warn("Error leaving current room: " + ex.Message);
			}
			ClearRoomState();
			this.RoomLeft?.Invoke(this, EventArgs.Empty);
		}

		public async Task LeaveRoomAsync()
		{
			if (IsInRoom)
			{
				try
				{
					await _connectionManager.InvokeAsync("LeaveRoom", _currentRoomId).ConfigureAwait(continueOnCapturedContext: false);
				}
				catch (Exception ex)
				{
					Logger.Warn("Error leaving room: " + ex.Message);
				}
				ClearRoomState();
				this.RoomLeft?.Invoke(this, EventArgs.Empty);
			}
		}

		public async Task DeleteRoomAsync()
		{
			if (IsHost && IsInRoom && IsConnected)
			{
				try
				{
					await _connectionManager.InvokeAsync("DeleteRoom", _currentRoomId).ConfigureAwait(continueOnCapturedContext: false);
				}
				catch (Exception ex)
				{
					Logger.Warn("Error deleting room: " + ex.Message);
				}
				ClearRoomState();
			}
		}

		public async Task UpdateRoomAsync(string roomName, string description)
		{
			if (IsHost && IsInRoom && IsConnected)
			{
				try
				{
					await _connectionManager.InvokeAsync("UpdateRoom", _currentRoomId, roomName, description).ConfigureAwait(continueOnCapturedContext: false);
				}
				catch (Exception ex)
				{
					Logger.Error("Failed to update room: " + ex.Message);
				}
			}
		}

		public async Task TogglePlaybackAsync()
		{
			WatchPartyLocalState state;
			lock (_stateLock)
			{
				state = _currentState;
			}
			if (!IsHost || state == null || !IsConnected)
			{
				return;
			}
			bool currentPlayState;
			lock (_pendingPlayStateLock)
			{
				currentPlayState = _pendingPlayState ?? state.IsPlaying;
				_pendingPlayState = !currentPlayState;
			}
			bool newPlayState = !currentPlayState;
			try
			{
				await _connectionManager.InvokeAsync("SetPlayState", _currentRoomId, newPlayState).ConfigureAwait(continueOnCapturedContext: false);
			}
			catch (Exception ex)
			{
				lock (_pendingPlayStateLock)
				{
					_pendingPlayState = null;
				}
				Logger.Error("Failed to toggle playback: " + ex.Message);
			}
		}

		public async Task SyncSeekAsync(double timestampSeconds)
		{
			WatchPartyLocalState state;
			lock (_stateLock)
			{
				state = _currentState;
			}
			if (IsHost && state != null && IsConnected)
			{
				bool isPlaying;
				lock (_pendingPlayStateLock)
				{
					isPlaying = _pendingPlayState ?? state.IsPlaying;
				}
				SyncPayload payload = new SyncPayload
				{
					Timestamp = timestampSeconds,
					IsPlaying = isPlaying
				};
				await SendSyncPayloadAsync(payload).ConfigureAwait(continueOnCapturedContext: false);
			}
		}

		private async Task SendSyncPayloadAsync(SyncPayload payload)
		{
			if (!_isDisposed)
			{
				try
				{
					await _connectionManager.InvokeAsync("UpdatePlayback", _currentRoomId, payload).ConfigureAwait(continueOnCapturedContext: false);
				}
				catch (Exception ex)
				{
					Logger.Error("Failed to sync seek: " + ex.Message);
				}
			}
		}

		public async Task PlayNextInQueueAsync()
		{
			if (IsHost && IsConnected)
			{
				try
				{
					await _connectionManager.InvokeAsync("PlayNextInQueue", _currentRoomId).ConfigureAwait(continueOnCapturedContext: false);
				}
				catch (Exception ex)
				{
					Logger.Error("Failed to play next: " + ex.Message);
				}
			}
		}

		public async Task<bool> AddVideoAsync(string videoIdOrUrl, string addedBy = null)
		{
			if (!IsInRoom || !IsConnected)
			{
				return false;
			}
			string videoId = YouTubeService.ExtractVideoId(videoIdOrUrl) ?? videoIdOrUrl;
			if (await _youtubeService.GetVideoInfoAsync(videoId).ConfigureAwait(continueOnCapturedContext: false) == null)
			{
				Logger.Warn("Invalid YouTube video: " + videoIdOrUrl);
				this.ErrorOccurred?.Invoke(this, "Invalid or unavailable YouTube video.");
				return false;
			}
			QueueItem item = new QueueItem
			{
				VideoId = videoId,
				AddedBy = (addedBy ?? LocalGw2Name)
			};
			try
			{
				await _connectionManager.InvokeAsync("EnqueueVideo", _currentRoomId, item).ConfigureAwait(continueOnCapturedContext: false);
				return true;
			}
			catch (HubException ex2)
			{
				Logger.Warn("Failed to add video: " + ex2.Message);
				this.ErrorOccurred?.Invoke(this, ex2.Message);
				return false;
			}
			catch (Exception ex)
			{
				Logger.Error("Failed to add video: " + ex.Message);
				this.ErrorOccurred?.Invoke(this, "Failed to add video to queue.");
				return false;
			}
		}

		public async Task RemoveFromQueueAsync(int index)
		{
			if (IsHost && IsConnected)
			{
				try
				{
					await _connectionManager.InvokeAsync("RemoveFromQueue", _currentRoomId, index).ConfigureAwait(continueOnCapturedContext: false);
				}
				catch (Exception ex)
				{
					Logger.Error("Failed to remove from queue: " + ex.Message);
				}
			}
		}

		public async Task ReorderQueueAsync(int fromIndex, int toIndex)
		{
			if (IsHost && IsConnected)
			{
				try
				{
					await _connectionManager.InvokeAsync("ReorderQueue", _currentRoomId, fromIndex, toIndex).ConfigureAwait(continueOnCapturedContext: false);
				}
				catch (Exception ex)
				{
					Logger.Error("Failed to reorder queue: " + ex.Message);
				}
			}
		}

		public async Task BanMemberAsync(string username)
		{
			if (IsHost && IsConnected && !string.IsNullOrWhiteSpace(username))
			{
				try
				{
					await _connectionManager.InvokeAsync("BanMember", _currentRoomId, username).ConfigureAwait(continueOnCapturedContext: false);
				}
				catch (Exception ex)
				{
					Logger.Error("Failed to ban member: " + ex.Message);
				}
			}
		}

		public async Task UpdateMaxQueuePerUserAsync(int maxQueuePerUser)
		{
			if (IsHost && IsConnected)
			{
				try
				{
					await _connectionManager.InvokeAsync("UpdateMaxQueuePerUser", _currentRoomId, maxQueuePerUser).ConfigureAwait(continueOnCapturedContext: false);
				}
				catch (Exception ex)
				{
					Logger.Error("Failed to update max queue per user: " + ex.Message);
				}
			}
		}

		public async Task ReportPlaybackTimeAsync(double currentTime)
		{
			if (IsInRoom && IsConnected)
			{
				_localPlaybackTime = currentTime;
				try
				{
					await _connectionManager.InvokeAsync("ReportPlaybackTime", _currentRoomId, currentTime).ConfigureAwait(continueOnCapturedContext: false);
				}
				catch (Exception ex)
				{
					Logger.Debug("Failed to report playback time: " + ex.Message);
				}
			}
		}

		public async Task ReportMemberStateAsync(MemberState state)
		{
			if (IsInRoom && IsConnected && state != _lastReportedMemberState)
			{
				_localMemberState = state;
				_lastReportedMemberState = state;
				try
				{
					await _connectionManager.InvokeAsync("ReportMemberState", _currentRoomId, state).ConfigureAwait(continueOnCapturedContext: false);
				}
				catch (Exception ex)
				{
					Logger.Debug("Failed to report member state: " + ex.Message);
				}
			}
		}

		public async Task RequestStateAsync()
		{
			if (IsInRoom && IsConnected)
			{
				try
				{
					await _connectionManager.InvokeAsync("RequestState", _currentRoomId).ConfigureAwait(continueOnCapturedContext: false);
				}
				catch (Exception ex)
				{
					Logger.Debug("Failed to request state: " + ex.Message);
				}
			}
		}

		public async Task MeasureLatencyAsync()
		{
			if (IsConnected)
			{
				try
				{
					DateTime startTime = DateTime.UtcNow;
					await _connectionManager.InvokeAsync<long>("Ping", Array.Empty<object>()).ConfigureAwait(continueOnCapturedContext: false);
					double roundTripMs = (DateTime.UtcNow - startTime).TotalMilliseconds;
					_measuredLatencySeconds = roundTripMs / 2000.0;
				}
				catch (Exception ex)
				{
					Logger.Debug("Failed to measure latency: " + ex.Message);
				}
			}
		}

		private void OnFullStateReceived(WatchPartyState serverState)
		{
			if (!_isDisposed && !IsStaleSequence(serverState.SequenceNumber))
			{
				WatchPartyLocalState newState = WatchPartyLocalState.FromServerState(serverState);
				WatchPartyLocalState currentState;
				lock (_stateLock)
				{
					currentState = _currentState;
				}
				if (newState.HasPlaybackDifference(currentState) || newState.HasMembershipDifference(currentState))
				{
					_lastSequenceNumber = serverState.SequenceNumber;
					UpdateState(newState, WatchPartyStateChangeType.FullStateReceived);
				}
			}
		}

		private void OnSyncUpdateReceived(SyncPayload payload)
		{
			if (_isDisposed || IsStaleSequence(payload.SequenceNumber))
			{
				return;
			}
			_lastSequenceNumber = payload.SequenceNumber;
			lock (_pendingPlayStateLock)
			{
				if (_pendingPlayState == payload.IsPlaying)
				{
					_pendingPlayState = null;
				}
			}
			WatchPartyLocalState currentState;
			lock (_stateLock)
			{
				currentState = _currentState;
			}
			if (currentState != null)
			{
				WatchPartyLocalState newState = currentState.WithPlaybackUpdate(payload.Timestamp, payload.IsPlaying, payload.IsPlaying, payload.SequenceNumber);
				UpdateState(newState, WatchPartyStateChangeType.PlaybackUpdated);
			}
		}

		private void OnPlayStateUpdateReceived(SyncPayload payload)
		{
			if (_isDisposed || IsStaleSequence(payload.SequenceNumber))
			{
				return;
			}
			_lastSequenceNumber = payload.SequenceNumber;
			lock (_pendingPlayStateLock)
			{
				if (_pendingPlayState == payload.IsPlaying)
				{
					_pendingPlayState = null;
				}
			}
			WatchPartyLocalState currentState;
			lock (_stateLock)
			{
				currentState = _currentState;
			}
			if (currentState != null)
			{
				WatchPartyLocalState newState = currentState.WithPlayStateUpdate(payload.IsPlaying, payload.SequenceNumber);
				UpdateState(newState, WatchPartyStateChangeType.PlayStateChanged);
			}
		}

		private void OnQueueUpdateReceived(List<QueueItem> queue)
		{
			TryUpdateStateFromCurrent((WatchPartyLocalState state) => state.WithQueue(queue ?? new List<QueueItem>()), WatchPartyStateChangeType.QueueUpdated);
		}

		private void OnMemberTimesReceived(Dictionary<string, double> memberTimes)
		{
			TryUpdateStateFromCurrent((WatchPartyLocalState state) => state.WithMemberTimes(memberTimes ?? new Dictionary<string, double>()), WatchPartyStateChangeType.MemberTimesUpdated);
		}

		private void OnMemberStatesReceived(Dictionary<string, MemberState> memberStates)
		{
			TryUpdateStateFromCurrent((WatchPartyLocalState state) => state.WithMemberStates(memberStates ?? new Dictionary<string, MemberState>()), WatchPartyStateChangeType.MemberStatesUpdated);
		}

		private void OnForceLoadVideo(string videoId)
		{
			TryUpdateStateFromCurrent((WatchPartyLocalState state) => state.WithNewVideo(videoId), WatchPartyStateChangeType.VideoChanged);
		}

		private void OnRoomDeleted()
		{
			if (!_isDisposed)
			{
				ClearRoomState();
				this.RoomLeft?.Invoke(this, EventArgs.Empty);
			}
		}

		private void OnMemberBanned(string username)
		{
			if (!_isDisposed)
			{
				ClearRoomState();
				this.MemberBanned?.Invoke(this, username);
				this.RoomLeft?.Invoke(this, EventArgs.Empty);
			}
		}

		private bool IsStaleSequence(long sequenceNumber)
		{
			if (sequenceNumber <= 0)
			{
				return false;
			}
			return sequenceNumber < _lastSequenceNumber;
		}

		private void ClearRoomState()
		{
			_currentRoomId = null;
			_currentPassword = null;
			_lastSequenceNumber = 0L;
			lock (_stateLock)
			{
				_currentState = null;
			}
			lock (_pendingPlayStateLock)
			{
				_pendingPlayState = null;
			}
			ResetMemberStateTracking();
		}

		private void ResetMemberStateTracking()
		{
			_lastReportedMemberState = (MemberState)(-1);
		}

		private bool TryUpdateStateFromCurrent(Func<WatchPartyLocalState, WatchPartyLocalState> stateTransform, WatchPartyStateChangeType changeType)
		{
			if (_isDisposed)
			{
				return false;
			}
			WatchPartyLocalState currentState;
			lock (_stateLock)
			{
				currentState = _currentState;
			}
			if (currentState == null)
			{
				return false;
			}
			WatchPartyLocalState newState = stateTransform(currentState);
			UpdateState(newState, changeType);
			return true;
		}

		private void UpdateState(WatchPartyLocalState newState, WatchPartyStateChangeType changeType)
		{
			WatchPartyLocalState previousState;
			bool wasHost;
			bool isNowHost;
			lock (_stateLock)
			{
				previousState = _currentState;
				_currentState = newState;
				wasHost = previousState?.HostUsername == LocalGw2Name;
				isNowHost = newState?.HostUsername == LocalGw2Name;
			}
			if (wasHost != isNowHost)
			{
				this.HostStatusChanged?.Invoke(this, isNowHost);
			}
			this.StateChanged?.Invoke(this, new WatchPartyStateArgs(newState, previousState, changeType));
		}

		public async Task RefreshApiAvailabilityAsync()
		{
			await _identityService.FetchAccountNameAsync().ConfigureAwait(continueOnCapturedContext: false);
		}

		public string GetDiagnosticInfo()
		{
			string roomInfo = (IsInRoom ? (", RoomId: " + _currentRoomId) : "");
			return $"{_identityService.GetDiagnosticInfo()}, IsConnected: {IsConnected}, IsInRoom: {IsInRoom}{roomInfo}";
		}

		public void Dispose()
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				_identityService.ApiAvailabilityChanged -= OnApiAvailabilityChanged;
				_connectionManager.ConnectionClosed -= OnConnectionClosed;
				_connectionManager.ConnectionReconnecting -= OnConnectionReconnecting;
				_connectionManager.ConnectionReconnected -= OnConnectionReconnected;
				_identityService.Dispose();
				_connectionManager.Dispose();
			}
		}
	}
}
