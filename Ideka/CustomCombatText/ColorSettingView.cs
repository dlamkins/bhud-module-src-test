using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Settings;
using Blish_HUD.Settings.UI.Views;
using Gw2Sharp.WebApi.V2.Models;
using Ideka.BHUDCommon;
using Microsoft.Xna.Framework;

namespace Ideka.CustomCombatText
{
	public class ColorSettingView : SettingView<Color>
	{
		private class Inner : Color
		{
			public Color Color
			{
				get
				{
					//IL_0033: Unknown result type (might be due to invalid IL or missing references)
					return new Color(((Color)this).get_Cloth().get_Rgb()[0], ((Color)this).get_Cloth().get_Rgb()[1], ((Color)this).get_Cloth().get_Rgb()[2]);
				}
				set
				{
					SetRGB(((Color)(ref value)).get_R(), ((Color)(ref value)).get_G(), ((Color)(ref value)).get_B());
				}
			}

			public Inner()
				: this()
			{
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				((Color)this).set_BaseRgb((IReadOnlyList<int>)new List<int>(3));
				Color = Color.get_White();
			}

			public void SetName(string name)
			{
				((Color)this).set_Name(name);
			}

			public void SetRGB(int r = 0, int g = 0, int b = 0)
			{
				((Color)this).get_Cloth().set_Rgb((IReadOnlyList<int>)new _003C_003Ez__ReadOnlyArray<int>(new int[3] { r, g, b }));
				((Color)this).set_Name($"RGB: {r} {g} {b}");
			}
		}

		private const int Spacing = 5;

		private readonly SettingEntry<Color> _setting;

		private Label _label;

		private Inner _color;

		private ColorBox _colorBox;

		private TextBox _textBox;

		public ColorSettingView(SettingEntry<Color> setting, int definedWidth = -1)
		{
			_setting = setting;
			base._002Ector(setting, definedWidth);
		}

		protected override void BuildSetting(Container buildPanel)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Expected O, but got Unknown
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Expected O, but got Unknown
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Expected O, but got Unknown
			Label val = new Label();
			((Control)val).set_Parent(buildPanel);
			val.set_AutoSizeWidth(true);
			_label = val;
			_color = new Inner
			{
				Color = _setting.get_Value()
			};
			ColorBox val2 = new ColorBox();
			((Control)val2).set_Parent(buildPanel);
			val2.set_Color((Color)(object)_color);
			((Control)val2).set_Enabled(false);
			_colorBox = val2;
			TextBox val3 = new TextBox();
			((Control)val3).set_Parent(buildPanel);
			((TextInputBase)val3).set_MaxLength(6);
			_textBox = val3;
			((TextInputBase)_textBox).add_InputFocusChanged((EventHandler<ValueEventArgs<bool>>)TextBoxInputFocusChanged);
			((TextInputBase)_textBox).add_TextChanged((EventHandler<EventArgs>)TextBoxTextChanged);
		}

		private void TextBoxInputFocusChanged(object sender, ValueEventArgs<bool> e)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			if (!e.get_Value())
			{
				UpdateColorBox();
				base.OnValueChanged(new ValueEventArgs<Color>(_color.Color));
			}
		}

		private void TextBoxTextChanged(object sender, EventArgs e)
		{
			UpdateColorBox();
		}

		private void UpdateColorBox()
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			Color color = default(Color);
			if (((TextInputBase)_textBox).get_Text().Length == 6 && ColorUtil.TryParseHex(((TextInputBase)_textBox).get_Text(), ref color))
			{
				_color.Color = color;
				((Control)_textBox).set_BackgroundColor(new Color(0, 0, 0));
			}
			else
			{
				_color.SetName("Invalid Color");
				((Control)_textBox).set_BackgroundColor(new Color(128, 0, 0));
			}
		}

		private void UpdateSizeAndLayout()
		{
			((Control)((View<IPresenter>)(object)this).get_ViewTarget()).set_Height(((Control)_colorBox).get_Bottom());
			((Control)_textBox).set_Width(80);
			((Control)_label).set_Left(5);
			((Control)(object)_label).ArrangeLeftRight(5, (Control)_colorBox, (Control)_textBox);
			((Control)(object)_label).MiddleWith((Control)(object)_colorBox);
			((Control)(object)_textBox).MiddleWith((Control)(object)_colorBox);
			if (base.get_DefinedWidth() > 0)
			{
				((Control)((View<IPresenter>)(object)this).get_ViewTarget()).set_Width(((Control)_textBox).get_Right() + 5);
			}
		}

		protected override void RefreshDisplayName(string displayName)
		{
			_label.set_Text(displayName);
			UpdateSizeAndLayout();
		}

		protected override void RefreshDescription(string description)
		{
			((Control)_textBox).set_BasicTooltipText(description);
			((Control)_label).set_BasicTooltipText(description);
		}

		protected override void RefreshValue(Color value)
		{
			((TextInputBase)_textBox).set_Text($"{((Color)(ref value)).get_R():X2}{((Color)(ref value)).get_G():X2}{((Color)(ref value)).get_B():X2}");
			UpdateColorBox();
		}

		protected override void Unload()
		{
			((TextInputBase)_textBox).remove_InputFocusChanged((EventHandler<ValueEventArgs<bool>>)TextBoxInputFocusChanged);
			((TextInputBase)_textBox).remove_TextChanged((EventHandler<EventArgs>)TextBoxTextChanged);
		}
	}
}
