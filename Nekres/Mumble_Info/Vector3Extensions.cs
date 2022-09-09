using System.Numerics;

namespace Nekres.Mumble_Info
{
	internal static class Vector3Extensions
	{
		public static float[] ToArray(this Vector3 v)
		{
			return new float[3] { v.X, v.Y, v.Z };
		}
	}
}
