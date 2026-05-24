using System.Runtime.InteropServices;

namespace DavidRice.BlishHud.MidiControl.Input
{
	[StructLayout(LayoutKind.Explicit)]
	public struct INPUT_UNION
	{
		[FieldOffset(0)]
		public MOUSEINPUT mi;

		[FieldOffset(0)]
		public KEYBDINPUT ki;

		[FieldOffset(0)]
		public HARDWAREINPUT hi;
	}
}
