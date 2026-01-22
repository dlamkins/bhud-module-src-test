using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using Blish_HUD;
using Blish_HUD.Gw2Mumble;
using Microsoft.Xna.Framework;

namespace NpcFinder.Util
{
	public static class MumbleReader
	{
		private sealed class ReferenceEqualityComparer : IEqualityComparer<object>
		{
			public static readonly ReferenceEqualityComparer Instance = new ReferenceEqualityComparer();

			public new bool Equals(object x, object y)
			{
				return x == y;
			}

			public int GetHashCode(object obj)
			{
				return RuntimeHelpers.GetHashCode(obj);
			}
		}

		private static readonly bool DEBUG_LOGS = false;

		private static readonly Logger Log = Logger.GetLogger(typeof(MumbleReader));

		private static object _uiObj;

		private static object _mapIdObj;

		private static bool _discovered;

		private static DateTime _lastWarn = DateTime.MinValue;

		private static DateTime _lastDumpUtc = DateTime.MinValue;

		private static bool _didHeaderDump = false;

		private static bool _dumped;

		public static void ResetDiscovery()
		{
			_discovered = false;
			_uiObj = null;
			_mapIdObj = null;
		}

		public static bool TryGetMapId(out int mapId)
		{
			mapId = 0;
			EnsureDiscovered();
			object src = _mapIdObj ?? GameService.Gw2Mumble;
			if (src == null)
			{
				return false;
			}
			if (TryGetInt(src, new string[3] { "MapId", "CurrentMapId", "mapId" }, out mapId))
			{
				return mapId > 0;
			}
			return false;
		}

		public static bool TryGetUiState(out uint uiState)
		{
			uiState = 0u;
			EnsureDiscovered();
			object src = _uiObj;
			if (src == null)
			{
				return false;
			}
			if (!TryGetUInt(src, new string[3] { "UiState", "UIState", "uiState" }, out uiState))
			{
				WarnOccasionally("[MumbleReader] UiState missing on discovered uiObj.");
				return false;
			}
			return true;
		}

		public static void DumpUiOnce()
		{
			if (_dumped)
			{
				return;
			}
			_dumped = true;
			try
			{
				Gw2MumbleService gw2Mumble = GameService.Gw2Mumble;
				UI ui = ((gw2Mumble != null) ? gw2Mumble.get_UI() : null);
				if (ui == null)
				{
					if (DEBUG_LOGS)
					{
						Log.Warn("[MumbleUI] GameService.Gw2Mumble.UI is null");
					}
				}
				else
				{
					if (!DEBUG_LOGS)
					{
						return;
					}
					Log.Warn("[MumbleUI] UI type=" + ((object)ui).GetType().FullName);
					DumpProp(ui, "IsMapOpen");
					DumpProp(ui, "IsCompassTopRight");
					DumpProp(ui, "MapCenter");
					DumpProp(ui, "MapScale");
					DumpProp(ui, "MapRotation");
					DumpProp(ui, "CompassRotation");
					PropertyInfo[] properties = ((object)ui).GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
					foreach (PropertyInfo p in properties)
					{
						string i = p.Name.ToLowerInvariant();
						if (i.Contains("map") || i.Contains("compass") || i.Contains("scale") || i.Contains("center") || i.Contains("zoom") || i.Contains("rotation"))
						{
							object val = null;
							try
							{
								val = p.GetValue(ui);
							}
							catch
							{
							}
							Log.Warn("[MumbleUI] " + p.Name + " = " + Fmt(val));
						}
					}
					return;
				}
			}
			catch (Exception ex)
			{
				Log.Warn("Exception [MumbleUI] Dump failed: " + ex);
			}
		}

		private static void DumpProp(object obj, string propName)
		{
			PropertyInfo p = obj.GetType().GetProperty(propName, BindingFlags.Instance | BindingFlags.Public);
			if (p == null)
			{
				Log.Warn("[MumbleUI] " + propName + " = <missing>");
				return;
			}
			object val = null;
			try
			{
				val = p.GetValue(obj);
			}
			catch
			{
				val = "<error reading>";
			}
			Log.Warn("[MumbleUI] " + propName + " = " + Fmt(val));
		}

		private static string Fmt(object v)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			if (v == null)
			{
				return "null";
			}
			if (v is Vector2)
			{
				Vector2 vv = (Vector2)v;
				return $"({vv.X},{vv.Y})";
			}
			return v.ToString();
		}

		public static void DumpUiOncePerSecond(bool requireMapOpen = true)
		{
			if (!DEBUG_LOGS)
			{
				return;
			}
			try
			{
				Gw2MumbleService gw2Mumble = GameService.Gw2Mumble;
				UI ui = ((gw2Mumble != null) ? gw2Mumble.get_UI() : null);
				if (ui == null)
				{
					WarnOccasionally("[MumbleUI] GameService.Gw2Mumble.UI is null");
					return;
				}
				bool mapOpen = false;
				try
				{
					mapOpen = ui.get_IsMapOpen();
				}
				catch
				{
				}
				if (requireMapOpen && !mapOpen)
				{
					return;
				}
				DateTime now = DateTime.UtcNow;
				if ((now - _lastDumpUtc).TotalSeconds < 1.0)
				{
					return;
				}
				_lastDumpUtc = now;
				if (!_didHeaderDump)
				{
					_didHeaderDump = true;
					Log.Warn("[MumbleUI] UI type=" + ((object)ui).GetType().FullName);
					PropertyInfo[] properties = ((object)ui).GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
					foreach (PropertyInfo p in properties)
					{
						string i = p.Name.ToLowerInvariant();
						if (i.Contains("map") || i.Contains("compass") || i.Contains("scale") || i.Contains("center") || i.Contains("zoom") || i.Contains("rotation") || i.Contains("position"))
						{
							object val = null;
							try
							{
								val = p.GetValue(ui);
							}
							catch
							{
							}
							Log.Warn("[MumbleUI] " + p.Name + " = " + Fmt(val));
						}
					}
				}
				Log.Warn($"MumbleUI open={SafeBool(() => ui.get_IsMapOpen())}" + $"scale={SafeObj(() => ui.get_MapScale())} " + "center=" + Fmt(SafeObj(() => ui.get_MapCenter())) + " pos=" + Fmt(SafeObj(() => GetAnyProp(ui, "MapPosition"))));
			}
			catch (Exception ex)
			{
				WarnOccasionally("[MumbleUI] Dump failed: " + ex.Message);
			}
		}

		private static object SafeObj(Func<object> f)
		{
			try
			{
				return f();
			}
			catch
			{
				return null;
			}
		}

		private static bool SafeBool(Func<bool> f)
		{
			try
			{
				return f();
			}
			catch
			{
				return false;
			}
		}

		private static object GetAnyProp(object obj, string propName)
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
			try
			{
				return p.GetValue(obj);
			}
			catch
			{
				return null;
			}
		}

		private static void EnsureDiscovered()
		{
			if (_discovered)
			{
				return;
			}
			_discovered = true;
			try
			{
				Gw2MumbleService deepRoot = GameService.Gw2Mumble;
				if (deepRoot != null)
				{
					_uiObj = FindBestContainer(deepRoot, new string[3] { "UiState", "UIState", "uiState" }, new string[3] { "MapScale", "mapScale", "WorldMapScale" }, new string[2][]
					{
						new string[3] { "MapCenter", "mapCenter", "WorldMapCenter" },
						new string[2] { "MapCenterX", "mapCenterX" }
					});
					_mapIdObj = FindBestContainer(deepRoot, new string[3] { "MapId", "CurrentMapId", "mapId" }, null, null);
					if (DEBUG_LOGS)
					{
						Log.Debug("[MumbleReader] discovery: root=" + ((object)deepRoot).GetType().FullName + " uiObj=" + (_uiObj?.GetType().FullName ?? "null") + " mapIdObj=" + (_mapIdObj?.GetType().FullName ?? "null"));
					}
				}
			}
			catch (Exception ex)
			{
				WarnOccasionally("[MumbleReader] discovery failed: " + ex.Message);
			}
		}

		private static object FindBestContainer(object root, string[] mustHaveAny, string[] alsoNeed, string[][] andOneOfGroups)
		{
			Queue<(object, int)> q = new Queue<(object, int)>();
			HashSet<object> seen = new HashSet<object>(ReferenceEqualityComparer.Instance);
			q.Enqueue((root, 0));
			seen.Add(root);
			while (q.Count > 0)
			{
				var (obj, depth) = q.Dequeue();
				if (obj == null)
				{
					continue;
				}
				if (!(obj.GetType().FullName ?? "").EndsWith(".Info", StringComparison.OrdinalIgnoreCase) && HasAnyMember(obj, mustHaveAny) && (alsoNeed == null || HasAnyMember(obj, alsoNeed)) && (andOneOfGroups == null || HasOneOfGroups(obj, andOneOfGroups)))
				{
					return obj;
				}
				if (depth >= 5)
				{
					continue;
				}
				foreach (object child in EnumerateChildren(obj))
				{
					if (child != null && !seen.Contains(child))
					{
						seen.Add(child);
						q.Enqueue((child, depth + 1));
					}
				}
			}
			return null;
		}

		private static bool HasOneOfGroups(object obj, string[][] groups)
		{
			for (int i = 0; i < groups.Length; i++)
			{
				if (HasAnyMember(obj, groups[i]))
				{
					return true;
				}
			}
			return false;
		}

		private static bool HasAnyMember(object obj, string[] names)
		{
			if (obj == null || names == null || names.Length == 0)
			{
				return false;
			}
			Type t = obj.GetType();
			BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
			for (int i = 0; i < names.Length; i++)
			{
				if (t.GetProperty(names[i], flags) != null)
				{
					return true;
				}
				if (t.GetField(names[i], flags) != null)
				{
					return true;
				}
			}
			return false;
		}

		[IteratorStateMachine(typeof(_003CEnumerateChildren_003Ed__24))]
		private static IEnumerable<object> EnumerateChildren(object obj)
		{
			return new _003CEnumerateChildren_003Ed__24(-2)
			{
				_003C_003E3__obj = obj
			};
		}

		private static bool IsWalkable(object v)
		{
			if (v == null)
			{
				return false;
			}
			Type t = v.GetType();
			if (t == typeof(string) || t.IsPrimitive)
			{
				return false;
			}
			return true;
		}

		private static bool TryGetObj(object obj, string[] names, out object value)
		{
			value = null;
			if (obj == null)
			{
				return false;
			}
			Type t = obj.GetType();
			BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
			for (int i = 0; i < names.Length; i++)
			{
				PropertyInfo p = t.GetProperty(names[i], flags);
				if (p != null)
				{
					try
					{
						value = p.GetValue(obj);
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
						value = f.GetValue(obj);
						return value != null;
					}
					catch
					{
					}
				}
			}
			return false;
		}

		private static bool TryGetFloat(object obj, string[] names, out float value)
		{
			value = 0f;
			if (!TryGetObj(obj, names, out var v))
			{
				return false;
			}
			try
			{
				value = Convert.ToSingle(v);
				return true;
			}
			catch
			{
				return false;
			}
		}

		private static bool TryGetInt(object obj, string[] names, out int value)
		{
			value = 0;
			if (!TryGetObj(obj, names, out var v))
			{
				return false;
			}
			try
			{
				value = Convert.ToInt32(v);
				return true;
			}
			catch
			{
				return false;
			}
		}

		private static bool TryGetUInt(object obj, string[] names, out uint value)
		{
			value = 0u;
			if (!TryGetObj(obj, names, out var v))
			{
				return false;
			}
			try
			{
				value = Convert.ToUInt32(v);
				return true;
			}
			catch
			{
				return false;
			}
		}

		private static void WarnOccasionally(string msg)
		{
			if (DEBUG_LOGS)
			{
				DateTime now = DateTime.UtcNow;
				if (!((now - _lastWarn).TotalSeconds < 2.0))
				{
					_lastWarn = now;
					Log.Warn(msg);
				}
			}
		}

		public static bool TryGetWorldMapUi(out float centerX, out float centerY, out float scale)
		{
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			centerX = (centerY = 0f);
			scale = 1f;
			Gw2MumbleService gw2Mumble = GameService.Gw2Mumble;
			UI ui = ((gw2Mumble != null) ? gw2Mumble.get_UI() : null);
			if (ui == null)
			{
				return false;
			}
			if (!TryToFloat(GetProp(new string[2] { "MapScale", "WorldMapScale" }), out scale) || Math.Abs(scale) < 1E-06f)
			{
				return false;
			}
			object centerObj = GetProp(new string[2] { "MapCenter", "WorldMapCenter" });
			if (centerObj is Vector2)
			{
				Vector2 v2 = (Vector2)centerObj;
				centerX = v2.X;
				centerY = v2.Y;
				return true;
			}
			if (centerObj != null)
			{
				Type t = centerObj.GetType();
				if (!(t.GetProperty("Width") != null) && !(t.GetProperty("Height") != null) && !(t.GetProperty("X1") != null) && !(t.GetProperty("X2") != null) && !(t.GetProperty("Y1") != null) && !(t.GetProperty("Y2") != null) && TryGetXY(centerObj, out centerX, out centerY))
				{
					return true;
				}
			}
			return false;
			object GetProp(string[] names)
			{
				Type t2 = ((object)ui).GetType();
				foreach (string i in names)
				{
					PropertyInfo p = t2.GetProperty(i, BindingFlags.Instance | BindingFlags.Public);
					if (p != null)
					{
						return p.GetValue(ui);
					}
				}
				return null;
			}
		}

		private static bool TryToFloat(object o, out float v)
		{
			v = 0f;
			if (o == null)
			{
				return false;
			}
			try
			{
				if (!(o is float))
				{
					if (o is double)
					{
						double d = (double)o;
						v = (float)d;
						return true;
					}
					if (o is int)
					{
						int i = (int)o;
						v = i;
						return true;
					}
					string s = o as string;
					if (s != null)
					{
						return float.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out v) || float.TryParse(s, NumberStyles.Float, CultureInfo.CurrentCulture, out v);
					}
					v = Convert.ToSingle(o, CultureInfo.InvariantCulture);
					return true;
				}
				float f = (v = (float)o);
				return true;
			}
			catch
			{
				return false;
			}
		}

		private static bool TryGetXY(object obj, out float x, out float y)
		{
			x = (y = 0f);
			if (obj == null)
			{
				return false;
			}
			try
			{
				Type type = obj.GetType();
				PropertyInfo px = type.GetProperty("X", BindingFlags.Instance | BindingFlags.Public);
				PropertyInfo py = type.GetProperty("Y", BindingFlags.Instance | BindingFlags.Public);
				if (px == null || py == null)
				{
					return false;
				}
				object value = px.GetValue(obj);
				object oy = py.GetValue(obj);
				return TryToFloat(value, out x) && TryToFloat(oy, out y);
			}
			catch
			{
				return false;
			}
		}
	}
}
