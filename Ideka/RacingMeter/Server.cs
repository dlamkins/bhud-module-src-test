using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD.Controls;
using Ideka.NetCommon;
using Ideka.RacingMeterLib;
using Microsoft.Xna.Framework.Graphics;

namespace Ideka.RacingMeter
{
	internal class Server : IDisposable
	{
		public enum OnlineStatus
		{
			Unchecked,
			Yes,
			No,
			Faulted
		}

		private readonly HttpClient _client;

		private readonly ConcurrentDictionary<string, Ghost> _ghostCache;

		private readonly CallCheckVersion _checkVersion = new CallCheckVersion
		{
			ExceptionFactory = FriendlyError.Create
		};

		private readonly CallGetRaces _getRaces = new CallGetRaces
		{
			ExceptionFactory = FriendlyError.Create
		};

		private readonly CallGetLeaderboard _getLeaderboard = new CallGetLeaderboard
		{
			ExceptionFactory = FriendlyError.Create
		};

		private readonly CallGetGhost _getGhost = new CallGetGhost
		{
			ExceptionFactory = FriendlyError.Create
		};

		private readonly CallUploadGhost _uploadGhost = new CallUploadGhost
		{
			ExceptionFactory = FriendlyError.Create
		};

		public OnlineStatus Online { get; private set; }

		public bool IsOnline => Online == OnlineStatus.Yes;

		public UserData User { get; } = new UserData();


		public RemoteRaces RemoteRaces { get; }

		public Leaderboards Leaderboards { get; }

		public event Action<UserData> UserChanged;

		public event Action<RemoteRaces> RemoteRacesChanged;

		public event Action<string> LeaderboardsChanged;

		public Server()
		{
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Expected O, but got Unknown
			RemoteRaces = RemoteRaces.FromCache(RacingModule.RaceCachePath);
			Leaderboards = new Leaderboards();
			_client = new HttpClient();
			_ghostCache = new ConcurrentDictionary<string, Ghost>();
		}

		public bool NotifyIfOffline()
		{
			if (!IsOnline)
			{
				ScreenNotification.ShowNotification(Strings.NotifyOfflineMode, (NotificationType)2, (Texture2D)null, 4);
			}
			return !IsOnline;
		}

		private async Task RefreshUser(CancellationToken ct = default(CancellationToken))
		{
			string prevAccountId = User?.AccountId;
			try
			{
				await User.Refresh(ct);
			}
			finally
			{
				if (prevAccountId != User?.AccountId)
				{
					this.UserChanged?.Invoke(User);
				}
			}
		}

		public async Task CheckVersion(string version, CancellationToken ct = default(CancellationToken))
		{
			try
			{
				Online = ((await _checkVersion.Call(_client, new CallCheckVersion.Req
				{
					Version = version
				}, ct)).Supported ? OnlineStatus.Yes : OnlineStatus.No);
			}
			catch (Exception ex)
			{
				Online = OnlineStatus.Faulted;
				throw ex;
			}
		}

		public async Task UpdateRaces(bool force = false, CancellationToken ct = default(CancellationToken))
		{
			try
			{
				this.RemoteRacesChanged?.Invoke(new RemoteRaces());
				CallGetRaces.Res res = await _getRaces.Call(_client, new CallGetRaces.Req
				{
					LastUpdate = ((!force && RemoteRaces.Races.Any()) ? RemoteRaces.Races.Values.Max((FullRace r) => r.Race.Modified) : DateTimeOffset.FromUnixTimeSeconds(0L).UtcDateTime)
				}, ct);
				RemoteRaces.Races.MergeOverwrite(res.Races);
				RemoteRaces.ToCache(RacingModule.RaceCachePath);
			}
			finally
			{
				this.RemoteRacesChanged?.Invoke(RemoteRaces);
			}
		}

		public async Task UpdateLeaderboard(string raceId, CancellationToken ct = default(CancellationToken))
		{
			try
			{
				await RefreshUser(ct);
			}
			catch
			{
			}
			CallGetLeaderboard.Res res = await _getLeaderboard.Call(_client, new CallGetLeaderboard.Req
			{
				RaceId = raceId,
				AccountId = User?.AccountId
			}, ct);
			Leaderboards.Boards[raceId] = res.Leaderboard;
			this.LeaderboardsChanged?.Invoke(raceId);
		}

		public async Task<Ghost> GetGhost(string ghostId, CancellationToken ct = default(CancellationToken))
		{
			if (_ghostCache.TryGetValue(ghostId, out var ghost))
			{
				return ghost;
			}
			CallGetGhost.Res res = await _getGhost.Call(_client, new CallGetGhost.Req
			{
				GhostId = ghostId
			}, ct);
			_ghostCache[ghostId] = res.Ghost;
			return res.Ghost;
		}

		public async Task UploadGhost(string raceId, Ghost ghost, CancellationToken ct = default(CancellationToken))
		{
			await RefreshUser(ct);
			await _uploadGhost.Call(_client, User?.AccessToken, new CallUploadGhost.Req
			{
				RaceId = raceId,
				Ghost = ghost
			}, ct);
			await UpdateLeaderboard(raceId, ct);
		}

		public void Dispose()
		{
			HttpClient client = _client;
			if (client != null)
			{
				((HttpMessageInvoker)client).Dispose();
			}
		}
	}
}
