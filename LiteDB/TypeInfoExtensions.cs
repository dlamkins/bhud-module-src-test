using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace LiteDB
{
	internal static class TypeInfoExtensions
	{
		public static bool IsAnonymousType(this Type type)
		{
			if (type.FullName.Contains("AnonymousType"))
			{
				return type.GetTypeInfo().GetCustomAttributes(typeof(CompilerGeneratedAttribute), inherit: false).Any();
			}
			return false;
		}

		public static bool IsEnumerable(this Type type)
		{
			if (type != typeof(string))
			{
				return typeof(IEnumerable).IsAssignableFrom(type);
			}
			return false;
		}
	}
}
