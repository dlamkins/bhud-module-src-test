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

		private sealed class FullscreenButton : Control
		{
			public bool Collapse;

			public Texture2D ExpandIcon;

			public Texture2D CollapseIcon;

			private bool _hover;

			protected override CaptureType CapturesInput()
			{
				return (CaptureType)4;
			}

			protected override void OnMouseEntered(MouseEventArgs e)
			{
				_hover = true;
				((Control)this).OnMouseEntered(e);
			}

			protected override void OnMouseLeft(MouseEventArgs e)
			{
				_hover = false;
				((Control)this).OnMouseLeft(e);
			}

			protected override void Paint(SpriteBatch sb, Rectangle b)
			{
				//IL_001d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0030: Unknown result type (might be due to invalid IL or missing references)
				//IL_003a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0041: Unknown result type (might be due to invalid IL or missing references)
				//IL_0055: Unknown result type (might be due to invalid IL or missing references)
				//IL_006a: Unknown result type (might be due to invalid IL or missing references)
				//IL_007d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0096: Unknown result type (might be due to invalid IL or missing references)
				//IL_009b: Unknown result type (might be due to invalid IL or missing references)
				//IL_009f: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
				//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
				//IL_00db: Unknown result type (might be due to invalid IL or missing references)
				//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
				//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
				//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
				//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
				//IL_010e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0115: Unknown result type (might be due to invalid IL or missing references)
				//IL_011b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0120: Unknown result type (might be due to invalid IL or missing references)
				//IL_0135: Unknown result type (might be due to invalid IL or missing references)
				//IL_014e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0153: Unknown result type (might be due to invalid IL or missing references)
				//IL_015f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0174: Unknown result type (might be due to invalid IL or missing references)
				//IL_0193: Unknown result type (might be due to invalid IL or missing references)
				//IL_0198: Unknown result type (might be due to invalid IL or missing references)
				//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
				//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
				//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
				//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
				//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
				//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
				//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
				//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
				//IL_0206: Unknown result type (might be due to invalid IL or missing references)
				//IL_020b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0220: Unknown result type (might be due to invalid IL or missing references)
				//IL_0225: Unknown result type (might be due to invalid IL or missing references)
				//IL_023a: Unknown result type (might be due to invalid IL or missing references)
				//IL_023f: Unknown result type (might be due to invalid IL or missing references)
				//IL_024a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0250: Unknown result type (might be due to invalid IL or missing references)
				//IL_025e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0264: Unknown result type (might be due to invalid IL or missing references)
				//IL_027c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0281: Unknown result type (might be due to invalid IL or missing references)
				//IL_0296: Unknown result type (might be due to invalid IL or missing references)
				//IL_029b: Unknown result type (might be due to invalid IL or missing references)
				//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
				//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
				//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
				//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
				Texture2D icon = (Collapse ? CollapseIcon : ExpandIcon);
				if (icon != null)
				{
					SpriteBatchExtensions.DrawOnCtrl(sb, (Control)(object)this, icon, b, (Rectangle?)null, _hover ? Color.get_White() : (Color.get_White() * 0.85f));
					return;
				}
				Texture2D px = Textures.get_Pixel();
				SpriteBatchExtensions.DrawOnCtrl(sb, (Control)(object)this, px, b, new Color(0, 0, 0, _hover ? 165 : 120));
				Color edge = new Color(150, 122, 60) * (_hover ? 1f : 0.85f);
				SpriteBatchExtensions.DrawOnCtrl(sb, (Control)(object)this, px, new Rectangle(b.X, b.Y, b.Width, 1), edge);
				SpriteBatchExtensions.DrawOnCtrl(sb, (Control)(object)this, px, new Rectangle(b.X, ((Rectangle)(ref b)).get_Bottom() - 1, b.Width, 1), edge);
				SpriteBatchExtensions.DrawOnCtrl(sb, (Control)(object)this, px, new Rectangle(b.X, b.Y, 1, b.Height), edge);
				SpriteBatchExtensions.DrawOnCtrl(sb, (Control)(object)this, px, new Rectangle(((Rectangle)(ref b)).get_Right() - 1, b.Y, 1, b.Height), edge);
				Color c = new Color(245, 233, 202) * (_hover ? 1f : 0.9f);
				if (!Collapse)
				{
					int L = b.X + 6;
					int R = ((Rectangle)(ref b)).get_Right() - 6;
					int T = b.Y + 6;
					int B2 = ((Rectangle)(ref b)).get_Bottom() - 6;
					SpriteBatchExtensions.DrawOnCtrl(sb, (Control)(object)this, px, new Rectangle(L, T, 9, 2), c);
					SpriteBatchExtensions.DrawOnCtrl(sb, (Control)(object)this, px, new Rectangle(L, T, 2, 9), c);
					SpriteBatchExtensions.DrawOnCtrl(sb, (Control)(object)this, px, new Rectangle(R - 9, T, 9, 2), c);
					SpriteBatchExtensions.DrawOnCtrl(sb, (Control)(object)this, px, new Rectangle(R - 2, T, 2, 9), c);
					SpriteBatchExtensions.DrawOnCtrl(sb, (Control)(object)this, px, new Rectangle(L, B2 - 2, 9, 2), c);
					SpriteBatchExtensions.DrawOnCtrl(sb, (Control)(object)this, px, new Rectangle(L, B2 - 9, 2, 9), c);
					SpriteBatchExtensions.DrawOnCtrl(sb, (Control)(object)this, px, new Rectangle(R - 9, B2 - 2, 9, 2), c);
					SpriteBatchExtensions.DrawOnCtrl(sb, (Control)(object)this, px, new Rectangle(R - 2, B2 - 9, 2, 9), c);
				}
				else
				{
					int s2 = 14;
					int x = b.X + (b.Width - s2) / 2;
					int y = b.Y + (b.Height - s2) / 2;
					SpriteBatchExtensions.DrawOnCtrl(sb, (Control)(object)this, px, new Rectangle(x, y, s2, 2), c);
					SpriteBatchExtensions.DrawOnCtrl(sb, (Control)(object)this, px, new Rectangle(x, y + s2 - 2, s2, 2), c);
					SpriteBatchExtensions.DrawOnCtrl(sb, (Control)(object)this, px, new Rectangle(x, y, 2, s2), c);
					SpriteBatchExtensions.DrawOnCtrl(sb, (Control)(object)this, px, new Rectangle(x + s2 - 2, y, 2, s2), c);
				}
			}

			public FullscreenButton()
				: this()
			{
			}
		}

		private sealed class ArrowButton : Control
		{
			private readonly Texture2D _tex;

			private bool _hover;

			public ArrowButton(Texture2D tex)
				: this()
			{
				_tex = tex;
			}

			protected override CaptureType CapturesInput()
			{
				return (CaptureType)4;
			}

			protected override void OnMouseEntered(MouseEventArgs e)
			{
				_hover = true;
				((Control)this).OnMouseEntered(e);
			}

			protected override void OnMouseLeft(MouseEventArgs e)
			{
				_hover = false;
				((Control)this).OnMouseLeft(e);
			}

			protected override void Paint(SpriteBatch sb, Rectangle bounds)
			{
				//IL_0008: Unknown result type (might be due to invalid IL or missing references)
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0024: Unknown result type (might be due to invalid IL or missing references)
				//IL_002b: Unknown result type (might be due to invalid IL or missing references)
				SpriteBatchExtensions.DrawOnCtrl(sb, (Control)(object)this, _tex, bounds, (Rectangle?)null, _hover ? Color.get_White() : (Color.get_White() * 0.82f));
			}
		}

		private static readonly Logger Logger = Logger.GetLogger<BookReaderPanel>();

		private static readonly Color InkColor = new Color(48, 36, 20);

		private static readonly Color HeadInk = new Color(36, 26, 12);

		private static readonly Color FaintInk = new Color(122, 104, 70);

		private static readonly Color StampInk = new Color(126, 54, 34) * 0.85f;

		private const int PadX = 46;

		private const int PadY = 20;

		private const int FsBtnSize = 46;

		private const int FsBtnTop = 8;

		private const int PadTop = 64;

		private const double TurnMs = 420.0;

		private readonly TextRenderer _tr;

		private readonly Texture2D _parchment;

		private readonly Texture2D _ornament;

		private readonly Texture2D _seal;

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

		private readonly Control _prevBtn;

		private readonly Control _nextBtn;

		private readonly FullscreenButton _fsBtn;

		private bool _isFullscreen;

		public bool IsFullscreen
		{
			get
			{
				return _isFullscreen;
			}
			set
			{
				_isFullscreen = value;
				if (_fsBtn != null)
				{
					_fsBtn.Collapse = value;
					((Control)_fsBtn).set_BasicTooltipText(value ? "Exit full-window reading" : "Read across the whole window");
				}
			}
		}

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

		public event EventHandler FullscreenToggled;

		public BookReaderPanel(TextRenderer tr, Texture2D parchment, Texture2D arrowLeft = null, Texture2D arrowRight = null, Texture2D ornament = null, Texture2D seal = null, Texture2D expandIcon = null, Texture2D collapseIcon = null)
			: this()
		{
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			_tr = tr;
			_parchment = parchment;
			_ornament = ornament;
			_seal = seal;
			_prevBtn = MakeArrow(arrowLeft, "‹");
			_nextBtn = MakeArrow(arrowRight, "›");
			_prevBtn.add_Click((EventHandler<MouseEventArgs>)delegate
			{
				Turn(-1);
			});
			_nextBtn.add_Click((EventHandler<MouseEventArgs>)delegate
			{
				Turn(1);
			});
			FullscreenButton fullscreenButton = new FullscreenButton();
			((Control)fullscreenButton).set_Parent((Container)(object)this);
			((Control)fullscreenButton).set_Visible(false);
			((Control)fullscreenButton).set_Size(new Point(46, 46));
			fullscreenButton.ExpandIcon = expandIcon;
			fullscreenButton.CollapseIcon = collapseIcon;
			((Control)fullscreenButton).set_BasicTooltipText("Read across the whole window");
			_fsBtn = fullscreenButton;
			((Control)_fsBtn).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				this.FullscreenToggled?.Invoke(this, EventArgs.Empty);
			});
		}

		private Control MakeArrow(Texture2D tex, string fallback)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Expected O, but got Unknown
			if (tex != null)
			{
				ArrowButton arrowButton = new ArrowButton(tex);
				((Control)arrowButton).set_Parent((Container)(object)this);
				((Control)arrowButton).set_Size(new Point(36, 36));
				((Control)arrowButton).set_Visible(false);
				return (Control)(object)arrowButton;
			}
			StandardButton val = new StandardButton();
			((Control)val).set_Parent((Container)(object)this);
			val.set_Text(fallback);
			((Control)val).set_Width(30);
			((Control)val).set_Visible(false);
			return (Control)val;
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
			if (_entry == null || w <= 112 || h <= 104)
			{
				return;
			}
			int wrapW = w - 92;
			float lineH = _tr.LineHeight(_fontSize);
			float headH = _tr.LineHeight(_fontSize + 2f, bold: true);
			float gapH = lineH * 0.55f;
			float availH = h - 64 - 20 - 18;
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
				List<string> lines = new List<string>();
				string[] array2 = para.Split('\n');
				for (int j = 0; j < array2.Length; j++)
				{
					string ln2 = array2[j].Trim();
					if (ln2.Length != 0)
					{
						lines.AddRange(_tr.WrapText(ln2, head ? (_fontSize + 2f) : _fontSize, wrapW, head));
					}
				}
				bool first = true;
				foreach (string ln in lines)
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
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			if (_prevBtn != null && _nextBtn != null && _fsBtn != null)
			{
				_prevBtn.set_Visible(_entry != null && _page > 0);
				_nextBtn.set_Visible(_entry != null && _page < _pages.Count);
				((Control)_fsBtn).set_Visible(_entry != null);
				_prevBtn.set_Location(new Point(4, ((Control)this).get_Height() / 2 - _prevBtn.get_Height() / 2));
				_nextBtn.set_Location(new Point(((Control)this).get_Width() - _nextBtn.get_Width() - 4, ((Control)this).get_Height() / 2 - _nextBtn.get_Height() / 2));
				((Control)_fsBtn).set_Location(new Point((((Control)this).get_Width() - ((Control)_fsBtn).get_Width()) / 2, 8));
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
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_013c: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_015c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Unknown result type (might be due to invalid IL or missing references)
			//IL_0175: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Unknown result type (might be due to invalid IL or missing references)
			//IL_018b: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0198: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0223: Unknown result type (might be due to invalid IL or missing references)
			//IL_0239: Unknown result type (might be due to invalid IL or missing references)
			//IL_026a: Unknown result type (might be due to invalid IL or missing references)
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
					double t3 = _turnT * 2.0;
					f = 1f - (float)(t3 * t3);
					anchorLeft = _turnDir > 0;
				}
				else
				{
					double t2 = (_turnT - 0.5) * 2.0;
					f = (float)(1.0 - (1.0 - t2) * (1.0 - t2));
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
			DrawEdge(sb, pageRect, 6, FaintInk * 0.08f);
			DrawEdge(sb, pageRect, 3, FaintInk * 0.1f);
			DrawEdge(sb, pageRect, 1, FaintInk * 0.3f);
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

		private void DrawEdge(SpriteBatch sb, Rectangle r, int t, Color c)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			Texture2D px = Textures.get_Pixel();
			SpriteBatchExtensions.DrawOnCtrl(sb, (Control)(object)this, px, new Rectangle(r.X, r.Y, r.Width, t), c);
			SpriteBatchExtensions.DrawOnCtrl(sb, (Control)(object)this, px, new Rectangle(r.X, ((Rectangle)(ref r)).get_Bottom() - t, r.Width, t), c);
			SpriteBatchExtensions.DrawOnCtrl(sb, (Control)(object)this, px, new Rectangle(r.X, r.Y, t, r.Height), c);
			SpriteBatchExtensions.DrawOnCtrl(sb, (Control)(object)this, px, new Rectangle(((Rectangle)(ref r)).get_Right() - t, r.Y, t, r.Height), c);
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
			float y = bounds.Y + 64;
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
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0208: Unknown result type (might be due to invalid IL or missing references)
			//IL_0227: Unknown result type (might be due to invalid IL or missing references)
			//IL_022d: Unknown result type (might be due to invalid IL or missing references)
			//IL_026c: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0321: Unknown result type (might be due to invalid IL or missing references)
			//IL_0327: Unknown result type (might be due to invalid IL or missing references)
			//IL_0372: Unknown result type (might be due to invalid IL or missing references)
			//IL_0378: Unknown result type (might be due to invalid IL or missing references)
			//IL_038a: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_040e: Unknown result type (might be due to invalid IL or missing references)
			//IL_04af: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_053d: Unknown result type (might be due to invalid IL or missing references)
			int fullW = bounds.Width;
			int os;
			if (_ornament != null)
			{
				os = Math.Min(56, fullW / 6);
				Corner(bounds.X + 8, bounds.Y + 8, (SpriteEffects)0);
				Corner(((Rectangle)(ref bounds)).get_Right() - 8 - os, bounds.Y + 8, (SpriteEffects)1);
				Corner(bounds.X + 8, ((Rectangle)(ref bounds)).get_Bottom() - 8 - os, (SpriteEffects)2);
				Corner(((Rectangle)(ref bounds)).get_Right() - 8 - os, ((Rectangle)(ref bounds)).get_Bottom() - 8 - os, (SpriteEffects)3);
			}
			string title = _entry.DisplayTitle ?? "";
			float titleSize = Math.Max(20f, _fontSize * 1.7f);
			List<string> list = _tr.WrapText(title, titleSize, (int)((float)fullW * 0.72f), bold: true, serif: true);
			float th = _tr.LineHeight(titleSize, bold: true, serif: true);
			float blockH = (float)list.Count * th + 34f + (float)((!HasExpansion()) ? ((_seal != null) ? 130 : 0) : ((_xpIcon != null) ? 160 : 92));
			float y = (float)bounds.Y + Math.Max(70f, ((float)bounds.Height - blockH) * 0.34f);
			foreach (string ln in list)
			{
				Texture2D tex = _tr.RenderLine(ln, titleSize, HeadInk, bold: true, serif: true);
				if (tex != null)
				{
					float cx4 = (float)bounds.X + (float)(fullW - tex.get_Width()) / 2f;
					int dx2 = SqueezeX(bounds, page, f, anchorLeft, cx4);
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
			else if (_seal != null)
			{
				int ss = Math.Min((int)((float)fullW * 0.28f), 104);
				int cx3 = SqueezeX(bounds, page, f, anchorLeft, (float)bounds.X + (float)fullW / 2f);
				SpriteBatchExtensions.DrawOnCtrl(sb, (Control)(object)this, _seal, new Rectangle(cx3, (int)y + 56, (int)((float)ss * f), ss), (Rectangle?)null, Color.get_White() * 0.95f, -0.08f, new Vector2((float)_seal.get_Width() / 2f, (float)_seal.get_Height() / 2f), (SpriteEffects)0);
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
					float cx2 = (float)bounds.X + (float)(fullW - mt.get_Width()) / 2f;
					int dx = SqueezeX(bounds, page, f, anchorLeft, cx2);
					SpriteBatchExtensions.DrawOnCtrl(sb, (Control)(object)this, mt, new Rectangle(dx, ((Rectangle)(ref bounds)).get_Bottom() - mt.get_Height() - 14, (int)((float)mt.get_Width() * f), mt.get_Height()));
				}
			}
			void Corner(float cx, int cy2, SpriteEffects fx)
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0009: Unknown result type (might be due to invalid IL or missing references)
				//IL_004b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0059: Unknown result type (might be due to invalid IL or missing references)
				//IL_0063: Unknown result type (might be due to invalid IL or missing references)
				//IL_006d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0072: Unknown result type (might be due to invalid IL or missing references)
				int dx3 = SqueezeX(bounds, page, f, anchorLeft, cx);
				SpriteBatchExtensions.DrawOnCtrl(sb, (Control)(object)this, _ornament, new Rectangle(dx3, cy2, (int)((float)os * f), os), (Rectangle?)null, Color.get_White() * 0.9f, 0f, Vector2.get_Zero(), fx);
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
