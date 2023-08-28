using Microsoft.Xna.Framework;

namespace Kenedia.Modules.Core.Extensions
{
	public static class RectangleExtension
	{
		public static bool Equals(this Rectangle a, Rectangle b)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			bool num = a.Height == b.Height;
			bool width = a.Width == b.Width;
			bool left = ((Rectangle)(ref a)).get_Left() == ((Rectangle)(ref b)).get_Left();
			bool right = ((Rectangle)(ref a)).get_Right() == ((Rectangle)(ref b)).get_Right();
			bool top = ((Rectangle)(ref a)).get_Top() == ((Rectangle)(ref b)).get_Top();
			bool bottom = ((Rectangle)(ref a)).get_Bottom() == ((Rectangle)(ref b)).get_Bottom();
			return num && width && left && right && top && bottom;
		}
	}
}
