using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Controls;
using Gw2Sharp.WebApi.V2.Models;
using Ideka.BHUDCommon;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ideka.RacingMeter
{
	public class MapOverlay : Control
	{
		public static class Data
		{
			private const double BlishScale = 1.1148272017837235;

			private static int LastTick { get; set; }

			public static Vector2 MapCenter { get; private set; }

			public static Matrix MapRotation { get; private set; }

			public static float Scale { get; private set; }

			public static Rectangle ScreenBounds { get; private set; }

			public static Vector2 BoundsCenter { get; private set; }

			public static void Update()
			{
				//IL_002b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0030: Unknown result type (might be due to invalid IL or missing references)
				//IL_0073: Unknown result type (might be due to invalid IL or missing references)
				//IL_007d: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
				//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
				//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
				//IL_00da: Unknown result type (might be due to invalid IL or missing references)
				if (GameService.Gw2Mumble.get_Tick() != LastTick)
				{
					LastTick = GameService.Gw2Mumble.get_Tick();
					MapCenter = GameService.Gw2Mumble.get_UI().get_MapCenter().ToXnaVector2();
					MapRotation = Matrix.CreateRotationZ((GameService.Gw2Mumble.get_UI().get_IsCompassRotationEnabled() && !GameService.Gw2Mumble.get_UI().get_IsMapOpen()) ? ((float)GameService.Gw2Mumble.get_UI().get_CompassRotation()) : 0f);
					ScreenBounds = MumbleUtils.GetMapBounds();
					Scale = (float)(1.1148272017837235 / GameService.Gw2Mumble.get_UI().get_MapScale());
					Rectangle screenBounds = ScreenBounds;
					Point val = ((Rectangle)(ref screenBounds)).get_Location();
					Vector2 val2 = ((Point)(ref val)).ToVector2();
					screenBounds = ScreenBounds;
					val = ((Rectangle)(ref screenBounds)).get_Size();
					BoundsCenter = val2 + ((Point)(ref val)).ToVector2() / 2f;
				}
			}

			public static Vector2 ToMapScreen(Vector3 worldMeters)
			{
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				return ToMapScreen(GameService.Gw2Mumble.get_CurrentMap().get_Id(), worldMeters);
			}

			public static Vector2 ToMapScreen(int mapId, Vector3 worldMeters)
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0011: Unknown result type (might be due to invalid IL or missing references)
				//IL_0016: Unknown result type (might be due to invalid IL or missing references)
				return ToMap(mapId, worldMeters, MapCenter, Scale, MapRotation, BoundsCenter);
			}

			public static Vector2 ToMap(int mapId, Vector3 worldMeters, Vector2 mapCenter, float scale, Matrix rotation, Vector2 boundsCenter)
			{
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_0011: Unknown result type (might be due to invalid IL or missing references)
				//IL_0016: Unknown result type (might be due to invalid IL or missing references)
				//IL_0017: Unknown result type (might be due to invalid IL or missing references)
				//IL_001d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0022: Unknown result type (might be due to invalid IL or missing references)
				//IL_0024: Unknown result type (might be due to invalid IL or missing references)
				//IL_0029: Unknown result type (might be due to invalid IL or missing references)
				//IL_002b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0031: Unknown result type (might be due to invalid IL or missing references)
				Map map = RacingModule.MapData.GetMap(mapId);
				if (map != null)
				{
					return Vector2.Transform((map.WorldMetersToMap(worldMeters) - mapCenter) * scale, rotation) + boundsCenter;
				}
				return Vector2.get_Zero();
			}
		}

		private readonly List<IMapEntity> _entities = new List<IMapEntity>();

		protected override CaptureType CapturesInput()
		{
			return (CaptureType)0;
		}

		public void AddEntity(IMapEntity entity)
		{
			_entities.Add(entity);
		}

		public void RemoveEntity(IMapEntity entity)
		{
			_entities.Remove(entity);
		}

		public override void DoUpdate(GameTime gameTime)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).DoUpdate(gameTime);
			Data.Update();
			Rectangle screenBounds = Data.ScreenBounds;
			((Control)this).set_Location(((Rectangle)(ref screenBounds)).get_Location());
			screenBounds = Data.ScreenBounds;
			((Control)this).set_Size(((Rectangle)(ref screenBounds)).get_Size());
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			if (!GameService.GameIntegration.get_Gw2Instance().get_IsInGame() || RacingModule.MapData.Current == null)
			{
				return;
			}
			((Rectangle)(ref bounds)).set_Location(((Control)this).get_Location());
			foreach (IMapEntity entity in _entities)
			{
				entity.DrawToMap(spriteBatch, new MapBounds
				{
					Rectangle = bounds
				});
			}
		}

		public MapOverlay()
			: this()
		{
		}
	}
}
