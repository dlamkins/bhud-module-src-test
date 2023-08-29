using Microsoft.Xna.Framework;

namespace Manlaan.CommanderMarkers.Presets
{
	public class ScreenMapBounds : MapBounds
	{
		private readonly MapData _mapData;

		public ScreenMapBounds(MapData mapData)
		{
			_mapData = mapData;
		}

		public override Vector2 FromWorld(int mapId, Vector3 worldMeters)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			return _mapData.WorldToScreenMap(mapId, worldMeters);
		}

		public override Vector2 FromMap(Vector2 mapCoords)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return MapData.MapToScreenMap(mapCoords);
		}
	}
}
