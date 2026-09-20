using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Gw2Sharp.WebApi.V2.Models;
using Quarry.Interfaces;
using Quarry.Models;
using Quarry.WikiData.Achievement;

namespace Quarry.Services
{
	public class HereService : IHereService
	{
		private static readonly string[] ExcludedGroupNames = new string[2] { "World vs. World", "Player vs. Player" };

		private const string ExcludedCategoryName = "Slayer";

		private const string LivingWorldMapCategoriesFileName = "here_map_categories.json";

		private readonly IAchievementService achievementService;

		private readonly ICurrentMapService currentMapService;

		private readonly Gw2ApiManager gw2ApiManager;

		private readonly IBitAlignmentService bitAlignmentService;

		private readonly IMarkerPackIndexService markerPackIndexService;

		private readonly INearestObjectiveService nearestObjectiveService;

		private readonly IHereExclusionService hereExclusionService;

		private readonly SettingEntry<HereGuidanceFilter> guidanceFilter;

		private readonly Logger logger;

		private readonly ContentsManager contentsManager;

		private IReadOnlyDictionary<string, IReadOnlyList<string>> livingWorldMapCategories = new Dictionary<string, IReadOnlyList<string>>();

		private readonly object cacheLock = new object();

		private int cachedMapId = int.MinValue;

		private int cachedMax = -1;

		private HereGuidanceFilter cachedFilter = (HereGuidanceFilter)(-1);

		private Task<HereResult> cachedTask;

		private readonly object anywhereCacheLock = new object();

		private int anywhereCachedMax = -1;

		private Task<IReadOnlyList<HereCandidate>> anywhereCachedTask;

		private static readonly TimeSpan InvalidateNotifyDelay = TimeSpan.FromMilliseconds(400.0);

		private int notifySequence;

		public event Action CandidatesInvalidated;

		public HereService(IAchievementService achievementService, ICurrentMapService currentMapService, Gw2ApiManager gw2ApiManager, IBitAlignmentService bitAlignmentService, IMarkerPackIndexService markerPackIndexService, INearestObjectiveService nearestObjectiveService, IHereExclusionService hereExclusionService, SettingEntry<HereGuidanceFilter> guidanceFilter, Logger logger, ContentsManager contentsManager)
		{
			this.achievementService = achievementService;
			this.currentMapService = currentMapService;
			this.gw2ApiManager = gw2ApiManager;
			this.bitAlignmentService = bitAlignmentService;
			this.markerPackIndexService = markerPackIndexService;
			this.nearestObjectiveService = nearestObjectiveService;
			this.hereExclusionService = hereExclusionService;
			this.guidanceFilter = guidanceFilter;
			this.logger = logger;
			this.contentsManager = contentsManager;
			this.achievementService.ApiAchievementsLoaded += ValidateLivingWorldMapCategories;
			this.achievementService.ApiAchievementsLoaded += InvalidateCacheAndNotify;
			this.achievementService.PlayerAchievementsLoaded += InvalidateCacheAndNotify;
			this.bitAlignmentService.AlignmentLoaded += OnAlignmentLoaded;
			this.currentMapService.Changed += InvalidateCache;
			this.markerPackIndexService.Changed += InvalidateCacheAndNotify;
			this.hereExclusionService.Changed += InvalidateCache;
			this.achievementService.PlayerAchievementsLoaded += InvalidateAnywhereCache;
			this.hereExclusionService.Changed += InvalidateAnywhereCache;
		}

		private void InvalidateCache()
		{
			lock (cacheLock)
			{
				cachedTask = null;
			}
		}

		private void OnAlignmentLoaded(int achievementId)
		{
			InvalidateCacheAndNotify();
		}

		private void InvalidateCacheAndNotify()
		{
			InvalidateCache();
			int ticket = Interlocked.Increment(ref notifySequence);
			Task.Run(async delegate
			{
				await Task.Delay(InvalidateNotifyDelay);
				if (ticket == Volatile.Read(ref notifySequence))
				{
					try
					{
						this.CandidatesInvalidated?.Invoke();
					}
					catch (Exception ex)
					{
						logger.Warn(ex, "HereService: a CandidatesInvalidated subscriber threw.");
					}
				}
			});
		}

		private void InvalidateAnywhereCache()
		{
			lock (anywhereCacheLock)
			{
				anywhereCachedTask = null;
			}
		}

		public async Task LoadAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			livingWorldMapCategories = await LoadLivingWorldMapCategoriesAsync(contentsManager, logger, cancellationToken);
			logger.Info(string.Format("HereService: loaded {0} ({1} map entries)", "here_map_categories.json", livingWorldMapCategories.Count));
		}

		private static async Task<IReadOnlyDictionary<string, IReadOnlyList<string>>> LoadLivingWorldMapCategoriesAsync(ContentsManager contentsManager, Logger logger, CancellationToken cancellationToken)
		{
			try
			{
				using Stream stream = contentsManager.GetFileStream("here_map_categories.json");
				JsonSerializerOptions options = new JsonSerializerOptions
				{
					ReadCommentHandling = JsonCommentHandling.Skip
				};
				return ((IEnumerable<KeyValuePair<string, List<string>>>)(await JsonSerializer.DeserializeAsync<Dictionary<string, List<string>>>(stream, options, cancellationToken))).ToDictionary((Func<KeyValuePair<string, List<string>>, string>)((KeyValuePair<string, List<string>> kv) => kv.Key), (Func<KeyValuePair<string, List<string>>, IReadOnlyList<string>>)((KeyValuePair<string, List<string>> kv) => kv.Value));
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Failed to load here_map_categories.json; Living World maps will fall back to name-matching (which doesn't work for them) until this is fixed.");
				return new Dictionary<string, IReadOnlyList<string>>();
			}
		}

		internal static string NormalizeCategoryName(string name)
		{
			if (name != null)
			{
				return Regex.Replace(name, "\\s+", " ").Trim();
			}
			return null;
		}

		private void ValidateLivingWorldMapCategories()
		{
			HashSet<string> knownCategoryNames = new HashSet<string>(achievementService.AchievementCategories.Select((AchievementCategory c) => NormalizeCategoryName(c.get_Name())), StringComparer.OrdinalIgnoreCase);
			foreach (KeyValuePair<string, IReadOnlyList<string>> mapEntry in livingWorldMapCategories)
			{
				foreach (string categoryName in mapEntry.Value)
				{
					if (!knownCategoryNames.Contains(NormalizeCategoryName(categoryName)))
					{
						logger.Warn("here_map_categories.json: map '" + mapEntry.Key + "' references category '" + categoryName + "', which doesn't exist in AchievementCategories. Fix the string.");
					}
				}
			}
		}

		public Task<HereResult> GetCandidatesAsync(int max, CancellationToken cancellationToken = default(CancellationToken))
		{
			int mapId = currentMapService.MapId;
			HereGuidanceFilter filter = guidanceFilter.get_Value();
			lock (cacheLock)
			{
				if (cachedTask != null && !cachedTask.IsFaulted && !cachedTask.IsCanceled && cachedMapId == mapId && cachedMax == max && cachedFilter == filter)
				{
					logger.Debug($"HereService: cache hit for map id {mapId}.");
					return cachedTask;
				}
				Task<HereResult> task = (cachedTask = ComputeCandidatesAsync(max, cancellationToken));
				cachedMapId = mapId;
				cachedMax = max;
				cachedFilter = filter;
				return task;
			}
		}

		public async Task<IReadOnlyList<HereCandidate>> GetHiddenAsync(int max, CancellationToken cancellationToken = default(CancellationToken))
		{
			List<int> hiddenIds = new List<int>(hereExclusionService.HiddenAchievementIds);
			hiddenIds.AddRange(hereExclusionService.SnoozedUntilUtc.Keys);
			if (hiddenIds.Count == 0 || achievementService.AchievementCategories == null)
			{
				return Array.Empty<HereCandidate>();
			}
			Dictionary<int, AchievementCategory> categoryByAchievementId = new Dictionary<int, AchievementCategory>();
			foreach (AchievementCategory category in achievementService.AchievementCategories)
			{
				foreach (int achievementId in category.get_Achievements())
				{
					categoryByAchievementId[achievementId] = category;
				}
			}
			IReadOnlyDictionary<int, Achievement> apiAchievements = await bitAlignmentService.GetAchievementsAsync(hiddenIds, cancellationToken);
			List<HereCandidate> result = new List<HereCandidate>();
			foreach (int id in hiddenIds)
			{
				if (!achievementService.AchievementsById.TryGetValue(id, out var wikiAchievement) || !categoryByAchievementId.TryGetValue(id, out var category2))
				{
					continue;
				}
				apiAchievements.TryGetValue(id, out var apiAchievement);
				achievementService.PlayerAchievementsById.TryGetValue(id, out var playerAchievement);
				HereCandidate obj = new HereCandidate
				{
					Achievement = wikiAchievement,
					Category = category2,
					Current = ((playerAchievement != null) ? playerAchievement.get_Current() : 0)
				};
				int max2;
				if (playerAchievement == null)
				{
					int? obj2;
					if (apiAchievement == null)
					{
						obj2 = null;
					}
					else
					{
						IReadOnlyList<AchievementTier> tiers = apiAchievement.get_Tiers();
						if (tiers == null)
						{
							obj2 = null;
						}
						else
						{
							AchievementTier obj3 = tiers.LastOrDefault();
							obj2 = ((obj3 != null) ? new int?(obj3.get_Count()) : null);
						}
					}
					int? num = obj2;
					max2 = num.GetValueOrDefault();
				}
				else
				{
					max2 = playerAchievement.get_Max();
				}
				obj.Max = max2;
				obj.AchievementPoints = ((apiAchievement == null) ? null : apiAchievement.get_Tiers()?.Sum((AchievementTier t) => t.get_Points())).GetValueOrDefault();
				result.Add(obj);
			}
			return result.OrderBy((HereCandidate c) => c.Achievement.Name).Take(max).ToList();
		}

		public Task<IReadOnlyList<HereCandidate>> GetNearlyDoneAnywhereAsync(int max, CancellationToken cancellationToken = default(CancellationToken))
		{
			lock (anywhereCacheLock)
			{
				if (anywhereCachedTask != null && anywhereCachedMax == max)
				{
					return anywhereCachedTask;
				}
				Task<IReadOnlyList<HereCandidate>> task = (anywhereCachedTask = ComputeNearlyDoneAnywhereAsync(max, cancellationToken));
				anywhereCachedMax = max;
				return task;
			}
		}

		private async Task<IReadOnlyList<HereCandidate>> ComputeNearlyDoneAnywhereAsync(int max, CancellationToken cancellationToken)
		{
			if (achievementService.PlayerAchievementsById == null || achievementService.AchievementCategories == null || achievementService.AchievementGroups == null || achievementService.Achievements == null)
			{
				return Array.Empty<HereCandidate>();
			}
			List<int> startedIds = (from kv in achievementService.PlayerAchievementsById
				where kv.Value.get_Current() > 0 && !achievementService.HasFinishedAchievement(kv.Key)
				select kv.Key).ToList();
			if (startedIds.Count == 0)
			{
				return Array.Empty<HereCandidate>();
			}
			HashSet<int> excludedCategoryIds = new HashSet<int>(achievementService.AchievementGroups.Where((AchievementGroup g) => ExcludedGroupNames.Contains(g.get_Name())).SelectMany((AchievementGroup g) => g.get_Categories()));
			Dictionary<int, AchievementCategory> categoryByAchievementId = new Dictionary<int, AchievementCategory>();
			foreach (AchievementCategory category in achievementService.AchievementCategories.Where((AchievementCategory c) => c.get_Name() != "Slayer" && !excludedCategoryIds.Contains(c.get_Id())))
			{
				foreach (int achievementId in category.get_Achievements())
				{
					categoryByAchievementId[achievementId] = category;
				}
			}
			IReadOnlyDictionary<int, Achievement> apiAchievements = await bitAlignmentService.GetAchievementsAsync(startedIds, cancellationToken);
			DateTime nowUtc = DateTime.UtcNow;
			List<HereCandidate> candidates = new List<HereCandidate>();
			foreach (int id in startedIds)
			{
				if (apiAchievements.TryGetValue(id, out var apiAchievement) && categoryByAchievementId.TryGetValue(id, out var category2) && !hereExclusionService.IsExcluded(id, nowUtc) && PassesHereRules(apiAchievement, out var wikiAchievement, out var current, out var progressMax))
				{
					candidates.Add(new HereCandidate
					{
						Achievement = wikiAchievement,
						Category = category2,
						Current = current,
						Max = progressMax,
						AchievementPoints = (apiAchievement.get_Tiers()?.Sum((AchievementTier t) => t.get_Points()) ?? 0)
					});
				}
			}
			List<HereCandidate> ranked = (from c in candidates
				orderby (double)c.Current / (double)c.Max descending, c.AchievementPoints descending
				select c).Take(max).ToList();
			logger.Debug($"HereService: Anywhere list built from {startedIds.Count} started achievement(s) -> {candidates.Count} eligible, showing {ranked.Count}.");
			return ranked;
		}

		private async Task<HereResult> ComputeCandidatesAsync(int max, CancellationToken cancellationToken)
		{
			int mapId = currentMapService.MapId;
			string mapName = currentMapService.MapName;
			if (string.IsNullOrEmpty(mapName) || achievementService.AchievementCategories == null || achievementService.AchievementGroups == null || achievementService.Achievements == null)
			{
				return new HereResult
				{
					Reason = HereResultReason.NotLoaded
				};
			}
			if (achievementService.PlayerAchievements == null)
			{
				HereResult hereResult = new HereResult();
				hereResult.Reason = (gw2ApiManager.HasPermissions((IEnumerable<TokenPermission>)(object)new TokenPermission[2]
				{
					(TokenPermission)1,
					(TokenPermission)6
				}) ? HereResultReason.NotLoaded : HereResultReason.NoPermission);
				return hereResult;
			}
			HashSet<int> excludedCategoryIds = new HashSet<int>(achievementService.AchievementGroups.Where((AchievementGroup g) => ExcludedGroupNames.Contains(g.get_Name())).SelectMany((AchievementGroup g) => g.get_Categories()));
			List<AchievementCategory> matchedCategories;
			if (livingWorldMapCategories.TryGetValue(mapName, out var configuredCategoryNames))
			{
				HashSet<string> configuredSet = new HashSet<string>(configuredCategoryNames.Select(NormalizeCategoryName), StringComparer.OrdinalIgnoreCase);
				matchedCategories = achievementService.AchievementCategories.Where((AchievementCategory c) => configuredSet.Contains(NormalizeCategoryName(c.get_Name())) && IsAllowed(c)).ToList();
			}
			else
			{
				matchedCategories = achievementService.AchievementCategories.Where((AchievementCategory c) => c.get_Name() == mapName && IsAllowed(c)).ToList();
			}
			Dictionary<int, AchievementCategory> categoryByAchievementId = new Dictionary<int, AchievementCategory>();
			foreach (AchievementCategory category in matchedCategories)
			{
				foreach (int achievementId2 in category.get_Achievements())
				{
					categoryByAchievementId[achievementId2] = category;
				}
			}
			HashSet<int> candidateIds = new HashSet<int>(categoryByAchievementId.Keys);
			HashSet<int> guidedIds = new HashSet<int>();
			if (markerPackIndexService.Ready)
			{
				Dictionary<int, AchievementCategory> categoryByAnyAchievementId = null;
				foreach (int id2 in markerPackIndexService.AchievementsOnMap(mapId))
				{
					if (!categoryByAchievementId.ContainsKey(id2))
					{
						if (categoryByAnyAchievementId == null)
						{
							categoryByAnyAchievementId = new Dictionary<int, AchievementCategory>();
							foreach (AchievementCategory anyCategory in achievementService.AchievementCategories.Where(IsAllowed))
							{
								foreach (int achievementId in anyCategory.get_Achievements())
								{
									categoryByAnyAchievementId[achievementId] = anyCategory;
								}
							}
						}
						if (!categoryByAnyAchievementId.TryGetValue(id2, out var category2))
						{
							continue;
						}
						categoryByAchievementId[id2] = category2;
						candidateIds.Add(id2);
					}
					guidedIds.Add(id2);
				}
			}
			if (candidateIds.Count == 0)
			{
				return new HereResult
				{
					Reason = HereResultReason.NoCategoryForMap,
					IndexReady = markerPackIndexService.Ready
				};
			}
			bool categorySupported = matchedCategories.Count > 0;
			(IReadOnlyDictionary<int, Achievement>, bool) obj = await FetchAchievementsAsync(candidateIds.ToList(), cancellationToken);
			IReadOnlyDictionary<int, Achievement> apiAchievements = obj.Item1;
			bool partial = obj.Item2;
			List<HereCandidate> candidates = new List<HereCandidate>();
			foreach (int id in candidateIds)
			{
				if (apiAchievements.TryGetValue(id, out var apiAchievement) && PassesHereRules(apiAchievement, out var wikiAchievement, out var current, out var progressMax))
				{
					GuidanceInfo guidance = nearestObjectiveService.GetGuidance(id, mapId);
					bool hasBits = apiAchievement.get_Bits() != null && apiAchievement.get_Bits().Count > 0;
					candidates.Add(new HereCandidate
					{
						Achievement = wikiAchievement,
						Category = categoryByAchievementId[id],
						Current = current,
						Max = progressMax,
						AchievementPoints = (apiAchievement.get_Tiers()?.Sum((AchievementTier t) => t.get_Points()) ?? 0),
						Guided = guidedIds.Contains(id),
						Guidance = guidance,
						Opportunistic = (!hasBits && guidance.Tier == GuidanceTier.None)
					});
				}
			}
			DateTime nowUtc = DateTime.UtcNow;
			List<HereCandidate> excludedCandidates = candidates.Where((HereCandidate c) => hereExclusionService.IsExcluded(c.Achievement.Id, nowUtc)).ToList();
			if (excludedCandidates.Count > 0)
			{
				candidates = candidates.Where((HereCandidate c) => !hereExclusionService.IsExcluded(c.Achievement.Id, nowUtc)).ToList();
			}
			List<HereCandidate> opportunistic = candidates.Where((HereCandidate c) => c.Opportunistic).ToList();
			if (opportunistic.Count > 0)
			{
				candidates = candidates.Where((HereCandidate c) => !c.Opportunistic).ToList();
			}
			GuidanceTier minimumTier = (GuidanceTier)guidanceFilter.get_Value();
			int beforeFilter = candidates.Count;
			if (minimumTier > GuidanceTier.None)
			{
				candidates = candidates.Where((HereCandidate c) => c.Guidance.Tier >= minimumTier).ToList();
			}
			List<HereCandidate> rankedUncapped = Rank(candidates).ToList();
			List<HereCandidate> ranked = rankedUncapped.Take(max).ToList();
			return new HereResult
			{
				Reason = HereResultReason.Ok,
				Candidates = ranked,
				RankedUncapped = rankedUncapped,
				Partial = partial,
				CategorySupported = categorySupported,
				FilteredByGuidance = beforeFilter - candidates.Count,
				HiddenCount = excludedCandidates.Count,
				Opportunistic = opportunistic.OrderByDescending((HereCandidate c) => (double)c.Current / (double)c.Max).ToList()
			};
			bool IsAllowed(AchievementCategory c)
			{
				if (c.get_Name() != "Slayer")
				{
					return !excludedCategoryIds.Contains(c.get_Id());
				}
				return false;
			}
		}

		private static IOrderedEnumerable<HereCandidate> Rank(IEnumerable<HereCandidate> candidates)
		{
			return from c in candidates
				orderby (double)c.Current / (double)c.Max descending, c.AchievementPoints descending, c.Guidance.Tier descending
				select c;
		}

		private bool PassesHereRules(Achievement apiAchievement, out AchievementTableEntry wikiAchievement, out int current, out int progressMax)
		{
			wikiAchievement = null;
			current = 0;
			progressMax = 0;
			int id = apiAchievement.get_Id();
			List<AchievementFlag> flags = ((IEnumerable<ApiEnum<AchievementFlag>>)apiAchievement.get_Flags()).Select((ApiEnum<AchievementFlag> f) => f.get_Value()).ToList();
			if (flags.Contains((AchievementFlag)4) || flags.Contains((AchievementFlag)2) || flags.Contains((AchievementFlag)5) || flags.Contains((AchievementFlag)1))
			{
				return false;
			}
			achievementService.PlayerAchievementsById.TryGetValue(id, out var playerAchievement);
			if (achievementService.HasFinishedAchievement(id))
			{
				return false;
			}
			if (flags.Contains((AchievementFlag)6) && (playerAchievement == null || playerAchievement.get_Unlocked() == false))
			{
				return false;
			}
			if (flags.Contains((AchievementFlag)7) && (playerAchievement == null || !playerAchievement.get_Unlocked().GetValueOrDefault()))
			{
				return false;
			}
			if (HasUnmetPrerequisite(apiAchievement))
			{
				return false;
			}
			if (!achievementService.AchievementsById.TryGetValue(id, out wikiAchievement))
			{
				return false;
			}
			current = ((playerAchievement != null) ? playerAchievement.get_Current() : 0);
			int num2;
			if (playerAchievement == null)
			{
				IReadOnlyList<AchievementTier> tiers = apiAchievement.get_Tiers();
				int? obj;
				if (tiers == null)
				{
					obj = null;
				}
				else
				{
					AchievementTier obj2 = tiers.LastOrDefault();
					obj = ((obj2 != null) ? new int?(obj2.get_Count()) : null);
				}
				int? num = obj;
				num2 = num.GetValueOrDefault();
			}
			else
			{
				num2 = playerAchievement.get_Max();
			}
			progressMax = num2;
			return progressMax > 0;
		}

		private bool HasUnmetPrerequisite(Achievement apiAchievement)
		{
			if (apiAchievement.get_Prerequisites() == null)
			{
				return false;
			}
			foreach (int prerequisiteId in apiAchievement.get_Prerequisites())
			{
				if (!achievementService.PlayerAchievementsById.TryGetValue(prerequisiteId, out var prerequisite) || !prerequisite.get_Done())
				{
					return true;
				}
			}
			return false;
		}

		private async Task<(IReadOnlyDictionary<int, Achievement> Results, bool Partial)> FetchAchievementsAsync(IReadOnlyList<int> ids, CancellationToken cancellationToken)
		{
			IReadOnlyDictionary<int, Achievement> obj = await bitAlignmentService.GetAchievementsAsync(ids, cancellationToken);
			return (obj, obj.Count < ids.Count);
		}
	}
}
