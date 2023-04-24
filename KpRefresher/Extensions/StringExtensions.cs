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
				if (attribute != null && attribute.Name == name)
				{
					return (T)field.GetValue(null);
				}
				if (field.Name == name)
				{
					return (T)field.GetValue(null);
				}
			}
			throw new ArgumentOutOfRangeException("name");
		}
	}
}
