using System.Runtime.InteropServices;

namespace Nekres.Mumble_Info.Core.Services
{
	public struct Link
	{
		public uint uiVersion;

		public ulong uiTick;

		public float[] fAvatarPosition;

		public float[] fAvatarFront;

		public float[] fAvatarTop;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
		public string name;

		public float[] fCameraPosition;

		public float[] fCameraFront;

		public float[] fCameraTop;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
		public string identity;

		public uint context_len;
	}
}
