using System;
using System.Collections.Generic;
using System.Diagnostics;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using RaidClears.Localization;
using RaidClears.Settings.Models;
using RaidClears.Utils;

namespace RaidClears.Settings.Views.SubViews.Generics
{
	public class GenericStyleView : View
	{
		private readonly DisplayStyle _settings;

		private readonly IEnumerable<SettingEntry<string>>? _extraSettings;

		private bool _showCopyRaids;

		public GenericStyleView(DisplayStyle settings, IEnumerable<SettingEntry<string>>? extraSettings = null, bool showCopyRaids = false)
			: this()
		{
			_settings = settings;
			_extraSettings = extraSettings;
			_showCopyRaids = showCopyRaids;
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Expected O, but got Unknown
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Expected O, but got Unknown
			//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d8: Expected O, but got Unknown
			((View<IPresenter>)this).Build(buildPanel);
			SettingComplianceExtensions.SetRange(_settings.BgOpacity, 0f, 1f);
			SettingComplianceExtensions.SetRange(_settings.LabelOpacity, 0.1f, 1f);
			SettingComplianceExtensions.SetRange(_settings.GridOpacity, 0.1f, 1f);
			FlowPanel panel2 = FlowPanelExtensions.BeginFlow(new FlowPanel(), buildPanel).AddSettingEnum((SettingEntry)(object)_settings.Layout).AddSettingEnum((SettingEntry)(object)_settings.FontSize)
				.AddSettingEnum((SettingEntry)(object)_settings.LabelDisplay)
				.AddSpace()
				.AddSetting((SettingEntry)(object)_settings.LabelOpacity)
				.AddSetting((SettingEntry)(object)_settings.GridOpacity)
				.AddSetting((SettingEntry)(object)_settings.BgOpacity)
				.AddSpace()
				.AddString(Strings.SettingsPanel_Raid_Visual_Colors);
			StandardButton val = new StandardButton();
			val.set_Text(Strings.SettingsPanel_Raid_Visual_ColorPickerButton);
			((Control)val).set_BasicTooltipText(Strings.SettingsPanel_Raid_Visual_ColorPickerButtonTooltip);
			((Control)val).set_Width(300);
			Control colorPickerButton;
			FlowPanel panel = panel2.AddFlowControl((Control)val, out colorPickerButton).AddSettingColor(_settings.Color.NotCleared).AddSettingColor(_settings.Color.Cleared)
				.AddSettingColor(_settings.Color.Text)
				.AddSettingColor(_extraSettings);
			colorPickerButton.add_Click((EventHandler<MouseEventArgs>)delegate
			{
				Process.Start(new ProcessStartInfo
				{
					FileName = "https://htmlcolorcodes.com/color-picker/",
					UseShellExecute = true
				});
			});
			if (_showCopyRaids)
			{
				FlowPanel panel3 = panel.AddSpace();
				StandardButton val2 = new StandardButton();
				val2.set_Text(Strings.Setting_CopyRaids);
				((Control)val2).set_BasicTooltipText(Strings.Settngs_CopyRaidTooltip);
				((Control)val2).set_Width(300);
				panel3.AddFlowControl((Control)val2, out var CopySettingsButton);
				CopySettingsButton.add_Click((EventHandler<MouseEventArgs>)delegate
				{
					Service.Settings.CopyRaidSettings(_settings);
					CopySettingsButton.set_Enabled(false);
				});
			}
		}
	}
}
