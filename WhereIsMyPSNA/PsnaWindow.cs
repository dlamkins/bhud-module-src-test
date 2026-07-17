using System;
using System.Globalization;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;

namespace WhereIsMyPSNA
{
	public class PsnaWindow : TabbedWindow2
	{
		private const int LeftOffset = 58;

		private const int RightMargin = 20;

		private const int WindowRegionWidth = 913;

		private const int ContentWidth = 835;

		private readonly PsnaDataService _dataService;

		private readonly CommunitySubmissionService _submissionService;

		private readonly SettingEntry<bool> _hideKnownNpcs;

		private readonly Tab _todayTab;

		private readonly Tab _communityTab;

		private TodayLocationsView _activeTodayView;

		private CommunitySubmitView _activeCommunityView;

		public PsnaWindow(ContentsManager contentsManager, Gw2ApiManager apiManager, DirectoriesManager directoriesManager, SettingEntry<bool> hideKnownNpcs)
			: this(contentsManager.GetTexture("window_bg.png"), new Rectangle(40, 26, 913, 691), new Rectangle(98, 40, 835, 636))
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Expected O, but got Unknown
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Expected O, but got Unknown
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Expected O, but got Unknown
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Expected O, but got Unknown
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((WindowBase2)this).set_Title("Where Is My PSNA");
			((WindowBase2)this).set_Emblem(contentsManager.GetTexture("window_emblem.png"));
			((WindowBase2)this).set_SavesPosition(true);
			((WindowBase2)this).set_Id("PsnaWindow_com.odizinne.whereismypsna_38d37290-b5f9-447d-97ea-45b0b50e5f56");
			_submissionService = new CommunitySubmissionService(directoriesManager);
			_dataService = new PsnaDataService(apiManager, _submissionService);
			_hideKnownNpcs = hideKnownNpcs;
			AsyncTexture2D todayIcon = new AsyncTexture2D(contentsManager.GetTexture("pact.png"));
			AsyncTexture2D commuIcon = new AsyncTexture2D(contentsManager.GetTexture("commu.png"));
			_todayTab = new Tab(todayIcon, (Func<IView>)CreateTodayView, Strings.Get("Tab_TodayLocations"), (int?)0);
			_communityTab = new Tab(commuIcon, (Func<IView>)CreateCommunityView, Strings.Get("Tab_Community"), (int?)100);
			((TabbedWindow2)this).get_Tabs().Add(_todayTab);
			((TabbedWindow2)this).get_Tabs().Add(_communityTab);
			((TabbedWindow2)this).set_SelectedTab(_todayTab);
			((WindowBase2)this).set_Subtitle(_todayTab.get_Name());
			((TabbedWindow2)this).add_TabChanged((EventHandler<ValueChangedEventArgs<Tab>>)delegate
			{
				Tab selectedTab = ((TabbedWindow2)this).get_SelectedTab();
				((WindowBase2)this).set_Subtitle((selectedTab != null) ? selectedTab.get_Name() : null);
			});
			GameService.Overlay.add_UserLocaleChanged((EventHandler<ValueEventArgs<CultureInfo>>)OnUserLocaleChanged);
		}

		private void OnUserLocaleChanged(object sender, ValueEventArgs<CultureInfo> e)
		{
			_todayTab.set_Name(Strings.Get("Tab_TodayLocations"));
			_communityTab.set_Name(Strings.Get("Tab_Community"));
			Tab selectedTab = ((TabbedWindow2)this).get_SelectedTab();
			((WindowBase2)this).set_Subtitle((selectedTab != null) ? selectedTab.get_Name() : null);
			if (((Control)this).get_Visible() && ((TabbedWindow2)this).get_SelectedTab() != null)
			{
				((WindowBase2)this).ShowView(((TabbedWindow2)this).get_SelectedTab().get_View()());
			}
		}

		protected override void DisposeControl()
		{
			GameService.Overlay.remove_UserLocaleChanged((EventHandler<ValueEventArgs<CultureInfo>>)OnUserLocaleChanged);
			((WindowBase2)this).DisposeControl();
		}

		private IView CreateTodayView()
		{
			return (IView)(object)new TodayLocationsView(_dataService, _hideKnownNpcs, 835, RegisterActiveView, UnregisterActiveView);
		}

		private IView CreateCommunityView()
		{
			return (IView)(object)new CommunitySubmitView(_submissionService, 835, RegisterActiveCommunityView, UnregisterActiveCommunityView);
		}

		private void RegisterActiveView(TodayLocationsView view)
		{
			_activeTodayView = view;
		}

		private void UnregisterActiveView(TodayLocationsView view)
		{
			if (_activeTodayView == view)
			{
				_activeTodayView = null;
			}
		}

		private void RegisterActiveCommunityView(CommunitySubmitView view)
		{
			_activeCommunityView = view;
		}

		private void UnregisterActiveCommunityView(CommunitySubmitView view)
		{
			if (_activeCommunityView == view)
			{
				_activeCommunityView = null;
			}
		}

		public void ToggleWindow()
		{
			bool wasVisible = ((Control)this).get_Visible();
			((WindowBase2)this).ToggleWindow();
			if (((Control)this).get_Visible() && !wasVisible)
			{
				bool num = ((TabbedWindow2)this).get_SelectedTab() != _todayTab;
				((TabbedWindow2)this).set_SelectedTab(_todayTab);
				_todayTab.set_Name(Strings.Get("Tab_TodayLocations"));
				_communityTab.set_Name(Strings.Get("Tab_Community"));
				((WindowBase2)this).set_Subtitle(((TabbedWindow2)this).get_SelectedTab().get_Name());
				if (!num)
				{
					((WindowBase2)this).ShowView(((TabbedWindow2)this).get_SelectedTab().get_View()());
				}
			}
			else if (!((Control)this).get_Visible())
			{
				_activeTodayView?.OnWindowHidden();
				_activeCommunityView?.OnWindowHidden();
			}
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			((TabbedWindow2)this).UpdateContainer(gameTime);
			_activeTodayView?.Tick(gameTime);
			_activeCommunityView?.Tick(gameTime);
		}
	}
}
