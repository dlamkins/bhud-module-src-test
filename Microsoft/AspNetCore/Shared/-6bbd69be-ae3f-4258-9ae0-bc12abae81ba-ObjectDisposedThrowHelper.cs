using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.AspNetCore.Shared
{
	internal static class _003C6bbd69be_002Dae3f_002D4258_002D9ae0_002Dbc12abae81ba_003EObjectDisposedThrowHelper
	{
		public static void ThrowIf([_003C6bbd69be_002Dae3f_002D4258_002D9ae0_002Dbc12abae81ba_003EDoesNotReturnIf(true)] bool condition, object instance)
		{
			if (condition)
			{
				ThrowObjectDisposedException(instance);
			}
		}

		public static void ThrowIf([_003C6bbd69be_002Dae3f_002D4258_002D9ae0_002Dbc12abae81ba_003EDoesNotReturnIf(true)] bool condition, Type type)
		{
			if (condition)
			{
				ThrowObjectDisposedException(type);
			}
		}

		[_003C6bbd69be_002Dae3f_002D4258_002D9ae0_002Dbc12abae81ba_003EDoesNotReturn]
		private static void ThrowObjectDisposedException(object instance)
		{
			throw new ObjectDisposedException(instance?.GetType().FullName);
		}

		[_003C6bbd69be_002Dae3f_002D4258_002D9ae0_002Dbc12abae81ba_003EDoesNotReturn]
		private static void ThrowObjectDisposedException(Type type)
		{
			throw new ObjectDisposedException(type?.FullName);
		}
	}
}
