using System.Collections.Generic;

namespace DavidRice.BlishHud.MidiControl.Keymaps.BuiltIn
{
	public static class MinstrelAutoKeymap
	{
		public static Keymap Instance { get; }

		static MinstrelAutoKeymap()
		{
			Keymap obj = new Keymap("minstrel-auto", "The Minstrel (Auto)")
			{
				AutoOctaveSwap = true,
				OctaveDownKey = "9",
				OctaveUpKey = "0",
				Notes = 
				{
					{
						"C3",
						new NoteDefinition("1", 0)
					},
					{
						"D3",
						new NoteDefinition("2", 0)
					},
					{
						"E3",
						new NoteDefinition("3", 0)
					},
					{
						"F3",
						new NoteDefinition("4", 0)
					},
					{
						"G3",
						new NoteDefinition("5", 0)
					},
					{
						"A3",
						new NoteDefinition("6", 0)
					},
					{
						"B3",
						new NoteDefinition("7", 0)
					},
					{
						"C4",
						new NoteDefinition("1", 1, 0, "8")
					},
					{
						"D4",
						new NoteDefinition("2", 1)
					},
					{
						"E4",
						new NoteDefinition("3", 1)
					},
					{
						"F4",
						new NoteDefinition("4", 1)
					},
					{
						"G4",
						new NoteDefinition("5", 1)
					},
					{
						"A4",
						new NoteDefinition("6", 1)
					},
					{
						"B4",
						new NoteDefinition("7", 1)
					},
					{
						"C5",
						new NoteDefinition("8", 1, 2, "1")
					},
					{
						"D5",
						new NoteDefinition("2", 2)
					},
					{
						"E5",
						new NoteDefinition("3", 2)
					},
					{
						"F5",
						new NoteDefinition("4", 2)
					},
					{
						"G5",
						new NoteDefinition("5", 2)
					},
					{
						"A5",
						new NoteDefinition("6", 2)
					},
					{
						"B5",
						new NoteDefinition("7", 2)
					},
					{
						"C6",
						new NoteDefinition("8", 2)
					},
					{
						"C#4",
						new NoteDefinition("9")
					},
					{
						"D#4",
						new NoteDefinition("0")
					}
				}
			};
			Dictionary<string, NoteDefinition> notes = obj.Notes;
			int? forceInternalOctave = 0;
			notes.Add("F#4", new NoteDefinition(null, null, null, null, forceInternalOctave));
			Dictionary<string, NoteDefinition> notes2 = obj.Notes;
			forceInternalOctave = 1;
			notes2.Add("G#4", new NoteDefinition(null, null, null, null, forceInternalOctave));
			Dictionary<string, NoteDefinition> notes3 = obj.Notes;
			forceInternalOctave = 2;
			notes3.Add("A#4", new NoteDefinition(null, null, null, null, forceInternalOctave));
			Instance = obj;
		}
	}
}
