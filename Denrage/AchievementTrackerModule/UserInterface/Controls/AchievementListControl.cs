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
using Denrage.AchievementTrackerModule.Models.Achievement;
using Microsoft.Xna.Framework;

namespace Denrage.AchievementTrackerModule.UserInterface.Controls
{
	public abstract class AchievementListControl<T, TEntry> : FlowPanel, IAchievementControl where T : AchievementTableEntryDescription
	{
		private readonly IItemDetailWindowManager itemDetailWindowManager;

		private readonly ContentsManager contentsManager;

		private readonly AchievementTableEntry achievement;

		private readonly T description;

		private readonly CollectionAchievementTable achievementDetails;

		private readonly List<Control> itemControls = new List<Control>();

		private Label gameTextLabel;

		private Label gameHintLabel;

		private FlowPanel panel;

		protected IAchievementService AchievementService { get; }

		public AchievementListControl(IItemDetailWindowManager itemDetailWindowManager, IAchievementService achievementService, ContentsManager contentsManager, AchievementTableEntry achievement, T description)
			: this()
		{
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			this.itemDetailWindowManager = itemDetailWindowManager;
			AchievementService = achievementService;
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
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Expected O, but got Unknown
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Expected O, but got Unknown
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Expected O, but got Unknown
			if (!string.IsNullOrEmpty(description.GameText))
			{
				Label val = new Label();
				((Control)val).set_Parent((Container)(object)this);
				val.set_Text(StringUtils.SanitizeHtml(description.GameText));
				val.set_AutoSizeHeight(true);
				((Control)val).set_Width(((Container)this).get_ContentRegion().Width);
				val.set_WrapText(true);
				gameTextLabel = val;
			}
			if (!string.IsNullOrEmpty(description.GameHint))
			{
				Label val2 = new Label();
				((Control)val2).set_Parent((Container)(object)this);
				val2.set_Text(StringUtils.SanitizeHtml(description.GameHint));
				val2.set_TextColor(Color.get_LightGray());
				((Control)val2).set_Width(((Container)this).get_ContentRegion().Width);
				val2.set_AutoSizeHeight(true);
				val2.set_WrapText(true);
				gameHintLabel = val2;
			}
			FlowPanel val3 = new FlowPanel();
			((Control)val3).set_Parent((Container)(object)this);
			val3.set_FlowDirection((ControlFlowDirection)0);
			((Control)val3).set_Width(((Container)this).get_ContentRegion().Width);
			val3.set_ControlPadding(new Vector2(7f));
			((Container)val3).set_HeightSizingMode((SizingMode)1);
			panel = val3;
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
				//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
				bool flag = AchievementService.HasFinishedAchievement(achievement.Id);
				TEntry[] entries = GetEntries(description).ToArray();
				for (int i = 0; i < entries.Length; i++)
				{
					Panel val4 = new Panel();
					((Control)val4).set_Parent((Container)(object)panel);
					val4.set_BackgroundTexture(AsyncTexture2D.op_Implicit(contentsManager.GetTexture("collection_item_background.png")));
					((Control)val4).set_Width(39);
					((Control)val4).set_Height(39);
					Panel val5 = val4;
					Control val6 = CreateEntryControl(i, entries[i], (Container)(object)val5);
					val6.set_Location(new Point((((Control)val5).get_Width() - val6.get_Width()) / 2, (((Control)val5).get_Height() - val6.get_Height()) / 2));
					Tooltip val7 = new Tooltip();
					((Container)val7).set_HeightSizingMode((SizingMode)1);
					((Container)val7).set_WidthSizingMode((SizingMode)1);
					val6.set_Tooltip(val7);
					Label val8 = new Label();
					((Control)val8).set_Parent((Container)(object)val6.get_Tooltip());
					val8.set_AutoSizeWidth(true);
					val8.set_AutoSizeHeight(true);
					val8.set_Text(GetDisplayName(entries[i]));
					ColorControl(val6, flag || AchievementService.HasFinishedAchievementBit(achievement.Id, i));
					if (achievementDetails != null)
					{
						int index = i;
						val6.add_Click((EventHandler<MouseEventArgs>)delegate
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
							val6.add_Click((EventHandler<MouseEventArgs>)delegate
							{
								Process.Start("https://wiki.guildwars2.com" + linkEntry.Link);
							});
						}
					}
					itemControls.Add(val6);
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
