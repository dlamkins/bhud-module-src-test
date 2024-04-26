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

namespace Estreya.BlishHUD.EventTable.Controls.Map
{
	public class EventTimer : MapEntity
	{
		private readonly Estreya.BlishHUD.EventTable.Models.Event _ev;

		private readonly Color _color;

		private readonly float _thickness;

		private readonly Func<DateTime> _getNow;

		private readonly TranslationService _translationService;

		private float X => _ev.Locations.Map.X;

		private float Y => _ev.Locations.Map.Y;

		private float Radius => _ev.Locations.Map.Radius * 0.041666668f;

		public EventTimer(Estreya.BlishHUD.EventTable.Models.Event ev, Color color, Func<DateTime> getNow, TranslationService translationService, float thickness = 1f)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			_ev = ev;
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
			//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01de: Unknown result type (might be due to invalid IL or missing references)
			//IL_01df: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0208: Unknown result type (might be due to invalid IL or missing references)
			//IL_0209: Unknown result type (might be due to invalid IL or missing references)
			//IL_020e: Unknown result type (might be due to invalid IL or missing references)
			//IL_020f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0219: Unknown result type (might be due to invalid IL or missing references)
			//IL_0227: Unknown result type (might be due to invalid IL or missing references)
			//IL_022c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0231: Unknown result type (might be due to invalid IL or missing references)
			//IL_0292: Unknown result type (might be due to invalid IL or missing references)
			//IL_0297: Unknown result type (might be due to invalid IL or missing references)
			//IL_0299: Unknown result type (might be due to invalid IL or missing references)
			//IL_029a: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_02af: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_02da: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0308: Unknown result type (might be due to invalid IL or missing references)
			Vector2 location = GetScaledLocation(X, Y, scale, offsetX, offsetY);
			float radius = Radius / (float)scale;
			CircleF circle = default(CircleF);
			((CircleF)(ref circle))._002Ector(new Point2(location.X, location.Y), radius);
			ShapeExtensions.DrawCircle(spriteBatch, circle, 50, _color * opacity, _thickness, 0f);
			DateTime now = _getNow();
			DateTime startTime = (from o in _ev.Occurences
				where o >= now || o.AddMinutes(_ev.Duration) >= now
				select o into x
				orderby x
				select x).First();
			DateTime endTime = startTime.AddMinutes(_ev.Duration);
			int occurenceIndex = _ev.Occurences.IndexOf(startTime);
			DateTime now2 = now;
			DateTime startTime2 = startTime;
			DateTime endTime2 = endTime;
			bool flag = (((uint)(occurenceIndex - -1) <= 1u) ? true : false);
			(TimeSpan, TimeSpan) remainingTime = GetTime(now2, startTime2, endTime2, flag ? null : new DateTime?(_ev.Occurences[occurenceIndex - 1]));
			double degree = remainingTime.Item2.TotalSeconds.Remap(0.0, remainingTime.Item1.TotalSeconds, 0.0, 360.0) * -1.0;
			double angle = Math.PI * (degree - 90.0) / 180.0;
			float angleX = (radius - _thickness) * (float)Math.Cos(angle);
			float angleY = (radius - _thickness) * (float)Math.Sin(angle);
			int angleLineThickness = 3;
			spriteBatch.DrawAngledLine(Textures.get_Pixel(), circle.Center, circle.Center + new Vector2(angleX, angleY), Color.get_Red(), angleLineThickness);
			int topLineThickness = 3;
			spriteBatch.DrawAngledLine(Textures.get_Pixel(), circle.Center, circle.Center + new Vector2(0f, 0f - (circle.Radius - _thickness)), Color.get_Gray(), topLineThickness);
			if (GameService.Gw2Mumble.get_UI().get_MapScale() <= 0.800000011920929)
			{
				string text = _ev.Name + ": " + GetEventDescription(now, startTime, endTime);
				BitmapFont font = GameService.Content.get_DefaultFont18();
				Size2 textSize = font.MeasureString(text);
				Point2 circleBottomCenter = circle.Center + new Vector2(0f, circle.Radius);
				RectangleF textLocation = default(RectangleF);
				((RectangleF)(ref textLocation))._002Ector(circleBottomCenter.X - textSize.Width / 2f, circleBottomCenter.Y + 5f, textSize.Width, textSize.Height);
				spriteBatch.DrawString(text, font, textLocation, Color.get_Red(), wrap: false, 1f, (HorizontalAlignment)0, (VerticalAlignment)1);
			}
			return ((CircleF)(ref circle)).ToRectangleF();
		}

		private (TimeSpan maxTime, TimeSpan calculatedTime) GetTime(DateTime now, DateTime startTime, DateTime endTime, DateTime? prevEndTime)
		{
			bool num = endTime < now;
			bool isNext = !num && startTime > now;
			bool isCurrent = !num && !isNext;
			if (num)
			{
				TimeSpan finishedSince = now - endTime;
				return (endTime - startTime, finishedSince);
			}
			if (isNext)
			{
				TimeSpan startsIn = startTime - now;
				return (prevEndTime.HasValue ? (startTime - prevEndTime.Value) : startsIn, startsIn);
			}
			if (isCurrent)
			{
				TimeSpan remaining = GetTimeRemaining(now, startTime, endTime);
				return (TimeSpan.FromMinutes(_ev.Duration), remaining);
			}
			return (TimeSpan.Zero, TimeSpan.Zero);
		}

		private string GetEventDescription(DateTime now, DateTime startTime, DateTime endTime)
		{
			bool num = endTime < now;
			bool isNext = !num && startTime > now;
			bool isCurrent = !num && !isNext;
			string description = _ev.Locations.Tooltip + "\n";
			if (num)
			{
				TimeSpan finishedSince = now - endTime;
				description = description + _translationService.GetTranslation("event-tooltip-finishedSince", "Finished since") + ": " + FormatTimespan(finishedSince);
			}
			else if (isNext)
			{
				TimeSpan startsIn = startTime - now;
				description = description + _translationService.GetTranslation("event-tooltip-startsIn", "Starts in") + ": " + FormatTimespan(startsIn);
			}
			else if (isCurrent)
			{
				TimeSpan remaining = GetTimeRemaining(now, startTime, endTime);
				description = description + _translationService.GetTranslation("event-tooltip-remaining", "Remaining") + ": " + FormatTimespan(remaining);
			}
			return description + " (" + _translationService.GetTranslation("event-tooltip-startsAt", "Starts at") + ": " + FormatAbsoluteTime(startTime) + ")";
		}

		private string FormatTimespan(TimeSpan ts)
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

		private string FormatAbsoluteTime(DateTime dt)
		{
			try
			{
				return dt.ToLocalTime().ToString("HH\\:mm", CultureInfo.InvariantCulture);
			}
			catch (Exception)
			{
				return string.Empty;
			}
		}

		private TimeSpan GetTimeRemaining(DateTime now, DateTime startTime, DateTime endTime)
		{
			if (!(now <= startTime) && !(now >= endTime))
			{
				return startTime.AddMinutes(_ev.Duration) - now;
			}
			return TimeSpan.Zero;
		}
	}
}
