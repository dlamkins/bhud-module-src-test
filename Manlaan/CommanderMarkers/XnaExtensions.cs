using Gw2Sharp.Models;
using Microsoft.Xna.Framework;

namespace Manlaan.CommanderMarkers
{
	public static class XnaExtensions
	{
		public static Vector2 ToXnaVector2(this Coordinates2 coords)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			return new Vector2((float)((Coordinates2)(ref coords)).get_X(), (float)((Coordinates2)(ref coords)).get_Y());
		}

		public static Vector2 ToVector2(this Vector3 vector)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			return new Vector2(vector.X, vector.Y);
		}
	}
}
