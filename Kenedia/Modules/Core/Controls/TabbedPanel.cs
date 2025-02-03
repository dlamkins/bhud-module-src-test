using System;
using System.Collections.Generic;
using Blish_HUD.Controls;
using Kenedia.Modules.Core.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.Core.Controls
{
	public class TabbedPanel : AnchoredContainer
	{
		protected readonly FlowPanel TabsButtonPanel = new FlowPanel
		{
			FlowDirection = ControlFlowDirection.SingleLeftToRight,
			WidthSizingMode = SizingMode.Fill,
			ControlPadding = new Vector2(1f, 0f),
			Height = 25
		};

		private PanelTab _activeTab;

		public List<PanelTab> Tabs { get; } = new List<PanelTab>();


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
			TabsButtonPanel.Parent = this;
			TabsButtonPanel.Resized += OnTabButtonPanelResized;
			HeightSizingMode = SizingMode.AutoSize;
			base.BackgroundImageColor = Color.get_Honeydew() * 0.95f;
		}

		public void AddTab(PanelTab tab)
		{
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			PanelTab tab2 = tab;
			tab2.Parent = this;
			tab2.Disposed += OnTabDisposed;
			tab2.TabButton.Parent = TabsButtonPanel;
			tab2.TabButton.Click += delegate
			{
				TabButton_Click(tab2);
			};
			tab2.Location = new Point(0, TabsButtonPanel.Bottom);
			Tabs.Add(tab2);
			this.TabAdded?.Invoke(this, EventArgs.Empty);
			if (ActiveTab == null)
			{
				PanelTab panelTab2 = (ActiveTab = tab2);
			}
			RecalculateLayout();
		}

		public void RemoveTab(PanelTab tab)
		{
			PanelTab tab2 = tab;
			tab2.Disposed -= OnTabDisposed;
			tab2.TabButton.Click -= delegate
			{
				TabButton_Click(tab2);
			};
			tab2.Parent = null;
			tab2.TabButton.Parent = null;
			Tabs.Remove(tab2);
			this.TabRemoved?.Invoke(this, EventArgs.Empty);
			RecalculateLayout();
		}

		public override void RecalculateLayout()
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			base.RecalculateLayout();
			int button_amount = Math.Max(1, TabsButtonPanel.Children.Count);
			int width = (TabsButtonPanel.Width - (button_amount - 1) * (int)TabsButtonPanel.ControlPadding.X) / button_amount;
			foreach (Control child in TabsButtonPanel.Children)
			{
				child.Width = width;
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
			Tabs.DisposeAll();
		}

		private void OnTabButtonPanelResized(object sender, ResizedEventArgs e)
		{
			RecalculateLayout();
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
