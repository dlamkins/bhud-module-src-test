using System;
using System.Collections.Generic;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Kenedia.Modules.Core.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.Core.Controls
{
	public class TabbedPanel : AnchoredContainer
	{
		protected readonly FlowPanel TabsButtonPanel;

		private PanelTab _activeTab;

		public List<PanelTab> Tabs { get; }

		public PanelTab ActiveTab
		{
			get
			{
				return _activeTab;
			}
			set
			{
				SwitchTab(value);
			}
		}

		private event EventHandler TabAdded;

		private event EventHandler TabRemoved;

		public TabbedPanel()
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			FlowPanel flowPanel = new FlowPanel();
			((FlowPanel)flowPanel).set_FlowDirection((ControlFlowDirection)2);
			((Container)flowPanel).set_WidthSizingMode((SizingMode)2);
			((FlowPanel)flowPanel).set_ControlPadding(new Vector2(1f, 0f));
			((Control)flowPanel).set_Height(25);
			TabsButtonPanel = flowPanel;
			Tabs = new List<PanelTab>();
			base._002Ector();
			((Control)TabsButtonPanel).set_Parent((Container)(object)this);
			((Control)TabsButtonPanel).add_Resized((EventHandler<ResizedEventArgs>)OnTabButtonPanelResized);
			((Container)this).set_HeightSizingMode((SizingMode)1);
			base.BackgroundImageColor = Color.get_Honeydew() * 0.95f;
		}

		public void AddTab(PanelTab tab)
		{
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			((Control)tab).set_Parent((Container)(object)this);
			((Control)tab).add_Disposed((EventHandler<EventArgs>)OnTabDisposed);
			((Control)tab.TabButton).set_Parent((Container)(object)TabsButtonPanel);
			((Control)tab.TabButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				TabButton_Click(tab);
			});
			((Control)tab).set_Location(new Point(0, ((Control)TabsButtonPanel).get_Bottom()));
			Tabs.Add(tab);
			this.TabAdded?.Invoke(this, EventArgs.Empty);
			if (ActiveTab == null)
			{
				PanelTab panelTab2 = (ActiveTab = tab);
			}
			((Control)this).RecalculateLayout();
		}

		public void RemoveTab(PanelTab tab)
		{
			((Control)tab).remove_Disposed((EventHandler<EventArgs>)OnTabDisposed);
			((Control)tab.TabButton).remove_Click((EventHandler<MouseEventArgs>)delegate
			{
				TabButton_Click(tab);
			});
			((Control)tab).set_Parent((Container)null);
			((Control)tab.TabButton).set_Parent((Container)null);
			Tabs.Remove(tab);
			this.TabRemoved?.Invoke(this, EventArgs.Empty);
			((Control)this).RecalculateLayout();
		}

		public override void RecalculateLayout()
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			base.RecalculateLayout();
			int button_amount = Math.Max(1, ((Container)TabsButtonPanel).get_Children().get_Count());
			int width = (((Control)TabsButtonPanel).get_Width() - (button_amount - 1) * (int)((FlowPanel)TabsButtonPanel).get_ControlPadding().X) / button_amount;
			foreach (Control child in ((Container)TabsButtonPanel).get_Children())
			{
				child.set_Width(width);
			}
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			base.PaintBeforeChildren(spriteBatch, bounds);
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			base.OnResized(e);
		}

		public virtual bool SwitchTab(PanelTab tab = null)
		{
			foreach (PanelTab t in Tabs)
			{
				if (t != tab)
				{
					t.Active = false;
				}
			}
			if (tab != null)
			{
				tab.Active = true;
			}
			_activeTab = tab;
			return false;
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
			((IEnumerable<IDisposable>)Tabs).DisposeAll();
		}

		private void OnTabButtonPanelResized(object sender, ResizedEventArgs e)
		{
			((Control)this).RecalculateLayout();
		}

		private void TabButton_Click(PanelTab t)
		{
			SwitchTab(t);
		}

		private void OnTabDisposed(object sender, EventArgs e)
		{
			RemoveTab((PanelTab)sender);
		}
	}
}
