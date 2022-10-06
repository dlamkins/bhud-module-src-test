using Gw2Sharp.WebApi.V2.Models;
using Ideka.BHUDCommon;
using Microsoft.Xna.Framework;

namespace Ideka.RacingMeter
{
	public class MapBounds
	{
		public Rectangle Rectangle { get; set; }

		public Point Location
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_0009: Unknown result type (might be due to invalid IL or missing references)
				Rectangle rectangle = Rectangle;
				return ((Rectangle)(ref rectangle)).get_Location();
			}
		}

		public Point Size
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_0009: Unknown result type (might be due to invalid IL or missing references)
				Rectangle rectangle = Rectangle;
				return ((Rectangle)(ref rectangle)).get_Size();
			}
		}

		public Point Center
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_0009: Unknown result type (might be due to invalid IL or missing references)
				Rectangle rectangle = Rectangle;
				return ((Rectangle)(ref rectangle)).get_Center();
			}
		}

		public int X => Rectangle.X;

		public int Width => Rectangle.Width;

		public int Left
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				Rectangle rectangle = Rectangle;
				return ((Rectangle)(ref rectangle)).get_Left();
			}
		}

		public int Right
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				Rectangle rectangle = Rectangle;
				return ((Rectangle)(ref rectangle)).get_Right();
			}
		}

		public int Y => Rectangle.Y;

		public int Height => Rectangle.Height;

		public int Top
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				Rectangle rectangle = Rectangle;
				return ((Rectangle)(ref rectangle)).get_Top();
			}
		}

		public int Bottom
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				Rectangle rectangle = Rectangle;
				return ((Rectangle)(ref rectangle)).get_Bottom();
			}
		}

		public virtual Vector2 FromWorld(int mapId, Vector3 worldMeters)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			return MapOverlay.Data.WorldToMapScreen(mapId, worldMeters);
		}

		public virtual Vector2 FromMap(Vector2 mapCoords)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return MapOverlay.Data.MapToMapScreen(mapCoords);
		}

		public Rectangle FromMap(Rectangle rect)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			Vector2 topLeft = FromMap(((Rectangle)(ref rect)).get_TopLeft().ToXnaVector2());
			Vector2 bottomRight = FromMap(((Rectangle)(ref rect)).get_BottomRight().ToXnaVector2());
			Point val = ((Vector2)(ref topLeft)).ToPoint();
			Vector2 val2 = bottomRight - topLeft;
			return new Rectangle(val, ((Vector2)(ref val2)).ToPoint());
		}

		public bool Contains(Vector2 point)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			Rectangle rectangle = Rectangle;
			return ((Rectangle)(ref rectangle)).Contains(point);
		}
	}
}
