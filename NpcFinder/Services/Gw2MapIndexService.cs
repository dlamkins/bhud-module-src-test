using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;

namespace NpcFinder.Services
{
	public class Gw2MapIndexService
	{
		private readonly IGw2WebApiV2Client _v2;

		private readonly CacheStore _cache;

		private const string CacheKey = "gw2-mapindex-v2";

		private Dictionary<string, int> _nameToId;

		private static string Normalize(string s)
		{
			return s.Trim().ToLowerInvariant();
		}

		public Gw2MapIndexService(IGw2WebApiV2Client v2, CacheStore cache)
		{
			_v2 = v2;
			_cache = cache;
		}

		public async Task<int?> ResolveMapIdByNameAsync(string mapName, CancellationToken ct)
		{
			if (string.IsNullOrWhiteSpace(mapName))
			{
				return null;
			}
			await EnsureLoadedAsync(ct);
			string i = Normalize(mapName);
			int id;
			return _nameToId.TryGetValue(i, out id) ? new int?(id) : null;
		}

		private async Task EnsureLoadedAsync(CancellationToken ct)
		{
			if (_nameToId != null)
			{
				return;
			}
			if (_cache.TryLoad<Dictionary<string, int>>("gw2-mapindex-v2", out var cached) && cached != null && cached.Count > 0)
			{
				_nameToId = cached;
				return;
			}
			ct.ThrowIfCancellationRequested();
			IApiV2ObjectList<Map> obj = await ((IAllExpandableClient<Map>)(object)_v2.get_Maps()).AllAsync(default(CancellationToken));
			Dictionary<string, int> dict = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
			foreach (Map j in (IEnumerable<Map>)obj)
			{
				if (!string.IsNullOrWhiteSpace(j.get_Name()))
				{
					string i = Normalize(j.get_Name());
					if (!dict.ContainsKey(i))
					{
						dict[i] = j.get_Id();
					}
				}
			}
			_nameToId = dict;
			_cache.Save("gw2-mapindex-v2", dict);
		}
	}
}
