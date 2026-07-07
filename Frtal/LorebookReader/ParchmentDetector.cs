using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace Frtal.LorebookReader
{
	public static class ParchmentDetector
	{
		public const int LumThresh = 185;

		public const int ChromaThresh = 60;

		public const int Cell = 8;

		public unsafe static Rectangle? Find(Bitmap bmp, out double solidity)
		{
			solidity = 0.0;
			int w = bmp.Width;
			int h = bmp.Height;
			int cw = w / 8;
			int ch = h / 8;
			if (cw < 4 || ch < 4)
			{
				return null;
			}
			int[,] counts = new int[ch, cw];
			BitmapData data = bmp.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
			try
			{
				byte* basePtr = (byte*)(void*)data.Scan0;
				int usableH = ch * 8;
				int usableW = cw * 8;
				for (int y3 = 0; y3 < usableH; y3++)
				{
					byte* row = basePtr + y3 * data.Stride;
					int cy = y3 / 8;
					for (int x3 = 0; x3 < usableW; x3++)
					{
						byte b = row[x3 * 3];
						byte g = row[x3 * 3 + 1];
						byte r = row[x3 * 3 + 2];
						int num = (299 * r + 587 * g + 114 * b) / 1000;
						int max = ((r <= g) ? ((g > b) ? g : b) : ((r > b) ? r : b));
						int min = ((r >= g) ? ((g < b) ? g : b) : ((r < b) ? r : b));
						if (num > 185 && max - min < 60)
						{
							counts[cy, x3 / 8]++;
						}
					}
				}
			}
			finally
			{
				bmp.UnlockBits(data);
			}
			int cellPixels = 64;
			bool[,] cells = new bool[ch, cw];
			for (int y2 = 0; y2 < ch; y2++)
			{
				for (int x = 0; x < cw; x++)
				{
					cells[y2, x] = (double)counts[y2, x] >= 0.45 * (double)cellPixels;
				}
			}
			bool[,] seen = new bool[ch, cw];
			Stack<(int y, int x)> stack = new Stack<(int, int)>();
			Rectangle? best = null;
			int bestArea = 0;
			for (int sy = 0; sy < ch; sy++)
			{
				for (int sx = 0; sx < cw; sx++)
				{
					if (!cells[sy, sx] || seen[sy, sx])
					{
						continue;
					}
					int minY = sy;
					int maxY = sy;
					int minX = sx;
					int maxX = sx;
					int area = 0;
					seen[sy, sx] = true;
					stack.Push((sy, sx));
					while (stack.Count > 0)
					{
						(int y, int x) tuple = stack.Pop();
						int y = tuple.y;
						int x2 = tuple.x;
						area++;
						if (y < minY)
						{
							minY = y;
						}
						if (y > maxY)
						{
							maxY = y;
						}
						if (x2 < minX)
						{
							minX = x2;
						}
						if (x2 > maxX)
						{
							maxX = x2;
						}
						TryVisit(y - 1, x2);
						TryVisit(y + 1, x2);
						TryVisit(y, x2 - 1);
						TryVisit(y, x2 + 1);
					}
					int bh = maxY - minY + 1;
					int bw = maxX - minX + 1;
					double sol = (double)area / (double)(bh * bw);
					double ratio = (double)bh / (double)bw;
					double wFrac = (double)bw / (double)cw;
					double hFrac = (double)bh / (double)ch;
					if (sol > 0.6 && ratio > 0.9 && ratio < 3.0 && wFrac > 0.05 && wFrac < 0.85 && hFrac > 0.12 && hFrac < 0.98 && area > bestArea)
					{
						bestArea = area;
						solidity = sol;
						best = new Rectangle(minX * 8, minY * 8, bw * 8, bh * 8);
					}
				}
			}
			return best;
			void TryVisit(int ny, int nx)
			{
				if (ny >= 0 && ny < ch && nx >= 0 && nx < cw && cells[ny, nx] && !seen[ny, nx])
				{
					seen[ny, nx] = true;
					stack.Push((ny, nx));
				}
			}
		}

		public static Rectangle InnerCrop(Rectangle box)
		{
			int dx = (int)((double)box.Width * 0.03);
			int dyT = (int)((double)box.Height * 0.04);
			int dyB = (int)((double)box.Height * 0.02);
			return new Rectangle(box.X + dx, box.Y + dyT, box.Width - 2 * dx, box.Height - dyT - dyB);
		}
	}
}
