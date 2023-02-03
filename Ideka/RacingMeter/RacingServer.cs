using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Ideka.RacingMeter.Lib;
using Ideka.RacingMeter.Lib.RacingServer;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Xna.Framework;

namespace Ideka.RacingMeter
{
	public class RacingServer : IRacingHubServer
	{
		public class UILock : IDisposable
		{
			private readonly RacingServer _server;

			public UILock(RacingServer server)
			{
				_server = server;
				lock (_server)
				{
					_server._locks[this] = true;
					foreach (Control key in _server._controls.Keys)
					{
						key.set_Enabled(false);
					}
				}
			}

			public void Dispose()
			{
				lock (_server)
				{
					_server._locks.TryRemove(this, out var _);
					if (_server._locks.Any())
					{
						return;
					}
					foreach (Control key in _server._controls.Keys)
					{
						key.set_Enabled(true);
					}
				}
			}
		}

		private static readonly Logger Logger = Logger.GetLogger<RacingServer>();

		private readonly HubConnection _conn;

		private readonly ConcurrentDictionary<Control, bool> _controls = new ConcurrentDictionary<Control, bool>();

		private readonly ConcurrentDictionary<UILock, bool> _locks = new ConcurrentDictionary<UILock, bool>();

		public RacingServer(HubConnection conn)
		{
			_conn = conn;
		}

		public T Register<T>(T control) where T : Control
		{
			T control2 = control;
			_controls[(Control)(object)control2] = true;
			((Control)control2).add_Disposed((EventHandler<EventArgs>)delegate
			{
				_controls.TryRemove((Control)(object)control2, out var _);
			});
			lock (this)
			{
				if (_locks.Any())
				{
					((Control)control2).set_Enabled(false);
				}
			}
			return control2;
		}

		public UILock Lock()
		{
			return new UILock(this);
		}

		public Task Ping()
		{
			return _conn.SendAsync("Ping");
		}

		public Task Authenticate(string accessToken)
		{
			return _conn.InvokeAsync("Authenticate", accessToken);
		}

		public Task GetLobbies()
		{
			return _conn.InvokeAsync("GetLobbies").Done(Logger, "Send failed");
		}

		public Task CreateLobby()
		{
			return _conn.InvokeAsync("CreateLobby").Done(Logger, "Send failed");
		}

		public Task JoinLobby(string lobbyId)
		{
			return _conn.InvokeAsync("JoinLobby", lobbyId).Done(Logger, "Send failed");
		}

		public Task LeaveLobby()
		{
			return _conn.InvokeAsync("LeaveLobby").Done(Logger, "Send failed");
		}

		public Task KickUser(string userId)
		{
			return _conn.InvokeAsync("KickUser", userId).Done(Logger, "Send failed");
		}

		public Task SetHost(string userId, bool value)
		{
			return _conn.InvokeAsync("SetHost", userId, value).Done(Logger, "Send failed");
		}

		public Task SetRacer(string userId, bool value)
		{
			return _conn.InvokeAsync("SetRacer", userId, value).Done(Logger, "Send failed");
		}

		public Task SetLobbyName(string name)
		{
			return _conn.InvokeAsync("SetLobbyName", name).Done(Logger, "Send failed");
		}

		public Task SetLobbySize(int size)
		{
			return _conn.InvokeAsync("SetLobbySize", size).Done(Logger, "Send failed");
		}

		public Task SetLobbyLaps(int laps)
		{
			return _conn.InvokeAsync("SetLobbyLaps", laps).Done(Logger, "Send failed");
		}

		public Task SetLobbyRace(FullRace race)
		{
			return _conn.InvokeAsync("SetLobbyRace", race).Done(Logger, "Send failed");
		}

		public Task StartCountdown()
		{
			return _conn.InvokeAsync("StartCountdown").Done(Logger, "Send failed");
		}

		public Task CancelRace()
		{
			return _conn.InvokeAsync("CancelRace").Done(Logger, "Send failed");
		}

		public Task UpdatePosition(int mapId, Vector3 position, Vector3 front)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			return _conn.SendAsync("UpdatePosition", mapId, position, front).Done(Logger, "Send failed");
		}
	}
}
