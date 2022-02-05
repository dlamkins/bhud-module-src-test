using System;
using System.Threading.Tasks;
using Blish_HUD.Controls;
using Blish_HUD.Settings;

namespace Estreya.BlishHUD.EventTable.UI.Views.Settings
{
	public class GeneralSettingsView : BaseSettingsView
	{
		public GeneralSettingsView(ModuleSettings settings)
			: base(settings)
		{
		}

		protected override void InternalBuild(FlowPanel parent)
		{
			RenderSetting((Panel)(object)parent, (SettingEntry)(object)base.ModuleSettings.GlobalEnabled);
			RenderSetting((Panel)(object)parent, (SettingEntry)(object)base.ModuleSettings.GlobalEnabledHotkey);
			RenderSetting((Panel)(object)parent, (SettingEntry)(object)base.ModuleSettings.RegisterCornerIcon);
			RenderEmptyLine((Panel)(object)parent);
			RenderSetting((Panel)(object)parent, (SettingEntry)(object)base.ModuleSettings.HideOnMissingMumbleTicks);
			RenderSetting((Panel)(object)parent, (SettingEntry)(object)base.ModuleSettings.HideInCombat);
			RenderSetting((Panel)(object)parent, (SettingEntry)(object)base.ModuleSettings.ShowTooltips);
			RenderSetting((Panel)(object)parent, (SettingEntry)(object)base.ModuleSettings.TooltipTimeMode);
			RenderSetting((Panel)(object)parent, (SettingEntry)(object)base.ModuleSettings.CopyWaypointOnClick);
			RenderSetting((Panel)(object)parent, (SettingEntry)(object)base.ModuleSettings.ShowContextMenuOnClick);
			RenderSetting((Panel)(object)parent, (SettingEntry)(object)base.ModuleSettings.BuildDirection);
		}

		protected override Task<bool> InternalLoad(IProgress<string> progress)
		{
			return Task.FromResult(result: true);
		}
	}
}
