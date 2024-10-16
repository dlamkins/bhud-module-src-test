using System;
using System.Collections.Generic;
using System.Linq;
using Gw2Sharp.WebApi.V2.Models;
using flakysalt.CharacterKeybinds.Data;

namespace flakysalt.CharacterKeybinds.Model
{
	public class CharacterKeybindModel
	{
		private CharacterKeybindsSettings _settings;

		private Dictionary<Profession, HashSet<Specialization>> ProfessionEliteSpecialization = new Dictionary<Profession, HashSet<Specialization>>();

		private IEnumerable<Character> characters = new List<Character>();

		private Action OnCharactersChanged;

		private Action OnKeymapChanged;

		public string currentKeybinds { get; set; }

		public CharacterKeybindModel(CharacterKeybindsSettings settings)
		{
			_settings = settings;
		}

		public void BindCharacterDataChanged(Action onAddButton)
		{
			OnCharactersChanged = (Action)Delegate.Combine(OnCharactersChanged, onAddButton);
		}

		public void BindKeymapChanged(Action onAddButton)
		{
			OnKeymapChanged = (Action)Delegate.Combine(OnKeymapChanged, onAddButton);
		}

		public List<string> GetCharacterNames()
		{
			return characters.Select((Character character) => character.get_Name()).ToList();
		}

		public Character GetCharacter(string Name)
		{
			return characters.FirstOrDefault((Character character) => character.get_Name() == Name);
		}

		public Profession GetProfession(string Name)
		{
			return ProfessionEliteSpecialization.FirstOrDefault((KeyValuePair<Profession, HashSet<Specialization>> character) => character.Key.get_Id() == Name).Key;
		}

		public Specialization GetProfessionSpecialization(string Name)
		{
			foreach (KeyValuePair<Profession, HashSet<Specialization>> item in ProfessionEliteSpecialization)
			{
				foreach (Specialization specialization in item.Value)
				{
					if (specialization.get_Name() == Name)
					{
						return specialization;
					}
				}
			}
			return null;
		}

		public List<Character> GetCharacters()
		{
			return (List<Character>)characters;
		}

		public List<string> GetProfessionSpecializations(string characterName)
		{
			Character character = characters.FirstOrDefault((Character c) => c.get_Name() == characterName);
			if (character == null)
			{
				return new List<string>();
			}
			Profession professionKey = ProfessionEliteSpecialization.Keys.FirstOrDefault((Profession p) => p.get_Name() == character.get_Profession());
			return ProfessionEliteSpecialization[professionKey].Select((Specialization specialization) => specialization.get_Name()).ToList();
		}

		public List<string> GetKeymapsNames()
		{
			return (from specialization in _settings.characterKeybinds.get_Value()
				select specialization.keymap).ToList();
		}

		public List<CharacterKeybind> GetKeymaps()
		{
			return _settings.characterKeybinds.get_Value();
		}

		public string GetDefaultKeybind()
		{
			return _settings.defaultKeybinds.get_Value();
		}

		public string GetKeybindsFolder()
		{
			return _settings.gw2KeybindsFolder.get_Value();
		}

		public CharacterKeybind GetKeymapName(string characterName, Specialization specialization)
		{
			foreach (CharacterKeybind keybindData in _settings.characterKeybinds.get_Value())
			{
				if (keybindData.characterName == characterName)
				{
					if (!specialization.get_Elite() && keybindData.spezialisation == "Core")
					{
						return keybindData;
					}
					if (specialization.get_Name() == keybindData.spezialisation)
					{
						return keybindData;
					}
					if (keybindData.spezialisation == "All Specialization")
					{
						return keybindData;
					}
				}
			}
			return null;
		}

		public void SetCharacters(IReadOnlyCollection<Character> characters)
		{
			this.characters = characters;
			OnCharactersChanged();
		}

		public void RemoveKeymap(CharacterKeybind characterKeybind)
		{
			CharacterKeybind element = _settings.characterKeybinds.get_Value().Find((CharacterKeybind e) => e.keymap == characterKeybind.keymap && e.characterName == characterKeybind.characterName && e.spezialisation == characterKeybind.spezialisation);
			if (element != null)
			{
				_settings.characterKeybinds.get_Value().Remove(element);
				OnKeymapChanged();
			}
		}

		public void AddKeymap()
		{
			_settings.characterKeybinds.get_Value().Add(new CharacterKeybind());
			OnKeymapChanged();
		}

		public void UpdateKeymap(CharacterKeybind oldValue, CharacterKeybind newValue)
		{
			if (oldValue != null)
			{
				int index = _settings.characterKeybinds.get_Value().FindIndex((CharacterKeybind e) => e.keymap == oldValue.keymap && e.characterName == oldValue.characterName && e.spezialisation == oldValue.spezialisation);
				if (index != -1)
				{
					_settings.characterKeybinds.get_Value()[index] = newValue;
					OnKeymapChanged();
				}
			}
		}

		public void AddProfessionEliteSpecialization(Profession profession, Specialization eliteSpecialization)
		{
			if (!ProfessionEliteSpecialization.ContainsKey(profession))
			{
				ProfessionEliteSpecialization[profession] = new HashSet<Specialization>();
			}
			ProfessionEliteSpecialization[profession].Add(eliteSpecialization);
		}

		public void SetDefaultKeymap(string keymap)
		{
			_settings.defaultKeybinds.set_Value(keymap);
		}
	}
}
