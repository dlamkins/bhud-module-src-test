using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DecorBlishhudModule.CustomControls.CustomTab
{
	public class CustomTabbedWindow2 : WindowBase2, ICustomTabOwner
	{
		private const int TAB_VERTICALOFFSET = 80;

		private const int TAB_HEIGHT = 50;

		private const int TAB_WIDTH = 80;

		private const int TAB_GAP = 40;

		private static readonly Texture2D _textureTabActive = Control.get_Content().GetTexture("window-tab-active");

		private CustomTab _selectedTabGroup1;

		private CustomTab _selectedTabGroup2;

		private CustomTab _selectedTabGroup3;

		public CustomTabCollection TabsGroup1 { get; }

		public CustomTabCollection TabsGroup2 { get; }

		public CustomTabCollection TabsGroup3 { get; }

		public CustomTab SelectedTabGroup1
		{
			get
			{
				return _selectedTabGroup1;
			}
			set
			{
				CustomTab previousTab = _selectedTabGroup1;
				if ((value == null || TabsGroup1.Contains(value)) && ((Control)this).SetProperty<CustomTab>(ref _selectedTabGroup1, value, true, "SelectedTabGroup1"))
				{
					OnTabChanged(new ValueChangedEventArgs<CustomTab>(previousTab, value));
				}
			}
		}

		public CustomTab SelectedTabGroup2
		{
			get
			{
				return _selectedTabGroup2;
			}
			set
			{
				CustomTab previousTab = _selectedTabGroup2;
				if ((value == null || TabsGroup2.Contains(value)) && ((Control)this).SetProperty<CustomTab>(ref _selectedTabGroup2, value, true, "SelectedTabGroup2"))
				{
					OnTabChanged(new ValueChangedEventArgs<CustomTab>(previousTab, value));
				}
			}
		}

		public CustomTab SelectedTabGroup3
		{
			get
			{
				return _selectedTabGroup3;
			}
			set
			{
				CustomTab previousTab = _selectedTabGroup3;
				if ((value == null || TabsGroup3.Contains(value)) && ((Control)this).SetProperty<CustomTab>(ref _selectedTabGroup3, value, true, "SelectedTabGroup3"))
				{
					OnTabChanged(new ValueChangedEventArgs<CustomTab>(previousTab, value));
				}
			}
		}

		private CustomTab HoveredTab { get; set; }

		public event EventHandler<ValueChangedEventArgs<CustomTab>> TabChanged;

		protected virtual void OnTabChanged(ValueChangedEventArgs<CustomTab> e)
		{
			if (((Control)this).get_Visible() && e.get_PreviousValue() != null)
			{
				Control.get_Content().PlaySoundEffectByName($"tab-swap-{RandomUtil.GetRandom(1, 5)}");
			}
			this.TabChanged?.Invoke(this, e);
		}

		public CustomTabbedWindow2(AsyncTexture2D background, Rectangle windowRegion, Rectangle contentRegion, Point windowSize)
			: this()
		{
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			TabsGroup1 = new CustomTabCollection(this);
			TabsGroup2 = new CustomTabCollection(this);
			TabsGroup3 = new CustomTabCollection(this);
			((WindowBase2)this).set_ShowSideBar(true);
			((WindowBase2)this).ConstructWindow(background, windowRegion, contentRegion, windowSize);
		}

		public CustomTabbedWindow2(Texture2D background, Rectangle windowRegion, Rectangle contentRegion, Point windowSize)
			: this(AsyncTexture2D.op_Implicit(background), windowRegion, contentRegion, windowSize)
		{
		}//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)


		protected override void OnClick(MouseEventArgs e)
		{
			if (HoveredTab != null && HoveredTab.Enabled)
			{
				if (TabsGroup1.Contains(HoveredTab))
				{
					SelectedTabGroup1 = HoveredTab;
					SelectedTabGroup2 = ((SelectedTabGroup2 == null) ? TabsGroup2.FromIndex(0) : SelectedTabGroup2);
					SelectedTabGroup3 = null;
				}
				else if (TabsGroup2.Contains(HoveredTab))
				{
					SelectedTabGroup1 = ((SelectedTabGroup1 == null) ? TabsGroup1.FromIndex(0) : SelectedTabGroup2);
					SelectedTabGroup2 = HoveredTab;
					SelectedTabGroup3 = null;
				}
				else if (TabsGroup3.Contains(HoveredTab))
				{
					SelectedTabGroup1 = null;
					SelectedTabGroup2 = null;
					SelectedTabGroup3 = HoveredTab;
				}
			}
			((WindowBase2)this).OnClick(e);
		}

		private void UpdateTabStates()
		{
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			((WindowBase2)this).set_SideBarHeight(80 + 50 * TabsGroup1.Count + 50 * TabsGroup2.Count + 50 * TabsGroup3.Count + 80);
			object hoveredTab;
			if (((Control)this).get_MouseOver())
			{
				Rectangle sidebarActiveBounds = ((WindowBase2)this).get_SidebarActiveBounds();
				if (((Rectangle)(ref sidebarActiveBounds)).Contains(((Control)this).get_RelativeMousePosition()))
				{
					int y = ((Control)this).get_RelativeMousePosition().Y;
					sidebarActiveBounds = ((WindowBase2)this).get_SidebarActiveBounds();
					hoveredTab = TabsFromPosition(y - ((Rectangle)(ref sidebarActiveBounds)).get_Top());
					goto IL_007a;
				}
			}
			hoveredTab = null;
			goto IL_007a;
			IL_007a:
			HoveredTab = (CustomTab)hoveredTab;
			((Control)this).set_BasicTooltipText(HoveredTab?.Name);
		}

		private CustomTab TabsFromPosition(int yPosition)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			int tabIndex = 0;
			Rectangle sidebarActiveBounds;
			foreach (CustomTab tab3 in TabsGroup1)
			{
				sidebarActiveBounds = ((WindowBase2)this).get_SidebarActiveBounds();
				int tabTop3 = ((Rectangle)(ref sidebarActiveBounds)).get_Top() + 80 + tabIndex * 50;
				if (yPosition + 40 >= tabTop3 && yPosition + 40 <= tabTop3 + 50)
				{
					return tab3;
				}
				tabIndex++;
			}
			foreach (CustomTab tab2 in TabsGroup2)
			{
				sidebarActiveBounds = ((WindowBase2)this).get_SidebarActiveBounds();
				int tabTop2 = ((Rectangle)(ref sidebarActiveBounds)).get_Top() + 80 + 40 + tabIndex * 50;
				if (yPosition + 40 >= tabTop2 && yPosition + 40 <= tabTop2 + 50)
				{
					return tab2;
				}
				tabIndex++;
			}
			foreach (CustomTab tab in TabsGroup3)
			{
				sidebarActiveBounds = ((WindowBase2)this).get_SidebarActiveBounds();
				int tabTop = ((Rectangle)(ref sidebarActiveBounds)).get_Top() + 80 + 80 + tabIndex * 50;
				if (yPosition + 40 >= tabTop && yPosition + 40 <= tabTop + 50)
				{
					return tab;
				}
				tabIndex++;
			}
			return null;
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			UpdateTabStates();
			((WindowBase2)this).UpdateContainer(gameTime);
		}

		public override void PaintAfterChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0182: Unknown result type (might be due to invalid IL or missing references)
			//IL_0187: Unknown result type (might be due to invalid IL or missing references)
			//IL_0193: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0211: Unknown result type (might be due to invalid IL or missing references)
			//IL_021b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0228: Unknown result type (might be due to invalid IL or missing references)
			//IL_0234: Unknown result type (might be due to invalid IL or missing references)
			//IL_0275: Unknown result type (might be due to invalid IL or missing references)
			//IL_027a: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0303: Unknown result type (might be due to invalid IL or missing references)
			//IL_030b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0310: Unknown result type (might be due to invalid IL or missing references)
			//IL_031b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0322: Unknown result type (might be due to invalid IL or missing references)
			//IL_0329: Unknown result type (might be due to invalid IL or missing references)
			//IL_033f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0349: Unknown result type (might be due to invalid IL or missing references)
			//IL_0356: Unknown result type (might be due to invalid IL or missing references)
			//IL_0362: Unknown result type (might be due to invalid IL or missing references)
			((WindowBase2)this).PaintAfterChildren(spriteBatch, bounds);
			int tabIndex = 0;
			Rectangle val;
			Rectangle destinationRectangle = default(Rectangle);
			Thickness padding;
			foreach (CustomTab item in TabsGroup1)
			{
				val = ((WindowBase2)this).get_SidebarActiveBounds();
				int y3 = ((Rectangle)(ref val)).get_Top() + 80 + tabIndex * 50;
				bool isSelected3 = item == SelectedTabGroup1;
				bool isHovered3 = item == HoveredTab;
				if (isSelected3)
				{
					val = ((WindowBase2)this).get_SidebarActiveBounds();
					((Rectangle)(ref destinationRectangle))._002Ector(((Rectangle)(ref val)).get_Left() - (80 - ((WindowBase2)this).get_SidebarActiveBounds().Width) + 2, y3, 80, 50);
					Texture2D obj = AsyncTexture2D.op_Implicit(((WindowBase2)this).get_WindowBackground());
					Rectangle val2 = destinationRectangle;
					val = ((WindowBase2)this).get_WindowRegion();
					int num = ((Rectangle)(ref val)).get_Left() + destinationRectangle.X + 20;
					int y4 = destinationRectangle.Y;
					padding = ((Control)this).get_Padding();
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, obj, val2, (Rectangle?)new Rectangle(num, y4 - (int)((Thickness)(ref padding)).get_Top(), destinationRectangle.Width, destinationRectangle.Height));
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _textureTabActive, destinationRectangle);
				}
				item.Draw((Control)(object)this, spriteBatch, new Rectangle(((WindowBase2)this).get_SidebarActiveBounds().X, y3, ((WindowBase2)this).get_SidebarActiveBounds().Width, 50), isSelected3, isHovered3);
				tabIndex++;
			}
			Rectangle destinationRectangle2 = default(Rectangle);
			foreach (CustomTab item2 in TabsGroup2)
			{
				val = ((WindowBase2)this).get_SidebarActiveBounds();
				int y2 = ((Rectangle)(ref val)).get_Top() + 80 + 40 + tabIndex * 50;
				bool isSelected2 = item2 == SelectedTabGroup2;
				bool isHovered2 = item2 == HoveredTab;
				if (isSelected2)
				{
					val = ((WindowBase2)this).get_SidebarActiveBounds();
					((Rectangle)(ref destinationRectangle2))._002Ector(((Rectangle)(ref val)).get_Left() - (80 - ((WindowBase2)this).get_SidebarActiveBounds().Width) + 2, y2, 80, 50);
					Texture2D obj2 = AsyncTexture2D.op_Implicit(((WindowBase2)this).get_WindowBackground());
					Rectangle val3 = destinationRectangle2;
					val = ((WindowBase2)this).get_WindowRegion();
					int num2 = ((Rectangle)(ref val)).get_Left() + destinationRectangle2.X + 20;
					int y5 = destinationRectangle2.Y;
					padding = ((Control)this).get_Padding();
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, obj2, val3, (Rectangle?)new Rectangle(num2, y5 - (int)((Thickness)(ref padding)).get_Top(), destinationRectangle2.Width, destinationRectangle2.Height));
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _textureTabActive, destinationRectangle2);
				}
				item2.Draw((Control)(object)this, spriteBatch, new Rectangle(((WindowBase2)this).get_SidebarActiveBounds().X, y2, ((WindowBase2)this).get_SidebarActiveBounds().Width, 50), isSelected2, isHovered2);
				tabIndex++;
			}
			Rectangle destinationRectangle3 = default(Rectangle);
			foreach (CustomTab item3 in TabsGroup3)
			{
				val = ((WindowBase2)this).get_SidebarActiveBounds();
				int y = ((Rectangle)(ref val)).get_Top() + 80 + 80 + tabIndex * 50;
				bool isSelected = item3 == SelectedTabGroup3;
				bool isHovered = item3 == HoveredTab;
				if (isSelected)
				{
					val = ((WindowBase2)this).get_SidebarActiveBounds();
					((Rectangle)(ref destinationRectangle3))._002Ector(((Rectangle)(ref val)).get_Left() - (80 - ((WindowBase2)this).get_SidebarActiveBounds().Width) + 2, y, 80, 50);
					Texture2D obj3 = AsyncTexture2D.op_Implicit(((WindowBase2)this).get_WindowBackground());
					Rectangle val4 = destinationRectangle3;
					val = ((WindowBase2)this).get_WindowRegion();
					int num3 = ((Rectangle)(ref val)).get_Left() + destinationRectangle3.X + 20;
					int y6 = destinationRectangle3.Y;
					padding = ((Control)this).get_Padding();
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, obj3, val4, (Rectangle?)new Rectangle(num3, y6 - (int)((Thickness)(ref padding)).get_Top(), destinationRectangle3.Width, destinationRectangle3.Height));
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _textureTabActive, destinationRectangle3);
				}
				item3.Draw((Control)(object)this, spriteBatch, new Rectangle(((WindowBase2)this).get_SidebarActiveBounds().X, y, ((WindowBase2)this).get_SidebarActiveBounds().Width, 50), isSelected, isHovered);
				tabIndex++;
			}
		}
	}
}
