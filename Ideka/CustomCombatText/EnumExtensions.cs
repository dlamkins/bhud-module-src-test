using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Ideka.CustomCombatText
{
	internal static class EnumExtensions
	{
		public static string? GetEnumMemberValue<T>(this T value) where T : Enum
		{
			T value2 = value;
			return typeof(T).GetTypeInfo().DeclaredMembers.SingleOrDefault((MemberInfo x) => x.Name == value2.ToString())?.GetCustomAttribute<EnumMemberAttribute>(inherit: false)?.Value;
		}
	}
}
