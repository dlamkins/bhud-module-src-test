using Microsoft.Xna.Framework;

namespace Manlaan.CommanderMarkers.Services
{
	internal static class RtApiCoordinates
	{
		public static Vector3 ToGame(float x, float elevation, float z)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			return new Vector3(x, z, elevation);
		}
	}
}
