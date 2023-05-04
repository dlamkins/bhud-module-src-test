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

		private readonly Func<HubConnection> _getConn;

		private readonly ConcurrentDictionary<Control, bool> _controls = new ConcurrentDictionary<Control, bool>();

		private readonly ConcurrentDictionary<UILock, bool> _locks = new ConcurrentDictionary<UILock, bool>();

		public RacingServer(Func<HubConnection> getConn)
		{
			_getConn = getConn;
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
			return _getConn().SendAsync("Ping");
		}

		public Task Validate(string version)
		{
			return _getConn().InvokeAsync("Validate", version);
		}

		public Task Authenticate(string accessToken, string? nickname)
		{
			return _getConn().InvokeAsync("Authenticate", accessToken, nickname);
		}

		public Task AuthenticateGuest(string? nickname)
		{
			return _getConn().InvokeAsync("AuthenticateGuest", nickname);
		}

		public Task UpdateNickname(string? nickname)
		{
			return _getConn().InvokeAsync("UpdateNickname", nickname).Done(Logger, Strings.ErrorSendFailed);
		}

		public Task GetLobbies()
		{
			return _getConn().InvokeAsync("GetLobbies").Done(Logger, Strings.ErrorSendFailed);
		}

		public Task CreateLobby()
		{
			return _getConn().InvokeAsync("CreateLobby").Done(Logger, Strings.ErrorSendFailed);
		}

		public Task JoinLobby(string lobbyId)
		{
			return _getConn().InvokeAsync("JoinLobby", lobbyId).Done(Logger, Strings.ErrorSendFailed);
		}

		public Task LeaveLobby()
		{
			return _getConn().InvokeAsync("LeaveLobby").Done(Logger, Strings.ErrorSendFailed);
		}

		public Task KickUser(string userId)
		{
			return _getConn().InvokeAsync("KickUser", userId).Done(Logger, Strings.ErrorSendFailed);
		}

		public Task SetHost(string userId, bool value)
		{
			return _getConn().InvokeAsync("SetHost", userId, value).Done(Logger, Strings.ErrorSendFailed);
		}

		public Task SetRacer(string userId, bool value)
		{
			return _getConn().InvokeAsync("SetRacer", userId, value).Done(Logger, Strings.ErrorSendFailed);
		}

		public Task SetLobbyName(string name)
		{
			return _getConn().InvokeAsync("SetLobbyName", name).Done(Logger, Strings.ErrorSendFailed);
		}

		public Task SetLobbySize(int size)
		{
			return _getConn().InvokeAsync("SetLobbySize", size).Done(Logger, Strings.ErrorSendFailed);
		}

		public Task SetLobbyLaps(int laps)
		{
			return _getConn().InvokeAsync("SetLobbyLaps", laps).Done(Logger, Strings.ErrorSendFailed);
		}

		public Task SetLobbyRace(FullRace fullRace)
		{
			return _getConn().InvokeAsync("SetLobbyRace", fullRace).Done(Logger, Strings.ErrorSendFailed);
		}

		public Task StartCountdown()
		{
			return _getConn().InvokeAsync("StartCountdown").Done(Logger, Strings.ErrorSendFailed);
		}

		public Task CancelRace()
		{
			return _getConn().InvokeAsync("CancelRace").Done(Logger, Strings.ErrorSendFailed);
		}

		public Task UpdatePosition(int mapId, Vector3 position, Vector3 front)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			return _getConn().SendAsync("UpdatePosition", mapId, position, front);
		}
	}
}
