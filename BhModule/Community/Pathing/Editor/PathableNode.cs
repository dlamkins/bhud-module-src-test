using System.Windows.Forms;
using BhModule.Community.Pathing.Entity;
using BhModule.Community.Pathing.Utility;

namespace BhModule.Community.Pathing.Editor
{
	public class PathableNode : TreeNode
	{
		public IPathingEntity PathingEntity { get; private set; }

		public PathableNode(IPathingEntity pathingEntity)
		{
			PathingEntity = pathingEntity;
			StandardMarker marker = PathingEntity as StandardMarker;
			if (marker != null)
			{
				base.SelectedImageKey = (base.ImageKey = "marker");
				base.Text = "Marker [" + marker.Guid.ToBase64String() + "]";
			}
			else if (PathingEntity is StandardTrail)
			{
				base.SelectedImageKey = (base.ImageKey = "trail");
				base.Text = "Trail";
			}
		}
	}
}
