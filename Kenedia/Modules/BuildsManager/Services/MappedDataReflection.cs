using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;

namespace Kenedia.Modules.BuildsManager.Services
{
	public static class MappedDataReflection
	{
		private static readonly ConcurrentDictionary<Type, PropertyInfo[]> s_cache = new ConcurrentDictionary<Type, PropertyInfo[]>();

		public static PropertyInfo[] GetMappedEntries(Type dataType)
		{
			return s_cache.GetOrAdd(dataType, (Type type) => (from p in type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
				where p.PropertyType.IsGenericType && IsMappedDataEntry(p.PropertyType)
				select p).ToArray());
		}

		private static bool IsMappedDataEntry(Type type)
		{
			while (type != null)
			{
				if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(MappedDataEntry<, >))
				{
					return true;
				}
				type = type.BaseType;
			}
			return false;
		}
	}
}
