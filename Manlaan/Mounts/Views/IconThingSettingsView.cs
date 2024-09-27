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
		private int totalWidth = 2500;

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
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			Label val = new Label();
			((Control)val).set_Location(new Point(10, 10));
			((Control)val).set_Width(800);
			val.set_AutoSizeHeight(true);
			val.set_WrapText(true);
			((Control)val).set_Parent(buildPanel);
			val.set_TextColor(Color.get_Red());
			val.set_Font(GameService.Content.get_DefaultFont18());
			val.set_Text("These icon settings configure which actions are being displayed in the icon rows and corner icons.\nFor more info, see the documentation.".Replace(" ", "  "));
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
			currentIconSettings = Module.IconThingSettings.Single((IconThingSettings settings) => settings.IsDefault);
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
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Expected O, but got Unknown
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_0160: Unknown result type (might be due to invalid IL or missing references)
			//IL_016c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			//IL_017a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0186: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01af: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01be: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01db: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_020e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0217: Expected O, but got Unknown
			//IL_0217: Unknown result type (might be due to invalid IL or missing references)
			//IL_021c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0226: Unknown result type (might be due to invalid IL or missing references)
			//IL_0230: Unknown result type (might be due to invalid IL or missing references)
			//IL_023c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0243: Unknown result type (might be due to invalid IL or missing references)
			//IL_024a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0256: Unknown result type (might be due to invalid IL or missing references)
			//IL_027b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0284: Expected O, but got Unknown
			//IL_0284: Unknown result type (might be due to invalid IL or missing references)
			//IL_0289: Unknown result type (might be due to invalid IL or missing references)
			//IL_0295: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bb: Expected O, but got Unknown
			//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0307: Unknown result type (might be due to invalid IL or missing references)
			//IL_0348: Unknown result type (might be due to invalid IL or missing references)
			//IL_034d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0359: Unknown result type (might be due to invalid IL or missing references)
			//IL_035e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0368: Unknown result type (might be due to invalid IL or missing references)
			//IL_0373: Unknown result type (might be due to invalid IL or missing references)
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
			((Control)val3).set_BasicTooltipText("Disabled icon rows are not displayed.");
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
			//IL_0197: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Expected O, but got Unknown
			//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01da: Unknown result type (might be due to invalid IL or missing references)
			//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fa: Expected O, but got Unknown
			//IL_0238: Unknown result type (might be due to invalid IL or missing references)
			//IL_023d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0247: Unknown result type (might be due to invalid IL or missing references)
			//IL_0251: Unknown result type (might be due to invalid IL or missing references)
			//IL_025d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0264: Unknown result type (might be due to invalid IL or missing references)
			//IL_026b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0277: Unknown result type (might be due to invalid IL or missing references)
			//IL_0282: Unknown result type (might be due to invalid IL or missing references)
			//IL_028f: Expected O, but got Unknown
			//IL_0291: Unknown result type (might be due to invalid IL or missing references)
			//IL_0296: Unknown result type (might be due to invalid IL or missing references)
			//IL_029b: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02da: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e9: Expected O, but got Unknown
			//IL_030b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0310: Unknown result type (might be due to invalid IL or missing references)
			//IL_0316: Unknown result type (might be due to invalid IL or missing references)
			//IL_0320: Unknown result type (might be due to invalid IL or missing references)
			//IL_0328: Unknown result type (might be due to invalid IL or missing references)
			//IL_032f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0336: Unknown result type (might be due to invalid IL or missing references)
			//IL_0342: Unknown result type (might be due to invalid IL or missing references)
			//IL_034d: Unknown result type (might be due to invalid IL or missing references)
			//IL_035a: Expected O, but got Unknown
			//IL_035b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0360: Unknown result type (might be due to invalid IL or missing references)
			//IL_0373: Unknown result type (might be due to invalid IL or missing references)
			//IL_037d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0385: Unknown result type (might be due to invalid IL or missing references)
			//IL_0396: Expected O, but got Unknown
			//IL_043a: Unknown result type (might be due to invalid IL or missing references)
			//IL_043f: Unknown result type (might be due to invalid IL or missing references)
			//IL_044a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0454: Unknown result type (might be due to invalid IL or missing references)
			//IL_045c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0463: Unknown result type (might be due to invalid IL or missing references)
			//IL_046a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0476: Unknown result type (might be due to invalid IL or missing references)
			//IL_0481: Unknown result type (might be due to invalid IL or missing references)
			//IL_048e: Expected O, but got Unknown
			//IL_048f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0494: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_04af: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f8: Expected O, but got Unknown
			//IL_050f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0514: Unknown result type (might be due to invalid IL or missing references)
			//IL_051f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0529: Unknown result type (might be due to invalid IL or missing references)
			//IL_0531: Unknown result type (might be due to invalid IL or missing references)
			//IL_0538: Unknown result type (might be due to invalid IL or missing references)
			//IL_053f: Unknown result type (might be due to invalid IL or missing references)
			//IL_054b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0556: Unknown result type (might be due to invalid IL or missing references)
			//IL_0563: Expected O, but got Unknown
			//IL_0564: Unknown result type (might be due to invalid IL or missing references)
			//IL_0569: Unknown result type (might be due to invalid IL or missing references)
			//IL_057a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0584: Unknown result type (might be due to invalid IL or missing references)
			//IL_058f: Unknown result type (might be due to invalid IL or missing references)
			//IL_059a: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d2: Expected O, but got Unknown
			//IL_05e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_05f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0603: Unknown result type (might be due to invalid IL or missing references)
			//IL_060f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0616: Unknown result type (might be due to invalid IL or missing references)
			//IL_061d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0629: Unknown result type (might be due to invalid IL or missing references)
			//IL_0634: Unknown result type (might be due to invalid IL or missing references)
			//IL_0641: Expected O, but got Unknown
			//IL_0642: Unknown result type (might be due to invalid IL or missing references)
			//IL_0647: Unknown result type (might be due to invalid IL or missing references)
			//IL_064c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0656: Unknown result type (might be due to invalid IL or missing references)
			//IL_0662: Unknown result type (might be due to invalid IL or missing references)
			//IL_0678: Unknown result type (might be due to invalid IL or missing references)
			//IL_068b: Unknown result type (might be due to invalid IL or missing references)
			//IL_069a: Expected O, but got Unknown
			//IL_06c3: Unknown result type (might be due to invalid IL or missing references)
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
			((Control)val4).set_BasicTooltipText("Disabled icon rows are not displayed.");
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
				val6.set_Text("Enable corner icons: ");
				((Control)val6).set_BasicTooltipText("Use these actions also for corner icons, only available on the default icon settings.");
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
			((Control)val8).set_BasicTooltipText("The orientation of the icon row, either horizontal or vertical.");
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
			val10.set_Text("Icon Size: ");
			((Control)val10).set_BasicTooltipText("The icon size of actions in the row.");
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
				((Control)settingImgWidth_Slider).set_BasicTooltipText($"{currentIconSettings.Size.get_Value()}");
			});
			Label val12 = new Label();
			((Control)val12).set_Location(new Point(0, ((Control)settingManualWidth_Label).get_Bottom() + 6));
			((Control)val12).set_Width(75);
			val12.set_AutoSizeHeight(false);
			val12.set_WrapText(false);
			((Control)val12).set_Parent((Container)(object)IconSettingsDetailPanel);
			val12.set_Text("Opacity: ");
			((Control)val12).set_BasicTooltipText("The opacity of actions in the row.");
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
				((Control)settingImgWidth_Slider).set_BasicTooltipText($"{currentIconSettings.Opacity.get_Value()}");
			});
			Label val14 = new Label();
			((Control)val14).set_Location(new Point(0, ((Control)settingManualOpacity_Label).get_Bottom() + 6));
			((Control)val14).set_Width(labelWidth);
			val14.set_AutoSizeHeight(false);
			val14.set_WrapText(false);
			((Control)val14).set_Parent((Container)(object)IconSettingsDetailPanel);
			val14.set_Text("Drag: ");
			((Control)val14).set_BasicTooltipText("Option to enable repositioning the icon row using the white icon at the beginning of the row.");
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
