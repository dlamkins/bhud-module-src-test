using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using MysticCrafting.Models.Items;
using MysticCrafting.Module.Items.Controls;
using MysticCrafting.Module.Recipe.TreeView;
using MysticCrafting.Module.Recipe.TreeView.Presenters;
using MysticCrafting.Module.Recipe.TreeView.Tooltip;
using MysticCrafting.Module.Services;
using MysticCrafting.Module.Services.Recurring;

namespace MysticCrafting.Module.Recipe
{
	public class RecipeDetailsView : View<IRecipeDetailsViewPresenter>
	{
		public EventHandler<MouseEventArgs> OnBackButtonClick;

		public Scrollbar _scrollBar;

		private IIngredientNodePresenter _ingredientNodePresenter;

		private List<string> _breadcrumbs;

		private static readonly Logger Logger = Logger.GetLogger<RecipeDetailsView>();

		public MysticItem Item { get; set; }

		public FlowPanel FlowPanel { get; set; }

		public Container BuildPanel { get; set; }

		public PanelHeaderButton HeaderButton { get; set; }

		public bool RecipeDetailsLoaded { get; set; }

		public Panel RecipeDetailsPanel { get; set; }

		public ViewContainer LoadingViewContainer { get; set; }

		public MysticCrafting.Module.Recipe.TreeView.TreeView RecipeItemList { get; set; }

		public Scrollbar Scrollbar
		{
			get
			{
				if (_scrollBar == null)
				{
					_scrollBar = RecipeDetailsPanel.Children.OfType<Scrollbar>().FirstOrDefault();
				}
				return _scrollBar;
			}
		}

		public RecipeDetailsView(MysticItem item, List<string> breadcrumbs)
		{
			Item = item;
			RecipeDetailsViewPresenter presenter = new RecipeDetailsViewPresenter(this, item);
			WithPresenter(presenter);
			_breadcrumbs = breadcrumbs;
			_ingredientNodePresenter = new IngredientNodePresenter(presenter, ServiceContainer.ItemRepository, ServiceContainer.ChoiceRepository);
		}

		protected override void Build(Container buildPanel)
		{
			BuildPanel = buildPanel;
			BuildHeaderPanel(buildPanel);
			try
			{
				RecipeDetailsPanel = new Panel
				{
					Parent = buildPanel,
					Size = new Point(buildPanel.Width, buildPanel.Height - HeaderButton.Height),
					Location = new Point(0, HeaderButton.Height)
				};
				base.Presenter.HandleServiceLoading(RecipeDetailsPanel);
			}
			catch (Exception ex)
			{
				Logger.Error("Loading recipe details failed with error: " + ex.Message + ". Stacktrace: " + ex.StackTrace);
			}
		}

		public void BuildLoadingPanel(Container parent, List<IRecurringService> services)
		{
			UnloadLoadingPanel();
			LoadingViewContainer = new ViewContainer
			{
				Parent = parent,
				Size = parent.Size,
				Location = new Point(parent.Width / 2 - 90, 200)
			};
			LoadingStatusDetailedView loadingView = new LoadingStatusDetailedView(services);
			LoadingViewContainer.Show(loadingView);
		}

		public void UnloadLoadingPanel()
		{
			LoadingViewContainer?.Dispose();
		}

		public void BuildRecipeDetails()
		{
			if (RecipeDetailsPanel != null)
			{
				UnloadLoadingPanel();
				BuildRecipeItemList(RecipeDetailsPanel);
				RecipeDetailsLoaded = true;
			}
		}

		public void BuildHeaderPanel(Container parent)
		{
			HeaderButton?.Dispose();
			HeaderButton = new PanelHeaderButton
			{
				Parent = parent,
				Size = new Point(parent.Width, 40),
				Breadcrumbs = _breadcrumbs
			};
			HeaderButton.Click += OnBackButtonClick;
		}

		private void BuildRecipeItemList(Container parent)
		{
			FlowPanel?.Dispose();
			FlowPanel = new FlowPanel
			{
				Parent = parent,
				Size = new Point(parent.Size.X - 20, parent.ContentRegion.Size.Y),
				Location = new Point(0, 0),
				CanScroll = true,
				ShowBorder = true
			};
			RecipeItemList?.Dispose();
			RecipeItemList = new MysticCrafting.Module.Recipe.TreeView.TreeView
			{
				Parent = FlowPanel,
				HeightSizingMode = SizingMode.AutoSize,
				Size = new Point(BuildPanel.Width, 600)
			};
			RecipeItemList.OnRecalculate += delegate
			{
				base.Presenter.UpdateScrollDistance();
			};
			try
			{
				_ingredientNodePresenter.BuildNode(Item, RecipeItemList);
			}
			catch (Exception ex)
			{
				Logger.Error("Loading recipe tree failed with error: " + ex.Message + ". Stacktrace: " + ex.StackTrace);
			}
		}
	}
}
