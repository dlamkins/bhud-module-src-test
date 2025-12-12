using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Gw2Sharp.WebApi.V2.Models;
using flakysalt.CharacterKeybinds.Data;
using flakysalt.CharacterKeybinds.Services;

namespace flakysalt.CharacterKeybinds.Model
{
	public class CharacterKeybindsModel
	{
		private readonly Logger _logger = Logger.GetLogger<CharacterKeybindsModel>();

		private readonly Gw2ApiService _apiService;

		private Dictionary<Profession, List<Specialization>> _professionEliteSpecialization = new Dictionary<Profession, List<Specialization>>();

		private List<Character> _characters = new List<Character>();

		private Action OnCharactersChanged;

		private Action OnKeymapChanged;

		public CharacterKeybindsSettings Settings { get; }

		public string CurrentKeybinds { get; set; }

		public bool IsDataLoaded => _professionEliteSpecialization.Any();

		public bool NeedsMigration
		{
			get
			{
				if (Settings.characterKeybinds.get_Value().Any())
				{
					return !Settings.Keymaps.get_Value().Any();
				}
				return false;
			}
		}

		public bool KeybindsFoldersValid
		{
			get
			{
				if (Directory.Exists(GetKeybindsCacheFolder()))
				{
					return Directory.Exists(GetKeybindsFolder());
				}
				return false;
			}
		}

		public CharacterKeybindsModel(CharacterKeybindsSettings settings, Gw2ApiService apiService)
		{
			Settings = settings;
			_apiService = apiService;
			Settings.gw2KeybindsFolder.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)SettingsFolderChanegd);
		}

		private void SettingsFolderChanegd(object sender, ValueChangedEventArgs<string> e)
		{
			OnKeymapChanged?.Invoke();
		}

		public async Task LoadResourcesAsync()
		{
			IEnumerable<Character> characters = await _apiService.GetCharactersAsync();
			IEnumerable<Profession> professions = await _apiService.GetProfessionsAsync();
			IEnumerable<Specialization> obj = await _apiService.GetSpecializationsAsync();
			_characters = characters.ToList();
			foreach (Specialization specialization in obj)
			{
				if (specialization.get_Elite())
				{
					Profession profession = professions.First((Profession p) => p.get_Id() == specialization.get_Profession());
					AddProfessionEliteSpecialization(profession, specialization);
				}
			}
			OnCharactersChanged?.Invoke();
		}

		public void ClearResources()
		{
			_professionEliteSpecialization.Clear();
		}

		public void BindCharacterDataChanged(Action onCharactersChanged)
		{
			OnCharactersChanged = (Action)Delegate.Combine(OnCharactersChanged, onCharactersChanged);
		}

		public void BindKeymapChanged(Action onKeymapChanged)
		{
			OnKeymapChanged = (Action)Delegate.Combine(OnKeymapChanged, onKeymapChanged);
		}

		public List<string> GetCharacterNames()
		{
			return _characters.Select((Character character) => character.get_Name()).ToList();
		}

		public Character GetCharacter(string name)
		{
			return _characters.FirstOrDefault((Character character) => character.get_Name() == name);
		}

		public Profession GetProfession(string name)
		{
			return _professionEliteSpecialization.FirstOrDefault((KeyValuePair<Profession, List<Specialization>> character) => character.Key.get_Id() == name).Key;
		}

		public Specialization GetProfessionSpecialization(string name)
		{
			foreach (KeyValuePair<Profession, List<Specialization>> item in _professionEliteSpecialization)
			{
				foreach (Specialization specialization in item.Value)
				{
					if (specialization.get_Name() == name)
					{
						return specialization;
					}
				}
			}
			return null;
		}

		public Specialization GetSpecializationById(int id)
		{
			foreach (KeyValuePair<Profession, List<Specialization>> item in _professionEliteSpecialization)
			{
				foreach (Specialization specialization in item.Value)
				{
					if (specialization.get_Id() == id)
					{
						return specialization;
					}
				}
			}
			return null;
		}

		public List<Character> GetCharacters()
		{
			return _characters.ToList();
		}

		public List<LocalizedSpecialization> GetProfessionSpecializations(string characterName)
		{
			Character character = _characters.FirstOrDefault((Character c) => c.get_Name() == characterName);
			if (character == null)
			{
				return new List<LocalizedSpecialization>();
			}
			Profession professionKey = _professionEliteSpecialization.Keys.FirstOrDefault((Profession p) => p.get_Id() == character.get_Profession());
			if (professionKey == null)
			{
				return new List<LocalizedSpecialization>();
			}
			List<LocalizedSpecialization> localizedSpecializations = new List<LocalizedSpecialization>();
			foreach (Specialization specialization in _professionEliteSpecialization[professionKey])
			{
				LocalizedSpecialization localizedSpec = new LocalizedSpecialization
				{
					displayName = specialization.get_Name(),
					id = specialization.get_Id()
				};
				localizedSpecializations.Add(localizedSpec);
			}
			return localizedSpecializations;
		}

		public List<string> GetKeymapsNames()
		{
			return (from specialization in Settings.characterKeybinds.get_Value()
				select specialization.keymap).ToList();
		}

		public List<Keymap> GetKeymaps()
		{
			return Settings.Keymaps.get_Value() ?? Enumerable.Empty<Keymap>().ToList();
		}

		public string GetDefaultKeybind()
		{
			return Settings.defaultKeybinds.get_Value();
		}

		public string GetKeybindsFolder()
		{
			return Settings.gw2KeybindsFolder.get_Value();
		}

		public string GetKeybindsCacheFolder()
		{
			return Path.Combine(Settings.gw2KeybindsFolder.get_Value(), "Cache");
		}

		public Keymap GetKeymapName(string characterName, Specialization specialization)
		{
			foreach (Keymap keybindData2 in Settings.Keymaps.get_Value())
			{
				if (keybindData2.CharacterName == characterName)
				{
					if (!specialization.get_Elite() && keybindData2.SpecialisationId == -1)
					{
						return keybindData2;
					}
					if (specialization.get_Id() == keybindData2.SpecialisationId)
					{
						return keybindData2;
					}
				}
			}
			foreach (Keymap keybindData in Settings.Keymaps.get_Value())
			{
				if (keybindData.CharacterName == characterName && keybindData.SpecialisationId == -2)
				{
					return keybindData;
				}
			}
			return null;
		}

		public void AddProfessionEliteSpecialization(Profession profession, Specialization specialization)
		{
			if (!_professionEliteSpecialization.ContainsKey(profession))
			{
				_professionEliteSpecialization[profession] = new List<Specialization>();
			}
			_professionEliteSpecialization[profession].Add(specialization);
		}

		public void SetDefaultKeymap(string keymap)
		{
			Settings.defaultKeybinds.set_Value(keymap);
		}

		public void AddKeymap()
		{
			Keymap keymap = new Keymap();
			Settings.Keymaps.get_Value().Add(keymap);
			OnKeymapChanged?.Invoke();
		}

		public void UpdateKeymap(Keymap oldKeymap, Keymap newKeymap)
		{
			if (TryGetKeymap(oldKeymap, out var foundMap))
			{
				Settings.Keymaps.get_Value()[Settings.Keymaps.get_Value().IndexOf(foundMap)] = newKeymap;
				OnKeymapChanged?.Invoke();
			}
		}

		public void RemoveKeymap(Keymap keymap)
		{
			if (TryGetKeymap(keymap, out var foundMap) && Settings.Keymaps.get_Value().Remove(foundMap))
			{
				OnKeymapChanged?.Invoke();
			}
		}

		private bool TryGetKeymap(Keymap map, out Keymap foundMap)
		{
			foundMap = Settings.Keymaps.get_Value().FirstOrDefault((Keymap e) => e.CharacterName == map.CharacterName && e.SpecialisationId == map.SpecialisationId && e.KeymapName == map.KeymapName);
			return foundMap != null;
		}
	}
}
