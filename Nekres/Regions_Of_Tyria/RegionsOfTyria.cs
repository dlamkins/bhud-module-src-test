using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Extended;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Nekres.Regions_Of_Tyria.Geometry;
using Nekres.Regions_Of_Tyria.UI.Controls;

namespace Nekres.Regions_Of_Tyria
{
	[Export(typeof(Module))]
	public class RegionsOfTyria : Module
	{
		internal static readonly Logger Logger = Logger.GetLogger(typeof(RegionsOfTyria));

		internal static RegionsOfTyria Instance;

		private SettingEntry<float> _showDuration;

		private SettingEntry<float> _fadeInDuration;

		private SettingEntry<float> _fadeOutDuration;

		private SettingEntry<float> _effectDuration;

		private SettingEntry<bool> _toggleMapNotification;

		private SettingEntry<bool> _toggleSectorNotification;

		private SettingEntry<bool> _includeRegionInMapNotification;

		private SettingEntry<bool> _includeMapInSectorNotification;

		private SettingEntry<bool> _hideInCombat;

		internal SettingEntry<bool> Translate;

		internal SettingEntry<MapNotification.RevealEffect> RevealEffect;

		internal SettingEntry<float> VerticalPosition;

		internal SettingEntry<float> FontSize;

		private AsyncCache<int, Map> _mapRepository;

		private AsyncCache<int, List<Sector>> _sectorRepository;

		private string _currentMap;

		private string _currentSector;

		private int _prevSectorId;

		private int _prevMapId;

		private DateTime _lastIndicatorChange = DateTime.UtcNow;

		private NotificationIndicator _notificationIndicator;

		private double _lastRun;

		private DateTime _lastUpdate = DateTime.UtcNow;

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		internal Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		[ImportingConstructor]
		public RegionsOfTyria([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
			Instance = this;
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			SettingCollection generalCol = settings.AddSubCollection("general", true, (Func<string>)(() => "General"));
			Translate = generalCol.DefineSetting<bool>("translate", true, (Func<string>)(() => "Translate from New Krytan"), (Func<string>)(() => "Makes zone notifications appear in New Krytan before they are revealed to you."));
			RevealEffect = generalCol.DefineSetting<MapNotification.RevealEffect>("reveal_effect", MapNotification.RevealEffect.Decode, (Func<string>)(() => "Reveal Effect"), (Func<string>)(() => "The type of transition to use for revealing zone names."));
			VerticalPosition = generalCol.DefineSetting<float>("pos_y", 25f, (Func<string>)(() => "Vertical Position"), (Func<string>)(() => "Sets the vertical position of area notifications."));
			FontSize = generalCol.DefineSetting<float>("font_size", 76f, (Func<string>)(() => "Font Size"), (Func<string>)(() => "Sets the size of the zone notification text."));
			_hideInCombat = generalCol.DefineSetting<bool>("hide_if_combat", true, (Func<string>)(() => "Disable Zone Notifications in Combat"), (Func<string>)(() => "Disables zone notifications during combat."));
			SettingCollection durationCol = settings.AddSubCollection("durations", true, (Func<string>)(() => "Durations"));
			_showDuration = durationCol.DefineSetting<float>("show", 80f, (Func<string>)(() => "Show Duration"), (Func<string>)(() => "The duration in which to stay in full opacity."));
			_fadeInDuration = durationCol.DefineSetting<float>("fade_in", 45f, (Func<string>)(() => "Fade-In Duration"), (Func<string>)(() => "The duration of the fade-in."));
			_fadeOutDuration = durationCol.DefineSetting<float>("fade_out", 65f, (Func<string>)(() => "Fade-Out Duration"), (Func<string>)(() => "The duration of the fade-out."));
			_effectDuration = durationCol.DefineSetting<float>("effect", 30f, (Func<string>)(() => "Reveal Effect Duration"), (Func<string>)(() => "The duration of the reveal or translation effect."));
			SettingCollection mapCol = settings.AddSubCollection("map_alert", true, (Func<string>)(() => "Map Notification"));
			_toggleMapNotification = mapCol.DefineSetting<bool>("enabled", true, (Func<string>)(() => "Enabled"), (Func<string>)(() => "Shows a map's name after entering it."));
			_includeRegionInMapNotification = mapCol.DefineSetting<bool>("prefix_region", true, (Func<string>)(() => "Include Region"), (Func<string>)(() => "Shows the region's name above the map's name."));
			SettingCollection sectorCol = settings.AddSubCollection("sector_alert", true, (Func<string>)(() => "Sector Notification"));
			_toggleSectorNotification = sectorCol.DefineSetting<bool>("enabled", true, (Func<string>)(() => "Enabled"), (Func<string>)(() => "Shows a sector's name after entering."));
			_includeMapInSectorNotification = sectorCol.DefineSetting<bool>("prefix_map", true, (Func<string>)(() => "Include Map"), (Func<string>)(() => "Shows the map's name above the sector's name."));
		}

		protected override void Initialize()
		{
			_mapRepository = new AsyncCache<int, Map>(RequestMap);
			_sectorRepository = new AsyncCache<int, List<Sector>>(RequestSectors);
		}

		protected override async void Update(GameTime gameTime)
		{
			float playerSpeed = GameService.Gw2Mumble.get_PlayerCharacter().GetSpeed(gameTime);
			if (DateTime.UtcNow.Subtract(_lastIndicatorChange).TotalMilliseconds > 250.0 && _notificationIndicator != null)
			{
				((Control)_notificationIndicator).Dispose();
				_notificationIndicator = null;
			}
			if (GameService.Gw2Mumble.get_IsAvailable() && GameService.GameIntegration.get_Gw2Instance().get_IsInGame() && _toggleSectorNotification.get_Value() && !(playerSpeed > 55f) && (!_hideInCombat.get_Value() || !GameService.Gw2Mumble.get_PlayerCharacter().get_IsInCombat()) && !(gameTime.get_TotalGameTime().TotalMilliseconds - _lastRun < 10.0) && !(DateTime.UtcNow.Subtract(_lastUpdate).TotalSeconds < 20.0))
			{
				_lastRun = gameTime.get_ElapsedGameTime().TotalMilliseconds;
				_lastUpdate = DateTime.UtcNow;
				Map currentMap = await _mapRepository.GetItem(GameService.Gw2Mumble.get_CurrentMap().get_Id());
				Sector currentSector = await GetSector(currentMap);
				if (currentSector != null)
				{
					_currentMap = currentMap.get_Name();
					_currentSector = currentSector.Name;
					MapNotification.ShowNotification(_includeMapInSectorNotification.get_Value() ? currentMap.get_Name() : null, currentSector.Name, null, _showDuration.get_Value() / 100f * 5f, _fadeInDuration.get_Value() / 100f * 3f, _fadeOutDuration.get_Value() / 100f * 3f, _effectDuration.get_Value() / 100f * 3f);
				}
			}
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			MapNotification.UpdateFonts(FontSize.get_Value() / 100f);
			GameService.Gw2Mumble.get_CurrentMap().add_MapChanged((EventHandler<ValueEventArgs<int>>)OnMapChanged);
			GameService.Overlay.add_UserLocaleChanged((EventHandler<ValueEventArgs<CultureInfo>>)OnUserLocaleChanged);
			VerticalPosition.add_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)OnVerticalPositionChanged);
			FontSize.add_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)OnFontSizeChanged);
			((Module)this).OnModuleLoaded(e);
		}

		protected override void Unload()
		{
			NotificationIndicator notificationIndicator = _notificationIndicator;
			if (notificationIndicator != null)
			{
				((Control)notificationIndicator).Dispose();
			}
			VerticalPosition.remove_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)OnVerticalPositionChanged);
			FontSize.remove_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)OnFontSizeChanged);
			GameService.Gw2Mumble.get_CurrentMap().remove_MapChanged((EventHandler<ValueEventArgs<int>>)OnMapChanged);
			GameService.Overlay.remove_UserLocaleChanged((EventHandler<ValueEventArgs<CultureInfo>>)OnUserLocaleChanged);
			Instance = null;
		}

		private void OnFontSizeChanged(object sender, ValueChangedEventArgs<float> e)
		{
			MapNotification.UpdateFonts(e.get_NewValue() / 100f);
			ShowPreviewSettingIndicator();
		}

		private void OnVerticalPositionChanged(object sender, ValueChangedEventArgs<float> e)
		{
			ShowPreviewSettingIndicator();
		}

		private void ShowPreviewSettingIndicator()
		{
			_lastIndicatorChange = DateTime.UtcNow;
			if (_notificationIndicator == null)
			{
				NotificationIndicator notificationIndicator = new NotificationIndicator(_currentMap, _currentSector);
				((Control)notificationIndicator).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
				_notificationIndicator = notificationIndicator;
			}
		}

		private void OnUserLocaleChanged(object o, ValueEventArgs<CultureInfo> e)
		{
			_mapRepository = new AsyncCache<int, Map>(RequestMap);
			_sectorRepository = new AsyncCache<int, List<Sector>>(RequestSectors);
		}

		private async void OnMapChanged(object o, ValueEventArgs<int> e)
		{
			if (!_toggleMapNotification.get_Value())
			{
				return;
			}
			Map currentMap = await _mapRepository.GetItem(e.get_Value());
			if (currentMap == null || currentMap.get_Id() == _prevMapId)
			{
				return;
			}
			_prevMapId = currentMap.get_Id();
			string header = currentMap.get_RegionName();
			string mapName = currentMap.get_Name();
			if (mapName.Equals(header, StringComparison.InvariantCultureIgnoreCase))
			{
				Sector currentSector = await GetSector(currentMap);
				if (currentSector != null && !string.IsNullOrEmpty(currentSector.Name))
				{
					mapName = currentSector.Name;
				}
			}
			MapNotification.ShowNotification(_includeRegionInMapNotification.get_Value() ? header : null, mapName, null, _showDuration.get_Value() / 100f * 5f, _fadeInDuration.get_Value() / 100f * 3f, _fadeOutDuration.get_Value() / 100f * 3f, _effectDuration.get_Value() / 100f * 3f);
		}

		private async Task<Sector> GetSector(Map currentMap)
		{
			if (currentMap == null)
			{
				return null;
			}
			Coordinates2 playerLocation = GameService.Gw2Mumble.get_RawClient().get_AvatarPosition().ToContinentCoords(CoordsUnit.METERS, currentMap.get_MapRect(), currentMap.get_ContinentRect())
				.SwapYz()
				.ToPlane();
			Sector sector2 = (await _sectorRepository.GetItem(GameService.Gw2Mumble.get_CurrentMap().get_Id()))?.FirstOrDefault((Sector sector) => PolygonUtil.IsPointInsidePolygon(playerLocation.ToPoint(), sector.Bounds));
			if (sector2 == null || _prevSectorId == sector2.Id)
			{
				return null;
			}
			_currentSector = sector2.Name;
			_prevSectorId = sector2.Id;
			return sector2;
		}

		private async Task<List<Sector>> RequestSectors(int mapId)
		{
			Map map = await _mapRepository.GetItem(mapId);
			if (map == null)
			{
				return null;
			}
			List<Sector> geometryZone = new List<Sector>();
			foreach (int floor in map.get_Floors())
			{
				IApiV2ObjectList<ContinentFloorRegionMapSector> sectors = await TaskUtil.RetryAsync(() => ((IAllExpandableClient<ContinentFloorRegionMapSector>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Continents()
					.get_Item(map.get_ContinentId())
					.get_Floors()
					.get_Item(floor)
					.get_Regions()
					.get_Item(map.get_RegionId())
					.get_Maps()
					.get_Item(map.get_Id())
					.get_Sectors()).AllAsync(default(CancellationToken)));
				if (sectors != null && ((IEnumerable<ContinentFloorRegionMapSector>)sectors).Any())
				{
					geometryZone.AddRange(from sector in ((IEnumerable<ContinentFloorRegionMapSector>)sectors).DistinctBy((ContinentFloorRegionMapSector sector) => sector.get_Id())
						select new Sector(sector));
				}
			}
			return geometryZone;
		}

		private async Task<Map> RequestMap(int id)
		{
			return await TaskUtil.RetryAsync(() => ((IBulkExpandableClient<Map, int>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Maps()).GetAsync(id, default(CancellationToken)));
		}
	}
}
