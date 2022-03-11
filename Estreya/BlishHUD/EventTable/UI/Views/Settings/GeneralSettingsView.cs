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

		protected override void InternalBuild(Panel parent)
		{
			RenderSetting(parent, (SettingEntry)(object)base.ModuleSettings.GlobalEnabled);
			RenderSetting(parent, (SettingEntry)(object)base.ModuleSettings.GlobalEnabledHotkey);
			RenderSetting(parent, (SettingEntry)(object)base.ModuleSettings.RegisterCornerIcon);
			RenderEmptyLine(parent);
			RenderSetting(parent, (SettingEntry)(object)base.ModuleSettings.HideOnOpenMap);
			RenderSetting(parent, (SettingEntry)(object)base.ModuleSettings.HideOnMissingMumbleTicks);
			RenderSetting(parent, (SettingEntry)(object)base.ModuleSettings.HideInCombat);
			RenderSetting(parent, (SettingEntry)(object)base.ModuleSettings.ShowTooltips);
			RenderSetting(parent, (SettingEntry)(object)base.ModuleSettings.TooltipTimeMode);
			RenderSetting(parent, (SettingEntry)(object)base.ModuleSettings.CopyWaypointOnClick);
			RenderSetting(parent, (SettingEntry)(object)base.ModuleSettings.ShowContextMenuOnClick);
			RenderSetting(parent, (SettingEntry)(object)base.ModuleSettings.BuildDirection);
		}

		protected override Task<bool> InternalLoad(IProgress<string> progress)
		{
			return Task.FromResult(result: true);
		}
	}
}
