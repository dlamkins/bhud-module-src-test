using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Text.Json.Serialization;

namespace System.Text.Json.Reflection
{
	internal static class ReflectionExtensions
	{
		private const string ImmutableArrayGenericTypeName = "System.Collections.Immutable.ImmutableArray`1";

		private const string ImmutableListGenericTypeName = "System.Collections.Immutable.ImmutableList`1";

		private const string ImmutableListGenericInterfaceTypeName = "System.Collections.Immutable.IImmutableList`1";

		private const string ImmutableStackGenericTypeName = "System.Collections.Immutable.ImmutableStack`1";

		private const string ImmutableStackGenericInterfaceTypeName = "System.Collections.Immutable.IImmutableStack`1";

		private const string ImmutableQueueGenericTypeName = "System.Collections.Immutable.ImmutableQueue`1";

		private const string ImmutableQueueGenericInterfaceTypeName = "System.Collections.Immutable.IImmutableQueue`1";

		private const string ImmutableSortedSetGenericTypeName = "System.Collections.Immutable.ImmutableSortedSet`1";

		private const string ImmutableHashSetGenericTypeName = "System.Collections.Immutable.ImmutableHashSet`1";

		private const string ImmutableSetGenericInterfaceTypeName = "System.Collections.Immutable.IImmutableSet`1";

		private const string ImmutableDictionaryGenericTypeName = "System.Collections.Immutable.ImmutableDictionary`2";

		private const string ImmutableDictionaryGenericInterfaceTypeName = "System.Collections.Immutable.IImmutableDictionary`2";

		private const string ImmutableSortedDictionaryGenericTypeName = "System.Collections.Immutable.ImmutableSortedDictionary`2";

		private const string ImmutableArrayTypeName = "System.Collections.Immutable.ImmutableArray";

		private const string ImmutableListTypeName = "System.Collections.Immutable.ImmutableList";

		private const string ImmutableStackTypeName = "System.Collections.Immutable.ImmutableStack";

		private const string ImmutableQueueTypeName = "System.Collections.Immutable.ImmutableQueue";

		private const string ImmutableSortedSetTypeName = "System.Collections.Immutable.ImmutableSortedSet";

		private const string ImmutableHashSetTypeName = "System.Collections.Immutable.ImmutableHashSet";

		private const string ImmutableDictionaryTypeName = "System.Collections.Immutable.ImmutableDictionary";

		private const string ImmutableSortedDictionaryTypeName = "System.Collections.Immutable.ImmutableSortedDictionary";

		public const string CreateRangeMethodName = "CreateRange";

		private static readonly Type s_nullableType = typeof(Nullable<>);

		public static Type GetCompatibleGenericBaseClass(this Type type, Type baseType)
		{
			if ((object)baseType == null)
			{
				return null;
			}
			Type type2 = type;
			while (type2 != null && type2 != typeof(object))
			{
				if (type2.IsGenericType)
				{
					Type genericTypeDefinition = type2.GetGenericTypeDefinition();
					if (genericTypeDefinition == baseType)
					{
						return type2;
					}
				}
				type2 = type2.BaseType;
			}
			return null;
		}

		[UnconditionalSuppressMessage("ReflectionAnalysis", "IL2070:UnrecognizedReflectionPattern", Justification = "The 'interfaceType' must exist and so trimmer kept it. In which case It also kept it on any type which implements it. The below call to GetInterfaces may return fewer results when trimmed but it will return the 'interfaceType' if the type implemented it, even after trimming.")]
		public static Type GetCompatibleGenericInterface(this Type type, Type interfaceType)
		{
			if ((object)interfaceType == null)
			{
				return null;
			}
			Type type2 = type;
			if (type2.IsGenericType)
			{
				type2 = type2.GetGenericTypeDefinition();
			}
			if (type2 == interfaceType)
			{
				return type;
			}
			Type[] interfaces = type.GetInterfaces();
			foreach (Type type3 in interfaces)
			{
				if (type3.IsGenericType)
				{
					Type genericTypeDefinition = type3.GetGenericTypeDefinition();
					if (genericTypeDefinition == interfaceType)
					{
						return type3;
					}
				}
			}
			return null;
		}

		public static bool IsImmutableDictionaryType(this Type type)
		{
			if (!type.IsGenericType || !type.Assembly.FullName.StartsWith("System.Collections.Immutable", StringComparison.Ordinal))
			{
				return false;
			}
			switch (GetBaseNameFromGenericType(type))
			{
			case "System.Collections.Immutable.ImmutableDictionary`2":
			case "System.Collections.Immutable.IImmutableDictionary`2":
			case "System.Collections.Immutable.ImmutableSortedDictionary`2":
				return true;
			default:
				return false;
			}
		}

		public static bool IsImmutableEnumerableType(this Type type)
		{
			if (!type.IsGenericType || !type.Assembly.FullName.StartsWith("System.Collections.Immutable", StringComparison.Ordinal))
			{
				return false;
			}
			string baseNameFromGenericType = GetBaseNameFromGenericType(type);
			if (baseNameFromGenericType != null)
			{
				switch (baseNameFromGenericType.Length)
				{
				case 45:
				{
					char c = baseNameFromGenericType[38];
					if ((uint)c <= 81u)
					{
						if (c != 'A')
						{
							if (c != 'Q' || !(baseNameFromGenericType == "System.Collections.Immutable.ImmutableQueue`1"))
							{
								break;
							}
						}
						else if (!(baseNameFromGenericType == "System.Collections.Immutable.ImmutableArray`1"))
						{
							break;
						}
					}
					else if (c != 'S')
					{
						if (c != 'e' || !(baseNameFromGenericType == "System.Collections.Immutable.IImmutableList`1"))
						{
							break;
						}
					}
					else if (!(baseNameFromGenericType == "System.Collections.Immutable.ImmutableStack`1"))
					{
						break;
					}
					goto IL_0152;
				}
				case 44:
				{
					char c = baseNameFromGenericType[30];
					if (c != 'I')
					{
						if (c != 'm' || !(baseNameFromGenericType == "System.Collections.Immutable.ImmutableList`1"))
						{
							break;
						}
					}
					else if (!(baseNameFromGenericType == "System.Collections.Immutable.IImmutableSet`1"))
					{
						break;
					}
					goto IL_0152;
				}
				case 46:
				{
					char c = baseNameFromGenericType[39];
					if (c != 'Q')
					{
						if (c != 'S' || !(baseNameFromGenericType == "System.Collections.Immutable.IImmutableStack`1"))
						{
							break;
						}
					}
					else if (!(baseNameFromGenericType == "System.Collections.Immutable.IImmutableQueue`1"))
					{
						break;
					}
					goto IL_0152;
				}
				case 49:
					if (!(baseNameFromGenericType == "System.Collections.Immutable.ImmutableSortedSet`1"))
					{
						break;
					}
					goto IL_0152;
				case 47:
					{
						if (!(baseNameFromGenericType == "System.Collections.Immutable.ImmutableHashSet`1"))
						{
							break;
						}
						goto IL_0152;
					}
					IL_0152:
					return true;
				}
			}
			return false;
		}

		public static string GetImmutableDictionaryConstructingTypeName(this Type type)
		{
			switch (GetBaseNameFromGenericType(type))
			{
			case "System.Collections.Immutable.ImmutableDictionary`2":
			case "System.Collections.Immutable.IImmutableDictionary`2":
				return "System.Collections.Immutable.ImmutableDictionary";
			case "System.Collections.Immutable.ImmutableSortedDictionary`2":
				return "System.Collections.Immutable.ImmutableSortedDictionary";
			default:
				return null;
			}
		}

		public static string GetImmutableEnumerableConstructingTypeName(this Type type)
		{
			string baseNameFromGenericType = GetBaseNameFromGenericType(type);
			if (baseNameFromGenericType != null)
			{
				switch (baseNameFromGenericType.Length)
				{
				case 45:
				{
					char c = baseNameFromGenericType[38];
					if ((uint)c <= 81u)
					{
						if (c != 'A')
						{
							if (c != 'Q' || !(baseNameFromGenericType == "System.Collections.Immutable.ImmutableQueue`1"))
							{
								break;
							}
							goto IL_0153;
						}
						if (!(baseNameFromGenericType == "System.Collections.Immutable.ImmutableArray`1"))
						{
							break;
						}
						return "System.Collections.Immutable.ImmutableArray";
					}
					if (c != 'S')
					{
						if (c != 'e' || !(baseNameFromGenericType == "System.Collections.Immutable.IImmutableList`1"))
						{
							break;
						}
						goto IL_0147;
					}
					if (!(baseNameFromGenericType == "System.Collections.Immutable.ImmutableStack`1"))
					{
						break;
					}
					goto IL_014d;
				}
				case 44:
				{
					char c = baseNameFromGenericType[30];
					if (c != 'I')
					{
						if (c != 'm' || !(baseNameFromGenericType == "System.Collections.Immutable.ImmutableList`1"))
						{
							break;
						}
						goto IL_0147;
					}
					if (!(baseNameFromGenericType == "System.Collections.Immutable.IImmutableSet`1"))
					{
						break;
					}
					goto IL_015f;
				}
				case 46:
				{
					char c = baseNameFromGenericType[39];
					if (c != 'Q')
					{
						if (c != 'S' || !(baseNameFromGenericType == "System.Collections.Immutable.IImmutableStack`1"))
						{
							break;
						}
						goto IL_014d;
					}
					if (!(baseNameFromGenericType == "System.Collections.Immutable.IImmutableQueue`1"))
					{
						break;
					}
					goto IL_0153;
				}
				case 49:
					if (!(baseNameFromGenericType == "System.Collections.Immutable.ImmutableSortedSet`1"))
					{
						break;
					}
					return "System.Collections.Immutable.ImmutableSortedSet";
				case 47:
					{
						if (!(baseNameFromGenericType == "System.Collections.Immutable.ImmutableHashSet`1"))
						{
							break;
						}
						goto IL_015f;
					}
					IL_0147:
					return "System.Collections.Immutable.ImmutableList";
					IL_014d:
					return "System.Collections.Immutable.ImmutableStack";
					IL_015f:
					return "System.Collections.Immutable.ImmutableHashSet";
					IL_0153:
					return "System.Collections.Immutable.ImmutableQueue";
				}
			}
			return null;
		}

		private static string GetBaseNameFromGenericType(Type genericType)
		{
			Type genericTypeDefinition = genericType.GetGenericTypeDefinition();
			return genericTypeDefinition.FullName;
		}

		public static bool IsVirtual(this PropertyInfo propertyInfo)
		{
			if (!(propertyInfo.GetMethod?.IsVirtual ?? false))
			{
				return propertyInfo.SetMethod?.IsVirtual ?? false;
			}
			return true;
		}

		public static bool IsKeyValuePair(this Type type)
		{
			if (type.IsGenericType)
			{
				return type.GetGenericTypeDefinition() == typeof(KeyValuePair<, >);
			}
			return false;
		}

		public static bool TryGetDeserializationConstructor([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)] this Type type, bool useDefaultCtorInAnnotatedStructs, out ConstructorInfo deserializationCtor)
		{
			ConstructorInfo constructorInfo = null;
			ConstructorInfo constructorInfo2 = null;
			ConstructorInfo constructorInfo3 = null;
			ConstructorInfo[] constructors = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public);
			if (constructors.Length == 1)
			{
				constructorInfo3 = constructors[0];
			}
			ConstructorInfo[] array = constructors;
			foreach (ConstructorInfo constructorInfo4 in array)
			{
				if (HasJsonConstructorAttribute(constructorInfo4))
				{
					if (constructorInfo != null)
					{
						deserializationCtor = null;
						return false;
					}
					constructorInfo = constructorInfo4;
				}
				else if (constructorInfo4.GetParameters().Length == 0)
				{
					constructorInfo2 = constructorInfo4;
				}
			}
			ConstructorInfo[] constructors2 = type.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);
			foreach (ConstructorInfo constructorInfo5 in constructors2)
			{
				if (HasJsonConstructorAttribute(constructorInfo5))
				{
					if (constructorInfo != null)
					{
						deserializationCtor = null;
						return false;
					}
					constructorInfo = constructorInfo5;
				}
			}
			if (useDefaultCtorInAnnotatedStructs && type.IsValueType && constructorInfo == null)
			{
				deserializationCtor = null;
				return true;
			}
			deserializationCtor = constructorInfo ?? constructorInfo2 ?? constructorInfo3;
			return true;
		}

		public static object GetDefaultValue(this ParameterInfo parameterInfo)
		{
			Type parameterType = parameterInfo.ParameterType;
			object defaultValue = parameterInfo.DefaultValue;
			if (defaultValue == null)
			{
				return null;
			}
			if (defaultValue == DBNull.Value && parameterType != typeof(DBNull))
			{
				return null;
			}
			if (parameterType.IsEnum)
			{
				return Enum.ToObject(parameterType, defaultValue);
			}
			Type underlyingType = Nullable.GetUnderlyingType(parameterType);
			if ((object)underlyingType != null && underlyingType.IsEnum)
			{
				return Enum.ToObject(underlyingType, defaultValue);
			}
			return defaultValue;
		}

		[RequiresUnreferencedCode("Should only be used by the reflection-based serializer.")]
		public static Type[] GetSortedTypeHierarchy(this Type type)
		{
			if (!type.IsInterface)
			{
				List<Type> list = new List<Type>();
				Type type2 = type;
				while (type2 != null)
				{
					list.Add(type2);
					type2 = type2.BaseType;
				}
				return list.ToArray();
			}
			return JsonHelpers.TraverseGraphWithTopologicalSort(type, (Type t) => t.GetInterfaces());
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsNullableOfT(this Type type)
		{
			if (type.IsGenericType)
			{
				return type.GetGenericTypeDefinition() == s_nullableType;
			}
			return false;
		}

		public static bool IsAssignableFromInternal(this Type type, Type from)
		{
			if (from.IsNullableOfT() && type.IsInterface)
			{
				return type.IsAssignableFrom(from.GetGenericArguments()[0]);
			}
			return type.IsAssignableFrom(from);
		}

		public static bool IsInSubtypeRelationshipWith(this Type type, Type other)
		{
			if (!type.IsAssignableFromInternal(other))
			{
				return other.IsAssignableFromInternal(type);
			}
			return true;
		}

		private static bool HasJsonConstructorAttribute(ConstructorInfo constructorInfo)
		{
			return constructorInfo.GetCustomAttribute<JsonConstructorAttribute>() != null;
		}

		public static bool HasRequiredMemberAttribute(this MemberInfo memberInfo)
		{
			return memberInfo.HasCustomAttributeWithName("System.Runtime.CompilerServices.RequiredMemberAttribute", inherit: false);
		}

		public static bool HasSetsRequiredMembersAttribute(this MemberInfo memberInfo)
		{
			return memberInfo.HasCustomAttributeWithName("System.Diagnostics.CodeAnalysis.SetsRequiredMembersAttribute", inherit: false);
		}

		private static bool HasCustomAttributeWithName(this MemberInfo memberInfo, string fullName, bool inherit)
		{
			object[] customAttributes = memberInfo.GetCustomAttributes(inherit);
			foreach (object obj in customAttributes)
			{
				if (obj.GetType().FullName == fullName)
				{
					return true;
				}
			}
			return false;
		}

		public static TAttribute GetUniqueCustomAttribute<TAttribute>(this MemberInfo memberInfo, bool inherit) where TAttribute : Attribute
		{
			object[] customAttributes = memberInfo.GetCustomAttributes(typeof(TAttribute), inherit);
			if (customAttributes.Length == 0)
			{
				return null;
			}
			if (customAttributes.Length == 1)
			{
				return (TAttribute)customAttributes[0];
			}
			ThrowHelper.ThrowInvalidOperationException_SerializationDuplicateAttribute(typeof(TAttribute), memberInfo);
			return null;
		}

		public static object CreateInstanceNoWrapExceptions([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.NonPublicConstructors)] this Type type, Type[] parameterTypes, object[] parameters)
		{
			ConstructorInfo constructor = type.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, parameterTypes, null);
			object result = null;
			try
			{
				result = constructor.Invoke(parameters);
				return result;
			}
			catch (TargetInvocationException ex)
			{
				ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
				return result;
			}
		}
	}
}
