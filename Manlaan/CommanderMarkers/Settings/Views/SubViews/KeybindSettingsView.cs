using System;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Manlaan.CommanderMarkers.Settings.Services;
using Microsoft.Xna.Framework;

namespace Manlaan.CommanderMarkers.Settings.Views.SubViews
{
	public class KeybindSettingsView : View
	{
		protected SettingService? _settings;

		protected override void Build(Container buildPanel)
		{
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Expected O, but got Unknown
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Expected O, but got Unknown
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Expected O, but got Unknown
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0142: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_0169: Unknown result type (might be due to invalid IL or missing references)
			//IL_017c: Unknown result type (might be due to invalid IL or missing references)
			//IL_018b: Expected O, but got Unknown
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01af: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01da: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fe: Expected O, but got Unknown
			//IL_0215: Unknown result type (might be due to invalid IL or missing references)
			//IL_021a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0223: Unknown result type (might be due to invalid IL or missing references)
			//IL_022d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0234: Unknown result type (might be due to invalid IL or missing references)
			//IL_023b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0242: Unknown result type (might be due to invalid IL or missing references)
			//IL_0249: Unknown result type (might be due to invalid IL or missing references)
			//IL_0256: Expected O, but got Unknown
			//IL_0257: Unknown result type (might be due to invalid IL or missing references)
			//IL_025c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0263: Unknown result type (might be due to invalid IL or missing references)
			//IL_0267: Unknown result type (might be due to invalid IL or missing references)
			//IL_0271: Unknown result type (might be due to invalid IL or missing references)
			//IL_0278: Unknown result type (might be due to invalid IL or missing references)
			//IL_028e: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b0: Expected O, but got Unknown
			//IL_02c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0314: Unknown result type (might be due to invalid IL or missing references)
			//IL_0323: Expected O, but got Unknown
			//IL_033a: Unknown result type (might be due to invalid IL or missing references)
			//IL_033f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0348: Unknown result type (might be due to invalid IL or missing references)
			//IL_0352: Unknown result type (might be due to invalid IL or missing references)
			//IL_0359: Unknown result type (might be due to invalid IL or missing references)
			//IL_0360: Unknown result type (might be due to invalid IL or missing references)
			//IL_0367: Unknown result type (might be due to invalid IL or missing references)
			//IL_036e: Unknown result type (might be due to invalid IL or missing references)
			//IL_037b: Expected O, but got Unknown
			//IL_037c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0381: Unknown result type (might be due to invalid IL or missing references)
			//IL_0388: Unknown result type (might be due to invalid IL or missing references)
			//IL_038c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0396: Unknown result type (might be due to invalid IL or missing references)
			//IL_039d: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d5: Expected O, but got Unknown
			//IL_03ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0407: Unknown result type (might be due to invalid IL or missing references)
			//IL_040e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0424: Unknown result type (might be due to invalid IL or missing references)
			//IL_0439: Unknown result type (might be due to invalid IL or missing references)
			//IL_0448: Expected O, but got Unknown
			//IL_045f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0464: Unknown result type (might be due to invalid IL or missing references)
			//IL_046d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0477: Unknown result type (might be due to invalid IL or missing references)
			//IL_047e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0485: Unknown result type (might be due to invalid IL or missing references)
			//IL_048c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0493: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a0: Expected O, but got Unknown
			//IL_04a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_04bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_04eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_04fa: Expected O, but got Unknown
			//IL_0512: Unknown result type (might be due to invalid IL or missing references)
			//IL_0517: Unknown result type (might be due to invalid IL or missing references)
			//IL_051e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0522: Unknown result type (might be due to invalid IL or missing references)
			//IL_052c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0533: Unknown result type (might be due to invalid IL or missing references)
			//IL_0549: Unknown result type (might be due to invalid IL or missing references)
			//IL_055e: Unknown result type (might be due to invalid IL or missing references)
			//IL_056d: Expected O, but got Unknown
			//IL_0584: Unknown result type (might be due to invalid IL or missing references)
			//IL_0589: Unknown result type (might be due to invalid IL or missing references)
			//IL_0592: Unknown result type (might be due to invalid IL or missing references)
			//IL_059c: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_05aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c5: Expected O, but got Unknown
			//IL_05c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_05cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_05fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0610: Unknown result type (might be due to invalid IL or missing references)
			//IL_061f: Expected O, but got Unknown
			//IL_0637: Unknown result type (might be due to invalid IL or missing references)
			//IL_063c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0643: Unknown result type (might be due to invalid IL or missing references)
			//IL_0647: Unknown result type (might be due to invalid IL or missing references)
			//IL_0651: Unknown result type (might be due to invalid IL or missing references)
			//IL_0658: Unknown result type (might be due to invalid IL or missing references)
			//IL_066e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0683: Unknown result type (might be due to invalid IL or missing references)
			//IL_0692: Expected O, but got Unknown
			//IL_06a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_06b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_06c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_06c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_06cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_06d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_06dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ea: Expected O, but got Unknown
			//IL_06eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_06f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_06f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_06fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0705: Unknown result type (might be due to invalid IL or missing references)
			//IL_070c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0722: Unknown result type (might be due to invalid IL or missing references)
			//IL_0735: Unknown result type (might be due to invalid IL or missing references)
			//IL_0744: Expected O, but got Unknown
			//IL_075c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0761: Unknown result type (might be due to invalid IL or missing references)
			//IL_0768: Unknown result type (might be due to invalid IL or missing references)
			//IL_076c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0776: Unknown result type (might be due to invalid IL or missing references)
			//IL_077d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0793: Unknown result type (might be due to invalid IL or missing references)
			//IL_07a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_07b7: Expected O, but got Unknown
			//IL_07ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_07d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_07dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_07e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_07ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_07f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_07fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0802: Unknown result type (might be due to invalid IL or missing references)
			//IL_080f: Expected O, but got Unknown
			//IL_0810: Unknown result type (might be due to invalid IL or missing references)
			//IL_0815: Unknown result type (might be due to invalid IL or missing references)
			//IL_081c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0820: Unknown result type (might be due to invalid IL or missing references)
			//IL_082a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0831: Unknown result type (might be due to invalid IL or missing references)
			//IL_0847: Unknown result type (might be due to invalid IL or missing references)
			//IL_085a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0869: Expected O, but got Unknown
			//IL_0881: Unknown result type (might be due to invalid IL or missing references)
			//IL_0886: Unknown result type (might be due to invalid IL or missing references)
			//IL_088d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0891: Unknown result type (might be due to invalid IL or missing references)
			//IL_089b: Unknown result type (might be due to invalid IL or missing references)
			//IL_08a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_08b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_08cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_08dc: Expected O, but got Unknown
			//IL_08f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_08f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0901: Unknown result type (might be due to invalid IL or missing references)
			//IL_090b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0912: Unknown result type (might be due to invalid IL or missing references)
			//IL_0919: Unknown result type (might be due to invalid IL or missing references)
			//IL_0920: Unknown result type (might be due to invalid IL or missing references)
			//IL_0927: Unknown result type (might be due to invalid IL or missing references)
			//IL_0934: Expected O, but got Unknown
			//IL_0935: Unknown result type (might be due to invalid IL or missing references)
			//IL_093a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0941: Unknown result type (might be due to invalid IL or missing references)
			//IL_0945: Unknown result type (might be due to invalid IL or missing references)
			//IL_094f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0956: Unknown result type (might be due to invalid IL or missing references)
			//IL_096c: Unknown result type (might be due to invalid IL or missing references)
			//IL_097f: Unknown result type (might be due to invalid IL or missing references)
			//IL_098e: Expected O, but got Unknown
			//IL_09a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_09ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_09b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_09b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_09c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_09c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_09dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_09f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a01: Expected O, but got Unknown
			//IL_0a18: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a1d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a26: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a30: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a37: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a3e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a45: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a4c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a59: Expected O, but got Unknown
			//IL_0a5a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a5f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a66: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a6a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a74: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a7b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a91: Unknown result type (might be due to invalid IL or missing references)
			//IL_0aa4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ab3: Expected O, but got Unknown
			//IL_0acb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ad0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ad7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0adb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ae5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0aec: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b02: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b17: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b26: Expected O, but got Unknown
			//IL_0b3d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b42: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b53: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b5d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b64: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b6b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b72: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b79: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b84: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b91: Expected O, but got Unknown
			//IL_0b92: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b97: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b9e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ba2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bac: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bb3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bc9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bdc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0be6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bf6: Expected O, but got Unknown
			_settings = Service.Settings;
			((View<IPresenter>)this).Build(buildPanel);
			int labelWidth = 60;
			int bindingWidth = 145;
			Panel val = new Panel();
			val.set_CanScroll(false);
			((Control)val).set_Parent(buildPanel);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Control)val).set_Width(360);
			((Control)val).set_Location(new Point(10, 10));
			Panel keysPanel = val;
			Label val2 = new Label();
			((Control)val2).set_Location(new Point(labelWidth + 5, 2));
			((Control)val2).set_Width(bindingWidth);
			val2.set_AutoSizeHeight(false);
			val2.set_WrapText(false);
			((Control)val2).set_Parent((Container)(object)keysPanel);
			val2.set_Text("Ground");
			val2.set_HorizontalAlignment((HorizontalAlignment)1);
			Label settingGround_Label = val2;
			Label val3 = new Label();
			((Control)val3).set_Location(new Point(((Control)settingGround_Label).get_Right(), ((Control)settingGround_Label).get_Top()));
			((Control)val3).set_Width(bindingWidth);
			val3.set_AutoSizeHeight(false);
			val3.set_WrapText(false);
			((Control)val3).set_Parent((Container)(object)keysPanel);
			val3.set_Text("Object");
			val3.set_HorizontalAlignment((HorizontalAlignment)1);
			Label val4 = new Label();
			((Control)val4).set_Location(new Point(0, ((Control)settingGround_Label).get_Bottom() + 2));
			((Control)val4).set_Width(labelWidth);
			val4.set_AutoSizeHeight(false);
			val4.set_WrapText(false);
			((Control)val4).set_Parent((Container)(object)keysPanel);
			val4.set_Text("Arrow");
			Label settingArrow_Label = val4;
			KeybindingAssigner val5 = new KeybindingAssigner();
			val5.set_NameWidth(0);
			((Control)val5).set_Size(new Point(bindingWidth, 18));
			((Control)val5).set_Parent((Container)(object)keysPanel);
			val5.set_KeyBinding(_settings!._settingArrowGndBinding.get_Value());
			((Control)val5).set_Location(new Point(((Control)settingArrow_Label).get_Right() + 5, ((Control)settingArrow_Label).get_Top() - 1));
			KeybindingAssigner settingArrowGnd_Keybind = val5;
			settingArrowGnd_Keybind.add_BindingChanged((EventHandler<EventArgs>)delegate
			{
				_settings!._settingArrowGndBinding.set_Value(settingArrowGnd_Keybind.get_KeyBinding());
			});
			KeybindingAssigner val6 = new KeybindingAssigner();
			val6.set_NameWidth(0);
			((Control)val6).set_Size(new Point(bindingWidth, 18));
			((Control)val6).set_Parent((Container)(object)keysPanel);
			val6.set_KeyBinding(_settings!._settingArrowObjBinding.get_Value());
			((Control)val6).set_Location(new Point(((Control)settingArrowGnd_Keybind).get_Right(), ((Control)settingArrow_Label).get_Top() - 1));
			KeybindingAssigner settingArrowObj_Keybind = val6;
			settingArrowGnd_Keybind.add_BindingChanged((EventHandler<EventArgs>)delegate
			{
				_settings!._settingArrowObjBinding.set_Value(settingArrowObj_Keybind.get_KeyBinding());
			});
			Label val7 = new Label();
			((Control)val7).set_Location(new Point(0, ((Control)settingArrow_Label).get_Bottom()));
			((Control)val7).set_Width(labelWidth);
			val7.set_AutoSizeHeight(false);
			val7.set_WrapText(false);
			((Control)val7).set_Parent((Container)(object)keysPanel);
			val7.set_Text("Circle");
			Label settingCircle_Label = val7;
			KeybindingAssigner val8 = new KeybindingAssigner();
			val8.set_NameWidth(0);
			((Control)val8).set_Size(new Point(bindingWidth, 18));
			((Control)val8).set_Parent((Container)(object)keysPanel);
			val8.set_KeyBinding(_settings!._settingCircleGndBinding.get_Value());
			((Control)val8).set_Location(new Point(((Control)settingCircle_Label).get_Right() + 5, ((Control)settingCircle_Label).get_Top() - 1));
			KeybindingAssigner settingCircleGnd_Keybind = val8;
			settingCircleGnd_Keybind.add_BindingChanged((EventHandler<EventArgs>)delegate
			{
				_settings!._settingCircleGndBinding.set_Value(settingCircleGnd_Keybind.get_KeyBinding());
			});
			KeybindingAssigner val9 = new KeybindingAssigner();
			val9.set_NameWidth(0);
			((Control)val9).set_Size(new Point(bindingWidth, 18));
			((Control)val9).set_Parent((Container)(object)keysPanel);
			val9.set_KeyBinding(_settings!._settingCircleObjBinding.get_Value());
			((Control)val9).set_Location(new Point(((Control)settingCircleGnd_Keybind).get_Right(), ((Control)settingCircle_Label).get_Top() - 1));
			KeybindingAssigner settingCircleObj_Keybind = val9;
			settingCircleGnd_Keybind.add_BindingChanged((EventHandler<EventArgs>)delegate
			{
				_settings!._settingCircleObjBinding.set_Value(settingCircleObj_Keybind.get_KeyBinding());
			});
			Label val10 = new Label();
			((Control)val10).set_Location(new Point(0, ((Control)settingCircle_Label).get_Bottom()));
			((Control)val10).set_Width(labelWidth);
			val10.set_AutoSizeHeight(false);
			val10.set_WrapText(false);
			((Control)val10).set_Parent((Container)(object)keysPanel);
			val10.set_Text("Heart");
			Label settingHeart_Label = val10;
			KeybindingAssigner val11 = new KeybindingAssigner();
			val11.set_NameWidth(0);
			((Control)val11).set_Size(new Point(bindingWidth, 18));
			((Control)val11).set_Parent((Container)(object)keysPanel);
			val11.set_KeyBinding(_settings!._settingHeartGndBinding.get_Value());
			((Control)val11).set_Location(new Point(((Control)settingHeart_Label).get_Right() + 5, ((Control)settingHeart_Label).get_Top() - 1));
			KeybindingAssigner settingHeartGnd_Keybind = val11;
			settingHeartGnd_Keybind.add_BindingChanged((EventHandler<EventArgs>)delegate
			{
				_settings!._settingHeartGndBinding.set_Value(settingHeartGnd_Keybind.get_KeyBinding());
			});
			KeybindingAssigner val12 = new KeybindingAssigner();
			val12.set_NameWidth(0);
			((Control)val12).set_Size(new Point(bindingWidth, 18));
			((Control)val12).set_Parent((Container)(object)keysPanel);
			val12.set_KeyBinding(_settings!._settingHeartObjBinding.get_Value());
			((Control)val12).set_Location(new Point(((Control)settingHeartGnd_Keybind).get_Right(), ((Control)settingHeart_Label).get_Top() - 1));
			KeybindingAssigner settingHeartObj_Keybind = val12;
			settingHeartGnd_Keybind.add_BindingChanged((EventHandler<EventArgs>)delegate
			{
				_settings!._settingHeartObjBinding.set_Value(settingHeartObj_Keybind.get_KeyBinding());
			});
			Label val13 = new Label();
			((Control)val13).set_Location(new Point(0, ((Control)settingHeart_Label).get_Bottom()));
			((Control)val13).set_Width(labelWidth);
			val13.set_AutoSizeHeight(false);
			val13.set_WrapText(false);
			((Control)val13).set_Parent((Container)(object)keysPanel);
			val13.set_Text("Square");
			Label settingSquare_Label = val13;
			KeybindingAssigner val14 = new KeybindingAssigner();
			val14.set_NameWidth(0);
			((Control)val14).set_Size(new Point(bindingWidth, 18));
			((Control)val14).set_Parent((Container)(object)keysPanel);
			val14.set_KeyBinding(_settings!._settingSquareGndBinding.get_Value());
			((Control)val14).set_Location(new Point(((Control)settingSquare_Label).get_Right() + 5, ((Control)settingSquare_Label).get_Top() - 1));
			KeybindingAssigner settingSquareGnd_Keybind = val14;
			settingSquareGnd_Keybind.add_BindingChanged((EventHandler<EventArgs>)delegate
			{
				_settings!._settingSquareGndBinding.set_Value(settingSquareGnd_Keybind.get_KeyBinding());
			});
			KeybindingAssigner val15 = new KeybindingAssigner();
			val15.set_NameWidth(0);
			((Control)val15).set_Size(new Point(bindingWidth, 18));
			((Control)val15).set_Parent((Container)(object)keysPanel);
			val15.set_KeyBinding(_settings!._settingSquareObjBinding.get_Value());
			((Control)val15).set_Location(new Point(((Control)settingSquareGnd_Keybind).get_Right(), ((Control)settingSquare_Label).get_Top() - 1));
			KeybindingAssigner settingSquareObj_Keybind = val15;
			settingSquareGnd_Keybind.add_BindingChanged((EventHandler<EventArgs>)delegate
			{
				_settings!._settingSquareObjBinding.set_Value(settingSquareObj_Keybind.get_KeyBinding());
			});
			Label val16 = new Label();
			((Control)val16).set_Location(new Point(0, ((Control)settingSquare_Label).get_Bottom()));
			((Control)val16).set_Width(labelWidth);
			val16.set_AutoSizeHeight(false);
			val16.set_WrapText(false);
			((Control)val16).set_Parent((Container)(object)keysPanel);
			val16.set_Text("Star");
			Label settingStar_Label = val16;
			KeybindingAssigner val17 = new KeybindingAssigner();
			val17.set_NameWidth(0);
			((Control)val17).set_Size(new Point(bindingWidth, 18));
			((Control)val17).set_Parent((Container)(object)keysPanel);
			val17.set_KeyBinding(_settings!._settingStarGndBinding.get_Value());
			((Control)val17).set_Location(new Point(((Control)settingStar_Label).get_Right() + 5, ((Control)settingStar_Label).get_Top() - 1));
			KeybindingAssigner settingStarGnd_Keybind = val17;
			settingStarGnd_Keybind.add_BindingChanged((EventHandler<EventArgs>)delegate
			{
				_settings!._settingStarGndBinding.set_Value(settingStarGnd_Keybind.get_KeyBinding());
			});
			KeybindingAssigner val18 = new KeybindingAssigner();
			val18.set_NameWidth(0);
			((Control)val18).set_Size(new Point(bindingWidth, 18));
			((Control)val18).set_Parent((Container)(object)keysPanel);
			val18.set_KeyBinding(_settings!._settingStarObjBinding.get_Value());
			((Control)val18).set_Location(new Point(((Control)settingStarGnd_Keybind).get_Right(), ((Control)settingStar_Label).get_Top() - 1));
			KeybindingAssigner settingStarObj_Keybind = val18;
			settingStarGnd_Keybind.add_BindingChanged((EventHandler<EventArgs>)delegate
			{
				_settings!._settingStarObjBinding.set_Value(settingStarObj_Keybind.get_KeyBinding());
			});
			Label val19 = new Label();
			((Control)val19).set_Location(new Point(0, ((Control)settingStar_Label).get_Bottom()));
			((Control)val19).set_Width(labelWidth);
			val19.set_AutoSizeHeight(false);
			val19.set_WrapText(false);
			((Control)val19).set_Parent((Container)(object)keysPanel);
			val19.set_Text("Spiral");
			Label settingSpiral_Label = val19;
			KeybindingAssigner val20 = new KeybindingAssigner();
			val20.set_NameWidth(0);
			((Control)val20).set_Size(new Point(bindingWidth, 18));
			((Control)val20).set_Parent((Container)(object)keysPanel);
			val20.set_KeyBinding(_settings!._settingSpiralGndBinding.get_Value());
			((Control)val20).set_Location(new Point(((Control)settingSpiral_Label).get_Right() + 5, ((Control)settingSpiral_Label).get_Top() - 1));
			KeybindingAssigner settingSpiralGnd_Keybind = val20;
			settingSpiralGnd_Keybind.add_BindingChanged((EventHandler<EventArgs>)delegate
			{
				_settings!._settingSpiralGndBinding.set_Value(settingSpiralGnd_Keybind.get_KeyBinding());
			});
			KeybindingAssigner val21 = new KeybindingAssigner();
			val21.set_NameWidth(0);
			((Control)val21).set_Size(new Point(bindingWidth, 18));
			((Control)val21).set_Parent((Container)(object)keysPanel);
			val21.set_KeyBinding(_settings!._settingSpiralObjBinding.get_Value());
			((Control)val21).set_Location(new Point(((Control)settingSpiralGnd_Keybind).get_Right(), ((Control)settingSpiral_Label).get_Top() - 1));
			KeybindingAssigner settingSpiralObj_Keybind = val21;
			settingSpiralGnd_Keybind.add_BindingChanged((EventHandler<EventArgs>)delegate
			{
				_settings!._settingSpiralObjBinding.set_Value(settingSpiralObj_Keybind.get_KeyBinding());
			});
			Label val22 = new Label();
			((Control)val22).set_Location(new Point(0, ((Control)settingSpiral_Label).get_Bottom()));
			((Control)val22).set_Width(labelWidth);
			val22.set_AutoSizeHeight(false);
			val22.set_WrapText(false);
			((Control)val22).set_Parent((Container)(object)keysPanel);
			val22.set_Text("Triangle");
			Label settingTriangle_Label = val22;
			KeybindingAssigner val23 = new KeybindingAssigner();
			val23.set_NameWidth(0);
			((Control)val23).set_Size(new Point(bindingWidth, 18));
			((Control)val23).set_Parent((Container)(object)keysPanel);
			val23.set_KeyBinding(_settings!._settingTriangleGndBinding.get_Value());
			((Control)val23).set_Location(new Point(((Control)settingTriangle_Label).get_Right() + 5, ((Control)settingTriangle_Label).get_Top() - 1));
			KeybindingAssigner settingTriangleGnd_Keybind = val23;
			settingTriangleGnd_Keybind.add_BindingChanged((EventHandler<EventArgs>)delegate
			{
				_settings!._settingTriangleGndBinding.set_Value(settingTriangleGnd_Keybind.get_KeyBinding());
			});
			KeybindingAssigner val24 = new KeybindingAssigner();
			val24.set_NameWidth(0);
			((Control)val24).set_Size(new Point(bindingWidth, 18));
			((Control)val24).set_Parent((Container)(object)keysPanel);
			val24.set_KeyBinding(_settings!._settingTriangleObjBinding.get_Value());
			((Control)val24).set_Location(new Point(((Control)settingTriangleGnd_Keybind).get_Right(), ((Control)settingTriangle_Label).get_Top() - 1));
			KeybindingAssigner settingTriangleObj_Keybind = val24;
			settingTriangleGnd_Keybind.add_BindingChanged((EventHandler<EventArgs>)delegate
			{
				_settings!._settingTriangleObjBinding.set_Value(settingTriangleObj_Keybind.get_KeyBinding());
			});
			Label val25 = new Label();
			((Control)val25).set_Location(new Point(0, ((Control)settingTriangle_Label).get_Bottom()));
			((Control)val25).set_Width(labelWidth);
			val25.set_AutoSizeHeight(false);
			val25.set_WrapText(false);
			((Control)val25).set_Parent((Container)(object)keysPanel);
			val25.set_Text("X");
			Label settingX_Label = val25;
			KeybindingAssigner val26 = new KeybindingAssigner();
			val26.set_NameWidth(0);
			((Control)val26).set_Size(new Point(bindingWidth, 18));
			((Control)val26).set_Parent((Container)(object)keysPanel);
			val26.set_KeyBinding(_settings!._settingXGndBinding.get_Value());
			((Control)val26).set_Location(new Point(((Control)settingX_Label).get_Right() + 5, ((Control)settingX_Label).get_Top() - 1));
			KeybindingAssigner settingXGnd_Keybind = val26;
			settingXGnd_Keybind.add_BindingChanged((EventHandler<EventArgs>)delegate
			{
				_settings!._settingXGndBinding.set_Value(settingXGnd_Keybind.get_KeyBinding());
			});
			KeybindingAssigner val27 = new KeybindingAssigner();
			val27.set_NameWidth(0);
			((Control)val27).set_Size(new Point(bindingWidth, 18));
			((Control)val27).set_Parent((Container)(object)keysPanel);
			val27.set_KeyBinding(_settings!._settingXObjBinding.get_Value());
			((Control)val27).set_Location(new Point(((Control)settingXGnd_Keybind).get_Right(), ((Control)settingX_Label).get_Top() - 1));
			KeybindingAssigner settingXObj_Keybind = val27;
			settingXGnd_Keybind.add_BindingChanged((EventHandler<EventArgs>)delegate
			{
				_settings!._settingXObjBinding.set_Value(settingXObj_Keybind.get_KeyBinding());
			});
			Label val28 = new Label();
			((Control)val28).set_Location(new Point(0, ((Control)settingX_Label).get_Bottom()));
			((Control)val28).set_Width(labelWidth);
			val28.set_AutoSizeHeight(false);
			val28.set_WrapText(false);
			((Control)val28).set_Parent((Container)(object)keysPanel);
			val28.set_Text("Clear");
			Label settingClear_Label = val28;
			KeybindingAssigner val29 = new KeybindingAssigner();
			val29.set_NameWidth(0);
			((Control)val29).set_Size(new Point(bindingWidth, 18));
			((Control)val29).set_Parent((Container)(object)keysPanel);
			val29.set_KeyBinding(_settings!._settingClearGndBinding.get_Value());
			((Control)val29).set_Location(new Point(((Control)settingClear_Label).get_Right() + 5, ((Control)settingClear_Label).get_Top() - 1));
			KeybindingAssigner settingClearGnd_Keybind = val29;
			settingClearGnd_Keybind.add_BindingChanged((EventHandler<EventArgs>)delegate
			{
				_settings!._settingClearGndBinding.set_Value(settingClearGnd_Keybind.get_KeyBinding());
			});
			KeybindingAssigner val30 = new KeybindingAssigner();
			val30.set_NameWidth(0);
			((Control)val30).set_Size(new Point(bindingWidth, 18));
			((Control)val30).set_Parent((Container)(object)keysPanel);
			val30.set_KeyBinding(_settings!._settingClearObjBinding.get_Value());
			((Control)val30).set_Location(new Point(((Control)settingClearGnd_Keybind).get_Right(), ((Control)settingClear_Label).get_Top() - 1));
			KeybindingAssigner settingClearObj_Keybind = val30;
			settingClearGnd_Keybind.add_BindingChanged((EventHandler<EventArgs>)delegate
			{
				_settings!._settingClearObjBinding.set_Value(settingClearObj_Keybind.get_KeyBinding());
			});
			Label val31 = new Label();
			((Control)val31).set_Location(new Point(0, ((Control)settingClear_Label).get_Bottom() + ((Control)settingClear_Label).get_Height()));
			((Control)val31).set_Width(labelWidth);
			val31.set_AutoSizeHeight(false);
			val31.set_WrapText(false);
			((Control)val31).set_Parent((Container)(object)keysPanel);
			val31.set_Text("Interact");
			((Control)val31).set_BasicTooltipText("The In-Game keybind for 'interact' (Default F)");
			Label settingInteract_Label = val31;
			KeybindingAssigner val32 = new KeybindingAssigner();
			val32.set_NameWidth(0);
			((Control)val32).set_Size(new Point(bindingWidth, 18));
			((Control)val32).set_Parent((Container)(object)keysPanel);
			val32.set_KeyBinding(_settings!._settingInteractKeyBinding.get_Value());
			((Control)val32).set_Location(new Point(((Control)settingInteract_Label).get_Right() + 5, ((Control)settingInteract_Label).get_Top() - 1));
			((Control)val32).set_BasicTooltipText("The In-Game keybind for 'interact' (Default F)");
			KeybindingAssigner settingInteract_Keybind = val32;
			settingInteract_Keybind.add_BindingChanged((EventHandler<EventArgs>)delegate
			{
				_settings!._settingInteractKeyBinding.set_Value(settingInteract_Keybind.get_KeyBinding());
			});
		}

		public KeybindSettingsView()
			: this()
		{
		}
	}
}
