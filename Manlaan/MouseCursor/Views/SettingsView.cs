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

		private static readonly Logger Logger = Logger.GetLogger<SettingsView>();

		protected override void Build(Container buildPanel)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Expected O, but got Unknown
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Expected O, but got Unknown
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Expected O, but got Unknown
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0182: Unknown result type (might be due to invalid IL or missing references)
			//IL_0189: Unknown result type (might be due to invalid IL or missing references)
			//IL_0190: Unknown result type (might be due to invalid IL or missing references)
			//IL_019c: Expected O, but got Unknown
			//IL_020c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0211: Unknown result type (might be due to invalid IL or missing references)
			//IL_0220: Unknown result type (might be due to invalid IL or missing references)
			//IL_0228: Unknown result type (might be due to invalid IL or missing references)
			//IL_022f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0236: Unknown result type (might be due to invalid IL or missing references)
			//IL_023d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0249: Expected O, but got Unknown
			//IL_024a: Unknown result type (might be due to invalid IL or missing references)
			//IL_024f: Unknown result type (might be due to invalid IL or missing references)
			//IL_026a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0275: Unknown result type (might be due to invalid IL or missing references)
			//IL_0281: Expected O, but got Unknown
			//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_031d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0324: Unknown result type (might be due to invalid IL or missing references)
			//IL_0346: Unknown result type (might be due to invalid IL or missing references)
			//IL_037a: Expected O, but got Unknown
			//IL_0394: Unknown result type (might be due to invalid IL or missing references)
			//IL_0399: Unknown result type (might be due to invalid IL or missing references)
			//IL_03af: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_03be: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d9: Expected O, but got Unknown
			//IL_03da: Unknown result type (might be due to invalid IL or missing references)
			//IL_03df: Unknown result type (might be due to invalid IL or missing references)
			//IL_03fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0407: Unknown result type (might be due to invalid IL or missing references)
			//IL_0412: Unknown result type (might be due to invalid IL or missing references)
			//IL_041d: Unknown result type (might be due to invalid IL or missing references)
			//IL_042e: Unknown result type (might be due to invalid IL or missing references)
			//IL_043a: Expected O, but got Unknown
			//IL_0459: Unknown result type (might be due to invalid IL or missing references)
			//IL_045e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0474: Unknown result type (might be due to invalid IL or missing references)
			//IL_047c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0483: Unknown result type (might be due to invalid IL or missing references)
			//IL_048a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0491: Unknown result type (might be due to invalid IL or missing references)
			//IL_049e: Expected O, but got Unknown
			//IL_049f: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_04cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0504: Expected O, but got Unknown
			//IL_051f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0524: Unknown result type (might be due to invalid IL or missing references)
			//IL_052b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0541: Unknown result type (might be due to invalid IL or missing references)
			//IL_054a: Expected O, but got Unknown
			//IL_056a: Unknown result type (might be due to invalid IL or missing references)
			//IL_056f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0585: Unknown result type (might be due to invalid IL or missing references)
			//IL_058c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0593: Unknown result type (might be due to invalid IL or missing references)
			//IL_059a: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_05af: Expected O, but got Unknown
			//IL_05af: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_05f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_05fc: Expected O, but got Unknown
			//IL_05fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0601: Unknown result type (might be due to invalid IL or missing references)
			//IL_061f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0626: Unknown result type (might be due to invalid IL or missing references)
			//IL_062d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0634: Unknown result type (might be due to invalid IL or missing references)
			//IL_063f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0649: Expected O, but got Unknown
			//IL_064d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0652: Unknown result type (might be due to invalid IL or missing references)
			//IL_066e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0675: Unknown result type (might be due to invalid IL or missing references)
			//IL_067c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0683: Unknown result type (might be due to invalid IL or missing references)
			//IL_068e: Unknown result type (might be due to invalid IL or missing references)
			//IL_069d: Expected O, but got Unknown
			//IL_069e: Unknown result type (might be due to invalid IL or missing references)
			//IL_06a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_06be: Unknown result type (might be due to invalid IL or missing references)
			//IL_06cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_06d7: Expected O, but got Unknown
			//IL_0755: Unknown result type (might be due to invalid IL or missing references)
			//IL_075a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0775: Unknown result type (might be due to invalid IL or missing references)
			//IL_0782: Unknown result type (might be due to invalid IL or missing references)
			//IL_078e: Expected O, but got Unknown
			//IL_080f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0814: Unknown result type (might be due to invalid IL or missing references)
			//IL_0830: Unknown result type (might be due to invalid IL or missing references)
			//IL_0837: Unknown result type (might be due to invalid IL or missing references)
			//IL_083e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0845: Unknown result type (might be due to invalid IL or missing references)
			//IL_0850: Unknown result type (might be due to invalid IL or missing references)
			//IL_085f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0864: Unknown result type (might be due to invalid IL or missing references)
			//IL_0887: Unknown result type (might be due to invalid IL or missing references)
			//IL_0898: Unknown result type (might be due to invalid IL or missing references)
			//IL_08a4: Expected O, but got Unknown
			//IL_0922: Unknown result type (might be due to invalid IL or missing references)
			//IL_0927: Unknown result type (might be due to invalid IL or missing references)
			//IL_094a: Unknown result type (might be due to invalid IL or missing references)
			//IL_095b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0967: Expected O, but got Unknown
			//IL_09ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_09f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a07: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a12: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a1b: Expected O, but got Unknown
			//IL_0a38: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a3d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a5a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a65: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a70: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a7b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a8b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0aaa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0aba: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ac6: Expected O, but got Unknown
			Panel val = new Panel();
			val.set_CanScroll(false);
			((Control)val).set_Parent(buildPanel);
			((Control)val).set_Height(((Control)buildPanel).get_Height());
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Control)val).set_Width(((Control)buildPanel).get_Width());
			Panel parentPanel = val;
			((Control)parentPanel).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)delegate
			{
				if (((Control)colorPickerPanel).get_Visible() && !((Control)colorPickerPanel).get_MouseOver() && !((Control)colorBox).get_MouseOver())
				{
					((Control)colorPickerPanel).set_Visible(false);
				}
			});
			Panel val2 = new Panel();
			((Control)val2).set_Location(new Point(((Control)parentPanel).get_Width() - 420, 10));
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
			val5.set_Text("Image: ");
			Label cursorLabel = val5;
			Dropdown val6 = new Dropdown();
			((Control)val6).set_Location(new Point(((Control)cursorLabel).get_Right() + 5, ((Control)cursorLabel).get_Top() - 5));
			((Control)val6).set_Width(175);
			((Control)val6).set_Parent((Container)(object)parentPanel);
			Dropdown cursorSelect = val6;
			foreach (MouseFile s6 in Module._mouseFiles)
			{
				cursorSelect.get_Items().Add(s6.Name);
			}
			cursorSelect.set_SelectedItem(Module._settingMouseCursorImage.get_Value());
			cursorSelect.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				Module._settingMouseCursorImage.set_Value(cursorSelect.get_SelectedItem());
			});
			ColorBox val7 = new ColorBox();
			((Control)val7).set_Location(new Point(((Control)cursorSelect).get_Right() + 5, ((Control)cursorSelect).get_Top()));
			((Control)val7).set_Parent((Container)(object)parentPanel);
			((Control)val7).set_Size(new Point(((Control)cursorSelect).get_Bottom() - ((Control)cursorSelect).get_Top()));
			val7.set_Color(Module._colors.Find((Color x) => x.get_Name().Equals(Module._settingMouseCursorColor.get_Value())));
			colorBox = val7;
			((Control)colorBox).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				((Control)colorPickerPanel).set_Visible(!((Control)colorPickerPanel).get_Visible());
			});
			Control prevContainer = (Control)(object)cursorLabel;
			Label val8 = new Label();
			((Control)val8).set_Location(new Point(10, prevContainer.get_Bottom() + 5));
			((Control)val8).set_Width(60);
			val8.set_AutoSizeHeight(false);
			val8.set_WrapText(false);
			((Control)val8).set_Parent((Container)(object)parentPanel);
			val8.set_Text("Size:");
			Label sizeLabel = val8;
			TrackBar val9 = new TrackBar();
			((Control)val9).set_Location(new Point(((Control)sizeLabel).get_Right() + 5, ((Control)sizeLabel).get_Top() + 2));
			((Control)val9).set_Width(250);
			val9.set_MaxValue(250f);
			val9.set_MinValue(0f);
			val9.set_Value((float)Module._settingMouseCursorSize.get_Value());
			((Control)val9).set_Parent((Container)(object)parentPanel);
			TrackBar sizeSlider = val9;
			sizeSlider.add_ValueChanged((EventHandler<ValueEventArgs<float>>)delegate
			{
				Module._settingMouseCursorSize.set_Value((int)sizeSlider.get_Value());
			});
			prevContainer = (Control)(object)sizeSlider;
			Label val10 = new Label();
			((Control)val10).set_Location(new Point(10, prevContainer.get_Bottom() + 5));
			((Control)val10).set_Width(60);
			val10.set_AutoSizeHeight(false);
			val10.set_WrapText(false);
			((Control)val10).set_Parent((Container)(object)parentPanel);
			val10.set_Text("Opacity:");
			Label opacityLabel = val10;
			TrackBar val11 = new TrackBar();
			((Control)val11).set_Location(new Point(((Control)opacityLabel).get_Right() + 5, ((Control)opacityLabel).get_Top() + 2));
			((Control)val11).set_Width(250);
			val11.set_MaxValue(100f);
			val11.set_MinValue(0f);
			val11.set_Value(Module._settingMouseCursorOpacity.get_Value() * 100f);
			((Control)val11).set_Parent((Container)(object)parentPanel);
			TrackBar opacitySlider = val11;
			opacitySlider.add_ValueChanged((EventHandler<ValueEventArgs<float>>)delegate
			{
				Module._settingMouseCursorOpacity.set_Value(opacitySlider.get_Value() / 100f);
			});
			prevContainer = (Control)(object)opacityLabel;
			ViewContainer val12 = new ViewContainer();
			((Container)val12).set_WidthSizingMode((SizingMode)2);
			((Control)val12).set_Location(new Point(10, prevContainer.get_Bottom() + 5));
			((Control)val12).set_Parent((Container)(object)parentPanel);
			ViewContainer _settingAboveBlishContainer = val12;
			IView settingAboveBlishView = SettingView.FromType((SettingEntry)(object)Module._settingMouseCursorAboveBlish, ((Control)_settingAboveBlishContainer).get_Width());
			_settingAboveBlishContainer.Show(settingAboveBlishView);
			prevContainer = (Control)(object)_settingAboveBlishContainer;
			Label val13 = new Label();
			((Control)val13).set_Location(new Point(10, prevContainer.get_Bottom() + 5));
			val13.set_AutoSizeHeight(false);
			val13.set_WrapText(false);
			((Control)val13).set_Parent((Container)(object)parentPanel);
			val13.set_Text("");
			((Control)val13).set_Width(85);
			Label cursorClipShowHeader0Label = val13;
			Label val14 = new Label();
			((Control)val14).set_Location(new Point(((Control)cursorClipShowHeader0Label).get_Right() + 5, prevContainer.get_Bottom() + 10));
			val14.set_AutoSizeHeight(false);
			val14.set_WrapText(false);
			((Control)val14).set_Parent((Container)(object)parentPanel);
			val14.set_Text("Out of Combat");
			((Control)val14).set_Width(100);
			Label cursorClipShowHeaderLabel = val14;
			Label val15 = new Label();
			((Control)val15).set_Location(new Point(((Control)cursorClipShowHeaderLabel).get_Right() + 5, prevContainer.get_Bottom() + 10));
			val15.set_AutoSizeHeight(false);
			val15.set_WrapText(false);
			((Control)val15).set_Parent((Container)(object)parentPanel);
			val15.set_Text("In Combat");
			((Control)val15).set_Width(100);
			Label cursorClipShowHeaderCombatLabel = val15;
			prevContainer = (Control)(object)cursorClipShowHeader0Label;
			Label val16 = new Label();
			((Control)val16).set_Location(new Point(prevContainer.get_Left(), prevContainer.get_Bottom() + 10));
			val16.set_AutoSizeHeight(false);
			val16.set_WrapText(false);
			((Control)val16).set_Parent((Container)(object)parentPanel);
			val16.set_Text("Show Image:");
			((Control)val16).set_Width(prevContainer.get_Width());
			Label cursorShowLabel = val16;
			Dropdown val17 = new Dropdown();
			((Control)val17).set_Location(new Point(((Control)cursorClipShowHeaderLabel).get_Left(), ((Control)cursorClipShowHeaderLabel).get_Bottom() + 5));
			((Control)val17).set_Width(((Control)cursorClipShowHeaderLabel).get_Width());
			((Control)val17).set_Parent((Container)(object)parentPanel);
			Dropdown cursorShowSelect = val17;
			string[] names = Enum.GetNames(typeof(Module.ShowMode));
			foreach (string s2 in names)
			{
				cursorShowSelect.get_Items().Add(s2);
			}
			cursorShowSelect.set_SelectedItem(Enum.GetName(typeof(Module.ShowMode), Module._settingMouseCursorShow.get_Value()));
			cursorShowSelect.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				Enum.TryParse<Module.ShowMode>(cursorShowSelect.get_SelectedItem(), out var result4);
				Module._settingMouseCursorShow.set_Value(result4);
			});
			Dropdown val18 = new Dropdown();
			((Control)val18).set_Location(new Point(((Control)cursorClipShowHeaderCombatLabel).get_Left(), ((Control)cursorClipShowHeaderCombatLabel).get_Bottom() + 5));
			((Control)val18).set_Width(((Control)cursorClipShowHeaderCombatLabel).get_Width());
			((Control)val18).set_Parent((Container)(object)parentPanel);
			Dropdown cursorShowCombatSelect = val18;
			names = Enum.GetNames(typeof(Module.ShowMode));
			foreach (string s3 in names)
			{
				cursorShowCombatSelect.get_Items().Add(s3);
			}
			cursorShowCombatSelect.set_SelectedItem(Enum.GetName(typeof(Module.ShowMode), Module._settingMouseCursorShowCombat.get_Value()));
			cursorShowCombatSelect.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				Enum.TryParse<Module.ShowMode>(cursorShowCombatSelect.get_SelectedItem(), out var result3);
				Module._settingMouseCursorShowCombat.set_Value(result3);
			});
			prevContainer = (Control)(object)cursorShowLabel;
			Label val19 = new Label();
			((Control)val19).set_Location(new Point(prevContainer.get_Left(), prevContainer.get_Bottom() + 10));
			val19.set_AutoSizeHeight(false);
			val19.set_WrapText(false);
			((Control)val19).set_Parent((Container)(object)parentPanel);
			val19.set_Text("Clip Cursor:");
			((Control)val19).set_Width(prevContainer.get_Width());
			Dropdown val20 = new Dropdown();
			((Control)val20).set_Location(new Point(((Control)cursorShowSelect).get_Left(), ((Control)cursorShowSelect).get_Bottom() + 5));
			((Control)val20).set_Width(((Control)cursorShowSelect).get_Width());
			((Control)val20).set_Parent((Container)(object)parentPanel);
			Dropdown cursorClipSelect = val20;
			names = Enum.GetNames(typeof(Module.ClipMode));
			foreach (string s4 in names)
			{
				cursorClipSelect.get_Items().Add(s4);
			}
			cursorClipSelect.set_SelectedItem(Enum.GetName(typeof(Module.ClipMode), Module._settingMouseCursorClip.get_Value()));
			cursorClipSelect.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				Enum.TryParse<Module.ClipMode>(cursorClipSelect.get_SelectedItem(), out var result2);
				Module._settingMouseCursorClip.set_Value(result2);
			});
			Dropdown val21 = new Dropdown();
			((Control)val21).set_Location(new Point(((Control)cursorShowCombatSelect).get_Left(), ((Control)cursorShowCombatSelect).get_Bottom() + 5));
			((Control)val21).set_Width(((Control)cursorShowCombatSelect).get_Width());
			((Control)val21).set_Parent((Container)(object)parentPanel);
			Dropdown cursorClipCombatSelect = val21;
			names = Enum.GetNames(typeof(Module.ClipMode));
			foreach (string s5 in names)
			{
				cursorClipCombatSelect.get_Items().Add(s5);
			}
			cursorClipCombatSelect.set_SelectedItem(Enum.GetName(typeof(Module.ClipMode), Module._settingMouseCursorClipCombat.get_Value()));
			cursorClipCombatSelect.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				Enum.TryParse<Module.ClipMode>(cursorClipCombatSelect.get_SelectedItem(), out var result);
				Module._settingMouseCursorClipCombat.set_Value(result);
			});
			prevContainer = (Control)(object)cursorClipCombatSelect;
			ViewContainer val22 = new ViewContainer();
			((Control)val22).set_Location(new Point(10, prevContainer.get_Bottom() + 5));
			((Control)val22).set_Width(210);
			((Control)val22).set_Parent((Container)(object)parentPanel);
			ViewContainer _settingFreezeCursorContainer = val22;
			IView settingFreezeCursorView = SettingView.FromType((SettingEntry)(object)Module._settingMouseCursorFreezeCursor, ((Control)_settingFreezeCursorContainer).get_Width());
			_settingFreezeCursorContainer.Show(settingFreezeCursorView);
			TrackBar val23 = new TrackBar();
			((Control)val23).set_Location(new Point(((Control)_settingFreezeCursorContainer).get_Right() + 5, ((Control)_settingFreezeCursorContainer).get_Top() + 5));
			((Control)val23).set_Width(250);
			val23.set_MinValue(1f);
			val23.set_MaxValue(500f);
			val23.set_Value(Module._settingMouseCursorFreezeCursorPeriod.get_Value());
			((Control)val23).set_BasicTooltipText($"{Module._settingMouseCursorFreezeCursorPeriod.get_Value():0} ms");
			((Control)val23).set_Visible(Module._settingMouseCursorFreezeCursor.get_Value());
			((Control)val23).set_Parent((Container)(object)parentPanel);
			TrackBar freezeCursorPeriodSlider = val23;
			freezeCursorPeriodSlider.add_ValueChanged((EventHandler<ValueEventArgs<float>>)delegate
			{
				Module._settingMouseCursorFreezeCursorPeriod.set_Value(freezeCursorPeriodSlider.get_Value());
				((Control)freezeCursorPeriodSlider).set_BasicTooltipText($"{freezeCursorPeriodSlider.get_Value():0} ms");
			});
			((SettingView<bool>)(object)((settingFreezeCursorView is BoolSettingView) ? settingFreezeCursorView : null)).add_ValueChanged((EventHandler<ValueEventArgs<bool>>)delegate
			{
				((Control)freezeCursorPeriodSlider).set_Visible(Module._settingMouseCursorFreezeCursor.get_Value());
			});
			prevContainer = (Control)(object)freezeCursorPeriodSlider;
		}

		public SettingsView()
			: this()
		{
		}
	}
}
