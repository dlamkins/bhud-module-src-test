using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using MysticCrafting.Module.Recipe.TreeView.Extensions;
using MysticCrafting.Module.Recipe.TreeView.Nodes;

namespace MysticCrafting.Module.Recipe.TreeView
{
	public class TreeView : Container, IIngredientContainer
	{
		public List<IngredientNode> IngredientNodes = new List<IngredientNode>();

		public event EventHandler<EventArgs> OnRecalculate;

		public override void RecalculateLayout()
		{
			base.RecalculateLayout();
			this.OnRecalculate?.Invoke(this, EventArgs.Empty);
		}

		protected override void OnChildAdded(ChildChangedEventArgs e)
		{
			IngredientNode node = e.ChangedChild as IngredientNode;
			if (node != null)
			{
				IngredientNodes.Add(node);
				node.UpdateRelatedNodes();
			}
			base.OnChildAdded(e);
		}

		protected override void OnChildRemoved(ChildChangedEventArgs e)
		{
			IngredientNode node = e.ChangedChild as IngredientNode;
			if (node != null)
			{
				RemoveNode(node);
			}
			base.OnChildRemoved(e);
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
				IngredientNodes.FirstOrDefault()?.Dispose();
			}
		}

		protected override void DisposeControl()
		{
			IngredientNodes = null;
			base.DisposeControl();
		}
	}
}
