using System.Collections.Generic;
using Blish_HUD.Controls;
using Blish_HUD.Settings;
using Blish_HUD.Settings.UI.Views;
using Microsoft.Xna.Framework;
using TargetYourFeet.Settings.Views;

namespace TargetYourFeet.Utils
{
	public static class FlowPanelExtensions
	{
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

		public static FlowPanel AddString(this FlowPanel panel, string text, Color color)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)panel);
			val.set_AutoSizeWidth(true);
			val.set_AutoSizeHeight(true);
			val.set_Text(text);
			val.set_WrapText(false);
			((Control)val).set_Location(new Point(25, 0));
			val.set_TextColor(color);
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
