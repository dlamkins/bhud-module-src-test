using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Modules;
using Blish_HUD.Settings;
using Estreya.BlishHUD.LiveMap.Models;
using Estreya.BlishHUD.LiveMap.Models.Player;
using Estreya.BlishHUD.LiveMap.UI.Views;
using Estreya.BlishHUD.Shared.Extensions;
using Estreya.BlishHUD.Shared.Helpers;
using Estreya.BlishHUD.Shared.Modules;
using Estreya.BlishHUD.Shared.Settings;
using Estreya.BlishHUD.Shared.Threading;
using Estreya.BlishHUD.Shared.Utils;
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
		private new static readonly Logger Logger = Logger.GetLogger<LiveMapModule>();

		private const string LIVE_MAP_BASE_API_URL = "https://gw2map.api.estreya.de/v2";

		private const string LIVE_MAP_GLOBAL_API_URL = "https://gw2map.api.estreya.de/v2/global/write";

		private const string LIVE_MAP_GUILD_API_URL = "https://gw2map.api.estreya.de/v2/guild/write";

		private SocketIO GlobalSocket = new SocketIO("https://gw2map.api.estreya.de/v2/global/write", new SocketIOOptions
		{
			Transport = TransportProtocol.WebSocket
		});

		private SocketIO GuildSocket = new SocketIO("https://gw2map.api.estreya.de/v2/guild/write", new SocketIOOptions
		{
			Transport = TransportProtocol.WebSocket
		});

		private TimeSpan _sendInterval = TimeSpan.FromMilliseconds(250.0);

		private AsyncRef<double> _lastSend = new AsyncRef<double>(0.0);

		private Player _lastSendPlayer;

		private TimeSpan _guildFetchInterval = TimeSpan.FromSeconds(30.0);

		private AsyncRef<double> _lastGuildFetch = new AsyncRef<double>(0.0);

		private TimeSpan _wvwColorFetchInterval = TimeSpan.FromHours(1.0);

		private AsyncRef<double> _lastWvWColorFetch = new AsyncRef<double>(0.0);

		private string _accountName;

		private string _guildId;

		private string _wvwColor;

		private Map _map;

		public string LIVE_MAP_GLOBAL_URL
		{
			get
			{
				Map map = _map;
				return $"https://gw2map.estreya.de/{((map == null) ? 1 : map.get_ContinentId())}";
			}
		}

		public string LIVE_MAP_GUILD_URL => LIVE_MAP_GLOBAL_URL + "/guild/{0}";

		public string GuildId => _guildId;

		public override string WebsiteModuleName => "live-map";

		protected override string API_VERSION_NO => "1";

		[ImportingConstructor]
		public LiveMapModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: base(moduleParameters)
		{
		}

		protected override void Initialize()
		{
			base.Initialize();
			base.Gw2ApiManager.add_SubtokenUpdated((EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>>)Gw2ApiManager_SubtokenUpdated);
			GameService.Gw2Mumble.get_PlayerCharacter().add_NameChanged((EventHandler<ValueEventArgs<string>>)PlayerCharacter_NameChanged);
			GameService.Gw2Mumble.get_CurrentMap().add_MapChanged((EventHandler<ValueEventArgs<int>>)CurrentMap_MapChanged);
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
			await GlobalSocket.ConnectAsync();
			await GuildSocket.ConnectAsync();
			await FetchAccountName();
			await FetchGuildId();
			await FetchWvWColor();
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			base.OnModuleLoaded(e);
		}

		private void Gw2ApiManager_SubtokenUpdated(object sender, ValueEventArgs<IEnumerable<TokenPermission>> e)
		{
			Task.Run((Func<Task>)FetchAccountName);
			_lastGuildFetch.Value = _guildFetchInterval.TotalMilliseconds;
			_lastWvWColorFetch.Value = _wvwColorFetchInterval.TotalMilliseconds;
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
					Logger.Debug(ex, "Failed to fetch guild id:");
				}
			}
		}

		private async Task FetchWvWColor()
		{
			string color = "white";
			if (base.Gw2ApiManager.HasPermissions((IEnumerable<TokenPermission>)(object)new TokenPermission[1] { (TokenPermission)1 }))
			{
				try
				{
					int worldId = (await ((IBlobClient<Account>)(object)base.Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Account()).GetAsync(default(CancellationToken))).get_World();
					IApiV2ObjectList<WvwMatch> source = await ((IAllExpandableClient<WvwMatch>)(object)base.Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Wvw()
						.get_Matches()).AllAsync(default(CancellationToken));
					if (((IEnumerable<WvwMatch>)source).Where((WvwMatch m) => m.get_AllWorlds().get_Green().Contains(worldId)).Any())
					{
						color = "green";
					}
					if (((IEnumerable<WvwMatch>)source).Where((WvwMatch m) => m.get_AllWorlds().get_Red().Contains(worldId)).Any())
					{
						color = "red";
					}
					if (((IEnumerable<WvwMatch>)source).Where((WvwMatch m) => m.get_AllWorlds().get_Blue().Contains(worldId)).Any())
					{
						color = "blue";
					}
				}
				catch (Exception ex)
				{
					Logger.Debug(ex, "Failed to fetch wvw team color:");
				}
			}
			_wvwColor = color;
		}

		private async Task SendPosition()
		{
			if (string.IsNullOrWhiteSpace(_accountName) || !GameService.Gw2Mumble.get_IsAvailable() || GameService.Gw2Mumble.get_TimeSinceTick().TotalSeconds > 0.5 || (base.ModuleSettings.StreamerModeEnabled.get_Value() && StreamerUtils.IsStreaming()))
			{
				return;
			}
			Player player = GetPlayer();
			if (_lastSendPlayer != null && player.Equals(_lastSendPlayer))
			{
				return;
			}
			_lastSendPlayer = player;
			try
			{
				switch (base.ModuleSettings.PublishType.get_Value())
				{
				case PublishType.Global:
					await PublishToGlobal(player);
					break;
				case PublishType.Guild:
					await PublishToGuild(player);
					break;
				case PublishType.Both:
					await PublishToGlobal(player);
					await PublishToGuild(player);
					break;
				}
			}
			catch (Exception ex)
			{
				Logger.Debug(ex.Message);
			}
		}

		private async Task PublishToGlobal(Player player)
		{
			await GlobalSocket.EmitAsync("update", player);
		}

		private async Task PublishToGuild(Player player)
		{
			if (!string.IsNullOrWhiteSpace(player.Identification.GuildId))
			{
				await GuildSocket.EmitAsync("update", player);
			}
		}

		protected override void Update(GameTime gameTime)
		{
			UpdateUtil.UpdateAsync(SendPosition, gameTime, _sendInterval.TotalMilliseconds, _lastSend, doLogging: false);
			UpdateUtil.UpdateAsync(FetchGuildId, gameTime, _guildFetchInterval.TotalMilliseconds, _lastGuildFetch);
			UpdateUtil.UpdateAsync(FetchWvWColor, gameTime, _wvwColorFetchInterval.TotalMilliseconds, _lastWvWColorFetch);
		}

		protected override void Unload()
		{
			base.Unload();
			base.Gw2ApiManager.remove_SubtokenUpdated((EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>>)Gw2ApiManager_SubtokenUpdated);
			GameService.Gw2Mumble.get_PlayerCharacter().remove_NameChanged((EventHandler<ValueEventArgs<string>>)PlayerCharacter_NameChanged);
			AsyncHelper.RunSync(GlobalSocket.DisconnectAsync);
			AsyncHelper.RunSync(GuildSocket.DisconnectAsync);
			BaseModule<LiveMapModule, ModuleSettings>.Instance = null;
		}

		public Player GetPlayer()
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			Vector2 position = _map?.WorldMeterCoordsToMapCoords(GameService.Gw2Mumble.get_PlayerCharacter().get_Position()) ?? Vector2.get_Zero();
			Vector3 cameraForward = ((base.ModuleSettings.PlayerFacingType.get_Value() == PlayerFacingType.Camera) ? GameService.Gw2Mumble.get_PlayerCamera().get_Forward() : GameService.Gw2Mumble.get_PlayerCharacter().get_Forward());
			double cameraAngle = Math.Atan2(cameraForward.X, cameraForward.Y) * 180.0 / Math.PI;
			if (cameraAngle < 0.0)
			{
				cameraAngle += 360.0;
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
			PlayerMap playerMap = new PlayerMap();
			Map map = _map;
			playerMap.Continent = ((map != null) ? map.get_ContinentId() : (-1));
			Map map2 = _map;
			playerMap.Name = ((map2 != null) ? map2.get_Name() : null);
			Map map3 = _map;
			playerMap.ID = ((map3 != null) ? map3.get_Id() : (-1));
			playerMap.Position = new PlayerPosition
			{
				X = position.X,
				Y = position.Y
			};
			obj.Map = playerMap;
			obj.Facing = new PlayerFacing
			{
				Angle = cameraAngle
			};
			obj.WvW = new PlayerWvW
			{
				TeamColor = _wvwColor
			};
			obj.Commander = !base.ModuleSettings.HideCommander.get_Value() && GameService.Gw2Mumble.get_PlayerCharacter().get_IsCommander();
			return obj;
		}

		public override IView GetSettingsView()
		{
			return (IView)(object)new SettingsView(base.Gw2ApiManager, base.IconState, base.TranslationState, base.ModuleSettings, () => GuildId, () => GetPlayer().Map.Position.X, () => GetPlayer().Map.Position.Y, () => LIVE_MAP_GLOBAL_URL, () => LIVE_MAP_GUILD_URL);
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
