using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace KpRefresher.Extensions
{
	public static class StringExtensions
	{
		public static T GetValueFromName<T>(this string name) where T : Enum
		{
			FieldInfo[] fields = typeof(T).GetFields();
			foreach (FieldInfo field in fields)
			{
				DisplayAttribute attribute = Attribute.GetCustomAttribute(field, typeof(DisplayAttribute)) as DisplayAttribute;
				if (attribute != null && string.Equals(attribute.Name, name, StringComparison.InvariantCultureIgnoreCase))
				{
					return (T)field.GetValue(null);
				}
				if (string.Equals(field.Name, name, StringComparison.InvariantCultureIgnoreCase))
				{
					return (T)field.GetValue(null);
				}
			}
			throw new ArgumentOutOfRangeException("name");
		}
	}
}
