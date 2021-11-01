using Gw2Sharp.Models;
using Gw2Sharp.WebApi.V2.Models;

namespace Nekres.Regions_Of_Tyria
{
	internal static class Coordinates3Extensions
	{
		private const float INCH_TO_METER = 0.0254f;

		public static Coordinates3 SwapYZ(this Coordinates3 coords)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			return new Coordinates3(((Coordinates3)(ref coords)).get_X(), ((Coordinates3)(ref coords)).get_Z(), ((Coordinates3)(ref coords)).get_Y());
		}

		public static Coordinates2 ToPlane(this Coordinates3 coords)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			return new Coordinates2(((Coordinates3)(ref coords)).get_X(), ((Coordinates3)(ref coords)).get_Y());
		}

		public static Coordinates3 ToUnit(this Coordinates3 coords, CoordsUnit fromUnit, CoordsUnit toUnit)
		{
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
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
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			coords = coords.ToUnit(fromUnit, CoordsUnit.Inches);
			return new Coordinates3(((Coordinates3)(ref coords)).get_X(), ((Coordinates3)(ref coords)).get_Y(), ((Coordinates3)(ref coords)).get_Z());
		}

		public static Coordinates3 ToContinentCoords(this Coordinates3 coords, CoordsUnit fromUnit, Rectangle mapRectangle, Rectangle continentRectangle)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			Coordinates3 mapCoords = coords.ToMapCoords(fromUnit);
			double x2 = ((Coordinates3)(ref mapCoords)).get_X();
			Coordinates2 val = ((Rectangle)(ref mapRectangle)).get_TopLeft();
			double num = (x2 - ((Coordinates2)(ref val)).get_X()) / ((Rectangle)(ref mapRectangle)).get_Width() * ((Rectangle)(ref continentRectangle)).get_Width();
			val = ((Rectangle)(ref continentRectangle)).get_TopLeft();
			double x = num + ((Coordinates2)(ref val)).get_X();
			double z2 = ((Coordinates3)(ref mapCoords)).get_Z();
			val = ((Rectangle)(ref mapRectangle)).get_BottomRight();
			double num2 = (1.0 - (z2 - ((Coordinates2)(ref val)).get_Y()) / ((Rectangle)(ref mapRectangle)).get_Height()) * ((Rectangle)(ref continentRectangle)).get_Height();
			val = ((Rectangle)(ref continentRectangle)).get_TopRight();
			double z = num2 + ((Coordinates2)(ref val)).get_Y();
			return new Coordinates3(x, ((Coordinates3)(ref mapCoords)).get_Y(), z);
		}
	}
}
