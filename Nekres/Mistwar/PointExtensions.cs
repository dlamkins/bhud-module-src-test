using Microsoft.Xna.Framework;

namespace Nekres.Mistwar
{
	internal static class PointExtensions
	{
		public static Point ToBounds(this Point point, Rectangle bounds)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			return point + ((Rectangle)(ref bounds)).get_Location();
		}
	}
}
