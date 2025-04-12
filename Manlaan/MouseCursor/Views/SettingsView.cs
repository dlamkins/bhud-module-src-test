using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Blish_HUD.Settings.UI.Views;
using Gw2Sharp.WebApi.V2.Models;
using Manlaan.MouseCursor.Models;
using Microsoft.Xna.Framework;

namespace Manlaan.MouseCursor.Views
{
	public class SettingsView : View
	{
		private Panel colorPickerPanel;

		private ColorPicker colorPicker;

		private ColorBox colorBox;

		protected override void Build(Container buildPanel)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Expected O, but got Unknown
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Expected O, but got Unknown
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Expected O, but got Unknown
			//IL_0142: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_015d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0184: Unknown result type (might be due to invalid IL or missing references)
			//IL_018b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0192: Unknown result type (might be due to invalid IL or missing references)
			//IL_019e: Expected O, but got Unknown
			//IL_020e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0213: Unknown result type (might be due to invalid IL or missing references)
			//IL_0222: Unknown result type (might be due to invalid IL or missing references)
			//IL_022a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0231: Unknown result type (might be due to invalid IL or missing references)
			//IL_0238: Unknown result type (might be due to invalid IL or missing references)
			//IL_023f: Unknown result type (might be due to invalid IL or missing references)
			//IL_024b: Expected O, but got Unknown
			//IL_024c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0251: Unknown result type (might be due to invalid IL or missing references)
			//IL_026c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0277: Unknown result type (might be due to invalid IL or missing references)
			//IL_0283: Expected O, but got Unknown
			//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0316: Unknown result type (might be due to invalid IL or missing references)
			//IL_031e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0325: Unknown result type (might be due to invalid IL or missing references)
			//IL_032c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0333: Unknown result type (might be due to invalid IL or missing references)
			//IL_0340: Expected O, but got Unknown
			//IL_0341: Unknown result type (might be due to invalid IL or missing references)
			//IL_0346: Unknown result type (might be due to invalid IL or missing references)
			//IL_0364: Unknown result type (might be due to invalid IL or missing references)
			//IL_036b: Unknown result type (might be due to invalid IL or missing references)
			//IL_039f: Expected O, but got Unknown
			//IL_03b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_03eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ff: Expected O, but got Unknown
			//IL_0400: Unknown result type (might be due to invalid IL or missing references)
			//IL_0405: Unknown result type (might be due to invalid IL or missing references)
			//IL_0420: Unknown result type (might be due to invalid IL or missing references)
			//IL_042b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0436: Unknown result type (might be due to invalid IL or missing references)
			//IL_0441: Unknown result type (might be due to invalid IL or missing references)
			//IL_0452: Unknown result type (might be due to invalid IL or missing references)
			//IL_045e: Expected O, but got Unknown
			//IL_0475: Unknown result type (might be due to invalid IL or missing references)
			//IL_047a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0494: Unknown result type (might be due to invalid IL or missing references)
			//IL_049c: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_04aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_04be: Expected O, but got Unknown
			//IL_04bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_04df: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0500: Unknown result type (might be due to invalid IL or missing references)
			//IL_0516: Unknown result type (might be due to invalid IL or missing references)
			//IL_0522: Expected O, but got Unknown
			//IL_054b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0550: Unknown result type (might be due to invalid IL or missing references)
			//IL_0557: Unknown result type (might be due to invalid IL or missing references)
			//IL_056d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0576: Expected O, but got Unknown
			//IL_0591: Unknown result type (might be due to invalid IL or missing references)
			//IL_0596: Unknown result type (might be due to invalid IL or missing references)
			//IL_059d: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_05bc: Expected O, but got Unknown
			//IL_05d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_05dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_05f9: Unknown result type (might be due to invalid IL or missing references)
			Panel val = new Panel();
			val.set_CanScroll(false);
			((Control)val).set_Parent(buildPanel);
			((Control)val).set_Height(((Control)buildPanel).get_Height());
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Control)val).set_Width(700);
			Panel parentPanel = val;
			((Control)parentPanel).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)delegate
			{
				if (((Control)colorPickerPanel).get_Visible() && !((Control)colorPickerPanel).get_MouseOver() && !((Control)colorBox).get_MouseOver())
				{
					((Control)colorPickerPanel).set_Visible(false);
				}
			});
			Panel val2 = new Panel();
			((Control)val2).set_Location(new Point(((Control)parentPanel).get_Width() - 420 - 10, 10));
			((Control)val2).set_Size(new Point(420, 255));
			((Control)val2).set_Visible(false);
			((Control)val2).set_ZIndex(10);
			((Control)val2).set_Parent((Container)(object)parentPanel);
			val2.set_BackgroundTexture(AsyncTexture2D.op_Implicit(Module.ModuleInstance.ContentsManager.GetTexture("155976.png")));
			val2.set_ShowBorder(false);
			colorPickerPanel = val2;
			Panel val3 = new Panel();
			((Control)val3).set_Location(new Point(15, 15));
			((Control)val3).set_Size(new Point(((Control)colorPickerPanel).get_Size().X - 35, ((Control)colorPickerPanel).get_Size().Y - 30));
			((Control)val3).set_Parent((Container)(object)colorPickerPanel);
			val3.set_BackgroundTexture(AsyncTexture2D.op_Implicit(Module.ModuleInstance.ContentsManager.GetTexture("buttondark.png")));
			val3.set_ShowBorder(true);
			Panel colorPickerBG = val3;
			ColorPicker val4 = new ColorPicker();
			((Control)val4).set_Location(new Point(10, 10));
			((Panel)val4).set_CanScroll(false);
			((Control)val4).set_Size(new Point(((Control)colorPickerBG).get_Size().X - 20, ((Control)colorPickerBG).get_Size().Y - 20));
			((Control)val4).set_Parent((Container)(object)colorPickerBG);
			((Panel)val4).set_ShowTint(false);
			((Control)val4).set_Visible(true);
			colorPicker = val4;
			colorPicker.add_SelectedColorChanged((EventHandler<EventArgs>)delegate
			{
				colorBox.set_Color(colorPicker.get_SelectedColor());
				Module._settingMouseCursorColor.set_Value(colorPicker.get_SelectedColor().get_Name());
				((Control)colorPickerPanel).set_Visible(false);
			});
			((Control)colorPicker).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)delegate
			{
				((Control)colorPickerPanel).set_Visible(false);
			});
			foreach (Color color in Module._colors)
			{
				colorPicker.get_Colors().Add(color);
			}
			Label val5 = new Label();
			((Control)val5).set_Location(new Point(10, 15));
			((Control)val5).set_Width(60);
			val5.set_AutoSizeHeight(false);
			val5.set_WrapText(false);
			((Control)val5).set_Parent((Container)(object)parentPanel);
			val5.set_Text("Cursor: ");
			Label cursorLabel = val5;
			Dropdown val6 = new Dropdown();
			((Control)val6).set_Location(new Point(((Control)cursorLabel).get_Right() + 8, ((Control)cursorLabel).get_Top() - 5));
			((Control)val6).set_Width(175);
			((Control)val6).set_Parent((Container)(object)parentPanel);
			Dropdown cursorSelect = val6;
			foreach (MouseFile s in Module._mouseFiles)
			{
				cursorSelect.get_Items().Add(s.Name);
			}
			cursorSelect.set_SelectedItem(Module._settingMouseCursorImage.get_Value());
			cursorSelect.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				Module._settingMouseCursorImage.set_Value(cursorSelect.get_SelectedItem());
			});
			Label val7 = new Label();
			((Control)val7).set_Location(new Point(10, ((Control)cursorSelect).get_Bottom() + 15));
			((Control)val7).set_Width(60);
			val7.set_AutoSizeHeight(false);
			val7.set_WrapText(false);
			((Control)val7).set_Parent((Container)(object)parentPanel);
			val7.set_Text("Tint: ");
			Label colorLabel = val7;
			ColorBox val8 = new ColorBox();
			((Control)val8).set_Location(new Point(((Control)colorLabel).get_Right() + 8, ((Control)colorLabel).get_Top() - 10));
			((Control)val8).set_Parent((Container)(object)parentPanel);
			val8.set_Color(Module._colors.Find((Color x) => x.get_Name().Equals(Module._settingMouseCursorColor.get_Value())));
			colorBox = val8;
			((Control)colorBox).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				((Control)colorPickerPanel).set_Visible(!((Control)colorPickerPanel).get_Visible());
			});
			Label val9 = new Label();
			((Control)val9).set_Location(new Point(10, ((Control)colorBox).get_Bottom() + 5));
			((Control)val9).set_Width(60);
			val9.set_AutoSizeHeight(false);
			val9.set_WrapText(false);
			((Control)val9).set_Parent((Container)(object)parentPanel);
			val9.set_Text("Size: ");
			Label sizeLabel = val9;
			TrackBar val10 = new TrackBar();
			((Control)val10).set_Location(new Point(((Control)sizeLabel).get_Right() + 8, ((Control)sizeLabel).get_Top()));
			((Control)val10).set_Width(250);
			val10.set_MaxValue(250f);
			val10.set_MinValue(0f);
			val10.set_Value((float)Module._settingMouseCursorSize.get_Value());
			((Control)val10).set_Parent((Container)(object)parentPanel);
			TrackBar sizeSlider = val10;
			sizeSlider.add_ValueChanged((EventHandler<ValueEventArgs<float>>)delegate
			{
				Module._settingMouseCursorSize.set_Value((int)sizeSlider.get_Value());
			});
			Label val11 = new Label();
			((Control)val11).set_Location(new Point(10, ((Control)sizeSlider).get_Bottom() + 7));
			((Control)val11).set_Width(60);
			val11.set_AutoSizeHeight(false);
			val11.set_WrapText(false);
			((Control)val11).set_Parent((Container)(object)parentPanel);
			val11.set_Text("Opacity: ");
			Label opacityLabel = val11;
			TrackBar val12 = new TrackBar();
			((Control)val12).set_Location(new Point(((Control)opacityLabel).get_Right() + 8, ((Control)opacityLabel).get_Top()));
			((Control)val12).set_Width(250);
			val12.set_MaxValue(100f);
			val12.set_MinValue(0f);
			val12.set_Value(Module._settingMouseCursorOpacity.get_Value() * 100f);
			((Control)val12).set_Parent((Container)(object)parentPanel);
			TrackBar opacitySlider = val12;
			opacitySlider.add_ValueChanged((EventHandler<ValueEventArgs<float>>)delegate
			{
				Module._settingMouseCursorOpacity.set_Value(opacitySlider.get_Value() / 100f);
			});
			IView settingCameraDragView = SettingView.FromType((SettingEntry)(object)Module._settingMouseCursorCameraDrag, ((Control)buildPanel).get_Width());
			ViewContainer val13 = new ViewContainer();
			((Container)val13).set_WidthSizingMode((SizingMode)2);
			((Control)val13).set_Location(new Point(10, ((Control)opacityLabel).get_Bottom() + 5));
			((Control)val13).set_Parent((Container)(object)parentPanel);
			ViewContainer _settingCameraDragContainer = val13;
			_settingCameraDragContainer.Show(settingCameraDragView);
			IView settingAboveBlishView = SettingView.FromType((SettingEntry)(object)Module._settingMouseCursorAboveBlish, ((Control)buildPanel).get_Width());
			ViewContainer val14 = new ViewContainer();
			((Container)val14).set_WidthSizingMode((SizingMode)2);
			((Control)val14).set_Location(new Point(10, ((Control)_settingCameraDragContainer).get_Bottom() + 5));
			((Control)val14).set_Parent((Container)(object)parentPanel);
			ViewContainer _settingAboveBlishContainer = val14;
			_settingAboveBlishContainer.Show(settingAboveBlishView);
			IView settingOnlyCombatView = SettingView.FromType((SettingEntry)(object)Module._settingMouseCursorOnlyCombat, ((Control)buildPanel).get_Width());
			ViewContainer val15 = new ViewContainer();
			((Container)val15).set_WidthSizingMode((SizingMode)2);
			((Control)val15).set_Location(new Point(10, ((Control)_settingAboveBlishContainer).get_Bottom() + 5));
			((Control)val15).set_Parent((Container)(object)parentPanel);
			val15.Show(settingOnlyCombatView);
		}

		public SettingsView()
			: this()
		{
		}
	}
}
