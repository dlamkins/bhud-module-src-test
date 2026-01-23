using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using NpcFinder.Models;

namespace NpcFinder.Services
{
	public class NpcMerchantResolverService
	{
		private sealed class LocationHint
		{
			public string Text;

			public int Weight;

			public string ScopeMap;

			public string Source;

			public override string ToString()
			{
				return string.Format("{0}(w={1},scope={2},src={3})", Text, Weight, ScopeMap ?? "-", Source ?? "-");
			}
		}

		private class CacheWrapper
		{
			public string Title { get; set; }

			public DateTime UtcSaved { get; set; }

			public List<NpcResolvedHit> Hits { get; set; }
		}

		private class ScoredCandidate
		{
			public string Kind;

			public string Name;

			public int X;

			public int Y;

			public int Score;

			public ScoredCandidate(string kind, string name, int x, int y, int score)
			{
				Kind = kind;
				Name = name;
				X = x;
				Y = y;
				Score = score;
			}
		}

		private static readonly bool DEBUG_LOGS = false;

		private static readonly Logger Logger = Logger.GetLogger<NpcMerchantResolverService>();

		private readonly WikiNpcService _wiki;

		private readonly Gw2MapIndexService _mapIndex;

		private readonly Gw2ApiService _gw2;

		private readonly Gw2MapDetailsService _details;

		private readonly string _cacheDir;

		private readonly Dictionary<string, int> _waypointToMapIdCache = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

		private static readonly TimeSpan CacheTtl = TimeSpan.FromDays(25.0);

		public NpcMerchantResolverService(WikiNpcService wiki, Gw2MapIndexService mapIndex, Gw2ApiService gw2, Gw2MapDetailsService details, string cacheDir)
		{
			_wiki = wiki;
			_mapIndex = mapIndex;
			_gw2 = gw2;
			_details = details;
			_cacheDir = cacheDir;
			try
			{
				if (!string.IsNullOrWhiteSpace(_cacheDir))
				{
					Directory.CreateDirectory(_cacheDir);
				}
			}
			catch
			{
			}
		}

		private static string TryGetSection(string all, string header)
		{
			if (string.IsNullOrWhiteSpace(all) || string.IsNullOrWhiteSpace(header))
			{
				return null;
			}
			Match i = new Regex("(?is)^\\s*==+\\s*" + Regex.Escape(header) + "\\s*==+\\s*(?<body>.*?)(^\\s*==+|\\z)", RegexOptions.Multiline).Match(all);
			if (!i.Success)
			{
				return null;
			}
			return i.Groups["body"].Value;
		}

		[IteratorStateMachine(typeof(_003CExtractWaypointLinksFromText_003Ed__14))]
		private IEnumerable<string> ExtractWaypointLinksFromText(string text)
		{
			return new _003CExtractWaypointLinksFromText_003Ed__14(-2)
			{
				_003C_003E3__text = text
			};
		}

		public async Task<List<NpcResolvedHit>> ResolveMerchantAsync(string npcTitle, CancellationToken ct)
		{
			if (TryLoadCachedResolvedHits(npcTitle, out var cached))
			{
				if (DEBUG_LOGS)
				{
					Logger.Debug("[MerchantResolve] CACHE HIT title='" + npcTitle + "' hits=" + cached.Count);
				}
				return cached;
			}
			WikiLookupResult wikiRes = await _wiki.ResolveByTitleAsync(npcTitle, ct).ConfigureAwait(continueOnCapturedContext: false);
			if (wikiRes == null)
			{
				return new List<NpcResolvedHit>();
			}
			string wikitext = wikiRes.Wikitext ?? "";
			List<LocationHint> hints = BuildLocationHintsWeighted(npcTitle, wikitext);
			if (DEBUG_LOGS)
			{
				Logger.Debug("[MerchantResolve] title='" + npcTitle + "' weightedHints=(" + hints.Count + ") " + string.Join(" | ", hints.Take(12)));
			}
			List<string> mapNameCandidates = BuildMapNameCandidatesFromHints(hints);
			List<int> mapIds = await ResolveAllMapIdsFromHintsAsync(mapNameCandidates, ct).ConfigureAwait(continueOnCapturedContext: false);
			if (mapIds.Count == 0)
			{
				List<string> wpCandidates = BuildWaypointCandidatesFromHints(hints);
				for (int i = 0; i < wpCandidates.Count; i++)
				{
					ct.ThrowIfCancellationRequested();
					int? wpMapId = await ResolveMapIdByWaypointNameAsync(wpCandidates[i], ct).ConfigureAwait(continueOnCapturedContext: false);
					if (wpMapId.HasValue)
					{
						mapIds.Add(wpMapId.Value);
					}
				}
				if (mapIds.Count == 0)
				{
					if (DEBUG_LOGS)
					{
						Logger.Warn("[MerchantResolve] could not resolve any mapId from hints (including waypoint fallback).");
					}
					return new List<NpcResolvedHit>();
				}
			}
			List<(string mapName, int x, int y)> coordHints = ExtractNpcCoordinates(wikitext);
			if (DEBUG_LOGS && coordHints.Count > 0)
			{
				Logger.Debug("[MerchantResolve] extracted " + coordHints.Count + " coord hint(s): " + string.Join(" | ", from c in coordHints.Take(5)
					select "[" + c.x + "," + c.y + "] map='" + c.mapName + "'"));
			}
			List<NpcResolvedHit> hits = new List<NpcResolvedHit>();
			for (int i = 0; i < mapIds.Count; i++)
			{
				int mapId = mapIds[i];
				ct.ThrowIfCancellationRequested();
				Gw2MapInfo mapInfo = await _gw2.GetMapInfoAsync(mapId, ct).ConfigureAwait(continueOnCapturedContext: false);
				if (mapInfo == null)
				{
					if (DEBUG_LOGS)
					{
						Logger.Warn("[MerchantResolve] mapInfo null for mapId=" + mapId);
					}
					continue;
				}
				if (DEBUG_LOGS)
				{
					Logger.Info("[MapInfo] mapId=" + mapId + " name='" + mapInfo.Name + "' continentId=" + mapInfo.ContinentId + " defaultFloor=" + mapInfo.DefaultFloor);
				}
				PoiWpFloorResult anchors = await _details.GetPoisAndWaypointsWithFloorFallbackAsync(mapInfo.ContinentId, mapInfo.DefaultFloor, mapInfo.Floors, mapInfo.Id, mapInfo.RegionId, ct).ConfigureAwait(continueOnCapturedContext: false);
				if (DEBUG_LOGS)
				{
					Logger.Debug("[MerchantResolve] mapId=" + mapInfo.Id + " usedFloor=" + anchors.UsedFloor + " pois=" + anchors.Pois.Count + " wps=" + anchors.Waypoints.Count);
				}
				if (anchors.Pois.Count == 0 && anchors.Waypoints.Count == 0)
				{
					hits.Add(new NpcResolvedHit
					{
						Title = npcTitle,
						MapId = mapInfo.Id,
						MapName = mapInfo.Name,
						ContinentId = mapInfo.ContinentId,
						ContinentX = 0.0,
						ContinentY = 0.0,
						Source = "MapOnly:" + mapInfo.Name,
						Debug = "NO_ANCHORS floor=" + anchors.UsedFloor + " continentId=" + mapInfo.ContinentId + " defaultFloor=" + mapInfo.DefaultFloor
					});
					continue;
				}
				(int, int)? maybeNpcPos = PickBestCoordForMap(coordHints, mapInfo.Name, mapInfo, mapIds.Count);
				ScoredCandidate best = null;
				if (maybeNpcPos.HasValue)
				{
					int npcX = maybeNpcPos.Value.Item1;
					int npcY = maybeNpcPos.Value.Item2;
					best = new ScoredCandidate("NPC", npcTitle, npcX, npcY, 10000);
					if (npcX <= 0 || npcY <= 0)
					{
						best = FindNearest("Waypoint", anchors.Waypoints, npcX, npcY) ?? FindNearest("POI", anchors.Pois, npcX, npcY);
						if (best != null)
						{
							best.Score = 9999;
						}
					}
				}
				if (best == null)
				{
					List<ScoredCandidate> candidates = new List<ScoredCandidate>();
					List<string> strongPlaceTerms = new List<string>();
					strongPlaceTerms.AddRange(from h in hints
						where h != null && !string.IsNullOrWhiteSpace(h.ScopeMap) && string.Equals(h.ScopeMap, mapInfo.Name, StringComparison.OrdinalIgnoreCase) && h.Weight >= 400
						select h.Text);
					strongPlaceTerms.AddRange(from h in hints
						where h != null && !string.IsNullOrWhiteSpace(h.ScopeMap) && string.Equals(h.ScopeMap, mapInfo.Name, StringComparison.OrdinalIgnoreCase) && h.Source != null && (h.Source.StartsWith("LocationsTree:Place", StringComparison.OrdinalIgnoreCase) || h.Source.StartsWith("LocationsTree:SynthWaypoint", StringComparison.OrdinalIgnoreCase)) && h.Weight >= 400
						select h.Text);
					if (strongPlaceTerms.Count == 0)
					{
						List<string> mapNodes = (from h in hints
							where h != null && h.Source != null && h.Source.StartsWith("LocationsTree:Map", StringComparison.OrdinalIgnoreCase) && h.Weight >= 200 && !string.IsNullOrWhiteSpace(h.Text) && !string.Equals(h.Text, mapInfo.Name, StringComparison.OrdinalIgnoreCase)
							select h.Text.Trim()).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
						List<string> evidence = (from h in hints
							where h != null && !string.IsNullOrWhiteSpace(h.Text) && (h.Source == null || !h.Source.StartsWith("LocationsTree:", StringComparison.OrdinalIgnoreCase))
							select h.Text).ToList();
						bool isMemoryMap = IsMemoryMapName(mapInfo.Name);
						HashSet<string> memoryTagged = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
						for (int k2 = 0; k2 < mapNodes.Count; k2++)
						{
							string node = mapNodes[k2];
							for (int e = 0; e < evidence.Count; e++)
							{
								string ev = evidence[e];
								if (ev.IndexOf(node, StringComparison.OrdinalIgnoreCase) >= 0 && (ev.IndexOf("Memory of", StringComparison.OrdinalIgnoreCase) >= 0 || (ev.IndexOf("(", StringComparison.OrdinalIgnoreCase) >= 0 && ev.IndexOf("Memory of", StringComparison.OrdinalIgnoreCase) >= 0)))
								{
									memoryTagged.Add(node);
									break;
								}
							}
						}
						if (isMemoryMap)
						{
							for (int n = 0; n < mapNodes.Count; n++)
							{
								string node3 = mapNodes[n];
								if (memoryTagged.Contains(node3))
								{
									strongPlaceTerms.Add(node3);
								}
							}
						}
						else if (memoryTagged.Count == 1 && mapNodes.Count >= 1)
						{
							for (int m = 0; m < mapNodes.Count; m++)
							{
								string node2 = mapNodes[m];
								if (!memoryTagged.Contains(node2))
								{
									strongPlaceTerms.Add(node2);
								}
							}
						}
					}
					strongPlaceTerms = (from s in strongPlaceTerms
						where !string.IsNullOrWhiteSpace(s)
						select s.Trim()).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
					if (DEBUG_LOGS)
					{
						Logger.Debug("[MerchantResolve] strongPlaceTerms map=" + mapInfo.Name + " => " + string.Join(", ", strongPlaceTerms));
					}
					for (int l = 0; l < anchors.Waypoints.Count; l++)
					{
						Tuple<string, int, int> w = anchors.Waypoints[l];
						int s2 = ScoreCandidateWeighted(w.Item1, hints, mapInfo.Name);
						if (strongPlaceTerms.Count > 0 && ContainsAny(w.Item1, strongPlaceTerms))
						{
							s2 += 900;
						}
						if (s2 > 0)
						{
							candidates.Add(new ScoredCandidate("Waypoint", w.Item1, w.Item2, w.Item3, s2));
						}
					}
					for (int k = 0; k < anchors.Pois.Count; k++)
					{
						Tuple<string, int, int> p = anchors.Pois[k];
						int s3 = ScoreCandidateWeighted(p.Item1, hints, mapInfo.Name);
						if (!IsMemoryMapName(mapInfo.Name) && IsOldPrefixed(p.Item1))
						{
							s3 = (int)((double)s3 * 0.05);
						}
						else if (strongPlaceTerms.Count > 0 && IsOldPrefixed(p.Item1))
						{
							s3 = (int)((double)s3 * 0.1);
						}
						if (strongPlaceTerms.Count > 0 && ContainsAny(p.Item1, strongPlaceTerms))
						{
							s3 += 80;
						}
						if (s3 > 0)
						{
							candidates.Add(new ScoredCandidate("POI", p.Item1, p.Item2, p.Item3, s3));
						}
					}
					if (DEBUG_LOGS)
					{
						List<ScoredCandidate> top = candidates.OrderByDescending((ScoredCandidate x) => x.Score).Take(5).ToList();
						Logger.Warn("[MerchantResolve] TOP candidates map=" + mapInfo.Name + " => " + string.Join(" | ", top.Select((ScoredCandidate x) => x.Kind + ":" + x.Name + "(s=" + x.Score + ")")));
					}
					if (candidates.Count == 0)
					{
						hits.Add(new NpcResolvedHit
						{
							Title = npcTitle,
							MapId = mapInfo.Id,
							MapName = mapInfo.Name,
							ContinentId = mapInfo.ContinentId,
							ContinentX = 0.0,
							ContinentY = 0.0,
							Source = "MapOnly:" + mapInfo.Name,
							Debug = "NO_SCORED_CANDIDATES floor=" + anchors.UsedFloor
						});
						continue;
					}
					best = (from c in candidates
						orderby c.Score descending, KindRank(c.Kind)
						select c).First();
				}
				if (best == null)
				{
					hits.Add(new NpcResolvedHit
					{
						Title = npcTitle,
						MapId = mapInfo.Id,
						MapName = mapInfo.Name,
						ContinentId = mapInfo.ContinentId,
						ContinentX = 0.0,
						ContinentY = 0.0,
						Source = "MapOnly:" + mapInfo.Name,
						Debug = "BEST_NULL floor=" + anchors.UsedFloor
					});
					continue;
				}
				double cx = best.X;
				double cy = best.Y;
				if (DEBUG_LOGS)
				{
					Logger.Warn("[MerchantResolve] HIT map=" + mapInfo.Name + " cont=" + mapInfo.ContinentId + " best=" + best.Kind + ":" + best.Name + " @" + best.X + "," + best.Y + " -> continent=(" + cx + "," + cy + ")");
				}
				hits.Add(new NpcResolvedHit
				{
					Title = npcTitle,
					MapId = mapInfo.Id,
					MapName = mapInfo.Name,
					ContinentId = mapInfo.ContinentId,
					ContinentX = cx,
					ContinentY = cy,
					Source = best.Kind + ":" + best.Name,
					Debug = "floor=" + anchors.UsedFloor + ", coord=" + (maybeNpcPos.HasValue ? (maybeNpcPos.Value.Item1 + "," + maybeNpcPos.Value.Item2) : "none")
				});
			}
			hits = hits.OrderBy((NpcResolvedHit h) => IsMemoryMapName(h.MapName) ? 1 : 0).ThenBy((NpcResolvedHit h) => h.MapName, StringComparer.OrdinalIgnoreCase).ThenBy((NpcResolvedHit h) => (!h.Source.StartsWith("Waypoint:", StringComparison.OrdinalIgnoreCase)) ? 1 : 0)
				.ToList();
			if (hits.Count == 0 && DEBUG_LOGS)
			{
				Logger.Warn("[MerchantResolve] No hits produced.");
			}
			SaveCachedResolvedHits(npcTitle, hits);
			return hits;
		}

		private static int KindRank(string kind)
		{
			if (kind.Equals("Waypoint", StringComparison.OrdinalIgnoreCase))
			{
				return 0;
			}
			if (kind.Equals("POI", StringComparison.OrdinalIgnoreCase))
			{
				return 1;
			}
			return 2;
		}

		private void EnsureCacheDir()
		{
			try
			{
				if (!string.IsNullOrWhiteSpace(_cacheDir))
				{
					Directory.CreateDirectory(_cacheDir);
				}
			}
			catch
			{
			}
		}

		private bool TryLoadCachedResolvedHits(string title, out List<NpcResolvedHit> hits)
		{
			hits = null;
			try
			{
				if (string.IsNullOrWhiteSpace(_cacheDir))
				{
					return false;
				}
				EnsureCacheDir();
				string path = GetCacheFilePath(title);
				if (!File.Exists(path))
				{
					return false;
				}
				FileInfo fi = new FileInfo(path);
				if (fi.Length <= 2)
				{
					return false;
				}
				if (DateTime.UtcNow - fi.LastWriteTimeUtc > CacheTtl)
				{
					return false;
				}
				CacheWrapper wrapper = JsonSerializer.Deserialize<CacheWrapper>(File.ReadAllText(path, Encoding.UTF8), (JsonSerializerOptions)null);
				if (wrapper == null || wrapper.Hits == null)
				{
					return false;
				}
				hits = wrapper.Hits;
				return true;
			}
			catch (Exception ex)
			{
				Logger.Warn("Exception [MerchantResolve] cache read failed: " + ex.Message);
				return false;
			}
		}

		private void SaveCachedResolvedHits(string title, List<NpcResolvedHit> hits)
		{
			try
			{
				if (!string.IsNullOrWhiteSpace(_cacheDir))
				{
					EnsureCacheDir();
					string path = GetCacheFilePath(title);
					string json = JsonSerializer.Serialize<CacheWrapper>(new CacheWrapper
					{
						Title = title,
						UtcSaved = DateTime.UtcNow,
						Hits = (hits ?? new List<NpcResolvedHit>())
					}, (JsonSerializerOptions)null);
					File.WriteAllText(path, json, Encoding.UTF8);
					if (DEBUG_LOGS)
					{
						Logger.Debug("[MerchantResolve] cache write OK path=" + path);
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Warn("Exception [MerchantResolve] cache write failed: " + ex.Message);
			}
		}

		private string GetCacheFilePath(string title)
		{
			string safe = Sha1Hex(title ?? "");
			return Path.Combine(_cacheDir, "merchant_" + safe + ".json");
		}

		private static string Sha1Hex(string s)
		{
			try
			{
				using SHA1 sha1 = SHA1.Create();
				byte[] bytes = Encoding.UTF8.GetBytes(s);
				byte[] hash = sha1.ComputeHash(bytes);
				StringBuilder sb = new StringBuilder(hash.Length * 2);
				for (int i = 0; i < hash.Length; i++)
				{
					sb.Append(hash[i].ToString("x2"));
				}
				return sb.ToString();
			}
			catch
			{
				return (s ?? "").GetHashCode().ToString("x8");
			}
		}

		private async Task<List<int>> ResolveAllMapIdsFromHintsAsync(List<string> hints, CancellationToken ct)
		{
			HashSet<int> ids = new HashSet<int>();
			if (hints == null || hints.Count == 0)
			{
				return ids.ToList();
			}
			List<string> expanded = new List<string>();
			for (int i = 0; i < hints.Count; i++)
			{
				string c = CleanHint(hints[i]);
				if (string.IsNullOrWhiteSpace(c))
				{
					continue;
				}
				string[] pipeParts = c.Split(new char[1] { '|' }, StringSplitOptions.RemoveEmptyEntries);
				for (int p = 0; p < pipeParts.Length; p++)
				{
					string s2 = pipeParts[p].Trim();
					if (s2.Length >= 3)
					{
						expanded.Add(s2);
					}
					List<string> more = SplitMapLikeString(s2);
					for (int k = 0; k < more.Count; k++)
					{
						expanded.Add(more[k]);
					}
				}
			}
			expanded = (from s in expanded
				select s.Trim() into s
				where s.Length >= 3 && s.Length <= 60
				where !s.Contains(":")
				select s).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
			for (int j = 0; j < expanded.Count; j++)
			{
				ct.ThrowIfCancellationRequested();
				string name = expanded[j];
				string nTrim = (name ?? "").Trim();
				if (!string.Equals(nTrim, "the", StringComparison.OrdinalIgnoreCase) && !string.Equals(nTrim, "a", StringComparison.OrdinalIgnoreCase) && !string.Equals(nTrim, "an", StringComparison.OrdinalIgnoreCase) && !string.Equals(nTrim, "merchant", StringComparison.OrdinalIgnoreCase) && !string.Equals(nTrim, "npc", StringComparison.OrdinalIgnoreCase) && !string.Equals(nTrim, "bandit", StringComparison.OrdinalIgnoreCase) && !string.Equals(nTrim, "scout", StringComparison.OrdinalIgnoreCase) && !string.Equals(nTrim, "animal", StringComparison.OrdinalIgnoreCase) && !string.Equals(nTrim, "farmer", StringComparison.OrdinalIgnoreCase) && !string.Equals(nTrim, "watchman", StringComparison.OrdinalIgnoreCase))
				{
					int? id = await _mapIndex.ResolveMapIdByNameAsync(name, ct).ConfigureAwait(continueOnCapturedContext: false);
					if (!id.HasValue && (name.EndsWith(" Waypoint", StringComparison.OrdinalIgnoreCase) || name.EndsWith(" WP", StringComparison.OrdinalIgnoreCase)))
					{
						id = await ResolveMapIdByWaypointNameAsync(name, ct).ConfigureAwait(continueOnCapturedContext: false);
					}
					if (DEBUG_LOGS)
					{
						Logger.Debug("[MerchantResolve] try mapName='" + name + "' => mapId=" + (id.HasValue ? id.Value.ToString() : "null"));
					}
					if (id.HasValue)
					{
						ids.Add(id.Value);
					}
					if (ids.Count >= 15)
					{
						break;
					}
				}
			}
			return ids.ToList();
		}

		private List<string> BuildMapNameCandidatesFromHints(List<LocationHint> hints)
		{
			if (hints == null || hints.Count == 0)
			{
				return new List<string>();
			}
			List<(string, int)> items = new List<(string, int)>();
			for (int j = 0; j < hints.Count; j++)
			{
				LocationHint h = hints[j];
				if (h != null)
				{
					if (!string.IsNullOrWhiteSpace(h.ScopeMap))
					{
						items.Add((h.ScopeMap, h.Weight + 80));
					}
					if (!string.IsNullOrWhiteSpace(h.Text))
					{
						items.Add((h.Text, h.Weight));
					}
				}
			}
			items = items.OrderByDescending<(string, int), int>(((string text, int weight) x) => x.weight).ToList();
			HashSet<string> seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			List<string> result = new List<string>();
			for (int i = 0; i < items.Count; i++)
			{
				string s = CleanHint(items[i].Item1);
				if (!string.IsNullOrWhiteSpace(s) && s.Length >= 3 && s.Length <= 60 && s.IndexOf("Waypoint", StringComparison.OrdinalIgnoreCase) < 0 && !s.EndsWith(" WP", StringComparison.OrdinalIgnoreCase) && s.IndexOf("POI", StringComparison.OrdinalIgnoreCase) < 0)
				{
					if (seen.Add(s))
					{
						result.Add(s);
					}
					if (result.Count >= 80)
					{
						break;
					}
				}
			}
			return result;
		}

		private async Task<int?> ResolveMapIdByWaypointNameAsync(string waypointName, CancellationToken ct)
		{
			waypointName = CleanHint(waypointName);
			if (string.IsNullOrWhiteSpace(waypointName))
			{
				return null;
			}
			if (_waypointToMapIdCache.TryGetValue(waypointName, out var cached) && cached > 0)
			{
				return cached;
			}
			List<int> allMapIds = await _mapIndex.GetAllKnownMapIdsAsync(ct).ConfigureAwait(continueOnCapturedContext: false);
			if (allMapIds == null || allMapIds.Count == 0)
			{
				return null;
			}
			int scanCap = Math.Min(allMapIds.Count, 120);
			for (int i = 0; i < scanCap; i++)
			{
				ct.ThrowIfCancellationRequested();
				int mapId = allMapIds[i];
				Gw2MapInfo mapInfo = await _gw2.GetMapInfoAsync(mapId, ct).ConfigureAwait(continueOnCapturedContext: false);
				if (mapInfo == null)
				{
					continue;
				}
				PoiWpFloorResult anchors = await _details.GetPoisAndWaypointsWithFloorFallbackAsync(mapInfo.ContinentId, mapInfo.DefaultFloor, mapInfo.Floors, mapInfo.Id, mapInfo.RegionId, ct).ConfigureAwait(continueOnCapturedContext: false);
				for (int w = 0; w < anchors.Waypoints.Count; w++)
				{
					if (string.Equals(anchors.Waypoints[w].Item1, waypointName, StringComparison.OrdinalIgnoreCase))
					{
						_waypointToMapIdCache[waypointName] = mapId;
						if (DEBUG_LOGS)
						{
							Logger.Debug("[MerchantResolve] waypoint->mapId '" + waypointName + "' => " + mapId);
						}
						return mapId;
					}
				}
			}
			if (DEBUG_LOGS)
			{
				Logger.Debug("[MerchantResolve] waypoint->mapId NOT FOUND '" + waypointName + "' (scanned " + scanCap + " maps)");
			}
			return null;
		}

		private List<string> BuildWaypointCandidatesFromHints(List<LocationHint> hints)
		{
			if (hints == null || hints.Count == 0)
			{
				return new List<string>();
			}
			List<(string, int)> items = new List<(string, int)>();
			for (int i = 0; i < hints.Count; i++)
			{
				LocationHint h = hints[i];
				if (h != null && !string.IsNullOrWhiteSpace(h.Text))
				{
					bool num = h.Source != null && h.Source.StartsWith("LocationsTree:SynthWaypoint", StringComparison.OrdinalIgnoreCase);
					bool looksLikeWp = h.Text.EndsWith(" Waypoint", StringComparison.OrdinalIgnoreCase) || h.Text.EndsWith(" WP", StringComparison.OrdinalIgnoreCase);
					if (num || looksLikeWp)
					{
						items.Add((h.Text, h.Weight));
					}
				}
			}
			return (from x in items
				orderby x.weight descending
				select CleanHint(x.text) into s
				where !string.IsNullOrWhiteSpace(s)
				select s).Distinct(StringComparer.OrdinalIgnoreCase).Take(3).ToList();
		}

		private List<(string mapName, int x, int y)> ExtractNpcCoordinates(string wikitext)
		{
			List<(string, int, int)> list = new List<(string, int, int)>();
			if (string.IsNullOrWhiteSpace(wikitext))
			{
				return list;
			}
			foreach (Match i in Regex.Matches(wikitext, "\\bcoordinates\\s*=\\s*\\[\\s*(?<x>-?\\d+)\\s*,\\s*(?<y>-?\\d+)\\s*\\]", RegexOptions.IgnoreCase))
			{
				if (int.TryParse(SafeDigitsSigned(i.Groups["x"].Value), out var x3) && int.TryParse(SafeDigitsSigned(i.Groups["y"].Value), out var y3))
				{
					list.Add((null, x3, y3));
				}
			}
			foreach (Match item in Regex.Matches(wikitext, "\\{\\{\\s*Interactive map\\b.*?\\}\\}", RegexOptions.IgnoreCase | RegexOptions.Singleline))
			{
				string block = item.Value;
				string mapName = TryGetTemplateParam(block, "map") ?? TryGetTemplateParam(block, "location") ?? TryGetTemplateParam(block, "region");
				string s2 = TryGetTemplateParam(block, "x");
				string yStr = TryGetTemplateParam(block, "y");
				if (int.TryParse(SafeDigitsSigned(s2), out var x2) && int.TryParse(SafeDigitsSigned(yStr), out var y2))
				{
					list.Add((mapName, x2, y2));
				}
			}
			foreach (Match item2 in Regex.Matches(wikitext, "\\{\\{\\s*coord\\s*\\|\\s*(?<x>\\d+)\\s*\\|\\s*(?<y>\\d+)[^}]*\\}\\}", RegexOptions.IgnoreCase))
			{
				int x = int.Parse(item2.Groups["x"].Value);
				int y = int.Parse(item2.Groups["y"].Value);
				list.Add((null, x, y));
			}
			return list;
			static string SafeDigitsSigned(string s)
			{
				if (string.IsNullOrWhiteSpace(s))
				{
					return "";
				}
				s = s.Trim();
				Match j = Regex.Match(s, "-?\\d+");
				if (!j.Success)
				{
					return "";
				}
				return j.Value;
			}
			static string TryGetTemplateParam(string tpl, string key)
			{
				Match mm = new Regex("\\|\\s*" + Regex.Escape(key) + "\\s*=\\s*(?<v>[^|\\}]+)", RegexOptions.IgnoreCase).Match(tpl);
				if (!mm.Success)
				{
					return null;
				}
				return mm.Groups["v"].Value.Trim();
			}
		}

		private (int x, int y)? PickBestCoordForMap(List<(string mapName, int x, int y)> coords, string mapName, Gw2MapInfo mapInfo, int totalResolvedMapCount)
		{
			if (coords == null || coords.Count == 0)
			{
				return null;
			}
			for (int j = 0; j < coords.Count; j++)
			{
				if (!string.IsNullOrWhiteSpace(coords[j].mapName) && string.Equals(coords[j].mapName.Trim(), mapName, StringComparison.OrdinalIgnoreCase))
				{
					return new(int, int)?((coords[j].x, coords[j].y));
				}
			}
			for (int i = 0; i < coords.Count; i++)
			{
				if (string.IsNullOrWhiteSpace(coords[i].mapName))
				{
					int x = coords[i].x;
					int y = coords[i].y;
					double minX = Math.Min(mapInfo.ContinentRect.X1, mapInfo.ContinentRect.X2);
					double maxX = Math.Max(mapInfo.ContinentRect.X1, mapInfo.ContinentRect.X2);
					double minY = Math.Min(mapInfo.ContinentRect.Y1, mapInfo.ContinentRect.Y2);
					double maxY = Math.Max(mapInfo.ContinentRect.Y1, mapInfo.ContinentRect.Y2);
					if ((double)x >= minX && (double)x <= maxX && (double)y >= minY && (double)y <= maxY)
					{
						return new(int, int)?((x, y));
					}
				}
			}
			if (coords.Count == 1 && totalResolvedMapCount == 1)
			{
				return new(int, int)?((coords[0].x, coords[0].y));
			}
			return null;
		}

		private ScoredCandidate FindNearest(string kind, List<Tuple<string, int, int>> pts, int x, int y)
		{
			if (pts == null || pts.Count == 0)
			{
				return null;
			}
			long bestD = long.MaxValue;
			Tuple<string, int, int> best = null;
			for (int i = 0; i < pts.Count; i++)
			{
				Tuple<string, int, int> p = pts[i];
				long dx = (long)p.Item2 - (long)x;
				long dy = (long)p.Item3 - (long)y;
				long d2 = dx * dx + dy * dy;
				if (d2 < bestD)
				{
					bestD = d2;
					best = p;
				}
			}
			if (best == null)
			{
				return null;
			}
			return new ScoredCandidate(kind, best.Item1, best.Item2, best.Item3, 0);
		}

		private void AddInfoboxSplitMemoryHints(List<LocationHint> hints, string infoboxLocationRaw)
		{
			if (string.IsNullOrWhiteSpace(infoboxLocationRaw))
			{
				return;
			}
			string raw = infoboxLocationRaw.Trim();
			int pipe = raw.IndexOf('|');
			int memIdx = raw.IndexOf("(Memory of", StringComparison.OrdinalIgnoreCase);
			if (pipe < 0 || memIdx < 0)
			{
				return;
			}
			string left = raw.Substring(0, pipe).Trim();
			string rightPlus = raw.Substring(pipe + 1).Trim();
			int open = rightPlus.IndexOf('(');
			int close = rightPlus.LastIndexOf(')');
			if (open < 0 || close <= open)
			{
				return;
			}
			string right = rightPlus.Substring(0, open).Trim();
			string memoryMap = rightPlus.Substring(open + 1, close - open - 1).Trim();
			string mainMap = null;
			for (int i = 0; i < hints.Count; i++)
			{
				LocationHint h = hints[i];
				if (h != null && h.Source != null && h.Source.StartsWith("LocationsTree:Map", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(h.Text) && !IsMemoryMapName(h.Text))
				{
					mainMap = h.Text.Trim();
					break;
				}
			}
			if (!string.IsNullOrWhiteSpace(left))
			{
				hints.Add(new LocationHint
				{
					Text = left,
					Weight = 500,
					ScopeMap = mainMap,
					Source = "Infobox:Scoped"
				});
			}
			if (!string.IsNullOrWhiteSpace(right))
			{
				hints.Add(new LocationHint
				{
					Text = right,
					Weight = 500,
					ScopeMap = memoryMap,
					Source = "Infobox:Scoped"
				});
			}
		}

		private List<LocationHint> BuildLocationHintsWeighted(string title, string wikitext)
		{
			List<LocationHint> hints = new List<LocationHint>();
			if (string.IsNullOrWhiteSpace(wikitext))
			{
				return hints;
			}
			string pruned = StripSection(wikitext, "Historical locations");
			pruned = StripSection(pruned, "Historic locations");
			List<(string, string)> tree = ExtractLocationsTree(pruned);
			for (int n = 0; n < tree.Count; n++)
			{
				string map = tree[n].Item1;
				string place = tree[n].Item2;
				if (!string.IsNullOrWhiteSpace(map))
				{
					hints.Add(new LocationHint
					{
						Text = map,
						Weight = 240,
						ScopeMap = null,
						Source = "LocationsTree:Map"
					});
				}
				if (!string.IsNullOrWhiteSpace(place))
				{
					hints.Add(new LocationHint
					{
						Text = place,
						Weight = 520,
						ScopeMap = map,
						Source = "LocationsTree:Place"
					});
					hints.Add(new LocationHint
					{
						Text = place + " Waypoint",
						Weight = 430,
						ScopeMap = map,
						Source = "LocationsTree:SynthWaypoint"
					});
					hints.Add(new LocationHint
					{
						Text = place + " WP",
						Weight = 320,
						ScopeMap = map,
						Source = "LocationsTree:SynthWaypoint"
					});
				}
			}
			string infobox = ExtractInfoboxMap(pruned);
			if (!string.IsNullOrWhiteSpace(infobox))
			{
				AddInfoboxSplitMemoryHints(hints, infobox);
				hints.Add(new LocationHint
				{
					Text = infobox,
					Weight = 120,
					ScopeMap = null,
					Source = "Infobox"
				});
			}
			List<string> locLinks = ExtractLinksFromSection(pruned, "Location");
			for (int m = 0; m < locLinks.Count; m++)
			{
				hints.Add(new LocationHint
				{
					Text = locLinks[m],
					Weight = 170,
					ScopeMap = null,
					Source = "Section:Location"
				});
			}
			string locSection = TryGetSection(pruned, "Location");
			foreach (string wp in ExtractWaypointLinksFromText(locSection))
			{
				hints.Add(new LocationHint
				{
					Text = wp,
					Weight = 420,
					ScopeMap = null,
					Source = "Section:Location:WaypointLink"
				});
			}
			List<string> lead = ExtractLeadSentenceLocation(pruned);
			for (int l = 0; l < lead.Count; l++)
			{
				hints.Add(new LocationHint
				{
					Text = lead[l],
					Weight = 150,
					ScopeMap = null,
					Source = "Lead"
				});
			}
			List<string> phrases = ExtractPlainLocationPhrases(pruned);
			for (int k = 0; k < phrases.Count; k++)
			{
				hints.Add(new LocationHint
				{
					Text = phrases[k],
					Weight = 120,
					ScopeMap = null,
					Source = "Phrases"
				});
			}
			List<string> allLinks = ExtractAllLinks(pruned);
			for (int j = 0; j < allLinks.Count; j++)
			{
				hints.Add(new LocationHint
				{
					Text = allLinks[j],
					Weight = 40,
					ScopeMap = null,
					Source = "AllLinks"
				});
			}
			Dictionary<string, LocationHint> dict = new Dictionary<string, LocationHint>(StringComparer.OrdinalIgnoreCase);
			for (int i = 0; i < hints.Count; i++)
			{
				LocationHint h = hints[i];
				if (h == null)
				{
					continue;
				}
				h.Text = CleanHint(h.Text);
				h.ScopeMap = CleanHint(h.ScopeMap);
				if (!string.IsNullOrWhiteSpace(h.Text) && h.Text.Length >= 3 && h.Text.Length <= 80 && !Regex.IsMatch(h.Text, "^[a-z]{2}:", RegexOptions.IgnoreCase))
				{
					string k2 = h.ScopeMap + "||" + h.Text;
					if (!dict.TryGetValue(k2, out var existing))
					{
						dict[k2] = h;
					}
					else if (h.Weight > existing.Weight)
					{
						dict[k2] = h;
					}
				}
			}
			return dict.Values.OrderByDescending((LocationHint x) => x.Weight).ThenBy((LocationHint x) => x.Text, StringComparer.OrdinalIgnoreCase).Take(220)
				.ToList();
		}

		private static string StripSection(string wikitext, string sectionTitle)
		{
			if (string.IsNullOrWhiteSpace(wikitext) || string.IsNullOrWhiteSpace(sectionTitle))
			{
				return wikitext;
			}
			return new Regex("==\\s*" + Regex.Escape(sectionTitle) + "\\s*==(.+?)(?:(\\r?\\n)==|$)", RegexOptions.IgnoreCase | RegexOptions.Singleline).Replace(wikitext, "\n");
		}

		private List<(string map, string place)> ExtractLocationsTree(string wikitext)
		{
			List<(string, string)> res = new List<(string, string)>();
			if (string.IsNullOrWhiteSpace(wikitext))
			{
				return res;
			}
			string[] lines = (TryGetSection(wikitext, "Locations") ?? wikitext).Split(new string[2] { "\r\n", "\n" }, StringSplitOptions.None);
			int maxDepth = 0;
			for (int j = 0; j < lines.Length; j++)
			{
				Match mm = Regex.Match(lines[j] ?? "", "^\\s*(?<bul>[\\*\\#]+)\\s*(?<rest>.+)$");
				if (mm.Success)
				{
					maxDepth = Math.Max(maxDepth, mm.Groups["bul"].Value.Length);
				}
			}
			bool regionMode = maxDepth >= 3;
			string region = null;
			string map = null;
			foreach (string line in lines)
			{
				if (string.IsNullOrWhiteSpace(line))
				{
					continue;
				}
				Match mm2 = Regex.Match(line, "^\\s*(?<bul>[\\*\\#]+)\\s*(?<rest>.+)$");
				if (!mm2.Success)
				{
					continue;
				}
				int depth = mm2.Groups["bul"].Value.Length;
				string content = mm2.Groups["rest"].Value;
				string text = ExtractFirstLinkText(content);
				if (string.IsNullOrWhiteSpace(text))
				{
					text = StripWikiMarkup(content);
				}
				text = CleanHint(text);
				if (string.IsNullOrWhiteSpace(text))
				{
					continue;
				}
				if (!regionMode)
				{
					if (depth == 1)
					{
						map = text;
						res.Add((map, null));
					}
					else if (depth >= 2 && !string.IsNullOrWhiteSpace(map))
					{
						res.Add((map, text));
					}
				}
				else if (depth == 1)
				{
					region = text;
					map = null;
				}
				else if (depth == 2)
				{
					map = text;
					res.Add((map, null));
				}
				else if (depth >= 3)
				{
					if (!string.IsNullOrWhiteSpace(map))
					{
						res.Add((map, text));
					}
					else if (!string.IsNullOrWhiteSpace(region))
					{
						res.Add((region, text));
					}
				}
			}
			return Dedup(res);
			List<(string map, string place)> Dedup(List<(string map, string place)> list)
			{
				HashSet<string> seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
				List<(string, string)> outList = new List<(string, string)>();
				for (int k = 0; k < list.Count; k++)
				{
					string l = CleanHint(list[k].map);
					string p = CleanHint(list[k].place);
					string key = l + "||" + p;
					if (seen.Add(key))
					{
						outList.Add((l, p));
					}
				}
				return outList;
			}
			static string ExtractFirstLinkText(string s)
			{
				Match m = Regex.Match(s, "\\[\\[(?<t>[^\\]|#]+)(?:#[^\\]|]+)?(?:\\|(?<d>[^\\]]+))?\\]\\]");
				if (!m.Success)
				{
					return null;
				}
				object obj = (m.Groups["d"].Success ? m.Groups["d"].Value : null);
				string t = (m.Groups["t"].Success ? m.Groups["t"].Value : null);
				if (obj == null)
				{
					obj = t ?? "";
				}
				return ((string)obj).Trim();
			}
			static string StripWikiMarkup(string s)
			{
				s = Regex.Replace(s, "\\{\\{.*?\\}\\}", "", RegexOptions.Singleline);
				s = Regex.Replace(s, "\\[\\[|\\]\\]", "");
				return s.Trim();
			}
			static string TryGetSection(string all, string header)
			{
				Match n = new Regex("(?is)^\\s*==+\\s*" + Regex.Escape(header) + "\\s*==+\\s*(?<body>.*?)(^\\s*==+|\\z)", RegexOptions.Multiline).Match(all);
				if (!n.Success)
				{
					return null;
				}
				return n.Groups["body"].Value;
			}
		}

		private List<string> ExtractAllLinks(string wikitext)
		{
			List<string> list = new List<string>();
			if (string.IsNullOrWhiteSpace(wikitext))
			{
				return list;
			}
			foreach (Match item in Regex.Matches(wikitext, "\\[\\[([^\\]\\|]+)(\\|[^\\]]+)?\\]\\]"))
			{
				string s = item.Groups[1].Value.Trim();
				if (s.Length >= 3 && s.Length <= 60)
				{
					list.Add(s);
				}
			}
			return list;
		}

		private List<string> ExtractLeadSentenceLocation(string wikitext)
		{
			List<string> list = new List<string>();
			if (string.IsNullOrWhiteSpace(wikitext))
			{
				return list;
			}
			string lead = wikitext;
			if (lead.Length > 600)
			{
				lead = lead.Substring(0, 600);
			}
			lead = Regex.Replace(lead, "<!--.*?-->", "", RegexOptions.Singleline);
			lead = Regex.Replace(lead, "<ref.*?>.*?</ref>", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
			lead = Regex.Replace(lead, "<.*?>", "", RegexOptions.Singleline);
			lead = Regex.Replace(lead, "\\{\\{.*?\\}\\}", "", RegexOptions.Singleline);
			lead = Regex.Replace(lead, "\\[\\[([^\\]\\|]+)(\\|[^\\]]+)?\\]\\]", "$1");
			foreach (Match item in Regex.Matches(lead, "\\bfound in\\b\\s*(the\\s+)?(?<loc>[A-Z][A-Za-z'\\- ]{3,60})", RegexOptions.IgnoreCase))
			{
				string s = item.Groups["loc"].Value.Trim().TrimEnd('.', ',', ';');
				if (s.Length >= 3 && s.Length <= 60)
				{
					list.Add(s);
				}
			}
			return list;
		}

		private List<string> ExtractLinksFromSection(string wikitext, string sectionName)
		{
			List<string> list = new List<string>();
			if (string.IsNullOrWhiteSpace(wikitext))
			{
				return list;
			}
			Match i = new Regex("==\\s*" + Regex.Escape(sectionName) + "\\s*==(.+?)(==|$)", RegexOptions.IgnoreCase | RegexOptions.Singleline).Match(wikitext);
			if (!i.Success)
			{
				return list;
			}
			foreach (Match item in Regex.Matches(i.Groups[1].Value, "\\[\\[([^\\]\\|]+)(\\|[^\\]]+)?\\]\\]"))
			{
				string s = item.Groups[1].Value.Trim();
				if (s.Length >= 3 && s.Length <= 60)
				{
					list.Add(s);
				}
			}
			return list;
		}

		private List<string> ExtractPlainLocationPhrases(string wikitext)
		{
			List<string> list = new List<string>();
			if (string.IsNullOrWhiteSpace(wikitext))
			{
				return list;
			}
			foreach (Match item in Regex.Matches(wikitext, "found in (the )?([A-Z][A-Za-z' \\-]{3,60})", RegexOptions.IgnoreCase))
			{
				string s = item.Groups[2].Value.Trim();
				if (s.Length >= 3 && s.Length <= 60)
				{
					list.Add(s);
				}
			}
			return list;
		}

		private string ExtractInfoboxMap(string wikitext)
		{
			if (string.IsNullOrWhiteSpace(wikitext))
			{
				return null;
			}
			return TryField(new string[5] { "map", "location", "zone", "region", "area" });
			string TryField(params string[] keys)
			{
				foreach (string i in keys)
				{
					Match j = new Regex("\\|\\s*" + Regex.Escape(i) + "\\s*=\\s*(?<v>[^\\r\\n]+)", RegexOptions.IgnoreCase).Match(wikitext);
					if (j.Success)
					{
						string raw = j.Groups["v"].Value.Trim();
						raw = Regex.Replace(raw, "\\[\\[([^\\]\\|]+)(\\|[^\\]]+)?\\]\\]", "$1");
						raw = Regex.Replace(raw, "<.*?>", "");
						raw = Regex.Replace(raw, "\\{\\{.*?\\}\\}", "", RegexOptions.Singleline);
						if (!string.IsNullOrWhiteSpace(raw))
						{
							return raw.Trim();
						}
					}
				}
				return null;
			}
		}

		private List<string> SplitMapLikeString(string s)
		{
			List<string> list = new List<string>();
			if (string.IsNullOrWhiteSpace(s))
			{
				return list;
			}
			char[] seps = new char[8] { '(', ')', ',', ';', '/', '\\', '-', '|' };
			string[] parts = s.Split(seps, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < parts.Length; i++)
			{
				string p = parts[i].Trim();
				if (p.Length >= 3 && p.Length <= 60)
				{
					list.Add(p);
				}
			}
			return list;
		}

		private string CleanHint(string s)
		{
			if (string.IsNullOrWhiteSpace(s))
			{
				return null;
			}
			if (Regex.IsMatch(s, "^[a-z]{2}:", RegexOptions.IgnoreCase))
			{
				return null;
			}
			s = s.Trim();
			s = Regex.Replace(s, "\\[\\[([^\\]\\|]+)(\\|[^\\]]+)?\\]\\]", "$1").Trim();
			s = Regex.Replace(s, "\\{\\{[^}]+\\}\\}", "").Trim();
			s = Regex.Replace(s, "<.*?>", "").Trim();
			s = s.Replace(";", "|");
			return s.Trim();
		}

		private int ScoreCandidateWeighted(string candidateName, List<LocationHint> hints, string currentMapName)
		{
			if (string.IsNullOrWhiteSpace(candidateName) || hints == null || hints.Count == 0)
			{
				return 0;
			}
			string candNorm = Normalize(candidateName.Trim());
			int score = 0;
			if (candNorm.Contains("waypoint"))
			{
				score += 80;
			}
			for (int i = 0; i < hints.Count; i++)
			{
				LocationHint h = hints[i];
				if (h == null || string.IsNullOrWhiteSpace(h.Text))
				{
					continue;
				}
				bool num = !string.IsNullOrWhiteSpace(h.ScopeMap) && !string.Equals(h.ScopeMap, currentMapName, StringComparison.OrdinalIgnoreCase);
				int w = h.Weight;
				if (num)
				{
					w = (int)((double)w * 0.15);
				}
				string hNorm = Normalize(h.Text);
				if (hNorm.Length < 3)
				{
					continue;
				}
				switch (hNorm)
				{
				case "the":
				case "merchant":
				case "npc":
					continue;
				}
				if (string.Equals(candNorm, hNorm, StringComparison.OrdinalIgnoreCase))
				{
					score += w;
					continue;
				}
				if (candNorm.Contains(hNorm) || hNorm.Contains(candNorm))
				{
					score += (int)((double)w * 0.55);
				}
				HashSet<string> cTokens = Tokenize(candNorm);
				HashSet<string> hashSet = Tokenize(hNorm);
				int overlap = 0;
				foreach (string token in hashSet)
				{
					if (cTokens.Contains(token))
					{
						overlap++;
					}
				}
				if (overlap > 0)
				{
					score += overlap * Math.Max(12, (int)((double)w * 0.06));
				}
			}
			if (score < 0)
			{
				score = 0;
			}
			return score;
			static string Normalize(string s)
			{
				if (string.IsNullOrWhiteSpace(s))
				{
					return "";
				}
				s = s.Trim().ToLowerInvariant();
				s = Regex.Replace(s, "[^\\p{L}\\p{Nd}\\s]+", " ");
				s = Regex.Replace(s, "\\s+", " ").Trim();
				return s;
			}
			static HashSet<string> Tokenize(string s)
			{
				HashSet<string> set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
				if (string.IsNullOrWhiteSpace(s))
				{
					return set;
				}
				string[] parts = s.Split(new char[1] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
				for (int j = 0; j < parts.Length; j++)
				{
					string t = parts[j].Trim();
					if (t.Length >= 3)
					{
						switch (t)
						{
						default:
							set.Add(t);
							break;
						case "the":
						case "of":
						case "and":
						case "in":
						case "on":
							break;
						}
					}
				}
				return set;
			}
		}

		private static bool ContainsAny(string text, List<string> terms)
		{
			if (string.IsNullOrWhiteSpace(text) || terms == null || terms.Count == 0)
			{
				return false;
			}
			string norm = NormalizeForContains(text);
			for (int i = 0; i < terms.Count; i++)
			{
				string t = terms[i];
				if (!string.IsNullOrWhiteSpace(t))
				{
					string tn = NormalizeForContains(t);
					if (tn.Length >= 3 && norm.Contains(tn))
					{
						return true;
					}
				}
			}
			return false;
		}

		private static string NormalizeForContains(string s)
		{
			s = (s ?? "").Trim().ToLowerInvariant();
			s = Regex.Replace(s, "[^\\p{L}\\p{Nd}\\s]+", " ");
			s = Regex.Replace(s, "\\s+", " ").Trim();
			return s;
		}

		private static bool IsOldPrefixed(string name)
		{
			if (string.IsNullOrWhiteSpace(name))
			{
				return false;
			}
			return name.TrimStart().StartsWith("Old ", StringComparison.OrdinalIgnoreCase);
		}

		private static bool IsMemoryMapName(string mapName)
		{
			if (string.IsNullOrWhiteSpace(mapName))
			{
				return false;
			}
			return mapName.TrimStart().StartsWith("Memory of", StringComparison.OrdinalIgnoreCase);
		}
	}
}
