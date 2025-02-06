using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD.Controls;

namespace DecorBlishhudModule.Sections.LeftSideTasks
{
	internal static class FilterDecorations
	{
		public static async Task FilterDecorationsAsync(FlowPanel decorationsFlowPanel, string searchText, bool _isIconView)
		{
			searchText = searchText.ToLower();
			foreach (FlowPanel categoryFlowPanel in ((IEnumerable)((Container)decorationsFlowPanel).get_Children()).OfType<FlowPanel>())
			{
				bool hasVisibleDecoration = false;
				List<Panel> visibleDecorations = new List<Panel>();
				foreach (Panel decorationIconPanel in ((IEnumerable)((Container)categoryFlowPanel).get_Children()).OfType<Panel>())
				{
					Image decorationIcon = ((IEnumerable)((Container)decorationIconPanel).get_Children()).OfType<Image>().FirstOrDefault();
					if (decorationIcon != null && ((Control)decorationIcon).get_Tooltip() != null)
					{
						Label tooltipLabel = ((IEnumerable)((Container)((Control)decorationIcon).get_Tooltip()).get_Children()).OfType<Label>().FirstOrDefault();
						bool matchesSearch = tooltipLabel != null && searchText.Split(' ').All((string word) => tooltipLabel.get_Text().ToLower().Contains(word));
						((Control)decorationIconPanel).set_Visible(matchesSearch);
						if (matchesSearch)
						{
							visibleDecorations.Add(decorationIconPanel);
							hasVisibleDecoration = true;
						}
					}
				}
				((Control)categoryFlowPanel).set_Visible(hasVisibleDecoration);
				if (!hasVisibleDecoration)
				{
					continue;
				}
				visibleDecorations.Sort(delegate(Panel a, Panel b)
				{
					Label obj = ((IEnumerable)((Container)((Control)((IEnumerable)((Container)a).get_Children()).OfType<Image>().FirstOrDefault()).get_Tooltip()).get_Children()).OfType<Label>().FirstOrDefault();
					string strA = ((obj != null) ? obj.get_Text() : null);
					Label obj2 = ((IEnumerable)((Container)((Control)((IEnumerable)((Container)b).get_Children()).OfType<Image>().FirstOrDefault()).get_Tooltip()).get_Children()).OfType<Label>().FirstOrDefault();
					return string.Compare(strA, (obj2 != null) ? obj2.get_Text() : null, StringComparison.OrdinalIgnoreCase);
				});
				foreach (Panel visibleDecoration2 in visibleDecorations)
				{
					((Container)categoryFlowPanel).get_Children().Remove((Control)(object)visibleDecoration2);
				}
				foreach (Panel visibleDecoration in visibleDecorations)
				{
					((Container)categoryFlowPanel).get_Children().Add((Control)(object)visibleDecoration);
				}
				await AdjustCategoryHeight.AdjustCategoryHeightAsync(categoryFlowPanel, _isIconView);
				((Control)categoryFlowPanel).Invalidate();
			}
			((Control)decorationsFlowPanel).Invalidate();
		}
	}
}
