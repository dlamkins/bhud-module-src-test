using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Modules.Managers;
using Events_Module.Properties;
using Flurl;
using Flurl.Http;
using Humanizer;
using Humanizer.Localisation;
using Newtonsoft.Json;

namespace Events_Module
{
	[JsonObject]
	public class Meta
	{
		[JsonObject]
		public struct Phase
		{
			public string Name { get; set; }

			public int Duration { get; set; }
		}

		private static readonly Logger Logger = Logger.GetLogger<Meta>();

		public static List<Meta> Events;

		protected List<DateTime> _times = new List<DateTime>();

		private DateTime _nextTime;

		[JsonIgnore]
		public bool IsWatched;

		[JsonIgnore]
		protected bool HasAlerted;

		private string _icon;

		private string _wikiEn;

		private Dictionary<string, string> _wikiLinks;

		public string Name { get; set; }

		public string Colloquial { get; set; }

		public string Category { get; set; }

		public DateTime Offset { get; set; }

		public string Difficulty { get; set; }

		public string Location { get; set; }

		public string Waypoint { get; set; }

		public string Wiki
		{
			get
			{
				string lang = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
				if (!(_wikiLinks?.ContainsKey(lang) ?? false))
				{
					return _wikiEn;
				}
				return _wikiLinks[lang];
			}
			set
			{
				_wikiEn = value;
			}
		}

		public int? Duration { get; set; }

		[JsonProperty(PropertyName = "Alert")]
		public int? Reminder { get; set; }

		[JsonProperty(PropertyName = "Repeat")]
		public TimeSpan? RepeatInterval { get; set; }

		public IReadOnlyList<DateTime> Times => _times;

		public Phase[] Phases { get; set; }

		public DateTime NextTime
		{
			get
			{
				return _nextTime;
			}
			protected set
			{
				if (!(_nextTime == value))
				{
					_nextTime = value;
					this.OnNextRunTimeChanged?.Invoke(this, EventArgs.Empty);
				}
			}
		}

		public string Icon
		{
			get
			{
				return _icon;
			}
			set
			{
				if (!(_icon == value))
				{
					_icon = value;
					if (!string.IsNullOrEmpty(_icon))
					{
						Texture = GameService.Content.GetRenderServiceTexture(_icon);
					}
				}
			}
		}

		[JsonIgnore]
		public AsyncTexture2D Texture { get; private set; } = new AsyncTexture2D(GameService.Content.GetTexture("102377"));


		public event EventHandler<EventArgs> OnNextRunTimeChanged;

		public static void UpdateEventSchedules()
		{
			if (Events == null)
			{
				return;
			}
			TimeSpan tsNow = DateTime.Now.ToLocalTime().TimeOfDay;
			foreach (Meta e in Events)
			{
				TimeSpan[] justTimes = (from time in e.Times
					select time.ToLocalTime().TimeOfDay into time
					orderby time.TotalSeconds
					select time).ToArray();
				TimeSpan nextTime = justTimes.FirstOrDefault((TimeSpan ts) => ts.TotalSeconds >= tsNow.TotalSeconds);
				if (nextTime.Ticks == 0L)
				{
					e.NextTime = DateTime.Today.AddDays(1.0) + justTimes[0];
				}
				else
				{
					e.NextTime = DateTime.Today + nextTime;
				}
				double timeUntil = (e.NextTime - DateTime.Now).TotalMinutes;
				if (timeUntil < (double)(e.Reminder ?? (-1)) && e.IsWatched)
				{
					if (!e.HasAlerted && EventsModule.ModuleInstance.NotificationsEnabled)
					{
						EventNotification.ShowNotification(Resources.ResourceManager.GetString(e.Name) ?? e.Name, e.Texture, string.Format(Resources.Starts_in__0_, TimeSpanHumanizeExtensions.Humanize(NumberToTimeSpanExtensions.Minutes(timeUntil), 1, (CultureInfo)null, (TimeUnit)5, (TimeUnit)0, ", ", false)), 10f, e.Waypoint);
						e.HasAlerted = true;
					}
				}
				else
				{
					e.HasAlerted = false;
				}
			}
		}

		public static async Task Load(ContentsManager cm)
		{
			List<Meta> metas = null;
			try
			{
				using StreamReader eventsReader = new StreamReader(cm.GetFileStream("events.json"));
				metas = JsonConvert.DeserializeObject<List<Meta>>(await eventsReader.ReadToEndAsync());
			}
			catch (Exception e)
			{
				Logger.Error(e, Resources.Failed_to_load_metas_from_events_json_);
			}
			if (metas == null)
			{
				return;
			}
			List<Meta> uniqueEvents = new List<Meta>();
			List<Task> wikiTasks = new List<Task>();
			foreach (Meta meta in metas)
			{
				meta._times.Add(meta.Offset);
				if (meta.RepeatInterval.HasValue && meta.RepeatInterval.Value.TotalSeconds > 0.0)
				{
					double dailyMinutes = 1440.0 - meta.RepeatInterval.Value.TotalMinutes;
					DateTime lastTime = meta.Offset;
					while (dailyMinutes > 0.0)
					{
						DateTime intervalTime = lastTime.Add(meta.RepeatInterval.Value);
						meta._times.Add(intervalTime);
						lastTime = intervalTime;
						dailyMinutes -= meta.RepeatInterval.Value.TotalMinutes;
					}
				}
				Meta rootEvent = uniqueEvents.Find((Meta m) => m.Name == meta.Name && m.Category == meta.Category);
				if (rootEvent != null)
				{
					rootEvent._times.AddRange(meta.Times);
				}
				else
				{
					uniqueEvents.Add(meta);
				}
				if (!string.IsNullOrEmpty(meta._wikiEn))
				{
					Task<Task<Dictionary<string, string>>> task = GetInterwikiLinks(new Uri(meta._wikiEn).Segments.Last()).ContinueWith(async delegate(Task<Dictionary<string, string>> v)
					{
						Meta meta2 = meta;
						return meta2._wikiLinks = await v;
					});
					wikiTasks.Add(task);
				}
			}
			await Task.WhenAll(wikiTasks.ToArray());
			Events = uniqueEvents;
			Logger.Info("Loaded {eventCount} events.", new object[1] { Events.Count });
			UpdateEventSchedules();
		}

		[Localizable(false)]
		private static async Task<Dictionary<string, string>> GetInterwikiLinks(string page)
		{
			dynamic wikiPage = ((dynamic)(await GeneratedExtensions.GetJsonAsync(StringExtensions.AppendPathSegment("https://wiki.guildwars2.com", (object)"api.php", false).SetQueryParams((object)new
			{
				action = "query",
				format = "json",
				prop = "langlinks",
				titles = page,
				redirects = 1,
				converttitles = 1,
				formatversion = 2,
				llprop = "url"
			}, (NullValueHandling)1), default(CancellationToken), (HttpCompletionOption)0))).query.pages[0];
			if (((IDictionary<string, object>)wikiPage).ContainsKey("langlinks"))
			{
				Dictionary<string, string> links = new Dictionary<string, string>();
				foreach (dynamic link in wikiPage.langlinks)
				{
					links.Add(link.lang, ((string)link.url).Replace("http://", "https://"));
				}
				return links;
			}
			return null;
		}
	}
}
