using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Frtal.LorebookReader
{
	public sealed class TextRenderer : IDisposable
	{
		private readonly GraphicsDevice _graphicsDevice;

		private readonly Dictionary<string, Texture2D> _cache = new Dictionary<string, Texture2D>();

		private readonly Queue<string> _cacheOrder = new Queue<string>();

		private const int MaxCache = 60;

		private static readonly string[] FontCandidates = new string[4] { "Cantarell", "Segoe UI", "Tahoma", "Arial" };

		private static readonly string ResolvedFamily = ResolveFontFamily();

		private readonly object _measureLock = new object();

		private Bitmap _measureBmp;

		private Graphics _measureGraphics;

		private readonly Dictionary<long, float> _lineHeightCache = new Dictionary<long, float>();

		public TextRenderer(GraphicsDevice graphicsDevice)
		{
			_graphicsDevice = graphicsDevice;
		}

		public Texture2D RenderLine(string text, float fontSize, Color color, bool bold = false)
		{
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			if (string.IsNullOrEmpty(text))
			{
				return null;
			}
			string key = $"{fontSize}|{(bold ? 1 : 0)}|{((Color)(ref color)).get_PackedValue()}|{text}";
			if (_cache.TryGetValue(key, out var cached))
			{
				return cached;
			}
			Texture2D tex = Render(text, fontSize, color, bold);
			_cache[key] = tex;
			_cacheOrder.Enqueue(key);
			if (_cacheOrder.Count > 60)
			{
				string old = _cacheOrder.Dequeue();
				if (_cache.TryGetValue(old, out var oldTex))
				{
					_cache.Remove(old);
					((GraphicsResource)oldTex).Dispose();
				}
			}
			return tex;
		}

		public float MeasureWidth(string text, float fontSize, bool bold = false)
		{
			if (string.IsNullOrEmpty(text))
			{
				return 0f;
			}
			lock (_measureLock)
			{
				EnsureMeasureContext();
				using Font font = MakeFont(fontSize, bold);
				return _measureGraphics.MeasureString(text, font, int.MaxValue, StringFormat.GenericTypographic).Width;
			}
		}

		private void EnsureMeasureContext()
		{
			if (_measureGraphics == null)
			{
				_measureBmp = new Bitmap(1, 1);
				_measureGraphics = Graphics.FromImage(_measureBmp);
				_measureGraphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
			}
		}

		public List<string> WrapText(string text, float fontSize, int maxWidth, bool bold = false)
		{
			List<string> lines = new List<string>();
			if (string.IsNullOrEmpty(text))
			{
				return lines;
			}
			string[] array = text.Replace("\r", "").Split('\n');
			foreach (string paragraph in array)
			{
				if (paragraph.Length == 0)
				{
					lines.Add("");
					continue;
				}
				string[] array2 = paragraph.Split(' ');
				string current = "";
				string[] array3 = array2;
				foreach (string word in array3)
				{
					string candidate = ((current.Length == 0) ? word : (current + " " + word));
					if (MeasureWidth(candidate, fontSize, bold) <= (float)maxWidth || current.Length == 0)
					{
						current = candidate;
						continue;
					}
					lines.Add(current);
					current = word;
				}
				if (current.Length > 0)
				{
					lines.Add(current);
				}
			}
			return lines;
		}

		public float LineHeight(float fontSize, bool bold = false)
		{
			long key = ((long)(fontSize * 10f) << 1) | (bold ? 1 : 0);
			if (_lineHeightCache.TryGetValue(key, out var h))
			{
				return h;
			}
			using (Font font = MakeFont(fontSize, bold))
			{
				h = font.GetHeight();
			}
			_lineHeightCache[key] = h;
			return h;
		}

		private Texture2D Render(string text, float fontSize, Color color, bool bold)
		{
			using Font font = MakeFont(fontSize, bold);
			int w;
			int h;
			using (Bitmap measureBmp = new Bitmap(1, 1))
			{
				using Graphics mg = Graphics.FromImage(measureBmp);
				SizeF size = mg.MeasureString(text, font, int.MaxValue, StringFormat.GenericTypographic);
				w = Math.Max(1, (int)Math.Ceiling(size.Width) + 4);
				h = Math.Max(1, (int)Math.Ceiling(size.Height) + 4);
			}
			using Bitmap bmp = new Bitmap(w, h, PixelFormat.Format32bppArgb);
			using Graphics g = Graphics.FromImage(bmp);
			g.SmoothingMode = SmoothingMode.AntiAlias;
			g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
			g.Clear(Color.Transparent);
			SolidBrush brush = new SolidBrush(Color.FromArgb(((Color)(ref color)).get_A(), ((Color)(ref color)).get_R(), ((Color)(ref color)).get_G(), ((Color)(ref color)).get_B()));
			g.DrawString(text, font, brush, 2f, 2f, StringFormat.GenericTypographic);
			brush.Dispose();
			return BitmapToTexture(bmp);
		}

		private Texture2D BitmapToTexture(Bitmap bmp)
		{
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Expected O, but got Unknown
			BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
			try
			{
				int byteCount = data.Stride * bmp.Height;
				byte[] bytes = new byte[byteCount];
				Marshal.Copy(data.Scan0, bytes, 0, byteCount);
				for (int i = 0; i < byteCount; i += 4)
				{
					byte b = bytes[i];
					byte gg = bytes[i + 1];
					byte r = bytes[i + 2];
					byte a = bytes[i + 3];
					bytes[i] = (byte)(r * a / 255);
					bytes[i + 1] = (byte)(gg * a / 255);
					bytes[i + 2] = (byte)(b * a / 255);
					bytes[i + 3] = a;
				}
				Texture2D val = new Texture2D(_graphicsDevice, bmp.Width, bmp.Height, false, (SurfaceFormat)0);
				val.SetData<byte>(bytes);
				return val;
			}
			finally
			{
				bmp.UnlockBits(data);
			}
		}

		private static Font MakeFont(float size, bool bold)
		{
			return new Font(ResolvedFamily, size, bold ? FontStyle.Bold : FontStyle.Regular, GraphicsUnit.Pixel);
		}

		private static string ResolveFontFamily()
		{
			try
			{
				using InstalledFontCollection installed = new InstalledFontCollection();
				HashSet<string> names = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
				FontFamily[] families = installed.Families;
				foreach (FontFamily fam in families)
				{
					names.Add(fam.Name);
				}
				string[] fontCandidates = FontCandidates;
				foreach (string candidate in fontCandidates)
				{
					if (names.Contains(candidate))
					{
						return candidate;
					}
				}
			}
			catch
			{
			}
			return "Arial";
		}

		public void Dispose()
		{
			foreach (Texture2D value in _cache.Values)
			{
				((GraphicsResource)value).Dispose();
			}
			_cache.Clear();
			_cacheOrder.Clear();
			lock (_measureLock)
			{
				_measureGraphics?.Dispose();
				_measureBmp?.Dispose();
				_measureGraphics = null;
				_measureBmp = null;
			}
		}
	}
}
