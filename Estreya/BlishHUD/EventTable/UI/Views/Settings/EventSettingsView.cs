using System;
using System.IO;
using System.Threading.Tasks;
using Blish_HUD.Controls;
using Estreya.BlishHUD.EventTable.Controls;
using Estreya.BlishHUD.EventTable.Helpers;
using Estreya.BlishHUD.EventTable.Models;
using Estreya.BlishHUD.EventTable.Resources;
using Estreya.BlishHUD.EventTable.Utils;
using Newtonsoft.Json;
using SemVer;

namespace Estreya.BlishHUD.EventTable.UI.Views.Settings
{
	public class EventSettingsView : BaseSettingsView
	{
		private SemVer.Version CurrentVersion;

		private SemVer.Version NewestVersion;

		public EventSettingsView(ModuleSettings settings)
			: base(settings)
		{
		}

		protected override void BuildView(Panel parent)
		{
			RenderSetting<string>(parent, base.ModuleSettings.EventTimeSpan);
			RenderSetting<int>(parent, base.ModuleSettings.EventHistorySplit);
			RenderSetting<bool>(parent, base.ModuleSettings.DrawEventBorder);
			RenderSetting<bool>(parent, base.ModuleSettings.UseEventTranslation);
			RenderEmptyLine(parent);
			RenderSetting<EventCompletedAction>(parent, base.ModuleSettings.EventCompletedAcion);
			RenderEmptyLine(parent);
			RenderButton(parent, Strings.EventSettingsView_ReloadEvents_Title, async delegate
			{
				await EventTableModule.ModuleInstance.LoadEvents();
				ScreenNotification.ShowNotification(Strings.EventSettingsView_ReloadEvents_Success, (NotificationType)0);
			});
			RenderEmptyLine(parent);
			RenderSetting<bool>(parent, base.ModuleSettings.AutomaticallyUpdateEventFile);
			RenderButton(parent, Strings.EventSettingsView_UpdateEventFile_Title, async delegate
			{
				await EventTableModule.ModuleInstance.EventFileState.ExportFile();
				await EventTableModule.ModuleInstance.LoadEvents();
				ScreenNotification.ShowNotification(Strings.EventSettingsView_UpdateEventFile_Success, (NotificationType)0);
			});
			RenderLabel(parent, Strings.EventSettingsView_CurrentVersion_Title, CurrentVersion?.ToString() ?? Strings.EventSettingsView_CurrentVersion_Unknown);
			RenderLabel(parent, Strings.EventSettingsView_NewestVersion_Title, NewestVersion?.ToString() ?? Strings.EventSettingsView_NewestVersion_Unknown);
			RenderButton(parent, "Diff in VS Code", async delegate
			{
				string filePath1 = FileUtil.CreateTempFile("json");
				await FileUtil.WriteStringAsync(filePath1, JsonConvert.SerializeObject((object)(await EventTableModule.ModuleInstance.EventFileState.GetExternalFile()), (Formatting)1));
				string filePath2 = FileUtil.CreateTempFile("json");
				await FileUtil.WriteStringAsync(filePath2, JsonConvert.SerializeObject((object)(await EventTableModule.ModuleInstance.EventFileState.GetInternalFile()), (Formatting)1));
				await VSCodeHelper.Diff(filePath1, filePath2);
				File.Delete(filePath1);
				File.Delete(filePath2);
			});
			RenderEmptyLine(parent);
			RenderButton(parent, Strings.EventSettingsView_ResetEventStates_Title, async delegate
			{
				await EventTableModule.ModuleInstance.EventState.Clear();
				ScreenNotification.ShowNotification(Strings.EventSettingsView_ResetEventStates_Success, (NotificationType)0);
			});
			RenderEmptyLine(parent);
			RenderSetting<bool>(parent, base.ModuleSettings.UseFiller);
			RenderSetting<bool>(parent, base.ModuleSettings.UseFillerEventNames);
			RenderColorSetting(parent, base.ModuleSettings.FillerTextColor);
		}

		protected override async Task<bool> InternalLoad(IProgress<string> progress)
		{
			CurrentVersion = (await EventTableModule.ModuleInstance.EventFileState.GetExternalFile())?.Version;
			NewestVersion = (await EventTableModule.ModuleInstance.EventFileState.GetInternalFile())?.Version;
			return true;
		}
	}
}
