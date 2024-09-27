using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Manlaan.Mounts.Things;
using Microsoft.Xna.Framework;
using Mounts;
using Mounts.Settings;

namespace Manlaan.Mounts.Views
{
	internal class RadialThingSettingsView : View
	{
		private int totalWidth = 2500;

		private int labelWidth = 170;

		private int bindingWidth = 170;

		private Panel RadialSettingsListPanel;

		private Panel RadialSettingsDetailPanel;

		private RadialThingSettings currentRadialSettings;

		private readonly Func<KeybindTriggerType, Task> _keybindCallback;

		private readonly Helper _helper;

		public RadialThingSettingsView(Func<KeybindTriggerType, Task> keybindCallback, Helper helper)
			: this()
		{
			_keybindCallback = keybindCallback;
			_helper = helper;
		}

		private Panel CreateDefaultPanel(Container buildPanel, Point location, int width)
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
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Expected O, but got Unknown
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			Label val = new Label();
			((Control)val).set_Location(new Point(10, 10));
			((Control)val).set_Width(800);
			val.set_AutoSizeHeight(true);
			val.set_WrapText(true);
			((Control)val).set_Parent(buildPanel);
			val.set_TextColor(Color.get_Red());
			val.set_Font(GameService.Content.get_DefaultFont18());
			val.set_Text("These radial settings dictate which actions are being displayed in contextual and user-defined radials.\nFor more info, see the documentation.".Replace(" ", "  "));
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
			RadialSettingsListPanel = CreateDefaultPanel(buildPanel, new Point(panelPadding, ((Control)labelExplanation).get_Bottom() + panelPadding), totalWidth);
			BuildRadialSettingsListPanel();
			RadialSettingsDetailPanel = CreateDefaultPanel(buildPanel, new Point(10, 500), totalWidth);
			RadialThingSettings settingsToDisplayOnOpen = Module.ContextualRadialSettings.Single((ContextualRadialThingSettings settings) => settings.IsDefault);
			RadialThingSettings triggeredRadialSettings = _helper.GetTriggeredRadialSettings();
			if (triggeredRadialSettings != null)
			{
				settingsToDisplayOnOpen = triggeredRadialSettings;
			}
			BuildRadialSettingsDetailPanel(settingsToDisplayOnOpen);
		}

		private void BuildRadialSettingsListPanel()
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Expected O, but got Unknown
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Expected O, but got Unknown
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Expected O, but got Unknown
			//IL_0168: Unknown result type (might be due to invalid IL or missing references)
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Unknown result type (might be due to invalid IL or missing references)
			//IL_017c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0188: Unknown result type (might be due to invalid IL or missing references)
			//IL_018f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0196: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cc: Expected O, but got Unknown
			//IL_0202: Unknown result type (might be due to invalid IL or missing references)
			//IL_0207: Unknown result type (might be due to invalid IL or missing references)
			//IL_0215: Unknown result type (might be due to invalid IL or missing references)
			//IL_021f: Unknown result type (might be due to invalid IL or missing references)
			//IL_022b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0232: Unknown result type (might be due to invalid IL or missing references)
			//IL_0239: Unknown result type (might be due to invalid IL or missing references)
			//IL_0245: Unknown result type (might be due to invalid IL or missing references)
			//IL_024d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0255: Unknown result type (might be due to invalid IL or missing references)
			//IL_025a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0268: Unknown result type (might be due to invalid IL or missing references)
			//IL_0272: Unknown result type (might be due to invalid IL or missing references)
			//IL_027e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0285: Unknown result type (might be due to invalid IL or missing references)
			//IL_028c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0298: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cb: Expected O, but got Unknown
			//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0302: Expected O, but got Unknown
			//IL_033c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0341: Unknown result type (might be due to invalid IL or missing references)
			//IL_034d: Unknown result type (might be due to invalid IL or missing references)
			//IL_035c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0366: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03dc: Unknown result type (might be due to invalid IL or missing references)
			((Container)RadialSettingsListPanel).ClearChildren();
			Label val = new Label();
			((Control)val).set_Location(new Point(0, 10));
			((Control)val).set_Width(labelWidth);
			val.set_AutoSizeHeight(false);
			val.set_WrapText(false);
			((Control)val).set_Parent((Container)(object)RadialSettingsListPanel);
			val.set_Text("Name");
			val.set_HorizontalAlignment((HorizontalAlignment)0);
			Label nameHeader_Label = val;
			Label val2 = new Label();
			((Control)val2).set_Location(new Point(((Control)nameHeader_Label).get_Right() + 5, ((Control)nameHeader_Label).get_Top()));
			((Control)val2).set_Width(bindingWidth);
			val2.set_AutoSizeHeight(false);
			val2.set_WrapText(false);
			((Control)val2).set_Parent((Container)(object)RadialSettingsListPanel);
			val2.set_Text("Evaluation Order");
			val2.set_HorizontalAlignment((HorizontalAlignment)1);
			((Control)val2).set_BasicTooltipText("Determines the order of evaluation of the contextual radials, 0 is checked first, then 1, etc.\nWhen an active contextual radial is found the evaluation stops.");
			Label orderHeader_label = val2;
			Label val3 = new Label();
			((Control)val3).set_Location(new Point(((Control)orderHeader_label).get_Right() + 5, ((Control)nameHeader_Label).get_Top()));
			((Control)val3).set_Width(bindingWidth);
			val3.set_AutoSizeHeight(false);
			val3.set_WrapText(false);
			((Control)val3).set_Parent((Container)(object)RadialSettingsListPanel);
			val3.set_Text("Enabled");
			((Control)val3).set_BasicTooltipText("Disabled radials are not taken into account for the evaluation and are thus not displayed.");
			val3.set_HorizontalAlignment((HorizontalAlignment)1);
			Label enabledHeader_label = val3;
			int curY = ((Control)nameHeader_Label).get_Bottom() + 6;
			foreach (RadialThingSettings radialSettings in _helper.GetAllGenericRadialThingSettings())
			{
				Label val4 = new Label();
				((Control)val4).set_Location(new Point(0, curY + 6));
				((Control)val4).set_Width(labelWidth);
				val4.set_AutoSizeHeight(false);
				val4.set_WrapText(false);
				((Control)val4).set_Parent((Container)(object)RadialSettingsListPanel);
				val4.set_Text(radialSettings.Name + ": ");
				val4.set_HorizontalAlignment((HorizontalAlignment)0);
				Label name_Label = val4;
				string orderText = "N/A";
				ContextualRadialThingSettings contextualRadialSettings = radialSettings as ContextualRadialThingSettings;
				if (contextualRadialSettings != null)
				{
					orderText = $"{contextualRadialSettings.Order}";
				}
				Label val5 = new Label();
				((Control)val5).set_Location(new Point(((Control)orderHeader_label).get_Left(), ((Control)name_Label).get_Top()));
				((Control)val5).set_Width(labelWidth);
				val5.set_AutoSizeHeight(false);
				val5.set_WrapText(false);
				((Control)val5).set_Parent((Container)(object)RadialSettingsListPanel);
				val5.set_Text(orderText);
				val5.set_HorizontalAlignment((HorizontalAlignment)1);
				Label val6 = new Label();
				((Control)val6).set_Location(new Point(((Control)enabledHeader_label).get_Left(), ((Control)name_Label).get_Top()));
				((Control)val6).set_Width(labelWidth);
				val6.set_AutoSizeHeight(false);
				val6.set_WrapText(false);
				((Control)val6).set_Parent((Container)(object)RadialSettingsListPanel);
				val6.set_Text(radialSettings.IsEnabled.get_Value() ? "Yes" : "No");
				val6.set_HorizontalAlignment((HorizontalAlignment)1);
				Label enabled_Label = val6;
				StandardButton val7 = new StandardButton();
				((Control)val7).set_Parent((Container)(object)RadialSettingsListPanel);
				((Control)val7).set_Location(new Point(((Control)enabled_Label).get_Right(), ((Control)name_Label).get_Top()));
				val7.set_Text(Strings.Edit);
				StandardButton editRadialSettingsButton = val7;
				((Control)editRadialSettingsButton).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					BuildRadialSettingsDetailPanel(radialSettings);
				});
				UserDefinedRadialThingSettings userDefinedRadialSettingsDelete = radialSettings as UserDefinedRadialThingSettings;
				if (userDefinedRadialSettingsDelete != null)
				{
					StandardButton val8 = new StandardButton();
					((Control)val8).set_Parent((Container)(object)RadialSettingsListPanel);
					((Control)val8).set_Location(new Point(((Control)editRadialSettingsButton).get_Right(), ((Control)editRadialSettingsButton).get_Top()));
					val8.set_Text(Strings.Delete);
					((Control)val8).add_Click((EventHandler<MouseEventArgs>)delegate
					{
						int val10 = Module.UserDefinedRadialSettings.IndexOf(userDefinedRadialSettingsDelete);
						Module.UserDefinedRadialSettings = Module.UserDefinedRadialSettings.Where((UserDefinedRadialThingSettings udrs) => udrs.Id != userDefinedRadialSettingsDelete.Id).ToList();
						userDefinedRadialSettingsDelete.DeleteFromSettings(Module.settingscollection);
						Module._settingUserDefinedRadialIds.set_Value((from id in Module._settingUserDefinedRadialIds.get_Value()
							where id != userDefinedRadialSettingsDelete.Id
							select id).ToList());
						BuildRadialSettingsListPanel();
						int num2 = Math.Min(val10, Module.UserDefinedRadialSettings.Count - 1);
						if (num2 >= 0)
						{
							currentRadialSettings = Module.UserDefinedRadialSettings.ElementAt(num2);
						}
						else
						{
							currentRadialSettings = Module.ContextualRadialSettings.Last();
						}
						BuildRadialSettingsDetailPanel();
					});
				}
				curY = ((Control)name_Label).get_Bottom();
			}
			StandardButton val9 = new StandardButton();
			((Control)val9).set_Parent((Container)(object)RadialSettingsListPanel);
			((Control)val9).set_Location(new Point(0, curY + 6));
			val9.set_Text(Strings.Add_UserDefined_Radial);
			((Control)val9).set_Width(labelWidth);
			((Control)val9).set_Enabled(Module._settingUserDefinedRadialIds.get_Value().Count <= 5);
			((Control)val9).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				int num = (from id in Module._settingUserDefinedRadialIds.get_Value()
					orderby id descending
					select id).FirstOrDefault() + 1;
				Module.UserDefinedRadialSettings.Add(new UserDefinedRadialThingSettings(Module.settingscollection, num, _keybindCallback));
				Module._settingUserDefinedRadialIds.set_Value(Module._settingUserDefinedRadialIds.get_Value().Append(num).ToList());
				BuildRadialSettingsListPanel();
				currentRadialSettings = Module.UserDefinedRadialSettings.Last();
				BuildRadialSettingsDetailPanel();
			});
		}

		private void BuildRadialSettingsDetailPanel(RadialThingSettings newCurrentRadialSettings = null)
		{
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Expected O, but got Unknown
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Expected O, but got Unknown
			//IL_015d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_016c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0176: Unknown result type (might be due to invalid IL or missing references)
			//IL_0182: Unknown result type (might be due to invalid IL or missing references)
			//IL_0189: Unknown result type (might be due to invalid IL or missing references)
			//IL_0190: Unknown result type (might be due to invalid IL or missing references)
			//IL_019c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b4: Expected O, but got Unknown
			//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01df: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0208: Unknown result type (might be due to invalid IL or missing references)
			//IL_021b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0220: Unknown result type (might be due to invalid IL or missing references)
			//IL_0225: Unknown result type (might be due to invalid IL or missing references)
			//IL_022f: Unknown result type (might be due to invalid IL or missing references)
			//IL_023b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0242: Unknown result type (might be due to invalid IL or missing references)
			//IL_0249: Unknown result type (might be due to invalid IL or missing references)
			//IL_0255: Unknown result type (might be due to invalid IL or missing references)
			//IL_0260: Unknown result type (might be due to invalid IL or missing references)
			//IL_026c: Expected O, but got Unknown
			//IL_026d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0272: Unknown result type (might be due to invalid IL or missing references)
			//IL_0283: Unknown result type (might be due to invalid IL or missing references)
			//IL_028d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0299: Unknown result type (might be due to invalid IL or missing references)
			//IL_02aa: Expected O, but got Unknown
			//IL_0380: Unknown result type (might be due to invalid IL or missing references)
			//IL_0385: Unknown result type (might be due to invalid IL or missing references)
			//IL_038f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0399: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d7: Expected O, but got Unknown
			//IL_03d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0406: Unknown result type (might be due to invalid IL or missing references)
			//IL_0417: Expected O, but got Unknown
			//IL_04bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_04cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_04fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0506: Unknown result type (might be due to invalid IL or missing references)
			//IL_0513: Expected O, but got Unknown
			//IL_0514: Unknown result type (might be due to invalid IL or missing references)
			//IL_0519: Unknown result type (might be due to invalid IL or missing references)
			//IL_0522: Unknown result type (might be due to invalid IL or missing references)
			//IL_052c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0538: Unknown result type (might be due to invalid IL or missing references)
			//IL_054e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0561: Unknown result type (might be due to invalid IL or missing references)
			//IL_0570: Expected O, but got Unknown
			//IL_0587: Unknown result type (might be due to invalid IL or missing references)
			//IL_058c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0597: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_05bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_05df: Expected O, but got Unknown
			//IL_05fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0602: Unknown result type (might be due to invalid IL or missing references)
			//IL_0607: Unknown result type (might be due to invalid IL or missing references)
			//IL_0611: Unknown result type (might be due to invalid IL or missing references)
			//IL_061d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0633: Unknown result type (might be due to invalid IL or missing references)
			//IL_0646: Unknown result type (might be due to invalid IL or missing references)
			//IL_0650: Unknown result type (might be due to invalid IL or missing references)
			//IL_065b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0672: Expected O, but got Unknown
			//IL_06b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_06bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_06c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_06d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_06de: Unknown result type (might be due to invalid IL or missing references)
			//IL_06e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_06f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0703: Unknown result type (might be due to invalid IL or missing references)
			//IL_0710: Expected O, but got Unknown
			//IL_0712: Unknown result type (might be due to invalid IL or missing references)
			//IL_0717: Unknown result type (might be due to invalid IL or missing references)
			//IL_071c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0726: Unknown result type (might be due to invalid IL or missing references)
			//IL_0732: Unknown result type (might be due to invalid IL or missing references)
			//IL_074e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0761: Unknown result type (might be due to invalid IL or missing references)
			//IL_0770: Expected O, but got Unknown
			//IL_07b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_07b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_07c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_07cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_07d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_07de: Unknown result type (might be due to invalid IL or missing references)
			//IL_07e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_07f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_07fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_07fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0807: Unknown result type (might be due to invalid IL or missing references)
			//IL_0814: Expected O, but got Unknown
			//IL_0816: Unknown result type (might be due to invalid IL or missing references)
			//IL_081b: Unknown result type (might be due to invalid IL or missing references)
			//IL_082e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0838: Unknown result type (might be due to invalid IL or missing references)
			//IL_0844: Unknown result type (might be due to invalid IL or missing references)
			//IL_0855: Expected O, but got Unknown
			//IL_0863: Unknown result type (might be due to invalid IL or missing references)
			//IL_096f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0974: Unknown result type (might be due to invalid IL or missing references)
			//IL_097f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0989: Unknown result type (might be due to invalid IL or missing references)
			//IL_0995: Unknown result type (might be due to invalid IL or missing references)
			//IL_099c: Unknown result type (might be due to invalid IL or missing references)
			//IL_09a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_09af: Unknown result type (might be due to invalid IL or missing references)
			//IL_09ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_09c7: Expected O, but got Unknown
			//IL_09c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_09ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_09d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_09dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_09e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a05: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a18: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a27: Expected O, but got Unknown
			//IL_0a52: Unknown result type (might be due to invalid IL or missing references)
			if (newCurrentRadialSettings != null)
			{
				if (currentRadialSettings != null)
				{
					currentRadialSettings.RadialSettingsUpdated -= CurrentRadialSettings_RadialSettingsUpdated;
				}
				currentRadialSettings = newCurrentRadialSettings;
				currentRadialSettings.RadialSettingsUpdated += CurrentRadialSettings_RadialSettingsUpdated;
			}
			((Container)RadialSettingsDetailPanel).ClearChildren();
			Label val = new Label();
			((Control)val).set_Location(new Point(0, 0));
			((Control)val).set_Width(labelWidth);
			val.set_AutoSizeHeight(false);
			val.set_WrapText(false);
			((Control)val).set_Parent((Container)(object)RadialSettingsDetailPanel);
			val.set_Text(currentRadialSettings.Name ?? "");
			Label radialSettingsName_Label = val;
			int curY = ((Control)radialSettingsName_Label).get_Bottom();
			RadialThingSettings radialThingSettings = currentRadialSettings;
			UserDefinedRadialThingSettings userDefinedRadialSettingsName = radialThingSettings as UserDefinedRadialThingSettings;
			if (userDefinedRadialSettingsName != null)
			{
				radialSettingsName_Label.set_Text("Name: ");
				TextBox val2 = new TextBox();
				((Control)val2).set_Location(new Point(((Control)radialSettingsName_Label).get_Right() + 5, 0));
				((Control)val2).set_Width(labelWidth);
				((Control)val2).set_Parent((Container)(object)RadialSettingsDetailPanel);
				((TextInputBase)val2).set_Text(userDefinedRadialSettingsName.Name ?? "");
				TextBox radialSettingstName_TextBox = val2;
				((TextInputBase)radialSettingstName_TextBox).add_TextChanged((EventHandler<EventArgs>)delegate
				{
					userDefinedRadialSettingsName.NameSetting.set_Value(((TextInputBase)radialSettingstName_TextBox).get_Text());
					BuildRadialSettingsListPanel();
				});
				Label val3 = new Label();
				((Control)val3).set_Location(new Point(0, ((Control)radialSettingsName_Label).get_Bottom() + 6));
				((Control)val3).set_Width(labelWidth);
				val3.set_AutoSizeHeight(false);
				val3.set_WrapText(false);
				((Control)val3).set_Parent((Container)(object)RadialSettingsDetailPanel);
				val3.set_Text("Key binding: ");
				((Control)val3).set_BasicTooltipText("The keybind to display this user-defined radial.");
				Label settingDefaultMountKeybind_Label = val3;
				KeybindingAssigner val4 = new KeybindingAssigner(userDefinedRadialSettingsName.Keybind.get_Value());
				val4.set_NameWidth(0);
				((Control)val4).set_Size(new Point(labelWidth, 20));
				((Control)val4).set_Parent((Container)(object)RadialSettingsDetailPanel);
				((Control)val4).set_Location(new Point(((Control)settingDefaultMountKeybind_Label).get_Right() + 4, ((Control)settingDefaultMountKeybind_Label).get_Top() - 1));
				curY = ((Control)settingDefaultMountKeybind_Label).get_Bottom();
			}
			Label val5 = new Label();
			((Control)val5).set_Location(new Point(0, curY + 6));
			((Control)val5).set_Width(labelWidth);
			val5.set_AutoSizeHeight(false);
			val5.set_WrapText(false);
			((Control)val5).set_Parent((Container)(object)RadialSettingsDetailPanel);
			val5.set_Text("Default: ");
			((Control)val5).set_BasicTooltipText("The default action using in this radial, see documentation (\"Default action\") for more info.");
			Label settingDefaultThing_Label = val5;
			Dropdown val6 = new Dropdown();
			((Control)val6).set_Location(new Point(((Control)settingDefaultThing_Label).get_Right() + 5, ((Control)settingDefaultThing_Label).get_Top() - 4));
			((Control)val6).set_Width(labelWidth);
			((Control)val6).set_Parent((Container)(object)RadialSettingsDetailPanel);
			Dropdown settingDefaultThing_Select = val6;
			settingDefaultThing_Select.get_Items().Add("Disabled");
			IEnumerable<string> thingNames = currentRadialSettings.Things.Select((Thing m) => m.Name);
			foreach (string k in thingNames)
			{
				settingDefaultThing_Select.get_Items().Add(k.ToString());
			}
			settingDefaultThing_Select.set_SelectedItem(thingNames.Any((string m) => m == currentRadialSettings.DefaultThingChoice.get_Value()) ? currentRadialSettings.DefaultThingChoice.get_Value() : "Disabled");
			settingDefaultThing_Select.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				currentRadialSettings.DefaultThingChoice.set_Value(settingDefaultThing_Select.get_SelectedItem());
			});
			Label val7 = new Label();
			((Control)val7).set_Location(new Point(0, ((Control)settingDefaultThing_Label).get_Bottom() + 6));
			((Control)val7).set_Width(labelWidth);
			val7.set_AutoSizeHeight(false);
			val7.set_WrapText(false);
			((Control)val7).set_Parent((Container)(object)RadialSettingsDetailPanel);
			val7.set_Text("Center: ");
			((Control)val7).set_BasicTooltipText("Which action is displayed in the middle of the radial.");
			Label settingRadialCenterMountBehavior_Label = val7;
			Dropdown val8 = new Dropdown();
			((Control)val8).set_Location(new Point(((Control)settingRadialCenterMountBehavior_Label).get_Right() + 5, ((Control)settingRadialCenterMountBehavior_Label).get_Top() - 4));
			((Control)val8).set_Width(labelWidth);
			((Control)val8).set_Parent((Container)(object)RadialSettingsDetailPanel);
			Dropdown settingRadialCenterMountBehavior_Select = val8;
			foreach (CenterBehavior j in Enum.GetValues(typeof(CenterBehavior)))
			{
				settingRadialCenterMountBehavior_Select.get_Items().Add(j.ToString());
			}
			settingRadialCenterMountBehavior_Select.set_SelectedItem(currentRadialSettings.CenterThingBehavior.get_Value().ToString());
			settingRadialCenterMountBehavior_Select.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				currentRadialSettings.CenterThingBehavior.set_Value((CenterBehavior)Enum.Parse(typeof(CenterBehavior), settingRadialCenterMountBehavior_Select.get_SelectedItem()));
			});
			Label val9 = new Label();
			((Control)val9).set_Location(new Point(0, ((Control)settingRadialCenterMountBehavior_Label).get_Bottom() + 6));
			((Control)val9).set_Width(labelWidth);
			val9.set_AutoSizeHeight(false);
			val9.set_WrapText(false);
			((Control)val9).set_Parent((Container)(object)RadialSettingsDetailPanel);
			val9.set_Text("Remove center from radial: ");
			((Control)val9).set_BasicTooltipText("Removes the center action from the radial ring when selected.");
			Label settingRadialRemoveCenterMount_Label = val9;
			Checkbox val10 = new Checkbox();
			((Control)val10).set_Size(new Point(labelWidth, 20));
			((Control)val10).set_Parent((Container)(object)RadialSettingsDetailPanel);
			val10.set_Checked(currentRadialSettings.RemoveCenterThing.get_Value());
			((Control)val10).set_Location(new Point(((Control)settingRadialRemoveCenterMount_Label).get_Right() + 5, ((Control)settingRadialRemoveCenterMount_Label).get_Top() - 1));
			Checkbox settingRadialRemoveCenterMount_Checkbox = val10;
			settingRadialRemoveCenterMount_Checkbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				currentRadialSettings.RemoveCenterThing.set_Value(settingRadialRemoveCenterMount_Checkbox.get_Checked());
			});
			Label val11 = new Label();
			((Control)val11).set_Location(new Point(0, ((Control)settingRadialRemoveCenterMount_Label).get_Bottom() + 6));
			((Control)val11).set_Width(labelWidth);
			val11.set_AutoSizeHeight(false);
			val11.set_WrapText(false);
			((Control)val11).set_Parent((Container)(object)RadialSettingsDetailPanel);
			val11.set_Text("Enabled");
			((Control)val11).set_BasicTooltipText("Disabled radials are not taken into account for the evaluation and are thus not displayed.");
			Label radialSettingsIsEnabled_Label = val11;
			bool IsDefault = false;
			ContextualRadialThingSettings contextualRadialSettings = currentRadialSettings as ContextualRadialThingSettings;
			if (contextualRadialSettings != null)
			{
				IsDefault = contextualRadialSettings.IsDefault;
			}
			Checkbox val12 = new Checkbox();
			((Control)val12).set_Size(new Point(20, 20));
			((Control)val12).set_Parent((Container)(object)RadialSettingsDetailPanel);
			val12.set_Checked(currentRadialSettings.IsEnabled.get_Value());
			((Control)val12).set_Location(new Point(((Control)radialSettingsIsEnabled_Label).get_Right() + 5, ((Control)radialSettingsIsEnabled_Label).get_Top() - 1));
			((Control)val12).set_Enabled(!IsDefault);
			((Control)val12).set_BasicTooltipText(IsDefault ? "Cannot disable Default" : null);
			Checkbox radialSettingsIsEnabled_Checkbox = val12;
			radialSettingsIsEnabled_Checkbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				currentRadialSettings.IsEnabled.set_Value(radialSettingsIsEnabled_Checkbox.get_Checked());
				BuildRadialSettingsListPanel();
			});
			radialThingSettings = currentRadialSettings;
			ContextualRadialThingSettings contextualRadialSettingsAtBottom = radialThingSettings as ContextualRadialThingSettings;
			if (contextualRadialSettingsAtBottom != null)
			{
				Label val13 = new Label();
				((Control)val13).set_Location(new Point(0, ((Control)radialSettingsIsEnabled_Label).get_Bottom() + 6));
				((Control)val13).set_Width(labelWidth);
				val13.set_AutoSizeHeight(false);
				val13.set_WrapText(false);
				((Control)val13).set_Parent((Container)(object)RadialSettingsDetailPanel);
				val13.set_Text("Apply instantly if single");
				((Control)val13).set_BasicTooltipText("When there is only 1 action configured in a radial context and this option is checked we do not display the radial, but we perform the action immediately instead.");
				Label radialSettingsApplyInstantlyIfSingle_Label = val13;
				Checkbox val14 = new Checkbox();
				((Control)val14).set_Size(new Point(20, 20));
				((Control)val14).set_Parent((Container)(object)RadialSettingsDetailPanel);
				val14.set_Checked(contextualRadialSettingsAtBottom.ApplyInstantlyIfSingle.get_Value());
				((Control)val14).set_Location(new Point(((Control)radialSettingsApplyInstantlyIfSingle_Label).get_Right() + 5, ((Control)radialSettingsApplyInstantlyIfSingle_Label).get_Top() - 1));
				Checkbox radialSettingsApplyInstantlyIfSingle_Checkbox = val14;
				radialSettingsApplyInstantlyIfSingle_Checkbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
				{
					contextualRadialSettingsAtBottom.ApplyInstantlyIfSingle.set_Value(radialSettingsApplyInstantlyIfSingle_Checkbox.get_Checked());
				});
				contextualRadialSettingsAtBottom.ApplyInstantlyIfSingle.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)delegate
				{
					BuildRadialSettingsDetailPanel();
				});
				Label val15 = new Label();
				((Control)val15).set_Location(new Point(0, ((Control)radialSettingsApplyInstantlyIfSingle_Label).get_Bottom() + 6));
				((Control)val15).set_Width(labelWidth);
				val15.set_AutoSizeHeight(false);
				val15.set_WrapText(false);
				((Control)val15).set_Parent((Container)(object)RadialSettingsDetailPanel);
				val15.set_Text("Apply instantly on tap: ");
				val15.set_TextColor(Color.get_White());
				((Control)val15).set_BasicTooltipText("The configured action will be hidden from the radial. When the module keybind is tapped we do not display the radial, but we perform this action immediately instead.");
				Label settingApplyInstantlyOnTap_Label = val15;
				Dropdown val16 = new Dropdown();
				((Control)val16).set_Location(new Point(((Control)settingApplyInstantlyOnTap_Label).get_Right() + 5, ((Control)settingApplyInstantlyOnTap_Label).get_Top() - 4));
				((Control)val16).set_Width(labelWidth);
				((Control)val16).set_Parent((Container)(object)RadialSettingsDetailPanel);
				Dropdown settingApplyInstantlyOnTap_Select = val16;
				if (Module._settingTapThresholdInMilliseconds.get_Value() == 0)
				{
					settingApplyInstantlyOnTap_Label.set_TextColor(Color.get_DarkGray());
					((Control)settingApplyInstantlyOnTap_Label).set_BasicTooltipText("Disabled since tap threshold is set to 0");
					((Control)settingApplyInstantlyOnTap_Select).set_Enabled(false);
				}
				settingApplyInstantlyOnTap_Select.get_Items().Add("Disabled");
				foreach (string i in contextualRadialSettingsAtBottom.Things.Select((Thing m) => m.Name))
				{
					settingApplyInstantlyOnTap_Select.get_Items().Add(i.ToString());
				}
				settingApplyInstantlyOnTap_Select.set_SelectedItem(thingNames.Any((string m) => m == contextualRadialSettingsAtBottom.ApplyInstantlyOnTap.get_Value()) ? contextualRadialSettingsAtBottom.ApplyInstantlyOnTap.get_Value() : "Disabled");
				settingApplyInstantlyOnTap_Select.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
				{
					contextualRadialSettingsAtBottom.ApplyInstantlyOnTap.set_Value(settingApplyInstantlyOnTap_Select.get_SelectedItem());
				});
				Label val17 = new Label();
				((Control)val17).set_Location(new Point(0, ((Control)settingApplyInstantlyOnTap_Label).get_Bottom() + 6));
				((Control)val17).set_Width(labelWidth);
				val17.set_AutoSizeHeight(false);
				val17.set_WrapText(false);
				((Control)val17).set_Parent((Container)(object)RadialSettingsDetailPanel);
				val17.set_Text("Unconditionally Do Action");
				((Control)val17).set_BasicTooltipText("Used to disable \"out of combat queuing\", \"LastUsed\" and \"mount automatically\" functionality. Only useful when the user has configured a mount action (e.g.: Raptor) instead of the dismount action to dismount in the IsPlayerMounted contextual radial settings.");
				Label radialSettingsUnconditionallyDoAction_Label = val17;
				Checkbox val18 = new Checkbox();
				((Control)val18).set_Size(new Point(20, 20));
				((Control)val18).set_Parent((Container)(object)RadialSettingsDetailPanel);
				val18.set_Checked(contextualRadialSettingsAtBottom.UnconditionallyDoAction.get_Value());
				((Control)val18).set_Location(new Point(((Control)radialSettingsUnconditionallyDoAction_Label).get_Right() + 5, ((Control)radialSettingsUnconditionallyDoAction_Label).get_Top() - 1));
				Checkbox radialSettingsUnconditionallyDoAction_Checkbox = val18;
				radialSettingsUnconditionallyDoAction_Checkbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
				{
					contextualRadialSettingsAtBottom.UnconditionallyDoAction.set_Value(radialSettingsUnconditionallyDoAction_Checkbox.get_Checked());
				});
			}
			ThingSettingsView thingSettingsView = new ThingSettingsView(currentRadialSettings);
			((Control)thingSettingsView).set_Location(new Point(500, 0));
			((Control)thingSettingsView).set_Parent((Container)(object)RadialSettingsDetailPanel);
		}

		private void CurrentRadialSettings_RadialSettingsUpdated(object sender, SettingsUpdatedEvent e)
		{
			BuildRadialSettingsDetailPanel();
		}
	}
}
