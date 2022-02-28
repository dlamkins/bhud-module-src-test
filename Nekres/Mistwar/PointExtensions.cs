using Microsoft.Xna.Framework;

namespace Nekres.Mistwar
{
	internal static class PointExtensions
	{
		public static Point ToBounds(this Point point, Rectangle bounds)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			return point + ((Rectangle)(ref bounds)).get_Location();
		}
	}
}
