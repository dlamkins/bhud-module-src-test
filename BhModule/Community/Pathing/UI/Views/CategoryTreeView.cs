using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BhModule.Community.Pathing.UI.Controls.TreeNodes;
using BhModule.Community.Pathing.UI.Controls.TreeView;
using BhModule.Community.Pathing.UI.Presenter;
using BhModule.Community.Pathing.Utility;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using TmfLib.Pathable;

namespace BhModule.Community.Pathing.UI.Views
{
	public class CategoryTreeView : View
	{
		private static readonly Logger _logger = Logger.GetLogger<CategoryTreeView>();

		private TextBox _searchBox;

		private Label _searchStatusLabel;

		private Label _packsNotInitializedLabel;

		private Label _packsNotLoadedLabel;

		private Label _helpTextLabel;

		private LoadingSpinner _loadingSpinner;

		private readonly PathingModule _module;

		private CancellationTokenSource _cancellationTokenSource;

		private static readonly SemaphoreSlim _searchSemaphore = new SemaphoreSlim(1, 1);

		private FlowPanel RepoFlowPanel { get; set; }

		public TreeView TreeView { get; private set; }

		public PathingCategory TargetCategory { get; set; }

		public CategoryTreeView(PathingModule module)
			: this()
		{
			_module = module;
			((View)this).WithPresenter((IPresenter)(object)new CategoryTreePresenter(this, module));
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Expected O, but got Unknown
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Expected O, but got Unknown
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Expected O, but got Unknown
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_014e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_015d: Unknown result type (might be due to invalid IL or missing references)
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0174: Unknown result type (might be due to invalid IL or missing references)
			//IL_017b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0192: Unknown result type (might be due to invalid IL or missing references)
			//IL_019c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01af: Expected O, but got Unknown
			//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fc: Expected O, but got Unknown
			//IL_0204: Unknown result type (might be due to invalid IL or missing references)
			//IL_020f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0233: Unknown result type (might be due to invalid IL or missing references)
			//IL_027d: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02de: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0304: Unknown result type (might be due to invalid IL or missing references)
			//IL_030e: Unknown result type (might be due to invalid IL or missing references)
			//IL_031a: Expected O, but got Unknown
			TextBox val = new TextBox();
			((TextInputBase)val).set_PlaceholderText("Search markers or insert path...");
			((Control)val).set_Parent(buildPanel);
			((Control)val).set_Location(new Point(0, 10));
			((Control)val).set_Width(buildPanel.get_ContentRegion().Width - 25);
			_searchBox = val;
			((TextInputBase)_searchBox).add_TextChanged((EventHandler<EventArgs>)SearchBoxTextChanged);
			Label val2 = new Label();
			val2.set_Text("Please enter the game to load the marker packs.");
			val2.set_Font(GameService.Content.get_DefaultFont18());
			val2.set_AutoSizeHeight(true);
			val2.set_AutoSizeWidth(true);
			((Control)val2).set_Location(new Point(((Control)buildPanel).get_Width() / 2 - 200, ((Control)buildPanel).get_Height() / 2 - 80));
			((Control)val2).set_Parent(buildPanel);
			((Control)val2).set_Visible(!PacksAreInitialized());
			_packsNotInitializedLabel = val2;
			Label val3 = new Label();
			val3.set_Text("No marker packs have been loaded.");
			val3.set_Font(GameService.Content.get_DefaultFont18());
			val3.set_AutoSizeHeight(true);
			val3.set_AutoSizeWidth(true);
			((Control)val3).set_Location(new Point(((Control)buildPanel).get_Width() / 2 - 160, ((Control)buildPanel).get_Height() / 2 - 80));
			((Control)val3).set_Parent(buildPanel);
			((Control)val3).set_Visible(PacksAreInitialized() && !PacksAreLoaded());
			_packsNotLoadedLabel = val3;
			Label val4 = new Label();
			val4.set_Text("No categories found...");
			((Control)val4).set_Size(new Point(100, 20));
			val4.set_Font(GameService.Content.get_DefaultFont18());
			val4.set_AutoSizeHeight(true);
			val4.set_AutoSizeWidth(true);
			((Control)val4).set_Location(new Point(((Control)buildPanel).get_Width() / 2 - 120, ((Control)buildPanel).get_Height() / 2 - 80));
			((Control)val4).set_Parent(buildPanel);
			((Control)val4).set_Visible(false);
			_searchStatusLabel = val4;
			Label val5 = new Label();
			((Control)val5).set_Parent(buildPanel);
			val5.set_Text("Right click on categories for options.");
			val5.set_AutoSizeHeight(true);
			val5.set_AutoSizeWidth(true);
			val5.set_StrokeText(true);
			val5.set_TextColor(Color.get_LightYellow());
			val5.set_Font(GameService.Content.get_DefaultFont16());
			_helpTextLabel = val5;
			CustomFlowPanel customFlowPanel = new CustomFlowPanel();
			((Control)customFlowPanel).set_Size(new Point(buildPanel.get_ContentRegion().Width, buildPanel.get_ContentRegion().Height - ((Control)_searchBox).get_Bottom() - ((Control)_helpTextLabel).get_Height() - 5));
			((Control)customFlowPanel).set_Top(((Control)_searchBox).get_Bottom() + 5);
			((Panel)customFlowPanel).set_CanScroll(true);
			((Panel)customFlowPanel).set_ShowBorder(true);
			((Control)customFlowPanel).set_Parent(buildPanel);
			RepoFlowPanel = (FlowPanel)(object)customFlowPanel;
			((Control)_helpTextLabel).set_Location(new Point(15, ((Control)RepoFlowPanel).get_Bottom()));
			TreeView treeView = new TreeView(_module.PackInitiator);
			((Container)treeView).set_HeightSizingMode((SizingMode)1);
			((Control)treeView).set_Size(new Point(((Control)RepoFlowPanel).get_Width(), ((Control)RepoFlowPanel).get_Height()));
			((Control)treeView).set_Parent((Container)(object)RepoFlowPanel);
			TreeView = treeView;
			LoadingSpinner val6 = new LoadingSpinner();
			((Control)val6).set_Parent(buildPanel);
			((Control)val6).set_Location(new Point(((Control)buildPanel).get_Width() / 2 - 75, ((Control)buildPanel).get_Height() / 2 - 75));
			((Control)val6).set_Size(new Point(55, 55));
			((Control)val6).set_Visible(false);
			_loadingSpinner = val6;
			TreeView.NodeLoadingStarted += delegate
			{
				_cancellationTokenSource?.Cancel();
				SetLoading(loading: true);
				ResetSearch();
			};
			TreeView.NodesLoadedFinished += delegate
			{
				SetLoading(loading: false);
			};
		}

		public bool ValidateMarkerPacksState()
		{
			bool packsAreInitialized = PacksAreInitialized();
			bool packsAreLoaded = PacksAreLoaded();
			if (_packsNotInitializedLabel != null)
			{
				((Control)_packsNotInitializedLabel).set_Visible(!packsAreInitialized);
			}
			if (_packsNotLoadedLabel != null)
			{
				((Control)_packsNotLoadedLabel).set_Visible(packsAreInitialized && !packsAreLoaded);
			}
			return packsAreLoaded;
		}

		public void SetLoading(bool loading)
		{
			if (loading)
			{
				if (_packsNotInitializedLabel != null)
				{
					((Control)_packsNotInitializedLabel).set_Visible(false);
				}
				if (_packsNotLoadedLabel != null)
				{
					((Control)_packsNotLoadedLabel).set_Visible(false);
				}
			}
			((Control)_loadingSpinner).set_Visible(loading);
		}

		private void SearchBoxTextChanged(object sender, EventArgs e)
		{
			CategoryTreePresenter presenter = ((View<IPresenter>)this).get_Presenter() as CategoryTreePresenter;
			if (presenter == null)
			{
				return;
			}
			_cancellationTokenSource?.Cancel();
			_cancellationTokenSource = new CancellationTokenSource();
			TreeView.RemoveNodeHighlights();
			((Control)_searchStatusLabel).set_Visible(false);
			if (((TextInputBase)_searchBox).get_Text().StartsWith("."))
			{
				TreeView.NavigateToPath(((TextInputBase)_searchBox).get_Text());
				return;
			}
			if (string.IsNullOrWhiteSpace(((TextInputBase)_searchBox).get_Text()))
			{
				((Presenter<CategoryTreeView, PackInitiator>)presenter).DoUpdateView();
				return;
			}
			TreeView.ClearChildNodes();
			SetLoading(loading: true);
			Task.Run(async delegate
			{
				await ExecuteSearch(((TextInputBase)_searchBox).get_Text(), _cancellationTokenSource.Token);
			}, _cancellationTokenSource.Token);
		}

		public void NavigateToCategory(PathingCategory category)
		{
			ResetSearch();
			TreeView?.LoadNodes();
			TreeView?.NavigateToPath(category.GetPath());
		}

		private async Task ExecuteSearch(string input, CancellationToken cancellationToken, bool forceShowAll = false)
		{
			await _searchSemaphore.WaitAsync(cancellationToken);
			try
			{
				cancellationToken.ThrowIfCancellationRequested();
				await Task.Delay(200, cancellationToken);
				cancellationToken.ThrowIfCancellationRequested();
				(List<PathingCategory>, int) searchResult = await TreeView.SearchAsync(input, cancellationToken, forceShowAll);
				cancellationToken.ThrowIfCancellationRequested();
				LabelNode showAllSkippedNode = TreeView.SetSearchResults(searchResult.Item1, _module.PackInitiator.PackState, searchResult.Item2);
				cancellationToken.ThrowIfCancellationRequested();
				if (showAllSkippedNode != null)
				{
					((Control)showAllSkippedNode).add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)async delegate
					{
						cancellationToken.ThrowIfCancellationRequested();
						await ExecuteSearch(input, cancellationToken, forceShowAll: true);
					});
				}
				((Control)_searchStatusLabel).set_Visible(searchResult.Item1.Count <= 0);
				SetLoading(loading: false);
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex)
			{
				_logger.Error(ex, "Category search failed.");
			}
			finally
			{
				_searchSemaphore.Release();
			}
		}

		public void ResetSearch()
		{
			((TextInputBase)_searchBox).set_Text(string.Empty);
		}

		private bool PacksAreInitialized()
		{
			return _module.PackInitiator?.GetAllMarkersCategories() != null;
		}

		private bool PacksAreLoaded()
		{
			PathingCategory rootCategory = _module.PackInitiator.GetAllMarkersCategories();
			if (rootCategory != null)
			{
				return rootCategory.Count((PathingCategory c) => c.LoadedFromPack) > 0;
			}
			return false;
		}
	}
}
