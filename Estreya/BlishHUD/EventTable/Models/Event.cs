using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Blish_HUD;
using Estreya.BlishHUD.Shared.Attributes;
using Estreya.BlishHUD.Shared.Json.Converter;
using Estreya.BlishHUD.Shared.Services;
using Estreya.BlishHUD.Shared.Utils;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace Estreya.BlishHUD.EventTable.Models
{
	[Serializable]
	public class Event : IUpdatable
	{
		[IgnoreCopy]
		private static readonly Logger Logger = Logger.GetLogger<Event>();

		[IgnoreCopy]
		private static TimeSpan _checkForRemindersInterval = TimeSpan.FromMilliseconds(5000.0);

		private Func<DateTime> _getNowAction;

		[IgnoreCopy]
		private double _lastCheckForReminders;

		[JsonIgnore]
		private ConcurrentDictionary<DateTime, List<TimeSpan>> _remindedFor = new ConcurrentDictionary<DateTime, List<TimeSpan>>();

		[Description("Specifies the key of the event. Should be unique for a event category. Avoid changing it, as it resets saved settings and states.")]
		[JsonProperty("key")]
		public string Key { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("offset")]
		[JsonConverter(typeof(TimeSpanJsonConverter), new object[]
		{
			"dd\\.hh\\:mm\\:ss",
			new string[] { "dd\\.hh\\:mm\\:ss", "hh\\:mm\\:ss" }
		})]
		public TimeSpan Offset { get; set; }

		[JsonProperty("repeat")]
		[JsonConverter(typeof(TimeSpanJsonConverter), new object[]
		{
			"dd\\.hh\\:mm\\:ss",
			new string[] { "dd\\.hh\\:mm\\:ss", "hh\\:mm\\:ss" }
		})]
		public TimeSpan Repeat { get; set; }

		[JsonProperty("startingDate")]
		[JsonConverter(typeof(DateJsonConverter))]
		public DateTime? StartingDate { get; set; }

		[JsonProperty("location")]
		public string Location { get; set; }

		[JsonProperty("mapIds")]
		public int[] MapIds { get; set; }

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

		[JsonProperty("colorGradient")]
		public string[] BackgroundColorGradientCodes { get; set; }

		[JsonProperty("apiType")]
		public APICodeType? APICodeType { get; set; }

		[JsonProperty("apiCode")]
		public string APICode { get; set; }

		[JsonProperty("linkedCompletion")]
		public bool LinkedCompletion { get; set; }

		[JsonProperty("filler")]
		public bool Filler { get; set; }

		[JsonProperty("occurences")]
		public List<DateTime> Occurences { get; set; } = new List<DateTime>();


		[JsonIgnore]
		public bool HostedBySystem { get; set; } = true;


		[JsonIgnore]
		public string SettingKey { get; private set; }

		[JsonIgnore]
		public WeakReference<EventCategory> Category { get; private set; }

		[JsonProperty("reminderTimes")]
		[JsonConverter(typeof(TimeSpanArrayJsonConverter), new object[]
		{
			"hh\\:mm\\:ss",
			new string[] { "hh\\:mm", "hh\\:mm\\:ss" },
			true
		})]
		public TimeSpan[] ReminderTimes { get; private set; } = new TimeSpan[1] { TimeSpan.FromMinutes(10.0) };


		public event EventHandler<TimeSpan> Reminder;

		public void Update(GameTime gameTime)
		{
			UpdateUtil.Update(CheckForReminder, gameTime, _checkForRemindersInterval.TotalMilliseconds, ref _lastCheckForReminders);
		}

		public double CalculateXPosition(DateTime start, DateTime min, double pixelPerMinute)
		{
			return start.Subtract(min).TotalMinutes * pixelPerMinute;
		}

		public double CalculateWidth(DateTime eventOccurence, DateTime min, int maxWidth, double pixelPerMinute)
		{
			double eventWidth = (double)Duration * pixelPerMinute;
			double x = CalculateXPosition(eventOccurence, min, pixelPerMinute);
			if (x < 0.0)
			{
				eventWidth -= Math.Abs(x);
			}
			if (((x > 0.0) ? x : 0.0) + eventWidth > (double)maxWidth)
			{
				eventWidth = (double)maxWidth - ((x > 0.0) ? x : 0.0);
			}
			return eventWidth;
		}

		public void Load(EventCategory ec, Func<DateTime> getNowAction, TranslationService translationService = null)
		{
			if (string.IsNullOrWhiteSpace(Key))
			{
				Key = Name;
			}
			if (string.IsNullOrWhiteSpace(Icon))
			{
				Icon = ec.Icon;
			}
			SettingKey = ec.Key + "_" + Key;
			Category = new WeakReference<EventCategory>(ec);
			_getNowAction = getNowAction;
			if (translationService != null)
			{
				Name = translationService.GetTranslation("event-" + ec.Key + "_" + Key + "-name", Name);
			}
		}

		private void CheckForReminder()
		{
			if (Filler)
			{
				return;
			}
			DateTime nowUTC = _getNowAction();
			foreach (DateTime occurence in Occurences.Where((DateTime o) => o >= nowUTC))
			{
				List<TimeSpan> alreadyRemindedTimes = _remindedFor.GetOrAdd(occurence, (DateTime o) => new List<TimeSpan>());
				List<(TimeSpan, TimeSpan)> eligableTimes = new List<(TimeSpan, TimeSpan)>();
				TimeSpan[] reminderTimes = ReminderTimes;
				foreach (TimeSpan time in reminderTimes)
				{
					if (!alreadyRemindedTimes.Contains(time))
					{
						DateTime remindAt = occurence - time;
						TimeSpan diff = remindAt - nowUTC;
						if (remindAt < nowUTC || (remindAt >= nowUTC && diff.TotalSeconds <= 0.0))
						{
							eligableTimes.Add((time, occurence - nowUTC));
						}
					}
				}
				if (eligableTimes.Count > 0)
				{
					eligableTimes.ForEach(delegate((TimeSpan reminderTime, TimeSpan timeLeft) et)
					{
						alreadyRemindedTimes.Add(et.reminderTime);
					});
					this.Reminder?.Invoke(this, eligableTimes.OrderBy<(TimeSpan, TimeSpan), TimeSpan>(((TimeSpan reminderTime, TimeSpan timeLeft) et) => et.reminderTime).First().Item2);
				}
			}
		}

		public void UpdateReminderTimes(TimeSpan[] reminderTimes)
		{
			ReminderTimes = reminderTimes;
			_remindedFor.Clear();
		}

		public override string ToString()
		{
			string[] keySplit = SettingKey?.Split('_') ?? new string[2]
			{
				string.Empty,
				Name
			};
			return $"Category: {keySplit[0]} - Name: {keySplit[1]} - Filler {Filler}";
		}
	}
}
