using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using NpcFinder.Models;

namespace NpcFinder.Services
{
	public class WikiNpcService
	{
		private sealed class WikiSearchResponse
		{
			public WikiSearchQuery query { get; set; }
		}

		private sealed class WikiSearchQuery
		{
			public List<WikiSearchItem> search { get; set; }
		}

		private sealed class WikiSearchItem
		{
			public string title { get; set; }
		}

		private static readonly string[] TitleBlacklistContains = new string[27]
		{
			"Game updates", "Update", "Patch", "Daily", "Achievement", "Achievements", "Collection", "Collections", "Zone", "Portal",
			"Waypoint", "Vista", "Point of Interest", "POI", "Dungeon", "Fractal", "Strike", "Raid", "Story", "Episode",
			"Chapter", "NPCs in", "List of", "Category:", "Template:", "File:", "Help:"
		};

		private static readonly string[] AnimalExactNames = new string[15]
		{
			"Rabbit", "Cow", "Dog", "Cat", "Deer", "Moa", "Pig", "Chicken", "Sheep", "Spider",
			"Bear", "Raptor", "Skimmer", "Jackal", "Springer"
		};

		private static readonly string[] ValidNpcMarkers = new string[22]
		{
			"{{NPC", "{{Merchant", "{{Vendor", "{{Banker", "{{Crafting", "{{Mystic Forge", "{{Infobox npc", "{{Infobox NPC", "{{Infobox merchant", "{{Infobox Merchant",
			"{{Infobox character", "{{Infobox Character", "{{Character", "{{character", "{{Vendor", "{{vendor", "{{Merchant", "{{merchant", "| race =", "| gender =",
			"| profession =", "| services ="
		};

		private static readonly string[] NotNpcMarkers = new string[11]
		{
			"{{POI", "{{Point of interest", "{{Area", "{{Zone", "{{Settlement", "{{Landmark", "{{Event", "{{Achievement", "{{Collection", "{{Patch",
			"{{Game updates"
		};

		private static readonly Logger Log = Logger.GetLogger<NpcFinderModule>();

		private readonly RateLimiter _rate;

		private readonly CacheStore _cache;

		private readonly HttpClient _http;

		public WikiNpcService(RateLimiter rate, CacheStore cache)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Expected O, but got Unknown
			_rate = rate;
			_cache = cache;
			_http = new HttpClient();
			_http.get_DefaultRequestHeaders().get_UserAgent().ParseAdd("NpcFinder-BlishHUD");
		}

		private static bool IsBlacklistedTitle(string title)
		{
			string[] titleBlacklistContains = TitleBlacklistContains;
			foreach (string bad in titleBlacklistContains)
			{
				if (title.IndexOf(bad, StringComparison.OrdinalIgnoreCase) >= 0)
				{
					return true;
				}
			}
			return false;
		}

		private static bool IsAnimalNpc(string title)
		{
			string[] animalExactNames = AnimalExactNames;
			foreach (string a in animalExactNames)
			{
				if (string.Equals(title, a, StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
			return false;
		}

		private async Task<bool> LooksLikeNpcPageAsync(string title, string mapName, CancellationToken ct)
		{
			WikiLookupResult wiki = await ResolveByTitleAsync(title, ct).ConfigureAwait(continueOnCapturedContext: false);
			if (wiki == null || wiki.Wikitext == null)
			{
				return false;
			}
			string head = wiki.Wikitext.Substring(0, Math.Min(4096, wiki.Wikitext.Length));
			string[] notNpcMarkers = NotNpcMarkers;
			foreach (string bad in notNpcMarkers)
			{
				if (head.IndexOf(bad, StringComparison.OrdinalIgnoreCase) >= 0)
				{
					return false;
				}
			}
			if (wiki.Hits != null)
			{
				foreach (NpcCandidateHit h in wiki.Hits)
				{
					if (!string.IsNullOrWhiteSpace(h.MapName) && string.Equals(h.MapName.Trim(), mapName.Trim(), StringComparison.OrdinalIgnoreCase))
					{
						return true;
					}
				}
			}
			notNpcMarkers = ValidNpcMarkers;
			foreach (string good in notNpcMarkers)
			{
				if (head.IndexOf(good, StringComparison.OrdinalIgnoreCase) >= 0)
				{
					return true;
				}
			}
			return false;
		}

		private async Task<List<string>> GetCategoryMembersAsync(string categoryTitle, int limit, CancellationToken ct)
		{
			limit = Math.Max(1, Math.Min(limit, 500));
			List<string> result = new List<string>();
			string cmcontinue = null;
			JsonElement q = default(JsonElement);
			JsonElement arr = default(JsonElement);
			JsonElement tEl = default(JsonElement);
			JsonElement cont = default(JsonElement);
			JsonElement cmc = default(JsonElement);
			while (result.Count < limit)
			{
				ct.ThrowIfCancellationRequested();
				string url = "https://wiki.guildwars2.com/api.php?action=query&list=categorymembers&cmnamespace=0&cmlimit=" + Math.Min(500, limit - result.Count) + "&format=json&cmtitle=" + Uri.EscapeDataString("Category:" + categoryTitle) + ((cmcontinue != null) ? ("&cmcontinue=" + Uri.EscapeDataString(cmcontinue)) : "");
				JsonDocument doc = JsonDocument.Parse(await DownloadStringAsync(url, ct).ConfigureAwait(continueOnCapturedContext: false), default(JsonDocumentOptions));
				try
				{
					JsonElement rootElement = doc.get_RootElement();
					if (((JsonElement)(ref rootElement)).TryGetProperty("query", ref q) && ((JsonElement)(ref q)).TryGetProperty("categorymembers", ref arr) && (int)((JsonElement)(ref arr)).get_ValueKind() == 2)
					{
						ArrayEnumerator val = ((JsonElement)(ref arr)).EnumerateArray();
						ArrayEnumerator enumerator = ((ArrayEnumerator)(ref val)).GetEnumerator();
						try
						{
							while (((ArrayEnumerator)(ref enumerator)).MoveNext())
							{
								JsonElement el = ((ArrayEnumerator)(ref enumerator)).get_Current();
								if (((JsonElement)(ref el)).TryGetProperty("title", ref tEl))
								{
									string t = ((JsonElement)(ref tEl)).GetString();
									if (!string.IsNullOrWhiteSpace(t))
									{
										result.Add(t);
									}
								}
							}
						}
						finally
						{
							((IDisposable)(ArrayEnumerator)(ref enumerator)).Dispose();
						}
					}
					rootElement = doc.get_RootElement();
					if (((JsonElement)(ref rootElement)).TryGetProperty("continue", ref cont))
					{
						if (((JsonElement)(ref cont)).TryGetProperty("cmcontinue", ref cmc))
						{
							if ((int)((JsonElement)(ref cmc)).get_ValueKind() == 3)
							{
								cmcontinue = ((JsonElement)(ref cmc)).GetString();
								continue;
							}
							return result;
						}
						return result;
					}
					return result;
				}
				finally
				{
					((IDisposable)doc)?.Dispose();
				}
			}
			return result;
		}

		private static List<string> ExtractWikiLinks(string wikitext)
		{
			if (string.IsNullOrWhiteSpace(wikitext))
			{
				return new List<string>();
			}
			List<string> titles = new List<string>();
			foreach (Match item in Regex.Matches(wikitext, "\\[\\[([^\\]\\|#]+)(?:#[^\\]\\|]+)?(?:\\|[^\\]]+)?\\]\\]"))
			{
				string t = item.Groups[1].Value.Trim();
				if (!string.IsNullOrWhiteSpace(t) && !t.Contains(":"))
				{
					titles.Add(t);
				}
			}
			return titles.Distinct(StringComparer.OrdinalIgnoreCase).ToList();
		}

		private async Task<string> FindNpcListPageTitleAsync(string mapName, CancellationToken ct)
		{
			string[] candidates = new string[3]
			{
				mapName + " NPCs",
				"NPCs in " + mapName,
				mapName + " (NPCs)"
			};
			string[] array = candidates;
			foreach (string c in array)
			{
				ct.ThrowIfCancellationRequested();
				if (!string.IsNullOrWhiteSpace(await GetWikitextAsync(c, ct).ConfigureAwait(continueOnCapturedContext: false)))
				{
					return c;
				}
			}
			List<string> hits = await SearchTitlesAsync(mapName + " NPCs", 10, ct).ConfigureAwait(continueOnCapturedContext: false);
			if (hits != null)
			{
				string best = hits.FirstOrDefault((string t) => t.Equals(mapName + " NPCs", StringComparison.OrdinalIgnoreCase) || t.Equals("NPCs in " + mapName, StringComparison.OrdinalIgnoreCase) || t.IndexOf("NPC", StringComparison.OrdinalIgnoreCase) >= 0);
				if (!string.IsNullOrWhiteSpace(best))
				{
					return best;
				}
			}
			return null;
		}

		public async Task<List<string>> SearchNpcTitlesByMapAsync(string mapName, int limit, CancellationToken ct)
		{
			if (string.IsNullOrWhiteSpace(mapName))
			{
				return new List<string>();
			}
			limit = Math.Max(1, Math.Min(limit, 200));
			string key = "wiki-map-npcs-v5-" + mapName.Trim().ToLowerInvariant();
			if (_cache.TryLoad<List<string>>(key, out var cached) && cached != null && cached.Count > 0)
			{
				return cached;
			}
			List<string> all = new List<string>();
			try
			{
				string listPage = await FindNpcListPageTitleAsync(mapName, ct).ConfigureAwait(continueOnCapturedContext: false);
				if (!string.IsNullOrWhiteSpace(listPage))
				{
					WikiLookupResult listWiki = await ResolveByTitleAsync(listPage, ct).ConfigureAwait(continueOnCapturedContext: false);
					if (listWiki != null && listWiki.Wikitext != null)
					{
						all.AddRange(ExtractWikiLinks(listWiki.Wikitext));
					}
				}
			}
			catch (Exception ex)
			{
				Log.Warn($"[MapMode] listPage extraction failed: {ex}");
			}
			foreach (string cat in await FindNpcCategoryTitlesForMapAsync(mapName, 10, ct).ConfigureAwait(continueOnCapturedContext: false))
			{
				ct.ThrowIfCancellationRequested();
				List<string> mem = await GetCategoryMembersAsync(cat, 500, ct).ConfigureAwait(continueOnCapturedContext: false);
				if (mem != null && mem.Count > 0)
				{
					all.AddRange(mem);
				}
			}
			if (all.Distinct(StringComparer.OrdinalIgnoreCase).Count() < Math.Min(25, limit))
			{
				string[] queries = new string[7]
				{
					"incategory:\"NPCs\" " + mapName,
					"incategory:\"Merchants\" " + mapName,
					"incategory:\"Vendors\" " + mapName,
					"incategory:\"Bankers\" " + mapName,
					"incategory:\"Traders\" " + mapName,
					"insource:\"location = [[" + mapName + "]]\"",
					"insource:\"location = " + mapName + "\""
				};
				string[] array = queries;
				foreach (string q in array)
				{
					ct.ThrowIfCancellationRequested();
					List<string> part = await SearchRawTitlesAsync(q, 50, ct).ConfigureAwait(continueOnCapturedContext: false);
					if (part != null && part.Count > 0)
					{
						all.AddRange(part);
					}
				}
			}
			int candidateCap = Math.Min(400, Math.Max(120, limit * 12));
			List<string> candidates = (from t in all
				where !string.IsNullOrWhiteSpace(t)
				where !IsBlacklistedTitle(t)
				where !IsAnimalNpc(t)
				select t).Distinct(StringComparer.OrdinalIgnoreCase).Take(candidateCap).ToList();
			List<string> final = new List<string>(limit);
			foreach (string t2 in candidates)
			{
				ct.ThrowIfCancellationRequested();
				if (await LooksLikeNpcPageAsync(t2, mapName, ct).ConfigureAwait(continueOnCapturedContext: false))
				{
					final.Add(t2);
				}
				if (final.Count >= limit)
				{
					break;
				}
			}
			if (final.Count > 0)
			{
				_cache.Save(key, final);
			}
			return final;
		}

		private async Task<List<string>> FindNpcCategoryTitlesForMapAsync(string mapName, int max, CancellationToken ct)
		{
			string[] searches = new string[4]
			{
				"intitle:\"" + mapName + "\" intitle:npc",
				"intitle:\"" + mapName + "\" intitle:merchant",
				"intitle:\"" + mapName + "\" intitle:vendor",
				"intitle:\"" + mapName + "\" intitle:character"
			};
			List<string> results = new List<string>();
			string[] array = searches;
			JsonElement q = default(JsonElement);
			JsonElement arr = default(JsonElement);
			JsonElement tEl = default(JsonElement);
			foreach (string s in array)
			{
				ct.ThrowIfCancellationRequested();
				string url = "https://wiki.guildwars2.com/api.php?action=query&list=search&srnamespace=14&srlimit=20&format=json&srsearch=" + Uri.EscapeDataString(s);
				JsonDocument doc = JsonDocument.Parse(await DownloadStringAsync(url, ct).ConfigureAwait(continueOnCapturedContext: false), default(JsonDocumentOptions));
				try
				{
					JsonElement rootElement = doc.get_RootElement();
					if (((JsonElement)(ref rootElement)).TryGetProperty("query", ref q) && ((JsonElement)(ref q)).TryGetProperty("search", ref arr) && (int)((JsonElement)(ref arr)).get_ValueKind() == 2)
					{
						ArrayEnumerator val = ((JsonElement)(ref arr)).EnumerateArray();
						ArrayEnumerator enumerator = ((ArrayEnumerator)(ref val)).GetEnumerator();
						try
						{
							while (((ArrayEnumerator)(ref enumerator)).MoveNext())
							{
								JsonElement el = ((ArrayEnumerator)(ref enumerator)).get_Current();
								if (!((JsonElement)(ref el)).TryGetProperty("title", ref tEl))
								{
									continue;
								}
								string t = ((JsonElement)(ref tEl)).GetString();
								if (!string.IsNullOrWhiteSpace(t))
								{
									if (t.StartsWith("Category:", StringComparison.OrdinalIgnoreCase))
									{
										t = t.Substring("Category:".Length);
									}
									if (t.IndexOf(mapName, StringComparison.OrdinalIgnoreCase) >= 0)
									{
										results.Add(t);
									}
								}
							}
						}
						finally
						{
							((IDisposable)(ArrayEnumerator)(ref enumerator)).Dispose();
						}
					}
				}
				finally
				{
					((IDisposable)doc)?.Dispose();
				}
				if (results.Distinct(StringComparer.OrdinalIgnoreCase).Count() >= max)
				{
					break;
				}
			}
			return results.Distinct(StringComparer.OrdinalIgnoreCase).Take(max).ToList();
		}

		private async Task<List<string>> SearchRawTitlesAsync(string srsearch, int limit, CancellationToken ct)
		{
			if (string.IsNullOrWhiteSpace(srsearch))
			{
				return new List<string>();
			}
			limit = Math.Max(1, Math.Min(limit, 50));
			string url = "https://wiki.guildwars2.com/api.php?action=query&list=search&srnamespace=0&srlimit=" + limit + "&format=json&srsearch=" + Uri.EscapeDataString(srsearch);
			string obj = await DownloadStringAsync(url, ct);
			List<string> result = new List<string>();
			JsonDocument doc = JsonDocument.Parse(obj, default(JsonDocumentOptions));
			try
			{
				JsonElement rootElement = doc.get_RootElement();
				JsonElement q = default(JsonElement);
				JsonElement arr = default(JsonElement);
				if (((JsonElement)(ref rootElement)).TryGetProperty("query", ref q) && ((JsonElement)(ref q)).TryGetProperty("search", ref arr) && (int)((JsonElement)(ref arr)).get_ValueKind() == 2)
				{
					ArrayEnumerator val = ((JsonElement)(ref arr)).EnumerateArray();
					ArrayEnumerator enumerator = ((ArrayEnumerator)(ref val)).GetEnumerator();
					try
					{
						JsonElement tEl = default(JsonElement);
						while (((ArrayEnumerator)(ref enumerator)).MoveNext())
						{
							JsonElement el = ((ArrayEnumerator)(ref enumerator)).get_Current();
							if (((JsonElement)(ref el)).TryGetProperty("title", ref tEl))
							{
								string t = ((JsonElement)(ref tEl)).GetString();
								if (!string.IsNullOrWhiteSpace(t))
								{
									result.Add(t);
								}
							}
						}
					}
					finally
					{
						((IDisposable)(ArrayEnumerator)(ref enumerator)).Dispose();
					}
				}
			}
			finally
			{
				((IDisposable)doc)?.Dispose();
			}
			return result;
		}

		public async Task<WikiLookupResult> ResolveByNpcNameAsync(string npcName, CancellationToken ct)
		{
			string key = "wiki-name-v2-" + npcName.Trim().ToLowerInvariant();
			if (_cache.TryLoad<WikiLookupResult>(key, out var cached) && cached != null)
			{
				return cached;
			}
			await _rate.WaitAsync(ct);
			List<string> titles = await SearchTitlesAsync(npcName, 20, ct);
			if (titles.Count == 0)
			{
				return null;
			}
			if (titles.Count > 1)
			{
				WikiLookupResult res = new WikiLookupResult
				{
					CandidateTitles = titles
				};
				_cache.Save(key, res);
				return res;
			}
			WikiLookupResult single = await ResolveByTitleAsync(titles[0], ct);
			if (single != null)
			{
				_cache.Save(key, single);
			}
			return single;
		}

		public async Task<List<string>> SearchTitlesAsync(string query, int limit, CancellationToken ct)
		{
			if (string.IsNullOrWhiteSpace(query))
			{
				return new List<string>();
			}
			limit = Math.Max(1, Math.Min(limit, 20));
			string sr = "intitle:" + query.Trim();
			string url = "https://wiki.guildwars2.com/api.php?action=query&list=search&srnamespace=0&srlimit=" + limit + "&format=json&srsearch=" + Uri.EscapeDataString(sr);
			string obj = await DownloadStringAsync(url, ct);
			List<string> result = new List<string>();
			JsonDocument doc = JsonDocument.Parse(obj, default(JsonDocumentOptions));
			try
			{
				JsonElement rootElement = doc.get_RootElement();
				JsonElement q = default(JsonElement);
				JsonElement arr = default(JsonElement);
				if (((JsonElement)(ref rootElement)).TryGetProperty("query", ref q) && ((JsonElement)(ref q)).TryGetProperty("search", ref arr) && (int)((JsonElement)(ref arr)).get_ValueKind() == 2)
				{
					ArrayEnumerator val = ((JsonElement)(ref arr)).EnumerateArray();
					ArrayEnumerator enumerator = ((ArrayEnumerator)(ref val)).GetEnumerator();
					try
					{
						JsonElement tEl = default(JsonElement);
						while (((ArrayEnumerator)(ref enumerator)).MoveNext())
						{
							JsonElement el = ((ArrayEnumerator)(ref enumerator)).get_Current();
							if (((JsonElement)(ref el)).TryGetProperty("title", ref tEl))
							{
								string t = ((JsonElement)(ref tEl)).GetString();
								if (!string.IsNullOrWhiteSpace(t))
								{
									result.Add(t);
								}
							}
						}
					}
					finally
					{
						((IDisposable)(ArrayEnumerator)(ref enumerator)).Dispose();
					}
				}
			}
			finally
			{
				((IDisposable)doc)?.Dispose();
			}
			return result.Distinct(StringComparer.OrdinalIgnoreCase).ToList();
		}

		public async Task<WikiLookupResult> ResolveByTitleAsync(string title, CancellationToken ct)
		{
			string key = "wiki-title-v2-" + title.Trim().ToLowerInvariant();
			if (_cache.TryLoad<WikiLookupResult>(key, out var cached) && cached != null && !string.IsNullOrWhiteSpace(cached.Wikitext))
			{
				return cached;
			}
			await _rate.WaitAsync(ct);
			string wikitext = await GetWikitextAsync(title, ct);
			if (string.IsNullOrWhiteSpace(wikitext))
			{
				return null;
			}
			string pageMapName = ParseMapNameBestEffort(wikitext);
			List<(int x, int y, string nearText)> list = ParseAllCoordinates(wikitext);
			List<NpcCandidateHit> hits = new List<NpcCandidateHit>();
			foreach (var c in list)
			{
				string mapName = ParseMapNameNearText(c.nearText) ?? pageMapName;
				List<NpcCandidateHit> list2 = hits;
				NpcCandidateHit obj = new NpcCandidateHit
				{
					Title = title,
					MapName = mapName,
					MapId = null
				};
				(obj.X, obj.Y, _) = c;
				list2.Add(obj);
			}
			hits = (from h in hits
				group h by h.MapName + "|" + h.X + "|" + h.Y into g
				select g.First()).ToList();
			WikiLookupResult res = new WikiLookupResult
			{
				Title = title,
				DisplayName = title,
				Wikitext = wikitext,
				Hits = hits
			};
			_cache.Save(key, res);
			return res;
		}

		private async Task<string> GetWikitextAsync(string title, CancellationToken ct)
		{
			string url = "https://wiki.guildwars2.com/api.php?action=parse&redirects=1&prop=wikitext&format=json&formatversion=2&page=" + Uri.EscapeDataString(title);
			JsonDocument doc = JsonDocument.Parse(await DownloadStringAsync(url, ct).ConfigureAwait(continueOnCapturedContext: false), default(JsonDocumentOptions));
			try
			{
				JsonElement root = doc.get_RootElement();
				JsonElement err = default(JsonElement);
				if (((JsonElement)(ref root)).TryGetProperty("error", ref err))
				{
					return null;
				}
				JsonElement parse = default(JsonElement);
				if (!((JsonElement)(ref root)).TryGetProperty("parse", ref parse))
				{
					return null;
				}
				JsonElement wt = default(JsonElement);
				if (((JsonElement)(ref parse)).TryGetProperty("wikitext", ref wt))
				{
					if ((int)((JsonElement)(ref wt)).get_ValueKind() == 3)
					{
						return ((JsonElement)(ref wt)).GetString();
					}
					JsonElement star = default(JsonElement);
					if ((int)((JsonElement)(ref wt)).get_ValueKind() == 1 && ((JsonElement)(ref wt)).TryGetProperty("*", ref star) && (int)((JsonElement)(ref star)).get_ValueKind() == 3)
					{
						return ((JsonElement)(ref star)).GetString();
					}
				}
				return null;
			}
			finally
			{
				((IDisposable)doc)?.Dispose();
			}
		}

		private async Task<string> DownloadStringAsync(string url, CancellationToken ct)
		{
			int backoffMs = 900;
			for (int attempt = 1; attempt <= 4; attempt++)
			{
				ct.ThrowIfCancellationRequested();
				try
				{
					using WebClient wc = new WebClient();
					wc.Headers[HttpRequestHeader.UserAgent] = "NpcFinder-BlishHUD";
					Task<string> dlTask = wc.DownloadStringTaskAsync(url);
					if (await Task.WhenAny(dlTask, Task.Delay(TimeSpan.FromSeconds(12.0), ct)).ConfigureAwait(continueOnCapturedContext: false) != dlTask)
					{
						throw new WebException("Wiki request timed out.");
					}
					return await dlTask.ConfigureAwait(continueOnCapturedContext: false);
				}
				catch (WebException ex)
				{
					int code = 0;
					try
					{
						HttpWebResponse resp = ex.Response as HttpWebResponse;
						if (resp != null)
						{
							code = (int)resp.StatusCode;
						}
					}
					catch
					{
					}
					if ((code != 429 && code != 503 && code != 502 && code != 504 && ex.Status != WebExceptionStatus.Timeout && ex.Status != WebExceptionStatus.ConnectFailure && ex.Status != WebExceptionStatus.NameResolutionFailure) || attempt == 4)
					{
						throw;
					}
					int jitter = new Random().Next(0, 250);
					int millisecondsDelay = backoffMs + jitter;
					backoffMs = Math.Min(backoffMs * 2, 6000);
					await Task.Delay(millisecondsDelay, ct).ConfigureAwait(continueOnCapturedContext: false);
				}
			}
			throw new WebException("Wiki request failed after retries.");
		}

		private static string ParseMapNameBestEffort(string wikitext)
		{
			return TryMatch("\\|\\s*map\\s*=\\s*([^\\r\\n]+)") ?? TryMatch("\\|\\s*location\\s*=\\s*([^\\r\\n]+)") ?? TryMatch("\\|\\s*zone\\s*=\\s*([^\\r\\n]+)") ?? null;
			string TryMatch(string pattern)
			{
				Match i = new Regex(pattern, RegexOptions.IgnoreCase).Match(wikitext);
				if (!i.Success)
				{
					return null;
				}
				string raw = i.Groups[1].Value.Trim();
				raw = Regex.Replace(raw, "\\[\\[([^\\]\\|]+)(\\|[^\\]]+)?\\]\\]", "$1");
				raw = Regex.Replace(raw, "\\{\\{[^}]+\\}\\}", "");
				raw = Regex.Replace(raw, "<!--.*?-->", "");
				raw = raw.Trim();
				if (raw.Length < 3)
				{
					return null;
				}
				raw = raw.Split(new string[2] { "<br", "\n" }, StringSplitOptions.None)[0].Trim();
				raw = raw.Trim().TrimEnd('.', ',', ';');
				if (!string.IsNullOrWhiteSpace(raw))
				{
					return raw;
				}
				return null;
			}
		}

		private static List<(int x, int y, string nearText)> ParseAllCoordinates(string wikitext)
		{
			List<(int, int, string)> list = new List<(int, int, string)>();
			if (string.IsNullOrWhiteSpace(wikitext))
			{
				return list;
			}
			foreach (Match k in Regex.Matches(wikitext, "\\[\\s*(?<x>-?\\d{1,6}(?:\\.\\d+)?)\\s*,\\s*(?<y>-?\\d{1,6}(?:\\.\\d+)?)\\s*\\]"))
			{
				if (double.TryParse(k.Groups["x"].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out var xd4) && double.TryParse(k.Groups["y"].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out var yd4))
				{
					Add(xd4, yd4, k.Index, k.Length);
				}
			}
			foreach (Match j in Regex.Matches(wikitext, "(?im)\\bcoordinates\\s*=\\s*(?:\\[\\s*)?(?<x>-?\\d{1,6}(?:\\.\\d+)?)\\s*,\\s*(?<y>-?\\d{1,6}(?:\\.\\d+)?)"))
			{
				if (double.TryParse(j.Groups["x"].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out var xd3) && double.TryParse(j.Groups["y"].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out var yd3))
				{
					Add(xd3, yd3, j.Index, j.Length);
				}
			}
			foreach (Match i in Regex.Matches(wikitext, "(?is)\\|\\s*x\\s*=\\s*(?<x>-?\\d{1,6}(?:\\.\\d+)?)\\s*\\|\\s*y\\s*=\\s*(?<y>-?\\d{1,6}(?:\\.\\d+)?)"))
			{
				if (double.TryParse(i.Groups["x"].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out var xd2) && double.TryParse(i.Groups["y"].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out var yd2))
				{
					Add(xd2, yd2, i.Index, i.Length);
				}
			}
			return (from t in list
				group t by t.Item1 + "|" + t.Item2 into g
				select g.First()).ToList();
			void Add(double xd, double yd, int idx, int len)
			{
				int x2 = (int)Math.Round(xd);
				int y2 = (int)Math.Round(yd);
				if (IsPlausible(x2, y2))
				{
					int start = Math.Max(0, idx - 140);
					int end = Math.Min(wikitext.Length, idx + len + 140);
					string near = wikitext.Substring(start, end - start);
					list.Add((x2, y2, near));
				}
			}
			static bool IsPlausible(int x, int y)
			{
				if (x < -1000 || y < -1000)
				{
					return false;
				}
				if (x > 300000 || y > 300000)
				{
					return false;
				}
				return true;
			}
		}

		public async Task<List<string>> SuggestTitlesAsync(string prefix, int limit, CancellationToken ct)
		{
			prefix = (prefix ?? "").Trim();
			if (prefix.Length < 2)
			{
				return new List<string>();
			}
			limit = Math.Max(1, Math.Min(limit, 20));
			string key = $"wiki-suggest-v1-{prefix.ToLowerInvariant()}-{limit}";
			if (_cache.TryLoad<List<string>>(key, out var cached) && cached != null && cached.Count > 0)
			{
				return cached;
			}
			await _rate.WaitAsync(ct);
			string url = "https://wiki.guildwars2.com/api.php?action=query&list=prefixsearch&pslimit=" + limit + "&pssearch=" + Uri.EscapeDataString(prefix) + "&format=json";
			string obj = await DownloadStringAsync(url, ct);
			List<string> list = new List<string>();
			JsonDocument doc = JsonDocument.Parse(obj, default(JsonDocumentOptions));
			try
			{
				JsonElement rootElement = doc.get_RootElement();
				JsonElement q = default(JsonElement);
				JsonElement arr = default(JsonElement);
				if (((JsonElement)(ref rootElement)).TryGetProperty("query", ref q) && ((JsonElement)(ref q)).TryGetProperty("prefixsearch", ref arr) && (int)((JsonElement)(ref arr)).get_ValueKind() == 2)
				{
					ArrayEnumerator val = ((JsonElement)(ref arr)).EnumerateArray();
					ArrayEnumerator enumerator = ((ArrayEnumerator)(ref val)).GetEnumerator();
					try
					{
						JsonElement tEl = default(JsonElement);
						while (((ArrayEnumerator)(ref enumerator)).MoveNext())
						{
							JsonElement el = ((ArrayEnumerator)(ref enumerator)).get_Current();
							if (((JsonElement)(ref el)).TryGetProperty("title", ref tEl))
							{
								string t = ((JsonElement)(ref tEl)).GetString();
								if (!string.IsNullOrWhiteSpace(t))
								{
									list.Add(t);
								}
							}
						}
					}
					finally
					{
						((IDisposable)(ArrayEnumerator)(ref enumerator)).Dispose();
					}
				}
			}
			finally
			{
				((IDisposable)doc)?.Dispose();
			}
			list = list.Distinct().Take(limit).ToList();
			if (list.Count > 0)
			{
				_cache.Save(key, list);
			}
			return list;
		}

		private static string ParseMapNameNearText(string nearText)
		{
			return TryMatch("\\|\\s*map\\s*=\\s*([^\\r\\n]+)") ?? TryMatch("\\|\\s*location\\s*=\\s*([^\\r\\n]+)") ?? TryMatch("\\|\\s*zone\\s*=\\s*([^\\r\\n]+)") ?? null;
			string TryMatch(string pattern)
			{
				Match i = new Regex(pattern, RegexOptions.IgnoreCase).Match(nearText);
				if (!i.Success)
				{
					return null;
				}
				string raw = i.Groups[1].Value.Trim();
				raw = Regex.Replace(raw, "\\[\\[([^\\]\\|]+)(\\|[^\\]]+)?\\]\\]", "$1");
				raw = Regex.Replace(raw, "\\{\\{[^}]+\\}\\}", "");
				raw = Regex.Replace(raw, "<!--.*?-->", "");
				raw = raw.Trim();
				if (raw.Length < 3)
				{
					return null;
				}
				raw = raw.Split(new string[4] { "<br", "\n", "|", "}" }, StringSplitOptions.None)[0].Trim();
				raw = raw.Trim().TrimEnd('.', ',', ';');
				if (!string.IsNullOrWhiteSpace(raw))
				{
					return raw;
				}
				return null;
			}
		}
	}
}
