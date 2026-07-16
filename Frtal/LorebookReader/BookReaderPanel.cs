using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Frtal.LorebookReader
{
	public sealed class BookReaderPanel : Container
	{
		private struct Line
		{
			public string Text;

			public bool Head;

			public bool Gap;
		}

		private static readonly Logger Logger = Logger.GetLogger<BookReaderPanel>();

		private static readonly Color InkColor = new Color(48, 36, 20);

		private static readonly Color HeadInk = new Color(36, 26, 12);

		private static readonly Color FaintInk = new Color(122, 104, 70);

		private static readonly Color StampInk = new Color(126, 54, 34) * 0.85f;

		private const int PadX = 46;

		private const int PadY = 20;

		private const double TurnMs = 420.0;

		private readonly TextRenderer _tr;

		private readonly Texture2D _parchment;

		private LorebookEntry _entry;

		private string _body = "";

		private Texture2D _xpIcon;

		private float _fontSize = 18f;

		private readonly List<List<Line>> _pages = new List<List<Line>>();

		private int _page;

		private int _lastLayoutW = -1;

		private int _lastLayoutH = -1;

		private bool _turning;

		private double _turnT;

		private int _turnDir;

		private int _pendingPage;

		private readonly StandardButton _prevBtn;

		private readonly StandardButton _nextBtn;

		public float FontSize
		{
			get
			{
				return _fontSize;
			}
			set
			{
				if (!(Math.Abs(_fontSize - value) < 0.1f))
				{
					_fontSize = value;
					Repaginate();
				}
			}
		}

		public BookReaderPanel(TextRenderer tr, Texture2D parchment)
			: this()
		{
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Expected O, but got Unknown
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Expected O, but got Unknown
			_tr = tr;
			_parchment = parchment;
			StandardButton val = new StandardButton();
			((Control)val).set_Parent((Container)(object)this);
			val.set_Text("‹");
			((Control)val).set_Width(30);
			((Control)val).set_Visible(false);
			_prevBtn = val;
			StandardButton val2 = new StandardButton();
			((Control)val2).set_Parent((Container)(object)this);
			val2.set_Text("›");
			((Control)val2).set_Width(30);
			((Control)val2).set_Visible(false);
			_nextBtn = val2;
			((Control)_prevBtn).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				Turn(-1);
			});
			((Control)_nextBtn).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				Turn(1);
			});
		}

		public void SetEntry(LorebookEntry entry, string body, Texture2D xpIcon)
		{
			_entry = entry;
			_body = body ?? "";
			_xpIcon = xpIcon;
			_page = 0;
			_turning = false;
			Repaginate();
		}

		private void Repaginate()
		{
			try
			{
				RepaginateCore();
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Book pagination failed.");
				_pages.Clear();
				_page = 0;
			}
		}

		private void RepaginateCore()
		{
			_pages.Clear();
			int w = ((Control)this).get_Width();
			int h = ((Control)this).get_Height();
			_lastLayoutW = w;
			_lastLayoutH = h;
			if (_entry == null || w <= 112 || h <= 60)
			{
				return;
			}
			int wrapW = w - 92;
			float lineH = _tr.LineHeight(_fontSize);
			float headH = _tr.LineHeight(_fontSize + 2f, bold: true);
			float gapH = lineH * 0.55f;
			float availH = h - 40 - 18;
			List<Line> cur = new List<Line>();
			float used = 0f;
			string[] array = _body.Replace("\r", "").Split(new string[1] { "\n\n" }, StringSplitOptions.None);
			for (int i = 0; i < array.Length; i++)
			{
				string para = array[i].Trim();
				if (para.Length == 0)
				{
					continue;
				}
				bool head = IsHeading(para);
				List<string> list = _tr.WrapText(para.Replace('\n', ' '), head ? (_fontSize + 2f) : _fontSize, wrapW, head);
				bool first = true;
				foreach (string ln in list)
				{
					float lh = (head ? headH : lineH);
					float need = lh + ((first && cur.Count > 0) ? gapH : 0f);
					if (used + need > availH && cur.Count > 0)
					{
						_pages.Add(cur);
						cur = new List<Line>();
						used = 0f;
						need = lh;
						first = false;
					}
					cur.Add(new Line
					{
						Text = ln,
						Head = head,
						Gap = (first && cur.Count > 0)
					});
					used += need;
					first = false;
				}
			}
			if (cur.Count > 0)
			{
				_pages.Add(cur);
			}
			if (_page > _pages.Count)
			{
				_page = _pages.Count;
			}
			UpdateButtons();
			((Control)this).Invalidate();
		}

		private static bool IsHeading(string para)
		{
			if (para.Length > 48 || para.Contains("\n"))
			{
				return false;
			}
			char last = para[para.Length - 1];
			if (last != '.' && last != '!' && last != '?' && last != ':' && last != '"')
			{
				return last != '\'';
			}
			return false;
		}

		private void Turn(int dir)
		{
			int np = _page + dir;
			if (!_turning && np >= 0 && np <= _pages.Count)
			{
				_turning = true;
				_turnT = 0.0;
				_turnDir = dir;
				_pendingPage = np;
				UpdateButtons();
			}
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			if (((Control)this).get_Width() != _lastLayoutW || ((Control)this).get_Height() != _lastLayoutH)
			{
				Repaginate();
			}
			if (_turning)
			{
				_turnT += gameTime.get_ElapsedGameTime().TotalMilliseconds / 420.0;
				if (_turnT >= 0.5 && _page != _pendingPage)
				{
					_page = _pendingPage;
				}
				if (_turnT >= 1.0)
				{
					_turning = false;
					_turnT = 0.0;
					UpdateButtons();
				}
			}
		}

		private void UpdateButtons()
		{
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			if (_prevBtn != null && _nextBtn != null)
			{
				((Control)_prevBtn).set_Visible(_entry != null && _page > 0);
				((Control)_nextBtn).set_Visible(_entry != null && _page < _pages.Count);
				((Control)_prevBtn).set_Location(new Point(4, ((Control)this).get_Height() / 2 - 13));
				((Control)_nextBtn).set_Location(new Point(((Control)this).get_Width() - 34, ((Control)this).get_Height() / 2 - 13));
			}
		}

		public override void RecalculateLayout()
		{
			((Control)this).RecalculateLayout();
			UpdateButtons();
		}

		public override void PaintBeforeChildren(SpriteBatch sb, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				PaintCore(sb, bounds);
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Book reader paint failed.");
			}
		}

		private void PaintCore(SpriteBatch sb, Rectangle bounds)
		{
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
			if (_entry == null)
			{
				return;
			}
			float f = 1f;
			bool anchorLeft = true;
			if (_turning)
			{
				if (_turnT < 0.5)
				{
					f = 1f - (float)(_turnT * 2.0);
					anchorLeft = _turnDir > 0;
				}
				else
				{
					f = (float)((_turnT - 0.5) * 2.0);
					anchorLeft = _turnDir < 0;
				}
				f = Math.Max(0.02f, Math.Min(1f, f));
			}
			int fullW = bounds.Width;
			int pageW = (int)((float)fullW * f);
			int pageX = bounds.X + ((!anchorLeft) ? (fullW - pageW) : 0);
			Rectangle pageRect = default(Rectangle);
			((Rectangle)(ref pageRect))._002Ector(pageX, bounds.Y, pageW, bounds.Height);
			if (_turning)
			{
				SpriteBatchExtensions.DrawOnCtrl(sb, (Control)(object)this, Textures.get_Pixel(), bounds, new Color(0, 0, 0, 70));
			}
			if (_parchment != null)
			{
				SpriteBatchExtensions.DrawOnCtrl(sb, (Control)(object)this, _parchment, pageRect, (Rectangle?)null, Color.get_White());
			}
			else
			{
				SpriteBatchExtensions.DrawOnCtrl(sb, (Control)(object)this, Textures.get_Pixel(), pageRect, new Color(231, 217, 182));
			}
			if (_page == 0)
			{
				PaintCover(sb, bounds, pageRect, f, anchorLeft);
			}
			else
			{
				PaintPage(sb, bounds, pageRect, f, anchorLeft);
			}
			if (!_turning && _page > 0 && _pages.Count > 0)
			{
				Texture2D t = _tr.RenderLine($"{_page} / {_pages.Count}", _fontSize * 0.68f, FaintInk);
				if (t != null)
				{
					SpriteBatchExtensions.DrawOnCtrl(sb, (Control)(object)this, t, new Rectangle(bounds.X + (fullW - t.get_Width()) / 2, ((Rectangle)(ref bounds)).get_Bottom() - t.get_Height() - 4, t.get_Width(), t.get_Height()));
				}
			}
		}

		private static int SqueezeX(Rectangle bounds, Rectangle page, float f, bool anchorLeft, float x)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			if (!anchorLeft)
			{
				return ((Rectangle)(ref page)).get_Right() - (int)(((float)((Rectangle)(ref bounds)).get_Right() - x) * f);
			}
			return page.X + (int)((x - (float)bounds.X) * f);
		}

		private void PaintPage(SpriteBatch sb, Rectangle bounds, Rectangle page, float f, bool anchorLeft)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			List<Line> list = _pages[_page - 1];
			float y = bounds.Y + 20;
			foreach (Line ln in list)
			{
				float lh = (ln.Head ? _tr.LineHeight(_fontSize + 2f, bold: true) : _tr.LineHeight(_fontSize));
				if (ln.Gap)
				{
					y += _tr.LineHeight(_fontSize) * 0.55f;
				}
				if (ln.Text.Length > 0)
				{
					Texture2D tex = (ln.Head ? _tr.RenderLine(ln.Text, _fontSize + 2f, HeadInk, bold: true) : _tr.RenderLine(ln.Text, _fontSize, InkColor));
					if (tex != null)
					{
						int dx = SqueezeX(bounds, page, f, anchorLeft, bounds.X + 46);
						SpriteBatchExtensions.DrawOnCtrl(sb, (Control)(object)this, tex, new Rectangle(dx, (int)y, (int)((float)tex.get_Width() * f), tex.get_Height()));
					}
				}
				y += lh;
			}
		}

		private void PaintCover(SpriteBatch sb, Rectangle bounds, Rectangle page, float f, bool anchorLeft)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_0158: Unknown result type (might be due to invalid IL or missing references)
			//IL_0177: Unknown result type (might be due to invalid IL or missing references)
			//IL_0178: Unknown result type (might be due to invalid IL or missing references)
			//IL_019f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0252: Unknown result type (might be due to invalid IL or missing references)
			//IL_0264: Unknown result type (might be due to invalid IL or missing references)
			//IL_027e: Unknown result type (might be due to invalid IL or missing references)
			//IL_027f: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
			int fullW = bounds.Width;
			string title = _entry.DisplayTitle ?? "";
			float titleSize = Math.Max(20f, _fontSize * 1.7f);
			List<string> list = _tr.WrapText(title, titleSize, (int)((float)fullW * 0.72f), bold: true, serif: true);
			float th = _tr.LineHeight(titleSize, bold: true, serif: true);
			float blockH = (float)list.Count * th + 34f + (float)(HasExpansion() ? ((_xpIcon != null) ? 160 : 92) : 0);
			float y = (float)bounds.Y + Math.Max(26f, ((float)bounds.Height - blockH) * 0.34f);
			foreach (string ln in list)
			{
				Texture2D tex = _tr.RenderLine(ln, titleSize, HeadInk, bold: true, serif: true);
				if (tex != null)
				{
					float cx2 = (float)bounds.X + (float)(fullW - tex.get_Width()) / 2f;
					int dx2 = SqueezeX(bounds, page, f, anchorLeft, cx2);
					SpriteBatchExtensions.DrawOnCtrl(sb, (Control)(object)this, tex, new Rectangle(dx2, (int)y, (int)((float)tex.get_Width() * f), tex.get_Height()));
				}
				y += th;
			}
			y += 10f;
			DrawRule(sb, bounds, page, f, anchorLeft, (int)y, 0.52f, 2);
			y += 7f;
			DrawRule(sb, bounds, page, f, anchorLeft, (int)y, 0.3f, 1);
			y += 26f;
			if (HasExpansion())
			{
				PaintStamp(sb, bounds, page, f, anchorLeft, (int)y + ((_xpIcon != null) ? 62 : 26));
			}
			string meta = ((_entry.TimestampLocal == DateTime.MinValue) ? "" : _entry.TimestampLocal.ToString("d MMMM yyyy"));
			if (!string.IsNullOrWhiteSpace(_entry.Location))
			{
				meta = meta + ((meta.Length > 0) ? "  ·  " : "") + _entry.Location;
			}
			if (meta.Length > 0)
			{
				Texture2D mt = _tr.RenderLine(meta, _fontSize * 0.72f, FaintInk, bold: false, serif: true);
				if (mt != null)
				{
					float cx = (float)bounds.X + (float)(fullW - mt.get_Width()) / 2f;
					int dx = SqueezeX(bounds, page, f, anchorLeft, cx);
					SpriteBatchExtensions.DrawOnCtrl(sb, (Control)(object)this, mt, new Rectangle(dx, ((Rectangle)(ref bounds)).get_Bottom() - mt.get_Height() - 14, (int)((float)mt.get_Width() * f), mt.get_Height()));
				}
			}
		}

		private bool HasExpansion()
		{
			return !string.IsNullOrWhiteSpace(_entry?.Expansion);
		}

		private void DrawRule(SpriteBatch sb, Rectangle bounds, Rectangle page, float f, bool anchorLeft, int y, float widthFrac, int thick)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			int w = (int)((float)bounds.Width * widthFrac);
			float cx = (float)bounds.X + (float)(bounds.Width - w) / 2f;
			int dx = SqueezeX(bounds, page, f, anchorLeft, cx);
			SpriteBatchExtensions.DrawOnCtrl(sb, (Control)(object)this, Textures.get_Pixel(), new Rectangle(dx, y, (int)((float)w * f), thick), FaintInk * 0.9f);
		}

		private void PaintStamp(SpriteBatch sb, Rectangle bounds, Rectangle page, float f, bool anchorLeft, int centerY)
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_0179: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_0208: Unknown result type (might be due to invalid IL or missing references)
			//IL_0210: Unknown result type (might be due to invalid IL or missing references)
			//IL_022d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0235: Unknown result type (might be due to invalid IL or missing references)
			//IL_0244: Unknown result type (might be due to invalid IL or missing references)
			//IL_0257: Unknown result type (might be due to invalid IL or missing references)
			float cxF = (float)bounds.X + (float)bounds.Width / 2f;
			Vector2 center = new Vector2((float)SqueezeX(bounds, page, f, anchorLeft, cxF), (float)centerY);
			if (_xpIcon != null)
			{
				int maxS = Math.Min((int)((float)bounds.Width * 0.4f), 150);
				float scale = Math.Min((float)maxS / (float)_xpIcon.get_Width(), (float)maxS / (float)_xpIcon.get_Height());
				int w2 = (int)((float)_xpIcon.get_Width() * scale);
				int h2 = (int)((float)_xpIcon.get_Height() * scale);
				SpriteBatchExtensions.DrawOnCtrl(sb, (Control)(object)this, _xpIcon, new Rectangle((int)center.X, (int)center.Y, (int)((float)w2 * f), h2), (Rectangle?)null, Color.get_White() * 0.85f, -0.14f, new Vector2((float)_xpIcon.get_Width() / 2f, (float)_xpIcon.get_Height() / 2f), (SpriteEffects)0);
				return;
			}
			string name = _entry.Expansion.Trim();
			float nameSize = Math.Max(12f, _fontSize * 0.78f);
			Texture2D nameTex = _tr.RenderLine(name.ToUpperInvariant(), nameSize, StampInk, bold: true, serif: true);
			if (nameTex != null)
			{
				int stampW = nameTex.get_Width() + 30;
				int stampH = nameTex.get_Height() + 18;
				Texture2D px = Textures.get_Pixel();
				Rot(px, new Vector2(0f, (float)(-stampH) / 2f), stampW, 2, StampInk);
				Rot(px, new Vector2(0f, (float)stampH / 2f), stampW, 2, StampInk);
				Rot(px, new Vector2((float)(-stampW) / 2f, 0f), 2, stampH, StampInk);
				Rot(px, new Vector2((float)stampW / 2f, 0f), 2, stampH, StampInk);
				Rot(nameTex, Vector2.get_Zero(), nameTex.get_Width(), nameTex.get_Height(), Color.get_White());
			}
			void Rot(Texture2D tex, Vector2 offset, int w, int h, Color c)
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				//IL_0018: Unknown result type (might be due to invalid IL or missing references)
				//IL_0019: Unknown result type (might be due to invalid IL or missing references)
				//IL_001e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0028: Unknown result type (might be due to invalid IL or missing references)
				//IL_002f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0043: Unknown result type (might be due to invalid IL or missing references)
				//IL_0051: Unknown result type (might be due to invalid IL or missing references)
				//IL_0072: Unknown result type (might be due to invalid IL or missing references)
				Vector2 o = Vector2.Transform(offset, Matrix.CreateRotationZ(-0.14f));
				Vector2 pos = center + o;
				SpriteBatchExtensions.DrawOnCtrl(sb, (Control)(object)this, tex, new Rectangle((int)pos.X, (int)pos.Y, (int)((float)w * f), h), (Rectangle?)null, c, -0.14f, new Vector2((float)tex.get_Width() / 2f, (float)tex.get_Height() / 2f), (SpriteEffects)0);
			}
		}
	}
}
