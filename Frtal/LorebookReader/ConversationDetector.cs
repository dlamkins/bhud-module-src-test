using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;

namespace Frtal.LorebookReader
{
	public static class ConversationDetector
	{
		private const int Cell = 8;

		private const int WarmDiffMin = 8;

		private const int LumMin = 20;

		private const int LumMax = 80;

		private const int LumMaxRelaxed = 150;

		private const int RedMin = 30;

		private const double MinWarmFrac = 0.35;

		private const double RelaxWarmFrac = 0.15;

		private const int BrightThresh = 170;

		private const int TextyCellMin = 3;

		private const int MinHeaderCells = 25;

		private const double MaxHeaderStartX = 0.65;

		private const double MaxAboveWarmFrac = 0.2;

		private const double MinTextBrightFrac = 0.003;

		private const int TextTopWindowRows = 20;

		private const int TextGapRows = 5;

		private const int MaxTextRows = 35;

		private const int HGapCols = 4;

		public static string LastDiagnostics { get; private set; } = "";


		public static Rectangle? Find(Bitmap bmp, out double solidity)
		{
			ConversationHit hit = FindHit(bmp);
			solidity = hit?.Confidence ?? 0.0;
			return hit?.Panel;
		}

		public unsafe static ConversationHit FindHit(Bitmap bmp)
		{
			int w = bmp.Width;
			int h = bmp.Height;
			int cw = w / 8;
			int ch = h / 8;
			if (cw < 20 || ch < 20)
			{
				LastDiagnostics = "frame too small";
				return null;
			}
			int[,] warmS = new int[ch, cw];
			int[,] warmR = new int[ch, cw];
			int[,] bright = new int[ch, cw];
			int cp = 64;
			BitmapData data = bmp.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
			try
			{
				byte* basePtr = (byte*)(void*)data.Scan0;
				int stride = data.Stride;
				int usableH = ch * 8;
				int usableW = cw * 8;
				for (int y = 0; y < usableH; y++)
				{
					byte* row = basePtr + y * stride;
					int cy = y / 8;
					for (int x = 0; x < usableW; x++)
					{
						byte b = row[x * 3];
						byte g = row[x * 3 + 1];
						byte r = row[x * 3 + 2];
						int lum = (299 * r + 587 * g + 114 * b) / 1000;
						int cx = x / 8;
						if (r > g && g > b && r - b >= 8 && r >= 30 && lum >= 20)
						{
							if (lum <= 80)
							{
								warmS[cy, cx]++;
							}
							if (lum <= 150)
							{
								warmR[cy, cx]++;
							}
						}
						if (lum > 170)
						{
							bright[cy, cx]++;
						}
					}
				}
			}
			finally
			{
				bmp.UnlockBits(data);
			}
			StringBuilder diag = new StringBuilder();
			ConversationHit result = Scan(warmS, bright, cw, ch, w, cp, diag, "strict") ?? Scan(warmR, bright, cw, ch, w, cp, diag, "relaxed");
			LastDiagnostics = diag.ToString();
			return result;
		}

		private static ConversationHit Scan(int[,] warm, int[,] bright, int cw, int ch, int w, int cp, StringBuilder diag, string pass)
		{
			int maxCellY = ch / 2;
			int cand = 0;
			int rejStart = 0;
			int rejAbove = 0;
			int rejText = 0;
			int rejTop = 0;
			int rejLR = 0;
			for (int cy = 4; cy < maxCellY; cy++)
			{
				int bestS = -1;
				int bestL = 0;
				int rs = -1;
				int rl = 0;
				for (int cx3 = 0; cx3 <= cw; cx3++)
				{
					if (cx3 < cw && (double)warm[cy, cx3] / (double)cp >= 0.35)
					{
						if (rs < 0)
						{
							rs = cx3;
						}
						rl++;
						continue;
					}
					if (rl > bestL)
					{
						bestS = rs;
						bestL = rl;
					}
					rs = -1;
					rl = 0;
				}
				if (bestL < 25)
				{
					continue;
				}
				cand++;
				if ((double)(bestS * 8) > (double)w * 0.65)
				{
					rejStart++;
					continue;
				}
				int aWarm = 0;
				int aTotal = 0;
				for (int dy2 = 4; dy2 <= 6; dy2++)
				{
					int ar = cy - dy2;
					if (ar < 0)
					{
						continue;
					}
					for (int cx = bestS; cx < bestS + bestL && cx < cw; cx++)
					{
						if ((double)warm[ar, cx] / (double)cp >= 0.35)
						{
							aWarm++;
						}
						aTotal++;
					}
				}
				if (aTotal > 0 && (double)aWarm / (double)aTotal > 0.2)
				{
					rejAbove++;
					continue;
				}
				long tBright = 0L;
				long tTotal = 0L;
				for (int dy = 4; dy <= 12; dy++)
				{
					int br = cy + dy;
					if (br >= ch)
					{
						break;
					}
					for (int cx2 = bestS; cx2 < bestS + bestL && cx2 < cw; cx2++)
					{
						tBright += bright[br, cx2];
						tTotal += cp;
					}
				}
				if (tTotal == 0L || (double)tBright / (double)tTotal < 0.003)
				{
					rejText++;
					continue;
				}
				int hL = bestS;
				while (hL > 0 && (double)warm[cy, hL - 1] / (double)cp >= 0.15)
				{
					hL--;
				}
				int hR;
				for (hR = bestS + bestL - 1; hR < cw - 1 && (double)warm[cy, hR + 1] / (double)cp >= 0.15; hR++)
				{
				}
				int halfR = hL + (hR - hL) / 2;
				int textTop = -1;
				int topLimit = Math.Min(cy + 20, ch);
				for (int r5 = cy + 2; r5 < topLimit; r5++)
				{
					int cnt2 = 0;
					for (int c5 = hL; c5 <= halfR && c5 < cw; c5++)
					{
						if (bright[r5, c5] >= 3)
						{
							cnt2++;
						}
					}
					if (cnt2 >= 3)
					{
						textTop = r5;
						break;
					}
				}
				if (textTop < 0)
				{
					rejTop++;
					continue;
				}
				int textBot = textTop;
				int empty = 0;
				for (int r4 = textTop; r4 < Math.Min(textTop + 35, ch); r4++)
				{
					int cnt = 0;
					for (int c4 = hL; c4 <= halfR && c4 < cw; c4++)
					{
						if (bright[r4, c4] >= 3)
						{
							cnt++;
						}
					}
					if (cnt < 2)
					{
						if (++empty >= 5)
						{
							break;
						}
					}
					else
					{
						empty = 0;
						textBot = r4;
					}
				}
				textBot++;
				int tL = hL;
				int tR = hR;
				int wideR = Math.Min(cw - 1, hR + (hR - hL));
				int wideL = Math.Max(0, hL - (hR - hL) / 4);
				int gap = 0;
				for (int c3 = hR; c3 <= wideR; c3++)
				{
					bool texty2 = false;
					for (int r3 = textTop; r3 < textBot; r3++)
					{
						if (texty2)
						{
							break;
						}
						if (bright[r3, c3] >= 3)
						{
							texty2 = true;
						}
					}
					if (texty2)
					{
						tR = c3;
						gap = 0;
					}
					else if (++gap > 4)
					{
						break;
					}
				}
				gap = 0;
				for (int c2 = hL; c2 >= wideL; c2--)
				{
					bool texty = false;
					for (int r2 = textTop; r2 < textBot; r2++)
					{
						if (texty)
						{
							break;
						}
						if (bright[r2, c2] >= 3)
						{
							texty = true;
						}
					}
					if (texty)
					{
						tL = c2;
						gap = 0;
					}
					else if (++gap > 4)
					{
						break;
					}
				}
				if (tR <= tL)
				{
					rejLR++;
					continue;
				}
				tL = Math.Max(0, tL - 2);
				tR = Math.Min(cw - 1, tR + 2);
				long areaBright = 0L;
				for (int r = textTop; r < textBot; r++)
				{
					for (int c = tL; c <= tR; c++)
					{
						areaBright += bright[r, c];
					}
				}
				Rectangle textArea = new Rectangle(tL * 8, Math.Max(0, (textTop - 1) * 8), (tR - tL + 1) * 8, (textBot - textTop + 2) * 8);
				int pL = Math.Min(hL * 8, textArea.Left);
				int pR = Math.Max((hR + 1) * 8, textArea.Right);
				int pT = cy * 8;
				int pB = textArea.Bottom;
				Rectangle panel = new Rectangle(pL, pT, pR - pL, pB - pT);
				long areaPx = (long)(textBot - textTop) * (long)(tR - tL + 1) * cp;
				diag.AppendLine($"[{pass}] HIT anchor row {cy}; " + $"candidates={cand} rejected: startX={rejStart} " + $"above={rejAbove} textBelow={rejText} " + $"textTop={rejTop} leftRight={rejLR}");
				return new ConversationHit
				{
					Panel = panel,
					TextArea = textArea,
					Confidence = ((areaPx > 0) ? ((double)areaBright / (double)areaPx) : 0.0)
				};
			}
			diag.AppendLine($"[{pass}] no hit; candidates={cand} rejected: " + $"startX={rejStart} above={rejAbove} textBelow={rejText} " + $"textTop={rejTop} leftRight={rejLR}");
			return null;
		}

		public unsafe static ConversationHit MeasureInZone(Bitmap bmp, Rectangle zone)
		{
			int zx = Math.Max(0, zone.X);
			int zy = Math.Max(0, zone.Y);
			int zr = Math.Min(bmp.Width, zone.Right);
			int num = Math.Min(bmp.Height, zone.Bottom);
			int zw = zr - zx;
			int zh = num - zy;
			if (zw < 32 || zh < 8)
			{
				LastDiagnostics = "zone too small";
				return null;
			}
			int cw = zw / 8;
			int ch = zh / 8;
			int[,] bright = new int[ch, cw];
			BitmapData data = bmp.LockBits(new Rectangle(zx, zy, cw * 8, ch * 8), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
			try
			{
				byte* basePtr = (byte*)(void*)data.Scan0;
				int stride = data.Stride;
				int uH = ch * 8;
				int uW = cw * 8;
				for (int y = 0; y < uH; y++)
				{
					byte* row = basePtr + y * stride;
					int cy = y / 8;
					for (int x = 0; x < uW; x++)
					{
						byte b = row[x * 3];
						byte g = row[x * 3 + 1];
						byte r3 = row[x * 3 + 2];
						if ((299 * r3 + 587 * g + 114 * b) / 1000 > 170)
						{
							bright[cy, x / 8]++;
						}
					}
				}
			}
			finally
			{
				bmp.UnlockBits(data);
			}
			int top = -1;
			int bot = -1;
			for (int r2 = 0; r2 < ch; r2++)
			{
				int cnt = 0;
				for (int c = 0; c < cw; c++)
				{
					if (bright[r2, c] >= 3)
					{
						cnt++;
					}
				}
				if (cnt >= 2)
				{
					if (top < 0)
					{
						top = r2;
					}
					bot = r2;
				}
			}
			if (top < 0)
			{
				LastDiagnostics = "[zone] no bright text — dialog closed?";
				return null;
			}
			int left = cw;
			int right = 0;
			long areaBright = 0L;
			for (int r = top; r <= bot; r++)
			{
				for (int c2 = 0; c2 < cw; c2++)
				{
					if (bright[r, c2] >= 3)
					{
						if (c2 < left)
						{
							left = c2;
						}
						if (c2 > right)
						{
							right = c2;
						}
					}
					areaBright += bright[r, c2];
				}
			}
			if (right < left)
			{
				LastDiagnostics = "[zone] no text columns";
				return null;
			}
			top = Math.Max(0, top - 1);
			left = Math.Max(0, left - 1);
			bot = Math.Min(ch - 1, bot + 1);
			right = Math.Min(cw - 1, right + 1);
			Rectangle textArea = new Rectangle(zx + left * 8, zy + top * 8, (right - left + 1) * 8, (bot - top + 1) * 8);
			Rectangle panel = new Rectangle(zx, zy, zw, zh);
			long areaPx = (long)(bot - top + 1) * (long)(right - left + 1) * 8 * 8;
			LastDiagnostics = $"[zone] text {textArea} in {panel}";
			return new ConversationHit
			{
				Panel = panel,
				TextArea = textArea,
				Confidence = ((areaPx > 0) ? ((double)areaBright / (double)areaPx) : 0.0)
			};
		}

		public static Rectangle TextCrop(Rectangle box)
		{
			int skipTop = (int)((double)box.Height * 0.18);
			int textH = (int)((double)box.Height * 0.4);
			return new Rectangle(box.X, box.Y + skipTop, box.Width, textH);
		}
	}
}
