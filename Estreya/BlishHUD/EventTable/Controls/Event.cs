using System;
using System.Globalization;
using Blish_HUD;
using Blish_HUD.Common.UI.Views;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Estreya.BlishHUD.EventTable.Models;
using Estreya.BlishHUD.Shared.Services;
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
		private static Logger logger = Logger.GetLogger<Event>();

		private readonly DateTime _endTime;

		private readonly Func<Color[]> _getColorAction;

		private readonly Func<bool> _getDrawBorders;

		private readonly Func<bool> _getDrawCrossout;

		private readonly Func<bool> _getDrawShadowAction;

		private readonly Func<BitmapFont> _getFontAction;

		private readonly Func<DateTime> _getNowAction;

		private readonly Func<Color> _getShadowColor;

		private readonly Func<string> _getAbsoluteTimeFormatStrings;

		private readonly Func<(string DaysFormat, string HoursFormat, string MinutesFormat)> _getTimespanFormatStrings;

		private readonly Func<Color> _getTextColor;

		private readonly DateTime _startTime;

		private Texture2D _backgroundColorTexture;

		private IconService _iconService;

		private TranslationService _translationService;

		public Estreya.BlishHUD.EventTable.Models.Event Model { get; private set; }

		public event EventHandler HideRequested;

		public event EventHandler DisableRequested;

		public event EventHandler ToggleFinishRequested;

		public Event(Estreya.BlishHUD.EventTable.Models.Event ev, IconService iconService, TranslationService translationService, Func<DateTime> getNowAction, DateTime startTime, DateTime endTime, Func<BitmapFont> getFontAction, Func<bool> getDrawBorders, Func<bool> getDrawCrossout, Func<Color> getTextColor, Func<Color[]> getColorAction, Func<bool> getDrawShadowAction, Func<Color> getShadowColor, Func<string> getDateTimeFormatString, Func<(string DaysFormat, string HoursFormat, string MinutesFormat)> getTimespanFormatStrings)
		{
			Model = ev;
			_iconService = iconService;
			_translationService = translationService;
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
			_getAbsoluteTimeFormatStrings = getDateTimeFormatString;
			_getTimespanFormatStrings = getTimespanFormatStrings;
		}

		public ContextMenuStrip BuildContextMenu()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected O, but got Unknown
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			ContextMenuStrip menu = new ContextMenuStrip();
			ContextMenuStripItem val = new ContextMenuStripItem(_translationService.GetTranslation("event-contextMenu-disable-title", "Disable"));
			((Control)val).set_Parent((Container)(object)menu);
			((Control)val).set_BasicTooltipText(_translationService.GetTranslation("event-contextMenu-disable-tooltip", "Disables the event entirely."));
			((Control)val).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				this.DisableRequested?.Invoke(this, EventArgs.Empty);
			});
			ContextMenuStripItem val2 = new ContextMenuStripItem(_translationService.GetTranslation("event-contextMenu-hide-title", "Hide"));
			((Control)val2).set_Parent((Container)(object)menu);
			((Control)val2).set_BasicTooltipText(_translationService.GetTranslation("event-contextMenu-hide-tooltip", "Hides the event until the next reset."));
			((Control)val2).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				this.HideRequested?.Invoke(this, EventArgs.Empty);
			});
			ContextMenuStripItem val3 = new ContextMenuStripItem(_translationService.GetTranslation("event-contextMenu-toggleFinish-title", "Toggle Finish"));
			((Control)val3).set_Parent((Container)(object)menu);
			((Control)val3).set_BasicTooltipText(_translationService.GetTranslation("event-contextMenu-toggleFinish-tooltip", "Toggles the completed state of the event."));
			((Control)val3).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				this.ToggleFinishRequested?.Invoke(this, EventArgs.Empty);
			});
			return menu;
		}

		public Tooltip BuildTooltip()
		{
			//IL_01be: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c4: Expected O, but got Unknown
			DateTime now = _getNowAction();
			bool num = _startTime.AddMinutes(Model.Duration) < now;
			bool isNext = !num && _startTime > now;
			bool isCurrent = !num && !isNext;
			string description = Model.Location + ((!string.IsNullOrWhiteSpace(Model.Location)) ? "\n" : string.Empty) + "\n";
			if (num)
			{
				TimeSpan finishedSince = now - _startTime.AddMinutes(Model.Duration);
				description = description + _translationService.GetTranslation("event-tooltip-finishedSince", "Finished since") + ": " + FormatTimespan(finishedSince);
			}
			else if (isNext)
			{
				TimeSpan startsIn = _startTime - now;
				description = description + _translationService.GetTranslation("event-tooltip-startsIn", "Starts in") + ": " + FormatTimespan(startsIn);
			}
			else if (isCurrent)
			{
				TimeSpan remaining = GetTimeRemaining(now);
				description = description + _translationService.GetTranslation("event-tooltip-remaining", "Remaining") + ": " + FormatTimespan(remaining);
			}
			description = description + " (" + _translationService.GetTranslation("event-tooltip-startsAt", "Starts at") + ": " + FormatAbsoluteTime(_startTime) + ")";
			return new Tooltip((ITooltipView)(object)new TooltipView(Model.Name, description, _iconService.GetIcon(Model.Icon), _translationService));
		}

		public void Render(SpriteBatch spriteBatch, RectangleF bounds)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			BitmapFont font = _getFontAction();
			DrawBackground(spriteBatch, bounds);
			float nameWidth = (Model.Filler ? 0f : DrawName(spriteBatch, bounds, font));
			DrawRemainingTime(spriteBatch, bounds, font, nameWidth);
			DrawCrossout(spriteBatch, bounds);
		}

		private void DrawBackground(SpriteBatch spriteBatch, RectangleF bounds)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			Color[] colors = _getColorAction();
			if (colors.Length == 1)
			{
				spriteBatch.DrawRectangle(Textures.get_Pixel(), bounds, colors[0], _getDrawBorders() ? 1 : 0, Color.get_Black());
				return;
			}
			int width = (int)Math.Ceiling(bounds.Width);
			int height = (int)Math.Ceiling(bounds.Height);
			if (_backgroundColorTexture == null || _backgroundColorTexture.get_Height() != height || _backgroundColorTexture.get_Width() != width)
			{
				Texture2D backgroundColorTexture = _backgroundColorTexture;
				if (backgroundColorTexture != null)
				{
					((GraphicsResource)backgroundColorTexture).Dispose();
				}
				_backgroundColorTexture = ColorUtil.CreateColorGradientsTexture(colors, width, height);
			}
			spriteBatch.DrawRectangle(_backgroundColorTexture, bounds, Color.get_White(), _getDrawBorders() ? 1 : 0, Color.get_Black());
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
			string text = Model.Name;
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
				string remainingTimeString = FormatTimespan(remainingTime);
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
				return _startTime.AddMinutes(Model.Duration) - now;
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

		private string FormatTimespan(TimeSpan ts)
		{
			(string, string, string) formatStrings = _getTimespanFormatStrings();
			try
			{
				if (ts.Days > 0)
				{
					return ts.ToString(formatStrings.Item1, CultureInfo.InvariantCulture);
				}
				if (ts.Hours > 0)
				{
					return ts.ToString(formatStrings.Item2, CultureInfo.InvariantCulture);
				}
				return ts.ToString(formatStrings.Item3, CultureInfo.InvariantCulture);
			}
			catch (Exception ex)
			{
				logger.Warn(ex, $"Failed to format timespan {ts}:");
				return string.Empty;
			}
		}

		private string FormatAbsoluteTime(DateTime dt)
		{
			try
			{
				return dt.ToLocalTime().ToString(_getAbsoluteTimeFormatStrings(), CultureInfo.InvariantCulture);
			}
			catch (Exception ex)
			{
				logger.Warn(ex, $"Failed to format datetime {dt}:");
				return string.Empty;
			}
		}

		public void Dispose()
		{
			_iconService = null;
			_translationService = null;
			Model = null;
			Texture2D backgroundColorTexture = _backgroundColorTexture;
			if (backgroundColorTexture != null)
			{
				((GraphicsResource)backgroundColorTexture).Dispose();
			}
			_backgroundColorTexture = null;
		}
	}
}
