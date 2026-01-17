using System;
using System.Reflection;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Common.UI.Views;
using Blish_HUD.Controls;
using Blish_HUD.Graphics;
using Blish_HUD.Input;
using CefSharp;
using Glide;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BhModule.WebPeeper
{
	public class BrowserWindow : StandardWindow
	{
		private static readonly Point _windowSize = new Point(500, 700);

		private static readonly Point _windowBgOffset = new Point(-35, -30);

		private static readonly Rectangle _windowBgEdgeSpliceBounds = new Rectangle(35, 20, 60, 35);

		private static readonly Rectangle _windowRegionParam = new Rectangle(-_windowBgOffset.X, -_windowBgOffset.Y, _windowSize.X, _windowSize.Y);

		private static readonly Rectangle _contentRegionParam = new Rectangle(50, 20, _windowSize.X - 30, _windowSize.Y - 5);

		private static readonly Texture2D _bg = BuildBg();

		private Rectangle _titleBounds;

		private static readonly Texture2D _pinTexture = WebPeeperModule.Instance.ContentsManager.GetTexture("pin.png");

		private Rectangle _pinBounds;

		private Vector2 _pinOrigin;

		private Rectangle _pinDestRect;

		private Tooltip _pinTooltip;

		private static readonly Color _pinColor = new Color(uint.MaxValue);

		private static readonly Color _pinHoveredColor = new Color(16777215u);

		private bool _pinHovered;

		private bool _firstShow = true;

		private string _titleWithEllipsis = "-";

		private CefService CefService => WebPeeperModule.Instance.CefService;

		private ModuleSettings Settings => WebPeeperModule.Instance.Settings;

		public BrowserWindow()
			: this(_bg, _windowRegionParam, _contentRegionParam)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			((WindowBase2)this).set_Subtitle("-");
			((Control)this).set_Parent((Container)(object)Control.get_Graphics().get_SpriteScreen());
			((WindowBase2)this).set_Title((string)null);
			((WindowBase2)this).set_SavesPosition(true);
			((WindowBase2)this).set_SavesSize(true);
			((Control)this).set_Location(new Point(((Control)Control.get_Graphics().get_SpriteScreen()).get_Width() / 2, 100));
			((WindowBase2)this).set_CanResize(true);
			((WindowBase2)this).set_Id("WebPeeper");
			_titleBounds = new Rectangle(40, -10, ((Control)this).get_Width(), 64);
			_pinBounds = new Rectangle(((Rectangle)(ref _titleBounds)).get_Location().X - 30, ((Rectangle)(ref _titleBounds)).get_Location().Y + 20, 20, 20);
			_pinOrigin = new Vector2((float)(_pinTexture.get_Width() / 2), (float)(_pinTexture.get_Height() / 2));
			_pinDestRect = new Rectangle(new Point(_pinBounds.X + (int)_pinOrigin.X, _pinBounds.Y + (int)_pinOrigin.Y), ((Rectangle)(ref _pinBounds)).get_Size());
			SetMaxOpacity();
		}

		private static Texture2D BuildBg()
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_0179: Unknown result type (might be due to invalid IL or missing references)
			//IL_017e: Unknown result type (might be due to invalid IL or missing references)
			//IL_018b: Unknown result type (might be due to invalid IL or missing references)
			//IL_019e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_01af: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01be: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d3: Expected O, but got Unknown
			GraphicsService graphics = GameService.Graphics;
			Texture2D texture = GameService.Content.GetTexture("controls/window/502049");
			Color[] array = (Color[])(object)new Color[texture.get_Width() * texture.get_Height()];
			texture.GetData<Color>(0, 0, (Rectangle?)texture.get_Bounds(), array, 0, array.Length);
			Point val = default(Point);
			((Point)(ref val))._002Ector(_windowSize.X - _windowBgOffset.X + _windowBgEdgeSpliceBounds.Width, _windowSize.Y - _windowBgOffset.Y + 20 + _windowBgEdgeSpliceBounds.Height);
			Color[] array2 = (Color[])(object)new Color[val.X * val.Y];
			Point val2 = default(Point);
			for (int i = 0; i < val.Y; i++)
			{
				for (int j = 0; j < val.X; j++)
				{
					((Point)(ref val2))._002Ector(j - (val.X - _windowBgEdgeSpliceBounds.Width), i - (val.Y - _windowBgEdgeSpliceBounds.Height));
					if (val2.X >= 0 || val2.Y >= 0)
					{
						if (j >= _windowBgEdgeSpliceBounds.X && i >= _windowBgEdgeSpliceBounds.Y)
						{
							int num = (texture.get_Height() - val.Y + i + 1) * texture.get_Width() - 1 - _windowBgEdgeSpliceBounds.Width;
							array2[i * val.X + j] = array[num + val2.X];
						}
					}
					else
					{
						array2[i * val.X + j] = array[i * texture.get_Width() + j];
					}
				}
			}
			GraphicsDeviceContext val3 = graphics.LendGraphicsDeviceContext();
			try
			{
				Texture2D val4 = new Texture2D(((GraphicsDeviceContext)(ref val3)).get_GraphicsDevice(), val.X, val.Y);
				val4.SetData<Color>(array2);
				return val4;
			}
			finally
			{
				((GraphicsDeviceContext)(ref val3)).Dispose();
			}
		}

		private void SetMaxOpacity()
		{
			object value = typeof(WindowBase2).GetField("_animFade", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(this);
			((Tween)((value is Tween) ? value : null)).OnUpdate((Action)delegate
			{
				if (((Control)this).get_Opacity() > Settings.WebWindowOpacity.get_Value())
				{
					((Control)this).set_Opacity(Settings.WebWindowOpacity.get_Value());
				}
			});
		}

		private void PaintTitle(SpriteBatch spriteBatch)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, _titleWithEllipsis, Control.get_Content().get_DefaultFont16(), _titleBounds, Colors.ColonialWhite, false, (HorizontalAlignment)0, (VerticalAlignment)1);
		}

		private void PaintPin(SpriteBatch spriteBatch)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _pinTexture, _pinDestRect, (Rectangle?)_pinTexture.get_Bounds(), _pinHovered ? _pinHoveredColor : _pinColor, ((WindowBase2)this).get_CanCloseWithEscape() ? 0f : MathHelper.ToRadians(-45f), _pinOrigin, (SpriteEffects)0);
		}

		public Task<bool> PrepareQuitBrowser()
		{
			if (!((Control)this).get_Visible())
			{
				((WindowBase2)this).ClearView();
				return Task.FromResult(result: true);
			}
			TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
			((Control)this).add_Hidden((EventHandler<EventArgs>)onHidden);
			((Control)this).Hide();
			tcs.Task.ContinueWith(delegate
			{
				((Control)this).remove_Hidden((EventHandler<EventArgs>)onHidden);
			});
			return tcs.Task;
			void onHidden(object sender, EventArgs e)
			{
				((WindowBase2)this).ClearView();
				tcs.SetResult(result: true);
			}
		}

		protected override void OnShown(EventArgs e)
		{
			if (_firstShow)
			{
				_firstShow = false;
				int num = ((Control)((Control)this).get_Parent()).get_Height() - 100;
				if (((Control)this).get_Height() > num)
				{
					((Control)this).set_Height(num);
				}
				int num2 = ((Control)((Control)this).get_Parent()).get_Width() - 200;
				if (((Control)this).get_Width() > num2)
				{
					((Control)this).set_Width(num2);
				}
			}
			if (CefService.WebBrowser != null)
			{
				if (Settings.IsAutoPauseWeb.get_Value())
				{
					CefService.WebBrowser?.GetBrowserHost().WasHidden(hidden: false);
				}
				((Control)this).OnShown(e);
			}
		}

		protected override void OnHidden(EventArgs e)
		{
			if (CefService.WebBrowser != null)
			{
				if (CefService.WebBrowser.CanExecuteJavascriptInMainFrame)
				{
					CefService.WebBrowser.ExecuteScriptAsync("webPeeper_blur()");
				}
				if (Settings.IsAutoQuitPrcess.get_Value())
				{
					CefService.CloseWebBrowser();
				}
				else if (Settings.IsAutoPauseWeb.get_Value())
				{
					CefService.WebBrowser?.GetBrowserHost().WasHidden(hidden: true);
				}
			}
			BookmarkPanel.Instance?.SetChildrenEditState(edit: false);
			((Control)this).OnHidden(e);
		}

		public override void RecalculateLayout()
		{
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			_titleWithEllipsis = (string.IsNullOrWhiteSpace(((WindowBase2)this).get_Subtitle()) ? "-" : ((WindowBase2)this).get_Subtitle());
			float width = Control.get_Content().get_DefaultFont16().MeasureString(_titleWithEllipsis)
				.Width;
			float num = width - (float)_titleBounds.Width + (float)_titleBounds.X + 120f;
			if (num > 0f)
			{
				float num2 = width / (float)_titleWithEllipsis.Length;
				float num3 = num / num2;
				int num4 = _titleWithEllipsis.Length - (int)num3;
				if (num4 <= 0)
				{
					return;
				}
				_titleWithEllipsis = _titleWithEllipsis.Substring(0, num4);
				_titleWithEllipsis += "...";
			}
			((Rectangle)(ref _pinBounds)).set_Location(new Point(((Rectangle)(ref _titleBounds)).get_Location().X - 30, ((Rectangle)(ref _titleBounds)).get_Location().Y + _pinBounds.Height));
			((WindowBase2)this).RecalculateLayout();
		}

		public override void PaintAfterChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			((WindowBase2)this).PaintAfterChildren(spriteBatch, bounds);
			PaintTitle(spriteBatch);
			PaintPin(spriteBatch);
		}

		protected override void OnMouseMoved(MouseEventArgs e)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Expected O, but got Unknown
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Expected O, but got Unknown
			_pinHovered = ((Rectangle)(ref _pinBounds)).Contains(((Control)this).get_RelativeMousePosition());
			if (_pinTooltip == null && _pinHovered)
			{
				_pinTooltip = new Tooltip((ITooltipView)new BasicTooltipView("Click To " + (((WindowBase2)this).get_CanCloseWithEscape() ? "Ignore" : "Allow") + " Esc To Close Window"));
				((Control)_pinTooltip).Show();
			}
			else if (!_pinHovered && _pinTooltip != null)
			{
				((Control)_pinTooltip).Hide();
				_pinTooltip = null;
			}
			((WindowBase2)this).OnMouseMoved(e);
		}

		protected override void OnLeftMouseButtonPressed(MouseEventArgs e)
		{
			if (_pinHovered)
			{
				((WindowBase2)this).set_CanCloseWithEscape(!((WindowBase2)this).get_CanCloseWithEscape());
			}
			((WindowBase2)this).OnLeftMouseButtonPressed(e);
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			_titleBounds.Width = ((Control)this).get_Width();
			((WindowBase2)this).OnResized(e);
		}

		protected override Point HandleWindowResize(Point newSize)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			return new Point(MathHelper.Clamp(newSize.X, 200, 1024), MathHelper.Clamp(newSize.Y, 300, 1024));
		}

		protected override void DisposeControl()
		{
			WebPainter.DisposeWebTexture();
			Texture2D bg = _bg;
			if (bg != null)
			{
				((GraphicsResource)bg).Dispose();
			}
			((WindowBase2)this).DisposeControl();
		}
	}
}
