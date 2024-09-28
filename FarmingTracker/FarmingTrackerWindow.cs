using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;

namespace FarmingTracker
{
	public class FarmingTrackerWindow : TabbedWindow2
	{
		private readonly SummaryTabView _summaryTabView;

		private readonly Tab _summaryTab;

		private readonly Tab _settingsTab;

		private readonly Tab _customStatProfitTab;

		private readonly ProfitWindow _profitWindow;

		public FarmingTrackerWindow(int windowWidth, int windowHeight, Model model, Services services)
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_015d: Expected O, but got Unknown
			//IL_0188: Unknown result type (might be due to invalid IL or missing references)
			//IL_0192: Expected O, but got Unknown
			//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cc: Expected O, but got Unknown
			//IL_0225: Unknown result type (might be due to invalid IL or missing references)
			//IL_022f: Expected O, but got Unknown
			//IL_0264: Unknown result type (might be due to invalid IL or missing references)
			//IL_026e: Expected O, but got Unknown
			//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ad: Expected O, but got Unknown
			//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fd: Expected O, but got Unknown
			Services services2 = services;
			Model model2 = model;
			((TabbedWindow2)this)._002Ector(services2.TextureService.WindowBackgroundTexture, new Rectangle(20, 26, windowWidth, windowHeight), new Rectangle(80, 20, windowWidth - 80, windowHeight - 20));
			FarmingTrackerWindow farmingTrackerWindow = this;
			((WindowBase2)this).set_Title("Farming Tracker");
			((WindowBase2)this).set_Emblem(services2.TextureService.WindowEmblemTexture);
			((WindowBase2)this).set_SavesPosition(true);
			((WindowBase2)this).set_Id("Ecksofa.FarmingTracker: FarmingTrackerWindow");
			((Control)this).set_Location(new Point(300, 300));
			((WindowBase2)this).set_CanResize(true);
			((WindowBase2)this).set_SavesSize(true);
			((Control)this).set_Width(630);
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)this).add_Resized((EventHandler<ResizedEventArgs>)delegate(object s, ResizedEventArgs e)
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				farmingTrackerWindow.ShowOrHideWindowSubtitle(e.get_CurrentSize().X);
			});
			((TabbedWindow2)this).add_TabChanged((EventHandler<ValueChangedEventArgs<Tab>>)delegate
			{
				farmingTrackerWindow.ShowOrHideWindowSubtitle(((Control)farmingTrackerWindow).get_Width());
			});
			_profitWindow = new ProfitWindow(services2);
			SummaryTabView summaryTabView = new SummaryTabView(_profitWindow, model2, services2);
			_summaryTabView = summaryTabView;
			_summaryTab = new Tab(AsyncTexture2D.op_Implicit(services2.TextureService.SummaryTabIconTexture), (Func<IView>)(() => (IView)(object)summaryTabView), "Summary", (int?)null);
			_settingsTab = new Tab(services2.TextureService.SettingsTabIconTexture, (Func<IView>)(() => (IView)(object)new SettingsTabView(services2)), "Settings", (int?)null);
			_customStatProfitTab = new Tab(AsyncTexture2D.op_Implicit(services2.TextureService.CustomStatProfitTabIconTexture), (Func<IView>)(() => (IView)(object)new CustomStatProfitTabView(model2, services2)), "Custom Profit", (int?)null);
			((TabbedWindow2)this).get_Tabs().Add(_summaryTab);
			((TabbedWindow2)this).get_Tabs().Add(new Tab(AsyncTexture2D.op_Implicit(services2.TextureService.TimelineTabIconTexture), (Func<IView>)(() => (IView)(object)new PlaceholderTabView("Timeline")), "Timeline", (int?)null));
			((TabbedWindow2)this).get_Tabs().Add(new Tab(AsyncTexture2D.op_Implicit(services2.TextureService.FilterTabIconTexture), (Func<IView>)(() => (IView)(object)new FilterTabView(services2)), "Filter", (int?)null));
			((TabbedWindow2)this).get_Tabs().Add(new Tab(AsyncTexture2D.op_Implicit(services2.TextureService.SortTabIconTexture), (Func<IView>)(() => (IView)(object)new SortTabView(services2)), "Sort Items", (int?)null));
			((TabbedWindow2)this).get_Tabs().Add(_customStatProfitTab);
			((TabbedWindow2)this).get_Tabs().Add(new Tab(AsyncTexture2D.op_Implicit(services2.TextureService.IgnoredItemsTabIconTexture), (Func<IView>)(() => (IView)(object)new IgnoredItemsTabView(model2, services2)), "Ignored Items", (int?)null));
			((TabbedWindow2)this).get_Tabs().Add(_settingsTab);
		}

		protected override void DisposeControl()
		{
			_summaryTabView?.Dispose();
			ProfitWindow profitWindow = _profitWindow;
			if (profitWindow != null)
			{
				((Control)profitWindow).Dispose();
			}
			((WindowBase2)this).DisposeControl();
		}

		public void SelectWindowTab(WindowTab windowTab, WindowVisibility windowVisibility)
		{
			if (windowVisibility == WindowVisibility.Toggle && ((Control)this).get_Visible())
			{
				((Control)this).Hide();
				return;
			}
			switch (windowTab)
			{
			case WindowTab.Summary:
				((TabbedWindow2)this).set_SelectedTab(_summaryTab);
				break;
			case WindowTab.Settings:
				((TabbedWindow2)this).set_SelectedTab(_settingsTab);
				break;
			case WindowTab.CustomProfit:
				((TabbedWindow2)this).set_SelectedTab(_customStatProfitTab);
				break;
			}
			((Control)this).Show();
		}

		public void Update2(GameTime gameTime)
		{
			_summaryTabView?.Update(gameTime);
		}

		private void ShowOrHideWindowSubtitle(int width)
		{
			((WindowBase2)this).set_Subtitle((width < 500) ? "" : ((TabbedWindow2)this).get_SelectedTab().get_Name());
		}
	}
}
