using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Gw2Sharp.WebApi;
using Microsoft.Xna.Framework;
using Quarry.Interfaces;
using Quarry.Models;
using Quarry.UserInterface.Controls;

namespace Quarry.UserInterface.Views
{
	public class HereView : View, IHereCardActions
	{
		private static readonly Logger Logger = Logger.GetLogger<HereView>();

		private const int HeaderControlsWidth = 360;

		private const int OpportunisticNamesShown = 5;

		private const int SectionLabelHeight = 24;

		private const int HiddenListCap = 60;

		private readonly IHereService hereService;

		private readonly ICurrentMapService currentMapService;

		private readonly IAchievementCardFactory achievementCardFactory;

		private readonly IHereExclusionService hereExclusionService;

		private readonly IAchievementTrackerService achievementTrackerService;

		private readonly SettingEntry<int> hereCap;

		private readonly Action openTargetList;

		private readonly CancellationTokenSource cts = new CancellationTokenSource();

		private int requestSequence;

		private Label headerLabel;

		private CardGrid flowPanel;

		private Checkbox showHiddenCheckbox;

		private bool showHidden;

		private IReadOnlyList<HereCandidate> lastCandidates = Array.Empty<HereCandidate>();

		private StandardButton trackTheseButton;

		private string pendingTrackFeedback;

		private bool anywhereExpanded;

		public HereView(IHereService hereService, ICurrentMapService currentMapService, IAchievementCardFactory achievementCardFactory, IHereExclusionService hereExclusionService, IAchievementTrackerService achievementTrackerService, SettingEntry<int> hereCap, Action openTargetList)
			: this()
		{
			this.hereService = hereService;
			this.currentMapService = currentMapService;
			this.achievementCardFactory = achievementCardFactory;
			this.hereExclusionService = hereExclusionService;
			this.achievementTrackerService = achievementTrackerService;
			this.hereCap = hereCap;
			this.openTargetList = openTargetList;
		}

		bool IHereCardActions.IsHidden(int achievementId)
		{
			return hereExclusionService.IsExcluded(achievementId, DateTime.UtcNow);
		}

		void IHereCardActions.SnoozeUntilReset(int achievementId)
		{
			hereExclusionService.Snooze(achievementId);
		}

		void IHereCardActions.HideIndefinitely(int achievementId)
		{
			hereExclusionService.Hide(achievementId);
		}

		void IHereCardActions.Unhide(int achievementId)
		{
			hereExclusionService.Unhide(achievementId);
		}

		string IHereCardActions.DescribeExclusion(int achievementId)
		{
			if (!hereExclusionService.IsSnoozed(achievementId, DateTime.UtcNow))
			{
				return "Hidden until you un-hide it.";
			}
			return "Hidden until the daily reset (00:00 UTC).";
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Expected O, but got Unknown
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_013c: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_0165: Expected O, but got Unknown
			//IL_017d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0182: Unknown result type (might be due to invalid IL or missing references)
			//IL_0189: Unknown result type (might be due to invalid IL or missing references)
			//IL_0194: Unknown result type (might be due to invalid IL or missing references)
			//IL_019f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d0: Expected O, but got Unknown
			Label val = new Label();
			val.set_Text("Loading achievements for " + (currentMapService.MapName ?? "the current map") + "...");
			((Control)val).set_Parent(buildPanel);
			val.set_Font(UiStyle.HeaderFont);
			((Control)val).set_Width(buildPanel.get_ContentRegion().Width - 360);
			((Control)val).set_Height(30);
			val.set_WrapText(true);
			headerLabel = val;
			UiStyle.ApplyTextPrimary(headerLabel);
			CardGrid cardGrid = new CardGrid();
			((Panel)cardGrid).set_ShowBorder(true);
			((Control)cardGrid).set_Parent(buildPanel);
			((Control)cardGrid).set_Location(new Point(0, ((Control)headerLabel).get_Height()));
			((Container)cardGrid).set_WidthSizingMode((SizingMode)2);
			((Container)cardGrid).set_HeightSizingMode((SizingMode)2);
			((Panel)cardGrid).set_CanScroll(true);
			flowPanel = cardGrid;
			StandardButton val2 = new StandardButton();
			((Control)val2).set_Parent(buildPanel);
			val2.set_Text("Target List");
			((Control)val2).set_Width(90);
			((Control)val2).set_Height(26);
			((Control)val2).set_Location(new Point(buildPanel.get_ContentRegion().Width - 229, 2));
			((Control)val2).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				openTargetList();
			});
			StandardButton val3 = new StandardButton();
			((Control)val3).set_Parent(buildPanel);
			val3.set_Text("Target these");
			((Control)val3).set_Width(130);
			((Control)val3).set_Height(26);
			((Control)val3).set_Location(new Point(buildPanel.get_ContentRegion().Width - 134, 2));
			((Control)val3).set_Visible(false);
			trackTheseButton = val3;
			((Control)trackTheseButton).add_Click((EventHandler<MouseEventArgs>)TrackTheseButton_Click);
			Checkbox val4 = new Checkbox();
			((Control)val4).set_Parent(buildPanel);
			val4.set_Text("Show hidden");
			((Control)val4).set_Width(130);
			((Control)val4).set_Height(20);
			((Control)val4).set_Location(new Point(buildPanel.get_ContentRegion().Width - 360, 6));
			((Control)val4).set_Visible(false);
			showHiddenCheckbox = val4;
			showHiddenCheckbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)ShowHiddenCheckbox_CheckedChanged);
			achievementTrackerService.AchievementTracked += AchievementTracker_Changed;
			achievementTrackerService.AchievementUntracked += AchievementTracker_Changed;
			currentMapService.Changed += CurrentMapService_Changed;
			hereExclusionService.Changed += HereExclusionService_Changed;
			hereService.CandidatesInvalidated += HereService_CandidatesInvalidated;
			hereCap.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)HereCap_SettingChanged);
			StartLoadCandidates();
		}

		protected override void Unload()
		{
			currentMapService.Changed -= CurrentMapService_Changed;
			hereExclusionService.Changed -= HereExclusionService_Changed;
			hereService.CandidatesInvalidated -= HereService_CandidatesInvalidated;
			hereCap.remove_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)HereCap_SettingChanged);
			achievementTrackerService.AchievementTracked -= AchievementTracker_Changed;
			achievementTrackerService.AchievementUntracked -= AchievementTracker_Changed;
			if (trackTheseButton != null)
			{
				((Control)trackTheseButton).remove_Click((EventHandler<MouseEventArgs>)TrackTheseButton_Click);
			}
			if (showHiddenCheckbox != null)
			{
				showHiddenCheckbox.remove_CheckedChanged((EventHandler<CheckChangedEvent>)ShowHiddenCheckbox_CheckedChanged);
			}
			cts.Cancel();
			cts.Dispose();
			((View<IPresenter>)this).Unload();
		}

		private void ShowHiddenCheckbox_CheckedChanged(object sender, CheckChangedEvent e)
		{
			showHidden = e.get_Checked();
			StartLoadCandidates();
		}

		private void HereExclusionService_Changed()
		{
			StartLoadCandidates();
		}

		private void HereCap_SettingChanged(object sender, ValueChangedEventArgs<int> e)
		{
			StartLoadCandidates();
		}

		private void AchievementTracker_Changed(int achievementId)
		{
			UpdateTrackTheseButton();
		}

		private int CountTrackable()
		{
			return Math.Min(lastCandidates.Count((HereCandidate c) => !achievementTrackerService.IsBeingTracked(c.Achievement.Id)), achievementTrackerService.FreeSlots);
		}

		private void UpdateTrackTheseButton()
		{
			if (trackTheseButton != null)
			{
				if (lastCandidates.Count == 0)
				{
					((Control)trackTheseButton).set_Visible(false);
					return;
				}
				int trackable = CountTrackable();
				bool full = achievementTrackerService.FreeSlots == 0;
				((Control)trackTheseButton).set_Visible(true);
				((Control)trackTheseButton).set_Enabled(trackable > 0);
				trackTheseButton.set_Text(full ? "Target list full" : $"Target these ({trackable})");
				((Control)trackTheseButton).set_BasicTooltipText(full ? "Untrack something to make room." : "Track everything listed here that isn't tracked yet. Nothing is ever untracked.");
			}
		}

		private void UpdateShowHiddenCheckbox()
		{
			if (showHiddenCheckbox != null)
			{
				int total = hereExclusionService.TotalExcludedCount;
				((Control)showHiddenCheckbox).set_Visible(total > 0 || showHidden);
				showHiddenCheckbox.set_Text($"Show hidden ({total})");
				if (total == 0 && showHiddenCheckbox.get_Checked())
				{
					showHiddenCheckbox.set_Checked(false);
				}
			}
		}

		private void TrackTheseButton_Click(object sender, MouseEventArgs e)
		{
			int tracked = 0;
			bool hitCap = false;
			foreach (HereCandidate candidate in lastCandidates)
			{
				if (!achievementTrackerService.IsBeingTracked(candidate.Achievement.Id))
				{
					if (!achievementTrackerService.TrackAchievement(candidate.Achievement.Id))
					{
						hitCap = true;
						break;
					}
					tracked++;
				}
			}
			pendingTrackFeedback = (hitCap ? $"Tracked {tracked} · list full" : $"Tracked {tracked}");
			StartLoadCandidates();
		}

		private void CurrentMapService_Changed()
		{
			StartLoadCandidates();
		}

		private void HereService_CandidatesInvalidated()
		{
			StartLoadCandidates();
		}

		private void StartLoadCandidates()
		{
			int requestId = Interlocked.Increment(ref requestSequence);
			CancellationToken token = cts.Token;
			Task.Run(() => LoadCandidatesAsync(requestId, token), token);
		}

		private async Task LoadCandidatesAsync(int requestId, CancellationToken cancellationToken)
		{
			int mapIdAtRequest = currentMapService.MapId;
			IReadOnlyList<HereCandidate> hidden;
			HereResult result;
			IReadOnlyList<HereCandidate> anywhere;
			try
			{
				hidden = ((!showHidden) ? Array.Empty<HereCandidate>() : (await hereService.GetHiddenAsync(60)));
				result = ((!showHidden) ? (await hereService.GetCandidatesAsync(hereCap.get_Value())) : null);
				anywhere = ((!showHidden && anywhereExpanded) ? (await hereService.GetNearlyDoneAnywhereAsync(hereCap.get_Value())) : Array.Empty<HereCandidate>());
			}
			catch (OperationCanceledException)
			{
				return;
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Here: the candidates lookup failed.");
				GameService.Overlay.QueueMainThreadUpdate((Action<GameTime>)delegate
				{
					if (!IsStale(requestId, mapIdAtRequest, cancellationToken))
					{
						headerLabel.set_Text((currentMapService.MapName ?? "This map") + ": couldn't load achievements — try again shortly.");
					}
				});
				return;
			}
			if (IsStale(requestId, mapIdAtRequest, cancellationToken))
			{
				return;
			}
			GameService.Overlay.QueueMainThreadUpdate((Action<GameTime>)delegate
			{
				if (!IsStale(requestId, mapIdAtRequest, cancellationToken))
				{
					UpdateShowHiddenCheckbox();
					flowPanel.ClearItems();
					if (showHidden)
					{
						RenderHiddenMode(hidden);
					}
					else
					{
						RenderLiveList(result, anywhere);
					}
				}
			});
		}

		private bool IsStale(int requestId, int mapIdAtRequest, CancellationToken cancellationToken)
		{
			if (!cancellationToken.IsCancellationRequested && requestId == requestSequence)
			{
				return currentMapService.MapId != mapIdAtRequest;
			}
			return true;
		}

		private void RenderHiddenMode(IReadOnlyList<HereCandidate> hidden)
		{
			lastCandidates = Array.Empty<HereCandidate>();
			UpdateTrackTheseButton();
			headerLabel.set_Text((hidden.Count == 0) ? "Nothing hidden." : $"Hidden: {hidden.Count} achievement(s), anywhere. Unhide from a card's + menu.");
			foreach (HereCandidate candidate in hidden)
			{
				CreateCard(candidate);
			}
		}

		private void RenderLiveList(HereResult result, IReadOnlyList<HereCandidate> anywhere)
		{
			string mapName = currentMapService.MapName ?? "This map";
			if (result.Reason == HereResultReason.NoCategoryForMap)
			{
				headerLabel.set_Text(result.IndexReady ? (mapName + ": only guided achievements shown here.") : (mapName + ": Core Tyria not supported yet."));
			}
			else if (result.Reason == HereResultReason.NoPermission)
			{
				headerLabel.set_Text(mapName + ": achievement permissions not granted for this API key.");
			}
			else if (result.Reason == HereResultReason.NotLoaded)
			{
				headerLabel.set_Text(mapName + ": still loading achievement data, try again shortly.");
			}
			else if (result.Candidates.Count == 0)
			{
				if (result.FilteredByGuidance > 0)
				{
					headerLabel.set_Text($"{mapName}: {result.FilteredByGuidance} achievement(s) here, none guided well enough for your filter.");
				}
				else
				{
					headerLabel.set_Text(result.Partial ? (mapName + ": some achievements couldn't be fetched — try again shortly.") : (mapName + ": nothing nearly complete here right now."));
				}
			}
			else if (!result.CategorySupported)
			{
				headerLabel.set_Text(result.Partial ? $"{mapName}: {result.Candidates.Count} guided achievement(s) here (some couldn't be fetched — try again shortly)." : $"{mapName}: {result.Candidates.Count} guided achievement(s) here.");
			}
			else
			{
				headerLabel.set_Text(result.Partial ? $"{mapName}: {result.Candidates.Count} achievement(s) close to done (some couldn't be fetched — try again shortly)." : $"{mapName}: {result.Candidates.Count} achievement(s) close to done.");
			}
			if (result.HiddenCount > 0)
			{
				Label obj = headerLabel;
				obj.set_Text(obj.get_Text() + $" · {result.HiddenCount} hidden here");
			}
			if (pendingTrackFeedback != null)
			{
				Label obj2 = headerLabel;
				obj2.set_Text(obj2.get_Text() + " · " + pendingTrackFeedback);
				pendingTrackFeedback = null;
			}
			IReadOnlyList<HereCandidate> readOnlyList2;
			if (result.Reason != 0)
			{
				IReadOnlyList<HereCandidate> readOnlyList = Array.Empty<HereCandidate>();
				readOnlyList2 = readOnlyList;
			}
			else
			{
				readOnlyList2 = result.Candidates;
			}
			lastCandidates = readOnlyList2;
			UpdateTrackTheseButton();
			if (result.Opportunistic.Count > 0)
			{
				RenderOpportunisticLine(result.Opportunistic);
			}
			List<IGrouping<int, HereCandidate>> list = (from c in result.Candidates
				group c by c.Category.get_Id()).ToList();
			bool showCategoryHeaders = list.Count > 1;
			Dictionary<int, int> rankById = result.Candidates.Select((HereCandidate c, int index) => (c.Achievement.Id, index + 1)).ToDictionary<(int, int), int, int>(((int Id, int Rank) x) => x.Id, ((int Id, int Rank) x) => x.Rank);
			foreach (IGrouping<int, HereCandidate> group in list)
			{
				if (showCategoryHeaders)
				{
					CreateSectionLabel(group.First().Category.get_Name());
				}
				foreach (HereCandidate candidate in group)
				{
					CreateCard(candidate, rankById[candidate.Achievement.Id]);
				}
			}
			if (result.Reason == HereResultReason.Ok || anywhereExpanded)
			{
				RenderAnywhereBlock(anywhere);
			}
		}

		private void RenderOpportunisticLine(IReadOnlyList<HereCandidate> opportunistic)
		{
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			List<string> named = (from c in opportunistic.Take(5)
				select c.Achievement.Name.Trim()).ToList();
			string text = "Also on this map, no route known: " + string.Join(", ", named);
			if (opportunistic.Count > named.Count)
			{
				text += $" (+{opportunistic.Count - named.Count} more)";
			}
			Label obj = CreateSectionLabel(StringUtils.TrimNameToWidth(text, ((Container)flowPanel).get_ContentRegion().Width - 16));
			obj.set_Font(UiStyle.BodyFont);
			UiStyle.ApplyShadow(obj, UiStyle.TextSecondary);
			((Control)obj).set_BasicTooltipText("On this map, but nothing can place them — no checklist steps to tick off, and no marker-pack route or wiki location. Still trackable from the All tab.\n\n" + string.Join("\n", opportunistic.Select((HereCandidate c) => $"{c.Achievement.Name.Trim()}  {c.Current}/{c.Max}")));
		}

		private Label CreateSectionLabel(string text)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Expected O, but got Unknown
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			Label val = new Label();
			val.set_Text(text);
			val.set_Font(UiStyle.SectionFont);
			((Control)val).set_Height(24);
			Label label = val;
			UiStyle.ApplyShadow(label, UiStyle.TextSecondary);
			flowPanel.Add((Control)(object)label, fullWidth: true);
			return label;
		}

		private void RenderAnywhereBlock(IReadOnlyList<HereCandidate> anywhere)
		{
			Label obj = CreateSectionLabel(anywhereExpanded ? "Anywhere: closest to done   (click to hide)" : "Anywhere: closest to done   (click to show)");
			((Control)obj).set_BasicTooltipText("Closest to done across your whole account, wherever they are. Click to show or hide.");
			((Control)obj).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				anywhereExpanded = !anywhereExpanded;
				StartLoadCandidates();
			});
			foreach (HereCandidate candidate in anywhere)
			{
				CreateCard(candidate);
			}
		}

		private AchievementCard CreateCard(HereCandidate candidate, int? rank = null)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			AchievementCard card = achievementCardFactory.Create(candidate.Achievement, RenderUrl.op_Implicit(candidate.Category.get_Icon()), candidate.Guidance, this, rank);
			((Control)card).set_Size(flowPanel.CardSize);
			flowPanel.Add((Control)(object)card);
			return card;
		}
	}
}
