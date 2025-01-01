using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Atzie.MysticCrafting.Models.Items;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using MysticCrafting.Module.Discovery.ItemList.Controls;
using MysticCrafting.Module.Extensions;
using MysticCrafting.Module.Models;
using MysticCrafting.Module.RecipeTree.TreeView;
using MysticCrafting.Module.RecipeTree.TreeView.Controls;
using MysticCrafting.Module.RecipeTree.TreeView.Extensions;
using MysticCrafting.Module.RecipeTree.TreeView.Presenters;
using MysticCrafting.Module.Services;
using MysticCrafting.Module.Strings;

namespace MysticCrafting.Module.RecipeTree
{
	public class RecipeDetailsView : View<IRecipeDetailsViewPresenter>
	{
		public EventHandler<MouseEventArgs> OnBackButtonClick;

		private Scrollbar _scrollBar;

		private IIngredientNodePresenter _ingredientNodePresenter;

		private IList<string> _breadcrumbs;

		private static readonly Logger Logger = Logger.GetLogger<RecipeDetailsView>();

		private readonly Timer _pricingUpdateTimer = new Timer(300000.0);

		public int ItemId { get; set; }

		public Item Item { get; set; }

		public FlowPanel TreeViewFlowPanel { get; set; }

		public Container BuildPanel { get; set; }

		public PanelHeaderButton HeaderButton { get; set; }

		public bool RecipeDetailsLoaded { get; set; }

		public Panel RecipeDetailsPanel { get; set; }

		public MysticCrafting.Module.RecipeTree.TreeView.TreeView TreeView { get; set; }

		public Scrollbar Scrollbar => _scrollBar ?? (_scrollBar = ((IEnumerable)((Container)RecipeDetailsPanel).get_Children()).OfType<Scrollbar>().FirstOrDefault());

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
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Expected O, but got Unknown
			BuildPanel = buildPanel;
			ImageButton imageButton = new ImageButton();
			((Control)imageButton).set_Parent(buildPanel);
			imageButton.Texture = AsyncTexture2D.FromAssetId(784268);
			imageButton.HoverTexture = AsyncTexture2D.FromAssetId(2175786);
			((Control)imageButton).set_Size(new Point(35, 35));
			((Control)imageButton).set_Location(new Point(5, 5));
			((Control)imageButton).set_ZIndex(9999);
			((Control)imageButton).add_Click(OnBackButtonClick);
			try
			{
				Panel val = new Panel();
				((Control)val).set_Parent(buildPanel);
				((Control)val).set_Size(new Point(buildPanel.get_ContentRegion().Width, buildPanel.get_ContentRegion().Height));
				((Control)val).set_Location(new Point(0, 0));
				RecipeDetailsPanel = val;
				base.get_Presenter().HandleServiceLoading((Container)(object)RecipeDetailsPanel);
			}
			catch (Exception ex)
			{
				Logger.Error("Loading recipe details failed with error: " + ex.Message + ". Stacktrace: " + ex.StackTrace);
			}
		}

		public void LoadRecipeDetails()
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Expected O, but got Unknown
			LoadingSpinner val = new LoadingSpinner();
			((Control)val).set_Parent((Container)(object)RecipeDetailsPanel);
			((Control)val).set_Size(new Point(60, 60));
			((Control)val).set_Location(new Point(((Control)RecipeDetailsPanel).get_Width() / 2 - 30, ((Control)RecipeDetailsPanel).get_Height() / 2 - 30));
			LoadingSpinner _loadingSpinner = val;
			Task.Run(delegate
			{
				Item = ServiceContainer.ItemRepository.GetItem(ItemId);
				if (ItemId == 103815)
				{
					ServiceContainer.AchievementRepository.LoadAchievementsAsync();
				}
				ServiceContainer.WizardsVaultRepository.LoadContainersAsync();
				LoadingSpinner obj = _loadingSpinner;
				if (obj != null)
				{
					((Control)obj).Dispose();
				}
				if (Item != null)
				{
					BuildRecipeDetails();
				}
				else
				{
					Logger.Error($"Could not retrieve item with ID: {ItemId}");
				}
			});
		}

		public void BuildRecipeDetails()
		{
			if (RecipeDetailsPanel != null)
			{
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
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Expected O, but got Unknown
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			List<IItemSource> sources = ServiceContainer.ItemSourceService.GetItemSources(Item).ToList();
			Image val = new Image(ServiceContainer.TextureRepository.GetItemSourceBackgroundTexture(sources));
			((Control)val).set_Parent(parent);
			((Control)val).set_Size(new Point(767, 745));
			((Control)val).set_Location(new Point(150, 50));
			((Control)val).set_Opacity(0.15f);
			FlowPanel treeViewFlowPanel = TreeViewFlowPanel;
			if (treeViewFlowPanel != null)
			{
				((Control)treeViewFlowPanel).Dispose();
			}
			FlowPanel val2 = new FlowPanel();
			((Control)val2).set_Parent(parent);
			int num = ((Control)parent).get_Width() - 20;
			Rectangle contentRegion = parent.get_ContentRegion();
			((Control)val2).set_Size(new Point(num, ((Rectangle)(ref contentRegion)).get_Size().Y));
			((Control)val2).set_Location(new Point(0, 0));
			((Panel)val2).set_CanScroll(true);
			((Panel)val2).set_ShowBorder(true);
			TreeViewFlowPanel = val2;
			Container container = BuildItemDetails();
			Panel headerPanel = BuildHeaderPanel(((Control)container).get_Width());
			MysticCrafting.Module.RecipeTree.TreeView.TreeView treeView = TreeView;
			if (treeView != null)
			{
				((Control)treeView).Dispose();
			}
			MysticCrafting.Module.RecipeTree.TreeView.TreeView treeView2 = new MysticCrafting.Module.RecipeTree.TreeView.TreeView();
			((Control)treeView2).set_Parent((Container)(object)TreeViewFlowPanel);
			((Container)treeView2).set_HeightSizingMode((SizingMode)1);
			((Control)treeView2).set_Size(new Point(BuildPanel.get_ContentRegion().Width, 640 - ((Control)headerPanel).get_Height()));
			TreeView = treeView2;
			TreeViewContextHelper.Context.CurrentTreeView = TreeView;
			TreeView.OnRecalculate += delegate
			{
				base.get_Presenter().UpdateScrollDistance();
			};
			base.get_Presenter().InitializeScrollbar();
			try
			{
				_ingredientNodePresenter.BuildItemNode(Item, (Container)(object)TreeView, expandable: false, isPrimaryNode: true);
				TreeView.ReIndex();
				TreeView.UpdatePrices(Item);
				_pricingUpdateTimer.Elapsed += delegate
				{
					_pricingUpdateTimer.Stop();
					if (MysticCraftingModule.WindowIsOpen())
					{
						TreeView.UpdatePrices(Item);
					}
					_pricingUpdateTimer.Start();
				};
				_pricingUpdateTimer.Start();
			}
			catch (Exception ex)
			{
				Logger.Error("Loading recipe tree failed with error: " + ex.Message + ". Stacktrace: " + ex.StackTrace);
			}
		}

		public Container BuildItemDetails()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Expected O, but got Unknown
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Expected O, but got Unknown
			//IL_0186: Unknown result type (might be due to invalid IL or missing references)
			//IL_018b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0192: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0209: Unknown result type (might be due to invalid IL or missing references)
			//IL_0210: Unknown result type (might be due to invalid IL or missing references)
			//IL_0217: Unknown result type (might be due to invalid IL or missing references)
			//IL_0228: Unknown result type (might be due to invalid IL or missing references)
			//IL_0230: Unknown result type (might be due to invalid IL or missing references)
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)TreeViewFlowPanel);
			val.set_ShowBorder(false);
			val.set_ShowTint(false);
			((Control)val).set_Size(new Point(1241, 80));
			Panel container = val;
			Image val2 = new Image(AsyncTexture2D.FromAssetId(358411));
			((Control)val2).set_Size(new Point(1241, 154));
			((Control)val2).set_Location(new Point(0, -25));
			((Control)val2).set_Parent((Container)(object)container);
			if (AsyncTexture2D.FromAssetId(Item.IconId) != null)
			{
				Image val3 = new Image(AsyncTexture2D.FromAssetId(Item.IconId));
				((Control)val3).set_Parent((Container)(object)container);
				((Control)val3).set_Size(new Point(80, 80));
				((Control)val3).set_Location(new Point(35, 0));
			}
			Label val4 = new Label();
			((Control)val4).set_Parent((Container)(object)container);
			val4.set_Font(GameService.Content.get_DefaultFont32());
			val4.set_TextColor(Color.get_White());
			val4.set_AutoSizeHeight(true);
			val4.set_AutoSizeWidth(true);
			val4.set_Text(Item.LocalizedName());
			((Control)val4).set_Location(new Point(135, 5));
			Label name = val4;
			string countText = string.Empty;
			if (Item.Rarity == ItemRarity.Legendary)
			{
				int unlockedCount = ServiceContainer.PlayerUnlocksService.LegendaryUnlockedCount(Item.Id);
				int? maxCount = Item.GetMaxCount();
				countText = $"({unlockedCount}/{maxCount})";
			}
			else
			{
				countText = $"({ServiceContainer.PlayerItemService.GetItemCount(Item.Id)})";
			}
			Label val5 = new Label();
			((Control)val5).set_Parent((Container)(object)container);
			val5.set_Font(GameService.Content.get_DefaultFont32());
			val5.set_TextColor(Color.get_DarkGray());
			val5.set_AutoSizeHeight(true);
			val5.set_AutoSizeWidth(true);
			val5.set_Text(countText);
			((Control)val5).set_Location(new Point(((Control)name).get_Right() + 15, 5));
			Label val6 = new Label();
			((Control)val6).set_Parent((Container)(object)container);
			val6.set_Font(GameService.Content.get_DefaultFont18());
			val6.set_TextColor(Color.get_White() * 0.6f);
			val6.set_AutoSizeHeight(true);
			val6.set_AutoSizeWidth(true);
			val6.set_Text(Item.DetailsType);
			((Control)val6).set_Location(new Point(140, 45));
			return (Container)(object)container;
		}

		public Panel BuildHeaderPanel(int width)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Expected O, but got Unknown
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)TreeViewFlowPanel);
			val.set_ShowBorder(false);
			val.set_ShowTint(false);
			val.set_BackgroundTexture(AsyncTexture2D.FromAssetId(1032325));
			((Control)val).set_Size(new Point(width, 36));
			Panel headerPanel = val;
			ViewContainer val2 = new ViewContainer();
			((Control)val2).set_Parent((Container)(object)headerPanel);
			((Control)val2).set_Location(new Point(15, 0));
			((Control)val2).set_Size(new Point(200, 30));
			RecipeTableHeaderItemView countLabel = new RecipeTableHeaderItemView(ServiceContainer.PlayerItemService, Recipe.TableHeaderCount);
			val2.Show((IView)(object)countLabel);
			ViewContainer val3 = new ViewContainer();
			((Control)val3).set_Parent((Container)(object)headerPanel);
			((Control)val3).set_Location(new Point(350, 0));
			((Control)val3).set_Size(new Point(200, 30));
			RecipeTableHeaderItemView priceLabel = new RecipeTableHeaderItemView(ServiceContainer.TradingPostService, Recipe.TableHeaderPrice);
			val3.Show((IView)(object)priceLabel);
			Label val4 = new Label();
			((Control)val4).set_Parent((Container)(object)headerPanel);
			val4.set_Font(GameService.Content.get_DefaultFont16());
			val4.set_TextColor(Color.get_White() * 0.7f);
			val4.set_AutoSizeHeight(true);
			val4.set_AutoSizeWidth(true);
			val4.set_Text(Recipe.TableHeaderSources);
			((Control)val4).set_Location(new Point(((Control)headerPanel).get_Width() - 250, 8));
			return headerPanel;
		}

		protected override void Unload()
		{
			_pricingUpdateTimer?.Dispose();
			MysticCrafting.Module.RecipeTree.TreeView.TreeView treeView = TreeView;
			if (treeView != null)
			{
				((Control)treeView).Dispose();
			}
			FlowPanel treeViewFlowPanel = TreeViewFlowPanel;
			if (treeViewFlowPanel != null)
			{
				((Control)treeViewFlowPanel).Dispose();
			}
			Panel recipeDetailsPanel = RecipeDetailsPanel;
			if (recipeDetailsPanel != null)
			{
				((Control)recipeDetailsPanel).Dispose();
			}
			PanelHeaderButton headerButton = HeaderButton;
			if (headerButton != null)
			{
				((Control)headerButton).Dispose();
			}
			Scrollbar scrollBar = _scrollBar;
			if (scrollBar != null)
			{
				((Control)scrollBar).Dispose();
			}
			OnBackButtonClick = null;
			Item = null;
			base.Unload();
		}
	}
}
