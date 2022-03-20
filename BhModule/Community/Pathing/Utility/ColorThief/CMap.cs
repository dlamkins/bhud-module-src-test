using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace BhModule.Community.Pathing.Utility.ColorThief
{
	internal class CMap
	{
		private readonly List<VBox> vboxes = new List<VBox>();

		private List<QuantizedColor> palette;

		public void Push(VBox box)
		{
			palette = null;
			vboxes.Add(box);
		}

		public List<QuantizedColor> GeneratePalette()
		{
			if (palette == null)
			{
				palette = (from vBox in vboxes
					let rgb = vBox.Avg(force: false)
					let color = FromRgb(rgb[0], rgb[1], rgb[2])
					select new QuantizedColor(color, vBox.Count(force: false))).ToList();
			}
			return palette;
		}

		public Color FromRgb(int red, int green, int blue)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			Color result = default(Color);
			((Color)(ref result)).set_A(byte.MaxValue);
			((Color)(ref result)).set_R((byte)red);
			((Color)(ref result)).set_G((byte)green);
			((Color)(ref result)).set_B((byte)blue);
			return result;
		}
	}
}
