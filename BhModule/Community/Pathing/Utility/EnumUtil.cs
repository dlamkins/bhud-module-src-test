using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace BhModule.Community.Pathing.Utility
{
	public static class EnumUtil
	{
		private static readonly ConcurrentDictionary<Type, Dictionary<string, object>> _cachedEnumLookup = new ConcurrentDictionary<Type, Dictionary<string, object>>();

		public static bool TryParseCacheEnum<T>(string raw, out T value) where T : Enum
		{
			if (!_cachedEnumLookup.TryGetValue(typeof(T), out var enumLookup))
			{
				Array values = Enum.GetValues(typeof(T));
				enumLookup = new Dictionary<string, object>(values.Length, StringComparer.OrdinalIgnoreCase);
				foreach (object enumValue in values)
				{
					enumLookup.Add(enumValue.ToString(), enumValue);
				}
				_cachedEnumLookup.TryAdd(typeof(T), enumLookup);
			}
			if (enumLookup.TryGetValue(raw, out var result))
			{
				value = (T)result;
				return true;
			}
			value = default(T);
			return false;
		}
	}
}
