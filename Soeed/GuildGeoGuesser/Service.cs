using Blish_HUD.Modules.Managers;
using Soeed.GuildGeoGuesser.Feature.Shared.Controls;
using Soeed.GuildGeoGuesser.Feature.Shared.Models;
using Soeed.GuildGeoGuesser.Feature.Shared.Services;
using Soeed.GuildGeoGuesser.Settings.Controls;
using Soeed.GuildGeoGuesser.Settings.Services;

namespace Soeed.GuildGeoGuesser
{
	public static class Service
	{
		public static UserManager UserManager { get; set; }

		public static ConfigModel Config { get; set; }

		public static Module ModuleInstance { get; set; }

		public static SettingService Settings { get; set; }

		public static ContentsManager ContentsManager { get; set; }

		public static Gw2ApiManager Gw2ApiManager { get; set; }

		public static DirectoriesManager DirectoriesManager { get; set; }

		public static CornerIconService? CornerIcon { get; set; }

		public static TextureService Textures { get; set; }

		public static GeoServerWrapper GeoServerWrapper { get; set; }

		public static GeoGuessWindow GeoGuessWindow { get; set; }

		public static SettingsWindow SettingsWindow { get; set; }
	}
}
