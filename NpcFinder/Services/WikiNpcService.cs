using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using NpcFinder.Models;

namespace NpcFinder.Services
{
	public class WikiNpcService
	{
		private readonly RateLimiter _rate;

		private readonly CacheStore _cache;

		public WikiNpcService(RateLimiter rate, CacheStore cache)
		{
			_rate = rate;
			_cache = cache;
		}

		public async Task<WikiLookupResult> ResolveByNpcNameAsync(string npcName, CancellationToken ct)
		{
			string key = "wiki-name-v2-" + npcName.Trim().ToLowerInvariant();
			if (_cache.TryLoad<WikiLookupResult>(key, out var cached) && cached != null)
			{
				return cached;
			}
			await _rate.WaitAsync(ct);
			List<string> titles = await SearchTitlesAsync(npcName, ct);
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

		private async Task<List<string>> SearchTitlesAsync(string query, CancellationToken ct)
		{
			string url = "https://wiki.guildwars2.com/api.php?action=query&list=search&srlimit=20&srsearch=" + Uri.EscapeDataString(query) + "&format=json";
			JsonDocument doc = JsonDocument.Parse(await DownloadStringAsync(url, ct), default(JsonDocumentOptions));
			try
			{
				JsonElement val = doc.get_RootElement();
				val = ((JsonElement)(ref val)).GetProperty("query");
				JsonElement arr = ((JsonElement)(ref val)).GetProperty("search");
				List<string> list = new List<string>();
				ArrayEnumerator val2 = ((JsonElement)(ref arr)).EnumerateArray();
				ArrayEnumerator enumerator = ((ArrayEnumerator)(ref val2)).GetEnumerator();
				try
				{
					while (((ArrayEnumerator)(ref enumerator)).MoveNext())
					{
						JsonElement el = ((ArrayEnumerator)(ref enumerator)).get_Current();
						val = ((JsonElement)(ref el)).GetProperty("title");
						string t = ((JsonElement)(ref val)).GetString();
						if (!string.IsNullOrWhiteSpace(t))
						{
							list.Add(t);
						}
					}
				}
				finally
				{
					((IDisposable)(ArrayEnumerator)(ref enumerator)).Dispose();
				}
				return list.Distinct().ToList();
			}
			finally
			{
				((IDisposable)doc)?.Dispose();
			}
		}

		private async Task<string> GetWikitextAsync(string title, CancellationToken ct)
		{
			string url = "https://wiki.guildwars2.com/api.php?action=parse&redirects=1&prop=wikitext&format=json&formatversion=2&page=" + Uri.EscapeDataString(title);
			JsonDocument doc = JsonDocument.Parse(await DownloadStringAsync(url, ct), default(JsonDocumentOptions));
			try
			{
				JsonElement rootElement = doc.get_RootElement();
				JsonElement parse = ((JsonElement)(ref rootElement)).GetProperty("parse");
				JsonElement wt = default(JsonElement);
				if (((JsonElement)(ref parse)).TryGetProperty("wikitext", ref wt))
				{
					if ((int)((JsonElement)(ref wt)).get_ValueKind() == 3)
					{
						return ((JsonElement)(ref wt)).GetString();
					}
					JsonElement star = default(JsonElement);
					if ((int)((JsonElement)(ref wt)).get_ValueKind() == 1 && ((JsonElement)(ref wt)).TryGetProperty("*", ref star))
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
			using WebClient wc = new WebClient();
			wc.Headers[HttpRequestHeader.UserAgent] = "NpcFinder-BlishHUD";
			ct.ThrowIfCancellationRequested();
			return await wc.DownloadStringTaskAsync(url);
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
			foreach (Match i in new Regex("\\[\\s*(\\d{1,5})\\s*,\\s*(\\d{1,5})\\s*\\]").Matches(wikitext))
			{
				if (int.TryParse(i.Groups[1].Value, out var x) && int.TryParse(i.Groups[2].Value, out var y) && x >= 0 && x <= 40000 && y >= 0 && y <= 40000)
				{
					int idx = i.Index;
					int start = Math.Max(0, idx - 120);
					int end = Math.Min(wikitext.Length, idx + i.Length + 120);
					string near = wikitext.Substring(start, end - start);
					list.Add((x, y, near));
				}
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
