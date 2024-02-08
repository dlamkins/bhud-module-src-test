using System;
using System.Collections.Generic;
using System.Timers;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using MysticCrafting.Module.Items;
using MysticCrafting.Module.Menu;
using MysticCrafting.Module.Overview.Loading;
using MysticCrafting.Module.Services;
using MysticCrafting.Module.Services.Recurring;
using MysticCrafting.Module.Strings;

namespace MysticCrafting.Module.Overview
{
	public class MainView : View<IMainViewPresenter>
	{
		private TextBox _searchBar;

		private Panel _menuPanel;

		private LoadingStatusView _loadingStatusView;

		private Timer _searchTimer;

		public CategoryMenu Menu { get; set; }

		public ItemListView ItemList { get; set; }

		public ViewContainer ItemListContainer { get; set; }

		public string NameFilter
		{
			get
			{
				return ItemList?.ItemFilter.NameContainsText ?? string.Empty;
			}
			set
			{
				if (ItemList?.ItemFilter != null)
				{
					ItemList.ItemFilter.NameContainsText = value;
				}
			}
		}

		public string SearchText
		{
			get
			{
				return _searchBar.Text;
			}
			set
			{
				if (_searchBar != null)
				{
					_searchBar.Text = value;
				}
			}
		}

		public MainView()
		{
			WithPresenter(new MainViewPresenter(this, new MainViewModel(), ServiceContainer.MenuItemRepository));
		}

		protected override void Build(Container buildPanel)
		{
			BuildSearchBar(buildPanel);
			BuildItemList(buildPanel);
			BuildMenu(buildPanel);
			BuildLoadingStatusView(buildPanel);
		}

		private void BuildItemList(Container parent)
		{
			ItemListContainer = new ViewContainer
			{
				Parent = parent,
				Size = new Point(parent.ContentRegion.Width - Panel.MenuStandard.Size.X, Panel.MenuStandard.Size.Y + 60),
				Location = new Point(Panel.MenuStandard.Size.X, 0),
				ShowBorder = true
			};
			ItemList = new ItemListView(null);
			ItemListContainer.Show(ItemList);
		}

		private void BuildMenu(Container parent)
		{
			_menuPanel = new Panel
			{
				Title = Common.MenuTitle,
				ShowBorder = true,
				Width = Panel.MenuStandard.Size.X,
				Height = Panel.MenuStandard.Size.Y + 80 - _searchBar.Height - 10,
				Location = new Point(0, _searchBar.Height + 10),
				Parent = parent,
				CanScroll = true
			};
			Menu = new CategoryMenu
			{
				Size = _menuPanel.ContentRegion.Size,
				MenuItemHeight = 40,
				CanSelect = true,
				Parent = _menuPanel
			};
			base.Presenter.InitializeMenu();
		}

		public void Menu_ItemClicked(object sender, ControlActivatedEventArgs e)
		{
			CategoryMenuItem menuItem = sender as CategoryMenuItem;
			if (menuItem != null)
			{
				base.Presenter.GoToMenuItem(menuItem);
			}
		}

		public void BuildLoadingStatusView(Container parent)
		{
			ViewContainer obj = new ViewContainer
			{
				Parent = parent,
				Size = new Point(300, 60),
				Location = new Point(parent.ContentRegion.Width - 160, parent.ContentRegion.Height - 60)
			};
			_loadingStatusView = new LoadingStatusView(new List<IRecurringService>
			{
				ServiceContainer.TradingPostService,
				ServiceContainer.PlayerItemService,
				ServiceContainer.WalletService
			});
			obj.Show(_loadingStatusView);
		}

		private void BuildSearchBar(Container parent)
		{
			_searchTimer = new Timer(500.0);
			_searchBar = new TextBox
			{
				Left = 5,
				PlaceholderText = Common.SearchBarPlaceholder,
				Parent = parent
			};
			_searchTimer.Elapsed += _searchTimer_Elapsed;
			_searchBar.TextChanged += _searchBar_TextChanged;
		}

		private void _searchTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			_searchTimer.Stop();
			base.Presenter.SearchAsync(_searchBar.Text);
		}

		private void _searchBar_TextChanged(object sender, EventArgs e)
		{
			_searchTimer.Stop();
			_searchTimer.Start();
		}
	}
}
