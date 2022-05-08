using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Modules.Managers;
using Denrage.AchievementTrackerModule.Interfaces;
using Denrage.AchievementTrackerModule.Models.Achievement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Denrage.AchievementTrackerModule.UserInterface.Windows
{
	public class ItemDetailWindow : WindowBase2
	{
		private const int PADDING = 15;

		private readonly ContentsManager contentsManager;

		private readonly IAchievementService achievementService;

		private readonly IAchievementTableEntryProvider achievementTableEntryProvider;

		private readonly string achievementLink;

		private readonly string name;

		private readonly string[] columns;

		private readonly List<CollectionAchievementTable.CollectionAchievementTableEntry> item;

		private readonly Texture2D texture;

		public ItemDetailWindow(ContentsManager contentsManager, IAchievementService achievementService, IAchievementTableEntryProvider achievementTableEntryProvider, string achievementLink, string name, string[] columns, List<CollectionAchievementTable.CollectionAchievementTableEntry> item)
			: this()
		{
			this.contentsManager = contentsManager;
			this.achievementService = achievementService;
			this.achievementTableEntryProvider = achievementTableEntryProvider;
			this.achievementLink = achievementLink;
			texture = this.contentsManager.GetTexture("item_detail_background.png");
			this.name = name;
			this.columns = columns;
			this.item = item;
			BuildWindow();
		}

		private void BuildWindow()
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Expected O, but got Unknown
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Expected O, but got Unknown
			//IL_019c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c2: Expected O, but got Unknown
			//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01db: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0201: Unknown result type (might be due to invalid IL or missing references)
			//IL_0208: Unknown result type (might be due to invalid IL or missing references)
			//IL_0211: Expected O, but got Unknown
			//IL_025b: Unknown result type (might be due to invalid IL or missing references)
			((WindowBase2)this).set_Title("Item Details");
			((WindowBase2)this).ConstructWindow(texture, new Rectangle(0, 0, 600, 400), new Rectangle(0, 30, 600, 370));
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent((Container)(object)this);
			Rectangle contentRegion = ((Container)this).get_ContentRegion();
			((Control)val).set_Size(((Rectangle)(ref contentRegion)).get_Size());
			val.set_FlowDirection((ControlFlowDirection)3);
			((Control)val).set_Width(((Container)this).get_ContentRegion().Width - 30);
			((Control)val).set_Location(new Point(15, 0));
			((Control)val).set_Height(((Container)this).get_ContentRegion().Height);
			val.set_ControlPadding(new Vector2(10f));
			((Panel)val).set_CanScroll(true);
			FlowPanel panel = val;
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)panel);
			((Control)val2).set_Width(((Container)panel).get_ContentRegion().Width);
			val2.set_Text(name);
			val2.set_AutoSizeHeight(true);
			val2.set_WrapText(true);
			val2.set_Font(Control.get_Content().get_DefaultFont18());
			Label itemTitle = val2;
			CollectionAchievementTable.CollectionAchievementTableItemEntry item = this.item.OfType<CollectionAchievementTable.CollectionAchievementTableItemEntry>().FirstOrDefault();
			string link = achievementLink;
			if (item != null)
			{
				link = item.Link;
			}
			if (!string.IsNullOrEmpty(link))
			{
				((Control)itemTitle).add_MouseEntered((EventHandler<MouseEventArgs>)delegate
				{
					//IL_0006: Unknown result type (might be due to invalid IL or missing references)
					itemTitle.set_TextColor(Color.get_LightBlue());
				});
				((Control)itemTitle).add_MouseLeft((EventHandler<MouseEventArgs>)delegate
				{
					//IL_0006: Unknown result type (might be due to invalid IL or missing references)
					itemTitle.set_TextColor(Color.get_White());
				});
				((Control)itemTitle).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)delegate
				{
					//IL_0015: Unknown result type (might be due to invalid IL or missing references)
					itemTitle.set_TextColor(new Color(206, 174, 250));
				});
				((Control)itemTitle).add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)delegate
				{
					//IL_0006: Unknown result type (might be due to invalid IL or missing references)
					itemTitle.set_TextColor(Color.get_LightBlue());
					Process.Start("https://wiki.guildwars2.com" + link);
				});
			}
			for (int i = 0; i < this.item.Count; i++)
			{
				Panel val3 = new Panel();
				((Control)val3).set_Parent((Container)(object)panel);
				((Control)val3).set_Width(((Container)panel).get_ContentRegion().Width);
				((Container)val3).set_HeightSizingMode((SizingMode)1);
				Panel innerPannel = val3;
				Label val4 = new Label();
				((Control)val4).set_Parent((Container)(object)innerPannel);
				((Control)val4).set_Width((int)Math.Floor(0.15 * (double)((Container)innerPannel).get_ContentRegion().Width));
				val4.set_Text(columns[i]);
				val4.set_WrapText(true);
				val4.set_AutoSizeHeight(true);
				Label label = val4;
				Control control = achievementTableEntryProvider.GetTableEntryControl(this.item[i]);
				if (control != null)
				{
					control.set_Parent((Container)(object)innerPannel);
					control.set_Width(((Control)innerPannel).get_Width() - ((Control)label).get_Width() - 15);
					control.set_Location(new Point(((Control)label).get_Width(), 0));
				}
			}
		}
	}
}
