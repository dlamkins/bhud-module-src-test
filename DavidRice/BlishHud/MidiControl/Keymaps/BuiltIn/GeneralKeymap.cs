namespace DavidRice.BlishHud.MidiControl.Keymaps.BuiltIn
{
	public static class GeneralKeymap
	{
		public static Keymap Instance { get; } = new Keymap("general", "General (Manual)")
		{
			AutoOctaveSwap = false,
			OctaveDownKey = "9",
			OctaveUpKey = "0",
			Notes = 
			{
				{
					"C4",
					new NoteDefinition("1")
				},
				{
					"C#4",
					new NoteDefinition("F1")
				},
				{
					"D4",
					new NoteDefinition("2")
				},
				{
					"D#4",
					new NoteDefinition("F2")
				},
				{
					"E4",
					new NoteDefinition("3")
				},
				{
					"F4",
					new NoteDefinition("4")
				},
				{
					"F#4",
					new NoteDefinition("F3")
				},
				{
					"G4",
					new NoteDefinition("5")
				},
				{
					"G#4",
					new NoteDefinition("F4")
				},
				{
					"A4",
					new NoteDefinition("6")
				},
				{
					"A#4",
					new NoteDefinition("F5")
				},
				{
					"B4",
					new NoteDefinition("7")
				},
				{
					"C5",
					new NoteDefinition("8")
				},
				{
					"D5",
					new NoteDefinition("9")
				},
				{
					"E5",
					new NoteDefinition("0")
				}
			}
		};

	}
}
