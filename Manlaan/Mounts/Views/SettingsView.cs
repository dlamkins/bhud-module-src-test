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
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Expected O, but got Unknown
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Expected O, but got Unknown
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Expected O, but got Unknown
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Expected O, but got Unknown
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_0150: Expected O, but got Unknown
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0160: Unknown result type (might be due to invalid IL or missing references)
			//IL_0168: Unknown result type (might be due to invalid IL or missing references)
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0177: Unknown result type (might be due to invalid IL or missing references)
			//IL_017a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0184: Unknown result type (might be due to invalid IL or missing references)
			//IL_0197: Expected O, but got Unknown
			//IL_0197: Unknown result type (might be due to invalid IL or missing references)
			//IL_019c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01db: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ef: Expected O, but got Unknown
			//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0201: Unknown result type (might be due to invalid IL or missing references)
			//IL_020b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0212: Unknown result type (might be due to invalid IL or missing references)
			//IL_0219: Unknown result type (might be due to invalid IL or missing references)
			//IL_0220: Unknown result type (might be due to invalid IL or missing references)
			//IL_0228: Unknown result type (might be due to invalid IL or missing references)
			//IL_0233: Unknown result type (might be due to invalid IL or missing references)
			//IL_023c: Expected O, but got Unknown
			//IL_023c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0241: Unknown result type (might be due to invalid IL or missing references)
			//IL_0252: Unknown result type (might be due to invalid IL or missing references)
			//IL_025c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0264: Unknown result type (might be due to invalid IL or missing references)
			//IL_026b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0272: Unknown result type (might be due to invalid IL or missing references)
			//IL_027a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0285: Unknown result type (might be due to invalid IL or missing references)
			//IL_028e: Expected O, but got Unknown
			//IL_028e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0293: Unknown result type (might be due to invalid IL or missing references)
			//IL_029b: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_0308: Unknown result type (might be due to invalid IL or missing references)
			//IL_030d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0313: Unknown result type (might be due to invalid IL or missing references)
			//IL_031d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0324: Unknown result type (might be due to invalid IL or missing references)
			//IL_032b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0332: Unknown result type (might be due to invalid IL or missing references)
			//IL_033a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0358: Expected O, but got Unknown
			//IL_035a: Unknown result type (might be due to invalid IL or missing references)
			//IL_035f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0372: Unknown result type (might be due to invalid IL or missing references)
			//IL_037c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0383: Unknown result type (might be due to invalid IL or missing references)
			//IL_0390: Expected O, but got Unknown
			//IL_0451: Unknown result type (might be due to invalid IL or missing references)
			//IL_0456: Unknown result type (might be due to invalid IL or missing references)
			//IL_045d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0462: Unknown result type (might be due to invalid IL or missing references)
			//IL_046c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0474: Unknown result type (might be due to invalid IL or missing references)
			//IL_048c: Unknown result type (might be due to invalid IL or missing references)
			//IL_049b: Expected O, but got Unknown
			//IL_04d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_04dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_04df: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_04fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0506: Unknown result type (might be due to invalid IL or missing references)
			//IL_0513: Expected O, but got Unknown
			//IL_0514: Unknown result type (might be due to invalid IL or missing references)
			//IL_0519: Unknown result type (might be due to invalid IL or missing references)
			//IL_052c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0536: Unknown result type (might be due to invalid IL or missing references)
			//IL_0541: Unknown result type (might be due to invalid IL or missing references)
			//IL_054e: Expected O, but got Unknown
			//IL_05ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_05bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_05cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_05dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_05f2: Expected O, but got Unknown
			//IL_05f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_05f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_05fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0607: Unknown result type (might be due to invalid IL or missing references)
			//IL_060f: Unknown result type (might be due to invalid IL or missing references)
			//IL_061f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0632: Unknown result type (might be due to invalid IL or missing references)
			//IL_0641: Expected O, but got Unknown
			//IL_0658: Unknown result type (might be due to invalid IL or missing references)
			//IL_065d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0668: Unknown result type (might be due to invalid IL or missing references)
			//IL_0672: Unknown result type (might be due to invalid IL or missing references)
			//IL_067a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0681: Unknown result type (might be due to invalid IL or missing references)
			//IL_0688: Unknown result type (might be due to invalid IL or missing references)
			//IL_0690: Unknown result type (might be due to invalid IL or missing references)
			//IL_069d: Expected O, but got Unknown
			//IL_069e: Unknown result type (might be due to invalid IL or missing references)
			//IL_06a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_06a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_06b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_06dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ec: Expected O, but got Unknown
			//IL_0703: Unknown result type (might be due to invalid IL or missing references)
			//IL_0708: Unknown result type (might be due to invalid IL or missing references)
			//IL_070b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0715: Unknown result type (might be due to invalid IL or missing references)
			//IL_0726: Unknown result type (might be due to invalid IL or missing references)
			//IL_072d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0734: Unknown result type (might be due to invalid IL or missing references)
			//IL_0740: Unknown result type (might be due to invalid IL or missing references)
			//IL_074b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0754: Expected O, but got Unknown
			//IL_0754: Unknown result type (might be due to invalid IL or missing references)
			//IL_0759: Unknown result type (might be due to invalid IL or missing references)
			//IL_0764: Unknown result type (might be due to invalid IL or missing references)
			//IL_076e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0776: Unknown result type (might be due to invalid IL or missing references)
			//IL_077d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0784: Unknown result type (might be due to invalid IL or missing references)
			//IL_0790: Unknown result type (might be due to invalid IL or missing references)
			//IL_079d: Expected O, but got Unknown
			//IL_079e: Unknown result type (might be due to invalid IL or missing references)
			//IL_07a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_07b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_07c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_07c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_07d9: Expected O, but got Unknown
			//IL_0838: Unknown result type (might be due to invalid IL or missing references)
			//IL_083d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0848: Unknown result type (might be due to invalid IL or missing references)
			//IL_0852: Unknown result type (might be due to invalid IL or missing references)
			//IL_085a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0861: Unknown result type (might be due to invalid IL or missing references)
			//IL_0868: Unknown result type (might be due to invalid IL or missing references)
			//IL_0874: Unknown result type (might be due to invalid IL or missing references)
			//IL_0881: Expected O, but got Unknown
			//IL_0882: Unknown result type (might be due to invalid IL or missing references)
			//IL_0887: Unknown result type (might be due to invalid IL or missing references)
			//IL_0898: Unknown result type (might be due to invalid IL or missing references)
			//IL_08a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_08ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_08b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_08c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_08d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_08e5: Expected O, but got Unknown
			//IL_08fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0901: Unknown result type (might be due to invalid IL or missing references)
			//IL_090c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0916: Unknown result type (might be due to invalid IL or missing references)
			//IL_091e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0925: Unknown result type (might be due to invalid IL or missing references)
			//IL_092c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0938: Unknown result type (might be due to invalid IL or missing references)
			//IL_0945: Expected O, but got Unknown
			//IL_0946: Unknown result type (might be due to invalid IL or missing references)
			//IL_094b: Unknown result type (might be due to invalid IL or missing references)
			//IL_095c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0966: Unknown result type (might be due to invalid IL or missing references)
			//IL_0971: Unknown result type (might be due to invalid IL or missing references)
			//IL_097c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0987: Unknown result type (might be due to invalid IL or missing references)
			//IL_099d: Unknown result type (might be due to invalid IL or missing references)
			//IL_09ae: Expected O, but got Unknown
			//IL_09d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_09dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_09e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_09ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_09f8: Unknown result type (might be due to invalid IL or missing references)
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
			mountDisplay = Module._mountOrientation;
			foreach (string s2 in mountDisplay)
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
			val27.Show(settingClockDrag_View);
			BuildDefaultMountPanel(defaultMountPanel, labelWidth2, mountsAndRadialInputWidth);
			BuildRadialPanel((Container)(object)radialPanel, labelWidth2, mountsAndRadialInputWidth);
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
			//IL_0321: Unknown result type (might be due to invalid IL or missing references)
			//IL_032b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0332: Unknown result type (might be due to invalid IL or missing references)
			//IL_0339: Unknown result type (might be due to invalid IL or missing references)
			//IL_0340: Unknown result type (might be due to invalid IL or missing references)
			//IL_0347: Unknown result type (might be due to invalid IL or missing references)
			//IL_0354: Expected O, but got Unknown
			//IL_035e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0363: Unknown result type (might be due to invalid IL or missing references)
			//IL_036a: Unknown result type (might be due to invalid IL or missing references)
			//IL_036e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0378: Unknown result type (might be due to invalid IL or missing references)
			//IL_037f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0392: Unknown result type (might be due to invalid IL or missing references)
			//IL_039e: Expected O, but got Unknown
			//IL_039e: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e1: Expected O, but got Unknown
			//IL_03e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_03fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0404: Unknown result type (might be due to invalid IL or missing references)
			//IL_0411: Unknown result type (might be due to invalid IL or missing references)
			//IL_041d: Expected O, but got Unknown
			//IL_04dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_04fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0505: Unknown result type (might be due to invalid IL or missing references)
			//IL_050c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0513: Unknown result type (might be due to invalid IL or missing references)
			//IL_0520: Expected O, but got Unknown
			//IL_0521: Unknown result type (might be due to invalid IL or missing references)
			//IL_0526: Unknown result type (might be due to invalid IL or missing references)
			//IL_052a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0534: Unknown result type (might be due to invalid IL or missing references)
			//IL_053b: Unknown result type (might be due to invalid IL or missing references)
			//IL_054b: Unknown result type (might be due to invalid IL or missing references)
			//IL_055e: Unknown result type (might be due to invalid IL or missing references)
			//IL_056d: Expected O, but got Unknown
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
