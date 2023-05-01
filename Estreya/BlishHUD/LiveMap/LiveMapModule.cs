using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
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
using Microsoft.Xna.Framework;
using SocketIOClient;
using SocketIOClient.Transport;

namespace Estreya.BlishHUD.LiveMap
{
	[Export(typeof(Module))]
	public class LiveMapModule : BaseModule<LiveMapModule, ModuleSettings>
	{
		public const string LIVE_MAP_BROWSER_URL = "https://gw2map.estreya.de/";

		private SocketIO GlobalSocket;

		private TimeSpan _sendInterval = TimeSpan.FromMilliseconds(250.0);

		private AsyncRef<double> _lastSend = new AsyncRef<double>(0.0);

		private Player _lastSendPlayer;

		private TimeSpan _guildFetchInterval = TimeSpan.FromSeconds(30.0);

		private AsyncRef<double> _lastGuildFetch = new AsyncRef<double>(0.0);

		private TimeSpan _wvwFetchInterval = TimeSpan.FromHours(1.0);

		private AsyncRef<double> _lastWvWFetch = new AsyncRef<double>(0.0);

		private string _accountName;

		private string _guildId;

		private PlayerWvW _wvw;

		private Map _map;

		private string LIVE_MAP_API_URL => base.MODULE_API_URL + "/write";

		public string GuildId => _guildId;

		public override string UrlModuleName => "live-map";

		protected override string API_VERSION_NO => "1";

		[ImportingConstructor]
		public LiveMapModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: base(moduleParameters)
		{
		}

		protected override void Initialize()
		{
			base.Initialize();
			GlobalSocket = new SocketIO(LIVE_MAP_API_URL, new SocketIOOptions
			{
				Transport = TransportProtocol.WebSocket
			});
			base.Gw2ApiManager.add_SubtokenUpdated((EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>>)Gw2ApiManager_SubtokenUpdated);
			GameService.Gw2Mumble.get_PlayerCharacter().add_NameChanged((EventHandler<ValueEventArgs<string>>)PlayerCharacter_NameChanged);
			GameService.Gw2Mumble.get_CurrentMap().add_MapChanged((EventHandler<ValueEventArgs<int>>)CurrentMap_MapChanged);
			GameService.ArcDps.get_Common().Activate();
			_lastSend.Value = _sendInterval.TotalMilliseconds;
			_lastGuildFetch.Value = _guildFetchInterval.TotalMilliseconds;
			BaseModule<LiveMapModule, ModuleSettings>.Instance = this;
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

		protected override async Task LoadAsync()
		{
			await base.LoadAsync();
			GlobalSocket.On("interval", delegate(SocketIOResponse resp)
			{
				int value = resp.GetValue<int>();
				_sendInterval = TimeSpan.FromMilliseconds(value);
			});
			Task.Run((Func<Task>)GlobalSocket.ConnectAsync);
			await FetchAccountName();
			await FetchGuildId();
			await FetchWvW();
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
			_lastGuildFetch = _guildFetchInterval.TotalMilliseconds;
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
					_guildId = (await ((IBulkExpandableClient<Character, string>)(object)base.Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Characters()).GetAsync(GameService.Gw2Mumble.get_PlayerCharacter().get_Name(), default(CancellationToken))).get_Guild().ToString();
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
				_lastSendPlayer = player;
				try
				{
					byte[] compressed = Compress(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(player)));
					await PublishToGlobal(compressed);
				}
				catch (Exception ex)
				{
					base.Logger.Debug(ex.Message);
				}
			}
		}

		private async Task PublishToGlobal(byte[] data)
		{
			if (GlobalSocket.Connected)
			{
				await GlobalSocket.EmitAsync("update", data);
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
			AsyncHelper.RunSync(GlobalSocket.DisconnectAsync);
			BaseModule<LiveMapModule, ModuleSettings>.Instance = null;
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
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			Vector2 position = _map?.WorldMeterCoordsToMapCoords(GameService.Gw2Mumble.get_PlayerCharacter().get_Position()) ?? Vector2.get_Zero();
			Vector3 forward = GameService.Gw2Mumble.get_PlayerCharacter().get_Forward();
			double angle = Math.Atan2(forward.X, forward.Y) * 180.0 / Math.PI;
			if (angle < 0.0)
			{
				angle += 360.0;
			}
			Player obj = new Player
			{
				Identification = new PlayerIdentification
				{
					Account = _accountName,
					Character = GameService.Gw2Mumble.get_PlayerCharacter().get_Name(),
					GuildId = _guildId
				}
			};
			PlayerMap obj2 = new PlayerMap
			{
				Continent = GetContinentId(_map)
			};
			Map map = _map;
			obj2.Name = ((map != null) ? map.get_Name() : null);
			Map map2 = _map;
			obj2.ID = ((map2 != null) ? map2.get_Id() : (-1));
			obj2.Position = new PlayerPosition
			{
				X = position.X,
				Y = position.Y
			};
			obj.Map = obj2;
			obj.Facing = new PlayerFacing
			{
				Angle = angle
			};
			obj.WvW = _wvw;
			obj.Group = new PlayerGroup
			{
				Squad = (base.ModuleSettings.SendGroupInformation.get_Value() ? (from p in GameService.ArcDps.get_Common().get_PlayersInSquad().Values
					select ((Player)(ref p)).get_AccountName().Trim(':') into p
					where p != _accountName
					select p).ToArray() : null)
			};
			obj.Commander = !base.ModuleSettings.HideCommander.get_Value() && GameService.Gw2Mumble.get_PlayerCharacter().get_IsCommander();
			return obj;
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
			return new ModuleSettings(settings);
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
