using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.GameServices;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using MysticCrafting.Module.Discovery.ItemList;
using MysticCrafting.Module.Discovery.ItemList.Controls;
using MysticCrafting.Module.Discovery.Loading;
using MysticCrafting.Module.Discovery.Menu;
using MysticCrafting.Module.Discovery.Version;
using MysticCrafting.Module.Extensions;
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
		private static readonly Logger Logger = Logger.GetLogger<DiscoveryTabView>();

		private TextBox _searchBar;

		private Panel _menuPanel;

		private LoadingStatusView _loadingStatusView;

		private VersionIndicatorView _versionIndicatorView;

		private readonly MysticCraftingModule _module;

		private Container _buildPanel;

		private ViewContainer _recipeViewContainer;

		private Panel _discoveryOverviewContainer;

		private RecipeDetailsView _recipeView;

		private FlowPanel _leftSidePanel;

		private readonly List<MenuPanel> _menuPanels = new List<MenuPanel>();

		private SkinsMenuPanel _skinsPanel;

		private SourcesMenuPanel _sourcesPanel;

		private RaritiesMenuPanel _raritiesPanel;

		private WeightClassesMenuPanel _weightClassesPanel;

		private Timer _searchTimer;

		public CategoryMenu Menu { get; set; }

		public ItemListView ItemList { get; set; }

		public ItemListModel ItemListModel { get; set; }

		public ViewContainer ItemListContainer { get; set; }

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
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Expected O, but got Unknown
			_buildPanel = buildPanel;
			ViewContainer val = new ViewContainer();
			((Control)val).set_Parent(buildPanel);
			((Control)val).set_Size(new Point(((Control)buildPanel).get_Width(), ((Control)buildPanel).get_Height() - 113));
			((Control)val).set_Visible(false);
			_recipeViewContainer = val;
			Panel val2 = new Panel();
			((Control)val2).set_Parent(buildPanel);
			((Control)val2).set_Size(new Point(((Control)buildPanel).get_Width() - 20, ((Control)buildPanel).get_Height() - 60));
			val2.set_ShowBorder(false);
			val2.set_ShowTint(false);
			_discoveryOverviewContainer = val2;
			try
			{
				BuildSearchBar((Container)(object)_discoveryOverviewContainer);
				BuildItemList((Container)(object)_discoveryOverviewContainer);
				FlowPanel val3 = new FlowPanel();
				((Control)val3).set_Parent((Container)(object)_discoveryOverviewContainer);
				((Control)val3).set_Size(new Point(((DesignStandard)(ref Panel.MenuStandard)).get_Size().X, ((DesignStandard)(ref Panel.MenuStandard)).get_Size().Y + 50 - ((Control)_searchBar).get_Height()));
				((Control)val3).set_Location(new Point(0, ((Control)_searchBar).get_Height()));
				val3.set_FlowDirection((ControlFlowDirection)6);
				_leftSidePanel = val3;
				BuildRaritiesPanel((Container)(object)_leftSidePanel);
				BuildSourcesPanel((Container)(object)_leftSidePanel);
				BuildWeightClassesPanel((Container)(object)_leftSidePanel);
				BuildMenu((Container)(object)_leftSidePanel);
				BuildLoadingStatusView(_buildPanel);
				BuildVersionIndicatorView(_buildPanel);
			}
			catch (Exception ex)
			{
				Logger.Error("Loading discovery view failed with error: " + ex.Message + ". Stacktrace: " + ex.StackTrace);
			}
		}

		public void ResizeMenu()
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			if (_menuPanel != null && Menu != null)
			{
				((Control)_menuPanel).set_Height(((DesignStandard)(ref Panel.MenuStandard)).get_Size().Y - ((Control)_sourcesPanel).get_Height() - ((Control)_raritiesPanel).get_Height() - ((Control)_weightClassesPanel).get_Height());
				CategoryMenu menu = Menu;
				Rectangle contentRegion = ((Container)_menuPanel).get_ContentRegion();
				((Control)menu).set_Size(((Rectangle)(ref contentRegion)).get_Size());
			}
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
			ItemListModel = new ItemListModel(ServiceContainer.ItemRepository);
			ItemList = new ItemListView(ItemListModel);
			ItemListView itemList = ItemList;
			itemList.ItemClicked = (EventHandler<EventArgs>)Delegate.Combine(itemList.ItemClicked, new EventHandler<EventArgs>(ItemClicked));
			ItemListContainer.Show((IView)(object)ItemList);
		}

		private void ItemClicked(object sender, EventArgs e)
		{
			ItemButton listItem = sender as ItemButton;
			if (listItem == null)
			{
				return;
			}
			RecipeDetailsView recipeView = _recipeView;
			if (recipeView != null)
			{
				TreeView treeView = recipeView.TreeView;
				if (treeView != null)
				{
					((Control)treeView).Dispose();
				}
			}
			IList<string> breadCrumbs = ((View<IItemListPresenter>)ItemList)?.get_Presenter()?.BuildBreadcrumbs();
			_recipeView = new RecipeDetailsView(listItem.Item.Id, breadCrumbs ?? new List<string>());
			RecipeDetailsView recipeView2 = _recipeView;
			recipeView2.OnBackButtonClick = (EventHandler<MouseEventArgs>)Delegate.Combine(recipeView2.OnBackButtonClick, (EventHandler<MouseEventArgs>)delegate
			{
				_recipeViewContainer.Show((IView)null);
				_recipeView = null;
				((Control)_recipeViewContainer).Hide();
				((Control)_discoveryOverviewContainer).Show();
				DirectoryUtil.get_CachePath();
				((ServiceModule<ContentService>)(object)GameService.Content.get_DatAssetCache()).Unload();
			});
			((Control)_discoveryOverviewContainer).Hide();
			((Control)_recipeViewContainer).Show();
			_recipeViewContainer.Show((IView)(object)_recipeView);
		}

		private void BuildMenu(Container parent)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Expected O, but got Unknown
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			Panel val = new Panel();
			val.set_Title(Common.MenuTitle);
			val.set_ShowBorder(true);
			((Control)val).set_Size(new Point(((DesignStandard)(ref Panel.MenuStandard)).get_Size().X, ((DesignStandard)(ref Panel.MenuStandard)).get_Size().Y));
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
			ResizeMenu();
		}

		private void BuildRaritiesPanel(Container parent)
		{
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			RaritiesMenuPanel raritiesPanel = _raritiesPanel;
			if (raritiesPanel != null)
			{
				((Control)raritiesPanel).Dispose();
			}
			RaritiesMenuPanel raritiesMenuPanel = new RaritiesMenuPanel(ItemListModel);
			((Panel)raritiesMenuPanel).set_Title(MysticCrafting.Module.Strings.Discovery.RaritiesPanelHeading);
			((Panel)raritiesMenuPanel).set_ShowBorder(true);
			((Control)raritiesMenuPanel).set_Size(new Point(((DesignStandard)(ref Panel.MenuStandard)).get_Size().X, 250));
			((Control)raritiesMenuPanel).set_Parent(parent);
			((Panel)raritiesMenuPanel).set_CanCollapse(true);
			((Panel)raritiesMenuPanel).set_Collapsed(true);
			((Panel)raritiesMenuPanel).set_CanScroll(true);
			((FlowPanel)raritiesMenuPanel).set_FlowDirection((ControlFlowDirection)1);
			_raritiesPanel = raritiesMenuPanel;
			((Control)_raritiesPanel).add_Resized((EventHandler<ResizedEventArgs>)delegate
			{
				ResizeMenu();
			});
			((Control)_raritiesPanel).add_Click((EventHandler<MouseEventArgs>)PanelOnClick);
			_menuPanels.Add(_raritiesPanel);
		}

		private void BuildWeightClassesPanel(Container parent)
		{
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			WeightClassesMenuPanel weightClassesPanel = _weightClassesPanel;
			if (weightClassesPanel != null)
			{
				((Control)weightClassesPanel).Dispose();
			}
			WeightClassesMenuPanel weightClassesMenuPanel = new WeightClassesMenuPanel(ItemListModel);
			((Panel)weightClassesMenuPanel).set_Title(MysticCrafting.Module.Strings.Discovery.WeightClassesPanelHeading);
			((Panel)weightClassesMenuPanel).set_ShowBorder(true);
			((Control)weightClassesMenuPanel).set_Size(new Point(((DesignStandard)(ref Panel.MenuStandard)).get_Size().X, 235));
			((Control)weightClassesMenuPanel).set_Parent(parent);
			((Panel)weightClassesMenuPanel).set_CanCollapse(true);
			((Panel)weightClassesMenuPanel).set_Collapsed(true);
			((Panel)weightClassesMenuPanel).set_CanScroll(true);
			((FlowPanel)weightClassesMenuPanel).set_FlowDirection((ControlFlowDirection)1);
			_weightClassesPanel = weightClassesMenuPanel;
			((Control)_weightClassesPanel).add_Resized((EventHandler<ResizedEventArgs>)delegate
			{
				ResizeMenu();
			});
			((Control)_weightClassesPanel).add_Click((EventHandler<MouseEventArgs>)PanelOnClick);
			_menuPanels.Add(_weightClassesPanel);
		}

		private void BuildSourcesPanel(Container parent)
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			SourcesMenuPanel sourcesMenuPanel = new SourcesMenuPanel(ItemListModel);
			((Panel)sourcesMenuPanel).set_Title(MysticCrafting.Module.Strings.Discovery.SourcesPanelHeading);
			((Panel)sourcesMenuPanel).set_ShowBorder(true);
			((Control)sourcesMenuPanel).set_Size(new Point(((DesignStandard)(ref Panel.MenuStandard)).get_Size().X, 250));
			((Control)sourcesMenuPanel).set_Parent(parent);
			((Panel)sourcesMenuPanel).set_CanCollapse(true);
			((Panel)sourcesMenuPanel).set_Collapsed(true);
			((Panel)sourcesMenuPanel).set_CanScroll(true);
			((FlowPanel)sourcesMenuPanel).set_FlowDirection((ControlFlowDirection)1);
			_sourcesPanel = sourcesMenuPanel;
			((Control)_sourcesPanel).add_Resized((EventHandler<ResizedEventArgs>)delegate
			{
				ResizeMenu();
			});
			((Control)_sourcesPanel).add_Click((EventHandler<MouseEventArgs>)PanelOnClick);
			_menuPanels.Add(_sourcesPanel);
		}

		private void PanelOnClick(object sender, MouseEventArgs e)
		{
			foreach (MenuPanel item in _menuPanels.Where((MenuPanel p) => p != sender))
			{
				((Panel)item).Collapse();
			}
		}

		private void BuildSkinsPanel(Container parent)
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			SkinsMenuPanel skinsMenuPanel = new SkinsMenuPanel(ItemListModel);
			((Panel)skinsMenuPanel).set_Title(MysticCrafting.Module.Strings.Discovery.SkinsPanelHeading);
			((Panel)skinsMenuPanel).set_ShowBorder(true);
			((Control)skinsMenuPanel).set_Size(new Point(((DesignStandard)(ref Panel.MenuStandard)).get_Size().X, 175));
			((Control)skinsMenuPanel).set_Parent(parent);
			((Panel)skinsMenuPanel).set_CanCollapse(true);
			((Panel)skinsMenuPanel).set_Collapsed(true);
			((Panel)skinsMenuPanel).set_CanScroll(true);
			((FlowPanel)skinsMenuPanel).set_FlowDirection((ControlFlowDirection)1);
			_skinsPanel = skinsMenuPanel;
			((Control)_skinsPanel).add_Resized((EventHandler<ResizedEventArgs>)delegate
			{
				ResizeMenu();
			});
			((Control)_skinsPanel).add_Click((EventHandler<MouseEventArgs>)PanelOnClick);
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
			((Control)val).set_Size(new Point(420, 60));
			((Control)val).set_Location(new Point(parent.get_ContentRegion().Width - 280, parent.get_ContentRegion().Height - 60));
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

		protected override void Unload()
		{
			_searchTimer?.Dispose();
			TextBox searchBar = _searchBar;
			if (searchBar != null)
			{
				((Control)searchBar).Dispose();
			}
			Panel menuPanel = _menuPanel;
			if (menuPanel != null)
			{
				((Control)menuPanel).Dispose();
			}
			CategoryMenu menu = Menu;
			if (menu != null)
			{
				((Control)menu).Dispose();
			}
			((IEnumerable<Control>)_menuPanels).SafeDispose();
			FlowPanel leftSidePanel = _leftSidePanel;
			if (leftSidePanel != null)
			{
				((Control)leftSidePanel).Dispose();
			}
			ViewContainer itemListContainer = ItemListContainer;
			if (itemListContainer != null)
			{
				((Control)itemListContainer).Dispose();
			}
			ViewContainer recipeViewContainer = _recipeViewContainer;
			if (recipeViewContainer != null)
			{
				((Control)recipeViewContainer).Dispose();
			}
			Panel discoveryOverviewContainer = _discoveryOverviewContainer;
			if (discoveryOverviewContainer != null)
			{
				((Control)discoveryOverviewContainer).Dispose();
			}
			((View<IPresenter>)(object)_versionIndicatorView)?.DoUnload();
			((View<IPresenter>)(object)_loadingStatusView)?.DoUnload();
			base.Unload();
		}
	}
}
