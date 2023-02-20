using Blish_HUD.Controls;
using Kenedia.Modules.Core.Controls;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.Core.Utility
{
	public static class UI
	{
		public static (Label, CtrlT) CreateLabeledControl<CtrlT>(Container parent, string text, int labelWidth = 175, int controlWidth = 100, int height = 25) where CtrlT : Control, new()
		{
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			Panel panel = new Panel();
			((Control)panel).set_Parent(parent);
			((Container)panel).set_WidthSizingMode((SizingMode)1);
			((Container)panel).set_HeightSizingMode((SizingMode)1);
			Panel p = panel;
			Label label2 = new Label();
			((Control)label2).set_Parent((Container)(object)p);
			((Control)label2).set_Width(labelWidth);
			((Control)label2).set_Height(height);
			((Label)label2).set_Text(text);
			Label label = label2;
			CtrlT val = new CtrlT();
			((Control)val).set_Location(new Point(((Control)label).get_Right() + 5, 0));
			((Control)val).set_Width(controlWidth);
			((Control)val).set_Height(height);
			((Control)val).set_Parent((Container)(object)p);
			CtrlT num = val;
			return (label, num);
		}
	}
}
