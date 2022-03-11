using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using DiscordRPC;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;

namespace Discord_Rich_Presence_Module
{
	[Export(typeof(Module))]
	public class DiscordRichPresenceModule : Module
	{
		private enum MapType
		{
			PvP = 2,
			Instance = 4,
			PvE = 5,
			Eternal_Battlegrounds = 9,
			WvW_Blue = 10,
			WvW_Green = 11,
			WvW_Red = 12,
			Edge_of_The_Mists = 0xF,
			Dry_Top = 0x10,
			Armistice_Bastion = 18
		}

		private static readonly Logger Logger = Logger.GetLogger(typeof(DiscordRichPresenceModule));

		internal static DiscordRichPresenceModule ModuleInstance;

		private const string DISCORD_APP_ID = "498585183792922677";

		private readonly Dictionary<string, string> _mapOverrides = new Dictionary<string, string>
		{
			{ "1206", "fractals_of_the_mists" },
			{ "350", "fractals_of_the_mists" },
			{ "95", "eternal_battlegrounds" },
			{ "96", "eternal_battlegrounds" }
		};

		private readonly Dictionary<int, string> _contextOverrides = new Dictionary<int, string>();

		private DiscordRpcClient _rpcClient;

		private DateTime _startTime;

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		internal Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		[ImportingConstructor]
		public DiscordRichPresenceModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
			ModuleInstance = this;
		}

		protected override void DefineSettings(SettingCollection settings)
		{
		}

		protected override void Initialize()
		{
		}

		protected override async Task LoadAsync()
		{
			GameService.Gw2Mumble.get_PlayerCharacter().add_NameChanged((EventHandler<ValueEventArgs<string>>)delegate
			{
				CurrentMapOnMapChanged(null, new ValueEventArgs<int>(GameService.Gw2Mumble.get_CurrentMap().get_Id()));
			});
			GameService.Gw2Mumble.get_CurrentMap().add_MapChanged((EventHandler<ValueEventArgs<int>>)CurrentMapOnMapChanged);
			GameService.GameIntegration.add_Gw2Started((EventHandler<EventArgs>)delegate
			{
				InitRichPresence();
			});
			GameService.GameIntegration.add_Gw2Closed((EventHandler<EventArgs>)delegate
			{
				CleanUpRichPresence();
			});
			InitRichPresence();
		}

		private void CurrentMapOnMapChanged(object sender, ValueEventArgs<int> e)
		{
			((IBulkExpandableClient<Map, int>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Maps()).GetAsync(e.get_Value(), default(CancellationToken)).ContinueWith(delegate(Task<Map> mapTask)
			{
				if (!mapTask.IsFaulted && mapTask.Result != null)
				{
					UpdateDetails(mapTask.Result);
				}
			});
		}

		private void UpdateDetails(Map map)
		{
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Expected I4, but got Unknown
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Expected I4, but got Unknown
			if (map.get_Id() > 0)
			{
				_rpcClient?.SetPresence(new RichPresence
				{
					Details = DiscordUtil.TruncateLength(GameService.Gw2Mumble.get_PlayerCharacter().get_Name(), 128),
					State = DiscordUtil.TruncateLength("in " + map.get_Name(), 128),
					Assets = new Assets
					{
						LargeImageKey = DiscordUtil.TruncateLength(_mapOverrides.ContainsKey(map.get_Id().ToString()) ? _mapOverrides[map.get_Id().ToString()] : DiscordUtil.GetDiscordSafeString(map.get_Name()), 32),
						LargeImageText = DiscordUtil.TruncateLength(map.get_Name(), 128),
						SmallImageKey = DiscordUtil.TruncateLength(((MapType)map.get_Type().get_Value()).ToString().ToLowerInvariant(), 32),
						SmallImageText = DiscordUtil.TruncateLength(((MapType)map.get_Type().get_Value()).ToString().Replace("_", " "), 128)
					},
					Timestamps = new Timestamps
					{
						Start = _startTime
					}
				});
				_rpcClient?.Invoke();
			}
		}

		private void InitRichPresence()
		{
			try
			{
				_startTime = GameService.GameIntegration.get_Gw2Process().StartTime.ToUniversalTime();
			}
			catch (Exception)
			{
				_startTime = DateTime.UtcNow;
			}
			_rpcClient = new DiscordRpcClient("498585183792922677");
			_rpcClient.Initialize();
			CurrentMapOnMapChanged(null, new ValueEventArgs<int>(GameService.Gw2Mumble.get_CurrentMap().get_Id()));
		}

		private void CleanUpRichPresence()
		{
			_rpcClient?.Dispose();
			_rpcClient = null;
		}

		protected override void Update(GameTime gameTime)
		{
		}

		protected override void Unload()
		{
			ModuleInstance = null;
			CleanUpRichPresence();
		}
	}
}
