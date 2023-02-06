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
using Ideka.NetCommon;
using SemVer;

namespace Ideka.RacingMeter
{
	[Export(typeof(Module))]
	public class RacingModule : Module
	{
		private static readonly Logger Logger = Logger.GetLogger<RacingModule>();

		private RacingSettings _settings;

		private ConfirmationModal _confirmationModal;

		private MapData _mapData;

		private ScreenMap _screenMap;

		private LocalData _localData;

		private Server _server;

		private readonly DisposableCollection _dc = new DisposableCollection();

		private static RacingModule Instance { get; set; }

		internal static string Name => ((Module)Instance).get_Name();

		internal static SemVer.Version Version => ((Module)Instance).get_Version();

		internal static ContentsManager ContentsManager => ((Module)Instance).ModuleParameters.get_ContentsManager();

		internal static Gw2ApiManager Gw2ApiManager => ((Module)Instance).ModuleParameters.get_Gw2ApiManager();

		internal static string BasePath => ((Module)Instance).ModuleParameters.get_DirectoriesManager().GetFullDirectoryPath("racingmeter");

		internal static string CachePath => Path.Combine(BasePath, "Cache");

		internal static string MapCachePath => Path.Combine(CachePath, "Maps.json");

		internal static string RaceCachePath => Path.Combine(CachePath, "Races.json");

		internal static string RacePath => Path.Combine(BasePath, "RaceData");

		internal static string GhostPath => Path.Combine(BasePath, "GhostData");

		internal static RacingSettings Settings => Instance._settings;

		internal static ConfirmationModal ConfirmationModal => Instance._confirmationModal;

		internal static MapData MapData => Instance._mapData;

		internal static ScreenMap ScreenMap => Instance._screenMap;

		internal static LocalData LocalData => Instance._localData;

		internal static Server Server => Instance._server;

		internal static string GhostRacePath(string raceId)
		{
			return Path.Combine(GhostPath, raceId);
		}

		[ImportingConstructor]
		public RacingModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
			Instance = this;
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
			_confirmationModal = _dc.Add<ConfirmationModal>(new ConfirmationModal(ContentsManager.GetTexture("Tooltip.png")));
			_mapData = _dc.Add(new MapData(MapCachePath));
			DisposableCollection dc = _dc;
			ScreenMap screenMap = new ScreenMap(_mapData);
			((Control)screenMap).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			_screenMap = dc.Add<ScreenMap>(screenMap);
			_localData = _dc.Add(new LocalData());
			_server = _dc.Add(new Server());
			MeasurerRealtime measurer = _dc.Add(new MeasurerRealtime());
			DisposableCollection dc2 = _dc;
			Speedometer speedometer = new Speedometer(measurer);
			((Control)speedometer).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)speedometer).set_ZIndex(50);
			dc2.Add<Speedometer>(speedometer);
			_dc.Add<PanelStack>(new PanelStack(GameService.Overlay.get_BlishHudWindow(), (PanelStack panelStack) => new MainPanel(panelStack, measurer)));
			Server.CheckVersion(Version.ToString()).Done(Logger, null);
		}

		protected override void Unload()
		{
			_dc.Dispose();
			Instance = null;
		}
	}
}
