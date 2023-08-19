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
using RBush;

namespace Nekres.Regions_Of_Tyria
{
	[Export(typeof(Module))]
	public class RegionsOfTyriaModule : Module
	{
		internal static readonly Logger Logger = Logger.GetLogger(typeof(RegionsOfTyriaModule));

		internal static RegionsOfTyriaModule ModuleInstance;

		private SettingEntry<float> _showDurationSetting;

		private SettingEntry<float> _fadeInDurationSetting;

		private SettingEntry<float> _fadeOutDurationSetting;

		private SettingEntry<bool> _toggleMapNotificationSetting;

		private SettingEntry<bool> _toggleSectorNotificationSetting;

		private SettingEntry<bool> _includeRegionInMapNotificationSetting;

		private SettingEntry<bool> _includeMapInSectorNotification;

		internal SettingEntry<float> VerticalPositionSetting;

		private DateTime _lastVerticalChange = DateTime.UtcNow;

		private ControlPositionIndicator _verticalIndicator;

		private float _showDuration;

		private float _fadeInDuration;

		private float _fadeOutDuration;

		private AsyncCache<int, Map> _mapRepository;

		private AsyncCache<int, RBush<Sector>> _sectorRepository;

		private static int _prevSectorId;

		private int _prevMapId;

		private double _lastRun;

		private DateTime _lastUpdate;

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		internal Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		public static int CurrentSector
		{
			get
			{
				return _prevSectorId;
			}
			private set
			{
				if (value != _prevSectorId)
				{
					_prevSectorId = value;
					RegionsOfTyriaModule.SectorChanged?.Invoke(ModuleInstance, new ValueEventArgs<int>(value));
				}
			}
		}

		public static event EventHandler<ValueEventArgs<int>> SectorChanged;

		[ImportingConstructor]
		public RegionsOfTyriaModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
			ModuleInstance = this;
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			SettingCollection mapCol = settings.AddSubCollection("map_alert", true, (Func<string>)(() => "Map Notification"));
			_toggleMapNotificationSetting = mapCol.DefineSetting<bool>("enabled", true, (Func<string>)(() => "Enabled"), (Func<string>)(() => "Shows a map's name after entering it."));
			_includeRegionInMapNotificationSetting = mapCol.DefineSetting<bool>("prefix_region", true, (Func<string>)(() => "Include Region"), (Func<string>)(() => "Shows the region's name above the map's name."));
			SettingCollection sectorCol = settings.AddSubCollection("sector_alert", true, (Func<string>)(() => "Sector Notification"));
			_toggleSectorNotificationSetting = sectorCol.DefineSetting<bool>("enabled", true, (Func<string>)(() => "Enabled"), (Func<string>)(() => "Shows a sector's name after entering."));
			_includeMapInSectorNotification = sectorCol.DefineSetting<bool>("prefix_map", true, (Func<string>)(() => "Include Map"), (Func<string>)(() => "Shows the map's name above the sector's name."));
			SettingCollection durationCol = settings.AddSubCollection("durations", true, (Func<string>)(() => "Durations"));
			_showDurationSetting = durationCol.DefineSetting<float>("show", 40f, (Func<string>)(() => "Show Duration"), (Func<string>)(() => "The duration in which to stay in full opacity."));
			_fadeInDurationSetting = durationCol.DefineSetting<float>("fade_in", 20f, (Func<string>)(() => "Fade-In Duration"), (Func<string>)(() => "The duration of the fade-in."));
			_fadeOutDurationSetting = durationCol.DefineSetting<float>("fade_out", 20f, (Func<string>)(() => "Fade-Out Duration"), (Func<string>)(() => "The duration of the fade-out."));
			SettingCollection positionCol = settings.AddSubCollection("position", true, (Func<string>)(() => "Position"));
			VerticalPositionSetting = positionCol.DefineSetting<float>("pos_y", 30f, (Func<string>)(() => "Vertical Position"), (Func<string>)(() => "Sets the vertical position of area notifications."));
		}

		protected override void Initialize()
		{
			_mapRepository = new AsyncCache<int, Map>(RequestMap);
			_sectorRepository = new AsyncCache<int, RBush<Sector>>(RequestSectors);
		}

		protected override async void Update(GameTime gameTime)
		{
			if (gameTime.get_TotalGameTime().TotalMilliseconds - _lastRun < 10.0 || DateTime.UtcNow.Subtract(_lastUpdate).TotalMilliseconds < 1000.0 || !_toggleSectorNotificationSetting.get_Value() || !GameService.Gw2Mumble.get_IsAvailable() || !GameService.GameIntegration.get_Gw2Instance().get_IsInGame())
			{
				return;
			}
			_lastRun = gameTime.get_ElapsedGameTime().TotalMilliseconds;
			_lastUpdate = DateTime.UtcNow;
			if (DateTime.UtcNow.Subtract(_lastVerticalChange).TotalMilliseconds > 250.0)
			{
				ControlPositionIndicator verticalIndicator = _verticalIndicator;
				if (verticalIndicator != null)
				{
					((Control)verticalIndicator).Dispose();
				}
				_verticalIndicator = null;
			}
			Map currentMap = await _mapRepository.GetItem(GameService.Gw2Mumble.get_CurrentMap().get_Id());
			Sector currentSector = await GetSector(currentMap);
			if (currentSector != null)
			{
				MapNotification.ShowNotification(_includeMapInSectorNotification.get_Value() ? currentMap.get_Name() : null, currentSector.Name, null, _showDuration, _fadeInDuration, _fadeOutDuration);
			}
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			GameService.Gw2Mumble.get_CurrentMap().add_MapChanged((EventHandler<ValueEventArgs<int>>)OnMapChanged);
			GameService.Overlay.add_UserLocaleChanged((EventHandler<ValueEventArgs<CultureInfo>>)OnUserLocaleChanged);
			OnShowDurationSettingChanged(_showDurationSetting, new ValueChangedEventArgs<float>(0f, _showDurationSetting.get_Value()));
			OnFadeInDurationSettingChanged(_fadeInDurationSetting, new ValueChangedEventArgs<float>(0f, _fadeInDurationSetting.get_Value()));
			OnFadeOutDurationSettingChanged(_fadeOutDurationSetting, new ValueChangedEventArgs<float>(0f, _fadeOutDurationSetting.get_Value()));
			_showDurationSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)OnShowDurationSettingChanged);
			_fadeInDurationSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)OnFadeInDurationSettingChanged);
			_fadeOutDurationSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)OnFadeOutDurationSettingChanged);
			VerticalPositionSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)OnVerticalPositionSettingChanged);
			((Module)this).OnModuleLoaded(e);
		}

		private void OnShowDurationSettingChanged(object o, ValueChangedEventArgs<float> e)
		{
			_showDuration = MathHelper.Clamp(e.get_NewValue(), 0f, 100f) / 10f;
		}

		private void OnFadeInDurationSettingChanged(object o, ValueChangedEventArgs<float> e)
		{
			_fadeInDuration = MathHelper.Clamp(e.get_NewValue(), 0f, 100f) / 10f;
		}

		private void OnFadeOutDurationSettingChanged(object o, ValueChangedEventArgs<float> e)
		{
			_fadeOutDuration = MathHelper.Clamp(e.get_NewValue(), 0f, 100f) / 10f;
		}

		protected override void Unload()
		{
			ControlPositionIndicator verticalIndicator = _verticalIndicator;
			if (verticalIndicator != null)
			{
				((Control)verticalIndicator).Dispose();
			}
			VerticalPositionSetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)OnVerticalPositionSettingChanged);
			_showDurationSetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)OnShowDurationSettingChanged);
			_fadeInDurationSetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)OnFadeInDurationSettingChanged);
			_fadeOutDurationSetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)OnFadeOutDurationSettingChanged);
			GameService.Gw2Mumble.get_CurrentMap().remove_MapChanged((EventHandler<ValueEventArgs<int>>)OnMapChanged);
			GameService.Overlay.remove_UserLocaleChanged((EventHandler<ValueEventArgs<CultureInfo>>)OnUserLocaleChanged);
			ModuleInstance = null;
		}

		private void OnVerticalPositionSettingChanged(object o, ValueChangedEventArgs<float> e)
		{
			_lastVerticalChange = DateTime.UtcNow;
			if (_verticalIndicator == null)
			{
				ControlPositionIndicator controlPositionIndicator = new ControlPositionIndicator();
				((Control)controlPositionIndicator).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
				_verticalIndicator = controlPositionIndicator;
			}
		}

		private void OnUserLocaleChanged(object o, ValueEventArgs<CultureInfo> e)
		{
			_mapRepository = new AsyncCache<int, Map>(RequestMap);
			_sectorRepository = new AsyncCache<int, RBush<Sector>>(RequestSectors);
		}

		private async void OnMapChanged(object o, ValueEventArgs<int> e)
		{
			if (!_toggleMapNotificationSetting.get_Value())
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
			MapNotification.ShowNotification(_includeRegionInMapNotificationSetting.get_Value() ? header : null, mapName, null, _showDuration, _fadeInDuration, _fadeOutDuration);
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
			RBush<Sector> rtree = await _sectorRepository.GetItem(GameService.Gw2Mumble.get_CurrentMap().get_Id());
			if (rtree == null)
			{
				return null;
			}
			Envelope boundingBox = new Envelope(((Coordinates2)(ref playerLocation)).get_X(), ((Coordinates2)(ref playerLocation)).get_Y(), ((Coordinates2)(ref playerLocation)).get_X(), ((Coordinates2)(ref playerLocation)).get_Y());
			IReadOnlyList<Sector> foundPoints = rtree.Search(in boundingBox);
			if (foundPoints.Count == 0 || _prevSectorId.Equals(foundPoints[0].Id))
			{
				return null;
			}
			CurrentSector = foundPoints[0].Id;
			return foundPoints[0];
		}

		private async Task<RBush<Sector>> RequestSectors(int mapId)
		{
			Map map = await _mapRepository.GetItem(mapId);
			if (map == null)
			{
				return null;
			}
			IEnumerable<Sector> geometryZone = new HashSet<Sector>();
			ProjectionEqualityComparer<Sector, int> comparer = ProjectionEqualityComparer<Sector>.Create((Sector x) => x.Id);
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
					geometryZone = geometryZone.Union(((IEnumerable<ContinentFloorRegionMapSector>)sectors).Select((ContinentFloorRegionMapSector x) => new Sector(x)), comparer);
				}
			}
			RBush<Sector> rtree = new RBush<Sector>();
			foreach (Sector sector in geometryZone)
			{
				rtree.Insert(sector);
			}
			return rtree;
		}

		private async Task<Map> RequestMap(int id)
		{
			return await TaskUtil.RetryAsync(() => ((IBulkExpandableClient<Map, int>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Maps()).GetAsync(id, default(CancellationToken)));
		}
	}
}
