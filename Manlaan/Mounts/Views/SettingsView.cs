using System;
using System.Collections.Generic;
using System.Linq;
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
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Expected O, but got Unknown
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			//IL_0151: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_0161: Unknown result type (might be due to invalid IL or missing references)
			//IL_0169: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Unknown result type (might be due to invalid IL or missing references)
			//IL_017e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0188: Expected O, but got Unknown
			//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01df: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0200: Unknown result type (might be due to invalid IL or missing references)
			//IL_0224: Expected O, but got Unknown
			//IL_0226: Unknown result type (might be due to invalid IL or missing references)
			//IL_022b: Unknown result type (might be due to invalid IL or missing references)
			//IL_023e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0249: Unknown result type (might be due to invalid IL or missing references)
			//IL_0251: Unknown result type (might be due to invalid IL or missing references)
			//IL_025f: Expected O, but got Unknown
			//IL_0325: Unknown result type (might be due to invalid IL or missing references)
			//IL_032a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0332: Unknown result type (might be due to invalid IL or missing references)
			//IL_0336: Unknown result type (might be due to invalid IL or missing references)
			//IL_0341: Unknown result type (might be due to invalid IL or missing references)
			//IL_034a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0367: Unknown result type (might be due to invalid IL or missing references)
			//IL_037f: Unknown result type (might be due to invalid IL or missing references)
			//IL_038c: Expected O, but got Unknown
			//IL_03b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f7: Expected O, but got Unknown
			//IL_03f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_03fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0407: Unknown result type (might be due to invalid IL or missing references)
			//IL_0412: Unknown result type (might be due to invalid IL or missing references)
			//IL_041a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0422: Unknown result type (might be due to invalid IL or missing references)
			//IL_042a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0433: Unknown result type (might be due to invalid IL or missing references)
			//IL_0441: Expected O, but got Unknown
			//IL_0442: Unknown result type (might be due to invalid IL or missing references)
			//IL_0447: Unknown result type (might be due to invalid IL or missing references)
			//IL_045a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0465: Unknown result type (might be due to invalid IL or missing references)
			//IL_046d: Unknown result type (might be due to invalid IL or missing references)
			//IL_047b: Expected O, but got Unknown
			//IL_0560: Unknown result type (might be due to invalid IL or missing references)
			//IL_0565: Unknown result type (might be due to invalid IL or missing references)
			//IL_0574: Unknown result type (might be due to invalid IL or missing references)
			//IL_057f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0587: Unknown result type (might be due to invalid IL or missing references)
			//IL_058f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0597: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ae: Expected O, but got Unknown
			//IL_05af: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_05da: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e8: Expected O, but got Unknown
			//IL_06f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_06f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0705: Unknown result type (might be due to invalid IL or missing references)
			//IL_0710: Unknown result type (might be due to invalid IL or missing references)
			//IL_0718: Unknown result type (might be due to invalid IL or missing references)
			//IL_0720: Unknown result type (might be due to invalid IL or missing references)
			//IL_0728: Unknown result type (might be due to invalid IL or missing references)
			//IL_0731: Unknown result type (might be due to invalid IL or missing references)
			//IL_073f: Expected O, but got Unknown
			//IL_073f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0744: Unknown result type (might be due to invalid IL or missing references)
			//IL_074c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0750: Unknown result type (might be due to invalid IL or missing references)
			//IL_075b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0764: Unknown result type (might be due to invalid IL or missing references)
			//IL_0775: Unknown result type (might be due to invalid IL or missing references)
			//IL_0788: Unknown result type (might be due to invalid IL or missing references)
			//IL_0795: Expected O, but got Unknown
			//IL_0795: Unknown result type (might be due to invalid IL or missing references)
			//IL_079a: Unknown result type (might be due to invalid IL or missing references)
			//IL_07a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_07b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_07b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_07c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_07c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_07d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_07df: Expected O, but got Unknown
			//IL_07e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_07e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_07f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0803: Unknown result type (might be due to invalid IL or missing references)
			//IL_080b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0819: Expected O, but got Unknown
			//IL_08e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_08e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_08f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_08fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0904: Unknown result type (might be due to invalid IL or missing references)
			//IL_090c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0914: Unknown result type (might be due to invalid IL or missing references)
			//IL_091d: Unknown result type (might be due to invalid IL or missing references)
			//IL_092b: Expected O, but got Unknown
			//IL_092c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0931: Unknown result type (might be due to invalid IL or missing references)
			//IL_0935: Unknown result type (might be due to invalid IL or missing references)
			//IL_0940: Unknown result type (might be due to invalid IL or missing references)
			//IL_0949: Unknown result type (might be due to invalid IL or missing references)
			//IL_095a: Unknown result type (might be due to invalid IL or missing references)
			//IL_096d: Unknown result type (might be due to invalid IL or missing references)
			//IL_097d: Expected O, but got Unknown
			//IL_0995: Unknown result type (might be due to invalid IL or missing references)
			//IL_099a: Unknown result type (might be due to invalid IL or missing references)
			//IL_09a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_09b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_09b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_09c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_09c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_09d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_09e0: Expected O, but got Unknown
			//IL_09e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_09e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_09f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_09fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a03: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a0b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a13: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a1c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a2a: Expected O, but got Unknown
			//IL_0a2b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a30: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a34: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a3f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a48: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a59: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a6c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a7c: Expected O, but got Unknown
			//IL_0a94: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a99: Unknown result type (might be due to invalid IL or missing references)
			//IL_0aa4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0aaf: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ab7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0abf: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ac7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ad0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ade: Expected O, but got Unknown
			//IL_0adf: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ae4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0af5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b00: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b09: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b15: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b21: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b38: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b46: Expected O, but got Unknown
			//IL_0b5e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b63: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b6e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b79: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b81: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b89: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b91: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b9a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ba8: Expected O, but got Unknown
			//IL_0ba9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bae: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bbf: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bca: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bd3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bdf: Unknown result type (might be due to invalid IL or missing references)
			//IL_0beb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c02: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c10: Expected O, but got Unknown
			//IL_0c28: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c2d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c38: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c43: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c4b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c53: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c5b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c64: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c72: Expected O, but got Unknown
			//IL_0c73: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c78: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c8b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c96: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c9e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0cac: Expected O, but got Unknown
			//IL_0d16: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d1b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d1e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d29: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d31: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d39: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d41: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d4a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d58: Expected O, but got Unknown
			//IL_0d59: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d5e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d71: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d7c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d88: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d96: Expected O, but got Unknown
			//IL_0dfb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e00: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e0b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e16: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e1e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e26: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e2e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e37: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e45: Expected O, but got Unknown
			//IL_0e46: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e4b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e4f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e5a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e63: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e74: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e87: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e97: Expected O, but got Unknown
			//IL_0eaf: Unknown result type (might be due to invalid IL or missing references)
			//IL_0eb4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ebf: Unknown result type (might be due to invalid IL or missing references)
			//IL_0eca: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ed2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0eda: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ee2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0eeb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ef9: Expected O, but got Unknown
			//IL_0efa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0eff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f03: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f0e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f17: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f28: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f3b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f4b: Expected O, but got Unknown
			//IL_0f63: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f68: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f6b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f76: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f88: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f90: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f98: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fa5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fb1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fbb: Expected O, but got Unknown
			//IL_0fbb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fc0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fcb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fd6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fdf: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fe7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fef: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ffc: Unknown result type (might be due to invalid IL or missing references)
			//IL_100a: Expected O, but got Unknown
			//IL_100b: Unknown result type (might be due to invalid IL or missing references)
			//IL_1010: Unknown result type (might be due to invalid IL or missing references)
			//IL_1023: Unknown result type (might be due to invalid IL or missing references)
			//IL_102e: Unknown result type (might be due to invalid IL or missing references)
			//IL_1037: Unknown result type (might be due to invalid IL or missing references)
			//IL_1049: Expected O, but got Unknown
			//IL_10ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_10b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_10be: Unknown result type (might be due to invalid IL or missing references)
			//IL_10c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_10d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_10da: Unknown result type (might be due to invalid IL or missing references)
			//IL_10e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_10ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_10fd: Expected O, but got Unknown
			//IL_10fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_1103: Unknown result type (might be due to invalid IL or missing references)
			//IL_1114: Unknown result type (might be due to invalid IL or missing references)
			//IL_111f: Unknown result type (might be due to invalid IL or missing references)
			//IL_112b: Unknown result type (might be due to invalid IL or missing references)
			//IL_1137: Unknown result type (might be due to invalid IL or missing references)
			//IL_1143: Unknown result type (might be due to invalid IL or missing references)
			//IL_1155: Unknown result type (might be due to invalid IL or missing references)
			//IL_1167: Expected O, but got Unknown
			//IL_117f: Unknown result type (might be due to invalid IL or missing references)
			//IL_1184: Unknown result type (might be due to invalid IL or missing references)
			//IL_118f: Unknown result type (might be due to invalid IL or missing references)
			//IL_119a: Unknown result type (might be due to invalid IL or missing references)
			//IL_11a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_11ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_11b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_11c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_11ce: Expected O, but got Unknown
			//IL_11cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_11d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_11e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_11f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_11fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_1208: Unknown result type (might be due to invalid IL or missing references)
			//IL_1214: Unknown result type (might be due to invalid IL or missing references)
			//IL_122b: Unknown result type (might be due to invalid IL or missing references)
			//IL_123d: Expected O, but got Unknown
			//IL_1267: Unknown result type (might be due to invalid IL or missing references)
			//IL_126c: Unknown result type (might be due to invalid IL or missing references)
			//IL_1274: Unknown result type (might be due to invalid IL or missing references)
			//IL_127f: Unknown result type (might be due to invalid IL or missing references)
			//IL_128a: Unknown result type (might be due to invalid IL or missing references)
			//IL_1299: Expected O, but got Unknown
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
			((Control)val3).set_Location(new Point(((Control)mountsLeftPanel).get_Right() + 20, 93));
			Panel manualPanel = val3;
			DisplayManualPanelIfNeeded(manualPanel);
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
				Dropdown settingRaptor_Select = val7;
				int[] mountOrder = Module._mountOrder;
				for (int num = 0; num < mountOrder.Length; num++)
				{
					int n = mountOrder[num];
					if (n == 0)
					{
						settingRaptor_Select.get_Items().Add("Disabled");
					}
					else
					{
						settingRaptor_Select.get_Items().Add(n.ToString());
					}
				}
				settingRaptor_Select.set_SelectedItem((mount.OrderSetting.get_Value() == 0) ? "Disabled" : mount.OrderSetting.get_Value().ToString());
				settingRaptor_Select.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
				{
					if (settingRaptor_Select.get_SelectedItem().Equals("Disabled"))
					{
						mount.OrderSetting.set_Value(0);
					}
					else
					{
						mount.OrderSetting.set_Value(int.Parse(settingRaptor_Select.get_SelectedItem()));
					}
				});
				KeybindingAssigner val8 = new KeybindingAssigner();
				val8.set_NameWidth(0);
				((Control)val8).set_Size(new Point(bindingWidth, 20));
				((Control)val8).set_Parent((Container)(object)mountsLeftPanel);
				val8.set_KeyBinding(mount.KeybindingSetting.get_Value());
				((Control)val8).set_Location(new Point(((Control)settingRaptor_Select).get_Right() + 5, ((Control)settingMount_Label).get_Top() - 1));
				KeybindingAssigner settingRaptor_Keybind = val8;
				curY = ((Control)settingMount_Label).get_Bottom();
			}
			Label val9 = new Label();
			((Control)val9).set_Location(new Point(0, curY + 24));
			((Control)val9).set_Width(bindingWidth);
			val9.set_AutoSizeHeight(false);
			val9.set_WrapText(false);
			((Control)val9).set_Parent((Container)(object)mountsLeftPanel);
			val9.set_Text("Default mount settings: ");
			Label settingDefaultSettingsMount_Label = val9;
			Label val10 = new Label();
			((Control)val10).set_Location(new Point(0, ((Control)settingDefaultSettingsMount_Label).get_Bottom() + 6));
			((Control)val10).set_Width(bindingWidth);
			val10.set_AutoSizeHeight(false);
			val10.set_WrapText(false);
			((Control)val10).set_Parent((Container)(object)mountsLeftPanel);
			val10.set_Text("Default mount: ");
			Label settingDefaultMount_Label = val10;
			Dropdown val11 = new Dropdown();
			((Control)val11).set_Location(new Point(((Control)settingDefaultMount_Label).get_Right() + 5, ((Control)settingDefaultMount_Label).get_Top() - 4));
			((Control)val11).set_Width(orderWidth);
			((Control)val11).set_Parent((Container)(object)mountsLeftPanel);
			Dropdown settingDefaultMount_Select = val11;
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
			Label val12 = new Label();
			((Control)val12).set_Location(new Point(0, ((Control)settingDefaultMount_Select).get_Bottom() + 6));
			((Control)val12).set_Width(bindingWidth);
			val12.set_AutoSizeHeight(false);
			val12.set_WrapText(false);
			((Control)val12).set_Parent((Container)(object)mountsLeftPanel);
			val12.set_Text("Default water mount: ");
			Label settingDefaultWaterMount_Label = val12;
			Dropdown val13 = new Dropdown();
			((Control)val13).set_Location(new Point(((Control)settingDefaultWaterMount_Label).get_Right() + 5, ((Control)settingDefaultWaterMount_Label).get_Top() - 4));
			((Control)val13).set_Width(orderWidth);
			((Control)val13).set_Parent((Container)(object)mountsLeftPanel);
			Dropdown settingDefaultWaterMount_Select = val13;
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
			Label val14 = new Label();
			((Control)val14).set_Location(new Point(0, ((Control)settingDefaultWaterMount_Select).get_Bottom() + 6));
			((Control)val14).set_Width(bindingWidth);
			val14.set_AutoSizeHeight(false);
			val14.set_WrapText(false);
			((Control)val14).set_Parent((Container)(object)mountsLeftPanel);
			val14.set_Text("Keybind: ");
			Label settingDefaultMountKeybind_Label = val14;
			KeybindingAssigner val15 = new KeybindingAssigner();
			val15.set_NameWidth(0);
			((Control)val15).set_Size(new Point(bindingWidth, 20));
			((Control)val15).set_Parent((Container)(object)mountsLeftPanel);
			val15.set_KeyBinding(Module._settingDefaultMountBinding.get_Value());
			((Control)val15).set_Location(new Point(((Control)settingDefaultMountKeybind_Label).get_Right() + 5, ((Control)settingDefaultMountKeybind_Label).get_Top() - 1));
			KeybindingAssigner settingDefaultMount_Keybind = val15;
			Label val16 = new Label();
			((Control)val16).set_Location(new Point(0, ((Control)settingDefaultMountKeybind_Label).get_Bottom() + 6));
			((Control)val16).set_Width(bindingWidth);
			val16.set_AutoSizeHeight(false);
			val16.set_WrapText(false);
			((Control)val16).set_Parent((Container)(object)mountsLeftPanel);
			val16.set_Text("Keybind behaviour: ");
			Label settingDefaultMountBehaviour_Label = val16;
			Dropdown val17 = new Dropdown();
			((Control)val17).set_Location(new Point(((Control)settingDefaultMountBehaviour_Label).get_Right() + 5, ((Control)settingDefaultMountBehaviour_Label).get_Top() - 4));
			((Control)val17).set_Width(orderWidth);
			((Control)val17).set_Parent((Container)(object)mountsLeftPanel);
			Dropdown settingDefaultMountBehaviour_Select = val17;
			settingDefaultMountBehaviour_Select.get_Items().Add("Disabled");
			List<string> mountBehaviours = Module._mountBehaviour.ToList();
			foreach (string j in mountBehaviours)
			{
				settingDefaultMountBehaviour_Select.get_Items().Add(j.ToString());
			}
			settingDefaultMountBehaviour_Select.set_SelectedItem(mountBehaviours.Any((string m) => m == Module._settingDefaultMountBehaviour.get_Value()) ? Module._settingDefaultMountBehaviour.get_Value() : "Disabled");
			settingDefaultMountBehaviour_Select.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				Module._settingDefaultMountBehaviour.set_Value(settingDefaultMountBehaviour_Select.get_SelectedItem());
			});
			Label val18 = new Label();
			((Control)val18).set_Location(new Point(0, ((Control)settingDefaultMountBehaviour_Label).get_Bottom() + 6));
			((Control)val18).set_Width(bindingWidth);
			val18.set_AutoSizeHeight(false);
			val18.set_WrapText(false);
			((Control)val18).set_Parent((Container)(object)mountsLeftPanel);
			val18.set_Text("Display Mount Queueing: ");
			Label settingDisplayMountQueueing_Label = val18;
			Checkbox val19 = new Checkbox();
			((Control)val19).set_Size(new Point(bindingWidth, 20));
			((Control)val19).set_Parent((Container)(object)mountsLeftPanel);
			val19.set_Checked(Module._settingDisplayMountQueueing.get_Value());
			((Control)val19).set_Location(new Point(((Control)settingDisplayMountQueueing_Label).get_Right() + 5, ((Control)settingDisplayMountQueueing_Label).get_Top() - 1));
			Checkbox settingDisplayMountQueueing_Checkbox = val19;
			settingDisplayMountQueueing_Checkbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				Module._settingDisplayMountQueueing.set_Value(settingDisplayMountQueueing_Checkbox.get_Checked());
			});
			Label val20 = new Label();
			((Control)val20).set_Location(new Point(0, ((Control)settingDisplayMountQueueing_Label).get_Bottom() + 24));
			((Control)val20).set_Width(bindingWidth);
			val20.set_AutoSizeHeight(false);
			val20.set_WrapText(false);
			((Control)val20).set_Parent((Container)(object)mountsLeftPanel);
			val20.set_Text("Radial settings: ");
			Label settingMountRadialSettingsMount_Label = val20;
			Label val21 = new Label();
			((Control)val21).set_Location(new Point(0, ((Control)settingMountRadialSettingsMount_Label).get_Bottom() + 6));
			((Control)val21).set_Width(bindingWidth);
			val21.set_AutoSizeHeight(false);
			val21.set_WrapText(false);
			((Control)val21).set_Parent((Container)(object)mountsLeftPanel);
			val21.set_Text("Spawn at mouse: ");
			Label settingMountRadialSpawnAtMouse_Label = val21;
			Checkbox val22 = new Checkbox();
			((Control)val22).set_Size(new Point(bindingWidth, 20));
			((Control)val22).set_Parent((Container)(object)mountsLeftPanel);
			val22.set_Checked(Module._settingMountRadialSpawnAtMouse.get_Value());
			((Control)val22).set_Location(new Point(((Control)settingMountRadialSpawnAtMouse_Label).get_Right() + 5, ((Control)settingMountRadialSpawnAtMouse_Label).get_Top() - 1));
			Checkbox settingMountRadialSpawnAtMouse_Checkbox = val22;
			settingMountRadialSpawnAtMouse_Checkbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				Module._settingMountRadialSpawnAtMouse.set_Value(settingMountRadialSpawnAtMouse_Checkbox.get_Checked());
			});
			Label val23 = new Label();
			((Control)val23).set_Location(new Point(0, ((Control)settingMountRadialSpawnAtMouse_Label).get_Bottom() + 6));
			((Control)val23).set_Width(bindingWidth);
			val23.set_AutoSizeHeight(false);
			val23.set_WrapText(false);
			((Control)val23).set_Parent((Container)(object)mountsLeftPanel);
			val23.set_Text("Radius: ");
			Label settingMountRadialRadiusModifier_Label = val23;
			TrackBar val24 = new TrackBar();
			((Control)val24).set_Location(new Point(((Control)settingMountRadialRadiusModifier_Label).get_Right() + 5, ((Control)settingMountRadialRadiusModifier_Label).get_Top()));
			((Control)val24).set_Width(120);
			val24.set_MaxValue(100f);
			val24.set_MinValue(20f);
			val24.set_Value(Module._settingMountRadialRadiusModifier.get_Value() * 100f);
			((Control)val24).set_Parent((Container)(object)mountsLeftPanel);
			TrackBar settingMountRadialRadiusModifier_Slider = val24;
			settingMountRadialRadiusModifier_Slider.add_ValueChanged((EventHandler<ValueEventArgs<float>>)delegate
			{
				Module._settingMountRadialRadiusModifier.set_Value(settingMountRadialRadiusModifier_Slider.get_Value() / 100f);
			});
			Label val25 = new Label();
			((Control)val25).set_Location(new Point(0, ((Control)settingMountRadialRadiusModifier_Label).get_Bottom() + 6));
			((Control)val25).set_Width(bindingWidth);
			val25.set_AutoSizeHeight(false);
			val25.set_WrapText(false);
			((Control)val25).set_Parent((Container)(object)mountsLeftPanel);
			val25.set_Text("Icon size: ");
			Label settingMountRadialIconSizeModifier_Label = val25;
			TrackBar val26 = new TrackBar();
			((Control)val26).set_Location(new Point(((Control)settingMountRadialIconSizeModifier_Label).get_Right() + 5, ((Control)settingMountRadialIconSizeModifier_Label).get_Top()));
			((Control)val26).set_Width(120);
			val26.set_MaxValue(100f);
			val26.set_MinValue(5f);
			val26.set_Value(Module._settingMountRadialIconSizeModifier.get_Value() * 100f);
			((Control)val26).set_Parent((Container)(object)mountsLeftPanel);
			TrackBar settingMountRadialIconSizeModifier_Slider = val26;
			settingMountRadialIconSizeModifier_Slider.add_ValueChanged((EventHandler<ValueEventArgs<float>>)delegate
			{
				Module._settingMountRadialIconSizeModifier.set_Value(settingMountRadialIconSizeModifier_Slider.get_Value() / 100f);
			});
			Label val27 = new Label();
			((Control)val27).set_Location(new Point(0, ((Control)settingMountRadialIconSizeModifier_Label).get_Bottom() + 6));
			((Control)val27).set_Width(bindingWidth);
			val27.set_AutoSizeHeight(false);
			val27.set_WrapText(false);
			((Control)val27).set_Parent((Container)(object)mountsLeftPanel);
			val27.set_Text("Center mount: ");
			Label settingMountRadialCenterMountBehavior_Label = val27;
			Dropdown val28 = new Dropdown();
			((Control)val28).set_Location(new Point(((Control)settingMountRadialCenterMountBehavior_Label).get_Right() + 5, ((Control)settingMountRadialCenterMountBehavior_Label).get_Top() - 4));
			((Control)val28).set_Width(orderWidth);
			((Control)val28).set_Parent((Container)(object)mountsLeftPanel);
			Dropdown settingMountRadialCenterMountBehavior_Select = val28;
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
			Label val29 = new Label();
			((Control)val29).set_Location(new Point(0, 4));
			((Control)val29).set_Width(labelWidth);
			val29.set_AutoSizeHeight(false);
			val29.set_WrapText(false);
			((Control)val29).set_Parent((Container)(object)otherPanel);
			val29.set_Text("Display: ");
			Label settingDisplay_Label = val29;
			Dropdown val30 = new Dropdown();
			((Control)val30).set_Location(new Point(((Control)settingDisplay_Label).get_Right() + 5, ((Control)settingDisplay_Label).get_Top() - 4));
			((Control)val30).set_Width(160);
			((Control)val30).set_Parent((Container)(object)otherPanel);
			Dropdown settingDisplay_Select = val30;
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
			Label val31 = new Label();
			((Control)val31).set_Location(new Point(0, ((Control)settingDisplay_Label).get_Bottom() + 6));
			((Control)val31).set_Width(bindingWidth);
			val31.set_AutoSizeHeight(false);
			val31.set_WrapText(false);
			((Control)val31).set_Parent((Container)(object)otherPanel);
			val31.set_Text("Display Corner Icons: ");
			Label settingDisplayCornerIcons_Label = val31;
			Checkbox val32 = new Checkbox();
			((Control)val32).set_Size(new Point(bindingWidth, 20));
			((Control)val32).set_Parent((Container)(object)otherPanel);
			val32.set_Checked(Module._settingDisplayCornerIcons.get_Value());
			((Control)val32).set_Location(new Point(((Control)settingDisplayCornerIcons_Label).get_Right() + 5, ((Control)settingDisplayCornerIcons_Label).get_Top() - 1));
			Checkbox settingDisplayCornerIcons_Checkbox = val32;
			settingDisplayCornerIcons_Checkbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				Module._settingDisplayCornerIcons.set_Value(settingDisplayCornerIcons_Checkbox.get_Checked());
			});
			Label val33 = new Label();
			((Control)val33).set_Location(new Point(0, ((Control)settingDisplayCornerIcons_Label).get_Bottom() + 6));
			((Control)val33).set_Width(bindingWidth);
			val33.set_AutoSizeHeight(false);
			val33.set_WrapText(false);
			((Control)val33).set_Parent((Container)(object)otherPanel);
			val33.set_Text("Display Manual Icons: ");
			Label settingDisplayManualIcons_Label = val33;
			Checkbox val34 = new Checkbox();
			((Control)val34).set_Size(new Point(bindingWidth, 20));
			((Control)val34).set_Parent((Container)(object)otherPanel);
			val34.set_Checked(Module._settingDisplayManualIcons.get_Value());
			((Control)val34).set_Location(new Point(((Control)settingDisplayManualIcons_Label).get_Right() + 5, ((Control)settingDisplayManualIcons_Label).get_Top() - 1));
			Checkbox settingDisplayManualIcons_Checkbox = val34;
			settingDisplayManualIcons_Checkbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				Module._settingDisplayManualIcons.set_Value(settingDisplayManualIcons_Checkbox.get_Checked());
				DisplayManualPanelIfNeeded(manualPanel);
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

		public SettingsView()
			: this()
		{
		}
	}
}
