using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD.Controls;

namespace DecorBlishhudModule.Sections.LeftSideTasks
{
	internal static class OrderDecorations
	{
		public static async Task OrderDecorationsAsync(FlowPanel decorationsFlowPanel, bool _isIconView)
		{
			foreach (FlowPanel categoryFlowPanel in ((IEnumerable)((Container)decorationsFlowPanel).get_Children()).OfType<FlowPanel>())
			{
				bool hasVisibleDecoration = false;
				List<Panel> visibleDecorations = new List<Panel>();
				foreach (Panel decorationIconPanel in ((IEnumerable)((Container)categoryFlowPanel).get_Children()).OfType<Panel>())
				{
					if (((IEnumerable)((Container)decorationIconPanel).get_Children()).OfType<Image>().FirstOrDefault() != null)
					{
						((Control)decorationIconPanel).set_Visible(true);
						visibleDecorations.Add(decorationIconPanel);
						hasVisibleDecoration = true;
					}
				}
				((Control)categoryFlowPanel).set_Visible(hasVisibleDecoration);
				if (!hasVisibleDecoration)
				{
					continue;
				}
				visibleDecorations.Sort((Panel a, Panel b) => string.Compare(((Control)((IEnumerable)((Container)a).get_Children()).OfType<Image>().FirstOrDefault()).get_BasicTooltipText(), ((Control)((IEnumerable)((Container)b).get_Children()).OfType<Image>().FirstOrDefault()).get_BasicTooltipText(), StringComparison.OrdinalIgnoreCase));
				foreach (Panel visibleDecoration2 in visibleDecorations)
				{
					((Container)categoryFlowPanel).get_Children().Remove((Control)(object)visibleDecoration2);
				}
				foreach (Panel visibleDecoration in visibleDecorations)
				{
					((Container)categoryFlowPanel).get_Children().Add((Control)(object)visibleDecoration);
				}
				await AdjustCategoryHeight.AdjustCategoryHeightAsync(categoryFlowPanel, _isIconView);
			}
		}
	}
}
