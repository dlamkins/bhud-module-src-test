using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Common.UI.Views;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Blish_HUD._Extensions;
using Estreya.BlishHUD.EventTable.Controls;
using Estreya.BlishHUD.EventTable.Helpers;
using Estreya.BlishHUD.EventTable.Input;
using Estreya.BlishHUD.EventTable.Json;
using Estreya.BlishHUD.EventTable.Resources;
using Estreya.BlishHUD.EventTable.UI.Views;
using Estreya.BlishHUD.EventTable.Utils;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using Newtonsoft.Json;

namespace Estreya.BlishHUD.EventTable.Models
{
	[Serializable]
	public class Event
	{
		private static readonly Logger Logger = Logger.GetLogger<Event>();

		[JsonIgnore]
		private Tooltip _tooltip;

		[JsonIgnore]
		private ContextMenuStrip _contextMenuStrip;

		[JsonProperty("key")]
		public string Key { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("offset")]
		[JsonConverter(typeof(TimeSpanJsonConverter), new object[]
		{
			"dd\\.hh\\:mm",
			new string[] { "hh\\:mm" }
		})]
		public TimeSpan Offset { get; set; }

		[JsonProperty("convertOffset")]
		public bool ConvertOffset { get; set; } = true;


		[JsonProperty("repeat")]
		[JsonConverter(typeof(TimeSpanJsonConverter), new object[]
		{
			"dd\\.hh\\:mm",
			new string[] { "dd\\.hh\\:mm", "hh\\:mm" }
		})]
		public TimeSpan Repeat { get; set; }

		[JsonProperty("diffculty")]
		public EventDifficulty Difficulty { get; set; }

		[JsonProperty("location")]
		public string Location { get; set; }

		[JsonProperty("waypoint")]
		public string Waypoint { get; set; }

		[JsonProperty("wiki")]
		public string Wiki { get; set; }

		[JsonProperty("duration")]
		public int Duration { get; set; }

		[JsonProperty("icon")]
		public string Icon { get; set; }

		[JsonProperty("color")]
		public string Color { get; set; }

		[JsonProperty("api")]
		public string APICode { get; set; }

		internal bool Filler { get; set; }

		internal EventCategory EventCategory { get; set; }

		[JsonIgnore]
		private Tooltip Tooltip
		{
			get
			{
				//IL_0029: Unknown result type (might be due to invalid IL or missing references)
				//IL_0033: Expected O, but got Unknown
				if (_tooltip == null)
				{
					_tooltip = new Tooltip((ITooltipView)(object)new TooltipView(Name, Location ?? "", Icon));
				}
				return _tooltip;
			}
		}

		[JsonIgnore]
		private ContextMenuStrip ContextMenuStrip
		{
			get
			{
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0016: Expected O, but got Unknown
				//IL_0016: Unknown result type (might be due to invalid IL or missing references)
				//IL_001c: Expected O, but got Unknown
				//IL_0046: Unknown result type (might be due to invalid IL or missing references)
				//IL_004c: Expected O, but got Unknown
				//IL_0076: Unknown result type (might be due to invalid IL or missing references)
				//IL_007c: Expected O, but got Unknown
				//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ac: Expected O, but got Unknown
				//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
				//IL_00dd: Expected O, but got Unknown
				if (_contextMenuStrip == null)
				{
					_contextMenuStrip = new ContextMenuStrip();
					ContextMenuStripItem copyWaypoint = new ContextMenuStripItem();
					copyWaypoint.set_Text(Strings.Event_CopyWaypoint);
					((Control)copyWaypoint).add_Click((EventHandler<MouseEventArgs>)delegate
					{
						CopyWaypoint();
					});
					_contextMenuStrip.AddMenuItem(copyWaypoint);
					ContextMenuStripItem openWiki = new ContextMenuStripItem();
					openWiki.set_Text(Strings.Event_OpenWiki);
					((Control)openWiki).add_Click((EventHandler<MouseEventArgs>)delegate
					{
						OpenWiki();
					});
					_contextMenuStrip.AddMenuItem(openWiki);
					ContextMenuStripItem hideCategory = new ContextMenuStripItem();
					hideCategory.set_Text(Strings.Event_HideCategory);
					((Control)hideCategory).add_Click((EventHandler<MouseEventArgs>)delegate
					{
						FinishCategory();
					});
					_contextMenuStrip.AddMenuItem(hideCategory);
					ContextMenuStripItem hideEvent = new ContextMenuStripItem();
					hideEvent.set_Text(Strings.Event_HideEvent);
					((Control)hideEvent).add_Click((EventHandler<MouseEventArgs>)delegate
					{
						Finish();
					});
					_contextMenuStrip.AddMenuItem(hideEvent);
					ContextMenuStripItem disable = new ContextMenuStripItem();
					disable.set_Text(Strings.Event_Disable);
					((Control)disable).add_Click((EventHandler<MouseEventArgs>)delegate
					{
						Disable();
					});
					_contextMenuStrip.AddMenuItem(disable);
				}
				return _contextMenuStrip;
			}
		}

		public void Draw(SpriteBatch spriteBatch, Rectangle bounds, Control control, Texture2D baseTexture, List<EventCategory> allCategories, EventCategory currentCategory, double pixelPerMinute, int eventHeight, DateTime now, DateTime min, DateTime max, BitmapFont font, List<DateTime> startOccurences)
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_017d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0182: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_0227: Unknown result type (might be due to invalid IL or missing references)
			//IL_0245: Unknown result type (might be due to invalid IL or missing references)
			//IL_0252: Unknown result type (might be due to invalid IL or missing references)
			//IL_0261: Unknown result type (might be due to invalid IL or missing references)
			//IL_0279: Unknown result type (might be due to invalid IL or missing references)
			//IL_027b: Unknown result type (might be due to invalid IL or missing references)
			//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0306: Unknown result type (might be due to invalid IL or missing references)
			//IL_0310: Unknown result type (might be due to invalid IL or missing references)
			//IL_0317: Unknown result type (might be due to invalid IL or missing references)
			//IL_032b: Unknown result type (might be due to invalid IL or missing references)
			//IL_033a: Unknown result type (might be due to invalid IL or missing references)
			//IL_034c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0353: Unknown result type (might be due to invalid IL or missing references)
			//IL_035b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0362: Unknown result type (might be due to invalid IL or missing references)
			//IL_0372: Unknown result type (might be due to invalid IL or missing references)
			//IL_0374: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c7: Unknown result type (might be due to invalid IL or missing references)
			RectangleF eventTexturePosition = default(RectangleF);
			RectangleF eventTimeRemainingPosition = default(RectangleF);
			foreach (DateTime eventStart in startOccurences)
			{
				float width = (float)GetWidth(eventStart, min, bounds, pixelPerMinute);
				if (width <= 0f)
				{
					continue;
				}
				int y = GetYPosition(allCategories, currentCategory, eventHeight, EventTableModule.ModuleInstance.Debug);
				float x = (float)GetXPosition(eventStart, min, pixelPerMinute);
				x = Math.Max(x, 0f);
				Color color = Color.get_Transparent();
				if (!Filler)
				{
					Color colorFromEvent = (string.IsNullOrWhiteSpace(Color) ? System.Drawing.Color.White : ColorTranslator.FromHtml(Color));
					((Color)(ref color))._002Ector((int)colorFromEvent.R, (int)colorFromEvent.G, (int)colorFromEvent.B);
				}
				((RectangleF)(ref eventTexturePosition))._002Ector(x, (float)y, width, (float)eventHeight);
				bool drawBorder = !Filler && EventTableModule.ModuleInstance.ModuleSettings.DrawEventBorder.get_Value();
				DrawRectangle(spriteBatch, control, baseTexture, eventTexturePosition, color * EventTableModule.ModuleInstance.ModuleSettings.Opacity.get_Value(), drawBorder ? 1 : 0, Color.get_Black());
				Color textColor = Color.get_Black();
				if (Filler)
				{
					if (EventTableModule.ModuleInstance.ModuleSettings.FillerTextColor.get_Value() != null)
					{
						Color value = EventTableModule.ModuleInstance.ModuleSettings.FillerTextColor.get_Value();
						if (value == null || value.get_Id() != 1)
						{
							textColor = ColorExtensions.ToXnaColor(EventTableModule.ModuleInstance.ModuleSettings.FillerTextColor.get_Value().get_Cloth());
						}
					}
				}
				else if (EventTableModule.ModuleInstance.ModuleSettings.TextColor.get_Value() != null)
				{
					Color value2 = EventTableModule.ModuleInstance.ModuleSettings.TextColor.get_Value();
					if (value2 == null || value2.get_Id() != 1)
					{
						textColor = ColorExtensions.ToXnaColor(EventTableModule.ModuleInstance.ModuleSettings.TextColor.get_Value().get_Cloth());
					}
				}
				RectangleF eventTextPosition = RectangleF.op_Implicit(Rectangle.get_Empty());
				if (!string.IsNullOrWhiteSpace(Name) && (!Filler || (Filler && EventTableModule.ModuleInstance.ModuleSettings.UseFillerEventNames.get_Value())))
				{
					string eventName = GetLongestEventName(eventTexturePosition.Width, font);
					float eventTextWidth = MeasureStringWidth(eventName, font);
					((RectangleF)(ref eventTextPosition))._002Ector(eventTexturePosition.X + 5f, eventTexturePosition.Y + 5f, eventTextWidth, eventTexturePosition.Height - 10f);
					spriteBatch.DrawStringOnCtrl(control, eventName, font, eventTextPosition, textColor, wrap: false, (HorizontalAlignment)0, (VerticalAlignment)1);
				}
				if (eventStart <= now && eventStart.AddMinutes(Duration) > now)
				{
					TimeSpan timeRemaining = eventStart.AddMinutes(Duration).Subtract(now);
					string timeRemainingString = FormatTime(timeRemaining);
					float timeRemainingWidth = MeasureStringWidth(timeRemainingString, font);
					float timeRemainingX = eventTexturePosition.X + (eventTexturePosition.Width / 2f - timeRemainingWidth / 2f);
					if (timeRemainingX < eventTextPosition.X + eventTextPosition.Width)
					{
						timeRemainingX = eventTextPosition.X + eventTextPosition.Width + 10f;
					}
					((RectangleF)(ref eventTimeRemainingPosition))._002Ector(timeRemainingX, eventTexturePosition.Y + 5f, timeRemainingWidth, eventTexturePosition.Height - 10f);
					if (eventTimeRemainingPosition.X + eventTimeRemainingPosition.Width <= eventTexturePosition.X + eventTexturePosition.Width)
					{
						spriteBatch.DrawStringOnCtrl(control, timeRemainingString, font, eventTimeRemainingPosition, textColor, wrap: false, (HorizontalAlignment)0, (VerticalAlignment)1);
					}
				}
				if (EventTableModule.ModuleInstance.ModuleSettings.WorldbossCompletedAcion.get_Value() == WorldbossCompletedAction.Crossout && !Filler && !string.IsNullOrWhiteSpace(APICode) && EventTableModule.ModuleInstance.WorldbossState.IsCompleted(APICode))
				{
					DrawCrossOut(spriteBatch, control, baseTexture, eventTexturePosition, Color.get_Red());
				}
			}
		}

		private void UpdateTooltip(string description)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Expected O, but got Unknown
			_tooltip = new Tooltip((ITooltipView)(object)new TooltipView(Name, description, Icon));
		}

		private string GetTimeRemaining(DateTime now, DateTime max, DateTime min)
		{
			IEnumerable<DateTime> filteredStartOccurences = from so in GetStartOccurences(now, max, min)
				where so <= now && so.AddMinutes(Duration) > now
				select so;
			if (filteredStartOccurences.Any())
			{
				TimeSpan timeRemaining = filteredStartOccurences.First().AddMinutes(Duration).Subtract(now);
				return FormatTime(timeRemaining);
			}
			return null;
		}

		private string FormatTime(TimeSpan ts)
		{
			if (ts.Hours <= 0)
			{
				return ts.ToString("mm\\:ss");
			}
			return ts.ToString("hh\\:mm\\:ss");
		}

		private string FormatTime(DateTime dateTime)
		{
			if (dateTime.Hour <= 0)
			{
				return dateTime.ToString("mm:ss");
			}
			return dateTime.ToString("HH:mm:ss");
		}

		private string GetLongestEventName(float maxSize, BitmapFont font)
		{
			if (MeasureStringWidth(Name, font) <= maxSize)
			{
				return Name;
			}
			for (int i = 0; i < Name.Length; i++)
			{
				string name = Name.Substring(0, Name.Length - i);
				if (MeasureStringWidth(name, font) <= maxSize)
				{
					return name;
				}
			}
			return "...";
		}

		private float MeasureStringWidth(string text, BitmapFont font)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			if (string.IsNullOrEmpty(text))
			{
				return 0f;
			}
			return font.MeasureString(text).Width + 10f;
		}

		private void DrawRectangle(SpriteBatch spriteBatch, Control control, Texture2D baseTexture, RectangleF coords, Color color)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			spriteBatch.DrawOnCtrl(control, baseTexture, coords, color);
		}

		private void DrawLine(SpriteBatch spriteBatch, Control control, Texture2D baseTexture, Rectangle coords, Color color)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, control, baseTexture, coords, color);
		}

		private void DrawCrossOut(SpriteBatch spriteBatch, Control control, Texture2D baseTexture, RectangleF coords, Color color)
		{
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			Point2 topLeft = default(Point2);
			((Point2)(ref topLeft))._002Ector(((RectangleF)(ref coords)).get_Left(), ((RectangleF)(ref coords)).get_Top());
			Point2 topRight = default(Point2);
			((Point2)(ref topRight))._002Ector(((RectangleF)(ref coords)).get_Right(), ((RectangleF)(ref coords)).get_Top());
			Point2 bottomLeft = default(Point2);
			((Point2)(ref bottomLeft))._002Ector(((RectangleF)(ref coords)).get_Left(), ((RectangleF)(ref coords)).get_Bottom());
			Point2 bottomRight = default(Point2);
			((Point2)(ref bottomRight))._002Ector(((RectangleF)(ref coords)).get_Right(), ((RectangleF)(ref coords)).get_Bottom());
			DrawAngledLine(spriteBatch, control, baseTexture, topLeft, bottomRight, color);
			DrawAngledLine(spriteBatch, control, baseTexture, bottomLeft, topRight, color);
		}

		private void DrawAngledLine(SpriteBatch spriteBatch, Control control, Texture2D baseTexture, Point2 start, Point2 end, Color color)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			float length = MathHelper.CalculeDistance(start, end);
			RectangleF lineRectangle = default(RectangleF);
			((RectangleF)(ref lineRectangle))._002Ector(start.X, start.Y, length, 1f);
			float angle = MathHelper.CalculeAngle(start, end);
			spriteBatch.DrawOnCtrl(control, baseTexture, lineRectangle, color, angle);
		}

		private void DrawRectangle(SpriteBatch spriteBatch, Control control, Texture2D baseTexture, RectangleF coords, Color color, int borderSize, Color borderColor)
		{
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			DrawRectangle(spriteBatch, control, baseTexture, coords, color);
			if (borderSize > 0 && borderColor != Color.get_Transparent())
			{
				DrawRectangle(spriteBatch, control, baseTexture, new RectangleF(((RectangleF)(ref coords)).get_Left(), ((RectangleF)(ref coords)).get_Top(), coords.Width - (float)borderSize, (float)borderSize), borderColor);
				DrawRectangle(spriteBatch, control, baseTexture, new RectangleF(((RectangleF)(ref coords)).get_Right() - (float)borderSize, ((RectangleF)(ref coords)).get_Top(), (float)borderSize, coords.Height), borderColor);
				DrawRectangle(spriteBatch, control, baseTexture, new RectangleF(((RectangleF)(ref coords)).get_Left(), ((RectangleF)(ref coords)).get_Bottom() - (float)borderSize, coords.Width, (float)borderSize), borderColor);
				DrawRectangle(spriteBatch, control, baseTexture, new RectangleF(((RectangleF)(ref coords)).get_Left(), ((RectangleF)(ref coords)).get_Top(), (float)borderSize, coords.Height), borderColor);
			}
		}

		public void CopyWaypoint()
		{
			if (!string.IsNullOrWhiteSpace(Waypoint))
			{
				ClipboardUtil.get_WindowsClipboardService().SetTextAsync(Waypoint);
				ScreenNotification.ShowNotification(new string[2]
				{
					Name ?? "",
					Strings.Event_WaypointCopied
				}, (NotificationType)0);
			}
			else
			{
				ScreenNotification.ShowNotification(new string[2]
				{
					Name ?? "",
					Strings.Event_NoWaypointFound
				}, (NotificationType)0);
			}
		}

		public void OpenWiki()
		{
			if (!string.IsNullOrWhiteSpace(Wiki))
			{
				Process.Start(Wiki);
			}
		}

		public List<DateTime> GetStartOccurences(DateTime now, DateTime max, DateTime min, bool addTimezoneOffset = true, bool limitsBetweenRanges = false)
		{
			List<DateTime> startOccurences = new List<DateTime>();
			if (IsDisabled())
			{
				return startOccurences;
			}
			DateTime zero = new DateTime(min.Year, min.Month, min.Day, 0, 0, 0).AddDays((Repeat.TotalMinutes != 0.0) ? (-1) : 0);
			TimeSpan offset = Offset;
			if (ConvertOffset && addTimezoneOffset)
			{
				offset = offset.Add(TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now));
			}
			DateTime eventStart = zero.Add(offset);
			while (eventStart < max)
			{
				bool startAfterMin = eventStart > min;
				bool endAfterMin = eventStart.AddMinutes(Duration) > min;
				bool endBeforeMax = eventStart.AddMinutes(Duration) < max;
				if ((limitsBetweenRanges ? (startAfterMin && endBeforeMax) : (startAfterMin || endAfterMin)) && eventStart < max)
				{
					startOccurences.Add(eventStart);
				}
				eventStart = ((Repeat.TotalMinutes != 0.0) ? eventStart.Add(Repeat) : eventStart.Add(TimeSpan.FromDays(1.0)));
			}
			return startOccurences;
		}

		public double GetXPosition(DateTime start, DateTime min, double pixelPerMinute)
		{
			return start.Subtract(min).TotalMinutes * pixelPerMinute;
		}

		private int GetMinYPosition(IEnumerable<EventCategory> eventCategories, int eventHight, bool debugEnabled)
		{
			int minY = 0;
			if (debugEnabled)
			{
				foreach (EventCategory eventCategory in eventCategories)
				{
					foreach (Event e in eventCategory.Events)
					{
						minY += eventHight;
						if (this == e)
						{
							return minY;
						}
					}
				}
				return minY;
			}
			return minY;
		}

		public int GetYPosition(IEnumerable<EventCategory> eventCategories, EventCategory evc, int eventHeight, bool debugEnabled)
		{
			int y = GetMinYPosition(eventCategories, eventHeight, debugEnabled);
			foreach (EventCategory category in eventCategories)
			{
				bool anyFromCategoryRendered = false;
				foreach (Event e in category.Events)
				{
					if (!e.IsDisabled())
					{
						anyFromCategoryRendered = true;
						if (((!e.Filler || !(category.Key == evc.Key)) && !(category.Key != evc.Key)) || (!e.Filler && !(e.GetSettingName() != GetSettingName())))
						{
							return y;
						}
					}
				}
				if (anyFromCategoryRendered)
				{
					y += eventHeight;
				}
			}
			return y;
		}

		public double GetWidth(DateTime eventOccurence, DateTime min, Rectangle bounds, double pixelPerMinute)
		{
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			double eventWidth = (double)Duration * pixelPerMinute;
			double x = GetXPosition(eventOccurence, min, pixelPerMinute);
			if (x < 0.0)
			{
				eventWidth -= Math.Abs(x);
			}
			if (((x > 0.0) ? x : 0.0) + eventWidth > (double)bounds.Width)
			{
				eventWidth = (double)bounds.Width - ((x > 0.0) ? x : 0.0);
			}
			return eventWidth;
		}

		public bool IsHovered(IEnumerable<EventCategory> eventCategories, EventCategory eventCategory, DateTime now, DateTime max, DateTime min, Rectangle bounds, Point relativeMousePosition, double pixelPerMinute, int eventHeight, bool debugEnabled)
		{
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			foreach (DateTime occurence in GetStartOccurences(now, max, min))
			{
				double x = GetXPosition(occurence, min, pixelPerMinute);
				int eo_y = GetYPosition(eventCategories, eventCategory, eventHeight, debugEnabled);
				double width = GetWidth(occurence, min, bounds, pixelPerMinute);
				x = Math.Max(x, 0.0);
				if ((double)relativeMousePosition.X >= x && (double)relativeMousePosition.X < x + width && relativeMousePosition.Y >= eo_y && relativeMousePosition.Y < eo_y + eventHeight)
				{
					return true;
				}
			}
			return false;
		}

		public void HandleClick(object sender, MouseEventArgs e)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Invalid comparison between Unknown and I4
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Invalid comparison between Unknown and I4
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			if ((int)e.get_EventType() == 513)
			{
				if (EventTableModule.ModuleInstance.ModuleSettings.CopyWaypointOnClick.get_Value())
				{
					CopyWaypoint();
				}
			}
			else if ((int)e.get_EventType() == 516 && EventTableModule.ModuleInstance.ModuleSettings.ShowContextMenuOnClick.get_Value())
			{
				int topPos = ((e.get_MousePosition().Y + ((Control)ContextMenuStrip).get_Height() > ((Control)GameService.Graphics.get_SpriteScreen()).get_Height()) ? (-((Control)ContextMenuStrip).get_Height()) : 0);
				int leftPos = ((e.get_MousePosition().X + ((Control)ContextMenuStrip).get_Width() >= ((Control)GameService.Graphics.get_SpriteScreen()).get_Width()) ? (-((Control)ContextMenuStrip).get_Width()) : 0);
				Point menuPosition = e.get_MousePosition() + new Point(leftPos, topPos);
				ContextMenuStrip.Show(menuPosition);
			}
		}

		public void HandleHover(object sender, MouseEventArgs e, double pixelPerMinute)
		{
			List<DateTime> occurences = GetStartOccurences(EventTableModule.ModuleInstance.DateTimeNow, EventTableModule.ModuleInstance.EventTimeMax, EventTableModule.ModuleInstance.EventTimeMin);
			IEnumerable<DateTime> hoveredOccurences = occurences.Where(delegate(DateTime eo)
			{
				//IL_0039: Unknown result type (might be due to invalid IL or missing references)
				//IL_004d: Unknown result type (might be due to invalid IL or missing references)
				double xPosition = GetXPosition(eo, EventTableModule.ModuleInstance.EventTimeMin, pixelPerMinute);
				double num2 = xPosition + (double)Duration * pixelPerMinute;
				return (double)e.Position.X > xPosition && (double)e.Position.X < num2;
			});
			if (((Control)Tooltip).get_Visible())
			{
				return;
			}
			string description = Location ?? "";
			if (hoveredOccurences.Any())
			{
				DateTime hoveredOccurence = hoveredOccurences.First();
				if (EventTableModule.ModuleInstance.ModuleSettings.TooltipTimeMode.get_Value() == TooltipTimeMode.Relative)
				{
					bool num = hoveredOccurence.AddMinutes(Duration) < EventTableModule.ModuleInstance.DateTimeNow;
					bool isNext = !num && hoveredOccurence > EventTableModule.ModuleInstance.DateTimeNow;
					bool isCurrent = !num && !isNext;
					description = Location + ((!string.IsNullOrWhiteSpace(Location)) ? "\n" : string.Empty) + "\n";
					if (num)
					{
						description = description + Strings.Event_Tooltip_FinishedSince + ": " + FormatTime(EventTableModule.ModuleInstance.DateTimeNow - hoveredOccurence.AddMinutes(Duration));
					}
					else if (isNext)
					{
						description = description + Strings.Event_Tooltip_StartsIn + ": " + FormatTime(hoveredOccurence - EventTableModule.ModuleInstance.DateTimeNow);
					}
					else if (isCurrent)
					{
						description = description + Strings.Event_Tooltip_Remaining + ": " + FormatTime(hoveredOccurence.AddMinutes(Duration) - EventTableModule.ModuleInstance.DateTimeNow);
					}
				}
				else
				{
					description = description + Location + ((!string.IsNullOrWhiteSpace(Location)) ? "\n" : string.Empty) + "\n" + Strings.Event_Tooltip_StartsAt + ": " + FormatTime(hoveredOccurence);
				}
			}
			else
			{
				Logger.Warn("Can't find hovered event: " + Name + " - " + string.Join(", ", occurences.Select((DateTime o) => o.ToString())));
			}
			UpdateTooltip(description);
			Tooltip.Show(0, 0);
		}

		public void HandleNonHover(object sender, MouseEventArgs e)
		{
			if (((Control)Tooltip).get_Visible())
			{
				((Control)Tooltip).Hide();
			}
		}

		public void Finish()
		{
			DateTime now = EventTableModule.ModuleInstance.DateTimeNow.ToUniversalTime();
			DateTime until = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0).AddDays(1.0);
			EventTableModule.ModuleInstance.HiddenState.Add(GetSettingName(), until, isUTC: true);
		}

		public void FinishCategory()
		{
			EventCategory.Finish();
		}

		public void Disable()
		{
			IEnumerable<SettingEntry<bool>> eventSetting = EventTableModule.ModuleInstance.ModuleSettings.AllEvents.Where((SettingEntry<bool> e) => ((SettingEntry)e).get_EntryKey().ToLowerInvariant() == GetSettingName().ToLowerInvariant());
			if (eventSetting.Any())
			{
				eventSetting.First().set_Value(false);
			}
		}

		public bool IsDisabled()
		{
			IEnumerable<SettingEntry<bool>> eventSetting = EventTableModule.ModuleInstance.ModuleSettings.AllEvents.Where((SettingEntry<bool> e) => ((SettingEntry)e).get_EntryKey().ToLowerInvariant() == GetSettingName().ToLowerInvariant());
			if (eventSetting.Any())
			{
				return !eventSetting.First().get_Value() || EventTableModule.ModuleInstance.HiddenState.IsHidden(GetSettingName());
			}
			return false;
		}

		public string GetSettingName()
		{
			return EventCategory.Key + "-" + Key;
		}
	}
}
