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
		private Label settingBuildPadPath_Warning;

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
			//IL_021d: Expected O, but got Unknown
			//IL_0238: Unknown result type (might be due to invalid IL or missing references)
			//IL_023d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0244: Unknown result type (might be due to invalid IL or missing references)
			//IL_0250: Unknown result type (might be due to invalid IL or missing references)
			//IL_025a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0263: Expected O, but got Unknown
			//IL_027e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0283: Unknown result type (might be due to invalid IL or missing references)
			//IL_028a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0296: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a9: Expected O, but got Unknown
			//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02de: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0307: Expected O, but got Unknown
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
			ViewContainer settingDrag_Container = val6;
			settingDrag_Container.Show(settingDrag_View);
			IView settingBuildPad_View = SettingView.FromType((SettingEntry)(object)Module._settingBuildPad, ((Control)buildPanel).get_Width());
			ViewContainer val7 = new ViewContainer();
			((Container)val7).set_WidthSizingMode((SizingMode)2);
			((Control)val7).set_Location(new Point(10, ((Control)settingDrag_Container).get_Bottom() + 6));
			((Control)val7).set_Parent((Container)(object)parentPanel);
			ViewContainer settingBuildPad_Container = val7;
			settingBuildPad_Container.Show(settingBuildPad_View);
			IView settingBuildPadPath_View = SettingView.FromType((SettingEntry)(object)Module._settingBuildPadPath, ((Control)buildPanel).get_Width());
			ViewContainer val8 = new ViewContainer();
			((Container)val8).set_WidthSizingMode((SizingMode)2);
			((Control)val8).set_Location(new Point(10, ((Control)settingBuildPad_Container).get_Bottom() + 6));
			((Control)val8).set_Parent((Container)(object)parentPanel);
			ViewContainer settingBuildPadPath_Container = val8;
			settingBuildPadPath_Container.Show(settingBuildPadPath_View);
			Label val9 = new Label();
			((Control)val9).set_Location(new Point(10, ((Control)settingBuildPadPath_Container).get_Bottom() + 10));
			((Control)val9).set_Width(100);
			val9.set_AutoSizeHeight(false);
			val9.set_WrapText(false);
			((Control)val9).set_Parent((Container)(object)parentPanel);
			val9.set_Text("Incorrect path");
			val9.set_TextColor(Color.get_Red());
			settingBuildPadPath_Warning = val9;
			ToggleIncorrectPathWarning();
		}

		public void ToggleIncorrectPathWarning()
		{
			if (Module._hasBuildPad)
			{
				Label obj = settingBuildPadPath_Warning;
				if (obj != null)
				{
					((Control)obj).Hide();
				}
			}
			else
			{
				Label obj2 = settingBuildPadPath_Warning;
				if (obj2 != null)
				{
					((Control)obj2).Show();
				}
			}
		}

		public SettingsView()
			: this()
		{
		}
	}
}
