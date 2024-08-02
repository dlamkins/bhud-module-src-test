using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Atzie.MysticCrafting.Models.Items;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using MysticCrafting.Module.Discovery.ItemList.Controls;
using MysticCrafting.Module.Discovery.Loading;
using MysticCrafting.Module.RecipeTree.TreeView;
using MysticCrafting.Module.RecipeTree.TreeView.Extensions;
using MysticCrafting.Module.RecipeTree.TreeView.Presenters;
using MysticCrafting.Module.Services;
using MysticCrafting.Module.Services.API;

namespace MysticCrafting.Module.RecipeTree
{
	public class RecipeDetailsView : View<IRecipeDetailsViewPresenter>
	{
		public EventHandler<MouseEventArgs> OnBackButtonClick;

		public Scrollbar _scrollBar;

		private IIngredientNodePresenter _ingredientNodePresenter;

		private IList<string> _breadcrumbs;

		private static readonly Logger Logger = Logger.GetLogger<RecipeDetailsView>();

		public int ItemId { get; set; }

		public Item Item { get; set; }

		public FlowPanel FlowPanel { get; set; }

		public Container BuildPanel { get; set; }

		public PanelHeaderButton HeaderButton { get; set; }

		public bool RecipeDetailsLoaded { get; set; }

		public Panel RecipeDetailsPanel { get; set; }

		public ViewContainer LoadingViewContainer { get; set; }

		public MysticCrafting.Module.RecipeTree.TreeView.TreeView TreeView { get; set; }

		public Scrollbar Scrollbar
		{
			get
			{
				if (_scrollBar == null)
				{
					_scrollBar = ((IEnumerable)((Container)RecipeDetailsPanel).get_Children()).OfType<Scrollbar>().FirstOrDefault();
				}
				return _scrollBar;
			}
		}

		public RecipeDetailsView(int itemId, IList<string> breadcrumbs)
		{
			ItemId = itemId;
			RecipeDetailsViewPresenter presenter = new RecipeDetailsViewPresenter(this, itemId);
			base.WithPresenter((IRecipeDetailsViewPresenter)presenter);
			_breadcrumbs = breadcrumbs;
			_ingredientNodePresenter = new IngredientNodePresenter(presenter, ServiceContainer.ChoiceRepository);
		}

		protected override void Build(Container buildPanel)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Expected O, but got Unknown
			BuildPanel = buildPanel;
			BuildHeaderPanel(buildPanel);
			try
			{
				Panel val = new Panel();
				((Control)val).set_Parent(buildPanel);
				((Control)val).set_Size(new Point(buildPanel.get_ContentRegion().Width, buildPanel.get_ContentRegion().Height - ((Control)HeaderButton).get_Height()));
				((Control)val).set_Location(new Point(0, ((Control)HeaderButton).get_Height()));
				RecipeDetailsPanel = val;
				base.get_Presenter().HandleServiceLoading((Container)(object)RecipeDetailsPanel);
			}
			catch (Exception ex)
			{
				Logger.Error("Loading recipe details failed with error: " + ex.Message + ". Stacktrace: " + ex.StackTrace);
			}
		}

		public void BuildServicesLoadingPanel(Container parent, List<IApiService> services)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Expected O, but got Unknown
			UnloadLoadingPanel();
			ViewContainer val = new ViewContainer();
			((Control)val).set_Parent(parent);
			((Control)val).set_Size(((Control)parent).get_Size());
			((Control)val).set_Location(new Point(((Control)parent).get_Width() / 2 - 90, 200));
			LoadingViewContainer = val;
			LoadingStatusDetailedView loadingView = new LoadingStatusDetailedView(services);
			LoadingViewContainer.Show((IView)(object)loadingView);
		}

		public void BuildLoadingPanel(Container parent, List<IApiService> services)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Expected O, but got Unknown
			UnloadLoadingPanel();
			ViewContainer val = new ViewContainer();
			((Control)val).set_Parent(parent);
			((Control)val).set_Size(((Control)parent).get_Size());
			((Control)val).set_Location(new Point(((Control)parent).get_Width() / 2 - 90, 200));
			LoadingViewContainer = val;
			LoadingStatusDetailedView loadingView = new LoadingStatusDetailedView(services);
			LoadingViewContainer.Show((IView)(object)loadingView);
		}

		public void UnloadLoadingPanel()
		{
			ViewContainer loadingViewContainer = LoadingViewContainer;
			if (loadingViewContainer != null)
			{
				((Control)loadingViewContainer).Dispose();
			}
		}

		public void LoadRecipeDetails()
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Expected O, but got Unknown
			UnloadLoadingPanel();
			LoadingSpinner val = new LoadingSpinner();
			((Control)val).set_Parent((Container)(object)RecipeDetailsPanel);
			((Control)val).set_Size(new Point(60, 60));
			((Control)val).set_Location(new Point(((Control)RecipeDetailsPanel).get_Width() / 2 - 30, ((Control)RecipeDetailsPanel).get_Height() / 2 - 30));
			LoadingSpinner _loadingSpinner = val;
			Task.Run(delegate
			{
				Item = ServiceContainer.ItemRepository.GetItem(ItemId);
				LoadingSpinner obj = _loadingSpinner;
				if (obj != null)
				{
					((Control)obj).Dispose();
				}
				BuildRecipeDetails();
			});
		}

		public void BuildRecipeDetails()
		{
			if (RecipeDetailsPanel != null)
			{
				UnloadLoadingPanel();
				BuildRecipeItemList((Container)(object)RecipeDetailsPanel);
				RecipeDetailsLoaded = true;
			}
		}

		public void BuildHeaderPanel(Container parent)
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			PanelHeaderButton headerButton = HeaderButton;
			if (headerButton != null)
			{
				((Control)headerButton).Dispose();
			}
			PanelHeaderButton panelHeaderButton = new PanelHeaderButton();
			((Control)panelHeaderButton).set_Parent(parent);
			((Control)panelHeaderButton).set_Size(new Point(((Control)parent).get_Width(), 40));
			panelHeaderButton.Breadcrumbs = _breadcrumbs;
			HeaderButton = panelHeaderButton;
			((Control)HeaderButton).add_Click(OnBackButtonClick);
		}

		private void BuildRecipeItemList(Container parent)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Expected O, but got Unknown
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			FlowPanel flowPanel = FlowPanel;
			if (flowPanel != null)
			{
				((Control)flowPanel).Dispose();
			}
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent(parent);
			int num = ((Control)parent).get_Width() - 20;
			Rectangle contentRegion = parent.get_ContentRegion();
			((Control)val).set_Size(new Point(num, ((Rectangle)(ref contentRegion)).get_Size().Y));
			((Control)val).set_Location(new Point(0, 0));
			((Panel)val).set_CanScroll(true);
			((Panel)val).set_ShowBorder(true);
			FlowPanel = val;
			MysticCrafting.Module.RecipeTree.TreeView.TreeView treeView = TreeView;
			if (treeView != null)
			{
				((Control)treeView).Dispose();
			}
			MysticCrafting.Module.RecipeTree.TreeView.TreeView treeView2 = new MysticCrafting.Module.RecipeTree.TreeView.TreeView();
			((Control)treeView2).set_Parent((Container)(object)FlowPanel);
			((Container)treeView2).set_HeightSizingMode((SizingMode)1);
			((Control)treeView2).set_Size(new Point(BuildPanel.get_ContentRegion().Width, 600));
			TreeView = treeView2;
			TreeViewContextHelper.Context.CurrentTreeView = TreeView;
			TreeView.OnRecalculate += delegate
			{
				base.get_Presenter().UpdateScrollDistance();
			};
			try
			{
				_ingredientNodePresenter.BuildItemNode(Item, (Container)(object)TreeView, expandable: false, isPrimaryNode: true);
				TreeView.ReIndex();
			}
			catch (Exception ex)
			{
				Logger.Error("Loading recipe tree failed with error: " + ex.Message + ". Stacktrace: " + ex.StackTrace);
			}
		}
	}
}
