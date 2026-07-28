using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Manlaan.CommanderMarkers.Library.Models;
using Manlaan.CommanderMarkers.Presets.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Manlaan.CommanderMarkers.Library.Services
{
	public class CommunityCatalogService
	{
		private sealed class CommunitySetsPage
		{
			[JsonProperty("total")]
			public int Total { get; set; }

			[JsonProperty("sets")]
			public List<CommunitySetSummary> Sets { get; set; } = new List<CommunitySetSummary>();

		}

		public const string IndexFileName = "community_index.json";

		private const int MaxDetailCacheEntries = 100;

		private readonly CommanderMarkersManifestService _manifestService;

		private readonly string _moduleDirectory;

		private readonly List<CommunitySetSummary> _sets = new List<CommunitySetSummary>();

		private readonly List<CommunityCategoryEntry> _categories = new List<CommunityCategoryEntry>();

		private readonly ConcurrentDictionary<string, (MarkerSet Set, long Version)> _detailCache = new ConcurrentDictionary<string, (MarkerSet, long)>();

		private readonly ConcurrentDictionary<string, Task<MarkerSet?>> _detailInflight = new ConcurrentDictionary<string, Task<MarkerSet>>();

		private readonly ConcurrentQueue<(string SetId, long Version)> _detailCacheOrder = new ConcurrentQueue<(string, long)>();

		private long _detailCacheVersion;

		private string _lastEdit = "";

		public IReadOnlyList<CommunitySetSummary> Sets => _sets;

		public IReadOnlyList<CommunityCategoryEntry> Categories => _categories;

		public event EventHandler? CatalogUpdated;

		public CommunityCatalogService(CommanderMarkersManifestService manifestService, string moduleDirectory)
		{
			_manifestService = manifestService;
			_moduleDirectory = moduleDirectory;
		}

		public void LoadCached()
		{
			string path = Path.Combine(_moduleDirectory, "community_index.json");
			if (!File.Exists(path))
			{
				return;
			}
			try
			{
				JObject i = JObject.Parse(File.ReadAllText(path));
				_lastEdit = i.Value<string>("lastEdit") ?? "";
				_sets.Clear();
				JArray setsArray = i["sets"] as JArray;
				if (setsArray == null)
				{
					return;
				}
				foreach (JToken item in setsArray)
				{
					CommunitySetSummary summary = item.ToObject<CommunitySetSummary>();
					if (summary != null)
					{
						_sets.Add(summary);
					}
				}
			}
			catch (Exception)
			{
			}
		}

		public bool SyncCatalog()
		{
			CommanderMarkersManifest manifest = _manifestService.Manifest;
			try
			{
				using WebClient client = ModuleHttp.CreateClient();
				string checkUrl = manifest.Absolute(manifest.CommunityCheckUrl);
				string remoteLastEdit = JObject.Parse(client.DownloadString(checkUrl)).Value<string>("lastEdit") ?? "";
				if (!string.IsNullOrEmpty(remoteLastEdit) && remoteLastEdit == _lastEdit && _sets.Count > 0)
				{
					return false;
				}
				string previousLastEdit = _lastEdit;
				List<CommunitySetSummary> fetched = new List<CommunitySetSummary>();
				int offset = 0;
				int total = -1;
				while (total < 0 || offset < total)
				{
					string pageUrl = $"{manifest.Absolute(manifest.SetsUrl)}?limit={200}&offset={offset}";
					CommunitySetsPage page = JsonConvert.DeserializeObject<CommunitySetsPage>(client.DownloadString(pageUrl));
					if (page == null)
					{
						break;
					}
					total = page.Total;
					fetched.AddRange(page.Sets);
					offset += 200;
					if (page.Sets.Count == 0)
					{
						break;
					}
				}
				try
				{
					List<CommunityCategoryEntry> categories = JsonConvert.DeserializeObject<List<CommunityCategoryEntry>>(client.DownloadString(manifest.Absolute(manifest.CategoriesUrl))) ?? new List<CommunityCategoryEntry>();
					_categories.Clear();
					_categories.AddRange(categories);
				}
				catch (Exception)
				{
					_categories.Clear();
				}
				_sets.Clear();
				_sets.AddRange(fetched);
				_lastEdit = remoteLastEdit;
				if (previousLastEdit != remoteLastEdit)
				{
					ClearDetailCache();
				}
				SaveIndex();
				this.CatalogUpdated?.Invoke(this, EventArgs.Empty);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public MarkerSet? FetchSetDetail(string setId)
		{
			if (string.IsNullOrWhiteSpace(setId))
			{
				return null;
			}
			if (_detailCache.TryGetValue(setId, out var cached))
			{
				TouchDetailCache(setId, cached);
				return CloneMarkerSet(cached.Item1);
			}
			Task<MarkerSet> task = _detailInflight.GetOrAdd(setId, delegate(string id)
			{
				string id2 = id;
				return Task.Run(() => DownloadSetDetail(id2));
			});
			try
			{
				MarkerSet result = task.GetAwaiter().GetResult();
				return (result == null) ? null : CloneMarkerSet(result);
			}
			finally
			{
				_detailInflight.TryRemove(setId, out var _);
			}
		}

		private MarkerSet? DownloadSetDetail(string setId)
		{
			string setId2 = setId;
			if (_detailCache.TryGetValue(setId2, out var cached))
			{
				TouchDetailCache(setId2, cached);
				return cached.Item1;
			}
			try
			{
				using WebClient client = ModuleHttp.CreateClient();
				string url = _manifestService.Manifest.Resolve(_manifestService.Manifest.SetDetailUrl, setId2);
				string value = client.DownloadString(url);
				CommunitySetSummary summary = _sets.FirstOrDefault((CommunitySetSummary s) => s.Id == setId2);
				MarkerSet markerSet = JsonConvert.DeserializeObject<MarkerSet>(value);
				if (markerSet == null)
				{
					return null;
				}
				markerSet.id = Guid.NewGuid().ToString();
				markerSet.communitySetId = setId2;
				markerSet.author = summary?.Author;
				markerSet.communityUpdatedAt = summary?.UpdatedAt;
				markerSet.source = "community";
				markerSet.syncDetached = false;
				markerSet.localModifiedAt = null;
				markerSet.syncBaselineHash = SyncBaselineHash.Compute(markerSet);
				StoreDetailCache(setId2, markerSet);
				return markerSet;
			}
			catch (Exception)
			{
				return null;
			}
		}

		private void StoreDetailCache(string setId, MarkerSet markerSet)
		{
			MarkerSet stored = CloneMarkerSet(markerSet);
			long version = Interlocked.Increment(ref _detailCacheVersion);
			_detailCache[setId] = (stored, version);
			_detailCacheOrder.Enqueue((setId, version));
			TrimDetailCache();
		}

		private void TouchDetailCache(string setId, (MarkerSet Set, long Version) current)
		{
			long version = Interlocked.Increment(ref _detailCacheVersion);
			if (_detailCache.TryUpdate(setId, (current.Set, version), current))
			{
				_detailCacheOrder.Enqueue((setId, version));
			}
		}

		private void TrimDetailCache()
		{
			(string, long) oldest;
			while (_detailCache.Count > 100 && _detailCacheOrder.TryDequeue(out oldest))
			{
				if (_detailCache.TryGetValue(oldest.Item1, out var current) && current.Item2 == oldest.Item2 && _detailCache.TryRemove(oldest.Item1, out var removed) && removed.Item2 != oldest.Item2)
				{
					_detailCache.TryAdd(oldest.Item1, removed);
				}
			}
		}

		private void ClearDetailCache()
		{
			_detailCache.Clear();
			(string, long) result;
			while (_detailCacheOrder.TryDequeue(out result))
			{
			}
		}

		private static MarkerSet CloneMarkerSet(MarkerSet source)
		{
			return JsonConvert.DeserializeObject<MarkerSet>(JsonConvert.SerializeObject(source)) ?? new MarkerSet();
		}

		private void SaveIndex()
		{
			Directory.CreateDirectory(_moduleDirectory);
			JObject payload = new JObject
			{
				["lastEdit"] = (JToken)_lastEdit,
				["fetchedAt"] = (JToken)DateTime.UtcNow.ToString("o"),
				["sets"] = JArray.FromObject(_sets)
			};
			File.WriteAllText(Path.Combine(_moduleDirectory, "community_index.json"), payload.ToString(Formatting.Indented));
		}
	}
}
