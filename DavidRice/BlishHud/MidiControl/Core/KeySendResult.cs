namespace DavidRice.BlishHud.MidiControl.Core
{
	public readonly struct KeySendResult
	{
		public SendAction[] Actions { get; }

		public int NewOctave { get; }

		public int PreviousOctave { get; }

		public string[] SentKeyNames { get; }

		public bool WasSuppressed { get; }

		public KeySendResult(SendAction[] actions, int newOctave, int previousOctave, string[] sentKeyNames, bool wasSuppressed = false)
		{
			Actions = actions;
			NewOctave = newOctave;
			PreviousOctave = previousOctave;
			SentKeyNames = sentKeyNames;
			WasSuppressed = wasSuppressed;
		}
	}
}
