using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SQLite
{
	internal class EnumCacheInfo
	{
		public bool IsEnum { get; private set; }

		public bool StoreAsText { get; private set; }

		public Dictionary<int, string> EnumValues { get; private set; }

		public EnumCacheInfo(Type type)
		{
			TypeInfo typeInfo = type.GetTypeInfo();
			IsEnum = typeInfo.IsEnum;
			if (!IsEnum)
			{
				return;
			}
			StoreAsText = typeInfo.CustomAttributes.Any((CustomAttributeData x) => x.AttributeType == typeof(StoreAsTextAttribute));
			if (!StoreAsText)
			{
				return;
			}
			EnumValues = new Dictionary<int, string>();
			foreach (object e in Enum.GetValues(type))
			{
				EnumValues[Convert.ToInt32(e)] = e.ToString();
			}
		}
	}
}
