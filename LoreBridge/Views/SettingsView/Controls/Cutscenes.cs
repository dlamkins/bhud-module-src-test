using Blish_HUD.Controls;
using LoreBridge.Models;
using Microsoft.Xna.Framework;

namespace LoreBridge.Views.SettingsView.Controls
{
	public class Cutscenes
	{
		public Cutscenes(Panel mainPanel, Settings settings)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent((Container)(object)mainPanel);
			((Panel)val).set_Title("Cutscenes (Experimental)");
			((Panel)val).set_CanCollapse(true);
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			val.set_FlowDirection((ControlFlowDirection)3);
			val.set_OuterControlPadding(new Vector2(6f, 6f));
			val.set_ControlPadding(new Vector2(6f, 6f));
			((Panel)val).set_ShowBorder(true);
		}
	}
}
