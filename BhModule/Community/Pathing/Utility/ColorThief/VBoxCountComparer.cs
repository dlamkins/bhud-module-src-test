using System.Collections.Generic;

namespace BhModule.Community.Pathing.Utility.ColorThief
{
	internal class VBoxCountComparer : IComparer<VBox>
	{
		public int Compare(VBox x, VBox y)
		{
			int a = x.Count(force: false);
			int b = y.Count(force: false);
			if (a >= b)
			{
				if (a <= b)
				{
					return 0;
				}
				return 1;
			}
			return -1;
		}
	}
}
