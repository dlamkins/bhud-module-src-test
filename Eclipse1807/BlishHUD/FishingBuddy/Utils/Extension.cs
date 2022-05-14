using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Eclipse1807.BlishHUD.FishingBuddy.Properties;

namespace Eclipse1807.BlishHUD.FishingBuddy.Utils
{
	public static class Extension
	{
		public static string GetEnumMemberValue(this Enum value)
		{
			string enumToString = value.GetType().GetMember(value.ToString()).FirstOrDefault()?.GetCustomAttribute<EnumMemberAttribute>(inherit: false)?.Value ?? value.ToString();
			return Strings.ResourceManager.GetString(enumToString, Strings.Culture);
		}
	}
}
