using System;

namespace DavidRice.BlishHud.MidiControl.Input
{
	public struct KEYBDINPUT
	{
		public ushort wVk;

		public ushort wScan;

		public uint dwFlags;

		public uint time;

		public UIntPtr dwExtraInfo;
	}
}
