using System;
using System.Collections.Generic;
using System.IO;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace flakysalt.CharacterKeybinds.Data
{
	public class CharacterKeybindsSettings
	{
		public SettingEntry<string> gw2KeybindsFolder;

		public SettingEntry<KeyBinding> optionsKeybind;

		public SettingEntry<bool> changeKeybindsWhenSwitchingSpecialization;

		public SettingEntry<bool> displayCornerIcon;

		public SettingEntry<List<CharacterKeybind>> characterKeybinds;

		public SettingEntry<List<Point>> clickPositions;

		public SettingCollection settingsCollection { get; private set; }

		public SettingCollection internalSettingsCollection { get; private set; }

		public CharacterKeybindsSettings(SettingCollection settings)
		{
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Expected O, but got Unknown
			settingsCollection = settings;
			string targetFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Guild Wars 2", "InputBinds");
			gw2KeybindsFolder = settings.DefineSetting<string>("GW2 Keybind Path", targetFolderPath, (Func<string>)(() => "Path to the Keybinds folder"), (Func<string>)(() => ""));
			optionsKeybind = settings.DefineSetting<KeyBinding>("optionsKeybind", new KeyBinding((Keys)122), (Func<string>)(() => "Options Menu Keybind"), (Func<string>)null);
			changeKeybindsWhenSwitchingSpecialization = settings.DefineSetting<bool>("Change Keybinds When Switching Specialization", true, (Func<string>)null, (Func<string>)null);
			displayCornerIcon = settings.DefineSetting<bool>("displayCornerIcon", true, (Func<string>)(() => "Show corner icon"), (Func<string>)null);
			internalSettingsCollection = settings.AddSubCollection("internal Settings", false);
			characterKeybinds = internalSettingsCollection.DefineSetting<List<CharacterKeybind>>("keybinds", new List<CharacterKeybind>(), (Func<string>)null, (Func<string>)null);
			clickPositions = internalSettingsCollection.DefineSetting<List<Point>>("clickpos", ClickPosLocations.importMarkerLocations, (Func<string>)null, (Func<string>)null);
		}
	}
}
