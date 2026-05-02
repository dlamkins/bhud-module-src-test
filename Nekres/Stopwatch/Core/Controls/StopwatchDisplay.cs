using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Extended;
using Blish_HUD.Graphics;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using Stopwatch;

namespace Nekres.Stopwatch.Core.Controls
{
	internal class StopwatchDisplay : Control
	{
		private const int TRACK_THICKNESS = 2;

		private const int ARC_THICKNESS = 6;

		private const int RING_PADDING = 25;

		private const int OUT_RING_PADDING = 4;

		private const int LOCK_BUTTON_SIZE = 18;

		private const int ARC_SEGMENTS = 90;

		private const float TRACK_OPACITY = 0.25f;

		private BitmapFontEx _font;

		private BitmapFont _statusFont;

		private Texture2D _circleTexture;

		private int _diameter;

		private bool _isDragging;

		private Point _grabOffset;

		private string _text;

		private bool _isStatusText;

		private FontSize _fontSize;

		private Color _color;

		private float _backgroundOpacity;

		private float _progress;

		private bool _isLocked = true;

		public string Text
		{
			get
			{
				return _text;
			}
			set
			{
				((Control)this).SetProperty<string>(ref _text, value, false, "Text");
			}
		}

		public bool IsStatusText
		{
			get
			{
				return _isStatusText;
			}
			set
			{
				((Control)this).SetProperty<bool>(ref _isStatusText, value, false, "IsStatusText");
			}
		}

		public FontSize FontSize
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _fontSize;
			}
			set
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_0036: Unknown result type (might be due to invalid IL or missing references)
				//IL_0047: Expected I4, but got Unknown
				if (((Control)this).SetProperty<FontSize>(ref _fontSize, value, false, "FontSize"))
				{
					_font?.Dispose();
					_font = StopwatchModule.ModuleInstance.ContentsManager.GetBitmapFont("fonts/RobotoMono-Regular.ttf", (int)value, Gw2FontRanges.DigitsOnly);
					_statusFont = GameService.Content.GetFont((FontFace)0, (FontSize)18, (FontStyle)0);
					RecalculateSize();
				}
			}
		}

		public Color Color
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _color;
			}
			set
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				((Control)this).SetProperty<Color>(ref _color, value, false, "Color");
			}
		}

		public float BackgroundOpacity
		{
			get
			{
				return _backgroundOpacity;
			}
			set
			{
				((Control)this).SetProperty<float>(ref _backgroundOpacity, value, false, "BackgroundOpacity");
			}
		}

		public float Progress
		{
			get
			{
				return _progress;
			}
			set
			{
				((Control)this).SetProperty<float>(ref _progress, MathHelper.Clamp(value, 0f, 1f), false, "Progress");
			}
		}

		public bool IsLocked
		{
			get
			{
				return _isLocked;
			}
			set
			{
				((Control)this).SetProperty<bool>(ref _isLocked, value, false, "IsLocked");
			}
		}

		public event EventHandler Dragged;

		public event EventHandler SetGoalTimeClicked;

		public StopwatchDisplay()
			: this()
		{
		}

		private void RecalculateSize()
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			if (_font != null)
			{
				Size2 val = ((BitmapFont)_font).MeasureString("-00:00:00.000");
				int textHalfW = (int)Math.Ceiling(val.Width / 2f);
				int textHalfH = (int)Math.Ceiling(val.Height / 2f);
				int innerRadius = (int)Math.Ceiling((float)Math.Sqrt(textHalfW * textHalfW + textHalfH * textHalfH)) + 25;
				_diameter = (int)Math.Ceiling(((float)innerRadius + 3f + 4f) * 2f);
				if (_diameter % 2 != 0)
				{
					_diameter++;
				}
				_diameter = Math.Max(_diameter, 100);
				((Control)this).set_Size(new Point(_diameter, _diameter));
				RegenerateCircleTexture();
			}
		}

		private void RegenerateCircleTexture()
		{
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Expected O, but got Unknown
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			Texture2D circleTexture = _circleTexture;
			if (circleTexture != null)
			{
				((GraphicsResource)circleTexture).Dispose();
			}
			_circleTexture = null;
			if (_diameter <= 0)
			{
				return;
			}
			try
			{
				GraphicsDeviceContext ctx = GameService.Graphics.LendGraphicsDeviceContext();
				try
				{
					Texture2D texture = new Texture2D(((GraphicsDeviceContext)(ref ctx)).get_GraphicsDevice(), _diameter, _diameter);
					Color[] data = (Color[])(object)new Color[_diameter * _diameter];
					float centerOffset = (float)_diameter / 2f;
					float fillRadius = centerOffset - 4f;
					for (int y = 0; y < _diameter; y++)
					{
						for (int x = 0; x < _diameter; x++)
						{
							float num = (float)x - centerOffset + 0.5f;
							float dy = (float)y - centerOffset + 0.5f;
							float dist = (float)Math.Sqrt(num * num + dy * dy);
							if (dist <= fillRadius)
							{
								byte a = (byte)(MathHelper.Clamp(fillRadius - dist, 0f, 1f) * 255f);
								data[y * _diameter + x] = new Color(a, a, a, a);
							}
						}
					}
					texture.SetData<Color>(data);
					_circleTexture = texture;
				}
				finally
				{
					((GraphicsDeviceContext)(ref ctx)).Dispose();
				}
			}
			catch (Exception ex)
			{
				StopwatchModule.Logger.Warn(ex, "Failed to create circle texture.");
			}
		}

		protected override CaptureType CapturesInput()
		{
			return (CaptureType)4;
		}

		private Rectangle GetLockButtonBounds()
		{
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			float cy;
			float cx;
			float buttonDist = (cy = (cx = (float)_diameter / 2f)) - 6f - 9f - 4f;
			float angle = -(float)Math.PI / 4f;
			int num = (int)(cx + (float)Math.Cos(angle) * buttonDist - 9f);
			int y = (int)(cy + (float)Math.Sin(angle) * buttonDist - 9f);
			return new Rectangle(num, y, 18, 18);
		}

		private bool IsOverLockButton()
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			if (((Control)this).get_MouseOver())
			{
				Rectangle lockButtonBounds = GetLockButtonBounds();
				return ((Rectangle)(ref lockButtonBounds)).Contains(((Control)this).get_RelativeMousePosition());
			}
			return false;
		}

		private Rectangle GetGoalButtonBounds()
		{
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			float cy;
			float cx;
			float buttonDist = (cy = (cx = (float)_diameter / 2f)) - 6f - 9f - 4f;
			float angle = (float)Math.PI * -3f / 4f;
			int num = (int)(cx + (float)Math.Cos(angle) * buttonDist - 9f);
			int y = (int)(cy + (float)Math.Sin(angle) * buttonDist - 9f);
			return new Rectangle(num, y, 18, 18);
		}

		private bool IsOverGoalButton()
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			if (((Control)this).get_MouseOver())
			{
				Rectangle goalButtonBounds = GetGoalButtonBounds();
				return ((Rectangle)(ref goalButtonBounds)).Contains(((Control)this).get_RelativeMousePosition());
			}
			return false;
		}

		protected override void OnLeftMouseButtonPressed(MouseEventArgs e)
		{
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			if (_isDragging)
			{
				_isDragging = false;
				this.Dragged?.Invoke(this, EventArgs.Empty);
			}
			if (IsOverLockButton())
			{
				_isLocked = !_isLocked;
				GameService.Content.PlaySoundEffectByName("button-click");
				return;
			}
			if (IsOverGoalButton())
			{
				GameService.Content.PlaySoundEffectByName("button-click");
				this.SetGoalTimeClicked?.Invoke(this, EventArgs.Empty);
				return;
			}
			if (!_isLocked)
			{
				_isDragging = true;
				_grabOffset = ((Control)this).get_RelativeMousePosition();
			}
			((Control)this).OnLeftMouseButtonPressed(e);
		}

		protected override void OnLeftMouseButtonReleased(MouseEventArgs e)
		{
			if (_isDragging)
			{
				_isDragging = false;
				this.Dragged?.Invoke(this, EventArgs.Empty);
			}
			((Control)this).OnLeftMouseButtonReleased(e);
		}

		protected override void OnMouseMoved(MouseEventArgs e)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			if (_isDragging)
			{
				Point delta = ((Control)this).get_RelativeMousePosition() - _grabOffset;
				if (delta != Point.get_Zero())
				{
					((Control)this).set_Location(((Control)this).get_Location() + delta);
				}
			}
			((Control)this).OnMouseMoved(e);
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_015c: Unknown result type (might be due to invalid IL or missing references)
			if (_font != null && _diameter > 0)
			{
				Vector2 center = default(Vector2);
				((Vector2)(ref center))._002Ector((float)bounds.Width / 2f, (float)bounds.Height / 2f);
				float ringRadius = (float)bounds.Width / 2f - 4f - 3f;
				if (_circleTexture != null && _backgroundOpacity > 0f)
				{
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _circleTexture, bounds, Color.get_Black() * _backgroundOpacity);
				}
				DrawArc(spriteBatch, center, ringRadius, 0f, (float)Math.PI * 2f, 2, Color.get_White() * 0.25f, 128);
				if (_progress > 0.001f)
				{
					float startAngle = -(float)Math.PI / 2f;
					float sweepAngle = (float)Math.PI * 2f * _progress;
					int segments = Math.Max(4, (int)(90f * _progress));
					DrawArc(spriteBatch, center, ringRadius, startAngle, sweepAngle, 6, _color, segments);
				}
				if (!string.IsNullOrEmpty(_text))
				{
					DrawText(spriteBatch, center, bounds);
				}
				if (((Control)this).get_MouseOver())
				{
					DrawLockIcon(spriteBatch, GetLockButtonBounds(), _isLocked);
					DrawGoalIcon(spriteBatch, GetGoalButtonBounds());
				}
				if (!_isLocked)
				{
					DrawArc(spriteBatch, center, ringRadius + 3f + 3f, 0f, (float)Math.PI * 2f, 1, Color.get_White() * 0.35f, 128);
				}
			}
		}

		private void DrawText(SpriteBatch spriteBatch, Vector2 center, Rectangle bounds)
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			if (_isStatusText && _statusFont != null)
			{
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, _text, _statusFont, bounds, _color, false, (HorizontalAlignment)1, (VerticalAlignment)1);
			}
			else if (_font != null)
			{
				string refString = ((_text.Contains(":") && _text.IndexOf(':') != _text.LastIndexOf(':')) ? "00:00:00.000" : "00:00.000");
				Size2 refSize = ((BitmapFont)_font).MeasureString(refString);
				float fixedX = center.X - refSize.Width / 2f;
				float fixedY = center.Y - refSize.Height / 2f;
				string drawText = _text;
				if (drawText.StartsWith("-"))
				{
					Size2 minusSize = ((BitmapFont)_font).MeasureString("-");
					fixedX -= minusSize.Width;
				}
				Rectangle fixedRect = default(Rectangle);
				((Rectangle)(ref fixedRect))._002Ector((int)fixedX, (int)fixedY, bounds.Width, bounds.Height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, drawText, (BitmapFont)(object)_font, fixedRect, _color, false, true, 2, (HorizontalAlignment)0, (VerticalAlignment)0);
			}
		}

		private void DrawArc(SpriteBatch spriteBatch, Vector2 center, float radius, float startAngle, float sweepAngle, int thickness, Color color, int segments)
		{
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			if (Math.Abs(sweepAngle) < 0.001f || segments <= 0)
			{
				return;
			}
			float step = sweepAngle / (float)segments;
			Texture2D pixel = Textures.get_Pixel();
			for (int i = 0; i < segments; i++)
			{
				float a1 = startAngle + step * (float)i;
				float a2 = a1 + step;
				Vector2 p1 = center + new Vector2((float)Math.Cos(a1), (float)Math.Sin(a1)) * radius;
				Vector2 edge = center + new Vector2((float)Math.Cos(a2), (float)Math.Sin(a2)) * radius - p1;
				float angle = (float)Math.Atan2(edge.Y, edge.X);
				int length = (int)Math.Ceiling(((Vector2)(ref edge)).Length());
				if (length > 0)
				{
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, new Rectangle((int)p1.X, (int)p1.Y, length + 1, thickness), (Rectangle?)null, color, angle, new Vector2(0f, 0.5f), (SpriteEffects)0);
				}
			}
		}

		private void DrawLockIcon(SpriteBatch spriteBatch, Rectangle area, bool locked)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			Texture2D pixel = Textures.get_Pixel();
			if (((Rectangle)(ref area)).Contains(((Control)this).get_RelativeMousePosition()))
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, area, Color.get_White() * 0.2f);
			}
			int bodyW = (int)((float)area.Width * 0.75f);
			int bodyH = (int)((float)area.Height * 0.45f);
			int bodyX = area.X + (area.Width - bodyW) / 2;
			int bodyY = ((Rectangle)(ref area)).get_Bottom() - bodyH - 1;
			int shackleW = (int)((float)bodyW * 0.6f);
			int shackleH = (int)((float)area.Height * 0.4f);
			int shackleThick = Math.Max(2, (int)((float)bodyW * 0.2f));
			Color iconColor = (Color)(locked ? Color.get_White() : new Color(100, 255, 100)) * 0.85f;
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, new Rectangle(bodyX, bodyY, bodyW, bodyH), iconColor);
			int shackleX = (locked ? (bodyX + (bodyW - shackleW) / 2) : (bodyX + bodyW - shackleW - 1));
			int shackleTop = bodyY - shackleH + shackleThick;
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, new Rectangle(shackleX, shackleTop, shackleThick, shackleH), iconColor);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, new Rectangle(shackleX + shackleW - shackleThick, shackleTop, shackleThick, shackleH), iconColor);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, new Rectangle(shackleX, shackleTop, shackleW, shackleThick), iconColor);
		}

		private void DrawGoalIcon(SpriteBatch spriteBatch, Rectangle area)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			Texture2D pixel = Textures.get_Pixel();
			if (((Rectangle)(ref area)).Contains(((Control)this).get_RelativeMousePosition()))
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, area, Color.get_White() * 0.2f);
			}
			Color iconColor = Color.get_White() * 0.85f;
			Vector2 center = default(Vector2);
			((Vector2)(ref center))._002Ector((float)((Rectangle)(ref area)).get_Center().X, (float)((Rectangle)(ref area)).get_Center().Y);
			float radius = (float)area.Width * 0.35f;
			DrawArc(spriteBatch, center, radius, 0f, (float)Math.PI * 2f, 2, iconColor, 32);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, new Rectangle((int)center.X - 1, (int)center.Y - (int)radius + 2, 2, (int)radius - 2), iconColor);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, new Rectangle((int)center.X, (int)center.Y - 1, (int)(radius * 0.7f), 2), iconColor);
		}

		protected override void DisposeControl()
		{
			_font?.Dispose();
			Texture2D circleTexture = _circleTexture;
			if (circleTexture != null)
			{
				((GraphicsResource)circleTexture).Dispose();
			}
			((Control)this).DisposeControl();
		}
	}
}
