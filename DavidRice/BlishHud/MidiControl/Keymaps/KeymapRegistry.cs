using System.Collections.Generic;
using System.Linq;
using DavidRice.BlishHud.MidiControl.Keymaps.BuiltIn;

namespace DavidRice.BlishHud.MidiControl.Keymaps
{
	public class KeymapRegistry
	{
		private readonly List<Keymap> _keymaps = new List<Keymap>();

		public IReadOnlyList<Keymap> AllKeymaps => _keymaps.AsReadOnly();

		public KeymapRegistry()
		{
			Register(GrandPianoAutoKeymap.Instance);
			Register(MinstrelAutoKeymap.Instance);
			Register(MinstrelKeymap.Instance);
			Register(ChoirBellAutoKeymap.Instance);
			Register(FluteCAutoKeymap.Instance);
			Register(FluteEAutoKeymap.Instance);
		}

		public void Register(Keymap keymap)
		{
			_keymaps.Add(keymap);
		}

		public Keymap? FindById(string id)
		{
			string id2 = id;
			return _keymaps.FirstOrDefault((Keymap k) => k.Id == id2);
		}

		public Keymap? FindByName(string name)
		{
			string name2 = name;
			return _keymaps.FirstOrDefault((Keymap k) => k.Name == name2);
		}
	}
}
