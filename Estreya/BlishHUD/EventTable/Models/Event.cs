using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Estreya.BlishHUD.Shared.Attributes;
using Estreya.BlishHUD.Shared.Json.Converter;
using Estreya.BlishHUD.Shared.State;
using Newtonsoft.Json;

namespace Estreya.BlishHUD.EventTable.Models
{
	[Serializable]
	public class Event
	{
		[IgnoreCopy]
		private static readonly Logger Logger = Logger.GetLogger<Event>();

		[JsonIgnore]
		public TimeSpan[] ReminderTimes = new TimeSpan[1] { TimeSpan.FromMinutes(10.0) };

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
			"dd\\.hh\\:mm",
			new string[] { "dd\\.hh\\:mm", "hh\\:mm" }
		})]
		public TimeSpan Offset { get; set; }

		[JsonProperty("repeat")]
		[JsonConverter(typeof(TimeSpanJsonConverter), new object[]
		{
			"dd\\.hh\\:mm",
			new string[] { "dd\\.hh\\:mm", "hh\\:mm" }
		})]
		public TimeSpan Repeat { get; set; }

		[JsonProperty("startingDate")]
		[JsonConverter(typeof(DateJsonConverter))]
		public DateTime? StartingDate { get; set; }

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
		public APICodeType? APICodeType { get; set; }

		[JsonProperty("apiCode")]
		public string APICode { get; set; }

		[JsonIgnore]
		public bool Filler { get; set; }

		[JsonProperty("occurences")]
		public List<DateTime> Occurences { get; private set; } = new List<DateTime>();


		[JsonIgnore]
		public string SettingKey { get; private set; }

		public event EventHandler<TimeSpan> Reminder;

		public DateTime? GetCurrentOccurence(DateTime now)
		{
			IEnumerable<DateTime> occurences = Occurences.Where((DateTime oc) => oc <= now && oc.AddMinutes(Duration) >= now);
			if (!occurences.Any())
			{
				return null;
			}
			return occurences.First();
		}

		public bool IsRunning(DateTime now)
		{
			return GetCurrentOccurence(now).HasValue;
		}

		public TimeSpan GetTimeRemaining(DateTime now)
		{
			DateTime? co = GetCurrentOccurence(now);
			if (co.HasValue)
			{
				return co.Value.AddMinutes(Duration) - now;
			}
			return TimeSpan.Zero;
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

		public Task LoadAsync(EventCategory ec, TranslationState translationState = null)
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
			if (translationState != null)
			{
				Name = translationState.GetTranslation("event-" + ec.Key + "_" + Key + "-name", Name);
			}
			return Task.CompletedTask;
		}

		public void Update(DateTime nowUTC)
		{
			if (Filler)
			{
				return;
			}
			foreach (DateTime occurence in Occurences.Where((DateTime o) => o >= nowUTC))
			{
				List<TimeSpan> alreadyRemindedTimes = _remindedFor.GetOrAdd(occurence, (DateTime o) => new List<TimeSpan>());
				TimeSpan[] reminderTimes = ReminderTimes;
				foreach (TimeSpan time in reminderTimes)
				{
					if (!alreadyRemindedTimes.Contains(time))
					{
						DateTime remindAt = occurence - time;
						TimeSpan diff = nowUTC - remindAt;
						if (remindAt <= nowUTC && Math.Abs(diff.TotalSeconds) <= 1.0)
						{
							this.Reminder?.Invoke(this, time);
							alreadyRemindedTimes.Add(time);
						}
					}
				}
			}
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
