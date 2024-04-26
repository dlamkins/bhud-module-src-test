using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Estreya.BlishHUD.Shared.Attributes;
using Newtonsoft.Json;

namespace Estreya.BlishHUD.Shared.Extensions
{
	public static class ObjectExtensions
	{
		private static readonly MethodInfo CloneMethod = typeof(object).GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic);

		public static bool IsPrimitive(this Type type)
		{
			if (type == typeof(string))
			{
				return true;
			}
			return type.IsValueType & type.IsPrimitive;
		}

		public static object Copy(this object originalObject)
		{
			return InternalCopy(originalObject, new Dictionary<object, object>(new ReferenceEqualityComparer()));
		}

		private static object InternalCopy(object originalObject, IDictionary<object, object> visited)
		{
			if (originalObject == null)
			{
				return null;
			}
			Type typeToReflect = originalObject.GetType();
			if (typeToReflect.IsPrimitive())
			{
				return originalObject;
			}
			if (visited.ContainsKey(originalObject))
			{
				return visited[originalObject];
			}
			if (typeof(Delegate).IsAssignableFrom(typeToReflect))
			{
				return null;
			}
			if (typeof(Pointer).IsAssignableFrom(typeToReflect))
			{
				return null;
			}
			object cloneObject = CloneMethod.Invoke(originalObject, null);
			if (typeToReflect.IsArray && !typeToReflect.GetElementType().IsPrimitive())
			{
				Array clonedArray = (Array)cloneObject;
				clonedArray.ForEach(delegate(Array array, int[] indices)
				{
					array.SetValue(InternalCopy(clonedArray.GetValue(indices), visited), indices);
				});
			}
			visited.Add(originalObject, cloneObject);
			CopyFields(originalObject, visited, cloneObject, typeToReflect);
			RecursiveCopyBaseTypePrivateFields(originalObject, visited, cloneObject, typeToReflect);
			return cloneObject;
		}

		private static bool ShouldIgnoreField(FieldInfo fi)
		{
			return fi.GetCustomAttribute<IgnoreCopyAttribute>() != null;
		}

		private static void RecursiveCopyBaseTypePrivateFields(object originalObject, IDictionary<object, object> visited, object cloneObject, Type typeToReflect)
		{
			if (typeToReflect.BaseType != null)
			{
				RecursiveCopyBaseTypePrivateFields(originalObject, visited, cloneObject, typeToReflect.BaseType);
				CopyFields(originalObject, visited, cloneObject, typeToReflect.BaseType, BindingFlags.Instance | BindingFlags.NonPublic, (FieldInfo info) => info.IsPrivate);
			}
		}

		private static void CopyFields(object originalObject, IDictionary<object, object> visited, object cloneObject, Type typeToReflect, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy, Func<FieldInfo, bool> filter = null)
		{
			FieldInfo[] fields = typeToReflect.GetFields(bindingFlags);
			foreach (FieldInfo fieldInfo in fields)
			{
				if ((filter == null || filter(fieldInfo)) && !fieldInfo.FieldType.IsPrimitive() && !ShouldIgnoreField(fieldInfo))
				{
					object clonedFieldValue = InternalCopy(fieldInfo.GetValue(originalObject), visited);
					fieldInfo.SetValue(cloneObject, clonedFieldValue);
				}
			}
		}

		public static T Copy<T>(this T original)
		{
			return (T)((object)original).Copy();
		}

		public static T CopyWithJson<T>(this T original, JsonSerializerSettings serializerSettings)
		{
			JsonSerializer jsonSerializer = JsonSerializer.CreateDefault(serializerSettings);
			using MemoryStream memoryStream = new MemoryStream();
			using (StreamWriter streamWriter = new StreamWriter(memoryStream, Encoding.UTF8, 1024, leaveOpen: true))
			{
				using JsonTextWriter jsonWriter = new JsonTextWriter(streamWriter);
				jsonSerializer.Serialize(jsonWriter, original);
			}
			memoryStream.Position = 0L;
			using StreamReader streamReader = new StreamReader(memoryStream);
			using JsonTextReader jsonTextReader = new JsonTextReader(streamReader);
			return jsonSerializer.Deserialize<T>(jsonTextReader);
		}
	}
}