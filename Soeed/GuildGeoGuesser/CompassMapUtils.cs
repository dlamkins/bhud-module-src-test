using Blish_HUD;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Soeed.GuildGeoGuesser.Feature.Shared.Models.V2;

namespace Soeed.GuildGeoGuesser
{
	public static class CompassMapUtils
	{
		public static bool TryGetScreenPosition(TutorialHint hint, Map? waypointMap, out CompassMarkerResult result)
		{
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			result = default(CompassMarkerResult);
			if (!GameService.Gw2Mumble.get_IsAvailable())
			{
				return false;
			}
			bool onWaypointMap = GameService.Gw2Mumble.get_CurrentMap().get_Id() == hint.MapId;
			bool mapOpen = GameService.Gw2Mumble.get_UI().get_IsMapOpen();
			if (!onWaypointMap && !mapOpen)
			{
				return false;
			}
			Vector2 screen;
			if (hint.TryGetContinent(out var continent))
			{
				screen = ScreenMapTransform.MapToScreen(continent);
			}
			else
			{
				if (waypointMap == null)
				{
					return false;
				}
				if (!ScreenMapTransform.TryWorldToScreen(waypointMap, hint.ToVector3(), out screen))
				{
					return false;
				}
			}
			Rectangle bounds = ScreenMapTransform.ScreenBounds;
			bool visible = ((Rectangle)(ref bounds)).Contains((int)screen.X, (int)screen.Y) || mapOpen;
			result = new CompassMarkerResult(screen, visible, !onWaypointMap);
			return visible;
		}
	}
}
