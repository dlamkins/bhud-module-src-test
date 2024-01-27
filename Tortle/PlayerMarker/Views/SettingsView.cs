using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tortle.PlayerMarker.Localization;
using Tortle.PlayerMarker.Models;
using Tortle.PlayerMarker.Services;

namespace Tortle.PlayerMarker.Views
{
	internal class SettingsView : View, IDisposable
	{
		private const int SliderWidth = 175;

		private static readonly Logger Logger = Logger.GetLogger(typeof(SettingsView));

		private readonly MarkerTextureManager _markerTextureManager;

		private readonly Tortle.PlayerMarker.Services.ModuleSettings _moduleSettings;

		private readonly ContentsManager _contentsManager;

		private Texture2D _panelBackgroundTexture;

		private Texture2D _buttonDarkTexture;

		private readonly Point _topLeft = new Point(10, 10);

		public SettingsView(MarkerTextureManager markerTextureManager, Tortle.PlayerMarker.Services.ModuleSettings moduleSettings, ContentsManager contentsManager)
			: this()
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			_markerTextureManager = markerTextureManager;
			_moduleSettings = moduleSettings;
			_contentsManager = contentsManager;
		}

		protected override void Build(Container buildPanel)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Expected O, but got Unknown
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Expected O, but got Unknown
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0169: Unknown result type (might be due to invalid IL or missing references)
			//IL_0170: Unknown result type (might be due to invalid IL or missing references)
			//IL_018c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0193: Unknown result type (might be due to invalid IL or missing references)
			//IL_019a: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a7: Expected O, but got Unknown
			//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_020e: Expected O, but got Unknown
			//IL_020f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0214: Unknown result type (might be due to invalid IL or missing references)
			//IL_0215: Unknown result type (might be due to invalid IL or missing references)
			//IL_0228: Unknown result type (might be due to invalid IL or missing references)
			//IL_0232: Unknown result type (might be due to invalid IL or missing references)
			//IL_023d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0249: Expected O, but got Unknown
			//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0302: Unknown result type (might be due to invalid IL or missing references)
			//IL_0309: Unknown result type (might be due to invalid IL or missing references)
			//IL_031f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0337: Expected O, but got Unknown
			//IL_0337: Unknown result type (might be due to invalid IL or missing references)
			//IL_033c: Unknown result type (might be due to invalid IL or missing references)
			//IL_033d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0352: Unknown result type (might be due to invalid IL or missing references)
			//IL_035c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0367: Unknown result type (might be due to invalid IL or missing references)
			//IL_0372: Unknown result type (might be due to invalid IL or missing references)
			//IL_037d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0399: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03da: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_03eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_040f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0427: Expected O, but got Unknown
			//IL_0428: Unknown result type (might be due to invalid IL or missing references)
			//IL_042d: Unknown result type (might be due to invalid IL or missing references)
			//IL_042e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0443: Unknown result type (might be due to invalid IL or missing references)
			//IL_044d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0458: Unknown result type (might be due to invalid IL or missing references)
			//IL_0463: Unknown result type (might be due to invalid IL or missing references)
			//IL_046e: Unknown result type (might be due to invalid IL or missing references)
			//IL_048a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0496: Expected O, but got Unknown
			//IL_04bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_050b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0523: Expected O, but got Unknown
			//IL_0524: Unknown result type (might be due to invalid IL or missing references)
			//IL_0529: Unknown result type (might be due to invalid IL or missing references)
			//IL_052a: Unknown result type (might be due to invalid IL or missing references)
			//IL_053f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0549: Unknown result type (might be due to invalid IL or missing references)
			//IL_0554: Unknown result type (might be due to invalid IL or missing references)
			//IL_055f: Unknown result type (might be due to invalid IL or missing references)
			//IL_056a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0586: Unknown result type (might be due to invalid IL or missing references)
			//IL_0592: Expected O, but got Unknown
			//IL_05b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_05be: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_05dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_060a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0619: Unknown result type (might be due to invalid IL or missing references)
			//IL_0621: Unknown result type (might be due to invalid IL or missing references)
			//IL_062e: Expected O, but got Unknown
			//IL_062e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0633: Unknown result type (might be due to invalid IL or missing references)
			//IL_064c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0656: Unknown result type (might be due to invalid IL or missing references)
			//IL_065d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0664: Unknown result type (might be due to invalid IL or missing references)
			//IL_066b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0681: Unknown result type (might be due to invalid IL or missing references)
			//IL_0699: Unknown result type (might be due to invalid IL or missing references)
			//IL_069e: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_06b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_06c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_06cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_06d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_06db: Unknown result type (might be due to invalid IL or missing references)
			//IL_06e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_06f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ff: Expected O, but got Unknown
			//IL_06ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0704: Unknown result type (might be due to invalid IL or missing references)
			//IL_0709: Unknown result type (might be due to invalid IL or missing references)
			//IL_0713: Unknown result type (might be due to invalid IL or missing references)
			//IL_071a: Unknown result type (might be due to invalid IL or missing references)
			//IL_072d: Unknown result type (might be due to invalid IL or missing references)
			//IL_073a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0744: Unknown result type (might be due to invalid IL or missing references)
			//IL_0750: Unknown result type (might be due to invalid IL or missing references)
			//IL_0761: Unknown result type (might be due to invalid IL or missing references)
			//IL_076a: Expected O, but got Unknown
			//IL_076b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0770: Unknown result type (might be due to invalid IL or missing references)
			//IL_0773: Unknown result type (might be due to invalid IL or missing references)
			//IL_077d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0784: Unknown result type (might be due to invalid IL or missing references)
			//IL_078c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0793: Unknown result type (might be due to invalid IL or missing references)
			//IL_079f: Expected O, but got Unknown
			//IL_07f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_084a: Unknown result type (might be due to invalid IL or missing references)
			//IL_084f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0868: Unknown result type (might be due to invalid IL or missing references)
			//IL_0872: Unknown result type (might be due to invalid IL or missing references)
			//IL_087d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0884: Unknown result type (might be due to invalid IL or missing references)
			//IL_088f: Unknown result type (might be due to invalid IL or missing references)
			//IL_08b4: Expected O, but got Unknown
			Logger.Info("Building settings view");
			Panel val = new Panel();
			val.set_CanScroll(false);
			((Control)val).set_Parent(buildPanel);
			((Control)val).set_Height(((Control)buildPanel).get_Height());
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Control)val).set_Width(((Control)buildPanel).get_Width());
			Panel parentPanel = val;
			Point topMiddleRight = default(Point);
			((Point)(ref topMiddleRight))._002Ector(((Control)parentPanel).get_Width() / 2 - 10, 10);
			Logger.Debug("Building 'Enabled' setting controls");
			Checkbox val2 = new Checkbox();
			((Control)val2).set_Location(new Point(_topLeft.X, _topLeft.Y + 2));
			((Control)val2).set_Parent((Container)(object)parentPanel);
			val2.set_Checked(_moduleSettings.Enabled.get_Value());
			((Control)val2).set_Width(10);
			Checkbox enableCheckbox = val2;
			Label val3 = new Label();
			((Control)val3).set_Location(new Point(((Control)enableCheckbox).get_Right() + 2, _topLeft.Y));
			val3.set_AutoSizeWidth(true);
			val3.set_WrapText(false);
			((Control)val3).set_Parent((Container)(object)parentPanel);
			val3.set_Text(((SettingEntry)_moduleSettings.Enabled).get_DisplayName());
			((Control)val3).set_BasicTooltipText(((SettingEntry)_moduleSettings.Enabled).get_Description());
			enableCheckbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object sender, CheckChangedEvent e)
			{
				_moduleSettings.Enabled.set_Value(e.get_Checked());
			});
			Logger.Debug("Building 'Image' setting controls");
			Label val4 = new Label();
			((Control)val4).set_Location(new Point(_topLeft.X, ((Control)enableCheckbox).get_Bottom() + 8));
			val4.set_WrapText(true);
			((Control)val4).set_Width(((Control)parentPanel).get_Width() / 2 - _topLeft.X * 2);
			val4.set_AutoSizeHeight(true);
			((Control)val4).set_Parent((Container)(object)parentPanel);
			val4.set_Text(Tortle.PlayerMarker.Localization.ModuleSettings.PlayerMarkerImage_Description);
			Label imageDescription = val4;
			Label val5 = new Label();
			((Control)val5).set_Location(new Point(_topLeft.X, ((Control)imageDescription).get_Bottom() + 8));
			val5.set_AutoSizeWidth(true);
			val5.set_WrapText(false);
			((Control)val5).set_Parent((Container)(object)parentPanel);
			val5.set_Text(((SettingEntry)_moduleSettings.ImageName).get_DisplayName());
			((Control)val5).set_BasicTooltipText(((SettingEntry)_moduleSettings.ImageName).get_Description());
			Label imageLabel = val5;
			Dropdown val6 = new Dropdown();
			((Control)val6).set_Location(new Point(topMiddleRight.X - 175, ((Control)imageLabel).get_Top()));
			((Control)val6).set_Width(175);
			((Control)val6).set_Parent((Container)(object)parentPanel);
			Dropdown imageSelect = val6;
			foreach (string name in _markerTextureManager.GetNames())
			{
				imageSelect.get_Items().Add(name);
			}
			imageSelect.set_SelectedItem(_moduleSettings.ImageName.get_Value());
			imageSelect.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				_moduleSettings.ImageName.set_Value(imageSelect.get_SelectedItem());
			});
			Logger.Debug("Building 'Size' setting controls");
			Label val7 = new Label();
			((Control)val7).set_Location(new Point(_topLeft.X, ((Control)imageLabel).get_Bottom() + 8));
			val7.set_AutoSizeWidth(true);
			val7.set_WrapText(false);
			((Control)val7).set_Parent((Container)(object)parentPanel);
			val7.set_Text(((SettingEntry)_moduleSettings.Size).get_DisplayName());
			((Control)val7).set_BasicTooltipText(((SettingEntry)_moduleSettings.Size).get_Description());
			Label sizeLabel = val7;
			TrackBar val8 = new TrackBar();
			((Control)val8).set_Location(new Point(topMiddleRight.X - 175, ((Control)sizeLabel).get_Top() + 2));
			((Control)val8).set_Width(175);
			val8.set_MaxValue(40f);
			val8.set_MinValue(1f);
			val8.set_Value(_moduleSettings.Size.get_Value() * 40f);
			((Control)val8).set_Parent((Container)(object)parentPanel);
			val8.add_ValueChanged((EventHandler<ValueEventArgs<float>>)delegate(object sender, ValueEventArgs<float> args)
			{
				_moduleSettings.Size.set_Value(args.get_Value() / 40f);
			});
			Logger.Debug("Building 'Opacity' setting controls");
			Label val9 = new Label();
			((Control)val9).set_Location(new Point(_topLeft.X, ((Control)sizeLabel).get_Bottom() + 8));
			val9.set_AutoSizeWidth(true);
			val9.set_WrapText(false);
			((Control)val9).set_Parent((Container)(object)parentPanel);
			val9.set_Text(((SettingEntry)_moduleSettings.Opacity).get_DisplayName());
			((Control)val9).set_BasicTooltipText(((SettingEntry)_moduleSettings.Opacity).get_Description());
			Label opacityLabel = val9;
			TrackBar val10 = new TrackBar();
			((Control)val10).set_Location(new Point(topMiddleRight.X - 175, ((Control)opacityLabel).get_Top() + 2));
			((Control)val10).set_Width(175);
			val10.set_MaxValue(100f);
			val10.set_MinValue(0f);
			val10.set_Value(_moduleSettings.Opacity.get_Value() * 100f);
			((Control)val10).set_Parent((Container)(object)parentPanel);
			TrackBar opacitySlider = val10;
			opacitySlider.add_ValueChanged((EventHandler<ValueEventArgs<float>>)delegate
			{
				_moduleSettings.Opacity.set_Value(opacitySlider.get_Value() / 100f);
			});
			Logger.Debug("Building 'Vertical Offset' setting controls");
			Label val11 = new Label();
			((Control)val11).set_Location(new Point(_topLeft.X, ((Control)opacityLabel).get_Bottom() + 8));
			val11.set_AutoSizeWidth(true);
			val11.set_WrapText(false);
			((Control)val11).set_Parent((Container)(object)parentPanel);
			val11.set_Text(((SettingEntry)_moduleSettings.VerticalOffset).get_DisplayName());
			((Control)val11).set_BasicTooltipText(((SettingEntry)_moduleSettings.VerticalOffset).get_Description());
			Label verticalOffsetLabel = val11;
			TrackBar val12 = new TrackBar();
			((Control)val12).set_Location(new Point(topMiddleRight.X - 175, ((Control)verticalOffsetLabel).get_Top() + 2));
			((Control)val12).set_Width(175);
			val12.set_MaxValue(60f);
			val12.set_MinValue(0f);
			val12.set_Value(_moduleSettings.VerticalOffset.get_Value() * 10f);
			((Control)val12).set_Parent((Container)(object)parentPanel);
			TrackBar verticalOffsetSlider = val12;
			verticalOffsetSlider.add_ValueChanged((EventHandler<ValueEventArgs<float>>)delegate
			{
				_moduleSettings.VerticalOffset.set_Value(verticalOffsetSlider.get_Value() / 10f);
			});
			Logger.Debug("Building 'Color' setting controls");
			ColorBox val13 = new ColorBox();
			((Control)val13).set_Location(new Point(_topLeft.X, ((Control)verticalOffsetLabel).get_Bottom() + 8));
			((Control)val13).set_Parent((Container)(object)parentPanel);
			val13.set_Color(ConvertColor(_moduleSettings.Color.get_Value(), ColorPresets.Colors[_moduleSettings.Color.get_Value()]));
			((Control)val13).set_Height(20);
			((Control)val13).set_Width(20);
			ColorBox colorBox = val13;
			Label val14 = new Label();
			((Control)val14).set_Location(new Point(((Control)colorBox).get_Right() + 2, ((Control)colorBox).get_Top()));
			val14.set_AutoSizeWidth(true);
			val14.set_WrapText(false);
			((Control)val14).set_Parent((Container)(object)parentPanel);
			val14.set_Text(((SettingEntry)_moduleSettings.Color).get_DisplayName());
			((Control)val14).set_BasicTooltipText(((SettingEntry)_moduleSettings.Color).get_Description());
			Panel val15 = new Panel();
			((Control)val15).set_Location(new Point(((Control)colorBox).get_Right() + 5, 0));
			((Control)val15).set_Size(new Point(400, 235));
			((Control)val15).set_Visible(false);
			((Control)val15).set_ZIndex(10);
			((Control)val15).set_Parent((Container)(object)parentPanel);
			val15.set_BackgroundTexture(AsyncTexture2D.op_Implicit(_panelBackgroundTexture));
			val15.set_ShowBorder(false);
			Panel colorPickerPanel = val15;
			Panel val16 = new Panel();
			((Control)val16).set_Location(new Point(15, 15));
			((Control)val16).set_Size(new Point(((Control)colorPickerPanel).get_Size().X - 35, ((Control)colorPickerPanel).get_Size().Y - 35));
			((Control)val16).set_Parent((Container)(object)colorPickerPanel);
			val16.set_BackgroundTexture(AsyncTexture2D.op_Implicit(_buttonDarkTexture));
			val16.set_ShowBorder(true);
			Panel colorPickerBg = val16;
			ColorPicker val17 = new ColorPicker();
			((Control)val17).set_Size(((Control)colorPickerBg).get_Size());
			((Panel)val17).set_CanScroll(false);
			((Control)val17).set_Parent((Container)(object)colorPickerBg);
			((Panel)val17).set_ShowTint(false);
			((Control)val17).set_Visible(true);
			ColorPicker colorPicker = val17;
			colorPicker.add_SelectedColorChanged((EventHandler<EventArgs>)delegate
			{
				colorPicker.get_AssociatedColorBox().set_Color(colorPicker.get_SelectedColor());
				_moduleSettings.Color.set_Value(colorPicker.get_SelectedColor().get_Name());
			});
			((Control)colorPicker).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				((Control)colorPickerPanel).set_Visible(false);
			});
			foreach (KeyValuePair<string, Color> color in ColorPresets.Colors)
			{
				colorPicker.get_Colors().Add(ConvertColor(color.Key, color.Value));
			}
			((Control)colorBox).add_Click((EventHandler<MouseEventArgs>)delegate(object sender, MouseEventArgs e)
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_0011: Expected O, but got Unknown
				colorPicker.set_AssociatedColorBox((ColorBox)sender);
				((Control)colorPickerPanel).set_Visible(!((Control)colorPickerPanel).get_Visible());
			});
			((Control)parentPanel).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)delegate
			{
				if (((Control)colorPickerPanel).get_Visible() && !((Control)colorPickerPanel).get_MouseOver() && !((Control)colorBox).get_MouseOver())
				{
					((Control)colorPickerPanel).set_Visible(false);
				}
			});
			StandardButton val18 = new StandardButton();
			((Control)val18).set_Location(new Point(_topLeft.X, ((Control)colorBox).get_Bottom() + 8));
			((Control)val18).set_Width(200);
			((Control)val18).set_Parent((Container)(object)parentPanel);
			val18.set_Text(Tortle.PlayerMarker.Localization.ModuleSettings.CleanupDuplicates_Name);
			((Control)val18).set_BasicTooltipText(string.Format(Tortle.PlayerMarker.Localization.ModuleSettings.CleanupDuplicates_Tooltip, _markerTextureManager.GetDuplicateCount()));
			StandardButton cleanUpDuplicatesButton = val18;
			((Control)cleanUpDuplicatesButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_markerTextureManager.CleanupDuplicates();
				((Control)cleanUpDuplicatesButton).set_BasicTooltipText(string.Format(Tortle.PlayerMarker.Localization.ModuleSettings.CleanupDuplicates_Tooltip, _markerTextureManager.GetDuplicateCount()));
			});
			Logger.Info("Finished building settings view");
		}

		private static Color ConvertColor(string name, Color color)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected O, but got Unknown
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Expected O, but got Unknown
			Color val = new Color();
			val.set_Name(name);
			ColorMaterial val2 = new ColorMaterial();
			val2.set_Rgb((IReadOnlyList<int>)new int[3]
			{
				((Color)(ref color)).get_R(),
				((Color)(ref color)).get_G(),
				((Color)(ref color)).get_B()
			});
			val.set_Cloth(val2);
			return val;
		}

		public void LoadTextures()
		{
			Logger.Info("Loading textures");
			_panelBackgroundTexture = _contentsManager.GetTexture("panelBackground.png");
			_buttonDarkTexture = _contentsManager.GetTexture("buttonDark.png");
		}

		public void Dispose()
		{
			Logger.Debug("Disposing");
			Texture2D panelBackgroundTexture = _panelBackgroundTexture;
			if (panelBackgroundTexture != null)
			{
				((GraphicsResource)panelBackgroundTexture).Dispose();
			}
			Texture2D buttonDarkTexture = _buttonDarkTexture;
			if (buttonDarkTexture != null)
			{
				((GraphicsResource)buttonDarkTexture).Dispose();
			}
		}
	}
}
