using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using NpcFinder.Models;

namespace NpcFinder.Services
{
	public class Gw2ApiService
	{
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
			int[] floors = ReadIntArrayProp(map, "Floors");
			Logger log = Logger.GetLogger<Gw2ApiService>();
			try
			{
				log.Info("MapRect type: " + ((object)map.get_MapRect()).GetType().FullName);
				log.Info("ContinentRect type: " + ((object)map.get_ContinentRect()).GetType().FullName);
				log.Info("Floors extracted: " + ((floors == null) ? "null" : string.Join(",", floors)));
			}
			catch
			{
			}
			Rect2D mapRect = ReadRectAny(map.get_MapRect());
			Rect2D contRect = ReadRectAny(map.get_ContinentRect());
			log.Warn($"[RectParse] mapRect=({mapRect.X1},{mapRect.Y1},{mapRect.X2},{mapRect.Y2}) " + $"contRect=({contRect.X1},{contRect.Y1},{contRect.X2},{contRect.Y2}) " + "types: mapRectType=" + ((object)map.get_MapRect()).GetType().FullName + " contRectType=" + ((object)map.get_ContinentRect()).GetType().FullName);
			Gw2MapInfo info = new Gw2MapInfo
			{
				Id = map.get_Id(),
				Name = map.get_Name(),
				ContinentId = map.get_ContinentId(),
				DefaultFloor = map.get_DefaultFloor(),
				Floors = (floors ?? Array.Empty<int>()),
				MapRect = mapRect,
				ContinentRect = contRect
			};
			_cache.Save(key, info);
			return info;
		}

		private static int[] ReadIntArrayProp(object obj, string propName)
		{
			if (obj == null)
			{
				return null;
			}
			PropertyInfo p = obj.GetType().GetProperty(propName, BindingFlags.Instance | BindingFlags.Public);
			if (p == null)
			{
				return null;
			}
			object v;
			try
			{
				v = p.GetValue(obj);
			}
			catch
			{
				return null;
			}
			if (v == null)
			{
				return null;
			}
			int[] ia = v as int[];
			if (ia != null)
			{
				return ia;
			}
			IEnumerable<int> ien = v as IEnumerable<int>;
			if (ien != null)
			{
				return ien.ToArray();
			}
			IEnumerable en = v as IEnumerable;
			if (en != null)
			{
				try
				{
					return (from x in en.Cast<object>().Select((Func<object, int?>)delegate(object o)
						{
							try
							{
								return Convert.ToInt32(o);
							}
							catch
							{
								return null;
							}
						})
						where x.HasValue
						select x.Value).ToArray();
				}
				catch
				{
					return null;
				}
			}
			return null;
		}

		private static Rect2D ReadRectAny(object rectObj)
		{
			if (rectObj == null)
			{
				return new Rect2D(0.0, 0.0, 0.0, 0.0);
			}
			if (TryParseRectFromNestedEnumerable(rectObj, out var r1))
			{
				return r1;
			}
			if (TryGetObjAny(rectObj, new string[5] { "Value", "Coordinates", "Data", "Rect", "Rectangle" }, out var inner) && inner != null && TryParseRectFromNestedEnumerable(inner, out var r2))
			{
				return r2;
			}
			if (TryGetProp(rectObj, "X1", out var x1) && TryGetProp(rectObj, "Y1", out var y1) && TryGetProp(rectObj, "X2", out var x2) && TryGetProp(rectObj, "Y2", out var y2))
			{
				return new Rect2D(x1, y1, x2, y2);
			}
			if (TryGetProp(rectObj, "Left", out var left) && TryGetProp(rectObj, "Top", out var top) && TryGetProp(rectObj, "Right", out var right) && TryGetProp(rectObj, "Bottom", out var bottom))
			{
				return new Rect2D(left, top, right, bottom);
			}
			if (TryGetObj(rectObj, "TopLeft", out var tl) && TryGetObj(rectObj, "BottomRight", out var br) && TryGetProp(tl, "X", out var tlx) && TryGetProp(tl, "Y", out var tly) && TryGetProp(br, "X", out var brx) && TryGetProp(br, "Y", out var bry))
			{
				return new Rect2D(tlx, tly, brx, bry);
			}
			if (TryGetProp(rectObj, "X", out var rx) && TryGetProp(rectObj, "Y", out var ry) && TryGetProp(rectObj, "Width", out var rw) && TryGetProp(rectObj, "Height", out var rh))
			{
				return new Rect2D(rx, ry, rx + rw, ry + rh);
			}
			return new Rect2D(0.0, 0.0, 0.0, 0.0);
		}

		private static bool TryParseRectFromNestedEnumerable(object obj, out Rect2D rect)
		{
			rect = new Rect2D(0.0, 0.0, 0.0, 0.0);
			if (obj == null)
			{
				return false;
			}
			IEnumerable outer = obj as IEnumerable;
			if (outer == null)
			{
				return false;
			}
			List<object> pts = new List<object>();
			foreach (object it in outer)
			{
				if (it != null)
				{
					pts.Add(it);
					if (pts.Count > 2)
					{
						break;
					}
				}
			}
			if (pts.Count != 2)
			{
				return false;
			}
			if (!TryGetTwoNumbers(pts[0], out var x1, out var y1))
			{
				return false;
			}
			if (!TryGetTwoNumbers(pts[1], out var x2, out var y2))
			{
				return false;
			}
			rect = new Rect2D(x1, y1, x2, y2);
			return true;
		}

		private static bool TryGetTwoNumbers(object pt, out double a, out double b)
		{
			a = (b = 0.0);
			if (pt == null)
			{
				return false;
			}
			IEnumerable en = pt as IEnumerable;
			if (en != null)
			{
				List<object> nums = new List<object>();
				foreach (object it in en)
				{
					if (it != null)
					{
						nums.Add(it);
						if (nums.Count > 2)
						{
							break;
						}
					}
				}
				if (nums.Count != 2)
				{
					return false;
				}
				try
				{
					a = Convert.ToDouble(nums[0]);
					b = Convert.ToDouble(nums[1]);
					return true;
				}
				catch
				{
					return false;
				}
			}
			if (TryGetProp(pt, "X", out a) && TryGetProp(pt, "Y", out b))
			{
				return true;
			}
			return false;
		}

		private static bool TryGetObjAny(object o, string[] names, out object value)
		{
			value = null;
			if (o == null)
			{
				return false;
			}
			Type t = o.GetType();
			BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
			for (int i = 0; i < names.Length; i++)
			{
				PropertyInfo p = t.GetProperty(names[i], flags);
				if (p != null)
				{
					try
					{
						value = p.GetValue(o);
						return value != null;
					}
					catch
					{
					}
				}
				FieldInfo f = t.GetField(names[i], flags);
				if (f != null)
				{
					try
					{
						value = f.GetValue(o);
						return value != null;
					}
					catch
					{
					}
				}
			}
			return false;
		}

		private static bool TryGetProp(object o, string name, out double value)
		{
			value = 0.0;
			if (o == null)
			{
				return false;
			}
			PropertyInfo p = o.GetType().GetProperty(name, BindingFlags.Instance | BindingFlags.Public);
			if (p == null)
			{
				return false;
			}
			try
			{
				value = ToDouble(p.GetValue(o));
				return true;
			}
			catch
			{
				return false;
			}
		}

		private static bool TryGetObj(object o, string name, out object value)
		{
			value = null;
			if (o == null)
			{
				return false;
			}
			PropertyInfo p = o.GetType().GetProperty(name, BindingFlags.Instance | BindingFlags.Public);
			if (p == null)
			{
				return false;
			}
			try
			{
				value = p.GetValue(o);
				return value != null;
			}
			catch
			{
				return false;
			}
		}

		private static double ToDouble(object v)
		{
			try
			{
				return Convert.ToDouble(v);
			}
			catch
			{
				return 0.0;
			}
		}
	}
}
