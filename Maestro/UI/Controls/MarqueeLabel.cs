using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace Maestro.UI.Controls
{
	public class MarqueeLabel : LabelBase
	{
		private enum ScrollState
		{
			Idle,
			PauseStart,
			ScrollingLeft,
			PauseEnd
		}

		private const float ScrollSpeed = 40f;

		private const double PauseDurationMs = 2000.0;

		private ScrollState _state;

		private float _scrollOffset;

		private double _pauseTimer;

		private double _lastPaintTime;

		public string Text
		{
			get
			{
				return base._text;
			}
			set
			{
				if (((Control)this).SetProperty<string>(ref base._text, value, true, "Text"))
				{
					ResetScroll();
					UpdateTooltip();
				}
			}
		}

		public BitmapFont Font
		{
			get
			{
				return base._font;
			}
			set
			{
				((Control)this).SetProperty<BitmapFont>(ref base._font, value, true, "Font");
			}
		}

		public Color TextColor
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return base._textColor;
			}
			set
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				((Control)this).SetProperty<Color>(ref base._textColor, value, false, "TextColor");
			}
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			if (base._font != null && !string.IsNullOrEmpty(base._text))
			{
				float textWidth = base._font.MeasureString(base._text).Width;
				if (textWidth > (float)bounds.Width)
				{
					UpdateScrollState(textWidth - (float)bounds.Width);
				}
				else if (_state != 0)
				{
					_state = ScrollState.Idle;
					_scrollOffset = 0f;
				}
				Rectangle drawBounds = default(Rectangle);
				((Rectangle)(ref drawBounds))._002Ector(bounds.X - (int)_scrollOffset, bounds.Y, (int)textWidth + 1, bounds.Height);
				((LabelBase)this).DrawText(spriteBatch, drawBounds, base._text);
			}
		}

		private void UpdateScrollState(float maxOffset)
		{
			double now = GameService.Overlay.get_CurrentGameTime().get_TotalGameTime().TotalMilliseconds;
			double elapsed = ((_lastPaintTime > 0.0) ? (now - _lastPaintTime) : 0.0);
			_lastPaintTime = now;
			if (elapsed <= 0.0 || elapsed > 500.0)
			{
				return;
			}
			switch (_state)
			{
			case ScrollState.Idle:
				_state = ScrollState.PauseStart;
				_pauseTimer = 2000.0;
				_scrollOffset = 0f;
				break;
			case ScrollState.PauseStart:
				_pauseTimer -= elapsed;
				if (_pauseTimer <= 0.0)
				{
					_state = ScrollState.ScrollingLeft;
				}
				break;
			case ScrollState.ScrollingLeft:
				_scrollOffset += 40f * (float)(elapsed / 1000.0);
				if (_scrollOffset >= maxOffset)
				{
					_scrollOffset = maxOffset;
					_state = ScrollState.PauseEnd;
					_pauseTimer = 2000.0;
				}
				break;
			case ScrollState.PauseEnd:
				_pauseTimer -= elapsed;
				if (_pauseTimer <= 0.0)
				{
					_scrollOffset = 0f;
					_state = ScrollState.PauseStart;
					_pauseTimer = 2000.0;
				}
				break;
			}
		}

		private void ResetScroll()
		{
			_scrollOffset = 0f;
			_state = ScrollState.Idle;
			_pauseTimer = 0.0;
			_lastPaintTime = 0.0;
		}

		private void UpdateTooltip()
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			if (base._font == null || string.IsNullOrEmpty(base._text))
			{
				((Control)this).set_BasicTooltipText((string)null);
				return;
			}
			float textWidth = base._font.MeasureString(base._text).Width;
			((Control)this).set_BasicTooltipText((textWidth > (float)((Control)this).get_Width()) ? base._text : null);
		}

		public MarqueeLabel()
			: this()
		{
		}
	}
}
