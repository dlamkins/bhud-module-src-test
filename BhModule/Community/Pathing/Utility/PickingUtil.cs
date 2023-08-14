using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BhModule.Community.Pathing.Utility
{
	public static class PickingUtil
	{
		public static Ray CalculateRay(GraphicsDevice graphicsDevice, Point mouseLocation, Matrix view, Matrix projection)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			Viewport viewport = graphicsDevice.get_Viewport();
			Vector3 nearPoint = ((Viewport)(ref viewport)).Unproject(new Vector3((float)mouseLocation.X, (float)mouseLocation.Y, 0f), projection, view, Matrix.get_Identity());
			viewport = graphicsDevice.get_Viewport();
			Vector3 direction = ((Viewport)(ref viewport)).Unproject(new Vector3((float)mouseLocation.X, (float)mouseLocation.Y, 1f), projection, view, Matrix.get_Identity()) - nearPoint;
			((Vector3)(ref direction)).Normalize();
			return new Ray(nearPoint, direction);
		}

		public static float? IntersectDistance(GraphicsDevice graphicsDevice, BoundingBox box, Point mouseLocation, Matrix view, Matrix projection)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return IntersectDistance(box, CalculateRay(graphicsDevice, mouseLocation, view, projection));
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
