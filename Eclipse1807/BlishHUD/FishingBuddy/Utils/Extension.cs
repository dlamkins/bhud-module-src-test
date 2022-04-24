using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Eclipse1807.BlishHUD.FishingBuddy.Utils
{
	public static class Extension
	{
		public static string GetEnumMemberValue(this Enum value)
		{
			string ret = value.GetType().GetMember(value.ToString()).FirstOrDefault()?.GetCustomAttribute<EnumMemberAttribute>(inherit: false)?.Value;
			if (ret == null)
			{
				return value.ToString();
			}
			return ret;
		}
	}
}
