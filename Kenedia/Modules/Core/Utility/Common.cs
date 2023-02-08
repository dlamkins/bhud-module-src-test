using System;
using System.Reflection;
using Blish_HUD;

namespace Kenedia.Modules.Core.Utility
{
	public class Common
	{
		public static double Now()
		{
			return GameService.Overlay.get_CurrentGameTime().get_TotalGameTime().TotalMilliseconds;
		}

		public static bool SetProperty<T>(ref T property, T newValue, bool triggerOnUpdate = true, Action OnUpdated = null)
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
	}
}
