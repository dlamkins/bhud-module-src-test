using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Atzie.MysticCrafting.Models.Items;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using MysticCrafting.Models.Items;
using MysticCrafting.Module.Discovery.Home;
using MysticCrafting.Module.Discovery.ItemList;
using MysticCrafting.Module.Discovery.ItemList.Controls;
using MysticCrafting.Module.Discovery.Loading;
using MysticCrafting.Module.Discovery.Version;
using MysticCrafting.Module.Menu;
using MysticCrafting.Module.RecipeTree;
using MysticCrafting.Module.RecipeTree.TreeView;
using MysticCrafting.Module.Services;
using MysticCrafting.Module.Services.API;
using MysticCrafting.Module.Strings;

namespace MysticCrafting.Module.Discovery
{
	public class DiscoveryTabView : View<IDiscoveryTabPresenter>
	{
		private TextBox _searchBar;

		private Panel _menuPanel;

		private LoadingStatusView _loadingStatusView;

		private VersionIndicatorView _versionIndicatorView;

		private readonly MysticCraftingModule _module;

		private Container BuildPanel;

		private ViewContainer RecipeViewContainer;

		private Panel DiscoveryOverviewContainer;

		private RecipeDetailsView RecipeView;

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
				return ((TextInputBase)_searchBar).get_Text();
			}
			set
			{
				if (_searchBar != null)
				{
					((TextInputBase)_searchBar).set_Text(value);
				}
			}
		}

		public event EventHandler<ControlActivatedEventArgs> MenuItemSelected;

		public DiscoveryTabView(MysticCraftingModule module)
		{
			_module = module;
			base.WithPresenter((IDiscoveryTabPresenter)new DiscoveryTabPresenter(this, new DiscoveryTabMenuModel(ServiceContainer.MenuItemRepository)));
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Expected O, but got Unknown
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Expected O, but got Unknown
			BuildPanel = buildPanel;
			ViewContainer val = new ViewContainer();
			((Control)val).set_Parent(buildPanel);
			((Control)val).set_Size(new Point(((Control)buildPanel).get_Width(), ((Control)buildPanel).get_Height() - 113));
			((Control)val).set_Visible(false);
			RecipeViewContainer = val;
			Panel val2 = new Panel();
			((Control)val2).set_Parent(buildPanel);
			((Control)val2).set_Size(new Point(((Control)buildPanel).get_Width() - 20, ((Control)buildPanel).get_Height() - 60));
			val2.set_ShowBorder(false);
			val2.set_ShowTint(false);
			DiscoveryOverviewContainer = val2;
			BuildSearchBar((Container)(object)DiscoveryOverviewContainer);
			BuildItemList((Container)(object)DiscoveryOverviewContainer);
			BuildMenu((Container)(object)DiscoveryOverviewContainer);
			BuildLoadingStatusView(BuildPanel);
			BuildVersionIndicatorView(BuildPanel);
		}

		private void BuildItemList(Container parent)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Expected O, but got Unknown
			ViewContainer val = new ViewContainer();
			((Control)val).set_Parent(parent);
			((Control)val).set_Size(new Point(parent.get_ContentRegion().Width - ((DesignStandard)(ref Panel.MenuStandard)).get_Size().X, ((DesignStandard)(ref Panel.MenuStandard)).get_Size().Y + 60));
			((Control)val).set_Location(new Point(((DesignStandard)(ref Panel.MenuStandard)).get_Size().X, 0));
			((Panel)val).set_ShowBorder(true);
			ItemListContainer = val;
			ReloadItemList(new ItemListModel(ServiceContainer.ItemRepository));
		}

		public void ReloadItemList(ItemListModel model)
		{
			((Container)ItemListContainer).ClearChildren();
			ItemRarity? rarity = ItemListModel?.Filter.Rarity;
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
			ItemListModel.Filter.Rarity = rarity.GetValueOrDefault();
			ItemListModel.Filter.HideSkinUnlocked = collectionOption1.GetValueOrDefault();
			ItemListModel.Filter.HideSkinUnlocked = collectionOption2.GetValueOrDefault();
			ItemListModel.Filter.HideMaxItemsCollected = collectionOption3.GetValueOrDefault();
			ItemListModel.Filter.Weight = weightClass.GetValueOrDefault();
			ItemListModel.Filter.LegendaryType = legendaryType ?? string.Empty;
			ItemList = new ItemListView(ItemListModel);
			ItemListView itemList = ItemList;
			itemList.ItemClicked = (EventHandler<EventArgs>)Delegate.Combine(itemList.ItemClicked, new EventHandler<EventArgs>(ItemClicked));
			ItemListContainer.Show((IView)(object)ItemList);
		}

		private void ItemClicked(object sender, EventArgs e)
		{
			RecipeDetailsView recipeView = RecipeView;
			if (recipeView != null)
			{
				TreeView treeView = recipeView.TreeView;
				if (treeView != null)
				{
					((Control)treeView).Dispose();
				}
			}
			ItemButton listItem = sender as ItemButton;
			if (listItem != null)
			{
				IList<string> breadCrumbs = ((View<IItemListPresenter>)ItemList)?.get_Presenter()?.BuildBreadcrumbs();
				RecipeView = new RecipeDetailsView(listItem.Item.Id, breadCrumbs ?? new List<string>());
				RecipeDetailsView recipeView2 = RecipeView;
				recipeView2.OnBackButtonClick = (EventHandler<MouseEventArgs>)Delegate.Combine(recipeView2.OnBackButtonClick, (EventHandler<MouseEventArgs>)delegate
				{
					((Control)RecipeViewContainer).Hide();
					((Control)DiscoveryOverviewContainer).Show();
				});
				((Control)DiscoveryOverviewContainer).Hide();
				((Control)RecipeViewContainer).Show();
				RecipeViewContainer.Show((IView)(object)RecipeView);
			}
		}

		public void BuildHomeView()
		{
			if (HomeView == null)
			{
				HomeView = new HomeView(new HomeViewModel(), base.get_Presenter());
				HomeView homeView = HomeView;
				homeView.LightClick = (EventHandler)Delegate.Combine(homeView.LightClick, (EventHandler)delegate
				{
					ItemListModel.Filter.Weight = WeightClass.Light;
					ItemListModel.Filter.Rarity = ItemRarity.Legendary;
					ItemListModel.Filter.LegendaryType = "Obsidian (PvE)";
					GoToArmorMenuItem();
				});
				HomeView homeView2 = HomeView;
				homeView2.MediumClick = (EventHandler)Delegate.Combine(homeView2.MediumClick, (EventHandler)delegate
				{
					ItemListModel.Filter.Weight = WeightClass.Medium;
					ItemListModel.Filter.Rarity = ItemRarity.Legendary;
					ItemListModel.Filter.LegendaryType = "Obsidian (PvE)";
					GoToArmorMenuItem();
				});
				HomeView homeView3 = HomeView;
				homeView3.HeavyClick = (EventHandler)Delegate.Combine(homeView3.HeavyClick, (EventHandler)delegate
				{
					ItemListModel.Filter.Weight = WeightClass.Heavy;
					ItemListModel.Filter.Rarity = ItemRarity.Legendary;
					ItemListModel.Filter.LegendaryType = "Obsidian (PvE)";
					GoToArmorMenuItem();
				});
			}
		}

		private void GoToArmorMenuItem()
		{
			((IEnumerable)((Container)Menu).get_Children()).OfType<CategoryMenuItem>().FirstOrDefault((CategoryMenuItem c) => c.ItemFilter.Type == ItemType.Armor)?.Select();
		}

		private void BuildMenu(Container parent)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Expected O, but got Unknown
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			Panel val = new Panel();
			val.set_Title(Common.MenuTitle);
			val.set_ShowBorder(true);
			((Control)val).set_Size(new Point(((DesignStandard)(ref Panel.MenuStandard)).get_Size().X, ((DesignStandard)(ref Panel.MenuStandard)).get_Size().Y + 50 - ((Control)_searchBar).get_Height()));
			((Control)val).set_Location(new Point(0, ((Control)_searchBar).get_Height() + 10));
			((Control)val).set_Parent(parent);
			val.set_CanScroll(true);
			_menuPanel = val;
			CategoryMenu categoryMenu = new CategoryMenu();
			Rectangle contentRegion = ((Container)_menuPanel).get_ContentRegion();
			((Control)categoryMenu).set_Size(((Rectangle)(ref contentRegion)).get_Size());
			categoryMenu.MenuItemHeight = 40;
			categoryMenu.CanSelect = true;
			((Control)categoryMenu).set_Parent((Container)(object)_menuPanel);
			Menu = categoryMenu;
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
			((Container)Menu).ClearChildren();
			foreach (CategoryMenuItem menuItem in menuItems)
			{
				((Control)menuItem).set_Parent((Container)(object)Menu);
			}
		}

		public void BuildLoadingStatusView(Container parent)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			ViewContainer val = new ViewContainer();
			((Control)val).set_Parent(parent);
			((Control)val).set_Size(new Point(320, 60));
			((Control)val).set_Location(new Point(parent.get_ContentRegion().Width - 180, parent.get_ContentRegion().Height - 60));
			_loadingStatusView = new LoadingStatusView(new List<IApiService>
			{
				ServiceContainer.TradingPostService,
				ServiceContainer.PlayerItemService,
				ServiceContainer.PlayerUnlocksService,
				ServiceContainer.WalletService
			});
			val.Show((IView)(object)_loadingStatusView);
		}

		private void BuildVersionIndicatorView(Container parent)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			ViewContainer val = new ViewContainer();
			((Control)val).set_Parent(parent);
			((Control)val).set_Size(new Point(150, 60));
			((Control)val).set_Location(new Point(0, parent.get_ContentRegion().Height - 60));
			_versionIndicatorView = new VersionIndicatorView();
			val.Show((IView)(object)_versionIndicatorView);
		}

		private void BuildSearchBar(Container parent)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Expected O, but got Unknown
			_searchTimer = new Timer(500.0);
			TextBox val = new TextBox();
			((Control)val).set_Left(5);
			((TextInputBase)val).set_PlaceholderText(Common.SearchBarPlaceholder);
			((Control)val).set_Parent(parent);
			_searchBar = val;
			_searchTimer.Elapsed += _searchTimer_Elapsed;
			((TextInputBase)_searchBar).add_TextChanged((EventHandler<EventArgs>)_searchBar_TextChanged);
		}

		private void _searchTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			_searchTimer.Stop();
			base.get_Presenter().SearchAsync(((TextInputBase)_searchBar).get_Text());
		}

		private void _searchBar_TextChanged(object sender, EventArgs e)
		{
			_searchTimer.Stop();
			_searchTimer.Start();
		}
	}
}
