using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

		private static readonly Logger Logger = Logger.GetLogger<NpcMerchantResolverService>();

		private readonly WikiNpcService _wiki;

		private readonly Gw2MapIndexService _mapIndex;

		private readonly Gw2ApiService _gw2;

		private readonly Gw2MapDetailsService _details;

		private readonly string _cacheDir;

		private static readonly TimeSpan CacheTtl = TimeSpan.FromDays(14.0);

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

		public async Task<List<NpcResolvedHit>> ResolveMerchantAsync(string npcTitle, CancellationToken ct)
		{
			if (TryLoadCachedResolvedHits(npcTitle, out var cached))
			{
				Logger.Debug("[MerchantResolve] CACHE HIT title='" + npcTitle + "' hits=" + cached.Count);
				return cached;
			}
			WikiLookupResult wikiRes = await _wiki.ResolveByTitleAsync(npcTitle, ct).ConfigureAwait(continueOnCapturedContext: false);
			if (wikiRes == null)
			{
				return new List<NpcResolvedHit>();
			}
			string wikitext = wikiRes.Wikitext ?? "";
			List<string> hints = BuildLocationHints(npcTitle, wikitext);
			Logger.Debug("[MerchantResolve] title='" + npcTitle + "' hints=(" + hints.Count + ") " + string.Join(" | ", hints.Take(12)));
			List<int> mapIds = await ResolveAllMapIdsFromHintsAsync(hints, ct).ConfigureAwait(continueOnCapturedContext: false);
			if (mapIds.Count == 0)
			{
				Logger.Warn("[MerchantResolve] could not resolve any mapId from hints.");
				return new List<NpcResolvedHit>();
			}
			List<(string mapName, int x, int y)> coordHints = ExtractNpcCoordinates(wikitext);
			if (coordHints.Count > 0)
			{
				Logger.Warn("[MerchantResolve] extracted " + coordHints.Count + " coord hint(s): " + string.Join(" | ", from c in coordHints.Take(5)
					select "[" + c.x + "," + c.y + "] map='" + c.mapName + "'"));
			}
			List<NpcResolvedHit> hits = new List<NpcResolvedHit>();
			for (int mi = 0; mi < mapIds.Count; mi++)
			{
				int mapId = mapIds[mi];
				ct.ThrowIfCancellationRequested();
				Gw2MapInfo mapInfo = await _gw2.GetMapInfoAsync(mapId, ct).ConfigureAwait(continueOnCapturedContext: false);
				if (mapInfo == null)
				{
					Logger.Warn("[MerchantResolve] mapInfo null for mapId=" + mapId);
					continue;
				}
				Logger.Info("[MapInfo] mapId=" + mapId + " name='" + mapInfo.Name + "' continentId=" + mapInfo.ContinentId + " defaultFloor=" + mapInfo.DefaultFloor);
				PoiWpFloorResult anchors = await _details.GetPoisAndWaypointsWithFloorFallbackAsync(mapInfo.ContinentId, mapInfo.DefaultFloor, mapInfo.Floors, mapInfo.Id, ct).ConfigureAwait(continueOnCapturedContext: false);
				Logger.Debug("[MerchantResolve] mapId=" + mapInfo.Id + " usedFloor=" + anchors.UsedFloor + " pois=" + anchors.Pois.Count + " wps=" + anchors.Waypoints.Count);
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
					for (int j = 0; j < anchors.Waypoints.Count; j++)
					{
						Tuple<string, int, int> w = anchors.Waypoints[j];
						int s = ScoreCandidate(w.Item1, hints);
						if (s > 0)
						{
							candidates.Add(new ScoredCandidate("Waypoint", w.Item1, w.Item2, w.Item3, s));
						}
					}
					for (int i = 0; i < anchors.Pois.Count; i++)
					{
						Tuple<string, int, int> p = anchors.Pois[i];
						int s2 = ScoreCandidate(p.Item1, hints);
						if (s2 > 0)
						{
							candidates.Add(new ScoredCandidate("POI", p.Item1, p.Item2, p.Item3, s2));
						}
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
						orderby c.Score descending, c.Kind
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
				Logger.Warn("[MerchantResolve] HIT map=" + mapInfo.Name + " cont=" + mapInfo.ContinentId + " best=" + best.Kind + ":" + best.Name + " @" + best.X + "," + best.Y + " -> continent=(" + cx + "," + cy + ")");
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
			hits = hits.OrderBy((NpcResolvedHit h) => h.MapName, StringComparer.OrdinalIgnoreCase).ThenBy((NpcResolvedHit h) => (!h.Source.StartsWith("Waypoint:", StringComparison.OrdinalIgnoreCase)) ? 1 : 0).ToList();
			if (hits.Count == 0)
			{
				Logger.Warn("[MerchantResolve] No hits produced.");
			}
			SaveCachedResolvedHits(npcTitle, hits);
			return hits;
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
				Logger.Warn("[MerchantResolve] cache read failed: " + ex.Message);
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
					Logger.Debug("[MerchantResolve] cache write OK path=" + path);
				}
			}
			catch (Exception ex)
			{
				Logger.Warn("[MerchantResolve] cache write failed: " + ex.Message);
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
				if (!Regex.IsMatch(name, "\\b(the|a|an|merchant|npc|bandit|scout|animal|farmer|watchman)\\b", RegexOptions.IgnoreCase))
				{
					int? id = await _mapIndex.ResolveMapIdByNameAsync(name, ct).ConfigureAwait(continueOnCapturedContext: false);
					Logger.Debug("[MerchantResolve] try mapName='" + name + "' => mapId=" + (id.HasValue ? id.Value.ToString() : "null"));
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

		private List<string> BuildLocationHints(string title, string wikitext)
		{
			List<string> ordered = new List<string>();
			string infobox = ExtractInfoboxMap(wikitext);
			if (!string.IsNullOrWhiteSpace(infobox))
			{
				ordered.Add(infobox);
			}
			List<string> locLinks = ExtractLinksFromSection(wikitext, "Location");
			for (int m = 0; m < locLinks.Count; m++)
			{
				ordered.Add(locLinks[m]);
			}
			List<string> lead = ExtractLeadSentenceLocation(wikitext);
			for (int l = 0; l < lead.Count; l++)
			{
				ordered.Add(lead[l]);
			}
			List<string> phrases = ExtractPlainLocationPhrases(wikitext);
			for (int k = 0; k < phrases.Count; k++)
			{
				ordered.Add(phrases[k]);
			}
			List<string> allLinks = ExtractAllLinks(wikitext);
			for (int j = 0; j < allLinks.Count; j++)
			{
				ordered.Add(allLinks[j]);
			}
			HashSet<string> seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			List<string> final = new List<string>();
			for (int i = 0; i < ordered.Count; i++)
			{
				string s = CleanHint(ordered[i]);
				if (!string.IsNullOrWhiteSpace(s) && seen.Add(s))
				{
					final.Add(s);
				}
			}
			return final;
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
					Match j = new Regex("\\|\\s*" + Regex.Escape(i) + "\\s*=\\s*([^\\r\\n\\|]+)", RegexOptions.IgnoreCase).Match(wikitext);
					if (j.Success)
					{
						string raw = j.Groups[1].Value.Trim();
						raw = Regex.Replace(raw, "\\[\\[([^\\]\\|]+)(\\|[^\\]]+)?\\]\\]", "$1");
						raw = Regex.Replace(raw, "<.*?>", "");
						raw = Regex.Replace(raw, "\\{\\{.*?\\}\\}", "", RegexOptions.Singleline);
						raw = raw.Split(new string[2] { "<br", "\n" }, StringSplitOptions.None)[0].Trim();
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
			s = s.Replace(";", "|");
			return s;
		}

		private int ScoreCandidate(string candidateName, List<string> hints)
		{
			if (string.IsNullOrWhiteSpace(candidateName) || hints == null || hints.Count == 0)
			{
				return 0;
			}
			string c = candidateName.Trim();
			string cNorm = Normalize(c);
			int score = 0;
			if (cNorm.Contains("waypoint"))
			{
				score += 60;
			}
			string[] localityKeywords = new string[6] { "village of", "town of", "city of", "hamlet of", "outpost of", "settlement of" };
			for (int i = 0; i < hints.Count; i++)
			{
				string hRaw = hints[i];
				if (string.IsNullOrWhiteSpace(hRaw))
				{
					continue;
				}
				string h = hRaw.Trim();
				if (h.Length < 3)
				{
					continue;
				}
				int colon = h.IndexOf(':');
				if (colon > 0 && colon <= 3)
				{
					continue;
				}
				string hNorm = Normalize(h);
				switch (hNorm)
				{
				case "the":
				case "merchant":
				case "npc":
					continue;
				}
				if (string.Equals(cNorm, hNorm, StringComparison.OrdinalIgnoreCase))
				{
					score += 220;
					continue;
				}
				if (cNorm.Contains(hNorm) || hNorm.Contains(cNorm))
				{
					score += 120;
				}
				foreach (string key in localityKeywords)
				{
					if (hNorm.StartsWith(key))
					{
						string core = hNorm.Substring(key.Length).Trim();
						if (core.Length >= 3 && (cNorm.Contains(core) || core.Contains(cNorm)))
						{
							score += 220;
						}
					}
				}
				HashSet<string> cTokens = Tokenize(cNorm);
				HashSet<string> hashSet = Tokenize(hNorm);
				int overlap = 0;
				foreach (string token in hashSet)
				{
					if (cTokens.Contains(token))
					{
						overlap++;
					}
				}
				score += overlap * 25;
			}
			if (c.IndexOf("Waypoint", StringComparison.OrdinalIgnoreCase) >= 0)
			{
				score += 20;
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
				for (int k = 0; k < parts.Length; k++)
				{
					string t = parts[k].Trim();
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
	}
}
