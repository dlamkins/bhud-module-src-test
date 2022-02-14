using System;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Settings;
using Blish_HUD.Settings.UI.Views;
using Microsoft.Xna.Framework;

namespace lk0001.CurrentTemplates.Views
{
	public class SettingsView : View
	{
		protected override void Build(Container buildPanel)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Expected O, but got Unknown
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Expected O, but got Unknown
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Expected O, but got Unknown
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Expected O, but got Unknown
			//IL_014e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_0164: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0176: Unknown result type (might be due to invalid IL or missing references)
			//IL_0182: Expected O, but got Unknown
			//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_020a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0214: Unknown result type (might be due to invalid IL or missing references)
			Panel val = new Panel();
			val.set_CanScroll(false);
			((Control)val).set_Parent(buildPanel);
			((Control)val).set_Height(((Control)buildPanel).get_Height());
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Control)val).set_Width(700);
			Panel parentPanel = val;
			Label val2 = new Label();
			((Control)val2).set_Location(new Point(10, 10));
			((Control)val2).set_Width(75);
			val2.set_AutoSizeHeight(false);
			val2.set_WrapText(false);
			((Control)val2).set_Parent((Container)(object)parentPanel);
			val2.set_Text("Font Size: ");
			Label settingFontSize_Label = val2;
			Dropdown val3 = new Dropdown();
			((Control)val3).set_Location(new Point(((Control)settingFontSize_Label).get_Right() + 8, ((Control)settingFontSize_Label).get_Top() - 4));
			((Control)val3).set_Width(50);
			((Control)val3).set_Parent((Container)(object)parentPanel);
			Dropdown settingFontSize_Select = val3;
			string[] fontSizes = Module._fontSizes;
			foreach (string s in fontSizes)
			{
				settingFontSize_Select.get_Items().Add(s);
			}
			settingFontSize_Select.set_SelectedItem(Module._settingFontSize.get_Value());
			settingFontSize_Select.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				Module._settingFontSize.set_Value(settingFontSize_Select.get_SelectedItem());
			});
			Label val4 = new Label();
			((Control)val4).set_Location(new Point(10, ((Control)settingFontSize_Label).get_Bottom() + 10));
			((Control)val4).set_Width(75);
			val4.set_AutoSizeHeight(false);
			val4.set_WrapText(false);
			((Control)val4).set_Parent((Container)(object)parentPanel);
			val4.set_Text("Align: ");
			Label settingAlign_Label = val4;
			Dropdown val5 = new Dropdown();
			((Control)val5).set_Location(new Point(((Control)settingAlign_Label).get_Right() + 8, ((Control)settingAlign_Label).get_Top() - 4));
			((Control)val5).set_Width(75);
			((Control)val5).set_Parent((Container)(object)parentPanel);
			Dropdown settingAlign_Select = val5;
			fontSizes = Module._fontAlign;
			foreach (string s2 in fontSizes)
			{
				settingAlign_Select.get_Items().Add(s2);
			}
			settingAlign_Select.set_SelectedItem(Module._settingAlign.get_Value());
			settingAlign_Select.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				Module._settingAlign.set_Value(settingAlign_Select.get_SelectedItem());
			});
			IView settingDrag_View = SettingView.FromType((SettingEntry)(object)Module._settingDrag, ((Control)buildPanel).get_Width());
			ViewContainer val6 = new ViewContainer();
			((Container)val6).set_WidthSizingMode((SizingMode)2);
			((Control)val6).set_Location(new Point(10, ((Control)settingAlign_Label).get_Bottom() + 6));
			((Control)val6).set_Parent((Container)(object)parentPanel);
			val6.Show(settingDrag_View);
		}

		public SettingsView()
			: this()
		{
		}
	}
}
