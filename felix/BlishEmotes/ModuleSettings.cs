using System;
using System.Collections.Generic;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using felix.BlishEmotes.Strings;

namespace felix.BlishEmotes
{
	public class ModuleSettings
	{
		private Helper _helper;

		private const string GLOBAL_SETTINGS = "global-settings";

		private const string EMOTES_SHORTCUT_SETTINGS = "emotes-shortcuts-settings";

		private const string RADIAL_MENU_SETTINGS = "radial-menu-settings";

		private const string EMOTES_RADIAL_SETTINGS = "emotes-radial-settings";

		public SettingCollection RootSettings { get; private set; }

		public SettingCollection GlobalSettings { get; private set; }

		public SettingEntry<bool> GlobalHideCornerIcon { get; private set; }

		public SettingEntry<KeyBinding> GlobalKeyBindToggleEmoteList { get; private set; }

		public SettingEntry<KeyBinding> GlobalKeyBindToggleSynchronize { get; private set; }

		public SettingEntry<KeyBinding> GlobalKeyBindToggleTargeting { get; private set; }

		public SettingEntry<bool> GlobalUseCategories { get; private set; }

		public SettingEntry<bool> GlobalUseRadialMenu { get; private set; }

		public SettingCollection EmotesShortcutsSettings { get; private set; }

		public Dictionary<Emote, SettingEntry<KeyBinding>> EmotesShortcutsKeybindsMap { get; private set; }

		public SettingCollection RadialMenuSettings { get; private set; }

		public SettingEntry<bool> RadialSpawnAtCursor { get; private set; }

		public SettingEntry<KeyBinding> RadialToggleActionCameraKeyBind { get; private set; }

		public SettingEntry<float> RadialRadiusModifier { get; private set; }

		public SettingEntry<float> RadialInnerRadiusPercentage { get; private set; }

		public SettingEntry<float> RadialIconSizeModifier { get; private set; }

		public SettingEntry<float> RadialIconOpacity { get; private set; }

		public SettingCollection EmotesRadialSettings { get; private set; }

		public Dictionary<Emote, SettingEntry<bool>> EmotesRadialEnabledMap { get; private set; }

		public event EventHandler OnAnyEmotesRadialSettingsChanged;

		public event EventHandler<bool> OnEmotesLoaded;

		public ModuleSettings(SettingCollection settings, Helper helper)
		{
			_helper = helper;
			RootSettings = settings;
			DefineGlobalSettings(settings);
			DefineEmotesKeybindSettings(settings);
			DefineRadialMenuSettings(settings);
		}

		private void DefineGlobalSettings(SettingCollection settings)
		{
			GlobalSettings = settings.AddSubCollection("global-settings");
			GlobalHideCornerIcon = GlobalSettings.DefineSetting("GlobalHideCornerIcon", defaultValue: false, () => Common.settings_global_hideCornerIcon);
			GlobalKeyBindToggleEmoteList = GlobalSettings.DefineSetting("GlobalKeyBindToggleEmoteList", new KeyBinding(), () => Common.settings_global_keybindToggleEmoteList);
			GlobalUseCategories = GlobalSettings.DefineSetting("GlobalUseCategories", defaultValue: false, () => Common.settings_global_useCategories);
			GlobalUseRadialMenu = GlobalSettings.DefineSetting("GlobalUseRadialMenu", defaultValue: false, () => Common.settings_global_useRadialMenu);
			GlobalKeyBindToggleSynchronize = GlobalSettings.DefineSetting("GlobalKeyBindToggleSynchronize", new KeyBinding(), () => Common.settings_global_keybindToggleSynchronize);
			GlobalKeyBindToggleTargeting = GlobalSettings.DefineSetting("GlobalKeyBindToggleTargeting", new KeyBinding(), () => Common.settings_global_keybindToggleTargeting);
		}

		private void DefineEmotesKeybindSettings(SettingCollection settings)
		{
			EmotesShortcutsSettings = settings.AddSubCollection("emotes-shortcuts-settings");
			EmotesShortcutsKeybindsMap = new Dictionary<Emote, SettingEntry<KeyBinding>>();
		}

		public void InitEmotesShortcuts(List<Emote> emotes)
		{
			EmotesShortcutsKeybindsMap.Clear();
			foreach (Emote emote in emotes)
			{
				EmotesShortcutsKeybindsMap.Add(emote, EmotesShortcutsSettings.DefineSetting("EmotesShortcutsKeybindsMap_" + emote.Id, new KeyBinding(), () => _helper.EmotesResourceManager.GetString(emote.Id)));
				EmotesShortcutsKeybindsMap[emote].Value.Enabled = !emote.Locked;
				EmotesShortcutsKeybindsMap[emote].Value.Activated += delegate
				{
					_helper.SendEmoteCommand(emote);
				};
			}
		}

		private void DefineRadialMenuSettings(SettingCollection settings)
		{
			RadialMenuSettings = settings.AddSubCollection("radial-menu-settings");
			RadialSpawnAtCursor = RadialMenuSettings.DefineSetting("RadialSpawnAtCursor", defaultValue: false, () => Common.settings_radial_spawnAtCursor);
			RadialToggleActionCameraKeyBind = RadialMenuSettings.DefineSetting("RadialToggleActionCameraKeyBind", new KeyBinding(), () => Common.settings_radial_actionCamKeybind);
			RadialRadiusModifier = RadialMenuSettings.DefineSetting("RadialRadiusModifier", 0.35f, () => Common.settings_radial_radiusModifier);
			RadialRadiusModifier.SetRange(0.25f, 0.5f);
			RadialInnerRadiusPercentage = RadialMenuSettings.DefineSetting("RadialInnerRadiusPercentage", 0.25f, () => Common.settings_radial_innerRadiusPercentage, () => Common.settings_radial_innerRadiusPercentage_description);
			RadialInnerRadiusPercentage.SetRange(0f, 0.5f);
			RadialIconSizeModifier = RadialMenuSettings.DefineSetting("RadialIconSizeModifier", 0.5f, () => Common.settings_radial_iconSizeModifier);
			RadialIconSizeModifier.SetRange(0.25f, 0.75f);
			RadialIconOpacity = RadialMenuSettings.DefineSetting("RadialIconOpacity", 0.5f, () => Common.settings_radial_iconOpacity);
			RadialIconOpacity.SetRange(0.5f, 0.75f);
			DefineEmotesRadialSettings(RadialMenuSettings);
		}

		private void DefineEmotesRadialSettings(SettingCollection settings)
		{
			EmotesRadialSettings = settings.AddSubCollection("emotes-radial-settings");
			EmotesRadialEnabledMap = new Dictionary<Emote, SettingEntry<bool>>();
		}

		public void InitEmotesRadialEnabled(List<Emote> emotes)
		{
			EmotesRadialEnabledMap.Clear();
			foreach (Emote emote in emotes)
			{
				SettingEntry<bool> newSetting = EmotesRadialSettings.DefineSetting("EmotesRadialEnabledMap_" + emote.Id, defaultValue: true, () => _helper.EmotesResourceManager.GetString(emote.Id));
				EmotesRadialEnabledMap.Add(emote, newSetting);
				newSetting.SettingChanged += delegate
				{
					this.OnAnyEmotesRadialSettingsChanged?.Invoke(this, null);
				};
			}
		}

		public void InitEmotesSettings(List<Emote> emotes)
		{
			InitEmotesShortcuts(emotes);
			InitEmotesRadialEnabled(emotes);
			this.OnEmotesLoaded?.Invoke(this, e: true);
		}

		public void Unload()
		{
			GlobalKeyBindToggleEmoteList.Value.Enabled = false;
			GlobalKeyBindToggleSynchronize.Value.Enabled = false;
			GlobalKeyBindToggleTargeting.Value.Enabled = false;
			foreach (KeyValuePair<Emote, SettingEntry<KeyBinding>> item in EmotesShortcutsKeybindsMap)
			{
				item.Value.Value.Enabled = false;
			}
			EmotesShortcutsKeybindsMap.Clear();
			EmotesRadialEnabledMap.Clear();
		}
	}
}
