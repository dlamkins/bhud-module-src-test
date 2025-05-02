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

		public SettingEntry<bool> CustomCursorPosition { get; }

		public SettingEntry<string> CustomCursorPositionX { get; }

		public SettingEntry<string> CustomCursorPositionY { get; }

		public SettingService(SettingCollection settings)
		{
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Expected O, but got Unknown
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Expected O, but got Unknown
			KeybindBehaviour = settings.DefineSetting<KeybindBehaviour>("KeybindBahaviour", TargetYourFeet.Settings.Enums.KeybindBehaviour.Hold, (Func<string>)(() => "Mode"), (Func<string>)(() => "Pick a mode\nPress and Hold) Press to move mouse, release to return cursor to first position\nToggle) Tap to move mouse, Tap again to return cursor\nSingle) Press to move mouse"));
			TargetFeetKeybind = settings.DefineSetting<KeyBinding>("TargetFeetKeybind", new KeyBinding((Keys)0), (Func<string>)(() => Strings.Keybind_Label), (Func<string>)(() => Strings.Keybind_Tooltip));
			ActionCamInUse = settings.DefineSetting<bool>("ActionCamInUse", false, (Func<string>)(() => Strings.ActionCamInUse_Label), (Func<string>)(() => Strings.ActionCamInUse_Tooltip));
			ActionCamKeybind = settings.DefineSetting<KeyBinding>("ActionCamKeybind", new KeyBinding((Keys)165), (Func<string>)(() => Strings.ActionCamKeybind_Label), (Func<string>)(() => Strings.ActionCamKeybind_Tooltip));
			CustomCursorPosition = settings.DefineSetting<bool>("CustomCursorPosition", false, (Func<string>)(() => "Custom cursor position"), (Func<string>)(() => "When checked, the cursor will be moved to a custom position instead of the default position over your health globe."));
			CustomCursorPositionX = settings.DefineSetting<string>("CustomCursorPositionX", "0", (Func<string>)(() => "Custom X coordinate"), (Func<string>)(() => "The X coordinate of the custom cursor position. 0 is the left edge of the screen."));
			CustomCursorPositionY = settings.DefineSetting<string>("CustomCursorPositionY", "0", (Func<string>)(() => "Custom Y coordinate"), (Func<string>)(() => "The Y coordinate of the custom cursor position. 0 is the top edge of the screen."));
		}
	}
}
