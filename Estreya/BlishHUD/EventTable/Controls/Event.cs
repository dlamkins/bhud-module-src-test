using System;
using System.Collections.Generic;
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

		private Texture2D _backgroundColorTexture;

		private IconService _iconService;

		private TranslationService _translationService;

		public DateTime StartTime { get; private set; }

		public Estreya.BlishHUD.EventTable.Models.Event Model { get; private set; }

		public event EventHandler HideClicked;

		public event EventHandler DisableClicked;

		public event EventHandler ToggleFinishClicked;

		public event EventHandler<string> MoveToAreaClicked;

		public event EventHandler<string> CopyToAreaClicked;

		public event EventHandler EnableReminderClicked;

		public event EventHandler DisableReminderClicked;

		public Event(Estreya.BlishHUD.EventTable.Models.Event ev, IconService iconService, TranslationService translationService, Func<DateTime> getNowAction, DateTime startTime, DateTime endTime, Func<BitmapFont> getFontAction, Func<bool> getDrawBorders, Func<bool> getDrawCrossout, Func<Color> getTextColor, Func<Color[]> getColorAction, Func<bool> getDrawShadowAction, Func<Color> getShadowColor, Func<string> getDateTimeFormatString, Func<(string DaysFormat, string HoursFormat, string MinutesFormat)> getTimespanFormatStrings)
		{
			Model = ev;
			_iconService = iconService;
			_translationService = translationService;
			_getNowAction = getNowAction;
			StartTime = startTime;
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

		public ContextMenuStrip BuildContextMenu(Func<List<string>> getAreaNames, string currentAreaName, Func<List<string>> getDisabledReminderKeys)
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
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Expected O, but got Unknown
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0200: Unknown result type (might be due to invalid IL or missing references)
			//IL_0207: Expected O, but got Unknown
			//IL_021c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0221: Unknown result type (might be due to invalid IL or missing references)
			//IL_0228: Unknown result type (might be due to invalid IL or missing references)
			//IL_0243: Unknown result type (might be due to invalid IL or missing references)
			//IL_024c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0253: Expected O, but got Unknown
			//IL_0268: Unknown result type (might be due to invalid IL or missing references)
			//IL_026d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0274: Unknown result type (might be due to invalid IL or missing references)
			//IL_028f: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_031d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0322: Unknown result type (might be due to invalid IL or missing references)
			//IL_032a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0345: Unknown result type (might be due to invalid IL or missing references)
			ContextMenuStrip menu = new ContextMenuStrip();
			ContextMenuStripItem val = new ContextMenuStripItem(_translationService.GetTranslation("event-contextMenu-disable-title", "Disable"));
			((Control)val).set_Parent((Container)(object)menu);
			((Control)val).set_BasicTooltipText(_translationService.GetTranslation("event-contextMenu-disable-tooltip", "Disables the event entirely."));
			((Control)val).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				this.DisableClicked?.Invoke(this, EventArgs.Empty);
			});
			ContextMenuStripItem val2 = new ContextMenuStripItem(_translationService.GetTranslation("event-contextMenu-hide-title", "Hide"));
			((Control)val2).set_Parent((Container)(object)menu);
			((Control)val2).set_BasicTooltipText(_translationService.GetTranslation("event-contextMenu-hide-tooltip", "Hides the event until the next reset."));
			((Control)val2).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				this.HideClicked?.Invoke(this, EventArgs.Empty);
			});
			ContextMenuStripItem val3 = new ContextMenuStripItem(_translationService.GetTranslation("event-contextMenu-toggleFinish-title", "Toggle Finish"));
			((Control)val3).set_Parent((Container)(object)menu);
			((Control)val3).set_BasicTooltipText(_translationService.GetTranslation("event-contextMenu-toggleFinish-tooltip", "Toggles the completed state of the event."));
			((Control)val3).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				this.ToggleFinishClicked?.Invoke(this, EventArgs.Empty);
			});
			bool isReminderEnabled = !getDisabledReminderKeys().Contains(Model.SettingKey);
			ContextMenuStrip reminderMenu = new ContextMenuStrip();
			ContextMenuStripItem val4 = new ContextMenuStripItem(_translationService.GetTranslation("event-contextMenu-reminderMenu-title", "Reminders..."));
			((Control)val4).set_Parent((Container)(object)menu);
			val4.set_Submenu(reminderMenu);
			ContextMenuStripItem val5 = new ContextMenuStripItem(_translationService.GetTranslation("event-contextMenu-reminderMenu-enable-title", "Enable Reminder"));
			((Control)val5).set_Parent((Container)(object)reminderMenu);
			((Control)val5).set_BasicTooltipText(_translationService.GetTranslation("event-contextMenu-reminderMenu-enable-tooltip", "Enables the corresponding reminder for this event."));
			((Control)val5).set_Enabled(!isReminderEnabled);
			((Control)val5).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				this.EnableReminderClicked?.Invoke(this, EventArgs.Empty);
			});
			ContextMenuStripItem val6 = new ContextMenuStripItem(_translationService.GetTranslation("event-contextMenu-reminderMenu-disable-title", "Disable Reminder"));
			((Control)val6).set_Parent((Container)(object)reminderMenu);
			((Control)val6).set_BasicTooltipText(_translationService.GetTranslation("event-contextMenu-reminderMenu-disable-tooltip", "Disables the corresponding reminder for this event."));
			((Control)val6).set_Enabled(isReminderEnabled);
			((Control)val6).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				this.DisableReminderClicked?.Invoke(this, EventArgs.Empty);
			});
			if (getAreaNames != null && currentAreaName != null)
			{
				List<string> areaNames = getAreaNames();
				if (areaNames.Count > 1)
				{
					ContextMenuStrip moveToAreaMenu = new ContextMenuStrip();
					ContextMenuStripItem val7 = new ContextMenuStripItem(_translationService.GetTranslation("event-contextMenu-moveToArea-title", "Move to Area..."));
					((Control)val7).set_Parent((Container)(object)menu);
					((Control)val7).set_BasicTooltipText(_translationService.GetTranslation("event-contextMenu-moveToArea-tooltip", "Moves the selected event to the selected area and disables it in the current area."));
					val7.set_Submenu(moveToAreaMenu);
					ContextMenuStrip copyToAreaMenu = new ContextMenuStrip();
					ContextMenuStripItem val8 = new ContextMenuStripItem(_translationService.GetTranslation("event-contextMenu-copyToArea-title", "Copy to Area..."));
					((Control)val8).set_Parent((Container)(object)menu);
					((Control)val8).set_BasicTooltipText(_translationService.GetTranslation("event-contextMenu-copyToArea-tooltip", "Copies the selected event to the selected area."));
					val8.set_Submenu(copyToAreaMenu);
					{
						foreach (string areaName in areaNames)
						{
							ContextMenuStripItem val9 = new ContextMenuStripItem(areaName);
							((Control)val9).set_Parent((Container)(object)moveToAreaMenu);
							((Control)val9).set_BasicTooltipText(_translationService.GetTranslation("event-contextMenu-moveToArea-tooltip", "Moves the selected event to the selected area and disables it in the current area."));
							((Control)val9).set_Enabled(areaName != currentAreaName);
							((Control)val9).add_Click((EventHandler<MouseEventArgs>)delegate
							{
								this.MoveToAreaClicked?.Invoke(this, areaName);
							});
							ContextMenuStripItem val10 = new ContextMenuStripItem(areaName);
							((Control)val10).set_Parent((Container)(object)copyToAreaMenu);
							((Control)val10).set_BasicTooltipText(_translationService.GetTranslation("event-contextMenu-copyToArea-tooltip", "Copies the selected event to the selected area."));
							((Control)val10).set_Enabled(areaName != currentAreaName);
							((Control)val10).add_Click((EventHandler<MouseEventArgs>)delegate
							{
								this.CopyToAreaClicked?.Invoke(this, areaName);
							});
						}
						return menu;
					}
				}
			}
			return menu;
		}

		public Tooltip BuildTooltip()
		{
			//IL_01be: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c4: Expected O, but got Unknown
			DateTime now = _getNowAction();
			bool num = StartTime.AddMinutes(Model.Duration) < now;
			bool isNext = !num && StartTime > now;
			bool isCurrent = !num && !isNext;
			string description = Model.Location + ((!string.IsNullOrWhiteSpace(Model.Location)) ? "\n" : string.Empty) + "\n";
			if (num)
			{
				TimeSpan finishedSince = now - StartTime.AddMinutes(Model.Duration);
				description = description + _translationService.GetTranslation("event-tooltip-finishedSince", "Finished since") + ": " + FormatTimespan(finishedSince);
			}
			else if (isNext)
			{
				TimeSpan startsIn = StartTime - now;
				description = description + _translationService.GetTranslation("event-tooltip-startsIn", "Starts in") + ": " + FormatTimespan(startsIn);
			}
			else if (isCurrent)
			{
				TimeSpan remaining = GetTimeRemaining(now);
				description = description + _translationService.GetTranslation("event-tooltip-remaining", "Remaining") + ": " + FormatTimespan(remaining);
			}
			description = description + " (" + _translationService.GetTranslation("event-tooltip-startsAt", "Starts at") + ": " + FormatAbsoluteTime(StartTime) + ")";
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
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
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
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
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
			spriteBatch.DrawString(text, font, nameRect, _getTextColor(), wrap: false, _getDrawShadowAction(), 1, _getShadowColor(), 1f, (HorizontalAlignment)0, (VerticalAlignment)1);
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
					spriteBatch.DrawString(remainingTimeString, font, timeRect, textColor, wrap: false, _getDrawShadowAction(), 1, _getShadowColor(), 1f, (HorizontalAlignment)0, (VerticalAlignment)1);
				}
			}
		}

		private TimeSpan GetTimeRemaining(DateTime now)
		{
			if (!(now <= StartTime) && !(now >= _endTime))
			{
				return StartTime.AddMinutes(Model.Duration) - now;
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
