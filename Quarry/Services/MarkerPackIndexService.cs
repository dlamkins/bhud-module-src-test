using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Debug;
using Blish_HUD.Modules.Managers;
using Quarry.Interfaces;
using Quarry.Models.Markers;
using TmfLib;
using TmfLib.Pathable;
using TmfLib.Prototype;
using TmfLib.Reader;

namespace Quarry.Services
{
	public class MarkerPackIndexService : IMarkerPackIndexService
	{
		private class MutablePackCollection : IPackCollection
		{
			public PathingCategory Categories { get; } = new PathingCategory(root: true);


			public IList<PointOfInterest> PointsOfInterest { get; } = new List<PointOfInterest>();

		}

		private const string CacheFileName = "markerPackIndex.json";

		private const int CacheSchema = 5;

		private const float TrailSampleSpacingMetres = 25f;

		private const int TrailMaxSamplesPerSection = 64;

		private static readonly Regex WaypointCodeRegex = new Regex("\\[&[A-Za-z0-9+/=]+\\]", RegexOptions.Compiled);

		private static readonly string[] WaypointAttributeNames = new string[2] { "copy", "copy-message" };

		private readonly DirectoriesManager directoriesManager;

		private readonly Logger logger;

		private readonly object indexLock = new object();

		private IReadOnlyDictionary<int, AchievementRoute> achievementsById = new Dictionary<int, AchievementRoute>();

		private IReadOnlyDictionary<int, IReadOnlyCollection<int>> achievementIdsByMapId = new Dictionary<int, IReadOnlyCollection<int>>();

		public bool Ready { get; private set; }

		public event Action Changed;

		public MarkerPackIndexService(DirectoriesManager directoriesManager, Logger logger)
		{
			this.directoriesManager = directoriesManager;
			this.logger = logger;
		}

		public bool TryGet(int achievementId, out AchievementRoute route)
		{
			lock (indexLock)
			{
				return achievementsById.TryGetValue(achievementId, out route);
			}
		}

		public IReadOnlyCollection<int> AchievementsOnMap(int mapId)
		{
			lock (indexLock)
			{
				IReadOnlyCollection<int> result;
				if (!achievementIdsByMapId.TryGetValue(mapId, out var ids))
				{
					IReadOnlyCollection<int> readOnlyCollection = (IReadOnlyCollection<int>)(object)Array.Empty<int>();
					result = readOnlyCollection;
				}
				else
				{
					result = ids;
				}
				return result;
			}
		}

		public async Task LoadAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			try
			{
				await LoadCoreAsync(cancellationToken);
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex)
			{
				logger.Warn(ex, "MarkerPackIndexService: indexing failed; hunt routes and nearest-objective distances will be unavailable this session.");
			}
		}

		private async Task LoadCoreAsync(CancellationToken cancellationToken)
		{
			Stopwatch stopwatch = Stopwatch.StartNew();
			string markersDir = directoriesManager.GetFullDirectoryPath("markers");
			Directory.CreateDirectory(markersDir);
			List<string> packFiles = Directory.EnumerateFiles(markersDir, "*.taco", SearchOption.AllDirectories).Concat(Directory.EnumerateFiles(markersDir, "*.zip", SearchOption.AllDirectories)).ToList();
			List<PackCacheEntry> packCacheEntries = (from f in packFiles
				select new FileInfo(f) into fi
				select new PackCacheEntry
				{
					FileName = fi.FullName,
					Length = fi.Length,
					LastWriteTimeUtc = fi.LastWriteTimeUtc
				}).OrderBy((PackCacheEntry e) => e.FileName, StringComparer.OrdinalIgnoreCase).ToList();
			string cacheFilePath = GetCacheFilePath();
			MarkerPackIndexCacheFile cached = TryLoadCache(cacheFilePath);
			if (cached != null && PacksMatch(cached.Packs, packCacheEntries))
			{
				ApplyIndex(cached.AchievementsById);
				logger.Info($"MarkerPackIndexService: cache hit for {packCacheEntries.Count} pack(s) ({cached.AchievementsById.Count} achievement(s)), {stopwatch.ElapsedMilliseconds} ms.");
				return;
			}
			Dictionary<int, AchievementRoute> achievementsById = new Dictionary<int, AchievementRoute>();
			PackReaderSettings settings = new PackReaderSettings
			{
				VenderPrefixes = { "bh-" }
			};
			foreach (string file in packFiles)
			{
				cancellationToken.ThrowIfCancellationRequested();
				await IndexPackAsync(() => Pack.FromArchivedMarkerPack(file), Path.GetFileName(file), settings, achievementsById, cancellationToken);
			}
			await IndexPackAsync(() => Pack.FromDirectoryMarkerPack(markersDir), markersDir + " (unpacked)", settings, achievementsById, cancellationToken, quietIfEmpty: true);
			ApplyIndex(achievementsById);
			try
			{
				MarkerPackIndexCacheFile cacheFile = new MarkerPackIndexCacheFile
				{
					Schema = 5,
					Packs = packCacheEntries,
					AchievementsById = achievementsById
				};
				Directory.CreateDirectory(Path.GetDirectoryName(cacheFilePath));
				File.WriteAllText(cacheFilePath, JsonSerializer.Serialize(cacheFile));
			}
			catch (UnauthorizedAccessException ex2)
			{
				logger.Warn((Exception)ex2, "MarkerPackIndexService: access denied writing markerPackIndex.json; the index will rebuild every load until this is fixed.");
				Contingency.NotifyFileSaveAccessDenied(cacheFilePath, "cache Quarry's marker-pack index", false);
			}
			catch (Exception ex)
			{
				logger.Warn(ex, "MarkerPackIndexService: failed to write markerPackIndex.json; the index will rebuild every load until this is fixed.");
			}
			logger.Info($"MarkerPackIndexService: indexed {packFiles.Count} pack(s), {achievementsById.Count} achievement(s) total, {stopwatch.ElapsedMilliseconds} ms.");
		}

		private void ApplyIndex(IReadOnlyDictionary<int, AchievementRoute> newAchievementsById)
		{
			Dictionary<int, HashSet<int>> byMapId = new Dictionary<int, HashSet<int>>();
			foreach (KeyValuePair<int, AchievementRoute> entry in newAchievementsById)
			{
				foreach (int mapId in entry.Value.MapIds)
				{
					if (!byMapId.TryGetValue(mapId, out var ids))
					{
						ids = (byMapId[mapId] = new HashSet<int>());
					}
					ids.Add(entry.Key);
				}
			}
			lock (indexLock)
			{
				achievementsById = newAchievementsById;
				achievementIdsByMapId = ((IEnumerable<KeyValuePair<int, HashSet<int>>>)byMapId).ToDictionary((Func<KeyValuePair<int, HashSet<int>>, int>)((KeyValuePair<int, HashSet<int>> kv) => kv.Key), (Func<KeyValuePair<int, HashSet<int>>, IReadOnlyCollection<int>>)((KeyValuePair<int, HashSet<int>> kv) => kv.Value));
				Ready = true;
			}
			try
			{
				this.Changed?.Invoke();
			}
			catch (Exception ex)
			{
				logger.Warn(ex, "MarkerPackIndexService: a Changed subscriber threw; the index is still applied.");
			}
		}

		private async Task IndexPackAsync(Func<Pack> packFactory, string displayName, PackReaderSettings settings, Dictionary<int, AchievementRoute> achievementsById, CancellationToken cancellationToken, bool quietIfEmpty = false)
		{
			Stopwatch stopwatch = Stopwatch.StartNew();
			Pack pack;
			try
			{
				pack = packFactory();
			}
			catch (Exception ex)
			{
				if (!quietIfEmpty)
				{
					logger.Warn(ex, "MarkerPackIndexService: failed to open pack '" + displayName + "'; skipping it this run.");
				}
				return;
			}
			IPackCollection collection;
			try
			{
				collection = await pack.LoadAllAsync(new MutablePackCollection(), settings);
			}
			catch (Exception ex2)
			{
				logger.Warn(ex2, "MarkerPackIndexService: failed to load pack '" + displayName + "'; skipping it this run. If it's locked by another process (Pathing mid-load), it'll be retried next start.");
				return;
			}
			finally
			{
				pack.ReleaseLocks();
			}
			HashSet<int> achievementIdsInPack = new HashSet<int>();
			int objectiveCount = 0;
			int untaggedCount = 0;
			int trailMapZeroCount = 0;
			foreach (PointOfInterest poi in collection.PointsOfInterest)
			{
				cancellationToken.ThrowIfCancellationRequested();
				AttributeCollection attributes = poi.GetAggregatedAttributes();
				if (!attributes.TryGetAttribute("achievementid", out var idAttribute) || !InvariantParseUtil.TryParseInt(idAttribute.Value, out var achievementId))
				{
					untaggedCount++;
					continue;
				}
				int bit = -1;
				if (attributes.TryGetAttribute("achievementbit", out var bitAttribute))
				{
					InvariantParseUtil.TryParseInt(bitAttribute.Value, out bit);
				}
				if (!achievementsById.TryGetValue(achievementId, out var route))
				{
					route = (achievementsById[achievementId] = new AchievementRoute());
				}
				string objectiveWaypoint = null;
				string[] waypointAttributeNames = WaypointAttributeNames;
				foreach (string waypointAttributeName in waypointAttributeNames)
				{
					if (!attributes.TryGetAttribute(waypointAttributeName, out var waypointAttribute))
					{
						continue;
					}
					foreach (Match match in WaypointCodeRegex.Matches(waypointAttribute.Value ?? string.Empty))
					{
						route.Waypoints.Add(match.Value);
						objectiveWaypoint = objectiveWaypoint ?? match.Value;
					}
				}
				route.Packs.Add(displayName);
				string categoryNamespace = poi.ParentPathingCategory?.Namespace;
				Trail trail = poi as Trail;
				int added;
				if (trail != null)
				{
					added = AddTrailObjectives(trail, route, categoryNamespace, bit, objectiveWaypoint, ref trailMapZeroCount);
				}
				else
				{
					float x = 0f;
					float y = 0f;
					float z = 0f;
					if (attributes.TryGetAttribute("xpos", out var xAttr))
					{
						InvariantParseUtil.TryParseFloat(xAttr.Value, out x);
					}
					if (attributes.TryGetAttribute("ypos", out var yAttr))
					{
						InvariantParseUtil.TryParseFloat(yAttr.Value, out y);
					}
					if (attributes.TryGetAttribute("zpos", out var zAttr))
					{
						InvariantParseUtil.TryParseFloat(zAttr.Value, out z);
					}
					route.MapIds.Add(poi.MapId);
					route.Objectives.Add(new AchievementObjective
					{
						Namespace = categoryNamespace,
						Bit = bit,
						MapId = poi.MapId,
						X = x,
						Y = y,
						Z = z,
						IsTrail = false,
						Waypoint = objectiveWaypoint
					});
					added = 1;
				}
				if (added != 0)
				{
					achievementIdsInPack.Add(achievementId);
					objectiveCount += added;
				}
			}
			if (!(objectiveCount == 0 && quietIfEmpty))
			{
				logger.Info($"Pack index: {displayName} — {achievementIdsInPack.Count} achievements, {objectiveCount} objectives, {stopwatch.ElapsedMilliseconds} ms (untagged POIs: {untaggedCount}, trail MapId=0: {trailMapZeroCount}).");
			}
		}

		private static int AddTrailObjectives(Trail trail, AchievementRoute route, string categoryNamespace, int bit, string waypoint, ref int trailMapZeroCount)
		{
			int added = 0;
			foreach (ITrailSection section in trail.TrailSections ?? Enumerable.Empty<ITrailSection>())
			{
				int mapId = ((section.MapId != 0) ? section.MapId : trail.MapId);
				if (mapId == 0)
				{
					trailMapZeroCount++;
					continue;
				}
				foreach (Vector3 point in SampleTrailPoints(section.TrailPoints))
				{
					route.MapIds.Add(mapId);
					route.Objectives.Add(new AchievementObjective
					{
						Namespace = categoryNamespace,
						Bit = bit,
						MapId = mapId,
						X = point.X,
						Y = point.Y,
						Z = point.Z,
						IsTrail = true,
						Waypoint = waypoint
					});
					added++;
				}
			}
			return added;
		}

		[IteratorStateMachine(typeof(_003CSampleTrailPoints_003Ed__26))]
		private static IEnumerable<Vector3> SampleTrailPoints(IEnumerable<Vector3> points)
		{
			return new _003CSampleTrailPoints_003Ed__26(-2)
			{
				_003C_003E3__points = points
			};
		}

		private string GetCacheFilePath()
		{
			return Path.Combine(directoriesManager.GetFullDirectoryPath("quarry"), "markerPackIndex.json");
		}

		private MarkerPackIndexCacheFile TryLoadCache(string cacheFilePath)
		{
			if (!File.Exists(cacheFilePath))
			{
				return null;
			}
			try
			{
				MarkerPackIndexCacheFile cache = JsonSerializer.Deserialize<MarkerPackIndexCacheFile>(File.ReadAllText(cacheFilePath));
				return (cache != null && cache.Schema == 5) ? cache : null;
			}
			catch (Exception ex)
			{
				logger.Warn(ex, "MarkerPackIndexService: failed to read markerPackIndex.json; rebuilding the index.");
				return null;
			}
		}

		private static bool PacksMatch(IReadOnlyList<PackCacheEntry> cached, IReadOnlyList<PackCacheEntry> current)
		{
			if (cached.Count != current.Count)
			{
				return false;
			}
			for (int i = 0; i < cached.Count; i++)
			{
				PackCacheEntry a = cached[i];
				PackCacheEntry b = current[i];
				if (!string.Equals(a.FileName, b.FileName, StringComparison.OrdinalIgnoreCase) || a.Length != b.Length || a.LastWriteTimeUtc != b.LastWriteTimeUtc)
				{
					return false;
				}
			}
			return true;
		}
	}
}
