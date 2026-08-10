using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Microsoft.AspNetCore.Shared
{
	internal static class ArgumentThrowHelper
	{
		public static void ThrowIfNullOrEmpty([_003Cdd9f8911_002Da76b_002D4733_002D8b22_002D48168607313e_003ENotNull] string? argument, [_003Cdd9f8911_002Da76b_002D4733_002D8b22_002D48168607313e_003ECallerArgumentExpression("argument")] string? paramName = null)
		{
			if (argument == null || argument == string.Empty)
			{
				_003Cdd9f8911_002Da76b_002D4733_002D8b22_002D48168607313e_003EArgumentNullThrowHelper.ThrowIfNull(argument, "argument");
				Throw(paramName);
			}
		}

		[_003Cdd9f8911_002Da76b_002D4733_002D8b22_002D48168607313e_003EDoesNotReturn]
		internal static void Throw(string? paramName)
		{
			throw new ArgumentException("The value cannot be an empty string.", paramName);
		}
	}
}
