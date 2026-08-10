using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.Options
{
	internal static class Options
	{
		internal const _003C7f228368_002D946a_002D4fc2_002Daf41_002Da7f494284193_003EDynamicallyAccessedMemberTypes DynamicallyAccessedMembers = _003C7f228368_002D946a_002D4fc2_002Daf41_002Da7f494284193_003EDynamicallyAccessedMemberTypes.PublicParameterlessConstructor;

		public static readonly string DefaultName = string.Empty;

		public static IOptions<TOptions> Create<[_003C7f228368_002D946a_002D4fc2_002Daf41_002Da7f494284193_003EDynamicallyAccessedMembers(_003C7f228368_002D946a_002D4fc2_002Daf41_002Da7f494284193_003EDynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TOptions>(TOptions options) where TOptions : class
		{
			return new OptionsWrapper<TOptions>(options);
		}
	}
}
