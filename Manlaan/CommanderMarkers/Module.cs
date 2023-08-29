using System.ComponentModel.Composition;
using System.IO;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Modules;
using Blish_HUD.Settings;
using Manlaan.CommanderMarkers.Markers;
using Manlaan.CommanderMarkers.Presets;
using Manlaan.CommanderMarkers.Presets.Service;
using Manlaan.CommanderMarkers.Settings.Services;
using Manlaan.CommanderMarkers.Settings.Views;
using Microsoft.Xna.Framework;

namespace Manlaan.CommanderMarkers
{
	[Export(typeof(Module))]
	public class Module : Module
	{
		public static string DIRECTORY_PATH = "commanderMarkers";

		private double _runningTime;

		private MapData _map;

		private ScreenMap _screenMap;

		private MarkerSequence _sequence;

		private static readonly Logger Logger = Logger.GetLogger<Module>();

		public static string[] _orientation = new string[2] { "Horizontal", "Vertical" };

		public static SettingService Settings { get; set; } = null;


		public static TextureService? Textures { get; set; } = null;


		public static MarkersPanel IconsPanel { get; set; } = null;


		[ImportingConstructor]
		public Module([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
			Service.ModuleInstance = this;
			Service.ContentsManager = moduleParameters.get_ContentsManager();
			Service.Gw2ApiManager = moduleParameters.get_Gw2ApiManager();
			Service.DirectoriesManager = moduleParameters.get_DirectoriesManager();
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			Settings = (Service.Settings = new SettingService(settings));
		}

		public override IView GetSettingsView()
		{
			return (IView)(object)new ModuleSettingsView(Settings);
		}

		protected override async Task LoadAsync()
		{
			Service.Textures = new TextureService(Service.ContentsManager);
			IconsPanel = new MarkersPanel(Settings, Service.Textures);
			_map = new MapData(GetCacheFile().FullName);
			Service.MarkersListing = MarkerListing.Load();
			Service.MapWatch = new MapWatchService(_map, Settings);
		}

		protected override void Update(GameTime gameTime)
		{
			IconsPanel.Update(gameTime);
			Service.MapWatch.Update(gameTime);
		}

		protected override void Unload()
		{
			Service.MapWatch?.Dispose();
			IconsPanel?.Dispose();
			Service.Settings?.Dispose();
			Service.Textures?.Dispose();
			_map?.Dispose();
		}

		private FileInfo GetCacheFile()
		{
			return new FileInfo(Service.DirectoriesManager.GetFullDirectoryPath(DIRECTORY_PATH) + "\\" + MapData.MAPDATA_CACHE_FILENAME);
		}
	}
}
