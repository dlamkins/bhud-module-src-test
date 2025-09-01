using Microsoft.Xna.Framework;

namespace Kenedia.Modules.Core.Structs
{
	public struct ColorGradient
	{
		public Color Start { get; set; }

		public Color End { get; set; }

		public ColorGradient(Color start, Color? end = null)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			Start = start;
			End = (Color)(((_003F?)end) ?? Color.get_Transparent());
		}
	}
}
