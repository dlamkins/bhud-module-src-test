using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.Options
{
	internal interface IOptionsMonitorCache<[_003C7f228368_002D946a_002D4fc2_002Daf41_002Da7f494284193_003EDynamicallyAccessedMembers(_003C7f228368_002D946a_002D4fc2_002Daf41_002Da7f494284193_003EDynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TOptions> where TOptions : class
	{
		TOptions GetOrAdd(string? name, Func<TOptions> createOptions);

		bool TryAdd(string? name, TOptions options);

		bool TryRemove(string? name);

		void Clear();
	}
}
