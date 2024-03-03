using System.Collections.Generic;

namespace BhModule.Community.Pathing.Utility.ColorThief
{
	internal class VBoxComparer : IComparer<VBox>
	{
		public int Compare(VBox x, VBox y)
		{
			int num = x.Count(force: false);
			int bCount = y.Count(force: false);
			int aVolume = x.Volume(force: false);
			int bVolume = y.Volume(force: false);
			int a = num * aVolume;
			int b = bCount * bVolume;
			if (a >= b)
			{
				return (a > b) ? 1 : 0;
			}
			return -1;
		}
	}
}
