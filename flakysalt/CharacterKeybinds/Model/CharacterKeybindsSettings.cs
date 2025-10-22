using System;
using System.Collections.Generic;
using System.IO;
using Blish_HUD;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using flakysalt.CharacterKeybinds.Data;

namespace flakysalt.CharacterKeybinds.Model
{
	public class CharacterKeybindsSettings
	{
		public SettingEntry<string> gw2KeybindsFolder;

		public SettingEntry<string> defaultKeybinds;

		public SettingEntry<KeyBinding> optionsKeybind;

		public SettingEntry<bool> useDefaultKeybinds;

		public SettingEntry<bool> changeKeybindsWhenSwitchingSpecialization;

		public SettingEntry<bool> displayCornerIcon;

		public SettingEntry<List<Keymap>> Keymaps;

		public SettingEntry<List<Point>> clickPositions;

		public SettingEntry<List<CharacterKeybind>> characterKeybinds;

		public SettingCollection settingsCollection { get; private set; }

		public SettingEntry<float> autoClickSpeedMultiplier { get; private set; }

		public SettingCollection internalSettingsCollection { get; private set; }

		private static string TargetFolderPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Guild Wars 2", "InputBinds");

		public CharacterKeybindsSettings(SettingCollection settings)
		{
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Expected O, but got Unknown
			settingsCollection = settings;
			gw2KeybindsFolder = settings.DefineSetting<string>("GW2 Keybind Path", TargetFolderPath, "Keybins Folder Path", "Path to the Keybinds folder.\nIf you dont know where this is, export your keybinds via the GW2 ingame menu and the location is shown in the ingame chat", (SettingTypeRendererDelegate)null);
			optionsKeybind = settings.DefineSetting<KeyBinding>("optionsKeybind", new KeyBinding((Keys)122), (Func<string>)(() => "Options Menu Keybind"), (Func<string>)null);
			useDefaultKeybinds = settings.DefineSetting<bool>("Use Default Keybinds", true, "Use Default Keybinds", "Switching to default keybinds when no others are defined for a character or specialization.", (SettingTypeRendererDelegate)null);
			changeKeybindsWhenSwitchingSpecialization = settings.DefineSetting<bool>("Change Keybinds When Switching Specialization", true, "Change keybinds When Switching Specialization", "Automatically change keybinds when switching elite specializations on the same character.", (SettingTypeRendererDelegate)null);
			displayCornerIcon = settings.DefineSetting<bool>("displayCornerIcon", true, "Show corner icon", "Show/Hide the corner icon to open the keybinds window.", (SettingTypeRendererDelegate)null);
			autoClickSpeedMultiplier = settings.DefineSetting<float>("autoClickSpeedMultiplier", 1f, "Keybindings Apply Speed", "Adjusts how fast the keybindings will be applied.\nLower (Left) this if your system is weaker and has trouble applying the keybindings.", (SettingTypeRendererDelegate)null);
			SettingComplianceExtensions.SetRange(autoClickSpeedMultiplier, 0.5f, 2.5f);
			internalSettingsCollection = settings.AddSubCollection("internal Settings", false);
			defaultKeybinds = internalSettingsCollection.DefineSetting<string>("defaultKeybinds", "", (Func<string>)null, (Func<string>)null);
			Keymaps = internalSettingsCollection.DefineSetting<List<Keymap>>("Keymaps", new List<Keymap>(), (Func<string>)null, (Func<string>)null);
			clickPositions = internalSettingsCollection.DefineSetting<List<Point>>("clickpos", ClickPositions.importClickPositions, (Func<string>)null, (Func<string>)null);
			characterKeybinds = internalSettingsCollection.DefineSetting<List<CharacterKeybind>>("keybinds", new List<CharacterKeybind>(), (Func<string>)null, (Func<string>)null);
		}

		public bool IsSaveFolderValid()
		{
			return Directory.Exists(TargetFolderPath);
		}
	}
}
