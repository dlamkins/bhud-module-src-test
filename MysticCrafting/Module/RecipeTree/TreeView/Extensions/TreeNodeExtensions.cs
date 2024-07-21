using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using MysticCrafting.Module.RecipeTree.TreeView.Nodes;

namespace MysticCrafting.Module.RecipeTree.TreeView.Extensions
{
	public static class TreeNodeExtensions
	{
		public static void ReIndex(this TreeView treeView)
		{
			int index = 1;
			((IEnumerable)((Container)treeView).get_Children()).OfType<TreeNodeBase>().CalculateIndices(ref index);
		}

		public static void CalculateIndices(this IEnumerable<TreeNodeBase> nodes, ref int index)
		{
			foreach (TreeNodeBase node in nodes)
			{
				node.NodeIndex = index;
				index++;
				(from t in ((IEnumerable)((Container)node).get_Children()).OfType<TreeNodeBase>()
					where ((Control)t).get_Visible()
					select t).CalculateIndices(ref index);
			}
		}
	}
}
