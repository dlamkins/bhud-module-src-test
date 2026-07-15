using System;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework.Input;

namespace Taskmaster.Settings
{
	public class ModuleSettings
	{
		public SettingEntry<KeyBinding> ToggleWindow { get; }

		public SettingEntry<bool> HideDone { get; }

		public SettingEntry<bool> LockTasks { get; }

		public SettingEntry<float> UnfocusedOpacity { get; }

		public SettingEntry<bool> ShowOnMap { get; }

		public ModuleSettings(SettingCollection settings)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Expected O, but got Unknown
			ToggleWindow = settings.DefineSetting<KeyBinding>("ToggleWindow", new KeyBinding((ModifierKeys)2, (Keys)84), (Func<string>)(() => "Show/hide window"), (Func<string>)(() => "Keybind to toggle the Taskmaster window."));
			HideDone = settings.DefineSetting<bool>("HideDone", false, (Func<string>)(() => "Hide completed tasks"), (Func<string>)(() => "Hide tasks that are already done, and hide tabs that are fully completed."));
			LockTasks = settings.DefineSetting<bool>("LockTasks", false, (Func<string>)(() => "Lock tasks"), (Func<string>)(() => "Prevent editing, adding, and deleting; checking still works."));
			UnfocusedOpacity = settings.DefineSetting<float>("UnfocusedOpacity", 1f, (Func<string>)(() => "Unfocused opacity"), (Func<string>)(() => "Window opacity when the mouse is not over it."));
			SettingComplianceExtensions.SetRange(UnfocusedOpacity, 0.2f, 1f);
			ShowOnMap = settings.DefineSetting<bool>("ShowOnMap", false, (Func<string>)(() => "Show on map"), (Func<string>)(() => "Keep the window visible while the full-screen map is open."));
		}
	}
}
