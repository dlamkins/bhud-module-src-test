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

		private const int TAB_WIDTH = 100;

		private const int TAB_GAP = 40;

		private static readonly Texture2D _textureTabActive = Control.get_Content().GetTexture("window-tab-active");

		private CustomTab _selectedTabGroup1;

		private CustomTab _selectedTabGroup2;

		public CustomTabCollection TabsGroup1 { get; }

		public CustomTabCollection TabsGroup2 { get; }

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

		public CustomTabbedWindow2(AsyncTexture2D background, Rectangle windowRegion, Rectangle contentRegion)
			: this()
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			TabsGroup1 = new CustomTabCollection(this);
			TabsGroup2 = new CustomTabCollection(this);
			((WindowBase2)this).set_ShowSideBar(true);
			((WindowBase2)this).ConstructWindow(background, windowRegion, contentRegion);
		}

		public CustomTabbedWindow2(Texture2D background, Rectangle windowRegion, Rectangle contentRegion)
			: this(AsyncTexture2D.op_Implicit(background), windowRegion, contentRegion)
		{
		}//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)


		public CustomTabbedWindow2(AsyncTexture2D background, Rectangle windowRegion, Rectangle contentRegion, Point windowSize)
			: this()
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			TabsGroup1 = new CustomTabCollection(this);
			TabsGroup2 = new CustomTabCollection(this);
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
				}
				else if (TabsGroup2.Contains(HoveredTab))
				{
					SelectedTabGroup2 = HoveredTab;
				}
			}
			((WindowBase2)this).OnClick(e);
		}

		private void UpdateTabStates()
		{
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			int totalTabHeightGroup1 = 80 + 50 * TabsGroup1.Count;
			((WindowBase2)this).set_SideBarHeight(totalTabHeightGroup1 + 50 * TabsGroup2.Count + 40);
			object hoveredTab;
			if (((Control)this).get_MouseOver())
			{
				Rectangle sidebarActiveBounds = ((WindowBase2)this).get_SidebarActiveBounds();
				if (((Rectangle)(ref sidebarActiveBounds)).Contains(((Control)this).get_RelativeMousePosition()))
				{
					int y = ((Control)this).get_RelativeMousePosition().Y;
					sidebarActiveBounds = ((WindowBase2)this).get_SidebarActiveBounds();
					hoveredTab = TabsFromPosition(y - ((Rectangle)(ref sidebarActiveBounds)).get_Top());
					goto IL_006d;
				}
			}
			hoveredTab = null;
			goto IL_006d;
			IL_006d:
			HoveredTab = (CustomTab)hoveredTab;
			((Control)this).set_BasicTooltipText(HoveredTab?.Name);
		}

		private CustomTab TabsFromPosition(int yPosition)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			int tabIndex = 0;
			Rectangle sidebarActiveBounds;
			foreach (CustomTab tab2 in TabsGroup1)
			{
				sidebarActiveBounds = ((WindowBase2)this).get_SidebarActiveBounds();
				int tabTop2 = ((Rectangle)(ref sidebarActiveBounds)).get_Top() + 80 + tabIndex * 50;
				if (yPosition + 40 >= tabTop2 && yPosition + 40 <= tabTop2 + 50)
				{
					return tab2;
				}
				tabIndex++;
			}
			foreach (CustomTab tab in TabsGroup2)
			{
				sidebarActiveBounds = ((WindowBase2)this).get_SidebarActiveBounds();
				int tabTop = ((Rectangle)(ref sidebarActiveBounds)).get_Top() + 80 + 50 * TabsGroup1.Count + 40 + (tabIndex - 2) * 50;
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
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_0149: Unknown result type (might be due to invalid IL or missing references)
			//IL_0190: Unknown result type (might be due to invalid IL or missing references)
			//IL_0195: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0206: Unknown result type (might be due to invalid IL or missing references)
			//IL_021c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0226: Unknown result type (might be due to invalid IL or missing references)
			//IL_0233: Unknown result type (might be due to invalid IL or missing references)
			//IL_023f: Unknown result type (might be due to invalid IL or missing references)
			((WindowBase2)this).PaintAfterChildren(spriteBatch, bounds);
			int tabIndex = 0;
			Rectangle val;
			Rectangle destinationRectangle = default(Rectangle);
			Thickness padding;
			foreach (CustomTab item in TabsGroup1)
			{
				val = ((WindowBase2)this).get_SidebarActiveBounds();
				int y2 = ((Rectangle)(ref val)).get_Top() + 80 + tabIndex * 50;
				bool isSelected2 = item == SelectedTabGroup1;
				bool isHovered2 = item == HoveredTab;
				if (isSelected2)
				{
					val = ((WindowBase2)this).get_SidebarActiveBounds();
					((Rectangle)(ref destinationRectangle))._002Ector(((Rectangle)(ref val)).get_Left() - (100 - ((WindowBase2)this).get_SidebarActiveBounds().Width) + 2, y2, 100, 50);
					Texture2D obj = AsyncTexture2D.op_Implicit(((WindowBase2)this).get_WindowBackground());
					Rectangle val2 = destinationRectangle;
					val = ((WindowBase2)this).get_WindowRegion();
					int num = ((Rectangle)(ref val)).get_Left() + destinationRectangle.X;
					int y3 = destinationRectangle.Y;
					padding = ((Control)this).get_Padding();
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, obj, val2, (Rectangle?)new Rectangle(num, y3 - (int)((Thickness)(ref padding)).get_Top(), destinationRectangle.Width, destinationRectangle.Height));
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _textureTabActive, destinationRectangle);
				}
				item.Draw((Control)(object)this, spriteBatch, new Rectangle(((WindowBase2)this).get_SidebarActiveBounds().X, y2, ((WindowBase2)this).get_SidebarActiveBounds().Width, 50), isSelected2, isHovered2);
				tabIndex++;
			}
			Rectangle destinationRectangle2 = default(Rectangle);
			foreach (CustomTab item2 in TabsGroup2)
			{
				val = ((WindowBase2)this).get_SidebarActiveBounds();
				int y = ((Rectangle)(ref val)).get_Top() + 80 + 50 * TabsGroup1.Count + 40 + (tabIndex - 2) * 50;
				bool isSelected = item2 == SelectedTabGroup2;
				bool isHovered = item2 == HoveredTab;
				if (isSelected)
				{
					val = ((WindowBase2)this).get_SidebarActiveBounds();
					((Rectangle)(ref destinationRectangle2))._002Ector(((Rectangle)(ref val)).get_Left() - (100 - ((WindowBase2)this).get_SidebarActiveBounds().Width) + 2, y, 100, 50);
					Texture2D obj2 = AsyncTexture2D.op_Implicit(((WindowBase2)this).get_WindowBackground());
					Rectangle val3 = destinationRectangle2;
					val = ((WindowBase2)this).get_WindowRegion();
					int num2 = ((Rectangle)(ref val)).get_Left() + destinationRectangle2.X;
					int y4 = destinationRectangle2.Y;
					padding = ((Control)this).get_Padding();
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, obj2, val3, (Rectangle?)new Rectangle(num2, y4 - (int)((Thickness)(ref padding)).get_Top(), destinationRectangle2.Width, destinationRectangle2.Height));
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _textureTabActive, destinationRectangle2);
				}
				item2.Draw((Control)(object)this, spriteBatch, new Rectangle(((WindowBase2)this).get_SidebarActiveBounds().X, y, ((WindowBase2)this).get_SidebarActiveBounds().Width, 50), isSelected, isHovered);
				tabIndex++;
			}
		}
	}
}
