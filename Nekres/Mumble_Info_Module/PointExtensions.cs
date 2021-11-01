using Microsoft.Xna.Framework;

namespace Nekres.Mumble_Info_Module
{
	internal static class PointExtensions
	{
		public static bool IsInBounds(this Point p, Rectangle bounds)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			return p.X < bounds.X + bounds.Width && p.X > bounds.X && p.Y < bounds.Y + bounds.Height && p.Y > bounds.Y;
		}
	}
}
