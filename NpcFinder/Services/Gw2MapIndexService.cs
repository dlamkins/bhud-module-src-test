using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;

namespace NpcFinder.Services
{
	public class Gw2MapIndexService
	{
		private sealed class MapRectEntry
		{
			public int Id { get; set; }

			public int ContinentId { get; set; }

			public double MinX { get; set; }

			public double MinY { get; set; }

			public double MaxX { get; set; }

			public double MaxY { get; set; }
		}

		private sealed class MapIndexCache
		{
			public Dictionary<string, int> NameToId { get; set; }

			public List<string> AllMapNamesOriginal { get; set; }

			public List<MapRectEntry> RectIndex { get; set; }
		}

		private readonly IGw2WebApiV2Client _v2;

		private readonly CacheStore _cache;

		private const string CacheKey = "gw2-mapindex-v4";

		private Dictionary<string, int> _nameToId;

		private List<string> _allMapNamesOriginal;

		private List<MapRectEntry> _rectIndex;

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

		public async Task<int?> FindMapIdByContinentPointAsync(int cx, int cy, int preferredContinentId, CancellationToken ct)
		{
			await EnsureLoadedAsync(ct).ConfigureAwait(continueOnCapturedContext: false);
			if (_rectIndex == null || _rectIndex.Count == 0)
			{
				return null;
			}
			if (preferredContinentId != 0)
			{
				foreach (MapRectEntry e3 in _rectIndex)
				{
					ct.ThrowIfCancellationRequested();
					if (e3.ContinentId == preferredContinentId && Contains(e3))
					{
						return e3.Id;
					}
				}
			}
			foreach (MapRectEntry e2 in _rectIndex)
			{
				ct.ThrowIfCancellationRequested();
				if (Contains(e2))
				{
					return e2.Id;
				}
			}
			return null;
			bool Contains(MapRectEntry e)
			{
				if ((double)cx >= e.MinX && (double)cx <= e.MaxX && (double)cy >= e.MinY)
				{
					return (double)cy <= e.MaxY;
				}
				return false;
			}
		}

		private static bool TryGetContinentRect(Rectangle r, out double minX, out double minY, out double maxX, out double maxY)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			minX = (minY = (maxX = (maxY = 0.0)));
			try
			{
				Coordinates2 tl = ((Rectangle)(ref r)).get_TopLeft();
				Coordinates2 br = ((Rectangle)(ref r)).get_BottomRight();
				minX = Math.Min(((Coordinates2)(ref tl)).get_X(), ((Coordinates2)(ref br)).get_X());
				maxX = Math.Max(((Coordinates2)(ref tl)).get_X(), ((Coordinates2)(ref br)).get_X());
				minY = Math.Min(((Coordinates2)(ref tl)).get_Y(), ((Coordinates2)(ref br)).get_Y());
				maxY = Math.Max(((Coordinates2)(ref tl)).get_Y(), ((Coordinates2)(ref br)).get_Y());
				if (double.IsNaN(minX) || double.IsNaN(minY) || double.IsNaN(maxX) || double.IsNaN(maxY))
				{
					return false;
				}
				if (Math.Abs(maxX - minX) < 1E-06 || Math.Abs(maxY - minY) < 1E-06)
				{
					return false;
				}
				return true;
			}
			catch
			{
				return false;
			}
		}

		private async Task EnsureLoadedAsync(CancellationToken ct)
		{
			if (_nameToId != null && _allMapNamesOriginal != null && _rectIndex != null)
			{
				return;
			}
			if (_cache.TryLoad<MapIndexCache>("gw2-mapindex-v4", out var cached) && cached != null && cached.NameToId != null && cached.NameToId.Count > 0 && cached.AllMapNamesOriginal != null && cached.AllMapNamesOriginal.Count > 0 && cached.RectIndex != null && cached.RectIndex.Count > 0)
			{
				_nameToId = cached.NameToId;
				_allMapNamesOriginal = cached.AllMapNamesOriginal;
				_rectIndex = cached.RectIndex;
				return;
			}
			ct.ThrowIfCancellationRequested();
			IApiV2ObjectList<Map> obj = await ((IAllExpandableClient<Map>)(object)_v2.get_Maps()).AllAsync(default(CancellationToken)).ConfigureAwait(continueOnCapturedContext: false);
			Dictionary<string, int> dict = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
			List<string> names = new List<string>();
			List<MapRectEntry> rects = new List<MapRectEntry>();
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
					if (TryGetContinentRect(i.get_ContinentRect(), out var minX, out var minY, out var maxX, out var maxY))
					{
						rects.Add(new MapRectEntry
						{
							Id = i.get_Id(),
							ContinentId = i.get_ContinentId(),
							MinX = minX,
							MinY = minY,
							MaxX = maxX,
							MaxY = maxY
						});
					}
				}
			}
			_nameToId = dict;
			_allMapNamesOriginal = (from s in names.Distinct(StringComparer.OrdinalIgnoreCase)
				orderby s
				select s).ToList();
			_rectIndex = rects;
			_cache.Save("gw2-mapindex-v4", new MapIndexCache
			{
				NameToId = _nameToId,
				AllMapNamesOriginal = _allMapNamesOriginal,
				RectIndex = _rectIndex
			});
		}
	}
}
