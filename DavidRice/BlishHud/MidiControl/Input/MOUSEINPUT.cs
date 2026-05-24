using System;

namespace DavidRice.BlishHud.MidiControl.Input
{
	public struct MOUSEINPUT
	{
		public int dx;

		public int dy;

		public uint mouseData;

		public uint dwFlags;

		public uint time;

		public UIntPtr dwExtraInfo;
	}
}
