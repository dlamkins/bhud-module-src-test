using System;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using GatheringTools.LogoutOverlay;
using Microsoft.Xna.Framework.Input;

namespace GatheringTools.Services
{
	public class SettingService
	{
		public SettingEntry<bool> ReminderScreenNotificationIsEnabled { get; set; }

		public SettingEntry<int> ReminderWindowSizeSetting { get; }

		public SettingEntry<int> ReminderWindowOffsetXSetting { get; set; }

		public SettingEntry<int> ReminderWindowOffsetYSetting { get; set; }

		public int ReminderWindowOffsetX => ReminderWindowOffsetXSetting.get_Value();

		public int ReminderWindowOffsetY => ReminderWindowOffsetYSetting.get_Value();

		public SettingEntry<DisplayDuration> ReminderDisplayDurationInSecondsSetting { get; }

		public SettingEntry<string> ReminderTextSetting { get; }

		public SettingEntry<int> ReminderTextFontSizeIndexSetting { get; }

		public SettingEntry<int> ReminderIconSizeSetting { get; }

		public SettingEntry<bool> ReminderIconsAreVisibleSettings { get; set; }

		public SettingEntry<bool> EscIsHidingReminderSetting { get; }

		public SettingEntry<bool> EnterIsHidingReminderSetting { get; }

		public SettingEntry<bool> ReminderIsVisibleForSetupSetting { get; }

		public SettingEntry<bool> ShowToolSearchCornerIconSetting { get; }

		public SettingEntry<bool> ShowOnlyUnlimitedToolsSetting { get; }

		public SettingEntry<bool> ShowSharedInventoryToolsSetting { get; }

		public SettingEntry<bool> ShowBankToolsSetting { get; }

		public SettingEntry<KeyBinding> ToolSearchKeyBindingSetting { get; }

		public SettingEntry<KeyBinding> LogoutKeyBindingSetting { get; }

		public SettingService(SettingCollection settings)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Expected O, but got Unknown
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Expected O, but got Unknown
			LogoutKeyBindingSetting = settings.DefineSetting<KeyBinding>("Logout key binding", new KeyBinding((Keys)123), (Func<string>)(() => "Logout (must match ingame key)"), (Func<string>)(() => "Double-click to change it. Logout key has to match the ingame logout key (default F12)."));
			ToolSearchKeyBindingSetting = settings.DefineSetting<KeyBinding>("tool search key binding", new KeyBinding((ModifierKeys)2, (Keys)67), (Func<string>)(() => "tool search window"), (Func<string>)(() => "Double-click to change it. Will show or hide the tool search window."));
			ReminderTextSetting = settings.DefineSetting<string>("text (logout overlay)", "Don't forget Tools!", (Func<string>)(() => "reminder text"), (Func<string>)(() => "text shown inside the reminder window"));
			ReminderDisplayDurationInSecondsSetting = settings.DefineSetting<DisplayDuration>("display duration (logout overlay)", DisplayDuration.Seconds3, (Func<string>)(() => "reminder display duration"), (Func<string>)(() => "The reminder will disappear automatically after this time has expired.\nThis setting is ignored when the 'hide on ESC/ENTER' setting is enabled"));
			ReminderWindowSizeSetting = settings.DefineSetting<int>("window size (logout overlay)", 34, (Func<string>)(() => "reminder size"), (Func<string>)(() => "Change reminder window size to fit to the size of the logout dialog with your current screen settings"));
			SettingComplianceExtensions.SetRange(ReminderWindowSizeSetting, 1, 100);
			ReminderWindowOffsetXSetting = settings.DefineSetting<int>("reminder window offset x", 0, (Func<string>)(() => "reminder X position"), (Func<string>)(() => "Change the reminder window position relatively to the logout dialog.\nPosition of the additional reminder hint is not affected."));
			SettingComplianceExtensions.SetRange(ReminderWindowOffsetXSetting, -1000, 1000);
			ReminderWindowOffsetYSetting = settings.DefineSetting<int>("reminder window offset y", 0, (Func<string>)(() => "reminder Y position"), (Func<string>)(() => "Change the reminder window position relatively to the logout dialog.\nPosition of the additional reminder hint is not affected."));
			SettingComplianceExtensions.SetRange(ReminderWindowOffsetYSetting, -1000, 1000);
			ReminderTextFontSizeIndexSetting = FontService.CreateFontSizeIndexSetting(settings);
			ReminderIconSizeSetting = settings.DefineSetting<int>("reminder icon size (logout overlay)", 60, (Func<string>)(() => "reminder icon size"), (Func<string>)(() => "Change size of the icons in the reminder window"));
			SettingComplianceExtensions.SetRange(ReminderIconSizeSetting, 10, 300);
			ReminderIconsAreVisibleSettings = settings.DefineSetting<bool>("show reminder icons", false, (Func<string>)(() => "show icons in reminder"), (Func<string>)(() => "Show gathering tool icons in the reminder.\nThe icons are static placeholders. They do not represent the tools your character has equipped. Sadly the API would be too slow for updating those correctly."));
			ReminderScreenNotificationIsEnabled = settings.DefineSetting<bool>("reminder screen notification is enabled", true, (Func<string>)(() => "additional reminder hint"), (Func<string>)(() => "Show an additional floating reminder hint above the logout dialog.\nThis is hint is not affected by 'hide on ESC/ENTER'."));
			EscIsHidingReminderSetting = settings.DefineSetting<bool>("hide on ESC", true, (Func<string>)(() => "hide on ESC"), (Func<string>)(() => "When you press ESC to close the logout dialog, the reminder will be hidden, too"));
			EnterIsHidingReminderSetting = settings.DefineSetting<bool>("hide on ENTER", true, (Func<string>)(() => "hide on ENTER"), (Func<string>)(() => "When you press ENTER to switch to the character selection, the reminder will be hidden"));
			ReminderIsVisibleForSetupSetting = settings.DefineSetting<bool>("show reminder for setup", false, (Func<string>)(() => "show reminder permanently for setup"), (Func<string>)(() => "show reminder for easier setup of position etc. This will ignore display duration and ESC or ENTER being pressed. Do not forget to uncheck after you set up everything."));
			ShowToolSearchCornerIconSetting = settings.DefineSetting<bool>("show tool search corner icon", true, (Func<string>)(() => "show sickle icon"), (Func<string>)(() => "Show sickle icon at the top left of GW2 next to other menu icons. Icon can be clicked to show/hide the gathering tool search window"));
			SettingCollection internalSettingSubCollection = settings.AddSubCollection("internal settings (not visible in UI)", false);
			ShowOnlyUnlimitedToolsSetting = internalSettingSubCollection.DefineSetting<bool>("only unlimited tools", true, (Func<string>)null, (Func<string>)null);
			ShowBankToolsSetting = internalSettingSubCollection.DefineSetting<bool>("show bank tools", true, (Func<string>)null, (Func<string>)null);
			ShowSharedInventoryToolsSetting = internalSettingSubCollection.DefineSetting<bool>("show shared inventory tools", true, (Func<string>)null, (Func<string>)null);
		}
	}
}
