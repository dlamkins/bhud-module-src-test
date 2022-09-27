using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Extended.Core.Views;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi.Exceptions;
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
			SettingCollection toggleCol = settings.AddSubCollection("Toggles", false);
			toggleCol.set_RenderInUi(true);
			_toggleMapNotificationSetting = toggleCol.DefineSetting<bool>("EnableMapChangedNotification", true, (Func<string>)(() => "Notify Map Change"), (Func<string>)(() => "Whether a map's name should be shown when entering a map."));
			_includeRegionInMapNotificationSetting = toggleCol.DefineSetting<bool>("IncludeRegionInMapNotification", true, (Func<string>)(() => "Include Region Name in Map Notification"), (Func<string>)(() => "Whether the corresponding region name of a map should be shown above a map's name."));
			_toggleSectorNotificationSetting = toggleCol.DefineSetting<bool>("EnableSectorChangedNotification", true, (Func<string>)(() => "Notify Sector Change"), (Func<string>)(() => "Whether a sector's name should be shown when entering a sector."));
			_includeMapInSectorNotification = toggleCol.DefineSetting<bool>("IncludeMapInSectorNotification", true, (Func<string>)(() => "Include Map Name in Sector Notification"), (Func<string>)(() => "Whether the corresponding map name of a sector should be shown above a sector's name."));
			SettingCollection durationCol = settings.AddSubCollection("Durations", false);
			durationCol.set_RenderInUi(true);
			_showDurationSetting = durationCol.DefineSetting<float>("ShowDuration", 40f, (Func<string>)(() => "Show Duration"), (Func<string>)(() => "The duration in which to stay in full opacity."));
			_fadeInDurationSetting = durationCol.DefineSetting<float>("FadeInDuration", 20f, (Func<string>)(() => "Fade-In Duration"), (Func<string>)(() => "The duration of the fade-in."));
			_fadeOutDurationSetting = durationCol.DefineSetting<float>("FadeOutDuration", 20f, (Func<string>)(() => "Fade-Out Duration"), (Func<string>)(() => "The duration of the fade-out."));
			SettingCollection positionCol = settings.AddSubCollection("Position", false);
			positionCol.set_RenderInUi(true);
			VerticalPositionSetting = positionCol.DefineSetting<float>("pos_y", 30f, (Func<string>)(() => "Vertical Position"), (Func<string>)(() => "Sets the vertical position of area notifications."));
		}

		protected override void Initialize()
		{
			_mapRepository = new AsyncCache<int, Map>(RequestMap);
			_sectorRepository = new AsyncCache<int, RBush<Sector>>(RequestSectors);
		}

		public override IView GetSettingsView()
		{
			return (IView)(object)new SocialsSettingsView(new SocialsSettingsModel(SettingsManager.get_ModuleSettings(), "https://pastebin.com/raw/Kk9DgVmL"));
		}

		protected override async void Update(GameTime gameTime)
		{
			if (!(gameTime.TotalGameTime.TotalMilliseconds - _lastRun < 10.0) && !(DateTime.UtcNow.Subtract(_lastUpdate).TotalMilliseconds < 1000.0) && _toggleSectorNotificationSetting.get_Value() && GameService.Gw2Mumble.get_IsAvailable() && GameService.GameIntegration.get_Gw2Instance().get_IsInGame())
			{
				_lastRun = gameTime.ElapsedGameTime.TotalMilliseconds;
				_lastUpdate = DateTime.UtcNow;
				Map currentMap = await _mapRepository.GetItem(GameService.Gw2Mumble.get_CurrentMap().get_Id());
				Sector currentSector = await GetSector(currentMap);
				if (currentSector != null)
				{
					MapNotification.ShowNotification(_includeMapInSectorNotification.get_Value() ? currentMap.get_Name() : null, currentSector.Name, null, _showDuration, _fadeInDuration, _fadeOutDuration);
				}
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
			_showDurationSetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)OnShowDurationSettingChanged);
			_fadeInDurationSetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)OnFadeInDurationSettingChanged);
			_fadeOutDurationSetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)OnFadeOutDurationSettingChanged);
			GameService.Gw2Mumble.get_CurrentMap().remove_MapChanged((EventHandler<ValueEventArgs<int>>)OnMapChanged);
			GameService.Overlay.remove_UserLocaleChanged((EventHandler<ValueEventArgs<CultureInfo>>)OnUserLocaleChanged);
			ModuleInstance = null;
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
			string text = currentMap.get_Name();
			if (text.Equals(header, StringComparison.InvariantCultureIgnoreCase))
			{
				Sector currentSector = await GetSector(currentMap);
				if (currentSector != null && !string.IsNullOrEmpty(currentSector.Name))
				{
					text = currentSector.Name;
				}
			}
			MapNotification.ShowNotification(_includeRegionInMapNotificationSetting.get_Value() ? header : null, text, null, _showDuration, _fadeInDuration, _fadeOutDuration);
		}

		private async Task<Sector> GetSector(Map currentMap)
		{
			if (currentMap == null)
			{
				return null;
			}
			Coordinates3 playerPos = (GameService.Gw2Mumble.get_RawClient().get_IsCompetitiveMode() ? GameService.Gw2Mumble.get_RawClient().get_CameraPosition() : GameService.Gw2Mumble.get_RawClient().get_AvatarPosition());
			Coordinates2 playerLocation = playerPos.ToContinentCoords(CoordsUnit.Meters, currentMap.get_MapRect(), currentMap.get_ContinentRect()).SwapYZ().ToPlane();
			RBush<Sector> obj = await _sectorRepository.GetItem(GameService.Gw2Mumble.get_CurrentMap().get_Id());
			Envelope boundingBox = new Envelope(((Coordinates2)(ref playerLocation)).get_X(), ((Coordinates2)(ref playerLocation)).get_Y(), ((Coordinates2)(ref playerLocation)).get_X(), ((Coordinates2)(ref playerLocation)).get_Y());
			IReadOnlyList<Sector> foundPoints = obj.Search(in boundingBox);
			if (foundPoints == null || foundPoints.Count == 0 || _prevSectorId.Equals(foundPoints[0].Id))
			{
				return null;
			}
			CurrentSector = foundPoints[0].Id;
			return foundPoints[0];
		}

		private async Task<RBush<Sector>> RequestSectors(int mapId)
		{
			return await (await _mapRepository.GetItem(mapId).ContinueWith(async delegate(Task<Map> result)
			{
				if (result.IsFaulted)
				{
					return null;
				}
				Map map = result.Result;
				IEnumerable<Sector> sectors = new HashSet<Sector>();
				ProjectionEqualityComparer<Sector, int> comparer = ProjectionEqualityComparer<Sector>.Create((Sector x) => x.Id);
				foreach (int floor in map.get_Floors())
				{
					IEnumerable<Sector> first = sectors;
					sectors = first.Union(await RequestSectorsForFloor(map.get_ContinentId(), floor, map.get_RegionId(), map.get_Id()), comparer);
				}
				RBush<Sector> rtree = new RBush<Sector>();
				foreach (Sector sector in sectors)
				{
					rtree.Insert(sector);
				}
				return rtree;
			}));
		}

		private async Task<IEnumerable<Sector>> RequestSectorsForFloor(int continentId, int floor, int regionId, int mapId)
		{
			try
			{
				return await ((IAllExpandableClient<ContinentFloorRegionMapSector>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Continents()
					.get_Item(continentId)
					.get_Floors()
					.get_Item(floor)
					.get_Regions()
					.get_Item(regionId)
					.get_Maps()
					.get_Item(mapId)
					.get_Sectors()).AllAsync(default(CancellationToken)).ContinueWith(delegate(Task<IApiV2ObjectList<ContinentFloorRegionMapSector>> task)
				{
					HashSet<Sector> hashSet = new HashSet<Sector>();
					if (task.IsFaulted)
					{
						return hashSet;
					}
					foreach (ContinentFloorRegionMapSector current in (IEnumerable<ContinentFloorRegionMapSector>)task.Result)
					{
						hashSet.Add(new Sector(current));
					}
					return hashSet;
				});
			}
			catch (BadRequestException val)
			{
				BadRequestException bre = val;
				Logger.Debug("{0} | The map id {1} does not exist on floor {2}.", new object[3]
				{
					((Exception)(object)bre).GetType().FullName,
					mapId,
					floor
				});
				return Enumerable.Empty<Sector>();
			}
			catch (UnexpectedStatusException val2)
			{
				UnexpectedStatusException use = val2;
				Logger.Debug(((Exception)(object)use).Message);
				return Enumerable.Empty<Sector>();
			}
		}

		private async Task<Map> RequestMap(int id)
		{
			try
			{
				return await ((IBulkExpandableClient<Map, int>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Maps()).GetAsync(id, default(CancellationToken)).ContinueWith((Task<Map> task) => (!task.IsFaulted && task.IsCompleted) ? task.Result : null);
			}
			catch (BadRequestException val)
			{
				BadRequestException bre = val;
				Logger.Debug(((Exception)(object)bre).Message);
				return null;
			}
			catch (UnexpectedStatusException val2)
			{
				UnexpectedStatusException use = val2;
				Logger.Debug(((Exception)(object)use).Message);
				return null;
			}
		}
	}
}
