using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Common.UI.Views;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Blish_HUD._Extensions;
using Estreya.BlishHUD.EventTable.Controls;
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
			new string[] { "dd\\.hh\\:mm", "hh\\:mm" }
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

		[JsonProperty("apiType")]
		public APICodeType APICodeType { get; set; }

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
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_012d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_0199: Unknown result type (might be due to invalid IL or missing references)
			//IL_019e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0200: Unknown result type (might be due to invalid IL or missing references)
			//IL_0205: Unknown result type (might be due to invalid IL or missing references)
			//IL_020a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0243: Unknown result type (might be due to invalid IL or missing references)
			//IL_0261: Unknown result type (might be due to invalid IL or missing references)
			//IL_026e: Unknown result type (might be due to invalid IL or missing references)
			//IL_027d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0295: Unknown result type (might be due to invalid IL or missing references)
			//IL_0297: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0301: Unknown result type (might be due to invalid IL or missing references)
			//IL_031c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0323: Unknown result type (might be due to invalid IL or missing references)
			//IL_032d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0334: Unknown result type (might be due to invalid IL or missing references)
			//IL_0348: Unknown result type (might be due to invalid IL or missing references)
			//IL_0357: Unknown result type (might be due to invalid IL or missing references)
			//IL_0369: Unknown result type (might be due to invalid IL or missing references)
			//IL_0370: Unknown result type (might be due to invalid IL or missing references)
			//IL_0378: Unknown result type (might be due to invalid IL or missing references)
			//IL_037f: Unknown result type (might be due to invalid IL or missing references)
			//IL_038f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0391: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d4: Unknown result type (might be due to invalid IL or missing references)
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
				spriteBatch.DrawRectangle(control, baseTexture, eventTexturePosition, BackgroundColor * EventTableModule.ModuleInstance.ModuleSettings.Opacity.get_Value(), drawBorder ? 1 : 0, Color.get_Black());
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
				if (EventTableModule.ModuleInstance.ModuleSettings.EventCompletedAcion.get_Value() == EventCompletedAction.Crossout && !Filler && !string.IsNullOrWhiteSpace(APICode) && IsCompleted())
				{
					spriteBatch.DrawCrossOut(control, baseTexture, eventTexturePosition, Color.get_Red());
				}
			}
			return occurences.Any();
		}

		public bool IsCompleted()
		{
			bool completed = false;
			switch (APICodeType)
			{
			case APICodeType.Worldboss:
				completed |= EventTableModule.ModuleInstance.WorldbossState.IsCompleted(APICode);
				break;
			case APICodeType.Mapchest:
				completed |= EventTableModule.ModuleInstance.MapchestState.IsCompleted(APICode);
				break;
			default:
				Logger.Warn($"Unsupported api code type: {APICodeType}");
				break;
			}
			return completed;
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
				offset = offset.Add(TimeZone.CurrentTimeZone.GetUtcOffset(now));
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
				eventStart = ((Repeat.TotalMinutes == 0.0) ? eventStart.Add(TimeSpan.FromDays(1.0)) : eventStart.Add(Repeat));
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
			EventCategory?.Finish();
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

		public Task LoadAsync()
		{
			if (string.IsNullOrWhiteSpace(Key))
			{
				Key = Name;
			}
			if (EventTableModule.ModuleInstance.ModuleSettings.UseEventTranslation.get_Value())
			{
				Name = Strings.ResourceManager.GetString("event-" + SettingKey) ?? Name;
			}
			if (string.IsNullOrWhiteSpace(Icon))
			{
				Icon = EventCategory.Icon;
			}
			return Task.CompletedTask;
		}

		public void Unload()
		{
			Logger.Debug("Unload event: {0}", new object[1] { Key });
			Tooltip tooltip = _tooltip;
			if (tooltip != null)
			{
				((Control)tooltip).Dispose();
			}
			_tooltip = null;
			ContextMenuStrip contextMenuStrip = _contextMenuStrip;
			if (contextMenuStrip != null)
			{
				((Control)contextMenuStrip).Dispose();
			}
			_contextMenuStrip = null;
			Logger.Debug("Unloaded event: {0}", new object[1] { Key });
		}

		public void Update(GameTime gameTime)
		{
			UpdateCadenceUtil.UpdateWithCadence(UpdateEventOccurences, gameTime, updateInterval.TotalMilliseconds, ref timeSinceUpdate);
		}
	}
}
