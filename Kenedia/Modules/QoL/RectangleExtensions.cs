using Microsoft.Xna.Framework;

namespace Kenedia.Modules.QoL
{
	internal static class RectangleExtensions
	{
		public static string ConvertToString(this Rectangle r)
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			return $"X: {((Rectangle)(ref r)).get_Left()}, Y: {((Rectangle)(ref r)).get_Top()}, Width: {r.Width}, Height: {r.Height}";
		}
	}
}
