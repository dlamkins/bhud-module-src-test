using System;

namespace flakysalt.CharacterKeybinds.Data
{
	[Serializable]
	[Obsolete("Use Keymap class instead")]
	public class CharacterKeybind
	{
		public string characterName;

		public string spezialisation;

		public string keymap;
	}
}
