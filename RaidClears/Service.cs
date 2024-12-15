using System;
using Blish_HUD.Modules.Managers;
using RaidClears.Features.Dungeons;
using RaidClears.Features.Fractals;
using RaidClears.Features.Fractals.Services;
using RaidClears.Features.Raids;
using RaidClears.Features.Raids.Services;
using RaidClears.Features.Shared.Services;
using RaidClears.Features.Strikes;
using RaidClears.Features.Strikes.Services;
using RaidClears.Settings.Controls;
using RaidClears.Settings.Services;

namespace RaidClears
{
	public static class Service
	{
		public static Random Random { get; set; } = new Random();


		public static string CurrentAccountName { get; set; } = AccountNameService.DEFAULT_ACCOUNT_NAME;


		public static Module ModuleInstance { get; set; } = null;


		public static SettingService Settings { get; set; } = null;


		public static ContentsManager ContentsManager { get; set; } = null;


		public static Gw2ApiManager Gw2ApiManager { get; set; } = null;


		public static DirectoriesManager DirectoriesManager { get; set; } = null;


		public static TextureService? Textures { get; set; }

		public static ApiPollService? ApiPollingService { get; set; }

		public static SettingsPanel SettingsWindow { get; set; } = null;


		public static StrikeData StrikeData { get; set; } = null;


		public static RaidSettingsPersistance RaidSettings { get; set; } = null;


		public static StrikeSettingsPersistance StrikeSettings { get; set; } = null;


		public static StrikePersistance StrikePersistance { get; set; } = null;


		public static FractalPersistance FractalPersistance { get; set; } = null;


		public static InstabilitiesData InstabilitiesData { get; set; } = null;


		public static FractalMapData FractalMapData { get; set; } = null;


		public static RaidData RaidData { get; set; } = null;


		public static RaidPanel RaidWindow { get; set; } = null;


		public static StrikesPanel StrikesWindow { get; set; } = null;


		public static FractalsPanel FractalWindow { get; set; } = null;


		public static DungeonPanel DungeonWindow { get; set; } = null;


		public static CornerIconService CornerIcon { get; set; } = null;


		public static MapWatcherService MapWatcher { get; set; } = null;


		public static FractalMapWatcherService FractalMapWatcher { get; set; } = null;


		public static ResetsWatcherService ResetWatcher { get; set; } = null;

	}
}
