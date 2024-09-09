using System.Runtime.InteropServices;

namespace Kenedia.Modules.Core.Utility.WinApi
{
	[StructLayout(LayoutKind.Explicit)]
	internal struct InputUnion
	{
		[FieldOffset(0)]
		internal MouseUtil.MouseInput _mi;

		[FieldOffset(0)]
		internal KeyboardUtil.KeybdInput _ki;

		[FieldOffset(0)]
		internal HardwareInput _hi;
	}
}
