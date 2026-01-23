using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using NpcFinder.Models;

namespace NpcFinder.Services
{
	public class Gw2ApiService
	{
		private static readonly bool DEBUG_LOGS;

		private readonly IGw2WebApiV2Client _v2;

		private readonly CacheStore _cache;

		public Gw2ApiService(IGw2WebApiV2Client v2, CacheStore cache)
		{
			_v2 = v2;
			_cache = cache;
		}

		private static bool IsValidRect(Rect2D r)
		{
			if ((r.X1 != 0.0 || r.Y1 != 0.0 || r.X2 != 0.0 || r.Y2 != 0.0) && r.X2 != r.X1)
			{
				return r.Y2 != r.Y1;
			}
			return false;
		}

		public async Task<Gw2MapInfo> GetMapInfoAsync(int mapId, CancellationToken ct)
		{
			string key = "gw2-map-" + mapId;
			if (_cache.TryLoad<Gw2MapInfo>(key, out var cached) && cached != null && IsValidRect(cached.MapRect) && IsValidRect(cached.ContinentRect))
			{
				return cached;
			}
			ct.ThrowIfCancellationRequested();
			Map map = await ((IBulkExpandableClient<Map, int>)(object)_v2.get_Maps()).GetAsync(mapId, default(CancellationToken)).ConfigureAwait(continueOnCapturedContext: false);
			if (map == null)
			{
				return null;
			}
			int regionId = map.get_RegionId();
			int[] floors = null;
			if (map.get_Floors() != null)
			{
				floors = map.get_Floors().ToArray();
			}
			Rect2D mapRect = RectFromRectangle(map.get_MapRect());
			Rect2D contRect = RectFromRectangle(map.get_ContinentRect());
			Logger log = Logger.GetLogger<Gw2ApiService>();
			if (DEBUG_LOGS)
			{
				try
				{
					log.Info("MapRect type: " + ((object)map.get_MapRect()).GetType().FullName);
					log.Info("ContinentRect type: " + ((object)map.get_ContinentRect()).GetType().FullName);
					log.Info("Floors extracted: " + ((floors == null) ? "null" : string.Join(",", floors)));
					log.Warn($"[RectParse] mapRect=({mapRect.X1},{mapRect.Y1},{mapRect.X2},{mapRect.Y2}) " + $"contRect=({contRect.X1},{contRect.Y1},{contRect.X2},{contRect.Y2}) " + "types: mapRectType=" + ((object)map.get_MapRect()).GetType().FullName + " contRectType=" + ((object)map.get_ContinentRect()).GetType().FullName);
				}
				catch
				{
				}
			}
			Gw2MapInfo info = new Gw2MapInfo
			{
				Id = map.get_Id(),
				Name = map.get_Name(),
				ContinentId = map.get_ContinentId(),
				DefaultFloor = map.get_DefaultFloor(),
				RegionId = regionId,
				Floors = (floors ?? Array.Empty<int>()),
				MapRect = mapRect,
				ContinentRect = contRect
			};
			_cache.Save(key, info);
			return info;
		}

		private static Rect2D RectFromRectangle(Rectangle r)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			if (TryGetXY(((Rectangle)(ref r)).get_TopLeft(), out var x1, out var y1) && TryGetXY(((Rectangle)(ref r)).get_BottomRight(), out var x2, out var y2))
			{
				return new Rect2D(x1, y1, x2, y2);
			}
			if (TryGetXY(((Rectangle)(ref r)).get_BottomLeft(), out x1, out y1) && TryGetXY(((Rectangle)(ref r)).get_TopRight(), out x2, out y2))
			{
				return new Rect2D(x1, y1, x2, y2);
			}
			if (TryGetXY(((Rectangle)(ref r)).get_TopRight(), out var trX, out var trY) && TryGetXY(((Rectangle)(ref r)).get_BottomLeft(), out var blX, out var blY))
			{
				return new Rect2D(blX, trY, trX, blY);
			}
			if (TryGetXY(((Rectangle)(ref r)).get_TopRight(), out var xTR, out var yTR) && ((Rectangle)(ref r)).get_Width() != 0.0 && ((Rectangle)(ref r)).get_Height() != 0.0)
			{
				x2 = xTR;
				y1 = yTR;
				x1 = x2 - ((Rectangle)(ref r)).get_Width();
				y2 = y1 + ((Rectangle)(ref r)).get_Height();
				return new Rect2D(x1, y1, x2, y2);
			}
			if (TryGetXY(((Rectangle)(ref r)).get_BottomRight(), out var xBR, out var yBR) && ((Rectangle)(ref r)).get_Width() != 0.0 && ((Rectangle)(ref r)).get_Height() != 0.0)
			{
				x2 = xBR;
				y2 = yBR;
				x1 = x2 - ((Rectangle)(ref r)).get_Width();
				y1 = y2 - ((Rectangle)(ref r)).get_Height();
				return new Rect2D(x1, y1, x2, y2);
			}
			return new Rect2D(0.0, 0.0, 0.0, 0.0);
		}

		private static bool TryGetXY(Coordinates2 c, out double x, out double y)
		{
			x = (y = 0.0);
			try
			{
				x = ((Coordinates2)(ref c)).get_X();
				y = ((Coordinates2)(ref c)).get_Y();
				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}
