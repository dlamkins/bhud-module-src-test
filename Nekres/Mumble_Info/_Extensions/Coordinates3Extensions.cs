using Gw2Sharp.Models;
using Gw2Sharp.WebApi.V2.Models;

namespace Nekres.Mumble_Info._Extensions
{
	internal static class Coordinates3Extensions
	{
		private const float INCH_TO_METER = 0.0254f;

		public static Coordinates3 SwapYZ(this Coordinates3 coords)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			return new Coordinates3(((Coordinates3)(ref coords)).get_X(), ((Coordinates3)(ref coords)).get_Z(), ((Coordinates3)(ref coords)).get_Y());
		}

		public static Coordinates2 ToPlane(this Coordinates3 coords)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			return new Coordinates2(((Coordinates3)(ref coords)).get_X(), ((Coordinates3)(ref coords)).get_Y());
		}

		public static Coordinates3 ToUnit(this Coordinates3 coords, CoordsUnit fromUnit, CoordsUnit toUnit)
		{
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			if (fromUnit == CoordsUnit.Meters && toUnit == CoordsUnit.Inches)
			{
				return new Coordinates3(((Coordinates3)(ref coords)).get_X() / 0.02539999969303608, ((Coordinates3)(ref coords)).get_Y() / 0.02539999969303608, ((Coordinates3)(ref coords)).get_Z() / 0.02539999969303608);
			}
			if (fromUnit == CoordsUnit.Inches && toUnit == CoordsUnit.Meters)
			{
				return new Coordinates3(((Coordinates3)(ref coords)).get_X() * 0.02539999969303608, ((Coordinates3)(ref coords)).get_Y() * 0.02539999969303608, ((Coordinates3)(ref coords)).get_Z() * 0.02539999969303608);
			}
			return coords;
		}

		public static Coordinates3 ToMapCoords(this Coordinates3 coords, CoordsUnit fromUnit)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			coords = coords.ToUnit(fromUnit, CoordsUnit.Inches);
			return new Coordinates3(((Coordinates3)(ref coords)).get_X(), ((Coordinates3)(ref coords)).get_Y(), ((Coordinates3)(ref coords)).get_Z());
		}

		public static Coordinates3 ToContinentCoords(this Coordinates3 coords, CoordsUnit fromUnit, Rectangle mapRectangle, Rectangle continentRectangle)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			Coordinates3 mapCoords = coords.ToMapCoords(fromUnit);
			double x = ((Coordinates3)(ref mapCoords)).get_X();
			Coordinates2 val = ((Rectangle)(ref mapRectangle)).get_TopLeft();
			double num = (x - ((Coordinates2)(ref val)).get_X()) / ((Rectangle)(ref mapRectangle)).get_Width() * ((Rectangle)(ref continentRectangle)).get_Width();
			val = ((Rectangle)(ref continentRectangle)).get_TopLeft();
			double num2 = num + ((Coordinates2)(ref val)).get_X();
			double z2 = ((Coordinates3)(ref mapCoords)).get_Z();
			val = ((Rectangle)(ref mapRectangle)).get_BottomRight();
			double num3 = (1.0 - (z2 - ((Coordinates2)(ref val)).get_Y()) / ((Rectangle)(ref mapRectangle)).get_Height()) * ((Rectangle)(ref continentRectangle)).get_Height();
			val = ((Rectangle)(ref continentRectangle)).get_TopRight();
			double z = num3 + ((Coordinates2)(ref val)).get_Y();
			return new Coordinates3(num2, ((Coordinates3)(ref mapCoords)).get_Y(), z);
		}
	}
}
