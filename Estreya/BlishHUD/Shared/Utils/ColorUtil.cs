using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Estreya.BlishHUD.Shared.Utils
{
	public static class ColorUtil
	{
		private static Random random = new Random();

		private static Color[][] CreateColorGradientRows(Color start, Color end, int width, int height)
		{
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			List<Color[]> bgc = new List<Color[]>();
			float stepA = (float)(((Color)(ref end)).get_A() - ((Color)(ref start)).get_A()) / ((float)width - 1f);
			float stepR = (float)(((Color)(ref end)).get_R() - ((Color)(ref start)).get_R()) / ((float)width - 1f);
			float stepG = (float)(((Color)(ref end)).get_G() - ((Color)(ref start)).get_G()) / ((float)width - 1f);
			float stepB = (float)(((Color)(ref end)).get_B() - ((Color)(ref start)).get_B()) / ((float)width - 1f);
			for (int h = 0; h < height; h++)
			{
				Color[] colorRow = (Color[])(object)new Color[width];
				for (int w = 0; w < width; w++)
				{
					colorRow[w] = new Color(((float)(int)((Color)(ref start)).get_R() + stepR * (float)w) / 255f, ((float)(int)((Color)(ref start)).get_G() + stepG * (float)w) / 255f, ((float)(int)((Color)(ref start)).get_B() + stepB * (float)w) / 255f, ((float)(int)((Color)(ref start)).get_A() + stepA * (float)w) / 255f);
				}
				bgc.Add(colorRow);
			}
			return bgc.ToArray();
		}

		public static Color[] CreateColorGradient(Color start, Color end, int width, int height)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return CreateColorGradientRows(start, end, width, height).SelectMany((Color[] r) => r).ToArray();
		}

		public static Color[] CreateColorGradients(Color[] colors, int width, int height)
		{
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			if (colors == null || colors.Length < 2)
			{
				throw new ArgumentOutOfRangeException("colors", "A color gradient needs at least 2 colors.");
			}
			if (width <= 0 || height <= 0)
			{
				throw new ArgumentOutOfRangeException("size", "Width and Height must be at least 1.");
			}
			List<Color[]> rows = new List<Color[]>();
			int sectionWidth = width / (colors.Length - 1);
			for (int i = 0; i < colors.Length - 1; i++)
			{
				Color start = colors[i];
				Color end = colors[i + 1];
				Color[][] newRows = CreateColorGradientRows(start, end, sectionWidth, height);
				if (i != 0 && newRows.Length != rows.Count)
				{
					throw new ArgumentOutOfRangeException("rowCount", "A subsequential run has generated more rows than expected.");
				}
				for (int r = 0; r < newRows.Length; r++)
				{
					if (r == rows.Count)
					{
						rows.Add(newRows[r]);
					}
					else
					{
						rows[r] = rows[r].Concat(newRows[r]).ToArray();
					}
				}
			}
			if (rows.Count > 0 && rows[0].Length != width)
			{
				int missingWidth = width - rows[0].Length;
				for (int r2 = 0; r2 < rows.Count; r2++)
				{
					rows[r2] = rows[r2].Concat(Enumerable.Repeat<Color>(rows[r2].Last(), missingWidth)).ToArray();
				}
			}
			return rows.SelectMany((Color[] b) => b).ToArray();
		}

		public static Texture2D CreateColorGradientTexture(Color start, Color end, int width, int height)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Expected O, but got Unknown
			GraphicsDeviceContext ctx = GameService.Graphics.LendGraphicsDeviceContext();
			try
			{
				Texture2D val = new Texture2D(((GraphicsDeviceContext)(ref ctx)).get_GraphicsDevice(), width, height);
				Color[] colors = CreateColorGradient(start, end, width, height);
				val.SetData<Color>(colors);
				return val;
			}
			finally
			{
				((GraphicsDeviceContext)(ref ctx)).Dispose();
			}
		}

		public static Texture2D CreateColorGradientsTexture(Color[] colors, int width, int height)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Expected O, but got Unknown
			GraphicsDeviceContext ctx = GameService.Graphics.LendGraphicsDeviceContext();
			try
			{
				Texture2D val = new Texture2D(((GraphicsDeviceContext)(ref ctx)).get_GraphicsDevice(), width, height);
				Color[] gradients = CreateColorGradients(colors, width, height);
				val.SetData<Color>(gradients);
				return val;
			}
			finally
			{
				((GraphicsDeviceContext)(ref ctx)).Dispose();
			}
		}

		public static Color GetRandom()
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			return new Color(random.Next(256), random.Next(256), random.Next(256));
		}
	}
}
