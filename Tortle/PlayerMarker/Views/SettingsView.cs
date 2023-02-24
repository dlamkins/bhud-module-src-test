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
		private static readonly Logger Logger = Logger.GetLogger(typeof(SettingsView));

		private readonly TextureCache _textureCache;

		private readonly Tortle.PlayerMarker.Services.ModuleSettings _moduleSettings;

		private readonly ContentsManager _contentsManager;

		private Texture2D _panelBackgroundTexture;

		private Texture2D _buttonDarkTexture;

		private readonly Point _topLeft = new Point(10, 18);

		public SettingsView(TextureCache textureCache, Tortle.PlayerMarker.Services.ModuleSettings moduleSettings, ContentsManager contentsManager)
			: this()
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			_textureCache = textureCache;
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
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Expected O, but got Unknown
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Expected O, but got Unknown
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_013c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0151: Unknown result type (might be due to invalid IL or missing references)
			//IL_0167: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Expected O, but got Unknown
			//IL_0180: Unknown result type (might be due to invalid IL or missing references)
			//IL_0185: Unknown result type (might be due to invalid IL or missing references)
			//IL_0198: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e3: Expected O, but got Unknown
			//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0202: Unknown result type (might be due to invalid IL or missing references)
			//IL_020d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0217: Unknown result type (might be due to invalid IL or missing references)
			//IL_021e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0226: Unknown result type (might be due to invalid IL or missing references)
			//IL_022d: Unknown result type (might be due to invalid IL or missing references)
			//IL_023e: Unknown result type (might be due to invalid IL or missing references)
			//IL_024a: Expected O, but got Unknown
			//IL_024a: Unknown result type (might be due to invalid IL or missing references)
			//IL_024f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0254: Unknown result type (might be due to invalid IL or missing references)
			//IL_025e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0265: Unknown result type (might be due to invalid IL or missing references)
			//IL_0278: Unknown result type (might be due to invalid IL or missing references)
			//IL_0285: Unknown result type (might be due to invalid IL or missing references)
			//IL_028f: Unknown result type (might be due to invalid IL or missing references)
			//IL_029b: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b5: Expected O, but got Unknown
			//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02be: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02de: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ea: Expected O, but got Unknown
			//IL_0343: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0409: Expected O, but got Unknown
			//IL_0409: Unknown result type (might be due to invalid IL or missing references)
			//IL_040e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0423: Unknown result type (might be due to invalid IL or missing references)
			//IL_042d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0434: Unknown result type (might be due to invalid IL or missing references)
			//IL_043b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0442: Unknown result type (might be due to invalid IL or missing references)
			//IL_0458: Unknown result type (might be due to invalid IL or missing references)
			//IL_0470: Expected O, but got Unknown
			//IL_0471: Unknown result type (might be due to invalid IL or missing references)
			//IL_0476: Unknown result type (might be due to invalid IL or missing references)
			//IL_0487: Unknown result type (might be due to invalid IL or missing references)
			//IL_0491: Unknown result type (might be due to invalid IL or missing references)
			//IL_049c: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a8: Expected O, but got Unknown
			//IL_052f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0534: Unknown result type (might be due to invalid IL or missing references)
			//IL_0549: Unknown result type (might be due to invalid IL or missing references)
			//IL_0553: Unknown result type (might be due to invalid IL or missing references)
			//IL_055a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0561: Unknown result type (might be due to invalid IL or missing references)
			//IL_0568: Unknown result type (might be due to invalid IL or missing references)
			//IL_057e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0596: Expected O, but got Unknown
			//IL_0596: Unknown result type (might be due to invalid IL or missing references)
			//IL_059b: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_05f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_061c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0621: Unknown result type (might be due to invalid IL or missing references)
			//IL_0636: Unknown result type (might be due to invalid IL or missing references)
			//IL_0640: Unknown result type (might be due to invalid IL or missing references)
			//IL_0647: Unknown result type (might be due to invalid IL or missing references)
			//IL_064e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0655: Unknown result type (might be due to invalid IL or missing references)
			//IL_066b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0683: Expected O, but got Unknown
			//IL_0684: Unknown result type (might be due to invalid IL or missing references)
			//IL_0689: Unknown result type (might be due to invalid IL or missing references)
			//IL_069c: Unknown result type (might be due to invalid IL or missing references)
			//IL_06a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_06b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_06bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_06c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_06e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ef: Expected O, but got Unknown
			//IL_0715: Unknown result type (might be due to invalid IL or missing references)
			//IL_071a: Unknown result type (might be due to invalid IL or missing references)
			//IL_072f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0739: Unknown result type (might be due to invalid IL or missing references)
			//IL_0740: Unknown result type (might be due to invalid IL or missing references)
			//IL_0747: Unknown result type (might be due to invalid IL or missing references)
			//IL_074e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0764: Unknown result type (might be due to invalid IL or missing references)
			//IL_077c: Expected O, but got Unknown
			//IL_077d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0782: Unknown result type (might be due to invalid IL or missing references)
			//IL_0795: Unknown result type (might be due to invalid IL or missing references)
			//IL_079f: Unknown result type (might be due to invalid IL or missing references)
			//IL_07aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_07b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_07c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_07dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_07e8: Expected O, but got Unknown
			Logger.Info("Building settings view");
			Panel val = new Panel();
			val.set_CanScroll(false);
			((Control)val).set_Parent(buildPanel);
			((Control)val).set_Height(((Control)buildPanel).get_Height());
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Control)val).set_Width(((Control)buildPanel).get_Width());
			Panel parentPanel = val;
			Logger.Debug("Building 'Enabled' setting controls");
			Label val2 = new Label();
			((Control)val2).set_Location(_topLeft);
			val2.set_AutoSizeWidth(true);
			val2.set_WrapText(false);
			((Control)val2).set_Parent((Container)(object)parentPanel);
			val2.set_Text(((SettingEntry)_moduleSettings.Enabled).get_DisplayName());
			((Control)val2).set_BasicTooltipText(((SettingEntry)_moduleSettings.Enabled).get_Description());
			Label enableLabel = val2;
			Checkbox val3 = new Checkbox();
			((Control)val3).set_Location(new Point(((Control)enableLabel).get_Right() + 5, ((Control)enableLabel).get_Top() + 2));
			((Control)val3).set_Parent((Container)(object)parentPanel);
			val3.set_Checked(_moduleSettings.Enabled.get_Value());
			((Control)val3).set_Width(10);
			Checkbox enableCheckbox = val3;
			enableCheckbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object sender, CheckChangedEvent e)
			{
				_moduleSettings.Enabled.set_Value(e.get_Checked());
			});
			Logger.Debug("Building 'Color' setting controls");
			Label val4 = new Label();
			((Control)val4).set_Location(new Point(((Control)enableCheckbox).get_Right() + 10, _topLeft.Y));
			val4.set_AutoSizeWidth(true);
			val4.set_WrapText(false);
			((Control)val4).set_Parent((Container)(object)parentPanel);
			val4.set_Text(((SettingEntry)_moduleSettings.Color).get_DisplayName());
			((Control)val4).set_BasicTooltipText(((SettingEntry)_moduleSettings.Color).get_Description());
			Label colorLabel = val4;
			ColorBox val5 = new ColorBox();
			((Control)val5).set_Location(new Point(((Control)colorLabel).get_Right() + 5, ((Control)colorLabel).get_Top() - 5));
			((Control)val5).set_Parent((Container)(object)parentPanel);
			val5.set_Color(ConvertColor(_moduleSettings.Color.get_Value(), ColorPresets.Colors[_moduleSettings.Color.get_Value()]));
			ColorBox colorBox = val5;
			Panel val6 = new Panel();
			((Control)val6).set_Location(new Point(((Control)colorBox).get_Right() + 5, 0));
			((Control)val6).set_Size(new Point(400, 235));
			((Control)val6).set_Visible(false);
			((Control)val6).set_ZIndex(10);
			((Control)val6).set_Parent((Container)(object)parentPanel);
			val6.set_BackgroundTexture(AsyncTexture2D.op_Implicit(_panelBackgroundTexture));
			val6.set_ShowBorder(false);
			Panel colorPickerPanel = val6;
			Panel val7 = new Panel();
			((Control)val7).set_Location(new Point(15, 15));
			((Control)val7).set_Size(new Point(((Control)colorPickerPanel).get_Size().X - 35, ((Control)colorPickerPanel).get_Size().Y - 35));
			((Control)val7).set_Parent((Container)(object)colorPickerPanel);
			val7.set_BackgroundTexture(AsyncTexture2D.op_Implicit(_buttonDarkTexture));
			val7.set_ShowBorder(true);
			Panel colorPickerBg = val7;
			ColorPicker val8 = new ColorPicker();
			((Control)val8).set_Size(((Control)colorPickerBg).get_Size());
			((Panel)val8).set_CanScroll(false);
			((Control)val8).set_Parent((Container)(object)colorPickerBg);
			((Panel)val8).set_ShowTint(false);
			((Control)val8).set_Visible(true);
			ColorPicker colorPicker = val8;
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
			Logger.Debug("Building 'Image' setting controls");
			Label val9 = new Label();
			((Control)val9).set_Location(new Point(_topLeft.X, ((Control)colorBox).get_Bottom() + 8));
			val9.set_WrapText(true);
			((Control)val9).set_Width(((Control)parentPanel).get_Width() / 2 - _topLeft.X * 2);
			val9.set_AutoSizeHeight(true);
			((Control)val9).set_Parent((Container)(object)parentPanel);
			val9.set_Text(Tortle.PlayerMarker.Localization.ModuleSettings.PlayerMarkerImage_Description);
			Label imageDescription = val9;
			Label val10 = new Label();
			((Control)val10).set_Location(new Point(_topLeft.X, ((Control)imageDescription).get_Bottom() + 8));
			val10.set_AutoSizeWidth(true);
			val10.set_WrapText(false);
			((Control)val10).set_Parent((Container)(object)parentPanel);
			val10.set_Text(((SettingEntry)_moduleSettings.ImageName).get_DisplayName());
			((Control)val10).set_BasicTooltipText(((SettingEntry)_moduleSettings.ImageName).get_Description());
			Label imageLabel = val10;
			Dropdown val11 = new Dropdown();
			((Control)val11).set_Location(new Point(((Control)imageLabel).get_Right() + 5, ((Control)imageLabel).get_Top()));
			((Control)val11).set_Width(250);
			((Control)val11).set_Parent((Container)(object)parentPanel);
			Dropdown imageSelect = val11;
			foreach (string name in _textureCache.GetNames())
			{
				imageSelect.get_Items().Add(name);
			}
			imageSelect.set_SelectedItem(_moduleSettings.ImageName.get_Value());
			imageSelect.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				_moduleSettings.ImageName.set_Value(imageSelect.get_SelectedItem());
			});
			Logger.Debug("Building 'Size' setting controls");
			Label val12 = new Label();
			((Control)val12).set_Location(new Point(_topLeft.X, ((Control)imageLabel).get_Bottom() + 8));
			val12.set_AutoSizeWidth(true);
			val12.set_WrapText(false);
			((Control)val12).set_Parent((Container)(object)parentPanel);
			val12.set_Text(((SettingEntry)_moduleSettings.Size).get_DisplayName());
			((Control)val12).set_BasicTooltipText(((SettingEntry)_moduleSettings.Size).get_Description());
			Label sizeLabel = val12;
			TrackBar val13 = new TrackBar();
			((Control)val13).set_Location(new Point(((Control)sizeLabel).get_Right() + 5, ((Control)sizeLabel).get_Top() + 2));
			((Control)val13).set_Width(250);
			val13.set_MaxValue(40f);
			val13.set_MinValue(1f);
			val13.set_Value(_moduleSettings.Size.get_Value() * 40f);
			((Control)val13).set_Parent((Container)(object)parentPanel);
			val13.add_ValueChanged((EventHandler<ValueEventArgs<float>>)delegate(object sender, ValueEventArgs<float> args)
			{
				_moduleSettings.Size.set_Value(args.get_Value() / 40f);
			});
			Logger.Debug("Building 'Opacity' setting controls");
			Label val14 = new Label();
			((Control)val14).set_Location(new Point(_topLeft.X, ((Control)sizeLabel).get_Bottom() + 8));
			val14.set_AutoSizeWidth(true);
			val14.set_WrapText(false);
			((Control)val14).set_Parent((Container)(object)parentPanel);
			val14.set_Text(((SettingEntry)_moduleSettings.Opacity).get_DisplayName());
			((Control)val14).set_BasicTooltipText(((SettingEntry)_moduleSettings.Opacity).get_Description());
			Label opacityLabel = val14;
			TrackBar val15 = new TrackBar();
			((Control)val15).set_Location(new Point(((Control)opacityLabel).get_Right() + 5, ((Control)opacityLabel).get_Top() + 2));
			((Control)val15).set_Width(250);
			val15.set_MaxValue(100f);
			val15.set_MinValue(0f);
			val15.set_Value(_moduleSettings.Opacity.get_Value() * 100f);
			((Control)val15).set_Parent((Container)(object)parentPanel);
			TrackBar opacitySlider = val15;
			opacitySlider.add_ValueChanged((EventHandler<ValueEventArgs<float>>)delegate
			{
				_moduleSettings.Opacity.set_Value(opacitySlider.get_Value() / 100f);
			});
			Logger.Debug("Building 'Vertical Offset' setting controls");
			Label val16 = new Label();
			((Control)val16).set_Location(new Point(_topLeft.X, ((Control)opacityLabel).get_Bottom() + 8));
			val16.set_AutoSizeWidth(true);
			val16.set_WrapText(false);
			((Control)val16).set_Parent((Container)(object)parentPanel);
			val16.set_Text(((SettingEntry)_moduleSettings.VerticalOffset).get_DisplayName());
			((Control)val16).set_BasicTooltipText(((SettingEntry)_moduleSettings.VerticalOffset).get_Description());
			Label verticalOffsetLabel = val16;
			TrackBar val17 = new TrackBar();
			((Control)val17).set_Location(new Point(((Control)verticalOffsetLabel).get_Right() + 5, ((Control)verticalOffsetLabel).get_Top() + 2));
			((Control)val17).set_Width(250);
			val17.set_MaxValue(60f);
			val17.set_MinValue(0f);
			val17.set_Value(_moduleSettings.VerticalOffset.get_Value() * 10f);
			((Control)val17).set_Parent((Container)(object)parentPanel);
			TrackBar verticalOffsetSlider = val17;
			verticalOffsetSlider.add_ValueChanged((EventHandler<ValueEventArgs<float>>)delegate
			{
				_moduleSettings.VerticalOffset.set_Value(verticalOffsetSlider.get_Value() / 10f);
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
