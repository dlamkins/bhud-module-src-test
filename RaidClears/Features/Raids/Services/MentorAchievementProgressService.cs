using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using Newtonsoft.Json;
using RaidClears.Features.Raids.Models;
using RaidClears.Features.Shared.Models;
using RaidClears.Settings.Services;

namespace RaidClears.Features.Raids.Services
{
	public class MentorAchievementProgressService
	{
		private const string CacheFilename = "mentor_achievement_progress.json";

		public const int DefaultMentorAchievementMax = 1000;

		private static readonly List<TokenPermission> NecessaryPermissions = new List<TokenPermission>
		{
			(TokenPermission)1,
			(TokenPermission)6
		};

		private readonly object _progressLock = new object();

		private Dictionary<int, MentorAchievementProgressEntry> _progress = new Dictionary<int, MentorAchievementProgressEntry>();

		private readonly Dictionary<int, int> _mentorAchievementMax = new Dictionary<int, int>();

		private readonly RaidData _raidData;

		public IReadOnlyDictionary<int, MentorAchievementProgressEntry> Progress
		{
			get
			{
				lock (_progressLock)
				{
					return new Dictionary<int, MentorAchievementProgressEntry>(_progress);
				}
			}
		}

		public event EventHandler<MentorProgressUpdatedEventArgs>? ProgressUpdated;

		public MentorAchievementProgressService(RaidData raidData)
		{
			_raidData = raidData ?? throw new ArgumentNullException("raidData");
			LoadMentorAchievementMax();
		}

		private void LoadMentorAchievementMax()
		{
			_mentorAchievementMax.Clear();
			foreach (ExpansionRaid expansion in _raidData.Expansions)
			{
				foreach (RaidWing wing in expansion.Wings)
				{
					foreach (BossEncounter encounter in wing.Encounters)
					{
						if (encounter.MentorAchievementId.HasValue)
						{
							int id = encounter.MentorAchievementId.Value;
							int max = encounter.MentorAchievementMax.GetValueOrDefault(1000);
							if (max <= 0)
							{
								max = 1000;
							}
							_mentorAchievementMax[id] = max;
						}
					}
				}
			}
		}

		public IReadOnlyCollection<int> GetMentorAchievementIds()
		{
			return _mentorAchievementMax.Keys.ToList();
		}

		public void LoadCache()
		{
			string path = GetCacheFilePath();
			if (!File.Exists(path))
			{
				return;
			}
			try
			{
				using StreamReader reader = new StreamReader(path, Encoding.UTF8);
				MentorAchievementProgressCache cache = JsonConvert.DeserializeObject<MentorAchievementProgressCache>(reader.ReadToEnd());
				if (cache?.Achievements == null || cache.Achievements.Count == 0)
				{
					return;
				}
				Dictionary<int, MentorAchievementProgressEntry> dict = new Dictionary<int, MentorAchievementProgressEntry>();
				foreach (MentorAchievementProgressEntry entry in cache.Achievements)
				{
					if (_mentorAchievementMax.TryGetValue(entry.Id, out var max))
					{
						entry.Max = max;
						dict[entry.Id] = entry;
					}
				}
				lock (_progressLock)
				{
					_progress = dict;
				}
			}
			catch (Exception ex)
			{
				Logger.GetLogger<Module>().Warn(ex, "Failed to load mentor achievement progress cache");
			}
		}

		public async Task RefreshFromApiAsync()
		{
			SettingService settings = Service.Settings;
			if (settings == null || !(settings.RaidSettings?.RaidPanelMentorProgress?.get_Value()).GetValueOrDefault())
			{
				return;
			}
			Gw2ApiManager gw2ApiManager = Service.Gw2ApiManager;
			Logger logger = Logger.GetLogger<Module>();
			if (gw2ApiManager == null || !gw2ApiManager.HasPermissions((IEnumerable<TokenPermission>)NecessaryPermissions))
			{
				return;
			}
			IReadOnlyCollection<int> mentorIds = GetMentorAchievementIds();
			if (mentorIds.Count == 0)
			{
				return;
			}
			try
			{
				List<AccountAchievement> obj = ((IEnumerable<AccountAchievement>)(await ((IBlobClient<IApiV2ObjectList<AccountAchievement>>)(object)gw2ApiManager.get_Gw2ApiClient().get_V2().get_Account()
					.get_Achievements()).GetAsync(default(CancellationToken))))?.ToList() ?? new List<AccountAchievement>();
				Dictionary<int, MentorAchievementProgressEntry> newProgress = new Dictionary<int, MentorAchievementProgressEntry>();
				foreach (AccountAchievement ach in obj)
				{
					if (mentorIds.Contains(ach.get_Id()))
					{
						int defMax;
						int maxFromStatic = ((_mentorAchievementMax.TryGetValue(ach.get_Id(), out defMax) && defMax > 0) ? defMax : ach.get_Max());
						newProgress[ach.get_Id()] = new MentorAchievementProgressEntry
						{
							Id = ach.get_Id(),
							Current = ach.get_Current(),
							Max = maxFromStatic,
							Done = ach.get_Done()
						};
					}
				}
				List<MentorProgressChange> increases = null;
				bool changed = false;
				lock (_progressLock)
				{
					if (!ProgressEquals(_progress, newProgress))
					{
						increases = new List<MentorProgressChange>();
						foreach (KeyValuePair<int, MentorAchievementProgressEntry> kv in newProgress)
						{
							if (_progress.TryGetValue(kv.Key, out var old) && kv.Value.Current > old.Current)
							{
								increases.Add(new MentorProgressChange
								{
									AchievementId = kv.Key,
									PreviousCurrent = old.Current,
									NewCurrent = kv.Value.Current
								});
							}
						}
						_progress = newProgress;
						changed = true;
					}
				}
				if (changed)
				{
					SaveCache(newProgress);
					if (increases != null && increases.Count > 0)
					{
						this.ProgressUpdated?.Invoke(this, new MentorProgressUpdatedEventArgs
						{
							Changes = increases
						});
					}
				}
			}
			catch (Exception ex)
			{
				logger.Warn(ex, "Failed to fetch mentor achievement progress from API");
			}
		}

		private static bool ProgressEquals(Dictionary<int, MentorAchievementProgressEntry> a, Dictionary<int, MentorAchievementProgressEntry> b)
		{
			if (a.Count != b.Count)
			{
				return false;
			}
			foreach (KeyValuePair<int, MentorAchievementProgressEntry> kv in a)
			{
				if (!b.TryGetValue(kv.Key, out var entry) || !kv.Value.Equals(entry))
				{
					return false;
				}
			}
			return true;
		}

		private void SaveCache(Dictionary<int, MentorAchievementProgressEntry> progress)
		{
			try
			{
				MentorAchievementProgressCache cache = new MentorAchievementProgressCache
				{
					UpdatedUtc = DateTime.UtcNow.ToString("o"),
					Achievements = progress.Values.ToList()
				};
				string cacheFilePath = GetCacheFilePath();
				string dir = Path.GetDirectoryName(cacheFilePath);
				if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
				{
					Directory.CreateDirectory(dir);
				}
				using StreamWriter writer = new StreamWriter(cacheFilePath, append: false, Encoding.UTF8);
				writer.Write(JsonConvert.SerializeObject(cache, Formatting.Indented));
			}
			catch (Exception ex)
			{
				Logger.GetLogger<Module>().Warn(ex, "Failed to save mentor achievement progress cache");
			}
		}

		private string GetCacheFilePath()
		{
			return Path.Combine(Service.DirectoriesManager.GetFullDirectoryPath(Module.DIRECTORY_PATH), "mentor_achievement_progress.json");
		}
	}
}
