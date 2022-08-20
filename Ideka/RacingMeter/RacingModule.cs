using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Ideka.BHUDCommon;
using SemVer;

namespace Ideka.RacingMeter
{
	[Export(typeof(Module))]
	public class RacingModule : Module
	{
		private RacingSettings _settings;

		private MapData _mapData;

		private Server _server;

		private MapOverlay _mapOverlay;

		private Measurer _measurer;

		private Racer _racer;

		private MetaPanel _metaPanel;

		private ConfirmationModal _confirmationModal;

		private readonly DisposableCollection _dc;

		private static RacingModule Instance { get; set; }

		internal static string Name => ((Module)Instance).get_Name();

		internal static SemVer.Version Version => ((Module)Instance).get_Version();

		internal static string Namespace => ((Module)Instance).get_Namespace();

		internal static ContentsManager ContentsManager => ((Module)Instance).ModuleParameters.get_ContentsManager();

		internal static Gw2ApiManager Gw2ApiManager => ((Module)Instance).ModuleParameters.get_Gw2ApiManager();

		internal static string BasePath => ((Module)Instance).ModuleParameters.get_DirectoriesManager().GetFullDirectoryPath("racingmeter");

		internal static string CachePath => Path.Combine(BasePath, "Cache");

		internal static string MapCachePath => Path.Combine(CachePath, "Maps.json");

		internal static string RaceCachePath => Path.Combine(CachePath, "Races.json");

		internal static string RacePath => Path.Combine(BasePath, "RaceData");

		internal static string GhostPath => Path.Combine(BasePath, "GhostData");

		internal static RacingSettings Settings => Instance._settings;

		internal static MapData MapData => Instance._mapData;

		internal static Server Server => Instance._server;

		internal static MapOverlay MapOverlay => Instance._mapOverlay;

		internal static Measurer Measurer => Instance._measurer;

		internal static Racer Racer => Instance._racer;

		internal static MetaPanel MetaPanel => Instance._metaPanel;

		internal static ConfirmationModal ConfirmationModal => Instance._confirmationModal;

		internal static string GhostRacePath(string raceId)
		{
			return Path.Combine(GhostPath, raceId);
		}

		[ImportingConstructor]
		public RacingModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
			Instance = this;
			_dc = new DisposableCollection();
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			_settings = _dc.Add(new RacingSettings(settings));
		}

		protected override async Task LoadAsync()
		{
			if (File.Exists(MapCachePath))
			{
				return;
			}
			using Stream file = ContentsManager.GetFileStream("Cache/Maps.json");
			using StreamReader reader = new StreamReader(file);
			Directory.CreateDirectory(Path.GetDirectoryName(MapCachePath));
			string mapCachePath = MapCachePath;
			File.WriteAllText(mapCachePath, await reader.ReadToEndAsync());
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			((Module)this).OnModuleLoaded(e);
			_server = _dc.Add(new Server());
			_mapData = _dc.Add(new MapData(MapCachePath));
			DisposableCollection dc = _dc;
			MapOverlay mapOverlay = new MapOverlay();
			((Control)mapOverlay).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			_mapOverlay = dc.Add<MapOverlay>(mapOverlay);
			_measurer = _dc.Add(new Measurer());
			DisposableCollection dc2 = _dc;
			Speedometer speedometer = new Speedometer();
			((Control)speedometer).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)speedometer).set_ZIndex(50);
			dc2.Add<Speedometer>(speedometer);
			_racer = _dc.Add(new Racer());
			_metaPanel = _dc.Add<MetaPanel>(new MetaPanel(GameService.Overlay.get_BlishHudWindow()));
			_confirmationModal = _dc.Add<ConfirmationModal>(new ConfirmationModal(ContentsManager.GetTexture("Tooltip.png")));
			_racer.EditMode = false;
		}

		protected override void Unload()
		{
			_dc.Dispose();
			Instance = null;
		}
	}
}
