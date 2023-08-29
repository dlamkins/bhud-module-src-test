using Blish_HUD;
using Microsoft.Xna.Framework;

namespace Manlaan.CommanderMarkers.Presets
{
	public static class PresetPlacementService
	{
		public static void PlaceMarkers()
		{
		}

		public static Vector2 ContinentToMapScreen(Vector2 continentCoords)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			Vector2 mapCenter = GameService.Gw2Mumble.get_UI().get_MapCenter().ToXnaVector2();
			Matrix mapRotation = Matrix.CreateRotationZ((GameService.Gw2Mumble.get_UI().get_IsCompassRotationEnabled() && !GameService.Gw2Mumble.get_UI().get_IsMapOpen()) ? ((float)GameService.Gw2Mumble.get_UI().get_CompassRotation()) : 0f);
			Rectangle screenBounds = MumbleUtils.GetMapBounds();
			float scale = (float)(GameService.Gw2Mumble.get_UI().get_MapScale() * 0.897);
			Point val = ((Rectangle)(ref screenBounds)).get_Location();
			Vector2 val2 = ((Point)(ref val)).ToVector2();
			val = ((Rectangle)(ref screenBounds)).get_Size();
			Vector2 boundsCenter = val2 + ((Point)(ref val)).ToVector2() / 2f;
			return Vector2.Transform((continentCoords - mapCenter) / scale, mapRotation) + boundsCenter;
		}
	}
}
