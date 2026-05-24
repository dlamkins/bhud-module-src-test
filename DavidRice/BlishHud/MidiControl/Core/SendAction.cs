namespace DavidRice.BlishHud.MidiControl.Core
{
	public readonly struct SendAction
	{
		public uint ScanCode { get; }

		public int DelayAfterMs { get; }

		public SendAction(uint scanCode, int delayAfterMs = 0)
		{
			ScanCode = scanCode;
			DelayAfterMs = delayAfterMs;
		}
	}
}
