using System;
using System.Threading.Tasks;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Estreya.BlishHUD.EventTable.Models;

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
			RenderSetting<bool>(parent, base.ModuleSettings.GlobalEnabled);
			RenderSetting<KeyBinding>(parent, base.ModuleSettings.GlobalEnabledHotkey);
			RenderSetting<bool>(parent, base.ModuleSettings.RegisterCornerIcon);
			RenderEmptyLine(parent);
			RenderSetting<bool>(parent, base.ModuleSettings.HideOnOpenMap);
			RenderSetting<bool>(parent, base.ModuleSettings.HideOnMissingMumbleTicks);
			RenderSetting<bool>(parent, base.ModuleSettings.HideInCombat);
			RenderSetting<bool>(parent, base.ModuleSettings.ShowTooltips);
			RenderSetting<TooltipTimeMode>(parent, base.ModuleSettings.TooltipTimeMode);
			RenderSetting<bool>(parent, base.ModuleSettings.CopyWaypointOnClick);
			RenderSetting<bool>(parent, base.ModuleSettings.ShowContextMenuOnClick);
			RenderSetting<BuildDirection>(parent, base.ModuleSettings.BuildDirection);
		}

		protected override Task<bool> InternalLoad(IProgress<string> progress)
		{
			return Task.FromResult(result: true);
		}
	}
}
