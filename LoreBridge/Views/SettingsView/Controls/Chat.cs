using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Settings;
using LoreBridge.Models;
using Microsoft.Xna.Framework;

namespace LoreBridge.Views.SettingsView.Controls
{
	public class Chat
	{
		public Chat(Panel mainPanel, Settings settings)
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Expected O, but got Unknown
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Expected O, but got Unknown
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0151: Unknown result type (might be due to invalid IL or missing references)
			//IL_0158: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_016a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0176: Expected O, but got Unknown
			//IL_0176: Unknown result type (might be due to invalid IL or missing references)
			//IL_017b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0183: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0195: Unknown result type (might be due to invalid IL or missing references)
			//IL_019d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0201: Unknown result type (might be due to invalid IL or missing references)
			//IL_0225: Unknown result type (might be due to invalid IL or missing references)
			//IL_022c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0234: Unknown result type (might be due to invalid IL or missing references)
			//IL_023b: Unknown result type (might be due to invalid IL or missing references)
			//IL_023c: Unknown result type (might be due to invalid IL or missing references)
			//IL_024b: Expected O, but got Unknown
			//IL_025c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0261: Unknown result type (might be due to invalid IL or missing references)
			//IL_0268: Unknown result type (might be due to invalid IL or missing references)
			//IL_026f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0276: Unknown result type (might be due to invalid IL or missing references)
			//IL_027d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0288: Unknown result type (might be due to invalid IL or missing references)
			//IL_0294: Expected O, but got Unknown
			//IL_0294: Unknown result type (might be due to invalid IL or missing references)
			//IL_0299: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02df: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0302: Unknown result type (might be due to invalid IL or missing references)
			//IL_0323: Unknown result type (might be due to invalid IL or missing references)
			//IL_0328: Unknown result type (might be due to invalid IL or missing references)
			//IL_032f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0336: Unknown result type (might be due to invalid IL or missing references)
			//IL_033d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0344: Unknown result type (might be due to invalid IL or missing references)
			//IL_034f: Unknown result type (might be due to invalid IL or missing references)
			//IL_035b: Expected O, but got Unknown
			//IL_035b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0360: Unknown result type (might be due to invalid IL or missing references)
			//IL_0368: Unknown result type (might be due to invalid IL or missing references)
			//IL_0373: Unknown result type (might be due to invalid IL or missing references)
			//IL_037a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0382: Unknown result type (might be due to invalid IL or missing references)
			//IL_038e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0393: Unknown result type (might be due to invalid IL or missing references)
			//IL_039b: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0400: Unknown result type (might be due to invalid IL or missing references)
			//IL_040c: Expected O, but got Unknown
			//IL_040c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0411: Unknown result type (might be due to invalid IL or missing references)
			//IL_0419: Unknown result type (might be due to invalid IL or missing references)
			//IL_0424: Unknown result type (might be due to invalid IL or missing references)
			//IL_042b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0433: Unknown result type (might be due to invalid IL or missing references)
			//IL_043f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0444: Unknown result type (might be due to invalid IL or missing references)
			//IL_044c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0467: Unknown result type (might be due to invalid IL or missing references)
			//IL_0485: Unknown result type (might be due to invalid IL or missing references)
			//IL_048a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0491: Unknown result type (might be due to invalid IL or missing references)
			//IL_0498: Unknown result type (might be due to invalid IL or missing references)
			//IL_049f: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_04bd: Expected O, but got Unknown
			//IL_04bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_04dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_04fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0518: Unknown result type (might be due to invalid IL or missing references)
			//IL_054b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0550: Unknown result type (might be due to invalid IL or missing references)
			//IL_0557: Unknown result type (might be due to invalid IL or missing references)
			//IL_0572: Unknown result type (might be due to invalid IL or missing references)
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent((Container)(object)mainPanel);
			((Panel)val).set_Title("Chat");
			((Panel)val).set_CanCollapse(true);
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			val.set_FlowDirection((ControlFlowDirection)3);
			val.set_OuterControlPadding(new Vector2(6f, 6f));
			val.set_ControlPadding(new Vector2(6f, 6f));
			((Panel)val).set_ShowBorder(true);
			FlowPanel translationWindowPanel = val;
			FlowPanel val2 = new FlowPanel();
			((Control)val2).set_Parent((Container)(object)translationWindowPanel);
			val2.set_FlowDirection((ControlFlowDirection)0);
			((Container)val2).set_WidthSizingMode((SizingMode)2);
			((Container)val2).set_HeightSizingMode((SizingMode)1);
			val2.set_ControlPadding(new Vector2(6f, 0f));
			FlowPanel autoTranslateNpcDialoguesPanel = val2;
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)autoTranslateNpcDialoguesPanel);
			val3.set_Text("Auto translate NPC dialogs");
			((Control)val3).set_BasicTooltipText("ArcDPS, ArcDPS Unofficial Extras, ArcDPS Blish HUD plugin must be installed");
			val3.set_ShowShadow(true);
			((Control)val3).set_Height(16);
			((Control)val3).set_Width(180);
			Checkbox val4 = new Checkbox();
			((Control)val4).set_Parent((Container)(object)autoTranslateNpcDialoguesPanel);
			val4.set_Checked(settings.TranslationAutoTranslateNpcDialogs.get_Value());
			((Control)val4).set_Height(16);
			val4.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object o, CheckChangedEvent e)
			{
				settings.TranslationAutoTranslateNpcDialogs.set_Value(e.get_Checked());
			});
			FlowPanel val5 = new FlowPanel();
			((Control)val5).set_Parent((Container)(object)translationWindowPanel);
			val5.set_FlowDirection((ControlFlowDirection)0);
			((Container)val5).set_WidthSizingMode((SizingMode)2);
			((Container)val5).set_HeightSizingMode((SizingMode)1);
			val5.set_ControlPadding(new Vector2(6f, 0f));
			FlowPanel fontSizePanel = val5;
			Label val6 = new Label();
			((Control)val6).set_Parent((Container)(object)fontSizePanel);
			val6.set_Text("Font size");
			val6.set_ShowShadow(true);
			((Control)val6).set_Height(16);
			((Control)val6).set_Width(180);
			TrackBar val7 = new TrackBar();
			((Control)val7).set_Parent((Container)(object)fontSizePanel);
			val7.set_MinValue(16f);
			val7.set_MaxValue(32f);
			((Control)val7).set_Width(160);
			val7.set_Value((float)settings.WindowFontSize.get_Value());
			Label val8 = new Label();
			((Control)val8).set_Parent((Container)(object)fontSizePanel);
			val8.set_Text(settings.WindowFontSize.get_Value().ToString());
			val8.set_ShowShadow(true);
			((Control)val8).set_Height(16);
			val8.set_AutoSizeWidth(true);
			val8.set_TextColor(Color.get_Gray());
			Label fontSizeCurrentLabel = val8;
			val7.add_ValueChanged((EventHandler<ValueEventArgs<float>>)delegate(object o, ValueEventArgs<float> e)
			{
				settings.WindowFontSize.set_Value((int)e.get_Value());
				fontSizeCurrentLabel.set_Text(settings.WindowFontSize.get_Value().ToString());
			});
			FlowPanel val9 = new FlowPanel();
			((Control)val9).set_Parent((Container)(object)translationWindowPanel);
			val9.set_FlowDirection((ControlFlowDirection)0);
			((Container)val9).set_WidthSizingMode((SizingMode)2);
			((Container)val9).set_HeightSizingMode((SizingMode)1);
			val9.set_ControlPadding(new Vector2(6f, 0f));
			FlowPanel fixedWindowPanel = val9;
			Label val10 = new Label();
			((Control)val10).set_Parent((Container)(object)fixedWindowPanel);
			val10.set_Text("Fixed");
			val10.set_ShowShadow(true);
			((Control)val10).set_Height(16);
			((Control)val10).set_Width(180);
			((Control)val10).set_BasicTooltipText("Prevents the window from resizing and moving");
			Checkbox val11 = new Checkbox();
			((Control)val11).set_Parent((Container)(object)fixedWindowPanel);
			val11.set_Checked(settings.WindowFixed.get_Value());
			((Control)val11).set_Height(16);
			((Control)val11).set_BasicTooltipText("Prevents the window from resizing and moving");
			val11.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object o, CheckChangedEvent e)
			{
				settings.WindowFixed.set_Value(e.get_Checked());
			});
			FlowPanel val12 = new FlowPanel();
			((Control)val12).set_Parent((Container)(object)translationWindowPanel);
			val12.set_FlowDirection((ControlFlowDirection)0);
			((Container)val12).set_WidthSizingMode((SizingMode)2);
			((Container)val12).set_HeightSizingMode((SizingMode)1);
			val12.set_ControlPadding(new Vector2(6f, 0f));
			FlowPanel transparentWindowPanel = val12;
			Label val13 = new Label();
			((Control)val13).set_Parent((Container)(object)transparentWindowPanel);
			val13.set_Text("Transparent");
			val13.set_ShowShadow(true);
			((Control)val13).set_Height(16);
			((Control)val13).set_Width(180);
			Checkbox val14 = new Checkbox();
			((Control)val14).set_Parent((Container)(object)transparentWindowPanel);
			val14.set_Checked(settings.WindowTransparent.get_Value());
			((Control)val14).set_Height(16);
			val14.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object o, CheckChangedEvent e)
			{
				settings.WindowTransparent.set_Value(e.get_Checked());
			});
			FlowPanel val15 = new FlowPanel();
			((Control)val15).set_Parent((Container)(object)translationWindowPanel);
			val15.set_FlowDirection((ControlFlowDirection)0);
			((Container)val15).set_WidthSizingMode((SizingMode)2);
			((Container)val15).set_HeightSizingMode((SizingMode)1);
			val15.set_ControlPadding(new Vector2(6f, 0f));
			FlowPanel coloredNamesPanel = val15;
			Label val16 = new Label();
			((Control)val16).set_Parent((Container)(object)coloredNamesPanel);
			val16.set_Text("Colored names");
			val16.set_ShowShadow(true);
			((Control)val16).set_Height(16);
			((Control)val16).set_Width(180);
			Checkbox val17 = new Checkbox();
			((Control)val17).set_Parent((Container)(object)coloredNamesPanel);
			val17.set_Checked(settings.WindowColoredNames.get_Value());
			((Control)val17).set_Height(16);
			val17.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object o, CheckChangedEvent e)
			{
				settings.WindowColoredNames.set_Value(e.get_Checked());
			});
			FlowPanel val18 = new FlowPanel();
			((Control)val18).set_Parent((Container)(object)translationWindowPanel);
			val18.set_FlowDirection((ControlFlowDirection)0);
			((Container)val18).set_WidthSizingMode((SizingMode)2);
			((Container)val18).set_HeightSizingMode((SizingMode)1);
			val18.set_ControlPadding(new Vector2(6f, 0f));
			FlowPanel showTimePanel = val18;
			Label val19 = new Label();
			((Control)val19).set_Parent((Container)(object)showTimePanel);
			val19.set_Text("Show time");
			val19.set_ShowShadow(true);
			((Control)val19).set_Height(16);
			((Control)val19).set_Width(180);
			Checkbox val20 = new Checkbox();
			((Control)val20).set_Parent((Container)(object)showTimePanel);
			val20.set_Checked(settings.WindowShowTime.get_Value());
			((Control)val20).set_Height(16);
			val20.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object o, CheckChangedEvent e)
			{
				settings.WindowShowTime.set_Value(e.get_Checked());
			});
			KeybindingAssigner val21 = new KeybindingAssigner(settings.ToggleTranslationWindowHotKey.get_Value());
			((Control)val21).set_Parent((Container)(object)translationWindowPanel);
			val21.set_KeyBindingName(((SettingEntry)settings.ToggleTranslationWindowHotKey).get_DisplayName());
			((Control)val21).set_BasicTooltipText(((SettingEntry)settings.ToggleTranslationWindowHotKey).get_Description());
		}
	}
}
