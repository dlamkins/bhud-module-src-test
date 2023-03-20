using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using RaidClears.Features.Dungeons;
using RaidClears.Features.Raids;
using RaidClears.Features.Shared.Services;
using RaidClears.Features.Strikes;
using RaidClears.Features.Strikes.Services;
using RaidClears.Localization;
using RaidClears.Settings.Controls;
using RaidClears.Settings.Services;
using RaidClears.Settings.Views;

namespace RaidClears
{
	[Export(typeof(Module))]
	public class Module : Module
	{
		public static string DIRECTORY_PATH = "clearsTracker";

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

		protected override Task LoadAsync()
		{
			Service.StrikePersistance = StrikePersistance.Load();
			Service.ApiPollingService = new ApiPollService(Service.Settings.ApiPollingPeriod);
			Service.Textures = new TextureService(Service.ContentsManager);
			Service.ResetWatcher = new ResetsWatcherService();
			Service.MapWatcher = new MapWatcherService();
			Service.SettingsWindow = new SettingsPanel();
			Service.RaidWindow = new RaidPanel();
			Service.StrikesWindow = new StrikesPanel();
			Service.DungeonWindow = new DungeonPanel();
			Service.CornerIcon = new CornerIconService(Service.Settings.GlobalCornerIconEnabled, Strings.Module_Title, Service.Textures!.CornerIconTexture, Service.Textures!.CornerIconHoverTexture, new List<CornerIconToggleMenuItem>
			{
				new CornerIconToggleMenuItem((Control)(object)Service.SettingsWindow, Strings.ModuleSettings_OpenSettings),
				new CornerIconToggleMenuItem(Service.Settings.RaidSettings.Generic.Visible, Strings.SettingsPanel_Tab_Raids),
				new CornerIconToggleMenuItem(Service.Settings.StrikeSettings.Generic.Visible, Strings.SettingsPanel_Tab_Strikes),
				new CornerIconToggleMenuItem(Service.Settings.DungeonSettings.Generic.Visible, Strings.SettingsPanel_Tab_Dunegons)
			});
			Service.CornerIcon.IconLeftClicked += new EventHandler<bool>(CornerIcon_IconLeftClicked);
			Service.Gw2ApiManager.add_SubtokenUpdated((EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>>)Gw2ApiManager_SubtokenUpdated);
			if (Service.Settings.StrikeSettings.AnchorToRaidPanel.get_Value())
			{
				Task.Delay(1500).ContinueWith(delegate
				{
					Service.Settings.AlignStrikesWithRaidPanel();
				});
			}
			return Task.CompletedTask;
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
			Service.ResetWatcher?.Dispose();
		}

		protected override void Update(GameTime gameTime)
		{
			Service.ApiPollingService?.Update(gameTime);
			Service.RaidWindow?.Update();
			Service.DungeonWindow?.Update();
			Service.StrikesWindow?.Update();
			Service.ResetWatcher?.Update(gameTime);
		}

		private void CornerIcon_IconLeftClicked(object sender, bool e)
		{
			Service.Settings.RaidSettings.Generic.ToggleVisible();
			Service.Settings.DungeonSettings.Generic.ToggleVisible();
			Service.Settings.StrikeSettings.Generic.ToggleVisible();
		}

		private void Gw2ApiManager_SubtokenUpdated(object sender, ValueEventArgs<IEnumerable<TokenPermission>> e)
		{
			Task.Run(async delegate
			{
				Service.CurrentAccountName = await AccountNameService.UpdateAccountName();
				Service.MapWatcher.DispatchCurrentStrikeClears();
			});
			Service.ApiPollingService?.Invoke();
		}
	}
}
