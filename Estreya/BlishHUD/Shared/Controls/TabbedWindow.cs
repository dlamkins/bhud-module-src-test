using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Estreya.BlishHUD.Shared.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Estreya.BlishHUD.Shared.Controls
{
	public class TabbedWindow : Window, ITabOwner
	{
		private const int TAB_VERTICALOFFSET = 40;

		private const int TAB_HEIGHT = 50;

		private const int TAB_WIDTH = 84;

		private static readonly Texture2D _textureTabActive = Control.get_Content().GetTexture("window-tab-active");

		private Tab _selectedTab;

		private Tab HoveredTab { get; set; }

		public TabCollection Tabs { get; }

		public Tab SelectedTab
		{
			get
			{
				return _selectedTab;
			}
			set
			{
				Tab selectedTab = _selectedTab;
				if ((value == null || Tabs.Contains(value)) && ((Control)this).SetProperty<Tab>(ref _selectedTab, value, true, "SelectedTab"))
				{
					OnTabChanged(new ValueChangedEventArgs<Tab>(selectedTab, value));
				}
			}
		}

		public event EventHandler<ValueChangedEventArgs<Tab>> TabChanged;

		public TabbedWindow(BaseModuleSettings baseModuleSettings, AsyncTexture2D background, Rectangle windowRegion, Rectangle contentRegion)
			: base(baseModuleSettings)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			Tabs = new TabCollection((ITabOwner)(object)this);
			base.ShowSideBar = true;
			ConstructWindow(background, windowRegion, contentRegion);
		}

		public TabbedWindow(BaseModuleSettings baseModuleSettings, Texture2D background, Rectangle windowRegion, Rectangle contentRegion)
			: this(baseModuleSettings, AsyncTexture2D.op_Implicit(background), windowRegion, contentRegion)
		{
		}//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)


		public TabbedWindow(BaseModuleSettings baseModuleSettings, AsyncTexture2D background, Rectangle windowRegion, Rectangle contentRegion, Point windowSize)
			: base(baseModuleSettings)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			Tabs = new TabCollection((ITabOwner)(object)this);
			base.ShowSideBar = true;
			ConstructWindow(background, windowRegion, contentRegion, windowSize);
		}

		public TabbedWindow(BaseModuleSettings baseModuleSettings, Texture2D background, Rectangle windowRegion, Rectangle contentRegion, Point windowSize)
			: this(baseModuleSettings, AsyncTexture2D.op_Implicit(background), windowRegion, contentRegion, windowSize)
		{
		}//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)


		protected virtual void OnTabChanged(ValueChangedEventArgs<Tab> e)
		{
			Tab newValue = e.get_NewValue();
			SetView((newValue != null) ? newValue.get_View()() : null);
			if (((Control)this).get_Visible() && e.get_PreviousValue() != null)
			{
				Control.get_Content().PlaySoundEffectByName($"tab-swap-{RandomUtil.GetRandom(1, 5)}");
			}
			this.TabChanged?.Invoke(this, e);
		}

		protected override void OnClick(MouseEventArgs e)
		{
			Tab hoveredTab = HoveredTab;
			if (hoveredTab != null && hoveredTab.get_Enabled())
			{
				SelectedTab = HoveredTab;
			}
			base.OnClick(e);
		}

		private void UpdateTabStates()
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			base.SideBarHeight = 40 + 50 * Tabs.get_Count();
			object hoveredTab;
			if (((Control)this).get_MouseOver())
			{
				Rectangle sidebarActiveBounds = base.SidebarActiveBounds;
				if (((Rectangle)(ref sidebarActiveBounds)).Contains(((Control)this).get_RelativeMousePosition()))
				{
					hoveredTab = Tabs.FromIndex((((Control)this).get_RelativeMousePosition().Y - base.SidebarActiveBounds.Y - 40) / 50);
					goto IL_0061;
				}
			}
			hoveredTab = null;
			goto IL_0061;
			IL_0061:
			HoveredTab = (Tab)hoveredTab;
			Tab hoveredTab2 = HoveredTab;
			((Control)this).set_BasicTooltipText((hoveredTab2 != null) ? hoveredTab2.get_Name() : null);
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			UpdateTabStates();
			base.UpdateContainer(gameTime);
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
			base.PaintAfterChildren(spriteBatch, bounds);
			int num = 0;
			Rectangle destinationRectangle = default(Rectangle);
			foreach (Tab tab in Tabs)
			{
				Rectangle val = base.SidebarActiveBounds;
				int y = ((Rectangle)(ref val)).get_Top() + 40 + num * 50;
				bool flag = tab == SelectedTab;
				bool hovered = tab == HoveredTab;
				if (flag)
				{
					val = base.SidebarActiveBounds;
					((Rectangle)(ref destinationRectangle))._002Ector(((Rectangle)(ref val)).get_Left() - (84 - base.SidebarActiveBounds.Width) + 2, y, 84, 50);
					Texture2D obj = AsyncTexture2D.op_Implicit(base.WindowBackground);
					Rectangle val2 = destinationRectangle;
					val = base.WindowRegion;
					int num2 = ((Rectangle)(ref val)).get_Left() + destinationRectangle.X;
					int y2 = destinationRectangle.Y;
					Thickness padding = ((Control)this).get_Padding();
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, obj, val2, (Rectangle?)new Rectangle(num2, y2 - (int)((Thickness)(ref padding)).get_Top(), destinationRectangle.Width, destinationRectangle.Height));
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _textureTabActive, destinationRectangle);
				}
				tab.Draw((Control)(object)this, spriteBatch, new Rectangle(base.SidebarActiveBounds.X, y, base.SidebarActiveBounds.Width, 50), flag, hovered);
				num++;
			}
		}
	}
}
