using System.Runtime.InteropServices;

namespace Estreya.BlishHUD.Shared.Windows.API
{
	public struct Input
	{
		internal InputType type;

		internal InputUnion U;

		internal static int Size => Marshal.SizeOf(typeof(Input));
	}
}
