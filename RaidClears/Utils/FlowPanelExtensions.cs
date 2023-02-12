using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Settings;
using Blish_HUD.Settings.UI.Views;
using Microsoft.Xna.Framework;
using RaidClears.Settings.Enums;
using RaidClears.Settings.Views;
using RaidClears.Settings.Views.Tabs;

namespace RaidClears.Utils
{
	public static class FlowPanelExtensions
	{
		public static void VisiblityChanged(this FlowPanel panel, SettingEntry<bool> setting)
		{
			FlowPanel panel2 = panel;
			setting.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)delegate(object _, ValueChangedEventArgs<bool> e)
			{
				((Control)panel2).set_Visible(e.get_NewValue());
				Container parent2 = ((Control)panel2).get_Parent();
				if (parent2 != null)
				{
					((Control)parent2).Invalidate();
				}
			});
			((Control)panel2).set_Visible(setting.get_Value());
			Container parent = ((Control)panel2).get_Parent();
			if (parent != null)
			{
				((Control)parent).Invalidate();
			}
		}

		public static void InvertedVisiblityChanged(this FlowPanel panel, SettingEntry<bool> setting)
		{
			FlowPanel panel2 = panel;
			setting.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)delegate(object _, ValueChangedEventArgs<bool> e)
			{
				((Control)panel2).set_Visible(!e.get_NewValue());
				Container parent2 = ((Control)panel2).get_Parent();
				if (parent2 != null)
				{
					((Control)parent2).Invalidate();
				}
			});
			((Control)panel2).set_Visible(!setting.get_Value());
			Container parent = ((Control)panel2).get_Parent();
			if (parent != null)
			{
				((Control)parent).Invalidate();
			}
		}

		public static void LayoutChange(this FlowPanel panel, SettingEntry<Layout> setting, int nestingLevel = 0)
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			FlowPanel panel2 = panel;
			setting.add_SettingChanged((EventHandler<ValueChangedEventArgs<Layout>>)delegate(object _, ValueChangedEventArgs<Layout> e)
			{
				//IL_0012: Unknown result type (might be due to invalid IL or missing references)
				panel2.set_FlowDirection(GetFlowDirection(e.get_NewValue(), nestingLevel));
			});
			panel2.set_FlowDirection(GetFlowDirection(setting.get_Value(), nestingLevel));
		}

		private static ControlFlowDirection GetFlowDirection(Layout orientation, int nestingLevel = 0)
		{
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			bool isChild = nestingLevel % 2 != 0;
			int num;
			switch (orientation)
			{
			case Layout.Horizontal:
				if (isChild)
				{
					return (ControlFlowDirection)3;
				}
				num = 1;
				break;
			case Layout.Vertical:
				if (isChild)
				{
					return (ControlFlowDirection)2;
				}
				num = 3;
				break;
			case Layout.SingleRow:
				if (isChild)
				{
					return (ControlFlowDirection)2;
				}
				num = 5;
				break;
			case Layout.SingleColumn:
				if (isChild)
				{
					return (ControlFlowDirection)3;
				}
				num = 7;
				break;
			default:
				num = 8;
				break;
			}
			if (!isChild)
			{
				switch (num)
				{
				case 1:
					return (ControlFlowDirection)2;
				case 3:
					return (ControlFlowDirection)3;
				case 5:
					return (ControlFlowDirection)2;
				case 7:
					return (ControlFlowDirection)3;
				case 8:
					return (ControlFlowDirection)3;
				}
			}
			return (ControlFlowDirection)2;
		}

		public static FlowPanel BeginFlow(this FlowPanel panel, Container parent, Point sizeOffset, Point locationOffset)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			panel.set_FlowDirection((ControlFlowDirection)3);
			panel.set_OuterControlPadding(new Vector2(20f, 25f));
			((Control)panel).set_Parent(parent);
			((Control)panel).set_Size(((Control)parent).get_Size() + sizeOffset);
			((Panel)panel).set_ShowBorder(true);
			((Control)panel).set_Location(((Control)panel).get_Location() + locationOffset);
			return panel;
		}

		public static FlowPanel BeginFlow(this FlowPanel panel, Container parent)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			return panel.BeginFlow(parent, new Point(0), new Point(0));
		}

		public static FlowPanel AddSetting(this FlowPanel panel, SettingEntry setting)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Expected O, but got Unknown
			ViewContainer val = new ViewContainer();
			((Control)val).set_Parent((Container)(object)panel);
			ViewContainer viewContainer = val;
			if (setting.get_SettingType() == typeof(bool))
			{
				viewContainer.Show(FixedWidthBoolSettingView.FromSetting((SettingEntry<bool>)(object)setting, ((Control)panel).get_Width()));
			}
			else
			{
				viewContainer.Show(SettingView.FromType(setting, ((Control)panel).get_Width()));
			}
			return panel;
		}

		public static FlowPanel AddSetting(this FlowPanel panel, IEnumerable<SettingEntry>? settings)
		{
			if (settings == null)
			{
				return panel;
			}
			foreach (SettingEntry setting in settings!)
			{
				panel.AddSetting(setting);
			}
			return panel;
		}

		public static FlowPanel AddSettingEnum(this FlowPanel panel, SettingEntry enumSetting)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			ViewContainer val = new ViewContainer();
			((Control)val).set_Parent((Container)(object)panel);
			val.Show(AlignedEnumSettingView.FromEnum(enumSetting, ((Control)panel).get_Width()));
			return panel;
		}

		public static FlowPanel AddSettingColor(this FlowPanel panel, IEnumerable<SettingEntry<string>>? colorSetting)
		{
			if (colorSetting == null)
			{
				return panel;
			}
			foreach (SettingEntry<string> color in colorSetting!)
			{
				panel.AddSettingColor(color);
			}
			return panel;
		}

		public static FlowPanel AddSettingColor(this FlowPanel panel, SettingEntry<string> colorSetting)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			ViewContainer val = new ViewContainer();
			((Control)val).set_Parent((Container)(object)panel);
			val.Show((IView)(object)new ColorSettingView(colorSetting, ((Control)panel).get_Width()));
			return panel;
		}

		public static FlowPanel AddSpace(this FlowPanel panel)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			((Control)new ViewContainer()).set_Parent((Container)(object)panel);
			return panel;
		}

		public static FlowPanel AddString(this FlowPanel panel, string text)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)panel);
			val.set_AutoSizeWidth(true);
			val.set_AutoSizeHeight(true);
			val.set_Text(text);
			val.set_WrapText(false);
			((Control)val).set_Location(new Point(25, 0));
			return panel;
		}

		public static FlowPanel AddChildPanel(this FlowPanel panel, Panel child)
		{
			((Control)child).set_Parent((Container)(object)panel);
			return panel;
		}

		public static FlowPanel Indent(this FlowPanel panel)
		{
			((Control)panel).set_Left(30);
			return panel;
		}

		public static FlowPanel AddFlowControl(this FlowPanel panel, Control control, out Control generatedControl)
		{
			control.set_Parent((Container)(object)panel);
			((Container)panel).AddChild(control);
			generatedControl = control;
			return panel;
		}

		public static FlowPanel AddFlowControl(this FlowPanel panel, Control control)
		{
			control.set_Parent((Container)(object)panel);
			((Container)panel).AddChild(control);
			return panel;
		}
	}
}
