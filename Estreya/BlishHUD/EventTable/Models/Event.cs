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
using Estreya.BlishHUD.EventTable.Helpers;
using Estreya.BlishHUD.EventTable.Input;
using Estreya.BlishHUD.EventTable.Json;
using Estreya.BlishHUD.EventTable.UI.Views;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("offset")]
		[JsonConverter(typeof(TimeSpanJsonConverter), new object[]
		{
			"dd\\.hh\\:mm",
			new string[] { "hh\\:mm" }
		})]
		public TimeSpan Offset { get; set; }

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

		[JsonProperty("filler")]
		internal bool Filler { get; set; }

		[JsonProperty("api")]
		internal string APICode { get; set; }

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
				if (_contextMenuStrip == null)
				{
					_contextMenuStrip = new ContextMenuStrip();
					ContextMenuStripItem copyWaypoint = new ContextMenuStripItem();
					copyWaypoint.set_Text("Copy Waypoint");
					((Control)copyWaypoint).add_Click((EventHandler<MouseEventArgs>)delegate
					{
						CopyWaypoint();
					});
					_contextMenuStrip.AddMenuItem(copyWaypoint);
					ContextMenuStripItem openWiki = new ContextMenuStripItem();
					openWiki.set_Text("Open Wiki");
					((Control)openWiki).add_Click((EventHandler<MouseEventArgs>)delegate
					{
						OpenWiki();
					});
					_contextMenuStrip.AddMenuItem(openWiki);
					ContextMenuStripItem finishedEvent = new ContextMenuStripItem();
					finishedEvent.set_Text("Hide until Reset");
					((Control)finishedEvent).add_Click((EventHandler<MouseEventArgs>)delegate
					{
						Finish();
					});
					_contextMenuStrip.AddMenuItem(finishedEvent);
					ContextMenuStripItem disable = new ContextMenuStripItem();
					disable.set_Text("Disable");
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
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_015d: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0198: Unknown result type (might be due to invalid IL or missing references)
			//IL_019f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01de: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0213: Unknown result type (might be due to invalid IL or missing references)
			//IL_0228: Unknown result type (might be due to invalid IL or missing references)
			//IL_022a: Unknown result type (might be due to invalid IL or missing references)
			//IL_02af: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02da: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_030c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0313: Unknown result type (might be due to invalid IL or missing references)
			//IL_031b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0322: Unknown result type (might be due to invalid IL or missing references)
			//IL_0332: Unknown result type (might be due to invalid IL or missing references)
			//IL_0334: Unknown result type (might be due to invalid IL or missing references)
			//IL_036f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0371: Unknown result type (might be due to invalid IL or missing references)
			Rectangle eventTexturePosition = default(Rectangle);
			Rectangle eventTimeRemainingPosition = default(Rectangle);
			foreach (DateTime eventStart in startOccurences)
			{
				double width = GetWidth(eventStart, min, bounds, pixelPerMinute);
				int y = GetYPosition(allCategories, currentCategory, eventHeight, EventTableModule.ModuleInstance.Debug);
				double x = GetXPosition(eventStart, min, pixelPerMinute);
				x = Math.Max(x, 0.0);
				Color color = Color.get_Transparent();
				if (!Filler)
				{
					Color colorFromEvent = (string.IsNullOrWhiteSpace(Color) ? System.Drawing.Color.White : ColorTranslator.FromHtml(Color));
					((Color)(ref color))._002Ector((int)colorFromEvent.R, (int)colorFromEvent.G, (int)colorFromEvent.B);
				}
				((Rectangle)(ref eventTexturePosition))._002Ector((int)Math.Floor(x), y, (int)Math.Ceiling(width), eventHeight);
				bool drawBorder = !Filler && EventTableModule.ModuleInstance.ModuleSettings.DrawEventBorder.get_Value();
				DrawRectangle(spriteBatch, control, baseTexture, eventTexturePosition, color * EventTableModule.ModuleInstance.ModuleSettings.Opacity.get_Value(), drawBorder ? 1 : 0, Color.get_Black());
				Color textColor = Color.get_Black();
				textColor = ((!Filler) ? ((EventTableModule.ModuleInstance.ModuleSettings.TextColor.get_Value().get_Id() == 1) ? textColor : ColorExtensions.ToXnaColor(EventTableModule.ModuleInstance.ModuleSettings.TextColor.get_Value().get_Cloth())) : ((EventTableModule.ModuleInstance.ModuleSettings.FillerTextColor.get_Value().get_Id() == 1) ? textColor : ColorExtensions.ToXnaColor(EventTableModule.ModuleInstance.ModuleSettings.FillerTextColor.get_Value().get_Cloth())));
				Rectangle eventTextPosition = Rectangle.get_Empty();
				if (!string.IsNullOrWhiteSpace(Name) && (!Filler || (Filler && EventTableModule.ModuleInstance.ModuleSettings.UseFillerEventNames.get_Value())))
				{
					string eventName = GetLongestEventName(eventTexturePosition.Width, font);
					((Rectangle)(ref eventTextPosition))._002Ector(eventTexturePosition.X + 5, eventTexturePosition.Y + 5, (int)Math.Floor(MeasureStringWidth(eventName, font)), eventTexturePosition.Height - 10);
					SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, control, eventName, font, eventTextPosition, textColor, false, (HorizontalAlignment)0, (VerticalAlignment)1);
				}
				if (eventStart <= now && eventStart.AddMinutes(Duration) > now)
				{
					TimeSpan timeRemaining = eventStart.AddMinutes(Duration).Subtract(now);
					string timeRemainingString = ((timeRemaining.Hours > 0) ? timeRemaining.ToString("hh\\:mm\\:ss") : timeRemaining.ToString("mm\\:ss"));
					int timeRemainingWidth = (int)Math.Ceiling(MeasureStringWidth(timeRemainingString, font));
					int timeRemainingX = eventTexturePosition.X + (eventTexturePosition.Width / 2 - timeRemainingWidth / 2);
					if (timeRemainingX < eventTextPosition.X + eventTextPosition.Width)
					{
						timeRemainingX = eventTextPosition.X + eventTextPosition.Width + 10;
					}
					((Rectangle)(ref eventTimeRemainingPosition))._002Ector(timeRemainingX, eventTexturePosition.Y + 5, timeRemainingWidth, eventTexturePosition.Height - 10);
					if (eventTimeRemainingPosition.X + eventTimeRemainingPosition.Width <= eventTexturePosition.X + eventTexturePosition.Width)
					{
						SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, control, timeRemainingString, font, eventTimeRemainingPosition, textColor, false, (HorizontalAlignment)0, (VerticalAlignment)1);
					}
				}
				if (!Filler && !string.IsNullOrWhiteSpace(APICode) && EventTableModule.ModuleInstance.WorldbossState.IsCompleted(APICode))
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
				return FormatTimeSpan(timeRemaining);
			}
			return null;
		}

		private string FormatTimeSpan(TimeSpan ts)
		{
			if (ts.Hours <= 0)
			{
				return ts.ToString("mm\\:ss");
			}
			return ts.ToString("hh\\:mm\\:ss");
		}

		private string GetLongestEventName(int maxSize, BitmapFont font)
		{
			if (MeasureStringWidth(Name, font) <= (float)maxSize)
			{
				return Name;
			}
			for (int i = 0; i < Name.Length; i++)
			{
				string name = Name.Substring(0, Name.Length - i);
				if (MeasureStringWidth(name, font) <= (float)maxSize)
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

		private void DrawRectangle(SpriteBatch spriteBatch, Control control, Texture2D baseTexture, Rectangle coords, Color color)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, control, baseTexture, coords, color);
		}

		private void DrawLine(SpriteBatch spriteBatch, Control control, Texture2D baseTexture, Rectangle coords, Color color)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, control, baseTexture, coords, color);
		}

		private void DrawCrossOut(SpriteBatch spriteBatch, Control control, Texture2D baseTexture, Rectangle coords, Color color)
		{
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			Point topLeft = default(Point);
			((Point)(ref topLeft))._002Ector(((Rectangle)(ref coords)).get_Left(), ((Rectangle)(ref coords)).get_Top());
			Point topRight = default(Point);
			((Point)(ref topRight))._002Ector(((Rectangle)(ref coords)).get_Right(), ((Rectangle)(ref coords)).get_Top());
			Point bottomLeft = default(Point);
			((Point)(ref bottomLeft))._002Ector(((Rectangle)(ref coords)).get_Left(), ((Rectangle)(ref coords)).get_Bottom());
			Point bottomRight = default(Point);
			((Point)(ref bottomRight))._002Ector(((Rectangle)(ref coords)).get_Right(), ((Rectangle)(ref coords)).get_Bottom());
			DrawAngledLine(spriteBatch, control, baseTexture, topLeft, bottomRight, color);
			DrawAngledLine(spriteBatch, control, baseTexture, bottomLeft, topRight, color);
		}

		private void DrawAngledLine(SpriteBatch spriteBatch, Control control, Texture2D baseTexture, Point start, Point end, Color color)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			int length = (int)Math.Floor(MathHelper.CalculeDistance(start, end));
			Rectangle lineRectangle = default(Rectangle);
			((Rectangle)(ref lineRectangle))._002Ector(start.X, start.Y, length, 1);
			float angle = (float)MathHelper.CalculeAngle(start, end);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, control, baseTexture, lineRectangle, (Rectangle?)null, color, angle, new Vector2(0f, 0f), (SpriteEffects)0);
		}

		private void DrawRectangle(SpriteBatch spriteBatch, Control control, Texture2D baseTexture, Rectangle coords, Color color, int borderSize, Color borderColor)
		{
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			DrawRectangle(spriteBatch, control, baseTexture, coords, color);
			if (borderSize > 0 && borderColor != Color.get_Transparent())
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, control, baseTexture, new Rectangle(((Rectangle)(ref coords)).get_Left(), ((Rectangle)(ref coords)).get_Top(), coords.Width - borderSize, borderSize), borderColor);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, control, baseTexture, new Rectangle(((Rectangle)(ref coords)).get_Right() - borderSize, ((Rectangle)(ref coords)).get_Top(), borderSize, coords.Height), borderColor);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, control, baseTexture, new Rectangle(((Rectangle)(ref coords)).get_Left(), ((Rectangle)(ref coords)).get_Bottom() - borderSize, coords.Width, borderSize), borderColor);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, control, baseTexture, new Rectangle(((Rectangle)(ref coords)).get_Left(), ((Rectangle)(ref coords)).get_Top(), borderSize, coords.Height), borderColor);
			}
		}

		public void CopyWaypoint()
		{
			if (!string.IsNullOrWhiteSpace(Waypoint))
			{
				ClipboardUtil.get_WindowsClipboardService().SetTextAsync(Waypoint);
				ScreenNotification.ShowNotification("Waypoint copied to clipboard!", (NotificationType)0, (Texture2D)null, 4);
				ScreenNotification.ShowNotification(Name ?? "", (NotificationType)0, (Texture2D)null, 4);
			}
			else
			{
				ScreenNotification.ShowNotification("No Waypoint found!", (NotificationType)0, (Texture2D)null, 4);
				ScreenNotification.ShowNotification(Name ?? "", (NotificationType)0, (Texture2D)null, 4);
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
			if (addTimezoneOffset)
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
						if (((!e.Filler || !(category.Key == evc.Key)) && !(category.Key != evc.Key)) || (!e.Filler && !(e.Name != Name)))
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
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			double eventWidth = (double)Duration * pixelPerMinute;
			double x = GetXPosition(eventOccurence, min, pixelPerMinute);
			if (x < 0.0)
			{
				eventWidth -= Math.Abs(x);
			}
			return Math.Min(eventWidth, (double)bounds.Width - x);
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
				bool num = hoveredOccurence.AddMinutes(Duration) < EventTableModule.ModuleInstance.DateTimeNow;
				bool isNext = !num && hoveredOccurence > EventTableModule.ModuleInstance.DateTimeNow;
				bool isCurrent = !num && !isNext;
				if (num)
				{
					description = Location + ((!string.IsNullOrWhiteSpace(Location)) ? "\n" : string.Empty) + "\nFinished since: " + FormatTimeSpan(EventTableModule.ModuleInstance.DateTimeNow - hoveredOccurence.AddMinutes(Duration));
				}
				else if (isNext)
				{
					description = Location + ((!string.IsNullOrWhiteSpace(Location)) ? "\n" : string.Empty) + "\nStarts in: " + FormatTimeSpan(hoveredOccurence - EventTableModule.ModuleInstance.DateTimeNow);
				}
				else if (isCurrent)
				{
					description = Location + ((!string.IsNullOrWhiteSpace(Location)) ? "\n" : string.Empty) + "\nRemaining: " + FormatTimeSpan(hoveredOccurence.AddMinutes(Duration) - EventTableModule.ModuleInstance.DateTimeNow);
				}
			}
			else
			{
				Logger.Error("Can't find hovered event: " + Name + " - " + string.Join(", ", occurences.Select((DateTime o) => o.ToString())));
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

		private void Finish()
		{
			DateTime now = EventTableModule.ModuleInstance.DateTimeNow.ToUniversalTime();
			DateTime until = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0).AddDays(1.0);
			EventTableModule.ModuleInstance.HiddenState.Add(Name, until, isUTC: true);
		}

		private void Disable()
		{
			IEnumerable<SettingEntry<bool>> eventSetting = EventTableModule.ModuleInstance.ModuleSettings.AllEvents.Where((SettingEntry<bool> e) => ((SettingEntry)e).get_EntryKey() == Name);
			if (eventSetting.Any())
			{
				eventSetting.First().set_Value(false);
			}
		}

		public bool IsDisabled()
		{
			IEnumerable<SettingEntry<bool>> eventSetting = EventTableModule.ModuleInstance.ModuleSettings.AllEvents.Where((SettingEntry<bool> e) => ((SettingEntry)e).get_EntryKey() == Name);
			if (eventSetting.Any())
			{
				return !eventSetting.First().get_Value() || EventTableModule.ModuleInstance.HiddenState.IsHidden(Name);
			}
			return false;
		}
	}
}
