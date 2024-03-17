using Blish_HUD;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Estreya.BlishHUD.Shared.Controls.Map
{
	public class MapCircle : MapEntity
	{
		private static Logger Logger = Logger.GetLogger<MapCircle>();

		private readonly Color _color;

		private readonly float _radius;

		private readonly float _thickness;

		private readonly float _x;

		private readonly float _y;

		public MapCircle(float x, float y, float radius, Color color, float thickness = 1f)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			_x = x;
			_y = y;
			_radius = radius;
			_color = color;
			_thickness = thickness;
		}

		public override RectangleF? RenderToMiniMap(SpriteBatch spriteBatch, Rectangle bounds, double offsetX, double offsetY, double scale, float opacity)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			Vector2 location = GetScaledLocation(_x, _y, scale, offsetX, offsetY);
			float radius = _radius / (float)scale;
			CircleF circle = default(CircleF);
			((CircleF)(ref circle))._002Ector(new Point2(location.X, location.Y), radius);
			ShapeExtensions.DrawCircle(spriteBatch, circle, 50, _color, _thickness, 0f);
			return ((CircleF)(ref circle)).ToRectangleF();
		}
	}
}
