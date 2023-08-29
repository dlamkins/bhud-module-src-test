using Blish_HUD.Modules.Managers;
using Manlaan.CommanderMarkers.Presets.Service;
using Manlaan.CommanderMarkers.Settings.Services;

namespace Manlaan.CommanderMarkers
{
	public static class Service
	{
		public static Module ModuleInstance { get; set; }

		public static SettingService Settings { get; set; }

		public static ContentsManager ContentsManager { get; set; }

		public static Gw2ApiManager Gw2ApiManager { get; set; }

		public static DirectoriesManager DirectoriesManager { get; set; }

		public static TextureService? Textures { get; set; }

		public static MarkerListing MarkersListing { get; set; }

		public static MapWatchService MapWatch { get; set; }
	}
}
