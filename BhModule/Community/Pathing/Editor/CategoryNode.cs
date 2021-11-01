using System.Linq;
using System.Windows.Forms;
using BhModule.Community.Pathing.State;
using BhModule.Community.Pathing.Utility;
using TmfLib.Pathable;

namespace BhModule.Community.Pathing.Editor
{
	public class CategoryNode : TreeNode
	{
		public PathingCategory PathingCategory { get; private set; }

		public CategoryNode(PathingCategory category, IPackState rootPackState)
		{
			PathingCategory = category;
			base.SelectedImageKey = (base.ImageKey = "category");
			base.Text = category.DisplayName + " [" + category.Name + "]";
			if (category.Any() || CategoryUtil.GetAssociatedPathingEntities(category, rootPackState.Entities.ToArray()).Any())
			{
				base.Nodes.Add("Loading...");
			}
		}
	}
}
