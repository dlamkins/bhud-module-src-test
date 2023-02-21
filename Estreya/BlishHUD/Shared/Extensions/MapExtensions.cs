using System;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;

namespace Estreya.BlishHUD.Shared.Extensions
{
	public static class MapExtensions
	{
		public static Vector3 MapCoordsToWorldInches(this Map map, Vector2 mapCoords)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			double num = mapCoords.X;
			Rectangle val = map.get_ContinentRect();
			Coordinates2 topLeft = ((Rectangle)(ref val)).get_TopLeft();
			double num2 = num - ((Coordinates2)(ref topLeft)).get_X();
			val = map.get_ContinentRect();
			double num3 = num2 / ((Rectangle)(ref val)).get_Width();
			val = map.get_MapRect();
			double num4 = num3 * ((Rectangle)(ref val)).get_Width();
			val = map.get_MapRect();
			topLeft = ((Rectangle)(ref val)).get_TopLeft();
			float num5 = (float)(num4 + ((Coordinates2)(ref topLeft)).get_X());
			val = map.get_ContinentRect();
			topLeft = ((Rectangle)(ref val)).get_TopLeft();
			double num6 = ((Coordinates2)(ref topLeft)).get_Y() - (double)mapCoords.Y;
			val = map.get_ContinentRect();
			double num7 = num6 / ((Rectangle)(ref val)).get_Height();
			val = map.get_MapRect();
			double num8 = num7 * ((Rectangle)(ref val)).get_Height();
			val = map.get_MapRect();
			topLeft = ((Rectangle)(ref val)).get_TopLeft();
			return new Vector3(num5, (float)(num8 + ((Coordinates2)(ref topLeft)).get_Y()), 0f);
		}

		public static Vector3 MapCoordsToWorldMeters(this Map map, Vector2 mapCoords)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			return map.MapCoordsToWorldInches(mapCoords) / 39.37008f;
		}

		public static Vector2 WorldInchCoordsToMapCoords(this Map map, Vector2 coords)
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
			double num = coords.X;
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
			double num5 = coords.Y;
			val = map.get_MapRect();
			topLeft = ((Rectangle)(ref val)).get_TopLeft();
			double num6 = num5 - ((Coordinates2)(ref topLeft)).get_Y();
			val = map.get_MapRect();
			double num7 = num6 / ((Rectangle)(ref val)).get_Height();
			val = map.get_ContinentRect();
			return new Vector2(num4, (float)(y - num7 * ((Rectangle)(ref val)).get_Height()));
		}

		public static Vector2 WorldInchCoordsToMapCoords(this Map map, Vector3 world)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			return MapExtensions.WorldInchCoordsToMapCoords(map, new Vector2(world.X, world.Y));
		}

		public static Vector2 WorldMeterCoordsToMapCoords(this Map map, Vector3 world)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			return map.WorldInchCoordsToMapCoords(world * 39.37008f);
		}

		public static double GetDynamicEventMapLengthScale(this Map map, double length)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			Rectangle map_rect = map.get_MapRect();
			length /= 0.041666666666666664;
			double num = length;
			Coordinates2 val = ((Rectangle)(ref map_rect)).get_BottomLeft();
			double num2 = num - ((Coordinates2)(ref val)).get_X();
			val = ((Rectangle)(ref map_rect)).get_TopRight();
			double x = ((Coordinates2)(ref val)).get_X();
			val = ((Rectangle)(ref map_rect)).get_BottomLeft();
			double num3 = num2 / (x - ((Coordinates2)(ref val)).get_X());
			double num4 = length;
			val = ((Rectangle)(ref map_rect)).get_BottomLeft();
			double num5 = num4 - ((Coordinates2)(ref val)).get_Y();
			val = ((Rectangle)(ref map_rect)).get_TopRight();
			double y = ((Coordinates2)(ref val)).get_Y();
			val = ((Rectangle)(ref map_rect)).get_BottomLeft();
			double scaley = num5 / (y - ((Coordinates2)(ref val)).get_Y());
			return Math.Sqrt(num3 * num3 + scaley * scaley);
		}

		public static Vector2 EventMapCoordinatesToMapCoordinates(this Map map, Vector2 coordinates)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			Rectangle continent_rect = map.get_ContinentRect();
			Rectangle map_rect = map.get_MapRect();
			Coordinates2 val = ((Rectangle)(ref continent_rect)).get_TopLeft();
			double x = ((Coordinates2)(ref val)).get_X();
			double num = coordinates.X;
			val = ((Rectangle)(ref map_rect)).get_BottomLeft();
			double num2 = 1.0 * (num - ((Coordinates2)(ref val)).get_X());
			val = ((Rectangle)(ref map_rect)).get_TopRight();
			double x2 = ((Coordinates2)(ref val)).get_X();
			val = ((Rectangle)(ref map_rect)).get_BottomLeft();
			double num3 = num2 / (x2 - ((Coordinates2)(ref val)).get_X());
			val = ((Rectangle)(ref continent_rect)).get_BottomRight();
			double x3 = ((Coordinates2)(ref val)).get_X();
			val = ((Rectangle)(ref continent_rect)).get_TopLeft();
			float num4 = (float)Math.Round(x + num3 * (x3 - ((Coordinates2)(ref val)).get_X()));
			val = ((Rectangle)(ref continent_rect)).get_TopLeft();
			double y2 = ((Coordinates2)(ref val)).get_Y();
			double num5 = coordinates.Y;
			val = ((Rectangle)(ref map_rect)).get_TopRight();
			double num6 = -1.0 * (num5 - ((Coordinates2)(ref val)).get_Y());
			val = ((Rectangle)(ref map_rect)).get_TopRight();
			double y3 = ((Coordinates2)(ref val)).get_Y();
			val = ((Rectangle)(ref map_rect)).get_BottomLeft();
			double num7 = num6 / (y3 - ((Coordinates2)(ref val)).get_Y());
			val = ((Rectangle)(ref continent_rect)).get_BottomRight();
			double y4 = ((Coordinates2)(ref val)).get_Y();
			val = ((Rectangle)(ref continent_rect)).get_TopLeft();
			float y = (float)Math.Round(y2 + num7 * (y4 - ((Coordinates2)(ref val)).get_Y()));
			return new Vector2(num4, y);
		}
	}
}
