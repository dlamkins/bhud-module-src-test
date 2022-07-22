using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using RaidClears.Raids.Controls;
using RaidClears.Raids.Model;
using RaidClears.Raids.Services;
using RaidClears.Settings;

namespace RaidClears
{
	[Export(typeof(Module))]
	public class Module : Module
	{
		private static readonly Logger Logger = Logger.GetLogger<Module>();

		private static Wing[] wingInfo = new Wing[7]
		{
			new Wing("Spirit Vale", 1, "SV", new Encounter[4]
			{
				new Encounter("vale_guardian", "Vale Guardian", "VG"),
				new Encounter("spirit_woods", "Spirit Run", "SR"),
				new Encounter("gorseval", "Gorseval", "G"),
				new Encounter("sabetha", "Sabetha", "S")
			}),
			new Wing("Salvation Pass", 2, "SP", new Encounter[3]
			{
				new Encounter("slothasor", "Slothasor", "S"),
				new Encounter("bandit_trio", "Bandit Trio", "B3"),
				new Encounter("matthias", "Matthias Gabrel", "M")
			}),
			new Wing("Stronghold of the Faithful", 3, "SotF", new Encounter[4]
			{
				new Encounter("escort", "Escort", "E"),
				new Encounter("keep_construct", "Keep Construct", "KC"),
				new Encounter("twisted_castle", "Twisted Castel", "TC"),
				new Encounter("xera", "Xera", "X")
			}),
			new Wing("Bastion of the Penitent", 4, "BotP", new Encounter[4]
			{
				new Encounter("cairn", "Cairn the Indominable", "C"),
				new Encounter("mursaat_overseer", "Mursaat Overseer", "MO"),
				new Encounter("samarog", "Samarog", "S"),
				new Encounter("deimos", "Deimos", "D")
			}),
			new Wing("Hall of Chains", 5, "HoC", new Encounter[4]
			{
				new Encounter("soulless_horror", "Soulless Horror", "SH"),
				new Encounter("river_of_souls", "River of Souls", "R"),
				new Encounter("statues_of_grenth", "Statues of Grenth", "S"),
				new Encounter("voice_in_the_void", "Dhuum", "D")
			}),
			new Wing("Mythwright Gambit", 6, "MG", new Encounter[3]
			{
				new Encounter("conjured_amalgamate", "Conjured Amalgamate", "CA"),
				new Encounter("twin_largos", "Twin Largos", "TL"),
				new Encounter("qadim", "Qadim", "Q1")
			}),
			new Wing("The Key of Ahdashim", 7, "TKoA", new Encounter[4]
			{
				new Encounter("gate", "Gate", "G"),
				new Encounter("adina", "Cardinal Adina", "A"),
				new Encounter("sabir", "Cardinal Sabir", "S"),
				new Encounter("qadim_the_peerless", "Qadim the Peerless", "Q2")
			})
		};

		private const int BUFFER_MS = 50;

		private const int MINUTE_MS = 60000;

		private double _lastApiCheck = -1.0;

		private double _API_QUERY_INTERVAL = 300100.0;

		private SettingService _settingService;

		private TextureService _textureService;

		private CornerIconService _cornerIconService;

		private RaidsPanel _raidsPanel;

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		internal Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		[ImportingConstructor]
		public Module([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			_settingService = new SettingService(settings);
		}

		public override IView GetSettingsView()
		{
			return (IView)(object)new ModuleSettingsView(_settingService, this);
		}

		protected override async Task LoadAsync()
		{
			_textureService = new TextureService(ContentsManager);
			_raidsPanel = new RaidsPanel(Logger, _settingService, _textureService, wingInfo);
			SetTimeoutValueInMinutes((int)_settingService.RaidPanelApiPollingPeriod.get_Value());
			_settingService.RaidPanelIsVisibleKeyBind.get_Value().add_Activated((EventHandler<EventArgs>)OnRaidPanelDisplayKeybindActivated);
			_settingService.RaidPanelApiPollingPeriod.add_SettingChanged((EventHandler<ValueChangedEventArgs<ApiPollPeriod>>)delegate(object s, ValueChangedEventArgs<ApiPollPeriod> e)
			{
				SetTimeoutValueInMinutes((int)e.get_NewValue());
			});
			_cornerIconService = new CornerIconService(_settingService.ShowRaidsCornerIconSetting, "Click to show/hide the Raid Clears window.\nIcon can be hidden by module settings.", delegate
			{
				_settingService.ToggleRaidPanelVisibility();
			}, _textureService);
			Gw2ApiManager.add_SubtokenUpdated((EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>>)Gw2ApiManager_SubtokenUpdated);
			if (Gw2ApiManager.HasPermissions((IEnumerable<TokenPermission>)GetCurrentClearsService.NECESSARY_API_TOKEN_PERMISSIONS))
			{
				_lastApiCheck = _API_QUERY_INTERVAL;
			}
		}

		protected override void Unload()
		{
			_settingService.RaidPanelIsVisibleKeyBind.get_Value().remove_Activated((EventHandler<EventArgs>)OnRaidPanelDisplayKeybindActivated);
			Gw2ApiManager.remove_SubtokenUpdated((EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>>)Gw2ApiManager_SubtokenUpdated);
			RaidsPanel raidsPanel = _raidsPanel;
			if (raidsPanel != null)
			{
				((Control)raidsPanel).Dispose();
			}
			_textureService?.Dispose();
			_cornerIconService?.Dispose();
		}

		protected override void Update(GameTime gameTime)
		{
			_raidsPanel?.ShowOrHide();
			ApiPollTimeout(gameTime.get_ElapsedGameTime().TotalMilliseconds);
		}

		private void SetTimeoutValueInMinutes(int minutes)
		{
			_API_QUERY_INTERVAL = minutes * 60000 + 50;
		}

		public int GetTimeoutSecondsRemaining()
		{
			if (_lastApiCheck == -1.0)
			{
				return -1;
			}
			return (int)((_API_QUERY_INTERVAL - _lastApiCheck) / 1000.0);
		}

		private void ApiPollTimeout(double elapsedTime)
		{
			if (!(_lastApiCheck >= 0.0))
			{
				return;
			}
			_lastApiCheck += elapsedTime;
			if (!(_lastApiCheck >= _API_QUERY_INTERVAL))
			{
				return;
			}
			_lastApiCheck = 0.0;
			Task.Run(async delegate
			{
				(ApiRaids, bool) obj = await GetCurrentClearsService.GetClearsFromApi(Gw2ApiManager, Logger);
				var (weeklyClears, _) = obj;
				if (!obj.Item2)
				{
					_raidsPanel.UpdateClearedStatus(weeklyClears);
				}
			});
		}

		private void Gw2ApiManager_SubtokenUpdated(object sender, ValueEventArgs<IEnumerable<TokenPermission>> e)
		{
			_lastApiCheck = _API_QUERY_INTERVAL;
		}

		private void OnRaidPanelDisplayKeybindActivated(object sender, EventArgs e)
		{
			_settingService.ToggleRaidPanelVisibility();
		}
	}
}
