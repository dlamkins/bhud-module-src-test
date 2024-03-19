using System;
using System.Reflection;

namespace Nekres.ChatMacros.Core
{
	public static class ObjectExtensions
	{
		public static FieldInfo GetPrivateField(this object target, string fieldName)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target", "The assignment target cannot be null.");
			}
			if (string.IsNullOrEmpty(fieldName))
			{
				throw new ArgumentException("The field name cannot be null or empty.", "fieldName");
			}
			Type t = target.GetType();
			FieldInfo fi;
			while ((fi = t.GetField(fieldName, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.NonPublic)) == null && (t = t.BaseType) != null)
			{
			}
			return fi;
		}
	}
}
