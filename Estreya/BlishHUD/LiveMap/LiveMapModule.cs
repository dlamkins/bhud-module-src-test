using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
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
using Estreya.BlishHUD.Shared.Helpers;
using Estreya.BlishHUD.Shared.Modules;
using Estreya.BlishHUD.Shared.Settings;
using Estreya.BlishHUD.Shared.Threading;
using Estreya.BlishHUD.Shared.Utils;
using Gw2Sharp.Models;
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

		private const string LIVE_MAP_BASE_API_URL = "https://gw2map.api.estreya.de/v1";

		private const string LIVE_MAP_GLOBAL_API_URL = "https://gw2map.api.estreya.de/v1/global/write";

		private const string LIVE_MAP_GUILD_API_URL = "https://gw2map.api.estreya.de/v1/guild/write";

		public const string LIVE_MAP_GLOBAL_URL = "https://gw2map.estreya.de";

		public const string LIVE_MAP_GUILD_URL = "https://gw2map.estreya.de/guild/{0}";

		private SocketIO GlobalSocket = new SocketIO("https://gw2map.api.estreya.de/v1/global/write", new SocketIOOptions
		{
			Transport = TransportProtocol.WebSocket
		});

		private SocketIO GuildSocket = new SocketIO("https://gw2map.api.estreya.de/v1/guild/write", new SocketIOOptions
		{
			Transport = TransportProtocol.WebSocket
		});

		private TimeSpan _sendInterval = TimeSpan.FromMilliseconds(250.0);

		private AsyncRef<double> _lastSend = new AsyncRef<double>(0.0);

		private Player _lastSendPlayer;

		private TimeSpan _guildFetchInterval = TimeSpan.FromSeconds(30.0);

		private AsyncRef<double> _lastGuildFetch = new AsyncRef<double>(0.0);

		private string _accountName;

		private string _guildId;

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
			_lastSend.Value = _sendInterval.TotalMilliseconds;
			_lastGuildFetch.Value = _guildFetchInterval.TotalMilliseconds;
			BaseModule<LiveMapModule, ModuleSettings>.Instance = this;
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
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			base.OnModuleLoaded(e);
		}

		private void Gw2ApiManager_SubtokenUpdated(object sender, ValueEventArgs<IEnumerable<TokenPermission>> e)
		{
			Task.Run((Func<Task>)FetchAccountName);
			_lastGuildFetch = _guildFetchInterval.TotalMilliseconds;
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
				_guildId = (await ((IBulkExpandableClient<Character, string>)(object)base.Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Characters()).GetAsync(GameService.Gw2Mumble.get_PlayerCharacter().get_Name(), default(CancellationToken))).get_Guild().ToString();
			}
		}

		private async Task SendPosition()
		{
			if (string.IsNullOrWhiteSpace(_accountName) || !GameService.Gw2Mumble.get_IsAvailable() || GameService.Gw2Mumble.get_TimeSinceTick().TotalSeconds > 0.5)
			{
				return;
			}
			Coordinates2 position = GameService.Gw2Mumble.get_UI().get_MapPosition();
			Vector3 cameraForward = ((base.ModuleSettings.PlayerFacingType.get_Value() == PlayerFacingType.Camera) ? GameService.Gw2Mumble.get_PlayerCamera().get_Forward() : GameService.Gw2Mumble.get_PlayerCharacter().get_Forward());
			double cameraAngle = Math.Atan2(cameraForward.X, cameraForward.Y) * 180.0 / Math.PI;
			if (cameraAngle < 0.0)
			{
				cameraAngle += 360.0;
			}
			Player player = new Player
			{
				Identification = new PlayerIdentification
				{
					Account = _accountName,
					Character = GameService.Gw2Mumble.get_PlayerCharacter().get_Name(),
					GuildId = _guildId
				},
				Position = new PlayerPosition
				{
					X = ((Coordinates2)(ref position)).get_X(),
					Y = ((Coordinates2)(ref position)).get_Y()
				},
				Facing = new PlayerFacing
				{
					Angle = cameraAngle
				}
			};
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

		public override IView GetSettingsView()
		{
			return (IView)(object)new SettingsView(base.Gw2ApiManager, base.IconState, base.TranslationState, base.ModuleSettings, () => GuildId, delegate
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				Coordinates2 mapPosition2 = GameService.Gw2Mumble.get_UI().get_MapPosition();
				return ((Coordinates2)(ref mapPosition2)).get_X();
			}, delegate
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				Coordinates2 mapPosition = GameService.Gw2Mumble.get_UI().get_MapPosition();
				return ((Coordinates2)(ref mapPosition)).get_Y();
			});
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
