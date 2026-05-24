using System.Collections.Generic;

namespace DavidRice.BlishHud.MidiControl
{
	public class Keymap
	{
		public string Id { get; }

		public string Name { get; }

		public bool AutoOctaveSwap { get; set; } = true;


		public Dictionary<string, NoteDefinition> Notes { get; } = new Dictionary<string, NoteDefinition>();


		public string? OctaveDownKey { get; set; }

		public string? OctaveUpKey { get; set; }

		public Keymap(string id, string name)
		{
			Id = id;
			Name = name;
		}
	}
}
