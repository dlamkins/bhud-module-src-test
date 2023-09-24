using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Settings;
using Blish_HUD.Settings.UI.Views;
using Manlaan.CommanderMarkers.Settings.Enums;
using Manlaan.CommanderMarkers.Settings.Views.Generic;
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
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			bool isChild = nestingLevel % 2 != 0;
			int num;
			switch (orientation)
			{
			case Layout.Vertical:
				if (isChild)
				{
					return (ControlFlowDirection)3;
				}
				num = 1;
				break;
			case Layout.Horizontal:
				if (isChild)
				{
					return (ControlFlowDirection)2;
				}
				num = 3;
				break;
			default:
				num = 4;
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
				case 4:
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

		public static FlowPanel AddSpace(this FlowPanel panel, int height = 0)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Expected O, but got Unknown
			ViewContainer val = new ViewContainer();
			((Control)val).set_Parent((Container)(object)panel);
			ViewContainer _ = val;
			if (height > 0)
			{
				((Control)_).set_Height(height);
			}
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
