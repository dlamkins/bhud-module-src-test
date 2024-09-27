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
using Mounts.Settings;

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
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Expected O, but got Unknown
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0158: Unknown result type (might be due to invalid IL or missing references)
			int labelWidth = 150;
			int labelWidth2 = 250;
			int orderWidth = 80;
			int bindingWidth = 170;
			int mountsAndRadialInputWidth = 170;
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
			Panel generalSettingsPanel = CreateDefaultPanel(buildPanel, new Point(((Control)thingsPanel).get_Right() + 20, ((Control)labelExplanation).get_Bottom() + panelPadding), 600);
			BuildGeneralSettingsPanel(generalSettingsPanel, labelWidth2, mountsAndRadialInputWidth);
			Panel radialPanel = CreateDefaultPanel(buildPanel, new Point(((Control)thingsPanel).get_Right() + 20, 500), 600);
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
			val2.set_Text("must match in-game key settings");
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
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			IEnumerable<KeybindingAssigner> kbaIssues = KeybindingAssigners.Where((KeybindingAssigner k) => settingDefaultMount_Keybind.get_KeyBinding().EqualsKeyBinding(k.get_KeyBinding()) && (int)k.get_KeyBinding().get_PrimaryKey() > 0);
			if (kbaIssues.Any() && (int)settingDefaultMount_Keybind.get_KeyBinding().get_PrimaryKey() != 0)
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

		private void BuildGeneralSettingsPanel(Panel defaultMountPanel, int labelWidth2, int optionWidth)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Expected O, but got Unknown
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Expected O, but got Unknown
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Expected O, but got Unknown
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_013c: Expected O, but got Unknown
			//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0208: Unknown result type (might be due to invalid IL or missing references)
			//IL_0212: Unknown result type (might be due to invalid IL or missing references)
			//IL_0219: Unknown result type (might be due to invalid IL or missing references)
			//IL_0220: Unknown result type (might be due to invalid IL or missing references)
			//IL_0227: Unknown result type (might be due to invalid IL or missing references)
			//IL_022e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0239: Unknown result type (might be due to invalid IL or missing references)
			//IL_0246: Expected O, but got Unknown
			//IL_0247: Unknown result type (might be due to invalid IL or missing references)
			//IL_024c: Unknown result type (might be due to invalid IL or missing references)
			//IL_025d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0267: Unknown result type (might be due to invalid IL or missing references)
			//IL_026e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0279: Unknown result type (might be due to invalid IL or missing references)
			//IL_0284: Unknown result type (might be due to invalid IL or missing references)
			//IL_0295: Unknown result type (might be due to invalid IL or missing references)
			//IL_029c: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c0: Expected O, but got Unknown
			//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0306: Unknown result type (might be due to invalid IL or missing references)
			//IL_030d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0318: Unknown result type (might be due to invalid IL or missing references)
			//IL_0325: Expected O, but got Unknown
			//IL_0325: Unknown result type (might be due to invalid IL or missing references)
			//IL_032a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0331: Unknown result type (might be due to invalid IL or missing references)
			//IL_0336: Unknown result type (might be due to invalid IL or missing references)
			//IL_0340: Unknown result type (might be due to invalid IL or missing references)
			//IL_0355: Unknown result type (might be due to invalid IL or missing references)
			//IL_0379: Unknown result type (might be due to invalid IL or missing references)
			//IL_037e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0385: Unknown result type (might be due to invalid IL or missing references)
			//IL_0389: Unknown result type (might be due to invalid IL or missing references)
			//IL_0393: Unknown result type (might be due to invalid IL or missing references)
			//IL_039a: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0406: Expected O, but got Unknown
			//IL_0407: Unknown result type (might be due to invalid IL or missing references)
			//IL_040c: Unknown result type (might be due to invalid IL or missing references)
			//IL_041d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0427: Unknown result type (might be due to invalid IL or missing references)
			//IL_042e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0439: Unknown result type (might be due to invalid IL or missing references)
			//IL_0440: Unknown result type (might be due to invalid IL or missing references)
			//IL_044b: Unknown result type (might be due to invalid IL or missing references)
			//IL_045b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0462: Unknown result type (might be due to invalid IL or missing references)
			//IL_0486: Expected O, but got Unknown
			//IL_049d: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_04be: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_04cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_04de: Unknown result type (might be due to invalid IL or missing references)
			//IL_04eb: Expected O, but got Unknown
			//IL_04ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0506: Unknown result type (might be due to invalid IL or missing references)
			//IL_0516: Unknown result type (might be due to invalid IL or missing references)
			//IL_0529: Unknown result type (might be due to invalid IL or missing references)
			//IL_0538: Expected O, but got Unknown
			//IL_054f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0554: Unknown result type (might be due to invalid IL or missing references)
			//IL_055f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0569: Unknown result type (might be due to invalid IL or missing references)
			//IL_0570: Unknown result type (might be due to invalid IL or missing references)
			//IL_0577: Unknown result type (might be due to invalid IL or missing references)
			//IL_057e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0585: Unknown result type (might be due to invalid IL or missing references)
			//IL_0590: Unknown result type (might be due to invalid IL or missing references)
			//IL_059d: Expected O, but got Unknown
			//IL_059e: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_05db: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ea: Expected O, but got Unknown
			//IL_0601: Unknown result type (might be due to invalid IL or missing references)
			//IL_0606: Unknown result type (might be due to invalid IL or missing references)
			//IL_0611: Unknown result type (might be due to invalid IL or missing references)
			//IL_061b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0622: Unknown result type (might be due to invalid IL or missing references)
			//IL_0629: Unknown result type (might be due to invalid IL or missing references)
			//IL_0630: Unknown result type (might be due to invalid IL or missing references)
			//IL_0637: Unknown result type (might be due to invalid IL or missing references)
			//IL_0642: Unknown result type (might be due to invalid IL or missing references)
			//IL_064f: Expected O, but got Unknown
			//IL_0650: Unknown result type (might be due to invalid IL or missing references)
			//IL_0655: Unknown result type (might be due to invalid IL or missing references)
			//IL_0659: Unknown result type (might be due to invalid IL or missing references)
			//IL_0663: Unknown result type (might be due to invalid IL or missing references)
			//IL_066a: Unknown result type (might be due to invalid IL or missing references)
			//IL_067a: Unknown result type (might be due to invalid IL or missing references)
			//IL_068d: Unknown result type (might be due to invalid IL or missing references)
			//IL_069c: Expected O, but got Unknown
			//IL_06b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_06b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_06c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_06cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_06d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_06db: Unknown result type (might be due to invalid IL or missing references)
			//IL_06e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_06e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_06f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0701: Expected O, but got Unknown
			//IL_0702: Unknown result type (might be due to invalid IL or missing references)
			//IL_0707: Unknown result type (might be due to invalid IL or missing references)
			//IL_070c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0716: Unknown result type (might be due to invalid IL or missing references)
			//IL_071d: Unknown result type (might be due to invalid IL or missing references)
			//IL_072d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0740: Unknown result type (might be due to invalid IL or missing references)
			//IL_074f: Expected O, but got Unknown
			//IL_0766: Unknown result type (might be due to invalid IL or missing references)
			//IL_076b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0776: Unknown result type (might be due to invalid IL or missing references)
			//IL_0780: Unknown result type (might be due to invalid IL or missing references)
			//IL_0787: Unknown result type (might be due to invalid IL or missing references)
			//IL_078e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0795: Unknown result type (might be due to invalid IL or missing references)
			//IL_079c: Unknown result type (might be due to invalid IL or missing references)
			//IL_07a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_07b4: Expected O, but got Unknown
			//IL_07b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_07ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_07be: Unknown result type (might be due to invalid IL or missing references)
			//IL_07c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_07cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_07df: Unknown result type (might be due to invalid IL or missing references)
			//IL_07f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0801: Expected O, but got Unknown
			//IL_0818: Unknown result type (might be due to invalid IL or missing references)
			//IL_081d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0828: Unknown result type (might be due to invalid IL or missing references)
			//IL_0832: Unknown result type (might be due to invalid IL or missing references)
			//IL_0839: Unknown result type (might be due to invalid IL or missing references)
			//IL_0840: Unknown result type (might be due to invalid IL or missing references)
			//IL_0847: Unknown result type (might be due to invalid IL or missing references)
			//IL_084e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0859: Unknown result type (might be due to invalid IL or missing references)
			//IL_0866: Expected O, but got Unknown
			//IL_0867: Unknown result type (might be due to invalid IL or missing references)
			//IL_086c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0871: Unknown result type (might be due to invalid IL or missing references)
			//IL_087b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0882: Unknown result type (might be due to invalid IL or missing references)
			//IL_0892: Unknown result type (might be due to invalid IL or missing references)
			//IL_08a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_08b4: Expected O, but got Unknown
			//IL_08cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_08d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_08db: Unknown result type (might be due to invalid IL or missing references)
			//IL_08e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_08ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_08f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_08fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0901: Unknown result type (might be due to invalid IL or missing references)
			//IL_090c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0919: Expected O, but got Unknown
			//IL_091a: Unknown result type (might be due to invalid IL or missing references)
			//IL_091f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0923: Unknown result type (might be due to invalid IL or missing references)
			//IL_092d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0934: Unknown result type (might be due to invalid IL or missing references)
			//IL_0944: Unknown result type (might be due to invalid IL or missing references)
			//IL_0957: Unknown result type (might be due to invalid IL or missing references)
			//IL_0966: Expected O, but got Unknown
			//IL_097d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0982: Unknown result type (might be due to invalid IL or missing references)
			//IL_098d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0997: Unknown result type (might be due to invalid IL or missing references)
			//IL_099e: Unknown result type (might be due to invalid IL or missing references)
			//IL_09a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_09ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_09b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_09be: Unknown result type (might be due to invalid IL or missing references)
			//IL_09cb: Expected O, but got Unknown
			//IL_09cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_09d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_09d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_09df: Unknown result type (might be due to invalid IL or missing references)
			//IL_09e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_09f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a09: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a18: Expected O, but got Unknown
			//IL_0a2f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a34: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a3f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a49: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a50: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a57: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a5e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a65: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a70: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a7d: Expected O, but got Unknown
			//IL_0a7e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a83: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a87: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a91: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a98: Unknown result type (might be due to invalid IL or missing references)
			//IL_0aa8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0abb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0aca: Expected O, but got Unknown
			//IL_0ae1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ae6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0af1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0afb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b02: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b09: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b10: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b17: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b22: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b2f: Expected O, but got Unknown
			//IL_0b2f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b34: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b3b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b40: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b4a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b62: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b7d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b82: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b95: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b9f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ba6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bb2: Expected O, but got Unknown
			Label val = new Label();
			((Control)val).set_Location(new Point(0, 0));
			((Control)val).set_Width(labelWidth2);
			val.set_AutoSizeHeight(false);
			val.set_WrapText(false);
			((Control)val).set_Parent((Container)(object)defaultMountPanel);
			val.set_Text("Module keybind: ");
			((Control)val).set_BasicTooltipText("The module keybind is used to trigger the behaviour in the next setting.");
			Label settingDefaultMountKeybind_Label = val;
			KeybindingAssigner val2 = new KeybindingAssigner(Module._settingDefaultMountBinding.get_Value());
			val2.set_NameWidth(0);
			((Control)val2).set_Width(optionWidth);
			((Control)val2).set_Size(new Point(optionWidth, 20));
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
			val3.set_Text("Module keybind behaviour: ");
			((Control)val3).set_BasicTooltipText("Either display the radial or use the default action when the module keybind is held down.\nBoth are dependent on the context the player is in.\nDefault: Radial");
			Label settingKeybindBehaviour_Label = val3;
			Dropdown val4 = new Dropdown();
			((Control)val4).set_Location(new Point(((Control)settingKeybindBehaviour_Label).get_Right() + 5, ((Control)settingKeybindBehaviour_Label).get_Top() - 4));
			((Control)val4).set_Width(optionWidth);
			((Control)val4).set_Parent((Container)(object)defaultMountPanel);
			Dropdown settingKeybindBehaviour_Select = val4;
			settingKeybindBehaviour_Select.get_Items().Add("Disabled");
			List<string> keybindBehaviours = Module._keybindBehaviours.ToList();
			foreach (string j in keybindBehaviours)
			{
				settingKeybindBehaviour_Select.get_Items().Add(j.ToString());
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
			val5.set_Text("Module Keybind Tap Threshold:");
			((Control)val5).set_BasicTooltipText("The threshold to determine whether a module keybind press is a \"tap\" (in milliseconds).\nOnly applicable for contextual radial settings.\nDefault: 500ms (0.5s).");
			Label settingTapThresholdInMilliseconds_Label = val5;
			TrackBar val6 = new TrackBar();
			((Control)val6).set_Location(new Point(((Control)settingTapThresholdInMilliseconds_Label).get_Right() + 5, ((Control)settingTapThresholdInMilliseconds_Label).get_Top()));
			((Control)val6).set_Width(optionWidth);
			val6.set_MaxValue(5000f);
			val6.set_MinValue(0f);
			val6.set_Value((float)Module._settingTapThresholdInMilliseconds.get_Value());
			((Control)val6).set_Parent((Container)(object)defaultMountPanel);
			((Control)val6).set_BasicTooltipText($"{Module._settingTapThresholdInMilliseconds.get_Value()}");
			TrackBar settingTapThresholdInMilliseconds_Slider = val6;
			settingTapThresholdInMilliseconds_Slider.add_ValueChanged((EventHandler<ValueEventArgs<float>>)delegate
			{
				Module._settingTapThresholdInMilliseconds.set_Value((int)settingTapThresholdInMilliseconds_Slider.get_Value());
				((Control)settingTapThresholdInMilliseconds_Slider).set_BasicTooltipText($"{Module._settingTapThresholdInMilliseconds.get_Value()}");
			});
			Label val7 = new Label();
			((Control)val7).set_Location(new Point(0, ((Control)settingTapThresholdInMilliseconds_Label).get_Bottom() + 6));
			((Control)val7).set_Width(labelWidth2);
			val7.set_AutoSizeHeight(false);
			val7.set_WrapText(false);
			((Control)val7).set_Parent((Container)(object)defaultMountPanel);
			val7.set_Text("In-game Jump key binding: ");
			((Control)val7).set_BasicTooltipText("Used to detect gliding better for the IsPlayerGlidingOrFalling contextual radial settings.");
			Label settingJumpbinding_Label = val7;
			Image val8 = new Image();
			((Control)val8).set_Parent((Container)(object)defaultMountPanel);
			((Control)val8).set_Size(new Point(16, 16));
			((Control)val8).set_Location(new Point(((Control)settingJumpbinding_Label).get_Right() - 80, ((Control)settingJumpbinding_Label).get_Bottom() - 16));
			val8.set_Texture(AsyncTexture2D.op_Implicit(anetTexture));
			KeybindingAssigner val9 = new KeybindingAssigner(Module._settingJumpBinding.get_Value());
			val9.set_NameWidth(0);
			((Control)val9).set_Size(new Point(optionWidth, 20));
			((Control)val9).set_Parent((Container)(object)defaultMountPanel);
			((Control)val9).set_Location(new Point(((Control)settingJumpbinding_Label).get_Right() + 4, ((Control)settingJumpbinding_Label).get_Top() - 1));
			Label val10 = new Label();
			((Control)val10).set_Location(new Point(0, ((Control)settingJumpbinding_Label).get_Bottom() + 6));
			((Control)val10).set_Width(labelWidth2);
			val10.set_AutoSizeHeight(false);
			val10.set_WrapText(false);
			((Control)val10).set_Parent((Container)(object)defaultMountPanel);
			val10.set_Text("Falling or gliding update frequency:");
			((Control)val10).set_BasicTooltipText("Used to detect gliding and falling better for the IsPlayerGlidingOrFalling contextual radial settings.\nLower: faster reaction (might cause flickering when holding the module keybind).\nHigher: slightly slower reaction time, but more stable detection.\nDefault: 0.1.");
			Label settingFallingOrGlidingUpdateFrequency_Label = val10;
			TrackBar val11 = new TrackBar();
			((Control)val11).set_Location(new Point(((Control)settingFallingOrGlidingUpdateFrequency_Label).get_Right() + 5, ((Control)settingFallingOrGlidingUpdateFrequency_Label).get_Top()));
			((Control)val11).set_Width(optionWidth);
			val11.set_MaxValue(1f);
			val11.set_SmallStep(true);
			val11.set_MinValue(0f);
			val11.set_Value(Module._settingFallingOrGlidingUpdateFrequency.get_Value());
			((Control)val11).set_Parent((Container)(object)defaultMountPanel);
			((Control)val11).set_BasicTooltipText($"{Module._settingFallingOrGlidingUpdateFrequency.get_Value()}");
			TrackBar settingFallingOrGlidingUpdateFrequency_Slider = val11;
			settingFallingOrGlidingUpdateFrequency_Slider.add_ValueChanged((EventHandler<ValueEventArgs<float>>)delegate
			{
				Module._settingFallingOrGlidingUpdateFrequency.set_Value(settingFallingOrGlidingUpdateFrequency_Slider.get_Value());
				((Control)settingFallingOrGlidingUpdateFrequency_Slider).set_BasicTooltipText($"{Module._settingFallingOrGlidingUpdateFrequency.get_Value()}");
			});
			Label val12 = new Label();
			((Control)val12).set_Location(new Point(0, ((Control)settingFallingOrGlidingUpdateFrequency_Label).get_Bottom() + 6));
			((Control)val12).set_Width(labelWidth2);
			val12.set_AutoSizeHeight(false);
			val12.set_WrapText(false);
			((Control)val12).set_Parent((Container)(object)defaultMountPanel);
			val12.set_Text("Block sequence from GW2:");
			((Control)val12).set_BasicTooltipText("When checked, the sequence is not sent to GW2 otherwise it is sent to GW2.\nDefault: enabled.");
			Label settingBlockSequenceFromGw2_Label = val12;
			Checkbox val13 = new Checkbox();
			((Control)val13).set_Size(new Point(labelWidth2, 20));
			((Control)val13).set_Parent((Container)(object)defaultMountPanel);
			val13.set_Checked(Module._settingBlockSequenceFromGw2.get_Value());
			((Control)val13).set_Location(new Point(((Control)settingBlockSequenceFromGw2_Label).get_Right() + 5, ((Control)settingBlockSequenceFromGw2_Label).get_Top() - 1));
			Checkbox settingBlockSequenceFromGw2_Checkbox = val13;
			settingBlockSequenceFromGw2_Checkbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				Module._settingBlockSequenceFromGw2.set_Value(settingBlockSequenceFromGw2_Checkbox.get_Checked());
				Module.UserDefinedRadialSettings.ForEach(delegate(UserDefinedRadialThingSettings u)
				{
					u.Keybind.get_Value().set_BlockSequenceFromGw2(settingBlockSequenceFromGw2_Checkbox.get_Checked());
				});
				Module._settingDefaultMountBinding.get_Value().set_BlockSequenceFromGw2(settingBlockSequenceFromGw2_Checkbox.get_Checked());
			});
			Label val14 = new Label();
			((Control)val14).set_Location(new Point(0, ((Control)settingBlockSequenceFromGw2_Label).get_Bottom() + 6));
			((Control)val14).set_Width(labelWidth2);
			val14.set_AutoSizeHeight(false);
			val14.set_WrapText(false);
			((Control)val14).set_Parent((Container)(object)defaultMountPanel);
			val14.set_Text("Display module on loading screen:");
			((Control)val14).set_BasicTooltipText("Allow the module to be displayed on the loading screen.");
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
			((Control)val16).set_BasicTooltipText("This option allows the an action to be activated after loading screen. Only applicable for mounts.\nDefault: disabled.");
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
			Label val18 = new Label();
			((Control)val18).set_Location(new Point(0, ((Control)settingMountAutomaticallyAfterLoadingScreen_Label).get_Bottom() + 6));
			((Control)val18).set_Width(labelWidth2);
			val18.set_AutoSizeHeight(false);
			val18.set_WrapText(false);
			((Control)val18).set_Parent((Container)(object)defaultMountPanel);
			val18.set_Text("Combat Launch mastery unlocked: ");
			((Control)val18).set_BasicTooltipText("Combat Launch mastery allows the user to mount on the Skyscale mount when in combat.\nThis is detected via the API.\nIf you don't want to use Combat Mastery you can disable this option and out of combat queuing will still happen.\nDefault: disabled.");
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
			Label val20 = new Label();
			((Control)val20).set_Location(new Point(0, ((Control)combatLaunchMasteryUnlocked_Label).get_Bottom() + 6));
			((Control)val20).set_Width(labelWidth2);
			val20.set_AutoSizeHeight(false);
			val20.set_WrapText(false);
			((Control)val20).set_Parent((Container)(object)defaultMountPanel);
			val20.set_Text("Enable out of combat queueing:");
			((Control)val20).set_BasicTooltipText("When using an action that cannot be done in combat we queue this action and perform in when the player is out of combat.\nOnly the last action in combat will be performed.\nDefault: disabled");
			Label settingEnableMountQueueing_Label = val20;
			Checkbox val21 = new Checkbox();
			((Control)val21).set_Size(new Point(labelWidth2, 20));
			((Control)val21).set_Parent((Container)(object)defaultMountPanel);
			val21.set_Checked(Module._settingEnableMountQueueing.get_Value());
			((Control)val21).set_Location(new Point(((Control)settingEnableMountQueueing_Label).get_Right() + 5, ((Control)settingEnableMountQueueing_Label).get_Top() - 1));
			Checkbox settingEnableMountQueueing_Checkbox = val21;
			settingEnableMountQueueing_Checkbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				Module._settingEnableMountQueueing.set_Value(settingEnableMountQueueing_Checkbox.get_Checked());
			});
			Label val22 = new Label();
			((Control)val22).set_Location(new Point(0, ((Control)settingEnableMountQueueing_Label).get_Bottom() + 6));
			((Control)val22).set_Width(labelWidth2);
			val22.set_AutoSizeHeight(false);
			val22.set_WrapText(false);
			((Control)val22).set_Parent((Container)(object)defaultMountPanel);
			((Control)val22).set_BasicTooltipText("The info panel displays out of combat queueing, \"mount automatically after loading screen\" and ground target action.\nSee settings and documentation for more info.");
			val22.set_Text("Drag info panel: ");
			Label dragInfoPanel_Label = val22;
			Checkbox val23 = new Checkbox();
			((Control)val23).set_Size(new Point(20, 20));
			((Control)val23).set_Parent((Container)(object)defaultMountPanel);
			val23.set_Checked(Module._settingDragInfoPanel.get_Value());
			((Control)val23).set_Location(new Point(((Control)dragInfoPanel_Label).get_Right() + 5, ((Control)dragInfoPanel_Label).get_Top() - 1));
			Checkbox dragInfoPanel_Checkbox = val23;
			dragInfoPanel_Checkbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				Module._settingDragInfoPanel.set_Value(dragInfoPanel_Checkbox.get_Checked());
			});
			Label val24 = new Label();
			((Control)val24).set_Location(new Point(0, ((Control)dragInfoPanel_Label).get_Bottom() + 6));
			((Control)val24).set_Width(labelWidth2);
			val24.set_AutoSizeHeight(false);
			val24.set_WrapText(false);
			((Control)val24).set_Parent((Container)(object)defaultMountPanel);
			val24.set_Text("Display out of combat queueing:");
			((Control)val24).set_BasicTooltipText("Displays \"out of combat queueing\" in the info panel.");
			Label settingDisplayMountQueueing_Label = val24;
			Checkbox val25 = new Checkbox();
			((Control)val25).set_Size(new Point(labelWidth2, 20));
			((Control)val25).set_Parent((Container)(object)defaultMountPanel);
			val25.set_Checked(Module._settingDisplayMountQueueing.get_Value());
			((Control)val25).set_Location(new Point(((Control)settingDisplayMountQueueing_Label).get_Right() + 5, ((Control)settingDisplayMountQueueing_Label).get_Top() - 1));
			Checkbox settingDisplayMountQueueing_Checkbox = val25;
			settingDisplayMountQueueing_Checkbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				Module._settingDisplayMountQueueing.set_Value(settingDisplayMountQueueing_Checkbox.get_Checked());
			});
			Label val26 = new Label();
			((Control)val26).set_Location(new Point(0, ((Control)settingDisplayMountQueueing_Label).get_Bottom() + 6));
			((Control)val26).set_Width(labelWidth2);
			val26.set_AutoSizeHeight(false);
			val26.set_WrapText(false);
			((Control)val26).set_Parent((Container)(object)defaultMountPanel);
			val26.set_Text("Display \"mount automatically after loading screen\":");
			((Control)val26).set_BasicTooltipText("Displays \"mount automatically after loading screen\" in the info panel.");
			Label settingDisplayLaterActivation_Label = val26;
			Checkbox val27 = new Checkbox();
			((Control)val27).set_Size(new Point(labelWidth2, 20));
			((Control)val27).set_Parent((Container)(object)defaultMountPanel);
			val27.set_Checked(Module._settingDisplayLaterActivation.get_Value());
			((Control)val27).set_Location(new Point(((Control)settingDisplayLaterActivation_Label).get_Right() + 5, ((Control)settingDisplayLaterActivation_Label).get_Top() - 1));
			Checkbox settingDisplayLaterActivation_Checkbox = val27;
			settingDisplayLaterActivation_Checkbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				Module._settingDisplayLaterActivation.set_Value(settingDisplayLaterActivation_Checkbox.get_Checked());
			});
			Label val28 = new Label();
			((Control)val28).set_Location(new Point(0, ((Control)settingDisplayLaterActivation_Label).get_Bottom() + 6));
			((Control)val28).set_Width(labelWidth2);
			val28.set_AutoSizeHeight(false);
			val28.set_WrapText(false);
			((Control)val28).set_Parent((Container)(object)defaultMountPanel);
			val28.set_Text("Display ground target action:");
			((Control)val28).set_BasicTooltipText("Displays the \"ground target action\" in the info panel.");
			Label settingDisplayGroundTargetAction_Label = val28;
			Checkbox val29 = new Checkbox();
			((Control)val29).set_Size(new Point(labelWidth2, 20));
			((Control)val29).set_Parent((Container)(object)defaultMountPanel);
			val29.set_Checked(Module._settingDisplayGroundTargetingAction.get_Value());
			((Control)val29).set_Location(new Point(((Control)settingDisplayGroundTargetAction_Label).get_Right() + 5, ((Control)settingDisplayGroundTargetAction_Label).get_Top() - 1));
			Checkbox settingDisplayGroundTargetAction_Checkbox = val29;
			settingDisplayGroundTargetAction_Checkbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				Module._settingDisplayGroundTargetingAction.set_Value(settingDisplayGroundTargetAction_Checkbox.get_Checked());
			});
			Label val30 = new Label();
			((Control)val30).set_Location(new Point(0, ((Control)settingDisplayGroundTargetAction_Label).get_Bottom() + 6));
			((Control)val30).set_Width(labelWidth2);
			val30.set_AutoSizeHeight(false);
			val30.set_WrapText(false);
			((Control)val30).set_Parent((Container)(object)defaultMountPanel);
			val30.set_Text("Ground targeting: ");
			((Control)val30).set_BasicTooltipText("Normal - Show range indicator on first press, cast on second.\nFast with range indicator - Show range indicator on keypress, cast on release.\nInstant - Instantly cast at your mouse cursor's location.");
			Label settingGroundTargetting_Label = val30;
			Image val31 = new Image();
			((Control)val31).set_Parent((Container)(object)defaultMountPanel);
			((Control)val31).set_Size(new Point(16, 16));
			((Control)val31).set_Location(new Point(((Control)settingGroundTargetting_Label).get_Right() - 140, ((Control)settingGroundTargetting_Label).get_Bottom() - 16));
			val31.set_Texture(AsyncTexture2D.op_Implicit(anetTexture));
			Dropdown val32 = new Dropdown();
			((Control)val32).set_Location(new Point(((Control)settingGroundTargetting_Label).get_Right() + 5, ((Control)settingGroundTargetting_Label).get_Top() - 4));
			((Control)val32).set_Width(optionWidth);
			((Control)val32).set_Parent((Container)(object)defaultMountPanel);
			Dropdown settingGroundTargetting_Select = val32;
			foreach (object i in Enum.GetValues(typeof(GroundTargeting)))
			{
				settingGroundTargetting_Select.get_Items().Add(i.ToString());
			}
			settingGroundTargetting_Select.set_SelectedItem(Module._settingGroundTargeting.get_Value().ToString());
			settingGroundTargetting_Select.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				Module._settingGroundTargeting.set_Value((GroundTargeting)Enum.Parse(typeof(GroundTargeting), settingGroundTargetting_Select.get_SelectedItem()));
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
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Expected O, but got Unknown
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Expected O, but got Unknown
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Expected O, but got Unknown
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Expected O, but got Unknown
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_015a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0164: Unknown result type (might be due to invalid IL or missing references)
			//IL_016b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0176: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_0197: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Expected O, but got Unknown
			//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01da: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0207: Expected O, but got Unknown
			//IL_0208: Unknown result type (might be due to invalid IL or missing references)
			//IL_020d: Unknown result type (might be due to invalid IL or missing references)
			//IL_021e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0228: Unknown result type (might be due to invalid IL or missing references)
			//IL_022f: Unknown result type (might be due to invalid IL or missing references)
			//IL_023a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0245: Unknown result type (might be due to invalid IL or missing references)
			//IL_025b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0267: Expected O, but got Unknown
			//IL_027e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0283: Unknown result type (might be due to invalid IL or missing references)
			//IL_028e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0298: Unknown result type (might be due to invalid IL or missing references)
			//IL_029f: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cc: Expected O, but got Unknown
			//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_030a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0320: Unknown result type (might be due to invalid IL or missing references)
			//IL_032c: Expected O, but got Unknown
			//IL_0343: Unknown result type (might be due to invalid IL or missing references)
			//IL_0348: Unknown result type (might be due to invalid IL or missing references)
			//IL_0353: Unknown result type (might be due to invalid IL or missing references)
			//IL_035d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0364: Unknown result type (might be due to invalid IL or missing references)
			//IL_036b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0372: Unknown result type (might be due to invalid IL or missing references)
			//IL_0379: Unknown result type (might be due to invalid IL or missing references)
			//IL_0384: Unknown result type (might be due to invalid IL or missing references)
			//IL_0391: Expected O, but got Unknown
			//IL_0392: Unknown result type (might be due to invalid IL or missing references)
			//IL_0397: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_03cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f1: Expected O, but got Unknown
			//IL_0408: Unknown result type (might be due to invalid IL or missing references)
			//IL_040d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0418: Unknown result type (might be due to invalid IL or missing references)
			//IL_0422: Unknown result type (might be due to invalid IL or missing references)
			//IL_0429: Unknown result type (might be due to invalid IL or missing references)
			//IL_0430: Unknown result type (might be due to invalid IL or missing references)
			//IL_0437: Unknown result type (might be due to invalid IL or missing references)
			//IL_043e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0449: Unknown result type (might be due to invalid IL or missing references)
			//IL_0456: Expected O, but got Unknown
			//IL_0456: Unknown result type (might be due to invalid IL or missing references)
			//IL_045b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0462: Unknown result type (might be due to invalid IL or missing references)
			//IL_0467: Unknown result type (might be due to invalid IL or missing references)
			//IL_0471: Unknown result type (might be due to invalid IL or missing references)
			//IL_0486: Unknown result type (might be due to invalid IL or missing references)
			//IL_04aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_04af: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_04cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_04de: Unknown result type (might be due to invalid IL or missing references)
			Label val = new Label();
			((Control)val).set_Location(new Point(0, 0));
			((Control)val).set_Width(labelWidth);
			val.set_AutoSizeHeight(false);
			val.set_WrapText(false);
			((Control)val).set_Parent(radialPanel);
			val.set_Text("Radial settings: ");
			((Control)val).set_BasicTooltipText("Settings applied to all radials, both contextual and user-defined.");
			Label settingMountRadialSettingsMount_Label = val;
			Label val2 = new Label();
			((Control)val2).set_Location(new Point(0, ((Control)settingMountRadialSettingsMount_Label).get_Bottom() + 6));
			((Control)val2).set_Width(labelWidth);
			val2.set_AutoSizeHeight(false);
			val2.set_WrapText(false);
			((Control)val2).set_Parent(radialPanel);
			val2.set_Text("Spawn at mouse: ");
			((Control)val2).set_BasicTooltipText("When enabled the radials will spawn at your current mouse position, otherwise they spawn at the middle of your screen and your mouse cursor will be moved towards that by the module.\nDefault: disabled.");
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
			((Control)val4).set_BasicTooltipText("Configures the size of the radials.");
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
				((Control)settingMountRadialRadiusModifier_Slider).set_BasicTooltipText($"{Module._settingMountRadialRadiusModifier.get_Value()}");
			});
			Label val6 = new Label();
			((Control)val6).set_Location(new Point(0, ((Control)settingMountRadialRadiusModifier_Label).get_Bottom() + 6));
			((Control)val6).set_Width(labelWidth);
			val6.set_AutoSizeHeight(false);
			val6.set_WrapText(false);
			((Control)val6).set_Parent(radialPanel);
			val6.set_Text("Start angle: ");
			((Control)val6).set_BasicTooltipText("Configures the start point of the first action in the radials when displaying.");
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
				((Control)settingMountRadialStartAngle_Slider).set_BasicTooltipText($"{Module._settingMountRadialStartAngle.get_Value()}");
			});
			Label val8 = new Label();
			((Control)val8).set_Location(new Point(0, ((Control)settingMountRadialStartAngle_Label).get_Bottom() + 6));
			((Control)val8).set_Width(labelWidth);
			val8.set_AutoSizeHeight(false);
			val8.set_WrapText(false);
			((Control)val8).set_Parent(radialPanel);
			val8.set_Text("Icon size: ");
			((Control)val8).set_BasicTooltipText("Configures the icon size when displaying icons in the radials.");
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
				((Control)settingMountRadialIconSizeModifier_Slider).set_BasicTooltipText($"{Module._settingMountRadialIconSizeModifier.get_Value()}");
			});
			Label val10 = new Label();
			((Control)val10).set_Location(new Point(0, ((Control)settingMountRadialIconSizeModifier_Label).get_Bottom() + 6));
			((Control)val10).set_Width(labelWidth);
			val10.set_AutoSizeHeight(false);
			val10.set_WrapText(false);
			((Control)val10).set_Parent(radialPanel);
			val10.set_Text("Icon opacity: ");
			((Control)val10).set_BasicTooltipText("Configures the icon opacity when displaying icons in the radials.");
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
				((Control)settingMountRadialIconOpacity_Slider).set_BasicTooltipText($"{Module._settingMountRadialIconOpacity.get_Value()}");
			});
			Label val12 = new Label();
			((Control)val12).set_Location(new Point(0, ((Control)settingMountRadialIconOpacity_Label).get_Bottom() + 6));
			((Control)val12).set_Width(labelWidth);
			val12.set_AutoSizeHeight(false);
			val12.set_WrapText(false);
			((Control)val12).set_Parent(radialPanel);
			val12.set_Text("In-game action camera key binding: ");
			((Control)val12).set_BasicTooltipText("Used to toggle the action camera in-game when displaying a radial to help with selecting an action.");
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
