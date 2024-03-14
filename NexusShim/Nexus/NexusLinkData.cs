using System.Runtime.InteropServices;

namespace NexusShim.Nexus
{
	[StructLayout(LayoutKind.Explicit)]
	public struct NexusLinkData
	{
		public const int SIZE = 9;

		[FieldOffset(0)]
		public uint AmountIcons;

		[FieldOffset(4)]
		public uint Mode;

		[FieldOffset(8)]
		public bool IsVerticalLayout;
	}
}
