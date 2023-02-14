using System;
using System.Reflection;
using Blish_HUD;
using Gw2Sharp.WebApi;

namespace Kenedia.Modules.Core.Utility
{
	public static class Common
	{
		public static double Now()
		{
			return GameService.Overlay.get_CurrentGameTime().get_TotalGameTime().TotalMilliseconds;
		}

		public static bool SetProperty<T>(ref T property, T newValue, Action OnUpdated = null, bool triggerOnUpdate = true)
		{
			if (object.Equals(property, newValue))
			{
				return false;
			}
			property = newValue;
			if (triggerOnUpdate)
			{
				OnUpdated?.Invoke();
			}
			return true;
		}

		public static T GetPropertyValue<T>(object obj, string propName)
		{
			PropertyInfo p = obj.GetType().GetProperty(propName);
			if (p == null)
			{
				return default(T);
			}
			object o = p.GetValue(obj, null);
			if (o == null)
			{
				return default(T);
			}
			if (o.GetType() == typeof(T))
			{
				return (T)o;
			}
			return default(T);
		}

		public static string GetPropertyValueAsString(object obj, string propName)
		{
			PropertyInfo p = obj.GetType().GetProperty(propName);
			if (p == null)
			{
				return null;
			}
			return p.GetValue(obj, null)?.ToString();
		}

		public static int GetAssetIdFromRenderUrl(this RenderUrl? url)
		{
			if (!url.HasValue)
			{
				return 0;
			}
			string s = url.ToString();
			int pos = s.LastIndexOf("/") + 1;
			if (!int.TryParse(s.Substring(pos, s.Length - pos - 4), out var id))
			{
				return 0;
			}
			return id;
		}

		public static int GetAssetIdFromRenderUrl(this RenderUrl url)
		{
			string s = ((object)(RenderUrl)(ref url)).ToString();
			int pos = s.LastIndexOf("/") + 1;
			if (!int.TryParse(s.Substring(pos, s.Length - pos - 4), out var id))
			{
				return 0;
			}
			return id;
		}

		public static int GetAssetIdFromRenderUrl(string s)
		{
			int pos = s.ToString().LastIndexOf("/") + 1;
			if (!int.TryParse(s.Substring(pos, s.Length - pos - 4), out var id))
			{
				return 0;
			}
			return id;
		}
	}
}
