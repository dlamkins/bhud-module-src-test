using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using MysticCrafting.Models.Items;
using MysticCrafting.Module.Discovery.Home;
using MysticCrafting.Module.Discovery.ItemList;
using MysticCrafting.Module.Discovery.Loading;
using MysticCrafting.Module.Menu;
using MysticCrafting.Module.Services;
using MysticCrafting.Module.Services.Recurring;
using MysticCrafting.Module.Strings;

namespace MysticCrafting.Module.Discovery
{
	public class DiscoveryTabView : View<IDiscoveryTabPresenter>
	{
		private TextBox _searchBar;

		private Panel _menuPanel;

		private LoadingStatusView _loadingStatusView;

		private Timer _searchTimer;

		public CategoryMenu Menu { get; set; }

		public ItemListView ItemList { get; set; }

		public ItemListModel ItemListModel { get; set; }

		public ViewContainer ItemListContainer { get; set; }

		public HomeView HomeView { get; set; }

		public string NameFilter
		{
			get
			{
				return ItemListModel?.Filter.NameContainsText ?? string.Empty;
			}
			set
			{
				if (ItemListModel?.Filter != null)
				{
					ItemListModel.Filter.NameContainsText = value;
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

		public event EventHandler<ControlActivatedEventArgs> MenuItemSelected;

		public DiscoveryTabView()
		{
			WithPresenter(new DiscoveryTabPresenter(this, new DiscoveryTabMenuModel(ServiceContainer.MenuItemRepository)));
		}

		protected override void Build(Container buildPanel)
		{
			BuildSearchBar(buildPanel);
			BuildItemList(buildPanel);
			BuildMenu(buildPanel);
			BuildLoadingStatusView(buildPanel);
			BuildHomeView();
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
			ReloadItemList(new ItemListModel(ServiceContainer.ItemRepository));
		}

		public void ReloadItemList(ItemListModel model)
		{
			ItemListContainer.ClearChildren();
			string rarity = ItemListModel?.Filter.Rarity;
			bool? collectionOption1 = ItemListModel?.Filter.HideSkinUnlocked;
			bool? collectionOption2 = ItemListModel?.Filter.HideSkinUnlocked;
			bool? collectionOption3 = ItemListModel?.Filter.HideMaxItemsCollected;
			WeightClass? weightClass = ItemListModel?.Filter.Weight;
			string legendaryType = ItemListModel?.Filter.LegendaryType;
			ItemListModel = model;
			if (ItemListModel.Filter == null)
			{
				ItemListModel.Filter = new MysticItemFilter();
			}
			ItemListModel.Filter.Rarity = rarity;
			ItemListModel.Filter.HideSkinUnlocked = collectionOption1.GetValueOrDefault();
			ItemListModel.Filter.HideSkinUnlocked = collectionOption2.GetValueOrDefault();
			ItemListModel.Filter.HideMaxItemsCollected = collectionOption3.GetValueOrDefault();
			ItemListModel.Filter.Weight = weightClass.GetValueOrDefault();
			ItemListModel.Filter.LegendaryType = legendaryType ?? string.Empty;
			ItemList = new ItemListView(ItemListModel);
			ItemListContainer.Show(ItemList);
		}

		public void BuildHomeView()
		{
			if (HomeView == null)
			{
				HomeView = new HomeView(new HomeViewModel(), base.Presenter);
				HomeView homeView = HomeView;
				homeView.LightClick = (EventHandler)Delegate.Combine(homeView.LightClick, (EventHandler)delegate
				{
					ItemListModel.Filter.Weight = WeightClass.Light;
					ItemListModel.Filter.Rarity = "Legendary";
					ItemListModel.Filter.LegendaryType = "Obsidian (PvE)";
					GoToArmorMenuItem();
				});
				HomeView homeView2 = HomeView;
				homeView2.MediumClick = (EventHandler)Delegate.Combine(homeView2.MediumClick, (EventHandler)delegate
				{
					ItemListModel.Filter.Weight = WeightClass.Medium;
					ItemListModel.Filter.Rarity = "Legendary";
					ItemListModel.Filter.LegendaryType = "Obsidian (PvE)";
					GoToArmorMenuItem();
				});
				HomeView homeView3 = HomeView;
				homeView3.HeavyClick = (EventHandler)Delegate.Combine(homeView3.HeavyClick, (EventHandler)delegate
				{
					ItemListModel.Filter.Weight = WeightClass.Heavy;
					ItemListModel.Filter.Rarity = "Legendary";
					ItemListModel.Filter.LegendaryType = "Obsidian (PvE)";
					GoToArmorMenuItem();
				});
			}
		}

		private void GoToArmorMenuItem()
		{
			Menu.Children.OfType<CategoryMenuItem>().FirstOrDefault((CategoryMenuItem c) => c.ItemFilter.Type == "Armor")?.Select();
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
			Menu.ItemSelected += delegate(object sender, ControlActivatedEventArgs e)
			{
				this.MenuItemSelected?.Invoke(sender, e);
			};
		}

		public void SetMenuItems(IList<CategoryMenuItem> menuItems)
		{
			if (Menu == null)
			{
				return;
			}
			Menu.ClearChildren();
			foreach (CategoryMenuItem menuItem in menuItems)
			{
				menuItem.Parent = Menu;
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
				ServiceContainer.PlayerUnlocksService,
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
