namespace DavidRice.BlishHud.MidiControl.Keymaps.BuiltIn
{
	public static class GrandPianoAutoKeymap
	{
		public static Keymap Instance { get; } = new Keymap("grand-piano-auto", "Ornate Grand Piano (Auto)")
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
					"C#3",
					new NoteDefinition("F1", 0)
				},
				{
					"D3",
					new NoteDefinition("2", 0)
				},
				{
					"D#3",
					new NoteDefinition("F2", 0)
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
					"F#3",
					new NoteDefinition("F3", 0)
				},
				{
					"G3",
					new NoteDefinition("5", 0)
				},
				{
					"G#3",
					new NoteDefinition("F4", 0)
				},
				{
					"A3",
					new NoteDefinition("6", 0)
				},
				{
					"A#3",
					new NoteDefinition("F5", 0)
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
					"C#4",
					new NoteDefinition("F1", 1)
				},
				{
					"D4",
					new NoteDefinition("2", 1)
				},
				{
					"D#4",
					new NoteDefinition("F2", 1)
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
					"F#4",
					new NoteDefinition("F3", 1)
				},
				{
					"G4",
					new NoteDefinition("5", 1)
				},
				{
					"G#4",
					new NoteDefinition("F4", 1)
				},
				{
					"A4",
					new NoteDefinition("6", 1)
				},
				{
					"A#4",
					new NoteDefinition("F5", 1)
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
					"C#5",
					new NoteDefinition("F1", 2)
				},
				{
					"D5",
					new NoteDefinition("2", 2)
				},
				{
					"D#5",
					new NoteDefinition("F2", 2)
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
					"F#5",
					new NoteDefinition("F3", 2)
				},
				{
					"G5",
					new NoteDefinition("5", 2)
				},
				{
					"G#5",
					new NoteDefinition("F4", 2)
				},
				{
					"A5",
					new NoteDefinition("6", 2)
				},
				{
					"A#5",
					new NoteDefinition("F5", 2)
				},
				{
					"B5",
					new NoteDefinition("7", 2)
				},
				{
					"C6",
					new NoteDefinition("8", 2)
				}
			}
		};

	}
}
