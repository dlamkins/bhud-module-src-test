using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Blish_HUD.Settings.UI.Views;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Manlaan.Mounts.Views
{
	internal class SettingsView : View
	{
		private ContentsManager ContentsManager { get; }

		private Texture2D anetTexture { get; }

		private Panel ManualPanel { get; set; }

		public SettingsView(ContentsManager contentsManager)
			: this()
		{
			ContentsManager = contentsManager;
			anetTexture = contentsManager.GetTexture("1441452.png");
		}

		private Panel CreateDefaultPanel(Container buildPanel, Point location)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Expected O, but got Unknown
			Panel val = new Panel();
			val.set_CanScroll(false);
			((Control)val).set_Parent(buildPanel);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Control)val).set_Width(420);
			((Control)val).set_Location(location);
			return val;
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Expected O, but got Unknown
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0142: Unknown result type (might be due to invalid IL or missing references)
			int labelWidth = 150;
			int labelWidth2 = 250;
			int orderWidth = 80;
			int bindingWidth = 170;
			int mountsAndRadialInputWidth = 125;
			Label val = new Label();
			((Control)val).set_Location(new Point(10, 10));
			((Control)val).set_Width(800);
			val.set_AutoSizeHeight(true);
			val.set_WrapText(true);
			((Control)val).set_Parent(buildPanel);
			val.set_TextColor(Color.get_Red());
			val.set_Font(GameService.Content.get_DefaultFont18());
			val.set_Text("For this module to work you need to fill in your in-game keykindings in the settings below.\nNo keybind means the mount is DISABLED.".Replace(" ", "  "));
			val.set_HorizontalAlignment((HorizontalAlignment)0);
			Label labelExplanation = val;
			int panelPadding = 20;
			Panel mountsPanel = CreateDefaultPanel(buildPanel, new Point(panelPadding, ((Control)labelExplanation).get_Bottom() + panelPadding));
			BuildMountsPanel(mountsPanel, labelWidth, bindingWidth, orderWidth);
			Panel otherPanel = CreateDefaultPanel(buildPanel, new Point(((Control)mountsPanel).get_Right() + panelPadding, ((Control)labelExplanation).get_Bottom() + panelPadding));
			BuildOtherPanel(otherPanel, bindingWidth, labelWidth);
			ManualPanel = CreateDefaultPanel(buildPanel, new Point(((Control)mountsPanel).get_Right() + panelPadding, 150 + panelPadding));
			BuildManualPanel(ManualPanel, buildPanel);
			Panel defaultMountPanel = CreateDefaultPanel(buildPanel, new Point(10, 350));
			BuildDefaultMountPanel(defaultMountPanel, labelWidth2, mountsAndRadialInputWidth);
			Panel radialPanel = CreateDefaultPanel(buildPanel, new Point(((Control)mountsPanel).get_Right() + 20, 350));
			BuildRadialPanel((Container)(object)radialPanel, labelWidth2, mountsAndRadialInputWidth);
			DisplayManualPanelIfNeeded();
		}

		private void BuildManualPanel(Panel manualPanel, Container buildPanel)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Expected O, but got Unknown
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Expected O, but got Unknown
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Expected O, but got Unknown
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0151: Unknown result type (might be due to invalid IL or missing references)
			//IL_0158: Unknown result type (might be due to invalid IL or missing references)
			//IL_0164: Expected O, but got Unknown
			//IL_0165: Unknown result type (might be due to invalid IL or missing references)
			//IL_016a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0179: Unknown result type (might be due to invalid IL or missing references)
			//IL_0183: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0199: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c1: Expected O, but got Unknown
			//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0200: Unknown result type (might be due to invalid IL or missing references)
			//IL_0207: Unknown result type (might be due to invalid IL or missing references)
			//IL_020e: Unknown result type (might be due to invalid IL or missing references)
			//IL_021b: Expected O, but got Unknown
			//IL_021c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0221: Unknown result type (might be due to invalid IL or missing references)
			//IL_0232: Unknown result type (might be due to invalid IL or missing references)
			//IL_023c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0247: Unknown result type (might be due to invalid IL or missing references)
			//IL_0252: Unknown result type (might be due to invalid IL or missing references)
			//IL_025d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0273: Unknown result type (might be due to invalid IL or missing references)
			//IL_027f: Expected O, but got Unknown
			//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
			Label val = new Label();
			((Control)val).set_Location(new Point(0, 2));
			((Control)val).set_Width(((Control)manualPanel).get_Width());
			val.set_AutoSizeHeight(false);
			val.set_WrapText(false);
			((Control)val).set_Parent((Container)(object)manualPanel);
			val.set_Text("Manual Settings");
			val.set_HorizontalAlignment((HorizontalAlignment)1);
			Label settingManual_Label = val;
			Label val2 = new Label();
			((Control)val2).set_Location(new Point(0, ((Control)settingManual_Label).get_Bottom() + 6));
			((Control)val2).set_Width(75);
			val2.set_AutoSizeHeight(false);
			val2.set_WrapText(false);
			((Control)val2).set_Parent((Container)(object)manualPanel);
			val2.set_Text("Orientation: ");
			Label settingManualOrientation_Label = val2;
			Dropdown val3 = new Dropdown();
			((Control)val3).set_Location(new Point(((Control)settingManualOrientation_Label).get_Right() + 5, ((Control)settingManualOrientation_Label).get_Top() - 4));
			((Control)val3).set_Width(100);
			((Control)val3).set_Parent((Container)(object)manualPanel);
			Dropdown settingManualOrientation_Select = val3;
			string[] mountOrientation = Module._mountOrientation;
			foreach (string s in mountOrientation)
			{
				settingManualOrientation_Select.get_Items().Add(s);
			}
			settingManualOrientation_Select.set_SelectedItem(Module._settingOrientation.get_Value());
			settingManualOrientation_Select.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				Module._settingOrientation.set_Value(settingManualOrientation_Select.get_SelectedItem());
			});
			Label val4 = new Label();
			((Control)val4).set_Location(new Point(0, ((Control)settingManualOrientation_Label).get_Bottom() + 6));
			((Control)val4).set_Width(75);
			val4.set_AutoSizeHeight(false);
			val4.set_WrapText(false);
			((Control)val4).set_Parent((Container)(object)manualPanel);
			val4.set_Text("Icon Width: ");
			Label settingManualWidth_Label = val4;
			TrackBar val5 = new TrackBar();
			((Control)val5).set_Location(new Point(((Control)settingManualWidth_Label).get_Right() + 5, ((Control)settingManualWidth_Label).get_Top()));
			((Control)val5).set_Width(220);
			val5.set_MaxValue(200f);
			val5.set_MinValue(0f);
			val5.set_Value((float)Module._settingImgWidth.get_Value());
			((Control)val5).set_Parent((Container)(object)manualPanel);
			TrackBar settingImgWidth_Slider = val5;
			settingImgWidth_Slider.add_ValueChanged((EventHandler<ValueEventArgs<float>>)delegate
			{
				Module._settingImgWidth.set_Value((int)settingImgWidth_Slider.get_Value());
			});
			Label val6 = new Label();
			((Control)val6).set_Location(new Point(0, ((Control)settingManualWidth_Label).get_Bottom() + 6));
			((Control)val6).set_Width(75);
			val6.set_AutoSizeHeight(false);
			val6.set_WrapText(false);
			((Control)val6).set_Parent((Container)(object)manualPanel);
			val6.set_Text("Opacity: ");
			Label settingManualOpacity_Label = val6;
			TrackBar val7 = new TrackBar();
			((Control)val7).set_Location(new Point(((Control)settingManualOpacity_Label).get_Right() + 5, ((Control)settingManualOpacity_Label).get_Top()));
			((Control)val7).set_Width(220);
			val7.set_MaxValue(100f);
			val7.set_MinValue(0f);
			val7.set_Value(Module._settingOpacity.get_Value() * 100f);
			((Control)val7).set_Parent((Container)(object)manualPanel);
			TrackBar settingOpacity_Slider = val7;
			settingOpacity_Slider.add_ValueChanged((EventHandler<ValueEventArgs<float>>)delegate
			{
				Module._settingOpacity.set_Value(settingOpacity_Slider.get_Value() / 100f);
			});
			IView settingClockDrag_View = SettingView.FromType((SettingEntry)(object)Module._settingDrag, ((Control)buildPanel).get_Width());
			ViewContainer val8 = new ViewContainer();
			((Container)val8).set_WidthSizingMode((SizingMode)2);
			((Control)val8).set_Location(new Point(0, ((Control)settingManualOpacity_Label).get_Bottom() + 3));
			((Control)val8).set_Parent((Container)(object)manualPanel);
			val8.Show(settingClockDrag_View);
		}

		private void BuildOtherPanel(Panel otherPanel, int bindingWidth, int labelWidth)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Expected O, but got Unknown
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Expected O, but got Unknown
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Expected O, but got Unknown
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			//IL_016a: Expected O, but got Unknown
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_0186: Unknown result type (might be due to invalid IL or missing references)
			//IL_0190: Unknown result type (might be due to invalid IL or missing references)
			//IL_019a: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01af: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c2: Expected O, but got Unknown
			//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_020d: Expected O, but got Unknown
			Label val = new Label();
			((Control)val).set_Location(new Point(0, 4));
			((Control)val).set_Width(labelWidth);
			val.set_AutoSizeHeight(false);
			val.set_WrapText(false);
			((Control)val).set_Parent((Container)(object)otherPanel);
			val.set_Text("Display: ");
			Label settingDisplay_Label = val;
			Dropdown val2 = new Dropdown();
			((Control)val2).set_Location(new Point(((Control)settingDisplay_Label).get_Right() + 5, ((Control)settingDisplay_Label).get_Top() - 4));
			((Control)val2).set_Width(160);
			((Control)val2).set_Parent((Container)(object)otherPanel);
			Dropdown settingDisplay_Select = val2;
			string[] mountDisplay = Module._mountDisplay;
			foreach (string s in mountDisplay)
			{
				settingDisplay_Select.get_Items().Add(s);
			}
			settingDisplay_Select.set_SelectedItem(Module._settingDisplay.get_Value());
			settingDisplay_Select.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				Module._settingDisplay.set_Value(settingDisplay_Select.get_SelectedItem());
			});
			Label val3 = new Label();
			((Control)val3).set_Location(new Point(0, ((Control)settingDisplay_Label).get_Bottom() + 6));
			((Control)val3).set_Width(bindingWidth);
			val3.set_AutoSizeHeight(false);
			val3.set_WrapText(false);
			((Control)val3).set_Parent((Container)(object)otherPanel);
			val3.set_Text("Display Corner Icons: ");
			Label settingDisplayCornerIcons_Label = val3;
			Checkbox val4 = new Checkbox();
			((Control)val4).set_Size(new Point(bindingWidth, 20));
			((Control)val4).set_Parent((Container)(object)otherPanel);
			val4.set_Checked(Module._settingDisplayCornerIcons.get_Value());
			((Control)val4).set_Location(new Point(((Control)settingDisplayCornerIcons_Label).get_Right() + 5, ((Control)settingDisplayCornerIcons_Label).get_Top() - 1));
			Checkbox settingDisplayCornerIcons_Checkbox = val4;
			settingDisplayCornerIcons_Checkbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				Module._settingDisplayCornerIcons.set_Value(settingDisplayCornerIcons_Checkbox.get_Checked());
			});
			Label val5 = new Label();
			((Control)val5).set_Location(new Point(0, ((Control)settingDisplayCornerIcons_Label).get_Bottom() + 6));
			((Control)val5).set_Width(bindingWidth);
			val5.set_AutoSizeHeight(false);
			val5.set_WrapText(false);
			((Control)val5).set_Parent((Container)(object)otherPanel);
			val5.set_Text("Display Manual Icons: ");
			Label settingDisplayManualIcons_Label = val5;
			Checkbox val6 = new Checkbox();
			((Control)val6).set_Size(new Point(bindingWidth, 20));
			((Control)val6).set_Parent((Container)(object)otherPanel);
			val6.set_Checked(Module._settingDisplayManualIcons.get_Value());
			((Control)val6).set_Location(new Point(((Control)settingDisplayManualIcons_Label).get_Right() + 5, ((Control)settingDisplayManualIcons_Label).get_Top() - 1));
			Checkbox settingDisplayManualIcons_Checkbox = val6;
			settingDisplayManualIcons_Checkbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				Module._settingDisplayManualIcons.set_Value(settingDisplayManualIcons_Checkbox.get_Checked());
				DisplayManualPanelIfNeeded();
			});
		}

		private void BuildMountsPanel(Panel mountsLeftPanel, int labelWidth, int bindingWidth, int orderWidth)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Expected O, but got Unknown
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Expected O, but got Unknown
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Expected O, but got Unknown
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Expected O, but got Unknown
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_019c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01eb: Expected O, but got Unknown
			//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0205: Unknown result type (might be due to invalid IL or missing references)
			//IL_020f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0217: Unknown result type (might be due to invalid IL or missing references)
			//IL_0223: Expected O, but got Unknown
			//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0305: Unknown result type (might be due to invalid IL or missing references)
			//IL_031d: Unknown result type (might be due to invalid IL or missing references)
			//IL_032c: Expected O, but got Unknown
			Image val = new Image();
			((Control)val).set_Parent((Container)(object)mountsLeftPanel);
			((Control)val).set_Size(new Point(16, 16));
			((Control)val).set_Location(new Point(5, 2));
			val.set_Texture(AsyncTexture2D.op_Implicit(anetTexture));
			Image anetImage = val;
			Label val2 = new Label();
			((Control)val2).set_Location(new Point(((Control)anetImage).get_Right() + 3, ((Control)anetImage).get_Bottom() - 16));
			((Control)val2).set_Width(300);
			val2.set_AutoSizeHeight(false);
			val2.set_WrapText(false);
			((Control)val2).set_Parent((Container)(object)mountsLeftPanel);
			val2.set_Text("must match in-game key binding");
			val2.set_HorizontalAlignment((HorizontalAlignment)0);
			Label keybindWarning_Label = val2;
			Label val3 = new Label();
			((Control)val3).set_Location(new Point(labelWidth + 5, ((Control)keybindWarning_Label).get_Bottom() + 6));
			((Control)val3).set_Width(orderWidth);
			val3.set_AutoSizeHeight(false);
			val3.set_WrapText(false);
			((Control)val3).set_Parent((Container)(object)mountsLeftPanel);
			val3.set_Text("Order");
			val3.set_HorizontalAlignment((HorizontalAlignment)1);
			Label settingOrderLeft_Label = val3;
			Label val4 = new Label();
			((Control)val4).set_Location(new Point(((Control)settingOrderLeft_Label).get_Right() + 5, ((Control)settingOrderLeft_Label).get_Top()));
			((Control)val4).set_Width(bindingWidth);
			val4.set_AutoSizeHeight(false);
			val4.set_WrapText(false);
			((Control)val4).set_Parent((Container)(object)mountsLeftPanel);
			val4.set_Text("In-game key binding");
			val4.set_HorizontalAlignment((HorizontalAlignment)1);
			Label settingBindingLeft_Label = val4;
			Image val5 = new Image();
			((Control)val5).set_Parent((Container)(object)mountsLeftPanel);
			((Control)val5).set_Size(new Point(16, 16));
			((Control)val5).set_Location(new Point(((Control)settingBindingLeft_Label).get_Right() - 20, ((Control)settingBindingLeft_Label).get_Bottom() - 16));
			val5.set_Texture(AsyncTexture2D.op_Implicit(anetTexture));
			int curY = ((Control)settingOrderLeft_Label).get_Bottom();
			foreach (Mount mount in Module._mounts)
			{
				Label val6 = new Label();
				((Control)val6).set_Location(new Point(0, curY + 6));
				((Control)val6).set_Width(labelWidth);
				val6.set_AutoSizeHeight(false);
				val6.set_WrapText(false);
				((Control)val6).set_Parent((Container)(object)mountsLeftPanel);
				val6.set_Text(mount.DisplayName + ": ");
				Label settingMount_Label = val6;
				Dropdown val7 = new Dropdown();
				((Control)val7).set_Location(new Point(((Control)settingMount_Label).get_Right() + 5, ((Control)settingMount_Label).get_Top() - 4));
				((Control)val7).set_Width(orderWidth);
				((Control)val7).set_Parent((Container)(object)mountsLeftPanel);
				Dropdown settingMount_Select = val7;
				int[] mountOrder = Module._mountOrder;
				for (int j = 0; j < mountOrder.Length; j++)
				{
					int i = mountOrder[j];
					if (i == 0)
					{
						settingMount_Select.get_Items().Add("Disabled");
					}
					else
					{
						settingMount_Select.get_Items().Add(i.ToString());
					}
				}
				settingMount_Select.set_SelectedItem((mount.OrderSetting.get_Value() == 0) ? "Disabled" : mount.OrderSetting.get_Value().ToString());
				settingMount_Select.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
				{
					if (settingMount_Select.get_SelectedItem().Equals("Disabled"))
					{
						mount.OrderSetting.set_Value(0);
					}
					else
					{
						mount.OrderSetting.set_Value(int.Parse(settingMount_Select.get_SelectedItem()));
					}
				});
				KeybindingAssigner val8 = new KeybindingAssigner(mount.KeybindingSetting.get_Value());
				val8.set_NameWidth(0);
				((Control)val8).set_Size(new Point(bindingWidth, 20));
				((Control)val8).set_Parent((Container)(object)mountsLeftPanel);
				((Control)val8).set_Location(new Point(((Control)settingMount_Select).get_Right() + 5, ((Control)settingMount_Label).get_Top() - 1));
				KeybindingAssigner settingRaptor_Keybind = val8;
				settingRaptor_Keybind.add_BindingChanged((EventHandler<EventArgs>)delegate
				{
					mount.KeybindingSetting.set_Value(settingRaptor_Keybind.get_KeyBinding());
				});
				curY = ((Control)settingMount_Label).get_Bottom();
			}
		}

		private void BuildDefaultMountPanel(Panel defaultMountPanel, int labelWidth2, int mountsAndRadialInputWidth)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Expected O, but got Unknown
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Expected O, but got Unknown
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Expected O, but got Unknown
			//IL_018f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0194: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d6: Expected O, but got Unknown
			//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0200: Unknown result type (might be due to invalid IL or missing references)
			//IL_020c: Expected O, but got Unknown
			//IL_030d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0312: Unknown result type (might be due to invalid IL or missing references)
			//IL_031d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0327: Unknown result type (might be due to invalid IL or missing references)
			//IL_032e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0335: Unknown result type (might be due to invalid IL or missing references)
			//IL_033c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0343: Unknown result type (might be due to invalid IL or missing references)
			//IL_0350: Expected O, but got Unknown
			//IL_0351: Unknown result type (might be due to invalid IL or missing references)
			//IL_0356: Unknown result type (might be due to invalid IL or missing references)
			//IL_0369: Unknown result type (might be due to invalid IL or missing references)
			//IL_0373: Unknown result type (might be due to invalid IL or missing references)
			//IL_037a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0386: Expected O, but got Unknown
			//IL_0487: Unknown result type (might be due to invalid IL or missing references)
			//IL_048c: Unknown result type (might be due to invalid IL or missing references)
			//IL_049b: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ce: Expected O, but got Unknown
			//IL_04d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_04dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_050c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0518: Expected O, but got Unknown
			//IL_0518: Unknown result type (might be due to invalid IL or missing references)
			//IL_051d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0528: Unknown result type (might be due to invalid IL or missing references)
			//IL_0532: Unknown result type (might be due to invalid IL or missing references)
			//IL_0539: Unknown result type (might be due to invalid IL or missing references)
			//IL_0540: Unknown result type (might be due to invalid IL or missing references)
			//IL_0547: Unknown result type (might be due to invalid IL or missing references)
			//IL_054e: Unknown result type (might be due to invalid IL or missing references)
			//IL_055b: Expected O, but got Unknown
			//IL_055c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0561: Unknown result type (might be due to invalid IL or missing references)
			//IL_0574: Unknown result type (might be due to invalid IL or missing references)
			//IL_057e: Unknown result type (might be due to invalid IL or missing references)
			//IL_058b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0597: Expected O, but got Unknown
			//IL_0657: Unknown result type (might be due to invalid IL or missing references)
			//IL_065c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0667: Unknown result type (might be due to invalid IL or missing references)
			//IL_0671: Unknown result type (might be due to invalid IL or missing references)
			//IL_0678: Unknown result type (might be due to invalid IL or missing references)
			//IL_067f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0686: Unknown result type (might be due to invalid IL or missing references)
			//IL_068d: Unknown result type (might be due to invalid IL or missing references)
			//IL_069a: Expected O, but got Unknown
			//IL_069b: Unknown result type (might be due to invalid IL or missing references)
			//IL_06a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_06a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_06b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_06c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_06d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_06e7: Expected O, but got Unknown
			//IL_06fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0703: Unknown result type (might be due to invalid IL or missing references)
			//IL_070e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0718: Unknown result type (might be due to invalid IL or missing references)
			//IL_071f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0726: Unknown result type (might be due to invalid IL or missing references)
			//IL_072d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0734: Unknown result type (might be due to invalid IL or missing references)
			//IL_0741: Expected O, but got Unknown
			//IL_0742: Unknown result type (might be due to invalid IL or missing references)
			//IL_0747: Unknown result type (might be due to invalid IL or missing references)
			//IL_074b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0755: Unknown result type (might be due to invalid IL or missing references)
			//IL_075c: Unknown result type (might be due to invalid IL or missing references)
			//IL_076c: Unknown result type (might be due to invalid IL or missing references)
			//IL_077f: Unknown result type (might be due to invalid IL or missing references)
			//IL_078e: Expected O, but got Unknown
			//IL_07a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_07aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_07b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_07bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_07c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_07cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_07d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_07db: Unknown result type (might be due to invalid IL or missing references)
			//IL_07e8: Expected O, but got Unknown
			//IL_07e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_07ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_07f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_07fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0803: Unknown result type (might be due to invalid IL or missing references)
			//IL_0813: Unknown result type (might be due to invalid IL or missing references)
			//IL_0826: Unknown result type (might be due to invalid IL or missing references)
			//IL_0835: Expected O, but got Unknown
			Label val = new Label();
			((Control)val).set_Location(new Point(0, 0));
			((Control)val).set_Width(labelWidth2);
			val.set_AutoSizeHeight(false);
			val.set_WrapText(false);
			((Control)val).set_Parent((Container)(object)defaultMountPanel);
			val.set_Text("Default mount settings: ");
			Label settingDefaultSettingsMount_Label = val;
			Label val2 = new Label();
			((Control)val2).set_Location(new Point(0, ((Control)settingDefaultSettingsMount_Label).get_Bottom() + 6));
			((Control)val2).set_Width(labelWidth2);
			val2.set_AutoSizeHeight(false);
			val2.set_WrapText(false);
			((Control)val2).set_Parent((Container)(object)defaultMountPanel);
			val2.set_Text("Default mount: ");
			Label settingDefaultMount_Label = val2;
			Dropdown val3 = new Dropdown();
			((Control)val3).set_Location(new Point(((Control)settingDefaultMount_Label).get_Right() + 5, ((Control)settingDefaultMount_Label).get_Top() - 4));
			((Control)val3).set_Width(mountsAndRadialInputWidth);
			((Control)val3).set_Parent((Container)(object)defaultMountPanel);
			Dropdown settingDefaultMount_Select = val3;
			settingDefaultMount_Select.get_Items().Add("Disabled");
			IEnumerable<string> mountNames = Module._mounts.Select((Mount m) => m.Name);
			foreach (string l in mountNames)
			{
				settingDefaultMount_Select.get_Items().Add(l.ToString());
			}
			settingDefaultMount_Select.set_SelectedItem(mountNames.Any((string m) => m == Module._settingDefaultMountChoice.get_Value()) ? Module._settingDefaultMountChoice.get_Value() : "Disabled");
			settingDefaultMount_Select.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				Module._settingDefaultMountChoice.set_Value(settingDefaultMount_Select.get_SelectedItem());
			});
			Label val4 = new Label();
			((Control)val4).set_Location(new Point(0, ((Control)settingDefaultMount_Select).get_Bottom() + 6));
			((Control)val4).set_Width(labelWidth2);
			val4.set_AutoSizeHeight(false);
			val4.set_WrapText(false);
			((Control)val4).set_Parent((Container)(object)defaultMountPanel);
			val4.set_Text("Default water mount: ");
			Label settingDefaultWaterMount_Label = val4;
			Dropdown val5 = new Dropdown();
			((Control)val5).set_Location(new Point(((Control)settingDefaultWaterMount_Label).get_Right() + 5, ((Control)settingDefaultWaterMount_Label).get_Top() - 4));
			((Control)val5).set_Width(mountsAndRadialInputWidth);
			((Control)val5).set_Parent((Container)(object)defaultMountPanel);
			Dropdown settingDefaultWaterMount_Select = val5;
			settingDefaultWaterMount_Select.get_Items().Add("Disabled");
			IEnumerable<string> mountNamesWater = from m in Module._mounts
				where m.IsWaterMount
				select m.Name;
			foreach (string k in mountNamesWater)
			{
				settingDefaultWaterMount_Select.get_Items().Add(k.ToString());
			}
			settingDefaultWaterMount_Select.set_SelectedItem(mountNamesWater.Any((string m) => m == Module._settingDefaultWaterMountChoice.get_Value()) ? Module._settingDefaultWaterMountChoice.get_Value() : "Disabled");
			settingDefaultWaterMount_Select.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				Module._settingDefaultWaterMountChoice.set_Value(settingDefaultWaterMount_Select.get_SelectedItem());
			});
			Label val6 = new Label();
			((Control)val6).set_Location(new Point(0, ((Control)settingDefaultWaterMount_Label).get_Bottom() + 6));
			((Control)val6).set_Width(labelWidth2);
			val6.set_AutoSizeHeight(false);
			val6.set_WrapText(false);
			((Control)val6).set_Parent((Container)(object)defaultMountPanel);
			val6.set_Text("Default flying mount: ");
			Label settingDefaultFlyingMount_Label = val6;
			Dropdown val7 = new Dropdown();
			((Control)val7).set_Location(new Point(((Control)settingDefaultFlyingMount_Label).get_Right() + 5, ((Control)settingDefaultFlyingMount_Label).get_Top() - 4));
			((Control)val7).set_Width(mountsAndRadialInputWidth);
			((Control)val7).set_Parent((Container)(object)defaultMountPanel);
			Dropdown settingDefaultFlyingMount_Select = val7;
			settingDefaultFlyingMount_Select.get_Items().Add("Disabled");
			IEnumerable<string> mountNamesFlying = from m in Module._mounts
				where m.IsFlyingMount
				select m.Name;
			foreach (string j in mountNamesFlying)
			{
				settingDefaultFlyingMount_Select.get_Items().Add(j.ToString());
			}
			settingDefaultFlyingMount_Select.set_SelectedItem(mountNamesFlying.Any((string m) => m == Module._settingDefaultFlyingMountChoice.get_Value()) ? Module._settingDefaultFlyingMountChoice.get_Value() : "Disabled");
			settingDefaultFlyingMount_Select.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				Module._settingDefaultFlyingMountChoice.set_Value(settingDefaultFlyingMount_Select.get_SelectedItem());
			});
			Label val8 = new Label();
			((Control)val8).set_Location(new Point(0, ((Control)settingDefaultFlyingMount_Select).get_Bottom() + 6));
			((Control)val8).set_Width(labelWidth2);
			val8.set_AutoSizeHeight(false);
			val8.set_WrapText(false);
			((Control)val8).set_Parent((Container)(object)defaultMountPanel);
			val8.set_Text("Key binding: ");
			Label settingDefaultMountKeybind_Label = val8;
			KeybindingAssigner val9 = new KeybindingAssigner(Module._settingDefaultMountBinding.get_Value());
			val9.set_NameWidth(0);
			((Control)val9).set_Size(new Point(mountsAndRadialInputWidth, 20));
			((Control)val9).set_Parent((Container)(object)defaultMountPanel);
			((Control)val9).set_Location(new Point(((Control)settingDefaultMountKeybind_Label).get_Right() + 4, ((Control)settingDefaultMountKeybind_Label).get_Top() - 1));
			KeybindingAssigner settingDefaultMount_Keybind = val9;
			Label val10 = new Label();
			((Control)val10).set_Location(new Point(0, ((Control)settingDefaultMountKeybind_Label).get_Bottom() + 6));
			((Control)val10).set_Width(labelWidth2);
			val10.set_AutoSizeHeight(false);
			val10.set_WrapText(false);
			((Control)val10).set_Parent((Container)(object)defaultMountPanel);
			val10.set_Text("Keybind behaviour: ");
			Label settingDefaultMountBehaviour_Label = val10;
			Dropdown val11 = new Dropdown();
			((Control)val11).set_Location(new Point(((Control)settingDefaultMountBehaviour_Label).get_Right() + 5, ((Control)settingDefaultMountBehaviour_Label).get_Top() - 4));
			((Control)val11).set_Width(((Control)settingDefaultMount_Keybind).get_Width());
			((Control)val11).set_Parent((Container)(object)defaultMountPanel);
			Dropdown settingDefaultMountBehaviour_Select = val11;
			settingDefaultMountBehaviour_Select.get_Items().Add("Disabled");
			List<string> mountBehaviours = Module._mountBehaviour.ToList();
			foreach (string i in mountBehaviours)
			{
				settingDefaultMountBehaviour_Select.get_Items().Add(i.ToString());
			}
			settingDefaultMountBehaviour_Select.set_SelectedItem(mountBehaviours.Any((string m) => m == Module._settingDefaultMountBehaviour.get_Value()) ? Module._settingDefaultMountBehaviour.get_Value() : "Disabled");
			settingDefaultMountBehaviour_Select.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				Module._settingDefaultMountBehaviour.set_Value(settingDefaultMountBehaviour_Select.get_SelectedItem());
			});
			Label val12 = new Label();
			((Control)val12).set_Location(new Point(0, ((Control)settingDefaultMountBehaviour_Label).get_Bottom() + 6));
			((Control)val12).set_Width(labelWidth2);
			val12.set_AutoSizeHeight(false);
			val12.set_WrapText(false);
			((Control)val12).set_Parent((Container)(object)defaultMountPanel);
			val12.set_Text("Display out of combat queueing:");
			Label settingDisplayMountQueueing_Label = val12;
			Checkbox val13 = new Checkbox();
			((Control)val13).set_Size(new Point(labelWidth2, 20));
			((Control)val13).set_Parent((Container)(object)defaultMountPanel);
			val13.set_Checked(Module._settingDisplayMountQueueing.get_Value());
			((Control)val13).set_Location(new Point(((Control)settingDisplayMountQueueing_Label).get_Right() + 5, ((Control)settingDisplayMountQueueing_Label).get_Top() - 1));
			Checkbox settingDisplayMountQueueing_Checkbox = val13;
			settingDisplayMountQueueing_Checkbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				Module._settingDisplayMountQueueing.set_Value(settingDisplayMountQueueing_Checkbox.get_Checked());
			});
			Label val14 = new Label();
			((Control)val14).set_Location(new Point(0, ((Control)settingDisplayMountQueueing_Label).get_Bottom() + 6));
			((Control)val14).set_Width(labelWidth2);
			val14.set_AutoSizeHeight(false);
			val14.set_WrapText(false);
			((Control)val14).set_Parent((Container)(object)defaultMountPanel);
			val14.set_Text("Display module on loading screen:");
			Label settingDisplayModuleOnLoadingScreen_Label = val14;
			Checkbox val15 = new Checkbox();
			((Control)val15).set_Size(new Point(labelWidth2, 20));
			((Control)val15).set_Parent((Container)(object)defaultMountPanel);
			val15.set_Checked(Module._settingDisplayModuleOnLoadingScreen.get_Value());
			((Control)val15).set_Location(new Point(((Control)settingDisplayModuleOnLoadingScreen_Label).get_Right() + 5, ((Control)settingDisplayModuleOnLoadingScreen_Label).get_Top() - 1));
			Checkbox settingDisplayModuleOnLoadingScreen_Checkbox = val15;
			settingDisplayModuleOnLoadingScreen_Checkbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				Module._settingDisplayModuleOnLoadingScreen.set_Value(settingDisplayModuleOnLoadingScreen_Checkbox.get_Checked());
			});
			Label val16 = new Label();
			((Control)val16).set_Location(new Point(0, ((Control)settingDisplayModuleOnLoadingScreen_Label).get_Bottom() + 6));
			((Control)val16).set_Width(labelWidth2);
			val16.set_AutoSizeHeight(false);
			val16.set_WrapText(false);
			((Control)val16).set_Parent((Container)(object)defaultMountPanel);
			val16.set_Text("Mount automatically after loading screen:");
			Label settingMountAutomaticallyAfterLoadingScreen_Label = val16;
			Checkbox val17 = new Checkbox();
			((Control)val17).set_Size(new Point(labelWidth2, 20));
			((Control)val17).set_Parent((Container)(object)defaultMountPanel);
			val17.set_Checked(Module._settingMountAutomaticallyAfterLoadingScreen.get_Value());
			((Control)val17).set_Location(new Point(((Control)settingMountAutomaticallyAfterLoadingScreen_Label).get_Right() + 5, ((Control)settingMountAutomaticallyAfterLoadingScreen_Label).get_Top() - 1));
			Checkbox settingMountAutomaticallyAfterLoadingScreen_Checkbox = val17;
			settingMountAutomaticallyAfterLoadingScreen_Checkbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				Module._settingMountAutomaticallyAfterLoadingScreen.set_Value(settingMountAutomaticallyAfterLoadingScreen_Checkbox.get_Checked());
			});
		}

		private void BuildRadialPanel(Container radialPanel, int labelWidth, int mountsAndRadialInputWidth)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Expected O, but got Unknown
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Expected O, but got Unknown
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Expected O, but got Unknown
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Expected O, but got Unknown
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0155: Unknown result type (might be due to invalid IL or missing references)
			//IL_0160: Unknown result type (might be due to invalid IL or missing references)
			//IL_0176: Unknown result type (might be due to invalid IL or missing references)
			//IL_0182: Expected O, but got Unknown
			//IL_0199: Unknown result type (might be due to invalid IL or missing references)
			//IL_019e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_01db: Expected O, but got Unknown
			//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0203: Unknown result type (might be due to invalid IL or missing references)
			//IL_020e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0219: Unknown result type (might be due to invalid IL or missing references)
			//IL_022f: Unknown result type (might be due to invalid IL or missing references)
			//IL_023b: Expected O, but got Unknown
			//IL_0252: Unknown result type (might be due to invalid IL or missing references)
			//IL_0257: Unknown result type (might be due to invalid IL or missing references)
			//IL_0262: Unknown result type (might be due to invalid IL or missing references)
			//IL_026c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0273: Unknown result type (might be due to invalid IL or missing references)
			//IL_027a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0281: Unknown result type (might be due to invalid IL or missing references)
			//IL_0288: Unknown result type (might be due to invalid IL or missing references)
			//IL_0295: Expected O, but got Unknown
			//IL_0296: Unknown result type (might be due to invalid IL or missing references)
			//IL_029b: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f5: Expected O, but got Unknown
			//IL_030c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0311: Unknown result type (might be due to invalid IL or missing references)
			//IL_031c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0326: Unknown result type (might be due to invalid IL or missing references)
			//IL_032d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0334: Unknown result type (might be due to invalid IL or missing references)
			//IL_033b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0342: Unknown result type (might be due to invalid IL or missing references)
			//IL_034f: Expected O, but got Unknown
			//IL_0350: Unknown result type (might be due to invalid IL or missing references)
			//IL_0355: Unknown result type (might be due to invalid IL or missing references)
			//IL_0366: Unknown result type (might be due to invalid IL or missing references)
			//IL_0370: Unknown result type (might be due to invalid IL or missing references)
			//IL_0377: Unknown result type (might be due to invalid IL or missing references)
			//IL_0382: Unknown result type (might be due to invalid IL or missing references)
			//IL_038d: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03af: Expected O, but got Unknown
			//IL_03c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0409: Expected O, but got Unknown
			//IL_040a: Unknown result type (might be due to invalid IL or missing references)
			//IL_040f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0422: Unknown result type (might be due to invalid IL or missing references)
			//IL_042c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0433: Unknown result type (might be due to invalid IL or missing references)
			//IL_043f: Expected O, but got Unknown
			//IL_04a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_04bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_04cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e6: Expected O, but got Unknown
			//IL_04e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_04fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0501: Unknown result type (might be due to invalid IL or missing references)
			//IL_0511: Unknown result type (might be due to invalid IL or missing references)
			//IL_0524: Unknown result type (might be due to invalid IL or missing references)
			//IL_0533: Expected O, but got Unknown
			//IL_054a: Unknown result type (might be due to invalid IL or missing references)
			//IL_054f: Unknown result type (might be due to invalid IL or missing references)
			//IL_055a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0564: Unknown result type (might be due to invalid IL or missing references)
			//IL_056b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0572: Unknown result type (might be due to invalid IL or missing references)
			//IL_0579: Unknown result type (might be due to invalid IL or missing references)
			//IL_0580: Unknown result type (might be due to invalid IL or missing references)
			//IL_058d: Expected O, but got Unknown
			//IL_058d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0592: Unknown result type (might be due to invalid IL or missing references)
			//IL_0599: Unknown result type (might be due to invalid IL or missing references)
			//IL_059e: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_05bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_05f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_05fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0602: Unknown result type (might be due to invalid IL or missing references)
			//IL_0615: Unknown result type (might be due to invalid IL or missing references)
			Label val = new Label();
			((Control)val).set_Location(new Point(0, 0));
			((Control)val).set_Width(labelWidth);
			val.set_AutoSizeHeight(false);
			val.set_WrapText(false);
			((Control)val).set_Parent(radialPanel);
			val.set_Text("Radial settings: ");
			Label settingMountRadialSettingsMount_Label = val;
			Label val2 = new Label();
			((Control)val2).set_Location(new Point(0, ((Control)settingMountRadialSettingsMount_Label).get_Bottom() + 6));
			((Control)val2).set_Width(labelWidth);
			val2.set_AutoSizeHeight(false);
			val2.set_WrapText(false);
			((Control)val2).set_Parent(radialPanel);
			val2.set_Text("Spawn at mouse: ");
			Label settingMountRadialSpawnAtMouse_Label = val2;
			Checkbox val3 = new Checkbox();
			((Control)val3).set_Size(new Point(labelWidth, 20));
			((Control)val3).set_Parent(radialPanel);
			val3.set_Checked(Module._settingMountRadialSpawnAtMouse.get_Value());
			((Control)val3).set_Location(new Point(((Control)settingMountRadialSpawnAtMouse_Label).get_Right() + 5, ((Control)settingMountRadialSpawnAtMouse_Label).get_Top() - 1));
			Checkbox settingMountRadialSpawnAtMouse_Checkbox = val3;
			settingMountRadialSpawnAtMouse_Checkbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				Module._settingMountRadialSpawnAtMouse.set_Value(settingMountRadialSpawnAtMouse_Checkbox.get_Checked());
			});
			Label val4 = new Label();
			((Control)val4).set_Location(new Point(0, ((Control)settingMountRadialSpawnAtMouse_Label).get_Bottom() + 6));
			((Control)val4).set_Width(labelWidth);
			val4.set_AutoSizeHeight(false);
			val4.set_WrapText(false);
			((Control)val4).set_Parent(radialPanel);
			val4.set_Text("Radius: ");
			Label settingMountRadialRadiusModifier_Label = val4;
			TrackBar val5 = new TrackBar();
			((Control)val5).set_Location(new Point(((Control)settingMountRadialRadiusModifier_Label).get_Right() + 5, ((Control)settingMountRadialRadiusModifier_Label).get_Top()));
			((Control)val5).set_Width(mountsAndRadialInputWidth);
			val5.set_MaxValue(100f);
			val5.set_MinValue(20f);
			val5.set_Value(Module._settingMountRadialRadiusModifier.get_Value() * 100f);
			((Control)val5).set_Parent(radialPanel);
			TrackBar settingMountRadialRadiusModifier_Slider = val5;
			settingMountRadialRadiusModifier_Slider.add_ValueChanged((EventHandler<ValueEventArgs<float>>)delegate
			{
				Module._settingMountRadialRadiusModifier.set_Value(settingMountRadialRadiusModifier_Slider.get_Value() / 100f);
			});
			Label val6 = new Label();
			((Control)val6).set_Location(new Point(0, ((Control)settingMountRadialRadiusModifier_Label).get_Bottom() + 6));
			((Control)val6).set_Width(labelWidth);
			val6.set_AutoSizeHeight(false);
			val6.set_WrapText(false);
			((Control)val6).set_Parent(radialPanel);
			val6.set_Text("Start angle: ");
			Label settingMountRadialStartAngle_Label = val6;
			TrackBar val7 = new TrackBar();
			((Control)val7).set_Location(new Point(((Control)settingMountRadialStartAngle_Label).get_Right() + 5, ((Control)settingMountRadialStartAngle_Label).get_Top()));
			((Control)val7).set_Width(mountsAndRadialInputWidth);
			val7.set_MaxValue(360f);
			val7.set_MinValue(0f);
			val7.set_Value(Module._settingMountRadialStartAngle.get_Value() * 360f);
			((Control)val7).set_Parent(radialPanel);
			TrackBar settingMountRadialStartAngle_Slider = val7;
			settingMountRadialStartAngle_Slider.add_ValueChanged((EventHandler<ValueEventArgs<float>>)delegate
			{
				Module._settingMountRadialStartAngle.set_Value(settingMountRadialStartAngle_Slider.get_Value() / 360f);
			});
			Label val8 = new Label();
			((Control)val8).set_Location(new Point(0, ((Control)settingMountRadialStartAngle_Label).get_Bottom() + 6));
			((Control)val8).set_Width(labelWidth);
			val8.set_AutoSizeHeight(false);
			val8.set_WrapText(false);
			((Control)val8).set_Parent(radialPanel);
			val8.set_Text("Icon size: ");
			Label settingMountRadialIconSizeModifier_Label = val8;
			TrackBar val9 = new TrackBar();
			((Control)val9).set_Location(new Point(((Control)settingMountRadialIconSizeModifier_Label).get_Right() + 5, ((Control)settingMountRadialIconSizeModifier_Label).get_Top()));
			((Control)val9).set_Width(mountsAndRadialInputWidth);
			val9.set_MaxValue(100f);
			val9.set_MinValue(5f);
			val9.set_Value(Module._settingMountRadialIconSizeModifier.get_Value() * 100f);
			((Control)val9).set_Parent(radialPanel);
			TrackBar settingMountRadialIconSizeModifier_Slider = val9;
			settingMountRadialIconSizeModifier_Slider.add_ValueChanged((EventHandler<ValueEventArgs<float>>)delegate
			{
				Module._settingMountRadialIconSizeModifier.set_Value(settingMountRadialIconSizeModifier_Slider.get_Value() / 100f);
			});
			Label val10 = new Label();
			((Control)val10).set_Location(new Point(0, ((Control)settingMountRadialIconSizeModifier_Label).get_Bottom() + 6));
			((Control)val10).set_Width(labelWidth);
			val10.set_AutoSizeHeight(false);
			val10.set_WrapText(false);
			((Control)val10).set_Parent(radialPanel);
			val10.set_Text("Icon opacity: ");
			Label settingMountRadialIconOpacity_Label = val10;
			TrackBar val11 = new TrackBar();
			((Control)val11).set_Location(new Point(((Control)settingMountRadialIconOpacity_Label).get_Right() + 5, ((Control)settingMountRadialIconOpacity_Label).get_Top()));
			((Control)val11).set_Width(mountsAndRadialInputWidth);
			val11.set_MaxValue(100f);
			val11.set_MinValue(5f);
			val11.set_Value(Module._settingMountRadialIconOpacity.get_Value() * 100f);
			((Control)val11).set_Parent(radialPanel);
			TrackBar settingMountRadialIconOpacity_Slider = val11;
			settingMountRadialIconOpacity_Slider.add_ValueChanged((EventHandler<ValueEventArgs<float>>)delegate
			{
				Module._settingMountRadialIconOpacity.set_Value(settingMountRadialIconOpacity_Slider.get_Value() / 100f);
			});
			Label val12 = new Label();
			((Control)val12).set_Location(new Point(0, ((Control)settingMountRadialIconOpacity_Label).get_Bottom() + 6));
			((Control)val12).set_Width(labelWidth);
			val12.set_AutoSizeHeight(false);
			val12.set_WrapText(false);
			((Control)val12).set_Parent(radialPanel);
			val12.set_Text("Center mount: ");
			Label settingMountRadialCenterMountBehavior_Label = val12;
			Dropdown val13 = new Dropdown();
			((Control)val13).set_Location(new Point(((Control)settingMountRadialCenterMountBehavior_Label).get_Right() + 5, ((Control)settingMountRadialCenterMountBehavior_Label).get_Top() - 4));
			((Control)val13).set_Width(mountsAndRadialInputWidth);
			((Control)val13).set_Parent(radialPanel);
			Dropdown settingMountRadialCenterMountBehavior_Select = val13;
			string[] mountRadialCenterMountBehavior = Module._mountRadialCenterMountBehavior;
			foreach (string i in mountRadialCenterMountBehavior)
			{
				settingMountRadialCenterMountBehavior_Select.get_Items().Add(i.ToString());
			}
			settingMountRadialCenterMountBehavior_Select.set_SelectedItem(Module._settingMountRadialCenterMountBehavior.get_Value());
			settingMountRadialCenterMountBehavior_Select.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				Module._settingMountRadialCenterMountBehavior.set_Value(settingMountRadialCenterMountBehavior_Select.get_SelectedItem());
			});
			Label val14 = new Label();
			((Control)val14).set_Location(new Point(0, ((Control)settingMountRadialCenterMountBehavior_Label).get_Bottom() + 6));
			((Control)val14).set_Width(labelWidth);
			val14.set_AutoSizeHeight(false);
			val14.set_WrapText(false);
			((Control)val14).set_Parent(radialPanel);
			val14.set_Text("Remove center mount from radial: ");
			Label settingMountRadialRemoveCenterMount_Label = val14;
			Checkbox val15 = new Checkbox();
			((Control)val15).set_Size(new Point(labelWidth, 20));
			((Control)val15).set_Parent(radialPanel);
			val15.set_Checked(Module._settingMountRadialRemoveCenterMount.get_Value());
			((Control)val15).set_Location(new Point(((Control)settingMountRadialRemoveCenterMount_Label).get_Right() + 5, ((Control)settingMountRadialRemoveCenterMount_Label).get_Top() - 1));
			Checkbox settingMountRadialRemoveCenterMount_Checkbox = val15;
			settingMountRadialRemoveCenterMount_Checkbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				Module._settingMountRadialRemoveCenterMount.set_Value(settingMountRadialRemoveCenterMount_Checkbox.get_Checked());
			});
			Label val16 = new Label();
			((Control)val16).set_Location(new Point(0, ((Control)settingMountRadialRemoveCenterMount_Label).get_Bottom() + 6));
			((Control)val16).set_Width(labelWidth);
			val16.set_AutoSizeHeight(false);
			val16.set_WrapText(false);
			((Control)val16).set_Parent(radialPanel);
			val16.set_Text("In-game action camera key binding: ");
			Label settingMountRadialToggleActionCameraKeyBinding_Label = val16;
			Image val17 = new Image();
			((Control)val17).set_Parent(radialPanel);
			((Control)val17).set_Size(new Point(16, 16));
			((Control)val17).set_Location(new Point(((Control)settingMountRadialToggleActionCameraKeyBinding_Label).get_Right() - 32, ((Control)settingMountRadialToggleActionCameraKeyBinding_Label).get_Bottom() - 16));
			val17.set_Texture(AsyncTexture2D.op_Implicit(anetTexture));
			KeybindingAssigner val18 = new KeybindingAssigner(Module._settingMountRadialToggleActionCameraKeyBinding.get_Value());
			val18.set_NameWidth(0);
			((Control)val18).set_Size(new Point(mountsAndRadialInputWidth, 20));
			((Control)val18).set_Parent(radialPanel);
			((Control)val18).set_Location(new Point(((Control)settingMountRadialToggleActionCameraKeyBinding_Label).get_Right() + 4, ((Control)settingMountRadialToggleActionCameraKeyBinding_Label).get_Top() - 1));
		}

		private void DisplayManualPanelIfNeeded()
		{
			if (Module._settingDisplayManualIcons.get_Value())
			{
				((Control)ManualPanel).Show();
			}
			else
			{
				((Control)ManualPanel).Hide();
			}
		}
	}
}
