using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Ideka.NetCommon;
using Ideka.RacingMeter.Lib;
using Ideka.RacingMeter.Lib.RacingServer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ideka.RacingMeter
{
	public class RacingClient : IDisposable
	{
		public enum ClientState
		{
			Offline,
			Connecting,
			Online
		}

		private class Internal : IRacingHubClient, IDisposable
		{
			private readonly RacingClient _outer;

			public readonly HubConnection Conn;

			public Internal(RacingClient outer)
			{
				_outer = outer;
				string url = ((RacingModule.Settings.OnlineUrl.Value != "") ? RacingModule.Settings.OnlineUrl.Value : RacingModule.Server.RacingUrl) ?? "";
				if (url != "")
				{
					GameService.Graphics.QueueMainThreadRender((Action<GraphicsDevice>)delegate
					{
						ScreenNotification.ShowNotification(StringExtensions.Format(Strings.NotifyConnectingTo, url), (NotificationType)0, (Texture2D)null, 2);
					});
				}
				Conn = new HubConnectionBuilder().WithUrl(url).AddNewtonsoftJsonProtocol(delegate(NewtonsoftJsonHubProtocolOptions options)
				{
					JsonSerialization.Configure(options.PayloadSerializerSettings);
				}).Build();
				Conn.Closed += delegate
				{
					_outer.OnConnectionClosed();
					return Task.CompletedTask;
				};
				Conn.On("Pong", new Func<Task>(Pong));
				Conn.On("Error", new Func<string, Task>(Error));
				Conn.On("Validate", new Func<string, Task>(Validate));
				Conn.On("Authenticate", new Func<string, Task>(Authenticate));
				Conn.On("JoinLobby", new Func<Lobby, Task>(JoinLobby));
				Conn.On("LeaveLobby", new Func<Task>(LeaveLobby));
				Conn.On("UpdateUser", new Func<User, bool, Task>(UpdateUser));
				Conn.On("UpdateLobbySettings", new Func<LobbySettings, Task>(UpdateLobbySettings));
				Conn.On("UpdateLobbyRace", new Func<FullRace, Task>(UpdateLobbyRace));
				Conn.On("StartCountdown", new Func<int, Task>(StartCountdown));
				Conn.On("StartRace", new Func<long, Task>(StartRace));
				Conn.On("CancelRace", new Func<Task>(CancelRace));
				Conn.On("RaceFinished", new Func<Task>(RaceFinished));
				Conn.On("UpdatePosition", new Func<string, int, Vector3, Vector3, Task>(UpdatePosition));
			}

			public Task Pong()
			{
				_outer.Pong();
				return Task.CompletedTask;
			}

			public Task Error(string message)
			{
				_outer.Error(message);
				return Task.CompletedTask;
			}

			public Task Validate(string version)
			{
				_outer.Validate(version);
				return Task.CompletedTask;
			}

			public Task Authenticate(string userId)
			{
				_outer.Authenticate(userId);
				return Task.CompletedTask;
			}

			public Task GetLobbies(IDictionary<string, Lobby> lobbies)
			{
				_outer.GetLobbies(lobbies);
				return Task.CompletedTask;
			}

			public Task JoinLobby(Lobby lobby)
			{
				_outer.JoinLobby(lobby);
				return Task.CompletedTask;
			}

			public Task LeaveLobby()
			{
				_outer.LeaveLobby();
				return Task.CompletedTask;
			}

			public Task UpdateUser(User user, bool leaving)
			{
				_outer.UpdateUser(user, leaving);
				return Task.CompletedTask;
			}

			public Task UpdateLobbySettings(LobbySettings settings)
			{
				_outer.UpdateLobbySettings(settings);
				return Task.CompletedTask;
			}

			public Task UpdateLobbyRace(FullRace? fullRace)
			{
				_outer.UpdateLobbyRace(fullRace);
				return Task.CompletedTask;
			}

			public Task StartCountdown(int seconds)
			{
				_outer.StartCountdown(seconds);
				return Task.CompletedTask;
			}

			public Task StartRace(long time)
			{
				_outer.StartRace(time);
				return Task.CompletedTask;
			}

			public Task CancelRace()
			{
				_outer.CancelRace();
				return Task.CompletedTask;
			}

			public Task UpdatePosition(string userId, int mapId, Vector3 position, Vector3 front)
			{
				//IL_0008: Unknown result type (might be due to invalid IL or missing references)
				//IL_0009: Unknown result type (might be due to invalid IL or missing references)
				_outer.UpdatePosition(userId, mapId, position, front);
				return Task.CompletedTask;
			}

			public Task RaceFinished()
			{
				_outer.RaceFinished();
				return Task.CompletedTask;
			}

			public void Dispose()
			{
				Conn?.DisposeAsync();
			}
		}

		private static readonly Logger Logger = Logger.GetLogger<RacingClient>();

		private Lobby? _lobby;

		public ClientState _state;

		private readonly DisposableCollection _dc = new DisposableCollection();

		private readonly object _lock = new object();

		private DateTime _lastPing = DateTime.MinValue;

		private readonly DropOutStack<TimeSpan> _latencies = new DropOutStack<TimeSpan>(5);

		private readonly MeasurerRealtime _measurer;

		private CancellationTokenSource? _cts;

		private CancellationTokenSource? _ping;

		private Internal? _int;

		private const int MaxUpdatesPerSecond = 30;

		private DateTime _lastUpdate;

		public RacingServer Server { get; }

		public RaceRouteData? Route { get; private set; }

		public Lobby? Lobby
		{
			get
			{
				return _lobby;
			}
			private set
			{
				if (_lobby != value)
				{
					_lobby = value;
					Route = ((_lobby?.FullRace?.Race == null) ? null : new RaceRouteData(_lobby!.FullRace!.Race));
					this.LobbyChanged?.Invoke(value);
					this.LobbyRaceUpdated?.Invoke(value?.FullRace);
					this.LobbySettingsUpdated?.Invoke(value);
				}
			}
		}

		public string? UserId { get; private set; }

		public User? User
		{
			get
			{
				string id = UserId;
				User user = default(User);
				if (id == null || !(Lobby?.Users.TryGetValue(id, out user) ?? false))
				{
					return null;
				}
				return user;
			}
		}

		public ClientState State
		{
			get
			{
				return _state;
			}
			private set
			{
				if (_state != value)
				{
					_state = value;
					this.StateChanged?.Invoke(value);
				}
			}
		}

		public TimeSpan Latency => TimeSpan.FromTicks((long)_latencies.Average((TimeSpan t) => t.Ticks));

		public int Ping => Latency.Milliseconds;

		private HubConnection Conn
		{
			get
			{
				lock (_lock)
				{
					if (_int == null)
					{
						_int = new Internal(this);
					}
					return _int!.Conn;
				}
			}
		}

		public event Action<ClientState>? StateChanged;

		public event Action<string>? Authenticated;

		public event Action<int>? PingUpdated;

		public event Action<Lobby?>? LobbyChanged;

		public event Action<User, bool>? UserUpdated;

		public event Action<Lobby?>? LobbySettingsUpdated;

		public event Action<FullRace?>? LobbyRaceUpdated;

		public event Action<int>? CountdownStarted;

		public event Action? RaceStarted;

		public event Action? RaceCanceled;

		public event Action<User>? PositionUpdated;

		public event Action<User>? CheckpointReached;

		public RacingClient(MeasurerRealtime measurer)
		{
			_measurer = measurer;
			Server = new RacingServer(() => Conn);
			_dc.Add(RacingModule.Settings.OnlineUrl.OnChanged(delegate
			{
				lock (_lock)
				{
					if (State == ClientState.Offline)
					{
						_int?.Dispose();
						_int = null;
					}
				}
			}));
			_measurer.NewPosition += new Action<PosSnapshot>(NewPosition);
		}

		public void Connect(bool asGuest, string? nickname)
		{
			string nickname2 = nickname;
			if (string.IsNullOrWhiteSpace(nickname2))
			{
				nickname2 = null;
			}
			TaskUtils.Cancel(ref _cts);
			CancellationToken ct2 = TaskUtils.New(out _cts);
			((Func<Task>)async delegate
			{
				State = ClientState.Connecting;
				if (asGuest)
				{
					await startAsync(ct2);
					await Server.AuthenticateGuest(nickname2);
				}
				else
				{
					await RacingModule.Server.RefreshUser(ct2);
					string accessToken = RacingModule.Server.User.AccessToken;
					if (accessToken == null)
					{
						throw FriendlyError.Create(new UnauthorizedAccessException(Strings.ExceptionUnauthenticated));
					}
					await startAsync(ct2);
					await Server.Authenticate(accessToken, nickname2);
				}
				TaskUtils.Cancel(ref _ping);
				CancellationToken pingToken = TaskUtils.New(out _ping);
				((Func<Task>)async delegate
				{
					while (true)
					{
						pingToken.ThrowIfCancellationRequested();
						_lastPing = DateTime.UtcNow;
						await Server.Ping();
						await Task.Delay(TimeSpan.FromSeconds(5.0));
					}
				})();
			})().Done(Logger, Strings.ErrorFailedToConnect, _cts).ContinueWith(delegate(Task<TaskUtils.TaskState> t)
			{
				State = (t.Result.Success ? ClientState.Online : ClientState.Offline);
			});
			async Task startAsync(CancellationToken ct)
			{
				await Conn.StartAsync(ct);
				await Server.Validate("1");
			}
		}

		public void Disconnect()
		{
			TaskUtils.Cancel(ref _cts);
			CancellationToken ct = TaskUtils.New(out _cts);
			((Func<Task>)async delegate
			{
				State = ClientState.Connecting;
				await Conn.StopAsync(ct);
			})().Done(Logger, Strings.ErrorDisconnect, _cts).ContinueWith(delegate(Task<TaskUtils.TaskState> t)
			{
				if (t.Result.Success)
				{
					TaskUtils.Cancel(ref _ping);
					State = ClientState.Offline;
				}
			});
		}

		public async Task UpdateNickname(string? nickname)
		{
			if (string.IsNullOrWhiteSpace(nickname))
			{
				nickname = null;
			}
			await Server.UpdateNickname(nickname);
		}

		private void NewPosition(PosSnapshot position)
		{
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			User user = User;
			if (user != null && user.LobbyData.IsRacer && DateTime.UtcNow - _lastUpdate > TimeSpan.FromSeconds(0.03333333333333333))
			{
				user.RacerData.Sent = true;
				user.RacerData.MapId = GameService.Gw2Mumble.get_CurrentMap().get_Id();
				user.RacerData.Position = position.Meters;
				user.RacerData.Front = position.Front;
				_lastUpdate = DateTime.UtcNow;
				Server.UpdatePosition(user.RacerData.MapId, user.RacerData.Position, user.RacerData.Front);
			}
		}

		private void OnConnectionClosed()
		{
			UserId = null;
			Lobby = null;
			_int?.Dispose();
			_int = null;
			State = ClientState.Offline;
		}

		private void Pong()
		{
			_latencies.Push(DateTime.UtcNow - _lastPing);
			this.PingUpdated?.Invoke(Ping);
		}

		private void Error(string message)
		{
			ScreenNotification.ShowNotification(message, (NotificationType)2, (Texture2D)null, 4);
		}

		private void Validate(string version)
		{
			if (version != "1")
			{
				ScreenNotification.ShowNotification(StringExtensions.Format(Strings.ErrorMismatchedVersion, version, "1"), (NotificationType)2, (Texture2D)null, 4);
				Disconnect();
			}
		}

		private void Authenticate(string userId)
		{
			UserId = userId;
			this.Authenticated?.Invoke(userId);
		}

		private void GetLobbies(IDictionary<string, Lobby> _)
		{
		}

		private void JoinLobby(Lobby lobby)
		{
			Lobby = lobby;
		}

		private void LeaveLobby()
		{
			Lobby = null;
		}

		private void UpdateUser(User user, bool leaving)
		{
			User user2 = user;
			WithCurrentLobby(delegate(Lobby lobby)
			{
				lobby.Users.TryGetValue(user2.Id, out var value);
				User value2;
				if (!leaving)
				{
					lobby.Users[user2.Id] = user2;
				}
				else if (!lobby.Users.TryRemove(user2.Id, out value2))
				{
					return;
				}
				this.UserUpdated?.Invoke(user2, leaving);
				if (!leaving && value != null && value.LobbyData.IsRacer && user2.LobbyData.IsRacer && value.RacerData.Times.Count < user2.RacerData.Times.Count)
				{
					this.CheckpointReached?.Invoke(user2);
				}
			});
		}

		private void UpdateLobbySettings(LobbySettings settings)
		{
			LobbySettings settings2 = settings;
			WithCurrentLobby(delegate(Lobby lobby)
			{
				lobby.Settings = settings2;
				this.LobbySettingsUpdated?.Invoke(lobby);
			});
		}

		private void UpdateLobbyRace(FullRace? fullRace)
		{
			FullRace fullRace2 = fullRace;
			WithCurrentLobby(delegate(Lobby lobby)
			{
				lobby.FullRace = fullRace2;
				Route = ((fullRace2?.Race == null) ? null : new RaceRouteData(fullRace2.Race));
				this.LobbyRaceUpdated?.Invoke(fullRace2);
			});
		}

		private void StartCountdown(int seconds)
		{
			WithCurrentLobby(delegate(Lobby lobby)
			{
				foreach (User value in lobby.Users.Values)
				{
					value.RacerData = new RacerUser();
				}
				this.CountdownStarted?.Invoke(seconds);
			});
		}

		private void StartRace(long time)
		{
			WithCurrentLobby(delegate(Lobby lobby)
			{
				lobby.IsRunning = true;
				lobby.StartTime = new DateTime(time);
				foreach (User racer in lobby.Racers)
				{
					racer.RacerData.AddTime(new RacerTime(TimeSpan.Zero));
				}
				this.RaceStarted?.Invoke();
			});
		}

		private void CancelRace()
		{
			WithCurrentLobby(delegate(Lobby lobby)
			{
				lobby.IsRunning = false;
				this.RaceCanceled?.Invoke();
			});
		}

		private void UpdatePosition(string userId, int mapId, Vector3 position, Vector3 front)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			string userId2 = userId;
			WithCurrentLobby(delegate(Lobby lobby)
			{
				//IL_003a: Unknown result type (might be due to invalid IL or missing references)
				//IL_004b: Unknown result type (might be due to invalid IL or missing references)
				if (lobby.Users.TryGetValue(userId2, out var value))
				{
					value.RacerData.Sent = true;
					value.RacerData.MapId = mapId;
					value.RacerData.Position = position;
					value.RacerData.Front = front;
					this.PositionUpdated?.Invoke(value);
				}
			});
		}

		private void RaceFinished()
		{
			WithCurrentLobby(delegate(Lobby lobby)
			{
				lobby.IsRunning = false;
			});
		}

		private void WithCurrentLobby(Action<Lobby> action)
		{
			Lobby lobby = Lobby;
			if (lobby != null)
			{
				action(lobby);
			}
		}

		public void Dispose()
		{
			_measurer.NewPosition -= new Action<PosSnapshot>(NewPosition);
			_int?.Dispose();
			_dc.Dispose();
			TaskUtils.Cancel(ref _cts);
			TaskUtils.Cancel(ref _ping);
		}
	}
}
