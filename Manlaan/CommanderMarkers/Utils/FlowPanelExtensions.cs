using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;

namespace Manlaan.CommanderMarkers.Utils
{
	public static class FlowPanelExtensions
	{
		public static void LayoutChange(this FlowPanel panel, SettingEntry<string> setting, int nestingLevel = 0)
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			FlowPanel panel2 = panel;
			setting.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)delegate(object _, ValueChangedEventArgs<string> e)
			{
				//IL_0012: Unknown result type (might be due to invalid IL or missing references)
				panel2.set_FlowDirection(GetFlowDirection(e.get_NewValue(), nestingLevel));
			});
			panel2.set_FlowDirection(GetFlowDirection(setting.get_Value(), nestingLevel));
		}

		private static ControlFlowDirection GetFlowDirection(string orientation, int nestingLevel = 0)
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			bool isChild = nestingLevel % 2 != 0;
			int num;
			if (!(orientation == "Vertical"))
			{
				if (orientation == "Horizontal")
				{
					if (isChild)
					{
						return (ControlFlowDirection)2;
					}
					num = 3;
				}
				else
				{
					num = 4;
				}
			}
			else
			{
				if (isChild)
				{
					return (ControlFlowDirection)3;
				}
				num = 1;
			}
			if (!isChild)
			{
				switch (num)
				{
				case 1:
					return (ControlFlowDirection)2;
				case 3:
					return (ControlFlowDirection)3;
				case 4:
					return (ControlFlowDirection)3;
				}
			}
			return (ControlFlowDirection)2;
		}

		public static void SizeChange(this FlowPanel panel, SettingEntry<int> setting)
		{
			FlowPanel panel2 = panel;
			setting.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)delegate(object _, ValueChangedEventArgs<int> e)
			{
				ResizeImageChildren(panel2, e.get_NewValue());
			});
			ResizeImageChildren(panel2, setting.get_Value());
		}

		private static void ResizeImageChildren(FlowPanel panel, int size)
		{
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			foreach (Control image in ((Container)panel).get_Children())
			{
				if (((object)image).GetType() == typeof(Image))
				{
					image.set_Size(new Point(size, size));
				}
			}
			((Control)panel).Invalidate();
		}

		public static void OpacityChange(this FlowPanel panel, SettingEntry<float> setting)
		{
			FlowPanel panel2 = panel;
			setting.add_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)delegate(object _, ValueChangedEventArgs<float> e)
			{
				((Control)panel2).set_Opacity(e.get_NewValue());
			});
			((Control)panel2).set_Opacity(setting.get_Value());
		}
	}
}
