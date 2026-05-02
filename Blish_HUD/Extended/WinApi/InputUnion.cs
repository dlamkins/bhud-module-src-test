using System.Runtime.InteropServices;

namespace Blish_HUD.Extended.WinApi
{
	[StructLayout(LayoutKind.Explicit)]
	internal struct InputUnion
	{
		[FieldOffset(0)]
		internal MouseUtil.MouseInput mi;

		[FieldOffset(0)]
		internal KeyboardUtil.KeybdInput ki;

		[FieldOffset(0)]
		internal HardwareInput hi;
	}
}
