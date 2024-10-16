using System;
using flakysalt.CharacterKeybinds.Data;

namespace flakysalt.CharacterKeybinds.Views.UiElements
{
	public class KeymapEventArgs : EventArgs
	{
		public CharacterKeybind OldCharacterKeybind;

		public CharacterKeybind NewCharacterKeybind;
	}
}
