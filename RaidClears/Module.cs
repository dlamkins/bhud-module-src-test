using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

namespace RaidClears
{
	[Export(typeof(Module))]
	public class Module : Module
	{
		public static string DIRECTORY_PATH = "clearsTracker";

		public static string STATIC_HOST_URL = "https://bhm.blishhud.com/Soeed.RaidClears/static/";

		public static string STATIC_HOST_API_VERSION = "v2/";

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
		}

		protected override async Task LoadAsync()
		{
			Service.Textures = new TextureService(Service.ContentsManager);
			ModuleMetaDataService metadata = ModuleMetaDataService.CheckVersions();
			Service.RaidData = RaidData.Load();
			Service.MentorAchievementProgress = new MentorAchievementProgressService(Service.RaidData);
			Service.MentorAchievementProgress.LoadCache();
			Service.StrikeData = StrikeData.Load();
			Service.DailyBountyData = DailyBountyDataService.Load();
			Service.DailyBountyProgress = new DailyBountyProgressService();
			Service.RaidSettings = RaidSettingsPersistance.Load();
			Service.StrikeSettings = StrikeSettingsPersistance.Load();
			Service.FractalMapData = FractalMapData.Load();
			Service.InstabilitiesData = InstabilitiesData.Load();
			Service.StrikePersistance = StrikePersistance.Load();
			Service.FractalPersistance = FractalPersistance.Load();
			Service.FractalSettings = FractalSettingsPersistance.Load();
			Service.ApiPollingService = new ApiPollService(Service.Settings.ApiPollingPeriod);
			Service.ResetWatcher = new ResetsWatcherService();
			Service.WeeklyBountyEncounters = new WeeklyBountyEncountersService();
			Service.ResetWatcher.DailyReset += delegate
			{
				Service.WeeklyBountyEncounters.Rebuild();
			};
			Service.MapWatcher = new MapWatcherService();
			Service.FractalMapWatcher = new FractalMapWatcherService();
			Service.SettingsWindow = new SettingsPanel();
			Service.RaidWindow = new RaidPanel();
			Service.StrikesWindow = new StrikesPanel();
			Service.FractalWindow = new FractalsPanel();
			Service.DungeonWindow = new DungeonPanel();
			try
			{
				ContextMenuStripItem refreshApiContextMenu = new ContextMenuStripItem(Strings.Settings_RefreshNow);
				((Control)refreshApiContextMenu).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					Service.ApiPollingService?.Invoke();
				});
				Service.CornerIcon = new CornerIconService(Service.Settings.GlobalCornerIconEnabled, Strings.Module_Title, Service.Textures!.CornerIconTexture, Service.Textures!.CornerIconHoverTexture, Service.Textures!.CornerIconNotificationTexture, Service.Textures!.CornerIconNotificationHoverTexture, new List<ContextMenuStripItem>
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
				if (Service.CornerIcon != null)
				{
					Service.CornerIcon.IconLeftClicked += new EventHandler<bool>(CornerIcon_IconLeftClicked);
					CheckMotd(metadata);
				}
				Service.Gw2ApiManager.add_SubtokenUpdated((EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>>)Gw2ApiManager_SubtokenUpdated);
				Service.MentorAchievementProgress.ProgressUpdated += new EventHandler<MentorProgressUpdatedEventArgs>(MentorAchievementProgress_ProgressUpdated);
				DispatchClears();
			}
			catch (Exception e2)
			{
				ModuleLogger.Error(e2, "Error loading module");
			}
		}

		private void DispatchClears()
		{
			Task.Run(async delegate
			{
				Service.CurrentAccountName = await AccountNameService.UpdateAccountName();
				Service.MapWatcher.DispatchCurrentStrikeClears();
				Service.FractalMapWatcher.DispatchCurrentClears();
				Service.CornerIcon?.UpdateAccountName(Service.CurrentAccountName);
				if (Service.Settings.RaidSettings.RaidPanelMentorProgress.get_Value())
				{
					await Service.MentorAchievementProgress.RefreshFromApiAsync();
				}
			});
		}

		protected override void Unload()
		{
			Service.MentorAchievementProgress.ProgressUpdated -= new EventHandler<MentorProgressUpdatedEventArgs>(MentorAchievementProgress_ProgressUpdated);
			Service.Gw2ApiManager.remove_SubtokenUpdated((EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>>)Gw2ApiManager_SubtokenUpdated);
			if (Service.CornerIcon != null)
			{
				Service.CornerIcon.IconLeftClicked -= new EventHandler<bool>(CornerIcon_IconLeftClicked);
			}
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

		private void MentorAchievementProgress_ProgressUpdated(object? sender, MentorProgressUpdatedEventArgs e)
		{
			SettingService settings = Service.Settings;
			if (settings == null || !(settings.RaidSettings?.RaidPanelMentorProgress?.get_Value()).GetValueOrDefault())
			{
				return;
			}
			SettingService settings2 = Service.Settings;
			if (settings2 == null || !(settings2.RaidSettings?.RaidPanelMentorProgressPopup?.get_Value()).GetValueOrDefault() || e.Changes.Count == 0)
			{
				return;
			}
			RaidData raidData = Service.RaidData;
			IReadOnlyDictionary<int, MentorAchievementProgressEntry> progress = Service.MentorAchievementProgress?.Progress;
			if (raidData == null || progress == null)
			{
				return;
			}
			List<MentorProgressChange> changes = new List<MentorProgressChange>(e.Changes);
			GameService.Graphics.QueueMainThreadRender((Action<GraphicsDevice>)delegate
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
				int num = ((Control)GameService.Graphics.get_SpriteScreen()).get_Size().X - 300 - 20;
				int num2 = 80;
				foreach (MentorProgressChange current in changes)
				{
					if (current.Delta > 0)
					{
						BossEncounter encounterByMentorAchievementId = raidData.GetEncounterByMentorAchievementId(current.AchievementId);
						MentorAchievementProgressEntry value;
						MentorProgressPopupPanel obj = new MentorProgressPopupPanel(encounterByMentorAchievementId?.Name ?? $"Achievement {current.AchievementId}", max: progress.TryGetValue(current.AchievementId, out value) ? value.Max : current.NewCurrent, iconAssetId: (encounterByMentorAchievementId != null && encounterByMentorAchievementId.AssetId > 0) ? encounterByMentorAchievementId.AssetId : raidData.MentorAssetId, current: current.NewCurrent, delta: current.Delta);
						((Control)obj).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
						((Control)obj).set_Location(new Point(num, num2));
						num2 += 80;
					}
				}
			});
		}

		private void CheckMotd(ModuleMetaDataService metadata)
		{
			try
			{
				if (!string.IsNullOrEmpty(metadata.Motd) && !string.IsNullOrEmpty(metadata.MotdId))
				{
					string lastShownId = Service.Settings.LastShownMotdId.get_Value();
					if (Service.CornerIcon != null)
					{
						Service.CornerIcon.SetCurrentMotdId(metadata.MotdId);
						Service.CornerIcon.SetNotificationState(lastShownId != metadata.MotdId, metadata.Motd);
					}
				}
			}
			catch (Exception ex)
			{
				ModuleLogger.Warn(ex, "Error checking MOTD");
			}
		}
	}
}
