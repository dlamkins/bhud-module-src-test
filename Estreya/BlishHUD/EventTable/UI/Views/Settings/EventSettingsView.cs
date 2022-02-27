using System;
using System.Threading.Tasks;
using Blish_HUD.Controls;
using Blish_HUD.Settings;
using Estreya.BlishHUD.EventTable.Helpers;
using Microsoft.Xna.Framework.Graphics;

namespace Estreya.BlishHUD.EventTable.UI.Views.Settings
{
	public class EventSettingsView : BaseSettingsView
	{
		public EventSettingsView(ModuleSettings settings)
			: base(settings)
		{
		}

		protected override void InternalBuild(FlowPanel parent)
		{
			RenderSetting((Panel)(object)parent, (SettingEntry)(object)base.ModuleSettings.EventHeight);
			RenderSetting((Panel)(object)parent, (SettingEntry)(object)base.ModuleSettings.EventFontSize);
			RenderSetting((Panel)(object)parent, (SettingEntry)(object)base.ModuleSettings.EventTimeSpan);
			RenderSetting((Panel)(object)parent, (SettingEntry)(object)base.ModuleSettings.EventHistorySplit);
			RenderSetting((Panel)(object)parent, (SettingEntry)(object)base.ModuleSettings.DrawEventBorder);
			RenderEmptyLine((Panel)(object)parent);
			RenderSetting((Panel)(object)parent, (SettingEntry)(object)base.ModuleSettings.WorldbossCompletedAcion);
			RenderEmptyLine((Panel)(object)parent);
			RenderSetting((Panel)(object)parent, (SettingEntry)(object)base.ModuleSettings.AutomaticallyUpdateEventFile);
			RenderButton((Panel)(object)parent, "Update Event File", delegate
			{
				AsyncHelper.RunSync(EventTableModule.ModuleInstance.EventFileState.ExportFile);
				ScreenNotification.ShowNotification("Successfully updated!", (NotificationType)0, (Texture2D)null, 4);
			});
			RenderEmptyLine((Panel)(object)parent);
			RenderButton((Panel)(object)parent, "Reset hidden states", delegate
			{
				EventTableModule.ModuleInstance.HiddenState.Clear();
			});
			RenderEmptyLine((Panel)(object)parent);
			RenderSetting((Panel)(object)parent, (SettingEntry)(object)base.ModuleSettings.UseFiller);
			RenderSetting((Panel)(object)parent, (SettingEntry)(object)base.ModuleSettings.UseFillerEventNames);
			RenderColorSetting((Panel)(object)parent, base.ModuleSettings.FillerTextColor);
		}

		protected override Task<bool> InternalLoad(IProgress<string> progress)
		{
			return Task.FromResult(result: true);
		}
	}
}
