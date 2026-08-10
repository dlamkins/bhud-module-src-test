using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.Options
{
	internal class OptionsWrapper<[_003C7f228368_002D946a_002D4fc2_002Daf41_002Da7f494284193_003EDynamicallyAccessedMembers(_003C7f228368_002D946a_002D4fc2_002Daf41_002Da7f494284193_003EDynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TOptions> : IOptions<TOptions> where TOptions : class
	{
		public TOptions Value { get; }

		public OptionsWrapper(TOptions options)
		{
			Value = options;
		}
	}
}
