using System;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework.Input;
using TargetYourFeet.Localization;
using TargetYourFeet.Settings.Enums;

namespace TargetYourFeet.Settings.Services
{
	public class SettingService
	{
		public SettingEntry<KeybindBehaviour> KeybindBehaviour { get; }

		public SettingEntry<KeyBinding> TargetFeetKeybind { get; }

		public SettingEntry<bool> ActionCamInUse { get; }

		public SettingEntry<KeyBinding> ActionCamKeybind { get; }

		public SettingService(SettingCollection settings)
		{
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Expected O, but got Unknown
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Expected O, but got Unknown
			KeybindBehaviour = settings.DefineSetting<KeybindBehaviour>("KeybindBahaviour", TargetYourFeet.Settings.Enums.KeybindBehaviour.Hold, (Func<string>)(() => "Mode"), (Func<string>)(() => "Pick a mode\nHold) Press to move mouse, release to return cursor to first psotion\nToggle) Tap to move mouse, Tap again to return cursor\nSingle) Press to move mouse"));
			TargetFeetKeybind = settings.DefineSetting<KeyBinding>("TargetFeetKeybind", new KeyBinding((Keys)0), (Func<string>)(() => Strings.Keybind_Label), (Func<string>)(() => Strings.Keybind_Tooltip));
			ActionCamInUse = settings.DefineSetting<bool>("ActionCamInUse", false, (Func<string>)(() => Strings.ActionCamInUse_Label), (Func<string>)(() => Strings.ActionCamInUse_Tooltip));
			ActionCamKeybind = settings.DefineSetting<KeyBinding>("ActionCamKeybind", new KeyBinding((Keys)165), (Func<string>)(() => Strings.ActionCamKeybind_Label), (Func<string>)(() => Strings.ActionCamKeybind_Tooltip));
		}
	}
}
