using Gw2Sharp.Models;
using Gw2Sharp.WebApi.V2.Models;

namespace Blish_HUD.Extended
{
	public static class MapExtensions
	{
		public static string GetHash(this Map map)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			object[] obj = new object[5]
			{
				map.get_ContinentId(),
				null,
				null,
				null,
				null
			};
			Rectangle continentRect = map.get_ContinentRect();
			Coordinates2 val = ((Rectangle)(ref continentRect)).get_TopLeft();
			obj[1] = ((Coordinates2)(ref val)).get_X();
			continentRect = map.get_ContinentRect();
			val = ((Rectangle)(ref continentRect)).get_TopLeft();
			obj[2] = ((Coordinates2)(ref val)).get_Y();
			continentRect = map.get_ContinentRect();
			val = ((Rectangle)(ref continentRect)).get_BottomRight();
			obj[3] = ((Coordinates2)(ref val)).get_X();
			continentRect = map.get_ContinentRect();
			val = ((Rectangle)(ref continentRect)).get_BottomRight();
			obj[4] = ((Coordinates2)(ref val)).get_Y();
			return string.Format("{0}{1}{2}{3}{4}", obj).ToSHA1Hash().Substring(0, 8);
		}
	}
}
