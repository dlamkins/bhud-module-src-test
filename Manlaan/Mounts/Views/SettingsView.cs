using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Settings;
using Blish_HUD.Settings.UI.Views;
using Microsoft.Xna.Framework;

namespace Manlaan.Mounts.Views
{
	internal class SettingsView : View
	{
		protected override void Build(Container buildPanel)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Expected O, but got Unknown
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Expected O, but got Unknown
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Expected O, but got Unknown
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
			//IL_0158: Unknown result type (might be due to invalid IL or missing references)
			//IL_0160: Unknown result type (might be due to invalid IL or missing references)
			//IL_0168: Unknown result type (might be due to invalid IL or missing references)
			//IL_0171: Unknown result type (might be due to invalid IL or missing references)
			//IL_017d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0187: Expected O, but got Unknown
			//IL_0187: Unknown result type (might be due to invalid IL or missing references)
			//IL_018c: Unknown result type (might be due to invalid IL or missing references)
			//IL_019d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01df: Expected O, but got Unknown
			//IL_01df: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0202: Unknown result type (might be due to invalid IL or missing references)
			//IL_020a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0212: Unknown result type (might be due to invalid IL or missing references)
			//IL_021b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0229: Expected O, but got Unknown
			//IL_022a: Unknown result type (might be due to invalid IL or missing references)
			//IL_022f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0242: Unknown result type (might be due to invalid IL or missing references)
			//IL_024d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0255: Unknown result type (might be due to invalid IL or missing references)
			//IL_0263: Expected O, but got Unknown
			//IL_030c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0311: Unknown result type (might be due to invalid IL or missing references)
			//IL_0319: Unknown result type (might be due to invalid IL or missing references)
			//IL_031d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0328: Unknown result type (might be due to invalid IL or missing references)
			//IL_0331: Unknown result type (might be due to invalid IL or missing references)
			//IL_0342: Unknown result type (might be due to invalid IL or missing references)
			//IL_0359: Unknown result type (might be due to invalid IL or missing references)
			//IL_0366: Expected O, but got Unknown
			//IL_0366: Unknown result type (might be due to invalid IL or missing references)
			//IL_036b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0376: Unknown result type (might be due to invalid IL or missing references)
			//IL_0381: Unknown result type (might be due to invalid IL or missing references)
			//IL_0389: Unknown result type (might be due to invalid IL or missing references)
			//IL_0391: Unknown result type (might be due to invalid IL or missing references)
			//IL_0399: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b0: Expected O, but got Unknown
			//IL_03b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_03dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ea: Expected O, but got Unknown
			//IL_0493: Unknown result type (might be due to invalid IL or missing references)
			//IL_0498: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_04af: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ed: Expected O, but got Unknown
			//IL_04ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_04fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0508: Unknown result type (might be due to invalid IL or missing references)
			//IL_0510: Unknown result type (might be due to invalid IL or missing references)
			//IL_0518: Unknown result type (might be due to invalid IL or missing references)
			//IL_0520: Unknown result type (might be due to invalid IL or missing references)
			//IL_0529: Unknown result type (might be due to invalid IL or missing references)
			//IL_0537: Expected O, but got Unknown
			//IL_0538: Unknown result type (might be due to invalid IL or missing references)
			//IL_053d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0550: Unknown result type (might be due to invalid IL or missing references)
			//IL_055b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0563: Unknown result type (might be due to invalid IL or missing references)
			//IL_0571: Expected O, but got Unknown
			//IL_061a: Unknown result type (might be due to invalid IL or missing references)
			//IL_061f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0627: Unknown result type (might be due to invalid IL or missing references)
			//IL_062b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0636: Unknown result type (might be due to invalid IL or missing references)
			//IL_063f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0650: Unknown result type (might be due to invalid IL or missing references)
			//IL_0667: Unknown result type (might be due to invalid IL or missing references)
			//IL_0674: Expected O, but got Unknown
			//IL_0674: Unknown result type (might be due to invalid IL or missing references)
			//IL_0679: Unknown result type (might be due to invalid IL or missing references)
			//IL_0684: Unknown result type (might be due to invalid IL or missing references)
			//IL_068f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0697: Unknown result type (might be due to invalid IL or missing references)
			//IL_069f: Unknown result type (might be due to invalid IL or missing references)
			//IL_06a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_06b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_06be: Expected O, but got Unknown
			//IL_06bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_06c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_06d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_06e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_06f8: Expected O, but got Unknown
			//IL_07a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_07a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_07ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_07b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_07bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_07c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_07d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_07ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_07fb: Expected O, but got Unknown
			//IL_07fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0800: Unknown result type (might be due to invalid IL or missing references)
			//IL_080b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0816: Unknown result type (might be due to invalid IL or missing references)
			//IL_081e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0826: Unknown result type (might be due to invalid IL or missing references)
			//IL_082e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0837: Unknown result type (might be due to invalid IL or missing references)
			//IL_0845: Expected O, but got Unknown
			//IL_0846: Unknown result type (might be due to invalid IL or missing references)
			//IL_084b: Unknown result type (might be due to invalid IL or missing references)
			//IL_085e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0869: Unknown result type (might be due to invalid IL or missing references)
			//IL_0871: Unknown result type (might be due to invalid IL or missing references)
			//IL_087f: Expected O, but got Unknown
			//IL_0928: Unknown result type (might be due to invalid IL or missing references)
			//IL_092d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0935: Unknown result type (might be due to invalid IL or missing references)
			//IL_0939: Unknown result type (might be due to invalid IL or missing references)
			//IL_0944: Unknown result type (might be due to invalid IL or missing references)
			//IL_094d: Unknown result type (might be due to invalid IL or missing references)
			//IL_095e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0975: Unknown result type (might be due to invalid IL or missing references)
			//IL_0982: Expected O, but got Unknown
			//IL_0982: Unknown result type (might be due to invalid IL or missing references)
			//IL_0987: Unknown result type (might be due to invalid IL or missing references)
			//IL_0992: Unknown result type (might be due to invalid IL or missing references)
			//IL_099d: Unknown result type (might be due to invalid IL or missing references)
			//IL_09a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_09ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_09b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_09be: Unknown result type (might be due to invalid IL or missing references)
			//IL_09cc: Expected O, but got Unknown
			//IL_09cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_09d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_09e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_09f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_09f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a06: Expected O, but got Unknown
			//IL_0aaf: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ab4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0abc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ac0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0acb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ad4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ae5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0afc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b09: Expected O, but got Unknown
			//IL_0b09: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b0e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b19: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b24: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b2c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b34: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b3c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b45: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b53: Expected O, but got Unknown
			//IL_0b54: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b59: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b6c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b77: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b7f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b8d: Expected O, but got Unknown
			//IL_0c36: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c3b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c43: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c47: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c52: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c5b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c6c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c83: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c90: Expected O, but got Unknown
			//IL_0c90: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c95: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ca0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0cab: Unknown result type (might be due to invalid IL or missing references)
			//IL_0cb3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0cbb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0cc3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ccc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0cda: Expected O, but got Unknown
			//IL_0cdb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ce0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0cf3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0cfe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d06: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d14: Expected O, but got Unknown
			//IL_0dbd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0dc2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0dca: Unknown result type (might be due to invalid IL or missing references)
			//IL_0dce: Unknown result type (might be due to invalid IL or missing references)
			//IL_0dd9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0de2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0df3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e0a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e17: Expected O, but got Unknown
			//IL_0e17: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e1c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e27: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e32: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e3a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e42: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e4a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e53: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e61: Expected O, but got Unknown
			//IL_0e62: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e67: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e7a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e85: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e8d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e9b: Expected O, but got Unknown
			//IL_0f37: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f3c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f44: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f48: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f53: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f5c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f6d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f84: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f91: Expected O, but got Unknown
			//IL_0f91: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f96: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f99: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fa4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fac: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fb4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fbc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fc5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fd3: Expected O, but got Unknown
			//IL_0fd4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fd9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fec: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ff7: Unknown result type (might be due to invalid IL or missing references)
			//IL_1003: Unknown result type (might be due to invalid IL or missing references)
			//IL_1011: Expected O, but got Unknown
			//IL_1076: Unknown result type (might be due to invalid IL or missing references)
			//IL_107b: Unknown result type (might be due to invalid IL or missing references)
			//IL_107e: Unknown result type (might be due to invalid IL or missing references)
			//IL_1089: Unknown result type (might be due to invalid IL or missing references)
			//IL_109b: Unknown result type (might be due to invalid IL or missing references)
			//IL_10a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_10ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_10b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_10c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_10ce: Expected O, but got Unknown
			//IL_10ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_10d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_10de: Unknown result type (might be due to invalid IL or missing references)
			//IL_10e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_10f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_10fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_1102: Unknown result type (might be due to invalid IL or missing references)
			//IL_110f: Unknown result type (might be due to invalid IL or missing references)
			//IL_111d: Expected O, but got Unknown
			//IL_111e: Unknown result type (might be due to invalid IL or missing references)
			//IL_1123: Unknown result type (might be due to invalid IL or missing references)
			//IL_1136: Unknown result type (might be due to invalid IL or missing references)
			//IL_1141: Unknown result type (might be due to invalid IL or missing references)
			//IL_114a: Unknown result type (might be due to invalid IL or missing references)
			//IL_115c: Expected O, but got Unknown
			//IL_11c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_11c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_11d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_11dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_11e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_11ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_11f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_1202: Unknown result type (might be due to invalid IL or missing references)
			//IL_1210: Expected O, but got Unknown
			//IL_1211: Unknown result type (might be due to invalid IL or missing references)
			//IL_1216: Unknown result type (might be due to invalid IL or missing references)
			//IL_1227: Unknown result type (might be due to invalid IL or missing references)
			//IL_1232: Unknown result type (might be due to invalid IL or missing references)
			//IL_123e: Unknown result type (might be due to invalid IL or missing references)
			//IL_124a: Unknown result type (might be due to invalid IL or missing references)
			//IL_1256: Unknown result type (might be due to invalid IL or missing references)
			//IL_1268: Unknown result type (might be due to invalid IL or missing references)
			//IL_127a: Expected O, but got Unknown
			//IL_1292: Unknown result type (might be due to invalid IL or missing references)
			//IL_1297: Unknown result type (might be due to invalid IL or missing references)
			//IL_12a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_12ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_12b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_12be: Unknown result type (might be due to invalid IL or missing references)
			//IL_12c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_12d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_12e1: Expected O, but got Unknown
			//IL_12e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_12e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_12f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_1303: Unknown result type (might be due to invalid IL or missing references)
			//IL_130f: Unknown result type (might be due to invalid IL or missing references)
			//IL_131b: Unknown result type (might be due to invalid IL or missing references)
			//IL_1327: Unknown result type (might be due to invalid IL or missing references)
			//IL_133e: Unknown result type (might be due to invalid IL or missing references)
			//IL_1350: Expected O, but got Unknown
			//IL_137a: Unknown result type (might be due to invalid IL or missing references)
			//IL_137f: Unknown result type (might be due to invalid IL or missing references)
			//IL_1387: Unknown result type (might be due to invalid IL or missing references)
			//IL_1392: Unknown result type (might be due to invalid IL or missing references)
			//IL_139d: Unknown result type (might be due to invalid IL or missing references)
			//IL_13ac: Expected O, but got Unknown
			int labelWidth = 100;
			int orderWidth = 80;
			int bindingWidth = 150;
			Panel val = new Panel();
			val.set_CanScroll(false);
			((Control)val).set_Parent(buildPanel);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Control)val).set_Width(330);
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
			((Control)val3).set_Location(new Point(((Control)mountsLeftPanel).get_Right() + 20, 63));
			Panel manualPanel = val3;
			if (Module._settingDisplay.get_Value().Equals("Transparent Manual") || Module._settingDisplay.get_Value().Equals("Solid Manual") || Module._settingDisplay.get_Value().Equals("Solid Manual Text"))
			{
				((Control)manualPanel).Show();
			}
			else
			{
				((Control)manualPanel).Hide();
			}
			Label val4 = new Label();
			((Control)val4).set_Location(new Point(labelWidth + 5, 2));
			((Control)val4).set_Width(orderWidth);
			val4.set_AutoSizeHeight(false);
			val4.set_WrapText(false);
			((Control)val4).set_Parent((Container)(object)mountsLeftPanel);
			val4.set_Text("Order");
			val4.set_HorizontalAlignment((HorizontalAlignment)1);
			Label settingOrderLeft_Label = val4;
			Label val5 = new Label();
			((Control)val5).set_Location(new Point(((Control)settingOrderLeft_Label).get_Right() + 5, ((Control)settingOrderLeft_Label).get_Top()));
			((Control)val5).set_Width(bindingWidth);
			val5.set_AutoSizeHeight(false);
			val5.set_WrapText(false);
			((Control)val5).set_Parent((Container)(object)mountsLeftPanel);
			val5.set_Text("Key Binding");
			val5.set_HorizontalAlignment((HorizontalAlignment)1);
			Label settingBindingLeft_Label = val5;
			Label val6 = new Label();
			((Control)val6).set_Location(new Point(0, ((Control)settingOrderLeft_Label).get_Bottom() + 6));
			((Control)val6).set_Width(labelWidth);
			val6.set_AutoSizeHeight(false);
			val6.set_WrapText(false);
			((Control)val6).set_Parent((Container)(object)mountsLeftPanel);
			val6.set_Text("Raptor: ");
			Label settingRaptor_Label = val6;
			Dropdown val7 = new Dropdown();
			((Control)val7).set_Location(new Point(((Control)settingRaptor_Label).get_Right() + 5, ((Control)settingRaptor_Label).get_Top() - 4));
			((Control)val7).set_Width(orderWidth);
			((Control)val7).set_Parent((Container)(object)mountsLeftPanel);
			Dropdown settingRaptor_Select = val7;
			int[] mountOrder = Module._mountOrder;
			for (int num = 0; num < mountOrder.Length; num++)
			{
				int i = mountOrder[num];
				if (i == 0)
				{
					settingRaptor_Select.get_Items().Add("Disabled");
				}
				else
				{
					settingRaptor_Select.get_Items().Add(i.ToString());
				}
			}
			settingRaptor_Select.set_SelectedItem((Module._settingRaptorOrder.get_Value() == 0) ? "Disabled" : Module._settingRaptorOrder.get_Value().ToString());
			settingRaptor_Select.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				if (settingRaptor_Select.get_SelectedItem().Equals("Disabled"))
				{
					Module._settingRaptorOrder.set_Value(0);
				}
				else
				{
					Module._settingRaptorOrder.set_Value(int.Parse(settingRaptor_Select.get_SelectedItem()));
				}
			});
			KeybindingAssigner val8 = new KeybindingAssigner();
			val8.set_NameWidth(0);
			((Control)val8).set_Size(new Point(bindingWidth, 20));
			((Control)val8).set_Parent((Container)(object)mountsLeftPanel);
			val8.set_KeyBinding(Module._settingRaptorBinding.get_Value());
			((Control)val8).set_Location(new Point(((Control)settingRaptor_Select).get_Right() + 5, ((Control)settingRaptor_Label).get_Top() - 1));
			KeybindingAssigner settingRaptor_Keybind = val8;
			Label val9 = new Label();
			((Control)val9).set_Location(new Point(0, ((Control)settingRaptor_Label).get_Bottom() + 6));
			((Control)val9).set_Width(labelWidth);
			val9.set_AutoSizeHeight(false);
			val9.set_WrapText(false);
			((Control)val9).set_Parent((Container)(object)mountsLeftPanel);
			val9.set_Text("Springer: ");
			Label settingSpringer_Label = val9;
			Dropdown val10 = new Dropdown();
			((Control)val10).set_Location(new Point(((Control)settingSpringer_Label).get_Right() + 5, ((Control)settingSpringer_Label).get_Top() - 4));
			((Control)val10).set_Width(orderWidth);
			((Control)val10).set_Parent((Container)(object)mountsLeftPanel);
			Dropdown settingSpringer_Select = val10;
			int[] mountOrder2 = Module._mountOrder;
			for (int num2 = 0; num2 < mountOrder2.Length; num2++)
			{
				int j = mountOrder2[num2];
				if (j == 0)
				{
					settingSpringer_Select.get_Items().Add("Disabled");
				}
				else
				{
					settingSpringer_Select.get_Items().Add(j.ToString());
				}
			}
			settingSpringer_Select.set_SelectedItem((Module._settingSpringerOrder.get_Value() == 0) ? "Disabled" : Module._settingSpringerOrder.get_Value().ToString());
			settingSpringer_Select.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				if (settingSpringer_Select.get_SelectedItem().Equals("Disabled"))
				{
					Module._settingSpringerOrder.set_Value(0);
				}
				else
				{
					Module._settingSpringerOrder.set_Value(int.Parse(settingSpringer_Select.get_SelectedItem()));
				}
			});
			KeybindingAssigner val11 = new KeybindingAssigner();
			val11.set_NameWidth(0);
			((Control)val11).set_Size(new Point(bindingWidth, 20));
			((Control)val11).set_Parent((Container)(object)mountsLeftPanel);
			val11.set_KeyBinding(Module._settingSpringerBinding.get_Value());
			((Control)val11).set_Location(new Point(((Control)settingSpringer_Select).get_Right() + 5, ((Control)settingSpringer_Label).get_Top() - 1));
			KeybindingAssigner settingSpringer_Keybind = val11;
			Label val12 = new Label();
			((Control)val12).set_Location(new Point(0, ((Control)settingSpringer_Label).get_Bottom() + 6));
			((Control)val12).set_Width(labelWidth);
			val12.set_AutoSizeHeight(false);
			val12.set_WrapText(false);
			((Control)val12).set_Parent((Container)(object)mountsLeftPanel);
			val12.set_Text("Skimmer: ");
			Label settingSkimmer_Label = val12;
			Dropdown val13 = new Dropdown();
			((Control)val13).set_Location(new Point(((Control)settingSkimmer_Label).get_Right() + 5, ((Control)settingSkimmer_Label).get_Top() - 4));
			((Control)val13).set_Width(orderWidth);
			((Control)val13).set_Parent((Container)(object)mountsLeftPanel);
			Dropdown settingSkimmer_Select = val13;
			int[] mountOrder3 = Module._mountOrder;
			for (int num3 = 0; num3 < mountOrder3.Length; num3++)
			{
				int k = mountOrder3[num3];
				if (k == 0)
				{
					settingSkimmer_Select.get_Items().Add("Disabled");
				}
				else
				{
					settingSkimmer_Select.get_Items().Add(k.ToString());
				}
			}
			settingSkimmer_Select.set_SelectedItem((Module._settingSkimmerOrder.get_Value() == 0) ? "Disabled" : Module._settingSkimmerOrder.get_Value().ToString());
			settingSkimmer_Select.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				if (settingSkimmer_Select.get_SelectedItem().Equals("Disabled"))
				{
					Module._settingSkimmerOrder.set_Value(0);
				}
				else
				{
					Module._settingSkimmerOrder.set_Value(int.Parse(settingSkimmer_Select.get_SelectedItem()));
				}
			});
			KeybindingAssigner val14 = new KeybindingAssigner();
			val14.set_NameWidth(0);
			((Control)val14).set_Size(new Point(bindingWidth, 20));
			((Control)val14).set_Parent((Container)(object)mountsLeftPanel);
			val14.set_KeyBinding(Module._settingSkimmerBinding.get_Value());
			((Control)val14).set_Location(new Point(((Control)settingSkimmer_Select).get_Right() + 5, ((Control)settingSkimmer_Label).get_Top() - 1));
			KeybindingAssigner settingSkimmer_Keybind = val14;
			Label val15 = new Label();
			((Control)val15).set_Location(new Point(0, ((Control)settingSkimmer_Label).get_Bottom() + 6));
			((Control)val15).set_Width(labelWidth);
			val15.set_AutoSizeHeight(false);
			val15.set_WrapText(false);
			((Control)val15).set_Parent((Container)(object)mountsLeftPanel);
			val15.set_Text("Jackal: ");
			Label settingJackal_Label = val15;
			Dropdown val16 = new Dropdown();
			((Control)val16).set_Location(new Point(((Control)settingJackal_Label).get_Right() + 5, ((Control)settingJackal_Label).get_Top() - 4));
			((Control)val16).set_Width(orderWidth);
			((Control)val16).set_Parent((Container)(object)mountsLeftPanel);
			Dropdown settingJackal_Select = val16;
			int[] mountOrder4 = Module._mountOrder;
			for (int num4 = 0; num4 < mountOrder4.Length; num4++)
			{
				int l = mountOrder4[num4];
				if (l == 0)
				{
					settingJackal_Select.get_Items().Add("Disabled");
				}
				else
				{
					settingJackal_Select.get_Items().Add(l.ToString());
				}
			}
			settingJackal_Select.set_SelectedItem((Module._settingJackalOrder.get_Value() == 0) ? "Disabled" : Module._settingJackalOrder.get_Value().ToString());
			settingJackal_Select.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				if (settingJackal_Select.get_SelectedItem().Equals("Disabled"))
				{
					Module._settingJackalOrder.set_Value(0);
				}
				else
				{
					Module._settingJackalOrder.set_Value(int.Parse(settingJackal_Select.get_SelectedItem()));
				}
			});
			KeybindingAssigner val17 = new KeybindingAssigner();
			val17.set_NameWidth(0);
			((Control)val17).set_Size(new Point(bindingWidth, 20));
			((Control)val17).set_Parent((Container)(object)mountsLeftPanel);
			val17.set_KeyBinding(Module._settingJackalBinding.get_Value());
			((Control)val17).set_Location(new Point(((Control)settingJackal_Select).get_Right() + 5, ((Control)settingJackal_Label).get_Top() - 1));
			KeybindingAssigner settingJackal_Keybind = val17;
			Label val18 = new Label();
			((Control)val18).set_Location(new Point(0, ((Control)settingJackal_Label).get_Bottom() + 6));
			((Control)val18).set_Width(labelWidth);
			val18.set_AutoSizeHeight(false);
			val18.set_WrapText(false);
			((Control)val18).set_Parent((Container)(object)mountsLeftPanel);
			val18.set_Text("Griffon: ");
			Label settingGriffon_Label = val18;
			Dropdown val19 = new Dropdown();
			((Control)val19).set_Location(new Point(((Control)settingGriffon_Label).get_Right() + 5, ((Control)settingGriffon_Label).get_Top() - 4));
			((Control)val19).set_Width(orderWidth);
			((Control)val19).set_Parent((Container)(object)mountsLeftPanel);
			Dropdown settingGriffon_Select = val19;
			int[] mountOrder5 = Module._mountOrder;
			for (int num5 = 0; num5 < mountOrder5.Length; num5++)
			{
				int m = mountOrder5[num5];
				if (m == 0)
				{
					settingGriffon_Select.get_Items().Add("Disabled");
				}
				else
				{
					settingGriffon_Select.get_Items().Add(m.ToString());
				}
			}
			settingGriffon_Select.set_SelectedItem((Module._settingGriffonOrder.get_Value() == 0) ? "Disabled" : Module._settingGriffonOrder.get_Value().ToString());
			settingGriffon_Select.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				if (settingGriffon_Select.get_SelectedItem().Equals("Disabled"))
				{
					Module._settingGriffonOrder.set_Value(0);
				}
				else
				{
					Module._settingGriffonOrder.set_Value(int.Parse(settingGriffon_Select.get_SelectedItem()));
				}
			});
			KeybindingAssigner val20 = new KeybindingAssigner();
			val20.set_NameWidth(0);
			((Control)val20).set_Size(new Point(bindingWidth, 20));
			((Control)val20).set_Parent((Container)(object)mountsLeftPanel);
			val20.set_KeyBinding(Module._settingGriffonBinding.get_Value());
			((Control)val20).set_Location(new Point(((Control)settingGriffon_Select).get_Right() + 5, ((Control)settingGriffon_Label).get_Top() - 1));
			KeybindingAssigner settingGriffon_Keybind = val20;
			Label val21 = new Label();
			((Control)val21).set_Location(new Point(0, ((Control)settingGriffon_Label).get_Bottom() + 6));
			((Control)val21).set_Width(labelWidth);
			val21.set_AutoSizeHeight(false);
			val21.set_WrapText(false);
			((Control)val21).set_Parent((Container)(object)mountsLeftPanel);
			val21.set_Text("Roller: ");
			Label settingRoller_Label = val21;
			Dropdown val22 = new Dropdown();
			((Control)val22).set_Location(new Point(((Control)settingRoller_Label).get_Right() + 5, ((Control)settingRoller_Label).get_Top() - 4));
			((Control)val22).set_Width(orderWidth);
			((Control)val22).set_Parent((Container)(object)mountsLeftPanel);
			Dropdown settingRoller_Select = val22;
			int[] mountOrder6 = Module._mountOrder;
			for (int num6 = 0; num6 < mountOrder6.Length; num6++)
			{
				int n = mountOrder6[num6];
				if (n == 0)
				{
					settingRoller_Select.get_Items().Add("Disabled");
				}
				else
				{
					settingRoller_Select.get_Items().Add(n.ToString());
				}
			}
			settingRoller_Select.set_SelectedItem((Module._settingRollerOrder.get_Value() == 0) ? "Disabled" : Module._settingRollerOrder.get_Value().ToString());
			settingRoller_Select.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				if (settingRoller_Select.get_SelectedItem().Equals("Disabled"))
				{
					Module._settingRollerOrder.set_Value(0);
				}
				else
				{
					Module._settingRollerOrder.set_Value(int.Parse(settingRoller_Select.get_SelectedItem()));
				}
			});
			KeybindingAssigner val23 = new KeybindingAssigner();
			val23.set_NameWidth(0);
			((Control)val23).set_Size(new Point(bindingWidth, 20));
			((Control)val23).set_Parent((Container)(object)mountsLeftPanel);
			val23.set_KeyBinding(Module._settingRollerBinding.get_Value());
			((Control)val23).set_Location(new Point(((Control)settingRoller_Select).get_Right() + 5, ((Control)settingRoller_Label).get_Top() - 1));
			KeybindingAssigner settingRoller_Keybind = val23;
			Label val24 = new Label();
			((Control)val24).set_Location(new Point(0, ((Control)settingRoller_Label).get_Bottom() + 6));
			((Control)val24).set_Width(labelWidth);
			val24.set_AutoSizeHeight(false);
			val24.set_WrapText(false);
			((Control)val24).set_Parent((Container)(object)mountsLeftPanel);
			val24.set_Text("Warclaw: ");
			Label settingWarclaw_Label = val24;
			Dropdown val25 = new Dropdown();
			((Control)val25).set_Location(new Point(((Control)settingWarclaw_Label).get_Right() + 5, ((Control)settingWarclaw_Label).get_Top() - 4));
			((Control)val25).set_Width(orderWidth);
			((Control)val25).set_Parent((Container)(object)mountsLeftPanel);
			Dropdown settingWarclaw_Select = val25;
			int[] mountOrder7 = Module._mountOrder;
			for (int num7 = 0; num7 < mountOrder7.Length; num7++)
			{
				int i2 = mountOrder7[num7];
				if (i2 == 0)
				{
					settingWarclaw_Select.get_Items().Add("Disabled");
				}
				else
				{
					settingWarclaw_Select.get_Items().Add(i2.ToString());
				}
			}
			settingWarclaw_Select.set_SelectedItem((Module._settingWarclawOrder.get_Value() == 0) ? "Disabled" : Module._settingWarclawOrder.get_Value().ToString());
			settingWarclaw_Select.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				if (settingWarclaw_Select.get_SelectedItem().Equals("Disabled"))
				{
					Module._settingWarclawOrder.set_Value(0);
				}
				else
				{
					Module._settingWarclawOrder.set_Value(int.Parse(settingWarclaw_Select.get_SelectedItem()));
				}
			});
			KeybindingAssigner val26 = new KeybindingAssigner();
			val26.set_NameWidth(0);
			((Control)val26).set_Size(new Point(bindingWidth, 20));
			((Control)val26).set_Parent((Container)(object)mountsLeftPanel);
			val26.set_KeyBinding(Module._settingWarclawBinding.get_Value());
			((Control)val26).set_Location(new Point(((Control)settingWarclaw_Select).get_Right() + 5, ((Control)settingWarclaw_Label).get_Top() - 1));
			KeybindingAssigner settingWarclaw_Keybind = val26;
			Label val27 = new Label();
			((Control)val27).set_Location(new Point(0, ((Control)settingWarclaw_Label).get_Bottom() + 6));
			((Control)val27).set_Width(labelWidth);
			val27.set_AutoSizeHeight(false);
			val27.set_WrapText(false);
			((Control)val27).set_Parent((Container)(object)mountsLeftPanel);
			val27.set_Text("Skyscale: ");
			Label settingSkyscale_Label = val27;
			Dropdown val28 = new Dropdown();
			((Control)val28).set_Location(new Point(((Control)settingSkyscale_Label).get_Right() + 5, ((Control)settingSkyscale_Label).get_Top() - 4));
			((Control)val28).set_Width(orderWidth);
			((Control)val28).set_Parent((Container)(object)mountsLeftPanel);
			Dropdown settingSkyscale_Select = val28;
			int[] mountOrder8 = Module._mountOrder;
			for (int num8 = 0; num8 < mountOrder8.Length; num8++)
			{
				int i3 = mountOrder8[num8];
				if (i3 == 0)
				{
					settingSkyscale_Select.get_Items().Add("Disabled");
				}
				else
				{
					settingSkyscale_Select.get_Items().Add(i3.ToString());
				}
			}
			settingSkyscale_Select.set_SelectedItem((Module._settingSkyscaleOrder.get_Value() == 0) ? "Disabled" : Module._settingSkyscaleOrder.get_Value().ToString());
			settingSkyscale_Select.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				if (settingSkyscale_Select.get_SelectedItem().Equals("Disabled"))
				{
					Module._settingSkyscaleOrder.set_Value(0);
				}
				else
				{
					Module._settingSkyscaleOrder.set_Value(int.Parse(settingSkyscale_Select.get_SelectedItem()));
				}
			});
			KeybindingAssigner val29 = new KeybindingAssigner();
			val29.set_NameWidth(0);
			((Control)val29).set_Size(new Point(bindingWidth, 20));
			((Control)val29).set_Parent((Container)(object)mountsLeftPanel);
			val29.set_KeyBinding(Module._settingSkyscaleBinding.get_Value());
			((Control)val29).set_Location(new Point(((Control)settingSkyscale_Select).get_Right() + 5, ((Control)settingSkyscale_Label).get_Top() - 1));
			KeybindingAssigner settingSkyscale_Keybind = val29;
			Label val30 = new Label();
			((Control)val30).set_Location(new Point(0, ((Control)settingSkyscale_Label).get_Bottom() + 6));
			((Control)val30).set_Width(labelWidth);
			val30.set_AutoSizeHeight(false);
			val30.set_WrapText(false);
			((Control)val30).set_Parent((Container)(object)mountsLeftPanel);
			val30.set_Text("Default mount: ");
			Label settingDefaultMount_Label = val30;
			Dropdown val31 = new Dropdown();
			((Control)val31).set_Location(new Point(((Control)settingDefaultMount_Label).get_Right() + 5, ((Control)settingDefaultMount_Label).get_Top() - 4));
			((Control)val31).set_Width(orderWidth);
			((Control)val31).set_Parent((Container)(object)mountsLeftPanel);
			Dropdown settingDefaultMount_Select = val31;
			string[] defaultMountChoices = Module._defaultMountChoices;
			foreach (string i4 in defaultMountChoices)
			{
				settingDefaultMount_Select.get_Items().Add(i4.ToString());
			}
			settingDefaultMount_Select.set_SelectedItem(Array.Exists(Module._defaultMountChoices, (string e) => e == Module._settingDefaultMountChoice.get_Value()) ? Module._settingDefaultMountChoice.get_Value() : "Disabled");
			settingDefaultMount_Select.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				Module._settingDefaultMountChoice.set_Value(settingDefaultMount_Select.get_SelectedItem());
			});
			KeybindingAssigner val32 = new KeybindingAssigner();
			val32.set_NameWidth(0);
			((Control)val32).set_Size(new Point(bindingWidth, 20));
			((Control)val32).set_Parent((Container)(object)mountsLeftPanel);
			val32.set_KeyBinding(Module._settingDefaultMountBinding.get_Value());
			((Control)val32).set_Location(new Point(((Control)settingDefaultMount_Select).get_Right() + 5, ((Control)settingDefaultMount_Label).get_Top() - 1));
			KeybindingAssigner settingDefaultMount_Keybind = val32;
			Label val33 = new Label();
			((Control)val33).set_Location(new Point(0, 4));
			((Control)val33).set_Width(labelWidth);
			val33.set_AutoSizeHeight(false);
			val33.set_WrapText(false);
			((Control)val33).set_Parent((Container)(object)otherPanel);
			val33.set_Text("Display: ");
			Label settingDisplay_Label = val33;
			Dropdown val34 = new Dropdown();
			((Control)val34).set_Location(new Point(((Control)settingDisplay_Label).get_Right() + 5, ((Control)settingDisplay_Label).get_Top() - 4));
			((Control)val34).set_Width(160);
			((Control)val34).set_Parent((Container)(object)otherPanel);
			Dropdown settingDisplay_Select = val34;
			string[] mountDisplay = Module._mountDisplay;
			foreach (string s in mountDisplay)
			{
				settingDisplay_Select.get_Items().Add(s);
			}
			settingDisplay_Select.set_SelectedItem(Module._settingDisplay.get_Value());
			settingDisplay_Select.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				Module._settingDisplay.set_Value(settingDisplay_Select.get_SelectedItem());
				if (settingDisplay_Select.get_SelectedItem().Equals("Transparent Manual") || settingDisplay_Select.get_SelectedItem().Equals("Solid Manual") || settingDisplay_Select.get_SelectedItem().Equals("Solid Manual Text"))
				{
					((Control)manualPanel).Show();
				}
				else
				{
					((Control)manualPanel).Hide();
				}
			});
			Label val35 = new Label();
			((Control)val35).set_Location(new Point(0, 2));
			((Control)val35).set_Width(((Control)manualPanel).get_Width());
			val35.set_AutoSizeHeight(false);
			val35.set_WrapText(false);
			((Control)val35).set_Parent((Container)(object)manualPanel);
			val35.set_Text("Manual Settings");
			val35.set_HorizontalAlignment((HorizontalAlignment)1);
			Label settingManual_Label = val35;
			Label val36 = new Label();
			((Control)val36).set_Location(new Point(0, ((Control)settingManual_Label).get_Bottom() + 6));
			((Control)val36).set_Width(75);
			val36.set_AutoSizeHeight(false);
			val36.set_WrapText(false);
			((Control)val36).set_Parent((Container)(object)manualPanel);
			val36.set_Text("Orientation: ");
			Label settingManualOrientation_Label = val36;
			Dropdown val37 = new Dropdown();
			((Control)val37).set_Location(new Point(((Control)settingManualOrientation_Label).get_Right() + 5, ((Control)settingManualOrientation_Label).get_Top() - 4));
			((Control)val37).set_Width(100);
			((Control)val37).set_Parent((Container)(object)manualPanel);
			Dropdown settingManualOrientation_Select = val37;
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
			Label val38 = new Label();
			((Control)val38).set_Location(new Point(0, ((Control)settingManualOrientation_Label).get_Bottom() + 6));
			((Control)val38).set_Width(75);
			val38.set_AutoSizeHeight(false);
			val38.set_WrapText(false);
			((Control)val38).set_Parent((Container)(object)manualPanel);
			val38.set_Text("Icon Width: ");
			Label settingManualWidth_Label = val38;
			TrackBar val39 = new TrackBar();
			((Control)val39).set_Location(new Point(((Control)settingManualWidth_Label).get_Right() + 5, ((Control)settingManualWidth_Label).get_Top()));
			((Control)val39).set_Width(220);
			val39.set_MaxValue(200f);
			val39.set_MinValue(0f);
			val39.set_Value((float)Module._settingImgWidth.get_Value());
			((Control)val39).set_Parent((Container)(object)manualPanel);
			TrackBar settingImgWidth_Slider = val39;
			settingImgWidth_Slider.add_ValueChanged((EventHandler<ValueEventArgs<float>>)delegate
			{
				Module._settingImgWidth.set_Value((int)settingImgWidth_Slider.get_Value());
			});
			Label val40 = new Label();
			((Control)val40).set_Location(new Point(0, ((Control)settingManualWidth_Label).get_Bottom() + 6));
			((Control)val40).set_Width(75);
			val40.set_AutoSizeHeight(false);
			val40.set_WrapText(false);
			((Control)val40).set_Parent((Container)(object)manualPanel);
			val40.set_Text("Opacity: ");
			Label settingManualOpacity_Label = val40;
			TrackBar val41 = new TrackBar();
			((Control)val41).set_Location(new Point(((Control)settingManualOpacity_Label).get_Right() + 5, ((Control)settingManualOpacity_Label).get_Top()));
			((Control)val41).set_Width(220);
			val41.set_MaxValue(100f);
			val41.set_MinValue(0f);
			val41.set_Value(Module._settingOpacity.get_Value() * 100f);
			((Control)val41).set_Parent((Container)(object)manualPanel);
			TrackBar settingOpacity_Slider = val41;
			settingOpacity_Slider.add_ValueChanged((EventHandler<ValueEventArgs<float>>)delegate
			{
				Module._settingOpacity.set_Value(settingOpacity_Slider.get_Value() / 100f);
			});
			IView settingClockDrag_View = SettingView.FromType((SettingEntry)(object)Module._settingDrag, ((Control)buildPanel).get_Width());
			ViewContainer val42 = new ViewContainer();
			((Container)val42).set_WidthSizingMode((SizingMode)2);
			((Control)val42).set_Location(new Point(0, ((Control)settingManualOpacity_Label).get_Bottom() + 3));
			((Control)val42).set_Parent((Container)(object)manualPanel);
			ViewContainer settingClockDrag_Container = val42;
			settingClockDrag_Container.Show(settingClockDrag_View);
		}

		public SettingsView()
			: this()
		{
		}
	}
}
