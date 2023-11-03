using System;
using System.Diagnostics;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Mounts;
using Mounts.Settings;

namespace Manlaan.Mounts.Views
{
	internal class IconThingSettingsView : View
	{
		private int totalWidth = 1000;

		private int labelWidth = 170;

		private Panel IconSettingsListPanel;

		private Panel IconSettingsDetailPanel;

		private IconThingSettings currentIconSettings;

		public IconThingSettingsView()
			: this()
		{
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
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Expected O, but got Unknown
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			Label val = new Label();
			((Control)val).set_Location(new Point(10, 10));
			((Control)val).set_Width(totalWidth);
			val.set_AutoSizeHeight(true);
			val.set_WrapText(true);
			((Control)val).set_Parent(buildPanel);
			val.set_TextColor(Color.get_Red());
			val.set_Font(GameService.Content.get_DefaultFont18());
			val.set_Text("When enabled, these icon settings dictate which actions are being displayed.\nFor more info, see the documentation.".Replace(" ", "  "));
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
			IconSettingsListPanel = CreateDefaultPanel(buildPanel, new Point(panelPadding, ((Control)labelExplanation).get_Bottom() + panelPadding), totalWidth);
			BuildIconSettingsListPanel();
			currentIconSettings = Module.IconThingSettings.First();
			IconSettingsDetailPanel = CreateDefaultPanel(buildPanel, new Point(10, 500), totalWidth);
			BuildIconSettingsDetailPanel();
		}

		private void BuildIconSettingsListPanel()
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
			//IL_013c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0155: Unknown result type (might be due to invalid IL or missing references)
			//IL_0161: Unknown result type (might be due to invalid IL or missing references)
			//IL_0168: Unknown result type (might be due to invalid IL or missing references)
			//IL_016f: Unknown result type (might be due to invalid IL or missing references)
			//IL_017b: Unknown result type (might be due to invalid IL or missing references)
			//IL_019c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0203: Unknown result type (might be due to invalid IL or missing references)
			//IL_020c: Expected O, but got Unknown
			//IL_020c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0211: Unknown result type (might be due to invalid IL or missing references)
			//IL_021b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0225: Unknown result type (might be due to invalid IL or missing references)
			//IL_0231: Unknown result type (might be due to invalid IL or missing references)
			//IL_0238: Unknown result type (might be due to invalid IL or missing references)
			//IL_023f: Unknown result type (might be due to invalid IL or missing references)
			//IL_024b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0270: Unknown result type (might be due to invalid IL or missing references)
			//IL_0279: Expected O, but got Unknown
			//IL_0279: Unknown result type (might be due to invalid IL or missing references)
			//IL_027e: Unknown result type (might be due to invalid IL or missing references)
			//IL_028a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0299: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b0: Expected O, but got Unknown
			//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_033d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0342: Unknown result type (might be due to invalid IL or missing references)
			//IL_034e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0353: Unknown result type (might be due to invalid IL or missing references)
			//IL_035d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0368: Unknown result type (might be due to invalid IL or missing references)
			((Container)IconSettingsListPanel).ClearChildren();
			Label val = new Label();
			((Control)val).set_Location(new Point(0, 10));
			((Control)val).set_Width(labelWidth);
			val.set_AutoSizeHeight(false);
			val.set_WrapText(false);
			((Control)val).set_Parent((Container)(object)IconSettingsListPanel);
			val.set_Text("Id");
			val.set_HorizontalAlignment((HorizontalAlignment)0);
			Label idHeader_Label = val;
			Label val2 = new Label();
			((Control)val2).set_Location(new Point(((Control)idHeader_Label).get_Right() + 5, ((Control)idHeader_Label).get_Top()));
			((Control)val2).set_Width(labelWidth);
			val2.set_AutoSizeHeight(false);
			val2.set_WrapText(false);
			((Control)val2).set_Parent((Container)(object)IconSettingsListPanel);
			val2.set_Text("Name");
			val2.set_HorizontalAlignment((HorizontalAlignment)0);
			Label nameHeader_Label = val2;
			Label val3 = new Label();
			((Control)val3).set_Location(new Point(((Control)nameHeader_Label).get_Right() + 5, ((Control)idHeader_Label).get_Top()));
			((Control)val3).set_Width(labelWidth);
			val3.set_AutoSizeHeight(false);
			val3.set_WrapText(false);
			((Control)val3).set_Parent((Container)(object)IconSettingsListPanel);
			val3.set_Text("Enabled");
			val3.set_HorizontalAlignment((HorizontalAlignment)0);
			Label enabledHeader_Label = val3;
			int curY = ((Control)nameHeader_Label).get_Bottom() + 6;
			foreach (IconThingSettings iconSettings in Module.IconThingSettings)
			{
				Label val4 = new Label();
				((Control)val4).set_Location(new Point(((Control)idHeader_Label).get_Left(), curY + 6));
				((Control)val4).set_Width(labelWidth);
				val4.set_AutoSizeHeight(false);
				val4.set_WrapText(false);
				((Control)val4).set_Parent((Container)(object)IconSettingsListPanel);
				val4.set_Text($"{iconSettings.Id}: ");
				val4.set_HorizontalAlignment((HorizontalAlignment)0);
				Label val5 = new Label();
				((Control)val5).set_Location(new Point(((Control)nameHeader_Label).get_Left(), curY + 6));
				((Control)val5).set_Width(labelWidth);
				val5.set_AutoSizeHeight(false);
				val5.set_WrapText(false);
				((Control)val5).set_Parent((Container)(object)IconSettingsListPanel);
				val5.set_Text(iconSettings.Name.get_Value() ?? "");
				val5.set_HorizontalAlignment((HorizontalAlignment)0);
				Label name_Label = val5;
				Label val6 = new Label();
				((Control)val6).set_Location(new Point(((Control)enabledHeader_Label).get_Left(), curY + 6));
				((Control)val6).set_Width(labelWidth);
				val6.set_AutoSizeHeight(false);
				val6.set_WrapText(false);
				((Control)val6).set_Parent((Container)(object)IconSettingsListPanel);
				val6.set_Text(iconSettings.IsEnabled.get_Value() ? "Yes" : "No");
				val6.set_HorizontalAlignment((HorizontalAlignment)0);
				Label enabled_Label = val6;
				StandardButton val7 = new StandardButton();
				((Control)val7).set_Parent((Container)(object)IconSettingsListPanel);
				((Control)val7).set_Location(new Point(((Control)enabled_Label).get_Right(), ((Control)name_Label).get_Top()));
				val7.set_Text(Strings.Edit);
				StandardButton editRadialSettingsButton = val7;
				((Control)editRadialSettingsButton).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					currentIconSettings = iconSettings;
					BuildIconSettingsDetailPanel();
				});
				if (!iconSettings.IsDefault)
				{
					StandardButton val8 = new StandardButton();
					((Control)val8).set_Parent((Container)(object)IconSettingsListPanel);
					((Control)val8).set_Location(new Point(((Control)editRadialSettingsButton).get_Right(), ((Control)editRadialSettingsButton).get_Top()));
					val8.set_Text(Strings.Delete);
					((Control)val8).add_Click((EventHandler<MouseEventArgs>)delegate
					{
						int val10 = Module.IconThingSettings.IndexOf(iconSettings);
						Module.IconThingSettings = Module.IconThingSettings.Where((IconThingSettings ics) => ics.Id != iconSettings.Id).ToList();
						iconSettings.DeleteFromSettings(Module.settingscollection);
						Module._settingDrawIconIds.set_Value((from id in Module._settingDrawIconIds.get_Value()
							where id != iconSettings.Id
							select id).ToList());
						BuildIconSettingsListPanel();
						currentIconSettings = Module.IconThingSettings.ElementAt(Math.Min(val10, Module.IconThingSettings.Count - 1));
						BuildIconSettingsDetailPanel();
					});
				}
				curY = ((Control)name_Label).get_Bottom();
			}
			StandardButton val9 = new StandardButton();
			((Control)val9).set_Parent((Container)(object)IconSettingsListPanel);
			((Control)val9).set_Location(new Point(0, curY + 6));
			val9.set_Text(Strings.Add);
			((Control)val9).set_Enabled(Module._settingDrawIconIds.get_Value().Count <= 5);
			((Control)val9).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				int num = (from id in Module._settingDrawIconIds.get_Value()
					orderby id descending
					select id).First() + 1;
				Module.IconThingSettings.Add(new IconThingSettings(Module.settingscollection, num));
				Module._settingDrawIconIds.set_Value(Module._settingDrawIconIds.get_Value().Append(num).ToList());
				BuildIconSettingsListPanel();
				currentIconSettings = Module.IconThingSettings.Last();
				BuildIconSettingsDetailPanel();
			});
		}

		private void BuildIconSettingsDetailPanel()
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Expected O, but got Unknown
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Expected O, but got Unknown
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_015c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Unknown result type (might be due to invalid IL or missing references)
			//IL_0179: Unknown result type (might be due to invalid IL or missing references)
			//IL_0180: Unknown result type (might be due to invalid IL or missing references)
			//IL_018c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0198: Expected O, but got Unknown
			//IL_0199: Unknown result type (might be due to invalid IL or missing references)
			//IL_019e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ef: Expected O, but got Unknown
			//IL_022d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0232: Unknown result type (might be due to invalid IL or missing references)
			//IL_023c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0246: Unknown result type (might be due to invalid IL or missing references)
			//IL_0252: Unknown result type (might be due to invalid IL or missing references)
			//IL_0259: Unknown result type (might be due to invalid IL or missing references)
			//IL_0260: Unknown result type (might be due to invalid IL or missing references)
			//IL_026c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0279: Expected O, but got Unknown
			//IL_027b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0280: Unknown result type (might be due to invalid IL or missing references)
			//IL_0285: Unknown result type (might be due to invalid IL or missing references)
			//IL_028f: Unknown result type (might be due to invalid IL or missing references)
			//IL_029b: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d3: Expected O, but got Unknown
			//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0300: Unknown result type (might be due to invalid IL or missing references)
			//IL_030a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0312: Unknown result type (might be due to invalid IL or missing references)
			//IL_0319: Unknown result type (might be due to invalid IL or missing references)
			//IL_0320: Unknown result type (might be due to invalid IL or missing references)
			//IL_032c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0339: Expected O, but got Unknown
			//IL_033a: Unknown result type (might be due to invalid IL or missing references)
			//IL_033f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0352: Unknown result type (might be due to invalid IL or missing references)
			//IL_035c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0364: Unknown result type (might be due to invalid IL or missing references)
			//IL_0375: Expected O, but got Unknown
			//IL_0419: Unknown result type (might be due to invalid IL or missing references)
			//IL_041e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0429: Unknown result type (might be due to invalid IL or missing references)
			//IL_0433: Unknown result type (might be due to invalid IL or missing references)
			//IL_043b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0442: Unknown result type (might be due to invalid IL or missing references)
			//IL_0449: Unknown result type (might be due to invalid IL or missing references)
			//IL_0455: Unknown result type (might be due to invalid IL or missing references)
			//IL_0462: Expected O, but got Unknown
			//IL_0463: Unknown result type (might be due to invalid IL or missing references)
			//IL_0468: Unknown result type (might be due to invalid IL or missing references)
			//IL_0479: Unknown result type (might be due to invalid IL or missing references)
			//IL_0483: Unknown result type (might be due to invalid IL or missing references)
			//IL_048e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0499: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_04bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_04cc: Expected O, but got Unknown
			//IL_04e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_04fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0505: Unknown result type (might be due to invalid IL or missing references)
			//IL_050c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0513: Unknown result type (might be due to invalid IL or missing references)
			//IL_051f: Unknown result type (might be due to invalid IL or missing references)
			//IL_052c: Expected O, but got Unknown
			//IL_052d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0532: Unknown result type (might be due to invalid IL or missing references)
			//IL_0543: Unknown result type (might be due to invalid IL or missing references)
			//IL_054d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0558: Unknown result type (might be due to invalid IL or missing references)
			//IL_0563: Unknown result type (might be due to invalid IL or missing references)
			//IL_056e: Unknown result type (might be due to invalid IL or missing references)
			//IL_058a: Unknown result type (might be due to invalid IL or missing references)
			//IL_059b: Expected O, but got Unknown
			//IL_05b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_05cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_05df: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_05f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ff: Expected O, but got Unknown
			//IL_0600: Unknown result type (might be due to invalid IL or missing references)
			//IL_0605: Unknown result type (might be due to invalid IL or missing references)
			//IL_060a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0614: Unknown result type (might be due to invalid IL or missing references)
			//IL_0620: Unknown result type (might be due to invalid IL or missing references)
			//IL_0636: Unknown result type (might be due to invalid IL or missing references)
			//IL_0649: Unknown result type (might be due to invalid IL or missing references)
			//IL_0658: Expected O, but got Unknown
			//IL_0681: Unknown result type (might be due to invalid IL or missing references)
			((Container)IconSettingsDetailPanel).ClearChildren();
			Label val = new Label();
			((Control)val).set_Location(new Point(0, 0));
			((Control)val).set_Width(labelWidth);
			val.set_AutoSizeHeight(false);
			val.set_WrapText(false);
			((Control)val).set_Parent((Container)(object)IconSettingsDetailPanel);
			val.set_Text("Name: ");
			Label radialSettingsName_Label = val;
			int curY = 0;
			if (currentIconSettings.IsDefault)
			{
				Label val2 = new Label();
				((Control)val2).set_Location(new Point(((Control)radialSettingsName_Label).get_Right() + 5, 0));
				((Control)val2).set_Width(labelWidth);
				((Control)val2).set_Parent((Container)(object)IconSettingsDetailPanel);
				val2.set_Text(currentIconSettings.Name.get_Value() ?? "");
				curY = ((Control)val2).get_Bottom();
			}
			else
			{
				TextBox val3 = new TextBox();
				((Control)val3).set_Location(new Point(((Control)radialSettingsName_Label).get_Right() + 5, 0));
				((Control)val3).set_Width(labelWidth);
				((Control)val3).set_Parent((Container)(object)IconSettingsDetailPanel);
				((TextInputBase)val3).set_Text(currentIconSettings.Name.get_Value() ?? "");
				TextBox radialSettingsName_TextBox = val3;
				((TextInputBase)radialSettingsName_TextBox).add_TextChanged((EventHandler<EventArgs>)delegate
				{
					currentIconSettings.Name.set_Value(((TextInputBase)radialSettingsName_TextBox).get_Text());
					BuildIconSettingsListPanel();
				});
				curY = ((Control)radialSettingsName_TextBox).get_Bottom();
			}
			Label val4 = new Label();
			((Control)val4).set_Location(new Point(0, curY + 6));
			((Control)val4).set_Width(labelWidth);
			val4.set_AutoSizeHeight(false);
			val4.set_WrapText(false);
			((Control)val4).set_Parent((Container)(object)IconSettingsDetailPanel);
			val4.set_Text("Enabled");
			Label radialSettingsIsEnabled_Label = val4;
			Checkbox val5 = new Checkbox();
			((Control)val5).set_Size(new Point(20, 20));
			((Control)val5).set_Parent((Container)(object)IconSettingsDetailPanel);
			val5.set_Checked(currentIconSettings.IsEnabled.get_Value());
			((Control)val5).set_Location(new Point(((Control)radialSettingsIsEnabled_Label).get_Right() + 5, ((Control)radialSettingsIsEnabled_Label).get_Top() - 1));
			Checkbox radialSettingsIsEnabled_Checkbox = val5;
			radialSettingsIsEnabled_Checkbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				currentIconSettings.IsEnabled.set_Value(radialSettingsIsEnabled_Checkbox.get_Checked());
				BuildIconSettingsListPanel();
			});
			int nextY = ((Control)radialSettingsIsEnabled_Label).get_Bottom();
			if (currentIconSettings.IsDefault)
			{
				Label val6 = new Label();
				((Control)val6).set_Location(new Point(0, ((Control)radialSettingsIsEnabled_Label).get_Bottom() + 6));
				((Control)val6).set_Width(labelWidth);
				val6.set_AutoSizeHeight(false);
				val6.set_WrapText(false);
				((Control)val6).set_Parent((Container)(object)IconSettingsDetailPanel);
				val6.set_Text("Enable corner iccons: ");
				Label radialSettingsDisplayCornerIcons_Label = val6;
				Checkbox val7 = new Checkbox();
				((Control)val7).set_Size(new Point(20, 20));
				((Control)val7).set_Parent((Container)(object)IconSettingsDetailPanel);
				val7.set_Checked(currentIconSettings.DisplayCornerIcons.get_Value());
				((Control)val7).set_Location(new Point(((Control)radialSettingsDisplayCornerIcons_Label).get_Right() + 5, ((Control)radialSettingsDisplayCornerIcons_Label).get_Top() - 1));
				Checkbox radialSettingsDisplayCornerIcons_Checkbox = val7;
				radialSettingsDisplayCornerIcons_Checkbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
				{
					currentIconSettings.DisplayCornerIcons.set_Value(radialSettingsDisplayCornerIcons_Checkbox.get_Checked());
				});
				nextY = ((Control)radialSettingsDisplayCornerIcons_Label).get_Bottom();
			}
			Label val8 = new Label();
			((Control)val8).set_Location(new Point(0, nextY + 6));
			((Control)val8).set_Width(75);
			val8.set_AutoSizeHeight(false);
			val8.set_WrapText(false);
			((Control)val8).set_Parent((Container)(object)IconSettingsDetailPanel);
			val8.set_Text("Orientation: ");
			Label settingManualOrientation_Label = val8;
			Dropdown val9 = new Dropdown();
			((Control)val9).set_Location(new Point(((Control)settingManualOrientation_Label).get_Right() + 5, ((Control)settingManualOrientation_Label).get_Top() - 4));
			((Control)val9).set_Width(100);
			((Control)val9).set_Parent((Container)(object)IconSettingsDetailPanel);
			Dropdown settingManualOrientation_Select = val9;
			foreach (IconOrientation s in Enum.GetValues(typeof(IconOrientation)))
			{
				settingManualOrientation_Select.get_Items().Add(s.ToString());
			}
			settingManualOrientation_Select.set_SelectedItem(currentIconSettings.Orientation.get_Value().ToString());
			settingManualOrientation_Select.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				currentIconSettings.Orientation.set_Value((IconOrientation)Enum.Parse(typeof(IconOrientation), settingManualOrientation_Select.get_SelectedItem()));
			});
			Label val10 = new Label();
			((Control)val10).set_Location(new Point(0, ((Control)settingManualOrientation_Label).get_Bottom() + 6));
			((Control)val10).set_Width(75);
			val10.set_AutoSizeHeight(false);
			val10.set_WrapText(false);
			((Control)val10).set_Parent((Container)(object)IconSettingsDetailPanel);
			val10.set_Text("Icon Width: ");
			Label settingManualWidth_Label = val10;
			TrackBar val11 = new TrackBar();
			((Control)val11).set_Location(new Point(((Control)settingManualWidth_Label).get_Right() + 5, ((Control)settingManualWidth_Label).get_Top()));
			((Control)val11).set_Width(220);
			val11.set_MaxValue(200f);
			val11.set_MinValue(0f);
			val11.set_Value((float)currentIconSettings.Size.get_Value());
			((Control)val11).set_Parent((Container)(object)IconSettingsDetailPanel);
			TrackBar settingImgWidth_Slider = val11;
			settingImgWidth_Slider.add_ValueChanged((EventHandler<ValueEventArgs<float>>)delegate
			{
				currentIconSettings.Size.set_Value((int)settingImgWidth_Slider.get_Value());
			});
			Label val12 = new Label();
			((Control)val12).set_Location(new Point(0, ((Control)settingManualWidth_Label).get_Bottom() + 6));
			((Control)val12).set_Width(75);
			val12.set_AutoSizeHeight(false);
			val12.set_WrapText(false);
			((Control)val12).set_Parent((Container)(object)IconSettingsDetailPanel);
			val12.set_Text("Opacity: ");
			Label settingManualOpacity_Label = val12;
			TrackBar val13 = new TrackBar();
			((Control)val13).set_Location(new Point(((Control)settingManualOpacity_Label).get_Right() + 5, ((Control)settingManualOpacity_Label).get_Top()));
			((Control)val13).set_Width(220);
			val13.set_MaxValue(100f);
			val13.set_MinValue(0f);
			val13.set_Value(currentIconSettings.Opacity.get_Value() * 100f);
			((Control)val13).set_Parent((Container)(object)IconSettingsDetailPanel);
			TrackBar settingOpacity_Slider = val13;
			settingOpacity_Slider.add_ValueChanged((EventHandler<ValueEventArgs<float>>)delegate
			{
				currentIconSettings.Opacity.set_Value(settingOpacity_Slider.get_Value() / 100f);
			});
			Label val14 = new Label();
			((Control)val14).set_Location(new Point(0, ((Control)settingManualOpacity_Label).get_Bottom() + 6));
			((Control)val14).set_Width(labelWidth);
			val14.set_AutoSizeHeight(false);
			val14.set_WrapText(false);
			((Control)val14).set_Parent((Container)(object)IconSettingsDetailPanel);
			val14.set_Text("Drag: ");
			Label radialSettingsIsDraggingEnabled_Label = val14;
			Checkbox val15 = new Checkbox();
			((Control)val15).set_Size(new Point(20, 20));
			((Control)val15).set_Parent((Container)(object)IconSettingsDetailPanel);
			val15.set_Checked(currentIconSettings.IsDraggingEnabled.get_Value());
			((Control)val15).set_Location(new Point(((Control)radialSettingsIsDraggingEnabled_Label).get_Right() + 5, ((Control)radialSettingsIsDraggingEnabled_Label).get_Top() - 1));
			Checkbox radialSettingsIsDraggingEnabled_Checkbox = val15;
			radialSettingsIsDraggingEnabled_Checkbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				currentIconSettings.IsDraggingEnabled.set_Value(radialSettingsIsDraggingEnabled_Checkbox.get_Checked());
			});
			ThingSettingsView thingSettingsView = new ThingSettingsView(currentIconSettings);
			((Control)thingSettingsView).set_Location(new Point(500, 0));
			((Control)thingSettingsView).set_Parent((Container)(object)IconSettingsDetailPanel);
		}
	}
}
