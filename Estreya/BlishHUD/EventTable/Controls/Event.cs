using System;
using Blish_HUD;
using Blish_HUD.Common.UI.Views;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Estreya.BlishHUD.EventTable.Models;
using Estreya.BlishHUD.Shared.State;
using Estreya.BlishHUD.Shared.UI.Views;
using Estreya.BlishHUD.Shared.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;

namespace Estreya.BlishHUD.EventTable.Controls
{
	public class Event : IDisposable
	{
		private IconState _iconState;

		private TranslationState _translationState;

		private readonly Func<DateTime> _getNowAction;

		private readonly DateTime _startTime;

		private readonly DateTime _endTime;

		private readonly Func<BitmapFont> _getFontAction;

		private readonly Func<bool> _getDrawBorders;

		private readonly Func<bool> _getDrawCrossout;

		private readonly Func<Color> _getTextColor;

		private readonly Func<Color> _getColorAction;

		private readonly Func<bool> _getDrawShadowAction;

		private readonly Func<Color> _getShadowColor;

		private readonly Func<bool> _getShowTooltips;

		private Tooltip _tooltip;

		public Estreya.BlishHUD.EventTable.Models.Event Ev { get; private set; }

		public event EventHandler HideRequested;

		public event EventHandler DisableRequested;

		public event EventHandler FinishRequested;

		public Event(Estreya.BlishHUD.EventTable.Models.Event ev, IconState iconState, TranslationState translationState, Func<DateTime> getNowAction, DateTime startTime, DateTime endTime, Func<BitmapFont> getFontAction, Func<bool> getDrawBorders, Func<bool> getDrawCrossout, Func<Color> getTextColor, Func<Color> getColorAction, Func<bool> getDrawShadowAction, Func<Color> getShadowColor, Func<bool> getShowTooltips)
		{
			Ev = ev;
			_iconState = iconState;
			_translationState = translationState;
			_getNowAction = getNowAction;
			_startTime = startTime;
			_endTime = endTime;
			_getFontAction = getFontAction;
			_getDrawBorders = getDrawBorders;
			_getDrawCrossout = getDrawCrossout;
			_getTextColor = getTextColor;
			_getColorAction = getColorAction;
			_getDrawShadowAction = getDrawShadowAction;
			_getShadowColor = getShadowColor;
			_getShowTooltips = getShowTooltips;
		}

		public ContextMenuStrip BuildContextMenu()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected O, but got Unknown
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			ContextMenuStrip menu = new ContextMenuStrip();
			ContextMenuStripItem val = new ContextMenuStripItem("Disable");
			((Control)val).set_Parent((Container)(object)menu);
			((Control)val).set_BasicTooltipText("Disables the event entirely.");
			((Control)val).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				this.DisableRequested?.Invoke(this, EventArgs.Empty);
			});
			ContextMenuStripItem val2 = new ContextMenuStripItem("Hide");
			((Control)val2).set_Parent((Container)(object)menu);
			((Control)val2).set_BasicTooltipText("Hides the event until the next reset.");
			((Control)val2).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				this.HideRequested?.Invoke(this, EventArgs.Empty);
			});
			ContextMenuStripItem val3 = new ContextMenuStripItem("Finish");
			((Control)val3).set_Parent((Container)(object)menu);
			((Control)val3).set_BasicTooltipText("Completes the event until the next reset.");
			((Control)val3).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				this.FinishRequested?.Invoke(this, EventArgs.Empty);
			});
			return menu;
		}

		public Tooltip BuildTooltip()
		{
			//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cd: Expected O, but got Unknown
			DateTime now = _getNowAction();
			bool num = _startTime.AddMinutes(Ev.Duration) < now;
			bool isNext = !num && _startTime > now;
			bool isCurrent = !num && !isNext;
			string description = Ev.Location + ((!string.IsNullOrWhiteSpace(Ev.Location)) ? "\n" : string.Empty) + "\n";
			if (num)
			{
				TimeSpan finishedSince = now - _startTime.AddMinutes(Ev.Duration);
				description = description + _translationState.GetTranslation("event-tooltip-finishedSince", "Finished since") + ": " + FormatTime(finishedSince);
			}
			else if (isNext)
			{
				TimeSpan startsIn = _startTime - now;
				description = description + _translationState.GetTranslation("event-tooltip-startsIn", "Starts in") + ": " + FormatTime(startsIn);
			}
			else if (isCurrent)
			{
				TimeSpan remaining = GetTimeRemaining(now);
				description = description + _translationState.GetTranslation("event-tooltip-remaining", "Remaining") + ": " + FormatTime(remaining);
			}
			description = description + " (" + _translationState.GetTranslation("event-tooltip-startsAt", "Starts at") + ": " + FormatTime(_startTime.ToLocalTime()) + ")";
			return new Tooltip((ITooltipView)(object)new TooltipView(Ev.Name, description, _iconState.GetIcon(Ev.Icon), _translationState));
		}

		public void Render(SpriteBatch spriteBatch, RectangleF bounds)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			BitmapFont font = _getFontAction();
			DrawBackground(spriteBatch, bounds);
			float nameWidth = (Ev.Filler ? 0f : DrawName(spriteBatch, bounds, font));
			DrawRemainingTime(spriteBatch, bounds, font, nameWidth);
			DrawCrossout(spriteBatch, bounds);
		}

		private void DrawBackground(SpriteBatch spriteBatch, RectangleF bounds)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			spriteBatch.DrawRectangle(Textures.get_Pixel(), bounds, _getColorAction(), _getDrawBorders() ? 1 : 0, Color.get_Black());
		}

		private float DrawName(SpriteBatch spriteBatch, RectangleF bounds, BitmapFont font)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			float xOffset = 5f;
			float maxWidth = bounds.Width - xOffset * 2f;
			float nameWidth = 0f;
			string text = Ev.Name;
			do
			{
				nameWidth = (float)Math.Ceiling(font.MeasureString(text).Width);
				if (string.IsNullOrWhiteSpace(text))
				{
					return 0f;
				}
				if (nameWidth > maxWidth)
				{
					text = text.Substring(0, text.Length - 1);
				}
			}
			while (nameWidth > maxWidth);
			RectangleF nameRect = default(RectangleF);
			((RectangleF)(ref nameRect))._002Ector(bounds.X + xOffset, bounds.Y, nameWidth, bounds.Height);
			spriteBatch.DrawString(text, font, nameRect, _getTextColor(), wrap: false, _getDrawShadowAction(), 1, _getShadowColor(), (HorizontalAlignment)0, (VerticalAlignment)1);
			return nameRect.Width;
		}

		private void DrawRemainingTime(SpriteBatch spriteBatch, RectangleF bounds, BitmapFont font, float nameWidth)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			if (nameWidth > bounds.Width)
			{
				return;
			}
			TimeSpan remainingTime = GetTimeRemaining(_getNowAction());
			if (!(remainingTime == TimeSpan.Zero))
			{
				string remainingTimeString = FormatTimeRemaining(remainingTime);
				float timeWidth = (float)Math.Ceiling(font.MeasureString(remainingTimeString).Width);
				float maxWidth = bounds.Width - nameWidth;
				float centerX = maxWidth / 2f - timeWidth / 2f;
				if (centerX < nameWidth)
				{
					centerX = nameWidth + 10f;
				}
				if (!(centerX + timeWidth > bounds.Width))
				{
					RectangleF timeRect = default(RectangleF);
					((RectangleF)(ref timeRect))._002Ector(centerX + bounds.X, bounds.Y, maxWidth, bounds.Height);
					Color textColor = _getTextColor();
					spriteBatch.DrawString(remainingTimeString, font, timeRect, textColor, wrap: false, _getDrawShadowAction(), 1, _getShadowColor(), (HorizontalAlignment)0, (VerticalAlignment)1);
				}
			}
		}

		private TimeSpan GetTimeRemaining(DateTime now)
		{
			if (!(now <= _startTime) && !(now >= _endTime))
			{
				return _startTime.AddMinutes(Ev.Duration) - now;
			}
			return TimeSpan.Zero;
		}

		private void DrawCrossout(SpriteBatch spriteBatch, RectangleF bounds)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			if (_getDrawCrossout())
			{
				spriteBatch.DrawCrossOut(Textures.get_Pixel(), bounds, Color.get_Red());
			}
		}

		private string FormatTimeRemaining(TimeSpan ts)
		{
			if (ts.Days > 0)
			{
				return ts.ToString("dd\\.hh\\:mm\\:ss");
			}
			if (ts.Hours > 0)
			{
				return ts.ToString("hh\\:mm\\:ss");
			}
			return ts.ToString("mm\\:ss");
		}

		private string FormatTime(DateTime dt)
		{
			return FormatTime(dt.TimeOfDay);
		}

		private string FormatTime(TimeSpan ts)
		{
			if (ts.Days > 0)
			{
				return ts.ToString("dd\\.hh\\:mm\\:ss");
			}
			return ts.ToString("hh\\:mm\\:ss");
		}

		public void Dispose()
		{
			_iconState = null;
			_translationState = null;
			Ev = null;
		}
	}
}
