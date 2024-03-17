using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Blish_HUD;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Estreya.BlishHUD.Shared.Controls.Map
{
	public class MapBorder : MapEntity
	{
		private static Logger Logger = Logger.GetLogger<MapBorder>();

		private readonly Color _color;

		private readonly float[][] _points;

		private readonly float _thickness;

		private readonly float _x;

		private readonly float _y;

		public MapBorder(float x, float y, float[][] points, Color color, float thickness = 1f)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			_x = x;
			_y = y;
			_points = points;
			_color = color;
			_thickness = thickness;
		}

		public override RectangleF? RenderToMiniMap(SpriteBatch spriteBatch, Rectangle bounds, double offsetX, double offsetY, double scale, float opacity)
		{
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			GetScaledLocation(_x, _y, scale, offsetX, offsetY);
			ReadOnlyCollection<Vector2> points = _points.Select((float[] p) => GetScaledLocation(p[0], p[1], scale, offsetX, offsetY)).ToList().AsReadOnly();
			ShapeExtensions.DrawPolygon(spriteBatch, Vector2.get_Zero(), (IReadOnlyList<Vector2>)points, _color, _thickness, 0f);
			float top = points.Min((Vector2 p) => p.Y);
			float bottom = points.Max((Vector2 p) => p.Y);
			float left = points.Min((Vector2 p) => p.X);
			float right = points.Max((Vector2 p) => p.X);
			return new RectangleF(left, top, right - left, bottom - top);
		}
	}
}
