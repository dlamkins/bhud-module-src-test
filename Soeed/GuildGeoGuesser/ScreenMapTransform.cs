using System;
using Blish_HUD;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Soeed.GuildGeoGuesser.Utils;

namespace Soeed.GuildGeoGuesser
{
	public static class ScreenMapTransform
	{
		private const double BlishScale = 1.1148272017837235;

		private static int _lastTick;

		private static Vector2 _mapCenter;

		private static Matrix _mapRotation;

		private static float _scale;

		private static Rectangle _screenBounds;

		private static Vector2 _boundsCenter;

		public static Vector2 MapCenter => UpdateAndGet(ref _mapCenter, new Action(Refresh));

		public static Matrix MapRotation => UpdateAndGet(ref _mapRotation, new Action(Refresh));

		public static float Scale => UpdateAndGet(ref _scale, new Action(Refresh));

		public static Rectangle ScreenBounds => UpdateAndGet(ref _screenBounds, new Action(Refresh));

		public static Vector2 BoundsCenter => UpdateAndGet(ref _boundsCenter, new Action(Refresh));

		private static void Refresh()
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			_mapCenter = GameService.Gw2Mumble.get_UI().get_MapCenter().ToXnaVector2();
			_mapRotation = Matrix.CreateRotationZ((GameService.Gw2Mumble.get_UI().get_IsCompassRotationEnabled() && !GameService.Gw2Mumble.get_UI().get_IsMapOpen()) ? ((float)GameService.Gw2Mumble.get_UI().get_CompassRotation()) : 0f);
			_screenBounds = MumbleUtils.GetMapBounds();
			_scale = (float)(1.1148272017837235 / GameService.Gw2Mumble.get_UI().get_MapScale());
			Point val = ((Rectangle)(ref _screenBounds)).get_Location();
			Vector2 val2 = ((Point)(ref val)).ToVector2();
			val = ((Rectangle)(ref _screenBounds)).get_Size();
			_boundsCenter = val2 + ((Point)(ref val)).ToVector2() / 2f;
			_lastTick = GameService.Gw2Mumble.get_Tick();
		}

		private static T UpdateAndGet<T>(ref T value, Action refresh)
		{
			if (GameService.Gw2Mumble.get_Tick() != _lastTick)
			{
				refresh();
			}
			return value;
		}

		public static Vector2 MapToScreen(Vector2 mapCoords)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			return Vector2.Transform((mapCoords - MapCenter) * Scale, MapRotation) + BoundsCenter;
		}

		public static Vector2 WorldToScreen(Map map, Vector3 worldMeters)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			return MapToScreen(map.WorldMetersToMap(worldMeters));
		}

		public static bool TryWorldToScreen(Map map, Vector3 worldMeters, out Vector2 screen)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			screen = default(Vector2);
			if (!GameService.Gw2Mumble.get_IsAvailable())
			{
				return false;
			}
			Rectangle bounds = ScreenBounds;
			if (bounds.Width <= 0 || bounds.Height <= 0)
			{
				return false;
			}
			screen = WorldToScreen(map, worldMeters);
			return true;
		}
	}
}
