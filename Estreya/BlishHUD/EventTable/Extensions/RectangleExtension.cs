using MonoGame.Extended;

namespace Estreya.BlishHUD.EventTable.Extensions
{
	public static class RectangleExtension
	{
		public static RectangleF Add(this RectangleF u1, float x, float y, float width, float height)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			return new RectangleF(u1.X + x, u1.Y + y, u1.Width + width, u1.Height + height);
		}

		public static RectangleF ToBounds(this RectangleF r1, RectangleF bounds)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			return new RectangleF(new Point2(r1.X + bounds.X, r1.Y + bounds.Y), ((RectangleF)(ref r1)).get_Size());
		}
	}
}
