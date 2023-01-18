using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
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

		private class Internal : IRacingHubClient
		{
			private readonly RacingClient _outer;

			public Internal(RacingClient outer)
			{
				_outer = outer;
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

			public Task UpdateLobbyRace(FullRace race)
			{
				_outer.UpdateLobbyRace(race);
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
		}

		private static readonly Logger Logger = Logger.GetLogger<RacingClient>();

		private Lobby _lobby;

		public ClientState _state;

		private DateTime _lastPing = DateTime.MinValue;

		private readonly DropOutStack<TimeSpan> _latencies = new DropOutStack<TimeSpan>(5);

		private CancellationTokenSource _cts;

		private CancellationTokenSource _ping;

		private readonly HubConnection _conn;

		private readonly Internal _int;

		private const int MaxUpdatesPerSecond = 30;

		private DateTime _lastUpdate;

		private CancellationTokenSource _countdown;

		public RacingServer Server { get; }

		public RaceRouteData Route { get; private set; }

		public Lobby Lobby
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
					Route = ((_lobby?.Race?.Race == null) ? null : new RaceRouteData(_lobby.Race!.Race));
					this.LobbyChanged?.Invoke(value);
					this.LobbyRaceUpdated?.Invoke(value?.Race);
					this.LobbySettingsUpdated?.Invoke(value);
				}
			}
		}

		public string UserId { get; private set; }

		public User User
		{
			get
			{
				User user = default(User);
				if (!(Lobby?.Users.TryGetValue(UserId, out user) ?? false))
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

		public event Action<ClientState> StateChanged;

		public event Action<string> Authenticated;

		public event Action<int> PingUpdated;

		public event Action<Lobby> LobbyChanged;

		public event Action<User, bool> UserUpdated;

		public event Action<Lobby> LobbySettingsUpdated;

		public event Action<FullRace> LobbyRaceUpdated;

		public event Action RaceStarted;

		public event Action RaceCanceled;

		public event Action<User> PositionUpdated;

		public RacingClient()
		{
			_conn = new HubConnectionBuilder().WithUrl("http://localhost:5000/racing").AddNewtonsoftJsonProtocol(delegate(NewtonsoftJsonHubProtocolOptions options)
			{
				JsonSerialization.Configure(options.PayloadSerializerSettings);
			}).Build();
			Server = new RacingServer(_conn);
			_int = new Internal(this);
			_conn.Closed += delegate
			{
				UserId = null;
				Lobby = null;
				State = ClientState.Offline;
				return Task.CompletedTask;
			};
			_conn.On("Pong", new Func<Task>(_int.Pong));
			_conn.On("Error", new Func<string, Task>(_int.Error));
			_conn.On("Authenticate", new Func<string, Task>(_int.Authenticate));
			_conn.On("JoinLobby", new Func<Lobby, Task>(_int.JoinLobby));
			_conn.On("LeaveLobby", new Func<Task>(_int.LeaveLobby));
			_conn.On("UpdateUser", new Func<User, bool, Task>(_int.UpdateUser));
			_conn.On("UpdateLobbySettings", new Func<LobbySettings, Task>(_int.UpdateLobbySettings));
			_conn.On("UpdateLobbyRace", new Func<FullRace, Task>(_int.UpdateLobbyRace));
			_conn.On("StartCountdown", new Func<int, Task>(_int.StartCountdown));
			_conn.On("StartRace", new Func<long, Task>(_int.StartRace));
			_conn.On("CancelRace", new Func<Task>(_int.CancelRace));
			_conn.On("RaceFinished", new Func<Task>(_int.RaceFinished));
			_conn.On("UpdatePosition", new Func<string, int, Vector3, Vector3, Task>(_int.UpdatePosition));
			RacingModule.Measurer.NewPosition += new Action<PosSnapshot>(NewPosition);
		}

		public void Connect()
		{
			TaskUtils.Cancel(ref _cts);
			CancellationToken ct = TaskUtils.New(ref _cts);
			((Func<Task>)async delegate
			{
				State = ClientState.Connecting;
				await RacingModule.Server.RefreshUser(ct);
				await _conn.StartAsync(ct);
				await Server.Authenticate(RacingModule.Server.User.AccessToken);
				TaskUtils.Cancel(ref _ping);
				CancellationToken pingToken = TaskUtils.New(ref _ping);
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
			})().Done(Logger, "Failed to connect", _cts).ContinueWith(delegate(Task<TaskUtils.TaskState> t)
			{
				State = (t.Result.Success ? ClientState.Online : ClientState.Offline);
			});
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

		private void Pong()
		{
			_latencies.Push(DateTime.UtcNow - _lastPing);
			this.PingUpdated?.Invoke(Ping);
		}

		private void Error(string message)
		{
			ScreenNotification.ShowNotification(message, (NotificationType)2, (Texture2D)null, 4);
		}

		private void Authenticate(string userId)
		{
			UserId = userId;
			this.Authenticated?.Invoke(userId);
		}

		private void GetLobbies(IDictionary<string, Lobby> lobbies)
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
			WithCurrentLobby(delegate(Lobby lobby)
			{
				User value;
				if (!leaving)
				{
					lobby.Users[user.Id] = user;
				}
				else if (!lobby.Users.TryRemove(user.Id, out value))
				{
					return;
				}
				this.UserUpdated?.Invoke(user, leaving);
			});
		}

		private void UpdateLobbySettings(LobbySettings settings)
		{
			WithCurrentLobby(delegate(Lobby lobby)
			{
				lobby.Settings = settings;
				this.LobbySettingsUpdated?.Invoke(lobby);
			});
		}

		private void UpdateLobbyRace(FullRace race)
		{
			WithCurrentLobby(delegate(Lobby lobby)
			{
				lobby.Race = race;
				Route = ((race?.Race == null) ? null : new RaceRouteData(race.Race));
				this.LobbyRaceUpdated?.Invoke(race);
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
				TaskUtils.Cancel(ref _countdown);
				CancellationToken ct = TaskUtils.New(ref _countdown);
				((Func<Task>)async delegate
				{
					int i;
					for (i = 0; i < seconds; i++)
					{
						ct.ThrowIfCancellationRequested();
						GameService.Graphics.QueueMainThreadRender((Action<GraphicsDevice>)delegate
						{
							ScreenNotification.ShowNotification($"{seconds - i}", (NotificationType)0, (Texture2D)null, 4);
						});
						await Task.Delay(TimeSpan.FromSeconds(1.0));
					}
				})();
			});
		}

		private void StartRace(long time)
		{
			WithCurrentLobby(delegate(Lobby lobby)
			{
				GameService.Graphics.QueueMainThreadRender((Action<GraphicsDevice>)delegate
				{
					ScreenNotification.ShowNotification("GO!", (NotificationType)0, (Texture2D)null, 4);
				});
				lobby.IsRunning = true;
				lobby.StartTime = new DateTime(time);
				this.RaceStarted?.Invoke();
			});
		}

		private void CancelRace()
		{
			WithCurrentLobby(delegate(Lobby lobby)
			{
				GameService.Graphics.QueueMainThreadRender((Action<GraphicsDevice>)delegate
				{
					ScreenNotification.ShowNotification("Race canceled", (NotificationType)0, (Texture2D)null, 4);
				});
				TaskUtils.Cancel(ref _countdown);
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
			WithCurrentLobby(delegate(Lobby lobby)
			{
				//IL_003a: Unknown result type (might be due to invalid IL or missing references)
				//IL_004b: Unknown result type (might be due to invalid IL or missing references)
				if (lobby.Users.TryGetValue(userId, out var value))
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
			RacingModule.Measurer.NewPosition -= new Action<PosSnapshot>(NewPosition);
			_conn.DisposeAsync();
			TaskUtils.Cancel(ref _cts);
			TaskUtils.Cancel(ref _countdown);
			TaskUtils.Cancel(ref _ping);
		}
	}
}
