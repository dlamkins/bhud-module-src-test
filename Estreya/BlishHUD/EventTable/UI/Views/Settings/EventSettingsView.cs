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

		protected override void InternalBuild(Panel parent)
		{
			RenderSetting(parent, (SettingEntry)(object)base.ModuleSettings.EventHeight);
			RenderSetting(parent, (SettingEntry)(object)base.ModuleSettings.EventFontSize);
			RenderSetting(parent, (SettingEntry)(object)base.ModuleSettings.EventTimeSpan);
			RenderSetting(parent, (SettingEntry)(object)base.ModuleSettings.EventHistorySplit);
			RenderSetting(parent, (SettingEntry)(object)base.ModuleSettings.DrawEventBorder);
			RenderEmptyLine(parent);
			RenderSetting(parent, (SettingEntry)(object)base.ModuleSettings.WorldbossCompletedAcion);
			RenderEmptyLine(parent);
			RenderSetting(parent, (SettingEntry)(object)base.ModuleSettings.AutomaticallyUpdateEventFile);
			RenderButton(parent, "Update Event File", delegate
			{
				AsyncHelper.RunSync(EventTableModule.ModuleInstance.EventFileState.ExportFile);
				ScreenNotification.ShowNotification("Successfully updated!", (NotificationType)0, (Texture2D)null, 4);
			});
			RenderEmptyLine(parent);
			RenderButton(parent, "Reset hidden states", delegate
			{
				EventTableModule.ModuleInstance.HiddenState.Clear();
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
