using System;

namespace flakysalt.CharacterKeybinds.Data
{
	[Serializable]
	public class Keymap
	{
		public string CharacterName;

		public int SpecialisationId;

		public string KeymapName;

		public const int CoreSpecializationId = -1;

		public const int AllSpecializationId = -2;

		public const int Invalid = -10;
	}
}
