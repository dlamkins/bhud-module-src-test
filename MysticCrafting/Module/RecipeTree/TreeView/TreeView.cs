using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using MysticCrafting.Module.RecipeTree.TreeView.Extensions;
using MysticCrafting.Module.RecipeTree.TreeView.Nodes;

namespace MysticCrafting.Module.RecipeTree.TreeView
{
	public class TreeView : Container, IIngredientContainer
	{
		public List<IngredientNode> IngredientNodes = new List<IngredientNode>();

		public event EventHandler<EventArgs> OnRecalculate;

		public event EventHandler<EventArgs> NodeChanged;

		public override void RecalculateLayout()
		{
			((Control)this).RecalculateLayout();
			this.OnRecalculate?.Invoke(this, EventArgs.Empty);
		}

		protected override void OnChildAdded(ChildChangedEventArgs e)
		{
			IngredientNode node = e.get_ChangedChild() as IngredientNode;
			if (node != null)
			{
				IngredientNodes.Add(node);
				node.UpdateRelatedNodes();
			}
			((Container)this).OnChildAdded(e);
		}

		protected override void OnChildRemoved(ChildChangedEventArgs e)
		{
			IngredientNode node = e.get_ChangedChild() as IngredientNode;
			if (node != null)
			{
				RemoveNode(node);
			}
			((Container)this).OnChildRemoved(e);
		}

		public void RemoveNode(IngredientNode node)
		{
			if (IngredientNodes != null)
			{
				IngredientNodes.RemoveNodeAndDescendants(node);
				node.UpdateRelatedNodesAndDescendants();
			}
		}

		public void ClearChildIngredientNodes()
		{
			while (IngredientNodes.Any())
			{
				IngredientNode ingredientNode = IngredientNodes.FirstOrDefault();
				if (ingredientNode != null)
				{
					((Control)ingredientNode).Dispose();
				}
			}
		}

		protected override void DisposeControl()
		{
			IngredientNodes = null;
			((Container)this).DisposeControl();
		}

		public TreeView()
			: this()
		{
		}
	}
}
