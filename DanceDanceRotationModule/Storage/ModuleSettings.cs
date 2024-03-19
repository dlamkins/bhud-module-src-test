using System;
using System.Collections.Generic;
using System.Reflection;
using Blish_HUD;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using DanceDanceRotationModule.Model;
using Microsoft.Xna.Framework.Input;

namespace DanceDanceRotationModule.Storage
{
	public class ModuleSettings
	{
		private static readonly Logger Logger = Logger.GetLogger<ModuleSettings>();

		internal SettingEntry<NotesOrientation> Orientation { get; private set; }

		internal SettingEntry<float> BackgroundOpacity { get; private set; }

		internal SettingEntry<bool> AutoHitWeapon1 { get; private set; }

		internal SettingEntry<bool> ShowAbilityIconsForNotes { get; private set; }

		internal SettingEntry<bool> ShowHotkeys { get; private set; }

		internal SettingEntry<bool> ShowOnlyCharacterClassSongs { get; private set; }

		internal SettingEntry<bool> CompactMode { get; private set; }

		internal SettingEntry<bool> StartSongsWithFirstSkill { get; private set; }

		internal SettingEntry<int> ShowNextAbilitiesCount { get; private set; }

		internal SettingCollection AbilityHotkeysSettings { get; private set; }

		internal SettingEntry<KeyBinding> SwapWeapons { get; private set; }

		internal SettingEntry<KeyBinding> Dodge { get; private set; }

		internal SettingEntry<KeyBinding> Weapon1 { get; private set; }

		internal SettingEntry<KeyBinding> Weapon2 { get; private set; }

		internal SettingEntry<KeyBinding> Weapon3 { get; private set; }

		internal SettingEntry<KeyBinding> Weapon4 { get; private set; }

		internal SettingEntry<KeyBinding> Weapon5 { get; private set; }

		internal SettingEntry<KeyBinding> HealingSkill { get; private set; }

		internal SettingEntry<KeyBinding> UtilitySkill1 { get; private set; }

		internal SettingEntry<KeyBinding> UtilitySkill2 { get; private set; }

		internal SettingEntry<KeyBinding> UtilitySkill3 { get; private set; }

		internal SettingEntry<KeyBinding> EliteSkill { get; private set; }

		internal SettingEntry<KeyBinding> ProfessionSkill1 { get; private set; }

		internal SettingEntry<KeyBinding> ProfessionSkill2 { get; private set; }

		internal SettingEntry<KeyBinding> ProfessionSkill3 { get; private set; }

		internal SettingEntry<KeyBinding> ProfessionSkill4 { get; private set; }

		internal SettingEntry<KeyBinding> ProfessionSkill5 { get; private set; }

		internal SettingEntry<KeyBinding> WeaponStow { get; private set; }

		internal SettingEntry<KeyBinding> ToggleHotkey { get; private set; }

		internal SettingEntry<KeyBinding> PlayHotkey { get; private set; }

		internal SettingEntry<KeyBinding> PauseHotkey { get; private set; }

		internal SettingEntry<KeyBinding> StopHotkey { get; private set; }

		internal SettingEntry<KeyBinding> ToggleNotesWindowHotkey { get; private set; }

		internal SettingEntry<KeyBinding> ToggleSongListWindowHotkey { get; private set; }

		internal SettingEntry<KeyBinding> ToggleSongInfoWindowHotkey { get; private set; }

		internal SettingEntry<List<SongData>> SongDatas { get; private set; }

		internal SettingEntry<Song.ID> SelectedSong { get; private set; }

		internal SettingEntry<bool> HasShownHelpWindow { get; private set; }

		internal SettingEntry<bool> HasShownInitialSongInfo { get; private set; }

		internal SettingEntry<string> LastDefaultSongsLoadedVersion { get; private set; }

		public ModuleSettings(SettingCollection settings)
		{
			Logger.Info("Initializing Settings");
			InitGeneral(settings);
			InitAbilityHotkeys(settings);
			InitControlHotkeys(settings);
			InitWindowVisibilityHotkeys(settings);
			InitSongRepoSettings(settings);
			InitHelpSettings(settings);
			InitHiddenSettings(settings);
		}

		private void InitGeneral(SettingCollection settings)
		{
			SettingCollection generalSettings = settings.AddSubCollection("general_settings", true, false, (Func<string>)(() => "General"));
			Orientation = generalSettings.DefineSetting<NotesOrientation>("NotesOrientation", NotesOrientation.RightToLeft, (Func<string>)(() => "Notes Orientation".PadRight(34)), (Func<string>)(() => "Sets the direction notes will travel while playing."));
			BackgroundOpacity = generalSettings.DefineSetting<float>("BackgroundOpacity", 0.9f, (Func<string>)(() => "Background Transparency"), (Func<string>)(() => "Sets the transparency of the notes background. Min=0% Max=100%"));
			SettingComplianceExtensions.SetRange(BackgroundOpacity, 0f, 1f);
			AutoHitWeapon1 = generalSettings.DefineSetting<bool>("AutoHitWeapon1", true, (Func<string>)(() => "Auto Hit Weapon 1"), (Func<string>)(() => "If enabled, Weapon1 skills will automatically clear, instead of requiring hotkey presses, since they are probably on auto-cast."));
			ShowAbilityIconsForNotes = generalSettings.DefineSetting<bool>("ShowAbilityIconsForNotes", true, (Func<string>)(() => "Show ability icon as note"), (Func<string>)(() => "If enabled, notes will use the actual ability icon instead of the generic."));
			ShowHotkeys = generalSettings.DefineSetting<bool>("ShowHotkeys", true, (Func<string>)(() => "Show ability hotkeys"), (Func<string>)(() => "If enabled, notes will have the hotkeys displayed on top of them."));
			ShowOnlyCharacterClassSongs = generalSettings.DefineSetting<bool>("ShowOnlyCharacterClassSongs", true, (Func<string>)(() => "Only show current profession songs"), (Func<string>)(() => "If enabled, the song list will only show songs for the current profession"));
			CompactMode = generalSettings.DefineSetting<bool>("CompactMode", false, (Func<string>)(() => "Compact Mode"), (Func<string>)(() => "If enabled, notes will try to be in a single lane and only shifted to other lanes to avoid collisions. Does NOT work with Ability Bar orientation."));
			StartSongsWithFirstSkill = generalSettings.DefineSetting<bool>("StartSongsWithFirstSkill", true, (Func<string>)(() => "Start with first skill"), (Func<string>)(() => "If enabled, the song can be started by pressing the hotkey for the first ability.\nNotes will be shifted so the first note is already in the 'Perfect' location.\nThis has no effect if the song is set to start later than the beginning."));
			ShowNextAbilitiesCount = generalSettings.DefineSetting<int>("ShowNextAbilitiesCount", 0, (Func<string>)(() => "Show next abilities"), (Func<string>)(() => "If enabled, the next X ability icons will be shown above the notes section, with no animations."));
			SettingComplianceExtensions.SetRange(ShowNextAbilitiesCount, 0, 10);
		}

		private void InitAbilityHotkeys(SettingCollection settings)
		{
			AbilityHotkeysSettings = settings.AddSubCollection("hotkey_settings", true, false, (Func<string>)(() => "Ability Hotkeys"));
			SwapWeapons = DefineHotkeySetting(AbilityHotkeysSettings, NoteType.WeaponSwap);
			Dodge = DefineHotkeySetting(AbilityHotkeysSettings, NoteType.Dodge);
			Weapon1 = DefineHotkeySetting(AbilityHotkeysSettings, NoteType.Weapon1);
			Weapon2 = DefineHotkeySetting(AbilityHotkeysSettings, NoteType.Weapon2);
			Weapon3 = DefineHotkeySetting(AbilityHotkeysSettings, NoteType.Weapon3);
			Weapon4 = DefineHotkeySetting(AbilityHotkeysSettings, NoteType.Weapon4);
			Weapon5 = DefineHotkeySetting(AbilityHotkeysSettings, NoteType.Weapon5);
			HealingSkill = DefineHotkeySetting(AbilityHotkeysSettings, NoteType.Heal);
			UtilitySkill1 = DefineHotkeySetting(AbilityHotkeysSettings, NoteType.Utility1);
			UtilitySkill2 = DefineHotkeySetting(AbilityHotkeysSettings, NoteType.Utility2);
			UtilitySkill3 = DefineHotkeySetting(AbilityHotkeysSettings, NoteType.Utility3);
			EliteSkill = DefineHotkeySetting(AbilityHotkeysSettings, NoteType.Elite);
			ProfessionSkill1 = DefineHotkeySetting(AbilityHotkeysSettings, NoteType.Profession1);
			ProfessionSkill2 = DefineHotkeySetting(AbilityHotkeysSettings, NoteType.Profession2);
			ProfessionSkill3 = DefineHotkeySetting(AbilityHotkeysSettings, NoteType.Profession3);
			ProfessionSkill4 = DefineHotkeySetting(AbilityHotkeysSettings, NoteType.Profession4);
			ProfessionSkill5 = DefineHotkeySetting(AbilityHotkeysSettings, NoteType.Profession5);
			WeaponStow = DefineHotkeySetting(AbilityHotkeysSettings, NoteType.WeaponStow);
		}

		private SettingEntry<KeyBinding> DefineHotkeySetting(SettingCollection settings, NoteType noteType)
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Expected O, but got Unknown
			SettingEntry<KeyBinding> obj = settings.DefineSetting<KeyBinding>(noteType.ToString(), new KeyBinding(NoteTypeExtensions.DefaultHotkey(noteType)), (Func<string>)(() => NoteTypeExtensions.HotkeyDescription(noteType)), (Func<string>)(() => "Hotkey used for " + NoteTypeExtensions.HotkeyDescription(noteType) + "\nThis must match your in-game hotkeys to work!"));
			obj.get_Value().set_Enabled(true);
			obj.get_Value().add_Activated((EventHandler<EventArgs>)delegate
			{
				if (DanceDanceRotationModule.Instance == null)
				{
					Logger.GetLogger<ModuleSettings>().Debug("Settings Hotkey was triggered byt the DDR Module Instance is null. This note press will be ignored : " + noteType);
				}
				else if (DanceDanceRotationModule.Instance.IsNotesWindowVisible())
				{
					DanceDanceRotationModule.Instance.GetNotesContainer()?.OnHotkeyPressed(noteType);
				}
			});
			return obj;
		}

		public SettingEntry<KeyBinding> GetKeyBindingForNoteType(NoteType noteType)
		{
			SettingEntry<KeyBinding> retval = new SettingEntry<KeyBinding>();
			AbilityHotkeysSettings.TryGetSetting<KeyBinding>(noteType.ToString(), ref retval);
			return retval;
		}

		private void InitControlHotkeys(SettingCollection settings)
		{
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Expected O, but got Unknown
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Expected O, but got Unknown
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a7: Expected O, but got Unknown
			//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_023c: Expected O, but got Unknown
			SettingCollection controlHotkeySettings = settings.AddSubCollection("control_hotkey_settings", true, false, (Func<string>)(() => "Control Hotkeys"));
			ToggleHotkey = controlHotkeySettings.DefineSetting<KeyBinding>("ToggleHotkey", new KeyBinding((Keys)0), (Func<string>)(() => "Toggle Play/Stop"), (Func<string>)(() => "Will start or stop the DDR rotation when pressed based on if it is currently playing."));
			ToggleHotkey.get_Value().set_Enabled(true);
			ToggleHotkey.get_Value().add_Activated((EventHandler<EventArgs>)delegate
			{
				DanceDanceRotationModule.Instance.GetNotesContainer()?.ToggleStart();
			});
			PlayHotkey = controlHotkeySettings.DefineSetting<KeyBinding>("PlayHotkey", new KeyBinding((Keys)0), (Func<string>)(() => "Play"), (Func<string>)(() => "Will start the DDR rotation when pressed, if not already playing."));
			PlayHotkey.get_Value().set_Enabled(true);
			PlayHotkey.get_Value().add_Activated((EventHandler<EventArgs>)delegate
			{
				DanceDanceRotationModule.Instance.GetNotesContainer()?.Play();
			});
			PauseHotkey = controlHotkeySettings.DefineSetting<KeyBinding>("PauseHotkey", new KeyBinding((Keys)0), (Func<string>)(() => "Pause"), (Func<string>)(() => "Will pause the DDR rotation when pressed, if playing."));
			PauseHotkey.get_Value().set_Enabled(true);
			PauseHotkey.get_Value().add_Activated((EventHandler<EventArgs>)delegate
			{
				DanceDanceRotationModule.Instance.GetNotesContainer()?.Pause();
			});
			StopHotkey = controlHotkeySettings.DefineSetting<KeyBinding>("StopHotkey", new KeyBinding((Keys)0), (Func<string>)(() => "Stop"), (Func<string>)(() => "Will stop the DDR rotation when pressed, if playing."));
			StopHotkey.get_Value().set_Enabled(true);
			StopHotkey.get_Value().add_Activated((EventHandler<EventArgs>)delegate
			{
				DanceDanceRotationModule.Instance.GetNotesContainer()?.Stop();
			});
		}

		private void InitWindowVisibilityHotkeys(SettingCollection settings)
		{
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Expected O, but got Unknown
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Expected O, but got Unknown
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a7: Expected O, but got Unknown
			SettingCollection windowHotkeySettings = settings.AddSubCollection("window_hotkey_settings", true, false, (Func<string>)(() => "Window Hotkeys"));
			ToggleNotesWindowHotkey = windowHotkeySettings.DefineSetting<KeyBinding>("ToggleNotesWindowHotkey", new KeyBinding((Keys)0), (Func<string>)(() => "Toggle Notes Window"), (Func<string>)(() => "Will Show/Hide the main Notes window"));
			ToggleNotesWindowHotkey.get_Value().set_Enabled(true);
			ToggleNotesWindowHotkey.get_Value().add_Activated((EventHandler<EventArgs>)delegate
			{
				DanceDanceRotationModule.Instance.ToggleNotesWindow();
			});
			ToggleSongListWindowHotkey = windowHotkeySettings.DefineSetting<KeyBinding>("ToggleSongListWindowHotkey", new KeyBinding((Keys)0), (Func<string>)(() => "Toggle Song List Window"), (Func<string>)(() => "Will Show/Hide the Song List window"));
			ToggleSongListWindowHotkey.get_Value().set_Enabled(true);
			ToggleSongListWindowHotkey.get_Value().add_Activated((EventHandler<EventArgs>)delegate
			{
				DanceDanceRotationModule.Instance.ToggleSongList();
			});
			ToggleSongInfoWindowHotkey = windowHotkeySettings.DefineSetting<KeyBinding>("ToggleSongInfoWindowHotkey", new KeyBinding((Keys)0), (Func<string>)(() => "Toggle Song Info Window"), (Func<string>)(() => "Will Show/Hide the Song Info window"));
			ToggleSongInfoWindowHotkey.get_Value().set_Enabled(true);
			ToggleSongInfoWindowHotkey.get_Value().add_Activated((EventHandler<EventArgs>)delegate
			{
				DanceDanceRotationModule.Instance.ToggleSongInfo();
			});
		}

		private void InitSongRepoSettings(SettingCollection settings)
		{
			SettingCollection songRepoSettings = settings.AddSubCollection("song_repo_settings", false);
			SongDatas = songRepoSettings.DefineSetting<List<SongData>>("SavedSongSettings", new List<SongData>(), (Func<string>)null, (Func<string>)null);
			SelectedSong = songRepoSettings.DefineSetting<Song.ID>("SelectedSong", default(Song.ID), (Func<string>)null, (Func<string>)null);
		}

		private void InitHelpSettings(SettingCollection settings)
		{
			SettingCollection helpSettings = settings.AddSubCollection("help_settings", false);
			HasShownHelpWindow = helpSettings.DefineSetting<bool>("HasShownHelpWindow", false, (Func<string>)null, (Func<string>)null);
			HasShownInitialSongInfo = helpSettings.DefineSetting<bool>("HasShownInitialSongInfo", false, (Func<string>)null, (Func<string>)null);
		}

		private void InitHiddenSettings(SettingCollection settings)
		{
			SettingCollection hiddenSettings = settings.AddSubCollection("hidden_settings", false);
			LastDefaultSongsLoadedVersion = hiddenSettings.DefineSetting<string>("LastDefaultSongsLoadedVersion", "0.0.0", (Func<string>)null, (Func<string>)null);
		}

		public void Dispose()
		{
			ClearSettingChanged<NotesOrientation>(Orientation);
			ClearSettingChanged<float>(BackgroundOpacity);
			ClearSettingChanged<bool>(AutoHitWeapon1);
			ClearSettingChanged<bool>(ShowAbilityIconsForNotes);
			ClearSettingChanged<bool>(ShowHotkeys);
			ClearSettingChanged<bool>(ShowOnlyCharacterClassSongs);
			ClearSettingChanged<int>(ShowNextAbilitiesCount);
			ClearSettingChanged<bool>(StartSongsWithFirstSkill);
			ClearSettingChanged<KeyBinding>(SwapWeapons);
			ClearSettingChanged<KeyBinding>(Dodge);
			ClearSettingChanged<KeyBinding>(Weapon1);
			ClearSettingChanged<KeyBinding>(Weapon2);
			ClearSettingChanged<KeyBinding>(Weapon3);
			ClearSettingChanged<KeyBinding>(Weapon4);
			ClearSettingChanged<KeyBinding>(Weapon5);
			ClearSettingChanged<KeyBinding>(HealingSkill);
			ClearSettingChanged<KeyBinding>(UtilitySkill1);
			ClearSettingChanged<KeyBinding>(UtilitySkill2);
			ClearSettingChanged<KeyBinding>(UtilitySkill3);
			ClearSettingChanged<KeyBinding>(EliteSkill);
			ClearSettingChanged<KeyBinding>(ProfessionSkill1);
			ClearSettingChanged<KeyBinding>(ProfessionSkill2);
			ClearSettingChanged<KeyBinding>(ProfessionSkill3);
			ClearSettingChanged<KeyBinding>(ProfessionSkill4);
			ClearSettingChanged<KeyBinding>(ProfessionSkill5);
			ClearSettingChanged<KeyBinding>(WeaponStow);
			ClearSettingChanged<List<SongData>>(SongDatas);
			ClearSettingChanged<Song.ID>(SelectedSong);
			ClearSettingChanged<bool>(HasShownHelpWindow);
			ClearSettingChanged<bool>(HasShownInitialSongInfo);
			ClearSettingChanged<string>(LastDefaultSongsLoadedVersion);
		}

		private static void ClearSettingChanged<T>(SettingEntry<T> entry)
		{
			FieldInfo f1 = GetEventField(((object)entry).GetType(), "SettingChanged");
			if (f1 == null)
			{
				return;
			}
			object obj = f1.GetValue(entry);
			if (obj != null)
			{
				Delegate[] invocationList = ((EventHandler<ValueChangedEventArgs<T>>)obj).GetInvocationList();
				foreach (Delegate del in invocationList)
				{
					entry.remove_SettingChanged((EventHandler<ValueChangedEventArgs<T>>)del);
				}
			}
		}

		private static FieldInfo GetEventField(Type type, string eventName)
		{
			FieldInfo field = null;
			while (type != null)
			{
				field = type.GetField(eventName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic);
				if (field != null && (field.FieldType == typeof(MulticastDelegate) || field.FieldType.IsSubclassOf(typeof(MulticastDelegate))))
				{
					break;
				}
				field = type.GetField("EVENT_" + eventName.ToUpper(), BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic);
				if (field != null)
				{
					break;
				}
				type = type.BaseType;
			}
			return field;
		}
	}
}
