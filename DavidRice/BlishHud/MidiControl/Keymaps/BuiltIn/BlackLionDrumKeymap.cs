namespace DavidRice.BlishHud.MidiControl.Keymaps.BuiltIn
{
	public static class BlackLionDrumKeymap
	{
		public static Keymap Instance { get; } = new Keymap("black-lion-drum", "Black Lion Drum Set")
		{
			AutoOctaveSwap = false,
			Notes = 
			{
				{
					"C4",
					new NoteDefinition("1", 0)
				},
				{
					"D4",
					new NoteDefinition("2", 0)
				},
				{
					"E4",
					new NoteDefinition("3", 0)
				},
				{
					"F4",
					new NoteDefinition("4", 0)
				},
				{
					"G4",
					new NoteDefinition("5", 0)
				},
				{
					"A4",
					new NoteDefinition("6", 0)
				},
				{
					"B4",
					new NoteDefinition("7", 0)
				},
				{
					"C5",
					new NoteDefinition("8", 0)
				},
				{
					"D5",
					new NoteDefinition("9", 0)
				},
				{
					"E5",
					new NoteDefinition("0", 0)
				},
				{
					"C#4",
					new NoteDefinition("F1", 0)
				},
				{
					"D#4",
					new NoteDefinition("F2", 0)
				},
				{
					"F#4",
					new NoteDefinition("F3", 0)
				},
				{
					"G#4",
					new NoteDefinition("F4", 0)
				},
				{
					"A#4",
					new NoteDefinition("F5", 0)
				}
			}
		};

	}
}
