using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Modules.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Quarry.Interfaces;
using Quarry.Models;
using Quarry.UserInterface.Controls;
using Quarry.WikiData.Achievement;

namespace Quarry.UserInterface.Windows
{
	public class InspectorWindow : WindowBase2
	{
		private enum ChipState
		{
			NotDone,
			TickedByYou,
			Confirmed
		}

		private const int WindowWidth = 320;

		private const int WindowHeight = 520;

		private const int ChipSize = 24;

		private const int ChipRingSize = 28;

		private const int ContentScrollbarAllowance = 14;

		private const int ImagePreviewHeight = 160;

		private const int ImageExpandedHeight = 320;

		private static readonly Logger Logger = Logger.GetLogger<InspectorWindow>();

		private readonly IAchievementService achievementService;

		private readonly IWikiSubpageDataService wikiSubpageDataService;

		private readonly IBitAlignmentService bitAlignmentService;

		private readonly IHuntService huntService;

		private readonly INearestObjectiveService nearestObjectiveService;

		private readonly ICurrentMapService currentMapService;

		private readonly IFormattedLabelHtmlService formattedLabelHtmlService;

		private readonly IExternalImageService externalImageService;

		private readonly IAchievementTrackerService achievementTrackerService;

		private readonly IInspectorWindowManager manager;

		private AchievementTableEntry achievement;

		private int? objectiveIndex;

		private FlowPanel contentPanel;

		private Label pinLabel;

		private bool isPinned;

		public InspectorWindow(ContentsManager contentsManager, IAchievementService achievementService, IWikiSubpageDataService wikiSubpageDataService, IBitAlignmentService bitAlignmentService, IHuntService huntService, INearestObjectiveService nearestObjectiveService, ICurrentMapService currentMapService, IFormattedLabelHtmlService formattedLabelHtmlService, IExternalImageService externalImageService, IAchievementTrackerService achievementTrackerService, IInspectorWindowManager manager, bool isDefault)
			: this()
		{
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Expected O, but got Unknown
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			this.achievementService = achievementService;
			this.wikiSubpageDataService = wikiSubpageDataService;
			this.bitAlignmentService = bitAlignmentService;
			this.huntService = huntService;
			this.nearestObjectiveService = nearestObjectiveService;
			this.currentMapService = currentMapService;
			this.formattedLabelHtmlService = formattedLabelHtmlService;
			this.externalImageService = externalImageService;
			this.achievementTrackerService = achievementTrackerService;
			this.manager = manager;
			Texture2D texture = contentsManager.GetTexture("window_blank.png");
			((WindowBase2)this).ConstructWindow(texture, new Rectangle(0, 0, 320, 520), new Rectangle(0, 30, 320, 490));
			if (isDefault)
			{
				((WindowBase2)this).set_SavesPosition(true);
				((WindowBase2)this).set_Id("Quarry_InspectorWindow");
			}
			((WindowBase2)this).set_Emblem(contentsManager.GetTexture("achievement_icon.png"));
			((WindowBase2)this).set_Title("Inspector");
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)this);
			val.set_Text("Pin");
			val.set_Font(UiStyle.BodyFont);
			((Control)val).set_Width(40);
			((Control)val).set_Height(16);
			val.set_HorizontalAlignment((HorizontalAlignment)2);
			((Control)val).set_Location(new Point(((Container)this).get_ContentRegion().Width - 44, 2));
			((Control)val).set_BasicTooltipText("Freeze this Inspector and open a fresh one on the next click");
			pinLabel = val;
			UiStyle.ApplyShadow(pinLabel, UiStyle.TextSecondary);
			((Control)pinLabel).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				TogglePin();
			});
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			WindowBodyPainter.PaintBody(spriteBatch, (Control)(object)this, ((Container)this).get_ContentRegion(), showLeftAccent: false);
			((WindowBase2)this).PaintBeforeChildren(spriteBatch, bounds);
		}

		private void TogglePin()
		{
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			isPinned = !isPinned;
			pinLabel.set_Text(isPinned ? "Pinned" : "Pin");
			UiStyle.ApplyShadow(pinLabel, isPinned ? UiStyle.NearDone : UiStyle.TextSecondary);
			if (isPinned)
			{
				manager.NotifyPinned(this);
			}
		}

		public void SetAchievement(AchievementTableEntry achievement)
		{
			this.achievement = achievement;
			objectiveIndex = null;
			RebuildContent();
		}

		public void SetObjective(AchievementTableEntry achievement, int index)
		{
			this.achievement = achievement;
			objectiveIndex = index;
			RebuildContent();
		}

		private void RebuildContent()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Expected O, but got Unknown
			FlowPanel obj = contentPanel;
			if (obj != null)
			{
				((Control)obj).Dispose();
			}
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Width(((Container)this).get_ContentRegion().Width);
			((Control)val).set_Height(((Container)this).get_ContentRegion().Height);
			val.set_FlowDirection((ControlFlowDirection)3);
			val.set_ControlPadding(new Vector2(0f, 8f));
			val.set_OuterControlPadding(new Vector2(10f, 10f));
			((Panel)val).set_CanScroll(true);
			contentPanel = val;
			if (objectiveIndex.HasValue)
			{
				BuildObjectiveView();
			}
			else
			{
				BuildAchievementView();
			}
		}

		private void BuildAchievementView()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Expected O, but got Unknown
			int contentWidth = ((Container)contentPanel).get_ContentRegion().Width - 14;
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)contentPanel);
			val.set_Text(achievement.Name.Trim());
			val.set_Font(UiStyle.NumeralFont);
			val.set_WrapText(true);
			val.set_AutoSizeHeight(true);
			((Control)val).set_Width(contentWidth);
			UiStyle.ApplyTextPrimary(val);
			BuildChipRow(null);
			AchievementTableEntryDescription description = achievement.Description;
			if (!string.IsNullOrEmpty(description?.GameText))
			{
				((Control)formattedLabelHtmlService.CreateLabel(description.GameText).AutoSizeHeight().SetWidth(contentWidth)
					.Wrap()
					.Build()).set_Parent((Container)(object)contentPanel);
			}
			if (!string.IsNullOrEmpty(description?.GameHint))
			{
				((Control)formattedLabelHtmlService.CreateLabel(description.GameHint).AutoSizeHeight().SetWidth(contentWidth)
					.Wrap()
					.Build()).set_Parent((Container)(object)contentPanel);
			}
			BuildNextLine(null);
			BuildActions(null);
		}

		private void BuildObjectiveView()
		{
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Expected O, but got Unknown
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Expected O, but got Unknown
			//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d0: Expected O, but got Unknown
			//IL_022d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0232: Unknown result type (might be due to invalid IL or missing references)
			//IL_023e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0245: Unknown result type (might be due to invalid IL or missing references)
			//IL_024c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0255: Expected O, but got Unknown
			int index = objectiveIndex.Value;
			IReadOnlyList<(string, string)> entries = GetEntries();
			(string, string) entry = ((index < entries.Count) ? entries[index] : (string.Empty, null));
			int contentWidth = ((Container)contentPanel).get_ContentRegion().Width - 14;
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)contentPanel);
			val.set_Text($"{achievement.Name.Trim()} · objective {index + 1} of {entries.Count}");
			val.set_Font(UiStyle.SectionFont);
			val.set_WrapText(true);
			val.set_AutoSizeHeight(true);
			((Control)val).set_Width(contentWidth);
			((Control)val).set_BasicTooltipText("Back to the achievement");
			UiStyle.ApplyShadow(val, UiStyle.TextSecondary);
			((Control)val).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				SetAchievement(achievement);
			});
			BuildChipRow(index);
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)contentPanel);
			val2.set_Text(entry.Item1 ?? string.Empty);
			val2.set_Font(UiStyle.NumeralFont);
			val2.set_WrapText(true);
			val2.set_AutoSizeHeight(true);
			((Control)val2).set_Width(contentWidth);
			UiStyle.ApplyTextPrimary(val2);
			int bitIndex = bitAlignmentService.MapRowToBit(achievement.Id, index);
			DerivedSubpage subPage = FindSubPage(entry.Item2);
			if (!string.IsNullOrEmpty(subPage?.ImageUrl))
			{
				AsyncTexture2D subPageTexture = externalImageService.GetImageFromIndirectLink(subPage.ImageUrl);
				ImageSpinner imageSpinner = new ImageSpinner(subPageTexture);
				((Control)imageSpinner).set_Parent((Container)(object)contentPanel);
				((Control)imageSpinner).set_Width(contentWidth);
				((Control)imageSpinner).set_Height(160);
				MakeImageExpandable((Control)(object)imageSpinner, subPageTexture);
			}
			else
			{
				FlowPanel val3 = new FlowPanel();
				((Control)val3).set_Parent((Container)(object)contentPanel);
				((Control)val3).set_Width(contentWidth);
				((Container)val3).set_HeightSizingMode((SizingMode)1);
				val3.set_FlowDirection((ControlFlowDirection)3);
				FlowPanel mapImagePanel = val3;
				PopulateMapColumnImage(mapImagePanel, achievement.Id, index, contentWidth);
			}
			if (!string.IsNullOrEmpty(subPage?.Description))
			{
				((Control)formattedLabelHtmlService.CreateLabel(subPage.Description).AutoSizeHeight().SetWidth(contentWidth)
					.Wrap()
					.Build()).set_Parent((Container)(object)contentPanel);
			}
			FlowPanel val4 = new FlowPanel();
			((Control)val4).set_Parent((Container)(object)contentPanel);
			((Control)val4).set_Width(contentWidth);
			((Container)val4).set_HeightSizingMode((SizingMode)1);
			val4.set_FlowDirection((ControlFlowDirection)3);
			FlowPanel hunterNotesPanel = val4;
			PopulateHunterNotes(hunterNotesPanel, achievement.Id, index, contentWidth);
			BuildNextLine(bitIndex);
			BuildTickToggle(index, bitIndex);
			BuildActions(entry.Item2);
		}

		private async Task PopulateMapColumnImage(FlowPanel container, int achievementId, int rowIndex, int contentWidth)
		{
			try
			{
				IReadOnlyList<CollectionAchievementTable> achievementDetails = await achievementService.GetAchievementDetailsAsync();
				AchievementTableEntry achievementTableEntry = achievement;
				if (achievementTableEntry == null || achievementTableEntry.Id != achievementId || objectiveIndex != rowIndex || ((Control)container).get_Parent() == null)
				{
					return;
				}
				CollectionAchievementTable table = achievementDetails.FirstOrDefault((CollectionAchievementTable t) => t.Id == achievementId);
				if (table == null || rowIndex >= table.Entries.Count)
				{
					return;
				}
				int mapColumnIndex = Array.FindIndex(table.ColumnNames, (string c) => string.Equals(c, "Map", StringComparison.OrdinalIgnoreCase));
				List<CollectionAchievementTable.CollectionAchievementTableEntry> row = table.Entries[rowIndex];
				if (mapColumnIndex < 0 || mapColumnIndex >= row.Count)
				{
					return;
				}
				CollectionAchievementTable.CollectionAchievementTableMapEntry mapEntry = row[mapColumnIndex] as CollectionAchievementTable.CollectionAchievementTableMapEntry;
				if (mapEntry == null || string.IsNullOrEmpty(mapEntry.ImageLink))
				{
					return;
				}
				string imageLink = mapEntry.ImageLink;
				GameService.Overlay.QueueMainThreadUpdate((Action<GameTime>)delegate
				{
					AchievementTableEntry achievementTableEntry2 = achievement;
					if (achievementTableEntry2 != null && achievementTableEntry2.Id == achievementId && objectiveIndex == rowIndex && ((Control)container).get_Parent() != null)
					{
						AsyncTexture2D imageFromIndirectLink = externalImageService.GetImageFromIndirectLink(imageLink);
						ImageSpinner imageSpinner = new ImageSpinner(imageFromIndirectLink);
						((Control)imageSpinner).set_Parent((Container)(object)container);
						((Control)imageSpinner).set_Width(contentWidth);
						((Control)imageSpinner).set_Height(160);
						MakeImageExpandable((Control)(object)imageSpinner, imageFromIndirectLink);
					}
				});
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Inspector: couldn't add the Map-column fallback image.");
			}
		}

		private static void MakeImageExpandable(Control image, AsyncTexture2D texture)
		{
			bool expanded = false;
			image.set_BasicTooltipText("Click to expand");
			image.add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)delegate
			{
				expanded = !expanded;
				if (!expanded)
				{
					image.set_Height(160);
				}
				else
				{
					Texture2D texture2 = texture.get_Texture();
					int num = ((texture2 != null) ? texture2.get_Width() : 0);
					Texture2D texture3 = texture.get_Texture();
					int num2 = ((texture3 != null) ? texture3.get_Height() : 0);
					image.set_Height((num > 0 && num2 > 0) ? ((int)Math.Round((float)image.get_Width() * ((float)num2 / (float)num))) : 320);
				}
				image.set_BasicTooltipText(expanded ? "Click to shrink" : "Click to expand");
			});
		}

		private async Task PopulateHunterNotes(FlowPanel container, int achievementId, int rowIndex, int contentWidth)
		{
			try
			{
				await PopulateHunterNotesCore(container, achievementId, rowIndex, contentWidth);
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Inspector: couldn't add the wiki Notes column.");
			}
		}

		private async Task PopulateHunterNotesCore(FlowPanel container, int achievementId, int rowIndex, int contentWidth)
		{
			IReadOnlyList<CollectionAchievementTable> achievementDetails = await achievementService.GetAchievementDetailsAsync();
			AchievementTableEntry achievementTableEntry = achievement;
			if (achievementTableEntry == null || achievementTableEntry.Id != achievementId || objectiveIndex != rowIndex || ((Control)container).get_Parent() == null)
			{
				return;
			}
			CollectionAchievementTable table = achievementDetails.FirstOrDefault((CollectionAchievementTable t) => t.Id == achievementId);
			if (table == null)
			{
				return;
			}
			int notesColumnIndex = Array.FindIndex(table.ColumnNames, (string c) => string.Equals(c, "Notes", StringComparison.OrdinalIgnoreCase) || string.Equals(c, "Note", StringComparison.OrdinalIgnoreCase));
			if (notesColumnIndex < 0)
			{
				string[] columnNames = table.ColumnNames;
				foreach (string columnName in columnNames)
				{
					if (columnName.IndexOf("note", StringComparison.OrdinalIgnoreCase) >= 0)
					{
						Logger.Debug($"Achievement {achievementId} has a column named \"{columnName}\" that the Notes match missed.");
					}
				}
				return;
			}
			if (rowIndex >= table.Entries.Count)
			{
				Logger.Debug($"Achievement {achievementId}'s table has {table.Entries.Count} rows, fewer than objective index {rowIndex} -- row indices may not line up for this achievement.");
				return;
			}
			List<CollectionAchievementTable.CollectionAchievementTableEntry> row = table.Entries[rowIndex];
			if (notesColumnIndex >= row.Count)
			{
				return;
			}
			CollectionAchievementTable.CollectionAchievementTableStringEntry stringEntry = row[notesColumnIndex] as CollectionAchievementTable.CollectionAchievementTableStringEntry;
			if (stringEntry == null || string.IsNullOrEmpty(stringEntry.Text))
			{
				return;
			}
			string notesHtml = stringEntry.Text;
			DateTime snapshotDate = achievementService.AchievementTablesSnapshotDate;
			GameService.Overlay.QueueMainThreadUpdate((Action<GameTime>)delegate
			{
				//IL_0059: Unknown result type (might be due to invalid IL or missing references)
				//IL_005e: Unknown result type (might be due to invalid IL or missing references)
				//IL_006a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0085: Unknown result type (might be due to invalid IL or missing references)
				//IL_0090: Unknown result type (might be due to invalid IL or missing references)
				//IL_0097: Unknown result type (might be due to invalid IL or missing references)
				//IL_009e: Unknown result type (might be due to invalid IL or missing references)
				//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b4: Expected O, but got Unknown
				AchievementTableEntry achievementTableEntry2 = achievement;
				if (achievementTableEntry2 != null && achievementTableEntry2.Id == achievementId && objectiveIndex == rowIndex && ((Control)container).get_Parent() != null)
				{
					Label val = new Label();
					((Control)val).set_Parent((Container)(object)container);
					val.set_Text($"Hunter notes — wiki data as of {snapshotDate:yyyy-MM-dd}");
					val.set_Font(UiStyle.SectionFont);
					val.set_WrapText(true);
					val.set_AutoSizeHeight(true);
					((Control)val).set_Width(contentWidth);
					UiStyle.ApplyShadow(val, UiStyle.TextSecondary);
					((Control)formattedLabelHtmlService.CreateLabel(notesHtml).AutoSizeHeight().SetWidth(contentWidth)
						.Wrap()
						.Build()).set_Parent((Container)(object)container);
				}
			});
		}

		private DerivedSubpage FindSubPage(string link)
		{
			if (string.IsNullOrEmpty(link))
			{
				return null;
			}
			string fullLink = (link.StartsWith("http", StringComparison.OrdinalIgnoreCase) ? link : ("https://wiki.guildwars2.com" + link));
			if (!wikiSubpageDataService.ByLink.TryGetValue(fullLink, out var subPage))
			{
				return null;
			}
			return subPage;
		}

		private void BuildChipRow(int? selectedIndex)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Expected O, but got Unknown
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Expected O, but got Unknown
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			//IL_012d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_0149: Unknown result type (might be due to invalid IL or missing references)
			//IL_0151: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Expected O, but got Unknown
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			//IL_0178: Unknown result type (might be due to invalid IL or missing references)
			//IL_0180: Unknown result type (might be due to invalid IL or missing references)
			//IL_0197: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d9: Expected O, but got Unknown
			IReadOnlyList<(string, string)> entries = GetEntries();
			if (entries.Count == 0)
			{
				return;
			}
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent((Container)(object)contentPanel);
			((Control)val).set_Width(((Container)contentPanel).get_ContentRegion().Width - 14);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			val.set_FlowDirection((ControlFlowDirection)0);
			val.set_ControlPadding(new Vector2(3f, 3f));
			FlowPanel chipsPanel = val;
			for (int i = 0; i < entries.Count; i++)
			{
				int index = i;
				ChipState state = GetChipState(index);
				bool selected = selectedIndex == index;
				Panel val2 = new Panel();
				((Control)val2).set_Parent((Container)(object)chipsPanel);
				((Control)val2).set_Width(28);
				((Control)val2).set_Height(28);
				((Control)val2).set_BackgroundColor(selected ? UiStyle.NearDone : Color.get_Transparent());
				Panel ring = val2;
				_003F val3;
				switch (state)
				{
				default:
					val3 = UiStyle.PipTodo;
					break;
				case ChipState.TickedByYou:
				{
					Color manualDone = UiStyle.ManualDone;
					byte r = ((Color)(ref manualDone)).get_R();
					manualDone = UiStyle.ManualDone;
					byte g = ((Color)(ref manualDone)).get_G();
					manualDone = UiStyle.ManualDone;
					val3 = Color.FromNonPremultiplied((int)r, (int)g, (int)((Color)(ref manualDone)).get_B(), 90);
					break;
				}
				case ChipState.Confirmed:
					val3 = new Color(60, 110, 65);
					break;
				}
				Color fill = (Color)val3;
				Panel val4 = new Panel();
				((Control)val4).set_Parent((Container)(object)ring);
				((Control)val4).set_Width(24);
				((Control)val4).set_Height(24);
				((Control)val4).set_Location(new Point(2, 2));
				((Control)val4).set_BackgroundColor(fill);
				val4.set_ShowBorder(state == ChipState.TickedByYou);
				Panel chip = val4;
				Label val5 = new Label();
				((Control)val5).set_Parent((Container)(object)chip);
				val5.set_Text((index + 1).ToString());
				val5.set_Font(UiStyle.BodyFont);
				val5.set_HorizontalAlignment((HorizontalAlignment)1);
				val5.set_VerticalAlignment((VerticalAlignment)1);
				((Control)val5).set_Width(24);
				((Control)val5).set_Height(24);
				val5.set_TextColor((state == ChipState.TickedByYou) ? UiStyle.ManualDone : UiStyle.TextSecondary);
				Label numberLabel = val5;
				string tooltip = state switch
				{
					ChipState.TickedByYou => "Ticked by you -- right-click to unmark", 
					ChipState.Confirmed => "Confirmed by the API", 
					_ => "Not done -- right-click to mark done", 
				};
				((Control)chip).set_BasicTooltipText(tooltip);
				((Control)numberLabel).set_BasicTooltipText(tooltip);
				((Control)chip).add_Click((EventHandler<MouseEventArgs>)NavigateOrDeselect);
				((Control)numberLabel).add_Click((EventHandler<MouseEventArgs>)NavigateOrDeselect);
				if (state != ChipState.Confirmed)
				{
					((Control)chip).add_RightMouseButtonReleased((EventHandler<MouseEventArgs>)delegate
					{
						ToggleAndRefresh(index);
					});
					((Control)numberLabel).add_RightMouseButtonReleased((EventHandler<MouseEventArgs>)delegate
					{
						ToggleAndRefresh(index);
					});
				}
				void NavigateOrDeselect(object s, MouseEventArgs e)
				{
					if (selected)
					{
						SetAchievement(achievement);
					}
					else
					{
						SetObjective(achievement, index);
					}
				}
			}
		}

		private void ToggleAndRefresh(int index)
		{
			achievementService.ToggleManualCompleteStatus(achievement.Id, index);
			RebuildContent();
		}

		private ChipState GetChipState(int rowIndex)
		{
			int bitIndex = bitAlignmentService.MapRowToBit(achievement.Id, rowIndex);
			if (bitIndex < 0)
			{
				return ChipState.NotDone;
			}
			if (achievementService.PlayerAchievementsById.TryGetValue(achievement.Id, out var playerAchievement) && (playerAchievement.get_Bits()?.Contains(bitIndex) ?? false))
			{
				return ChipState.Confirmed;
			}
			if (!achievementService.HasFinishedAchievementBit(achievement.Id, rowIndex))
			{
				return ChipState.NotDone;
			}
			return ChipState.TickedByYou;
		}

		private IReadOnlyList<(string DisplayName, string Link)> GetEntries()
		{
			AchievementTableEntryDescription description = achievement.Description;
			CollectionDescription collection = description as CollectionDescription;
			if (collection == null)
			{
				ObjectivesDescription objectives = description as ObjectivesDescription;
				if (objectives != null)
				{
					return objectives.EntryList.Select((TableDescriptionEntry e) => (e.DisplayName, e.Link)).ToList();
				}
				return Array.Empty<(string, string)>();
			}
			return collection.EntryList.Select((CollectionDescriptionEntry e) => (e.DisplayName, e.Link)).ToList();
		}

		private void BuildNextLine(int? bitIndex)
		{
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_0161: Expected O, but got Unknown
			if (achievementService.PlayerAchievements != null && nearestObjectiveService.HasAnyObjectives(achievement.Id))
			{
				int mapId = currentMapService.MapId;
				Vector3 player = GameService.Gw2Mumble.get_PlayerCharacter().get_Position();
				IReadOnlyList<RemainingObjective> nearest = nearestObjectiveService.GetRemaining(achievement.Id, mapId, player);
				bool num = bitIndex.HasValue && bitIndex.Value < 0;
				RemainingObjective target = (num ? null : (bitIndex.HasValue ? nearest.FirstOrDefault((RemainingObjective o) => o.Bit == bitIndex.Value) : nearest.FirstOrDefault()));
				int contentWidth = ((Container)contentPanel).get_ContentRegion().Width - 14;
				string text = (num ? "step not identified — this wiki row couldn't be matched to an API step" : ((target == null) ? "no route on this map" : string.Format("{0} · {1:F0} m{2}", target.Name, target.DistanceMetres, target.GroundDistanceOnly ? " (ground)" : string.Empty)));
				Label val = new Label();
				((Control)val).set_Parent((Container)(object)contentPanel);
				val.set_Text(text);
				val.set_Font(UiStyle.BodyFont);
				val.set_WrapText(true);
				val.set_AutoSizeHeight(true);
				((Control)val).set_Width(contentWidth);
				UiStyle.ApplyShadow(val, (target == null) ? UiStyle.Rank : UiStyle.TextPrimary);
			}
		}

		private void BuildTickToggle(int rowIndex, int bitIndex)
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			ChipState state = GetChipState(rowIndex);
			if (state != ChipState.Confirmed)
			{
				Checkbox val = new Checkbox();
				((Control)val).set_Parent((Container)(object)contentPanel);
				val.set_Text("Ticked by you");
				val.set_Checked(state == ChipState.TickedByYou);
				((Control)val).set_Width(((Container)contentPanel).get_ContentRegion().Width - 14);
				((Control)val).set_BasicTooltipText("Marks this done for you only, until the API confirms it or you unmark it.");
				val.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
				{
					ToggleAndRefresh(rowIndex);
				});
			}
		}

		private void BuildActions(string entryLink)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Expected O, but got Unknown
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0128: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_016c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0171: Unknown result type (might be due to invalid IL or missing references)
			//IL_0178: Unknown result type (might be due to invalid IL or missing references)
			//IL_0183: Unknown result type (might be due to invalid IL or missing references)
			//IL_018b: Unknown result type (might be due to invalid IL or missing references)
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent((Container)(object)contentPanel);
			((Control)val).set_Width(((Container)contentPanel).get_ContentRegion().Width - 14);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			val.set_FlowDirection((ControlFlowDirection)0);
			val.set_ControlPadding(new Vector2(4f, 4f));
			FlowPanel actionsPanel = val;
			GuidanceInfo guidance = nearestObjectiveService.GetGuidance(achievement.Id, currentMapService.MapId);
			bool packBacked = guidance.Tier == GuidanceTier.Tagged || guidance.Tier == GuidanceTier.Route;
			bool alreadyTracked = achievementTrackerService.IsBeingTracked(achievement.Id);
			if (huntService.CanPeek && packBacked && !alreadyTracked)
			{
				StandardButton val2 = new StandardButton();
				((Control)val2).set_Parent((Container)(object)actionsPanel);
				val2.set_Text("Show route in Pathing");
				((Control)val2).set_Width(150);
				((Control)val2).set_Height(26);
				((Control)val2).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					huntService.Peek(achievement.Id);
				});
			}
			if (nearestObjectiveService.HasAnyObjectives(achievement.Id))
			{
				StandardButton val3 = new StandardButton();
				((Control)val3).set_Parent((Container)(object)actionsPanel);
				val3.set_Text("Copy waypoint");
				((Control)val3).set_Width(110);
				((Control)val3).set_Height(26);
				((Control)val3).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					//IL_001b: Unknown result type (might be due to invalid IL or missing references)
					//IL_0020: Unknown result type (might be due to invalid IL or missing references)
					//IL_003d: Unknown result type (might be due to invalid IL or missing references)
					int mapId = currentMapService.MapId;
					Vector3 position = GameService.Gw2Mumble.get_PlayerCharacter().get_Position();
					WaypointSuggestion waypointSuggestion = nearestObjectiveService.NearestWaypoint(achievement.Id, mapId, position);
					if (waypointSuggestion == null)
					{
						ScreenNotification.ShowNotification("No waypoint known for this map yet", (NotificationType)0, (Texture2D)null, 4);
					}
					else
					{
						ClipboardUtil.get_WindowsClipboardService().SetTextAsync(waypointSuggestion.Code);
						ScreenNotification.ShowNotification((waypointSuggestion.Name == null) ? "Waypoint copied" : ("Copied " + waypointSuggestion.Name), (NotificationType)0, (Texture2D)null, 4);
					}
				});
			}
			string wikiLink = entryLink ?? achievement.Link;
			if (!string.IsNullOrEmpty(wikiLink))
			{
				StandardButton val4 = new StandardButton();
				((Control)val4).set_Parent((Container)(object)actionsPanel);
				val4.set_Text("Wiki");
				((Control)val4).set_Width(60);
				((Control)val4).set_Height(26);
				((Control)val4).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					Process.Start("https://wiki.guildwars2.com" + wikiLink);
				});
			}
		}
	}
}
