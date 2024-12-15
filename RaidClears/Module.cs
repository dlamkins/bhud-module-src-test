using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.GameIntegration;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using RaidClears.Features.Dungeons;
using RaidClears.Features.Fractals;
using RaidClears.Features.Fractals.Services;
using RaidClears.Features.Raids;
using RaidClears.Features.Raids.Services;
using RaidClears.Features.Shared.Models;
using RaidClears.Features.Shared.Services;
using RaidClears.Features.Strikes;
using RaidClears.Features.Strikes.Services;
using RaidClears.Localization;
using RaidClears.Settings.Controls;
using RaidClears.Settings.Services;
using RaidClears.Settings.Views;
using RaidClears.Shared.Services;
using SemVer;

namespace RaidClears
{
	[Export(typeof(Module))]
	public class Module : Module
	{
		public static string DIRECTORY_PATH = "clearsTracker";

		public static string STATIC_HOST_URL = "https://bhm.blishhud.com/Soeed.RaidClears/static";

		internal static readonly Logger ModuleLogger = Logger.GetLogger<Module>();

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
			Service.Settings = new SettingService(settings);
		}

		public override IView GetSettingsView()
		{
			return (IView)(object)new ModuleMainSettingsView();
		}

		protected override void Initialize()
		{
			TEMP_FIX_SetTacOAsActive();
		}

		protected override Task LoadAsync()
		{
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Expected O, but got Unknown
			Service.Textures = new TextureService(Service.ContentsManager);
			ModuleMetaDataService.CheckVersions();
			Service.RaidData = RaidData.Load();
			Service.StrikeData = StrikeData.Load();
			Service.RaidSettings = RaidSettingsPersistance.Load();
			Service.StrikeSettings = StrikeSettingsPersistance.Load();
			Service.FractalMapData = FractalMapData.Load();
			Service.InstabilitiesData = InstabilitiesData.Load();
			Service.StrikePersistance = StrikePersistance.Load();
			Service.FractalPersistance = FractalPersistance.Load();
			Service.ApiPollingService = new ApiPollService(Service.Settings.ApiPollingPeriod);
			Service.ResetWatcher = new ResetsWatcherService();
			Service.MapWatcher = new MapWatcherService();
			Service.FractalMapWatcher = new FractalMapWatcherService();
			Service.SettingsWindow = new SettingsPanel();
			Service.RaidWindow = new RaidPanel();
			Service.StrikesWindow = new StrikesPanel();
			Service.FractalWindow = new FractalsPanel();
			Service.DungeonWindow = new DungeonPanel();
			ContextMenuStripItem refreshApiContextMenu = new ContextMenuStripItem(Strings.Settings_RefreshNow);
			((Control)refreshApiContextMenu).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				Service.ApiPollingService?.Invoke();
			});
			Service.CornerIcon = new CornerIconService(Service.Settings.GlobalCornerIconEnabled, Strings.Module_Title, Service.Textures!.CornerIconTexture, Service.Textures!.CornerIconHoverTexture, new List<ContextMenuStripItem>
			{
				(ContextMenuStripItem)(object)new CornerIconToggleMenuItem((Control)(object)Service.SettingsWindow, Strings.ModuleSettings_OpenSettings),
				(ContextMenuStripItem)(object)new ContextMenuStripItemSeparator(),
				(ContextMenuStripItem)(object)new CornerIconToggleMenuItem(Service.Settings.RaidSettings.Generic.Visible, Strings.SettingsPanel_Tab_Raids),
				(ContextMenuStripItem)(object)new CornerIconToggleMenuItem(Service.Settings.StrikeSettings.Generic.Visible, Strings.SettingsPanel_Tab_Strikes),
				(ContextMenuStripItem)(object)new CornerIconToggleMenuItem(Service.Settings.FractalSettings.Generic.Visible, "Fractals"),
				(ContextMenuStripItem)(object)new CornerIconToggleMenuItem(Service.Settings.DungeonSettings.Generic.Visible, Strings.SettingsPanel_Tab_Dunegons),
				(ContextMenuStripItem)(object)new ContextMenuStripItemSeparator(),
				refreshApiContextMenu
			});
			Service.CornerIcon.IconLeftClicked += new EventHandler<bool>(CornerIcon_IconLeftClicked);
			Service.Gw2ApiManager.add_SubtokenUpdated((EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>>)Gw2ApiManager_SubtokenUpdated);
			DispatchClears();
			return Task.CompletedTask;
		}

		private void DispatchClears()
		{
			Task.Run(async delegate
			{
				Service.CurrentAccountName = await AccountNameService.UpdateAccountName();
				Service.MapWatcher.DispatchCurrentStrikeClears();
				Service.FractalMapWatcher.DispatchCurrentClears();
				Service.CornerIcon?.UpdateAccountName(Service.CurrentAccountName);
			});
		}

		private void TEMP_FIX_SetTacOAsActive()
		{
			if (DateTime.UtcNow.Date >= new DateTime(2023, 8, 22, 0, 0, 0, DateTimeKind.Utc) && Program.get_OverlayVersion() < new SemVer.Version(1, 1, 0))
			{
				try
				{
					typeof(TacOIntegration).GetProperty("TacOIsRunning").GetSetMethod(nonPublic: true)?.Invoke(GameService.GameIntegration.get_TacO(), new object[1] { true });
				}
				catch
				{
				}
			}
		}

		protected override void Unload()
		{
			Service.Gw2ApiManager.remove_SubtokenUpdated((EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>>)Gw2ApiManager_SubtokenUpdated);
			Service.CornerIcon.IconLeftClicked -= new EventHandler<bool>(CornerIcon_IconLeftClicked);
			ContentsManager contentsManager = Service.ContentsManager;
			if (contentsManager != null)
			{
				contentsManager.Dispose();
			}
			Service.Textures?.Dispose();
			Service.ApiPollingService?.Dispose();
			FractalsPanel fractalWindow = Service.FractalWindow;
			if (fractalWindow != null)
			{
				((Control)fractalWindow).Dispose();
			}
			StrikesPanel strikesWindow = Service.StrikesWindow;
			if (strikesWindow != null)
			{
				((Control)strikesWindow).Dispose();
			}
			DungeonPanel dungeonWindow = Service.DungeonWindow;
			if (dungeonWindow != null)
			{
				((Control)dungeonWindow).Dispose();
			}
			RaidPanel raidWindow = Service.RaidWindow;
			if (raidWindow != null)
			{
				((Control)raidWindow).Dispose();
			}
			SettingsPanel settingsWindow = Service.SettingsWindow;
			if (settingsWindow != null)
			{
				((Control)settingsWindow).Dispose();
			}
			Service.CornerIcon?.Dispose();
			Service.MapWatcher?.Dispose();
			Service.FractalMapWatcher.Dispose();
			Service.ResetWatcher?.Dispose();
		}

		protected override void Update(GameTime gameTime)
		{
			Service.ApiPollingService?.Update(gameTime);
			Service.RaidWindow?.Update();
			Service.DungeonWindow?.Update();
			Service.StrikesWindow?.Update();
			Service.FractalWindow?.Update();
			Service.ResetWatcher?.Update(gameTime);
		}

		private void CornerIcon_IconLeftClicked(object sender, bool e)
		{
			Service.Settings.RaidSettings.Generic.ToggleVisible();
			Service.Settings.DungeonSettings.Generic.ToggleVisible();
			Service.Settings.StrikeSettings.Generic.ToggleVisible();
			Service.Settings.FractalSettings.Generic.ToggleVisible();
		}

		private void Gw2ApiManager_SubtokenUpdated(object sender, ValueEventArgs<IEnumerable<TokenPermission>> e)
		{
			DispatchClears();
			Service.ApiPollingService?.Invoke();
		}
	}
}
