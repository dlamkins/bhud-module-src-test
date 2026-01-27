using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Modules.Managers;
using Microsoft.Xna.Framework;

namespace EmoteTome
{
	public class FavoriteBar : StandardWindow
	{
		private ContentsManager contents;

		private Module module;

		private List<Emote> coreEmoteList;

		private List<Emote> unlockEmoteList;

		private List<Emote> rankEmoteList;

		private FlowPanel mainPanel;

		private ContextMenuStrip coreMenu;

		private ContextMenuStrip unlockMenu;

		private ContextMenuStrip rankMenu;

		public Checkbox targetCheckbox;

		public Checkbox synchronCheckbox;

		public FavoriteBar(Module module, ContentsManager contents, List<Emote> coreEmoteList, List<Emote> unlockEmoteList, List<Emote> rankEmoteList)
			: this(contents.GetTexture("WindowBackground.png"), new Rectangle(40, 26, 913, 750), new Rectangle(40, 26, 913, 750))
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_015a: Expected O, but got Unknown
			//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01db: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0205: Expected O, but got Unknown
			//IL_0206: Unknown result type (might be due to invalid IL or missing references)
			//IL_020b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0212: Unknown result type (might be due to invalid IL or missing references)
			//IL_021a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0224: Unknown result type (might be due to invalid IL or missing references)
			//IL_0230: Expected O, but got Unknown
			//IL_025d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0262: Unknown result type (might be due to invalid IL or missing references)
			//IL_0269: Unknown result type (might be due to invalid IL or missing references)
			//IL_026b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0270: Unknown result type (might be due to invalid IL or missing references)
			//IL_027c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0286: Unknown result type (might be due to invalid IL or missing references)
			//IL_028b: Unknown result type (might be due to invalid IL or missing references)
			//IL_029a: Expected O, but got Unknown
			//IL_029b: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02af: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c5: Expected O, but got Unknown
			//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0301: Unknown result type (might be due to invalid IL or missing references)
			//IL_0306: Unknown result type (might be due to invalid IL or missing references)
			//IL_0312: Unknown result type (might be due to invalid IL or missing references)
			//IL_031c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0321: Unknown result type (might be due to invalid IL or missing references)
			//IL_0330: Expected O, but got Unknown
			//IL_0331: Unknown result type (might be due to invalid IL or missing references)
			//IL_0336: Unknown result type (might be due to invalid IL or missing references)
			//IL_033d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0345: Unknown result type (might be due to invalid IL or missing references)
			//IL_034f: Unknown result type (might be due to invalid IL or missing references)
			//IL_035b: Expected O, but got Unknown
			//IL_03ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_03bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_03df: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0410: Unknown result type (might be due to invalid IL or missing references)
			//IL_041c: Expected O, but got Unknown
			//IL_0439: Unknown result type (might be due to invalid IL or missing references)
			//IL_043e: Unknown result type (might be due to invalid IL or missing references)
			//IL_045e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0465: Unknown result type (might be due to invalid IL or missing references)
			//IL_046f: Unknown result type (might be due to invalid IL or missing references)
			//IL_048f: Unknown result type (might be due to invalid IL or missing references)
			//IL_049b: Expected O, but got Unknown
			FavoriteBar favoriteBar = this;
			this.contents = contents;
			this.module = module;
			this.coreEmoteList = coreEmoteList;
			this.unlockEmoteList = unlockEmoteList;
			this.rankEmoteList = rankEmoteList;
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)this).set_Size(new Point(730, 190));
			((Control)this).set_Location(new Point(680, 950));
			((WindowBase2)this).set_SavesPosition(true);
			((WindowBase2)this).set_SavesSize(true);
			((WindowBase2)this).set_CanResize(true);
			((WindowBase2)this).set_Title("");
			FlowPanel val = new FlowPanel();
			((Panel)val).set_ShowBorder(false);
			((Control)val).set_Size(new Point(((Container)this).get_ContentRegion().Width, ((Container)this).get_ContentRegion().Height));
			((Control)val).set_Location(new Point(0, 35));
			val.set_FlowDirection((ControlFlowDirection)2);
			((Control)val).set_Parent((Container)(object)this);
			val.set_ControlPadding(new Vector2(15f, 5f));
			val.set_OuterControlPadding(new Vector2(15f, 5f));
			mainPanel = val;
			module.createEmoteContainer(coreEmoteList, (Panel)(object)mainPanel, "core", fav: true);
			module.createEmoteContainer(unlockEmoteList, (Panel)(object)mainPanel, "unlock", fav: true);
			module.createEmoteContainer(rankEmoteList, (Panel)(object)mainPanel, "rank", fav: true);
			Image val2 = new Image(AsyncTexture2D.op_Implicit(GameService.Content.GetTexture("155052")));
			((Control)val2).set_Parent((Container)(object)this);
			Rectangle contentRegion = ((Container)this).get_ContentRegion();
			((Control)val2).set_Location(new Point(((Rectangle)(ref contentRegion)).get_Left() + 10, 0));
			((Control)val2).set_Size(new Point(32, 32));
			Image coreMenuImage = val2;
			ContextMenuStrip val3 = new ContextMenuStrip();
			((Control)val3).set_Parent((Container)(object)this);
			((Control)val3).set_Size(new Point(150, 50));
			((Control)val3).set_Visible(false);
			coreMenu = val3;
			((Container)coreMenu).ClearChildren();
			generateEmoteMenus(coreMenu, coreEmoteList);
			Image val4 = new Image(AsyncTexture2D.op_Implicit(GameService.Content.GetTexture("155052")));
			((Control)val4).set_Parent((Container)(object)this);
			contentRegion = ((Container)this).get_ContentRegion();
			((Control)val4).set_Location(new Point(((Rectangle)(ref contentRegion)).get_Left() + 50, 0));
			((Control)val4).set_Size(new Point(32, 32));
			Image unlockMenuImage = val4;
			ContextMenuStrip val5 = new ContextMenuStrip();
			((Control)val5).set_Parent((Container)(object)this);
			((Control)val5).set_Size(new Point(150, 50));
			((Control)val5).set_Visible(false);
			unlockMenu = val5;
			((Container)unlockMenu).ClearChildren();
			generateEmoteMenus(unlockMenu, unlockEmoteList);
			Image val6 = new Image(AsyncTexture2D.op_Implicit(GameService.Content.GetTexture("155052")));
			((Control)val6).set_Parent((Container)(object)this);
			contentRegion = ((Container)this).get_ContentRegion();
			((Control)val6).set_Location(new Point(((Rectangle)(ref contentRegion)).get_Left() + 90, 0));
			((Control)val6).set_Size(new Point(32, 32));
			Image rankMenuImage = val6;
			ContextMenuStrip val7 = new ContextMenuStrip();
			((Control)val7).set_Parent((Container)(object)this);
			((Control)val7).set_Size(new Point(150, 50));
			((Control)val7).set_Visible(false);
			rankMenu = val7;
			((Container)rankMenu).ClearChildren();
			generateEmoteMenus(rankMenu, rankEmoteList);
			((Control)coreMenuImage).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				favoriteBar.coreMenu.Show((Control)(object)coreMenuImage);
			});
			((Control)unlockMenuImage).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				favoriteBar.unlockMenu.Show((Control)(object)unlockMenuImage);
			});
			((Control)rankMenuImage).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				favoriteBar.rankMenu.Show((Control)(object)rankMenuImage);
			});
			Checkbox val8 = new Checkbox();
			val8.set_Text(BadLocalization.TARGETCHECKBOXTEXT[module.language]);
			((Control)val8).set_Location(new Point(140, 5));
			((Control)val8).set_BasicTooltipText(BadLocalization.TARGETCHECKBOXTOOLTIP[module.language]);
			((Control)val8).set_Parent((Container)(object)this);
			targetCheckbox = val8;
			targetCheckbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				module.setTargetCheckbox();
				module.targetCheckbox.set_Checked(favoriteBar.targetCheckbox.get_Checked());
			});
			Checkbox val9 = new Checkbox();
			val9.set_Text(BadLocalization.SYNCHRONCHECKBOXTEXT[module.language]);
			((Control)val9).set_Location(new Point(320, 5));
			((Control)val9).set_BasicTooltipText(BadLocalization.SYNCHRONCHECKBOXTOOLTIP[module.language]);
			((Control)val9).set_Parent((Container)(object)this);
			synchronCheckbox = val9;
			synchronCheckbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				module.synchronCheckbox.set_Checked(favoriteBar.synchronCheckbox.get_Checked());
			});
			((Control)this).add_Resized((EventHandler<ResizedEventArgs>)delegate
			{
				//IL_0011: Unknown result type (might be due to invalid IL or missing references)
				((Control)favoriteBar.mainPanel).set_Width(((Container)favoriteBar).get_ContentRegion().Width);
				((Control)favoriteBar).set_Height(190);
			});
		}

		protected override Point HandleWindowResize(Point newSize)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			Point screen = ((Control)GameService.Graphics.get_SpriteScreen()).get_Size();
			return new Point(MathHelper.Clamp(newSize.X, 600, screen.X), MathHelper.Clamp(newSize.Y, 190, 190));
		}

		private void generateEmoteMenus(ContextMenuStrip menu, List<Emote> emoteList)
		{
			foreach (Emote emote in emoteList)
			{
				ContextMenuStripItem menuitem = menu.AddMenuItem(emote.getToolTipp()[module.language]);
				if (module.getFavoriteList().Contains(emote.getToolTipp()[module.language]))
				{
					menuitem.set_Text(emote.getToolTipp()[module.language] + " *");
					((Control)emote.getFavContainer()).set_Visible(true);
				}
				((Control)menuitem).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					if (module.getFavoriteList().Contains(emote.getToolTipp()[module.language]))
					{
						menuitem.set_Text(emote.getToolTipp()[module.language]);
						module.removeFromFavoriteSetting(emote.getToolTipp()[module.language]);
						((Control)emote.getFavContainer()).set_Visible(false);
					}
					else
					{
						menuitem.set_Text(emote.getToolTipp()[module.language] + " *");
						module.addToFavoriteSetting(emote.getToolTipp()[module.language]);
						((Control)emote.getFavContainer()).set_Visible(true);
					}
					((Control)mainPanel).RecalculateLayout();
				});
			}
			((Control)mainPanel).RecalculateLayout();
		}

		public void unload()
		{
		}
	}
}
