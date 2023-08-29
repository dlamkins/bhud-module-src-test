using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;

namespace Manlaan.CommanderMarkers.Presets
{
	public interface IMapBounds
	{
		Rectangle Rectangle { get; }

		Vector2 FromWorld(int mapId, Vector3 worldMeters);

		Vector2 FromMap(Vector2 mapCoords);

		Rectangle FromMap(Rectangle rect);

		bool Contains(Vector2 point);
	}
}
