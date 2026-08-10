using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;

namespace Microsoft.AspNetCore.SignalR
{
	internal static class ReflectionHelper
	{
		public static bool IsStreamingType(Type type, bool mustBeDirectType = false)
		{
			if (IsIAsyncEnumerable(type))
			{
				return true;
			}
			Type type2 = type;
			do
			{
				if (type2.IsGenericType && type2.GetGenericTypeDefinition() == typeof(ChannelReader<>))
				{
					return true;
				}
				type2 = type2.BaseType;
			}
			while (!mustBeDirectType && type2 != null);
			return false;
		}

		public static bool IsIAsyncEnumerable(Type type)
		{
			if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IAsyncEnumerable<>))
			{
				return true;
			}
			return type.GetInterfaces().Any((Type t) => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IAsyncEnumerable<>));
		}
	}
}
