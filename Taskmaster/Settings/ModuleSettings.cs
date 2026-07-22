using System;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework.Input;

namespace Taskmaster.Settings
{
	public class ModuleSettings
	{
		public SettingEntry<KeyBinding> ToggleWindow { get; }

		public SettingEntry<float> UnfocusedOpacity { get; }

		public SettingEntry<bool> ShowOnMap { get; }

		public SettingEntry<float> InterfaceScale { get; }

		public SettingEntry<float> TextScale { get; }

		public SettingEntry<bool> EnableDragReordering { get; }

		public ModuleSettings(SettingCollection settings)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Expected O, but got Unknown
			ToggleWindow = settings.DefineSetting<KeyBinding>("ToggleWindow", new KeyBinding((ModifierKeys)2, (Keys)84), (Func<string>)(() => "Show/hide window"), (Func<string>)(() => "Keybind to toggle the Taskmaster window."));
			UnfocusedOpacity = settings.DefineSetting<float>("UnfocusedOpacity", 1f, (Func<string>)(() => "Unfocused opacity"), (Func<string>)(() => "Window opacity when the mouse is not over it."));
			SettingComplianceExtensions.SetRange(UnfocusedOpacity, 0.2f, 1f);
			ShowOnMap = settings.DefineSetting<bool>("ShowOnMap", false, (Func<string>)(() => "Show on map"), (Func<string>)(() => "Keep the window visible while the full-screen map is open."));
			InterfaceScale = settings.DefineSetting<float>("InterfaceScale", 1f, (Func<string>)(() => "Interface scale"), (Func<string>)(() => "Scale rows, buttons, icons, spacing, and minimum window size."));
			SettingComplianceExtensions.SetRange(InterfaceScale, 0.8f, 1.5f);
			TextScale = settings.DefineSetting<float>("TextScale", 1f, (Func<string>)(() => "Text scale"), (Func<string>)(() => "Choose larger or smaller native Blish HUD fonts throughout Taskmaster."));
			SettingComplianceExtensions.SetRange(TextScale, 0.85f, 1.4f);
			EnableDragReordering = settings.DefineSetting<bool>("EnableDragReordering", true, (Func<string>)(() => "Drag to reorder tasks"), (Func<string>)(() => "Allow dragging task and subtask rows to reorder them."));
		}
	}
}
