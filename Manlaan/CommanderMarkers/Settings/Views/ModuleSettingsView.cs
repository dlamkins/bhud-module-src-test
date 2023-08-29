using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Blish_HUD.Settings.UI.Views;
using Manlaan.CommanderMarkers.Settings.Enums;
using Manlaan.CommanderMarkers.Settings.Services;
using Microsoft.Xna.Framework;

namespace Manlaan.CommanderMarkers.Settings.Views
{
	internal class ModuleSettingsView : View
	{
		protected SettingService _settings;

		public ModuleSettingsView(SettingService settings)
			: this()
		{
			_settings = settings;
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Expected O, but got Unknown
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Expected O, but got Unknown
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Expected O, but got Unknown
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			//IL_017e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0188: Unknown result type (might be due to invalid IL or missing references)
			//IL_018f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0196: Unknown result type (might be due to invalid IL or missing references)
			//IL_019d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b1: Expected O, but got Unknown
			//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01be: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_020b: Expected O, but got Unknown
			//IL_0223: Unknown result type (might be due to invalid IL or missing references)
			//IL_0228: Unknown result type (might be due to invalid IL or missing references)
			//IL_022f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0233: Unknown result type (might be due to invalid IL or missing references)
			//IL_023d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0244: Unknown result type (might be due to invalid IL or missing references)
			//IL_025a: Unknown result type (might be due to invalid IL or missing references)
			//IL_026f: Unknown result type (might be due to invalid IL or missing references)
			//IL_027e: Expected O, but got Unknown
			//IL_0295: Unknown result type (might be due to invalid IL or missing references)
			//IL_029a: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d6: Expected O, but got Unknown
			//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_030e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0321: Unknown result type (might be due to invalid IL or missing references)
			//IL_0330: Expected O, but got Unknown
			//IL_0348: Unknown result type (might be due to invalid IL or missing references)
			//IL_034d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0354: Unknown result type (might be due to invalid IL or missing references)
			//IL_0358: Unknown result type (might be due to invalid IL or missing references)
			//IL_0362: Unknown result type (might be due to invalid IL or missing references)
			//IL_0369: Unknown result type (might be due to invalid IL or missing references)
			//IL_037f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0394: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a3: Expected O, but got Unknown
			//IL_03ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_03bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_03fb: Expected O, but got Unknown
			//IL_03fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0401: Unknown result type (might be due to invalid IL or missing references)
			//IL_0408: Unknown result type (might be due to invalid IL or missing references)
			//IL_040c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0416: Unknown result type (might be due to invalid IL or missing references)
			//IL_041d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0433: Unknown result type (might be due to invalid IL or missing references)
			//IL_0446: Unknown result type (might be due to invalid IL or missing references)
			//IL_0455: Expected O, but got Unknown
			//IL_046d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0472: Unknown result type (might be due to invalid IL or missing references)
			//IL_0479: Unknown result type (might be due to invalid IL or missing references)
			//IL_047d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0487: Unknown result type (might be due to invalid IL or missing references)
			//IL_048e: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c8: Expected O, but got Unknown
			//IL_04df: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_04fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0505: Unknown result type (might be due to invalid IL or missing references)
			//IL_050c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0513: Unknown result type (might be due to invalid IL or missing references)
			//IL_0520: Expected O, but got Unknown
			//IL_0521: Unknown result type (might be due to invalid IL or missing references)
			//IL_0526: Unknown result type (might be due to invalid IL or missing references)
			//IL_052d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0531: Unknown result type (might be due to invalid IL or missing references)
			//IL_053b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0542: Unknown result type (might be due to invalid IL or missing references)
			//IL_0558: Unknown result type (might be due to invalid IL or missing references)
			//IL_056b: Unknown result type (might be due to invalid IL or missing references)
			//IL_057a: Expected O, but got Unknown
			//IL_0592: Unknown result type (might be due to invalid IL or missing references)
			//IL_0597: Unknown result type (might be due to invalid IL or missing references)
			//IL_059e: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_05de: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ed: Expected O, but got Unknown
			//IL_0604: Unknown result type (might be due to invalid IL or missing references)
			//IL_0609: Unknown result type (might be due to invalid IL or missing references)
			//IL_0612: Unknown result type (might be due to invalid IL or missing references)
			//IL_061c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0623: Unknown result type (might be due to invalid IL or missing references)
			//IL_062a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0631: Unknown result type (might be due to invalid IL or missing references)
			//IL_0638: Unknown result type (might be due to invalid IL or missing references)
			//IL_0645: Expected O, but got Unknown
			//IL_0646: Unknown result type (might be due to invalid IL or missing references)
			//IL_064b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0652: Unknown result type (might be due to invalid IL or missing references)
			//IL_0656: Unknown result type (might be due to invalid IL or missing references)
			//IL_0660: Unknown result type (might be due to invalid IL or missing references)
			//IL_0667: Unknown result type (might be due to invalid IL or missing references)
			//IL_067d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0690: Unknown result type (might be due to invalid IL or missing references)
			//IL_069f: Expected O, but got Unknown
			//IL_06b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_06bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_06c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_06c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_06d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_06d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_0703: Unknown result type (might be due to invalid IL or missing references)
			//IL_0712: Expected O, but got Unknown
			//IL_0729: Unknown result type (might be due to invalid IL or missing references)
			//IL_072e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0737: Unknown result type (might be due to invalid IL or missing references)
			//IL_0741: Unknown result type (might be due to invalid IL or missing references)
			//IL_0748: Unknown result type (might be due to invalid IL or missing references)
			//IL_074f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0756: Unknown result type (might be due to invalid IL or missing references)
			//IL_075d: Unknown result type (might be due to invalid IL or missing references)
			//IL_076a: Expected O, but got Unknown
			//IL_076b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0770: Unknown result type (might be due to invalid IL or missing references)
			//IL_0777: Unknown result type (might be due to invalid IL or missing references)
			//IL_077b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0785: Unknown result type (might be due to invalid IL or missing references)
			//IL_078c: Unknown result type (might be due to invalid IL or missing references)
			//IL_07a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_07b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_07c4: Expected O, but got Unknown
			//IL_07dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_07e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_07e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_07ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_07f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_07fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0813: Unknown result type (might be due to invalid IL or missing references)
			//IL_0828: Unknown result type (might be due to invalid IL or missing references)
			//IL_0837: Expected O, but got Unknown
			//IL_084e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0853: Unknown result type (might be due to invalid IL or missing references)
			//IL_085c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0866: Unknown result type (might be due to invalid IL or missing references)
			//IL_086d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0874: Unknown result type (might be due to invalid IL or missing references)
			//IL_087b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0882: Unknown result type (might be due to invalid IL or missing references)
			//IL_088f: Expected O, but got Unknown
			//IL_0890: Unknown result type (might be due to invalid IL or missing references)
			//IL_0895: Unknown result type (might be due to invalid IL or missing references)
			//IL_089c: Unknown result type (might be due to invalid IL or missing references)
			//IL_08a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_08aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_08b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_08c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_08da: Unknown result type (might be due to invalid IL or missing references)
			//IL_08e9: Expected O, but got Unknown
			//IL_0901: Unknown result type (might be due to invalid IL or missing references)
			//IL_0906: Unknown result type (might be due to invalid IL or missing references)
			//IL_090d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0911: Unknown result type (might be due to invalid IL or missing references)
			//IL_091b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0922: Unknown result type (might be due to invalid IL or missing references)
			//IL_0938: Unknown result type (might be due to invalid IL or missing references)
			//IL_094d: Unknown result type (might be due to invalid IL or missing references)
			//IL_095c: Expected O, but got Unknown
			//IL_0973: Unknown result type (might be due to invalid IL or missing references)
			//IL_0978: Unknown result type (might be due to invalid IL or missing references)
			//IL_0981: Unknown result type (might be due to invalid IL or missing references)
			//IL_098b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0992: Unknown result type (might be due to invalid IL or missing references)
			//IL_0999: Unknown result type (might be due to invalid IL or missing references)
			//IL_09a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_09a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_09b4: Expected O, but got Unknown
			//IL_09b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_09ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_09c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_09c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_09cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_09d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_09ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_09ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a0e: Expected O, but got Unknown
			//IL_0a26: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a2b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a32: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a36: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a40: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a47: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a5d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a72: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a81: Expected O, but got Unknown
			//IL_0a98: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a9d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0aa6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ab0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ab7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0abe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ac5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0acc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ad9: Expected O, but got Unknown
			//IL_0ada: Unknown result type (might be due to invalid IL or missing references)
			//IL_0adf: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ae6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0aea: Unknown result type (might be due to invalid IL or missing references)
			//IL_0af4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0afb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b11: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b24: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b33: Expected O, but got Unknown
			//IL_0b4b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b50: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b57: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b5b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b65: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b6c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b82: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b97: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ba6: Expected O, but got Unknown
			//IL_0bbd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bc2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bd3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bdd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0be4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0beb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bf2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bf9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c04: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c11: Expected O, but got Unknown
			//IL_0c12: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c17: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c1e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c22: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c2c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c33: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c49: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c5c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c66: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c76: Expected O, but got Unknown
			//IL_0ca5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0caa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0cb1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0cb4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0cbe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0cc8: Expected O, but got Unknown
			//IL_0ce9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0cee: Unknown result type (might be due to invalid IL or missing references)
			//IL_0cf5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d02: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d0c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d1b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d20: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d2b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d35: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d3d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d44: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d4b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d53: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d60: Expected O, but got Unknown
			//IL_0d61: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d66: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d79: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d83: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d8b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d98: Expected O, but got Unknown
			//IL_0e0a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e0f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e1a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e24: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e2c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e33: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e3a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e42: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e4f: Expected O, but got Unknown
			//IL_0e50: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e55: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e66: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e70: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e7b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e86: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e91: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ea8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0eb5: Expected O, but got Unknown
			//IL_0ecc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ed1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0edc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ee6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0eee: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ef5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0efc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f04: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f11: Expected O, but got Unknown
			//IL_0f12: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f17: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f28: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f32: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f3d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f48: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f53: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f6f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f7c: Expected O, but got Unknown
			//IL_0fab: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fb0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fb7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fc3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fcd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fd7: Expected O, but got Unknown
			//IL_0fe0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fe5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ff0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ffa: Unknown result type (might be due to invalid IL or missing references)
			//IL_1002: Unknown result type (might be due to invalid IL or missing references)
			//IL_1009: Unknown result type (might be due to invalid IL or missing references)
			//IL_1010: Unknown result type (might be due to invalid IL or missing references)
			//IL_1018: Unknown result type (might be due to invalid IL or missing references)
			//IL_1023: Unknown result type (might be due to invalid IL or missing references)
			//IL_1030: Expected O, but got Unknown
			//IL_1031: Unknown result type (might be due to invalid IL or missing references)
			//IL_1036: Unknown result type (might be due to invalid IL or missing references)
			//IL_1047: Unknown result type (might be due to invalid IL or missing references)
			//IL_1051: Unknown result type (might be due to invalid IL or missing references)
			//IL_105c: Unknown result type (might be due to invalid IL or missing references)
			//IL_1067: Unknown result type (might be due to invalid IL or missing references)
			//IL_1072: Unknown result type (might be due to invalid IL or missing references)
			//IL_1089: Unknown result type (might be due to invalid IL or missing references)
			//IL_1091: Unknown result type (might be due to invalid IL or missing references)
			//IL_10a1: Expected O, but got Unknown
			int labelWidth = 60;
			int bindingWidth = 145;
			Image val = new Image(AsyncTexture2D.op_Implicit(Service.Textures!._blishHeart));
			((Control)val).set_Parent(buildPanel);
			((Control)val).set_Width(128);
			((Control)val).set_Height(128);
			((Control)val).set_Location(new Point(((Control)buildPanel).get_Width() - 128, ((Control)buildPanel).get_Height() - 128));
			Panel val2 = new Panel();
			val2.set_CanScroll(false);
			((Control)val2).set_Parent(buildPanel);
			((Container)val2).set_HeightSizingMode((SizingMode)1);
			((Control)val2).set_Width(360);
			((Control)val2).set_Location(new Point(10, 10));
			Panel keysPanel = val2;
			Panel val3 = new Panel();
			val3.set_CanScroll(false);
			((Control)val3).set_Parent(buildPanel);
			((Container)val3).set_HeightSizingMode((SizingMode)1);
			((Control)val3).set_Width(310);
			((Control)val3).set_Location(new Point(((Control)keysPanel).get_Right() + 10, 10));
			Panel manualPanel = val3;
			Label val4 = new Label();
			((Control)val4).set_Location(new Point(labelWidth + 5, 2));
			((Control)val4).set_Width(bindingWidth);
			val4.set_AutoSizeHeight(false);
			val4.set_WrapText(false);
			((Control)val4).set_Parent((Container)(object)keysPanel);
			val4.set_Text("Ground");
			val4.set_HorizontalAlignment((HorizontalAlignment)1);
			Label settingGround_Label = val4;
			Label val5 = new Label();
			((Control)val5).set_Location(new Point(((Control)settingGround_Label).get_Right(), ((Control)settingGround_Label).get_Top()));
			((Control)val5).set_Width(bindingWidth);
			val5.set_AutoSizeHeight(false);
			val5.set_WrapText(false);
			((Control)val5).set_Parent((Container)(object)keysPanel);
			val5.set_Text("Object");
			val5.set_HorizontalAlignment((HorizontalAlignment)1);
			Label val6 = new Label();
			((Control)val6).set_Location(new Point(0, ((Control)settingGround_Label).get_Bottom() + 2));
			((Control)val6).set_Width(labelWidth);
			val6.set_AutoSizeHeight(false);
			val6.set_WrapText(false);
			((Control)val6).set_Parent((Container)(object)keysPanel);
			val6.set_Text("Arrow");
			Label settingArrow_Label = val6;
			KeybindingAssigner val7 = new KeybindingAssigner();
			val7.set_NameWidth(0);
			((Control)val7).set_Size(new Point(bindingWidth, 18));
			((Control)val7).set_Parent((Container)(object)keysPanel);
			val7.set_KeyBinding(_settings._settingArrowGndBinding.get_Value());
			((Control)val7).set_Location(new Point(((Control)settingArrow_Label).get_Right() + 5, ((Control)settingArrow_Label).get_Top() - 1));
			KeybindingAssigner settingArrowGnd_Keybind = val7;
			settingArrowGnd_Keybind.add_BindingChanged((EventHandler<EventArgs>)delegate
			{
				_settings._settingArrowGndBinding.set_Value(settingArrowGnd_Keybind.get_KeyBinding());
			});
			KeybindingAssigner val8 = new KeybindingAssigner();
			val8.set_NameWidth(0);
			((Control)val8).set_Size(new Point(bindingWidth, 18));
			((Control)val8).set_Parent((Container)(object)keysPanel);
			val8.set_KeyBinding(_settings._settingArrowObjBinding.get_Value());
			((Control)val8).set_Location(new Point(((Control)settingArrowGnd_Keybind).get_Right(), ((Control)settingArrow_Label).get_Top() - 1));
			KeybindingAssigner settingArrowObj_Keybind = val8;
			settingArrowGnd_Keybind.add_BindingChanged((EventHandler<EventArgs>)delegate
			{
				_settings._settingArrowObjBinding.set_Value(settingArrowObj_Keybind.get_KeyBinding());
			});
			Label val9 = new Label();
			((Control)val9).set_Location(new Point(0, ((Control)settingArrow_Label).get_Bottom()));
			((Control)val9).set_Width(labelWidth);
			val9.set_AutoSizeHeight(false);
			val9.set_WrapText(false);
			((Control)val9).set_Parent((Container)(object)keysPanel);
			val9.set_Text("Circle");
			Label settingCircle_Label = val9;
			KeybindingAssigner val10 = new KeybindingAssigner();
			val10.set_NameWidth(0);
			((Control)val10).set_Size(new Point(bindingWidth, 18));
			((Control)val10).set_Parent((Container)(object)keysPanel);
			val10.set_KeyBinding(_settings._settingCircleGndBinding.get_Value());
			((Control)val10).set_Location(new Point(((Control)settingCircle_Label).get_Right() + 5, ((Control)settingCircle_Label).get_Top() - 1));
			KeybindingAssigner settingCircleGnd_Keybind = val10;
			settingCircleGnd_Keybind.add_BindingChanged((EventHandler<EventArgs>)delegate
			{
				_settings._settingCircleGndBinding.set_Value(settingCircleGnd_Keybind.get_KeyBinding());
			});
			KeybindingAssigner val11 = new KeybindingAssigner();
			val11.set_NameWidth(0);
			((Control)val11).set_Size(new Point(bindingWidth, 18));
			((Control)val11).set_Parent((Container)(object)keysPanel);
			val11.set_KeyBinding(_settings._settingCircleObjBinding.get_Value());
			((Control)val11).set_Location(new Point(((Control)settingCircleGnd_Keybind).get_Right(), ((Control)settingCircle_Label).get_Top() - 1));
			KeybindingAssigner settingCircleObj_Keybind = val11;
			settingCircleGnd_Keybind.add_BindingChanged((EventHandler<EventArgs>)delegate
			{
				_settings._settingCircleObjBinding.set_Value(settingCircleObj_Keybind.get_KeyBinding());
			});
			Label val12 = new Label();
			((Control)val12).set_Location(new Point(0, ((Control)settingCircle_Label).get_Bottom()));
			((Control)val12).set_Width(labelWidth);
			val12.set_AutoSizeHeight(false);
			val12.set_WrapText(false);
			((Control)val12).set_Parent((Container)(object)keysPanel);
			val12.set_Text("Heart");
			Label settingHeart_Label = val12;
			KeybindingAssigner val13 = new KeybindingAssigner();
			val13.set_NameWidth(0);
			((Control)val13).set_Size(new Point(bindingWidth, 18));
			((Control)val13).set_Parent((Container)(object)keysPanel);
			val13.set_KeyBinding(_settings._settingHeartGndBinding.get_Value());
			((Control)val13).set_Location(new Point(((Control)settingHeart_Label).get_Right() + 5, ((Control)settingHeart_Label).get_Top() - 1));
			KeybindingAssigner settingHeartGnd_Keybind = val13;
			settingHeartGnd_Keybind.add_BindingChanged((EventHandler<EventArgs>)delegate
			{
				_settings._settingHeartGndBinding.set_Value(settingHeartGnd_Keybind.get_KeyBinding());
			});
			KeybindingAssigner val14 = new KeybindingAssigner();
			val14.set_NameWidth(0);
			((Control)val14).set_Size(new Point(bindingWidth, 18));
			((Control)val14).set_Parent((Container)(object)keysPanel);
			val14.set_KeyBinding(_settings._settingHeartObjBinding.get_Value());
			((Control)val14).set_Location(new Point(((Control)settingHeartGnd_Keybind).get_Right(), ((Control)settingHeart_Label).get_Top() - 1));
			KeybindingAssigner settingHeartObj_Keybind = val14;
			settingHeartGnd_Keybind.add_BindingChanged((EventHandler<EventArgs>)delegate
			{
				_settings._settingHeartObjBinding.set_Value(settingHeartObj_Keybind.get_KeyBinding());
			});
			Label val15 = new Label();
			((Control)val15).set_Location(new Point(0, ((Control)settingHeart_Label).get_Bottom()));
			((Control)val15).set_Width(labelWidth);
			val15.set_AutoSizeHeight(false);
			val15.set_WrapText(false);
			((Control)val15).set_Parent((Container)(object)keysPanel);
			val15.set_Text("Square");
			Label settingSquare_Label = val15;
			KeybindingAssigner val16 = new KeybindingAssigner();
			val16.set_NameWidth(0);
			((Control)val16).set_Size(new Point(bindingWidth, 18));
			((Control)val16).set_Parent((Container)(object)keysPanel);
			val16.set_KeyBinding(_settings._settingSquareGndBinding.get_Value());
			((Control)val16).set_Location(new Point(((Control)settingSquare_Label).get_Right() + 5, ((Control)settingSquare_Label).get_Top() - 1));
			KeybindingAssigner settingSquareGnd_Keybind = val16;
			settingSquareGnd_Keybind.add_BindingChanged((EventHandler<EventArgs>)delegate
			{
				_settings._settingSquareGndBinding.set_Value(settingSquareGnd_Keybind.get_KeyBinding());
			});
			KeybindingAssigner val17 = new KeybindingAssigner();
			val17.set_NameWidth(0);
			((Control)val17).set_Size(new Point(bindingWidth, 18));
			((Control)val17).set_Parent((Container)(object)keysPanel);
			val17.set_KeyBinding(_settings._settingSquareObjBinding.get_Value());
			((Control)val17).set_Location(new Point(((Control)settingSquareGnd_Keybind).get_Right(), ((Control)settingSquare_Label).get_Top() - 1));
			KeybindingAssigner settingSquareObj_Keybind = val17;
			settingSquareGnd_Keybind.add_BindingChanged((EventHandler<EventArgs>)delegate
			{
				_settings._settingSquareObjBinding.set_Value(settingSquareObj_Keybind.get_KeyBinding());
			});
			Label val18 = new Label();
			((Control)val18).set_Location(new Point(0, ((Control)settingSquare_Label).get_Bottom()));
			((Control)val18).set_Width(labelWidth);
			val18.set_AutoSizeHeight(false);
			val18.set_WrapText(false);
			((Control)val18).set_Parent((Container)(object)keysPanel);
			val18.set_Text("Star");
			Label settingStar_Label = val18;
			KeybindingAssigner val19 = new KeybindingAssigner();
			val19.set_NameWidth(0);
			((Control)val19).set_Size(new Point(bindingWidth, 18));
			((Control)val19).set_Parent((Container)(object)keysPanel);
			val19.set_KeyBinding(_settings._settingStarGndBinding.get_Value());
			((Control)val19).set_Location(new Point(((Control)settingStar_Label).get_Right() + 5, ((Control)settingStar_Label).get_Top() - 1));
			KeybindingAssigner settingStarGnd_Keybind = val19;
			settingStarGnd_Keybind.add_BindingChanged((EventHandler<EventArgs>)delegate
			{
				_settings._settingStarGndBinding.set_Value(settingStarGnd_Keybind.get_KeyBinding());
			});
			KeybindingAssigner val20 = new KeybindingAssigner();
			val20.set_NameWidth(0);
			((Control)val20).set_Size(new Point(bindingWidth, 18));
			((Control)val20).set_Parent((Container)(object)keysPanel);
			val20.set_KeyBinding(_settings._settingStarObjBinding.get_Value());
			((Control)val20).set_Location(new Point(((Control)settingStarGnd_Keybind).get_Right(), ((Control)settingStar_Label).get_Top() - 1));
			KeybindingAssigner settingStarObj_Keybind = val20;
			settingStarGnd_Keybind.add_BindingChanged((EventHandler<EventArgs>)delegate
			{
				_settings._settingStarObjBinding.set_Value(settingStarObj_Keybind.get_KeyBinding());
			});
			Label val21 = new Label();
			((Control)val21).set_Location(new Point(0, ((Control)settingStar_Label).get_Bottom()));
			((Control)val21).set_Width(labelWidth);
			val21.set_AutoSizeHeight(false);
			val21.set_WrapText(false);
			((Control)val21).set_Parent((Container)(object)keysPanel);
			val21.set_Text("Spiral");
			Label settingSpiral_Label = val21;
			KeybindingAssigner val22 = new KeybindingAssigner();
			val22.set_NameWidth(0);
			((Control)val22).set_Size(new Point(bindingWidth, 18));
			((Control)val22).set_Parent((Container)(object)keysPanel);
			val22.set_KeyBinding(_settings._settingSpiralGndBinding.get_Value());
			((Control)val22).set_Location(new Point(((Control)settingSpiral_Label).get_Right() + 5, ((Control)settingSpiral_Label).get_Top() - 1));
			KeybindingAssigner settingSpiralGnd_Keybind = val22;
			settingSpiralGnd_Keybind.add_BindingChanged((EventHandler<EventArgs>)delegate
			{
				_settings._settingSpiralGndBinding.set_Value(settingSpiralGnd_Keybind.get_KeyBinding());
			});
			KeybindingAssigner val23 = new KeybindingAssigner();
			val23.set_NameWidth(0);
			((Control)val23).set_Size(new Point(bindingWidth, 18));
			((Control)val23).set_Parent((Container)(object)keysPanel);
			val23.set_KeyBinding(_settings._settingSpiralObjBinding.get_Value());
			((Control)val23).set_Location(new Point(((Control)settingSpiralGnd_Keybind).get_Right(), ((Control)settingSpiral_Label).get_Top() - 1));
			KeybindingAssigner settingSpiralObj_Keybind = val23;
			settingSpiralGnd_Keybind.add_BindingChanged((EventHandler<EventArgs>)delegate
			{
				_settings._settingSpiralObjBinding.set_Value(settingSpiralObj_Keybind.get_KeyBinding());
			});
			Label val24 = new Label();
			((Control)val24).set_Location(new Point(0, ((Control)settingSpiral_Label).get_Bottom()));
			((Control)val24).set_Width(labelWidth);
			val24.set_AutoSizeHeight(false);
			val24.set_WrapText(false);
			((Control)val24).set_Parent((Container)(object)keysPanel);
			val24.set_Text("Triangle");
			Label settingTriangle_Label = val24;
			KeybindingAssigner val25 = new KeybindingAssigner();
			val25.set_NameWidth(0);
			((Control)val25).set_Size(new Point(bindingWidth, 18));
			((Control)val25).set_Parent((Container)(object)keysPanel);
			val25.set_KeyBinding(_settings._settingTriangleGndBinding.get_Value());
			((Control)val25).set_Location(new Point(((Control)settingTriangle_Label).get_Right() + 5, ((Control)settingTriangle_Label).get_Top() - 1));
			KeybindingAssigner settingTriangleGnd_Keybind = val25;
			settingTriangleGnd_Keybind.add_BindingChanged((EventHandler<EventArgs>)delegate
			{
				_settings._settingTriangleGndBinding.set_Value(settingTriangleGnd_Keybind.get_KeyBinding());
			});
			KeybindingAssigner val26 = new KeybindingAssigner();
			val26.set_NameWidth(0);
			((Control)val26).set_Size(new Point(bindingWidth, 18));
			((Control)val26).set_Parent((Container)(object)keysPanel);
			val26.set_KeyBinding(_settings._settingTriangleObjBinding.get_Value());
			((Control)val26).set_Location(new Point(((Control)settingTriangleGnd_Keybind).get_Right(), ((Control)settingTriangle_Label).get_Top() - 1));
			KeybindingAssigner settingTriangleObj_Keybind = val26;
			settingTriangleGnd_Keybind.add_BindingChanged((EventHandler<EventArgs>)delegate
			{
				_settings._settingTriangleObjBinding.set_Value(settingTriangleObj_Keybind.get_KeyBinding());
			});
			Label val27 = new Label();
			((Control)val27).set_Location(new Point(0, ((Control)settingTriangle_Label).get_Bottom()));
			((Control)val27).set_Width(labelWidth);
			val27.set_AutoSizeHeight(false);
			val27.set_WrapText(false);
			((Control)val27).set_Parent((Container)(object)keysPanel);
			val27.set_Text("X");
			Label settingX_Label = val27;
			KeybindingAssigner val28 = new KeybindingAssigner();
			val28.set_NameWidth(0);
			((Control)val28).set_Size(new Point(bindingWidth, 18));
			((Control)val28).set_Parent((Container)(object)keysPanel);
			val28.set_KeyBinding(_settings._settingXGndBinding.get_Value());
			((Control)val28).set_Location(new Point(((Control)settingX_Label).get_Right() + 5, ((Control)settingX_Label).get_Top() - 1));
			KeybindingAssigner settingXGnd_Keybind = val28;
			settingXGnd_Keybind.add_BindingChanged((EventHandler<EventArgs>)delegate
			{
				_settings._settingXGndBinding.set_Value(settingXGnd_Keybind.get_KeyBinding());
			});
			KeybindingAssigner val29 = new KeybindingAssigner();
			val29.set_NameWidth(0);
			((Control)val29).set_Size(new Point(bindingWidth, 18));
			((Control)val29).set_Parent((Container)(object)keysPanel);
			val29.set_KeyBinding(_settings._settingXObjBinding.get_Value());
			((Control)val29).set_Location(new Point(((Control)settingXGnd_Keybind).get_Right(), ((Control)settingX_Label).get_Top() - 1));
			KeybindingAssigner settingXObj_Keybind = val29;
			settingXGnd_Keybind.add_BindingChanged((EventHandler<EventArgs>)delegate
			{
				_settings._settingXObjBinding.set_Value(settingXObj_Keybind.get_KeyBinding());
			});
			Label val30 = new Label();
			((Control)val30).set_Location(new Point(0, ((Control)settingX_Label).get_Bottom()));
			((Control)val30).set_Width(labelWidth);
			val30.set_AutoSizeHeight(false);
			val30.set_WrapText(false);
			((Control)val30).set_Parent((Container)(object)keysPanel);
			val30.set_Text("Clear");
			Label settingClear_Label = val30;
			KeybindingAssigner val31 = new KeybindingAssigner();
			val31.set_NameWidth(0);
			((Control)val31).set_Size(new Point(bindingWidth, 18));
			((Control)val31).set_Parent((Container)(object)keysPanel);
			val31.set_KeyBinding(_settings._settingClearGndBinding.get_Value());
			((Control)val31).set_Location(new Point(((Control)settingClear_Label).get_Right() + 5, ((Control)settingClear_Label).get_Top() - 1));
			KeybindingAssigner settingClearGnd_Keybind = val31;
			settingClearGnd_Keybind.add_BindingChanged((EventHandler<EventArgs>)delegate
			{
				_settings._settingClearGndBinding.set_Value(settingClearGnd_Keybind.get_KeyBinding());
			});
			KeybindingAssigner val32 = new KeybindingAssigner();
			val32.set_NameWidth(0);
			((Control)val32).set_Size(new Point(bindingWidth, 18));
			((Control)val32).set_Parent((Container)(object)keysPanel);
			val32.set_KeyBinding(_settings._settingClearObjBinding.get_Value());
			((Control)val32).set_Location(new Point(((Control)settingClearGnd_Keybind).get_Right(), ((Control)settingClear_Label).get_Top() - 1));
			KeybindingAssigner settingClearObj_Keybind = val32;
			settingClearGnd_Keybind.add_BindingChanged((EventHandler<EventArgs>)delegate
			{
				_settings._settingClearObjBinding.set_Value(settingClearObj_Keybind.get_KeyBinding());
			});
			Label val33 = new Label();
			((Control)val33).set_Location(new Point(0, ((Control)settingClear_Label).get_Bottom() + ((Control)settingClear_Label).get_Height()));
			((Control)val33).set_Width(labelWidth);
			val33.set_AutoSizeHeight(false);
			val33.set_WrapText(false);
			((Control)val33).set_Parent((Container)(object)keysPanel);
			val33.set_Text("Interact");
			((Control)val33).set_BasicTooltipText("The In-Game keybind for 'interact' (Default F)");
			Label settingInteract_Label = val33;
			KeybindingAssigner val34 = new KeybindingAssigner();
			val34.set_NameWidth(0);
			((Control)val34).set_Size(new Point(bindingWidth, 18));
			((Control)val34).set_Parent((Container)(object)keysPanel);
			val34.set_KeyBinding(_settings._settingInteractKeyBinding.get_Value());
			((Control)val34).set_Location(new Point(((Control)settingInteract_Label).get_Right() + 5, ((Control)settingInteract_Label).get_Top() - 1));
			((Control)val34).set_BasicTooltipText("The In-Game keybind for 'interact' (Default F)");
			KeybindingAssigner settingInteract_Keybind = val34;
			settingInteract_Keybind.add_BindingChanged((EventHandler<EventArgs>)delegate
			{
				_settings._settingInteractKeyBinding.set_Value(settingInteract_Keybind.get_KeyBinding());
			});
			IView settingMarkersVisibleView = SettingView.FromType((SettingEntry)(object)_settings._settingShowMarkersPanel, ((Control)buildPanel).get_Width());
			ViewContainer val35 = new ViewContainer();
			((Container)val35).set_WidthSizingMode((SizingMode)2);
			((Control)val35).set_Location(new Point(0, 0));
			((Control)val35).set_Parent((Container)(object)manualPanel);
			ViewContainer settingMarkersVisibleContainer = val35;
			settingMarkersVisibleContainer.Show(settingMarkersVisibleView);
			IView settingClickDrag_View = SettingView.FromType((SettingEntry)(object)_settings._settingDrag, ((Control)buildPanel).get_Width());
			ViewContainer val36 = new ViewContainer();
			((Container)val36).set_WidthSizingMode((SizingMode)2);
			((Control)val36).set_Location(new Point(((Control)manualPanel).get_Width() / 2 + 5, 0));
			((Control)val36).set_Parent((Container)(object)manualPanel);
			val36.Show(settingClickDrag_View);
			Label val37 = new Label();
			((Control)val37).set_Location(new Point(0, ((Control)settingMarkersVisibleContainer).get_Bottom() + 6));
			((Control)val37).set_Width(75);
			val37.set_AutoSizeHeight(false);
			val37.set_WrapText(false);
			((Control)val37).set_Parent((Container)(object)manualPanel);
			val37.set_Text("Orientation");
			Label settingOrientation_Label = val37;
			Dropdown val38 = new Dropdown();
			((Control)val38).set_Location(new Point(((Control)settingOrientation_Label).get_Right() + 5, ((Control)settingOrientation_Label).get_Top() - 4));
			((Control)val38).set_Width(100);
			((Control)val38).set_Parent((Container)(object)manualPanel);
			Dropdown settingManualOrientation_Select = val38;
			settingManualOrientation_Select.get_Items().Add(Layout.Horizontal.ToString());
			settingManualOrientation_Select.get_Items().Add(Layout.Vertical.ToString());
			settingManualOrientation_Select.set_SelectedItem(_settings._settingOrientation.get_Value());
			settingManualOrientation_Select.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				_settings._settingOrientation.set_Value(settingManualOrientation_Select.get_SelectedItem());
			});
			Label val39 = new Label();
			((Control)val39).set_Location(new Point(0, ((Control)settingOrientation_Label).get_Bottom() + 6));
			((Control)val39).set_Width(75);
			val39.set_AutoSizeHeight(false);
			val39.set_WrapText(false);
			((Control)val39).set_Parent((Container)(object)manualPanel);
			val39.set_Text("Icon Width: ");
			Label settingWidth_Label = val39;
			TrackBar val40 = new TrackBar();
			((Control)val40).set_Location(new Point(((Control)settingWidth_Label).get_Right() + 5, ((Control)settingWidth_Label).get_Top()));
			((Control)val40).set_Width(220);
			val40.set_MaxValue(200f);
			val40.set_MinValue(16f);
			val40.set_Value((float)_settings._settingImgWidth.get_Value());
			((Control)val40).set_Parent((Container)(object)manualPanel);
			TrackBar settingImgWidth_Slider = val40;
			settingImgWidth_Slider.add_ValueChanged((EventHandler<ValueEventArgs<float>>)delegate
			{
				_settings._settingImgWidth.set_Value((int)settingImgWidth_Slider.get_Value());
			});
			Label val41 = new Label();
			((Control)val41).set_Location(new Point(0, ((Control)settingWidth_Label).get_Bottom() + 6));
			((Control)val41).set_Width(75);
			val41.set_AutoSizeHeight(false);
			val41.set_WrapText(false);
			((Control)val41).set_Parent((Container)(object)manualPanel);
			val41.set_Text("Opacity: ");
			Label settingOpacity_Label = val41;
			TrackBar val42 = new TrackBar();
			((Control)val42).set_Location(new Point(((Control)settingOpacity_Label).get_Right() + 5, ((Control)settingOpacity_Label).get_Top()));
			((Control)val42).set_Width(220);
			val42.set_MaxValue(100f);
			val42.set_MinValue(10f);
			val42.set_Value(_settings._settingOpacity.get_Value() * 100f);
			((Control)val42).set_Parent((Container)(object)manualPanel);
			TrackBar settingOpacity_Slider = val42;
			settingOpacity_Slider.add_ValueChanged((EventHandler<ValueEventArgs<float>>)delegate
			{
				_settings._settingOpacity.set_Value(settingOpacity_Slider.get_Value() / 100f);
			});
			IView settingOnlyComm_View = SettingView.FromType((SettingEntry)(object)_settings._settingOnlyWhenCommander, ((Control)buildPanel).get_Width());
			ViewContainer val43 = new ViewContainer();
			((Container)val43).set_WidthSizingMode((SizingMode)2);
			((Control)val43).set_Location(new Point(0, ((Control)settingOpacity_Label).get_Bottom() + 15));
			((Control)val43).set_Parent((Container)(object)manualPanel);
			ViewContainer settingOnlyComm_Container = val43;
			settingOnlyComm_Container.Show(settingOnlyComm_View);
			Label val44 = new Label();
			((Control)val44).set_Location(new Point(0, ((Control)settingOnlyComm_Container).get_Bottom() + 6));
			((Control)val44).set_Width(75);
			val44.set_AutoSizeHeight(false);
			val44.set_WrapText(false);
			((Control)val44).set_Parent((Container)(object)manualPanel);
			val44.set_Text("Delay: ");
			((Control)val44).set_BasicTooltipText("Delay in milliseconds to wait between marker placement\nFaster <-----> Slower");
			Label settingMarkerDelay_Label = val44;
			TrackBar val45 = new TrackBar();
			((Control)val45).set_Location(new Point(((Control)settingMarkerDelay_Label).get_Right() + 5, ((Control)settingMarkerDelay_Label).get_Top()));
			((Control)val45).set_Width(220);
			val45.set_MaxValue(300f);
			val45.set_MinValue(50f);
			val45.set_Value((float)_settings._settingMarkerPlaceDelay.get_Value());
			((Control)val45).set_Parent((Container)(object)manualPanel);
			((Control)val45).set_BasicTooltipText("Delay in milliseconds to wait between marker placement\nFaster <-----> Slower");
			TrackBar settingMarkerDelay_Slider = val45;
			settingMarkerDelay_Slider.add_ValueChanged((EventHandler<ValueEventArgs<float>>)delegate
			{
				_settings._settingMarkerPlaceDelay.set_Value((int)settingMarkerDelay_Slider.get_Value());
			});
		}

		protected void CreateDoubleKeybind(Container buildPanel, string label, SettingEntry<KeyBinding> groundSetting, SettingEntry<KeyBinding> objectSetting)
		{
		}
	}
}
