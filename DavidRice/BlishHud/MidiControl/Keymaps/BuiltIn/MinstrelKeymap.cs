namespace DavidRice.BlishHud.MidiControl.Keymaps.BuiltIn
{
	public static class MinstrelKeymap
	{
		public static Keymap Instance { get; } = new Keymap("minstrel", "The Minstrel")
		{
			AutoOctaveSwap = false,
			Notes = 
			{
				{
					"C4",
					new NoteDefinition("1")
				},
				{
					"D4",
					new NoteDefinition("2")
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
					"G4",
					new NoteDefinition("5")
				},
				{
					"A4",
					new NoteDefinition("6")
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
