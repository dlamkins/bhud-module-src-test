using System;
using System.Collections.Generic;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;

namespace SeaStove.Gw2Tldr.UI
{
	internal sealed class GuideSettingsWindow : IDisposable
	{
		private readonly UtilityWindow _window;

		private readonly Func<int> _opacity;

		private readonly Func<int> _textSize;

		private readonly Func<int> _fontStyle;

		private readonly Func<bool> _colors;

		private readonly Func<bool> _fade;

		private readonly Func<bool> _auto;

		private readonly Func<bool> _light;

		private readonly Func<bool> _showCopy;

		private readonly Func<bool> _colorText;

		private readonly Func<bool> _hideUnmapped;

		private readonly Func<bool> _showMapped;

		private readonly Func<bool> _hideLoading;

		private readonly Func<bool> _autoHeight;

		private readonly Action<int> _setOpacity;

		private readonly Action<int> _setTextSize;

		private readonly Action<int> _setFontStyle;

		private readonly Action<bool> _setColors;

		private readonly Action<bool> _setFade;

		private readonly Action<bool> _setAuto;

		private readonly Action<bool> _setLight;

		private readonly Action<bool> _setShowCopy;

		private readonly Action<bool> _setColorText;

		private readonly Action<bool> _setHideUnmapped;

		private readonly Action<bool> _setShowMapped;

		private readonly Action<bool> _setHideLoading;

		private readonly Action<bool> _setAutoHeight;

		private readonly Func<bool> _closeOnEscape;

		private readonly Action<bool> _setCloseOnEscape;

		private readonly Func<string, bool> _categoryEnabled;

		private readonly Action<string, bool> _setCategoryEnabled;

		private readonly Dictionary<string, Checkbox> _categoryButtons = new Dictionary<string, Checkbox>();

		private readonly DarkButton _escapeValue;

		private Panel _opacityMenu;

		private readonly DarkButton _opacityValue;

		private readonly DarkButton _fontValue;

		private readonly DarkButton _textValue;

		private readonly DarkButton _colorsValue;

		private readonly DarkButton _fadeValue;

		private readonly DarkButton _autoValue;

		private readonly DarkButton _lightValue;

		private readonly DarkButton _copyValue;

		private readonly DarkButton _textColorValue;

		private readonly DarkButton _hideUnmappedValue;

		private readonly DarkButton _showMappedValue;

		private readonly DarkButton _hideLoadingValue;

		private readonly DarkButton _autoHeightValue;

		public GuideSettingsWindow(Func<int> opacity, Action<int> setOpacity, Func<int> textSize, Action<int> setTextSize, Func<bool> colors, Action<bool> setColors, Func<bool> fade, Action<bool> setFade, Func<bool> auto, Action<bool> setAuto, Func<bool> light, Action<bool> setLight, Func<bool> showCopy, Action<bool> setShowCopy, Func<bool> colorText, Action<bool> setColorText, Func<bool> hideUnmapped, Action<bool> setHideUnmapped, Func<bool> showMapped, Action<bool> setShowMapped, Func<int> fontStyle, Action<int> setFontStyle, Func<bool> hideLoading, Action<bool> setHideLoading, Func<bool> autoHeight, Action<bool> setAutoHeight, Func<bool> closeOnEscape, Action<bool> setCloseOnEscape, Func<string, bool> categoryEnabled, Action<string, bool> setCategoryEnabled)
		{
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_016f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0174: Unknown result type (might be due to invalid IL or missing references)
			//IL_017e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0186: Unknown result type (might be due to invalid IL or missing references)
			//IL_0190: Unknown result type (might be due to invalid IL or missing references)
			//IL_019b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0378: Unknown result type (might be due to invalid IL or missing references)
			//IL_037d: Unknown result type (might be due to invalid IL or missing references)
			//IL_038e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0396: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_04df: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_04fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0506: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_05bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_05cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_05db: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_0601: Unknown result type (might be due to invalid IL or missing references)
			//IL_0630: Expected O, but got Unknown
			//IL_0630: Unknown result type (might be due to invalid IL or missing references)
			//IL_0635: Unknown result type (might be due to invalid IL or missing references)
			//IL_0646: Unknown result type (might be due to invalid IL or missing references)
			//IL_064e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0658: Unknown result type (might be due to invalid IL or missing references)
			//IL_0660: Unknown result type (might be due to invalid IL or missing references)
			//IL_066a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0671: Unknown result type (might be due to invalid IL or missing references)
			//IL_0672: Unknown result type (might be due to invalid IL or missing references)
			//IL_06c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_06cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_06de: Unknown result type (might be due to invalid IL or missing references)
			//IL_06e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_06f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_06f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0702: Unknown result type (might be due to invalid IL or missing references)
			//IL_070d: Unknown result type (might be due to invalid IL or missing references)
			//IL_070e: Unknown result type (might be due to invalid IL or missing references)
			_closeOnEscape = closeOnEscape;
			_setCloseOnEscape = setCloseOnEscape;
			_categoryEnabled = categoryEnabled;
			_setCategoryEnabled = setCategoryEnabled;
			_autoHeight = autoHeight;
			_setAutoHeight = setAutoHeight;
			_hideLoading = hideLoading;
			_setHideLoading = setHideLoading;
			_fontStyle = fontStyle;
			_setFontStyle = setFontStyle;
			_opacity = opacity;
			_setOpacity = setOpacity;
			_textSize = textSize;
			_setTextSize = setTextSize;
			_colors = colors;
			_setColors = setColors;
			_fade = fade;
			_setFade = setFade;
			_auto = auto;
			_setAuto = setAuto;
			_light = light;
			_setLight = setLight;
			_showCopy = showCopy;
			_setShowCopy = setShowCopy;
			_colorText = colorText;
			_setColorText = setColorText;
			_hideUnmapped = hideUnmapped;
			_setHideUnmapped = setHideUnmapped;
			_showMapped = showMapped;
			_setShowMapped = setShowMapped;
			_window = new UtilityWindow("Settings", "GW2 TLDR", new Point(280, 180));
			((Control)_window).set_Height(735);
			((Control)_window.Body).set_Height(675);
			_window.Body.set_CanScroll(true);
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)_window.Body);
			((Control)val).set_Location(new Point(15, 10));
			((Control)val).set_Size(new Point(470, 30));
			val.set_Text("Overlay appearance");
			val.set_TextColor(Color.get_White());
			_opacityValue = AddRow("Background opacity", 50, "Choose background opacity");
			((Control)_opacityValue).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				ToggleOpacityMenu();
			});
			((Control)_window).add_Hidden((EventHandler<EventArgs>)OnHidden);
			_textValue = AddRow("Guide text size", 95, "Cycle through small, medium and large");
			((Control)_textValue).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_setTextSize((_textSize() + 1) % 3);
				Refresh();
			});
			_fontValue = AddRow("Guide font style", 130, "Cycle through regular, bold and italic");
			((Control)_fontValue).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_setFontStyle((_fontStyle() + 1) % 3);
				Refresh();
			});
			_colorsValue = AddRow("Color-coded mechanic icons", 175, "Toggle mechanic colors");
			((Control)_colorsValue).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_setColors(!_colors());
				Refresh();
			});
			_textColorValue = AddRow("Color-coded note text", 210, "Color text by mechanic type");
			((Control)_textColorValue).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_setColorText(!_colorText());
				Refresh();
			});
			_copyValue = AddRow("Show copy buttons", 245, "Toggle all guide copy controls");
			((Control)_copyValue).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_setShowCopy(!_showCopy());
				Refresh();
			});
			_fadeValue = AddRow("Hide header buttons until hover", 280, "Keep header buttons subtle until the pointer enters the header");
			((Control)_fadeValue).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_setFade(!_fade());
				Refresh();
			});
			_lightValue = AddRow("Appearance", 315, "Toggle light and dark mode");
			((Control)_lightValue).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_setLight(!_light());
				Refresh();
			});
			_autoHeightValue = AddRow("Fit guide height to content", 350, "Resize the guide height to fit its notes");
			((Control)_autoHeightValue).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_setAutoHeight(!_autoHeight());
				Refresh();
			});
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)_window.Body);
			((Control)val2).set_Location(new Point(15, 395));
			((Control)val2).set_Size(new Point(470, 30));
			val2.set_Text("Guide selection");
			val2.set_TextColor(Color.get_White());
			_autoValue = AddRow("Auto-select by map", 435, "Switch to the first guide on a mapped area");
			((Control)_autoValue).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_setAuto(!_auto());
				Refresh();
			});
			_hideUnmappedValue = AddRow("Hide on maps without guides", 470, "Hide when entering an unmapped map");
			((Control)_hideUnmappedValue).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_setHideUnmapped(!_hideUnmapped());
				Refresh();
			});
			_showMappedValue = AddRow("Show on maps with guides", 505, "Show when entering a mapped map");
			((Control)_showMappedValue).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_setShowMapped(!_showMapped());
				Refresh();
			});
			_hideLoadingValue = AddRow("Hide during loading screens", 540, "Also hides during cinematics and character selection");
			((Control)_hideLoadingValue).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_setHideLoading(!_hideLoading());
				Refresh();
			});
			_escapeValue = AddRow("Esc closes guide", 575, "Hide the guide when Escape is pressed");
			((Control)_escapeValue).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_setCloseOnEscape(!_closeOnEscape());
				Refresh();
			});
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)_window.Body);
			((Control)val3).set_Location(new Point(15, 615));
			((Control)val3).set_Size(new Point(470, 30));
			val3.set_Text("Auto-show content types");
			val3.set_TextColor(Color.get_White());
			string[] categories = new string[4] { "raids", "fractals", "metas", "dungeons" };
			for (int index = 0; index < categories.Length; index++)
			{
				string category = categories[index];
				string label = ((category == "metas") ? "Meta events" : (char.ToUpperInvariant(category[0]) + category.Substring(1)));
				int x = 24 + index % 2 * 220;
				int y = 650 + index / 2 * 38;
				Checkbox val4 = new Checkbox();
				((Control)val4).set_Parent((Container)(object)_window.Body);
				((Control)val4).set_Location(new Point(x, y));
				((Control)val4).set_Size(new Point(24, 28));
				val4.set_Checked(_categoryEnabled(category));
				((Control)val4).set_BasicTooltipText("Auto-show guides for " + ((category == "raids") ? "raids and strikes" : label));
				Checkbox checkbox = val4;
				Label val5 = new Label();
				((Control)val5).set_Parent((Container)(object)_window.Body);
				((Control)val5).set_Location(new Point(x + 26, y));
				((Control)val5).set_Size(new Point(180, 28));
				val5.set_Text(label);
				val5.set_TextColor(Color.get_LightGray());
				((Control)val5).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					checkbox.set_Checked(!checkbox.get_Checked());
				});
				_categoryButtons[category] = checkbox;
				checkbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object s, CheckChangedEvent e)
				{
					_setCategoryEnabled(category, e.get_Checked());
				});
			}
			Label val6 = new Label();
			((Control)val6).set_Parent((Container)(object)_window.Body);
			((Control)val6).set_Location(new Point(15, 740));
			((Control)val6).set_Size(new Point(480, 60));
			val6.set_Text("To change the shortcut, open the GW2 TLDR module settings in Blish HUD.");
			val6.set_TextColor(Color.get_LightGray());
			val6.set_WrapText(true);
			Refresh();
			((Control)_window).Show();
		}

		private DarkButton AddRow(string label, int y, string tooltip)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)_window.Body);
			((Control)val).set_Location(new Point(15, y));
			((Control)val).set_Size(new Point(320, 28));
			val.set_Text(label);
			val.set_TextColor(Color.get_LightGray());
			DarkButton darkButton = new DarkButton();
			((Control)darkButton).set_Parent((Container)(object)_window.Body);
			((Control)darkButton).set_Location(new Point(350, y));
			((Control)darkButton).set_Size(new Point(125, 26));
			((Control)darkButton).set_BasicTooltipText(tooltip);
			return darkButton;
		}

		private void Refresh()
		{
			((Label)_escapeValue).set_Text(_closeOnEscape() ? "On" : "Off");
			foreach (KeyValuePair<string, Checkbox> item in _categoryButtons)
			{
				item.Value.set_Checked(_categoryEnabled(item.Key));
			}
			((Label)_autoHeightValue).set_Text(_autoHeight() ? "On" : "Off");
			((Label)_hideLoadingValue).set_Text(_hideLoading() ? "On" : "Off");
			((Label)_hideUnmappedValue).set_Text(_hideUnmapped() ? "On" : "Off");
			((Label)_showMappedValue).set_Text(_showMapped() ? "On" : "Off");
			((Label)_opacityValue).set_Text(_opacity() + "%");
			((Label)_fontValue).set_Text((new string[3] { "Regular", "Bold", "Italic" })[Math.Max(0, Math.Min(2, _fontStyle()))]);
			((Label)_copyValue).set_Text(_showCopy() ? "On" : "Off");
			((Label)_textColorValue).set_Text(_colorText() ? "On" : "Off");
			((Label)_lightValue).set_Text(_light() ? "Light" : "Dark");
			((Label)_textValue).set_Text((new string[3] { "Small", "Medium", "Large" })[Math.Max(0, Math.Min(2, _textSize()))]);
			((Label)_colorsValue).set_Text(_colors() ? "On" : "Off");
			((Label)_fadeValue).set_Text(_fade() ? "On" : "Off");
			((Label)_autoValue).set_Text(_auto() ? "On" : "Off");
		}

		private void ToggleOpacityMenu()
		{
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			if (_opacityMenu != null)
			{
				CloseOpacityMenu();
				return;
			}
			SearchResultPanel searchResultPanel = new SearchResultPanel();
			((Control)searchResultPanel).set_Parent((Container)(object)_window.Body);
			((Control)searchResultPanel).set_Location(new Point(350, 80));
			((Control)searchResultPanel).set_Size(new Point(125, 245));
			((Control)searchResultPanel).set_ZIndex(100);
			((Panel)searchResultPanel).set_CanScroll(true);
			_opacityMenu = (Panel)(object)searchResultPanel;
			for (int opacityChoice = 0; opacityChoice <= 100; opacityChoice += 5)
			{
				int selected = opacityChoice;
				DarkButton darkButton = new DarkButton();
				((Control)darkButton).set_Parent((Container)(object)_opacityMenu);
				((Control)darkButton).set_Location(new Point(3, opacityChoice / 5 * 27));
				((Control)darkButton).set_Size(new Point(98, 25));
				((Label)darkButton).set_Text(selected + "%");
				((Control)darkButton).set_BasicTooltipText("Set background opacity to " + selected + "%");
				((Control)darkButton).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					CloseOpacityMenu();
					_setOpacity(selected);
					Refresh();
				});
			}
			_window.RefreshTheme();
		}

		private void CloseOpacityMenu()
		{
			Panel opacityMenu = _opacityMenu;
			if (opacityMenu != null)
			{
				((Control)opacityMenu).Dispose();
			}
			_opacityMenu = null;
		}

		private void OnHidden(object sender, EventArgs args)
		{
			CloseOpacityMenu();
		}

		public void Update()
		{
			_window.UpdateDrag();
		}

		public void Dispose()
		{
			CloseOpacityMenu();
			((Control)_window).remove_Hidden((EventHandler<EventArgs>)OnHidden);
			((Control)_window).Dispose();
		}
	}
}
