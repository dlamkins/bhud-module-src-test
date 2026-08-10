using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.AspNetCore.Shared
{
	internal static class ObjectDisposedThrowHelper
	{
		public static void ThrowIf([_003C9e183aca_002D65c7_002D4d6a_002Dae9d_002De571aba8b55a_003EDoesNotReturnIf(true)] bool condition, object instance)
		{
			if (condition)
			{
				ThrowObjectDisposedException(instance);
			}
		}

		public static void ThrowIf([_003C9e183aca_002D65c7_002D4d6a_002Dae9d_002De571aba8b55a_003EDoesNotReturnIf(true)] bool condition, Type type)
		{
			if (condition)
			{
				ThrowObjectDisposedException(type);
			}
		}

		[_003C9e183aca_002D65c7_002D4d6a_002Dae9d_002De571aba8b55a_003EDoesNotReturn]
		private static void ThrowObjectDisposedException(object instance)
		{
			throw new ObjectDisposedException(instance?.GetType().FullName);
		}

		[_003C9e183aca_002D65c7_002D4d6a_002Dae9d_002De571aba8b55a_003EDoesNotReturn]
		private static void ThrowObjectDisposedException(Type type)
		{
			throw new ObjectDisposedException(type?.FullName);
		}
	}
}
