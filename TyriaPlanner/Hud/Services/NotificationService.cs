using System;
using System.Media;
using System.Threading;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using TyriaPlanner.Hud.Api;
using TyriaPlanner.Hud.Settings;
using TyriaPlanner.Hud.Ui;

namespace TyriaPlanner.Hud.Services
{
	public sealed class NotificationService
	{
		private static readonly Logger Logger = Logger.GetLogger<NotificationService>();

		private readonly ToastStack _stack;

		private readonly ModuleSettings _settings;

		private readonly NotificationHistory _history;

		public NotificationService(ToastStack stack, ModuleSettings settings, NotificationHistory history)
		{
			_stack = stack;
			_settings = settings;
			_history = history;
		}

		private void RecordAndChime(string title, string subtitle, string eventType, string eventId)
		{
			_history.Record(title, subtitle, eventType, eventId);
			if (_settings.PlaySoundOnToast.get_Value())
			{
				try
				{
					SystemSounds.Asterisk.Play();
				}
				catch (Exception ex)
				{
					Logger.Warn(ex, "System sound failed.");
				}
			}
		}

		public void PostCheckinOpenToast(MySignup signup)
		{
			string title = "Check-in time · " + Pretty(signup.Title, signup.Type);
			string subtitle = "Open the Tyria Planner app and check in";
			RecordAndChime(title, subtitle, signup.Type, signup.Id);
			_stack.Push((Container)(object)new EventToast(_settings, title, subtitle, ToastAccent.Reminder, signup.Type, signup.CommanderAccountName, voiceChannelUrl: signup.VoiceChannelUrl, eventId: signup.Id, eventBaseUrl: BuildEventUrl(signup.Id), showSqjoin: true, showJoinFromAppHint: false, isRecurring: signup.IsRecurring, onSnooze: delegate(int minutes)
			{
				ScheduleSnooze(minutes, delegate
				{
					PostCheckinOpenToast(signup);
				});
			}));
		}

		public void PostStartingToast(MySignup signup)
		{
			string title = "Starting now · " + Pretty(signup.Title, signup.Type);
			string subtitle = ((signup.GuildName != null) ? (signup.GuildName + " · commander " + Commander(signup)) : ("Public · commander " + Commander(signup)));
			RecordAndChime(title, subtitle, signup.Type, signup.Id);
			_stack.Push((Container)(object)new EventToast(_settings, title, subtitle, ToastAccent.Reminder, signup.Type, signup.CommanderAccountName, voiceChannelUrl: signup.VoiceChannelUrl, eventId: signup.Id, eventBaseUrl: BuildEventUrl(signup.Id), showSqjoin: true, showJoinFromAppHint: false, isRecurring: signup.IsRecurring, onSnooze: delegate(int minutes)
			{
				ScheduleSnooze(minutes, delegate
				{
					PostStartingToast(signup);
				});
			}));
		}

		public void PostGuildAnnouncementToast(Announcement ann)
		{
			string title = (string.IsNullOrWhiteSpace(ann.GuildTag) ? ("\ud83d\udce2 " + ann.GuildName) : ("\ud83d\udce2 [" + ann.GuildTag + "] " + ann.GuildName));
			string subtitle = (string.IsNullOrWhiteSpace(ann.Title) ? "" : ann.Title);
			string body = ann.Content ?? string.Empty;
			RecordAndChime(title, subtitle, "announcement", ann.Id);
			_stack.Push((Container)(object)new AnnouncementToast(_settings, title, subtitle, body));
		}

		public void PostNewGuildEventToast(NewGuildEvent ev)
		{
			string title = "New event · " + Pretty(ev.Title, ev.Type);
			double when = (ev.ScheduledAt - DateTime.UtcNow).TotalMinutes;
			string subtitle = ((ev.GuildName != null) ? (ev.GuildName + " · in " + FormatRelative(when) + " · " + Commander(ev)) : ("in " + FormatRelative(when) + " · " + Commander(ev)));
			RecordAndChime(title, subtitle, ev.Type, ev.Id);
			_stack.Push((Container)(object)new EventToast(_settings, title, subtitle, ToastAccent.NewEvent, ev.Type, ev.CommanderAccountName, voiceChannelUrl: ev.VoiceChannelUrl, eventId: ev.Id, eventBaseUrl: BuildEventUrl(ev.Id), showSqjoin: false, showJoinFromAppHint: true, isRecurring: ev.IsRecurring));
		}

		private static void ScheduleSnooze(int minutes, Action repost)
		{
			if (minutes <= 0)
			{
				repost();
				return;
			}
			Timer t = null;
			t = new Timer(delegate
			{
				GameService.Overlay.QueueMainThreadUpdate((Action<GameTime>)delegate
				{
					repost();
				});
				t?.Dispose();
			}, null, TimeSpan.FromMinutes(minutes), Timeout.InfiniteTimeSpan);
		}

		private static string Pretty(string title, string type)
		{
			if (!string.IsNullOrWhiteSpace(title))
			{
				return title;
			}
			return type switch
			{
				"raid" => "Raid", 
				"strike" => "Strike", 
				"fractal" => "Fractal", 
				"wvw" => "WvW", 
				"open_world" => "Open World", 
				_ => type ?? "Event", 
			};
		}

		private static string Commander(EventBase ev)
		{
			if (!string.IsNullOrWhiteSpace(ev.CommanderAccountName))
			{
				return ev.CommanderAccountName;
			}
			if (!string.IsNullOrWhiteSpace(ev.CommanderDisplayName))
			{
				return ev.CommanderDisplayName;
			}
			return ev.CommanderUsername ?? "?";
		}

		private static string FormatRelative(double minutes)
		{
			if (minutes < 1.0)
			{
				return "now";
			}
			if (minutes < 60.0)
			{
				return (int)Math.Round(minutes) + "m";
			}
			if (minutes < 1440.0)
			{
				int h = (int)(minutes / 60.0);
				int i = (int)Math.Round(minutes - (double)(h * 60));
				if (i >= 60)
				{
					h++;
					i = 0;
				}
				if (i <= 0)
				{
					return $"{h}h";
				}
				return $"{h}h {i}m";
			}
			int d = (int)(minutes / 1440.0);
			int rh = (int)Math.Round((minutes - (double)(d * 24 * 60)) / 60.0);
			if (rh >= 24)
			{
				d++;
				rh = 0;
			}
			if (rh <= 0)
			{
				return $"{d}d";
			}
			return $"{d}d {rh}h";
		}

		private static string BuildEventUrl(string id)
		{
			return "https://tyriaplanner.com/event/" + id;
		}
	}
}
