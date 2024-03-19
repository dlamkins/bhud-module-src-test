using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace LiteDB
{
	internal class Reflection
	{
		private static readonly Dictionary<Type, CreateObject> _cacheCtor = new Dictionary<Type, CreateObject>();

		public static readonly Dictionary<Type, PropertyInfo> ConvertType = new Dictionary<Type, PropertyInfo>
		{
			[typeof(DateTime)] = typeof(BsonValue).GetProperty("AsDateTime"),
			[typeof(decimal)] = typeof(BsonValue).GetProperty("AsDecimal"),
			[typeof(double)] = typeof(BsonValue).GetProperty("AsDouble"),
			[typeof(long)] = typeof(BsonValue).GetProperty("AsInt64"),
			[typeof(int)] = typeof(BsonValue).GetProperty("AsInt32"),
			[typeof(bool)] = typeof(BsonValue).GetProperty("AsBoolean"),
			[typeof(byte[])] = typeof(BsonValue).GetProperty("AsBinary"),
			[typeof(BsonDocument)] = typeof(BsonValue).GetProperty("AsDocument"),
			[typeof(BsonArray)] = typeof(BsonValue).GetProperty("AsArray"),
			[typeof(ObjectId)] = typeof(BsonValue).GetProperty("AsObjectId"),
			[typeof(string)] = typeof(BsonValue).GetProperty("AsString"),
			[typeof(Guid)] = typeof(BsonValue).GetProperty("AsGuid")
		};

		public static readonly PropertyInfo DocumentItemProperty = (from x in typeof(BsonDocument).GetProperties()
			where x.Name == "Item" && x.GetGetMethod().GetParameters().First()
				.ParameterType == typeof(string)
			select x).First();

		private static readonly Dictionary<MethodInfo, string> _cacheName = new Dictionary<MethodInfo, string>();

		public static object CreateInstance(Type type)
		{
			try
			{
				if (_cacheCtor.TryGetValue(type, out var c2))
				{
					return c2(null);
				}
			}
			catch (Exception ex2)
			{
				throw LiteException.InvalidCtor(type, ex2);
			}
			lock (_cacheCtor)
			{
				try
				{
					if (_cacheCtor.TryGetValue(type, out var c))
					{
						return c(null);
					}
					TypeInfo typeInfo = type.GetTypeInfo();
					if (typeInfo.IsClass)
					{
						_cacheCtor.Add(type, c = CreateClass(type));
					}
					else
					{
						if (typeInfo.IsInterface)
						{
							if (typeInfo.IsGenericType)
							{
								Type typeDef = type.GetGenericTypeDefinition();
								if (typeDef == typeof(ISet<>))
								{
									return CreateInstance(GetGenericSetOfType(UnderlyingTypeOf(type)));
								}
								if (typeDef == typeof(IDictionary<, >))
								{
									Type k = type.GetGenericArguments()[0];
									Type v = type.GetGenericArguments()[1];
									return CreateInstance(GetGenericDictionaryOfType(k, v));
								}
								if (typeDef == typeof(IList<>) || typeDef == typeof(ICollection<>) || typeDef == typeof(IEnumerable<>) || typeof(IEnumerable).IsAssignableFrom(typeDef))
								{
									return CreateInstance(GetGenericListOfType(UnderlyingTypeOf(type)));
								}
							}
							throw LiteException.InvalidCtor(type, null);
						}
						_cacheCtor.Add(type, c = CreateStruct(type));
					}
					return c(null);
				}
				catch (Exception ex)
				{
					throw LiteException.InvalidCtor(type, ex);
				}
			}
		}

		public static bool IsNullable(Type type)
		{
			if (!type.GetTypeInfo().IsGenericType)
			{
				return false;
			}
			return type.GetGenericTypeDefinition().Equals(typeof(Nullable<>));
		}

		public static Type UnderlyingTypeOf(Type type)
		{
			if (!type.GetTypeInfo().IsGenericType)
			{
				return type;
			}
			return type.GetGenericArguments()[0];
		}

		public static Type GetGenericListOfType(Type type)
		{
			return typeof(List<>).MakeGenericType(type);
		}

		public static Type GetGenericSetOfType(Type type)
		{
			return typeof(HashSet<>).MakeGenericType(type);
		}

		public static Type GetGenericDictionaryOfType(Type k, Type v)
		{
			return typeof(Dictionary<, >).MakeGenericType(k, v);
		}

		public static Type GetListItemType(Type listType)
		{
			if (listType.IsArray)
			{
				return listType.GetElementType();
			}
			Type[] interfaces = listType.GetInterfaces();
			foreach (Type i in interfaces)
			{
				if (i.GetTypeInfo().IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>))
				{
					return i.GetGenericArguments()[0];
				}
				if (listType.GetTypeInfo().IsGenericType && i == typeof(IEnumerable))
				{
					return listType.GetGenericArguments()[0];
				}
			}
			return typeof(object);
		}

		public static bool IsEnumerable(Type type)
		{
			if (type == typeof(IEnumerable) || type.IsArray)
			{
				return true;
			}
			if (type == typeof(string))
			{
				return false;
			}
			Type[] interfaces = type.GetInterfaces();
			foreach (Type @interface in interfaces)
			{
				if (@interface.GetTypeInfo().IsGenericType && @interface.GetGenericTypeDefinition() == typeof(IEnumerable<>))
				{
					return true;
				}
			}
			return false;
		}

		public static bool IsSimpleType(Type type)
		{
			if (!(type == typeof(string)) && !(type == typeof(bool)) && !(type == typeof(byte)) && !(type == typeof(sbyte)) && !(type == typeof(short)) && !(type == typeof(int)) && !(type == typeof(long)) && !(type == typeof(ushort)) && !(type == typeof(uint)) && !(type == typeof(ulong)) && !(type == typeof(double)) && !(type == typeof(float)) && !(type == typeof(decimal)) && !(type == typeof(ObjectId)) && !(type == typeof(DateTime)))
			{
				return type == typeof(Guid);
			}
			return true;
		}

		public static bool IsCollection(Type type)
		{
			if (!type.GetTypeInfo().IsGenericType || !type.GetGenericTypeDefinition().Equals(typeof(ICollection<>)))
			{
				return type.GetInterfaces().Any((Type x) => x == typeof(ICollection) || (x.GetTypeInfo().IsGenericType && x.GetGenericTypeDefinition() == typeof(ICollection<>)));
			}
			return true;
		}

		public static bool IsDictionary(Type type)
		{
			if (!type.GetTypeInfo().IsGenericType || !type.GetGenericTypeDefinition().Equals(typeof(IDictionary<, >)))
			{
				return type.GetInterfaces().Any((Type x) => x == typeof(IDictionary) || (x.GetTypeInfo().IsGenericType && x.GetGenericTypeDefinition().Equals(typeof(IDictionary<, >))));
			}
			return true;
		}

		public static MemberInfo SelectMember(IEnumerable<MemberInfo> members, params Func<MemberInfo, bool>[] predicates)
		{
			foreach (Func<MemberInfo, bool> predicate in predicates)
			{
				MemberInfo member = members.FirstOrDefault(predicate);
				if (member != null)
				{
					return member;
				}
			}
			return null;
		}

		public static string MethodName(MethodInfo method, int skipParameters = 0)
		{
			lock (_cacheName)
			{
				if (_cacheName.TryGetValue(method, out var value))
				{
					return value;
				}
				value = MethodNameInternal(method, skipParameters);
				_cacheName.Add(method, value);
				return value;
			}
		}

		private static string MethodNameInternal(MethodInfo method, int skipParameters = 0)
		{
			StringBuilder sb = new StringBuilder(method.Name + "(");
			int index = 0;
			foreach (ParameterInfo p in method.GetParameters().Skip(skipParameters))
			{
				if (index++ > 0)
				{
					sb.Append(",");
				}
				sb.Append(FriendlyTypeName(p.ParameterType));
				if (p.ParameterType.GetTypeInfo().IsGenericType)
				{
					Type[] types = p.ParameterType.GetGenericTypeDefinition().GetGenericArguments();
					sb.Append("<");
					sb.Append(string.Join(",", types.Select((Type x) => FriendlyTypeName(x))));
					sb.Append(">");
				}
			}
			sb.Append(")");
			return sb.ToString();
		}

		private static string FriendlyTypeName(Type type)
		{
			int generic = type.Name.IndexOf("`");
			string fullName = type.FullName;
			if (fullName != null)
			{
				switch (fullName.Length)
				{
				case 13:
					switch (fullName[11])
					{
					case 'c':
						if (!(fullName == "System.Object"))
						{
							break;
						}
						return "object";
					case 'n':
						if (!(fullName == "System.String"))
						{
							break;
						}
						return "string";
					case 'l':
						if (!(fullName == "System.Double"))
						{
							if (!(fullName == "System.Single"))
							{
								break;
							}
							return "float";
						}
						return "double";
					case '1':
						if (!(fullName == "System.UInt16"))
						{
							break;
						}
						return "ushort";
					case '3':
						if (!(fullName == "System.UInt32"))
						{
							break;
						}
						return "uint";
					case '6':
						if (!(fullName == "System.UInt64"))
						{
							break;
						}
						return "ulong";
					}
					break;
				case 14:
					switch (fullName[7])
					{
					case 'B':
						if (!(fullName == "System.Boolean"))
						{
							break;
						}
						return "bool";
					case 'D':
						if (!(fullName == "System.Decimal"))
						{
							break;
						}
						return "decimal";
					}
					break;
				case 11:
					switch (fullName[7])
					{
					case 'B':
						if (!(fullName == "System.Byte"))
						{
							break;
						}
						return "byte";
					case 'C':
						if (!(fullName == "System.Char"))
						{
							break;
						}
						return "char";
					}
					break;
				case 12:
					switch (fullName[10])
					{
					case '1':
						if (!(fullName == "System.Int16"))
						{
							break;
						}
						return "short";
					case '3':
						if (!(fullName == "System.Int32"))
						{
							break;
						}
						return "int";
					case '6':
						if (!(fullName == "System.Int64"))
						{
							break;
						}
						return "long";
					case 't':
						if (!(fullName == "System.SByte"))
						{
							break;
						}
						return "sbyte";
					}
					break;
				}
			}
			if (generic <= 0)
			{
				return type.Name;
			}
			return type.Name.Substring(0, generic);
		}

		public static CreateObject CreateClass(Type type)
		{
			ParameterExpression pDoc = Expression.Parameter(typeof(BsonDocument), "_doc");
			return Expression.Lambda<CreateObject>(Expression.New(type), new ParameterExpression[1] { pDoc }).Compile();
		}

		public static CreateObject CreateStruct(Type type)
		{
			ParameterExpression pDoc = Expression.Parameter(typeof(BsonDocument), "_doc");
			return Expression.Lambda<CreateObject>(Expression.Convert(Expression.New(type), typeof(object)), new ParameterExpression[1] { pDoc }).Compile();
		}

		public static GenericGetter CreateGenericGetter(Type type, MemberInfo memberInfo)
		{
			if (memberInfo == null)
			{
				throw new ArgumentNullException("memberInfo");
			}
			if (memberInfo is PropertyInfo && !(memberInfo as PropertyInfo).CanRead)
			{
				return null;
			}
			ParameterExpression obj = Expression.Parameter(typeof(object), "o");
			return Expression.Lambda<GenericGetter>(Expression.Convert(Expression.MakeMemberAccess(Expression.Convert(obj, memberInfo.DeclaringType), memberInfo), typeof(object)), new ParameterExpression[1] { obj }).Compile();
		}

		public static GenericSetter CreateGenericSetter(Type type, MemberInfo memberInfo)
		{
			if (memberInfo == null)
			{
				throw new ArgumentNullException("memberInfo");
			}
			FieldInfo fieldInfo = memberInfo as FieldInfo;
			PropertyInfo propertyInfo = memberInfo as PropertyInfo;
			if (memberInfo is PropertyInfo && !propertyInfo.CanWrite)
			{
				return null;
			}
			if (type.GetTypeInfo().IsValueType)
			{
				if (!(memberInfo is FieldInfo))
				{
					return delegate(object t, object v)
					{
						propertyInfo.SetValue(t, v, null);
					};
				}
				return fieldInfo.SetValue;
			}
			Type dataType = ((memberInfo is PropertyInfo) ? propertyInfo.PropertyType : fieldInfo.FieldType);
			ParameterExpression target = Expression.Parameter(typeof(object), "obj");
			ParameterExpression value = Expression.Parameter(typeof(object), "val");
			UnaryExpression castTarget = Expression.Convert(target, type);
			UnaryExpression castValue = Expression.ConvertChecked(value, dataType);
			return Expression.Lambda<GenericSetter>(Expression.Convert(Expression.Assign((memberInfo is PropertyInfo) ? Expression.Property(castTarget, propertyInfo) : Expression.Field(castTarget, fieldInfo), castValue), typeof(object)), new ParameterExpression[2] { target, value }).Compile();
		}
	}
}
