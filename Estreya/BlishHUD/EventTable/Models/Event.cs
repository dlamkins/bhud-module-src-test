using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using Blish_HUD;
using Estreya.BlishHUD.Shared.Attributes;
using Estreya.BlishHUD.Shared.Services;
using Estreya.BlishHUD.Shared.Utils;
using Gw2Sharp.WebApi.V2.Models;
using Humanizer;
using Humanizer.Localisation;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using NodaTime;

namespace Estreya.BlishHUD.EventTable.Models
{
	[Serializable]
	public class Event : IUpdatable
	{
		[IgnoreCopy]
		private static readonly Logger Logger = Logger.GetLogger<Event>();

		[IgnoreCopy]
		private static Duration _checkForRemindersInterval = Duration.FromMilliseconds(5000L);

		[JsonIgnore]
		[IgnoreCopy]
		private Func<Instant> _getNowAction;

		[JsonIgnore]
		[IgnoreCopy]
		private TranslationService _translationService;

		[IgnoreCopy]
		private double _lastCheckForReminders;

		[JsonIgnore]
		private ConcurrentDictionary<Instant, List<Duration>> _remindedFor = new ConcurrentDictionary<Instant, List<Duration>>();

		[Description("Specifies the key of the event. Should be unique for a event category. Avoid changing it, as it resets saved settings and states.")]
		[JsonProperty("key")]
		public string Key { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("offset")]
		public Duration Offset { get; set; }

		[JsonProperty("repeat")]
		public Duration Repeat { get; set; }

		[JsonProperty("startingDate")]
		public LocalDate? StartingDate { get; set; }

		[JsonProperty("location")]
		public string Location { get; set; }

		[JsonProperty("timers")]
		public EventTimers[] Timers { get; set; }

		[JsonProperty("mapIds")]
		public int[] MapIds { get; set; }

		[JsonProperty("waypoints")]
		public EventWaypoints Waypoints { get; set; }

		[JsonProperty("wiki")]
		public string Wiki { get; set; }

		[JsonProperty("duration")]
		public Duration Duration { get; set; }

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

		[JsonProperty("linkedCompletionKeys")]
		public string[] LinkedCompletionKeys { get; set; }

		[JsonProperty("filler")]
		public bool Filler { get; set; }

		[JsonProperty("occurrences")]
		public List<Instant> Occurences { get; set; } = new List<Instant>();


		[JsonIgnore]
		public string SettingKey { get; private set; }

		[JsonIgnore]
		public WeakReference<EventCategory> Category { get; private set; }

		[JsonProperty("reminderTimes")]
		public Duration[] ReminderTimes { get; private set; } = new Duration[1] { Duration.FromMinutes(10L) };


		public event EventHandler<Duration> Reminder;

		public void Update(GameTime gameTime)
		{
			UpdateUtil.Update(CheckForReminder, gameTime, _checkForRemindersInterval.TotalMilliseconds, ref _lastCheckForReminders);
		}

		public double CalculateXPosition(Instant start, Instant min, double pixelPerMinute)
		{
			return start.Minus(min).TotalMinutes * pixelPerMinute;
		}

		public double CalculateWidth(Instant eventOccurence, Instant min, int maxWidth, double pixelPerMinute)
		{
			double eventWidth = Duration.TotalMinutes * pixelPerMinute;
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

		public void Load(EventCategory ec, Func<Instant> getNowAction, TranslationService translationService = null)
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
			_translationService = translationService;
			if (_translationService != null)
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
			Instant nowUTC = _getNowAction();
			foreach (Instant occurence in Occurences.Where((Instant o) => o >= nowUTC))
			{
				List<Duration> alreadyRemindedTimes = _remindedFor.GetOrAdd(occurence, (Instant o) => new List<Duration>());
				List<(Duration, Duration)> eligableTimes = new List<(Duration, Duration)>();
				Duration[] reminderTimes = ReminderTimes;
				foreach (Duration time in reminderTimes)
				{
					if (!alreadyRemindedTimes.Contains(time))
					{
						Instant remindAt = occurence - time;
						Duration diff = remindAt - nowUTC;
						if (remindAt < nowUTC || (remindAt >= nowUTC && diff.TotalSeconds <= 0.0))
						{
							eligableTimes.Add((time, occurence - nowUTC));
						}
					}
				}
				if (eligableTimes.Count > 0)
				{
					eligableTimes.ForEach(delegate((Duration reminderTime, Duration timeLeft) et)
					{
						alreadyRemindedTimes.Add(et.reminderTime);
					});
					this.Reminder?.Invoke(this, eligableTimes.OrderBy<(Duration, Duration), Duration>(((Duration reminderTime, Duration timeLeft) et) => et.reminderTime).First().Item2);
				}
			}
		}

		public void UpdateReminderTimes(Duration[] reminderTimes)
		{
			ReminderTimes = reminderTimes;
			_remindedFor.Clear();
		}

		public Instant GetCurrentOccurrence()
		{
			Instant now = _getNowAction();
			return Occurences.OrderBy((Instant x) => x).FirstOrDefault((Instant x) => x <= now && x.Plus(Duration) >= now);
		}

		public Instant GetNextOccurrence()
		{
			Instant now = _getNowAction();
			return Occurences.OrderBy((Instant x) => x).FirstOrDefault((Instant x) => x >= now);
		}

		public string GetWaypoint(Account account)
		{
			if (account == null)
			{
				Logger.Warn("Account is null. Returning EU waypoint.");
				return Waypoints.EU;
			}
			if (account.get_World().ToString().First() == '1')
			{
				return Waypoints.NA;
			}
			return Waypoints.EU;
		}

		public string GetChatText(EventChatFormat format, Instant occurence, Account account)
		{
			Instant now = _getNowAction();
			Instant endTime = occurence.Plus(Duration);
			bool num = endTime < now;
			bool isNext = !num && occurence > now;
			bool isCurrent = !num && !isNext;
			string timeString = occurence.InZone(DateTimeZoneProviders.Tzdb.GetSystemDefault()).ToString("HH:mm zzz", CultureInfo.CurrentUICulture);
			if (num)
			{
				Duration finishedSince = now - occurence.Plus(Duration);
				timeString = _translationService.GetTranslation("event-chatText-finishedXAgo", "finished {0} ago").FormatWith(finishedSince.ToTimeSpan().Humanize(2, null, TimeUnit.Week, TimeUnit.Second)) ?? "";
			}
			else if (isNext)
			{
				Duration startsIn = occurence - now;
				timeString = _translationService.GetTranslation("event-chatText-startsInX", "starts in {0}").FormatWith(startsIn.ToTimeSpan().Humanize(2, null, TimeUnit.Week, TimeUnit.Second)) ?? "";
			}
			else if (isCurrent)
			{
				Duration remaining = endTime - now;
				timeString = _translationService.GetTranslation("event-chatText-hasXRemaining", "has {0} remaining").FormatWith(remaining.ToTimeSpan().Humanize(2, null, TimeUnit.Week, TimeUnit.Second)) ?? "";
			}
			string waypoint = GetWaypoint(account);
			return format switch
			{
				EventChatFormat.Full => _translationService.GetTranslation("event-chatText-format-full", "Event \"{0}\" {1} in \"{2}\": {3}").FormatWith(Name, timeString, Location, waypoint), 
				EventChatFormat.WithTime => _translationService.GetTranslation("event-chatText-format-withTime", "Event \"{0}\" {1}: {2}").FormatWith(Name, timeString, waypoint), 
				EventChatFormat.WithLocation => _translationService.GetTranslation("event-chatText-format-withLocation", "Event \"{0}\" in \"{1}\": {2}").FormatWith(Name, Location, waypoint), 
				_ => waypoint, 
			};
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
