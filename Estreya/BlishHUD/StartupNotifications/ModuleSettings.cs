using System;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Estreya.BlishHUD.Shared.Controls;
using Estreya.BlishHUD.Shared.Settings;

namespace Estreya.BlishHUD.StartupNotifications
{
	public class ModuleSettings : BaseModuleSettings
	{
		public SettingEntry<int> Duration { get; set; }

		public SettingEntry<ScreenNotification.NotificationType> Type { get; set; }

		public SettingEntry<bool> AwaitEach { get; set; }

		public ModuleSettings(SettingCollection settings)
			: base(settings, new KeyBinding())
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Expected O, but got Unknown
			base.RegisterCornerIcon.set_Value(false);
		}

		protected override void DoInitializeGlobalSettings(SettingCollection globalSettingCollection)
		{
			Duration = globalSettingCollection.DefineSetting<int>("Duration", 5, (Func<string>)(() => "Duration"), (Func<string>)(() => "The duration each notification should be shown. (Range: 1-30 seconds)"));
			SettingComplianceExtensions.SetRange(Duration, 1, 30);
			Type = globalSettingCollection.DefineSetting<ScreenNotification.NotificationType>("Type", ScreenNotification.NotificationType.Info, (Func<string>)(() => "Type"), (Func<string>)(() => "The type each notification should be shown as."));
			SettingComplianceExtensions.SetIncluded<ScreenNotification.NotificationType>(Type, new ScreenNotification.NotificationType[3]
			{
				ScreenNotification.NotificationType.Info,
				ScreenNotification.NotificationType.Warning,
				ScreenNotification.NotificationType.Error
			});
			AwaitEach = globalSettingCollection.DefineSetting<bool>("AwaitEach", true, (Func<string>)(() => "Await Each"), (Func<string>)(() => "If each notification should be awaited before the next one is shown."));
		}
	}
}
