using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Kenedia.Modules.Core.Extensions
{
	public static class BitMapExtension
	{
		public static Bitmap ToBlackWhite(this Bitmap b, float colorThreshold = 0.5f)
		{
			Color black = Color.FromArgb(255, 0, 0, 0);
			Color white = Color.FromArgb(255, 255, 255, 255);
			for (int i = 0; i < b.Width; i++)
			{
				for (int j = 0; j < b.Height; j++)
				{
					b.SetPixel(i, j, (b.GetPixel(i, j).GetBrightness() > colorThreshold) ? black : white);
				}
			}
			return b;
		}

		public static (Bitmap, bool, double) IsColorAndCheckFilled(this Bitmap b, double threshold, Color startColor, Color endColor)
		{
			Color black = Color.FromArgb(255, 0, 0, 0);
			Color.FromArgb(255, 255, 255, 255);
			double total = b.Width * b.Height;
			double filled = 0.0;
			Point first = new Point(-1, -1);
			Point last = new Point(-1, -1);
			List<List<Point>> imageMap = new List<List<Point>>();
			for (int i = 0; i < b.Width; i++)
			{
				List<Point> column = new List<Point>();
				for (int j = 0; j < b.Height; j++)
				{
					Color oc = b.GetPixel(i, j);
					bool num = oc.R >= startColor.R || oc.R <= endColor.R;
					bool G = oc.G >= startColor.G || oc.G <= endColor.G;
					bool B = oc.B >= startColor.B || oc.B <= endColor.B;
					if (num && G && B)
					{
						if (first.X == -1 && first.Y == -1)
						{
							first = new Point(i, j);
						}
						if (last.X == -1 && last.Y == -1)
						{
							last = new Point(i, j);
						}
						first.X = Math.Min(first.X, i);
						first.Y = Math.Min(first.Y, j);
						last.X = Math.Max(last.X, i);
						last.Y = Math.Max(last.Y, j);
						column.Add(new Point(i, j));
					}
				}
				if (column.Count > 0)
				{
					imageMap.Add(column);
				}
				else if (imageMap.Count > 0 && i >= b.Width / 2)
				{
					break;
				}
			}
			int? maxHeight = imageMap.Select((List<Point> l) => l.Max((Point e) => e.Y))?.ToList()?.FirstOrDefault();
			Bitmap bitmap = new Bitmap(last.X - first.X + 1, last.Y - first.Y + 1);
			if (imageMap.Count > 0 && maxHeight.HasValue && maxHeight > 0)
			{
				foreach (List<Point> item in imageMap)
				{
					foreach (Point p in item)
					{
						int x = p.X - first.X;
						int y = p.Y - first.Y;
						bitmap.SetPixel(x, y, black);
					}
				}
			}
			return (bitmap, filled / total >= threshold, filled / total);
		}

		public static (Bitmap, bool, double) IsNotBlackAndCheckFilled(this Bitmap b, double threshold)
		{
			Color white = Color.FromArgb(255, 255, 255, 255);
			Point first = new Point(-1, -1);
			Point last = new Point(-1, -1);
			List<(Point, Color)> imageMap = new List<(Point, Color)>();
			int emptyInRow = 0;
			for (int j = 0; j < b.Width; j++)
			{
				List<(Point, Color)> column = new List<(Point, Color)>();
				for (int l = 0; l < b.Height; l++)
				{
					Color oc = b.GetPixel(j, l);
					if (oc.R < 5 && oc.G < 5 && oc.B < 5)
					{
						if (first.X == -1 && first.Y == -1)
						{
							first = new Point(j, l);
						}
						if (last.X == -1 && last.Y == -1)
						{
							last = new Point(j, l);
						}
						first.X = Math.Min(first.X, j);
						first.Y = Math.Min(first.Y, l);
						last.X = Math.Max(last.X, j);
						last.Y = Math.Max(last.Y, l);
						column.Add((new Point(j, l), oc));
					}
				}
				if (column.Count > 0)
				{
					imageMap.AddRange(column);
					emptyInRow = 0;
					continue;
				}
				emptyInRow++;
				if (emptyInRow >= 5 && first.X > -1)
				{
					break;
				}
			}
			Bitmap bitmap = new Bitmap(last.X - first.X + 1, last.Y - first.Y + 1);
			double total = bitmap.Width * bitmap.Height;
			for (int i = 0; i < bitmap.Width; i++)
			{
				for (int k = 0; k < bitmap.Height; k++)
				{
					bitmap.SetPixel(i, k, white);
				}
			}
			if (imageMap.Count > 0)
			{
				foreach (var t in imageMap)
				{
					Point item = t.Item1;
					int x = item.X - first.X;
					item = t.Item1;
					int y = item.Y - first.Y;
					bitmap.SetPixel(x, y, t.Item2);
				}
			}
			return (bitmap, (double)imageMap.Count / total >= threshold, (double)imageMap.Count / total);
		}

		public static (Bitmap, bool, double) IsCutAndCheckFilled(this Bitmap b, double threshold, float colorThreshold = 0.5f)
		{
			Color white = Color.FromArgb(255, 255, 255, 255);
			Point first = new Point(-1, -1);
			Point last = new Point(-1, -1);
			List<(Point, Color)> imageMap = new List<(Point, Color)>();
			int emptyInRow = 0;
			for (int j = 0; j < b.Width; j++)
			{
				List<(Point, Color)> column = new List<(Point, Color)>();
				for (int l = 0; l < b.Height; l++)
				{
					Color oc = b.GetPixel(j, l);
					if (oc.GetBrightness() > colorThreshold)
					{
						if (first.X == -1 && first.Y == -1)
						{
							first = new Point(j, l);
						}
						if (last.X == -1 && last.Y == -1)
						{
							last = new Point(j, l);
						}
						first.X = Math.Min(first.X, j);
						first.Y = Math.Min(first.Y, l);
						last.X = Math.Max(last.X, j);
						last.Y = Math.Max(last.Y, l);
						column.Add((new Point(j, l), oc));
					}
				}
				if (column.Count > 0)
				{
					imageMap.AddRange(column);
					emptyInRow = 0;
					continue;
				}
				emptyInRow++;
				if (emptyInRow >= 5 && first.X > -1)
				{
					break;
				}
			}
			Bitmap bitmap = new Bitmap(last.X - first.X + 1, last.Y - first.Y + 1);
			double total = bitmap.Width * bitmap.Height;
			for (int i = 0; i < bitmap.Width; i++)
			{
				for (int k = 0; k < bitmap.Height; k++)
				{
					bitmap.SetPixel(i, k, white);
				}
			}
			if (imageMap.Count > 0)
			{
				foreach (var t in imageMap)
				{
					Point item = t.Item1;
					int x = item.X - first.X;
					item = t.Item1;
					int y = item.Y - first.Y;
					bitmap.SetPixel(x, y, t.Item2);
				}
			}
			return (bitmap, (double)imageMap.Count / total >= threshold, (double)imageMap.Count / total);
		}

		public static (Bitmap, bool, double) IsFilled(this Bitmap b, double threshold, float colorThreshold = 0.5f)
		{
			Color black = Color.FromArgb(255, 0, 0, 0);
			Color white = Color.FromArgb(255, 255, 255, 255);
			double total = b.Width * b.Height;
			double filled = 0.0;
			for (int i = 0; i < b.Width; i++)
			{
				for (int j = 0; j < b.Height; j++)
				{
					Color oc = b.GetPixel(i, j);
					bool isFilled = oc.GetBrightness() > colorThreshold;
					filled += (double)(isFilled ? 1 : 0);
					b.SetPixel(i, j, (oc.GetBrightness() > colorThreshold) ? black : white);
				}
			}
			return (b, filled / total >= threshold, filled / total);
		}
	}
}
