using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SQLite
{
	public static class Orm
	{
		public const int DefaultMaxStringLength = 140;

		public const string ImplicitPkName = "Id";

		public const string ImplicitIndexSuffix = "Id";

		public static Type GetType(object obj)
		{
			if (obj == null)
			{
				return typeof(object);
			}
			IReflectableType rt = obj as IReflectableType;
			if (rt != null)
			{
				return rt.GetTypeInfo().AsType();
			}
			return obj.GetType();
		}

		public static string SqlDecl(TableMapping.Column p, bool storeDateTimeAsTicks, bool storeTimeSpanAsTicks)
		{
			string decl = "\"" + p.Name + "\" " + SqlType(p, storeDateTimeAsTicks, storeTimeSpanAsTicks) + " ";
			if (p.IsPK)
			{
				decl += "primary key ";
			}
			if (p.IsAutoInc)
			{
				decl += "autoincrement ";
			}
			if (!p.IsNullable)
			{
				decl += "not null ";
			}
			if (!string.IsNullOrEmpty(p.Collation))
			{
				decl = decl + "collate " + p.Collation + " ";
			}
			return decl;
		}

		public static string SqlType(TableMapping.Column p, bool storeDateTimeAsTicks, bool storeTimeSpanAsTicks)
		{
			Type clrType = p.ColumnType;
			if (clrType == typeof(bool) || clrType == typeof(byte) || clrType == typeof(ushort) || clrType == typeof(sbyte) || clrType == typeof(short) || clrType == typeof(int) || clrType == typeof(uint) || clrType == typeof(long))
			{
				return "integer";
			}
			if (clrType == typeof(float) || clrType == typeof(double) || clrType == typeof(decimal))
			{
				return "float";
			}
			if (clrType == typeof(string) || clrType == typeof(StringBuilder) || clrType == typeof(Uri) || clrType == typeof(UriBuilder))
			{
				int? len = p.MaxStringLength;
				if (len.HasValue)
				{
					return "varchar(" + len.Value + ")";
				}
				return "varchar";
			}
			if (clrType == typeof(TimeSpan))
			{
				if (!storeTimeSpanAsTicks)
				{
					return "time";
				}
				return "bigint";
			}
			if (clrType == typeof(DateTime))
			{
				if (!storeDateTimeAsTicks)
				{
					return "datetime";
				}
				return "bigint";
			}
			if (clrType == typeof(DateTimeOffset))
			{
				return "bigint";
			}
			if (clrType.GetTypeInfo().IsEnum)
			{
				if (p.StoreAsText)
				{
					return "varchar";
				}
				return "integer";
			}
			if (clrType == typeof(byte[]))
			{
				return "blob";
			}
			if (clrType == typeof(Guid))
			{
				return "varchar(36)";
			}
			throw new NotSupportedException("Don't know about " + clrType);
		}

		public static bool IsPK(MemberInfo p)
		{
			return p.CustomAttributes.Any((CustomAttributeData x) => x.AttributeType == typeof(PrimaryKeyAttribute));
		}

		public static string Collation(MemberInfo p)
		{
			return p.CustomAttributes.Where((CustomAttributeData x) => typeof(CollationAttribute) == x.AttributeType).Select(delegate(CustomAttributeData x)
			{
				IList<CustomAttributeTypedArgument> constructorArguments = x.ConstructorArguments;
				return (constructorArguments.Count <= 0) ? "" : ((constructorArguments[0].Value as string) ?? "");
			}).FirstOrDefault() ?? "";
		}

		public static bool IsAutoInc(MemberInfo p)
		{
			return p.CustomAttributes.Any((CustomAttributeData x) => x.AttributeType == typeof(AutoIncrementAttribute));
		}

		public static FieldInfo GetField(TypeInfo t, string name)
		{
			FieldInfo f = t.GetDeclaredField(name);
			if (f != null)
			{
				return f;
			}
			return GetField(t.BaseType.GetTypeInfo(), name);
		}

		public static PropertyInfo GetProperty(TypeInfo t, string name)
		{
			PropertyInfo f = t.GetDeclaredProperty(name);
			if (f != null)
			{
				return f;
			}
			return GetProperty(t.BaseType.GetTypeInfo(), name);
		}

		public static object InflateAttribute(CustomAttributeData x)
		{
			TypeInfo typeInfo = x.AttributeType.GetTypeInfo();
			object[] args = x.ConstructorArguments.Select((CustomAttributeTypedArgument a) => a.Value).ToArray();
			object r = Activator.CreateInstance(x.AttributeType, args);
			foreach (CustomAttributeNamedArgument arg in x.NamedArguments)
			{
				if (arg.IsField)
				{
					GetField(typeInfo, arg.MemberName).SetValue(r, arg.TypedValue.Value);
				}
				else
				{
					GetProperty(typeInfo, arg.MemberName).SetValue(r, arg.TypedValue.Value);
				}
			}
			return r;
		}

		public static IEnumerable<IndexedAttribute> GetIndices(MemberInfo p)
		{
			TypeInfo indexedInfo = typeof(IndexedAttribute).GetTypeInfo();
			return from x in p.CustomAttributes
				where indexedInfo.IsAssignableFrom(x.AttributeType.GetTypeInfo())
				select (IndexedAttribute)InflateAttribute(x);
		}

		public static int? MaxStringLength(PropertyInfo p)
		{
			CustomAttributeData attr = p.CustomAttributes.FirstOrDefault((CustomAttributeData x) => x.AttributeType == typeof(MaxLengthAttribute));
			if (attr != null)
			{
				return ((MaxLengthAttribute)InflateAttribute(attr)).Value;
			}
			return null;
		}

		public static bool IsMarkedNotNull(MemberInfo p)
		{
			return p.CustomAttributes.Any((CustomAttributeData x) => x.AttributeType == typeof(NotNullAttribute));
		}
	}
}
