using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD.Controls;

namespace DecorBlishhudModule.Sections.LeftSideTasks
{
	internal static class AdjustCategoryHeight
	{
		public static Task AdjustCategoryHeightAsync(FlowPanel categoryFlowPanel, bool _isIconView)
		{
			int visibleDecorationCount = ((IEnumerable)((Container)categoryFlowPanel).get_Children()).OfType<Panel>().Count((Panel p) => ((Control)p).get_Visible());
			if (visibleDecorationCount == 0)
			{
				((Control)categoryFlowPanel).set_Height(45);
			}
			else
			{
				int heightIncrementPerDecorationSet = (_isIconView ? 53 : 311);
				int numDecorationSets = (int)Math.Ceiling((double)visibleDecorationCount / (_isIconView ? 9.0 : 4.0));
				int calculatedHeight = 45 + numDecorationSets * heightIncrementPerDecorationSet;
				((Control)categoryFlowPanel).set_Height(calculatedHeight + (_isIconView ? 4 : 10));
				((Control)categoryFlowPanel).Invalidate();
			}
			return Task.CompletedTask;
		}
	}
}
