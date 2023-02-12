using System;
using System.Collections.Generic;
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
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_0149: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_016a: Expected O, but got Unknown
			((View<IPresenter>)this).Build(buildPanel);
			SettingComplianceExtensions.SetRange(_settings.BgOpacity, 0f, 1f);
			SettingComplianceExtensions.SetRange(_settings.LabelOpacity, 0.1f, 1f);
			SettingComplianceExtensions.SetRange(_settings.GridOpacity, 0.1f, 1f);
			FlowPanel panel = FlowPanelExtensions.BeginFlow(new FlowPanel(), buildPanel).AddSettingEnum((SettingEntry)(object)_settings.Layout).AddSettingEnum((SettingEntry)(object)_settings.FontSize)
				.AddSettingEnum((SettingEntry)(object)_settings.LabelDisplay)
				.AddSpace()
				.AddSetting((SettingEntry)(object)_settings.LabelOpacity)
				.AddSetting((SettingEntry)(object)_settings.GridOpacity)
				.AddSetting((SettingEntry)(object)_settings.BgOpacity)
				.AddSpace()
				.AddString(Strings.SettingsPanel_Raid_Visual_Colors)
				.AddString(Strings.SettingsPanel_Raid_Visual_ColorsTip)
				.AddSettingColor(_settings.Color.NotCleared)
				.AddSettingColor(_settings.Color.Cleared)
				.AddSettingColor(_settings.Color.Text)
				.AddSettingColor(_extraSettings);
			if (_showCopyRaids)
			{
				FlowPanel panel2 = panel.AddSpace();
				StandardButton val = new StandardButton();
				val.set_Text(Strings.Setting_CopyRaids);
				((Control)val).set_BasicTooltipText(Strings.Settngs_CopyRaidTooltip);
				panel2.AddFlowControl((Control)val, out var CopySettingsButton);
				CopySettingsButton.add_Click((EventHandler<MouseEventArgs>)delegate
				{
					Service.Settings.CopyRaidSettings(_settings);
					CopySettingsButton.set_Enabled(false);
				});
			}
		}
	}
}
