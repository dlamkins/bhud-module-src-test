using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Quarry.Interfaces;
using Quarry.Models;
using Quarry.Models.Persistence;
using Quarry.Services;
using Quarry.UserInterface.Windows;
using Quarry.WikiData.Achievement;

namespace Quarry
{
	[Export(typeof(Module))]
	public class Module : Module
	{
		private static readonly Logger Logger = Logger.GetLogger<Module>();

		private readonly DependencyInjectionContainer dependencyInjectionContainer;

		private readonly Logger logger;

		private AchievementOverviewWindow overviewWindow;

		private AchievementTrackWindow window;

		private CornerIcon cornerIcon;

		private bool purposelyHidden;

		private SettingEntry<bool> autoSave;

		private SettingEntry<bool> limitAchievements;

		private SettingEntry<bool> hereToast;

		private SettingEntry<bool> showCornerIcon;

		private SettingEntry<bool> bitAlignmentValidation;

		private SettingEntry<bool> huntMode;

		private SettingEntry<bool> autoUntrackCompleted;

		private SettingEntry<bool> huntRevertOnUnload;

		private SettingEntry<HereGuidanceFilter> hereGuidanceFilter;

		private SettingEntry<int> hereCap;

		private SettingEntry<KeyBinding> toggleTrackWindowKeyBind;

		private bool suppressNextHereToast = true;

		private static readonly TimeSpan HereToastCooldown = TimeSpan.FromMinutes(10.0);

		private readonly Dictionary<int, DateTime> lastHereToastByMapId = new Dictionary<int, DateTime>();

		private CancellationTokenSource cts;

		private CancellationTokenSource saveDebounceCts;

		private Stopwatch loadStopwatch;

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		internal Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		[ImportingConstructor]
		public Module([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
			logger = Logger;
			dependencyInjectionContainer = new DependencyInjectionContainer(Gw2ApiManager, ContentsManager, GameService.Content, DirectoriesManager, logger, GameService.Graphics);
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_018f: Expected O, but got Unknown
			autoSave = settings.DefineSetting<bool>("AutoSave", false, (Func<string>)(() => "Auto save every 5 minutes"), (Func<string>)(() => "Auto save tracked achievements, windows and their positions every 5 minutes"));
			limitAchievements = settings.DefineSetting<bool>("LimitAchievements", true, (Func<string>)(() => "Limit Achievements to 15"), (Func<string>)(() => "This will limit the maximum of achievements to 15. If it's disabled expect performance and usability issues."));
			hereToast = settings.DefineSetting<bool>("HereToast", true, (Func<string>)(() => "Notify on map change"), (Func<string>)(() => "Show a short notification when the current map has nearly-complete achievements"));
			showCornerIcon = settings.DefineSetting<bool>("ShowCornerIcon", true, (Func<string>)(() => "Show corner icon"), (Func<string>)(() => "Show a corner icon to open the Quarry window. Turn off if you only use the keybind or the Target List button."));
			toggleTrackWindowKeyBind = settings.DefineSetting<KeyBinding>("ToggleTrackWindow", new KeyBinding(), (Func<string>)(() => "Toggle Target List"), (Func<string>)(() => "Shows or hides the Target List."));
			toggleTrackWindowKeyBind.get_Value().set_BlockSequenceFromGw2(true);
			toggleTrackWindowKeyBind.get_Value().set_Enabled(true);
			bitAlignmentValidation = settings.DefineSetting<bool>("BitAlignmentValidation", false, (Func<string>)(() => "Validate bit alignment (debug)"), (Func<string>)(() => "One-time startup check: aligns every collection/objective achievement and logs a summary, including a comparison against the old hand-written table. Leave off unless debugging \"what's left\" text."));
			huntMode = settings.DefineSetting<bool>("HuntMode", false, (Func<string>)(() => "Hunt mode"), (Func<string>)(() => "Tracking a guided achievement turns its Pathing routes on; untracking (or completing it) turns off only what we turned on. Requires the Pathing module."));
			autoUntrackCompleted = settings.DefineSetting<bool>("AutoUntrackCompleted", true, (Func<string>)(() => "Untrack achievements on completion"), (Func<string>)(() => "Automatically untrack an achievement once it's Done, with a short \"Done: \" notification."));
			hereGuidanceFilter = settings.DefineSetting<HereGuidanceFilter>("HereGuidanceFilter", HereGuidanceFilter.Everything, (Func<string>)(() => "Here: minimum guidance"), (Func<string>)(() => "Only list achievements this well guided on the current map. TaggedOnly = marker-pack objectives that disappear as you finish them; CoordinatesOrBetter = those plus wiki coordinates; AnyGuidance = anything with a route or an area; Everything = no filter."));
			hereCap = settings.DefineSetting<int>("HereCap", 10, (Func<string>)(() => "Here: how many to list"), (Func<string>)(() => "How many achievements Here lists for the current map. Smaller is the point."));
			SettingComplianceExtensions.SetRange(hereCap, 5, 15);
			huntRevertOnUnload = settings.DefineSetting<bool>("HuntRevertOnUnload", false, (Func<string>)(() => "Revert hunt routes on disable"), (Func<string>)(() => "Turn off every route this module enabled when the module is disabled. Leave off to keep routes visible in Pathing across a restart."));
		}

		protected override void Initialize()
		{
			Gw2ApiManager.add_SubtokenUpdated((EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>>)Gw2ApiManager_SubtokenUpdated);
		}

		private async void Gw2ApiManager_SubtokenUpdated(object sender, EventArgs args)
		{
			try
			{
				logger.Info("Subtoken updated");
				if (dependencyInjectionContainer?.AchievementService != null)
				{
					await dependencyInjectionContainer.AchievementService.LoadPlayerAchievements();
				}
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Exception occured handling SubtokenUpdated");
			}
		}

		protected override async Task LoadAsync()
		{
			cts = new CancellationTokenSource();
			CancellationToken token = cts.Token;
			loadStopwatch = Stopwatch.StartNew();
			try
			{
				await Task.Delay(TimeSpan.FromSeconds(3.0), token);
				logger.Info($"Startup timing: fixed startup delay took {loadStopwatch.ElapsedMilliseconds} ms");
				await dependencyInjectionContainer.InitializeAsync(autoSave, limitAchievements, huntMode, hereGuidanceFilter, hereCap, token);
				dependencyInjectionContainer.AchievementTrackerService.AchievementTracked += AchievementTrackerService_AchievementTracked;
				dependencyInjectionContainer.AchievementTrackerService.AchievementTracked += DebounceSave;
				dependencyInjectionContainer.AchievementTrackerService.AchievementUntracked += DebounceSave;
				dependencyInjectionContainer.AchievementTrackerService.AchievementTracked += PrefetchBitAlignment;
				dependencyInjectionContainer.SessionSummaryService.AchievementCompleted += SessionSummaryService_AchievementCompleted;
				if (bitAlignmentValidation.get_Value())
				{
					dependencyInjectionContainer.BitAlignmentService.RunValidationAsync(token);
				}
				Task.Run(() => PrefetchTrackedBitAlignment(token), token);
				dependencyInjectionContainer.PersistenceService.AutoSave += SavePersistentInformation;
				dependencyInjectionContainer.CurrentMapService.Changed += CurrentMapService_Changed;
				CurrentMapService_Changed();
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Exception occured during module load");
				throw;
			}
			await _003C_003En__0();
		}

		private void InitializeWindow()
		{
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_014e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_0158: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			//IL_0188: Unknown result type (might be due to invalid IL or missing references)
			if (window == null)
			{
				AchievementTrackWindow achievementTrackWindow = new AchievementTrackWindow(ContentsManager, dependencyInjectionContainer.AchievementTrackerService, dependencyInjectionContainer.AchievementService, dependencyInjectionContainer.InspectorWindowManager, dependencyInjectionContainer.HuntService, dependencyInjectionContainer.PersistenceService, dependencyInjectionContainer.SessionSummaryService, dependencyInjectionContainer.NearestObjectiveService, dependencyInjectionContainer.MarkerPackIndexService, dependencyInjectionContainer.CurrentMapService, dependencyInjectionContainer.HereService, dependencyInjectionContainer.HereExclusionService, hereCap, logger, OpenOverview);
				((Control)achievementTrackWindow).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
				window = achievementTrackWindow;
				Storage savedWindowLocation = dependencyInjectionContainer.PersistenceService.Get();
				logger.Debug($"SavedWindowLocation -  X:{savedWindowLocation.TrackWindowLocationX} Y:{savedWindowLocation.TrackWindowLocationY}");
				((Control)window).set_Location((Point)((savedWindowLocation.TrackWindowLocationX == -1 || savedWindowLocation.TrackWindowLocationY == -1) ? (((Control)GameService.Graphics.get_SpriteScreen()).get_Size() / new Point(2) - new Point(256, 178) / new Point(2)) : new Point(savedWindowLocation.TrackWindowLocationX, savedWindowLocation.TrackWindowLocationY)));
				logger.Debug($"AchievementTrackWindowLocation -  X:{((Control)window).get_Location().X} Y:{((Control)window).get_Location().Y}");
			}
		}

		private void OpenOverview()
		{
			((Control)overviewWindow).Show();
			((WindowBase2)overviewWindow).BringWindowToFront();
		}

		private void CreateCornerIcon()
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Expected O, but got Unknown
			if (cornerIcon == null)
			{
				CornerIcon val = new CornerIcon();
				val.set_IconName("Open Quarry");
				val.set_Icon(AsyncTexture2D.op_Implicit(ContentsManager.GetTexture("corner_icon_inactive.png")));
				val.set_HoverIcon(AsyncTexture2D.op_Implicit(ContentsManager.GetTexture("corner_icon_active.png")));
				val.set_Priority(int.MinValue);
				((Control)val).set_Width(64);
				((Control)val).set_Height(64);
				cornerIcon = val;
				((Control)cornerIcon).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					((WindowBase2)overviewWindow).ToggleWindow();
				});
			}
		}

		private void ShowCornerIcon_SettingChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			if (e.get_NewValue())
			{
				CreateCornerIcon();
				return;
			}
			CornerIcon obj = cornerIcon;
			if (obj != null)
			{
				((Control)obj).Dispose();
			}
			cornerIcon = null;
		}

		private void ToggleTrackWindowKeyBind_Activated(object sender, EventArgs e)
		{
			ToggleTrackWindow();
		}

		private void ToggleTrackWindow()
		{
			InitializeWindow();
			((WindowBase2)window).ToggleWindow();
		}

		private void CurrentMapService_Changed()
		{
			bool suppressToast = suppressNextHereToast;
			suppressNextHereToast = false;
			CancellationToken token = cts?.Token ?? new CancellationToken(canceled: true);
			int mapId;
			string mapName;
			string message;
			Task.Run(async delegate
			{
				try
				{
					ICurrentMapService mapService = dependencyInjectionContainer.CurrentMapService;
					mapId = mapService.MapId;
					mapName = mapService.MapName;
					HereResult result = await dependencyInjectionContainer.HereService.GetCandidatesAsync(hereCap.get_Value());
					string candidateSummary = string.Join(", ", result.Candidates.Select((HereCandidate c) => $"{c.Achievement.Name} ({c.Current}/{c.Max}, {c.AchievementPoints}AP)"));
					logger.Debug($"Here candidates [{result.Reason}] for map '{mapName}' (Id={mapId}): {candidateSummary}");
					if (!token.IsCancellationRequested && mapService.MapId == mapId && !suppressToast && hereToast.get_Value() && result.Reason == HereResultReason.Ok && result.Candidates.Count > 0)
					{
						IEnumerable<string> lines = from c in result.Candidates.Take(2)
							select $"{c.Achievement.Name}  {c.Current}/{c.Max}";
						message = string.Format("{0} — {1} nearly done\n{2}", mapName, result.Candidates.Count, string.Join("\n", lines));
						GameService.Overlay.QueueMainThreadUpdate((Action<GameTime>)delegate
						{
							if (!token.IsCancellationRequested)
							{
								DateTime utcNow = DateTime.UtcNow;
								if (lastHereToastByMapId.TryGetValue(mapId, out var value) && utcNow - value < HereToastCooldown)
								{
									logger.Debug($"Here toast: suppressed for map '{mapName}' (Id={mapId}) -- shown {(utcNow - value).TotalMinutes:F1} min ago, cooldown is {HereToastCooldown.TotalMinutes} min.");
								}
								else
								{
									lastHereToastByMapId[mapId] = utcNow;
									ScreenNotification.ShowNotification(message, (NotificationType)0, (Texture2D)null, 4);
								}
							}
						});
					}
				}
				catch (OperationCanceledException)
				{
				}
				catch (Exception ex)
				{
					logger.Warn(ex, "Here: the map-change candidates lookup failed.");
				}
			});
		}

		private void AchievementTrackerService_AchievementTracked(int achievement)
		{
			InitializeWindow();
			if (!((Control)window).get_Visible())
			{
				((Control)window).Show();
			}
		}

		private async Task PrefetchTrackedBitAlignment(CancellationToken cancellationToken)
		{
			_ = 1;
			try
			{
				List<int> tracked = dependencyInjectionContainer.AchievementTrackerService.ActiveAchievements.ToList();
				if (tracked.Count == 0)
				{
					return;
				}
				await dependencyInjectionContainer.BitAlignmentService.GetAchievementsAsync(tracked, cancellationToken);
				IReadOnlyDictionary<int, AchievementTableEntry> achievementsById = dependencyInjectionContainer.AchievementService.AchievementsById;
				int prefetched = 0;
				foreach (int achievementId in tracked)
				{
					if (cancellationToken.IsCancellationRequested)
					{
						return;
					}
					if (achievementsById != null && achievementsById.TryGetValue(achievementId, out var achievement))
					{
						await dependencyInjectionContainer.BitAlignmentService.PrefetchAsync(achievementId, achievement, cancellationToken);
						prefetched++;
					}
				}
				logger.Info($"Bit alignment: prefetched {prefetched} of {tracked.Count} restored tracked achievement(s).");
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex)
			{
				logger.Warn(ex, "Failed to prefetch bit alignment for the restored tracked set; those achievements fall back to identity mapping.");
			}
		}

		private void PrefetchBitAlignment(int achievementId)
		{
			IReadOnlyDictionary<int, AchievementTableEntry> achievementsById = dependencyInjectionContainer.AchievementService.AchievementsById;
			if (achievementsById != null && achievementsById.TryGetValue(achievementId, out var achievement))
			{
				dependencyInjectionContainer.BitAlignmentService.PrefetchAsync(achievementId, achievement, cts?.Token ?? default(CancellationToken));
			}
		}

		private void SessionSummaryService_AchievementCompleted(int achievementId)
		{
			GameService.Overlay.QueueMainThreadUpdate((Action<GameTime>)delegate
			{
				dependencyInjectionContainer.HuntService.RevertForCompletion(achievementId);
				if (autoUntrackCompleted.get_Value() && dependencyInjectionContainer.AchievementTrackerService.IsBeingTracked(achievementId))
				{
					string text = dependencyInjectionContainer.AchievementService.Achievements?.FirstOrDefault((AchievementTableEntry a) => a.Id == achievementId)?.Name ?? $"#{achievementId}";
					dependencyInjectionContainer.AchievementTrackerService.RemoveAchievement(achievementId);
					ScreenNotification.ShowNotification("Done: " + text, (NotificationType)0, (Texture2D)null, 4);
				}
			});
		}

		private void DebounceSave(int achievementId)
		{
			CancellationTokenSource cancellationTokenSource = saveDebounceCts;
			cancellationTokenSource?.Cancel();
			cancellationTokenSource?.Dispose();
			saveDebounceCts = CancellationTokenSource.CreateLinkedTokenSource(cts.Token);
			CancellationToken token = saveDebounceCts.Token;
			Task.Run(async delegate
			{
				try
				{
					await Task.Delay(TimeSpan.FromSeconds(5.0), token);
					SavePersistentInformation();
					logger.Debug("Save-on-change: saved tracked achievements after a track/untrack.");
				}
				catch (OperationCanceledException)
				{
				}
			});
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			if (cts == null || cts.IsCancellationRequested || dependencyInjectionContainer.AchievementItemOverviewFactory == null)
			{
				((Module)this).OnModuleLoaded(e);
				return;
			}
			AchievementOverviewWindow achievementOverviewWindow = new AchievementOverviewWindow(ContentsManager, dependencyInjectionContainer.AchievementItemOverviewFactory, dependencyInjectionContainer.AchievementService, dependencyInjectionContainer.TextureService, dependencyInjectionContainer.HereService, dependencyInjectionContainer.CurrentMapService, dependencyInjectionContainer.AchievementCardFactory, dependencyInjectionContainer.HereExclusionService, dependencyInjectionContainer.AchievementTrackerService, dependencyInjectionContainer.PersistenceService, hereCap, ToggleTrackWindow);
			((Control)achievementOverviewWindow).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)achievementOverviewWindow).set_Location(new Point(100, 100));
			overviewWindow = achievementOverviewWindow;
			if (showCornerIcon.get_Value())
			{
				CreateCornerIcon();
			}
			logger.Info($"Startup timing: module load to corner icon visible took {loadStopwatch?.ElapsedMilliseconds ?? 0} ms");
			showCornerIcon.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)ShowCornerIcon_SettingChanged);
			toggleTrackWindowKeyBind.get_Value().add_Activated((EventHandler<EventArgs>)ToggleTrackWindowKeyBind_Activated);
			if (dependencyInjectionContainer.PersistenceService.Get().ShowTrackWindow)
			{
				InitializeWindow();
				((Control)window).Show();
			}
			((Module)this).OnModuleLoaded(e);
		}

		protected override void Update(GameTime gameTime)
		{
			dependencyInjectionContainer?.InspectorWindowManager?.Update();
			if (!GameService.Gw2Mumble.get_IsAvailable() || window == null)
			{
				return;
			}
			if (!GameService.GameIntegration.get_Gw2Instance().get_IsInGame() || GameService.Gw2Mumble.get_UI().get_IsMapOpen())
			{
				if (((Control)window).get_Visible())
				{
					purposelyHidden = true;
					((Control)window).Hide();
				}
			}
			else if (purposelyHidden)
			{
				((Control)window).Show();
				purposelyHidden = false;
			}
		}

		protected override void Unload()
		{
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			Logger obj = logger;
			string arg = ((window == null) ? "null" : "exists");
			AchievementTrackWindow achievementTrackWindow = window;
			object arg2 = ((achievementTrackWindow != null) ? new Point?(((Control)achievementTrackWindow).get_Location()) : null);
			AchievementTrackWindow achievementTrackWindow2 = window;
			obj.Debug($"Unload starting: window={arg} Location={arg2} Visible={((achievementTrackWindow2 != null) ? new bool?(((Control)achievementTrackWindow2).get_Visible()) : null)}");
			cts?.Cancel();
			Gw2ApiManager.remove_SubtokenUpdated((EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>>)Gw2ApiManager_SubtokenUpdated);
			if (dependencyInjectionContainer.SessionSummaryService != null)
			{
				dependencyInjectionContainer.SessionSummaryService.AchievementCompleted -= SessionSummaryService_AchievementCompleted;
			}
			logger.Info(dependencyInjectionContainer.SessionSummaryService?.GetSummaryLine() ?? "This session: nothing completed");
			SavePersistentInformation();
			showCornerIcon.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)ShowCornerIcon_SettingChanged);
			toggleTrackWindowKeyBind.get_Value().set_Enabled(false);
			toggleTrackWindowKeyBind.get_Value().remove_Activated((EventHandler<EventArgs>)ToggleTrackWindowKeyBind_Activated);
			CornerIcon obj2 = cornerIcon;
			if (obj2 != null)
			{
				((Control)obj2).Dispose();
			}
			AchievementTrackWindow achievementTrackWindow3 = window;
			if (achievementTrackWindow3 != null)
			{
				((Control)achievementTrackWindow3).Dispose();
			}
			AchievementOverviewWindow achievementOverviewWindow = overviewWindow;
			if (achievementOverviewWindow != null)
			{
				((Control)achievementOverviewWindow).Dispose();
			}
			if (huntRevertOnUnload.get_Value())
			{
				dependencyInjectionContainer.HuntService?.RevertAllForUnload();
			}
			dependencyInjectionContainer.HuntService?.Dispose();
			if (dependencyInjectionContainer.CurrentMapService != null)
			{
				dependencyInjectionContainer.CurrentMapService.Changed -= CurrentMapService_Changed;
				dependencyInjectionContainer.CurrentMapService.Dispose();
			}
			dependencyInjectionContainer.InspectorWindowManager?.Dispose();
			dependencyInjectionContainer.AchievementService?.Dispose();
			dependencyInjectionContainer.PersistenceService?.Dispose();
			dependencyInjectionContainer.TextureService?.Dispose();
			saveDebounceCts?.Dispose();
			saveDebounceCts = null;
			cts?.Dispose();
		}

		private void SavePersistentInformation()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			AchievementTrackWindow achievementTrackWindow = window;
			Point location = (Point)((achievementTrackWindow != null) ? ((Control)achievementTrackWindow).get_Location() : new Point(-1, -1));
			Logger obj = logger;
			object[] obj2 = new object[4]
			{
				(window == null) ? "null" : "exists",
				location.X,
				location.Y,
				null
			};
			AchievementTrackWindow achievementTrackWindow2 = window;
			obj2[3] = ((achievementTrackWindow2 != null) ? new bool?(((Control)achievementTrackWindow2).get_Visible()) : null);
			obj.Debug(string.Format("SavePersistentInformation: window={0} Location=X:{1} Y:{2} Visible={3}", obj2));
			IPersistenceService persistenceService = dependencyInjectionContainer.PersistenceService;
			if (persistenceService != null)
			{
				int x = location.X;
				int y = location.Y;
				AchievementTrackWindow achievementTrackWindow3 = window;
				bool showTrackWindow = achievementTrackWindow3 != null && ((Control)achievementTrackWindow3).get_Visible();
				int trackWindowCompactWidth = window?.CompactSize.X ?? (-1);
				int trackWindowCompactHeight = window?.CompactSize.Y ?? (-1);
				AchievementOverviewWindow achievementOverviewWindow = overviewWindow;
				int overviewWindowWidth = ((achievementOverviewWindow != null) ? ((Control)achievementOverviewWindow).get_Size().X : (-1));
				AchievementOverviewWindow achievementOverviewWindow2 = overviewWindow;
				persistenceService.Save(x, y, showTrackWindow, trackWindowCompactWidth, trackWindowCompactHeight, overviewWindowWidth, (achievementOverviewWindow2 != null) ? ((Control)achievementOverviewWindow2).get_Size().Y : (-1));
			}
		}
	}
}
