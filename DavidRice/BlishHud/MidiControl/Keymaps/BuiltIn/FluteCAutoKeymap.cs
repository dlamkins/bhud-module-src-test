namespace DavidRice.BlishHud.MidiControl.Keymaps.BuiltIn
{
	public static class FluteCAutoKeymap
	{
		public static Keymap Instance { get; } = new Keymap("flute-c-auto", "Flute (C) (Auto)")
		{
			AutoOctaveSwap = true,
			OctaveDownKey = "9",
			OctaveUpKey = "9",
			Notes = 
			{
				{
					"C4",
					new NoteDefinition("1", 1)
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

	}
}
