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

		private ProfitWindow _profitWindow;

		public const string SUMMARY_TAB_TITLE = "Summary";

		private const string TIMELINE_TAB_TITLE = "Timeline";

		private const string FILTER_TAB_TITLE = "Filter";

		private const string SORT_TAB_TITLE = "Sort Items";

		public const string IGNORED_ITEMS_TAB_TITLE = "Ignored Items";

		private const string SETTINGS_TAB_TITLE = "Settings";

		private const string DEBUG_TAB_TITLE = "Debug";

		public FarmingTrackerWindow(int windowWidth, int windowHeight, Model model, Services services)
			: this(services.TextureService.WindowBackgroundTexture, new Rectangle(20, 26, windowWidth, windowHeight), new Rectangle(80, 20, windowWidth - 80, windowHeight - 20))
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_015e: Expected O, but got Unknown
			//IL_0189: Unknown result type (might be due to invalid IL or missing references)
			//IL_0193: Expected O, but got Unknown
			//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f6: Expected O, but got Unknown
			//IL_022b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0235: Expected O, but got Unknown
			//IL_026a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0274: Expected O, but got Unknown
			//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b3: Expected O, but got Unknown
			FarmingTrackerWindow farmingTrackerWindow = this;
			((WindowBase2)this).set_Title("Farming Tracker");
			((WindowBase2)this).set_Emblem(services.TextureService.WindowEmblemTexture);
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
			_profitWindow = new ProfitWindow(services);
			SummaryTabView summaryTabView = new SummaryTabView(this, _profitWindow, model, services);
			_summaryTabView = summaryTabView;
			_summaryTab = new Tab(AsyncTexture2D.op_Implicit(services.TextureService.SummaryTabIconTexture), (Func<IView>)(() => (IView)(object)summaryTabView), "Summary", (int?)null);
			_settingsTab = new Tab(services.TextureService.SettingsTabIconTexture, (Func<IView>)(() => (IView)(object)new SettingsTabView(services)), "Settings", (int?)null);
			((TabbedWindow2)this).get_Tabs().Add(_summaryTab);
			((TabbedWindow2)this).get_Tabs().Add(new Tab(AsyncTexture2D.op_Implicit(services.TextureService.TimelineTabIconTexture), (Func<IView>)(() => (IView)(object)new PlaceholderTabView("Timeline")), "Timeline", (int?)null));
			((TabbedWindow2)this).get_Tabs().Add(new Tab(AsyncTexture2D.op_Implicit(services.TextureService.FilterTabIconTexture), (Func<IView>)(() => (IView)(object)new FilterTabView(services)), "Filter", (int?)null));
			((TabbedWindow2)this).get_Tabs().Add(new Tab(AsyncTexture2D.op_Implicit(services.TextureService.SortTabIconTexture), (Func<IView>)(() => (IView)(object)new SortTabView(services)), "Sort Items", (int?)null));
			((TabbedWindow2)this).get_Tabs().Add(new Tab(AsyncTexture2D.op_Implicit(services.TextureService.IgnoredItemsTabIconTexture), (Func<IView>)(() => (IView)(object)new IgnoredItemsTabView(model, services)), "Ignored Items", (int?)null));
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

		public void ShowWindowAndSelectSettingsTab()
		{
			((Control)this).Show();
			((TabbedWindow2)this).set_SelectedTab(_settingsTab);
		}

		public void ToggleWindowAndSelectSummaryTab()
		{
			if (((Control)this).get_Visible())
			{
				((Control)this).Hide();
				return;
			}
			((TabbedWindow2)this).set_SelectedTab(_summaryTab);
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
