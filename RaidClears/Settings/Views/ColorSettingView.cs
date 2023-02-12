using System;
using System.Text.RegularExpressions;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Settings;
using Blish_HUD.Settings.UI.Views;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using RaidClears.Settings.Models;

namespace RaidClears.Settings.Views
{
	public class ColorSettingView : SettingView<string>
	{
		private readonly SettingEntry<string> _setting;

		private Label _displayNameLabel;

		private TextBox _stringTextBox;

		private ColorHelper _colorHelper;

		private ColorBox _colorBox;

		public ColorSettingView(SettingEntry<string> setting, int definedWidth = -1)
			: base(setting, definedWidth)
		{
			_setting = setting;
		}

		protected override void BuildSetting(Container buildPanel)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Expected O, but got Unknown
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Expected O, but got Unknown
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Expected O, but got Unknown
			TextBox val = new TextBox();
			((Control)val).set_Size(new Point(80, 27));
			((Control)val).set_Location(new Point(5, 3));
			((Control)val).set_Parent(buildPanel);
			((TextInputBase)val).set_MaxLength(7);
			_stringTextBox = val;
			_colorHelper = new ColorHelper();
			_colorHelper.SetRGB(_setting.get_Value());
			_colorBox = new ColorBox();
			((Control)_colorBox).set_Location(new Point(90, 0));
			_colorBox.set_Color((Color)(object)_colorHelper);
			((Control)_colorBox).set_Parent(buildPanel);
			((Control)_colorBox).set_Enabled(false);
			Label val2 = new Label();
			val2.set_AutoSizeWidth(true);
			((Control)val2).set_Location(new Point(130, 0));
			((Control)val2).set_Parent(buildPanel);
			_displayNameLabel = val2;
			((TextInputBase)_stringTextBox).add_InputFocusChanged((EventHandler<ValueEventArgs<bool>>)StringTextBoxOnInputFocusChanged);
			((TextInputBase)_stringTextBox).add_TextChanged((EventHandler<EventArgs>)TextChangedEventHandler);
		}

		private void StringTextBoxOnInputFocusChanged(object sender, ValueEventArgs<bool> e)
		{
			if (!e.get_Value())
			{
				base.OnValueChanged(new ValueEventArgs<string>(((TextInputBase)_stringTextBox).get_Text()));
				UpdateColorBox(((TextInputBase)_stringTextBox).get_Text());
			}
		}

		private void TextChangedEventHandler(object sender, EventArgs e)
		{
			UpdateColorBox(((TextInputBase)_stringTextBox).get_Text());
		}

		private void UpdateColorBox(string text)
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			if (Regex.Match(text, "([a-fA-F0-9]{6})").Success)
			{
				_colorHelper.SetRGB(text);
				((Control)_stringTextBox).set_BackgroundColor(new Color(0, 0, 0));
			}
			else
			{
				_colorHelper.SetRGB(255, 255, 255);
				_colorHelper.SetName("Invalid Color");
				((Control)_stringTextBox).set_BackgroundColor(new Color(128, 0, 0));
			}
		}

		private void UpdateSizeAndLayout()
		{
			((Control)((View<IPresenter>)(object)this).get_ViewTarget()).set_Height(((Control)_stringTextBox).get_Bottom());
			((Control)_displayNameLabel).set_Height(((Control)_stringTextBox).get_Bottom());
		}

		protected override void RefreshDisplayName(string displayName)
		{
			_displayNameLabel.set_Text(displayName);
			UpdateSizeAndLayout();
		}

		protected override void RefreshDescription(string description)
		{
			((Control)_stringTextBox).set_BasicTooltipText(description);
			((Control)_displayNameLabel).set_BasicTooltipText(description);
		}

		protected override void RefreshValue(string value)
		{
			((TextInputBase)_stringTextBox).set_Text(value);
			UpdateColorBox(value);
		}

		protected override void Unload()
		{
			((TextInputBase)_stringTextBox).remove_InputFocusChanged((EventHandler<ValueEventArgs<bool>>)StringTextBoxOnInputFocusChanged);
			((TextInputBase)_stringTextBox).remove_TextChanged((EventHandler<EventArgs>)TextChangedEventHandler);
		}
	}
}
