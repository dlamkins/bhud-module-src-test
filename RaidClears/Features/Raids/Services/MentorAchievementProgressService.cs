using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
		private sealed class AchievementDefinitionDto
		{
			[JsonProperty("id")]
			public int Id { get; set; }

			[JsonProperty("tiers")]
			public List<AchievementTierDto> Tiers { get; set; } = new List<AchievementTierDto>();

		}

		private sealed class AchievementTierDto
		{
			[JsonProperty("count")]
			public int Count { get; set; }
		}

		private const string CacheFilename = "mentor_achievement_progress.json";

		private const string DefinitionCacheFilename = "mentor_achievement_definitions.json";

		private static readonly List<TokenPermission> NecessaryPermissions = new List<TokenPermission>
		{
			(TokenPermission)1,
			(TokenPermission)6
		};

		private readonly object _progressLock = new object();

		private Dictionary<int, MentorAchievementProgressEntry> _progress = new Dictionary<int, MentorAchievementProgressEntry>();

		private readonly Dictionary<int, int> _definitionMax = new Dictionary<int, int>();

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
		}

		public IReadOnlyCollection<int> GetMentorAchievementIds()
		{
			HashSet<int> ids = new HashSet<int>();
			foreach (ExpansionRaid expansion in _raidData.Expansions)
			{
				foreach (RaidWing wing in expansion.Wings)
				{
					foreach (BossEncounter encounter in wing.Encounters)
					{
						if (encounter.MentorAchievementId.HasValue)
						{
							ids.Add(encounter.MentorAchievementId.Value);
						}
					}
				}
			}
			return ids;
		}

		public void LoadCache()
		{
			LoadDefinitionCache();
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
				IReadOnlyCollection<int> mentorIds = GetMentorAchievementIds();
				Dictionary<int, MentorAchievementProgressEntry> dict = new Dictionary<int, MentorAchievementProgressEntry>();
				foreach (MentorAchievementProgressEntry entry in cache.Achievements)
				{
					if (mentorIds.Contains(entry.Id))
					{
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
				await Task.Run(delegate
				{
					EnsureDefinitions(mentorIds);
				});
				List<AccountAchievement> obj = ((IEnumerable<AccountAchievement>)(await ((IBlobClient<IApiV2ObjectList<AccountAchievement>>)(object)gw2ApiManager.get_Gw2ApiClient().get_V2().get_Account()
					.get_Achievements()).GetAsync(default(CancellationToken))))?.ToList() ?? new List<AccountAchievement>();
				Dictionary<int, MentorAchievementProgressEntry> newProgress = new Dictionary<int, MentorAchievementProgressEntry>();
				foreach (AccountAchievement ach in obj)
				{
					if (mentorIds.Contains(ach.get_Id()))
					{
						int defMax;
						int maxFromDef = (_definitionMax.TryGetValue(ach.get_Id(), out defMax) ? defMax : ach.get_Max());
						newProgress[ach.get_Id()] = new MentorAchievementProgressEntry
						{
							Id = ach.get_Id(),
							Current = ach.get_Current(),
							Max = maxFromDef,
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

		private void LoadDefinitionCache()
		{
			string path = GetDefinitionCacheFilePath();
			if (!File.Exists(path))
			{
				return;
			}
			try
			{
				using StreamReader reader = new StreamReader(path, Encoding.UTF8);
				MentorAchievementDefinitionCache cache = JsonConvert.DeserializeObject<MentorAchievementDefinitionCache>(reader.ReadToEnd());
				if (cache?.Achievements == null || cache.Achievements.Count == 0)
				{
					return;
				}
				_definitionMax.Clear();
				foreach (MentorAchievementDefinitionEntry entry in cache.Achievements)
				{
					_definitionMax[entry.Id] = entry.Max;
				}
			}
			catch (Exception ex)
			{
				Logger.GetLogger<Module>().Warn(ex, "Failed to load mentor achievement definition cache");
			}
		}

		private void EnsureDefinitions(IReadOnlyCollection<int> mentorIds)
		{
			List<int> missing = mentorIds.Where((int id) => !_definitionMax.ContainsKey(id)).ToList();
			if (missing.Count == 0)
			{
				return;
			}
			try
			{
				using WebClient webClient = new WebClient();
				string url = "https://api.guildwars2.com/v2/achievements?ids=" + string.Join(",", missing);
				foreach (AchievementDefinitionDto def in (JsonConvert.DeserializeObject<List<AchievementDefinitionDto>>(webClient.DownloadString(url)) ?? new List<AchievementDefinitionDto>())!)
				{
					if (def.Tiers != null && def.Tiers.Count != 0)
					{
						int max = def.Tiers.Max((AchievementTierDto t) => t.Count);
						_definitionMax[def.Id] = max;
					}
				}
				SaveDefinitionCache();
			}
			catch (Exception ex)
			{
				Logger.GetLogger<Module>().Warn(ex, "Failed to download mentor achievement definitions");
			}
		}

		private void SaveDefinitionCache()
		{
			try
			{
				MentorAchievementDefinitionCache cache = new MentorAchievementDefinitionCache
				{
					UpdatedUtc = DateTime.UtcNow.ToString("o"),
					Achievements = _definitionMax.Select((KeyValuePair<int, int> kv) => new MentorAchievementDefinitionEntry
					{
						Id = kv.Key,
						Max = kv.Value
					}).ToList()
				};
				string definitionCacheFilePath = GetDefinitionCacheFilePath();
				string dir = Path.GetDirectoryName(definitionCacheFilePath);
				if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
				{
					Directory.CreateDirectory(dir);
				}
				using StreamWriter writer = new StreamWriter(definitionCacheFilePath, append: false, Encoding.UTF8);
				writer.Write(JsonConvert.SerializeObject(cache, Formatting.Indented));
			}
			catch (Exception ex)
			{
				Logger.GetLogger<Module>().Warn(ex, "Failed to save mentor achievement definition cache");
			}
		}

		private string GetDefinitionCacheFilePath()
		{
			return Path.Combine(Service.DirectoriesManager.GetFullDirectoryPath(Module.DIRECTORY_PATH), "mentor_achievement_definitions.json");
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
