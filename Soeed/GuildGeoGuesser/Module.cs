using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Settings;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Soeed.GuildGeoGuesser.Feature.Shared.Controls;
using Soeed.GuildGeoGuesser.Feature.Shared.Services;
using Soeed.GuildGeoGuesser.Settings.Controls;
using Soeed.GuildGeoGuesser.Settings.Services;
using Soeed.GuildGeoGuesser.Settings.Views;

namespace Soeed.GuildGeoGuesser
{
	[Export(typeof(Module))]
	public class Module : Module
	{
		public static string MODULE_VERSION = "0.7.1";

		public static string DIRECTORY_PATH = "guildgeoguesser";

		public static string STATIC_HOST_URL = "https://bhm.blishhud.com/Soeed.GuildGeoGuesser";

		private static readonly Logger Logger = Logger.GetLogger<Module>();

		private DynamicConfigService _configService;

		public static SettingService Settings { get; set; } = null;


		public static TextureService? Textures { get; set; } = null;


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

		protected override void Initialize()
		{
			((Module)this).Initialize();
			Service.UserManager = new UserManager();
			Service.Gw2ApiManager.add_SubtokenUpdated((EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>>)Gw2ApiManager_SubtokenUpdated);
		}

		protected override async Task LoadAsync()
		{
			_configService = new DynamicConfigService();
			Service.Config = await _configService.LoadConfig();
			Service.Textures = new TextureService(Service.ContentsManager);
			Service.GeoGuessWindow = new GeoGuessWindow();
			Service.SettingsWindow = new SettingsWindow();
			Service.GeoServerWrapper = new GeoServerWrapper(Service.Config.ServerUrl, this, Service.Config, Service.GeoGuessWindow, Service.SettingsWindow);
			ContextMenuStripItem openMainWindow = new ContextMenuStripItem("Open " + Service.Config.Name);
			((Control)openMainWindow).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				Service.GeoGuessWindow.OpenWindowState();
			});
			ContextMenuStripItem openSettingWindow = new ContextMenuStripItem("Open Settings Window");
			((Control)openSettingWindow).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				((Control)Service.SettingsWindow).Show();
			});
			Service.CornerIcon = new CornerIconService(Service.Config.Name, Service.Textures.DatAsset(156740), new _003C_003Ez__ReadOnlyArray<ContextMenuStripItem>((ContextMenuStripItem[])(object)new ContextMenuStripItem[3]
			{
				openMainWindow,
				new ContextMenuStripItemSeparator(),
				openSettingWindow
			}));
			Service.CornerIcon!.IconLeftClicked += delegate
			{
				Service.GeoGuessWindow.WindowToggle();
			};
			Service.Gw2ApiManager.add_SubtokenUpdated((EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>>)Gw2ApiManager_SubtokenUpdatedLoadAsync);
		}

		protected override void Update(GameTime gameTime)
		{
			GeoGuessWindow geoGuessWindow = Service.GeoGuessWindow;
			if (geoGuessWindow != null)
			{
				((Control)geoGuessWindow).Update(gameTime);
			}
		}

		protected override void Unload()
		{
			try
			{
				Service.Gw2ApiManager.remove_SubtokenUpdated((EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>>)Gw2ApiManager_SubtokenUpdated);
				Service.Gw2ApiManager.remove_SubtokenUpdated((EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>>)Gw2ApiManager_SubtokenUpdatedLoadAsync);
				Service.CornerIcon?.Dispose();
				SettingsWindow settingsWindow = Service.SettingsWindow;
				if (settingsWindow != null)
				{
					((Control)settingsWindow).Dispose();
				}
				GeoGuessWindow geoGuessWindow = Service.GeoGuessWindow;
				if (geoGuessWindow != null)
				{
					((Control)geoGuessWindow).Dispose();
				}
				Service.GeoServerWrapper?.Dispose();
				Service.Textures?.Dispose();
				Service.UserManager?.Dispose();
				_configService?.Dispose();
			}
			catch (Exception e)
			{
				Logger.Warn(e, "geoguesser unload failure");
			}
		}

		private void Gw2ApiManager_SubtokenUpdated(object sender, ValueEventArgs<IEnumerable<TokenPermission>> e)
		{
			Service.UserManager.SubtokenUpdated();
		}

		private void Gw2ApiManager_SubtokenUpdatedLoadAsync(object sender, ValueEventArgs<IEnumerable<TokenPermission>> e)
		{
			Service.UserManager.SubtokenUpdated();
		}
	}
}
