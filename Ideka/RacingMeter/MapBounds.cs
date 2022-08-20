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

		public virtual Vector2 Locate(int mapId, Vector3 worldMeters)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			return MapOverlay.Data.ToMapScreen(mapId, worldMeters);
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
