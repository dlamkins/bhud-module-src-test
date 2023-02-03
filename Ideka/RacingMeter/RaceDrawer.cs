using System;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Ideka.BHUDCommon;
using Ideka.RacingMeter.Lib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;

namespace Ideka.RacingMeter
{
	public abstract class RaceDrawer : Control
	{
		private static readonly Primitive Circle = Primitive.HorizontalCircle(1f, 100);

		private static readonly Primitive Arc = Primitive.VerticalArc(1f, 1f, 0f, -1f, 50);

		private static readonly Primitive Arrow = new Primitive(new Vector3(-0.5f, 0f, 0f), new Vector3(0f, 0.5f, 0f), new Vector3(0.5f, 0f, 0f), new Vector3(355f / (678f * (float)Math.PI), 0f, 0f), new Vector3(355f / (678f * (float)Math.PI), -0.5f, 0f), new Vector3(-355f / (678f * (float)Math.PI), -0.5f, 0f), new Vector3(-355f / (678f * (float)Math.PI), 0f, 0f), new Vector3(-0.5f, 0f, 0f));

		private static readonly Vector3 Up = new Vector3(0f, 0f, 1f);

		private static readonly Primitive GhostA = Primitive.HorizontalCircle(1f, 100);

		private static readonly Primitive GhostB = new Primitive(Vector3.get_Zero(), new Vector3(0f, 1f, 0f));

		public const float MapThickness = 2f;

		public static readonly Color ResetColor = Color.get_Red();

		public static readonly Color CheckpointColor = Color.get_White();

		public static readonly Color TouchedCheckpointColor = Color.get_NavajoWhite();

		public static readonly Color GuidePointColor = Color.get_SkyBlue();

		public static readonly Color NextCheckpointColor = Color.get_SkyBlue();

		public static readonly Color FinalCheckpointColor = Color.get_Yellow();

		public static readonly Color GhostColor = Color.get_White();

		public static float Thickness => RacingModule.Settings.CheckpointLineThickness.Value;

		public static float Alpha => RacingModule.Settings.CheckpointAlpha.Value;

		public static bool ShowArrow => RacingModule.Settings.CheckpointArrow.Value;

		public abstract Race? Race { get; }

		protected SpriteBatchParameters UIParams { get; }

		public RaceDrawer()
			: this()
		{
			UIParams = ((Control)this).get_SpriteBatchParameters();
			((Control)this).set_SpriteBatchParameters(((Control)this).get_SpriteBatchParameters().Clone());
			((Control)this).get_SpriteBatchParameters().set_SortMode((SpriteSortMode)3);
			((Control)this).get_SpriteBatchParameters().set_BlendState(BlendState.NonPremultiplied);
		}

		protected override CaptureType CapturesInput()
		{
			return (CaptureType)0;
		}

		public static Matrix GetTRS(float radius)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			return Matrix.CreateScale(radius) * Matrix.CreateRotationX(-(float)Math.PI / 2f);
		}

		public void DrawRacePoint(SpriteBatch spriteBatch, Vector3 position, float radius, Vector3? next, Color color, bool flat = false)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			Matrix trs = GetTRS(radius) * Matrix.CreateConstrainedBillboard(position, GameService.Gw2Mumble.get_PlayerCamera().get_Position(), Up, (Vector3?)null, (Vector3?)null);
			Circle.Transformed(trs).ToScreen().Draw(spriteBatch, color * Alpha, Thickness);
			if (!flat)
			{
				Arc.Transformed(trs).ToScreen().Draw(spriteBatch, color * Alpha, Thickness);
			}
			if (next.HasValue)
			{
				Vector3 nextPosition = next.GetValueOrDefault();
				if (ShowArrow)
				{
					DrawRaceArrow(spriteBatch, position, radius, nextPosition, color);
				}
			}
		}

		public void DrawRaceArrow(SpriteBatch spriteBatch, Vector3 position, float radius, Vector3 next, Color color)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			Matrix tRS = GetTRS(radius);
			Vector3 up = default(Vector3);
			((Vector3)(ref up))._002Ector(0f, 0f, 1f);
			Matrix trs = tRS * Matrix.CreateConstrainedBillboard(position, next, up, (Vector3?)null, (Vector3?)null);
			Arrow.Transformed(trs).ToScreen().Draw(spriteBatch, color * Alpha, Thickness);
		}

		public void DrawRacePoint(SpriteBatch spriteBatch, Vector3 position, float radius, Color color, bool flat = false)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			DrawRacePoint(spriteBatch, position, radius, null, color, flat);
		}

		public void DrawRacePoint(SpriteBatch spriteBatch, RacePoint point, RacePoint next, Color color, bool flat = false)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			DrawRacePoint(spriteBatch, point.Position, point.Radius, next?.Position, color, flat);
		}

		public void DrawRacePoint(SpriteBatch spriteBatch, RacePoint point, Color color, bool flat = false)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			DrawRacePoint(spriteBatch, point.Position, point.Radius, null, color, flat);
		}

		public void DrawMapRacePoint(SpriteBatch spriteBatch, Vector2 position, Color color, float radius = 10f)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			ShapeExtensions.DrawCircle(spriteBatch, position, radius, 16, color, 2f, 0f);
		}

		public void DrawText(SpriteBatch spriteBatch, Vector2 position, BitmapFont font, Color color, string text)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			Point size = (Point)font.MeasureString(text);
			Rectangle rect = default(Rectangle);
			((Rectangle)(ref rect))._002Ector(new Point((int)(position.X - (float)size.X / 2f), (int)(position.Y - (float)size.Y / 2f)), size);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, font, rect, color, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
		}

		public void DrawMapRacePoint(SpriteBatch spriteBatch, IMapBounds map, RacePoint point, Color color, float radius = 10f)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			Race race = Race;
			if (race != null)
			{
				DrawMapRacePoint(spriteBatch, map.FromWorld(race.MapId, point.Position), color, radius);
			}
		}

		public void DrawMapEdgeRacePoint(SpriteBatch spriteBatch, IMapBounds map, RacePoint point, Color color, float radius = 10f)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			Race race = Race;
			if (race != null)
			{
				Vector2 pos = map.FromWorld(race.MapId, point.Position);
				if (map.Contains(pos))
				{
					DrawMapRacePoint(spriteBatch, map, point, color, radius);
				}
				else
				{
					DrawMapRacePoint(spriteBatch, Intersect(pos, map.Rectangle), color, radius);
				}
			}
		}

		public void DrawMapLine(SpriteBatch spriteBatch, IMapBounds map)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			IMapBounds map2 = map;
			Race race = Race;
			if (race != null)
			{
				spriteBatch.DrawPolygon(Vector2.get_Zero(), race.RoadPoints.Select((RacePoint c) => map2.FromWorld(Race!.MapId, c.Position)), CheckpointColor, 2f, 0f, open: true);
			}
		}

		public void DrawGhost(SpriteBatch spriteBatch, IMapBounds map, GhostSnapshot snapshot)
		{
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			DrawGhost(spriteBatch, map, snapshot, GhostColor);
		}

		public void DrawGhost(SpriteBatch spriteBatch, IMapBounds map, GhostSnapshot snapshot, Color color)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			Race race = Race;
			if (race != null)
			{
				Matrix trs = Matrix.CreateScale(10f) * Matrix.CreateRotationZ((float)(Math.Atan2(snapshot.Front.X, snapshot.Front.Y) + Math.PI));
				Vector2 position = map.FromWorld(race.MapId, snapshot.Position);
				spriteBatch.DrawPolygon(position, GhostA.Transformed(trs).Flat(), color, 2f);
				spriteBatch.DrawPolygon(position, GhostB.Transformed(trs).Flat(), color, 2f);
			}
		}

		public static Vector2 Intersect(Vector2 point, Rectangle rect)
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0164: Unknown result type (might be due to invalid IL or missing references)
			int midX = (((Rectangle)(ref rect)).get_Left() + ((Rectangle)(ref rect)).get_Right()) / 2;
			int midY = (((Rectangle)(ref rect)).get_Top() + ((Rectangle)(ref rect)).get_Bottom()) / 2;
			float i = ((float)midY - point.Y) / ((float)midX - point.X);
			if (point.X <= (float)midX)
			{
				float minXy = i * ((float)((Rectangle)(ref rect)).get_Left() - point.X) + point.Y;
				if ((float)((Rectangle)(ref rect)).get_Top() <= minXy && minXy <= (float)((Rectangle)(ref rect)).get_Bottom())
				{
					return new Vector2((float)((Rectangle)(ref rect)).get_Left(), minXy);
				}
			}
			if (point.X >= (float)midX)
			{
				float maxXy = i * ((float)((Rectangle)(ref rect)).get_Right() - point.X) + point.Y;
				if ((float)((Rectangle)(ref rect)).get_Top() <= maxXy && maxXy <= (float)((Rectangle)(ref rect)).get_Bottom())
				{
					return new Vector2((float)((Rectangle)(ref rect)).get_Right(), maxXy);
				}
			}
			if (point.Y <= (float)midY)
			{
				float minYx = ((float)((Rectangle)(ref rect)).get_Top() - point.Y) / i + point.X;
				if ((float)((Rectangle)(ref rect)).get_Left() <= minYx && minYx <= (float)((Rectangle)(ref rect)).get_Right())
				{
					return new Vector2(minYx, (float)((Rectangle)(ref rect)).get_Top());
				}
			}
			if (point.Y >= (float)midY)
			{
				float maxYx = ((float)((Rectangle)(ref rect)).get_Bottom() - point.Y) / i + point.X;
				if ((float)((Rectangle)(ref rect)).get_Left() <= maxYx && maxYx <= (float)((Rectangle)(ref rect)).get_Right())
				{
					return new Vector2(maxYx, (float)((Rectangle)(ref rect)).get_Bottom());
				}
			}
			return point;
		}
	}
}
