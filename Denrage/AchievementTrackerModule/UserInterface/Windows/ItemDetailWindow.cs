using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Modules.Managers;
using Denrage.AchievementTrackerModule.Interfaces;
using Denrage.AchievementTrackerModule.Libs.Achievement;
using Denrage.AchievementTrackerModule.UserInterface.Controls.FormattedLabel;
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

		private readonly ISubPageInformationWindowManager subPageInformationWindowManager;

		private readonly string achievementLink;

		private readonly string name;

		private readonly string[] columns;

		private readonly List<CollectionAchievementTable.CollectionAchievementTableEntry> item;

		private readonly Texture2D texture;

		public ItemDetailWindow(ContentsManager contentsManager, IAchievementService achievementService, IAchievementTableEntryProvider achievementTableEntryProvider, ISubPageInformationWindowManager subPageInformationWindowManager, string achievementLink, string name, string[] columns, List<CollectionAchievementTable.CollectionAchievementTableEntry> item)
			: this()
		{
			this.contentsManager = contentsManager;
			this.achievementService = achievementService;
			this.achievementTableEntryProvider = achievementTableEntryProvider;
			this.subPageInformationWindowManager = subPageInformationWindowManager;
			this.achievementLink = achievementLink;
			texture = this.contentsManager.GetTexture("item_detail_background.png");
			this.name = name;
			this.columns = columns;
			this.item = item;
			BuildWindow();
		}

		private void BuildWindow()
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Expected O, but got Unknown
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01de: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0204: Expected O, but got Unknown
			//IL_0204: Unknown result type (might be due to invalid IL or missing references)
			//IL_0209: Unknown result type (might be due to invalid IL or missing references)
			//IL_0211: Unknown result type (might be due to invalid IL or missing references)
			//IL_021d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0234: Unknown result type (might be due to invalid IL or missing references)
			//IL_0243: Unknown result type (might be due to invalid IL or missing references)
			//IL_024a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0253: Expected O, but got Unknown
			//IL_029d: Unknown result type (might be due to invalid IL or missing references)
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
			CollectionAchievementTable.CollectionAchievementTableItemEntry item = this.item.OfType<CollectionAchievementTable.CollectionAchievementTableItemEntry>().FirstOrDefault();
			string link = achievementLink;
			if (item != null)
			{
				link = item.Link;
			}
			FormattedLabelBuilder itemTitleBuilder = new FormattedLabelBuilder().SetWidth(((Container)panel).get_ContentRegion().Width).AutoSizeHeight().Wrap();
			FormattedLabelPartBuilder itemTitlePart = itemTitleBuilder.CreatePart(name);
			itemTitlePart.SetFontSize((FontSize)18);
			if (!string.IsNullOrEmpty(link))
			{
				bool inSubpages = false;
				foreach (SubPageInformation subPage in achievementService.Subpages)
				{
					if (subPage.Link == "https://wiki.guildwars2.com" + link)
					{
						inSubpages = true;
						itemTitlePart.SetLink(delegate
						{
							subPageInformationWindowManager.Create(subPage);
						}).MakeUnderlined();
					}
				}
				if (!inSubpages)
				{
					if (link.StartsWith("/"))
					{
						link = "https://wiki.guildwars2.com/" + link;
					}
					itemTitlePart.SetHyperLink(link).MakeUnderlined();
				}
			}
			((Control)itemTitleBuilder.CreatePart(itemTitlePart).Build()).set_Parent((Container)(object)panel);
			for (int i = 0; i < this.item.Count; i++)
			{
				Panel val2 = new Panel();
				((Control)val2).set_Parent((Container)(object)panel);
				((Control)val2).set_Width(((Container)panel).get_ContentRegion().Width);
				((Container)val2).set_HeightSizingMode((SizingMode)1);
				Panel innerPannel = val2;
				Label val3 = new Label();
				((Control)val3).set_Parent((Container)(object)innerPannel);
				((Control)val3).set_Width((int)Math.Floor(0.15 * (double)((Container)innerPannel).get_ContentRegion().Width));
				val3.set_Text(columns[i]);
				val3.set_WrapText(true);
				val3.set_AutoSizeHeight(true);
				Label label = val3;
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
