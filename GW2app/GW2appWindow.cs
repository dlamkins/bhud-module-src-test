using System;
using System.ComponentModel;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace GW2app
{
	internal class GW2appWindow : WindowBase2
	{
		public enum WindowTheme
		{
			[Description("Game (default)")]
			Game,
			[Description("GW2.app")]
			GW2app,
			[Description("Black")]
			Black
		}

		private static readonly Logger Logger = Logger.GetLogger<GW2appWindow>();

		private const int GameTextureAssetId = 155997;

		public const int ContentTopPadding = 6;

		public const int ContentBottomMargin = 5;

		private static readonly Color BlackBg = new Color(0, 0, 0, 255);

		private static readonly Color DarkBg = new Color(28, 33, 43, 255);

		public const float MinBgOpacity = 0.75f;

		public const float MaxBgOpacity = 1f;

		public const float DefaultBgOpacity = 0.85f;

		private const float GameTextureBrightness = 0.6f;

		private WindowTheme _theme;

		private Texture2D _ownedBackground;

		private float _bgOpacity = 0.85f;

		private int _width;

		private int _height;

		private static Texture2D _transparentFill;

		private AsyncTexture2D _gameTextureSource;

		private EventHandler<ValueChangedEventArgs<Texture2D>> _gameTextureSwapHandler;

		private bool _compactTitle;

		private string _customTitle = "";

		private string _customSubtitle = "";

		public const int MinResizeHeight = 120;

		public const int MaxResizeHeight = 1200;

		private bool _userResized;

		private bool _recalculating;

		private static bool _srcOpaqueBoundsKnown;

		private static Rectangle _srcOpaqueBounds;

		private const byte OpaqueAlphaThreshold = 240;

		private Texture2D _ownedEmblem;

		private const int OverlayIconSize = 16;

		private const int OverlayIconSizeCompact = 14;

		private const int OverlayGapAfterIcon = 4;

		private const int OverlayRightMargin = 50;

		private const int OverlayIconY = 13;

		private Texture2D _rechargeIcon;

		private string _countdownText;

		public bool UserResized
		{
			get
			{
				return _userResized;
			}
			set
			{
				_userResized = value;
				if (!value)
				{
					UserPreferredHeight = null;
				}
			}
		}

		public int? UserPreferredHeight { get; private set; }

		public int MaxAllowedHeight { get; set; } = 1200;


		public string Title
		{
			get
			{
				if (!_compactTitle)
				{
					return ((WindowBase2)this).get_Title();
				}
				return _customTitle;
			}
			set
			{
				if (_compactTitle)
				{
					_customTitle = value ?? "";
				}
				else
				{
					((WindowBase2)this).set_Title(value);
				}
			}
		}

		public string Subtitle
		{
			get
			{
				if (!_compactTitle)
				{
					return ((WindowBase2)this).get_Subtitle();
				}
				return _customSubtitle;
			}
			set
			{
				if (_compactTitle)
				{
					_customSubtitle = value ?? "";
				}
				else
				{
					((WindowBase2)this).set_Subtitle(value);
				}
			}
		}

		public event EventHandler LayoutRefreshed;

		public GW2appWindow(int width, int height, WindowTheme theme = WindowTheme.Game, bool compactTitle = false, float bgOpacity = 0.85f)
			: this()
		{
			_theme = theme;
			_width = width;
			_height = height;
			_compactTitle = compactTitle;
			_bgOpacity = ClampOpacity(bgOpacity);
			if (_compactTitle)
			{
				((WindowBase2)this).set_Title("");
				((WindowBase2)this).set_Subtitle("");
			}
			((WindowBase2)this).set_CanCloseWithEscape(false);
			Recalculate();
		}

		public void SetWindowHeight(int newHeight)
		{
			if (newHeight != _height)
			{
				_height = newHeight;
				Recalculate();
			}
		}

		public void SetWindowSize(int newWidth, int newHeight)
		{
			if (newWidth != _width || newHeight != _height)
			{
				_width = newWidth;
				_height = newHeight;
				Recalculate();
			}
		}

		protected override Point HandleWindowResize(Point newSize)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			int max = Math.Max(120, Math.Min(1200, MaxAllowedHeight));
			int h = Math.Max(120, Math.Min(max, newSize.Y));
			return new Point(((Control)this).get_Size().X, h);
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			((WindowBase2)this).OnResized(e);
			_height = ((Control)this).get_Size().Y;
			if (!_recalculating)
			{
				_userResized = true;
				UserPreferredHeight = ((Control)this).get_Size().Y;
				Recalculate();
			}
		}

		public void SetCompactTitle(bool compact)
		{
			if (compact != _compactTitle)
			{
				if (compact)
				{
					_customTitle = ((WindowBase2)this).get_Title() ?? "";
					_customSubtitle = ((WindowBase2)this).get_Subtitle() ?? "";
					((WindowBase2)this).set_Title("");
					((WindowBase2)this).set_Subtitle("");
				}
				else
				{
					((WindowBase2)this).set_Title(_customTitle);
					((WindowBase2)this).set_Subtitle(_customSubtitle);
					_customTitle = "";
					_customSubtitle = "";
				}
				_compactTitle = compact;
			}
		}

		public void SetWindowTheme(WindowTheme theme)
		{
			if (theme != _theme)
			{
				_theme = theme;
				Recalculate();
			}
		}

		public void SetBackgroundOpacity(float opacity)
		{
			_bgOpacity = ClampOpacity(opacity);
		}

		private static float ClampOpacity(float o)
		{
			if (!(o < 0.75f))
			{
				if (!(o > 1f))
				{
					return o;
				}
				return 1f;
			}
			return 0.75f;
		}

		private void Recalculate()
		{
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			if (_recalculating)
			{
				return;
			}
			_recalculating = true;
			try
			{
				Texture2D newBg = CreateBackground(_width, _height);
				Texture2D oldBg = _ownedBackground;
				_ownedBackground = newBg;
				((WindowBase2)this).ConstructWindow(TransparentFill(), WindowRegionFor(_width, _height), ContentRegionFor(_width, _height), new Point(_width, _height));
				Rectangle val = ((WindowBase2)this).get_WindowRegion();
				int right = ((Rectangle)(ref val)).get_Right();
				val = ((WindowBase2)this).get_WindowRelativeContentRegion();
				int marginX = right - ((Rectangle)(ref val)).get_Right();
				val = ((WindowBase2)this).get_WindowRegion();
				int bottom = ((Rectangle)(ref val)).get_Bottom();
				val = ((WindowBase2)this).get_WindowRelativeContentRegion();
				int marginY = bottom - ((Rectangle)(ref val)).get_Bottom();
				((Container)this).set_ContentRegion(new Rectangle(((Container)this).get_ContentRegion().X, ((Container)this).get_ContentRegion().Y, ((Control)this).get_Width() - ((Container)this).get_ContentRegion().X - marginX, ((Control)this).get_Height() - ((Container)this).get_ContentRegion().Y - marginY));
				try
				{
					if (oldBg != null)
					{
						((GraphicsResource)oldBg).Dispose();
					}
				}
				catch
				{
				}
				this.LayoutRefreshed?.Invoke(this, EventArgs.Empty);
			}
			finally
			{
				_recalculating = false;
			}
		}

		private static Rectangle WindowRegionFor(int w, int h)
		{
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			return new Rectangle(0, 0, w, h);
		}

		private static Rectangle ContentRegionFor(int w, int h)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return new Rectangle(0, 6, w, h - 5);
		}

		private Texture2D CreateBackground(int w, int h)
		{
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			DetachGameTextureSubscription();
			if (_theme == WindowTheme.Game)
			{
				AsyncTexture2D src = AsyncTexture2D.FromAssetId(155997);
				if (src != null && src.get_HasSwapped())
				{
					try
					{
						return CreateClippedFrom(src.get_Texture(), w, h);
					}
					catch (Exception e2)
					{
						Logger.Warn(e2, "Clip failed.");
					}
				}
				if (src != null)
				{
					_gameTextureSource = src;
					_gameTextureSwapHandler = delegate
					{
						Recalculate();
					};
					src.add_TextureSwapped(_gameTextureSwapHandler);
				}
				return CreateSolidTexture(w, h, BlackBg);
			}
			return CreateSolidTexture(w, h, (_theme == WindowTheme.GW2app) ? DarkBg : BlackBg);
		}

		private void DetachGameTextureSubscription()
		{
			if (_gameTextureSource != null && _gameTextureSwapHandler != null)
			{
				try
				{
					_gameTextureSource.remove_TextureSwapped(_gameTextureSwapHandler);
				}
				catch
				{
				}
			}
			_gameTextureSource = null;
			_gameTextureSwapHandler = null;
		}

		private static Texture2D TransparentFill()
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			if (_transparentFill == null)
			{
				_transparentFill = CreateSolidTexture(1, 1, Color.get_Transparent());
			}
			return _transparentFill;
		}

		private static Texture2D CreateSolidTexture(int w, int h, Color c)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Expected O, but got Unknown
			Color[] pixels = (Color[])(object)new Color[w * h];
			for (int i = 0; i < pixels.Length; i++)
			{
				pixels[i] = c;
			}
			GraphicsDeviceContext gdc = GameService.Graphics.LendGraphicsDeviceContext();
			try
			{
				Texture2D val = new Texture2D(((GraphicsDeviceContext)(ref gdc)).get_GraphicsDevice(), w, h);
				val.SetData<Color>(pixels);
				return val;
			}
			finally
			{
				((GraphicsDeviceContext)(ref gdc)).Dispose();
			}
		}

		private static Texture2D CreateClippedFrom(Texture2D src, int w, int h)
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_016b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0170: Unknown result type (might be due to invalid IL or missing references)
			//IL_017b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0180: Unknown result type (might be due to invalid IL or missing references)
			//IL_018a: Expected O, but got Unknown
			int srcW = src.get_Width() - 25;
			int srcH = src.get_Height() - 26;
			if (srcW <= 0 || srcH <= 0)
			{
				return CreateSolidTexture(w, h, BlackBg);
			}
			Color[] srcPixels = (Color[])(object)new Color[srcW * srcH];
			src.GetData<Color>(0, (Rectangle?)new Rectangle(25, 26, srcW, srcH), srcPixels, 0, srcW * srcH);
			if (!_srcOpaqueBoundsKnown)
			{
				_srcOpaqueBounds = FindOpaqueBounds(srcPixels, srcW, srcH);
				_srcOpaqueBoundsKnown = true;
			}
			int opX = _srcOpaqueBounds.X;
			int opY = _srcOpaqueBounds.Y;
			int opW = _srcOpaqueBounds.Width;
			int opH = _srcOpaqueBounds.Height;
			Color[] dstPixels = (Color[])(object)new Color[w * h];
			for (int dy = 0; dy < h; dy++)
			{
				int sy = opY + (int)((long)dy * (long)opH / h);
				if (sy >= opY + opH)
				{
					sy = opY + opH - 1;
				}
				int srcRow = sy * srcW;
				int dstRow = dy * w;
				for (int dx = 0; dx < w; dx++)
				{
					int sx = opX + (int)((long)dx * (long)opW / w);
					if (sx >= opX + opW)
					{
						sx = opX + opW - 1;
					}
					Color c = srcPixels[srcRow + sx];
					dstPixels[dstRow + dx] = new Color((byte)((float)(int)((Color)(ref c)).get_R() * 0.6f), (byte)((float)(int)((Color)(ref c)).get_G() * 0.6f), (byte)((float)(int)((Color)(ref c)).get_B() * 0.6f), ((Color)(ref c)).get_A());
				}
			}
			GraphicsDeviceContext gdc = GameService.Graphics.LendGraphicsDeviceContext();
			try
			{
				Texture2D val = new Texture2D(((GraphicsDeviceContext)(ref gdc)).get_GraphicsDevice(), w, h);
				val.SetData<Color>(dstPixels);
				return val;
			}
			finally
			{
				((GraphicsDeviceContext)(ref gdc)).Dispose();
			}
		}

		private static Rectangle FindOpaqueBounds(Color[] pixels, int w, int h)
		{
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			int top = h;
			int bottom = -1;
			int left = w;
			int right = -1;
			for (int y = 0; y < h; y++)
			{
				int row = y * w;
				for (int x = 0; x < w; x++)
				{
					if (((Color)(ref pixels[row + x])).get_A() >= 240)
					{
						if (y < top)
						{
							top = y;
						}
						if (y > bottom)
						{
							bottom = y;
						}
						if (x < left)
						{
							left = x;
						}
						if (x > right)
						{
							right = x;
						}
					}
				}
			}
			if (bottom < top || right < left)
			{
				return new Rectangle(0, 0, w, h);
			}
			return new Rectangle(left, top, right - left + 1, bottom - top + 1);
		}

		public void SetEmblemTinted(Texture2D source, Color tint, float scale = 1f)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			if (source == null)
			{
				return;
			}
			Texture2D newEmblem = TintTexture(source, tint, scale);
			Texture2D old = _ownedEmblem;
			_ownedEmblem = newEmblem;
			((WindowBase2)this).set_Emblem(newEmblem);
			if (old != null)
			{
				try
				{
					((GraphicsResource)old).Dispose();
				}
				catch
				{
				}
			}
		}

		private static Texture2D TintTexture(Texture2D src, Color tint, float scale)
		{
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0192: Unknown result type (might be due to invalid IL or missing references)
			//IL_0197: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_036a: Unknown result type (might be due to invalid IL or missing references)
			//IL_036f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0395: Unknown result type (might be due to invalid IL or missing references)
			//IL_039a: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b3: Expected O, but got Unknown
			int dstW = Math.Max(1, (int)Math.Round((float)src.get_Width() * scale));
			int dstH = Math.Max(1, (int)Math.Round((float)src.get_Height() * scale));
			Color[] srcPixels = (Color[])(object)new Color[src.get_Width() * src.get_Height()];
			src.GetData<Color>(srcPixels);
			Color[] dstPixels = (Color[])(object)new Color[dstW * dstH];
			if (dstW == src.get_Width() && dstH == src.get_Height())
			{
				for (int i = 0; i < srcPixels.Length; i++)
				{
					Color c = srcPixels[i];
					dstPixels[i] = new Color((byte)(((Color)(ref c)).get_R() * ((Color)(ref tint)).get_R() / 255), (byte)(((Color)(ref c)).get_G() * ((Color)(ref tint)).get_G() / 255), (byte)(((Color)(ref c)).get_B() * ((Color)(ref tint)).get_B() / 255), ((Color)(ref c)).get_A());
				}
			}
			else
			{
				float invScaleX = (float)src.get_Width() / (float)dstW;
				float invScaleY = (float)src.get_Height() / (float)dstH;
				for (int y = 0; y < dstH; y++)
				{
					float sy = ((float)y + 0.5f) * invScaleY - 0.5f;
					int y2 = Math.Max(0, (int)Math.Floor(sy));
					int y3 = Math.Min(src.get_Height() - 1, y2 + 1);
					float dy = sy - (float)y2;
					for (int x = 0; x < dstW; x++)
					{
						float sx = ((float)x + 0.5f) * invScaleX - 0.5f;
						int x2 = Math.Max(0, (int)Math.Floor(sx));
						int x3 = Math.Min(src.get_Width() - 1, x2 + 1);
						float dx = sx - (float)x2;
						Color c2 = srcPixels[y2 * src.get_Width() + x2];
						Color c4 = srcPixels[y2 * src.get_Width() + x3];
						Color c3 = srcPixels[y3 * src.get_Width() + x2];
						Color c5 = srcPixels[y3 * src.get_Width() + x3];
						float wR = (1f - dx) * (1f - dy) * (float)(int)((Color)(ref c2)).get_R() + dx * (1f - dy) * (float)(int)((Color)(ref c4)).get_R() + (1f - dx) * dy * (float)(int)((Color)(ref c3)).get_R() + dx * dy * (float)(int)((Color)(ref c5)).get_R();
						float wG = (1f - dx) * (1f - dy) * (float)(int)((Color)(ref c2)).get_G() + dx * (1f - dy) * (float)(int)((Color)(ref c4)).get_G() + (1f - dx) * dy * (float)(int)((Color)(ref c3)).get_G() + dx * dy * (float)(int)((Color)(ref c5)).get_G();
						float wB = (1f - dx) * (1f - dy) * (float)(int)((Color)(ref c2)).get_B() + dx * (1f - dy) * (float)(int)((Color)(ref c4)).get_B() + (1f - dx) * dy * (float)(int)((Color)(ref c3)).get_B() + dx * dy * (float)(int)((Color)(ref c5)).get_B();
						float wA = (1f - dx) * (1f - dy) * (float)(int)((Color)(ref c2)).get_A() + dx * (1f - dy) * (float)(int)((Color)(ref c4)).get_A() + (1f - dx) * dy * (float)(int)((Color)(ref c3)).get_A() + dx * dy * (float)(int)((Color)(ref c5)).get_A();
						dstPixels[y * dstW + x] = new Color((byte)(wR * (float)(int)((Color)(ref tint)).get_R() / 255f), (byte)(wG * (float)(int)((Color)(ref tint)).get_G() / 255f), (byte)(wB * (float)(int)((Color)(ref tint)).get_B() / 255f), (byte)wA);
					}
				}
			}
			GraphicsDeviceContext gdc = GameService.Graphics.LendGraphicsDeviceContext();
			try
			{
				Texture2D val = new Texture2D(((GraphicsDeviceContext)(ref gdc)).get_GraphicsDevice(), dstW, dstH);
				val.SetData<Color>(dstPixels);
				return val;
			}
			finally
			{
				((GraphicsDeviceContext)(ref gdc)).Dispose();
			}
		}

		public void SetResetCountdownOverlay(Texture2D icon, string countdown)
		{
			_rechargeIcon = icon;
			_countdownText = countdown;
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			((WindowBase2)this).PaintBeforeChildren(spriteBatch, bounds);
			if (_ownedBackground != null && ((Container)this).get_ContentRegion().Width > 0 && ((Container)this).get_ContentRegion().Height > 0)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _ownedBackground, ((Container)this).get_ContentRegion(), (Rectangle?)null, Color.get_White() * _bgOpacity);
			}
		}

		public override void PaintAfterChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0151: Unknown result type (might be due to invalid IL or missing references)
			//IL_0164: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
			((WindowBase2)this).PaintAfterChildren(spriteBatch, bounds);
			if (_compactTitle)
			{
				int leftBarX = ((WindowBase2)this).get_TitleBarBounds().X - 2;
				int leftBarY = ((WindowBase2)this).get_TitleBarBounds().Y - 11;
				int titleWidth = 0;
				if (!string.IsNullOrWhiteSpace(_customTitle))
				{
					BitmapFont titleFont = GameService.Content.get_DefaultFont18();
					titleWidth = (int)titleFont.MeasureString(_customTitle).Width;
					int titleY = leftBarY + 4 + 12;
					SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, _customTitle, titleFont, new Rectangle(leftBarX + 80, titleY, titleWidth + 4, 28), Colors.ColonialWhite, false, (HorizontalAlignment)0, (VerticalAlignment)1);
				}
				if (!string.IsNullOrWhiteSpace(_customSubtitle) && titleWidth > 0)
				{
					BitmapFont subFont = GameService.Content.get_DefaultFont14();
					int subWidth = (int)subFont.MeasureString(_customSubtitle).Width;
					int subX = leftBarX + 80 + titleWidth + 12;
					int subY = leftBarY + 4 + 12 + 6;
					SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, _customSubtitle, subFont, new Rectangle(subX, subY, subWidth + 4, 20), Color.get_White(), false, (HorizontalAlignment)0, (VerticalAlignment)1);
				}
			}
			if (_rechargeIcon != null && !string.IsNullOrEmpty(_countdownText))
			{
				int iconSize = (_compactTitle ? 14 : 16);
				BitmapFont font = (_compactTitle ? GameService.Content.get_DefaultFont14() : GameService.Content.get_DefaultFont16());
				int textWidth = (int)font.MeasureString(_countdownText).Width;
				int blockWidth = iconSize + 4 + textWidth;
				int iconX = ((Control)this).get_Size().X - 50 - blockWidth;
				int iconY = 13 + (16 - iconSize) / 2;
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _rechargeIcon, new Rectangle(iconX, iconY, iconSize, iconSize));
				int textX = iconX + iconSize + 4;
				int textY = iconY - (_compactTitle ? 5 : 3);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, _countdownText, font, new Rectangle(textX, textY, textWidth + 2, 22), Color.get_White(), false, (HorizontalAlignment)0, (VerticalAlignment)1);
			}
		}

		protected override void DisposeControl()
		{
			DetachGameTextureSubscription();
			try
			{
				Texture2D ownedBackground = _ownedBackground;
				if (ownedBackground != null)
				{
					((GraphicsResource)ownedBackground).Dispose();
				}
			}
			catch
			{
			}
			try
			{
				Texture2D ownedEmblem = _ownedEmblem;
				if (ownedEmblem != null)
				{
					((GraphicsResource)ownedEmblem).Dispose();
				}
			}
			catch
			{
			}
			((WindowBase2)this).DisposeControl();
		}
	}
}
