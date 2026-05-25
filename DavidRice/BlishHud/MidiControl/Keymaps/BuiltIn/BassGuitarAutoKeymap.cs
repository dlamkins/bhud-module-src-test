namespace DavidRice.BlishHud.MidiControl.Keymaps.BuiltIn
{
	public static class BassGuitarAutoKeymap
	{
		public static Keymap Instance { get; } = new Keymap("bass-guitar-auto", "Bass Guitar (Auto)")
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
					new NoteDefinition("8", 1)
				}
			}
		};

	}
}
