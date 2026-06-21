namespace DavidRice.BlishHud.MidiControl.Keymaps.BuiltIn
{
	public static class FrameDrumAutoKeymap
	{
		public static Keymap Instance { get; } = new Keymap("frame-drum-auto", "Frame Drum (Auto)")
		{
			AutoOctaveSwap = false,
			Notes = 
			{
				{
					"C4",
					new NoteDefinition("1", 0)
				},
				{
					"C#4",
					new NoteDefinition("2", 0)
				},
				{
					"D4",
					new NoteDefinition("3", 0)
				},
				{
					"D#4",
					new NoteDefinition("4", 0)
				},
				{
					"E4",
					new NoteDefinition("5", 0)
				}
			}
		};

	}
}
