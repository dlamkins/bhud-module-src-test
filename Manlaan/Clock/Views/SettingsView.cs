using System;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Settings;
using Blish_HUD.Settings.UI.Views;
using Microsoft.Xna.Framework;

namespace Manlaan.Clock.Views
{
	public class SettingsView : View
	{
		protected override void Build(Container buildPanel)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Expected O, but got Unknown
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Expected O, but got Unknown
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Expected O, but got Unknown
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Expected O, but got Unknown
			//IL_012d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_0160: Expected O, but got Unknown
			//IL_017c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_0189: Unknown result type (might be due to invalid IL or missing references)
			//IL_0194: Unknown result type (might be due to invalid IL or missing references)
			//IL_019f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a9: Expected O, but got Unknown
			//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01da: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f9: Expected O, but got Unknown
			//IL_0203: Unknown result type (might be due to invalid IL or missing references)
			//IL_0208: Unknown result type (might be due to invalid IL or missing references)
			//IL_0215: Unknown result type (might be due to invalid IL or missing references)
			//IL_0220: Unknown result type (might be due to invalid IL or missing references)
			//IL_0229: Unknown result type (might be due to invalid IL or missing references)
			//IL_0231: Unknown result type (might be due to invalid IL or missing references)
			//IL_0239: Unknown result type (might be due to invalid IL or missing references)
			//IL_0241: Unknown result type (might be due to invalid IL or missing references)
			//IL_024f: Expected O, but got Unknown
			//IL_0250: Unknown result type (might be due to invalid IL or missing references)
			//IL_0255: Unknown result type (might be due to invalid IL or missing references)
			//IL_0268: Unknown result type (might be due to invalid IL or missing references)
			//IL_0273: Unknown result type (might be due to invalid IL or missing references)
			//IL_027c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0289: Expected O, but got Unknown
			//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0300: Unknown result type (might be due to invalid IL or missing references)
			//IL_030b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0314: Unknown result type (might be due to invalid IL or missing references)
			//IL_031c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0324: Unknown result type (might be due to invalid IL or missing references)
			//IL_032c: Unknown result type (might be due to invalid IL or missing references)
			//IL_033a: Expected O, but got Unknown
			//IL_033b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0340: Unknown result type (might be due to invalid IL or missing references)
			//IL_0353: Unknown result type (might be due to invalid IL or missing references)
			//IL_035e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0367: Unknown result type (might be due to invalid IL or missing references)
			//IL_0374: Expected O, but got Unknown
			//IL_03d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03de: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0408: Unknown result type (might be due to invalid IL or missing references)
			//IL_0410: Unknown result type (might be due to invalid IL or missing references)
			//IL_0418: Unknown result type (might be due to invalid IL or missing references)
			//IL_0420: Unknown result type (might be due to invalid IL or missing references)
			//IL_042e: Expected O, but got Unknown
			//IL_042f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0434: Unknown result type (might be due to invalid IL or missing references)
			//IL_0447: Unknown result type (might be due to invalid IL or missing references)
			//IL_0452: Unknown result type (might be due to invalid IL or missing references)
			//IL_045b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0468: Expected O, but got Unknown
			//IL_04df: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0503: Unknown result type (might be due to invalid IL or missing references)
			//IL_050d: Expected O, but got Unknown
			Panel val = new Panel();
			val.set_CanScroll(false);
			((Control)val).set_Parent(buildPanel);
			((Control)val).set_Height(((Control)buildPanel).get_Height());
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Control)val).set_Width(700);
			Panel parentPanel = val;
			IView settingClockLocal_View = SettingView.FromType((SettingEntry)(object)Module._settingClockLocal, ((Control)buildPanel).get_Width());
			ViewContainer val2 = new ViewContainer();
			((Container)val2).set_WidthSizingMode((SizingMode)2);
			((Control)val2).set_Location(new Point(10, 10));
			((Control)val2).set_Parent((Container)(object)parentPanel);
			ViewContainer settingClockLocal_Container = val2;
			settingClockLocal_Container.Show(settingClockLocal_View);
			IView settingClockTyria_View = SettingView.FromType((SettingEntry)(object)Module._settingClockTyria, ((Control)buildPanel).get_Width());
			ViewContainer val3 = new ViewContainer();
			((Container)val3).set_WidthSizingMode((SizingMode)2);
			((Control)val3).set_Location(new Point(160, ((Control)settingClockLocal_Container).get_Location().Y));
			((Control)val3).set_Parent((Container)(object)parentPanel);
			ViewContainer settingClockTyria_Container = val3;
			settingClockTyria_Container.Show(settingClockTyria_View);
			IView settingClockServer_View = SettingView.FromType((SettingEntry)(object)Module._settingClockServer, ((Control)buildPanel).get_Width());
			ViewContainer val4 = new ViewContainer();
			((Container)val4).set_WidthSizingMode((SizingMode)2);
			((Control)val4).set_Location(new Point(310, ((Control)settingClockLocal_Container).get_Location().Y));
			((Control)val4).set_Parent((Container)(object)parentPanel);
			ViewContainer settingClockServer_Container = val4;
			settingClockServer_Container.Show(settingClockServer_View);
			IView settingClockDayNight_View = SettingView.FromType((SettingEntry)(object)Module._settingClockDayNight, ((Control)buildPanel).get_Width());
			ViewContainer val5 = new ViewContainer();
			((Container)val5).set_WidthSizingMode((SizingMode)2);
			((Control)val5).set_Location(new Point(460, ((Control)settingClockLocal_Container).get_Location().Y));
			((Control)val5).set_Parent((Container)(object)parentPanel);
			ViewContainer settingClockDayNight_Container = val5;
			settingClockDayNight_Container.Show(settingClockDayNight_View);
			IView settingClock24H_View = SettingView.FromType((SettingEntry)(object)Module._settingClock24H, ((Control)buildPanel).get_Width());
			ViewContainer val6 = new ViewContainer();
			((Container)val6).set_WidthSizingMode((SizingMode)2);
			((Control)val6).set_Location(new Point(10, ((Control)settingClockLocal_Container).get_Bottom() + 5));
			((Control)val6).set_Parent((Container)(object)parentPanel);
			ViewContainer settingClock24H_Container = val6;
			settingClock24H_Container.Show(settingClock24H_View);
			IView settingClockHideLabel_View = SettingView.FromType((SettingEntry)(object)Module._settingClockHideLabel, ((Control)buildPanel).get_Width());
			ViewContainer val7 = new ViewContainer();
			((Container)val7).set_WidthSizingMode((SizingMode)2);
			((Control)val7).set_Location(new Point(160, ((Control)settingClock24H_Container).get_Location().Y));
			((Control)val7).set_Parent((Container)(object)parentPanel);
			ViewContainer settingClockHideLabel_Container = val7;
			settingClockHideLabel_Container.Show(settingClockHideLabel_View);
			Label val8 = new Label();
			((Control)val8).set_Location(new Point(10, ((Control)settingClock24H_Container).get_Bottom() + 10));
			((Control)val8).set_Width(75);
			val8.set_AutoSizeHeight(false);
			val8.set_WrapText(false);
			((Control)val8).set_Parent((Container)(object)parentPanel);
			val8.set_Text("Font Size: ");
			Label settingClockFontSize_Label = val8;
			Dropdown val9 = new Dropdown();
			((Control)val9).set_Location(new Point(((Control)settingClockFontSize_Label).get_Right() + 8, ((Control)settingClockFontSize_Label).get_Top() - 4));
			((Control)val9).set_Width(50);
			((Control)val9).set_Parent((Container)(object)parentPanel);
			Dropdown settingClockFontSize_Select = val9;
			string[] fontSizes = Module._fontSizes;
			foreach (string s in fontSizes)
			{
				settingClockFontSize_Select.get_Items().Add(s);
			}
			settingClockFontSize_Select.set_SelectedItem(Module._settingClockFontSize.get_Value());
			settingClockFontSize_Select.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				Module._settingClockFontSize.set_Value(settingClockFontSize_Select.get_SelectedItem());
			});
			Label val10 = new Label();
			((Control)val10).set_Location(new Point(10, ((Control)settingClockFontSize_Label).get_Bottom() + 10));
			((Control)val10).set_Width(75);
			val10.set_AutoSizeHeight(false);
			val10.set_WrapText(false);
			((Control)val10).set_Parent((Container)(object)parentPanel);
			val10.set_Text("Label Align: ");
			Label settingClockLabelAlign_Label = val10;
			Dropdown val11 = new Dropdown();
			((Control)val11).set_Location(new Point(((Control)settingClockLabelAlign_Label).get_Right() + 8, ((Control)settingClockLabelAlign_Label).get_Top() - 4));
			((Control)val11).set_Width(75);
			((Control)val11).set_Parent((Container)(object)parentPanel);
			Dropdown settingClockLabelAlign_Select = val11;
			string[] fontAlign = Module._fontAlign;
			foreach (string s2 in fontAlign)
			{
				settingClockLabelAlign_Select.get_Items().Add(s2);
			}
			settingClockLabelAlign_Select.set_SelectedItem(Module._settingClockLabelAlign.get_Value());
			settingClockLabelAlign_Select.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				Module._settingClockLabelAlign.set_Value(settingClockLabelAlign_Select.get_SelectedItem());
			});
			Label val12 = new Label();
			((Control)val12).set_Location(new Point(((Control)settingClockLabelAlign_Select).get_Right() + 20, ((Control)settingClockLabelAlign_Label).get_Top()));
			((Control)val12).set_Width(75);
			val12.set_AutoSizeHeight(false);
			val12.set_WrapText(false);
			((Control)val12).set_Parent((Container)(object)parentPanel);
			val12.set_Text("Time Align: ");
			Label settingClockTimeAlign_Label = val12;
			Dropdown val13 = new Dropdown();
			((Control)val13).set_Location(new Point(((Control)settingClockTimeAlign_Label).get_Right() + 8, ((Control)settingClockTimeAlign_Label).get_Top() - 4));
			((Control)val13).set_Width(75);
			((Control)val13).set_Parent((Container)(object)parentPanel);
			Dropdown settingClockTimeAlign_Select = val13;
			string[] fontAlign2 = Module._fontAlign;
			foreach (string s3 in fontAlign2)
			{
				settingClockTimeAlign_Select.get_Items().Add(s3);
			}
			settingClockTimeAlign_Select.set_SelectedItem(Module._settingClockTimeAlign.get_Value());
			settingClockTimeAlign_Select.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				Module._settingClockTimeAlign.set_Value(settingClockTimeAlign_Select.get_SelectedItem());
			});
			IView settingClockDrag_View = SettingView.FromType((SettingEntry)(object)Module._settingClockDrag, ((Control)buildPanel).get_Width());
			ViewContainer val14 = new ViewContainer();
			((Container)val14).set_WidthSizingMode((SizingMode)2);
			((Control)val14).set_Location(new Point(10, ((Control)settingClockTimeAlign_Label).get_Bottom() + 6));
			((Control)val14).set_Parent((Container)(object)parentPanel);
			ViewContainer settingClockDrag_Container = val14;
			settingClockDrag_Container.Show(settingClockDrag_View);
		}

		public SettingsView()
			: this()
		{
		}
	}
}
