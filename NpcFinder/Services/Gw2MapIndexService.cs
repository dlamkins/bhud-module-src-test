using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;

namespace NpcFinder.Services
{
	public class Gw2MapIndexService
	{
		private sealed class MapIndexCache
		{
			public Dictionary<string, int> NameToId { get; set; }

			public List<string> AllMapNamesOriginal { get; set; }
		}

		private readonly IGw2WebApiV2Client _v2;

		private readonly CacheStore _cache;

		private const string CacheKey = "gw2-mapindex-v3";

		private Dictionary<string, int> _nameToId;

		private List<string> _allMapNamesOriginal;

		private static string Normalize(string s)
		{
			return (s ?? "").Trim().ToLowerInvariant();
		}

		public Gw2MapIndexService(IGw2WebApiV2Client v2, CacheStore cache)
		{
			_v2 = v2;
			_cache = cache;
		}

		public async Task<List<int>> GetAllKnownMapIdsAsync(CancellationToken ct)
		{
			await EnsureLoadedAsync(ct).ConfigureAwait(continueOnCapturedContext: false);
			return _nameToId.Values.Distinct().ToList();
		}

		public async Task<int?> ResolveMapIdByNameAsync(string mapName, CancellationToken ct)
		{
			if (string.IsNullOrWhiteSpace(mapName))
			{
				return null;
			}
			await EnsureLoadedAsync(ct).ConfigureAwait(continueOnCapturedContext: false);
			string i = Normalize(mapName);
			int id;
			return _nameToId.TryGetValue(i, out id) ? new int?(id) : null;
		}

		public async Task<List<string>> SuggestMapNamesAsync(string prefix, int limit, CancellationToken ct)
		{
			prefix = (prefix ?? "").Trim();
			if (prefix.Length < 2)
			{
				return new List<string>();
			}
			limit = Math.Max(1, Math.Min(limit, 20));
			await EnsureLoadedAsync(ct).ConfigureAwait(continueOnCapturedContext: false);
			string p = Normalize(prefix);
			return _allMapNamesOriginal.Where((string n) => Normalize(n).StartsWith(p)).Take(limit).ToList();
		}

		private async Task EnsureLoadedAsync(CancellationToken ct)
		{
			if (_nameToId != null && _allMapNamesOriginal != null)
			{
				return;
			}
			if (_cache.TryLoad<MapIndexCache>("gw2-mapindex-v3", out var cached) && cached != null && cached.NameToId != null && cached.NameToId.Count > 0 && cached.AllMapNamesOriginal != null && cached.AllMapNamesOriginal.Count > 0)
			{
				_nameToId = cached.NameToId;
				_allMapNamesOriginal = cached.AllMapNamesOriginal;
				return;
			}
			ct.ThrowIfCancellationRequested();
			IApiV2ObjectList<Map> obj = await ((IAllExpandableClient<Map>)(object)_v2.get_Maps()).AllAsync(default(CancellationToken));
			Dictionary<string, int> dict = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
			List<string> names = new List<string>();
			foreach (Map i in (IEnumerable<Map>)obj)
			{
				if (!string.IsNullOrWhiteSpace(i.get_Name()))
				{
					string norm = Normalize(i.get_Name());
					if (!dict.ContainsKey(norm))
					{
						dict[norm] = i.get_Id();
					}
					names.Add(i.get_Name().Trim());
				}
			}
			_nameToId = dict;
			_allMapNamesOriginal = (from s in names.Distinct(StringComparer.OrdinalIgnoreCase)
				orderby s
				select s).ToList();
			_cache.Save("gw2-mapindex-v3", new MapIndexCache
			{
				NameToId = _nameToId,
				AllMapNamesOriginal = _allMapNamesOriginal
			});
		}
	}
}
