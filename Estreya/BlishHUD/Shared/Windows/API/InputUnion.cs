using System.Runtime.InteropServices;

namespace Estreya.BlishHUD.Shared.Windows.API
{
	[StructLayout(LayoutKind.Explicit)]
	public struct InputUnion
	{
		[FieldOffset(0)]
		internal MouseInput mi;

		[FieldOffset(0)]
		internal KeyboardInput ki;

		[FieldOffset(0)]
		internal HardwareInput hi;
	}
}
