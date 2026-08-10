using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Microsoft.AspNetCore.Shared
{
	internal static class _003C7d92d028_002Dbcfd_002D4d41_002D98e5_002D27c14689a0c0_003EArgumentNullThrowHelper
	{
		public static void ThrowIfNull([_003C7d92d028_002Dbcfd_002D4d41_002D98e5_002D27c14689a0c0_003ENotNull] object? argument, [_003C7d92d028_002Dbcfd_002D4d41_002D98e5_002D27c14689a0c0_003ECallerArgumentExpression("argument")] string? paramName = null)
		{
			if (argument == null)
			{
				Throw(paramName);
			}
		}

		[_003C7d92d028_002Dbcfd_002D4d41_002D98e5_002D27c14689a0c0_003EDoesNotReturn]
		internal static void Throw(string? paramName)
		{
			throw new ArgumentNullException(paramName);
		}
	}
}
