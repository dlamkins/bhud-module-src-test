using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Modules;
using Blish_HUD.Settings;
using Manlaan.CommanderMarkers.CornerIcon;
using Manlaan.CommanderMarkers.Library.Controls;
using Manlaan.CommanderMarkers.Markers;
using Manlaan.CommanderMarkers.Presets;
using Manlaan.CommanderMarkers.Presets.Services;
using Manlaan.CommanderMarkers.Settings.Controls;
using Manlaan.CommanderMarkers.Settings.Enums;
using Manlaan.CommanderMarkers.Settings.Services;
using Manlaan.CommanderMarkers.Settings.Views;
using Microsoft.Xna.Framework;

namespace Manlaan.CommanderMarkers
{
	[Export(typeof(Module))]
	public class Module : Module
	{
		public static string DIRECTORY_PATH = "commanderMarkers";

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
			Service.MapDataCache = new MapData(GetCacheFile().FullName);
			Service.MarkersListing = MarkerListing.Load();
			Service.MapWatch = new MapWatchService(Service.MapDataCache, Settings);
			Service.SettingsWindow = new SettingsPanel();
			ContextMenuStripItem val = new ContextMenuStripItem("Lieutenant's Mode");
			((Control)val).set_BasicTooltipText("Temporarily override the 'Only While Commander' settings");
			val.set_CanCheck(true);
			val.set_Checked(false);
			ContextMenuStripItem LtMode = val;
			LtMode.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object s, CheckChangedEvent e)
			{
				Service.LtMode.set_Value(e.get_Checked());
			});
			Service.CornerIcon = new CornerIconService(Service.Settings.CornerIconEnabled, "Commander Markers", Service.Textures!.IconCorner, Service.Textures!._imgHeart, new List<ContextMenuStripItem>
			{
				(ContextMenuStripItem)(object)new CornerIconToggleMenuItem((Control)(object)Service.SettingsWindow, "Open Settings"),
				(ContextMenuStripItem)(object)new LibrayCornerIconMenuItem(Service.Settings.AutoMarker_FeatureEnabled, "Open Library"),
				(ContextMenuStripItem)(object)new ContextMenuStripItemSeparator(),
				LtMode
			});
			Service.CornerIcon!.IconLeftClicked += new EventHandler<bool>(CornerIcon_IconLeftClicked);
		}

		private void CornerIcon_IconLeftClicked(object sender, bool e)
		{
			switch (Service.Settings.CornerIconLeftClickAction.get_Value())
			{
			case CornerIconActions.SHOW_ICON_MENU:
				Service.CornerIcon?.OpenContextMenu();
				break;
			case CornerIconActions.SHOW_SETTINGS:
				((Control)Service.SettingsWindow).Show();
				break;
			case CornerIconActions.LIEUTENANT:
				LieutentantMode();
				break;
			case CornerIconActions.LIBRARY:
				Service.SettingsWindow.ShowLibrary();
				break;
			case CornerIconActions.CLICKMARKER_TOGGLE:
				Service.Settings._settingShowMarkersPanel.set_Value(!Service.Settings._settingShowMarkersPanel.get_Value());
				break;
			}
		}

		private void LieutentantMode()
		{
			Service.LtMode.set_Value(!Service.LtMode.get_Value());
		}

		protected override void Update(GameTime gameTime)
		{
			IconsPanel?.Update(gameTime);
			Service.MapWatch?.Update(gameTime);
		}

		protected override void Unload()
		{
			if (Service.CornerIcon != null)
			{
				Service.CornerIcon!.IconLeftClicked += new EventHandler<bool>(CornerIcon_IconLeftClicked);
			}
			Service.CornerIcon?.Dispose();
			SettingsPanel settingsWindow = Service.SettingsWindow;
			if (settingsWindow != null)
			{
				((Control)settingsWindow).Dispose();
			}
			Service.MapWatch?.Dispose();
			Service.MapDataCache?.Dispose();
			MarkersPanel iconsPanel = IconsPanel;
			if (iconsPanel != null)
			{
				((Control)iconsPanel).Dispose();
			}
			Service.Settings?.Dispose();
			Service.Textures?.Dispose();
		}

		private FileInfo GetCacheFile()
		{
			return new FileInfo(Service.DirectoriesManager.GetFullDirectoryPath(DIRECTORY_PATH) + "\\" + MapData.MAPDATA_CACHE_FILENAME);
		}
	}
}
