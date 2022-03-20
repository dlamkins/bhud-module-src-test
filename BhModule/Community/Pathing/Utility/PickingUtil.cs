using Blish_HUD;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BhModule.Community.Pathing.Utility
{
	public static class PickingUtil
	{
		public static Ray CalculateRay(Point mouseLocation, Matrix view, Matrix projection)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			Viewport viewport = GameService.Graphics.get_GraphicsDevice().get_Viewport();
			Vector3 nearPoint = ((Viewport)(ref viewport)).Unproject(new Vector3((float)mouseLocation.X, (float)mouseLocation.Y, 0f), projection, view, Matrix.get_Identity());
			viewport = GameService.Graphics.get_GraphicsDevice().get_Viewport();
			Vector3 direction = ((Viewport)(ref viewport)).Unproject(new Vector3((float)mouseLocation.X, (float)mouseLocation.Y, 1f), projection, view, Matrix.get_Identity()) - nearPoint;
			((Vector3)(ref direction)).Normalize();
			return new Ray(nearPoint, direction);
		}

		public static float? IntersectDistance(BoundingBox box, Point mouseLocation, Matrix view, Matrix projection)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			return IntersectDistance(box, CalculateRay(mouseLocation, view, projection));
		}

		public static float? IntersectDistance(BoundingBox box, Ray ray)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			return ((Ray)(ref ray)).Intersects(box);
		}

		public static float? IntersectDistance(BoundingSphere sphere, Ray ray)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			return ((Ray)(ref ray)).Intersects(sphere);
		}
	}
}
