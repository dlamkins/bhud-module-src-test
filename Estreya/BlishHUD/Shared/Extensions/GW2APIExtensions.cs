using Gw2Sharp.Models;
using Microsoft.Xna.Framework;

namespace Estreya.BlishHUD.Shared.Extensions
{
	public static class GW2APIExtensions
	{
		public static Vector2 ToVector2(this Coordinates2 coords)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			return new Vector2((float)((Coordinates2)(ref coords)).get_X(), (float)((Coordinates2)(ref coords)).get_Y());
		}
	}
}
