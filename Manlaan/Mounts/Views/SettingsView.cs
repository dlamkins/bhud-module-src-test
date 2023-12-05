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
using Microsoft.Xna.Framework.Input;
using Mounts;

namespace Manlaan.Mounts.Views
{
	internal class SettingsView : View
	{
		private const string NoValueSelected = "Please select a value";

		private List<KeybindingAssigner> KeybindingAssigners = new List<KeybindingAssigner>();

		private KeybindingAssigner settingDefaultMount_Keybind;

		private Label labelExplanation;

		private Texture2D anetTexture { get; }

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
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Expected O, but got Unknown
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0128: Unknown result type (might be due to invalid IL or missing references)
			//IL_0155: Unknown result type (might be due to invalid IL or missing references)
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
			val.set_HorizontalAlignment((HorizontalAlignment)0);
			labelExplanation = val;
			UpdateLabelText("");
			StandardButton val2 = new StandardButton();
			((Control)val2).set_Parent(buildPanel);
			((Control)val2).set_Location(new Point(((Control)labelExplanation).get_Right(), ((Control)labelExplanation).get_Top()));
			val2.set_Text(Strings.Documentation_Button_Label);
			((Control)val2).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				Process.Start("https://github.com/bennieboj/BlishHud-Mounts/#settings");
			});
			int panelPadding = 20;
			Panel thingsPanel = CreateDefaultPanel(buildPanel, new Point(panelPadding, ((Control)labelExplanation).get_Bottom() + panelPadding), 600);
			BuildThingsPanel(thingsPanel, labelWidth, bindingWidth, orderWidth);
			Panel generalSettingsPanel = CreateDefaultPanel(buildPanel, new Point(((Control)thingsPanel).get_Right() + 20, ((Control)labelExplanation).get_Bottom() + panelPadding));
			BuildGeneralSettingsPanel(generalSettingsPanel, labelWidth2, mountsAndRadialInputWidth);
			Panel radialPanel = CreateDefaultPanel(buildPanel, new Point(((Control)thingsPanel).get_Right() + 20, 500));
			BuildRadialSettingsPanel((Container)(object)radialPanel, labelWidth2, mountsAndRadialInputWidth);
			ValidateKeybindOverlaps();
		}

		private void UpdateLabelText(string addendum)
		{
			string mystring = "For this module to work you need to fill in your in-game keybindings in the settings below.\nNo keybind means the action is DISABLED. For more info, see the documentation.";
			if (!string.IsNullOrWhiteSpace(addendum))
			{
				mystring = string.Concat(mystring, "\n" + addendum);
			}
			labelExplanation.set_Text(mystring.Replace(" ", "  "));
		}

		private void BuildThingsPanel(Panel mountsPanel, int labelWidth, int bindingWidth, int orderWidth)
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
			//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01df: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0209: Expected O, but got Unknown
			//IL_0221: Unknown result type (might be due to invalid IL or missing references)
			//IL_0226: Unknown result type (might be due to invalid IL or missing references)
			//IL_022d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0231: Unknown result type (might be due to invalid IL or missing references)
			//IL_023b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0242: Unknown result type (might be due to invalid IL or missing references)
			//IL_0255: Unknown result type (might be due to invalid IL or missing references)
			//IL_0264: Expected O, but got Unknown
			//IL_0291: Unknown result type (might be due to invalid IL or missing references)
			//IL_0296: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cf: Expected O, but got Unknown
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
				KeybindingAssigners.Add(settingMount_Keybind);
				settingMount_Keybind.add_BindingChanged((EventHandler<EventArgs>)delegate
				{
					thing.KeybindingSetting.set_Value(settingMount_Keybind.get_KeyBinding());
					ValidateKeybindOverlaps();
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

		private void ValidateKeybindOverlaps()
		{
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			IEnumerable<KeybindingAssigner> kbaIssues = KeybindingAssigners.Where(delegate(KeybindingAssigner k)
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_0019: Unknown result type (might be due to invalid IL or missing references)
				Keys primaryKey = settingDefaultMount_Keybind.get_KeyBinding().get_PrimaryKey();
				return ((object)(Keys)(ref primaryKey)).Equals((object)k.get_KeyBinding().get_PrimaryKey());
			});
			if (kbaIssues.Any())
			{
				foreach (KeybindingAssigner item in kbaIssues)
				{
					((Control)item).set_BackgroundColor(Color.get_Red());
				}
				((Control)settingDefaultMount_Keybind).set_BackgroundColor(Color.get_Red());
				UpdateLabelText("Validation failed: overlapping keybinds are not supported!");
				return;
			}
			foreach (KeybindingAssigner keybindingAssigner in KeybindingAssigners)
			{
				((Control)keybindingAssigner).set_BackgroundColor(Color.get_Transparent());
			}
			((Control)settingDefaultMount_Keybind).set_BackgroundColor(Color.get_Transparent());
			UpdateLabelText("");
		}

		private void BuildGeneralSettingsPanel(Panel defaultMountPanel, int labelWidth2, int mountsAndRadialInputWidth)
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
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Expected O, but got Unknown
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Expected O, but got Unknown
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Expected O, but got Unknown
			//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0206: Unknown result type (might be due to invalid IL or missing references)
			//IL_020d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0214: Unknown result type (might be due to invalid IL or missing references)
			//IL_021b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0226: Unknown result type (might be due to invalid IL or missing references)
			//IL_0233: Expected O, but got Unknown
			//IL_0233: Unknown result type (might be due to invalid IL or missing references)
			//IL_0238: Unknown result type (might be due to invalid IL or missing references)
			//IL_023f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0244: Unknown result type (might be due to invalid IL or missing references)
			//IL_024e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0263: Unknown result type (might be due to invalid IL or missing references)
			//IL_0287: Unknown result type (might be due to invalid IL or missing references)
			//IL_028c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0293: Unknown result type (might be due to invalid IL or missing references)
			//IL_0297: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0309: Expected O, but got Unknown
			//IL_030a: Unknown result type (might be due to invalid IL or missing references)
			//IL_030f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0313: Unknown result type (might be due to invalid IL or missing references)
			//IL_031d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0324: Unknown result type (might be due to invalid IL or missing references)
			//IL_0334: Unknown result type (might be due to invalid IL or missing references)
			//IL_0347: Unknown result type (might be due to invalid IL or missing references)
			//IL_0356: Expected O, but got Unknown
			//IL_036d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0372: Unknown result type (might be due to invalid IL or missing references)
			//IL_037d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0387: Unknown result type (might be due to invalid IL or missing references)
			//IL_038e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0395: Unknown result type (might be due to invalid IL or missing references)
			//IL_039c: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b0: Expected O, but got Unknown
			//IL_03b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_03cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_03db: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_03fd: Expected O, but got Unknown
			//IL_0414: Unknown result type (might be due to invalid IL or missing references)
			//IL_0419: Unknown result type (might be due to invalid IL or missing references)
			//IL_0424: Unknown result type (might be due to invalid IL or missing references)
			//IL_042e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0435: Unknown result type (might be due to invalid IL or missing references)
			//IL_043c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0443: Unknown result type (might be due to invalid IL or missing references)
			//IL_044a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0457: Expected O, but got Unknown
			//IL_0458: Unknown result type (might be due to invalid IL or missing references)
			//IL_045d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0461: Unknown result type (might be due to invalid IL or missing references)
			//IL_046b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0472: Unknown result type (might be due to invalid IL or missing references)
			//IL_0482: Unknown result type (might be due to invalid IL or missing references)
			//IL_0495: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a4: Expected O, but got Unknown
			//IL_04bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_04cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_04dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_04fe: Expected O, but got Unknown
			//IL_04ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0504: Unknown result type (might be due to invalid IL or missing references)
			//IL_0508: Unknown result type (might be due to invalid IL or missing references)
			//IL_0512: Unknown result type (might be due to invalid IL or missing references)
			//IL_0519: Unknown result type (might be due to invalid IL or missing references)
			//IL_0529: Unknown result type (might be due to invalid IL or missing references)
			//IL_053c: Unknown result type (might be due to invalid IL or missing references)
			//IL_054b: Expected O, but got Unknown
			//IL_0562: Unknown result type (might be due to invalid IL or missing references)
			//IL_0567: Unknown result type (might be due to invalid IL or missing references)
			//IL_0572: Unknown result type (might be due to invalid IL or missing references)
			//IL_057c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0583: Unknown result type (might be due to invalid IL or missing references)
			//IL_058a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0591: Unknown result type (might be due to invalid IL or missing references)
			//IL_0598: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a5: Expected O, but got Unknown
			//IL_05a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_05f3: Expected O, but got Unknown
			//IL_060a: Unknown result type (might be due to invalid IL or missing references)
			//IL_060f: Unknown result type (might be due to invalid IL or missing references)
			//IL_061a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0624: Unknown result type (might be due to invalid IL or missing references)
			//IL_062b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0632: Unknown result type (might be due to invalid IL or missing references)
			//IL_0639: Unknown result type (might be due to invalid IL or missing references)
			//IL_0640: Unknown result type (might be due to invalid IL or missing references)
			//IL_064b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0658: Expected O, but got Unknown
			//IL_0659: Unknown result type (might be due to invalid IL or missing references)
			//IL_065e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0663: Unknown result type (might be due to invalid IL or missing references)
			//IL_066d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0674: Unknown result type (might be due to invalid IL or missing references)
			//IL_0684: Unknown result type (might be due to invalid IL or missing references)
			//IL_0697: Unknown result type (might be due to invalid IL or missing references)
			//IL_06a6: Expected O, but got Unknown
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
			settingDefaultMount_Keybind = val2;
			settingDefaultMount_Keybind.add_BindingChanged((EventHandler<EventArgs>)delegate
			{
				ValidateKeybindOverlaps();
			});
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
			val5.set_Text("In-game Jump key binding: ");
			((Control)val5).set_BasicTooltipText("Used to detect gliding better for the IsPlayerGlidingOrFalling radial context.");
			Label settingJumpbinding_Label = val5;
			Image val6 = new Image();
			((Control)val6).set_Parent((Container)(object)defaultMountPanel);
			((Control)val6).set_Size(new Point(16, 16));
			((Control)val6).set_Location(new Point(((Control)settingJumpbinding_Label).get_Right() - 80, ((Control)settingJumpbinding_Label).get_Bottom() - 16));
			val6.set_Texture(AsyncTexture2D.op_Implicit(anetTexture));
			KeybindingAssigner val7 = new KeybindingAssigner(Module._settingJumpBinding.get_Value());
			val7.set_NameWidth(0);
			((Control)val7).set_Size(new Point(mountsAndRadialInputWidth, 20));
			((Control)val7).set_Parent((Container)(object)defaultMountPanel);
			((Control)val7).set_Location(new Point(((Control)settingJumpbinding_Label).get_Right() + 4, ((Control)settingJumpbinding_Label).get_Top() - 1));
			Label val8 = new Label();
			((Control)val8).set_Location(new Point(0, ((Control)settingJumpbinding_Label).get_Bottom() + 6));
			((Control)val8).set_Width(labelWidth2);
			val8.set_AutoSizeHeight(false);
			val8.set_WrapText(false);
			((Control)val8).set_Parent((Container)(object)defaultMountPanel);
			val8.set_Text("Display module on loading screen:");
			Label settingDisplayModuleOnLoadingScreen_Label = val8;
			Checkbox val9 = new Checkbox();
			((Control)val9).set_Size(new Point(labelWidth2, 20));
			((Control)val9).set_Parent((Container)(object)defaultMountPanel);
			val9.set_Checked(Module._settingDisplayModuleOnLoadingScreen.get_Value());
			((Control)val9).set_Location(new Point(((Control)settingDisplayModuleOnLoadingScreen_Label).get_Right() + 5, ((Control)settingDisplayModuleOnLoadingScreen_Label).get_Top() - 1));
			Checkbox settingDisplayModuleOnLoadingScreen_Checkbox = val9;
			settingDisplayModuleOnLoadingScreen_Checkbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				Module._settingDisplayModuleOnLoadingScreen.set_Value(settingDisplayModuleOnLoadingScreen_Checkbox.get_Checked());
			});
			Label val10 = new Label();
			((Control)val10).set_Location(new Point(0, ((Control)settingDisplayModuleOnLoadingScreen_Label).get_Bottom() + 6));
			((Control)val10).set_Width(labelWidth2);
			val10.set_AutoSizeHeight(false);
			val10.set_WrapText(false);
			((Control)val10).set_Parent((Container)(object)defaultMountPanel);
			val10.set_Text("Mount automatically after loading screen:");
			Label settingMountAutomaticallyAfterLoadingScreen_Label = val10;
			Checkbox val11 = new Checkbox();
			((Control)val11).set_Size(new Point(labelWidth2, 20));
			((Control)val11).set_Parent((Container)(object)defaultMountPanel);
			val11.set_Checked(Module._settingMountAutomaticallyAfterLoadingScreen.get_Value());
			((Control)val11).set_Location(new Point(((Control)settingMountAutomaticallyAfterLoadingScreen_Label).get_Right() + 5, ((Control)settingMountAutomaticallyAfterLoadingScreen_Label).get_Top() - 1));
			Checkbox settingMountAutomaticallyAfterLoadingScreen_Checkbox = val11;
			settingMountAutomaticallyAfterLoadingScreen_Checkbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				Module._settingMountAutomaticallyAfterLoadingScreen.set_Value(settingMountAutomaticallyAfterLoadingScreen_Checkbox.get_Checked());
			});
			Label val12 = new Label();
			((Control)val12).set_Location(new Point(0, ((Control)settingMountAutomaticallyAfterLoadingScreen_Label).get_Bottom() + 6));
			((Control)val12).set_Width(labelWidth2);
			val12.set_AutoSizeHeight(false);
			val12.set_WrapText(false);
			((Control)val12).set_Parent((Container)(object)defaultMountPanel);
			val12.set_Text("Enable out of combat queueing:");
			Label settingEnableMountQueueing_Label = val12;
			Checkbox val13 = new Checkbox();
			((Control)val13).set_Size(new Point(labelWidth2, 20));
			((Control)val13).set_Parent((Container)(object)defaultMountPanel);
			val13.set_Checked(Module._settingEnableMountQueueing.get_Value());
			((Control)val13).set_Location(new Point(((Control)settingEnableMountQueueing_Label).get_Right() + 5, ((Control)settingEnableMountQueueing_Label).get_Top() - 1));
			Checkbox settingEnableMountQueueing_Checkbox = val13;
			settingEnableMountQueueing_Checkbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				Module._settingEnableMountQueueing.set_Value(settingEnableMountQueueing_Checkbox.get_Checked());
			});
			Label val14 = new Label();
			((Control)val14).set_Location(new Point(0, ((Control)settingEnableMountQueueing_Label).get_Bottom() + 6));
			((Control)val14).set_Width(labelWidth2);
			val14.set_AutoSizeHeight(false);
			val14.set_WrapText(false);
			((Control)val14).set_Parent((Container)(object)defaultMountPanel);
			val14.set_Text("Display out of combat queueing:");
			Label settingDisplayMountQueueing_Label = val14;
			Checkbox val15 = new Checkbox();
			((Control)val15).set_Size(new Point(labelWidth2, 20));
			((Control)val15).set_Parent((Container)(object)defaultMountPanel);
			val15.set_Checked(Module._settingDisplayMountQueueing.get_Value());
			((Control)val15).set_Location(new Point(((Control)settingDisplayMountQueueing_Label).get_Right() + 5, ((Control)settingDisplayMountQueueing_Label).get_Top() - 1));
			Checkbox settingDisplayMountQueueing_Checkbox = val15;
			settingDisplayMountQueueing_Checkbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				Module._settingDisplayMountQueueing.set_Value(settingDisplayMountQueueing_Checkbox.get_Checked());
			});
			Label val16 = new Label();
			((Control)val16).set_Location(new Point(0, ((Control)settingDisplayMountQueueing_Label).get_Bottom() + 6));
			((Control)val16).set_Width(labelWidth2);
			val16.set_AutoSizeHeight(false);
			val16.set_WrapText(false);
			((Control)val16).set_Parent((Container)(object)defaultMountPanel);
			val16.set_Text("Drag out of combat queueing: ");
			Label dragMountQueueing_Label = val16;
			Checkbox val17 = new Checkbox();
			((Control)val17).set_Size(new Point(20, 20));
			((Control)val17).set_Parent((Container)(object)defaultMountPanel);
			val17.set_Checked(Module._settingDragMountQueueing.get_Value());
			((Control)val17).set_Location(new Point(((Control)dragMountQueueing_Label).get_Right() + 5, ((Control)dragMountQueueing_Label).get_Top() - 1));
			Checkbox dragMountQueueing_Checkbox = val17;
			dragMountQueueing_Checkbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				Module._settingDragMountQueueing.set_Value(dragMountQueueing_Checkbox.get_Checked());
			});
			Label val18 = new Label();
			((Control)val18).set_Location(new Point(0, ((Control)dragMountQueueing_Label).get_Bottom() + 6));
			((Control)val18).set_Width(labelWidth2);
			val18.set_AutoSizeHeight(false);
			val18.set_WrapText(false);
			((Control)val18).set_Parent((Container)(object)defaultMountPanel);
			val18.set_Text("Combat Launch mastery unlocked: ");
			((Control)val18).set_BasicTooltipText("EoD and SotO masteries are not detectable in the API yet, see documentation for more info.");
			Label combatLaunchMasteryUnlocked_Label = val18;
			Checkbox val19 = new Checkbox();
			((Control)val19).set_Size(new Point(20, 20));
			((Control)val19).set_Parent((Container)(object)defaultMountPanel);
			val19.set_Checked(Module._settingCombatLaunchMasteryUnlocked.get_Value());
			((Control)val19).set_Location(new Point(((Control)combatLaunchMasteryUnlocked_Label).get_Right() + 5, ((Control)combatLaunchMasteryUnlocked_Label).get_Top() - 1));
			Checkbox combatLaunchMasteryUnlocked_Checkbox = val19;
			combatLaunchMasteryUnlocked_Checkbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				Module._settingCombatLaunchMasteryUnlocked.set_Value(combatLaunchMasteryUnlocked_Checkbox.get_Checked());
			});
		}

		private void BuildRadialSettingsPanel(Container radialPanel, int labelWidth, int mountsAndRadialInputWidth)
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
