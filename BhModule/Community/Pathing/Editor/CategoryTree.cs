using System.Linq;
using System.Windows.Forms;
using BhModule.Community.Pathing.Entity;
using BhModule.Community.Pathing.State;
using BhModule.Community.Pathing.Utility;
using TmfLib.Pathable;

namespace BhModule.Community.Pathing.Editor
{
	public class CategoryTree : TreeView
	{
		public IPackState _packState;

		public IPackState PackState
		{
			get
			{
				return _packState;
			}
			set
			{
				if (!object.Equals(_packState, value))
				{
					_packState = value;
					UpdateTreeView();
				}
			}
		}

		protected override void OnAfterExpand(TreeViewEventArgs e)
		{
			TreeNode node = e.Node;
			CategoryNode categoryNode = node as CategoryNode;
			if (categoryNode != null)
			{
				e.Node.Nodes.Clear();
				TreeNodeCollection treeNodeCollection = e.Node.Nodes;
				TreeNode[] array = (from category in categoryNode.PathingCategory
					where CategoryUtil.GetCategoryIsNotFiltered(category, _packState.Entities.ToArray(), CategoryUtil.CurrentMapCategoryFilter)
					select category into childCategory
					select new CategoryNode(childCategory, _packState)).ToArray();
				treeNodeCollection.AddRange(array);
				TreeNodeCollection treeNodeCollection2 = e.Node.Nodes;
				array = (from pathable in _packState.Entities
					where string.Equals(pathable.Category.Namespace, categoryNode.PathingCategory.Namespace)
					select new PathableNode(pathable)).ToArray();
				treeNodeCollection2.AddRange(array);
			}
			base.OnAfterExpand(e);
		}

		protected override void OnNodeMouseClick(TreeNodeMouseClickEventArgs e)
		{
			CategoryNode categoryNode = e.Node as CategoryNode;
			if (categoryNode != null)
			{
				MarkerEditWindow.SetPathingCategory(_packState, categoryNode.PathingCategory);
			}
			else
			{
				PathableNode pathableNode = e.Node as PathableNode;
				if (pathableNode != null)
				{
					MarkerEditWindow.SetPathingEntity(_packState, pathableNode.PathingEntity);
				}
			}
			base.OnNodeMouseClick(e);
		}

		private void UpdateTreeView()
		{
			base.Nodes.Clear();
			if (_packState != null)
			{
				TreeNodeCollection treeNodeCollection = base.Nodes;
				TreeNode[] array = (from category in _packState.RootCategory
					where CategoryUtil.GetCategoryIsNotFiltered(category, _packState.Entities.ToArray(), CategoryUtil.CurrentMapCategoryFilter)
					select category into childCategory
					select new CategoryNode(childCategory, _packState)).ToArray();
				treeNodeCollection.AddRange(array);
			}
		}
	}
}
