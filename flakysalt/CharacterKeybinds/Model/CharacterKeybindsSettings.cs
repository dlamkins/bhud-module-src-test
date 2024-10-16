using System;
using System.Collections.Generic;
using System.IO;
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

		public SettingEntry<bool> changeKeybindsWhenSwitchingSpecialization;

		public SettingEntry<bool> displayCornerIcon;

		public SettingEntry<List<CharacterKeybind>> characterKeybinds;

		public SettingEntry<List<Point>> clickPositions;

		public SettingCollection settingsCollection { get; private set; }

		public SettingEntry<float> autoClickSpeedMultiplier { get; private set; }

		public SettingCollection internalSettingsCollection { get; private set; }

		public CharacterKeybindsSettings(SettingCollection settings)
		{
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Expected O, but got Unknown
			settingsCollection = settings;
			string targetFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Guild Wars 2", "InputBinds");
			gw2KeybindsFolder = settings.DefineSetting<string>("GW2 Keybind Path", targetFolderPath, (Func<string>)(() => "Path to the Keybinds folder. Usually somewhere inside your documents folder"), (Func<string>)(() => ""));
			optionsKeybind = settings.DefineSetting<KeyBinding>("optionsKeybind", new KeyBinding((Keys)122), (Func<string>)(() => "Options Menu Keybind"), (Func<string>)null);
			changeKeybindsWhenSwitchingSpecialization = settings.DefineSetting<bool>("Change Keybinds When Switching Specialization", true, (Func<string>)null, (Func<string>)null);
			displayCornerIcon = settings.DefineSetting<bool>("displayCornerIcon", true, (Func<string>)(() => "Show corner icon"), (Func<string>)null);
			autoClickSpeedMultiplier = settings.DefineSetting<float>("autoClickSpeedMultiplier", 1f, (Func<string>)(() => "Keybindings Apply Speed"), (Func<string>)(() => "Adjusts how fast the keybindings will be applied.\nLower this if your system is weaker and has trouble applying the keybindings."));
			SettingComplianceExtensions.SetRange(autoClickSpeedMultiplier, 0.5f, 2.5f);
			internalSettingsCollection = settings.AddSubCollection("internal Settings", false);
			defaultKeybinds = internalSettingsCollection.DefineSetting<string>("defaultKeybinds", "", (Func<string>)null, (Func<string>)null);
			characterKeybinds = internalSettingsCollection.DefineSetting<List<CharacterKeybind>>("keybinds", new List<CharacterKeybind>(), (Func<string>)null, (Func<string>)null);
			clickPositions = internalSettingsCollection.DefineSetting<List<Point>>("clickpos", ClickPositions.importClickPositions, (Func<string>)null, (Func<string>)null);
		}
	}
}
