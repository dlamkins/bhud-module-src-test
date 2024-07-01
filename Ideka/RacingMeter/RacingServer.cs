using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Runtime.CompilerServices;
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

		[CompilerGenerated]
		private Func<HubConnection> _003CgetConn_003EP;

		private static readonly Logger Logger = Logger.GetLogger<RacingServer>();

		private readonly ConcurrentDictionary<Control, bool> _controls;

		private readonly ConcurrentDictionary<UILock, bool> _locks;

		public RacingServer(Func<HubConnection> getConn)
		{
			_003CgetConn_003EP = getConn;
			_controls = new ConcurrentDictionary<Control, bool>();
			_locks = new ConcurrentDictionary<UILock, bool>();
			base._002Ector();
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
			return _003CgetConn_003EP().SendAsync("Ping");
		}

		public Task Validate(string version)
		{
			return _003CgetConn_003EP().InvokeAsync("Validate", version);
		}

		public Task Authenticate(string accessToken, string? nickname)
		{
			return _003CgetConn_003EP().InvokeAsync("Authenticate", accessToken, nickname);
		}

		public Task AuthenticateGuest(string? nickname)
		{
			return _003CgetConn_003EP().InvokeAsync("AuthenticateGuest", nickname);
		}

		public Task UpdateNickname(string? nickname)
		{
			return _003CgetConn_003EP().InvokeAsync("UpdateNickname", nickname).Done(Logger, Strings.ErrorSendFailed);
		}

		public Task GetLobbies()
		{
			return _003CgetConn_003EP().InvokeAsync("GetLobbies").Done(Logger, Strings.ErrorSendFailed);
		}

		public Task CreateLobby()
		{
			return _003CgetConn_003EP().InvokeAsync("CreateLobby").Done(Logger, Strings.ErrorSendFailed);
		}

		public Task JoinLobby(string lobbyId)
		{
			return _003CgetConn_003EP().InvokeAsync("JoinLobby", lobbyId).Done(Logger, Strings.ErrorSendFailed);
		}

		public Task LeaveLobby()
		{
			return _003CgetConn_003EP().InvokeAsync("LeaveLobby").Done(Logger, Strings.ErrorSendFailed);
		}

		public Task KickUser(string userId)
		{
			return _003CgetConn_003EP().InvokeAsync("KickUser", userId).Done(Logger, Strings.ErrorSendFailed);
		}

		public Task SetHost(string userId, bool value)
		{
			return _003CgetConn_003EP().InvokeAsync("SetHost", userId, value).Done(Logger, Strings.ErrorSendFailed);
		}

		public Task SetRacer(string userId, bool value)
		{
			return _003CgetConn_003EP().InvokeAsync("SetRacer", userId, value).Done(Logger, Strings.ErrorSendFailed);
		}

		public Task SetLobbyName(string name)
		{
			return _003CgetConn_003EP().InvokeAsync("SetLobbyName", name).Done(Logger, Strings.ErrorSendFailed);
		}

		public Task SetLobbySize(int size)
		{
			return _003CgetConn_003EP().InvokeAsync("SetLobbySize", size).Done(Logger, Strings.ErrorSendFailed);
		}

		public Task SetLobbyLaps(int laps)
		{
			return _003CgetConn_003EP().InvokeAsync("SetLobbyLaps", laps).Done(Logger, Strings.ErrorSendFailed);
		}

		public Task SetLobbyRace(FullRace fullRace)
		{
			return _003CgetConn_003EP().InvokeAsync("SetLobbyRace", fullRace).Done(Logger, Strings.ErrorSendFailed);
		}

		public Task StartCountdown()
		{
			return _003CgetConn_003EP().InvokeAsync("StartCountdown").Done(Logger, Strings.ErrorSendFailed);
		}

		public Task CancelRace()
		{
			return _003CgetConn_003EP().InvokeAsync("CancelRace").Done(Logger, Strings.ErrorSendFailed);
		}

		public Task UpdatePosition(int mapId, Vector3 position, Vector3 front)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			return _003CgetConn_003EP().SendAsync("UpdatePosition", mapId, position, front);
		}
	}
}
