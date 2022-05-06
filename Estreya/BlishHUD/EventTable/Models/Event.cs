using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Common.UI.Views;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Blish_HUD._Extensions;
using Estreya.BlishHUD.EventTable.Controls;
using Estreya.BlishHUD.EventTable.Input;
using Estreya.BlishHUD.EventTable.Json;
using Estreya.BlishHUD.EventTable.Resources;
using Estreya.BlishHUD.EventTable.State;
using Estreya.BlishHUD.EventTable.UI.Views;
using Estreya.BlishHUD.EventTable.UI.Views.Edit;
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

		private bool _editing;

		[JsonIgnore]
		private string _backgroundColorCode;

		[JsonIgnore]
		private Tooltip _tooltip;

		[JsonIgnore]
		private ContextMenuStrip _contextMenuStrip;

		[JsonIgnore]
		private string _settingKey;

		[JsonIgnore]
		private Color? _backgroundColor;

		[JsonIgnore]
		private bool? _isDisabled;

		[JsonIgnore]
		private int _lastYPosition;

		[Description("Specifies the key of the event. Should be unique for a event category. Avoid changing it, as it resets saved settings and states.")]
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
		public string BackgroundColorCode
		{
			get
			{
				return _backgroundColorCode;
			}
			set
			{
				_backgroundColorCode = value;
				_backgroundColor = null;
			}
		}

		[JsonProperty("apiType")]
		public APICodeType APICodeType { get; set; }

		[JsonProperty("api")]
		public string APICode { get; set; }

		[JsonIgnore]
		internal bool Filler { get; set; }

		[JsonIgnore]
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
				//IL_0015: Unknown result type (might be due to invalid IL or missing references)
				//IL_001f: Expected O, but got Unknown
				if (_contextMenuStrip == null)
				{
					_contextMenuStrip = new ContextMenuStrip((Func<IEnumerable<ContextMenuStripItem>>)GetContextMenu);
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
				//IL_006e: Unknown result type (might be due to invalid IL or missing references)
				//IL_008f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0097: Unknown result type (might be due to invalid IL or missing references)
				if (!_backgroundColor.HasValue && !Filler)
				{
					try
					{
						Color colorFromEvent = (string.IsNullOrWhiteSpace(BackgroundColorCode) ? Color.White : ColorTranslator.FromHtml(BackgroundColorCode));
						_backgroundColor = new Color((int)colorFromEvent.R, (int)colorFromEvent.G, (int)colorFromEvent.B);
					}
					catch (Exception ex)
					{
						Logger.Error(ex, "Failed generating background color:");
						_backgroundColor = Color.get_Transparent();
					}
				}
				return (Color)(((_003F?)_backgroundColor) ?? Color.get_Transparent());
			}
		}

		[JsonIgnore]
		public bool IsDisabled
		{
			get
			{
				if (!_isDisabled.HasValue)
				{
					if (Filler)
					{
						_isDisabled = false;
					}
					IEnumerable<SettingEntry<bool>> eventSetting = EventTableModule.ModuleInstance.ModuleSettings.AllEvents.Where((SettingEntry<bool> e) => ((SettingEntry)e).get_EntryKey().ToLowerInvariant() == SettingKey.ToLowerInvariant());
					if (eventSetting.Any())
					{
						bool enabled = eventSetting.First().get_Value() && !EventTableModule.ModuleInstance.EventState.Contains(SettingKey, EventState.EventStates.Hidden);
						_isDisabled = !enabled;
					}
					if (!_isDisabled.HasValue)
					{
						_isDisabled = false;
					}
				}
				return _isDisabled.Value;
			}
		}

		[JsonIgnore]
		public List<DateTime> Occurences { get; private set; } = new List<DateTime>();


		public event EventHandler Edited;

		private IEnumerable<ContextMenuStripItem> GetContextMenu()
		{
			ContextMenuStripItem val = new ContextMenuStripItem();
			val.set_Text(Strings.Event_CopyWaypoint);
			((Control)val).set_BasicTooltipText("Copies the waypoint to the clipboard.");
			ContextMenuStripItem copyWaypoint = val;
			((Control)copyWaypoint).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				CopyWaypoint();
			});
			yield return copyWaypoint;
			ContextMenuStripItem val2 = new ContextMenuStripItem();
			val2.set_Text(Strings.Event_OpenWiki);
			((Control)val2).set_BasicTooltipText("Open the wiki in the default browser.");
			ContextMenuStripItem openWiki = val2;
			((Control)openWiki).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				OpenWiki();
			});
			yield return openWiki;
			ContextMenuStrip hideMenuStrip = new ContextMenuStrip((Func<IEnumerable<ContextMenuStripItem>>)delegate
			{
				//IL_0005: Unknown result type (might be due to invalid IL or missing references)
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0015: Unknown result type (might be due to invalid IL or missing references)
				//IL_0021: Expected O, but got Unknown
				//IL_003a: Unknown result type (might be due to invalid IL or missing references)
				//IL_003f: Unknown result type (might be due to invalid IL or missing references)
				//IL_004a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0056: Expected O, but got Unknown
				List<ContextMenuStripItem> list2 = new List<ContextMenuStripItem>();
				ContextMenuStripItem val11 = new ContextMenuStripItem();
				val11.set_Text(Strings.Event_HideCategory);
				((Control)val11).set_BasicTooltipText("Hides the event category until reset.");
				ContextMenuStripItem val12 = val11;
				((Control)val12).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					HideCategory();
				});
				list2.Add(val12);
				ContextMenuStripItem val13 = new ContextMenuStripItem();
				val13.set_Text(Strings.Event_HideEvent);
				((Control)val13).set_BasicTooltipText("Hides the event until reset.");
				ContextMenuStripItem val14 = val13;
				((Control)val14).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					Hide();
				});
				list2.Add(val14);
				return list2;
			});
			ContextMenuStripItem val3 = new ContextMenuStripItem();
			val3.set_Text("Hide");
			val3.set_Submenu(hideMenuStrip);
			((Control)val3).set_BasicTooltipText("Adds options for hiding events.");
			yield return val3;
			ContextMenuStrip finishMenuStrip = new ContextMenuStrip((Func<IEnumerable<ContextMenuStripItem>>)delegate
			{
				//IL_0005: Unknown result type (might be due to invalid IL or missing references)
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0015: Unknown result type (might be due to invalid IL or missing references)
				//IL_0021: Expected O, but got Unknown
				//IL_003a: Unknown result type (might be due to invalid IL or missing references)
				//IL_003f: Unknown result type (might be due to invalid IL or missing references)
				//IL_004a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0056: Expected O, but got Unknown
				List<ContextMenuStripItem> list = new List<ContextMenuStripItem>();
				ContextMenuStripItem val7 = new ContextMenuStripItem();
				val7.set_Text("Finish Category");
				((Control)val7).set_BasicTooltipText("Completes the event category until reset.");
				ContextMenuStripItem val8 = val7;
				((Control)val8).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					FinishCategory();
				});
				list.Add(val8);
				ContextMenuStripItem val9 = new ContextMenuStripItem();
				val9.set_Text("Finish Event");
				((Control)val9).set_BasicTooltipText("Completes the event until reset.");
				ContextMenuStripItem val10 = val9;
				((Control)val10).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)delegate
				{
					Finish();
				});
				list.Add(val10);
				return list;
			});
			ContextMenuStripItem val4 = new ContextMenuStripItem();
			val4.set_Text("Finish");
			val4.set_Submenu(finishMenuStrip);
			((Control)val4).set_BasicTooltipText("Adds options for finishing events.");
			yield return val4;
			ContextMenuStripItem val5 = new ContextMenuStripItem();
			val5.set_Text("Edit");
			((Control)val5).set_BasicTooltipText("Opens the edit window.");
			ContextMenuStripItem edit = val5;
			((Control)edit).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				Edit();
			});
			yield return edit;
			ContextMenuStripItem val6 = new ContextMenuStripItem();
			val6.set_Text(Strings.Event_Disable);
			((Control)val6).set_BasicTooltipText("Disables the event completely.");
			ContextMenuStripItem disable = val6;
			((Control)disable).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				Disable();
			});
			yield return disable;
		}

		public Event()
		{
			timeSinceUpdate = updateInterval.TotalMilliseconds;
		}

		public bool Draw(SpriteBatch spriteBatch, Rectangle bounds, Texture2D baseTexture, int y, double pixelPerMinute, DateTime now, DateTime min, DateTime max, BitmapFont font)
		{
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_0197: Unknown result type (might be due to invalid IL or missing references)
			//IL_019c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0203: Unknown result type (might be due to invalid IL or missing references)
			//IL_0208: Unknown result type (might be due to invalid IL or missing references)
			//IL_0241: Unknown result type (might be due to invalid IL or missing references)
			//IL_025f: Unknown result type (might be due to invalid IL or missing references)
			//IL_026c: Unknown result type (might be due to invalid IL or missing references)
			//IL_027b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0292: Unknown result type (might be due to invalid IL or missing references)
			//IL_0294: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0319: Unknown result type (might be due to invalid IL or missing references)
			//IL_0320: Unknown result type (might be due to invalid IL or missing references)
			//IL_032a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0331: Unknown result type (might be due to invalid IL or missing references)
			//IL_0345: Unknown result type (might be due to invalid IL or missing references)
			//IL_0354: Unknown result type (might be due to invalid IL or missing references)
			//IL_0366: Unknown result type (might be due to invalid IL or missing references)
			//IL_036d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0375: Unknown result type (might be due to invalid IL or missing references)
			//IL_037c: Unknown result type (might be due to invalid IL or missing references)
			//IL_038b: Unknown result type (might be due to invalid IL or missing references)
			//IL_038d: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b7: Unknown result type (might be due to invalid IL or missing references)
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
				spriteBatch.DrawRectangle(baseTexture, eventTexturePosition, BackgroundColor * EventTableModule.ModuleInstance.ModuleSettings.Opacity.get_Value(), drawBorder ? 1 : 0, Color.get_Black());
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
					spriteBatch.DrawString(eventName, font, eventTextPosition, textColor, wrap: false, (HorizontalAlignment)0, (VerticalAlignment)1);
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
						spriteBatch.DrawString(timeRemainingString, font, eventTimeRemainingPosition, textColor, wrap: false, (HorizontalAlignment)0, (VerticalAlignment)1);
					}
				}
				if ((EventCategory?.IsFinished() ?? false) || IsFinished())
				{
					spriteBatch.DrawCrossOut(baseTexture, eventTexturePosition, Color.get_Red());
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
			if (IsDisabled)
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
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Invalid comparison between Unknown and I4
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_0128: Unknown result type (might be due to invalid IL or missing references)
			if (Filler)
			{
				return;
			}
			if ((int)e.get_EventType() == 513 && EventTableModule.ModuleInstance.ModuleSettings.HandleLeftClick.get_Value())
			{
				if (EventTableModule.ModuleInstance.ModuleSettings.LeftClickAction.get_Value() == LeftClickAction.CopyWaypoint)
				{
					CopyWaypoint();
				}
				else
				{
					if (EventTableModule.ModuleInstance.ModuleSettings.LeftClickAction.get_Value() != LeftClickAction.NavigateToWaypoint)
					{
						return;
					}
					Task.Run(async delegate
					{
						if (EventTableModule.ModuleInstance.PointOfInterestState.Loading)
						{
							ScreenNotification.ShowNotification("Point of Interest State is currently loading...", (NotificationType)2);
						}
						else
						{
							ContinentFloorRegionMapPoi waypoint = EventTableModule.ModuleInstance.PointOfInterestState.GetPointOfInterest(Waypoint);
							if (waypoint != null && !(await MapNavigationUtil.NavigateToPosition(waypoint)))
							{
								ScreenNotification.ShowNotification("Navigation failed.", (NotificationType)2);
							}
						}
					});
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

		public void Hide()
		{
			DateTime now = EventTableModule.ModuleInstance.DateTimeNow.ToUniversalTime();
			DateTime until = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, DateTimeKind.Utc).AddDays(1.0);
			EventTableModule.ModuleInstance.EventState.Add(SettingKey, until, EventState.EventStates.Hidden);
		}

		private void HideCategory()
		{
			EventCategory?.Hide();
		}

		public void Disable()
		{
			IEnumerable<SettingEntry<bool>> eventSetting = EventTableModule.ModuleInstance.ModuleSettings.AllEvents.Where((SettingEntry<bool> e) => ((SettingEntry)e).get_EntryKey().ToLowerInvariant() == SettingKey.ToLowerInvariant());
			if (eventSetting.Any())
			{
				eventSetting.First().set_Value(false);
			}
		}

		public void Finish()
		{
			DateTime now = EventTableModule.ModuleInstance.DateTimeNow.ToUniversalTime();
			DateTime until = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, DateTimeKind.Utc).AddDays(1.0);
			EventTableModule.ModuleInstance.EventState.Add(SettingKey, until, EventState.EventStates.Completed);
		}

		private void FinishCategory()
		{
			EventCategory?.Finish();
		}

		public bool IsFinished()
		{
			if (Filler)
			{
				return false;
			}
			return EventTableModule.ModuleInstance.EventState.Contains(SettingKey, EventState.EventStates.Completed);
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
				DateTime min = now.AddDays(-2.0);
				DateTime max = now.AddDays(2.0);
				List<DateTime> occurences = GetStartOccurences(now, max, min);
				lock (Occurences)
				{
					Occurences.AddRange(occurences);
				}
			}
		}

		public Task LoadAsync()
		{
			EventTableModule.ModuleInstance.ModuleSettings.EventSettingChanged += ModuleSettings_EventSettingChanged;
			EventTableModule.ModuleInstance.EventState.StateAdded += EventState_StateAdded;
			EventTableModule.ModuleInstance.EventState.StateRemoved += EventState_StateRemoved;
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

		private void EventState_StateRemoved(object sender, ValueEventArgs<string> e)
		{
			if (SettingKey == e.get_Value())
			{
				_isDisabled = null;
			}
		}

		private void EventState_StateAdded(object sender, ValueEventArgs<EventState.VisibleStateInfo> e)
		{
			if (SettingKey == e.get_Value().Key && e.get_Value().State == EventState.EventStates.Hidden)
			{
				_isDisabled = null;
			}
		}

		private void ModuleSettings_EventSettingChanged(object sender, ModuleSettings.EventSettingsChangedEventArgs e)
		{
			if (SettingKey.ToLowerInvariant() == e.Name.ToLowerInvariant())
			{
				_isDisabled = null;
			}
		}

		public void Unload()
		{
			Logger.Debug("Unload event: {0}", new object[1] { Key });
			EventTableModule.ModuleInstance.ModuleSettings.EventSettingChanged -= ModuleSettings_EventSettingChanged;
			EventTableModule.ModuleInstance.EventState.StateAdded -= EventState_StateAdded;
			EventTableModule.ModuleInstance.EventState.StateRemoved -= EventState_StateRemoved;
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
			UpdateUtil.Update(UpdateEventOccurences, gameTime, updateInterval.TotalMilliseconds, ref timeSinceUpdate);
		}

		public void Edit()
		{
			lock (this)
			{
				if (_editing)
				{
					return;
				}
				_editing = true;
			}
			Task.Run(async delegate
			{
				Texture2D backgroundTexture = await EventTableModule.ModuleInstance.IconState.GetIconAsync("controls/window/502049", checkRenderAPI: false);
				Rectangle settingsWindowSize = default(Rectangle);
				((Rectangle)(ref settingsWindowSize))._002Ector(35, 50, 913, 691);
				int contentRegionPaddingY = settingsWindowSize.Y - 15;
				int contentRegionPaddingX = settingsWindowSize.X + 46;
				Rectangle contentRegion = default(Rectangle);
				((Rectangle)(ref contentRegion))._002Ector(contentRegionPaddingX, contentRegionPaddingY, settingsWindowSize.Width - 52, settingsWindowSize.Height - contentRegionPaddingY);
				StandardWindow val = new StandardWindow(backgroundTexture, settingsWindowSize, contentRegion);
				((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
				((WindowBase2)val).set_Title("Edit");
				StandardWindow val2 = val;
				((WindowBase2)val2).set_Emblem(await EventTableModule.ModuleInstance.IconState.GetIconAsync("156684", checkRenderAPI: false));
				((WindowBase2)val).set_Subtitle(Name);
				((WindowBase2)val).set_SavesPosition(true);
				((WindowBase2)val).set_CanClose(false);
				((WindowBase2)val).set_Id("EventTableModule_f925849b-44bd-4c9f-aaac-76826d93ba6f");
				StandardWindow window = val;
				EditEventView editView = new EditEventView(Clone());
				editView.SavePressed += delegate(object s, ValueEventArgs<Event> e)
				{
					((Control)window).Hide();
					((Control)window).Dispose();
					lock (this)
					{
						Name = e.get_Value().Name;
						Offset = e.get_Value().Offset;
						Repeat = e.get_Value().Repeat;
						Location = e.get_Value().Location;
						Waypoint = e.get_Value().Waypoint;
						Wiki = e.get_Value().Wiki;
						Duration = e.get_Value().Duration;
						Icon = e.get_Value().Icon;
						BackgroundColorCode = e.get_Value().BackgroundColorCode;
						APICodeType = e.get_Value().APICodeType;
						APICode = e.get_Value().APICode;
						timeSinceUpdate = updateInterval.TotalMilliseconds;
						_editing = false;
					}
					this.Edited?.Invoke(this, EventArgs.Empty);
				};
				editView.CancelPressed += delegate
				{
					((Control)window).Hide();
					((Control)window).Dispose();
					lock (this)
					{
						_editing = false;
					}
				};
				window.Show((IView)(object)editView);
			});
		}

		public Event Clone()
		{
			return (Event)MemberwiseClone();
		}
	}
}
