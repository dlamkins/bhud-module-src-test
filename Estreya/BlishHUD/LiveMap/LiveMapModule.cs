using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.ArcDps.Common;
using Blish_HUD.Content;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Modules;
using Blish_HUD.Settings;
using Estreya.BlishHUD.LiveMap.Models.Player;
using Estreya.BlishHUD.LiveMap.SignalR;
using Estreya.BlishHUD.LiveMap.UI.Views;
using Estreya.BlishHUD.Shared.Extensions;
using Estreya.BlishHUD.Shared.Helpers;
using Estreya.BlishHUD.Shared.Modules;
using Estreya.BlishHUD.Shared.Settings;
using Estreya.BlishHUD.Shared.Threading;
using Estreya.BlishHUD.Shared.Utils;
using Flurl.Util;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using SocketIOClient;

namespace Estreya.BlishHUD.LiveMap
{
	[Export(typeof(Module))]
	public class LiveMapModule : BaseModule<LiveMapModule, ModuleSettings>
	{
		public const string LIVE_MAP_BROWSER_URL = "https://gw2map.estreya.de/";

		private string _accountName;

		private TimeSpan _guildFetchInterval = TimeSpan.FromSeconds(30.0);

		private readonly AsyncRef<double> _lastGuildFetch = new AsyncRef<double>(0.0);

		private readonly AsyncRef<double> _lastSend = new AsyncRef<double>(0.0);

		private Player _lastSendPlayer;

		private readonly AsyncRef<double> _lastWvWFetch = new AsyncRef<double>(0.0);

		private Map _map;

		private TimeSpan _sendInterval = TimeSpan.FromMilliseconds(250.0);

		private PlayerWvW _wvw;

		private TimeSpan _wvwFetchInterval = TimeSpan.FromHours(1.0);

		private HubConnection _hubConnection;

		private string LIVE_MAP_API_URL => base.MODULE_API_URL + "/writer";

		public string GuildId { get; private set; }

		protected override string UrlModuleName => "live-map";

		protected override string API_VERSION_NO => "2";

		protected override bool NeedsBackend => true;

		protected override int CornerIconPriority => 1289351275;

		[ImportingConstructor]
		public LiveMapModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: base(moduleParameters)
		{
		}

		protected override void Initialize()
		{
			base.Initialize();
			_hubConnection = new HubConnectionBuilder().WithUrl(LIVE_MAP_API_URL).ConfigureLogging(delegate(ILoggingBuilder options)
			{
				options.SetMinimumLevel(LogLevel.Debug);
				options.AddProvider(new LoggerProvider());
			}).WithAutomaticReconnect(new UnlimitedRetryPolicy(TimeSpan.FromSeconds(5.0)))
				.AddJsonProtocol(delegate(JsonHubProtocolOptions options)
				{
					options.PayloadSerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.General);
				})
				.Build();
			base.Gw2ApiManager.add_SubtokenUpdated((EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>>)Gw2ApiManager_SubtokenUpdated);
			GameService.Gw2Mumble.get_PlayerCharacter().add_NameChanged((EventHandler<ValueEventArgs<string>>)PlayerCharacter_NameChanged);
			GameService.Gw2Mumble.get_CurrentMap().add_MapChanged((EventHandler<ValueEventArgs<int>>)CurrentMap_MapChanged);
			GameService.ArcDps.get_Common().Activate();
			_lastSend.Value = _sendInterval.TotalMilliseconds;
			_lastGuildFetch.Value = _guildFetchInterval.TotalMilliseconds;
		}

		private void CurrentMap_MapChanged(object sender, ValueEventArgs<int> e)
		{
			((IBulkExpandableClient<Map, int>)(object)base.Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Maps()).GetAsync(e.get_Value(), default(CancellationToken)).ContinueWith(delegate(Task<Map> response)
			{
				if (response.Exception == null && !response.IsFaulted && !response.IsCanceled)
				{
					Map val = (_map = response.Result);
				}
			});
		}

		private async Task<bool> ConnectWithRetryAsync(CancellationToken token)
		{
			while (true)
			{
				try
				{
					await _hubConnection.StartAsync(token);
					return true;
				}
				catch when (token.IsCancellationRequested)
				{
					return false;
				}
				catch
				{
					await Task.Delay(5000);
				}
			}
		}

		protected override async Task LoadAsync()
		{
			await base.LoadAsync();
			_hubConnection.Closed += HubConnection_Closed;
			_hubConnection.Reconnecting += HubConnection_Reconnecting;
			_hubConnection.Reconnected += HubConnection_Reconnected;
			_hubConnection.On("SetSendingInterval", delegate(int intervalMs)
			{
				_sendInterval = TimeSpan.FromMilliseconds(intervalMs);
			});
			await ConnectWithRetryAsync(default(CancellationToken));
			await FetchAccountName();
			await FetchGuildId();
			await FetchWvW();
		}

		private Task HubConnection_Reconnected(string arg)
		{
			base.Logger.Info("Reconnected.");
			return Task.CompletedTask;
		}

		private Task HubConnection_Reconnecting(Exception ex)
		{
			base.Logger.Info("Attempt reconnect: " + ex.Message);
			return Task.CompletedTask;
		}

		private Task HubConnection_Closed(Exception ex)
		{
			base.Logger.Warn("Disconnected: " + ex.Message);
			return Task.CompletedTask;
		}

		private void GlobalSocket_OnConnected(object sender, EventArgs e)
		{
			base.Logger.Info("Connected.");
		}

		private void GlobalSocket_OnDisconnected(object sender, string e)
		{
			base.Logger.Warn("Disconnected: " + e);
			if (e == DisconnectReason.IOServerDisconnect)
			{
				base.Logger.Info("Trying to reconnect...");
			}
		}

		private void GlobalSocket_OnError(object sender, string e)
		{
			base.Logger.Warn("Error: " + e);
		}

		private void GlobalSocket_OnReconnectAttempt(object sender, int e)
		{
			base.Logger.Info($"Attempt reconnect: {e}");
		}

		private void GlobalSocket_OnReconnectFailed(object sender, EventArgs e)
		{
			base.Logger.Warn("Reconnect failed.");
		}

		private void GlobalSocket_OnReconnectError(object sender, Exception e)
		{
			base.Logger.Warn(e, "Could not reconnect");
		}

		public static byte[] Compress(byte[] bytes)
		{
			using MemoryStream memoryStream = new MemoryStream();
			using (GZipStream gzipStream = new GZipStream(memoryStream, CompressionLevel.Optimal))
			{
				gzipStream.Write(bytes, 0, bytes.Length);
			}
			return memoryStream.ToArray();
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			base.OnModuleLoaded(e);
		}

		private void Gw2ApiManager_SubtokenUpdated(object sender, ValueEventArgs<IEnumerable<TokenPermission>> e)
		{
			Task.Run((Func<Task>)FetchAccountName);
			_lastGuildFetch.Value = _guildFetchInterval.TotalMilliseconds;
			_lastWvWFetch.Value = _wvwFetchInterval.TotalMilliseconds;
		}

		private void PlayerCharacter_NameChanged(object sender, ValueEventArgs<string> e)
		{
			_lastGuildFetch.Value = _guildFetchInterval.TotalMilliseconds;
		}

		private async Task FetchAccountName()
		{
			if (base.Gw2ApiManager.HasPermissions((IEnumerable<TokenPermission>)(object)new TokenPermission[1] { (TokenPermission)1 }))
			{
				_accountName = (await ((IBlobClient<Account>)(object)base.Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Account()).GetAsync(default(CancellationToken))).get_Name();
			}
		}

		private async Task FetchGuildId()
		{
			if (GameService.Gw2Mumble.get_IsAvailable() && base.Gw2ApiManager.HasPermissions((IEnumerable<TokenPermission>)(object)new TokenPermission[1] { (TokenPermission)3 }))
			{
				try
				{
					GuildId = (await ((IBulkExpandableClient<Character, string>)(object)base.Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Characters()).GetAsync(GameService.Gw2Mumble.get_PlayerCharacter().get_Name(), default(CancellationToken))).get_Guild().ToString();
				}
				catch (Exception ex)
				{
					base.Logger.Debug(ex, "Failed to fetch guild id:");
				}
			}
		}

		private async Task FetchWvW()
		{
			string color = "white";
			string matchId = "0-0";
			if (base.Gw2ApiManager.HasPermissions((IEnumerable<TokenPermission>)(object)new TokenPermission[1] { (TokenPermission)1 }))
			{
				try
				{
					int worldId = (await ((IBlobClient<Account>)(object)base.Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Account()).GetAsync(default(CancellationToken))).get_World();
					IApiV2ObjectList<WvwMatch> source = await ((IAllExpandableClient<WvwMatch>)(object)base.Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Wvw()
						.get_Matches()).AllAsync(default(CancellationToken));
					WvwMatch match = ((IEnumerable<WvwMatch>)source).Where((WvwMatch m) => m.get_AllWorlds().get_Green().Contains(worldId)).FirstOrDefault();
					if (match != null)
					{
						color = "green";
						matchId = match.get_Id();
					}
					match = ((IEnumerable<WvwMatch>)source).Where((WvwMatch m) => m.get_AllWorlds().get_Red().Contains(worldId)).FirstOrDefault();
					if (match != null)
					{
						color = "red";
						matchId = match.get_Id();
					}
					match = ((IEnumerable<WvwMatch>)source).Where((WvwMatch m) => m.get_AllWorlds().get_Blue().Contains(worldId)).FirstOrDefault();
					if (match != null)
					{
						color = "blue";
						matchId = match.get_Id();
					}
				}
				catch (Exception ex)
				{
					base.Logger.Debug(ex, "Failed to fetch wvw team color:");
				}
			}
			_wvw = new PlayerWvW
			{
				Match = matchId,
				TeamColor = color
			};
		}

		private async Task SendPosition()
		{
			if (string.IsNullOrWhiteSpace(_accountName) || !GameService.Gw2Mumble.get_IsAvailable() || GameService.Gw2Mumble.get_TimeSinceTick().TotalSeconds > 0.5 || (base.ModuleSettings.StreamerModeEnabled.get_Value() && StreamerUtils.IsStreaming()))
			{
				return;
			}
			Player player = GetPlayer();
			if (_lastSendPlayer == null || !player.Equals(_lastSendPlayer))
			{
				try
				{
					await PublishToGlobal(player);
					_lastSendPlayer = player;
				}
				catch (Exception ex)
				{
					base.Logger.Debug(ex.Message);
				}
			}
		}

		private async Task PublishToGlobal(Player player)
		{
			if (_hubConnection.State == HubConnectionState.Connected)
			{
				await _hubConnection.InvokeAsync("UpdatePlayer", player);
			}
		}

		protected override void Update(GameTime gameTime)
		{
			UpdateUtil.UpdateAsync(SendPosition, gameTime, _sendInterval.TotalMilliseconds, _lastSend, doLogging: false);
			UpdateUtil.UpdateAsync(FetchGuildId, gameTime, _guildFetchInterval.TotalMilliseconds, _lastGuildFetch);
			UpdateUtil.UpdateAsync(FetchWvW, gameTime, _wvwFetchInterval.TotalMilliseconds, _lastWvWFetch);
		}

		protected override void Unload()
		{
			base.Unload();
			base.Gw2ApiManager.remove_SubtokenUpdated((EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>>)Gw2ApiManager_SubtokenUpdated);
			GameService.Gw2Mumble.get_PlayerCharacter().remove_NameChanged((EventHandler<ValueEventArgs<string>>)PlayerCharacter_NameChanged);
			_hubConnection.Closed -= HubConnection_Closed;
			_hubConnection.Reconnecting -= HubConnection_Reconnecting;
			_hubConnection.Reconnected -= HubConnection_Reconnected;
			AsyncHelper.RunSync(async delegate
			{
				await _hubConnection.StopAsync();
				await _hubConnection.DisposeAsync();
			});
		}

		public Player GetPlayer()
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			Vector2 position = _map?.WorldMeterCoordsToMapCoords(GameService.Gw2Mumble.get_PlayerCharacter().get_Position()) ?? Vector2.get_Zero();
			Vector3 forward = GameService.Gw2Mumble.get_PlayerCharacter().get_Forward();
			double angle = Math.Atan2(forward.X, forward.Y) * 180.0 / Math.PI;
			if (angle < 0.0)
			{
				angle += 360.0;
			}
			return new Player
			{
				Identification = new PlayerIdentification
				{
					Account = _accountName,
					Character = GameService.Gw2Mumble.get_PlayerCharacter().get_Name(),
					GuildId = GuildId
				},
				Map = new PlayerMap
				{
					Continent = GetContinentId(_map),
					Position = new PlayerPosition
					{
						X = position.X,
						Y = position.Y
					}
				},
				Facing = new PlayerFacing
				{
					Angle = angle
				},
				WvW = _wvw,
				Group = new PlayerGroup
				{
					Squad = (base.ModuleSettings.SendGroupInformation.get_Value() ? (from p in GameService.ArcDps.get_Common().get_PlayersInSquad().Values
						select ((Player)(ref p)).get_AccountName().Trim(':') into p
						where p != _accountName
						select p).ToArray() : null)
				},
				Commander = (!base.ModuleSettings.HideCommander.get_Value() && GameService.Gw2Mumble.get_PlayerCharacter().get_IsCommander())
			};
		}

		private int GetContinentId(Map map)
		{
			if (map == null)
			{
				return -1;
			}
			if (map.get_Id() == 1206)
			{
				return 1;
			}
			return map.get_ContinentId();
		}

		private string GetGlobalUrl(bool formatPositions = true)
		{
			string baseUrl = "https://gw2map.estreya.de/";
			string url = baseUrl;
			Map map = _map;
			if (map != null && map.get_ContinentId() == 1)
			{
				url = Path.Combine(url, "tyria");
			}
			else
			{
				Map map2 = _map;
				if (map2 == null || map2.get_ContinentId() != 2)
				{
					return baseUrl;
				}
				url = Path.Combine(url, "mists");
				if (_wvw != null)
				{
					url = Path.Combine(url, _wvw.Match);
				}
			}
			if (!formatPositions)
			{
				return url;
			}
			return FormatUrlWithPosition(url);
		}

		private string GetGuildUrl(bool formatPositions = true)
		{
			string baseUrl = GetGlobalUrl(formatPositions: false);
			string url = baseUrl;
			if (!string.IsNullOrWhiteSpace(GuildId))
			{
				url = Path.Combine(url, "guild", GuildId);
				if (!formatPositions)
				{
					return url;
				}
				return FormatUrlWithPosition(url);
			}
			return baseUrl;
		}

		private string FormatUrlWithPosition(string url)
		{
			Player player = GetPlayer();
			return url + "?posX=" + player.Map.Position.X.ToInvariantString() + "&posY=" + player.Map.Position.Y.ToInvariantString() + "&zoom=6" + ((!string.IsNullOrWhiteSpace(_accountName)) ? ("&account=" + _accountName) : "") + "&follow=" + (base.ModuleSettings.FollowOnMap.get_Value() ? "true" : "false");
		}

		public override IView GetSettingsView()
		{
			return (IView)(object)new SettingsView(base.Gw2ApiManager, base.IconService, base.TranslationService, base.SettingEventService, base.ModuleSettings, () => GetGlobalUrl(), () => GetGuildUrl());
		}

		protected override BaseModuleSettings DefineModuleSettings(SettingCollection settings)
		{
			return new ModuleSettings(settings, ((Module)this).get_Version());
		}

		protected override string GetDirectoryName()
		{
			return null;
		}

		protected override AsyncTexture2D GetEmblem()
		{
			return null;
		}

		protected override AsyncTexture2D GetCornerIcon()
		{
			return null;
		}
	}
}
