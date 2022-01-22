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
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Expected O, but got Unknown
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Expected O, but got Unknown
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Expected O, but got Unknown
			//IL_0155: Unknown result type (might be due to invalid IL or missing references)
			//IL_015a: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_016a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Unknown result type (might be due to invalid IL or missing references)
			//IL_0174: Unknown result type (might be due to invalid IL or missing references)
			//IL_0182: Unknown result type (might be due to invalid IL or missing references)
			//IL_018f: Unknown result type (might be due to invalid IL or missing references)
			//IL_019a: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b7: Expected O, but got Unknown
			//IL_022e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0233: Unknown result type (might be due to invalid IL or missing references)
			//IL_0238: Unknown result type (might be due to invalid IL or missing references)
			//IL_0243: Unknown result type (might be due to invalid IL or missing references)
			//IL_024c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0254: Unknown result type (might be due to invalid IL or missing references)
			//IL_025c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0264: Unknown result type (might be due to invalid IL or missing references)
			//IL_0271: Expected O, but got Unknown
			//IL_0272: Unknown result type (might be due to invalid IL or missing references)
			//IL_0277: Unknown result type (might be due to invalid IL or missing references)
			//IL_0288: Unknown result type (might be due to invalid IL or missing references)
			//IL_0293: Unknown result type (might be due to invalid IL or missing references)
			//IL_029f: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ac: Expected O, but got Unknown
			//IL_0326: Unknown result type (might be due to invalid IL or missing references)
			//IL_032b: Unknown result type (might be due to invalid IL or missing references)
			//IL_033c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0347: Unknown result type (might be due to invalid IL or missing references)
			//IL_0350: Unknown result type (might be due to invalid IL or missing references)
			//IL_0358: Unknown result type (might be due to invalid IL or missing references)
			//IL_0360: Unknown result type (might be due to invalid IL or missing references)
			//IL_0368: Unknown result type (might be due to invalid IL or missing references)
			//IL_0376: Expected O, but got Unknown
			//IL_0377: Unknown result type (might be due to invalid IL or missing references)
			//IL_037c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0390: Unknown result type (might be due to invalid IL or missing references)
			//IL_039b: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d8: Expected O, but got Unknown
			//IL_03f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0405: Unknown result type (might be due to invalid IL or missing references)
			//IL_0410: Unknown result type (might be due to invalid IL or missing references)
			//IL_0419: Unknown result type (might be due to invalid IL or missing references)
			//IL_0421: Unknown result type (might be due to invalid IL or missing references)
			//IL_0429: Unknown result type (might be due to invalid IL or missing references)
			//IL_0431: Unknown result type (might be due to invalid IL or missing references)
			//IL_043f: Expected O, but got Unknown
			//IL_0440: Unknown result type (might be due to invalid IL or missing references)
			//IL_0445: Unknown result type (might be due to invalid IL or missing references)
			//IL_0456: Unknown result type (might be due to invalid IL or missing references)
			//IL_0461: Unknown result type (might be due to invalid IL or missing references)
			//IL_046d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0479: Unknown result type (might be due to invalid IL or missing references)
			//IL_0485: Unknown result type (might be due to invalid IL or missing references)
			//IL_0497: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a4: Expected O, but got Unknown
			//IL_04bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_04dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_04fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_050b: Expected O, but got Unknown
			//IL_050c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0511: Unknown result type (might be due to invalid IL or missing references)
			//IL_0522: Unknown result type (might be due to invalid IL or missing references)
			//IL_052d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0539: Unknown result type (might be due to invalid IL or missing references)
			//IL_0545: Unknown result type (might be due to invalid IL or missing references)
			//IL_0551: Unknown result type (might be due to invalid IL or missing references)
			//IL_0568: Unknown result type (might be due to invalid IL or missing references)
			//IL_0575: Expected O, but got Unknown
			//IL_059f: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_05cd: Expected O, but got Unknown
			//IL_05e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_05f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0602: Unknown result type (might be due to invalid IL or missing references)
			//IL_060d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0617: Expected O, but got Unknown
			//IL_0633: Unknown result type (might be due to invalid IL or missing references)
			//IL_0638: Unknown result type (might be due to invalid IL or missing references)
			//IL_0640: Unknown result type (might be due to invalid IL or missing references)
			//IL_064c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0657: Unknown result type (might be due to invalid IL or missing references)
			//IL_0661: Expected O, but got Unknown
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
			ViewContainer _settingOnlyCombatContainer = val15;
			_settingOnlyCombatContainer.Show(settingOnlyCombatView);
		}

		public SettingsView()
			: this()
		{
		}
	}
}
