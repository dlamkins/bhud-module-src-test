using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Settings;
using LoreBridge.Models;
using Microsoft.Xna.Framework;

namespace LoreBridge.Views.SettingsView.Controls
{
	public class Area
	{
		public Area(Panel mainPanel, Settings settings)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Expected O, but got Unknown
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Expected O, but got Unknown
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Unknown result type (might be due to invalid IL or missing references)
			//IL_0167: Unknown result type (might be due to invalid IL or missing references)
			//IL_0176: Expected O, but got Unknown
			//IL_0197: Unknown result type (might be due to invalid IL or missing references)
			//IL_019c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent((Container)(object)mainPanel);
			((Panel)val).set_Title("Area");
			((Panel)val).set_CanCollapse(true);
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			val.set_FlowDirection((ControlFlowDirection)3);
			val.set_OuterControlPadding(new Vector2(6f, 6f));
			val.set_ControlPadding(new Vector2(6f, 6f));
			((Panel)val).set_ShowBorder(true);
			FlowPanel translationAreaPanel = val;
			FlowPanel val2 = new FlowPanel();
			((Control)val2).set_Parent((Container)(object)translationAreaPanel);
			val2.set_FlowDirection((ControlFlowDirection)0);
			((Container)val2).set_WidthSizingMode((SizingMode)2);
			((Container)val2).set_HeightSizingMode((SizingMode)1);
			val2.set_ControlPadding(new Vector2(6f, 0f));
			FlowPanel areaFontSizePanel = val2;
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)areaFontSizePanel);
			val3.set_Text("Font size");
			val3.set_ShowShadow(true);
			((Control)val3).set_Height(16);
			((Control)val3).set_Width(180);
			TrackBar val4 = new TrackBar();
			((Control)val4).set_Parent((Container)(object)areaFontSizePanel);
			val4.set_MinValue(16f);
			val4.set_MaxValue(32f);
			((Control)val4).set_Width(160);
			val4.set_Value((float)settings.AreaFontSize.get_Value());
			Label val5 = new Label();
			((Control)val5).set_Parent((Container)(object)areaFontSizePanel);
			val5.set_Text(settings.AreaFontSize.get_Value().ToString());
			val5.set_ShowShadow(true);
			((Control)val5).set_Height(16);
			val5.set_AutoSizeWidth(true);
			val5.set_TextColor(Color.get_Gray());
			Label areaFontSizeCurrentLabel = val5;
			val4.add_ValueChanged((EventHandler<ValueEventArgs<float>>)delegate(object o, ValueEventArgs<float> e)
			{
				settings.AreaFontSize.set_Value((int)e.get_Value());
				areaFontSizeCurrentLabel.set_Text(settings.AreaFontSize.get_Value().ToString());
			});
			KeybindingAssigner val6 = new KeybindingAssigner(settings.ToggleCapturerHotkey.get_Value());
			((Control)val6).set_Parent((Container)(object)translationAreaPanel);
			val6.set_KeyBindingName(((SettingEntry)settings.ToggleCapturerHotkey).get_DisplayName());
			((Control)val6).set_BasicTooltipText(((SettingEntry)settings.ToggleCapturerHotkey).get_Description());
		}
	}
}
