using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.Options
{
	internal interface IOptionsFactory<[_003C7f228368_002D946a_002D4fc2_002Daf41_002Da7f494284193_003EDynamicallyAccessedMembers(_003C7f228368_002D946a_002D4fc2_002Daf41_002Da7f494284193_003EDynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TOptions> where TOptions : class
	{
		TOptions Create(string name);
	}
}
