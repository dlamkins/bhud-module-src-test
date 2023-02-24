using Microsoft.Xna.Framework;
using Tortle.PlayerMarker.Models;

namespace Tortle.PlayerMarker.Util
{
	internal static class ConversionUtil
	{
		public static Color ToRgb(string colorName)
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			if (!ColorPresets.Colors.TryGetValue(colorName, out var color))
			{
				return ColorPresets.Colors["White"];
			}
			return color;
		}
	}
}
