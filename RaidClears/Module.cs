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
using RaidClears.Dungeons.Controls;
using RaidClears.Dungeons.Model;
using RaidClears.Dungeons.Services;
using RaidClears.Raids.Controls;
using RaidClears.Raids.Model;
using RaidClears.Raids.Services;
using RaidClears.Settings;
using Settings.Enums;

namespace RaidClears
{
	[Export(typeof(Module))]
	public class Module : Module
	{
		private static readonly Logger Logger = Logger.GetLogger<Module>();

		private const int BUFFER_MS = 50;

		private const int MINUTE_MS = 60000;

		private double _lastApiCheck = -1.0;

		private double _API_QUERY_INTERVAL = 300100.0;

		private TextureService _textureService;

		private WingRotationService _wingRotationService;

		private SettingService _settingService;

		private CornerIconService _cornerIconService;

		private DungeonCornerIconService _dungeonCornerIconService;

		private RaidsPanel _raidsPanel;

		private DungeonsPanel _dungeonsPanel;

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
			return (IView)(object)new ModuleSettingsView(_settingService, this, _textureService);
		}

		protected override async Task LoadAsync()
		{
			_textureService = new TextureService(ContentsManager);
			_wingRotationService = new WingRotationService();
			_raidsPanel = new RaidsPanel(Logger, _settingService, Wing.GetWingMetaData(), _wingRotationService);
			_dungeonsPanel = new DungeonsPanel(Logger, _settingService, Dungeon.GetDungeonMetaData());
			_dungeonsPanel.UpdateClearedStatus(new ApiDungeons());
			SetTimeoutValueInMinutes((int)_settingService.RaidPanelApiPollingPeriod.get_Value());
			_settingService.RaidPanelIsVisibleKeyBind.get_Value().add_Activated((EventHandler<EventArgs>)OnRaidPanelDisplayKeybindActivated);
			_settingService.DungeonPanelIsVisibleKeyBind.get_Value().add_Activated((EventHandler<EventArgs>)OnDungeonPanelDisplayKeybindActivated);
			_settingService.RaidPanelApiPollingPeriod.add_SettingChanged((EventHandler<ValueChangedEventArgs<ApiPollPeriod>>)delegate(object s, ValueChangedEventArgs<ApiPollPeriod> e)
			{
				SetTimeoutValueInMinutes((int)e.get_NewValue());
			});
			_cornerIconService = new CornerIconService(_settingService.ShowRaidsCornerIconSetting, "Click to show/hide the Raid Clears window.\nIcon can be hidden by module settings.", delegate
			{
				_settingService.ToggleRaidPanelVisibility();
			}, _textureService);
			_dungeonCornerIconService = new DungeonCornerIconService(_settingService.ShowDungeonCornerIconSetting, _settingService.DungeonsEnabled, "Click to show/hide the Dungeon Clears window.\nIcon can be hidden by module settings.", delegate
			{
				_settingService.ToggleDungeonPanelVisibility();
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
			_settingService.DungeonPanelIsVisibleKeyBind.get_Value().remove_Activated((EventHandler<EventArgs>)OnDungeonPanelDisplayKeybindActivated);
			Gw2ApiManager.remove_SubtokenUpdated((EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>>)Gw2ApiManager_SubtokenUpdated);
			RaidsPanel raidsPanel = _raidsPanel;
			if (raidsPanel != null)
			{
				((Control)raidsPanel).Dispose();
			}
			DungeonsPanel dungeonsPanel = _dungeonsPanel;
			if (dungeonsPanel != null)
			{
				((Control)dungeonsPanel).Dispose();
			}
			_textureService?.Dispose();
			_cornerIconService?.Dispose();
			_dungeonCornerIconService?.Dispose();
		}

		protected override void Update(GameTime gameTime)
		{
			_raidsPanel?.ShowOrHide();
			_dungeonsPanel?.ShowOrHide();
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
				(ApiRaids, bool) obj2 = await GetCurrentClearsService.GetClearsFromApi(Gw2ApiManager, Logger);
				var (weeklyClears2, _) = obj2;
				if (!obj2.Item2)
				{
					_raidsPanel.UpdateClearedStatus(weeklyClears2);
				}
			});
			if (!_settingService.DungeonsEnabled.get_Value())
			{
				return;
			}
			Task.Run(async delegate
			{
				(ApiDungeons, bool) obj = await DungeonsClearsService.GetDungeonClearsFromApi(Gw2ApiManager, Logger);
				var (weeklyClears, _) = obj;
				if (!obj.Item2)
				{
					_dungeonsPanel.UpdateClearedStatus(weeklyClears);
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

		private void OnDungeonPanelDisplayKeybindActivated(object sender, EventArgs e)
		{
			_settingService.ToggleDungeonPanelVisibility();
		}
	}
}
