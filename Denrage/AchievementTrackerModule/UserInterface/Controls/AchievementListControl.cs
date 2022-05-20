using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Modules.Managers;
using Denrage.AchievementTrackerModule.Interfaces;
using Denrage.AchievementTrackerModule.Libs.Achievement;
using Denrage.AchievementTrackerModule.UserInterface.Controls.FormattedLabel;
using Microsoft.Xna.Framework;

namespace Denrage.AchievementTrackerModule.UserInterface.Controls
{
	public abstract class AchievementListControl<T, TEntry> : FlowPanel, IAchievementControl where T : AchievementTableEntryDescription
	{
		private readonly IItemDetailWindowManager itemDetailWindowManager;

		private readonly IFormattedLabelHtmlService formattedLabelHtmlService;

		private readonly ContentsManager contentsManager;

		private readonly AchievementTableEntry achievement;

		private readonly T description;

		private readonly CollectionAchievementTable achievementDetails;

		private readonly List<Control> itemControls = new List<Control>();

		private Denrage.AchievementTrackerModule.UserInterface.Controls.FormattedLabel.FormattedLabel gameTextLabel;

		private Denrage.AchievementTrackerModule.UserInterface.Controls.FormattedLabel.FormattedLabel gameHintLabel;

		private FlowPanel panel;

		protected IAchievementService AchievementService { get; }

		public AchievementListControl(IItemDetailWindowManager itemDetailWindowManager, IAchievementService achievementService, IFormattedLabelHtmlService formattedLabelHtmlService, ContentsManager contentsManager, AchievementTableEntry achievement, T description)
			: this()
		{
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			this.itemDetailWindowManager = itemDetailWindowManager;
			AchievementService = achievementService;
			this.formattedLabelHtmlService = formattedLabelHtmlService;
			this.contentsManager = contentsManager;
			this.achievement = achievement;
			this.description = description;
			achievementDetails = AchievementService.AchievementDetails.FirstOrDefault((CollectionAchievementTable x) => x.Id == achievement.Id);
			((FlowPanel)this).set_FlowDirection((ControlFlowDirection)3);
			((FlowPanel)this).set_ControlPadding(new Vector2(7f));
			AchievementService.PlayerAchievementsLoaded += AchievementService_PlayerAchievementsLoaded;
		}

		private void AchievementService_PlayerAchievementsLoaded()
		{
			bool finishedAchievement = AchievementService.HasFinishedAchievement(achievement.Id);
			for (int i = 0; i < itemControls.Count; i++)
			{
				ColorControl(itemControls[i], finishedAchievement || AchievementService.HasFinishedAchievementBit(achievement.Id, i));
			}
		}

		public void BuildControl()
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Expected O, but got Unknown
			if (!string.IsNullOrEmpty(description.GameText))
			{
				FormattedLabelBuilder labelBuilder2 = formattedLabelHtmlService.CreateLabel(description.GameText).AutoSizeHeight().SetWidth(((Container)this).get_ContentRegion().Width)
					.Wrap();
				gameTextLabel = labelBuilder2.Build();
				((Control)gameTextLabel).set_Parent((Container)(object)this);
			}
			if (!string.IsNullOrEmpty(description.GameHint))
			{
				FormattedLabelBuilder labelBuilder = formattedLabelHtmlService.CreateLabel(description.GameHint).AutoSizeHeight().SetWidth(((Container)this).get_ContentRegion().Width)
					.Wrap();
				gameHintLabel = labelBuilder.Build();
				((Control)gameHintLabel).set_Parent((Container)(object)this);
			}
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent((Container)(object)this);
			val.set_FlowDirection((ControlFlowDirection)0);
			((Control)val).set_Width(((Container)this).get_ContentRegion().Width);
			val.set_ControlPadding(new Vector2(7f));
			((Container)val).set_HeightSizingMode((SizingMode)1);
			panel = val;
			Task.Run(delegate
			{
				//IL_0042: Unknown result type (might be due to invalid IL or missing references)
				//IL_0047: Unknown result type (might be due to invalid IL or missing references)
				//IL_0053: Unknown result type (might be due to invalid IL or missing references)
				//IL_006e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0076: Unknown result type (might be due to invalid IL or missing references)
				//IL_007f: Expected O, but got Unknown
				//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
				//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
				//IL_00db: Expected O, but got Unknown
				//IL_00db: Unknown result type (might be due to invalid IL or missing references)
				//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
				//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
				//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
				//IL_0105: Expected O, but got Unknown
				//IL_0105: Unknown result type (might be due to invalid IL or missing references)
				//IL_010a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0112: Unknown result type (might be due to invalid IL or missing references)
				//IL_0115: Unknown result type (might be due to invalid IL or missing references)
				//IL_0124: Unknown result type (might be due to invalid IL or missing references)
				//IL_012b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0132: Unknown result type (might be due to invalid IL or missing references)
				bool flag = AchievementService.HasFinishedAchievement(achievement.Id);
				TEntry[] entries = GetEntries(description).ToArray();
				for (int i = 0; i < entries.Length; i++)
				{
					Panel val2 = new Panel();
					((Control)val2).set_Parent((Container)(object)panel);
					val2.set_BackgroundTexture(AsyncTexture2D.op_Implicit(contentsManager.GetTexture("collection_item_background.png")));
					((Control)val2).set_Width(39);
					((Control)val2).set_Height(39);
					Panel val3 = val2;
					Control val4 = CreateEntryControl(i, entries[i], (Container)(object)val3);
					val4.set_Location(new Point((((Control)val3).get_Width() - val4.get_Width()) / 2, (((Control)val3).get_Height() - val4.get_Height()) / 2));
					Tooltip val5 = new Tooltip();
					((Container)val5).set_HeightSizingMode((SizingMode)1);
					((Container)val5).set_WidthSizingMode((SizingMode)1);
					val4.set_Tooltip(val5);
					FlowPanel val6 = new FlowPanel();
					((Control)val6).set_Parent((Container)(object)val4.get_Tooltip());
					((Control)val6).set_Width(100);
					((Container)val6).set_HeightSizingMode((SizingMode)1);
					val6.set_FlowDirection((ControlFlowDirection)3);
					FlowPanel val7 = val6;
					Label val8 = new Label();
					((Control)val8).set_Parent((Container)(object)val7);
					((Control)val8).set_Width(((Container)val7).get_ContentRegion().Width);
					val8.set_WrapText(true);
					val8.set_AutoSizeHeight(true);
					val8.set_Font(Control.get_Content().get_DefaultFont18());
					val8.set_Text(GetDisplayName(entries[i]));
					ColorControl(val4, flag || AchievementService.HasFinishedAchievementBit(achievement.Id, i));
					if (achievementDetails != null)
					{
						int index = i;
						val4.add_Click((EventHandler<MouseEventArgs>)delegate
						{
							itemDetailWindowManager.CreateAndShowWindow(GetDisplayName(entries[index]), achievementDetails.ColumnNames, achievementDetails.Entries[index], achievementDetails.Link, achievementDetails.Id, index);
						});
					}
					else
					{
						TEntry val9 = entries[i];
						ILinkEntry linkEntry = val9 as ILinkEntry;
						if (linkEntry != null)
						{
							val4.add_Click((EventHandler<MouseEventArgs>)delegate
							{
								Process.Start("https://wiki.guildwars2.com" + linkEntry.Link);
							});
						}
					}
					itemControls.Add(val4);
				}
			});
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			if (gameTextLabel != null)
			{
				((Control)gameTextLabel).set_Width(((Container)this).get_ContentRegion().Width);
			}
			if (gameHintLabel != null)
			{
				((Control)gameHintLabel).set_Width(((Container)this).get_ContentRegion().Width);
			}
			((Control)panel).set_Width(((Container)this).get_ContentRegion().Width);
			((Container)this).OnResized(e);
		}

		protected abstract IEnumerable<TEntry> GetEntries(T description);

		protected abstract Control CreateEntryControl(int index, TEntry entry, Container parent);

		protected abstract void ColorControl(Control control, bool achievementBitFinished);

		protected abstract string GetDisplayName(TEntry entry);

		protected override void DisposeControl()
		{
			foreach (Control itemControl in itemControls)
			{
				itemControl.Dispose();
			}
			itemControls.Clear();
			((Panel)this).DisposeControl();
		}

		Point IAchievementControl.get_Size()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return ((Control)this).get_Size();
		}

		void IAchievementControl.set_Size(Point value)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Size(value);
		}
	}
}
