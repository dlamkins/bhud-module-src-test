using System;
using System.Text.RegularExpressions;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using RaidClears.Raids.Model;

namespace Blish_HUD.Settings.UI.Views
{
	public class ColorSettingView : SettingView<string>
	{
		private const int CONTROL_PADDING = 5;

		private const int TEXTBOX_WIDTH = 250;

		private const int TEXTBOX_HEIGHT = 27;

		private SettingEntry<string> _setting;

		private Label _displayNameLabel;

		private TextBox _stringTextbox;

		private ColorBox _colorBox;

		private ColorHelper _colorHelper;

		public ColorSettingView(SettingEntry<string> setting, int definedWidth = -1)
			: base(setting, definedWidth)
		{
			_setting = setting;
		}

		public override bool HandleComplianceRequisite(IComplianceRequisite complianceRequisite)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (complianceRequisite is SettingDisabledComplianceRequisite)
			{
				SettingDisabledComplianceRequisite settingDisabledComplianceRequisite = (SettingDisabledComplianceRequisite)(object)complianceRequisite;
				((Control)_displayNameLabel).set_Enabled(!((SettingDisabledComplianceRequisite)(ref settingDisabledComplianceRequisite)).get_Disabled());
				((Control)_stringTextbox).set_Enabled(!((SettingDisabledComplianceRequisite)(ref settingDisabledComplianceRequisite)).get_Disabled());
				return true;
			}
			return false;
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
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Expected O, but got Unknown
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Expected O, but got Unknown
			TextBox val = new TextBox();
			((Control)val).set_Size(new Point(80, 27));
			((Control)val).set_Location(new Point(5, 3));
			((Control)val).set_Parent(buildPanel);
			((TextInputBase)val).set_MaxLength(7);
			_stringTextbox = val;
			_colorHelper = new ColorHelper();
			_colorHelper.setRGB(_setting.get_Value());
			ColorBox val2 = new ColorBox();
			((Control)val2).set_Location(new Point(90, 0));
			val2.set_Color((Color)(object)_colorHelper);
			((Control)val2).set_Parent(buildPanel);
			((Control)val2).set_Enabled(false);
			_colorBox = val2;
			Label val3 = new Label();
			val3.set_AutoSizeWidth(true);
			((Control)val3).set_Location(new Point(130, 0));
			((Control)val3).set_Parent(buildPanel);
			_displayNameLabel = val3;
			((TextInputBase)_stringTextbox).add_InputFocusChanged((EventHandler<ValueEventArgs<bool>>)StringTextboxOnInputFocusChanged);
			((TextInputBase)_stringTextbox).add_TextChanged((EventHandler<EventArgs>)TextChangedEventHandler);
		}

		private void StringTextboxOnInputFocusChanged(object sender, ValueEventArgs<bool> e)
		{
			if (!e.get_Value())
			{
				base.OnValueChanged(new ValueEventArgs<string>(((TextInputBase)_stringTextbox).get_Text()));
				updateColorBox(((TextInputBase)_stringTextbox).get_Text());
			}
		}

		private void TextChangedEventHandler(object sender, EventArgs e)
		{
			updateColorBox(((TextInputBase)_stringTextbox).get_Text());
		}

		private void updateColorBox(string text)
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			if (Regex.Match(text, "([a-fA-F0-9]{6})").Success)
			{
				_colorHelper.setRGB(text);
				((Control)_stringTextbox).set_BackgroundColor(new Color(0, 0, 0));
			}
			else
			{
				_colorHelper.setRGB(255, 255, 255);
				_colorHelper.setName("Invalid Color");
				((Control)_stringTextbox).set_BackgroundColor(new Color(128, 0, 0));
			}
		}

		private void UpdateSizeAndLayout()
		{
			((Control)((View<IPresenter>)(object)this).get_ViewTarget()).set_Height(((Control)_stringTextbox).get_Bottom());
			((Control)_displayNameLabel).set_Height(((Control)_stringTextbox).get_Bottom());
		}

		protected override void RefreshDisplayName(string displayName)
		{
			_displayNameLabel.set_Text(displayName);
			UpdateSizeAndLayout();
		}

		protected override void RefreshDescription(string description)
		{
			((Control)_stringTextbox).set_BasicTooltipText(description);
			((Control)_displayNameLabel).set_BasicTooltipText(description);
		}

		protected override void RefreshValue(string value)
		{
			((TextInputBase)_stringTextbox).set_Text(value);
			updateColorBox(value);
		}

		protected override void Unload()
		{
			if (_stringTextbox != null)
			{
				((TextInputBase)_stringTextbox).remove_InputFocusChanged((EventHandler<ValueEventArgs<bool>>)StringTextboxOnInputFocusChanged);
				((TextInputBase)_stringTextbox).remove_TextChanged((EventHandler<EventArgs>)TextChangedEventHandler);
			}
		}
	}
}
