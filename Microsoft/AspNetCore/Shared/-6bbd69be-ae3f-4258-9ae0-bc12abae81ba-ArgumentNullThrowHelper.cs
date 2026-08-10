using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Microsoft.AspNetCore.Shared
{
	internal static class _003C6bbd69be_002Dae3f_002D4258_002D9ae0_002Dbc12abae81ba_003EArgumentNullThrowHelper
	{
		public static void ThrowIfNull([_003C6bbd69be_002Dae3f_002D4258_002D9ae0_002Dbc12abae81ba_003ENotNull] object? argument, [_003C6bbd69be_002Dae3f_002D4258_002D9ae0_002Dbc12abae81ba_003ECallerArgumentExpression("argument")] string? paramName = null)
		{
			if (argument == null)
			{
				Throw(paramName);
			}
		}

		[_003C6bbd69be_002Dae3f_002D4258_002D9ae0_002Dbc12abae81ba_003EDoesNotReturn]
		internal static void Throw(string? paramName)
		{
			throw new ArgumentNullException(paramName);
		}
	}
}
