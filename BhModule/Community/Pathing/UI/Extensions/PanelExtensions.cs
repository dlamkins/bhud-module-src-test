using Blish_HUD.Controls;

namespace BhModule.Community.Pathing.UI.Extensions
{
	internal static class PanelExtensions
	{
		public static int ContainsChildPosition(this Container container, Control child)
		{
			foreach (Control panelChild in container.get_Children())
			{
				if (panelChild == child)
				{
					return child.get_Top();
				}
				Container childContainer = (Container)(object)((panelChild is Container) ? panelChild : null);
				if (childContainer != null)
				{
					int childPosition = childContainer.ContainsChildPosition(child);
					if (childPosition != -1)
					{
						return childPosition + ((Control)childContainer).get_Top();
					}
				}
			}
			return -1;
		}
	}
}
