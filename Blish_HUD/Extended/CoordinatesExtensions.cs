using System.Collections.Generic;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;

namespace Blish_HUD.Extended
{
	public static class CoordinatesExtensions
	{
		private const float INCH_TO_METER = 0.0254f;

		public static Coordinates3 SwapYZ(this Coordinates3 coords)
		{
			return new Coordinates3(coords.X, coords.Z, coords.Y);
		}

		public static Coordinates2 ToPlane(this Coordinates3 coords)
		{
			return new Coordinates2(coords.X, coords.Y);
		}

		public static Coordinates3 ToUnit(this Coordinates3 coords, CoordsUnit fromUnit, CoordsUnit toUnit)
		{
			if (fromUnit == CoordsUnit.Meters && toUnit == CoordsUnit.Inches)
			{
				return new Coordinates3(coords.X / 0.02539999969303608, coords.Y / 0.02539999969303608, coords.Z / 0.02539999969303608);
			}
			if (fromUnit == CoordsUnit.Inches && toUnit == CoordsUnit.Meters)
			{
				return new Coordinates3(coords.X * 0.02539999969303608, coords.Y * 0.02539999969303608, coords.Z * 0.02539999969303608);
			}
			return coords;
		}

		public static Coordinates3 ToMapCoords(this Coordinates3 coords, CoordsUnit fromUnit)
		{
			coords = coords.ToUnit(fromUnit, CoordsUnit.Inches);
			return new Coordinates3(coords.X, coords.Y, coords.Z);
		}

		public static Coordinates3 ToContinentCoords(this Coordinates3 coords, CoordsUnit fromUnit, Rectangle mapRectangle, Rectangle continentRectangle)
		{
			Coordinates3 mapCoords = coords.ToMapCoords(fromUnit);
			return new Coordinates3((mapCoords.X - mapRectangle.TopLeft.X) / mapRectangle.Width * continentRectangle.Width + continentRectangle.TopLeft.X, z: (1.0 - (mapCoords.Z - mapRectangle.BottomRight.Y) / mapRectangle.Height) * continentRectangle.Height + continentRectangle.TopRight.Y, y: mapCoords.Y);
		}

		public static Vector3 ToXnaVector3(this Coordinates3 coords)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			return new Vector3((float)coords.X, (float)coords.Y, (float)coords.Z);
		}

		public static bool Inside(this Coordinates2 targetPoint, IReadOnlyList<Coordinates2> polygon)
		{
			if (polygon.Count < 3)
			{
				return false;
			}
			double x = targetPoint.X;
			double y = targetPoint.Y;
			bool isInside = false;
			int i = 0;
			int j = polygon.Count - 1;
			while (i < polygon.Count)
			{
				if (polygon[i].Y > y != polygon[j].Y > y && x < (polygon[j].X - polygon[i].X) * (y - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) + polygon[i].X)
				{
					isInside = !isInside;
				}
				j = i++;
			}
			return isInside;
		}
	}
}
