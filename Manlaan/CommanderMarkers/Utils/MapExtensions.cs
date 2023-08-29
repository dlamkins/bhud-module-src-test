using Gw2Sharp.Models;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;

namespace Manlaan.CommanderMarkers.Utils
{
	public static class MapExtensions
	{
		public static Vector2 WorldInchesToMap(this Map map, Vector3 world)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			Rectangle val = map.get_ContinentRect();
			Coordinates2 topLeft = ((Rectangle)(ref val)).get_TopLeft();
			double x = ((Coordinates2)(ref topLeft)).get_X();
			double num = world.X;
			val = map.get_MapRect();
			topLeft = ((Rectangle)(ref val)).get_TopLeft();
			double num2 = num - ((Coordinates2)(ref topLeft)).get_X();
			val = map.get_MapRect();
			double num3 = num2 / ((Rectangle)(ref val)).get_Width();
			val = map.get_ContinentRect();
			float num4 = (float)(x + num3 * ((Rectangle)(ref val)).get_Width());
			val = map.get_ContinentRect();
			topLeft = ((Rectangle)(ref val)).get_TopLeft();
			double y = ((Coordinates2)(ref topLeft)).get_Y();
			double num5 = world.Y;
			val = map.get_MapRect();
			topLeft = ((Rectangle)(ref val)).get_TopLeft();
			double num6 = num5 - ((Coordinates2)(ref topLeft)).get_Y();
			val = map.get_MapRect();
			double num7 = num6 / ((Rectangle)(ref val)).get_Height();
			val = map.get_ContinentRect();
			return new Vector2(num4, (float)(y - num7 * ((Rectangle)(ref val)).get_Height()));
		}

		public static Vector2 WorldMetersToMap(this Map map, Vector3 world)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			return map.WorldInchesToMap(world * 39.37008f);
		}
	}
}
