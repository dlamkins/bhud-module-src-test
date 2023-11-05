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
		private int totalWidth = 1000;

		private int labelWidth = 170;

		private int bindingWidth = 170;

		private Panel RadialSettingsListPanel;

		private Panel RadialSettingsDetailPanel;

		private RadialThingSettings currentRadialSettings;

		private readonly Func<Task> _keybindCallback;

		public RadialThingSettingsView(Func<Task> keybindCallback)
			: this()
		{
			_keybindCallback = keybindCallback;
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
			val.set_Text("When enabled, these radial settings dictate which actions are being displayed.\nFor more info, see the documentation.".Replace(" ", "  "));
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
			BuildRadialSettingsDetailPanel(Module.ContextualRadialSettings.Single((ContextualRadialThingSettings settings) => settings.IsDefault));
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
			//IL_00ae: Expected O, but got Unknown
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Expected O, but got Unknown
			//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0206: Expected O, but got Unknown
			//IL_023c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0241: Unknown result type (might be due to invalid IL or missing references)
			//IL_024f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0259: Unknown result type (might be due to invalid IL or missing references)
			//IL_0265: Unknown result type (might be due to invalid IL or missing references)
			//IL_026c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0273: Unknown result type (might be due to invalid IL or missing references)
			//IL_027f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0287: Unknown result type (might be due to invalid IL or missing references)
			//IL_028f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0294: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0305: Expected O, but got Unknown
			//IL_0305: Unknown result type (might be due to invalid IL or missing references)
			//IL_030a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0316: Unknown result type (might be due to invalid IL or missing references)
			//IL_0325: Unknown result type (might be due to invalid IL or missing references)
			//IL_032f: Unknown result type (might be due to invalid IL or missing references)
			//IL_033c: Expected O, but got Unknown
			//IL_0376: Unknown result type (might be due to invalid IL or missing references)
			//IL_037b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0387: Unknown result type (might be due to invalid IL or missing references)
			//IL_0396: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03df: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_040a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0416: Unknown result type (might be due to invalid IL or missing references)
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
			Label orderHeader_label = val2;
			Label val3 = new Label();
			((Control)val3).set_Location(new Point(((Control)orderHeader_label).get_Right() + 5, ((Control)nameHeader_Label).get_Top()));
			((Control)val3).set_Width(bindingWidth);
			val3.set_AutoSizeHeight(false);
			val3.set_WrapText(false);
			((Control)val3).set_Parent((Container)(object)RadialSettingsListPanel);
			val3.set_Text("Enabled");
			val3.set_HorizontalAlignment((HorizontalAlignment)1);
			Label enabledHeader_label = val3;
			int curY = ((Control)nameHeader_Label).get_Bottom() + 6;
			List<RadialThingSettings> first = Module.ContextualRadialSettings.ConvertAll((Converter<ContextualRadialThingSettings, RadialThingSettings>)((ContextualRadialThingSettings x) => x));
			List<RadialThingSettings> userDefinedRadialSettingsCasted = Module.UserDefinedRadialSettings.ConvertAll((Converter<UserDefinedRadialThingSettings, RadialThingSettings>)((UserDefinedRadialThingSettings x) => x));
			foreach (RadialThingSettings radialSettings in first.Concat(userDefinedRadialSettingsCasted))
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
			//IL_01a9: Expected O, but got Unknown
			//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01de: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0210: Unknown result type (might be due to invalid IL or missing references)
			//IL_0215: Unknown result type (might be due to invalid IL or missing references)
			//IL_021a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0224: Unknown result type (might be due to invalid IL or missing references)
			//IL_0230: Unknown result type (might be due to invalid IL or missing references)
			//IL_0237: Unknown result type (might be due to invalid IL or missing references)
			//IL_023e: Unknown result type (might be due to invalid IL or missing references)
			//IL_024a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0256: Expected O, but got Unknown
			//IL_0257: Unknown result type (might be due to invalid IL or missing references)
			//IL_025c: Unknown result type (might be due to invalid IL or missing references)
			//IL_026d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0277: Unknown result type (might be due to invalid IL or missing references)
			//IL_0283: Unknown result type (might be due to invalid IL or missing references)
			//IL_0294: Expected O, but got Unknown
			//IL_036a: Unknown result type (might be due to invalid IL or missing references)
			//IL_036f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0379: Unknown result type (might be due to invalid IL or missing references)
			//IL_0383: Unknown result type (might be due to invalid IL or missing references)
			//IL_038f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0396: Unknown result type (might be due to invalid IL or missing references)
			//IL_039d: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b6: Expected O, but got Unknown
			//IL_03b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_03bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_03cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f6: Expected O, but got Unknown
			//IL_049a: Unknown result type (might be due to invalid IL or missing references)
			//IL_049f: Unknown result type (might be due to invalid IL or missing references)
			//IL_04aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_04da: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e7: Expected O, but got Unknown
			//IL_04e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0500: Unknown result type (might be due to invalid IL or missing references)
			//IL_050c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0522: Unknown result type (might be due to invalid IL or missing references)
			//IL_0535: Unknown result type (might be due to invalid IL or missing references)
			//IL_0544: Expected O, but got Unknown
			//IL_055b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0560: Unknown result type (might be due to invalid IL or missing references)
			//IL_056b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0575: Unknown result type (might be due to invalid IL or missing references)
			//IL_0581: Unknown result type (might be due to invalid IL or missing references)
			//IL_0588: Unknown result type (might be due to invalid IL or missing references)
			//IL_058f: Unknown result type (might be due to invalid IL or missing references)
			//IL_059b: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a8: Expected O, but got Unknown
			//IL_05c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_05cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_05da: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_05fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_060f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0619: Unknown result type (might be due to invalid IL or missing references)
			//IL_0624: Unknown result type (might be due to invalid IL or missing references)
			//IL_063b: Expected O, but got Unknown
			//IL_0681: Unknown result type (might be due to invalid IL or missing references)
			//IL_0686: Unknown result type (might be due to invalid IL or missing references)
			//IL_0691: Unknown result type (might be due to invalid IL or missing references)
			//IL_069b: Unknown result type (might be due to invalid IL or missing references)
			//IL_06a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_06b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_06c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ce: Expected O, but got Unknown
			//IL_06d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_06d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_06da: Unknown result type (might be due to invalid IL or missing references)
			//IL_06e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_06f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_070c: Unknown result type (might be due to invalid IL or missing references)
			//IL_071f: Unknown result type (might be due to invalid IL or missing references)
			//IL_072e: Expected O, but got Unknown
			//IL_0781: Unknown result type (might be due to invalid IL or missing references)
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
			Label settingDefaultThing_Label = val5;
			Dropdown val6 = new Dropdown();
			((Control)val6).set_Location(new Point(((Control)settingDefaultThing_Label).get_Right() + 5, ((Control)settingDefaultThing_Label).get_Top() - 4));
			((Control)val6).set_Width(labelWidth);
			((Control)val6).set_Parent((Container)(object)RadialSettingsDetailPanel);
			Dropdown settingDefaultThing_Select = val6;
			settingDefaultThing_Select.get_Items().Add("Disabled");
			IEnumerable<string> thingNames = currentRadialSettings.Things.Select((Thing m) => m.Name);
			foreach (string j in thingNames)
			{
				settingDefaultThing_Select.get_Items().Add(j.ToString());
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
			Label settingRadialCenterMountBehavior_Label = val7;
			Dropdown val8 = new Dropdown();
			((Control)val8).set_Location(new Point(((Control)settingRadialCenterMountBehavior_Label).get_Right() + 5, ((Control)settingRadialCenterMountBehavior_Label).get_Top() - 4));
			((Control)val8).set_Width(labelWidth);
			((Control)val8).set_Parent((Container)(object)RadialSettingsDetailPanel);
			Dropdown settingRadialCenterMountBehavior_Select = val8;
			foreach (CenterBehavior i in Enum.GetValues(typeof(CenterBehavior)))
			{
				settingRadialCenterMountBehavior_Select.get_Items().Add(i.ToString());
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
			Label settingRadialRemoveCenterMount_Label = val9;
			Checkbox val10 = new Checkbox();
			((Control)val10).set_Size(new Point(labelWidth, 20));
			((Control)val10).set_Parent((Container)(object)RadialSettingsDetailPanel);
			val10.set_Checked(currentRadialSettings.RemoveCenterMount.get_Value());
			((Control)val10).set_Location(new Point(((Control)settingRadialRemoveCenterMount_Label).get_Right() + 5, ((Control)settingRadialRemoveCenterMount_Label).get_Top() - 1));
			Checkbox settingRadialRemoveCenterMount_Checkbox = val10;
			settingRadialRemoveCenterMount_Checkbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				currentRadialSettings.RemoveCenterMount.set_Value(settingRadialRemoveCenterMount_Checkbox.get_Checked());
			});
			Label val11 = new Label();
			((Control)val11).set_Location(new Point(0, ((Control)settingRadialRemoveCenterMount_Label).get_Bottom() + 6));
			((Control)val11).set_Width(labelWidth);
			val11.set_AutoSizeHeight(false);
			val11.set_WrapText(false);
			((Control)val11).set_Parent((Container)(object)RadialSettingsDetailPanel);
			val11.set_Text("Enabled");
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
			ContextualRadialThingSettings contextualRadialSettingsApplyInstantlyIfSingle = radialThingSettings as ContextualRadialThingSettings;
			if (contextualRadialSettingsApplyInstantlyIfSingle != null)
			{
				Label val13 = new Label();
				((Control)val13).set_Location(new Point(0, ((Control)radialSettingsIsEnabled_Label).get_Bottom() + 6));
				((Control)val13).set_Width(labelWidth);
				val13.set_AutoSizeHeight(false);
				val13.set_WrapText(false);
				((Control)val13).set_Parent((Container)(object)RadialSettingsDetailPanel);
				val13.set_Text("Apply instantly if single");
				Label radialSettingsApplyInstantlyIfSingle_Label = val13;
				Checkbox val14 = new Checkbox();
				((Control)val14).set_Size(new Point(20, 20));
				((Control)val14).set_Parent((Container)(object)RadialSettingsDetailPanel);
				val14.set_Checked(contextualRadialSettingsApplyInstantlyIfSingle.ApplyInstantlyIfSingle.get_Value());
				((Control)val14).set_Location(new Point(((Control)radialSettingsApplyInstantlyIfSingle_Label).get_Right() + 5, ((Control)radialSettingsApplyInstantlyIfSingle_Label).get_Top() - 1));
				Checkbox radialSettingsApplyInstantlyIfSingle_Checkbox = val14;
				radialSettingsApplyInstantlyIfSingle_Checkbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
				{
					contextualRadialSettingsApplyInstantlyIfSingle.ApplyInstantlyIfSingle.set_Value(radialSettingsApplyInstantlyIfSingle_Checkbox.get_Checked());
				});
				contextualRadialSettingsApplyInstantlyIfSingle.ApplyInstantlyIfSingle.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)delegate
				{
					BuildRadialSettingsDetailPanel();
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
