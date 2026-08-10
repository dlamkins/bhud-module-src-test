using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Threading;

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup
{
	internal static class ServiceLookupHelpers
	{
		private const BindingFlags LookupFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

		private static readonly MethodInfo ArrayEmptyMethodInfo = typeof(Array).GetMethod("Empty");

		internal static readonly MethodInfo InvokeFactoryMethodInfo = typeof(Func<IServiceProvider, object>).GetMethod("Invoke", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

		internal static readonly MethodInfo CaptureDisposableMethodInfo = typeof(ServiceProviderEngineScope).GetMethod("CaptureDisposable", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

		internal static readonly MethodInfo TryGetValueMethodInfo = typeof(IDictionary<ServiceCacheKey, object>).GetMethod("TryGetValue", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

		internal static readonly MethodInfo ResolveCallSiteAndScopeMethodInfo = typeof(CallSiteRuntimeResolver).GetMethod("Resolve", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

		internal static readonly MethodInfo AddMethodInfo = typeof(IDictionary<ServiceCacheKey, object>).GetMethod("Add", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

		internal static readonly MethodInfo MonitorEnterMethodInfo = typeof(Monitor).GetMethod("Enter", BindingFlags.Static | BindingFlags.Public, null, new Type[2]
		{
			typeof(object),
			typeof(bool).MakeByRefType()
		}, null);

		internal static readonly MethodInfo MonitorExitMethodInfo = typeof(Monitor).GetMethod("Exit", BindingFlags.Static | BindingFlags.Public, null, new Type[1] { typeof(object) }, null);

		[_003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003ERequiresDynamicCode("The code for an array of the specified type might not be available.")]
		[_003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003EUnconditionalSuppressMessage("ReflectionAnalysis", "IL2060:MakeGenericMethod", Justification = "Calling Array.Empty<T>() is safe since the T doesn't have trimming annotations.")]
		internal static MethodInfo GetArrayEmptyMethodInfo(Type itemType)
		{
			return ArrayEmptyMethodInfo.MakeGenericMethod(itemType);
		}
	}
}
