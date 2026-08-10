using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Microsoft.AspNetCore.Shared
{
	internal static class _003C9e183aca_002D65c7_002D4d6a_002Dae9d_002De571aba8b55a_003EArgumentNullThrowHelper
	{
		public static void ThrowIfNull([_003C9e183aca_002D65c7_002D4d6a_002Dae9d_002De571aba8b55a_003ENotNull] object? argument, [_003C9e183aca_002D65c7_002D4d6a_002Dae9d_002De571aba8b55a_003ECallerArgumentExpression("argument")] string? paramName = null)
		{
			if (argument == null)
			{
				Throw(paramName);
			}
		}

		[_003C9e183aca_002D65c7_002D4d6a_002Dae9d_002De571aba8b55a_003EDoesNotReturn]
		internal static void Throw(string? paramName)
		{
			throw new ArgumentNullException(paramName);
		}
	}
}
