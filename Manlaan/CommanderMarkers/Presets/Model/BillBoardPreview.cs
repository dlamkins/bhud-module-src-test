using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics;
using Blish_HUD.Input;
using Manlaan.CommanderMarkers.Library.Enums;
using Manlaan.CommanderMarkers.Pathing.Entities;
using Manlaan.CommanderMarkers.Settings.Services;
using Manlaan.CommanderMarkers.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Manlaan.CommanderMarkers.Presets.Model
{
	public class BillBoardPreview
	{
		private MapData _mapData;

		private Vector3 _trigger;

		private List<Billboard> _billboard = new List<Billboard>();

		private Billboard trigger = new Billboard();

		private MarkerSet _markerSet;

		public BillBoardPreview(MapData mapData, MarkerSet markerSet)
		{
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			_markerSet = markerSet;
			_mapData = mapData;
			markerSet.marks.ForEach(delegate(MarkerCoord mark)
			{
				//IL_0014: Unknown result type (might be due to invalid IL or missing references)
				//IL_0023: Unknown result type (might be due to invalid IL or missing references)
				Texture2D icon = ((SquadMarker)mark.icon).GetIcon();
				_billboard.Add(new Billboard(icon, mark.ToVector3(), new Vector2(1f, 1f)));
			});
			if (markerSet.trigger != null)
			{
				Texture2D markerIcon = Service.Textures!._blishHeart;
				trigger = new Billboard(markerIcon, markerSet.trigger!.ToVector3(), new Vector2(1f, 1f));
			}
		}

		public float DistanceFrom(Vector3 playerPosition)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			Vector3 val = playerPosition - _trigger;
			return ((Vector3)(ref val)).Length();
		}

		public void Draw()
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			GraphicsDeviceContext ctx = GameService.Graphics.LendGraphicsDeviceContext();
			try
			{
				if (GameService.Gw2Mumble.get_PlayerCharacter().get_IsInCombat() && !Service.Settings.AutoMarker_Allow_Combat_Placement.get_Value())
				{
					return;
				}
				float d = trigger.DistanceFromPlayer;
				trigger.HandleRebuild(((GraphicsDeviceContext)(ref ctx)).get_GraphicsDevice());
				trigger.Opacity = ((d < 2f) ? 0.25f : 0.8f);
				trigger.Draw(((GraphicsDeviceContext)(ref ctx)).get_GraphicsDevice());
				if (Service.Settings.AutoMarker_Billboard_Preview.get_Value() && d < 2f)
				{
					for (int i = 0; i < _billboard.Count; i++)
					{
						_billboard[i].HandleRebuild(((GraphicsDeviceContext)(ref ctx)).get_GraphicsDevice());
						_billboard[i].Draw(((GraphicsDeviceContext)(ref ctx)).get_GraphicsDevice());
					}
				}
			}
			finally
			{
				((GraphicsDeviceContext)(ref ctx)).Dispose();
			}
		}

		public string GetMarkerText()
		{
			return _markerSet?.name ?? "";
		}

		public bool PlayerWithinTriggerDistance()
		{
			return trigger.DistanceFromPlayer < 2f;
		}

		private Task PlaceMarkersInWorld(MarkerSet markers, MapData mapData)
		{
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_013c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_015d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
			if (markers.marks == null)
			{
				return Task.CompletedTask;
			}
			SettingService _setting = Service.Settings;
			float scale = GameService.Graphics.get_UIScaleMultiplier();
			List<KeyBinding> keys = new List<KeyBinding>
			{
				_setting._settingClearGndBinding.get_Value(),
				_setting._settingArrowGndBinding.get_Value(),
				_setting._settingCircleGndBinding.get_Value(),
				_setting._settingHeartGndBinding.get_Value(),
				_setting._settingSquareGndBinding.get_Value(),
				_setting._settingStarGndBinding.get_Value(),
				_setting._settingSpiralGndBinding.get_Value(),
				_setting._settingTriangleGndBinding.get_Value(),
				_setting._settingXGndBinding.get_Value(),
				_setting._settingClearGndBinding.get_Value()
			};
			int delay = _setting.AutoMarker_PlacementDelay.get_Value();
			bool useScreenCoords = MarkerPlacementHelper.UseScreenCoordinatesForPlacement();
			Point originalMousePos = MarkerPlacementHelper.GetPlacementCursorPosition(useScreenCoords);
			Rectangle screenBounds = ScreenMap.Data.ScreenBounds;
			InputHelper.DoHotKey(keys[0]);
			Thread.Sleep(delay / 2);
			List<string> errors = new List<string>();
			for (int i = 0; i < markers.marks.Count; i++)
			{
				MarkerCoord marker = markers.marks[i];
				if (marker.icon <= 9 && marker.icon >= 0)
				{
					Vector2 blishCoord = mapData.WorldToScreenMap(marker.ToVector3());
					Point placementPos = MarkerPlacementHelper.BlishToPlacementPosition(blishCoord, scale);
					if (((Rectangle)(ref screenBounds)).Contains(blishCoord))
					{
						MarkerPlacementHelper.SetPlacementMousePosition(placementPos, useScreenCoords);
						Thread.Sleep(delay / 2);
						InputHelper.DoHotKey(keys[marker.icon]);
						Thread.Sleep(delay);
					}
					else
					{
						errors.Add(((SquadMarker)marker.icon).EnumValue() + " " + marker.name);
					}
				}
			}
			if (errors.Count > 0)
			{
				ScreenNotification.ShowNotification($"Unable to place {errors.Count} marker(s)\nTry moving your map to the marker trigger", (NotificationType)1, (Texture2D)null, 6);
			}
			MarkerPlacementHelper.SetPlacementMousePosition(originalMousePos, useScreenCoords);
			return Task.CompletedTask;
		}
	}
}
