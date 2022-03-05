using System;
using System.Threading.Tasks;
using Blish_HUD.Controls;
using Blish_HUD.Settings;
using Estreya.BlishHUD.EventTable.Controls;
using Estreya.BlishHUD.EventTable.Resources;

namespace Estreya.BlishHUD.EventTable.UI.Views.Settings
{
	public class EventSettingsView : BaseSettingsView
	{
		public EventSettingsView(ModuleSettings settings)
			: base(settings)
		{
		}

		protected override void InternalBuild(Panel parent)
		{
			RenderSetting(parent, (SettingEntry)(object)base.ModuleSettings.EventHeight);
			RenderSetting(parent, (SettingEntry)(object)base.ModuleSettings.EventFontSize);
			RenderSetting(parent, (SettingEntry)(object)base.ModuleSettings.EventTimeSpan);
			RenderSetting(parent, (SettingEntry)(object)base.ModuleSettings.EventHistorySplit);
			RenderSetting(parent, (SettingEntry)(object)base.ModuleSettings.DrawEventBorder);
			RenderSetting(parent, (SettingEntry)(object)base.ModuleSettings.UseEventTranslation);
			RenderEmptyLine(parent);
			RenderSetting(parent, (SettingEntry)(object)base.ModuleSettings.WorldbossCompletedAcion);
			RenderEmptyLine(parent);
			RenderSetting(parent, (SettingEntry)(object)base.ModuleSettings.AutomaticallyUpdateEventFile);
			RenderButton(parent, Strings.EventSettingsView_UpdateEventFile_Title, async delegate
			{
				await EventTableModule.ModuleInstance.EventFileState.ExportFile();
				ScreenNotification.ShowNotification(Strings.EventSettingsView_UpdateEventFile_Success, (NotificationType)0);
			});
			RenderEmptyLine(parent);
			RenderButton(parent, Strings.EventSettingsView_ResetHiddenStates_Title, delegate
			{
				EventTableModule.ModuleInstance.HiddenState.Clear();
				ScreenNotification.ShowNotification(Strings.EventSettingsView_ResetHiddenStates_Success, (NotificationType)0);
			});
			RenderEmptyLine(parent);
			RenderSetting(parent, (SettingEntry)(object)base.ModuleSettings.UseFiller);
			RenderSetting(parent, (SettingEntry)(object)base.ModuleSettings.UseFillerEventNames);
			RenderColorSetting(parent, base.ModuleSettings.FillerTextColor);
		}

		protected override Task<bool> InternalLoad(IProgress<string> progress)
		{
			return Task.FromResult(result: true);
		}
	}
}
