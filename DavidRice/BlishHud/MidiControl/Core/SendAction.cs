namespace DavidRice.BlishHud.MidiControl.Core
{
	public readonly struct SendAction
	{
		public uint ScanCode { get; }

		public int DelayAfterMs { get; }

		public KeyEventType EventType { get; }

		public SendAction(uint scanCode, int delayAfterMs = 0, KeyEventType eventType = KeyEventType.KeyTap)
		{
			ScanCode = scanCode;
			DelayAfterMs = delayAfterMs;
			EventType = eventType;
		}
	}
}
