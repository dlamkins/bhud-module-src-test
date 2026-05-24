namespace DavidRice.BlishHud.MidiControl.Keymaps.BuiltIn
{
	public static class FluteEAutoKeymap
	{
		public static Keymap Instance { get; } = new Keymap("flute-e-auto", "Flute (E) (Auto)")
		{
			AutoOctaveSwap = true,
			OctaveDownKey = "9",
			OctaveUpKey = "9",
			Notes = 
			{
				{
					"E4",
					new NoteDefinition("1", 1)
				},
				{
					"F#4",
					new NoteDefinition("2", 1)
				},
				{
					"G#4",
					new NoteDefinition("3", 1)
				},
				{
					"A4",
					new NoteDefinition("4", 1)
				},
				{
					"B4",
					new NoteDefinition("5", 1)
				},
				{
					"C#5",
					new NoteDefinition("6", 1)
				},
				{
					"D#5",
					new NoteDefinition("7", 1)
				},
				{
					"E5",
					new NoteDefinition("8", 1, 2, "1")
				},
				{
					"F#5",
					new NoteDefinition("2", 2)
				},
				{
					"G#5",
					new NoteDefinition("3", 2)
				},
				{
					"A5",
					new NoteDefinition("4", 2)
				},
				{
					"B5",
					new NoteDefinition("5", 2)
				},
				{
					"C#6",
					new NoteDefinition("6", 2)
				},
				{
					"D#6",
					new NoteDefinition("7", 2)
				},
				{
					"E6",
					new NoteDefinition("8", 2)
				},
				{
					"F4",
					new NoteDefinition("9")
				},
				{
					"G4",
					new NoteDefinition("0")
				}
			}
		};

	}
}
