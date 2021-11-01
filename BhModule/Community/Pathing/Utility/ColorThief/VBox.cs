using System;

namespace BhModule.Community.Pathing.Utility.ColorThief
{
	internal class VBox
	{
		private readonly int[] histo;

		private int[] avg;

		public int B1;

		public int B2;

		private int? count;

		public int G1;

		public int G2;

		public int R1;

		public int R2;

		private int? volume;

		public VBox(int r1, int r2, int g1, int g2, int b1, int b2, int[] histo)
		{
			R1 = r1;
			R2 = r2;
			G1 = g1;
			G2 = g2;
			B1 = b1;
			B2 = b2;
			this.histo = histo;
		}

		public int Volume(bool force)
		{
			if (!volume.HasValue || force)
			{
				volume = (R2 - R1 + 1) * (G2 - G1 + 1) * (B2 - B1 + 1);
			}
			return volume.Value;
		}

		public int Count(bool force)
		{
			if (!count.HasValue || force)
			{
				int npix = 0;
				for (int i = R1; i <= R2; i++)
				{
					for (int j = G1; j <= G2; j++)
					{
						for (int k = B1; k <= B2; k++)
						{
							int index = Mmcq.GetColorIndex(i, j, k);
							npix += histo[index];
						}
					}
				}
				count = npix;
			}
			return count.Value;
		}

		public VBox Clone()
		{
			return new VBox(R1, R2, G1, G2, B1, B2, histo);
		}

		public int[] Avg(bool force)
		{
			if (avg == null || force)
			{
				int ntot = 0;
				int rsum = 0;
				int gsum = 0;
				int bsum = 0;
				for (int i = R1; i <= R2; i++)
				{
					for (int j = G1; j <= G2; j++)
					{
						for (int k = B1; k <= B2; k++)
						{
							int histoindex = Mmcq.GetColorIndex(i, j, k);
							int hval = histo[histoindex];
							ntot += hval;
							rsum += Convert.ToInt32((double)hval * ((double)i + 0.5) * 8.0);
							gsum += Convert.ToInt32((double)hval * ((double)j + 0.5) * 8.0);
							bsum += Convert.ToInt32((double)hval * ((double)k + 0.5) * 8.0);
						}
					}
				}
				if (ntot > 0)
				{
					avg = new int[3]
					{
						Math.Abs(rsum / ntot),
						Math.Abs(gsum / ntot),
						Math.Abs(bsum / ntot)
					};
				}
				else
				{
					avg = new int[3]
					{
						Math.Abs(8 * (R1 + R2 + 1) / 2),
						Math.Abs(8 * (G1 + G2 + 1) / 2),
						Math.Abs(8 * (B1 + B2 + 1) / 2)
					};
				}
			}
			return avg;
		}
	}
}
