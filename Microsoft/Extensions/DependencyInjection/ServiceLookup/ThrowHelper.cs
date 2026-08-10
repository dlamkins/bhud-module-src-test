using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup
{
	internal static class ThrowHelper
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[_003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003EDoesNotReturn]
		internal static void ThrowObjectDisposedException()
		{
			throw new ObjectDisposedException("IServiceProvider");
		}
	}
}
