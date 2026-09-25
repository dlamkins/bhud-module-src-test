using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Quarry.Interfaces;
using Quarry.Models;
using Quarry.Models.Persistence;
using Quarry.Services;
using Quarry.WikiData.Achievement;

namespace Quarry.UserInterface.Windows
{
	public class AchievementTrackWindow : WindowBase2
	{
		private const int RowHeight = 40;

		private const int NearestStripHeight = 28;

		private const int SummaryRowHeight = 18;

		private const int CompactMinRows = 3;

		private const int SessionSummaryTooltipLines = 15;

		private const int DefaultWindowWidth = 300;

		private const int DropEyeSize = 14;

		private const int WaypointIconSize = 20;

		private const double NearestRefreshIntervalSeconds = 2.0;

		private const int HereStripCap = 3;

		private const int HereHeaderHeight = 24;

		private const int HereRowHeight = 24;

		private const int HereDividerHeight = 1;

		private const int HereStripPadding = 4;

		private const int HereAddButtonWidth = 22;

		private const int WindowTitleBarHeight = 40;

		private static readonly Point ConstructRect = new Point(350, 600);

		private readonly ContentsManager contentsManager;

		private readonly IAchievementTrackerService achievementTrackerService;

		private readonly IAchievementService achievementService;

		private readonly IInspectorWindowManager inspectorWindowManager;

		private readonly IHuntService huntService;

		private readonly IPersistenceService persistenceService;

		private readonly ISessionSummaryService sessionSummaryService;

		private readonly INearestObjectiveService nearestObjectiveService;

		private readonly IMarkerPackIndexService markerPackIndexService;

		private readonly ICurrentMapService currentMapService;

		private readonly IHereService hereService;

		private readonly IHereExclusionService hereExclusionService;

		private readonly SettingEntry<int> hereCap;

		private readonly Logger logger;

		private readonly Action openOverview;

		private readonly Texture2D texture;

		private readonly Dictionary<int, Panel> trackedAchievements = new Dictionary<int, Panel>();

		private readonly Dictionary<int, (Panel Row, Label NameLabel, Label ProgressLabel, Label NextLabel, ContextMenuStrip Menu, Image DropEye)> rowControlsById = new Dictionary<int, (Panel, Label, Label, Label, ContextMenuStrip, Image)>();

		private readonly List<(Panel Row, Label NameLabel, Label ProgressLabel, Label AddLabel, ContextMenuStrip Menu)> hereStripRows = new List<(Panel, Label, Label, Label, ContextMenuStrip)>();

		private readonly CancellationTokenSource hereStripCts = new CancellationTokenSource();

		private double nearestRefreshAccumulator;

		private int hereStripRequestSequence;

		private int hereStripDividerY;

		private bool hereStripDividerVisible;

		private FlowPanel flowPanel;

		private Label noAchievementsLabel;

		private Label sessionSummaryLabel;

		private Label nearestStripLabel;

		private Image nearestWaypointIcon;

		private Image openQuarryIcon;

		private Label menuGlyph;

		private Label hereStripHeaderLabel;

		private ContextMenuStrip windowMenu;

		private Point windowSize;

		private EventHandler<MouseEventArgs> currentNearestWaypointHandler;

		private volatile bool pendingRebuild;

		private readonly Action sessionSummaryChangedHandler;

		private const int MinWindowWidth = 220;

		private const int MinWindowHeight = 150;

		private bool applyingSizeClamp;

		public Point CompactSize => windowSize;

		public AchievementTrackWindow(ContentsManager contentsManager, IAchievementTrackerService achievementTrackerService, IAchievementService achievementService, IInspectorWindowManager inspectorWindowManager, IHuntService huntService, IPersistenceService persistenceService, ISessionSummaryService sessionSummaryService, INearestObjectiveService nearestObjectiveService, IMarkerPackIndexService markerPackIndexService, ICurrentMapService currentMapService, IHereService hereService, IHereExclusionService hereExclusionService, SettingEntry<int> hereCap, Logger logger, Action openOverview)
			: this()
		{
			this.contentsManager = contentsManager;
			this.achievementTrackerService = achievementTrackerService;
			this.achievementService = achievementService;
			this.inspectorWindowManager = inspectorWindowManager;
			this.huntService = huntService;
			this.persistenceService = persistenceService;
			this.sessionSummaryService = sessionSummaryService;
			this.nearestObjectiveService = nearestObjectiveService;
			this.markerPackIndexService = markerPackIndexService;
			this.currentMapService = currentMapService;
			this.hereService = hereService;
			this.hereExclusionService = hereExclusionService;
			this.hereCap = hereCap;
			this.logger = logger;
			this.openOverview = openOverview;
			texture = this.contentsManager.GetTexture("window_blank.png");
			this.achievementTrackerService.AchievementTracked += AchievementTrackerService_AchievementTracked;
			this.achievementTrackerService.AchievementUntracked += AchievementTrackerService_AchievementUntracked;
			sessionSummaryChangedHandler = delegate
			{
				GameService.Overlay.QueueMainThreadUpdate((Action<GameTime>)delegate
				{
					UpdateSessionSummaryLabel();
					RelayoutChildren();
				});
			};
			this.sessionSummaryService.Changed += sessionSummaryChangedHandler;
			this.achievementService.PlayerAchievementsLoaded += AchievementService_PlayerAchievementsLoaded;
			this.achievementService.ApiAchievementsLoaded += AchievementService_ApiAchievementsLoaded;
			this.currentMapService.Changed += CurrentMapService_Changed;
			this.markerPackIndexService.Changed += MarkerPackIndexService_Changed;
			this.hereExclusionService.Changed += HereExclusionService_Changed;
			this.hereService.CandidatesInvalidated += RefreshHereStrip;
			BuildWindow();
			foreach (int item in this.achievementTrackerService.ActiveAchievements.ToList())
			{
				AchievementTrackerService_AchievementTracked(item);
			}
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			WindowBodyPainter.PaintBody(spriteBatch, (Control)(object)this, ((Container)this).get_ContentRegion(), showLeftAccent: false);
			if (hereStripDividerVisible)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Container)this).get_ContentRegion().X, hereStripDividerY, ((Container)this).get_ContentRegion().Width, 1), UiStyle.CardBorder);
			}
			((WindowBase2)this).PaintBeforeChildren(spriteBatch, bounds);
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			((WindowBase2)this).OnResized(e);
			if (!applyingSizeClamp)
			{
				int clampedWidth = Math.Max(((Control)this).get_Size().X, 220);
				int clampedHeight = Math.Max(((Control)this).get_Size().Y, 150);
				if (clampedWidth != ((Control)this).get_Size().X || clampedHeight != ((Control)this).get_Size().Y)
				{
					applyingSizeClamp = true;
					((Control)this).set_Size(new Point(clampedWidth, clampedHeight));
					applyingSizeClamp = false;
				}
				windowSize = new Point(((Control)this).get_Size().X, ((Control)this).get_Size().Y - 40);
				RelayoutChildren();
			}
		}

		private void CreateRow(int achievementId)
		{
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Expected O, but got Unknown
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Expected O, but got Unknown
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_013c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			//IL_014e: Unknown result type (might be due to invalid IL or missing references)
			//IL_015c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Unknown result type (might be due to invalid IL or missing references)
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			//IL_017a: Expected O, but got Unknown
			//IL_018d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0194: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01de: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0208: Expected O, but got Unknown
			//IL_020f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0214: Unknown result type (might be due to invalid IL or missing references)
			//IL_021b: Unknown result type (might be due to invalid IL or missing references)
			//IL_021d: Unknown result type (might be due to invalid IL or missing references)
			//IL_022e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0236: Unknown result type (might be due to invalid IL or missing references)
			//IL_023a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0244: Unknown result type (might be due to invalid IL or missing references)
			//IL_0251: Expected O, but got Unknown
			AchievementTableEntry achievement = achievementService.Achievements?.FirstOrDefault((AchievementTableEntry x) => x.Id == achievementId);
			if (achievement == null)
			{
				logger.Warn($"AchievementTrackWindow: tracked achievement id {achievementId} has no wiki data; skipping its row.");
				return;
			}
			(int, int, string, string, double) progress = AchievementProgress.Get(achievementService, achievement);
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)flowPanel);
			((Control)val).set_Width(((Container)flowPanel).get_ContentRegion().Width - 16);
			((Control)val).set_Height(40);
			Panel row = val;
			Image val2 = new Image();
			((Control)val2).set_Parent((Container)(object)row);
			((Control)val2).set_Width(14);
			((Control)val2).set_Height(14);
			((Control)val2).set_Location(new Point(((Container)row).get_ContentRegion().Width - 14, 3));
			val2.set_Texture(AsyncTexture2D.op_Implicit(contentsManager.GetTexture("track_enabled.png")));
			((Control)val2).set_BasicTooltipText("Drop this");
			Image dropEye = val2;
			((Control)dropEye).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				achievementTrackerService.RemoveAchievement(achievementId);
			});
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)row);
			val3.set_Text(progress.Item3 ?? string.Empty);
			((Control)val3).set_Width(76);
			((Control)val3).set_Height(16);
			((Control)val3).set_Location(new Point(((Control)dropEye).get_Location().X - 80, 2));
			val3.set_HorizontalAlignment((HorizontalAlignment)2);
			val3.set_Font(UiStyle.BodyFont);
			Label progressLabel = val3;
			UiStyle.ApplyShadow(progressLabel, (progress.Item5 >= 0.75) ? UiStyle.NearDone : UiStyle.TextPrimary);
			int nameWidth = Math.Max(((Control)progressLabel).get_Location().X - 8, 20);
			Label val4 = new Label();
			((Control)val4).set_Parent((Container)(object)row);
			val4.set_Text(StringUtils.TrimNameToWidth(achievement.Name.Trim(), nameWidth));
			((Control)val4).set_Width(nameWidth);
			((Control)val4).set_Height(16);
			((Control)val4).set_Location(new Point(4, 2));
			val4.set_Font(UiStyle.BodyFont);
			Label nameLabel = val4;
			UiStyle.ApplyTextPrimary(nameLabel);
			Label val5 = new Label();
			((Control)val5).set_Parent((Container)(object)row);
			((Control)val5).set_Width(((Container)row).get_ContentRegion().Width - 8);
			((Control)val5).set_Height(16);
			((Control)val5).set_Location(new Point(4, 20));
			val5.set_Font(UiStyle.BodyFont);
			Label nextLabel = val5;
			ContextMenuStrip menu = BuildRowMenu(achievementId);
			((Control)row).set_Menu(menu);
			((Control)nameLabel).add_Click((EventHandler<MouseEventArgs>)OpenInInspector);
			((Control)progressLabel).add_Click((EventHandler<MouseEventArgs>)OpenInInspector);
			((Control)nextLabel).add_Click((EventHandler<MouseEventArgs>)OpenInInspector);
			trackedAchievements.Add(achievementId, row);
			rowControlsById[achievementId] = (row, nameLabel, progressLabel, nextLabel, menu, dropEye);
			RefreshNearestForAchievement(achievementId);
			SortTrackedPanels();
			void OpenInInspector(object s, MouseEventArgs e)
			{
				inspectorWindowManager.ShowAchievement(achievement);
			}
		}

		private ContextMenuStrip BuildRowMenu(int achievementId)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Expected O, but got Unknown
			ContextMenuStrip menu = new ContextMenuStrip();
			((Control)menu.AddMenuItem("Drop")).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				achievementTrackerService.RemoveAchievement(achievementId);
			});
			if (huntService.CanPeek && nearestObjectiveService.HasAnyObjectives(achievementId))
			{
				((Control)menu.AddMenuItem("Show route in Pathing")).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					huntService.Peek(achievementId);
				});
			}
			return menu;
		}

		private void SortTrackedPanels()
		{
			if (trackedAchievements.Count == 0)
			{
				return;
			}
			Dictionary<Panel, (bool NeverStarted, double Fraction, string Name)> infoByPanel = new Dictionary<Panel, (bool, double, string)>();
			foreach (KeyValuePair<int, Panel> entry in trackedAchievements)
			{
				AchievementTableEntry achievement = achievementService.Achievements?.FirstOrDefault((AchievementTableEntry x) => x.Id == entry.Key);
				if (achievement != null)
				{
					(int, int, string, string, double) progress = AchievementProgress.Get(achievementService, achievement);
					infoByPanel[entry.Value] = (progress.Item2 <= 0, progress.Item5, achievement.Name);
				}
			}
			flowPanel.SortChildren<Panel>((Comparison<Panel>)delegate(Panel a, Panel b)
			{
				if (!infoByPanel.TryGetValue(a, out var value) || !infoByPanel.TryGetValue(b, out var value2))
				{
					return 0;
				}
				int num = value.Item1.CompareTo(value2.Item1);
				if (num != 0)
				{
					return num;
				}
				int num2 = value2.Item2.CompareTo(value.Item2);
				return (num2 != 0) ? num2 : string.Compare(value.Item3, value2.Item3, StringComparison.OrdinalIgnoreCase);
			});
		}

		private void AchievementService_PlayerAchievementsLoaded()
		{
			GameService.Overlay.QueueMainThreadUpdate((Action<GameTime>)delegate
			{
				RefreshProgress();
				SortTrackedPanels();
				RefreshNearestObjectives();
			});
		}

		private void AchievementService_ApiAchievementsLoaded()
		{
			GameService.Overlay.QueueMainThreadUpdate((Action<GameTime>)delegate
			{
				foreach (int current in achievementTrackerService.ActiveAchievements.ToList())
				{
					if (!trackedAchievements.ContainsKey(current))
					{
						AchievementTrackerService_AchievementTracked(current);
					}
				}
			});
		}

		private void RefreshProgress()
		{
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			foreach (int achievementId in trackedAchievements.Keys.ToList())
			{
				AchievementTableEntry achievement = achievementService.Achievements?.FirstOrDefault((AchievementTableEntry x) => x.Id == achievementId);
				if (achievement != null && rowControlsById.TryGetValue(achievementId, out var rowControls))
				{
					(int, int, string, string, double) progress = AchievementProgress.Get(achievementService, achievement);
					rowControls.Item3.set_Text(progress.Item3 ?? string.Empty);
					UiStyle.ApplyShadow(rowControls.Item3, (progress.Item5 >= 0.75) ? UiStyle.NearDone : UiStyle.TextPrimary);
				}
			}
		}

		private void CurrentMapService_Changed()
		{
			GameService.Overlay.QueueMainThreadUpdate((Action<GameTime>)delegate
			{
				RefreshNearestObjectives();
				RefreshHereStrip();
			});
		}

		private void HereExclusionService_Changed()
		{
			RefreshHereStrip();
		}

		private void RefreshHereStrip()
		{
			int requestId = Interlocked.Increment(ref hereStripRequestSequence);
			CancellationToken token = hereStripCts.Token;
			Task.Run(async delegate
			{
				HereResult result;
				try
				{
					result = await hereService.GetCandidatesAsync(hereCap.get_Value(), token);
				}
				catch (OperationCanceledException)
				{
					return;
				}
				catch (Exception ex)
				{
					logger.Warn(ex, "Here strip: the candidates lookup failed.");
					result = null;
				}
				if (!token.IsCancellationRequested && requestId == hereStripRequestSequence)
				{
					GameService.Overlay.QueueMainThreadUpdate((Action<GameTime>)delegate
					{
						RenderHereStrip(result);
					});
				}
			}, token);
		}

		private void RenderHereStrip(HereResult result)
		{
			foreach (var hereStripRow in hereStripRows)
			{
				ContextMenuStrip item = hereStripRow.Menu;
				if (item != null)
				{
					((Control)item).Dispose();
				}
				((Control)hereStripRow.Row).Dispose();
			}
			hereStripRows.Clear();
			IReadOnlyList<HereCandidate> shown;
			string headerText = BuildHereStripHeaderText(result, out shown);
			((Control)hereStripHeaderLabel).set_Visible(headerText != null);
			if (headerText != null)
			{
				hereStripHeaderLabel.set_Text(headerText);
				foreach (HereCandidate candidate in shown)
				{
					CreateHereStripRow(candidate);
				}
			}
			RelayoutChildren();
		}

		private string BuildHereStripHeaderText(HereResult result, out IReadOnlyList<HereCandidate> shown)
		{
			shown = Array.Empty<HereCandidate>();
			if (result == null)
			{
				return "HERE  couldn't load — try again shortly";
			}
			if (result.Reason == HereResultReason.NotLoaded)
			{
				return null;
			}
			string mapName = currentMapService.MapName ?? "this map";
			if (result.Reason == HereResultReason.NoCategoryForMap)
			{
				return "HERE  " + mapName + ": no guided achievements yet";
			}
			if (result.Reason == HereResultReason.NoPermission)
			{
				return "HERE  achievement permissions not granted";
			}
			IReadOnlyList<HereCandidate> pool = ((result.RankedUncapped.Count > 0) ? result.RankedUncapped : result.Candidates);
			shown = pool.Where((HereCandidate c) => !achievementTrackerService.IsBeingTracked(c.Achievement.Id)).Take(3).ToList();
			string text = ((shown.Count > 0 || result.FilteredByGuidance == 0) ? $"HERE  {mapName} · {shown.Count}" : $"HERE  {mapName} · {result.FilteredByGuidance} filtered by your guidance filter");
			if (result.HiddenCount > 0)
			{
				text += $" · {result.HiddenCount} hidden";
			}
			if (result.Partial)
			{
				text += " · partial";
			}
			return text;
		}

		private void CreateHereStripRow(HereCandidate candidate)
		{
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Expected O, but got Unknown
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Expected O, but got Unknown
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Expected O, but got Unknown
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_0169: Unknown result type (might be due to invalid IL or missing references)
			//IL_0174: Unknown result type (might be due to invalid IL or missing references)
			//IL_0189: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0195: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dc: Expected O, but got Unknown
			//IL_0208: Unknown result type (might be due to invalid IL or missing references)
			//IL_020f: Expected O, but got Unknown
			AchievementTableEntry achievement = candidate.Achievement;
			int achievementId = achievement.Id;
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Width(((Container)this).get_ContentRegion().Width - 16);
			((Control)val).set_Height(24);
			Panel row = val;
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)row);
			val2.set_Text("+");
			((Control)val2).set_Width(22);
			((Control)val2).set_Height(24);
			((Control)val2).set_Location(new Point(((Container)row).get_ContentRegion().Width - 22, 0));
			val2.set_Font(UiStyle.NumeralFont);
			val2.set_HorizontalAlignment((HorizontalAlignment)1);
			((Control)val2).set_BasicTooltipText("Target this");
			Label addLabel = val2;
			UiStyle.ApplyShadow(addLabel, UiStyle.NearDone);
			((Control)addLabel).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				if (!achievementTrackerService.TrackAchievement(achievementId))
				{
					ScreenNotification.ShowNotification("Target list full", (NotificationType)0, (Texture2D)null, 4);
				}
			});
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)row);
			val3.set_Text($"{candidate.Current}/{candidate.Max}");
			((Control)val3).set_Width(60);
			((Control)val3).set_Height(24);
			((Control)val3).set_Location(new Point(((Control)addLabel).get_Location().X - 64, 0));
			val3.set_HorizontalAlignment((HorizontalAlignment)2);
			val3.set_Font(UiStyle.BodyFont);
			Label progressLabel = val3;
			UiStyle.ApplyShadow(progressLabel, (candidate.Max > 0 && (double)candidate.Current / (double)candidate.Max >= 0.75) ? UiStyle.NearDone : UiStyle.TextPrimary);
			int nameWidth = Math.Max(((Control)progressLabel).get_Location().X - 8, 20);
			Label val4 = new Label();
			((Control)val4).set_Parent((Container)(object)row);
			val4.set_Text(StringUtils.TrimNameToWidth(achievement.Name.Trim(), nameWidth));
			((Control)val4).set_Width(nameWidth);
			((Control)val4).set_Height(24);
			((Control)val4).set_Location(new Point(4, 0));
			val4.set_Font(UiStyle.BodyFont);
			Label nameLabel = val4;
			UiStyle.ApplyTextPrimary(nameLabel);
			((Control)nameLabel).add_Click((EventHandler<MouseEventArgs>)OpenInInspector);
			((Control)progressLabel).add_Click((EventHandler<MouseEventArgs>)OpenInInspector);
			ContextMenuStrip menu = new ContextMenuStrip();
			ContextMenuStripItem obj = menu.AddMenuItem("Not today");
			((Control)obj).set_BasicTooltipText("Hide from Here until the daily reset (00:00 UTC).");
			((Control)obj).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				hereExclusionService.Snooze(achievementId);
			});
			ContextMenuStripItem obj2 = menu.AddMenuItem("Not interested");
			((Control)obj2).set_BasicTooltipText("Hide from Here until you un-hide it.");
			((Control)obj2).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				hereExclusionService.Hide(achievementId);
			});
			((Control)row).set_Menu(menu);
			hereStripRows.Add((row, nameLabel, progressLabel, addLabel, menu));
			void OpenInInspector(object s, MouseEventArgs e)
			{
				inspectorWindowManager.ShowAchievement(achievement);
			}
		}

		private int HereStripContentHeight()
		{
			if (hereStripHeaderLabel == null || !((Control)hereStripHeaderLabel).get_Visible())
			{
				return 0;
			}
			return 29 + hereStripRows.Count * 24;
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			((WindowBase2)this).UpdateContainer(gameTime);
			DrainPendingRebuild();
			if (((Control)this).get_Visible())
			{
				nearestRefreshAccumulator += gameTime.get_ElapsedGameTime().TotalSeconds;
				if (!(nearestRefreshAccumulator < 2.0))
				{
					nearestRefreshAccumulator = 0.0;
					RefreshNearestObjectives();
				}
			}
		}

		private void RefreshNearestObjectives()
		{
			RemainingObjective bestObjective = null;
			int bestAchievementId = 0;
			foreach (int achievementId in trackedAchievements.Keys.ToList())
			{
				RemainingObjective top = RefreshNearestForAchievement(achievementId);
				if (top != null && (bestObjective == null || top.DistanceMetres < bestObjective.DistanceMetres))
				{
					bestObjective = top;
					bestAchievementId = achievementId;
				}
			}
			UpdateNearestStrip(bestObjective, bestAchievementId);
		}

		private RemainingObjective RefreshNearestForAchievement(int achievementId)
		{
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			if (!rowControlsById.TryGetValue(achievementId, out var rowControls))
			{
				return null;
			}
			if (achievementService.PlayerAchievements == null)
			{
				rowControls.Item4.set_Text("Loading achievement data…");
				((Control)rowControls.Item4).set_BasicTooltipText((string)null);
				return null;
			}
			int mapId = currentMapService.MapId;
			Vector3 player = GameService.Gw2Mumble.get_PlayerCharacter().get_Position();
			IReadOnlyList<RemainingObjective> nearest = nearestObjectiveService.GetRemaining(achievementId, mapId, player);
			RemainingObjective top = ((nearest.Count > 0) ? nearest[0] : null);
			if (top == null)
			{
				GuidanceInfo guidance = nearestObjectiveService.GetGuidance(achievementId, mapId);
				rowControls.Item4.set_Text((guidance.Tier == GuidanceTier.Area) ? "somewhere on this map" : "no route on this map");
				UiStyle.ApplyShadow(rowControls.Item4, UiStyle.Rank);
				((Control)rowControls.Item4).set_BasicTooltipText((string)null);
			}
			else
			{
				string suffix = (top.GroundDistanceOnly ? $" · ~{top.DistanceMetres:F0} m" : $" · {top.DistanceMetres:F0} m");
				int nameWidth = Math.Max(((Control)rowControls.Item4).get_Width() - (int)UiStyle.BodyFont.MeasureString(suffix).Width - 10, 20);
				rowControls.Item4.set_Text(StringUtils.TrimNameToWidth(top.Name, nameWidth) + suffix);
				UiStyle.ApplyTextPrimary(rowControls.Item4);
				((Control)rowControls.Item4).set_BasicTooltipText(AchievementProgress.FormatNearestText(nearest));
			}
			return top;
		}

		private void UpdateNearestStrip(RemainingObjective objective, int achievementId)
		{
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			if (objective == null)
			{
				((Control)nearestStripLabel).set_Visible(false);
				((Control)nearestWaypointIcon).set_Visible(false);
				return;
			}
			((Control)nearestStripLabel).set_Visible(true);
			string suffix = (objective.GroundDistanceOnly ? $" · ~{objective.DistanceMetres:F0} m" : $" · {objective.DistanceMetres:F0} m");
			nearestStripLabel.set_Text("NEAREST  " + objective.Name + suffix);
			int mapId = currentMapService.MapId;
			Vector3 player = GameService.Gw2Mumble.get_PlayerCharacter().get_Position();
			WaypointSuggestion suggestion = nearestObjectiveService.NearestWaypoint(achievementId, mapId, player);
			((Control)nearestWaypointIcon).set_Visible(suggestion != null);
			((Control)nearestWaypointIcon).set_BasicTooltipText(suggestion?.Describe() ?? "No waypoint known for this map yet");
			((Control)nearestWaypointIcon).remove_Click(currentNearestWaypointHandler);
			currentNearestWaypointHandler = null;
			if (suggestion != null)
			{
				currentNearestWaypointHandler = delegate
				{
					ClipboardUtil.get_WindowsClipboardService().SetTextAsync(suggestion.Code);
					ScreenNotification.ShowNotification((suggestion.Name == null) ? "Waypoint copied" : ("Copied " + suggestion.Name), (NotificationType)0, (Texture2D)null, 4);
				};
				((Control)nearestWaypointIcon).add_Click(currentNearestWaypointHandler);
			}
		}

		private void AchievementTrackerService_AchievementUntracked(int achievement)
		{
			if (trackedAchievements.TryGetValue(achievement, out var panel))
			{
				trackedAchievements.Remove(achievement);
				((Control)panel).Dispose();
			}
			if (rowControlsById.TryGetValue(achievement, out var rowControls))
			{
				ContextMenuStrip item = rowControls.Item5;
				if (item != null)
				{
					((Control)item).Dispose();
				}
			}
			rowControlsById.Remove(achievement);
			if (trackedAchievements.Count == 0)
			{
				((Control)noAchievementsLabel).set_Visible(true);
			}
			RelayoutChildren();
			SortTrackedPanels();
			RefreshNearestObjectives();
			RefreshHereStrip();
		}

		private void AchievementTrackerService_AchievementTracked(int achievementId)
		{
			if (((Control)noAchievementsLabel).get_Visible())
			{
				((Control)noAchievementsLabel).set_Visible(false);
			}
			if (achievementService.Achievements?.FirstOrDefault((AchievementTableEntry x) => x.Id == achievementId) == null)
			{
				logger.Warn($"AchievementTrackWindow: tracked achievement id {achievementId} has no wiki data; skipping its row.");
			}
			else if (!trackedAchievements.ContainsKey(achievementId))
			{
				CreateRow(achievementId);
				RelayoutChildren();
				RefreshHereStrip();
			}
		}

		private void BuildWindow()
		{
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Expected O, but got Unknown
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0142: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_016c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0187: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_019e: Expected O, but got Unknown
			//IL_019f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_020b: Expected O, but got Unknown
			//IL_0223: Unknown result type (might be due to invalid IL or missing references)
			//IL_0228: Unknown result type (might be due to invalid IL or missing references)
			//IL_022f: Unknown result type (might be due to invalid IL or missing references)
			//IL_023a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0245: Unknown result type (might be due to invalid IL or missing references)
			//IL_024d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0255: Unknown result type (might be due to invalid IL or missing references)
			//IL_0257: Unknown result type (might be due to invalid IL or missing references)
			//IL_0265: Unknown result type (might be due to invalid IL or missing references)
			//IL_026f: Unknown result type (might be due to invalid IL or missing references)
			//IL_027b: Expected O, but got Unknown
			//IL_0281: Unknown result type (might be due to invalid IL or missing references)
			//IL_028c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0296: Expected O, but got Unknown
			//IL_0311: Unknown result type (might be due to invalid IL or missing references)
			//IL_0316: Unknown result type (might be due to invalid IL or missing references)
			//IL_031e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0320: Unknown result type (might be due to invalid IL or missing references)
			//IL_032f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0332: Unknown result type (might be due to invalid IL or missing references)
			//IL_033f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0349: Unknown result type (might be due to invalid IL or missing references)
			//IL_0350: Unknown result type (might be due to invalid IL or missing references)
			//IL_0357: Unknown result type (might be due to invalid IL or missing references)
			//IL_0362: Unknown result type (might be due to invalid IL or missing references)
			//IL_0369: Unknown result type (might be due to invalid IL or missing references)
			//IL_0379: Expected O, but got Unknown
			//IL_037f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0390: Unknown result type (might be due to invalid IL or missing references)
			//IL_0395: Unknown result type (might be due to invalid IL or missing references)
			//IL_039c: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_03af: Unknown result type (might be due to invalid IL or missing references)
			//IL_03bb: Expected O, but got Unknown
			//IL_03c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03df: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0409: Unknown result type (might be due to invalid IL or missing references)
			//IL_040b: Unknown result type (might be due to invalid IL or missing references)
			//IL_041a: Unknown result type (might be due to invalid IL or missing references)
			//IL_041c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0436: Expected O, but got Unknown
			//IL_0437: Unknown result type (might be due to invalid IL or missing references)
			//IL_043c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0443: Unknown result type (might be due to invalid IL or missing references)
			//IL_044e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0455: Unknown result type (might be due to invalid IL or missing references)
			//IL_045c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0466: Unknown result type (might be due to invalid IL or missing references)
			//IL_0477: Unknown result type (might be due to invalid IL or missing references)
			//IL_0488: Unknown result type (might be due to invalid IL or missing references)
			//IL_048f: Unknown result type (might be due to invalid IL or missing references)
			//IL_049a: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ad: Expected O, but got Unknown
			//IL_04b3: Unknown result type (might be due to invalid IL or missing references)
			Storage storage = persistenceService.Get();
			int height = ((storage.TrackWindowCompactHeight > 0) ? storage.TrackWindowCompactHeight : ComputeDefaultContentHeight());
			windowSize = new Point((storage.TrackWindowCompactWidth > 0) ? storage.TrackWindowCompactWidth : 300, height);
			((WindowBase2)this).set_Title("Target List");
			((WindowBase2)this).set_Emblem(contentsManager.GetTexture("track_enabled.png"));
			((WindowBase2)this).ConstructWindow(texture, new Rectangle(0, 0, ConstructRect.X, ConstructRect.Y), new Rectangle(0, 30, ConstructRect.X, ConstructRect.Y - 30));
			((WindowBase2)this).set_CanResize(true);
			((Control)this).set_Size(new Point(windowSize.X, windowSize.Y + 40));
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Location(new Point(4, 0));
			((Control)val).set_Width(((Container)this).get_ContentRegion().Width - 96);
			((Control)val).set_Height(28);
			val.set_Font(UiStyle.SectionFont);
			((Control)val).set_Visible(false);
			nearestStripLabel = val;
			UiStyle.ApplyShadow(nearestStripLabel, UiStyle.Accent);
			Image val2 = new Image();
			((Control)val2).set_Parent((Container)(object)this);
			((Control)val2).set_Width(20);
			((Control)val2).set_Height(20);
			((Control)val2).set_Location(new Point(((Container)this).get_ContentRegion().Width - 72, 4));
			val2.set_Texture(AsyncTexture2D.op_Implicit(contentsManager.GetTexture("link.png")));
			((Control)val2).set_Visible(false);
			((Control)val2).set_BasicTooltipText("Copy the waypoint nearest the closest objective");
			nearestWaypointIcon = val2;
			Image val3 = new Image();
			((Control)val3).set_Parent((Container)(object)this);
			((Control)val3).set_Width(20);
			((Control)val3).set_Height(20);
			((Control)val3).set_Location(new Point(((Container)this).get_ContentRegion().Width - 46, 4));
			val3.set_Texture(AsyncTexture2D.op_Implicit(contentsManager.GetTexture("achievement_icon.png")));
			val3.set_Tint(UiStyle.NearDone);
			((Control)val3).set_BasicTooltipText("Open Quarry");
			openQuarryIcon = val3;
			((Control)openQuarryIcon).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				openOverview();
			});
			Label val4 = new Label();
			((Control)val4).set_Parent((Container)(object)this);
			val4.set_Text("⋯");
			val4.set_Font(UiStyle.SectionFont);
			((Control)val4).set_Width(20);
			((Control)val4).set_Height(28);
			((Control)val4).set_Location(new Point(((Container)this).get_ContentRegion().Width - 20, 0));
			val4.set_HorizontalAlignment((HorizontalAlignment)1);
			menuGlyph = val4;
			UiStyle.ApplyShadow(menuGlyph, UiStyle.TextSecondary);
			windowMenu = new ContextMenuStrip();
			((Control)windowMenu.AddMenuItem("Open Quarry")).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				openOverview();
			});
			((Control)windowMenu.AddMenuItem("Reload from file")).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				persistenceService.Reload();
			});
			((Control)windowMenu.AddMenuItem("Save now")).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				PersistWindowState();
				ScreenNotification.ShowNotification("Tracked achievements saved", (NotificationType)0, (Texture2D)null, 4);
			});
			((Control)menuGlyph).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				windowMenu.Show(GameService.Input.get_Mouse().get_Position());
			});
			Label val5 = new Label();
			((Control)val5).set_Height(18);
			((Control)val5).set_Width(((Container)this).get_ContentRegion().Width);
			((Control)val5).set_Location(new Point(0, ((Container)this).get_ContentRegion().Height - 18));
			((Control)val5).set_Parent((Container)(object)this);
			val5.set_HorizontalAlignment((HorizontalAlignment)1);
			val5.set_Font(UiStyle.BodyFont);
			((Control)val5).set_Visible(false);
			((Control)val5).set_BasicTooltipText("Achievement progress can lag the API by a few minutes, so this may lag behind what you just did.");
			sessionSummaryLabel = val5;
			UiStyle.ApplyShadow(sessionSummaryLabel, UiStyle.TextMuted);
			UpdateSessionSummaryLabel();
			Label val6 = new Label();
			((Control)val6).set_Parent((Container)(object)this);
			val6.set_Font(UiStyle.SectionFont);
			((Control)val6).set_Height(24);
			((Control)val6).set_Visible(false);
			hereStripHeaderLabel = val6;
			UiStyle.ApplyShadow(hereStripHeaderLabel, UiStyle.Accent);
			FlowPanel val7 = new FlowPanel();
			((Control)val7).set_Parent((Container)(object)this);
			((Panel)val7).set_CanScroll(true);
			val7.set_FlowDirection((ControlFlowDirection)3);
			val7.set_ControlPadding(new Vector2(0f, 4f));
			((Control)val7).set_Location(new Point(0, 28));
			((Control)val7).set_Width(((Container)this).get_ContentRegion().Width);
			((Control)val7).set_Height(((Container)this).get_ContentRegion().Height - 28 - 18);
			flowPanel = val7;
			Label val8 = new Label();
			((Control)val8).set_Parent((Container)(object)this);
			val8.set_Text("No targets yet — open Quarry\nand target something.");
			((Control)val8).set_Visible(true);
			((Control)val8).set_Location(((Control)flowPanel).get_Location());
			((Control)val8).set_Width(((Control)flowPanel).get_Width());
			((Control)val8).set_Height(((Control)flowPanel).get_Height());
			val8.set_WrapText(false);
			val8.set_Font(UiStyle.BodyFont);
			val8.set_HorizontalAlignment((HorizontalAlignment)1);
			val8.set_VerticalAlignment((VerticalAlignment)1);
			noAchievementsLabel = val8;
			UiStyle.ApplyShadow(noAchievementsLabel, UiStyle.TextMuted);
			RefreshHereStrip();
		}

		private int ComputeDefaultContentHeight()
		{
			int rowsHeight = Math.Max(achievementTrackerService.ActiveAchievements.Count, 3) * 44;
			int hereStripHeight = HereStripContentHeight();
			int summaryHeight = ((sessionSummaryService.GetSummaryLine() != null) ? 18 : 0);
			return 28 + rowsHeight + hereStripHeight + summaryHeight;
		}

		private void RelayoutChildren()
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_016f: Unknown result type (might be due to invalid IL or missing references)
			//IL_017d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0195: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0208: Unknown result type (might be due to invalid IL or missing references)
			//IL_0219: Unknown result type (might be due to invalid IL or missing references)
			//IL_0249: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_02da: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0300: Unknown result type (might be due to invalid IL or missing references)
			//IL_0311: Unknown result type (might be due to invalid IL or missing references)
			//IL_0342: Unknown result type (might be due to invalid IL or missing references)
			if (flowPanel == null)
			{
				return;
			}
			((Control)nearestStripLabel).set_Width(((Container)this).get_ContentRegion().Width - 96);
			((Control)nearestWaypointIcon).set_Location(new Point(((Container)this).get_ContentRegion().Width - 72, 4));
			((Control)openQuarryIcon).set_Location(new Point(((Container)this).get_ContentRegion().Width - 46, 4));
			((Control)menuGlyph).set_Location(new Point(((Container)this).get_ContentRegion().Width - 20, 0));
			int bottomY = ((Container)this).get_ContentRegion().Height;
			((Control)sessionSummaryLabel).set_Width(((Container)this).get_ContentRegion().Width);
			if (((Control)sessionSummaryLabel).get_Visible())
			{
				bottomY -= 18;
			}
			((Control)sessionSummaryLabel).set_Location(new Point(0, bottomY));
			int hereStripHeight = HereStripContentHeight();
			if (hereStripHeight > 0)
			{
				bottomY = (hereStripDividerY = bottomY - hereStripHeight);
				hereStripDividerVisible = true;
				int y = bottomY + 1 + 4;
				((Control)hereStripHeaderLabel).set_Location(new Point(4, y));
				((Control)hereStripHeaderLabel).set_Width(((Container)this).get_ContentRegion().Width - 8);
				y += 24;
				foreach (var controls2 in hereStripRows)
				{
					((Control)controls2.Row).set_Location(new Point(8, y));
					((Control)controls2.Row).set_Width(((Container)this).get_ContentRegion().Width - 16);
					((Control)controls2.AddLabel).set_Location(new Point(((Container)controls2.Row).get_ContentRegion().Width - 22, 0));
					((Control)controls2.ProgressLabel).set_Location(new Point(((Control)controls2.AddLabel).get_Location().X - 64, 0));
					int rowNameWidth = Math.Max(((Control)controls2.ProgressLabel).get_Location().X - 8, 20);
					((Control)controls2.NameLabel).set_Width(rowNameWidth);
					y += 24;
				}
			}
			else
			{
				hereStripDividerVisible = false;
			}
			((Control)flowPanel).set_Location(new Point(0, 28));
			((Control)flowPanel).set_Width(((Container)this).get_ContentRegion().Width);
			((Control)flowPanel).set_Height(Math.Max(bottomY - 28, 0));
			((Control)noAchievementsLabel).set_Location(((Control)flowPanel).get_Location());
			((Control)noAchievementsLabel).set_Width(((Control)flowPanel).get_Width());
			((Control)noAchievementsLabel).set_Height(((Control)flowPanel).get_Height());
			foreach (var controls in rowControlsById.Values)
			{
				((Control)controls.Row).set_Width(((Container)flowPanel).get_ContentRegion().Width - 16);
				((Control)controls.DropEye).set_Location(new Point(((Container)controls.Row).get_ContentRegion().Width - 14, 3));
				((Control)controls.ProgressLabel).set_Location(new Point(((Control)controls.DropEye).get_Location().X - 80, 2));
				int nameWidth = Math.Max(((Control)controls.ProgressLabel).get_Location().X - 8, 20);
				((Control)controls.NameLabel).set_Width(nameWidth);
				((Control)controls.NextLabel).set_Width(((Container)controls.Row).get_ContentRegion().Width - 8);
			}
		}

		private List<int> TearDownAllEntries()
		{
			List<int> trackedIds = trackedAchievements.Keys.ToList();
			foreach (var value in rowControlsById.Values)
			{
				ContextMenuStrip item = value.Menu;
				if (item != null)
				{
					((Control)item).Dispose();
				}
			}
			foreach (Panel value2 in trackedAchievements.Values)
			{
				((Control)value2).Dispose();
			}
			trackedAchievements.Clear();
			rowControlsById.Clear();
			return trackedIds;
		}

		private void RecreateEntries(List<int> trackedIds)
		{
			foreach (int id in trackedIds)
			{
				AchievementTrackerService_AchievementTracked(id);
			}
			((Control)noAchievementsLabel).set_Visible(trackedAchievements.Count == 0);
		}

		private void MarkerPackIndexService_Changed()
		{
			pendingRebuild = true;
		}

		private void DrainPendingRebuild()
		{
			if (pendingRebuild && flowPanel != null)
			{
				pendingRebuild = false;
				RecreateEntries(TearDownAllEntries());
			}
		}

		private void PersistWindowState()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			persistenceService.Save(((Control)this).get_Location().X, ((Control)this).get_Location().Y, ((Control)this).get_Visible(), windowSize.X, windowSize.Y);
		}

		private void UpdateSessionSummaryLabel()
		{
			if (sessionSummaryLabel == null)
			{
				return;
			}
			string line = sessionSummaryService.GetSummaryLine();
			sessionSummaryLabel.set_Text(line);
			((Control)sessionSummaryLabel).set_Visible(line != null);
			IReadOnlyList<string> names = sessionSummaryService.Summary?.CompletedAchievementNames;
			if (names == null || names.Count == 0)
			{
				((Control)sessionSummaryLabel).set_BasicTooltipText((string)null);
				return;
			}
			List<string> shown = names.Take(15).ToList();
			string tooltip = string.Join("\n", shown);
			if (names.Count > shown.Count)
			{
				tooltip += $"\n... and {names.Count - shown.Count} more";
			}
			((Control)sessionSummaryLabel).set_BasicTooltipText(tooltip);
		}

		protected override void DisposeControl()
		{
			achievementTrackerService.AchievementTracked -= AchievementTrackerService_AchievementTracked;
			achievementTrackerService.AchievementUntracked -= AchievementTrackerService_AchievementUntracked;
			sessionSummaryService.Changed -= sessionSummaryChangedHandler;
			achievementService.PlayerAchievementsLoaded -= AchievementService_PlayerAchievementsLoaded;
			achievementService.ApiAchievementsLoaded -= AchievementService_ApiAchievementsLoaded;
			currentMapService.Changed -= CurrentMapService_Changed;
			markerPackIndexService.Changed -= MarkerPackIndexService_Changed;
			hereExclusionService.Changed -= HereExclusionService_Changed;
			hereService.CandidatesInvalidated -= RefreshHereStrip;
			hereStripCts.Cancel();
			hereStripCts.Dispose();
			foreach (var value in rowControlsById.Values)
			{
				ContextMenuStrip item = value.Menu;
				if (item != null)
				{
					((Control)item).Dispose();
				}
			}
			foreach (var hereStripRow in hereStripRows)
			{
				ContextMenuStrip item2 = hereStripRow.Menu;
				if (item2 != null)
				{
					((Control)item2).Dispose();
				}
			}
			ContextMenuStrip obj = windowMenu;
			if (obj != null)
			{
				((Control)obj).Dispose();
			}
			foreach (KeyValuePair<int, Panel> trackedAchievement in trackedAchievements)
			{
				((Control)trackedAchievement.Value).Dispose();
			}
			trackedAchievements.Clear();
			hereStripRows.Clear();
			((Control)flowPanel).Dispose();
			((WindowBase2)this).DisposeControl();
		}
	}
}
