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

		public SettingsView(ContentsManager contentsManager)
			: this()
		{
			ContentsManager = contentsManager;
			anetTexture = contentsManager.GetTexture("1441452.png");
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Expected O, but got Unknown
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Expected O, but got Unknown
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Expected O, but got Unknown
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Expected O, but got Unknown
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			//IL_015d: Unknown result type (might be due to invalid IL or missing references)
			//IL_016a: Expected O, but got Unknown
			//IL_0176: Unknown result type (might be due to invalid IL or missing references)
			//IL_017b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0184: Unknown result type (might be due to invalid IL or missing references)
			//IL_0189: Unknown result type (might be due to invalid IL or missing references)
			//IL_0194: Unknown result type (might be due to invalid IL or missing references)
			//IL_0197: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b6: Expected O, but got Unknown
			//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01da: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_020b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0215: Expected O, but got Unknown
			//IL_0215: Unknown result type (might be due to invalid IL or missing references)
			//IL_021a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0227: Unknown result type (might be due to invalid IL or missing references)
			//IL_0232: Unknown result type (might be due to invalid IL or missing references)
			//IL_023a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0242: Unknown result type (might be due to invalid IL or missing references)
			//IL_024a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0253: Unknown result type (might be due to invalid IL or missing references)
			//IL_025f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0269: Expected O, but got Unknown
			//IL_0269: Unknown result type (might be due to invalid IL or missing references)
			//IL_026e: Unknown result type (might be due to invalid IL or missing references)
			//IL_027f: Unknown result type (might be due to invalid IL or missing references)
			//IL_028a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0293: Unknown result type (might be due to invalid IL or missing references)
			//IL_029b: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c2: Expected O, but got Unknown
			//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0352: Unknown result type (might be due to invalid IL or missing references)
			//IL_0357: Unknown result type (might be due to invalid IL or missing references)
			//IL_035d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0368: Unknown result type (might be due to invalid IL or missing references)
			//IL_0370: Unknown result type (might be due to invalid IL or missing references)
			//IL_0378: Unknown result type (might be due to invalid IL or missing references)
			//IL_0380: Unknown result type (might be due to invalid IL or missing references)
			//IL_0389: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ad: Expected O, but got Unknown
			//IL_03af: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03da: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e8: Expected O, but got Unknown
			//IL_04c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_04cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_0504: Unknown result type (might be due to invalid IL or missing references)
			//IL_0514: Expected O, but got Unknown
			//IL_0553: Unknown result type (might be due to invalid IL or missing references)
			//IL_0558: Unknown result type (might be due to invalid IL or missing references)
			//IL_055b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0566: Unknown result type (might be due to invalid IL or missing references)
			//IL_056e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0576: Unknown result type (might be due to invalid IL or missing references)
			//IL_057e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0587: Unknown result type (might be due to invalid IL or missing references)
			//IL_0595: Expected O, but got Unknown
			//IL_0596: Unknown result type (might be due to invalid IL or missing references)
			//IL_059b: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d3: Expected O, but got Unknown
			//IL_0638: Unknown result type (might be due to invalid IL or missing references)
			//IL_063d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0648: Unknown result type (might be due to invalid IL or missing references)
			//IL_0653: Unknown result type (might be due to invalid IL or missing references)
			//IL_065c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0664: Unknown result type (might be due to invalid IL or missing references)
			//IL_066c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0675: Unknown result type (might be due to invalid IL or missing references)
			//IL_0683: Expected O, but got Unknown
			//IL_0684: Unknown result type (might be due to invalid IL or missing references)
			//IL_0689: Unknown result type (might be due to invalid IL or missing references)
			//IL_068e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0699: Unknown result type (might be due to invalid IL or missing references)
			//IL_06a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_06b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_06c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_06d6: Expected O, but got Unknown
			//IL_06ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_06f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_06fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0709: Unknown result type (might be due to invalid IL or missing references)
			//IL_0712: Unknown result type (might be due to invalid IL or missing references)
			//IL_071a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0722: Unknown result type (might be due to invalid IL or missing references)
			//IL_072b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0739: Expected O, but got Unknown
			//IL_073a: Unknown result type (might be due to invalid IL or missing references)
			//IL_073f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0744: Unknown result type (might be due to invalid IL or missing references)
			//IL_074f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0758: Unknown result type (might be due to invalid IL or missing references)
			//IL_0769: Unknown result type (might be due to invalid IL or missing references)
			//IL_077c: Unknown result type (might be due to invalid IL or missing references)
			//IL_078c: Expected O, but got Unknown
			//IL_07a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_07a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_07ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_07b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_07c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_07d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_07d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_07e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_07f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_07fc: Expected O, but got Unknown
			//IL_07fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0801: Unknown result type (might be due to invalid IL or missing references)
			//IL_080c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0817: Unknown result type (might be due to invalid IL or missing references)
			//IL_0820: Unknown result type (might be due to invalid IL or missing references)
			//IL_0828: Unknown result type (might be due to invalid IL or missing references)
			//IL_0830: Unknown result type (might be due to invalid IL or missing references)
			//IL_083d: Unknown result type (might be due to invalid IL or missing references)
			//IL_084b: Expected O, but got Unknown
			//IL_084c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0851: Unknown result type (might be due to invalid IL or missing references)
			//IL_0864: Unknown result type (might be due to invalid IL or missing references)
			//IL_086f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0878: Unknown result type (might be due to invalid IL or missing references)
			//IL_088a: Expected O, but got Unknown
			//IL_08ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_08f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_08ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_090a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0913: Unknown result type (might be due to invalid IL or missing references)
			//IL_091b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0923: Unknown result type (might be due to invalid IL or missing references)
			//IL_0930: Unknown result type (might be due to invalid IL or missing references)
			//IL_093e: Expected O, but got Unknown
			//IL_093f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0944: Unknown result type (might be due to invalid IL or missing references)
			//IL_0955: Unknown result type (might be due to invalid IL or missing references)
			//IL_0960: Unknown result type (might be due to invalid IL or missing references)
			//IL_096c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0978: Unknown result type (might be due to invalid IL or missing references)
			//IL_0984: Unknown result type (might be due to invalid IL or missing references)
			//IL_0996: Unknown result type (might be due to invalid IL or missing references)
			//IL_09a8: Expected O, but got Unknown
			//IL_09c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_09c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_09d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_09db: Unknown result type (might be due to invalid IL or missing references)
			//IL_09e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_09ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_09f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a01: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a0f: Expected O, but got Unknown
			//IL_0a10: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a15: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a26: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a31: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a3d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a49: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a55: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a6c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a7e: Expected O, but got Unknown
			//IL_0aa8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0aad: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ab5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ac0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0acb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ada: Expected O, but got Unknown
			int labelWidth = 150;
			int labelWidth2 = 250;
			int orderWidth = 80;
			int bindingWidth = 170;
			int mountsAndRadialInputWidth = 125;
			Panel val = new Panel();
			val.set_CanScroll(false);
			((Control)val).set_Parent(buildPanel);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Control)val).set_Width(420);
			((Control)val).set_Location(new Point(10, 10));
			Panel mountsLeftPanel = val;
			Panel val2 = new Panel();
			val2.set_CanScroll(false);
			((Control)val2).set_Parent(buildPanel);
			((Container)val2).set_HeightSizingMode((SizingMode)1);
			((Control)val2).set_Width(330);
			((Control)val2).set_Location(new Point(((Control)mountsLeftPanel).get_Right() + 20, 10));
			Panel otherPanel = val2;
			Panel val3 = new Panel();
			val3.set_CanScroll(false);
			((Control)val3).set_Parent(buildPanel);
			((Container)val3).set_HeightSizingMode((SizingMode)1);
			((Control)val3).set_Width(330);
			((Control)val3).set_Location(new Point(((Control)mountsLeftPanel).get_Right() + 20, 93));
			Panel manualPanel = val3;
			Panel val4 = new Panel();
			val4.set_CanScroll(false);
			((Control)val4).set_Parent(buildPanel);
			((Container)val4).set_HeightSizingMode((SizingMode)1);
			((Control)val4).set_Width(420);
			((Control)val4).set_Location(new Point(10, 350));
			Panel defaultMountPanel = val4;
			Panel val5 = new Panel();
			val5.set_CanScroll(false);
			((Control)val5).set_Parent(buildPanel);
			((Container)val5).set_HeightSizingMode((SizingMode)1);
			((Control)val5).set_Width(420);
			((Control)val5).set_Location(new Point(((Control)mountsLeftPanel).get_Right() + 20, 350));
			Panel radialPanel = val5;
			DisplayManualPanelIfNeeded(manualPanel);
			Image val6 = new Image();
			((Control)val6).set_Parent((Container)(object)mountsLeftPanel);
			((Control)val6).set_Size(new Point(16, 16));
			((Control)val6).set_Location(new Point(5, 2));
			val6.set_Texture(AsyncTexture2D.op_Implicit(anetTexture));
			Image anetImage = val6;
			Label val7 = new Label();
			((Control)val7).set_Location(new Point(((Control)anetImage).get_Right() + 3, ((Control)anetImage).get_Bottom() - 16));
			((Control)val7).set_Width(300);
			val7.set_AutoSizeHeight(false);
			val7.set_WrapText(false);
			((Control)val7).set_Parent((Container)(object)mountsLeftPanel);
			val7.set_Text("must match in-game key binding");
			val7.set_HorizontalAlignment((HorizontalAlignment)0);
			Label keybindWarning_Label = val7;
			Label val8 = new Label();
			((Control)val8).set_Location(new Point(labelWidth + 5, ((Control)keybindWarning_Label).get_Bottom() + 6));
			((Control)val8).set_Width(orderWidth);
			val8.set_AutoSizeHeight(false);
			val8.set_WrapText(false);
			((Control)val8).set_Parent((Container)(object)mountsLeftPanel);
			val8.set_Text("Order");
			val8.set_HorizontalAlignment((HorizontalAlignment)1);
			Label settingOrderLeft_Label = val8;
			Label val9 = new Label();
			((Control)val9).set_Location(new Point(((Control)settingOrderLeft_Label).get_Right() + 5, ((Control)settingOrderLeft_Label).get_Top()));
			((Control)val9).set_Width(bindingWidth);
			val9.set_AutoSizeHeight(false);
			val9.set_WrapText(false);
			((Control)val9).set_Parent((Container)(object)mountsLeftPanel);
			val9.set_Text("In-game key binding");
			val9.set_HorizontalAlignment((HorizontalAlignment)1);
			Label settingBindingLeft_Label = val9;
			Image val10 = new Image();
			((Control)val10).set_Parent((Container)(object)mountsLeftPanel);
			((Control)val10).set_Size(new Point(16, 16));
			((Control)val10).set_Location(new Point(((Control)settingBindingLeft_Label).get_Right() - 20, ((Control)settingBindingLeft_Label).get_Bottom() - 16));
			val10.set_Texture(AsyncTexture2D.op_Implicit(anetTexture));
			int curY = ((Control)settingOrderLeft_Label).get_Bottom();
			foreach (Mount mount in Module._mounts)
			{
				Label val11 = new Label();
				((Control)val11).set_Location(new Point(0, curY + 6));
				((Control)val11).set_Width(labelWidth);
				val11.set_AutoSizeHeight(false);
				val11.set_WrapText(false);
				((Control)val11).set_Parent((Container)(object)mountsLeftPanel);
				val11.set_Text(mount.DisplayName + ": ");
				Label settingMount_Label = val11;
				Dropdown val12 = new Dropdown();
				((Control)val12).set_Location(new Point(((Control)settingMount_Label).get_Right() + 5, ((Control)settingMount_Label).get_Top() - 4));
				((Control)val12).set_Width(orderWidth);
				((Control)val12).set_Parent((Container)(object)mountsLeftPanel);
				Dropdown settingMount_Select = val12;
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
				KeybindingAssigner val13 = new KeybindingAssigner(mount.KeybindingSetting.get_Value());
				val13.set_NameWidth(0);
				((Control)val13).set_Size(new Point(bindingWidth, 20));
				((Control)val13).set_Parent((Container)(object)mountsLeftPanel);
				((Control)val13).set_Location(new Point(((Control)settingMount_Select).get_Right() + 5, ((Control)settingMount_Label).get_Top() - 1));
				KeybindingAssigner settingRaptor_Keybind = val13;
				settingRaptor_Keybind.add_BindingChanged((EventHandler<EventArgs>)delegate
				{
					mount.KeybindingSetting.set_Value(settingRaptor_Keybind.get_KeyBinding());
				});
				curY = ((Control)settingMount_Label).get_Bottom();
			}
			Label val14 = new Label();
			((Control)val14).set_Location(new Point(0, 4));
			((Control)val14).set_Width(labelWidth);
			val14.set_AutoSizeHeight(false);
			val14.set_WrapText(false);
			((Control)val14).set_Parent((Container)(object)otherPanel);
			val14.set_Text("Display: ");
			Label settingDisplay_Label = val14;
			Dropdown val15 = new Dropdown();
			((Control)val15).set_Location(new Point(((Control)settingDisplay_Label).get_Right() + 5, ((Control)settingDisplay_Label).get_Top() - 4));
			((Control)val15).set_Width(160);
			((Control)val15).set_Parent((Container)(object)otherPanel);
			Dropdown settingDisplay_Select = val15;
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
			Label val16 = new Label();
			((Control)val16).set_Location(new Point(0, ((Control)settingDisplay_Label).get_Bottom() + 6));
			((Control)val16).set_Width(bindingWidth);
			val16.set_AutoSizeHeight(false);
			val16.set_WrapText(false);
			((Control)val16).set_Parent((Container)(object)otherPanel);
			val16.set_Text("Display Corner Icons: ");
			Label settingDisplayCornerIcons_Label = val16;
			Checkbox val17 = new Checkbox();
			((Control)val17).set_Size(new Point(bindingWidth, 20));
			((Control)val17).set_Parent((Container)(object)otherPanel);
			val17.set_Checked(Module._settingDisplayCornerIcons.get_Value());
			((Control)val17).set_Location(new Point(((Control)settingDisplayCornerIcons_Label).get_Right() + 5, ((Control)settingDisplayCornerIcons_Label).get_Top() - 1));
			Checkbox settingDisplayCornerIcons_Checkbox = val17;
			settingDisplayCornerIcons_Checkbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				Module._settingDisplayCornerIcons.set_Value(settingDisplayCornerIcons_Checkbox.get_Checked());
			});
			Label val18 = new Label();
			((Control)val18).set_Location(new Point(0, ((Control)settingDisplayCornerIcons_Label).get_Bottom() + 6));
			((Control)val18).set_Width(bindingWidth);
			val18.set_AutoSizeHeight(false);
			val18.set_WrapText(false);
			((Control)val18).set_Parent((Container)(object)otherPanel);
			val18.set_Text("Display Manual Icons: ");
			Label settingDisplayManualIcons_Label = val18;
			Checkbox val19 = new Checkbox();
			((Control)val19).set_Size(new Point(bindingWidth, 20));
			((Control)val19).set_Parent((Container)(object)otherPanel);
			val19.set_Checked(Module._settingDisplayManualIcons.get_Value());
			((Control)val19).set_Location(new Point(((Control)settingDisplayManualIcons_Label).get_Right() + 5, ((Control)settingDisplayManualIcons_Label).get_Top() - 1));
			Checkbox settingDisplayManualIcons_Checkbox = val19;
			settingDisplayManualIcons_Checkbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				Module._settingDisplayManualIcons.set_Value(settingDisplayManualIcons_Checkbox.get_Checked());
				DisplayManualPanelIfNeeded(manualPanel);
			});
			Label val20 = new Label();
			((Control)val20).set_Location(new Point(0, 2));
			((Control)val20).set_Width(((Control)manualPanel).get_Width());
			val20.set_AutoSizeHeight(false);
			val20.set_WrapText(false);
			((Control)val20).set_Parent((Container)(object)manualPanel);
			val20.set_Text("Manual Settings");
			val20.set_HorizontalAlignment((HorizontalAlignment)1);
			Label settingManual_Label = val20;
			Label val21 = new Label();
			((Control)val21).set_Location(new Point(0, ((Control)settingManual_Label).get_Bottom() + 6));
			((Control)val21).set_Width(75);
			val21.set_AutoSizeHeight(false);
			val21.set_WrapText(false);
			((Control)val21).set_Parent((Container)(object)manualPanel);
			val21.set_Text("Orientation: ");
			Label settingManualOrientation_Label = val21;
			Dropdown val22 = new Dropdown();
			((Control)val22).set_Location(new Point(((Control)settingManualOrientation_Label).get_Right() + 5, ((Control)settingManualOrientation_Label).get_Top() - 4));
			((Control)val22).set_Width(100);
			((Control)val22).set_Parent((Container)(object)manualPanel);
			Dropdown settingManualOrientation_Select = val22;
			string[] mountOrientation = Module._mountOrientation;
			foreach (string s2 in mountOrientation)
			{
				settingManualOrientation_Select.get_Items().Add(s2);
			}
			settingManualOrientation_Select.set_SelectedItem(Module._settingOrientation.get_Value());
			settingManualOrientation_Select.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				Module._settingOrientation.set_Value(settingManualOrientation_Select.get_SelectedItem());
			});
			Label val23 = new Label();
			((Control)val23).set_Location(new Point(0, ((Control)settingManualOrientation_Label).get_Bottom() + 6));
			((Control)val23).set_Width(75);
			val23.set_AutoSizeHeight(false);
			val23.set_WrapText(false);
			((Control)val23).set_Parent((Container)(object)manualPanel);
			val23.set_Text("Icon Width: ");
			Label settingManualWidth_Label = val23;
			TrackBar val24 = new TrackBar();
			((Control)val24).set_Location(new Point(((Control)settingManualWidth_Label).get_Right() + 5, ((Control)settingManualWidth_Label).get_Top()));
			((Control)val24).set_Width(220);
			val24.set_MaxValue(200f);
			val24.set_MinValue(0f);
			val24.set_Value((float)Module._settingImgWidth.get_Value());
			((Control)val24).set_Parent((Container)(object)manualPanel);
			TrackBar settingImgWidth_Slider = val24;
			settingImgWidth_Slider.add_ValueChanged((EventHandler<ValueEventArgs<float>>)delegate
			{
				Module._settingImgWidth.set_Value((int)settingImgWidth_Slider.get_Value());
			});
			Label val25 = new Label();
			((Control)val25).set_Location(new Point(0, ((Control)settingManualWidth_Label).get_Bottom() + 6));
			((Control)val25).set_Width(75);
			val25.set_AutoSizeHeight(false);
			val25.set_WrapText(false);
			((Control)val25).set_Parent((Container)(object)manualPanel);
			val25.set_Text("Opacity: ");
			Label settingManualOpacity_Label = val25;
			TrackBar val26 = new TrackBar();
			((Control)val26).set_Location(new Point(((Control)settingManualOpacity_Label).get_Right() + 5, ((Control)settingManualOpacity_Label).get_Top()));
			((Control)val26).set_Width(220);
			val26.set_MaxValue(100f);
			val26.set_MinValue(0f);
			val26.set_Value(Module._settingOpacity.get_Value() * 100f);
			((Control)val26).set_Parent((Container)(object)manualPanel);
			TrackBar settingOpacity_Slider = val26;
			settingOpacity_Slider.add_ValueChanged((EventHandler<ValueEventArgs<float>>)delegate
			{
				Module._settingOpacity.set_Value(settingOpacity_Slider.get_Value() / 100f);
			});
			IView settingClockDrag_View = SettingView.FromType((SettingEntry)(object)Module._settingDrag, ((Control)buildPanel).get_Width());
			ViewContainer val27 = new ViewContainer();
			((Container)val27).set_WidthSizingMode((SizingMode)2);
			((Control)val27).set_Location(new Point(0, ((Control)settingManualOpacity_Label).get_Bottom() + 3));
			((Control)val27).set_Parent((Container)(object)manualPanel);
			ViewContainer settingClockDrag_Container = val27;
			settingClockDrag_Container.Show(settingClockDrag_View);
			BuildDefaultMountPanel(defaultMountPanel, labelWidth2, mountsAndRadialInputWidth);
			BuildRadialPanel((Container)(object)radialPanel, labelWidth2, mountsAndRadialInputWidth);
		}

		private void BuildDefaultMountPanel(Panel defaultMountPanel, int labelWidth2, int mountsAndRadialInputWidth)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Expected O, but got Unknown
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Expected O, but got Unknown
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Expected O, but got Unknown
			//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01de: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f4: Expected O, but got Unknown
			//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_020d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0218: Unknown result type (might be due to invalid IL or missing references)
			//IL_0220: Unknown result type (might be due to invalid IL or missing references)
			//IL_022d: Expected O, but got Unknown
			//IL_0336: Unknown result type (might be due to invalid IL or missing references)
			//IL_033b: Unknown result type (might be due to invalid IL or missing references)
			//IL_034a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0355: Unknown result type (might be due to invalid IL or missing references)
			//IL_035d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0365: Unknown result type (might be due to invalid IL or missing references)
			//IL_036d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0375: Unknown result type (might be due to invalid IL or missing references)
			//IL_0383: Expected O, but got Unknown
			//IL_038d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0392: Unknown result type (might be due to invalid IL or missing references)
			//IL_039a: Unknown result type (might be due to invalid IL or missing references)
			//IL_039e: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d1: Expected O, but got Unknown
			//IL_03d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_03fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0404: Unknown result type (might be due to invalid IL or missing references)
			//IL_040c: Unknown result type (might be due to invalid IL or missing references)
			//IL_041a: Expected O, but got Unknown
			//IL_041b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0420: Unknown result type (might be due to invalid IL or missing references)
			//IL_0433: Unknown result type (might be due to invalid IL or missing references)
			//IL_043e: Unknown result type (might be due to invalid IL or missing references)
			//IL_044c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0459: Expected O, but got Unknown
			//IL_0521: Unknown result type (might be due to invalid IL or missing references)
			//IL_0526: Unknown result type (might be due to invalid IL or missing references)
			//IL_0531: Unknown result type (might be due to invalid IL or missing references)
			//IL_053c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0544: Unknown result type (might be due to invalid IL or missing references)
			//IL_054c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0554: Unknown result type (might be due to invalid IL or missing references)
			//IL_055c: Unknown result type (might be due to invalid IL or missing references)
			//IL_056a: Expected O, but got Unknown
			//IL_056b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0570: Unknown result type (might be due to invalid IL or missing references)
			//IL_0574: Unknown result type (might be due to invalid IL or missing references)
			//IL_057f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0587: Unknown result type (might be due to invalid IL or missing references)
			//IL_0598: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_05bb: Expected O, but got Unknown
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
			foreach (string k in mountNames)
			{
				settingDefaultMount_Select.get_Items().Add(k.ToString());
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
			foreach (string j in mountNamesWater)
			{
				settingDefaultWaterMount_Select.get_Items().Add(j.ToString());
			}
			settingDefaultWaterMount_Select.set_SelectedItem(mountNamesWater.Any((string m) => m == Module._settingDefaultWaterMountChoice.get_Value()) ? Module._settingDefaultWaterMountChoice.get_Value() : "Disabled");
			settingDefaultWaterMount_Select.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				Module._settingDefaultWaterMountChoice.set_Value(settingDefaultWaterMount_Select.get_SelectedItem());
			});
			Label val6 = new Label();
			((Control)val6).set_Location(new Point(0, ((Control)settingDefaultWaterMount_Select).get_Bottom() + 6));
			((Control)val6).set_Width(labelWidth2);
			val6.set_AutoSizeHeight(false);
			val6.set_WrapText(false);
			((Control)val6).set_Parent((Container)(object)defaultMountPanel);
			val6.set_Text("Key binding: ");
			Label settingDefaultMountKeybind_Label = val6;
			KeybindingAssigner val7 = new KeybindingAssigner(Module._settingDefaultMountBinding.get_Value());
			val7.set_NameWidth(0);
			((Control)val7).set_Size(new Point(mountsAndRadialInputWidth, 20));
			((Control)val7).set_Parent((Container)(object)defaultMountPanel);
			((Control)val7).set_Location(new Point(((Control)settingDefaultMountKeybind_Label).get_Right() + 4, ((Control)settingDefaultMountKeybind_Label).get_Top() - 1));
			KeybindingAssigner settingDefaultMount_Keybind = val7;
			Label val8 = new Label();
			((Control)val8).set_Location(new Point(0, ((Control)settingDefaultMountKeybind_Label).get_Bottom() + 6));
			((Control)val8).set_Width(labelWidth2);
			val8.set_AutoSizeHeight(false);
			val8.set_WrapText(false);
			((Control)val8).set_Parent((Container)(object)defaultMountPanel);
			val8.set_Text("Keybind behaviour: ");
			Label settingDefaultMountBehaviour_Label = val8;
			Dropdown val9 = new Dropdown();
			((Control)val9).set_Location(new Point(((Control)settingDefaultMountBehaviour_Label).get_Right() + 5, ((Control)settingDefaultMountBehaviour_Label).get_Top() - 4));
			((Control)val9).set_Width(((Control)settingDefaultMount_Keybind).get_Width());
			((Control)val9).set_Parent((Container)(object)defaultMountPanel);
			Dropdown settingDefaultMountBehaviour_Select = val9;
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
			Label val10 = new Label();
			((Control)val10).set_Location(new Point(0, ((Control)settingDefaultMountBehaviour_Label).get_Bottom() + 6));
			((Control)val10).set_Width(labelWidth2);
			val10.set_AutoSizeHeight(false);
			val10.set_WrapText(false);
			((Control)val10).set_Parent((Container)(object)defaultMountPanel);
			val10.set_Text("Display out of combat queueing:");
			Label settingDisplayMountQueueing_Label = val10;
			Checkbox val11 = new Checkbox();
			((Control)val11).set_Size(new Point(labelWidth2, 20));
			((Control)val11).set_Parent((Container)(object)defaultMountPanel);
			val11.set_Checked(Module._settingDisplayMountQueueing.get_Value());
			((Control)val11).set_Location(new Point(((Control)settingDisplayMountQueueing_Label).get_Right() + 5, ((Control)settingDisplayMountQueueing_Label).get_Top() - 1));
			Checkbox settingDisplayMountQueueing_Checkbox = val11;
			settingDisplayMountQueueing_Checkbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				Module._settingDisplayMountQueueing.set_Value(settingDisplayMountQueueing_Checkbox.get_Checked());
			});
		}

		private void BuildRadialPanel(Container radialPanel, int labelWidth, int mountsAndRadialInputWidth)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Expected O, but got Unknown
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Expected O, but got Unknown
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Expected O, but got Unknown
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_013c: Expected O, but got Unknown
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0142: Unknown result type (might be due to invalid IL or missing references)
			//IL_0151: Unknown result type (might be due to invalid IL or missing references)
			//IL_015c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0164: Unknown result type (might be due to invalid IL or missing references)
			//IL_0170: Unknown result type (might be due to invalid IL or missing references)
			//IL_017c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0193: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a0: Expected O, but got Unknown
			//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01da: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0200: Expected O, but got Unknown
			//IL_0201: Unknown result type (might be due to invalid IL or missing references)
			//IL_0206: Unknown result type (might be due to invalid IL or missing references)
			//IL_0217: Unknown result type (might be due to invalid IL or missing references)
			//IL_0222: Unknown result type (might be due to invalid IL or missing references)
			//IL_022a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0236: Unknown result type (might be due to invalid IL or missing references)
			//IL_0242: Unknown result type (might be due to invalid IL or missing references)
			//IL_0259: Unknown result type (might be due to invalid IL or missing references)
			//IL_0266: Expected O, but got Unknown
			//IL_027e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0283: Unknown result type (might be due to invalid IL or missing references)
			//IL_028e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0299: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c7: Expected O, but got Unknown
			//IL_02c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_02de: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0309: Unknown result type (might be due to invalid IL or missing references)
			//IL_0320: Unknown result type (might be due to invalid IL or missing references)
			//IL_032d: Expected O, but got Unknown
			//IL_0345: Unknown result type (might be due to invalid IL or missing references)
			//IL_034a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0355: Unknown result type (might be due to invalid IL or missing references)
			//IL_0360: Unknown result type (might be due to invalid IL or missing references)
			//IL_0368: Unknown result type (might be due to invalid IL or missing references)
			//IL_0370: Unknown result type (might be due to invalid IL or missing references)
			//IL_0378: Unknown result type (might be due to invalid IL or missing references)
			//IL_0380: Unknown result type (might be due to invalid IL or missing references)
			//IL_038e: Expected O, but got Unknown
			//IL_038f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0394: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c7: Expected O, but got Unknown
			//IL_0431: Unknown result type (might be due to invalid IL or missing references)
			//IL_0436: Unknown result type (might be due to invalid IL or missing references)
			//IL_0441: Unknown result type (might be due to invalid IL or missing references)
			//IL_044c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0454: Unknown result type (might be due to invalid IL or missing references)
			//IL_045c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0464: Unknown result type (might be due to invalid IL or missing references)
			//IL_046c: Unknown result type (might be due to invalid IL or missing references)
			//IL_047a: Expected O, but got Unknown
			//IL_047b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0480: Unknown result type (might be due to invalid IL or missing references)
			//IL_0484: Unknown result type (might be due to invalid IL or missing references)
			//IL_048f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0497: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_04bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_04cb: Expected O, but got Unknown
			//IL_04e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_04fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0506: Unknown result type (might be due to invalid IL or missing references)
			//IL_050e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0516: Unknown result type (might be due to invalid IL or missing references)
			//IL_051e: Unknown result type (might be due to invalid IL or missing references)
			//IL_052c: Expected O, but got Unknown
			//IL_052c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0531: Unknown result type (might be due to invalid IL or missing references)
			//IL_0539: Unknown result type (might be due to invalid IL or missing references)
			//IL_053e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0549: Unknown result type (might be due to invalid IL or missing references)
			//IL_055e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0584: Unknown result type (might be due to invalid IL or missing references)
			//IL_0589: Unknown result type (might be due to invalid IL or missing references)
			//IL_0591: Unknown result type (might be due to invalid IL or missing references)
			//IL_0595: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_05bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c8: Expected O, but got Unknown
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
			val6.set_Text("Icon size: ");
			Label settingMountRadialIconSizeModifier_Label = val6;
			TrackBar val7 = new TrackBar();
			((Control)val7).set_Location(new Point(((Control)settingMountRadialIconSizeModifier_Label).get_Right() + 5, ((Control)settingMountRadialIconSizeModifier_Label).get_Top()));
			((Control)val7).set_Width(mountsAndRadialInputWidth);
			val7.set_MaxValue(100f);
			val7.set_MinValue(5f);
			val7.set_Value(Module._settingMountRadialIconSizeModifier.get_Value() * 100f);
			((Control)val7).set_Parent(radialPanel);
			TrackBar settingMountRadialIconSizeModifier_Slider = val7;
			settingMountRadialIconSizeModifier_Slider.add_ValueChanged((EventHandler<ValueEventArgs<float>>)delegate
			{
				Module._settingMountRadialIconSizeModifier.set_Value(settingMountRadialIconSizeModifier_Slider.get_Value() / 100f);
			});
			Label val8 = new Label();
			((Control)val8).set_Location(new Point(0, ((Control)settingMountRadialIconSizeModifier_Label).get_Bottom() + 6));
			((Control)val8).set_Width(labelWidth);
			val8.set_AutoSizeHeight(false);
			val8.set_WrapText(false);
			((Control)val8).set_Parent(radialPanel);
			val8.set_Text("Icon opacity: ");
			Label settingMountRadialIconOpacity_Label = val8;
			TrackBar val9 = new TrackBar();
			((Control)val9).set_Location(new Point(((Control)settingMountRadialIconOpacity_Label).get_Right() + 5, ((Control)settingMountRadialIconOpacity_Label).get_Top()));
			((Control)val9).set_Width(mountsAndRadialInputWidth);
			val9.set_MaxValue(100f);
			val9.set_MinValue(5f);
			val9.set_Value(Module._settingMountRadialIconOpacity.get_Value() * 100f);
			((Control)val9).set_Parent(radialPanel);
			TrackBar settingMountRadialIconOpacity_Slider = val9;
			settingMountRadialIconOpacity_Slider.add_ValueChanged((EventHandler<ValueEventArgs<float>>)delegate
			{
				Module._settingMountRadialIconOpacity.set_Value(settingMountRadialIconOpacity_Slider.get_Value() / 100f);
			});
			Label val10 = new Label();
			((Control)val10).set_Location(new Point(0, ((Control)settingMountRadialIconOpacity_Label).get_Bottom() + 6));
			((Control)val10).set_Width(labelWidth);
			val10.set_AutoSizeHeight(false);
			val10.set_WrapText(false);
			((Control)val10).set_Parent(radialPanel);
			val10.set_Text("Center mount: ");
			Label settingMountRadialCenterMountBehavior_Label = val10;
			Dropdown val11 = new Dropdown();
			((Control)val11).set_Location(new Point(((Control)settingMountRadialCenterMountBehavior_Label).get_Right() + 5, ((Control)settingMountRadialCenterMountBehavior_Label).get_Top() - 4));
			((Control)val11).set_Width(mountsAndRadialInputWidth);
			((Control)val11).set_Parent(radialPanel);
			Dropdown settingMountRadialCenterMountBehavior_Select = val11;
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
			Label val12 = new Label();
			((Control)val12).set_Location(new Point(0, ((Control)settingMountRadialCenterMountBehavior_Label).get_Bottom() + 6));
			((Control)val12).set_Width(labelWidth);
			val12.set_AutoSizeHeight(false);
			val12.set_WrapText(false);
			((Control)val12).set_Parent(radialPanel);
			val12.set_Text("Remove center mount from radial: ");
			Label settingMountRadialRemoveCenterMount_Label = val12;
			Checkbox val13 = new Checkbox();
			((Control)val13).set_Size(new Point(labelWidth, 20));
			((Control)val13).set_Parent(radialPanel);
			val13.set_Checked(Module._settingMountRadialRemoveCenterMount.get_Value());
			((Control)val13).set_Location(new Point(((Control)settingMountRadialRemoveCenterMount_Label).get_Right() + 5, ((Control)settingMountRadialRemoveCenterMount_Label).get_Top() - 1));
			Checkbox settingMountRadialRemoveCenterMount_Checkbox = val13;
			settingMountRadialRemoveCenterMount_Checkbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				Module._settingMountRadialRemoveCenterMount.set_Value(settingMountRadialRemoveCenterMount_Checkbox.get_Checked());
			});
			Label val14 = new Label();
			((Control)val14).set_Location(new Point(0, ((Control)settingMountRadialRemoveCenterMount_Label).get_Bottom() + 6));
			((Control)val14).set_Width(labelWidth);
			val14.set_AutoSizeHeight(false);
			val14.set_WrapText(false);
			((Control)val14).set_Parent(radialPanel);
			val14.set_Text("In-game action camera key binding: ");
			Label settingMountRadialToggleActionCameraKeyBinding_Label = val14;
			Image val15 = new Image();
			((Control)val15).set_Parent(radialPanel);
			((Control)val15).set_Size(new Point(16, 16));
			((Control)val15).set_Location(new Point(((Control)settingMountRadialToggleActionCameraKeyBinding_Label).get_Right() - 32, ((Control)settingMountRadialToggleActionCameraKeyBinding_Label).get_Bottom() - 16));
			val15.set_Texture(AsyncTexture2D.op_Implicit(anetTexture));
			KeybindingAssigner val16 = new KeybindingAssigner(Module._settingMountRadialToggleActionCameraKeyBinding.get_Value());
			val16.set_NameWidth(0);
			((Control)val16).set_Size(new Point(mountsAndRadialInputWidth, 20));
			((Control)val16).set_Parent(radialPanel);
			((Control)val16).set_Location(new Point(((Control)settingMountRadialToggleActionCameraKeyBinding_Label).get_Right() + 4, ((Control)settingMountRadialToggleActionCameraKeyBinding_Label).get_Top() - 1));
			KeybindingAssigner settingMountRadialToggleActionCameraKeyBinding_Keybind = val16;
		}

		private static void DisplayManualPanelIfNeeded(Panel manualPanel)
		{
			if (Module._settingDisplayManualIcons.get_Value())
			{
				((Control)manualPanel).Show();
			}
			else
			{
				((Control)manualPanel).Hide();
			}
		}
	}
}
