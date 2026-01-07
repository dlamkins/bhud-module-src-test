using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BhModule.Community.Pathing.State;
using BhModule.Community.Pathing.UI.Controls.TreeNodes;
using BhModule.Community.Pathing.Utility;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using TmfLib.Pathable;

namespace BhModule.Community.Pathing.UI.Controls.TreeView
{
	public class TreeView : Container
	{
		private Control _scrollToChildControl;

		private PathingCategoryNode _rootNode;

		private static readonly Logger _logger = Logger.GetLogger<TreeView>();

		private PathingCategoryNode _skipStateCheckNode;

		public PackInitiator PackInitiator { get; private set; }

		public IList<TreeNodeBase> AllBaseNodes { get; } = new List<TreeNodeBase>();


		public IList<TreeNodeBase> ChildBaseNodes { get; } = new List<TreeNodeBase>();


		private IList<PathingCategory> AllCategories { get; set; } = new List<PathingCategory>();


		public event EventHandler<EventArgs> NodeLoadingStarted;

		public event EventHandler<EventArgs> NodesLoadedFinished;

		public TreeView(PackInitiator packInitiator)
			: this()
		{
			PackInitiator = packInitiator;
		}

		public void AddNode(TreeNodeBase node)
		{
			if (!AllBaseNodes.Contains(node))
			{
				AllBaseNodes.Add(node);
			}
		}

		public void RemoveNode(TreeNodeBase node)
		{
			if (AllBaseNodes.Contains(node))
			{
				AllBaseNodes.Remove(node);
			}
		}

		public void SetPackInitiator(PackInitiator packInitiator)
		{
			if (packInitiator != null && PackInitiator != packInitiator)
			{
				if (PackInitiator != null)
				{
					PackInitiator.PackState.UserConfiguration.GlobalPathablesEnabled.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)GlobalPathablesEnabledOnSettingChanged);
				}
				PackInitiator = packInitiator;
				PackInitiator.PackState.UserConfiguration.GlobalPathablesEnabled.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)GlobalPathablesEnabledOnSettingChanged);
			}
		}

		protected override void OnChildAdded(ChildChangedEventArgs e)
		{
			TreeNodeBase newChild = e.get_ChangedChild() as TreeNodeBase;
			if (newChild != null)
			{
				AddNode(newChild);
				ChildBaseNodes.Add(newChild);
				ReflowChildLayout(ChildBaseNodes);
				((Container)this).OnChildAdded(e);
			}
		}

		protected override void OnChildRemoved(ChildChangedEventArgs e)
		{
			TreeNodeBase newChild = e.get_ChangedChild() as TreeNodeBase;
			if (newChild != null)
			{
				RemoveNode(newChild);
				ChildBaseNodes.Remove(newChild);
			}
			((Container)this).OnChildRemoved(e);
		}

		public override void RecalculateLayout()
		{
			try
			{
				ReflowChildLayout(ChildBaseNodes);
			}
			catch (Exception ex)
			{
				_logger.Warn("Could not recalculate TreeView layout: " + ex.Message);
			}
			((Control)this).RecalculateLayout();
		}

		private int ReflowChildLayout(IList<TreeNodeBase> containerChildren)
		{
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			int lastBottom = 0;
			foreach (TreeNodeBase item in containerChildren.Where((TreeNodeBase c) => ((Control)c).get_Visible()).ToList())
			{
				((Control)item).set_Location(new Point(0, lastBottom));
				lastBottom = ((Control)item).get_Bottom();
			}
			return lastBottom;
		}

		public void ClearChildNodes()
		{
			Queue<Control> controlsQueue = new Queue<Control>((IEnumerable<Control>)ChildBaseNodes);
			while (controlsQueue.Count > 0)
			{
				Control obj = controlsQueue.Dequeue();
				obj.set_Parent((Container)null);
				obj.Dispose();
			}
		}

		public void LoadNodes()
		{
			this.NodeLoadingStarted?.Invoke(this, EventArgs.Empty);
			ClearChildNodes();
			AllBaseNodes.Clear();
			PathingCategory rootCategory = PackInitiator.GetAllMarkersCategories();
			if (rootCategory == null)
			{
				return;
			}
			PathingCategoryNode rootNode = _rootNode;
			if (rootNode != null)
			{
				((Control)rootNode).Dispose();
			}
			if (rootCategory.Count((PathingCategory c) => c.LoadedFromPack) <= 0)
			{
				return;
			}
			PathingCategoryNode obj = new PathingCategoryNode(PackInitiator.PackState, rootCategory, showForceAll: false)
			{
				Name = "All Markers"
			};
			((Control)obj).set_Width(((Control)this).get_Width() - 30);
			((Control)obj).set_Parent((Container)(object)this);
			_rootNode = obj;
			_rootNode.Checked = PackInitiator.PackState.UserConfiguration.GlobalPathablesEnabled.get_Value();
			PathingCategoryNode rootNode2 = _rootNode;
			rootNode2.CheckedChanged = (EventHandler<CheckChangedEvent>)Delegate.Combine(rootNode2.CheckedChanged, (EventHandler<CheckChangedEvent>)delegate(object _, CheckChangedEvent e)
			{
				if (PackInitiator?.PackState != null)
				{
					PackInitiator.PackState.UserConfiguration.GlobalPathablesEnabled.set_Value(e.get_Checked());
				}
			});
			_rootNode.Expand();
			AllCategories = CategoryUtil.FlattenCategories(rootCategory).ToList();
			this.NodesLoadedFinished?.Invoke(this, EventArgs.Empty);
		}

		private void GlobalPathablesEnabledOnSettingChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			if (_rootNode != null && PackInitiator?.PackState?.UserConfiguration != null)
			{
				_rootNode.Checked = PackInitiator.PackState.UserConfiguration.GlobalPathablesEnabled.get_Value();
			}
		}

		public async Task<(List<PathingCategory> categories, int skipped)> SearchAsync(string input, CancellationToken cancellationToken = default(CancellationToken), bool forceShowAll = false)
		{
			if (string.IsNullOrWhiteSpace(input))
			{
				await Task.CompletedTask;
				return (new List<PathingCategory>(), 0);
			}
			cancellationToken.ThrowIfCancellationRequested();
			string normalizedInput = input.Replace(" ", "");
			var (filteredResults, skipped) = AllCategories.AsParallel().WithCancellation(cancellationToken).Where(delegate(PathingCategory c)
			{
				if (string.IsNullOrWhiteSpace(c.DisplayName) || string.IsNullOrWhiteSpace(c.Name))
				{
					return false;
				}
				string text = c.DisplayName?.Replace(" ", "");
				string text2 = c.Name.Replace(" ", "");
				cancellationToken.ThrowIfCancellationRequested();
				return (text != null && text.IndexOf(normalizedInput, StringComparison.OrdinalIgnoreCase) >= 0) || text2.IndexOf(normalizedInput, StringComparison.OrdinalIgnoreCase) >= 0;
			})
				.ToList()
				.FilterCategories(PackInitiator.PackState, forceShowAll);
			return (filteredResults.ToList(), skipped);
		}

		public void RemoveNodeHighlights()
		{
			foreach (TreeNodeBase node in AllBaseNodes)
			{
				if (node.Highlighted)
				{
					node.Highlighted = false;
				}
			}
		}

		public LabelNode SetSearchResults(IList<PathingCategory> categories, IPackState packState, int skipped = 0)
		{
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			ClearChildNodes();
			LabelNode showAllSkippedCategories = null;
			if (skipped > 0 && packState.UserConfiguration.PackShowWhenCategoriesAreFiltered.get_Value())
			{
				LabelNode obj = new LabelNode($"{skipped} hidden (click to show)", AsyncTexture2D.FromAssetId(358463))
				{
					Clickable = true
				};
				((Control)obj).set_Width(((Control)((Control)this).get_Parent()).get_Width() - 14);
				obj.TextColor = Color.get_LightYellow();
				((Control)obj).set_BasicTooltipText(string.Format(Strings.Info_HiddenCategories, ((SettingEntry)PackInitiator.PackState.UserConfiguration.PackEnableSmartCategoryFilter).get_DisplayName()));
				((Control)obj).set_Parent((Container)(object)this);
				showAllSkippedCategories = obj;
			}
			foreach (PathingCategory category in categories)
			{
				PathingCategoryNode pathingCategoryNode = new PathingCategoryNode(packState, category, showForceAll: false);
				((Control)pathingCategoryNode).set_Width(((Control)this).get_Width() - 30);
				pathingCategoryNode.IsSearchResult = true;
				((Control)pathingCategoryNode).set_Parent((Container)(object)this);
				pathingCategoryNode.Active = !packState.CategoryStates.GetNamespaceInactive(category.Namespace);
			}
			return showAllSkippedCategories;
		}

		public void NavigateToPath(string path)
		{
			if (_rootNode == null)
			{
				return;
			}
			PathingCategoryNode currentNode = _rootNode;
			RemoveNodeHighlights();
			string[] splitPath = path.Split('.');
			foreach (var item in splitPath.Select((string pathItem, int index) => new
			{
				pathItem = pathItem,
				index = index,
				isLast = (index == splitPath.Length - 1)
			}))
			{
				if (!string.IsNullOrWhiteSpace(item.pathItem))
				{
					PathingCategory categoryResult = currentNode.PathingCategory?.FirstOrDefault((PathingCategory n) => !string.IsNullOrWhiteSpace(n?.Name) && n.Name.Equals(item.pathItem));
					if (categoryResult == null || !categoryResult.LoadedFromPack)
					{
						return;
					}
					IList<TreeNodeBase> baseNodes = currentNode.ChildBaseNodes ?? _rootNode?.ChildBaseNodes;
					if (baseNodes == null)
					{
						return;
					}
					PathingCategoryNode tempCurrentNode = baseNodes.OfType<PathingCategoryNode>().FirstOrDefault((PathingCategoryNode n) => n.PathingCategory == categoryResult);
					if (tempCurrentNode == null)
					{
						PathingCategoryNode pathingCategoryNode = new PathingCategoryNode(PackInitiator.PackState, categoryResult, showForceAll: false);
						((Control)pathingCategoryNode).set_Width(((Control)currentNode).get_Width() - 30);
						((Control)pathingCategoryNode).set_Parent((Container)(object)currentNode);
						tempCurrentNode = pathingCategoryNode;
					}
					currentNode = tempCurrentNode;
					if (!item.isLast)
					{
						currentNode.Expand();
					}
				}
			}
			if (currentNode != null)
			{
				currentNode.Highlighted = true;
				_scrollToChildControl = (Control)(object)currentNode;
				if (_scrollToChildControl != null)
				{
					(((Control)this).get_Parent() as CustomFlowPanel)?.ScrollToChild(_scrollToChildControl, _scrollToChildControl.get_Height());
				}
			}
		}

		public void UpdateCheckedState(PathingCategory category, bool active)
		{
			PathingCategoryNode node = AllBaseNodes.OfType<PathingCategoryNode>().FirstOrDefault((PathingCategoryNode n) => n.PathingCategory == category);
			if (node != null)
			{
				node.Checked = active;
			}
		}

		public void SkipNextStateCheck(PathingCategoryNode node)
		{
			_skipStateCheckNode = node;
		}

		public void UpdateSearchResultsCheckState(IPackState packState)
		{
			foreach (TreeNodeBase allBaseNode in AllBaseNodes)
			{
				PathingCategoryNode categoryNode = allBaseNode as PathingCategoryNode;
				if (categoryNode != null && categoryNode.IsSearchResult && categoryNode.Checkable && categoryNode != _skipStateCheckNode)
				{
					categoryNode.Checked = !packState.CategoryStates.GetCategoryInactive(categoryNode.PathingCategory);
					categoryNode.Active = !packState.CategoryStates.GetNamespaceInactive(categoryNode.PathingCategory.Namespace);
					categoryNode.InvalidatePath();
				}
			}
			_skipStateCheckNode = null;
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			((Container)this).OnResized(e);
			if (_scrollToChildControl != null)
			{
				CustomFlowPanel parentPanel = ((Control)this).get_Parent() as CustomFlowPanel;
				if (parentPanel != null)
				{
					parentPanel.ScrollToChild(_scrollToChildControl, _scrollToChildControl.get_Height());
					_scrollToChildControl = null;
				}
			}
		}

		protected override void DisposeControl()
		{
			if (PackInitiator != null)
			{
				PackInitiator.PackState.UserConfiguration.GlobalPathablesEnabled.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)GlobalPathablesEnabledOnSettingChanged);
			}
			((Container)this).DisposeControl();
		}
	}
}
