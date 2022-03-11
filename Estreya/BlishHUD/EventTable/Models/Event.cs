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

		private readonly TimeSpan updateInterval = TimeSpan.FromMinutes(15.0);

		private double timeSinceUpdate;

		[JsonIgnore]
		private Tooltip _tooltip;

		[JsonIgnore]
		private ContextMenuStrip _contextMenuStrip;

		[JsonIgnore]
		private string _settingKey;

		[JsonIgnore]
		private Color? _backgroundColor;

		[JsonIgnore]
		private int _lastYPosition;

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
		public string BackgroundColorCode { get; set; }

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
		public string SettingKey
		{
			get
			{
				if (_settingKey == null)
				{
					_settingKey = EventCategory.Key + "-" + (Key ?? Name);
				}
				return _settingKey;
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
				//IL_001b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0027: Expected O, but got Unknown
				//IL_0046: Unknown result type (might be due to invalid IL or missing references)
				//IL_004b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0057: Expected O, but got Unknown
				//IL_0076: Unknown result type (might be due to invalid IL or missing references)
				//IL_007b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0087: Expected O, but got Unknown
				//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b7: Expected O, but got Unknown
				//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
				//IL_00db: Unknown result type (might be due to invalid IL or missing references)
				//IL_00e8: Expected O, but got Unknown
				if (_contextMenuStrip == null)
				{
					_contextMenuStrip = new ContextMenuStrip();
					ContextMenuStripItem val = new ContextMenuStripItem();
					val.set_Text(Strings.Event_CopyWaypoint);
					ContextMenuStripItem copyWaypoint = val;
					((Control)copyWaypoint).add_Click((EventHandler<MouseEventArgs>)delegate
					{
						CopyWaypoint();
					});
					_contextMenuStrip.AddMenuItem(copyWaypoint);
					ContextMenuStripItem val2 = new ContextMenuStripItem();
					val2.set_Text(Strings.Event_OpenWiki);
					ContextMenuStripItem openWiki = val2;
					((Control)openWiki).add_Click((EventHandler<MouseEventArgs>)delegate
					{
						OpenWiki();
					});
					_contextMenuStrip.AddMenuItem(openWiki);
					ContextMenuStripItem val3 = new ContextMenuStripItem();
					val3.set_Text(Strings.Event_HideCategory);
					ContextMenuStripItem hideCategory = val3;
					((Control)hideCategory).add_Click((EventHandler<MouseEventArgs>)delegate
					{
						FinishCategory();
					});
					_contextMenuStrip.AddMenuItem(hideCategory);
					ContextMenuStripItem val4 = new ContextMenuStripItem();
					val4.set_Text(Strings.Event_HideEvent);
					ContextMenuStripItem hideEvent = val4;
					((Control)hideEvent).add_Click((EventHandler<MouseEventArgs>)delegate
					{
						Finish();
					});
					_contextMenuStrip.AddMenuItem(hideEvent);
					ContextMenuStripItem val5 = new ContextMenuStripItem();
					val5.set_Text(Strings.Event_Disable);
					ContextMenuStripItem disable = val5;
					((Control)disable).add_Click((EventHandler<MouseEventArgs>)delegate
					{
						Disable();
					});
					_contextMenuStrip.AddMenuItem(disable);
				}
				return _contextMenuStrip;
			}
		}

		[JsonIgnore]
		public Color BackgroundColor
		{
			get
			{
				//IL_004b: Unknown result type (might be due to invalid IL or missing references)
				//IL_006a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0072: Unknown result type (might be due to invalid IL or missing references)
				if (!_backgroundColor.HasValue && !Filler)
				{
					Color colorFromEvent = (string.IsNullOrWhiteSpace(BackgroundColorCode) ? Color.White : ColorTranslator.FromHtml(BackgroundColorCode));
					_backgroundColor = new Color((int)colorFromEvent.R, (int)colorFromEvent.G, (int)colorFromEvent.B);
				}
				return (Color)(((_003F?)_backgroundColor) ?? Color.get_Transparent());
			}
		}

		[JsonIgnore]
		public List<DateTime> Occurences { get; private set; } = new List<DateTime>();


		public Event()
		{
			timeSinceUpdate = updateInterval.TotalMilliseconds;
		}

		public bool Draw(SpriteBatch spriteBatch, Rectangle bounds, Control control, Texture2D baseTexture, int y, double pixelPerMinute, DateTime now, DateTime min, DateTime max, BitmapFont font)
		{
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_019a: Unknown result type (might be due to invalid IL or missing references)
			//IL_019f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0201: Unknown result type (might be due to invalid IL or missing references)
			//IL_0206: Unknown result type (might be due to invalid IL or missing references)
			//IL_020b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0244: Unknown result type (might be due to invalid IL or missing references)
			//IL_0262: Unknown result type (might be due to invalid IL or missing references)
			//IL_026f: Unknown result type (might be due to invalid IL or missing references)
			//IL_027e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0296: Unknown result type (might be due to invalid IL or missing references)
			//IL_0298: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0302: Unknown result type (might be due to invalid IL or missing references)
			//IL_031d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0324: Unknown result type (might be due to invalid IL or missing references)
			//IL_032e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0335: Unknown result type (might be due to invalid IL or missing references)
			//IL_0349: Unknown result type (might be due to invalid IL or missing references)
			//IL_0358: Unknown result type (might be due to invalid IL or missing references)
			//IL_036a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0371: Unknown result type (might be due to invalid IL or missing references)
			//IL_0379: Unknown result type (might be due to invalid IL or missing references)
			//IL_0380: Unknown result type (might be due to invalid IL or missing references)
			//IL_0390: Unknown result type (might be due to invalid IL or missing references)
			//IL_0392: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e5: Unknown result type (might be due to invalid IL or missing references)
			List<DateTime> occurences = new List<DateTime>();
			lock (Occurences)
			{
				occurences.AddRange(Occurences.Where((DateTime oc) => (oc >= min || oc.AddMinutes(Duration) >= min) && oc <= max));
			}
			_lastYPosition = y;
			RectangleF eventTexturePosition = default(RectangleF);
			RectangleF eventTimeRemainingPosition = default(RectangleF);
			foreach (DateTime eventStart in occurences)
			{
				float width = (float)GetWidth(eventStart, min, bounds, pixelPerMinute);
				if (width <= 0f)
				{
					continue;
				}
				float x = (float)GetXPosition(eventStart, min, pixelPerMinute);
				x = Math.Max(x, 0f);
				((RectangleF)(ref eventTexturePosition))._002Ector(x, (float)y, width, (float)EventTableModule.ModuleInstance.EventHeight);
				bool drawBorder = !Filler && EventTableModule.ModuleInstance.ModuleSettings.DrawEventBorder.get_Value();
				DrawRectangle(spriteBatch, control, baseTexture, eventTexturePosition, BackgroundColor * EventTableModule.ModuleInstance.ModuleSettings.Opacity.get_Value(), drawBorder ? 1 : 0, Color.get_Black());
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
			return occurences.Any();
		}

		private void UpdateTooltip(string description)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Expected O, but got Unknown
			_tooltip = new Tooltip((ITooltipView)(object)new TooltipView(Name, description, Icon));
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

		private List<DateTime> GetStartOccurences(DateTime now, DateTime max, DateTime min, bool addTimezoneOffset = true, bool limitsBetweenRanges = false)
		{
			List<DateTime> startOccurences = new List<DateTime>();
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

		public bool IsHovered(DateTime min, Rectangle bounds, Point relativeMousePosition, double pixelPerMinute)
		{
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			if (IsDisabled())
			{
				return false;
			}
			foreach (DateTime occurence in Occurences)
			{
				double x = GetXPosition(occurence, min, pixelPerMinute);
				double width = GetWidth(occurence, min, bounds, pixelPerMinute);
				x = Math.Max(x, 0.0);
				if ((double)relativeMousePosition.X >= x && (double)relativeMousePosition.X < x + width && relativeMousePosition.Y >= _lastYPosition && relativeMousePosition.Y < _lastYPosition + EventTableModule.ModuleInstance.EventHeight)
				{
					return true;
				}
			}
			return false;
		}

		public void HandleClick(object sender, MouseEventArgs e)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Invalid comparison between Unknown and I4
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Invalid comparison between Unknown and I4
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			if (Filler)
			{
				return;
			}
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
			if (Filler)
			{
				return;
			}
			List<DateTime> occurences = Occurences;
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
					description = Location + ((!string.IsNullOrWhiteSpace(Location)) ? "\n" : string.Empty) + "\n" + Strings.Event_Tooltip_StartsAt + ": " + FormatTime(hoveredOccurence);
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
			EventTableModule.ModuleInstance.HiddenState.Add(SettingKey, until, isUTC: true);
		}

		public void FinishCategory()
		{
			EventCategory.Finish();
		}

		public void Disable()
		{
			IEnumerable<SettingEntry<bool>> eventSetting = EventTableModule.ModuleInstance.ModuleSettings.AllEvents.Where((SettingEntry<bool> e) => ((SettingEntry)e).get_EntryKey().ToLowerInvariant() == SettingKey.ToLowerInvariant());
			if (eventSetting.Any())
			{
				eventSetting.First().set_Value(false);
			}
		}

		public bool IsDisabled()
		{
			if (Filler)
			{
				return false;
			}
			IEnumerable<SettingEntry<bool>> eventSetting = EventTableModule.ModuleInstance.ModuleSettings.AllEvents.Where((SettingEntry<bool> e) => ((SettingEntry)e).get_EntryKey().ToLowerInvariant() == SettingKey.ToLowerInvariant());
			if (eventSetting.Any())
			{
				return !eventSetting.First().get_Value() || EventTableModule.ModuleInstance.HiddenState.IsHidden(SettingKey);
			}
			return false;
		}

		private void UpdateEventOccurences(GameTime gameTime)
		{
			if (!Filler)
			{
				lock (Occurences)
				{
					Occurences.Clear();
				}
				DateTime now = EventTableModule.ModuleInstance.DateTimeNow;
				DateTime min = now.AddDays(-4.0);
				DateTime max = now.AddDays(4.0);
				List<DateTime> occurences = GetStartOccurences(now, max, min);
				lock (Occurences)
				{
					Occurences.AddRange(occurences);
				}
			}
		}

		public void Update(GameTime gameTime)
		{
			UpdateCadenceUtil.UpdateWithCadence(UpdateEventOccurences, gameTime, updateInterval.TotalMilliseconds, ref timeSinceUpdate);
		}
	}
}
