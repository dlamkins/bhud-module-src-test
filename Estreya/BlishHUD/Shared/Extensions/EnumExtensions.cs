using System;
using System.Collections.Generic;
using System.Linq;

namespace Estreya.BlishHUD.Shared.Extensions
{
	public static class EnumExtensions
	{
		public static IEnumerable<Enum> GetFlags(this Enum e)
		{
			return Enum.GetValues(e.GetType()).Cast<Enum>().Where(e.HasFlag);
		}

		public static T GetAttributeOfType<T>(this Enum enumVal) where T : Attribute
		{
			object[] attributes = enumVal.GetType().GetMember(enumVal.ToString())[0].GetCustomAttributes(typeof(T), inherit: false);
			if (attributes.Length == 0)
			{
				return null;
			}
			return (T)attributes[0];
		}
	}
}
