using System;
using Blish_HUD.Controls;
using DrmTracker.UI.Controls;
using Microsoft.Xna.Framework;

namespace DrmTracker.Utils
{
	public static class UiUtils
	{
		public static (FlowPanel panel, Label label) CreateLabel(Func<string> labelText, Func<string> tooltipText, FlowPanel parent, int amount = 12, HorizontalAlignment alignment = 1)
		{
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			FlowPanel flowPanel = new FlowPanel();
			((Control)flowPanel).set_Parent((Container)(object)parent);
			((FlowPanel)flowPanel).set_FlowDirection((ControlFlowDirection)2);
			((FlowPanel)flowPanel).set_ControlPadding(new Vector2(5f));
			flowPanel.SetLocalizedTooltip = tooltipText;
			((Container)flowPanel).set_HeightSizingMode((SizingMode)1);
			FlowPanel panel = flowPanel;
			Label label2 = new Label();
			((Control)label2).set_Parent((Container)(object)panel);
			label2.SetLocalizedText = labelText;
			((Control)label2).set_Height(25);
			((Label)label2).set_VerticalAlignment((VerticalAlignment)1);
			((Label)label2).set_HorizontalAlignment(alignment);
			label2.SetLocalizedTooltip = tooltipText;
			Label label = label2;
			((Container)panel).add_ContentResized((EventHandler<RegionChangedEventArgs>)FitToPanel);
			((Container)parent).add_ContentResized((EventHandler<RegionChangedEventArgs>)FitToParent);
			return (panel, label);
			void FitToPanel(object sender, RegionChangedEventArgs e)
			{
				((Control)panel).Invalidate();
			}
			void FitToParent(object sender, RegionChangedEventArgs e)
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_0016: Unknown result type (might be due to invalid IL or missing references)
				int width = (((Container)parent).get_ContentRegion().Width - (int)(((FlowPanel)parent).get_ControlPadding().X * (float)(amount - 1))) / amount;
				((Control)panel).set_Width(width);
				((Control)label).set_Width(width);
				((Control)panel).Invalidate();
			}
		}
	}
}
