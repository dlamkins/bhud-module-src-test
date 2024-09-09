using System.Runtime.InteropServices;

namespace Kenedia.Modules.Core.Utility.WinApi
{
	internal struct Input
	{
		internal InputType _type;

		internal InputUnion _u;

		internal static int Size => Marshal.SizeOf(typeof(Input));
	}
}
