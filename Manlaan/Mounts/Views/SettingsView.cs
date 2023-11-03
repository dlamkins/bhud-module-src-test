using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Manlaan.Mounts.Things;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mounts;

namespace Manlaan.Mounts.Views
{
	internal class SettingsView : View
	{
		private const string NoValueSelected = "Please select a value";

		private Texture2D anetTexture { get; }

		private Panel ManualPanel { get; set; }

		public SettingsView(TextureCache textureCache)
			: this()
		{
			anetTexture = textureCache.GetImgFile(TextureCache.AnetIconTextureName);
		}

		private Panel CreateDefaultPanel(Container buildPanel, Point location, int width = 420)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Expected O, but got Unknown
			Panel val = new Panel();
			val.set_CanScroll(false);
			((Control)val).set_Parent(buildPanel);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Control)val).set_Width(width);
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
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
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
			val.set_Text("For this module to work you need to fill in your in-game keybindings in the settings below.\nNo keybind means the action is DISABLED. For more info, see the documentation.".Replace(" ", "  "));
			val.set_HorizontalAlignment((HorizontalAlignment)0);
			Label labelExplanation = val;
			StandardButton val2 = new StandardButton();
			((Control)val2).set_Parent(buildPanel);
			((Control)val2).set_Location(new Point(((Control)labelExplanation).get_Right(), ((Control)labelExplanation).get_Top()));
			val2.set_Text(Strings.Documentation_Button_Label);
			((Control)val2).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				Process.Start("https://github.com/bennieboj/BlishHud-Mounts/#settings");
			});
			int panelPadding = 20;
			Panel mountsPanel = CreateDefaultPanel(buildPanel, new Point(panelPadding, ((Control)labelExplanation).get_Bottom() + panelPadding), 600);
			BuildMountsPanel(mountsPanel, labelWidth, bindingWidth, orderWidth);
			Panel defaultMountPanel = CreateDefaultPanel(buildPanel, new Point(((Control)mountsPanel).get_Right() + 20, ((Control)labelExplanation).get_Bottom() + panelPadding));
			BuildDefaultMountPanel(defaultMountPanel, labelWidth2, mountsAndRadialInputWidth);
			Panel radialPanel = CreateDefaultPanel(buildPanel, new Point(((Control)mountsPanel).get_Right() + 20, 500));
			BuildRadialPanel((Container)(object)radialPanel, labelWidth2, mountsAndRadialInputWidth);
		}

		private void BuildMountsPanel(Panel mountsPanel, int labelWidth, int bindingWidth, int orderWidth)
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
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Expected O, but got Unknown
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Expected O, but got Unknown
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_014e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0155: Unknown result type (might be due to invalid IL or missing references)
			//IL_015c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0167: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01de: Unknown result type (might be due to invalid IL or missing references)
			//IL_0201: Expected O, but got Unknown
			//IL_0219: Unknown result type (might be due to invalid IL or missing references)
			//IL_021e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0225: Unknown result type (might be due to invalid IL or missing references)
			//IL_0229: Unknown result type (might be due to invalid IL or missing references)
			//IL_0233: Unknown result type (might be due to invalid IL or missing references)
			//IL_023a: Unknown result type (might be due to invalid IL or missing references)
			//IL_024d: Unknown result type (might be due to invalid IL or missing references)
			//IL_025c: Expected O, but got Unknown
			//IL_0277: Unknown result type (might be due to invalid IL or missing references)
			//IL_027c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0294: Unknown result type (might be due to invalid IL or missing references)
			//IL_029e: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b5: Expected O, but got Unknown
			Image val = new Image();
			((Control)val).set_Parent((Container)(object)mountsPanel);
			((Control)val).set_Size(new Point(16, 16));
			((Control)val).set_Location(new Point(5, 2));
			val.set_Texture(AsyncTexture2D.op_Implicit(anetTexture));
			Image anetImage = val;
			Label val2 = new Label();
			((Control)val2).set_Location(new Point(((Control)anetImage).get_Right() + 3, ((Control)anetImage).get_Bottom() - 16));
			((Control)val2).set_Width(300);
			val2.set_AutoSizeHeight(false);
			val2.set_WrapText(false);
			((Control)val2).set_Parent((Container)(object)mountsPanel);
			val2.set_Text("must match in-game key binding");
			val2.set_HorizontalAlignment((HorizontalAlignment)0);
			Label keybindWarning_Label = val2;
			Label val3 = new Label();
			((Control)val3).set_Location(new Point(labelWidth + 5, ((Control)keybindWarning_Label).get_Bottom() + 6));
			((Control)val3).set_Width(bindingWidth);
			val3.set_AutoSizeHeight(false);
			val3.set_WrapText(false);
			((Control)val3).set_Parent((Container)(object)mountsPanel);
			val3.set_Text("In-game key binding");
			val3.set_HorizontalAlignment((HorizontalAlignment)1);
			Label settingBinding_Label = val3;
			Image val4 = new Image();
			((Control)val4).set_Parent((Container)(object)mountsPanel);
			((Control)val4).set_Size(new Point(16, 16));
			((Control)val4).set_Location(new Point(((Control)settingBinding_Label).get_Right() - 20, ((Control)settingBinding_Label).get_Bottom() - 16));
			val4.set_Texture(AsyncTexture2D.op_Implicit(anetTexture));
			Image settingBinding_Image = val4;
			Label val5 = new Label();
			((Control)val5).set_Location(new Point(((Control)settingBinding_Image).get_Right() + 5, ((Control)settingBinding_Label).get_Top()));
			((Control)val5).set_Width(bindingWidth);
			val5.set_AutoSizeHeight(false);
			val5.set_WrapText(false);
			((Control)val5).set_Parent((Container)(object)mountsPanel);
			val5.set_Text("Image");
			val5.set_HorizontalAlignment((HorizontalAlignment)1);
			int curY = ((Control)settingBinding_Label).get_Bottom();
			foreach (Thing thing in Module._things)
			{
				Label val6 = new Label();
				((Control)val6).set_Location(new Point(0, curY + 6));
				((Control)val6).set_Width(labelWidth);
				val6.set_AutoSizeHeight(false);
				val6.set_WrapText(false);
				((Control)val6).set_Parent((Container)(object)mountsPanel);
				val6.set_Text(thing.DisplayName + ": ");
				Label settingMount_Label = val6;
				KeybindingAssigner val7 = new KeybindingAssigner(thing.KeybindingSetting.get_Value());
				val7.set_NameWidth(0);
				((Control)val7).set_Size(new Point(bindingWidth, 20));
				((Control)val7).set_Parent((Container)(object)mountsPanel);
				((Control)val7).set_Location(new Point(((Control)settingMount_Label).get_Right() + 5, ((Control)settingMount_Label).get_Top() - 1));
				KeybindingAssigner settingMount_Keybind = val7;
				settingMount_Keybind.add_BindingChanged((EventHandler<EventArgs>)delegate
				{
					thing.KeybindingSetting.set_Value(settingMount_Keybind.get_KeyBinding());
				});
				Dropdown val8 = new Dropdown();
				((Control)val8).set_Location(new Point(((Control)settingMount_Keybind).get_Right() + 5, ((Control)settingMount_Label).get_Top() - 4));
				((Control)val8).set_Width(200);
				((Control)val8).set_Parent((Container)(object)mountsPanel);
				Dropdown settingMountImageFile_Select = val8;
				settingMountImageFile_Select.get_Items().Add("Please select a value");
				(from mIF in Module._thingImageFiles
					where mIF.Name.Contains(thing.ImageFileName)
					orderby mIF.Name descending
					select mIF).ToList().ForEach(delegate(ThingImageFile mIF)
				{
					settingMountImageFile_Select.get_Items().Add(mIF.Name);
				});
				settingMountImageFile_Select.set_SelectedItem((thing.ImageFileNameSetting.get_Value() == "") ? "Please select a value" : thing.ImageFileNameSetting.get_Value());
				settingMountImageFile_Select.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
				{
					if (settingMountImageFile_Select.get_SelectedItem().Equals("Please select a value"))
					{
						thing.ImageFileNameSetting.set_Value("");
					}
					else
					{
						thing.ImageFileNameSetting.set_Value(settingMountImageFile_Select.get_SelectedItem());
					}
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
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Expected O, but got Unknown
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Expected O, but got Unknown
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Expected O, but got Unknown
			//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01da: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0203: Expected O, but got Unknown
			//IL_0204: Unknown result type (might be due to invalid IL or missing references)
			//IL_0209: Unknown result type (might be due to invalid IL or missing references)
			//IL_020d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0217: Unknown result type (might be due to invalid IL or missing references)
			//IL_021e: Unknown result type (might be due to invalid IL or missing references)
			//IL_022e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0241: Unknown result type (might be due to invalid IL or missing references)
			//IL_0250: Expected O, but got Unknown
			//IL_0267: Unknown result type (might be due to invalid IL or missing references)
			//IL_026c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0277: Unknown result type (might be due to invalid IL or missing references)
			//IL_0281: Unknown result type (might be due to invalid IL or missing references)
			//IL_0288: Unknown result type (might be due to invalid IL or missing references)
			//IL_028f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0296: Unknown result type (might be due to invalid IL or missing references)
			//IL_029d: Unknown result type (might be due to invalid IL or missing references)
			//IL_02aa: Expected O, but got Unknown
			//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02be: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f7: Expected O, but got Unknown
			//IL_030e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0313: Unknown result type (might be due to invalid IL or missing references)
			//IL_031e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0328: Unknown result type (might be due to invalid IL or missing references)
			//IL_032f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0336: Unknown result type (might be due to invalid IL or missing references)
			//IL_033d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0344: Unknown result type (might be due to invalid IL or missing references)
			//IL_0351: Expected O, but got Unknown
			//IL_0352: Unknown result type (might be due to invalid IL or missing references)
			//IL_0357: Unknown result type (might be due to invalid IL or missing references)
			//IL_035b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0365: Unknown result type (might be due to invalid IL or missing references)
			//IL_036c: Unknown result type (might be due to invalid IL or missing references)
			//IL_037c: Unknown result type (might be due to invalid IL or missing references)
			//IL_038f: Unknown result type (might be due to invalid IL or missing references)
			//IL_039e: Expected O, but got Unknown
			//IL_03b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_03eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f8: Expected O, but got Unknown
			//IL_03f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0402: Unknown result type (might be due to invalid IL or missing references)
			//IL_040c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0413: Unknown result type (might be due to invalid IL or missing references)
			//IL_0423: Unknown result type (might be due to invalid IL or missing references)
			//IL_0436: Unknown result type (might be due to invalid IL or missing references)
			//IL_0445: Expected O, but got Unknown
			//IL_045c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0461: Unknown result type (might be due to invalid IL or missing references)
			//IL_046c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0476: Unknown result type (might be due to invalid IL or missing references)
			//IL_047d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0484: Unknown result type (might be due to invalid IL or missing references)
			//IL_048b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0492: Unknown result type (might be due to invalid IL or missing references)
			//IL_049f: Expected O, but got Unknown
			//IL_04a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_04aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_04bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_04cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_04de: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ed: Expected O, but got Unknown
			//IL_0504: Unknown result type (might be due to invalid IL or missing references)
			//IL_0509: Unknown result type (might be due to invalid IL or missing references)
			//IL_0514: Unknown result type (might be due to invalid IL or missing references)
			//IL_051e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0525: Unknown result type (might be due to invalid IL or missing references)
			//IL_052c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0533: Unknown result type (might be due to invalid IL or missing references)
			//IL_053a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0545: Unknown result type (might be due to invalid IL or missing references)
			//IL_0552: Expected O, but got Unknown
			//IL_0553: Unknown result type (might be due to invalid IL or missing references)
			//IL_0558: Unknown result type (might be due to invalid IL or missing references)
			//IL_055d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0567: Unknown result type (might be due to invalid IL or missing references)
			//IL_056e: Unknown result type (might be due to invalid IL or missing references)
			//IL_057e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0591: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a0: Expected O, but got Unknown
			Label val = new Label();
			((Control)val).set_Location(new Point(0, 0));
			((Control)val).set_Width(labelWidth2);
			val.set_AutoSizeHeight(false);
			val.set_WrapText(false);
			((Control)val).set_Parent((Container)(object)defaultMountPanel);
			val.set_Text("Key binding: ");
			Label settingDefaultMountKeybind_Label = val;
			KeybindingAssigner val2 = new KeybindingAssigner(Module._settingDefaultMountBinding.get_Value());
			val2.set_NameWidth(0);
			((Control)val2).set_Size(new Point(mountsAndRadialInputWidth, 20));
			((Control)val2).set_Parent((Container)(object)defaultMountPanel);
			((Control)val2).set_Location(new Point(((Control)settingDefaultMountKeybind_Label).get_Right() + 4, ((Control)settingDefaultMountKeybind_Label).get_Top() - 1));
			KeybindingAssigner settingDefaultMount_Keybind = val2;
			Label val3 = new Label();
			((Control)val3).set_Location(new Point(0, ((Control)settingDefaultMountKeybind_Label).get_Bottom() + 6));
			((Control)val3).set_Width(labelWidth2);
			val3.set_AutoSizeHeight(false);
			val3.set_WrapText(false);
			((Control)val3).set_Parent((Container)(object)defaultMountPanel);
			val3.set_Text("Keybind behaviour: ");
			Label settingKeybindBehaviour_Label = val3;
			Dropdown val4 = new Dropdown();
			((Control)val4).set_Location(new Point(((Control)settingKeybindBehaviour_Label).get_Right() + 5, ((Control)settingKeybindBehaviour_Label).get_Top() - 4));
			((Control)val4).set_Width(((Control)settingDefaultMount_Keybind).get_Width());
			((Control)val4).set_Parent((Container)(object)defaultMountPanel);
			Dropdown settingKeybindBehaviour_Select = val4;
			settingKeybindBehaviour_Select.get_Items().Add("Disabled");
			List<string> keybindBehaviours = Module._keybindBehaviours.ToList();
			foreach (string i in keybindBehaviours)
			{
				settingKeybindBehaviour_Select.get_Items().Add(i.ToString());
			}
			settingKeybindBehaviour_Select.set_SelectedItem(keybindBehaviours.Any((string m) => m == Module._settingKeybindBehaviour.get_Value()) ? Module._settingKeybindBehaviour.get_Value() : "Disabled");
			settingKeybindBehaviour_Select.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				Module._settingKeybindBehaviour.set_Value(settingKeybindBehaviour_Select.get_SelectedItem());
			});
			Label val5 = new Label();
			((Control)val5).set_Location(new Point(0, ((Control)settingKeybindBehaviour_Label).get_Bottom() + 6));
			((Control)val5).set_Width(labelWidth2);
			val5.set_AutoSizeHeight(false);
			val5.set_WrapText(false);
			((Control)val5).set_Parent((Container)(object)defaultMountPanel);
			val5.set_Text("Display module on loading screen:");
			Label settingDisplayModuleOnLoadingScreen_Label = val5;
			Checkbox val6 = new Checkbox();
			((Control)val6).set_Size(new Point(labelWidth2, 20));
			((Control)val6).set_Parent((Container)(object)defaultMountPanel);
			val6.set_Checked(Module._settingDisplayModuleOnLoadingScreen.get_Value());
			((Control)val6).set_Location(new Point(((Control)settingDisplayModuleOnLoadingScreen_Label).get_Right() + 5, ((Control)settingDisplayModuleOnLoadingScreen_Label).get_Top() - 1));
			Checkbox settingDisplayModuleOnLoadingScreen_Checkbox = val6;
			settingDisplayModuleOnLoadingScreen_Checkbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				Module._settingDisplayModuleOnLoadingScreen.set_Value(settingDisplayModuleOnLoadingScreen_Checkbox.get_Checked());
			});
			Label val7 = new Label();
			((Control)val7).set_Location(new Point(0, ((Control)settingDisplayModuleOnLoadingScreen_Label).get_Bottom() + 6));
			((Control)val7).set_Width(labelWidth2);
			val7.set_AutoSizeHeight(false);
			val7.set_WrapText(false);
			((Control)val7).set_Parent((Container)(object)defaultMountPanel);
			val7.set_Text("Mount automatically after loading screen:");
			Label settingMountAutomaticallyAfterLoadingScreen_Label = val7;
			Checkbox val8 = new Checkbox();
			((Control)val8).set_Size(new Point(labelWidth2, 20));
			((Control)val8).set_Parent((Container)(object)defaultMountPanel);
			val8.set_Checked(Module._settingMountAutomaticallyAfterLoadingScreen.get_Value());
			((Control)val8).set_Location(new Point(((Control)settingMountAutomaticallyAfterLoadingScreen_Label).get_Right() + 5, ((Control)settingMountAutomaticallyAfterLoadingScreen_Label).get_Top() - 1));
			Checkbox settingMountAutomaticallyAfterLoadingScreen_Checkbox = val8;
			settingMountAutomaticallyAfterLoadingScreen_Checkbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				Module._settingMountAutomaticallyAfterLoadingScreen.set_Value(settingMountAutomaticallyAfterLoadingScreen_Checkbox.get_Checked());
			});
			Label val9 = new Label();
			((Control)val9).set_Location(new Point(0, ((Control)settingMountAutomaticallyAfterLoadingScreen_Label).get_Bottom() + 6));
			((Control)val9).set_Width(labelWidth2);
			val9.set_AutoSizeHeight(false);
			val9.set_WrapText(false);
			((Control)val9).set_Parent((Container)(object)defaultMountPanel);
			val9.set_Text("Enable out of combat queueing:");
			Label settingEnableMountQueueing_Label = val9;
			Checkbox val10 = new Checkbox();
			((Control)val10).set_Size(new Point(labelWidth2, 20));
			((Control)val10).set_Parent((Container)(object)defaultMountPanel);
			val10.set_Checked(Module._settingEnableMountQueueing.get_Value());
			((Control)val10).set_Location(new Point(((Control)settingEnableMountQueueing_Label).get_Right() + 5, ((Control)settingEnableMountQueueing_Label).get_Top() - 1));
			Checkbox settingEnableMountQueueing_Checkbox = val10;
			settingEnableMountQueueing_Checkbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				Module._settingEnableMountQueueing.set_Value(settingEnableMountQueueing_Checkbox.get_Checked());
			});
			Label val11 = new Label();
			((Control)val11).set_Location(new Point(0, ((Control)settingEnableMountQueueing_Label).get_Bottom() + 6));
			((Control)val11).set_Width(labelWidth2);
			val11.set_AutoSizeHeight(false);
			val11.set_WrapText(false);
			((Control)val11).set_Parent((Container)(object)defaultMountPanel);
			val11.set_Text("Display out of combat queueing:");
			Label settingDisplayMountQueueing_Label = val11;
			Checkbox val12 = new Checkbox();
			((Control)val12).set_Size(new Point(labelWidth2, 20));
			((Control)val12).set_Parent((Container)(object)defaultMountPanel);
			val12.set_Checked(Module._settingDisplayMountQueueing.get_Value());
			((Control)val12).set_Location(new Point(((Control)settingDisplayMountQueueing_Label).get_Right() + 5, ((Control)settingDisplayMountQueueing_Label).get_Top() - 1));
			Checkbox settingDisplayMountQueueing_Checkbox = val12;
			settingDisplayMountQueueing_Checkbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				Module._settingDisplayMountQueueing.set_Value(settingDisplayMountQueueing_Checkbox.get_Checked());
			});
			Label val13 = new Label();
			((Control)val13).set_Location(new Point(0, ((Control)settingDisplayMountQueueing_Label).get_Bottom() + 6));
			((Control)val13).set_Width(labelWidth2);
			val13.set_AutoSizeHeight(false);
			val13.set_WrapText(false);
			((Control)val13).set_Parent((Container)(object)defaultMountPanel);
			val13.set_Text("Drag out of combat queueing: ");
			Label dragMountQueueing_Label = val13;
			Checkbox val14 = new Checkbox();
			((Control)val14).set_Size(new Point(20, 20));
			((Control)val14).set_Parent((Container)(object)defaultMountPanel);
			val14.set_Checked(Module._settingDragMountQueueing.get_Value());
			((Control)val14).set_Location(new Point(((Control)dragMountQueueing_Label).get_Right() + 5, ((Control)dragMountQueueing_Label).get_Top() - 1));
			Checkbox dragMountQueueing_Checkbox = val14;
			dragMountQueueing_Checkbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				Module._settingDragMountQueueing.set_Value(dragMountQueueing_Checkbox.get_Checked());
			});
			Label val15 = new Label();
			((Control)val15).set_Location(new Point(0, ((Control)dragMountQueueing_Label).get_Bottom() + 6));
			((Control)val15).set_Width(labelWidth2);
			val15.set_AutoSizeHeight(false);
			val15.set_WrapText(false);
			((Control)val15).set_Parent((Container)(object)defaultMountPanel);
			val15.set_Text("Combat Launch mastery unlocked: ");
			((Control)val15).set_BasicTooltipText("EoD and SotO masteries are not detectable in the API yet, see documentation for more info.");
			Label combatLaunchMasteryUnlocked_Label = val15;
			Checkbox val16 = new Checkbox();
			((Control)val16).set_Size(new Point(20, 20));
			((Control)val16).set_Parent((Container)(object)defaultMountPanel);
			val16.set_Checked(Module._settingCombatLaunchMasteryUnlocked.get_Value());
			((Control)val16).set_Location(new Point(((Control)combatLaunchMasteryUnlocked_Label).get_Right() + 5, ((Control)combatLaunchMasteryUnlocked_Label).get_Top() - 1));
			Checkbox combatLaunchMasteryUnlocked_Checkbox = val16;
			combatLaunchMasteryUnlocked_Checkbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				Module._settingCombatLaunchMasteryUnlocked.set_Value(combatLaunchMasteryUnlocked_Checkbox.get_Checked());
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
			//IL_0409: Unknown result type (might be due to invalid IL or missing references)
			//IL_040e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0415: Unknown result type (might be due to invalid IL or missing references)
			//IL_041a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0424: Unknown result type (might be due to invalid IL or missing references)
			//IL_0439: Unknown result type (might be due to invalid IL or missing references)
			//IL_045d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0462: Unknown result type (might be due to invalid IL or missing references)
			//IL_0469: Unknown result type (might be due to invalid IL or missing references)
			//IL_046d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0477: Unknown result type (might be due to invalid IL or missing references)
			//IL_047e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0491: Unknown result type (might be due to invalid IL or missing references)
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
			val12.set_Text("In-game action camera key binding: ");
			Label settingMountRadialToggleActionCameraKeyBinding_Label = val12;
			Image val13 = new Image();
			((Control)val13).set_Parent(radialPanel);
			((Control)val13).set_Size(new Point(16, 16));
			((Control)val13).set_Location(new Point(((Control)settingMountRadialToggleActionCameraKeyBinding_Label).get_Right() - 32, ((Control)settingMountRadialToggleActionCameraKeyBinding_Label).get_Bottom() - 16));
			val13.set_Texture(AsyncTexture2D.op_Implicit(anetTexture));
			KeybindingAssigner val14 = new KeybindingAssigner(Module._settingMountRadialToggleActionCameraKeyBinding.get_Value());
			val14.set_NameWidth(0);
			((Control)val14).set_Size(new Point(mountsAndRadialInputWidth, 20));
			((Control)val14).set_Parent(radialPanel);
			((Control)val14).set_Location(new Point(((Control)settingMountRadialToggleActionCameraKeyBinding_Label).get_Right() + 4, ((Control)settingMountRadialToggleActionCameraKeyBinding_Label).get_Top() - 1));
		}
	}
}
