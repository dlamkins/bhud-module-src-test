using System;
using Blish_HUD.Controls;

namespace Universal_Search_Module
{
	public static class LabelUtil
	{
		public static void HandleMaxWidth(Label control, int maxWidth, int offset = 0, Action afterRecalculate = null)
		{
			if (((Control)control).get_Width() > maxWidth - offset)
			{
				control.set_AutoSizeWidth(false);
				((Control)control).set_Width(maxWidth - offset);
				control.set_WrapText(true);
				((Control)control).RecalculateLayout();
			}
		}
	}
}
