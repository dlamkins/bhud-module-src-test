using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace KpRefresher.Extensions
{
	public static class EnumExtensions
	{
		public static TAttribute GetAttribute<TAttribute>(this Enum enumValue) where TAttribute : Attribute
		{
			return enumValue.GetType().GetMember(enumValue.ToString()).First()
				.GetCustomAttribute<TAttribute>();
		}

		public static bool HasAttribute<TAttribute>(this Enum enumValue) where TAttribute : Attribute
		{
			return enumValue.GetType().GetMember(enumValue.ToString()).First()
				.GetCustomAttribute<TAttribute>() != null;
		}

		public static string GetDisplayName(this Enum enumValue)
		{
			return enumValue.GetType().GetMember(enumValue.ToString()).FirstOrDefault()?.GetCustomAttribute<DisplayAttribute>().GetDescription() ?? "unknown";
		}
	}
}
