using System;
using System.Collections.Generic;

namespace BhModule.Community.Pathing.Utility.ColorThief
{
	internal static class Mmcq
	{
		public const int Sigbits = 5;

		public const int Rshift = 3;

		public const int Mult = 8;

		public const int Histosize = 32768;

		public const int VboxLength = 32;

		public const double FractByPopulation = 0.75;

		public const int MaxIterations = 1000;

		private static readonly VBoxComparer ComparatorProduct = new VBoxComparer();

		private static readonly VBoxCountComparer ComparatorCount = new VBoxCountComparer();

		public static int GetColorIndex(int r, int g, int b)
		{
			return (r << 10) + (g << 5) + b;
		}

		private static int[] GetHisto(IEnumerable<byte[]> pixels)
		{
			int[] histo = new int[32768];
			foreach (byte[] pixel in pixels)
			{
				int rval = pixel[0] >> 3;
				int gval = pixel[1] >> 3;
				int bval = pixel[2] >> 3;
				int index = GetColorIndex(rval, gval, bval);
				histo[index]++;
			}
			return histo;
		}

		private static VBox VboxFromPixels(IList<byte[]> pixels, int[] histo)
		{
			int rmin = 1000000;
			int rmax = 0;
			int gmin = 1000000;
			int gmax = 0;
			int bmin = 1000000;
			int bmax = 0;
			int numPixels = pixels.Count;
			for (int i = 0; i < numPixels; i++)
			{
				byte[] array = pixels[i];
				int rval = array[0] >> 3;
				int gval = array[1] >> 3;
				int bval = array[2] >> 3;
				if (rval < rmin)
				{
					rmin = rval;
				}
				else if (rval > rmax)
				{
					rmax = rval;
				}
				if (gval < gmin)
				{
					gmin = gval;
				}
				else if (gval > gmax)
				{
					gmax = gval;
				}
				if (bval < bmin)
				{
					bmin = bval;
				}
				else if (bval > bmax)
				{
					bmax = bval;
				}
			}
			return new VBox(rmin, rmax, gmin, gmax, bmin, bmax, histo);
		}

		private static VBox[] DoCut(char color, VBox vbox, IList<int> partialsum, IList<int> lookaheadsum, int total)
		{
			int vboxDim1;
			int vboxDim2;
			switch (color)
			{
			case 'r':
				vboxDim1 = vbox.R1;
				vboxDim2 = vbox.R2;
				break;
			case 'g':
				vboxDim1 = vbox.G1;
				vboxDim2 = vbox.G2;
				break;
			default:
				vboxDim1 = vbox.B1;
				vboxDim2 = vbox.B2;
				break;
			}
			for (int i = vboxDim1; i <= vboxDim2; i++)
			{
				if (partialsum[i] > total / 2)
				{
					VBox vbox2 = vbox.Clone();
					VBox vbox3 = vbox.Clone();
					int left = i - vboxDim1;
					int right = vboxDim2 - i;
					int d2;
					for (d2 = ((left <= right) ? Math.Min(vboxDim2 - 1, Math.Abs(i + right / 2)) : Math.Max(vboxDim1, Math.Abs(Convert.ToInt32((double)(i - 1) - (double)left / 2.0)))); d2 < 0 || partialsum[d2] <= 0; d2++)
					{
					}
					int count2 = lookaheadsum[d2];
					while (count2 == 0 && d2 > 0 && partialsum[d2 - 1] > 0)
					{
						count2 = lookaheadsum[--d2];
					}
					switch (color)
					{
					case 'r':
						vbox2.R2 = d2;
						vbox3.R1 = d2 + 1;
						break;
					case 'g':
						vbox2.G2 = d2;
						vbox3.G1 = d2 + 1;
						break;
					default:
						vbox2.B2 = d2;
						vbox3.B1 = d2 + 1;
						break;
					}
					return new VBox[2] { vbox2, vbox3 };
				}
			}
			throw new Exception("VBox can't be cut");
		}

		private static VBox[] MedianCutApply(IList<int> histo, VBox vbox)
		{
			if (vbox.Count(force: false) == 0)
			{
				return null;
			}
			if (vbox.Count(force: false) == 1)
			{
				return new VBox[2]
				{
					vbox.Clone(),
					null
				};
			}
			int rw = vbox.R2 - vbox.R1 + 1;
			int gw = vbox.G2 - vbox.G1 + 1;
			int bw = vbox.B2 - vbox.B1 + 1;
			int maxw = Math.Max(Math.Max(rw, gw), bw);
			int total = 0;
			int[] partialsum = new int[32];
			for (int m = 0; m < partialsum.Length; m++)
			{
				partialsum[m] = -1;
			}
			int[] lookaheadsum = new int[32];
			for (int l = 0; l < lookaheadsum.Length; l++)
			{
				lookaheadsum[l] = -1;
			}
			if (maxw == rw)
			{
				for (int i = vbox.R1; i <= vbox.R2; i++)
				{
					int sum = 0;
					for (int j = vbox.G1; j <= vbox.G2; j++)
					{
						for (int k = vbox.B1; k <= vbox.B2; k++)
						{
							int index = GetColorIndex(i, j, k);
							sum += histo[index];
						}
					}
					total = (partialsum[i] = total + sum);
				}
			}
			else if (maxw == gw)
			{
				for (int i = vbox.G1; i <= vbox.G2; i++)
				{
					int sum = 0;
					for (int j = vbox.R1; j <= vbox.R2; j++)
					{
						for (int k = vbox.B1; k <= vbox.B2; k++)
						{
							int index = GetColorIndex(j, i, k);
							sum += histo[index];
						}
					}
					total = (partialsum[i] = total + sum);
				}
			}
			else
			{
				for (int i = vbox.B1; i <= vbox.B2; i++)
				{
					int sum = 0;
					for (int j = vbox.R1; j <= vbox.R2; j++)
					{
						for (int k = vbox.G1; k <= vbox.G2; k++)
						{
							int index = GetColorIndex(j, k, i);
							sum += histo[index];
						}
					}
					total = (partialsum[i] = total + sum);
				}
			}
			for (int i = 0; i < 32; i++)
			{
				if (partialsum[i] != -1)
				{
					lookaheadsum[i] = total - partialsum[i];
				}
			}
			if (maxw != rw)
			{
				if (maxw != gw)
				{
					return DoCut('b', vbox, partialsum, lookaheadsum, total);
				}
				return DoCut('g', vbox, partialsum, lookaheadsum, total);
			}
			return DoCut('r', vbox, partialsum, lookaheadsum, total);
		}

		private static void Iter(List<VBox> lh, IComparer<VBox> comparator, int target, IList<int> histo)
		{
			int ncolors = 1;
			int niters = 0;
			while (niters < 1000)
			{
				VBox vbox = lh[lh.Count - 1];
				if (vbox.Count(force: false) == 0)
				{
					lh.Sort(comparator);
					niters++;
					continue;
				}
				lh.RemoveAt(lh.Count - 1);
				VBox[] array = MedianCutApply(histo, vbox);
				VBox vbox2 = array[0];
				VBox vbox3 = array[1];
				if (vbox2 == null)
				{
					throw new Exception("vbox1 not defined; shouldn't happen!");
				}
				lh.Add(vbox2);
				if (vbox3 != null)
				{
					lh.Add(vbox3);
					ncolors++;
				}
				lh.Sort(comparator);
				if (ncolors < target && niters++ <= 1000)
				{
					continue;
				}
				break;
			}
		}

		public static CMap Quantize(byte[][] pixels, int maxcolors)
		{
			if (pixels.Length == 0 || maxcolors < 2 || maxcolors > 256)
			{
				return null;
			}
			int[] histo = GetHisto(pixels);
			VBox vbox = VboxFromPixels(pixels, histo);
			List<VBox> pq = new List<VBox> { vbox };
			int target = (int)Math.Ceiling(0.75 * (double)maxcolors);
			Iter(pq, ComparatorCount, target, histo);
			pq.Sort(ComparatorProduct);
			Iter(pq, ComparatorProduct, maxcolors - pq.Count, histo);
			pq.Reverse();
			CMap cmap = new CMap();
			foreach (VBox vb in pq)
			{
				cmap.Push(vb);
			}
			return cmap;
		}
	}
}
