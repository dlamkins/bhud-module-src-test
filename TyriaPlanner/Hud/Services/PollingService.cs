using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using TyriaPlanner.Hud.Api;
using TyriaPlanner.Hud.Settings;

namespace TyriaPlanner.Hud.Services
{
	public sealed class PollingService : IDisposable
	{
		private static readonly Logger Logger = Logger.GetLogger<PollingService>();

		private readonly ApiClient _api;

		private readonly ModuleSettings _settings;

		private readonly NotificationService _notify;

		private CancellationTokenSource _cancel;

		private Task _loop;

		private readonly HashSet<string> _shownReminders = new HashSet<string>();

		private readonly HashSet<string> _shownGuildEvents = new HashSet<string>();

		private readonly HashSet<string> _shownAnnouncements = new HashSet<string>();

		private DateTimeOffset? _lastSeen;

		public PollingService(ApiClient api, ModuleSettings settings, NotificationService notify)
		{
			_api = api;
			_settings = settings;
			_notify = notify;
		}

		public void Start()
		{
			Stop();
			_cancel = new CancellationTokenSource();
			_loop = Task.Run(() => RunAsync(_cancel.Token));
		}

		public void Stop()
		{
			if (_cancel != null)
			{
				_cancel.Cancel();
				_cancel.Dispose();
				_cancel = null;
			}
			_loop = null;
		}

		public void RefreshNow()
		{
			if (_cancel == null)
			{
				return;
			}
			CancellationToken cancel = _cancel.Token;
			Task.Run(async delegate
			{
				try
				{
					await PollOnceAsync(cancel).ConfigureAwait(continueOnCapturedContext: false);
				}
				catch (TaskCanceledException)
				{
				}
				catch (Exception ex)
				{
					Logger.Warn(ex, "Stream-triggered refresh failed.");
				}
			});
		}

		private async Task RunAsync(CancellationToken cancel)
		{
			while (!cancel.IsCancellationRequested)
			{
				try
				{
					await PollOnceAsync(cancel).ConfigureAwait(continueOnCapturedContext: false);
				}
				catch (TaskCanceledException)
				{
				}
				catch (Exception ex)
				{
					Logger.Warn(ex, "Tyria Planner poll failed; will retry next cycle.");
				}
				int delaySeconds = Math.Max(30, _settings.PollIntervalSeconds.get_Value());
				try
				{
					await Task.Delay(TimeSpan.FromSeconds(delaySeconds), cancel).ConfigureAwait(continueOnCapturedContext: false);
				}
				catch (TaskCanceledException)
				{
					return;
				}
			}
		}

		private async Task PollOnceAsync(CancellationToken cancel)
		{
			string baseUrl = _settings.ApiBaseUrl.get_Value();
			string gw2Key = _settings.Gw2ApiKey.get_Value();
			if (string.IsNullOrWhiteSpace(gw2Key) || string.IsNullOrWhiteSpace(baseUrl))
			{
				return;
			}
			string bearer = _settings.CachedBearer.get_Value();
			if (string.IsNullOrWhiteSpace(bearer))
			{
				bearer = await _api.ExchangeAsync(baseUrl, gw2Key, cancel).ConfigureAwait(continueOnCapturedContext: false);
				if (string.IsNullOrWhiteSpace(bearer))
				{
					return;
				}
				_settings.CachedBearer.set_Value(bearer);
			}
			UpcomingResponse resp = await _api.FetchUpcomingAsync(baseUrl, bearer, _lastSeen, cancel).ConfigureAwait(continueOnCapturedContext: false);
			if (resp == null)
			{
				_settings.CachedBearer.set_Value(string.Empty);
				string fresh = await _api.ExchangeAsync(baseUrl, gw2Key, cancel).ConfigureAwait(continueOnCapturedContext: false);
				if (string.IsNullOrWhiteSpace(fresh))
				{
					return;
				}
				_settings.CachedBearer.set_Value(fresh);
				resp = await _api.FetchUpcomingAsync(baseUrl, fresh, _lastSeen, cancel).ConfigureAwait(continueOnCapturedContext: false);
				if (resp == null)
				{
					return;
				}
			}
			_lastSeen = resp.ServerTime;
			if (_settings.NotifyOwnSignups.get_Value() && resp.MySignups != null)
			{
				MySignup[] mySignups = resp.MySignups;
				foreach (MySignup signup in mySignups)
				{
					HandleSignupTriggers(signup);
				}
			}
			if (_settings.NotifyNewGuildEvents.get_Value() && resp.NewGuildEvents != null)
			{
				NewGuildEvent[] newGuildEvents = resp.NewGuildEvents;
				foreach (NewGuildEvent ev in newGuildEvents)
				{
					if (_shownGuildEvents.Add(ev.Id))
					{
						_notify.PostNewGuildEventToast(ev);
					}
				}
			}
			if (!_settings.NotifyGuildAnnouncements.get_Value() || resp.NewAnnouncements == null)
			{
				return;
			}
			Announcement[] newAnnouncements = resp.NewAnnouncements;
			foreach (Announcement ann in newAnnouncements)
			{
				if (_shownAnnouncements.Add(ann.Id))
				{
					_notify.PostGuildAnnouncementToast(ann);
				}
			}
		}

		private void HandleSignupTriggers(MySignup signup)
		{
			double minutesUntil = (signup.ScheduledAt - DateTime.UtcNow).TotalMinutes;
			if (minutesUntil <= 1.0 && minutesUntil > -2.0)
			{
				string key2 = signup.Id + "|starting";
				if (_shownReminders.Add(key2))
				{
					_notify.PostStartingToast(signup);
					Logger.Info("Starting toast · event={0} minutesUntil={1:0.#}", new object[2] { signup.Title, minutesUntil });
				}
				return;
			}
			int checkinAt = signup.CheckinReminderMinutes.GetValueOrDefault(30);
			if (checkinAt > 0 && minutesUntil <= (double)checkinAt && minutesUntil > 1.0)
			{
				string key = signup.Id + "|checkin";
				if (_shownReminders.Add(key))
				{
					_notify.PostCheckinOpenToast(signup);
					Logger.Info("Check-in toast · event={0} minutesUntil={1:0.#} window={2}", new object[3] { signup.Title, minutesUntil, checkinAt });
				}
			}
		}

		public void Dispose()
		{
			Stop();
		}
	}
}
