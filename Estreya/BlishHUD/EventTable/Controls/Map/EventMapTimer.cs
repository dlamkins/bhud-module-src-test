using System;
using System.Globalization;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Estreya.BlishHUD.EventTable.Models;
using Estreya.BlishHUD.Shared.Controls.Map;
using Estreya.BlishHUD.Shared.Extensions;
using Estreya.BlishHUD.Shared.Services;
using Estreya.BlishHUD.Shared.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using NodaTime;

namespace Estreya.BlishHUD.EventTable.Controls.Map
{
	public class EventMapTimer : MapEntity
	{
		private readonly Estreya.BlishHUD.EventTable.Models.Event _ev;

		private readonly Estreya.BlishHUD.EventTable.Models.EventMapTimer _mapTimer;

		private readonly Color _color;

		private readonly float _thickness;

		private readonly Func<Instant> _getNow;

		private readonly TranslationService _translationService;

		private float X => _mapTimer.X;

		private float Y => _mapTimer.Y;

		private float Radius => _mapTimer.Radius * 0.041666668f;

		public EventMapTimer(Estreya.BlishHUD.EventTable.Models.Event ev, Estreya.BlishHUD.EventTable.Models.EventMapTimer mapTimer, Color color, Func<Instant> getNow, TranslationService translationService, float thickness = 1f)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			_ev = ev;
			_mapTimer = mapTimer;
			_color = color;
			_thickness = thickness;
			_getNow = getNow;
			_translationService = translationService;
		}

		public override RectangleF? RenderToMiniMap(SpriteBatch spriteBatch, Rectangle bounds, double offsetX, double offsetY, double scale, float opacity)
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_022b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0230: Unknown result type (might be due to invalid IL or missing references)
			//IL_0232: Unknown result type (might be due to invalid IL or missing references)
			//IL_0233: Unknown result type (might be due to invalid IL or missing references)
			//IL_023d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0243: Unknown result type (might be due to invalid IL or missing references)
			//IL_0248: Unknown result type (might be due to invalid IL or missing references)
			//IL_024d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0251: Unknown result type (might be due to invalid IL or missing references)
			//IL_0258: Unknown result type (might be due to invalid IL or missing references)
			//IL_0266: Unknown result type (might be due to invalid IL or missing references)
			//IL_0273: Unknown result type (might be due to invalid IL or missing references)
			//IL_0280: Unknown result type (might be due to invalid IL or missing references)
			//IL_0297: Unknown result type (might be due to invalid IL or missing references)
			//IL_0299: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
			Vector2 location = GetScaledLocation(X, Y, scale, offsetX, offsetY);
			float radius = Radius / (float)scale;
			CircleF circle = default(CircleF);
			((CircleF)(ref circle))._002Ector(new Point2(location.X, location.Y), radius);
			ShapeExtensions.DrawCircle(spriteBatch, circle, 50, _color * opacity, _thickness, 0f);
			Instant now = _getNow();
			Instant startTime = (from o in _ev.Occurences
				where o >= now || o.Plus(_ev.Duration) >= now
				select o into x
				orderby x
				select x).First();
			Instant endTime = startTime.Plus(_ev.Duration);
			int occurenceIndex = _ev.Occurences.IndexOf(startTime);
			Instant now2 = now;
			Instant startTime2 = startTime;
			Instant endTime2 = endTime;
			bool flag = (((uint)(occurenceIndex - -1) <= 1u) ? true : false);
			(Duration, Duration) remainingTime = GetTime(now2, startTime2, endTime2, flag ? null : new Instant?(_ev.Occurences[occurenceIndex - 1]));
			double degree = remainingTime.Item2.TotalSeconds.Remap(0.0, remainingTime.Item1.TotalSeconds, 0.0, 360.0) * -1.0;
			double angle = Math.PI * (degree - 90.0) / 180.0;
			_ = _thickness;
			Math.Cos(angle);
			_ = _thickness;
			Math.Sin(angle);
			if (!GameService.Gw2Mumble.get_UI().get_IsMapOpen() || GameService.Gw2Mumble.get_UI().get_MapScale() <= 1.0)
			{
				string text = _ev.Name + "\n" + GetEventDescription(now, startTime, endTime);
				BitmapFont font = GameService.Content.get_DefaultFont18();
				Size2 textSize = font.MeasureString(text);
				Point2 circleBottomCenter = circle.Center + new Vector2(0f, circle.Radius);
				RectangleF textLocation = default(RectangleF);
				((RectangleF)(ref textLocation))._002Ector(circleBottomCenter.X - textSize.Width / 2f, circleBottomCenter.Y + 10f, textSize.Width + 5f, textSize.Height + 5f);
				spriteBatch.DrawString(text, font, textLocation, Color.get_Red(), wrap: false, 1f, (HorizontalAlignment)1, (VerticalAlignment)0);
			}
			return ((CircleF)(ref circle)).ToRectangleF();
		}

		private (Duration maxTime, Duration calculatedTime) GetTime(Instant now, Instant startTime, Instant endTime, Instant? prevEndTime)
		{
			bool num = endTime < now;
			bool isNext = !num && startTime > now;
			bool isCurrent = !num && !isNext;
			if (num)
			{
				Duration finishedSince = now - endTime;
				return (endTime - startTime, finishedSince);
			}
			if (isNext)
			{
				Duration startsIn = startTime - now;
				return (prevEndTime.HasValue ? (startTime - prevEndTime.Value) : startsIn, startsIn);
			}
			if (isCurrent)
			{
				Duration remaining = GetTimeRemaining(now, startTime, endTime);
				return (_ev.Duration, remaining);
			}
			return (Duration.Zero, Duration.Zero);
		}

		private string GetEventDescription(Instant now, Instant startTime, Instant endTime)
		{
			bool num = endTime < now;
			bool isNext = !num && startTime > now;
			bool isCurrent = !num && !isNext;
			string description = "";
			if (num)
			{
				Duration finishedSince = now - endTime;
				description = description + _translationService.GetTranslation("event-tooltip-finishedSince", "Finished since") + ": " + FormatDuration(finishedSince);
			}
			else if (isNext)
			{
				Duration startsIn = startTime - now;
				description = description + _translationService.GetTranslation("event-tooltip-startsIn", "Starts in") + ": " + FormatDuration(startsIn);
			}
			else if (isCurrent)
			{
				Duration remaining = GetTimeRemaining(now, startTime, endTime);
				description = description + _translationService.GetTranslation("event-tooltip-remaining", "Remaining") + ": " + FormatDuration(remaining);
			}
			return description + " (" + _translationService.GetTranslation("event-tooltip-startsAt", "Starts at") + ": " + FormatAbsoluteTime(startTime) + ")";
		}

		private string FormatDuration(Duration ts)
		{
			(string, string, string) formatStrings = ("dd\\.hh\\:mm\\:ss", "hh\\:mm\\:ss", "mm\\:ss");
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
			catch (Exception)
			{
				return string.Empty;
			}
		}

		private string FormatAbsoluteTime(Instant dt)
		{
			try
			{
				return dt.InZone(DateTimeZoneProviders.Tzdb.GetSystemDefault()).ToString("HH\\:mm", CultureInfo.InvariantCulture);
			}
			catch (Exception)
			{
				return string.Empty;
			}
		}

		private Duration GetTimeRemaining(Instant now, Instant startTime, Instant endTime)
		{
			if (!(now <= startTime) && !(now >= endTime))
			{
				return startTime.Plus(_ev.Duration) - now;
			}
			return Duration.Zero;
		}
	}
}
