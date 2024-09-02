using Blish_HUD.Controls;
using Microsoft.Xna.Framework;

namespace FarmingTracker
{
	public class SettingsFlowPanel : FlowPanel
	{
		public SettingsFlowPanel(Container parent, string title)
			: this()
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			((Panel)this).set_Title(title);
			((FlowPanel)this).set_FlowDirection((ControlFlowDirection)3);
			((Control)this).set_BackgroundColor(Color.get_Black() * 0.5f);
			((FlowPanel)this).set_OuterControlPadding(new Vector2(5f, 5f));
			((FlowPanel)this).set_ControlPadding(new Vector2(0f, 0f));
			((Panel)this).set_ShowBorder(true);
			((Control)this).set_Width(500);
			((Container)this).set_HeightSizingMode((SizingMode)1);
			((Control)this).set_Parent(parent);
		}
	}
}
