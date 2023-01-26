using Gw2Sharp.Models;
using Microsoft.Xna.Framework;

namespace Blish_HUD.Extended
{
	public static class CoordinatesExtensions
	{
		public static Vector2 ToVector(this Coordinates2 coords)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			return new Vector2((float)((Coordinates2)(ref coords)).get_X(), (float)((Coordinates2)(ref coords)).get_Y());
		}
	}
}
