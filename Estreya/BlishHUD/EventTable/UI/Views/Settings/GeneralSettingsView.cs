using System;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Estreya.BlishHUD.EventTable.Controls;
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
			if (!EventTableModule.ModuleInstance.Debug)
			{
				return;
			}
			RenderEmptyLine(parent);
			RenderButton(parent, "Test Error", delegate
			{
				ShowError("New error" + new Random().Next());
			});
			RenderTextbox(parent, "Finish Event", "Event.Key", delegate(string val)
			{
				EventTableModule.ModuleInstance.EventCategories.SelectMany((EventCategory ec) => ec.Events.Where((Event ev) => ev.Key == val)).ToList().ForEach(delegate(Event ev)
				{
					ev.Finish();
				});
			});
			RenderTextbox(parent, "Finish Category", "EventCategory.Key", delegate(string val)
			{
				EventTableModule.ModuleInstance.EventCategories.Where((EventCategory ec) => ec.Key == val).ToList().ForEach(delegate(EventCategory ev)
				{
					ev.Finish();
				});
			});
			RenderEmptyLine(parent);
			RenderButton(parent, "Clear States", async delegate
			{
				await EventTableModule.ModuleInstance.ClearStates();
				ScreenNotification.ShowNotification("States cleared", (NotificationType)0);
			});
			RenderButton(parent, "Reload States", async delegate
			{
				await EventTableModule.ModuleInstance.ReloadStates();
				ScreenNotification.ShowNotification("States reloaded", (NotificationType)0);
			});
		}

		protected override Task<bool> InternalLoad(IProgress<string> progress)
		{
			return Task.FromResult(result: true);
		}
	}
}
