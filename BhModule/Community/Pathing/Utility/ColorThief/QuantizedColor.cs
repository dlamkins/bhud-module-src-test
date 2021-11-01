using System;
using Microsoft.Xna.Framework;

namespace BhModule.Community.Pathing.Utility.ColorThief
{
	public class QuantizedColor
	{
		public Color Color { get; private set; }

		public int Population { get; private set; }

		public bool IsDark { get; private set; }

		public QuantizedColor(Color color, int population)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			Color = color;
			Population = population;
			IsDark = CalculateYiqLuma(color) < 128;
		}

		public int CalculateYiqLuma(Color color)
		{
			return Convert.ToInt32(Math.Round((float)(299 * ((Color)(ref color)).get_R() + 587 * ((Color)(ref color)).get_G() + 114 * ((Color)(ref color)).get_B()) / 1000f));
		}
	}
}
