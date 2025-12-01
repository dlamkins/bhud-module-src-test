using System;
using System.Collections.Generic;
using System.IO;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using flakysalt.CharacterKeybinds.Data;
using flakysalt.CharacterKeybinds.Resources;

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

		public SettingEntry<bool> experiencedFtue;

		public SettingEntry<List<Keymap>> Keymaps;

		public SettingEntry<List<Point>> clickPositions;

		[Obsolete("Use optionsKeybind instead")]
		public SettingEntry<List<CharacterKeybind>> characterKeybinds;

		public SettingCollection settingsCollection { get; private set; }

		public SettingEntry<float> autoClickSpeedMultiplier { get; private set; }

		public SettingCollection internalSettingsCollection { get; private set; }

		private static string TargetFolderPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Guild Wars 2", "InputBinds");

		public CharacterKeybindsSettings(SettingCollection settings)
		{
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Expected O, but got Unknown
			settingsCollection = settings;
			gw2KeybindsFolder = settings.DefineSetting<string>("GW2 Keybind Path", TargetFolderPath, (Func<string>)(() => SettingsLoca.keybindsDirectorySetting), (Func<string>)(() => SettingsLoca.keybindsDirectoryHint));
			optionsKeybind = settings.DefineSetting<KeyBinding>("optionsKeybind", new KeyBinding((Keys)122), (Func<string>)(() => SettingsLoca.optionsMenuKeybindsSetting), (Func<string>)null);
			useDefaultKeybinds = settings.DefineSetting<bool>("Use Default Keybinds", true, (Func<string>)(() => SettingsLoca.useDefaultKeybindsSetting), (Func<string>)(() => SettingsLoca.useDefaultKeybindsHint));
			changeKeybindsWhenSwitchingSpecialization = settings.DefineSetting<bool>("Change Keybinds When Switching Specialization", true, (Func<string>)(() => SettingsLoca.changeKeybindsOnSpecSwitchSetting), (Func<string>)(() => SettingsLoca.changeKeybindsOnSpecSwitchHint));
			displayCornerIcon = settings.DefineSetting<bool>("displayCornerIcon", true, (Func<string>)(() => SettingsLoca.showCornerIconSetting), (Func<string>)(() => SettingsLoca.showCornerIconHint));
			autoClickSpeedMultiplier = settings.DefineSetting<float>("autoClickSpeedMultiplier", 1f, (Func<string>)(() => SettingsLoca.autoClickSpeedMultiplierSetting), (Func<string>)(() => SettingsLoca.autoClickSpeedMultiplierHint));
			SettingComplianceExtensions.SetRange(autoClickSpeedMultiplier, 0.5f, 2.5f);
			internalSettingsCollection = settings.AddSubCollection("internal Settings", false);
			experiencedFtue = internalSettingsCollection.DefineSetting<bool>("experiencedFtue", false, (Func<string>)null, (Func<string>)null);
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
